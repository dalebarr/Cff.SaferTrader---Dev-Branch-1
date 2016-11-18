using System;
using Cff.SaferTrader.Core.Presenters.ReportPresenters;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.TransactionReportView;

namespace Cff.SaferTrader.Core.Presenters.TransactionReportPresenters
{
    public class UnclaimedCreditNotesPresenter : ReportPresenterTemplate
    {
        private readonly IUnclaimedCreditNotesView view;
        private readonly ITransactionReportsService service;
        private readonly IReportManager reportManager;

        public UnclaimedCreditNotesPresenter(IUnclaimedCreditNotesView view, ITransactionReportsService service, IReportManager reportManager)
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
            view.DisplayReportNotAvailableError();
        }
        protected override void ShowAllClientsReport(bool firstViewOrScopeChangeOnPage)
        {
            if ((firstViewOrScopeChangeOnPage))
            {
                view.Clear();
            }
            else
            {
              view.DisplayReport(service.LoadUnclaimedCreditNotesReportForAllClients(view.EndDate(),view.FacilityType(),view.IsSalvageIncluded(), view.ClientFacilityType()));
            }
        }
        protected override void ShowClientReport()
        {
            view.DisplayReport(service.LoadUnclaimedCreditNotesReport(getDate(), view.ClientId(), view.ClientFacilityType()));
        }
        protected override void ShowCustomerReport()
        {
           view.DisplayReportNotAvailableError();
        }
        protected override bool CanViewReport()
        {
            return reportManager.CanViewUnclaimedCreditNotesReport();
        }

        protected override void DisplayAccessDeniedError()
        {
            view.DisplayAccessDeniedError();
        }

        private Date getDate()
        {
            if (view.EndDate().Month < DateTime.Today.Month)
            {
                return view.LastDate();
            }
            return view.EndDate();
        }
    }
}