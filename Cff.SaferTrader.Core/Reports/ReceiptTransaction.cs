namespace Cff.SaferTrader.Core.Reports
{
    public class ReceiptTransaction : ISubledgerTransaction
    {
        private readonly decimal amount;

        public ReceiptTransaction(decimal amount)
        {
            this.amount = amount;
        }

        public void AddTo(Subledger subledger)
        {
            subledger.AddReceipt(amount);
        }
    }
}