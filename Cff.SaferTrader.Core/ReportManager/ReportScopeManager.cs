using System;

namespace Cff.SaferTrader.Core.ReportManager
{
    public class ReportScopeManager
    {
        private readonly Scope scope;

        public ReportScopeManager(Scope scope)
        {
            this.scope = scope;
        }

        public bool IsControlReportAvailable()
        {
            return scope != Scope.CustomerScope;
        }

        public bool IsAgedBalancesReportAvailable()
        {
            return scope != Scope.CustomerScope;
        }

        public bool IsPromptReportAvailable()
        {
            return scope != Scope.CustomerScope;
        }

        public bool IsUnclaimedCreditNotesReportAvailable()
        {
            return scope != Scope.CustomerScope;
        }

        public bool IsUnclaimedRepurchasesReportAvailable()
        {
            return scope != Scope.CustomerScope;
        }
        public bool IsRetentionReleaseEstimateReportAvailable()
        {
            if(scope == Scope.ClientScope)
            {
                return true;
            }
            return false;
        }
        public bool IsCreditLimitExceededReportAvailable()
        {
            return scope != Scope.CustomerScope;
        }

        public bool IsCreditStopSuggestionsReportAvailable()
        {
            //return scope != Scope.CustomerScope;
            if (scope == Scope.ClientScope)
            {
                return true;
            }
            return false;

        }

        public bool IsCallsDueReportAvailable()
        {
            return scope != Scope.CustomerScope;
        }
        public bool IsClientActionReportAvailable()
        {
            return scope != Scope.CustomerScope;
        }
        public bool IsCustomerWatchReportAvailable()
        {
            if (scope == Scope.AllClientsScope)
            {
                return true;
            }
            return false;
        }

        public bool IsRepurchasesReportAvailable()
        {
            return scope != Scope.CustomerScope;
        }

        public bool IsCurrentShortPaidReportAvailable()
        {
            return scope != Scope.CustomerScope;
        }

        public bool IsCurrentOverpaidReportAvailable()
        {
            return scope != Scope.CustomerScope;
        }

        public bool IsUnallocatedReportAvailable()
        {
            return scope != Scope.CustomerScope;
        }

        public bool IsStatementsReportAvailable()
        {
            return scope == Scope.CustomerScope;
        }

        public bool IsAccountTransReportAvailable()
        {
            return scope == Scope.ClientScope;
        }

        


    }
}