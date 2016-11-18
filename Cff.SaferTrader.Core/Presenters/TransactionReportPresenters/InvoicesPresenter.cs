using System;
using Cff.SaferTrader.Core.Presenters.ReportPresenters;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.TransactionReportView;

namespace Cff.SaferTrader.Core.Presenters.TransactionReportPresenters
{
    public class InvoicesPresenter : ReportPresenterTemplate
    {
        private readonly IInvoicesView view;
        private readonly ITransactionReportsService service;
        private readonly IReportManager reportManager;

        public InvoicesPresenter(IInvoicesView view, ITransactionReportsService service, IReportManager reportManager)
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
                view.DisplayReport(service.LoadInvoicesReportForAllClients(view.EndDate(), view.FacilityType(),
                                                                           view.IsSalvageIncluded(), view.ClientFacilityType(), SessionWrapper.Instance.Get.SelectedTransactionFilter));
            }
        }
        protected override void ShowClientReport()
        {
            view.DisplayReport(service.LoadInvoicesReport(view.EndDate(), view.ClientId(), SessionWrapper.Instance.Get.SelectedTransactionFilter, view.ClientFacilityType()));
        }
        protected override void ShowCustomerReport()
        {
            view.DisplayReportForCustomer(service.LoadInvoicesReportForCustomer(view.DateRange(), view.CustomerId()));
        }
        protected override bool CanViewReport()
        {
            return reportManager.CanViewInvoicesReport();
        }
        protected override void DisplayAccessDeniedError()
        {
            view.DisplayAccessDeniedError();
        }
    }
}