namespace DataImport.Application.Requests
{
    public class ImportCandlestickFromApiRequest
    {
        public string Symbol { get; set; }

        public string Timeframe { get; set; }
    }
}
