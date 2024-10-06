using System;
using System.Collections.Generic;
using System.Diagnostics;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.PaymentSchemeStrategies.StrategyResolver;

public class PaymentSchemeResolver(IEnumerable<IPaymentSchemeStrategy> strategies) : IPaymentSchemeStrategyResolver
{
    public IPaymentSchemeStrategy? GetPaymentSchemeStrategy(PaymentScheme paymentScheme, Account account)
    {
        foreach (var strategy in strategies)
        {
            if(!strategy.IsRequestedSchemeSupportedByTheAccount(paymentScheme, account)) continue;
            Activity.Current?.SetTag("Payment.Strategy", strategy.GetType().Name);
            return strategy;
        }
        
        // Decided on using a nullable return as opposed to having a default strategy, as it removed the need for strategy ordering
        return null;
    }
}