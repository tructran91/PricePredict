using DataImport.Application.Candlesticks.Commands;
using DataImport.Application.Candlesticks.Queries;
using DataImport.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DataImport.API.Controllers
{
    [Route("api/candlestick")]
    [ApiController]
    public class CandlestickController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CandlestickController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("candlesticks")]
        public async Task<IActionResult> GetCandlesticks([FromQuery] GetCandlesticksQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("stock-data")]
        public async Task<IActionResult> ImportStockData([FromForm] ImportCandlestickRequest request)
        {
            var result = await _mediator.Send(new ImportCandlestickCommand(request.File));
            return Ok(result);
        }

        [HttpPost("stock-data-api")]
        public async Task<IActionResult> ImportStockDataFromApi([FromQuery] ImportCandlestickFromApiRequest request)
        {
            var result = await _mediator.Send(new ImportCandlestickFromApiCommand(request));
            return Ok(result);
        }
    }
}
