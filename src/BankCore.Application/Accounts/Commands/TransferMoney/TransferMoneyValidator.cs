using FluentValidation;

namespace BankCore.Application.Accounts.Commands.TransferMoney;

public sealed class TransferMoneyValidator : AbstractValidator<TransferMoneyCommand>
{
    public TransferMoneyValidator()
    {
        RuleFor(x => x.SourceAccountId)
            .NotEmpty().WithMessage("La cuenta origen es requerida.");

        RuleFor(x => x.DestinationAccountId)
            .NotEmpty().WithMessage("La cuenta destino es requerida.");

        RuleFor(x => x.SourceAccountId)
            .NotEqual(x => x.DestinationAccountId)
            .WithMessage("La cuenta origen y destino no pueden ser la misma.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a cero.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("La moneda es requerida.")
            .Must(x => x == "MXN" || x == "USD" || x == "EUR")
            .WithMessage("Moneda no soportada.");
    }
}