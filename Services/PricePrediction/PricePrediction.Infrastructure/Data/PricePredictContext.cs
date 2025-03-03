using Microsoft.EntityFrameworkCore;
using PricePrediction.Core.Entities;

namespace PricePrediction.Infrastructure.Data
{
    public class PricePredictContext : DbContext
    {
        public PricePredictContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Candlestick> Candlesticks { get; set; }
    }
}
