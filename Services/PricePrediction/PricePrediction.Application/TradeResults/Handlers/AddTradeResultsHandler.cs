using MediatR;
using Microsoft.Extensions.Logging;
using PricePrediction.Application.DTOs;
using PricePrediction.Application.TradeResults.Commands;
using PricePrediction.Application.TradeSignals.Handler;
using PricePrediction.Core.Entities;
using PricePrediction.Core.Repositories;

namespace PricePrediction.Application.TradeResults.Handlers
{
    public class AddTradeResultsHandler : IRequestHandler<AddTradeResultsCommand>
    {
        private readonly IBaseRepository<TradeResult> _repository;
        private readonly ILogger<AddTradeResultsHandler> _logger;

        public AddTradeResultsHandler(
            IBaseRepository<TradeResult> repository,
            ILogger<AddTradeResultsHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(AddTradeResultsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tradeResults = new List<TradeResult>();
                foreach (var entrySignal in request.TradeSignals)
                {
                    var exitSignal = request.TradeSignals.FirstOrDefault(s => s.TradeId == entrySignal.TradeId &&
                                     ((entrySignal.Signal == "BUY" && s.Signal == "SELL") ||
                                      (entrySignal.Signal == "SHORT" && s.Signal == "COVER")) &&
                                     s.Timestamp > entrySignal.Timestamp);

                    if (exitSignal == null) continue;

                    var isWin = (entrySignal.Signal == "BUY" && exitSignal.PriceAtSignal > entrySignal.PriceAtSignal) ||
                        (entrySignal.Signal == "SHORT" && exitSignal.PriceAtSignal < entrySignal.PriceAtSignal);

                    var profit = entrySignal.Signal == "BUY"
                        ? exitSignal.PriceAtSignal - entrySignal.PriceAtSignal
                        : entrySignal.PriceAtSignal - exitSignal.PriceAtSignal;

                    var tradeResult = new TradeResult
                    {
                        TradeId = entrySignal.TradeId,
                        Symbol = entrySignal.Symbol,
                        Timeframe = entrySignal.Timeframe,
                        IndicatorType = entrySignal.IndicatorType,
                        Signal = entrySignal.Signal,
                        PriceAtSignal = entrySignal.PriceAtSignal,
                        ExitPrice = exitSignal.PriceAtSignal,
                        IsWin = isWin,
                        Profit = profit
                    };

                    tradeResults.Add(tradeResult);
                }

                await _repository.AddRangeAsync(tradeResults);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(AddTradeResultsCommand)}: {ex.Message}");
            }
        }
    }
}
