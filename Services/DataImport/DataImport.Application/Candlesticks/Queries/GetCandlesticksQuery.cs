using DataImport.Application.Responses;
using MediatR;
using PricePredict.Shared.Models;

namespace DataImport.Application.Candlesticks.Queries
{
    public class GetCandlesticksQuery : IRequest<BaseResponse<List<CandlestickResponse>>>
    {
        public string Symbol { get; set; }

        public string TargetTimeframe { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }
    }
}
