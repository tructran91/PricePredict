﻿using DataImport.Core.Entities;

namespace DataImport.Core.Repositories
{
    public interface ICandlestickRepository : IBaseRepository<Candlestick>
    {
        Task<List<Candlestick>> GetCandlesticksAsync(string symbol, string timeframe, DateTime startTime, DateTime endTime);
    }
}
