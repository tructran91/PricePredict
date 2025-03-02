using MediatR;
using PricePrediction.Application.PredictPrice.Commands;
using PricePrediction.Core.Algorithms;
using PricePrediction.Core.Entities;
using PricePrediction.Core.Repositories;

namespace PricePrediction.Application.PredictPrice.Handler
{
    public class PredictPriceHandler : IRequestHandler<PredictPriceCommand, string>
    {
        private readonly IBaseRepository<StockPrice> _repository;
        private readonly ILogger<PredictPriceHandler> _logger;

        public PredictPriceHandler(IBaseRepository<StockPrice> repository, ILogger<PredictPriceHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<string> Handle(PredictPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var prices = await _context.StockPrices
                //    .Where(p => p.Symbol == request.Symbol)
                //    .OrderByDescending(p => p.Date)
                //    .Take(request.LongPeriod)
                //    .ToListAsync(cancellationToken);

                //PredictionAlgorithm algorithm = new PricePrediction.Domain.MovingAverageCrossoverAlgorithm();

                //return algorithm.Predict(prices, request.ShortPeriod, request.LongPeriod);
                return "";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error predicting price: {ex.Message}");
                return "Error";
            }
        }
    }
}
