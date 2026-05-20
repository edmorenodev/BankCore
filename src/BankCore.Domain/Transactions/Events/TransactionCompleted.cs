using BankCore.Domain.Shared;

namespace BankCore.Domain.Transactions.Events;

public sealed class TransactionCompleted : DomainEvent
{
    public Guid TransactionId { get; }
    public Guid SourceAccountId { get; }
    public Guid DestinationAccountId { get; }
    public decimal Amount { get; }
    public string Currency { get; }

    public TransactionCompleted(
        Guid transactionId,
        Guid sourceAccountId,
        Guid destinationAccountId,
        decimal amount,
        string currency)
    {
        TransactionId = transactionId;
        SourceAccountId = sourceAccountId;
        DestinationAccountId = destinationAccountId;
        Amount = amount;
        Currency = currency;
    }
}