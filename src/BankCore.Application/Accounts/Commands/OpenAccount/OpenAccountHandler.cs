using BankCore.Domain.Accounts;
using BankCore.Domain.Shared;
using MediatR;

namespace BankCore.Application.Accounts.Commands.OpenAccount;

public sealed class OpenAccountHandler : IRequestHandler<OpenAccountCommand, Guid>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OpenAccountHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(OpenAccountCommand request, CancellationToken cancellationToken)
    {
        var accountType = Enum.Parse<AccountType>(request.AccountType);

        var account = Account.Open(request.OwnerId, accountType, request.Currency);

        await _accountRepository.AddAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return account.Id;
    }
}