using MediatR;
using Microsoft.AspNetCore.Mvc;
using PricePrediction.Application.PredictPrice.Commands;

namespace PricePrediction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PredictionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("price")]
        public async Task<IActionResult> PredictPrice([FromBody] PredictPriceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Prediction = result });
        }
    }
}
