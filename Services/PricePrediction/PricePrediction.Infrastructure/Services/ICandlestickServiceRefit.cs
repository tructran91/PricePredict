using PricePredict.Shared.Models;
using PricePrediction.Core.Entities;
using Refit;

namespace PricePrediction.Infrastructure.Services
{
    public interface ICandlestickServiceRefit
    {
        [Get("/api/candlestick/candlesticks")]
        Task<BaseResponse<List<Candlestick>>> GetCandlesticksAsync(string symbol, string targetTimeframe, DateTime startTime, DateTime endTime);
    }
}
