using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class ControlReport : ReportBase
    {
        private readonly decimal creditsToBeClaimed;
        private readonly DebtorsLedger debtorsLedger;
        private readonly FactorsLedger factorsLedger;
        private readonly decimal fundedInvoicesBalance;
        private readonly decimal fundedToBeRepurchased;
        private readonly decimal nonFundedInvoicesBalance;
        private readonly decimal unallocatedTransactions;
        private readonly decimal repurchasesThisMonth;
        private readonly decimal allocatedthisperiod;
        private readonly decimal cbtsinperiod;
        private readonly string title;
        private readonly int facilityType;

        public ControlReport(ICalendar calendar, string title, string clientName, DebtorsLedger debtorsLedger, 
                                FactorsLedger factorsLedger, decimal fundedInvoicesBalance, decimal nonFundedInvoicesBalance, 
                                    decimal fundedToBeRepurchased, decimal creditsToBeClaimed, decimal unallocatedTransactions, Date endDate,
                                        decimal allocatedThisPeriod, decimal cbtsInPeriod, int facilityType)  
            : base(calendar, "Control Report", clientName)
        {
            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            ArgumentChecker.ThrowIfNull(debtorsLedger, "debtorsLedger");
            ArgumentChecker.ThrowIfNull(factorsLedger, "factorsLedger");
            ArgumentChecker.ThrowIfNullOrEmpty(title, "title");

            this.debtorsLedger = debtorsLedger;
            this.title = title;
            this.factorsLedger = factorsLedger;
            this.fundedInvoicesBalance = fundedInvoicesBalance;
            this.nonFundedInvoicesBalance = nonFundedInvoicesBalance;
            this.fundedToBeRepurchased = fundedToBeRepurchased;
            this.creditsToBeClaimed = creditsToBeClaimed;
            this.unallocatedTransactions = unallocatedTransactions;
            this.allocatedthisperiod = allocatedThisPeriod;
            this.cbtsinperiod = cbtsInPeriod;
            this.facilityType = facilityType;

            repurchasesThisMonth = factorsLedger.CurrentLedger.Repurchase;
        }

        public DebtorsLedger DebtorsLedger
        {
            get { return debtorsLedger; }
        }

        public FactorsLedger FactorsLedger
        {
            get { return factorsLedger; }
        }

        public decimal FundedInvoicesBalance
        {
            get { return fundedInvoicesBalance; }
        }

        public decimal NonFundedInvoicesBalance
        {
            get { return nonFundedInvoicesBalance; }
        }

        public decimal FundedToBeRepurchased
        {
            get { return fundedToBeRepurchased; }
        }

        public decimal CreditsToBeClaimed
        {
            get { return creditsToBeClaimed; }
        }

        public decimal UnallocatedTransactions
        {
            get { return unallocatedTransactions; }
        }

        public decimal RepurchasesThisMonth
        {
            get { return repurchasesThisMonth; }
        }

        public decimal AllocatedThisPeriod
        {
            get { return allocatedthisperiod; }
        }

        public decimal CbtsInPeriod
        {
            get { return cbtsinperiod; }
        }
        public string Title
        {
            get {
                return title;
            }
        }
        public int FacilityType
        {
            get
            {
                return facilityType;
            }
        }




    }
}