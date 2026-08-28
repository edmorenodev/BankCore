using BankCore.Domain.Shared;
using FluentAssertions;

namespace BankCore.UnitTests.Domain;

public class MoneyTests
{
    [Fact]
    public void Add_ShouldReturnCorrectSum()
    {
        var a = new Money(300, "MXN");
        var b = new Money(200, "MXN");

        var result = a.Add(b);

        result.Should().Be(new Money(500, "MXN"));
    }

    [Fact]
    public void Subtract_ShouldReturnCorrectDifference()
    {
        var a = new Money(1000, "MXN");
        var b = new Money(400, "MXN");

        var result = a.Subtract(b);

        result.Should().Be(new Money(600, "MXN"));
    }

    [Fact]
    public void Add_ShouldFail_WhenCurrenciesDiffer()
    {
        var mxn = new Money(100, "MXN");
        var usd = new Money(100, "USD");

        Action act = () => mxn.Add(usd);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Subtract_ShouldFail_WhenCurrenciesDiffer()
    {
        var mxn = new Money(500, "MXN");
        var usd = new Money(100, "USD");

        Action act = () => mxn.Subtract(usd);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Constructor_ShouldFail_WhenAmountIsNegative()
    {
        Action act = () => new Money(-1, "MXN");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Zero_ShouldReturnMoneyWithZeroAmount()
    {
        var zero = Money.Zero("MXN");

        zero.Amount.Should().Be(0);
        zero.Currency.Should().Be("MXN");
    }

    [Fact]
    public void IsGreaterThan_ShouldReturnTrue_WhenAmountIsLarger()
    {
        var big = new Money(500, "MXN");
        var small = new Money(100, "MXN");

        big.IsGreaterThan(small).Should().BeTrue();
    }
}
