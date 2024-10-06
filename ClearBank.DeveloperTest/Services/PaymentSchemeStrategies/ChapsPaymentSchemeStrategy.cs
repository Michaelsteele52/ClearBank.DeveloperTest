using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentSchemeStrategies;

public class ChapsPaymentSchemeStrategy : IPaymentSchemeStrategy
{
    public MakePaymentResult AuthenticatePayment(Account account, decimal requestedAmount)
    {
        return account.Status == AccountStatus.Live ? new MakePaymentResult { Success = true } : new MakePaymentResult { Success = false };
    }
    public bool IsRequestedSchemeSupportedByTheAccount(PaymentScheme paymentScheme, Account account) => paymentScheme == PaymentScheme.Chaps && account.AllowedPaymentSchemes == AllowedPaymentSchemes.Chaps;
}