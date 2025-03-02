using Microsoft.AspNetCore.Http;

namespace DataImport.Application.Requests
{
    public class ImportCandlestickRequest
    {
        public IFormFile File { get; set; }
    }
}
