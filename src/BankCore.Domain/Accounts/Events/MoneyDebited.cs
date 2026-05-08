using BankCore.Domain.Shared;

namespace BankCore.Domain.Accounts.Events;

public sealed class MoneyDebited : DomainEvent
{
    public Guid AccountId { get; }
    public decimal Amount { get; }
    public string Currency { get; }

    public MoneyDebited(Guid accountId, decimal amount, string currency)
    {
        AccountId = accountId;
        Amount = amount;
        Currency = currency;
    }
}