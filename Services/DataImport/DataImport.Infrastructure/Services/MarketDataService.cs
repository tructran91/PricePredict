using DataImport.Core.Entities;
using DataImport.Core.Services;
using System.Text.Json;

namespace DataImport.Infrastructure.Services
{
    public class MarketDataService : IMarketDataService
    {
        private readonly HttpClient _httpClient;

        public MarketDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Candlestick>> GetCandlestickDataAsync(string symbol, string timeframe, DateTime startDate, DateTime endDate)
        {
            var url = BuildBinanceUrl(symbol, timeframe, startDate, endDate);
            var response = await _httpClient.GetStringAsync(url);
            using var document = JsonDocument.Parse(response);

            var candles = document.RootElement.EnumerateArray().Select(element => new Candlestick
            {
                Symbol = symbol,
                Timeframe = timeframe,
                Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(element[0].GetInt64()).UtcDateTime,
                OpenPrice = decimal.Parse(element[1].GetString()!),
                HighPrice = decimal.Parse(element[2].GetString()!),
                LowPrice = decimal.Parse(element[3].GetString()!),
                ClosePrice = decimal.Parse(element[4].GetString()!),
                Volume = decimal.Parse(element[5].GetString()!)
            })
                .OrderBy(c => c.Timestamp)
                .DistinctBy(c => c.Timestamp)
                .ToList();

            return candles;
        }

        private static string BuildBinanceUrl(string symbol, string timeframe, DateTime startDate, DateTime endDate)
        {
            var startTime = new DateTimeOffset(startDate).ToUnixTimeMilliseconds();
            var endTime = new DateTimeOffset(endDate).ToUnixTimeMilliseconds();
            return $"https://api.binance.com/api/v3/klines?symbol={symbol}&interval={timeframe}&startTime={startTime}&endTime={endTime}&limit=500";
        }

    }
}
