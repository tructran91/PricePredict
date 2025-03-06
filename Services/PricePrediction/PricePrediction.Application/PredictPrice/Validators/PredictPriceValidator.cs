﻿using FluentValidation;
using PricePredict.Shared.Constants;
using PricePrediction.Application.PredictPrice.Commands;

namespace PricePrediction.Application.PredictPrice.Validators
{
    public class PredictPriceValidator : AbstractValidator<PredictPriceCommand>
    {
        private readonly DateTime _minDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private readonly DateTime _maxFutureDate = DateTime.UtcNow.AddDays(1);

        public PredictPriceValidator()
        {
            RuleFor(x => x.Symbol)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Symbol"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Symbol"))
                .Must(symbol => CandlestickSetting.ValidSymbols.Contains(symbol)).WithMessage("Invalid symbol.");

            RuleFor(x => x.Timeframe)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Timeframe"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Timeframe"))
                .Must(timeframe => CandlestickSetting.ValidTimeframes.Contains(timeframe)).WithMessage("Invalid timeframe.");

            RuleFor(x => x.StartDate)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("StartTime"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("StartTime"))
                .GreaterThanOrEqualTo(_minDate).WithMessage($"StartTime must be after {_minDate:yyyy-MM-dd}.")
                .LessThanOrEqualTo(x => x.EndDate).WithMessage("StartTime must be before or equal to end date.");

            RuleFor(x => x.EndDate)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("EndTime"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("EndTime"))
                .LessThanOrEqualTo(_maxFutureDate).WithMessage("EndTime cannot exceed 1 day in the future.");
        }
    }
}
