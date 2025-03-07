namespace PricePrediction.Core.Entities
{
    public class TradeResult : BaseEntity
    {
        public Guid TradeId { get; set; } // Liên kết với TradeSignal
        public string Symbol { get; set; }
        public string Timeframe { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string IndicatorType { get; set; } // SMA, EMA, MACD, RSI...

        public string Signal { get; set; } // BUY, SHORT
        public decimal PriceAtSignal { get; set; } // Giá khi nhận tín hiệu
        public decimal ExitPrice { get; set; } // Giá thoát lệnh

        public bool IsWin { get; set; } // True nếu lệnh thắng, False nếu thua
        public decimal Profit { get; set; } // Lợi nhuận/tổn thất của lệnh
    }

}
