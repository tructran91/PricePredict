using MediatR;
using Microsoft.Extensions.Logging;
using PricePredict.Shared.Models;
using PricePrediction.Application.PredictPrice.Commands;
using PricePrediction.Application.Responses;
using PricePrediction.Application.Services;
using PricePrediction.Core.Entities;
using PricePrediction.Core.Repositories;
using System.Text.Json;

namespace PricePrediction.Application.PredictPrice.Handler
{
    public class PredictPriceHandler : IRequestHandler<PredictPriceCommand, BaseResponse<List<TradeSignalResponse>>>
    {
        private readonly IBaseRepository<Candlestick> _repository;
        private readonly ICandlestickService _candlestickService;
        private readonly ILogger<PredictPriceHandler> _logger;

        public PredictPriceHandler(
            IBaseRepository<Candlestick> repository,
            ICandlestickService candlestickService,
            ILogger<PredictPriceHandler> logger)
        {
            _repository = repository;
            _candlestickService = candlestickService;
            _logger = logger;
        }

        public async Task<BaseResponse<List<TradeSignalResponse>>> Handle(PredictPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"{nameof(PredictPriceHandler)}: {JsonSerializer.Serialize(request)}");

                var candlesticks = await _candlestickService.GetCandlesticksAsync(request.Symbol, request.Timeframe, request.StartDate, request.EndDate);

                var shortMA = CalculateMovingAverage(candlesticks, request.ShortPeriod);
                var longMA = CalculateMovingAverage(candlesticks, request.LongPeriod);

                var result = GenerateTradeSignals(candlesticks, shortMA, longMA);

                return BaseResponse<List<TradeSignalResponse>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(PredictPriceHandler)}: Error predicting price: {ex.Message}");
                return BaseResponse<List<TradeSignalResponse>>.Failure("Error predicting price.");
            }
        }

        private List<decimal> CalculateMovingAverage(List<Candlestick> prices, int period)
        {
            var maList = new List<decimal>();
            for (int i = 0; i < prices.Count; i++)
            {
                if (i >= period - 1)
                {
                    decimal sum = prices.Skip(i - period + 1).Take(period).Sum(p => p.ClosePrice);
                    maList.Add(sum / period);
                }
                else
                {
                    maList.Add(0);
                }
            }
            return maList;
        }

        private List<TradeSignalResponse> GenerateTradeSignals(List<Candlestick> prices, List<decimal> shortMA, List<decimal> longMA)
        {
            var signals = new List<TradeSignalResponse>();
            for (int i = 1; i < prices.Count; i++)
            {
                if (shortMA[i - 1] <= longMA[i - 1] && shortMA[i] > longMA[i])
                {
                    signals.Add(new TradeSignalResponse
                    {
                        Date = prices[i].Timestamp,
                        Signal = "BUY"
                    });
                }
                else if (shortMA[i - 1] >= longMA[i - 1] && shortMA[i] < longMA[i])
                {
                    signals.Add(new TradeSignalResponse
                    {
                        Date = prices[i].Timestamp,
                        Signal = "SELL"
                    });
                }
            }
            return signals;
        }
    }
}
