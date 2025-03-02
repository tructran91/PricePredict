using DataImport.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataImport.Infrastructure.Data.Configurations
{
    public class CandlestickConfiguration : IEntityTypeConfiguration<Candlestick>
    {
        public void Configure(EntityTypeBuilder<Candlestick> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasIndex(c => new { c.Symbol, c.Timeframe, c.Timestamp }).IsUnique(); // Index tối ưu truy vấn

            builder.Property(c => c.Symbol).HasMaxLength(20).IsRequired();
            builder.Property(c => c.Timeframe).HasMaxLength(10).IsRequired();
            builder.Property(c => c.Timestamp).IsRequired();

            builder.Property(c => c.OpenPrice).HasColumnType("decimal(18,8)").IsRequired();
            builder.Property(c => c.HighPrice).HasColumnType("decimal(18,8)").IsRequired();
            builder.Property(c => c.LowPrice).HasColumnType("decimal(18,8)").IsRequired();
            builder.Property(c => c.ClosePrice).HasColumnType("decimal(18,8)").IsRequired();
            builder.Property(c => c.Volume).HasColumnType("decimal(18,8)").IsRequired();

            builder.Property(c => c.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(c => c.LastModifiedDate).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
