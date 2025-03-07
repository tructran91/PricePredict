using PricePrediction.Core.Entities;

namespace PricePrediction.Core.Repositories
{
    public interface ITradeSignalRepository : IBaseRepository<TradeSignal>
    {
        Task<List<TradeSignal>> GetTradeSignalsAsync(string symbol, string timeframe, DateTimeOffset? startDateTime, DateTimeOffset? endDateTime, string indicatorType);
    }
}
