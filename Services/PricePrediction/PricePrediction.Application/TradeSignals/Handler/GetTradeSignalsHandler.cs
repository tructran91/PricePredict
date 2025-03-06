using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PricePrediction.Application.TradeSignals.Queries;
using PricePrediction.Core.Entities;
using PricePrediction.Core.Repositories;

namespace PricePrediction.Application.TradeSignals.Handler
{
    public class GetTradeSignalsHandler : IRequestHandler<GetTradeSignalsQuery, List<TradeSignal>>
    {
        private readonly IBaseRepository<TradeSignal> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTradeSignalsHandler> _logger;

        public GetTradeSignalsHandler(
            IBaseRepository<TradeSignal> repository,
            IMapper mapper,
            ILogger<GetTradeSignalsHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TradeSignal>> Handle(GetTradeSignalsQuery request, CancellationToken cancellationToken)
        {
            var signals = await _repository.GetAsync(
                predicate: t => t.Symbol == request.Symbol && t.Timeframe == request.Timeframe && t.Timestamp == request.Timestamp && t.IndicatorType == request.IndicatorType,
                orderBy: x => x.OrderBy(y => y.Timestamp),
                pageNumber: 1,
                pageSize: 1000);

            return signals.ToList();
        }
    }
}
