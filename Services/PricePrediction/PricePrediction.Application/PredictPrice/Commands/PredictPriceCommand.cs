using MediatR;

namespace PricePrediction.Application.PredictPrice.Commands
{
    public class PredictPriceCommand : IRequest<string>
    {
        public string Symbol { get; set; }

        public int ShortPeriod { get; set; }

        public int LongPeriod { get; set; }

        public PredictPriceCommand(string symbol, int shortPeriod, int longPeriod)
        {
            Symbol = symbol;
            ShortPeriod = shortPeriod;
            LongPeriod = longPeriod;
        }
    }
}
