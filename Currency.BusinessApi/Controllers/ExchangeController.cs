using Currency.BusinessApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Currency.BusinessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly ICurrencyCacheService _cacheService;

        public ExchangeController(ICurrencyCacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet("{currencyCode}")]
        public async Task<IActionResult> GetRates(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                return BadRequest("Kur kodu boş olamaz.");

            var rates = await _cacheService.GetCurrencyRatesAsync(currencyCode);

            if (rates == null || !rates.Any())
                return NotFound($"{currencyCode} kuru için veri bulunamadı.");

            return Ok(rates);
        }
    }
}