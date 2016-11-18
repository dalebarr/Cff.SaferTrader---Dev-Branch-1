using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class RetentionSummary
    {
        private readonly int factorDays;
        private readonly decimal invoicesPurchased;
        private readonly decimal openingBalance;
        private readonly decimal nonFactored;
        private readonly decimal factored;

        public RetentionSummary(int factorDays, decimal openingBalance, decimal invoicesPurchased, decimal nonFactored, decimal factored)
        {
            this.factorDays = factorDays;
            this.openingBalance = openingBalance;
            this.invoicesPurchased = invoicesPurchased;
            this.nonFactored = nonFactored;
            this.factored = factored;
        }

        public decimal CalculateClosingBalance()
        {
            return nonFactored + factored;
        }

        public decimal CalculateCreditTransactions()
        {
            return openingBalance + invoicesPurchased - CalculateClosingBalance();
        }

        public int FactorDays
        {
            get { return factorDays; }
        }

        public decimal OpeningBalance
        {
            get { return openingBalance; }
        }

        public decimal InvoicesPurchased
        {
            get { return invoicesPurchased; }
        }
    }
}