using MediatR;
using PricePredict.Shared.Models;
using PricePrediction.Core.Entities;

namespace PricePrediction.Application.TradeSignals.Queries
{
    public class GetTradeSignalsQuery : IRequest<List<TradeSignal>>
    {
        public string Symbol { get; set; }

        public string Timeframe { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public string IndicatorType { get; set; }
    }
}
