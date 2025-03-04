using AutoMapper;
using DataImport.Application.Candlesticks.Queries;
using DataImport.Application.Responses;
using DataImport.Core.Repositories;
using MediatR;
using PricePredict.Shared.Models;

namespace DataImport.Application.Candlesticks.Handlers
{
    public class GetCandlesticksHandler : IRequestHandler<GetCandlesticksQuery, BaseResponse<List<CandlestickResponse>>>
    {
        private readonly ICandlestickRepository _repository;
        private readonly IMapper _mapper;

        public GetCandlesticksHandler(ICandlestickRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<CandlestickResponse>>> Handle(GetCandlesticksQuery request, CancellationToken cancellationToken)
        {
            var startTimeUtc = request.StartTime.UtcDateTime;
            var endTimeUtc = request.EndTime.UtcDateTime;

            var candles = await _repository.GetCandlesticksAsync(request.Symbol, request.TargetTimeframe, startTimeUtc, endTimeUtc);

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            var response = candles.Select(c => new CandlestickResponse
            {
                Symbol = c.Symbol,
                Timeframe = c.Timeframe,
                Timestamp = TimeZoneInfo.ConvertTimeFromUtc(c.Timestamp.UtcDateTime, timeZone),
                OpenPrice = c.OpenPrice,
                HighPrice = c.HighPrice,
                LowPrice = c.LowPrice,
                ClosePrice = c.ClosePrice,
                Volume = c.Volume
            }).ToList();

            return BaseResponse<List<CandlestickResponse>>.Success(response);
        }
    }
}
