namespace Cff.SaferTrader.Core.Reports
{
    public class OverpaymentTransaction : ISubledgerTransaction
    {
        private readonly decimal amount;

        public OverpaymentTransaction(decimal amount)
        {
            this.amount = amount;
        }

        public void AddTo(Subledger subledger)
        {
            subledger.AddOverpayment(amount);
        }
    }
}