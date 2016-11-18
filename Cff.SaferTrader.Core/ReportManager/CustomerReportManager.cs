using System;

namespace Cff.SaferTrader.Core.ReportManager
{
    public class CustomerReportManager : IReportManager
    {
        public bool CanViewControlReport()
        {
            return false;
        }
        public bool CanViewAgedBalances()
        {
            return false;
        }

        public bool CanViewOverdueChargesReport()
        {
            return false;
        }
        public bool CanViewPromptReport()
        {
            return false;
        }
        public bool CanViewStatusReport()
        {
            return false;
        }
        public bool CanViewUnclaimedCreditNotesReport()
        {
            return false;
        }
        public bool CanViewUnclaimedRepurchasesReport()
        {
            return false;
        }
        public bool CanViewRetentionReleaseEstimateReport()
        {
            return false;
        }
        public bool CanViewCreditLimitExceededReport()
        {
            return false;
        }
        public bool CanViewCreditStopSuggestionsReport()
        {
            return false;
        }

        public bool CanViewCallsDueReport()
        {
            return false;
        }
        public bool CanViewClientActionReport()
        {
            return false;
        }
        public bool CanViewCustomerWatchReport()
        {
            return false;
        }
        public bool CanViewCreditNotesReport()
        {
            return true;
        }
        public bool CanViewJournalsReport()
        {
            return true;
        }
        public bool CanViewCreditBalanceTransfersReport()
        {
            return true;
        }
        public bool CanViewInvoicesReport()
        {
            return true;
        }
        public bool CanViewReceiptsReport()
        {
            return true;
        }
        public bool CanViewDiscountsReport()
        {
            return true;
        }
        public bool CanViewRepurchaserasReport()
        {
            return false;
        }
        public bool CanViewCurrentShortPaidReport()
        {
            return false;
        }
        public bool CanViewCurrentOverpaidReport()
        {
            return false;
        }
        public bool CanViewUnallocatedReport()
        {
            return false;
        }
        public bool CanViewOverpaymentsReport()
        {
            return false;
        }
        public bool CanViewStatementReport()
        {
            return true;
        }

        public bool CanViewAccountTransReport()
        { 
            return false;
        }
    }
}