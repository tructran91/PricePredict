namespace DataImport.Application.Requests
{
    public class ImportCandlestickFromApiRequest
    {
        public string Symbol { get; set; }

        public DateTime Date { get; set; }
    }
}
