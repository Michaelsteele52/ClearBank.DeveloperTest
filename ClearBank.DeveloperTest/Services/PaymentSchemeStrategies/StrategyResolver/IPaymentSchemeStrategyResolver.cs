using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentSchemeStrategies.StrategyResolver;

public interface IPaymentSchemeStrategyResolver
{
    // Decided on using a nullable return as opposed to having a default strategy, as it removed the need for strategy ordering
    IPaymentSchemeStrategy? GetPaymentSchemeStrategy(PaymentScheme paymentScheme, Account account);
}