using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PricePrediction.Core.Entities;

namespace PricePrediction.Infrastructure.Data.Configurations
{
    public class TradeResultConfiguration : IEntityTypeConfiguration<TradeResult>
    {
        public void Configure(EntityTypeBuilder<TradeResult> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Symbol).IsRequired().HasMaxLength(20);
            builder.Property(c => c.Timeframe).IsRequired().HasMaxLength(10);
            builder.Property(c => c.Timestamp).IsRequired().HasColumnType("datetimeoffset(0)");
            builder.Property(c => c.PriceAtSignal).IsRequired().HasColumnType("decimal(18,8)");
            builder.Property(c => c.ExitPrice).IsRequired().HasColumnType("decimal(18,8)");
            builder.Property(t => t.TradeId).IsRequired();
            builder.Property(c => c.Profit).IsRequired().HasColumnType("decimal(18,8)");
            builder.Property(c => c.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(c => c.LastModifiedDate).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
