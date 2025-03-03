namespace PricePrediction.Application.Responses
{
    public class TradeSignalResponse
    {
        public DateTime Date { get; set; }

        public string Signal { get; set; } // "BUY" hoặc "SELL"
    }
}
