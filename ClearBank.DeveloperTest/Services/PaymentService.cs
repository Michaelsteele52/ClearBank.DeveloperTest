using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Services.PaymentSchemeStrategies.StrategyResolver;
using ClearBank.DeveloperTest.Validation;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService(
        IDataStoreFactory dataStoreFactory,
        IPaymentSchemeStrategyResolver paymentSchemeStrategyResolver,
        IMakePaymentRequestValidator paymentRequestValidator)
        : IPaymentService
    {   
        private readonly IAccountDataStore _accountDataStore = dataStoreFactory.GetDataStore();

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            if (!paymentRequestValidator.IsValid(request)) return new MakePaymentResult{ Success = false };
            
            var account = _accountDataStore.GetAccount(request.DebtorAccountNumber);
            // Would prefer to respond with something for useful to the user, depending on the interface, maybe a problem details exception highlighting the account was not found.
            if (account == null) return new MakePaymentResult { Success = false };
            
            var strategy = paymentSchemeStrategyResolver.GetPaymentSchemeStrategy(request.PaymentScheme, account);
            // Would prefer to respond with something more useful to the user. Used nullable to remove the need for a default strategy and associated ordering.
            if (strategy == null) return new MakePaymentResult { Success = false };
            
            var result = strategy.AuthenticatePayment(account, request.Amount);
            
            if (!result.Success) return result;
            UpdateBalance(account, request.Amount);

            return result;
        }
        
        // There should be some error handling here, maybe that is handled by the repository and picked up by middleware etc
        // Typically use cancellation tokens if appropriate
        private void UpdateBalance(Account debtorAccount, decimal requestedAmount)
        {
            // Potentially want some validation, but is currently validated on the request class.
            debtorAccount.ReduceBalance(requestedAmount);
            _accountDataStore.UpdateAccount(debtorAccount);
        }
    }
}
