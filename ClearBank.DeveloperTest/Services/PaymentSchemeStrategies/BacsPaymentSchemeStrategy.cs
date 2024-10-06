using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentSchemeStrategies;

public class BacsPaymentSchemeStrategy : IPaymentSchemeStrategy
{
    public MakePaymentResult AuthenticatePayment(Account account, decimal requestedAmount) => new() { Success = true };
    public bool IsRequestedSchemeSupportedByTheAccount(PaymentScheme paymentScheme, Account account) => paymentScheme == PaymentScheme.Bacs && account.AllowedPaymentSchemes == AllowedPaymentSchemes.Bacs;
}