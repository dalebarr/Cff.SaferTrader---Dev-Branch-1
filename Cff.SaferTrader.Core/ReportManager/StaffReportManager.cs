using System;

namespace Cff.SaferTrader.Core.ReportManager
{
    public class StaffReportManager : IReportManager
    {
        private readonly Scope scope;

        public StaffReportManager(Scope scope)
        {
            this.scope = scope;
        }

        public bool CanViewControlReport()
        {
            if(scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }

        public bool CanViewAgedBalances()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }

        public bool CanViewOverdueChargesReport()
        {
            return scope != Scope.AllClientsScope;
        }

        public bool CanViewPromptReport()
        {
            return true;
        }

        public bool CanViewStatusReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }

        public bool CanViewUnclaimedCreditNotesReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }

        public bool CanViewUnclaimedRepurchasesReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }

        public bool CanViewRetentionReleaseEstimateReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
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
            return true;
        }
        public bool CanViewCreditNotesReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }
        public bool CanViewJournalsReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }
        public bool CanViewCreditBalanceTransfersReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }
        public bool CanViewInvoicesReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }
        public bool CanViewReceiptsReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }
        public bool CanViewDiscountsReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }
        public bool CanViewRepurchaserasReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }
        public bool CanViewCurrentShortPaidReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }
        public bool CanViewCurrentOverpaidReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }
        public bool CanViewUnallocatedReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }
        public bool CanViewOverpaymentsReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }
        public bool CanViewStatementReport()
        {
            if (scope == Scope.AllClientsScope)
            {
                return false;
            }
            return true;
        }

        public bool CanViewAccountTransReport()
        {
            if (scope == Scope.CustomerScope)
            {
                return false;
            }
            return true;
        }
    }
}