using MediatR;
using Microsoft.AspNetCore.Mvc;
using PricePrediction.Application.TradeResults.Queries;

namespace PricePrediction.API.Controllers
{
    [Route("api/trade-results")]
    [ApiController]
    public class TradeResultsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TradeResultsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTradeResults([FromQuery] GetTradeResultsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(new { Prediction = result });
        }
    }
}
