using System.Collections.Generic;
using AutoFixture;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Services.PaymentSchemeStrategies;
using ClearBank.DeveloperTest.Services.PaymentSchemeStrategies.StrategyResolver;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests;

public class PaymentServiceE2ETests
{
    private readonly Mock<IAccountDataStore> _accountDataStore = new();
    private readonly Mock<IDataStoreFactory> _dataStoreFactory = new();
    private readonly Fixture _fixture = new();
    private readonly IPaymentService _sut;
    
    public PaymentServiceE2ETests()
    {
        _dataStoreFactory.Setup(x => x.GetDataStore()).Returns(_accountDataStore.Object);
        _sut = new PaymentService(_dataStoreFactory.Object, 
            new PaymentSchemeResolver(new List<IPaymentSchemeStrategy>
            {
                new BacsPaymentSchemeStrategy(),
                new ChapsPaymentSchemeStrategy(),
                new FasterPaymentsSchemeStrategy()
            }));
    }
    
    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.FasterPayments)]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Chaps)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.Chaps)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.Bacs)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.FasterPayments)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.Bacs)]
    public void GivenAllowedPaymentScheme_DoesNotMatchPaymentScheme_ThenPaymentResultIsFalse(
        AllowedPaymentSchemes allowedPaymentScheme, PaymentScheme paymentScheme)
    {
        // Arrange
        var request = _fixture.Build<MakePaymentRequest>()
            .With(x => x.PaymentScheme, paymentScheme)
            .With(x => x.Amount, 1)
            .Create();
        var debtorAccount = _fixture.Build<Account>()
            .With(x => x.AccountNumber, request.DebtorAccountNumber)
            .With(x => x.AllowedPaymentSchemes, allowedPaymentScheme)
            .With(x => x.Balance, 10)
            .With(x => x.Status, AccountStatus.Live)
            .Create();
        _accountDataStore.Setup(x => x.GetAccount(request.DebtorAccountNumber)).Returns(debtorAccount);
        // Act
        var result = _sut.MakePayment(request);

        // Assert
        result.Success.Should().BeFalse();
        _accountDataStore.Verify(x => x.UpdateAccount(debtorAccount), Times.Never);
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps)]
    public void GivenAdequateBalance_WhenAllowedPaymentSchemeMatchesPaymentScheme_ThenApprovePayment(AllowedPaymentSchemes allowedPaymentScheme, PaymentScheme paymentScheme)
    {
        // Arrange
        var request = _fixture.Build<MakePaymentRequest>()
            .With(x => x.PaymentScheme, paymentScheme)
            .With(x => x.Amount, 1)
            .Create();
        var debtorAccount = _fixture.Build<Account>()
            .With(x => x.AccountNumber, request.DebtorAccountNumber)
            .With(x => x.AllowedPaymentSchemes, allowedPaymentScheme)
            .With(x => x.Balance, 10)
            .With(x => x.Status, AccountStatus.Live)
            .Create();
        _accountDataStore.Setup(x => x.GetAccount(request.DebtorAccountNumber)).Returns(debtorAccount);
        // Act
        var result = _sut.MakePayment(request);
        
        // Assert
        result.Success.Should().BeTrue();
        _accountDataStore.Verify(x => x.UpdateAccount(debtorAccount), Times.Once);
    }
    
    [Fact]
    public void GivenTheRequestIsNotValid_WhenAPaymentIsRequested_ThenPaymentResultIsFalse()
    {
        // Arrange
        var request = _fixture.Build<MakePaymentRequest>()
            .With(x => x.Amount, -1)
            .Create();
        // Act
        var result = _sut.MakePayment(request);
        
        // Assert
        result.Success.Should().BeFalse();
        _accountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
    }
}