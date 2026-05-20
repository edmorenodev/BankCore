using System.Net;
using BankCore.Domain.Shared;
using BankCore.Domain.Transactions.Events;

namespace BankCore.Domain.Transactions;

public sealed class Transaction : Entity
{
    public Guid Id { get; private set; }
    public Guid SourceAccountId { get; private set; }
    public Guid DestinationAccountId { get; private set; }
    public Money Amount { get; private set; } = default!;
    public TransactionType Type { get; private set; }
    public TransactionStatus Status { get; private set; }
    public string? FailureReason { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private Transaction() { }

    public static Transaction Create(
        Guid sourceAccountId,
        Guid destinationAccountId,
        Money amount,
        TransactionType type
    )
    {
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            SourceAccountId = sourceAccountId,
            DestinationAccountId = destinationAccountId,
            Amount = amount,
            Type = type,
            Status = TransactionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        return transaction;
    }

    public void Complete()
    {
        if (Status != TransactionStatus.Pending)
            throw new InvalidOperationException("Solo se puede completar una transacción pendiente.");

        Status = TransactionStatus.Completed;
        CompletedAt = DateTime.UtcNow;

        RaiseDomainEvent(new TransactionCompleted(
            Id,
            SourceAccountId,
            DestinationAccountId,
            Amount.Amount,
            Amount.Currency
        ));
    }

    public void Fail(string reason)
    {
        if (Status != TransactionStatus.Pending)
            throw new InvalidOperationException("Solo se puede fallar una transacción pendiente.");

        Status = TransactionStatus.Failed;
        FailureReason = reason;
    }
}