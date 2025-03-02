using DataImport.Application.Candlesticks.Commands;
using DataImport.Core.Entities;
using DataImport.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DataImport.Application.Candlesticks.Handlers
{
    public class ImportCandlestickHandler : IRequestHandler<ImportCandlestickCommand, bool>
    {
        private readonly IBaseRepository<Candlestick> _repository;
        private readonly ILogger<ImportCandlestickHandler> _logger;

        public ImportCandlestickHandler(IBaseRepository<Candlestick> repository, ILogger<ImportCandlestickHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(ImportCandlestickCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var candles = new List<CandleStick>();

                //using (var stream = new StreamReader(request.File.OpenReadStream()))
                //using (var csv = new CsvReader(stream, CultureInfo.InvariantCulture))
                //{
                //    var records = csv.GetRecords<CandlestickCsvModel>().ToList();

                //    foreach (var record in records)
                //    {
                //        var candle = new CandleStick
                //        {
                //            Symbol = record.Symbol,
                //            Timeframe = record.Timeframe,
                //            Timestamp = record.Timestamp.ToUniversalTime(), // Chuyển sang UTC
                //            OpenPrice = record.OpenPrice,
                //            HighPrice = record.HighPrice,
                //            LowPrice = record.LowPrice,
                //            ClosePrice = record.ClosePrice,
                //            Volume = record.Volume
                //        };

                //        candles.Add(candle);
                //    }
                //}

                //await _context.CandlestickData.AddRangeAsync(candles, cancellationToken);
                //await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error importing data: {ex.Message}");
                return false;
            }
        }
    }
}
