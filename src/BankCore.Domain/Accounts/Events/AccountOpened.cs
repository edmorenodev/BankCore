using BankCore.Domain.Shared;

namespace BankCore.Domain.Shared;

public sealed class AccountOpened : DomainEvent
{
    public Guid AccountId { get; }
    public Guid OwnerId { get; }
    public string AccountNumber { get; }
    public string Currency { get; }

    public AccountOpened(Guid accountId, Guid ownerId, string accountNumber, string currency)
    {
        AccountId = accountId;
        OwnerId = ownerId;
        AccountNumber = accountNumber;
        Currency = currency;
    }
}