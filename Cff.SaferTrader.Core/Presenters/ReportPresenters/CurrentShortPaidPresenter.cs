using System;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Core.Presenters.ReportPresenters
{
    public class CurrentShortPaidPresenter : ReportPresenterTemplate
    {
        private readonly ICurrentShortPaidView view;
        private readonly IReportsService service;
        private readonly IReportManager reportManager;

        public CurrentShortPaidPresenter(ICurrentShortPaidView view, IReportsService service, IReportManager reportManager)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(service, "repository");
            ArgumentChecker.ThrowIfNull(reportManager, "reportManager");

            this.reportManager = reportManager;
            this.view = view;
            this.service = service;
        }

        protected override void ShowAllClientsView()
        {
            view.ShowAllClientsView();
        }
        protected override void ShowClientView()
        {
           view.ShowClientView();
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
            }else
            {
                view.DisplayReport(service.LoadCurrentShortPaidReportForAllClients(view.FacilityType(), view.IsSalvageIncluded(), view.ClientFacilityType()));

            }

        }
        protected override void ShowClientReport()
        {
            view.DisplayReport(service.LoadCurrentShortPaidReport(view.ClientId(), view.ClientFacilityType()));
        }
        protected override void ShowCustomerReport()
        {
            view.DisplayReportNotAvailableError();
        }

        protected override bool CanViewReport()
        {
            return reportManager.CanViewCurrentShortPaidReport();
        }

        protected override void DisplayAccessDeniedError()
        {
            view.DisplayAccessDeniedError();
        }
    }
}