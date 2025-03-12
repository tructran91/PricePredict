namespace PricePrediction.Application.DTOs
{
    public class DailyPerformance
    {
        public string IndicatorType { get; set; }

        public DateTimeOffset Date { get; set; }

        public double WinRate { get; set; }

        public decimal TotalProfit { get; set; }
    }
}
