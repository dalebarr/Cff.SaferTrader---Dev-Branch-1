namespace Cff.SaferTrader.Core.Reports
{
    public class InvoiceTransaction : ISubledgerTransaction
    {
        private readonly decimal amount;
        private readonly TransactionStatus status;

        public InvoiceTransaction(TransactionStatus status, decimal amount)
        {
            this.status = status;
            this.amount = amount;
        }

        #region ISubledgerTransaction Members

        public void AddTo(Subledger subledger)
        {
            if (status == TransactionStatus.Funded || status == TransactionStatus.Marked)
            {
                subledger.AddFundedInvoice(amount);
            }
            else if (status == TransactionStatus.NonFunded || status == TransactionStatus.Claimed)
            {
                subledger.AddNonFundedInvoice(amount);
            }
        }

        #endregion
    }
}