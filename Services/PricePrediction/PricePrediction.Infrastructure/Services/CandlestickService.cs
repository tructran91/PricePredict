using PricePrediction.Application.DTOs;
using PricePrediction.Application.Services;

namespace PricePrediction.Infrastructure.Services
{
    public class CandlestickService : ICandlestickService
    {
        private readonly ICandlestickServiceRefit _candlestickService;

        public CandlestickService(ICandlestickServiceRefit candlestickService)
        {
            _candlestickService = candlestickService;
        }

        public async Task<List<Candlestick>> GetCandlesticksAsync(string symbol, string timeframe, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            var candles = await _candlestickService.GetCandlesticksAsync(symbol, timeframe, startTime, endTime);
            if (!candles.IsSuccess)
            {
                return new List<Candlestick>();
            }
            return candles.Data;
        }
    }
}
