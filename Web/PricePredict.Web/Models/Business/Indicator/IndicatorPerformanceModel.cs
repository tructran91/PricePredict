using System.ComponentModel.DataAnnotations;

namespace PricePredict.Web.Models.Business.Indicator
{
    public class IndicatorPerformanceModel
    {
        public string IndicatorType { get; set; }

        public List<DailyPerformanceModel> DailyResults { get; set; }
    }
}
