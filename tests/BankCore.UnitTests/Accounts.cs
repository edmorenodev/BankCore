using BankCore.Domain.Accounts;
using FluentAssertions;

namespace BankCore.UnitTests.Accounts;

public class AccountTests
{
    [Fact]
    public void Open_ShouldCreateActiveAccountWithZeroBalance()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Checking, "MXN");

        account.Status.Should().Be(AccountStatus.Active);
        account.Balance.Should().Be(0);
        account.Currency.Should().Be("MXN");
    }

    [Fact]
    public void Debit_ShouldReduceBalance()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Checking, "MXN");
        account.Credit(1000);
        account.Debit(400);
        account.Balance.Should().Be(600);
    }

    [Fact]    
    public void Debit_ShouldFail_WhenAccount_IsBlocked()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Checking, "MXN");
        account.Credit(1000);
        account.Block();

        Action act = () => account.Debit(100);

        act.Should().Throw<InvalidOperationException>().WithMessage("La cuenta está bloqueada");
    }

    [Fact]
    public void Close_ShouldFail_WhenBalanceIsGreaterThanZero()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Checking, "MXN");
        account.Credit(500);

        Action act = () => account.Close();

        act.Should().Throw<InvalidOperationException>().WithMessage("No se puede cerrar una cuenta con saldo");
    }
}