using PricePrediction.Core.Entities;

namespace PricePrediction.Core.Repositories
{
    public interface ITradeResultRepository : IBaseRepository<TradeResult>
    {
        Task<List<TradeResult>> GetTradeResultsAsync(string symbol, string timeframe, DateTimeOffset? startDateTime, DateTimeOffset? endDateTime, string indicatorType);
    }
}
