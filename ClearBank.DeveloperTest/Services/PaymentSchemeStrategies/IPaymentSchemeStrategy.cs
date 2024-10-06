using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentSchemeStrategies;

public interface IPaymentSchemeStrategy
{
    MakePaymentResult AuthenticatePayment(Account account, decimal requestedAmount);
    bool IsRequestedSchemeSupportedByTheAccount(PaymentScheme paymentScheme, Account account);
}