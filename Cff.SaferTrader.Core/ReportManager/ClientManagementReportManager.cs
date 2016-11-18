using System;

namespace Cff.SaferTrader.Core.ReportManager
{
    public class ClientManagementReportManager : IReportManager
    {
        public bool CanViewControlReport()
        {
            return true;
        }
        public bool CanViewAgedBalances()
        {
            return true;
        }

        public bool CanViewOverdueChargesReport()
        {
            return true;
        }

        public bool CanViewPromptReport()
        {
            return true;
        }
        public bool CanViewStatusReport()
        {
            return true;
        }
        public bool CanViewUnclaimedCreditNotesReport()
        {
            return true;
        }
        public bool CanViewUnclaimedRepurchasesReport()
        {
            return true;
        }
        public bool CanViewRetentionReleaseEstimateReport()
        {
            return true;
        }
        public bool CanViewCreditLimitExceededReport()
        {
            return true;
        }

        public bool CanViewCreditStopSuggestionsReport()
        {
            return true;
        }


        public bool CanViewCallsDueReport()
        {
            return true;
        }
        public bool CanViewClientActionReport()
        {
            return true;
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
            return true;
        }
        public bool CanViewCurrentShortPaidReport()
        {
            return true;
        }
        public bool CanViewCurrentOverpaidReport()
        {
            return true;
        }
        public bool CanViewUnallocatedReport()
        {
            return true;
        }
        public bool CanViewOverpaymentsReport()
        {
            return true;
        }
        public bool CanViewStatementReport()
        {
            return true;
        }

        public bool CanViewAccountTransReport()
        {
            return true;
        }
    }
}