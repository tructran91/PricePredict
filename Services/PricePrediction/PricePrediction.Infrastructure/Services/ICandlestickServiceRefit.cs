using PricePredict.Shared.Models;
using PricePrediction.Application.DTOs;
using Refit;

namespace PricePrediction.Infrastructure.Services
{
    public interface ICandlestickServiceRefit
    {
        [Get("/api/candlestick")]
        Task<BaseResponse<List<Candlestick>>> GetCandlesticksAsync(string symbol, string targetTimeframe, DateTimeOffset startTime, DateTimeOffset endTime);
    }
}
