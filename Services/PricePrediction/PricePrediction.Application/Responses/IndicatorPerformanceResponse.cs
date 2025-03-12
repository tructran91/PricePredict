using PricePrediction.Application.DTOs;

namespace PricePrediction.Application.Responses
{
    public class IndicatorPerformanceResponse
    {
        public string IndicatorType { get; set; }

        public List<DailyPerformance> DailyResults { get; set; }
    }
}
