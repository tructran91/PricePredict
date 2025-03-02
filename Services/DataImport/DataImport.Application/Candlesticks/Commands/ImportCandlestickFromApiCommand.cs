using DataImport.Application.Requests;
using DataImport.Application.Responses;
using MediatR;
using PricePredict.Shared.Models;

namespace DataImport.Application.Candlesticks.Commands
{
    public class ImportCandlestickFromApiCommand : IRequest<BaseResponse<List<CandlestickResponse>>>
    {
        public ImportCandlestickFromApiRequest Payload { get; set; }

        public ImportCandlestickFromApiCommand(ImportCandlestickFromApiRequest payload)
        {
            Payload = payload;
        }
    }
}
