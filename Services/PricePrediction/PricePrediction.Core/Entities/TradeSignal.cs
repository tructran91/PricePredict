namespace PricePrediction.Core.Entities
{
    public class TradeSignal : BaseEntity
    {
        public string Timeframe { get; set; }

        public string Symbol { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public string IndicatorType { get; set; }

        /// <summary>
        /// BUY → SELL = Mua vào, bán ra.
        /// SHORT → COVER = Bán khống, mua lại để đóng lệnh.
        /// </summary>
        public string Signal { get; set; }

        public decimal PriceAtSignal { get; set; }

        public string TradeId { get; set; }
    }
}
