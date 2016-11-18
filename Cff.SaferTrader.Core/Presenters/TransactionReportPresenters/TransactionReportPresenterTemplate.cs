namespace Cff.SaferTrader.Core.Presenters.TransactionReportPresenters
{
    public abstract class TransactionReportPresenterTemplate
    {
        public void ShowReport(Scope scope, bool firstViewOrScopeChangeOnPage)
        {
            if (scope == Scope.AllClientsScope)
            {
                ShowAllClientsReport(firstViewOrScopeChangeOnPage);
            }
            else if (scope == Scope.ClientScope)
            {
                ShowClientReport();
            }
            else if (scope == Scope.CustomerScope)
            {
                ShowCustomerReport();
            }
        }
        public void ConfigureView(Scope scope)
        {
            if (scope == Scope.AllClientsScope)
            {
                ShowAllClientsView();
            }
            else if (scope == Scope.ClientScope)
            {
                ShowClientView();
            }
            else if (scope == Scope.CustomerScope)
            {
                ShowCustomerView();
            }
        }

        protected abstract void ShowAllClientsView();
        protected abstract void ShowClientView();
        protected abstract void ShowCustomerView();
        protected abstract void ShowAllClientsReport(bool firstViewOrScopeChangeOnPage);
        protected abstract void ShowClientReport();
        protected abstract void ShowCustomerReport();
    }
}