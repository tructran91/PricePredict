namespace PricePrediction.Application.Responses
{
    public class IndicatorPerformanceResponse
    {
        public string IndicatorType { get; set; }

        public int TotalTrades { get; set; }

        public int WinTrades { get; set; }

        public double WinRate { get; set; }

        public decimal AverageProfit { get; set; }

        public decimal TotalProfit { get; set; }
    }
}
