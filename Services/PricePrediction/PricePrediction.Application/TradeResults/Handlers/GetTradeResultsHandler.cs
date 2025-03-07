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
        private readonly ITradeResultRepository _repository;
        private readonly ILogger<GetTradeResultsHandler> _logger;

        public GetTradeResultsHandler(
            ITradeResultRepository repository,
            ILogger<GetTradeResultsHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BaseResponse<List<IndicatorPerformanceResponse>>> Handle(GetTradeResultsQuery request, CancellationToken cancellationToken)
        {
            var results = await _repository.GetTradeResultsAsync(request.Symbol, request.Timeframe, request.StartDateTime, request.EndDateTime, request.IndicatorType);

            var performance = results
                .GroupBy(tr => tr.IndicatorType)
                .Select(group => new IndicatorPerformanceResponse
                {
                    IndicatorType = group.Key,
                    TotalTrades = group.Count(),
                    WinTrades = group.Count(tr => tr.IsWin),
                    WinRate = Math.Round(group.Count(tr => tr.IsWin) / (double)group.Count() * 100, 2),
                    AverageProfit = Math.Round(group.Average(tr => tr.Profit), 2),
                    TotalProfit = Math.Round(group.Sum(tr => tr.Profit), 2)
                })
                .OrderByDescending(p => p.WinRate)
                .ToList();

            return BaseResponse<List<IndicatorPerformanceResponse>>.Success(performance);
        }
    }
}
