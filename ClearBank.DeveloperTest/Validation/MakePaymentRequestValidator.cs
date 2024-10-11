using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation;

public static class MakePaymentRequestValidator
{
    public static bool IsValid(MakePaymentRequest request)
    {
        // Very basic validation. Would validate with annotations or similar depending how the request was sent. 
        // Would like to respond with more meaningful error message to the client.
        if(request.Amount < 0m) return false;
        return !string.IsNullOrWhiteSpace(request.DebtorAccountNumber);
    }
}