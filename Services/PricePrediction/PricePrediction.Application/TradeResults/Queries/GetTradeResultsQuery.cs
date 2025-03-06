using MediatR;
using PricePredict.Shared.Models;
using PricePrediction.Application.Responses;

namespace PricePrediction.Application.TradeResults.Queries
{
    public class GetTradeResultsQuery : IRequest<BaseResponse<List<IndicatorPerformanceResponse>>>
    {
        public string Symbol { get; set; }

        //public string Timeframe { get; set; }

        //public DateTimeOffset Timestamp { get; set; }

        //public string IndicatorType { get; set; }
    }
}
