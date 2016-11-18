using System;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Core.Presenters.ReportPresenters
{
    public class CreditLimitExceededPresenter : ReportPresenterTemplate
    {
        private readonly ICreditLimitExceededView view;
        private readonly IReportsService service;
        private readonly IReportManager reportManager;

        public CreditLimitExceededPresenter(ICreditLimitExceededView view, IReportsService service, IReportManager reportManager)
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
                view.DisplayReport(service.LoadCreditLimitExceededReportForAllClients(view.FacilityType(), view.IsSalvageIncluded(), view.ClientFacilityType()));

            }
        }

        protected override void ShowClientReport()
        {
            view.DisplayReport(service.LoadCreditLimitExceededReport(view.ClientId(), view.ClientFacilityType()));
        }

        protected override void ShowCustomerReport()
        {
            view.DisplayReportNotAvailableError();
        }

        protected override bool CanViewReport()
        {
            return reportManager.CanViewCreditLimitExceededReport();
        }

        protected override void DisplayAccessDeniedError()
        {
            view.DisplayAccessDeniedError();
        }
        
    }
}