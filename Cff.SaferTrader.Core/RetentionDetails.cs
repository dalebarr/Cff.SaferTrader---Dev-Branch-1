using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class RetentionDetails
    {
        private readonly ChargeCollection charges;
        private readonly RetentionDeductable retentionDeductable;
        private readonly Date endOfMonth;
        private readonly decimal factored;
        private readonly decimal nonFactored;
        private readonly decimal nonFactoredReceipts;
        private readonly OverdueFee overdueFee;
        private readonly string status;
        private readonly IDate releasedDate;
        private readonly RetentionInfo retentionInfo;
        private readonly int retentionId;
        private readonly RetentionSummary retentionSummary;
        private readonly TransactionsAfterEndOfMonth transactionsAfterEndOfMonth;
        private readonly Int16 _hold;
        private readonly int _clientFacilityType;

        public RetentionDetails(int retentionId, IDate releasedDate, Date endOfMonth, decimal nonFactored, decimal factored, RetentionInfo retentionInfo, RetentionDeductable retentionDeductable, decimal nonFactoredReceipts, TransactionsAfterEndOfMonth transactionsAfterEndOfMonth, RetentionSummary retentionSummary, ChargeCollection charges, OverdueFee overdueFee, string status, Int16 hold, int clientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(releasedDate, "releasedDate");
            ArgumentChecker.ThrowIfNull(endOfMonth, "endOfMonth");
            ArgumentChecker.ThrowIfNull(retentionInfo, "retentionInfo");
            ArgumentChecker.ThrowIfNull(retentionDeductable, "deductable");
            ArgumentChecker.ThrowIfNull(transactionsAfterEndOfMonth, "transactionsAfterEndOfMonth");
            ArgumentChecker.ThrowIfNull(retentionSummary, "retentionSummary");
            ArgumentChecker.ThrowIfNull(charges, "charges"); 
            ArgumentChecker.ThrowIfNull(overdueFee, "overdueFee");
            ArgumentChecker.ThrowIfNullOrEmpty(status, "status");
            
            this.retentionId = retentionId;
            this.releasedDate = releasedDate;
            this.endOfMonth = endOfMonth;
            this.nonFactored = nonFactored;
            this.factored = factored;
            this.retentionInfo = retentionInfo;
            this.retentionDeductable = retentionDeductable;
            this.nonFactoredReceipts = nonFactoredReceipts;
            this.transactionsAfterEndOfMonth = transactionsAfterEndOfMonth;
            this.retentionSummary = retentionSummary;
            this.charges = charges;
            this.overdueFee = overdueFee;
            this.status = status;
            this._hold = hold;
            this._clientFacilityType = clientFacilityType;
        }

        public bool IsHeld
        {
            get { return status.Equals("Held", StringComparison.OrdinalIgnoreCase); }
        }

        public decimal CalculateRetentionReleasePriorToEndOfMonth()
        {
            return CalculateSurplusLessDeductables() + nonFactoredReceipts;
        }

        public decimal CalculateSurplusLessDeductables()
        {
            return retentionInfo.Surplus - retentionDeductable.CalculateTotal();
        }

        public decimal CalculateEstimatedRetentionRelease()
        {
            return CalculateRetentionReleasePriorToEndOfMonth()
                   - transactionsAfterEndOfMonth.Balance
                   - Adjustments;
        }

        public decimal CalculateSumChargedViaCA()
        {
            return retentionDeductable.CAChargesTotal()
                   + Adjustments;
        }
        
        public decimal CalculateTotalLedger()
        {
            return factored + nonFactored;
        }

        public decimal CalculateGst()
        {
            return retentionDeductable.CalculateGst() + overdueFee.CalculateGstOnAdminFee();
        }

        public decimal Adjustments
        {
            get { return charges.CalculateTotal(); }
        }

        public RetentionSummary RetentionSummary
        {
            get { return retentionSummary; }
        }

        public TransactionsAfterEndOfMonth TransactionsAfterEndOfMonth
        {
            get { return transactionsAfterEndOfMonth; }
        }

        public decimal NonFactoredReceipts
        {
            get { return nonFactoredReceipts; }
        }

        public int RetentionId
        {
            get { return retentionId; }
        }

        public IDate ReleasedDate
        {
            get { return releasedDate; }
        }

        public Date EndOfMonth
        {
            get { return endOfMonth; }
        }

        public decimal NonFactored
        {
            get { return nonFactored; }
        }

        public decimal Factored
        {
            get { return factored; }
        }

        public RetentionInfo RetentionInfo
        {
            get { return retentionInfo; }
        }

        public RetentionDeductable RetentionDeductable
        {
            get { return retentionDeductable; }
        }

        public Int16 Hold
        {
            get { return _hold; }
        }

        public int ClientFacilityType
        {
            get { return _clientFacilityType; }
        }

        


    }
}