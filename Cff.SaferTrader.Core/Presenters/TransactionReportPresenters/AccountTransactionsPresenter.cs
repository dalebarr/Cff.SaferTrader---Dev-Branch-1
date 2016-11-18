using System;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Presenters.ReportPresenters;
using Cff.SaferTrader.Core.Views.TransactionReportView;

namespace Cff.SaferTrader.Core.Presenters.TransactionReportPresenters
{
    public class AccountTransactionsPresenter : ReportPresenterTemplate
    {
        private readonly IAccountTransactionsView view;
        private readonly ITransactionReportsService service;
        private readonly IReportManager reportManager;
        private readonly int coderef;

        //constructor
        public AccountTransactionsPresenter(IAccountTransactionsView view, ITransactionReportsService service, IReportManager reportManager)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(service, "service");
            ArgumentChecker.ThrowIfNull(reportManager, "reportManager");

            this.coderef = 5000 + view.ClientId(); //TODO: this code is for retention account transactions
            this.view = view;
            this.service = service;
            this.reportManager = reportManager;
        }

        #region "ReportPresenterTemplate"

        protected override void ShowAllClientsView()
        {
            view.ShowAllClientsView();
        }

        protected override void ShowClientView()
        {
            view.ShowClientView();
        }

        protected override void ShowAllClientsReport(bool firstViewOrScopeChangeOnPage)
        {
            if (firstViewOrScopeChangeOnPage)
            {
                view.Clear();
            }
            else
            {
                Cff.SaferTrader.Core.Reports.AccountTransactionReportBase xTrn = service.LoadAccountTransactionReportForAllClients(coderef, GetDate(), view.FacilityType(), view.IsSalvageIncluded());
                view.DisplayReport(xTrn);
            }
        }

        protected override void ShowClientReport()
        {
            view.DisplayReport(service.LoadAccountTransactionReport(coderef, GetDate(), view.ClientId()));
        }

        protected override bool CanViewReport()
        {
            return reportManager.CanViewAccountTransReport();
        }

        protected override void DisplayAccessDeniedError()
        {
            view.DisplayAccessDeniedError();
        }

        protected override void ShowCustomerReport()
        {
            view.DisplayReportNotAvailableError();
        }

        protected override void ShowCustomerView()
        {
            view.DisplayReportNotAvailableError();
        }
        #endregion

        private Date GetDate()
        {
            return view.EndDate();
        }
    }
}
