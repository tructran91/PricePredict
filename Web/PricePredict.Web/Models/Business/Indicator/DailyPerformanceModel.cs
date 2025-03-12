namespace PricePredict.Web.Models.Business.Indicator
{
    public class DailyPerformanceModel
    {
        public string IndicatorType { get; set; }

        public DateTime Date { get; set; }

        public double WinRate { get; set; }

        public decimal TotalProfit { get; set; }
    }
}
