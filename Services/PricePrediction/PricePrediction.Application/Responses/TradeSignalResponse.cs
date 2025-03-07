namespace PricePrediction.Application.Responses
{
    public class TradeSignalResponse
    {
        public string Symbol { get; set; } = string.Empty;

        public decimal PriceAtSignal { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// BUY → SELL = Mua vào, bán ra.
        /// SHORT → COVER = Bán khống, mua lại để đóng lệnh.
        /// </summary>
        public string Signal { get; set; }

        public string TradeId { get; set; }

        public string IndicatorType { get; set; }
    }
}
