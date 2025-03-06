using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PricePredict.Shared.Models;
using PricePrediction.Application.DTOs;
using PricePrediction.Application.TradeSignals.Commands;
using PricePrediction.Application.Responses;
using PricePrediction.Application.Services;
using PricePrediction.Core.Entities;
using PricePrediction.Core.Repositories;
using System.Text.Json;
using PricePrediction.Application.TradeResults.Commands;

namespace PricePrediction.Application.TradeSignals.Handler
{
    public class AddTradeSignalsHandler : IRequestHandler<AddTradeSignalsCommand, BaseResponse<List<TradeSignalResponse>>>
    {
        private readonly IBaseRepository<TradeSignal> _repository;
        private readonly ICandlestickService _candlestickService;
        private readonly IMediator _mediator;
        //private readonly ITradingAnalysisService _tradingAnalysisService;
        private readonly IMapper _mapper;
        private readonly ILogger<AddTradeSignalsHandler> _logger;

        public AddTradeSignalsHandler(
            IBaseRepository<TradeSignal> repository,
            ICandlestickService candlestickService,
            IMediator mediator,
            //ITradingAnalysisService tradingAnalysisService,
            IMapper mapper,
            ILogger<AddTradeSignalsHandler> logger)
        {
            _repository = repository;
            _candlestickService = candlestickService;
            _mediator = mediator;
            //_tradingAnalysisService = tradingAnalysisService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<TradeSignalResponse>>> Handle(AddTradeSignalsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"{nameof(AddTradeSignalsHandler)}: {JsonSerializer.Serialize(request)}");

                var candlesticks = await _candlestickService.GetCandlesticksAsync(request.Symbol, request.Timeframe, request.StartDate, request.EndDate);

                if (candlesticks == null || !candlesticks.Any())
                {
                    return BaseResponse<List<TradeSignalResponse>>.Success(new List<TradeSignalResponse>(), "No candlestick data available.");
                }

                candlesticks = candlesticks.OrderBy(c => c.Timestamp).ToList();

                var shortMA = CalculateSMA(candlesticks, request.ShortPeriod);
                var longMA = CalculateSMA(candlesticks, request.LongPeriod);

                var signals = GenerateTradeSignals(candlesticks, shortMA, longMA);
                if (signals.Any())
                {
                    var indicatorType = $"MA {request.ShortPeriod}-{request.LongPeriod}";

                    var existingSignals = await _repository.GetAsync(
                        predicate: t => t.Symbol == request.Symbol &&
                                        t.Timeframe == request.Timeframe &&
                                        t.IndicatorType == indicatorType &&
                                        signals.Select(s => s.Timestamp).Contains(t.Timestamp),
                        orderBy: x => x.OrderBy(y => y.Timestamp),
                        pageNumber: 1,
                        pageSize: 1000);

                    var existingTimestamps = existingSignals.Select(t => t.Timestamp).ToHashSet();

                    var newSignals = signals
                        .Where(s => !existingTimestamps.Contains(s.Timestamp)) // Lọc ra các signal chưa có trong DB
                        .ToList();

                    if (newSignals.Any())
                    {
                        var signalsEntity = _mapper.Map<List<TradeSignal>>(newSignals);
                        signalsEntity.ForEach(s =>
                        {
                            s.IndicatorType = indicatorType;
                            s.Timeframe = request.Timeframe;
                        });

                        await _repository.AddRangeAsync(signalsEntity);

                        // TODO: move to post-processor
                        await _mediator.Send(new AddTradeResultsCommand
                        {
                            TradeSignals = signalsEntity
                        });
                    }
                }

                return BaseResponse<List<TradeSignalResponse>>.Success(signals);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(AddTradeSignalsHandler)}: Error predicting price: {ex.Message}");
                return BaseResponse<List<TradeSignalResponse>>.Failure("Error predicting price.");
            }
        }

        /// <summary>
        /// Tính toán đường trung bình động (MA) dựa trên số lượng chu kỳ chỉ định.
        /// </summary>
        /// <param name="prices">Danh sách giá nến</param>
        /// <param name="period">Số chu kỳ dùng để tính MA</param>
        /// <returns>Danh sách giá trị MA</returns>
        private List<decimal?> CalculateSMA(List<Candlestick> prices, int period)
        {
            var maList = new List<decimal?>(new decimal?[prices.Count]);

            // Nếu dữ liệu không đủ số kỳ, trả về danh sách rỗng
            if (prices.Count < period) return maList;

            for (int i = period - 1; i < prices.Count; i++)
            {
                decimal sum = 0;
                for (int j = i - (period - 1); j <= i; j++)
                {
                    sum += prices[j].ClosePrice;
                }
                maList[i] = sum / period;
            }

            return maList;
        }

        /// <summary>
        /// Tính toán Exponential Moving Average (EMA) cho danh sách giá đóng cửa.
        /// EMA phản ứng nhanh hơn SMA vì nó gán trọng số cao hơn cho các giá gần đây.
        /// </summary>
        /// <param name="prices">Danh sách các nến giá.</param>
        /// <param name="period">Số kỳ để tính EMA (ví dụ: EMA 9, EMA 20, EMA 50).</param>
        /// <returns>Danh sách EMA theo từng nến, giá trị null nếu chưa đủ dữ liệu.</returns>
        private List<decimal?> CalculateEMA(List<Candlestick> prices, int period)
        {
            var emaList = new List<decimal?>(new decimal?[prices.Count]);

            // Nếu dữ liệu không đủ số kỳ, trả về danh sách rỗng
            if (prices.Count < period) return emaList;

            // Hệ số làm mượt α = 2 / (period + 1), giúp EMA phản ứng nhanh hơn với giá mới
            decimal multiplier = 2m / (period + 1);
            decimal? previousEMA = null; // Lưu EMA của kỳ trước đó

            for (int i = 0; i < prices.Count; i++)
            {
                if (i < period - 1)
                {
                    // Chưa đủ dữ liệu để tính EMA, để giá trị là null
                    emaList[i] = null;
                }
                else if (i == period - 1)
                {
                    // Khởi tạo EMA đầu tiên bằng SMA của 'period' giá đầu tiên
                    decimal sum = 0;
                    for (int j = i - (period - 1); j <= i; j++)
                        sum += prices[j].ClosePrice;

                    previousEMA = sum / period;
                    emaList[i] = previousEMA;
                }
                else
                {
                    // EMA = α * Giá hiện tại + (1 - α) * EMA trước đó
                    previousEMA = (prices[i].ClosePrice - previousEMA) * multiplier + previousEMA;
                    emaList[i] = previousEMA;
                }
            }

            return emaList;
        }

        /// <summary>
        /// Sinh tín hiệu giao dịch với ID để liên kết lệnh mở và lệnh đóng.
        /// - BUY khi EMA ngắn cắt lên EMA dài, tạo Trade ID.
        /// - SELL khi EMA ngắn cắt xuống EMA dài, gán ID của lệnh BUY gần nhất.
        /// - SHORT khi EMA ngắn cắt xuống EMA dài, tạo Trade ID.
        /// - COVER khi EMA ngắn cắt lên EMA dài, gán ID của lệnh SHORT gần nhất.
        /// </summary>
        /// <param name="prices">Danh sách nến giá.</param>
        /// <param name="shortEMA">Danh sách EMA ngắn hạn.</param>
        /// <param name="longEMA">Danh sách EMA dài hạn.</param>
        /// <returns>Danh sách tín hiệu giao dịch có ID.</returns>
        private List<TradeSignalResponse> GenerateTradeSignals(List<Candlestick> prices, List<decimal?> shortEMA, List<decimal?> longEMA)
        {
            var signals = new List<TradeSignalResponse>();
            string? activeLongTradeId = null;  // Lưu ID của lệnh BUY đang mở
            string? activeShortTradeId = null; // Lưu ID của lệnh SHORT đang mở

            for (int i = 1; i < prices.Count; i++)
            {
                if (shortEMA[i] == null || longEMA[i] == null || shortEMA[i - 1] == null || longEMA[i - 1] == null)
                    continue;

                // Kiểm tra điều kiện đóng các lệnh trước

                // Điều kiện đóng SHORT: Short EMA cắt lên Long EMA
                if (shortEMA[i - 1].Value <= longEMA[i - 1].Value && shortEMA[i].Value > longEMA[i].Value && activeShortTradeId != null)
                {
                    signals.Add(new TradeSignalResponse
                    {
                        TradeId = activeShortTradeId, // Gán ID của lệnh SHORT mở trước đó
                        Symbol = prices[i].Symbol,
                        Timestamp = prices[i].Timestamp,
                        Signal = "COVER",
                        PriceAtSignal = prices[i].ClosePrice
                    });

                    activeShortTradeId = null; // Reset ID vì lệnh đã đóng
                }

                // Điều kiện đóng LONG: Short EMA cắt xuống Long EMA
                if (shortEMA[i - 1].Value >= longEMA[i - 1].Value && shortEMA[i].Value < longEMA[i].Value && activeLongTradeId != null)
                {
                    signals.Add(new TradeSignalResponse
                    {
                        TradeId = activeLongTradeId, // Gán ID của lệnh BUY mở trước đó
                        Symbol = prices[i].Symbol,
                        Timestamp = prices[i].Timestamp,
                        Signal = "SELL",
                        PriceAtSignal = prices[i].ClosePrice
                    });

                    activeLongTradeId = null; // Reset ID vì lệnh đã đóng
                }

                // Sau khi đóng các lệnh, kiểm tra điều kiện mở lệnh mới

                // Mở lệnh BUY (cắt lên) nếu chưa có lệnh BUY đang mở
                if (shortEMA[i - 1].Value <= longEMA[i - 1].Value && shortEMA[i].Value > longEMA[i].Value && activeLongTradeId == null)
                {
                    activeLongTradeId = Guid.NewGuid().ToString(); // Tạo ID cho lệnh BUY
                    signals.Add(new TradeSignalResponse
                    {
                        TradeId = activeLongTradeId,
                        Symbol = prices[i].Symbol,
                        Timestamp = prices[i].Timestamp,
                        Signal = "BUY",
                        PriceAtSignal = prices[i].ClosePrice
                    });
                }

                // Mở lệnh SHORT (cắt xuống) nếu chưa có lệnh SHORT đang mở
                if (shortEMA[i - 1].Value >= longEMA[i - 1].Value && shortEMA[i].Value < longEMA[i].Value && activeShortTradeId == null)
                {
                    activeShortTradeId = Guid.NewGuid().ToString(); // Tạo ID cho lệnh SHORT
                    signals.Add(new TradeSignalResponse
                    {
                        TradeId = activeShortTradeId,
                        Symbol = prices[i].Symbol,
                        Timestamp = prices[i].Timestamp,
                        Signal = "SHORT",
                        PriceAtSignal = prices[i].ClosePrice
                    });
                }
            }

            return signals;
        }
    }
}
