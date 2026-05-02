using Currency.BusinessApi.Data;
using Currency.BusinessApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Currency.BusinessApi.Services
{
    public class CurrencyCacheService : ICurrencyCacheService
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache;

        public CurrencyCacheService(AppDbContext context, IDistributedCache cache) { 
        
            _context = context;
            _cache = cache;

        }
        public async Task<IEnumerable<CurrencyRate>> GetCurrencyRatesAsync(string currencyCode)
        {
            currencyCode = currencyCode.ToUpper();
            string cacheKey = $"rates_{currencyCode}";

        
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
           
                return JsonSerializer.Deserialize<List<CurrencyRate>>(cachedData);
            }

      
            var rates = await _context.CurrencyRates
                .Where(r => r.CurrencyCode == currencyCode)
                .OrderBy(r => r.Date)
                .ToListAsync();

            if (rates.Any())
            {
             
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };

                string serializedData = JsonSerializer.Serialize(rates);
                await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions);
            }

            return rates;
        }
    }
}
