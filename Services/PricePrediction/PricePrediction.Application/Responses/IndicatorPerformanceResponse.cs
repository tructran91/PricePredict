using PricePrediction.Application.DTOs;

namespace PricePrediction.Application.Responses
{
    public class IndicatorPerformanceResponse
    {
        public string IndicatorType { get; set; }

        public double? TotalWinRate { get; set; }

        public decimal? TotalProfit { get; set; }

        public List<DailyPerformance> DailyResults { get; set; }
    }
}
