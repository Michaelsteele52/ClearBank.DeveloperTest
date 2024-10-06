using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentSchemeStrategies;

public class FasterPaymentsSchemeStrategy : IPaymentSchemeStrategy
{
    public MakePaymentResult AuthenticatePayment(Account account, decimal requestedAmount)
    {
        return account.Balance < requestedAmount ? new MakePaymentResult { Success = false } : new MakePaymentResult { Success = true };
    }

    public bool IsRequestedSchemeSupportedByTheAccount(PaymentScheme paymentScheme, Account account) => paymentScheme == PaymentScheme.FasterPayments && account.AllowedPaymentSchemes == AllowedPaymentSchemes.FasterPayments;
}