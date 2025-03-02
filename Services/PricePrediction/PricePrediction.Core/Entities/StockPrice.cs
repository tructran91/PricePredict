namespace PricePrediction.Core.Entities
{
    public class StockPrice : BaseEntity
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public decimal Close { get; set; }
    }
}
