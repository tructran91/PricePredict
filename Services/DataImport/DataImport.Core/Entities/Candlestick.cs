﻿namespace DataImport.Core.Entities
{
    public class Candlestick : BaseEntity
    {
        public string Symbol { get; set; } = string.Empty; // Mã chứng khoán / Crypto (SHB, VPB...)

        public string Timeframe { get; set; } = string.Empty; // Lưu 1m

        public DateTime Timestamp { get; set; } // Thời điểm mở nến (UTC)

        public decimal OpenPrice { get; set; } // Giá mở cửa
        public decimal HighPrice { get; set; } // Giá cao nhất
        public decimal LowPrice { get; set; } // Giá thấp nhất
        public decimal ClosePrice { get; set; } // Giá đóng cửa

        public decimal Volume { get; set; } // Khối lượng giao dịch

        public long GetUnixTimestamp() => new DateTimeOffset(Timestamp).ToUnixTimeMilliseconds();
    }
}
