using PricePrediction.Core.Entities;

namespace PricePrediction.Core.Algorithms
{
    public class MovingAverageCrossoverAlgorithm : PredictionAlgorithm
    {
        public override string Predict(List<StockPrice> prices, int shortPeriod, int longPeriod)
        {
            if (prices.Count < longPeriod) return "Insufficient data";

            var shortMA = prices.TakeLast(shortPeriod).Average(p => p.Close);
            var longMA = prices.TakeLast(longPeriod).Average(p => p.Close);

            return shortMA > longMA ? "BUY" : "SELL";
        }
    }
}
