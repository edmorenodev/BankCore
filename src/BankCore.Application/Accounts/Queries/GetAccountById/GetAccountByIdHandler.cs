using BankCore.Domain.Accounts;
using MediatR;

namespace BankCore.Application.Accounts.Queries.GetAccountById;

public sealed class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, AccountResponse>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountByIdHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountResponse> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
    
        if(account is null)
            throw new KeyNotFoundException($"Cuenta {request.AccountId} no encontrada");
        
        return new AccountResponse(
            account.Id,
            account.AccountNumber,
            account.OwnerId,
            account.Type.ToString(),
            account.Status.ToString(),
            account.Balance.Amount,
            account.Balance.Currency,
            account.OpenedAt,
            account.LastMovementAt
        );
    }
}