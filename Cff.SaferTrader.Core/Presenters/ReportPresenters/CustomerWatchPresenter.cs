using System;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Core.Presenters.ReportPresenters
{
    public class CustomerWatchPresenter : ReportPresenterTemplate
    {
        private readonly ICustomerWatchView view;
        private readonly IReportsService service;
        private readonly IReportManager reportManager;

        public CustomerWatchPresenter(ICustomerWatchView view, IReportsService service, IReportManager reportManager)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(service, "repository");
            ArgumentChecker.ThrowIfNull(reportManager, "reportManager");

            this.view = view;
            this.reportManager = reportManager;
            this.service = service;
        }
       
        protected override void ShowAllClientsView()
        {
            view.ShowAllClientsView();
        }
        protected override void ShowClientView()
        {
            view.DisplayReportNotAvailableError();
        }
        protected override void ShowCustomerView()
        {
            view.DisplayReportNotAvailableError();
        }
        protected override void ShowAllClientsReport(bool firstViewOrScopeChangeOnPage)
        {
            if (firstViewOrScopeChangeOnPage)
            {
                view.Clear();
            }
            else
            {
                view.DisplayReport(service.LoadCustomerWatchReportForAllClients(view.ReportType(), view.BalanceRange(), view.FacilityType(), view.IsSalvageIncluded(), view.AllClientsOrderByString()));

            }
        }
        protected override void ShowClientReport()
        {
           view.DisplayReportNotAvailableError();
        }
        protected override void ShowCustomerReport()
        {
            view.DisplayReportNotAvailableError();
        }

        protected override bool CanViewReport()
        {
            return reportManager.CanViewCustomerWatchReport();
        }

        protected override void DisplayAccessDeniedError()
        {
            view.DisplayAccessDeniedError();
        }
    }
}