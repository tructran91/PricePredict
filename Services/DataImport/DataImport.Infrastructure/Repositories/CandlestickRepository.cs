using DataImport.Core.Entities;
using DataImport.Core.Repositories;
using DataImport.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DataImport.Infrastructure.Repositories
{
    public class CandlestickRepository : BaseRepository<Candlestick>, ICandlestickRepository
    {
        public CandlestickRepository(DataImportContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Candlestick>> GetCandlesticksAsync(string symbol, string timeframe, DateTimeOffset startDateTime, DateTimeOffset endDateTime)
        {
            if (timeframe.ToLower() == "1m")
            {
                var query = _dbContext.Candlesticks
                    .Where(p => p.Symbol == symbol && p.Timestamp >= startDateTime && p.Timestamp <= endDateTime)
                    .OrderByDescending(p => p.Timestamp)
                    .AsNoTracking();

                return await query.ToListAsync();
            }
            else
            {
                var timeSpan = ParseTimeframe(timeframe);

                var baseCandlesticks = await _dbContext.Candlesticks
                    .Where(p => p.Symbol == symbol && p.Timestamp >= startDateTime && p.Timestamp <= endDateTime)
                    .OrderByDescending(p => p.Timestamp)
                    .AsNoTracking()
                    .ToListAsync();

                if (!baseCandlesticks.Any())
                    return new List<Candlestick>();

                var groupedCandlesticks = baseCandlesticks
                    .GroupBy(p => p.Timestamp.UtcTicks / timeSpan.Ticks)
                    .Select(group => new Candlestick
                    {
                        Symbol = symbol,
                        Timeframe = timeframe,
                        Timestamp = group.Min(p => p.Timestamp),
                        OpenPrice = group.First().OpenPrice,
                        HighPrice = group.Max(p => p.HighPrice),
                        LowPrice = group.Min(p => p.LowPrice),
                        ClosePrice = group.Last().ClosePrice,
                        Volume = group.Sum(p => p.Volume)
                    })
                    .ToList();

                return groupedCandlesticks;
            }
        }

        public TimeSpan ParseTimeframe(string timeframe)
        {
            return timeframe.ToLower() switch
            {
                "1m" => TimeSpan.FromMinutes(1),
                "5m" => TimeSpan.FromMinutes(5),
                "15m" => TimeSpan.FromMinutes(15),
                "30m" => TimeSpan.FromMinutes(30),
                "1h" => TimeSpan.FromHours(1),
                "2h" => TimeSpan.FromHours(2),
                "4h" => TimeSpan.FromHours(4),
                "1d" => TimeSpan.FromDays(1),
                "1w" => TimeSpan.FromDays(7),
                "1mo" => TimeSpan.FromDays(30),  // Giả sử 1 tháng có 30 ngày
                _ => throw new ArgumentException($"Timeframe '{timeframe}' không hợp lệ.")
            };
        }
    }
}
