using BankCore.Domain.Accounts;
using BankCore.Domain.Shared;
using MediatR;

namespace BankCore.Application.Accounts.Commands.TransferMoney;

public sealed class TransferMoneyHandler : IRequestHandler<TransferMoneyCommand, Guid>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransferMoneyHandler(
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
    {
        var source = await _accountRepository.GetByIdAsync(request.SourceAccountId, cancellationToken);

        if (source is null)
            throw new KeyNotFoundException($"Cuenta origen {request.SourceAccountId} no encontrada.");

        var destination = await _accountRepository.GetByIdAsync(request.DestinationAccountId);

        if(destination is null)
            throw new KeyNotFoundException($"Cuenta destino {request.DestinationAccountId} no encontrada");

        var amount = new Money(request.Amount, request.Currency);

        source.Debit(amount);
        destination.Credit(amount);

        await _accountRepository.UpdateAsync(source, cancellationToken);
        await _accountRepository.UpdateAsync(destination, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return source.Id;
    }
}