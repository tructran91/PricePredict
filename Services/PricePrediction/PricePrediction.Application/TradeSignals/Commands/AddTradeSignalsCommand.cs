using MediatR;
using PricePredict.Shared.Models;
using PricePrediction.Application.Responses;

namespace PricePrediction.Application.TradeSignals.Commands
{
    public class AddTradeSignalsCommand : IRequest<BaseResponse<List<TradeSignalResponse>>>
    {
        public string Symbol { get; set; }

        public string Timeframe { get; set; }

        public int ShortPeriod { get; set; }

        public int LongPeriod { get; set; }

        public DateTimeOffset StartDateTime { get; set; }

        public DateTimeOffset EndDateTime { get; set; }
    }
}
