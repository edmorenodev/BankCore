using MediatR;

namespace BankCore.Application.Accounts.Commands.OpenAccount;

public sealed record OpenAccountCommand(
    Guid OwnerId,
    string AccountType,
    string Currency
) : IRequest<Guid>;