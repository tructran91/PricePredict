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

        public async Task<List<Candlestick>> GetCandlestickDataAsync(string symbol, string timeframe)
        {
            string url = $"https://api.binance.com/api/v3/klines?symbol={symbol}&interval={timeframe}";
            var response = await _httpClient.GetStringAsync(url);

            using (JsonDocument document = JsonDocument.Parse(response))
            {
                var candles = new List<Candlestick>();
                foreach (JsonElement element in document.RootElement.EnumerateArray())
                {
                    var candle = new Candlestick
                    {
                        Symbol = symbol,
                        Timeframe = timeframe,
                        Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(element[0].GetInt64()).UtcDateTime,
                        OpenPrice = decimal.Parse(element[1].GetString()),
                        HighPrice = decimal.Parse(element[2].GetString()),
                        LowPrice = decimal.Parse(element[3].GetString()),
                        ClosePrice = decimal.Parse(element[4].GetString()),
                        Volume = decimal.Parse(element[5].GetString())
                    };
                    candles.Add(candle);
                }
                return candles.OrderBy(c => c.Timestamp).ToList();
            }
        }
    }
}
