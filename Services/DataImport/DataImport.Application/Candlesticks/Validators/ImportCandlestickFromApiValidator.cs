using DataImport.Application.Candlesticks.Commands;
using FluentValidation;
using PricePredict.Shared.Constants;

namespace DataImport.Application.Candlesticks.Validators
{
    public class ImportCandlestickFromApiValidator : AbstractValidator<ImportCandlestickFromApiCommand>
    {
        public ImportCandlestickFromApiValidator()
        {
            RuleFor(x => x.Payload.Symbol)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Symbol"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Symbol"))
                .Must(symbol => CandlestickSetting.ValidSymbols.Contains(symbol)).WithMessage("Invalid symbol.");

            RuleFor(x => x.Payload.Timeframe)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Timeframe"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Timeframe"))
                .Must(timeframe => CandlestickSetting.ValidTimeframes.Contains(timeframe)).WithMessage("Invalid timeframe.");
        }
    }
}
