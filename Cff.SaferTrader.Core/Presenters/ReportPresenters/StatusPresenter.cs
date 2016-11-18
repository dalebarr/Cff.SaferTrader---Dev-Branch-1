using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Core.Presenters.ReportPresenters
{
    public class StatusPresenter : ReportPresenterTemplate
    {
        private readonly IStatusView view;
        private readonly IReportsService service;
        private readonly IReportManager reportManager;

        public StatusPresenter(IStatusView view, IReportsService service, IReportManager reportManager)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(service, "service");
            ArgumentChecker.ThrowIfNull(reportManager, "reportManager");

            this.reportManager = reportManager;
            this.view = view;
            this.service = service;
        }

        protected override void ShowAllClientsReport(bool firstViewOrScopeChangeOnPage)
        {
            if (firstViewOrScopeChangeOnPage)
            {
                view.Clear();
            }
            else
            {
                view.DisplayReport(service.LoadStatusReportForAllClients(view.EndDate(), view.transactionStatus(), view.FacilityType(), view.IsSalvageIncluded(),view.ClientFacilityType()));
                
            }
        }

        protected override void ShowClientReport()
        {
            view.DisplayReport(service.LoadStatusReport(view.EndDate(),view.ClientId(),view.transactionStatus(), view.ClientFacilityType()));
        }



        protected override void ShowCustomerReport()
        {
            view.DisplayReport(service.LoadStatusReportForCustomer(view.EndDate(), view.ClientId(), view.Customer(), view.transactionStatus(), view.ClientFacilityType()));
    
        }

        protected override bool CanViewReport()
        {
            return reportManager.CanViewStatusReport();
        }

        protected override void DisplayAccessDeniedError()
        {
            view.DisplayAccessDeniedError();
        }

        protected override void ShowAllClientsView()
        {
            view.ShowAllClientsView();
        }

        protected override void ShowClientView()
        {
            view.ShowClientView(); ;
        }

        protected override void ShowCustomerView()
        {
            view.ShowCustomerView();
        }
    }
}