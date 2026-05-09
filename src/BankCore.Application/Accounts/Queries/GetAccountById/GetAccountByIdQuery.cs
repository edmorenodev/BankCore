using MediatR;

namespace BankCore.Application.Accounts.Queries.GetAccountById;

public sealed record GetAccountByIdQuery(Guid AccountId) : IRequest<AccountResponse>;