using System;
using Cff.SaferTrader.Core.Presenters.ReportPresenters;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.TransactionReportView;

namespace Cff.SaferTrader.Core.Presenters.TransactionReportPresenters
{
    public class OverpaymentsPresenter: ReportPresenterTemplate
    {
        private readonly IOverpaymentsView view;
        private readonly ITransactionReportsService service;
        private readonly IReportManager reportManager;

        public OverpaymentsPresenter(IOverpaymentsView view, ITransactionReportsService service, IReportManager reportManager)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(service, "service");
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
                          view.DisplayReport(service.LoadOverpaymentsReportForAllClients(view.EndDate(),view.FacilityType(),view.IsSalvageIncluded(), view.ClientFacilityType()));
  
            }
        }
        protected override void ShowClientReport()
        {
            view.DisplayReport(service.LoadOverpaymentsReport(view.EndDate(),view.ClientId(), view.ClientFacilityType()));
        }
        protected override void ShowCustomerReport()
        {
            //ShowClientReport();
            view.DisplayReportForCustomer(service.LoadOverpaymentsReportForCustomer(view.DateRange(), view.CustomerId()));
        }
        protected override bool CanViewReport()
        {
            return reportManager.CanViewOverpaymentsReport();
        }
        protected override void DisplayAccessDeniedError()
        {
            view.DisplayAccessDeniedError();
        }
    }
}