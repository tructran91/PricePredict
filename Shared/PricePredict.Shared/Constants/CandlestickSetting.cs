namespace PricePredict.Shared.Constants
{
    public static class CandlestickSetting
    {
        public static readonly HashSet<string> ValidTimeframes = new HashSet<string>
        {
            "1m", "5m", "15m", "30m", "1h", "2h", "4h", "1d", "1w", "1mo"
        };

        public static readonly HashSet<string> ValidSymbols = new HashSet<string>
        {
            "BTCUSDT", "ETHUSDT", "AAPL", "GOOGL", "MSFT", "TSLA"
        };
    }
}
