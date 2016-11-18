using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class HistoricalTransaction
    {
        private readonly decimal credit;
        private readonly string yrMonth;
        private readonly decimal discount;
        private readonly decimal factored;
        private readonly decimal journal;
        private readonly decimal nonFactored;
        private readonly decimal overpayment;
        private readonly decimal receipt;
        private readonly decimal repurchase;

        public HistoricalTransaction(String yrMonth, decimal factored, decimal nonFactored, decimal credit, decimal receipt, decimal journal, decimal discount, decimal repurchase, decimal overpayment)
        {
            this.yrMonth = yrMonth;
            this.factored = factored;
            this.nonFactored = nonFactored;
            this.credit = credit;
            this.receipt = receipt;
            this.journal = journal;
            this.discount = discount;
            this.repurchase = repurchase;
            this.overpayment = overpayment;
        }

        public String YrMonth
        {
            get { return yrMonth; }
        }

        public decimal Factored
        {
            get { return factored; }
        }

        public decimal NonFactored
        {
            get { return nonFactored; }
        }

        public decimal Credit
        {
            get { return credit; }
        }

        public decimal Receipt
        {
            get { return receipt; }
        }

        public decimal Journal
        {
            get { return journal; }
        }

        public decimal Discount
        {
            get { return discount; }
        }

        public decimal Repurchase
        {
            get { return repurchase; }
        }

        public decimal Overpayment
        {
            get { return overpayment; }
        }
    }
}