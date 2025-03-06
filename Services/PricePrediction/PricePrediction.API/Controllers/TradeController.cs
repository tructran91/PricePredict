using MediatR;
using Microsoft.AspNetCore.Mvc;
using PricePrediction.Application.TradeResults.Queries;
using PricePrediction.Application.TradeSignals.Commands;

namespace PricePrediction.API.Controllers
{
    [Route("api/trade")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TradeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("signals")]
        public async Task<IActionResult> GetTradeSignals([FromQuery] AddTradeSignalsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Prediction = result });
        }

        [HttpGet("results")]
        public async Task<IActionResult> GetTradeResults([FromQuery] GetTradeResultsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(new { Prediction = result });
        }
    }
}
