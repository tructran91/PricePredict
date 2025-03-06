using MediatR;
using PricePredict.Shared.Models;
using PricePrediction.Application.Responses;

namespace PricePrediction.Application.PredictPrice.Commands
{
    public class PredictPriceCommand : IRequest<BaseResponse<List<TradeSignalResponse>>>
    {
        public string Symbol { get; set; }

        public string Timeframe { get; set; }

        public int ShortPeriod { get; set; }

        public int LongPeriod { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public PredictPriceCommand(string symbol, string timeframe, int shortPeriod, int longPeriod, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            Symbol = symbol;
            Timeframe = timeframe;
            ShortPeriod = shortPeriod;
            LongPeriod = longPeriod;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
