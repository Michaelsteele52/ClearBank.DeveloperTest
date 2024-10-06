using System;
using System.ComponentModel.DataAnnotations;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation;

public class MakePaymentRequestValidator : IMakePaymentRequestValidator
{
    public bool IsValid(MakePaymentRequest request)
    {
        try
        {
            Validator.ValidateObject(request, new ValidationContext(request), true);
            return true;
        }
        // the validation handling here is inadequate. It would be preferred to respond with a meaningful message.
        catch (ValidationException e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}