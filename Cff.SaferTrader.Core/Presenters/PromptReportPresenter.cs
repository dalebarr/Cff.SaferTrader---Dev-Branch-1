using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class PromptReportPresenter
    {
        private readonly IPromptReportView view;
        private readonly IReportRepository repository;
        private readonly IReportManager reportManager;

        public PromptReportPresenter(IPromptReportView view, IReportRepository repository, IReportManager reportManager)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");
            ArgumentChecker.ThrowIfNull(reportManager, "reportManager");

            this.reportManager = reportManager;
            this.view = view;
            this.repository = repository;
        }

        public void LoadDefaultPromptDays(int clientId)
        {
            if (CanViewReport())
            {
                view.DisplayDefaultPromptDays(repository.CalculatePromptDays(clientId));
            }
        }

        public void LoadAllInvoicesPromptReport(int promptDays, int clientId, int ClientFacilityType)
        {
            if (CanViewReport())
            {
                view.ShowReport(repository.LoadPromptReportForAllInvoices(promptDays, clientId, ClientFacilityType));
            }
        }

        public void LoadFactoredPromptReport(int promptDays, int clientId, int ClientFacilityType)
        {
            if (CanViewReport())
            {
                view.ShowReport(repository.LoadPromptReportForFactoredInvoices(promptDays, clientId, ClientFacilityType));
            }
        }

        public void LoadFactoredPromptReportForAllClients(int promptDays, FacilityType facilityType, bool isSalvageIncluded)
        {
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");
            if (CanViewReport())
            {
                view.ShowReport(repository.LoadPromptReportForFactoredInvoicesForAllClients(promptDays, facilityType, isSalvageIncluded));
            }
        }

        public void LoadAllPromptReportForAllClients(int promptDays, FacilityType facilityType, bool isSalvageIncluded)
        {
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");
            if (CanViewReport())
            {
                view.ShowReport(repository.LoadPromptReportForAllInvoicesForAllClients(promptDays, facilityType, isSalvageIncluded));
            }
        }

        private bool CanViewReport()
        {
            bool hasAccess = reportManager.CanViewPromptReport();
            if (!hasAccess)
            {
                view.DisplayAccessDeniedError();
            }
            return hasAccess;
        }
    }
}