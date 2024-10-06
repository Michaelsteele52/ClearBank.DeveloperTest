using AutoFixture;
using ClearBank.DeveloperTest.Services.PaymentSchemeStrategies;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.PaymentStrategiesTests;

public class ChapsPaymentSchemeStrategyTests
{
    private readonly ChapsPaymentSchemeStrategy _sut = new();
    private readonly Fixture _fixture = new();

    [Fact]
    public void GivenTheStrategyIsUsed_WithAnyAmount_ThenShouldSuccessfullyAuthenticatePayment()
    {
        var account = _fixture.Create<Account>();
        var result = _sut.AuthenticatePayment(account, 0);
        result.Success.Should().BeTrue();
    }

    [Theory]
    [InlineData(PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps, true)]
    [InlineData(PaymentScheme.Chaps, AllowedPaymentSchemes.FasterPayments, false)]
    [InlineData(PaymentScheme.FasterPayments, AllowedPaymentSchemes.Chaps, false)]
    [InlineData(PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments, false)]
    public void GivenTheAccountAllowedPaymentSchemesIsBacs_WhenAllowedIsCalled_ThenTheResultIsTrue(PaymentScheme paymentScheme, AllowedPaymentSchemes accountAllowedPaymentScheme, bool expectedResult)
    {
        var account = _fixture.Build<Account>().With(x => x.AllowedPaymentSchemes, accountAllowedPaymentScheme)
            .Create();
        _sut.IsRequestedSchemeSupportedByTheAccount(paymentScheme, account).Should().Be(expectedResult);
    }
}