using MediatR;
using Microsoft.Extensions.Logging;
using PricePredict.Shared.Models;
using PricePrediction.Application.Responses;
using PricePrediction.Application.TradeResults.Queries;
using PricePrediction.Core.Entities;
using PricePrediction.Core.Repositories;

namespace PricePrediction.Application.TradeResults.Handlers
{
    public class GetTradeResultsHandler : IRequestHandler<GetTradeResultsQuery, BaseResponse<List<IndicatorPerformanceResponse>>>
    {
        private readonly IBaseRepository<TradeResult> _repository;
        private readonly ILogger<GetTradeResultsHandler> _logger;

        public GetTradeResultsHandler(
            IBaseRepository<TradeResult> repository,
            ILogger<GetTradeResultsHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BaseResponse<List<IndicatorPerformanceResponse>>> Handle(GetTradeResultsQuery request, CancellationToken cancellationToken)
        {
            var results = await _repository.GetAsync(
                        predicate: t => t.Symbol == request.Symbol,
                        pageNumber: 1,
                        pageSize: 1000);

            var performance = results
                .GroupBy(tr => tr.IndicatorType)
                .Select(group => new IndicatorPerformanceResponse
                {
                    IndicatorType = group.Key,
                    TotalTrades = group.Count(),
                    WinTrades = group.Count(tr => tr.IsWin),
                    WinRate = group.Count(tr => tr.IsWin) / (double)group.Count(),
                    AverageProfit = group.Average(tr => tr.Profit)
                })
                .OrderByDescending(p => p.WinRate)
                .ToList();

            return BaseResponse<List<IndicatorPerformanceResponse>>.Success(performance);
        }
    }
}
