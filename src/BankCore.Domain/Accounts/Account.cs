namespace BankCore.Domain.Accounts;

public class Account
{
    public Guid Id { get; private set; }
    public string AccountNumber { get; private set; } = default!;
    public Guid OwnerId { get; private set; }
    public AccountType Type { get; private set; }
    public AccountStatus Status { get; private set; }
    public decimal Balance { get; private set; }
    public string Currency { get; private set; } = default!;
    public DateTime OpenedAt { get; private set; }
    public DateTime? LastMovementAt { get; private set; }
    public uint Version { get; private set; }

    // Constructor privado - Nadie crea un Account con "new Account()"
    private Account() { }

    public static Account Open(Guid ownerId, AccountType type, string currency)
    {
        return new Account
        {
            Id = Guid.NewGuid(),
            AccountNumber = GenerateAccountNumber(),
            OwnerId = ownerId,
            Type = type,
            Status = AccountStatus.Active,
            Balance = 0,
            Currency = currency,
            OpenedAt = DateTime.UtcNow
        };
    }

    public void Debit(decimal amount)
    {
        EnsureIsActive();

        if (amount <= 0)
        {
            throw new InvalidOperationException("El monto a debitar debe ser mayor a 0");
        }

        if (Balance < amount)
        {
            throw new InvalidOperationException("Saldo insuficiente");
        }

        Balance -= amount;
        LastMovementAt = DateTime.UtcNow;
    }

    public void Credit(decimal amount)
    {
        EnsureIsActive();

        if (amount <= 0)
        {
            throw new InvalidOperationException("El monto a acreditar debe ser mayor a 0");
        }

        Balance += amount;
        LastMovementAt = DateTime.UtcNow;
    }

    public void Block()
    {
        if (Status == AccountStatus.Closed)
        {
            throw new InvalidOperationException("No se puede bloquear una cuenta cerrada");
        }

        Status = AccountStatus.Blocked;
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
