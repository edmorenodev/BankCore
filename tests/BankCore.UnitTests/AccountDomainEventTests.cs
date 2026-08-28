using BankCore.Domain.Accounts;
using BankCore.Domain.Accounts.Events;
using BankCore.Domain.Shared;
using FluentAssertions;

namespace BankCore.UnitTests.Accounts;

public class AccountDomainEventTests
{
    [Fact]
    public void Open_ShouldRaise_AccountOpenedEvent()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Checking, "MXN");

        account.DomainEvents.Should().ContainSingle(e => e is AccountOpened);
    }

    [Fact]
    public void Debit_ShouldRaise_MoneyDebitedEvent()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Checking, "MXN");
        account.Credit(new Money(500, "MXN"));
        account.ClearDomainEvents();

        account.Debit(new Money(200, "MXN"));

        account.DomainEvents.Should().ContainSingle(e => e is MoneyDebited);
    }

    [Fact]
    public void Credit_ShouldRaise_MoneyCreditedEvent()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Checking, "MXN");
        account.ClearDomainEvents();

        account.Credit(new Money(300, "MXN"));

        account.DomainEvents.Should().ContainSingle(e => e is MoneyCredited);
    }

    [Fact]
    public void Block_ShouldChangeStatus_ToBlocked()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Savings, "USD");

        account.Block();

        account.Status.Should().Be(AccountStatus.Blocked);
    }

    [Fact]
    public void Block_ShouldFail_WhenAccountIsClosed()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Checking, "MXN");
        account.Close();

        Action act = () => account.Block();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("No se puede bloquear una cuenta cerrada");
    }

    [Fact]
    public void Close_ShouldChangeStatus_ToClosed_WhenBalanceIsZero()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Checking, "MXN");

        account.Close();

        account.Status.Should().Be(AccountStatus.Closed);
    }

    [Fact]
    public void Credit_ShouldFail_WhenAccountIsBlocked()
    {
        var account = Account.Open(Guid.NewGuid(), AccountType.Checking, "MXN");
        account.Block();

        Action act = () => account.Credit(new Money(100, "MXN"));

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("La cuenta está bloqueada.");
    }
}
