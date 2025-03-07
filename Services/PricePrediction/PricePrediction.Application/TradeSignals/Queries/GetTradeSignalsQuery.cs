using MediatR;
using PricePredict.Shared.Models;
using PricePrediction.Application.Responses;

namespace PricePrediction.Application.TradeSignals.Queries
{
    public class GetTradeSignalsQuery : IRequest<BaseResponse<List<TradeSignalResponse>>>
    {
        public string Symbol { get; set; }

        public string? Timeframe { get; set; }

        public DateTimeOffset? StartDateTime { get; set; }

        public DateTimeOffset? EndDateTime { get; set; }

        public string? IndicatorType { get; set; }
    }
}
