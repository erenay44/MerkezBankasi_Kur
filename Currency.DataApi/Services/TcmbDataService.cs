using Currency.DataApi.Data;
using Currency.DataApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

using System.Xml.Linq;
namespace Currency.DataApi.Services
{
    public class TcmbDataService : ITcmbDataService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        public TcmbDataService(AppDbContext context, HttpClient httpClient)
        {

            _context = context;
            _httpClient = httpClient;

        }
        public async Task FetchAndSaveLastTwoMonthsRatesAsync()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Tarihleri en baþýndan UTC formatýnda baþlatýyoruz
            var endDate = DateTime.UtcNow.Date;
            var startDate = endDate.AddMonths(-2);

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Npgsql'in EF Core sorgusunda hata fýrlatmamasý için tarihi açýkça "UTC" olarak iþaretliyoruz
                var utcDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);

                try
                {
                    string url = (date == endDate)
                        ? "https://www.tcmb.gov.tr/kurlar/today.xml"
                        : $"https://www.tcmb.gov.tr/kurlar/{date:yyyyMM}/{date:ddMMyyyy}.xml";

                    var response = await _httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        continue;

                    var xmlString = await response.Content.ReadAsStringAsync();
                    var xDocument = XDocument.Parse(xmlString);

                    var currencies = xDocument.Descendants("Currency");
                        

                    foreach (var currency in currencies)
                    {
                        var currencyCode = currency.Attribute("CurrencyCode")?.Value;
                        var buyingStr = currency.Element("ForexBuying")?.Value;
                        var sellingStr = currency.Element("ForexSelling")?.Value;

                        if (decimal.TryParse(buyingStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal buying) &&
                            decimal.TryParse(sellingStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal selling))
                        {
                            // Sorguyu yaparken iþaretlediðimiz UTC tarihi kullanýyoruz
                            var exists = await _context.CurrencyRates.AnyAsync(c => c.CurrencyCode == currencyCode && c.Date == utcDate);

                            if (!exists)
                            {
                                _context.CurrencyRates.Add(new CurrencyRate
                                {
                                    CurrencyCode = currencyCode,
                                    ForexBuying = buying,
                                    ForexSelling = selling,
                                    Date = utcDate // Kaydederken de veritabanýna UTC olarak yazýyoruz
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Tarih: {date.ToShortDateString()} için veri çekilemedi. Hata: {ex.Message}");
                }
            }

            await _context.SaveChangesAsync();
        }
    }
    }
