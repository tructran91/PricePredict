using PricePrediction.Core.Entities;

namespace PricePrediction.Application.Services
{
    public interface ICandlestickService
    {
        Task<List<Candlestick>> GetCandlesticksAsync(string symbol, string timeframe, DateTime startTime, DateTime endTime);
    }
}
