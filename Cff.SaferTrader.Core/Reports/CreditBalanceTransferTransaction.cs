namespace Cff.SaferTrader.Core.Reports
{
    public class CreditBalanceTransferTransaction : ISubledgerTransaction
    {
        private readonly decimal amount;

        public CreditBalanceTransferTransaction(decimal amount)
        {
            this.amount = amount;
        }

        public void AddTo(Subledger subledger)
        {
            subledger.AddCreditBalanceTransfer(amount);
        }
    }
}
