using Currency.BusinessApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Currency.BusinessApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }
    }
}