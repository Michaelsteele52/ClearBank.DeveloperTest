using System;
using System.ComponentModel.DataAnnotations;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests;

public class MakePaymentRequestValidationTests
{
    [Fact]
    public void GivenMakePaymentRequest_WhenTheAmountIsLessThanZero_ThenFailValidation()
    {
        var request = new MakePaymentRequest()
        {
            DebtorAccountNumber = Guid.NewGuid().ToString()
        };
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(request, new ValidationContext(request), true)).ValidationResult.ErrorMessage.Should().Be("The field Amount must be between 5E-324 and 1.7976931348623157E+308.");
    }
    
    [Fact]
    public void GivenMakePaymentRequest_WhenDebtorAccountNumberIsNull_ThenFailValidation()
    {
        var request = new MakePaymentRequest();
        Assert.Throws<ValidationException>(() => Validator.ValidateObject(request, new ValidationContext(request), true)).ValidationResult.ErrorMessage.Should().Be("The DebtorAccountNumber field is required.");
    }
}