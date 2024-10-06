using System;
using System.Collections.Generic;
using AutoFixture;
using ClearBank.DeveloperTest.Services.PaymentSchemeStrategies;
using ClearBank.DeveloperTest.Services.PaymentSchemeStrategies.StrategyResolver;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.PaymentStrategiesTests;

public class PaymentSchemeResolverTests
{
    private readonly IPaymentSchemeStrategyResolver _sut;
    private readonly Fixture _fixture = new();
    private readonly Mock<IPaymentSchemeStrategy> _mockStrategy = new();

    public PaymentSchemeResolverTests()
    {
        var strategies = new List<IPaymentSchemeStrategy> { _mockStrategy.Object };
        _sut = new PaymentSchemeResolver(strategies);
    }

    [Fact]
    public void GivenStrategyIsAllowed_WhenResolverIsUsed_ThenReturnStrategy()
    {
        // Arrange
        var account = _fixture.Create<Account>();
        var paymentScheme = PaymentScheme.FasterPayments;
        _mockStrategy.Setup(x => x.IsRequestedSchemeSupportedByTheAccount(paymentScheme, account))
            .Returns(true);
        // Act
        var strategy = _sut.GetPaymentSchemeStrategy(paymentScheme, account);
        // Assert
        strategy.Should().NotBeNull();
    }

    [Fact]
    public void GivenNoStrategyIsAllowed_WhenResolveIsUsed_ThenItReturnsNull()
    {
        // Arrange
        var account = _fixture.Build<Account>().With(x => x.AllowedPaymentSchemes, AllowedPaymentSchemes.FasterPayments).Create();
        var paymentScheme = PaymentScheme.FasterPayments;
        _mockStrategy.Setup(x => x.IsRequestedSchemeSupportedByTheAccount(paymentScheme, account))
            .Returns(false);
        // Act
        var strategy = _sut.GetPaymentSchemeStrategy(paymentScheme, account);
        
        // Assert
        strategy.Should().BeNull();
    }
}