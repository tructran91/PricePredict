using AutoMapper;
using DataImport.Application.Candlesticks.Queries;
using DataImport.Application.Responses;
using DataImport.Core.Entities;
using DataImport.Core.Repositories;
using MediatR;
using PricePredict.Shared.Constants;
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
            var candles = await _repository.GetCandlesticksAsync(request.Symbol, request.TargetTimeframe, request.StartTime, request.EndTime);
            var response = _mapper.Map<List<CandlestickResponse>>(candles);

            return BaseResponse<List<CandlestickResponse>>.Success(response);
        }
    }
}
