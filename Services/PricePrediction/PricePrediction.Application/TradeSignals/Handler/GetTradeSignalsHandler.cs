using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PricePredict.Shared.Models;
using PricePrediction.Application.Responses;
using PricePrediction.Application.TradeSignals.Queries;
using PricePrediction.Core.Entities;
using PricePrediction.Core.Repositories;
using System.Text.Json;

namespace PricePrediction.Application.TradeSignals.Handler
{
    public class GetTradeSignalsHandler : IRequestHandler<GetTradeSignalsQuery, BaseResponse<List<TradeSignalResponse>>>
    {
        private readonly ITradeSignalRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTradeSignalsHandler> _logger;

        public GetTradeSignalsHandler(
            ITradeSignalRepository repository,
            IMapper mapper,
            ILogger<GetTradeSignalsHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<TradeSignalResponse>>> Handle(GetTradeSignalsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetTradeSignalsHandler)}: {JsonSerializer.Serialize(request)}");

            var signals = await _repository.GetTradeSignalsAsync(
                request.Symbol,
                request.Timeframe,
                request.StartDateTime,
                request.EndDateTime,
                request.IndicatorType);

            var response = _mapper.Map<List<TradeSignalResponse>>(signals);

            return BaseResponse<List<TradeSignalResponse>>.Success(response);
        }
    }
}
