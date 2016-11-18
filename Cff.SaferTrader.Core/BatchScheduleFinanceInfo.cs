using System;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class BatchScheduleFinanceInfo
    {
        private readonly decimal factored;
        private readonly decimal nonFactored;
        private readonly decimal adminFee;
        private readonly decimal adminFeeGST;
        private readonly decimal factorFee;
        private readonly decimal retention;
        private readonly decimal repurchase;
        private readonly decimal credit;
        private readonly decimal post;
        private readonly decimal postGST;
        private readonly decimal checkConfirm;
        private readonly decimal totalInvoice;
        private readonly ChargeCollection charges;
        private readonly int facilityType;
        private readonly decimal nonCompliantFee;
        private readonly decimal retnPercent;


        public BatchScheduleFinanceInfo(decimal factored, decimal nonFactored, decimal adminFee, decimal adminFeeGST,
                                        decimal factorFee, decimal retention, decimal repurchase, decimal credit,
                                        decimal post, decimal postGST, decimal checkConfirm, decimal totalInvoice,
                                        ChargeCollection charges,int facilityType, decimal nonCompliantFee, decimal retnPercent)
        {
            this.factored = factored;
            this.nonFactored = nonFactored;
            this.adminFee = adminFee;
            this.adminFeeGST = adminFeeGST;
            this.factorFee = factorFee;
            this.retention = retention;
            this.repurchase = repurchase;
            this.credit = credit;
            this.post = post;
            this.postGST = postGST;
            this.checkConfirm = checkConfirm;
            this.totalInvoice = totalInvoice;
            this.charges = charges;
            this.facilityType = facilityType;
            this.nonCompliantFee = nonCompliantFee;
            this.retnPercent = retnPercent;
        }

        public void Display(IScheduleTabView view)
        {
            if (checkConfirm != 0)
            {
                view.ShowCheckOrConfirmRow();
            }
            else
            {
                view.HideCheckOrConfirmRow();
            }
            view.DisplayScheduleSummary(this);
        }

        public ChargeCollection Charges
        {
            get { return charges; }
        }

        public decimal TotalInvoice
        {
            get { return totalInvoice; }
        }

        public decimal CheckConfirm
        {
            get { return checkConfirm; }
        }

        public decimal PostGst
        {
            get { return postGST; }
        }

        public decimal Post
        {
            get { return post; }
        }

        public decimal Credit
        {
            get { return credit; }
        }

        public decimal CreditFacility2()
        {
            return credit * (1- retnPercent) ;
        }
        
        public decimal Repurchase
        {
            get { return repurchase; }
        }

        public decimal RepurchaseFacility2()
        {
            return repurchase * (1 - retnPercent);
        }


        public decimal Retention
        {
            get { return retention; }
        }


        public decimal FactorFee
        {
            get { return factorFee; }
        }

        public decimal AdminFeeGst
        {
            get { return adminFeeGST; }
        }

        public decimal AdminFee
        {
            get { return adminFee; }
        }

        public decimal NonFactored
        {
            get { return nonFactored; }
        }

        public decimal Factored
        {
            get { return factored; }
        }
      
        public decimal NonCompliantFee
        {
            get { return nonCompliantFee; }
        }

        public decimal RetnPercent
        {
            get { return retnPercent; }
        }

        public int FacilityType
        {
            get { return facilityType; }
        }

        public decimal CreditResidual()
        {
            return retnPercent * credit;
        }

        public decimal RepurchResidual()
        {
            return retnPercent * repurchase;
        }

        public decimal ChargesTotal
        {
            get { return factored; }
        }

        public decimal CalculatePostageTotal()
        {
            return postGST + Post;
        }

        public decimal CalculateAdminFeeTotal()
        {
            return adminFee + adminFeeGST;
        }

        public decimal CalculateDeductions()
        {
            return CalculateAdminFeeTotal() + factorFee + retention + repurchase + credit + CalculatePostageTotal();
        }

        public decimal CalculateDeductionsFacility2()
        {
            return CalculateAdminFeeTotal() + nonCompliantFee + factorFee + RepurchaseFacility2() + CreditFacility2() + CalculatePostageTotal();
        }

        public decimal AssignmentCr()
        {
            return factored - retention;
        }
       
        public decimal CalculateTotalCharges()
        {
            return charges.CalculateTotal();
        }

        public decimal CalculateAvailableForRelease()
        {
            return factored - CalculateTotalCharges() - CalculateDeductions();
        }

        public decimal CalculateAvailableForReleaseFacility2()
        {
            return AssignmentCr() - CalculateTotalCharges() - CalculateDeductionsFacility2();
        }

        
        public decimal CalculateToCAFeesFacility2()
        {
            return CalculateAdminFeeTotal() + nonCompliantFee + factorFee  + CalculatePostageTotal();
        }

        private int ClientFacilityType
        {
            get {
                if (SessionWrapper.Instance.Get != null)
                {
                    if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
                        return SessionWrapper.Instance.Get.ClientFromQueryString.ClientFacilityType;
                }
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                {
                    if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString != null)
                        return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.ClientFacilityType;
                }

                return 0;
            }
        }


        public string strTotalInvoiceLabel
        {
            get
            {
                switch (this.ClientFacilityType)
                {
                    case 4: return "Total Debit Transactions:";
                    case 5: return "Total Debit Transactions:";
                    default: return "Total Invoices Processed:";
                }
            }
        }


        public string strNonFactoredLabel
        {
            get
            {
                switch (this.ClientFacilityType)
                {
                    case 2: return "Non Funding Invoices:";
                    case 4: return "Fees/Charges:";
                    case 5: return "Fees/Charges:";
                    default: return "Non Funded Invoices:";
                }
            }
        }

       
        public string strFactoredLabel
        {
            get
            {
                switch (this.ClientFacilityType)
                {
                    case 2: return "Funding Invoices:";
                    case 4: return "Funding Transactions:";
                    case 5: return "Funding Transactions:";
                    default: return "Invoices Funded:";
                }
            }
        }

    }
}