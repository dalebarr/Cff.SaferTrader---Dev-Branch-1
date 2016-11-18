using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class ReleaseSummary
    {
        private readonly decimal fundedTransactionRelease;
        private readonly decimal nonFundedTransactionRelease;

        public ReleaseSummary(decimal fundedTransactionRelease, decimal nonFundedTransactionRelease)
        {
            this.fundedTransactionRelease = fundedTransactionRelease;
            this.nonFundedTransactionRelease = nonFundedTransactionRelease;
        }

        public decimal Total
        {
            get { return fundedTransactionRelease + nonFundedTransactionRelease; }
        }

        public decimal FundedTransactionRelease
        {
            get { return fundedTransactionRelease; }
        }

        public decimal NonFundedTransactionRelease
        {
            get { return nonFundedTransactionRelease; }
        }
    }
}