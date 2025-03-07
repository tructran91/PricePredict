using Azure.Core;
using Microsoft.EntityFrameworkCore;
using PricePrediction.Core.Entities;
using PricePrediction.Core.Repositories;
using PricePrediction.Infrastructure.Data;

namespace PricePrediction.Infrastructure.Repositories
{
    public class TradeResultRepository : BaseRepository<TradeResult>, ITradeResultRepository
    {
        public TradeResultRepository(PricePredictContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<TradeResult>> GetTradeResultsAsync(string symbol, string timeframe, DateTimeOffset? startDateTime, DateTimeOffset? endDateTime, string indicatorType)
        {
            var query = _dbContext.Set<TradeResult>().AsQueryable();

            query = query.Where(x => x.Symbol == symbol);

            if (!string.IsNullOrEmpty(timeframe))
            {
                query = query.Where(x => x.Timeframe == timeframe);
            }

            if (startDateTime.HasValue && startDateTime != default)
            {
                query = query.Where(x => x.Timestamp >= startDateTime);
            }

            if (endDateTime.HasValue && endDateTime != default)
            {
                query = query.Where(x => x.Timestamp <= endDateTime);
            }

            if (!string.IsNullOrEmpty(indicatorType))
            {
                query = query.Where(x => x.IndicatorType == indicatorType);
            }

            return await query.AsNoTracking().ToListAsync();
        }
    }
}
