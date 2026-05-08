namespace BankCore.Domain.Shared;
/* 
    Esto es un Value Object, representa dinero mediante valores y reglas del dominio.
    En este caso Money se puede usar como tipo dentro de la clase Account.

    public Money Balance { get; private set; } <- Balance es del tipo Money

    Encapsula reglas bancarias del concepto como:
    - No permitir montos negativos
    - Validar monedas soportadas
    - Evitar operaciones entre monedas distintas

    El value object tambien puede tener comportamiento relacionado con el conecepto,
    (sus propios métodos) para ser utilizados en otra clase, por ejemplo en este value object de Money:
    - Usamos los métodos de Money en Account para realizar operaciones.
    
    En lugar de:
        Balance = Balance + amount;
    
    Usamos:
        Balance = Balance.Add(amount);
*/
public sealed class Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    private static readonly HashSet<string> ValidCurrencies = ["MXN", "USD", "EUR"];

    private Money() { }

    public Money(decimal amount, string currency)
    {
        if (amount < 0)
        {
            throw new ArgumentException("El monto no puede ser negativo.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("La moneda es requerida");
        }

        if (!ValidCurrencies.Contains(currency.ToUpper()))
        {
            throw new ArgumentException($"Moneda no soportada: {currency}.");
        }

        Amount = amount;
        Currency = currency.ToUpper();
    }

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);
        if (Amount < other.Amount)
        {
            throw new InvalidOperationException("El resultado no puede ser negativo");
        }

        return new Money(Amount - other.Amount, Currency);
    }

    public bool IsGreaterThan(Money other)
    {
        EnsureSameCurrency(other);
        return Amount > other.Amount;
    }
    private void EnsureSameCurrency(Money other)
    {
        if(Currency != other.Currency)
        {
            throw new InvalidOperationException($"No se pueden operar monedas distintas {Currency} y {other.Currency}");
        }
    }

    // Dos Money son iguales si tienen el mismo monto y moneda
    public override bool Equals(object? obj) => obj is Money other && Amount == other.Amount && Currency == other.Currency;

    public override int GetHashCode() => HashCode.Combine(Amount, Currency);

    public override string ToString() => $"{Amount:N2} {Currency}";

    public static Money Zero(string currency) => new(0, currency);
}