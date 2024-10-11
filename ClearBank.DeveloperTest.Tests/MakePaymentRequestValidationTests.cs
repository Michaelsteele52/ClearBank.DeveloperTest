using System;
using System.ComponentModel.DataAnnotations;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validation;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests;

public class MakePaymentRequestValidationTests
{
    [Fact]
    public void GivenMakePaymentRequest_WhenTheAmountIsLessThanZero_ThenFailValidation()
    {
        var request = new MakePaymentRequest
        {
            Amount = new decimal(-0.1),
            DebtorAccountNumber = Guid.NewGuid().ToString()
        };
        var result = MakePaymentRequestValidator.IsValid(request);
        result.Should().BeFalse();
    }
    
    [Fact]
    public void GivenMakePaymentRequest_WhenDebtorAccountNumberIsNull_ThenFailValidation()
    {
        var request = new MakePaymentRequest
        {
            Amount = new decimal(1.0),
            DebtorAccountNumber = string.Empty
        };
        var result = MakePaymentRequestValidator.IsValid(request);
        result.Should().BeFalse();
    }

    [Fact]
    public void GivenMakePaymentRequest_WhenRequestIsValid_ThenReturnsTrue()
    {
        var request = new MakePaymentRequest
        {
            DebtorAccountNumber = Guid.NewGuid().ToString(),
            Amount = new decimal(1)
        };
        var result = MakePaymentRequestValidator.IsValid(request);
        result.Should().BeTrue();
    }
}