using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Core.Presenters.ReportPresenters
{
    public class CallsDuePresenter : ReportPresenterTemplate
    {
        private readonly ICallsDueView view;
        private readonly IReportsService service;
        private readonly IReportManager reportManager;

        public CallsDuePresenter(ICallsDueView view, IReportsService service, IReportManager reportManager)
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
            if (firstViewOrScopeChangeOnPage)
            {
                view.Clear();
            }
            else
            {
                view.DisplayReport(service.LoadCallsDueReportForAllClients(view.ReportType(),view.BalanceRange(),view.FacilityType(),view.IsSalvageIncluded(),view.AllClientsOrderByString()));
            }
        }

        protected override void ShowClientReport()
        {
            view.DisplayReport(service.LoadCallsDueReport(view.ClientId(), view.ReportType(), view.BalanceRange(), view.ClientOrderByString()));

        }

        protected override void ShowCustomerReport()
        {
            view.DisplayReportNotAvailableError();
        }

        protected override bool CanViewReport()
        {
            return reportManager.CanViewCallsDueReport();
        }

        protected override void DisplayAccessDeniedError()
        {
            view.DisplayAccessDeniedError();
        }

       
    }
}