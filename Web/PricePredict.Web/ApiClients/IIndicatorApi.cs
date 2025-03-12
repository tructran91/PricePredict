using PricePredict.Web.Models.Business.Indicator;
using PricePredict.Web.Models.Common;
using Refit;

namespace PricePredict.Web.ApiClients
{
    public interface IIndicatorApi
    {
        [Get("/api/trade-results")]
        Task<BaseResponse<List<IndicatorPerformanceModel>>> GetTradeResultsAsync(string symbol, string timeframe, DateTimeOffset startDateTime, DateTimeOffset endDateTime);
    }
}
