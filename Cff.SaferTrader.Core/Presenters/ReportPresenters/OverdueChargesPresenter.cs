using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Core.Presenters.ReportPresenters
{
    public class OverdueChargesPresenter : ReportPresenterTemplate
    {
        private readonly IReportsService service;
        private readonly IReportManager reportManager;
        private readonly IOverdueChargesView view;

        public OverdueChargesPresenter(IOverdueChargesView view, IReportsService service, IReportManager reportManager)
        {
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
            view.ShowCustomerView();
        }

        protected override void ShowAllClientsReport(bool firstViewOrScopeChangeOnPage)
        {
            if (firstViewOrScopeChangeOnPage)
            {
                view.Clear();
            }
            else
            {
                view.DisplayReport(service.LoadOverdueChargesReportForAllClients(view.EndDate(),
                                                                                 view.TransactionStatus(),
                                                                                 view.FacilityType(),
                                                                                 view.IsSalvageIncluded(),
                                                                                 view.ClientFacilityType()));
            }
        }

        protected override void ShowClientReport()
        {
            view.DisplayReport(service.LoadOverdueChargesReport(view.EndDate(), view.ClientId(),
                                                                view.TransactionStatus(), view.ClientFacilityType()));
        }

        protected override void ShowCustomerReport()
        {
            view.DisplayReport(service.LoadOverdueChargesReportForCustomer(view.EndDate(), view.ClientId(),
                                                                               (CffCustomer)view.Customer, view.TransactionStatus(), view.ClientFacilityType()));
        }

        protected override bool CanViewReport()
        {
            return reportManager.CanViewOverdueChargesReport();
        }

        protected override void DisplayAccessDeniedError()
        {
            view.DisplayAccessDeniedError();
        }
    }
}