using Microsoft.EntityFrameworkCore;
using PricePrediction.Core.Entities;

namespace PricePrediction.Infrastructure.Data
{
    public class PricePredictContext : DbContext
    {
        public PricePredictContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TradeSignal> TradeSignals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PricePredictContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
