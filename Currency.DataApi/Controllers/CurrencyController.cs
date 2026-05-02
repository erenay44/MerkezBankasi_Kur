using Currency.DataApi.Data;
using Currency.DataApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Currency.DataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ITcmbDataService _tcmbDataService;
        private readonly AppDbContext _context;

        public CurrencyController(ITcmbDataService tcmbDataService, AppDbContext context)
        {
            _tcmbDataService = tcmbDataService;
            _context = context;
        }

        // TCMB'den son 2 aylýk veriyi çekip DB'ye kaydeden endpoint
        [HttpPost("sync")]
        public async Task<IActionResult> SyncData()
        {
            try
            {
                await _tcmbDataService.FetchAndSaveLastTwoMonthsRatesAsync();
                return Ok(new { Message = "Son 2 aylýk kur verileri baþarýyla çekildi ve veritabanýna kaydedildi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Veri çekilirken bir hata oluþtu: {ex.Message}");
            }
        }

        // Veritabanýndaki kurlarý listelemek için kullanacaðýmýz endpoint
        [HttpGet]
        public async Task<IActionResult> GetRates()
        {
            var rates = await _context.CurrencyRates
                .OrderByDescending(r => r.Date)
                .ToListAsync();

            return Ok(rates);
        }
    }
}