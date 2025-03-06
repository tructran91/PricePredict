using DataImport.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataImport.Infrastructure.Data
{
    public class DataImportContext : DbContext
    {
        public DataImportContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Candlestick> Candlesticks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataImportContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
