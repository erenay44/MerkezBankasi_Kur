using Currency.BusinessApi.Models;

namespace Currency.BusinessApi.Services
{
    public interface ICurrencyCacheService
    {
        Task<IEnumerable<CurrencyRate>> GetCurrencyRatesAsync(string currencyCode);
    }
}
