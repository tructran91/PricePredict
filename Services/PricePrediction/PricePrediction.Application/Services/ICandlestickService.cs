using PricePrediction.Application.DTOs;

namespace PricePrediction.Application.Services
{
    public interface ICandlestickService
    {
        Task<List<Candlestick>> GetCandlesticksAsync(string symbol, string timeframe, DateTimeOffset startTime, DateTimeOffset endTime);
    }
}
