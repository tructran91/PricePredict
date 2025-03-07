using AutoMapper;
using DataImport.Application.Candlesticks.Commands;
using DataImport.Application.Responses;
using DataImport.Core.Entities;
using DataImport.Core.Repositories;
using DataImport.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using PricePredict.Shared.Models;
using System.Text.Json;

namespace DataImport.Application.Candlesticks.Handlers
{
    public class ImportCandlestickFromApiHandler : IRequestHandler<ImportCandlestickFromApiCommand, BaseResponse<List<CandlestickResponse>>>
    {
        private readonly ICandlestickRepository _repository;
        private readonly IMarketDataService _marketDataService;
        private readonly ILogger<ImportCandlestickFromApiHandler> _logger;
        private readonly IMapper _mapper;

        public ImportCandlestickFromApiHandler(
            ICandlestickRepository repository,
            IMarketDataService marketDataService,
            ILogger<ImportCandlestickFromApiHandler> logger,
            IMapper mapper)
        {
            _repository = repository;
            _marketDataService = marketDataService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<CandlestickResponse>>> Handle(ImportCandlestickFromApiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"{nameof(ImportCandlestickFromApiHandler)}: {JsonSerializer.Serialize(request.Payload)}");

                var symbol = request.Payload.Symbol;
                var startTime = new DateTimeOffset(request.Payload.Date.Date, TimeSpan.Zero);
                var endTime = startTime.AddDays(1);

                // Lấy nến mới nhất đã lưu trong DB để biết lấy từ đâu tiếp
                var lastStoredCandle = await _repository.GetAsync(
                    predicate: t => t.Symbol == symbol && t.Timestamp >= startTime && t.Timestamp < endTime,
                    orderBy: x => x.OrderByDescending(y => y.Timestamp)
                );
                if (lastStoredCandle != null && lastStoredCandle.Count > 0)
                {
                    startTime = lastStoredCandle.First().Timestamp.AddMinutes(1);
                }

                var allCandlesFromAPI = new List<Candlestick>();

                while (startTime < endTime)
                {
                    var candles = await _marketDataService.GetCandlestickDataAsync(symbol, "1m", startTime, endTime);
                    if (!candles.Any()) break;

                    // Prevent Data return from API is not in the range of requested time
                    candles = candles.Where(c => c.Timestamp >= startTime && c.Timestamp < endTime).ToList();

                    allCandlesFromAPI.AddRange(candles);
                    startTime = candles.Max(c => c.Timestamp).AddMinutes(1);

                    if (!allCandlesFromAPI.Any())
                    {
                        return BaseResponse<List<CandlestickResponse>>.Failure("Cannot retrieve candlestick data.");
                    }
                }

                if (!allCandlesFromAPI.Any())
                {
                    return BaseResponse<List<CandlestickResponse>>.Success(new List<CandlestickResponse>(), "No new data.");
                }

                var existingCandles = await _repository.GetCandlesticksAsync(symbol, "1m", request.Payload.Date.ToUniversalTime(), endTime);
                var newCandles = allCandlesFromAPI.ExceptBy(existingCandles.Select(ec => ec.Timestamp), c => c.Timestamp).ToList();
                if (!newCandles.Any())
                {
                    return BaseResponse<List<CandlestickResponse>>.Success(new List<CandlestickResponse>(), "No new data.");
                }

                var createdSticks = await _repository.AddRangeAsync(newCandles);
                var response = _mapper.Map<List<CandlestickResponse>>(createdSticks);
                return BaseResponse<List<CandlestickResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ImportCandlestickFromApiHandler)}: {ex.Message}");
                return BaseResponse<List<CandlestickResponse>>.Failure("Error importing data from API");
            }
        }
    }
}
