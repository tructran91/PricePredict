using MediatR;
using Microsoft.AspNetCore.Mvc;
using PricePrediction.Application.TradeSignals.Commands;
using PricePrediction.Application.TradeSignals.Queries;

namespace PricePrediction.API.Controllers
{
    [Route("api/trade-signals")]
    [ApiController]
    public class TradeSignalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TradeSignalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTradeSignals([FromQuery] GetTradeSignalsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddTradeSignals([FromQuery] AddTradeSignalsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
