using Currency.DataApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Currency.DataApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }
    }
}