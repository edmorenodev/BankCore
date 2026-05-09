using FluentValidation;

namespace BankCore.Application.Accounts.Commands.OpenAccount;

public sealed class OpenAccountValidator : AbstractValidator<OpenAccountCommand>
{
    public OpenAccountValidator()
    {
        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("El OwnerId es requerido.");

        RuleFor(x => x.AccountType)
            .NotEmpty().WithMessage("El tipo de cuenta es requerido.")
            .Must(x => x == "Checking" || x == "Savings")
            .WithMessage("El tipo de cuenta debe ser Checking o Savings");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("La moneda es requerida.")
            .Must(x => x == "MXN" || x == "USD" || x == "EUR")
            .WithMessage("Moneda no soportada");
    }
}