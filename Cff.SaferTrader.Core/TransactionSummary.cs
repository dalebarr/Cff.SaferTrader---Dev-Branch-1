namespace Cff.SaferTrader.Core
{
    public class TransactionSummary
    {
        private readonly decimal invoices;
        private readonly decimal receipts;
        private readonly decimal credits;
        private readonly decimal discounts;

        public TransactionSummary(decimal invoices, decimal receipts, decimal credits, decimal discounts)
        {
            this.invoices = invoices;
            this.receipts = receipts;
            this.credits = credits;
            this.discounts = discounts;
        }

        public decimal Invoices
        {
            get { return invoices; }
        }

        public decimal Receipts
        {
            get { return receipts; }
        }

        public decimal Credits
        {
            get { return credits; }
        }

        public decimal Discounts
        {
            get { return discounts; }
        }
    }
}