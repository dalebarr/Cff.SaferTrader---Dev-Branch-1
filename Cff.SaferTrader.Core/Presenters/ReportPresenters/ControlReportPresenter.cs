using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Core.Presenters.ReportPresenters
{
    public class ControlReportPresenter : ReportPresenterTemplate
    {
        private readonly IControlView view;
        private readonly IReportsService service;
        private readonly IReportManager reportManager;

        public ControlReportPresenter(IControlView view, IReportsService service, IReportManager reportManager)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(service, "repository");
            ArgumentChecker.ThrowIfNull(reportManager, "reportManager");

            this.view = view;
            this.service = service;
            this.reportManager = reportManager;
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
            if (firstViewOrScopeChangeOnPage) {
                view.Clear();
            }
            view.DisplayReport(service.LoadControlReportForAllClients(view.EndDate(), view.IsSalvageIncluded(),
                                                                          view.FacilityType()));
        }

        protected override void ShowClientReport()
        {
            view.DisplayReport(service.LoadControlReport(view.EndDate(), view.ClientId()));
        }

        protected override void ShowCustomerReport()
        {
            view.DisplayReportNotAvailableError();
        }

        protected override bool CanViewReport()
        {
            return reportManager.CanViewControlReport();
        }

        protected override void DisplayAccessDeniedError()
        {
            view.DisplayAccessDeniedError();
        }
    }
}