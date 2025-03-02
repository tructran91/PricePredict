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
        private readonly IBaseRepository<Candlestick> _repository;
        private readonly IMarketDataService _marketDataService;
        private readonly ILogger<ImportCandlestickFromApiHandler> _logger;
        private readonly IMapper _mapper;

        public ImportCandlestickFromApiHandler(
            IBaseRepository<Candlestick> repository,
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

                var candles = await _marketDataService.GetCandlestickDataAsync(request.Payload.Symbol, request.Payload.Timeframe);

                if (candles == null || candles.Count == 0)
                {
                    _logger.LogError($"{nameof(ImportCandlestickFromApiHandler)}: Cannot retrieve candlestick data.");
                    return BaseResponse<List<CandlestickResponse>>.Failure("Cannot retrieve candlestick data.");
                }

                var createdSticks =  await _repository.AddRangeAsync(candles);
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
