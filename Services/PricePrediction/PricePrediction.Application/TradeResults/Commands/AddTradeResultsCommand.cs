using MediatR;
using PricePrediction.Core.Entities;

namespace PricePrediction.Application.TradeResults.Commands
{
    public class AddTradeResultsCommand : IRequest
    {
        public List<TradeSignal> TradeSignals { get; set; }
    }
}
