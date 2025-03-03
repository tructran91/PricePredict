using DataImport.Core.Entities;

namespace DataImport.Core.Services
{
    public interface IMarketDataService
    {
        Task<List<Candlestick>> GetCandlestickDataAsync(string symbol, string timeframe, DateTime startDate, DateTime endDate);
    }
}
