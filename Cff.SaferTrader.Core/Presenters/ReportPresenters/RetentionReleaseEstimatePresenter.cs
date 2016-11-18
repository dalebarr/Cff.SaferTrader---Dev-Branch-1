using System;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Core.Presenters.ReportPresenters
{
    public class RetentionReleaseEstimatePresenter : ReportPresenterTemplate
    {
        private readonly IRetentionReleaseEstimateView view;
        private readonly IReportsService service;
        private readonly IReportManager reportManager;

        public RetentionReleaseEstimatePresenter(IRetentionReleaseEstimateView view, IReportsService service, IReportManager reportManager)
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
            throw new NotImplementedException();
        }
        protected override void ShowClientView()
        {
            throw new NotImplementedException();
        }
        protected override void ShowCustomerView()
        {
            throw new NotImplementedException();
        }
        protected override void ShowAllClientsReport(bool firstViewOrScopeChangeOnPage)
        {
            view.DisplayReportNotAvailableError();
        }
        protected override void ShowClientReport()
        {
            view.DisplayReport(service.LoadRetentionReleaseEstimateReport(view.ClientId()));
        }
        protected override void ShowCustomerReport()
        {
            view.DisplayReportNotAvailableError();
        }

        protected override bool CanViewReport()
        {
           return reportManager.CanViewRetentionReleaseEstimateReport();
        }

        protected override void DisplayAccessDeniedError()
        {
            view.DisplayAccessDeniedError();
        }
    }
}