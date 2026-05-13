using MediatR;

namespace BankCore.Application.Accounts.Commands.TransferMoney;

public sealed record TransferMoneyCommand(
    Guid SourceAccountId,
    Guid DestinationAccountId,
    decimal Amount,
    string Currency
) : IRequest<Guid>;