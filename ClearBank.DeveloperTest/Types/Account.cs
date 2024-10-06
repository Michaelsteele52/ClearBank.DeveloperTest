namespace ClearBank.DeveloperTest.Types
{
    public class Account
    {
        private decimal _balance;
        public decimal Balance { get => _balance;
            init => _balance = value;
        }
        public string AccountNumber { get; init; }
        public AccountStatus Status { get; init; }
        public AllowedPaymentSchemes AllowedPaymentSchemes { get; init; }

        public void ReduceBalance(decimal requestedAmount)
        {
            // Encapsulating Balance change
            _balance -= requestedAmount;
        }
    }
}
