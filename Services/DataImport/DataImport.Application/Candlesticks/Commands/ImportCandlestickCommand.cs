using MediatR;
using Microsoft.AspNetCore.Http;

namespace DataImport.Application.Candlesticks.Commands
{
    public class ImportCandlestickCommand : IRequest<bool>
    {
        public IFormFile File { get; set; }

        public ImportCandlestickCommand(IFormFile file)
        {
            File = file;
        }
    }
}
