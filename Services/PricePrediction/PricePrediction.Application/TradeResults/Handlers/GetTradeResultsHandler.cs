using MediatR;
using Microsoft.Extensions.Logging;
using PricePredict.Shared.Models;
using PricePrediction.Application.DTOs;
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

            var dailyPerformance = results
                .GroupBy(tr => new { tr.IndicatorType, tr.Timestamp.Date })
                .Select(group => new DailyPerformance
                {
                    IndicatorType = group.Key.IndicatorType,
                    Date = group.Key.Date,
                    WinRate = Math.Round(group.Count(tr => tr.IsWin) / (double)group.Count() * 100, 2),
                    TotalProfit = Math.Round(group.Sum(tr => tr.Profit), 2)
                })
                .ToList()
                .GroupBy(dp => dp.IndicatorType)
                .Select(g => new IndicatorPerformanceResponse
                {
                    IndicatorType = g.Key,
                    DailyResults = g.OrderBy(dp => dp.Date).ToList()
                })
                .ToList();

            var totalPerformance = results
                .GroupBy(tr => tr.IndicatorType)
                .Select(g => new IndicatorPerformanceResponse
                {
                    IndicatorType = g.Key,
                    TotalWinRate = Math.Round(g.Count(tr => tr.IsWin) / (double)g.Count() * 100, 2),
                    TotalProfit = Math.Round(g.Sum(tr => tr.Profit), 2)
                })
                .ToList();


            return BaseResponse<List<IndicatorPerformanceResponse>>.Success(dailyPerformance);
        }
    }
}
