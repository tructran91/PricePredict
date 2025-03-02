using DataImport.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataImport.Infrastructure.Data
{
    public class PricePredictContext : DbContext
    {
        public PricePredictContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Candlestick> Candlesticks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PricePredictContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
