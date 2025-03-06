using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PricePrediction.Core.Entities;

namespace PricePrediction.Infrastructure.Data.Configurations
{
    public class TradeSignalConfiguration : IEntityTypeConfiguration<TradeSignal>
    {
        public void Configure(EntityTypeBuilder<TradeSignal> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Symbol).IsRequired().HasMaxLength(20);
            builder.Property(c => c.Timeframe).IsRequired().HasMaxLength(10);
            builder.Property(c => c.Timestamp).IsRequired().HasColumnType("datetimeoffset(0)");
            builder.Property(c => c.PriceAtSignal).IsRequired().HasColumnType("decimal(18,8)");
            builder.Property(t => t.TradeId).IsRequired().HasMaxLength(50);
            builder.Property(c => c.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(c => c.LastModifiedDate).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
