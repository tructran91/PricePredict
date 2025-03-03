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
        }
    }
}
