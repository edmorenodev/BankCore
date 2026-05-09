using BankCore.Domain.Shared;
using BankCore.Domain.Accounts.Events;

namespace BankCore.Domain.Accounts;

public class Account : Entity
{
    public Guid Id { get; private set; }
    public string AccountNumber { get; private set; } = default!;
    public Guid OwnerId { get; private set; }
    public AccountType Type { get; private set; }
    public AccountStatus Status { get; private set; }
    public Money Balance { get; private set; } = default!;
    public string Currency { get; private set; } = default!;
    public DateTime OpenedAt { get; private set; }
    public DateTime? LastMovementAt { get; private set; }
    public uint Version { get; private set; }

    // Constructor privado - Nadie crea un Account con "new Account()"
    private Account() { }

    public static Account Open(Guid ownerId, AccountType type, string currency)
    {
        var account = new Account
        {
            Id = Guid.NewGuid(),
            AccountNumber = GenerateAccountNumber(),
            OwnerId = ownerId,
            Type = type,
            Status = AccountStatus.Active,
            Balance = Money.Zero(currency),
            Currency = currency,
            OpenedAt = DateTime.UtcNow
        };

        account.RaiseDomainEvent(new AccountOpened(
            account.Id,
            account.OwnerId,
            account.AccountNumber,
            account.Currency
        ));

        return account;
    }

    public void Debit(Money amount)
    {
        EnsureIsActive();

        if (amount.Amount <= 0)
        {
            throw new InvalidOperationException("El monto a debitar debe ser mayor a 0");
        }

        if (amount.IsGreaterThan(Balance))
        {
            throw new InvalidOperationException("El monto a debitar no debe exceder el saldo de la cuenta.");
        }

        Balance = Balance.Subtract(amount);
        LastMovementAt = DateTime.UtcNow;

        RaiseDomainEvent(new MoneyDebited(Id, amount.Amount, amount.Currency));
    }

    public void Credit(Money amount)
    {
        EnsureIsActive();

        if (amount.Amount <= 0)
        {
            throw new InvalidOperationException("El monto a acreditar debe ser mayor a 0");
        }

        Balance = Balance.Add(amount);
        LastMovementAt = DateTime.UtcNow;

        RaiseDomainEvent(new MoneyDebited(Id, amount.Amount, amount.Currency));
    }

    public void Block()
    {
        if (Status == AccountStatus.Closed)
        {
            throw new InvalidOperationException("No se puede bloquear una cuenta cerrada");
        }

        Status = AccountStatus.Blocked;
    }

    public void Close()
    {
        if (Balance.Amount > 0)
        {
            throw new InvalidOperationException("No se puede cerrar una cuenta con saldo.");
        }

        Status = AccountStatus.Closed;
    }

    private void EnsureIsActive()
    {
        if (Status == AccountStatus.Blocked)
        {
            throw new InvalidOperationException("La cuenta está bloqueada.");
        }

        if (Status == AccountStatus.Closed)
        {
            throw new InvalidOperationException("La cuenta está cerrada.");
        }
    }
    private static string GenerateAccountNumber()
    {
        return $"AC-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Random.Shared.Next(1000, 9999)}";
    }
}