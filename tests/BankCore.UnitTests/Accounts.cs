using BankCore.Domain.Accounts;
using BankCore.Domain.Shared;
using FluentAssertions;

namespace BankCore.UnitTests.Accounts;

public class AccountTests
{
    [Fact]
    public void Open_ShouldCreateActiveAccountWithZeroBalance()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Checking, "MXN");

        account.Status.Should().Be(AccountStatus.Active);
        account.Balance.Should().Be(Money.Zero("MXN"));
        account.Currency.Should().Be("MXN");
    }

    [Fact]
    public void Debit_ShouldReduceBalance()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Checking, "MXN");
        account.Credit(new Money(1000, "MXN"));
        account.Debit(new Money(400, "MXN"));
        account.Balance.Should().Be(new Money(600, "MXN"));
    }

    [Fact]
    public void Debit_ShouldFail_WhenAccount_IsBlocked()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Checking, "MXN");
        account.Credit(new Money(1000, "MXN"));
        account.Block();

        Action act = () => account.Debit(new Money(100, "MXN"));

        act.Should().Throw<InvalidOperationException>().WithMessage("La cuenta está bloqueada.");
    }

    [Fact]
    public void Close_ShouldFail_WhenBalanceIsGreaterThanZero()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Checking, "MXN");
        account.Credit(new Money(500, "MXN"));

        Action act = () => account.Close();

        act.Should().Throw<InvalidOperationException>().WithMessage("No se puede cerrar una cuenta con saldo.");
    }

    [Fact]
    public void Debit_ShouldFail_WhenInsufficientBalance()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Checking, "MXN");
        account.Credit(new Money(100, "MXN"));

        Action act = () => account.Debit(new Money(500, "MXN"));

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("El monto a debitar no debe exceder el saldo de la cuenta.");
    }
}