using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validation;

public interface IMakePaymentRequestValidator
{
    bool IsValid(MakePaymentRequest request);
}