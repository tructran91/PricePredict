using PricePrediction.Core.Entities;

namespace PricePrediction.Core.Algorithms
{
    public abstract class PredictionAlgorithm
    {
        public abstract string Predict(List<StockPrice> prices, int shortPeriod, int longPeriod);
    }
}
