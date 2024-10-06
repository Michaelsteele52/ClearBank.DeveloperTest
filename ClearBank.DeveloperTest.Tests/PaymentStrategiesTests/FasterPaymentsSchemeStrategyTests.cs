using AutoFixture;
using ClearBank.DeveloperTest.Services.PaymentSchemeStrategies;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.PaymentStrategiesTests;

public class FasterPaymentsSchemeStrategyTests
{
    private readonly FasterPaymentsSchemeStrategy _sut = new();
    private readonly Fixture _fixture = new();
    
    [Theory]
    [InlineData(10, 10, true)]
    [InlineData(11, 10, true)]
    [InlineData(0, 10, false)]
    [InlineData(9.99999, 9.999999, false)]
    [InlineData(-10, 10, false)]
    public void GivenTheAccountBalance_WhereTheAmount_ThenShouldAuthenticatePayment(decimal balance, decimal requestAmount, bool expectedResult)
    {
        var account = _fixture.Build<Account>().With(x => x.Balance, balance).Create();
        var result = _sut.AuthenticatePayment(account, requestAmount);
        result.Success.Should().Be(expectedResult);
    }
    
    [Theory]
    [InlineData(PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments, true)]
    [InlineData(PaymentScheme.Chaps, AllowedPaymentSchemes.FasterPayments, false)]
    [InlineData(PaymentScheme.FasterPayments, AllowedPaymentSchemes.Chaps, false)]
    [InlineData(PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps, false)]
    public void GivenTheAccountAllowedPaymentSchemesIsBacs_WhenAllowedIsCalled_ThenTheResultIsTrue(PaymentScheme paymentScheme, AllowedPaymentSchemes accountAllowedPaymentScheme, bool expectedResult)
    {
        var account = _fixture.Build<Account>().With(x => x.AllowedPaymentSchemes, accountAllowedPaymentScheme)
            .Create();
        _sut.IsRequestedSchemeSupportedByTheAccount(paymentScheme, account).Should().Be(expectedResult);
    }
}