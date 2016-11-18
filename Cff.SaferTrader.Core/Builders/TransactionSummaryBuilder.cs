namespace Cff.SaferTrader.Core.Builders
{
    public class TransactionSummaryBuilder
    {
        private readonly CleverReader reader;

        public TransactionSummaryBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public TransactionSummary Build()
        {
            decimal invoices = 0, receipts = 0, credits = 0, discounts = 0;

            while (reader.Read())
            {
                TransactionType type = TransactionType.Parse(reader.ToSmallInteger("TransTypeID"));
                decimal amount = reader.ToDecimal("Amount");

                if (type == TransactionType.Invoice)
                {
                    invoices = amount;
                }
                else if (type == TransactionType.Receipt)
                {
                    receipts = amount;
                }
                else if (type == TransactionType.Credit)
                {
                    credits = amount;
                }
                else if (type == TransactionType.Discount)
                {
                    discounts = amount;
                }
            }

            return new TransactionSummary(invoices, receipts, credits, discounts);
        }
    }
}