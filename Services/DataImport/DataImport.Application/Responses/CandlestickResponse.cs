namespace DataImport.Application.Responses
{
    public class CandlestickResponse
    {
        public string Symbol { get; set; } = string.Empty;

        public string Timeframe { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }

        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }

        public decimal Volume { get; set; }
    }
}
