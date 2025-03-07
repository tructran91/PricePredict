using FluentValidation;
using PricePredict.Shared.Constants;
using PricePrediction.Application.TradeResults.Queries;

namespace PricePrediction.Application.TradeResults.Validators
{
    public class GetTradeResultsValidator : AbstractValidator<GetTradeResultsQuery>
    {
        private readonly DateTime _minDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private readonly DateTime _maxFutureDate = DateTime.UtcNow.AddDays(1);

        public GetTradeResultsValidator()
        {
            RuleFor(x => x.Symbol)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Symbol"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Symbol"))
                .Must(symbol => CandlestickSetting.ValidSymbols.Contains(symbol)).WithMessage("Invalid symbol.");

            When(q => !string.IsNullOrEmpty(q.Timeframe), () => {
                RuleFor(x => x.Timeframe)
                .Must(timeframe => CandlestickSetting.ValidTimeframes.Contains(timeframe)).WithMessage("Invalid timeframe.");
            });

            When(q => q.StartDateTime.HasValue && q.EndDateTime.HasValue, () => {
                RuleFor(x => x.StartDateTime)
                    .GreaterThanOrEqualTo(_minDate).WithMessage($"StartTime must be after {_minDate:yyyy-MM-dd}.")
                    .LessThanOrEqualTo(x => x.EndDateTime).WithMessage("StartTime must be before or equal to end date.");

                RuleFor(x => x.EndDateTime)
                    .LessThanOrEqualTo(_maxFutureDate).WithMessage("EndTime cannot exceed 1 day in the future.");
            });
        }
    }
}
