using AutoFixture;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Services.PaymentSchemeStrategies;
using ClearBank.DeveloperTest.Services.PaymentSchemeStrategies.StrategyResolver;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validation;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests;

public class PaymentServiceTests
{
    private readonly Mock<IAccountDataStore> _accountDataStore = new();
    private readonly Mock<IDataStoreFactory> _dataStoreFactory = new();
    private readonly Mock<IPaymentSchemeStrategyResolver> _paymentSchemeResolver = new();
    private readonly Fixture _fixture = new();
    private readonly IPaymentService _sut;

    public PaymentServiceTests()
    {
        _dataStoreFactory.Setup(x => x.GetDataStore()).Returns(_accountDataStore.Object);
        _sut = new PaymentService(_dataStoreFactory.Object, 
            _paymentSchemeResolver.Object);
    }

    [Fact]
    public void GivenDebtorAccountNotFound_WhenMakingPayment_ThenPaymentIsUnSuccessful()
    {
        // Arrange
        var request = _fixture.Build<MakePaymentRequest>()
            .With(x => x.Amount, 1)
            .Create();
        _accountDataStore.Setup(x => x.GetAccount(request.DebtorAccountNumber)).Returns((Account)null!);
        // Act
        var result = _sut.MakePayment(request);
        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public void GivenAPaymentSchemeStrategyIsSupported_WhenAPaymentIsRequested_ThenApprovePayment()
    {
        // Arrange
        var request = _fixture.Build<MakePaymentRequest>()
            .With(x => x.Amount, 1)
            .Create();
        var debtorAccount = _fixture.Create<Account>();
        _accountDataStore.Setup(x => x.GetAccount(request.DebtorAccountNumber)).Returns(debtorAccount);
        _paymentSchemeResolver.Setup(x => x.GetPaymentSchemeStrategy(request.PaymentScheme, debtorAccount))
            .Returns(new BacsPaymentSchemeStrategy());
        // Act
        var result = _sut.MakePayment(request);
        
        // Assert
        result.Success.Should().BeTrue();
        _accountDataStore.Verify(x => x.UpdateAccount(debtorAccount), Times.Once);
    }

    [Fact]
    public void GivenAPaymentSchemeStrategyIsNotSupported_WhenAPaymentIsRequested_ThenThePaymentIsUnsuccessful()
    {
        // Arrange
        var request = _fixture.Build<MakePaymentRequest>()
            .With(x => x.Amount, 1)
            .Create();
        var debtorAccount = _fixture.Create<Account>();
        _accountDataStore.Setup(x => x.GetAccount(request.DebtorAccountNumber)).Returns(debtorAccount);
        _paymentSchemeResolver.Setup(x => x.GetPaymentSchemeStrategy(request.PaymentScheme, debtorAccount))
            .Returns((IPaymentSchemeStrategy)null!);
        // Act
        var result = _sut.MakePayment(request);
        
        // Assert
        result.Success.Should().BeFalse();
        _accountDataStore.Verify(x => x.UpdateAccount(debtorAccount), Times.Never);
    }
}