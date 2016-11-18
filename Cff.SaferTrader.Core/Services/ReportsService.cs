using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IReportRepository repository;

        public ReportsService(IReportRepository repository)
        {
           this.repository = repository;
        }

        public AgedBalancesReport LoadAgedBalancesReport(Date dateAsAt, int clientId, AgedBalancesReportType reportType, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(dateAsAt, "dateAsAt");
            return repository.LoadAgedBalancesReport(dateAsAt, clientId, reportType, ClientFacilityType);
        }

        public AgedBalancesReport LoadAgedBalancesReportForAllClients(AgedBalancesReportType reportType, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(reportType, "reportType");
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");

            return repository.LoadAgedBalancesReportForAllClients(reportType, facilityType, isSalvageIncluded, ClientFacilityType);
        }

        public ControlReport LoadControlReport(Date endDate, int clientId)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            return repository.LoadControlReport(endDate, clientId);
        }

        public StatusReport LoadStatusReport(Date endDate, int clientId, TransactionStatus status, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            ArgumentChecker.ThrowIfNull(status, "status");

           return repository.LoadStatusReport(endDate, clientId, status, ClientFacilityType);
        }

        public StatusReport LoadStatusReportForAllClients(Date endDate, TransactionStatus status, FacilityType facilityType, bool isSalvageIncluded,  int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            ArgumentChecker.ThrowIfNull(status, "status");
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");

            return repository.LoadStatusReportForAllClients(endDate, status, facilityType, isSalvageIncluded);
        }

        public StatusReport LoadStatusReportForCustomer(Date endDate, int clientId, CffCustomer customer, TransactionStatus status, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            ArgumentChecker.ThrowIfNull(status, "status");

            return repository.LoadStatusReportForCustomer(endDate, clientId, customer, status, ClientFacilityType);
        }

        public RetentionReleaseEstimateReport LoadRetentionReleaseEstimateReport(int clientId)
        {
           return repository.LoadRetentionReleaseEstimateReport(clientId );
        }

        public OverdueChargesReport LoadOverdueChargesReport(Date endDate, int clientId, TransactionStatus status, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            ArgumentChecker.ThrowIfNull(status, "status");

            return repository.LoadOverdueChargesReport(endDate, clientId, status, ClientFacilityType);
        }

        public OverdueChargesReport LoadOverdueChargesReportForCustomer(Date endDate, int clientId, CffCustomer customer, TransactionStatus status, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            ArgumentChecker.ThrowIfNull(status, "status");

            return repository.LoadOverdueChargesReportForCustomer(endDate, clientId, customer, status, ClientFacilityType);
        }

        public OverdueChargesReport LoadOverdueChargesReportForAllClients(Date endDate, TransactionStatus status, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(endDate, "endDate");
            ArgumentChecker.ThrowIfNull(status, "status");
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");

            return repository.LoadOverdueChargesReportForAllClients(endDate, status, facilityType, isSalvageIncluded, ClientFacilityType);
        }

        public CurrentPaymentsReport LoadCurrentShortPaidReport(int clientId, int ClientFacilityType)
        {
           return repository.LoadCurrentShortPaidReport(clientId,  ClientFacilityType);
        }

        public CurrentPaymentsReport LoadCurrentShortPaidReportForAllClients(FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType)
        {
            return repository.LoadCurrentShortPaidReportForAllClients(facilityType, isSalvageIncluded, ClientFacilityType);
        }

        public CurrentPaymentsReport LoadCurrentOverpaidReport(int clientId, int ClientFacilityType)
        {
            return repository.LoadCurrentOverpaidReport(clientId, ClientFacilityType);
        }

        public CurrentPaymentsReport LoadCurrentOverpaidReportForAllClients(FacilityType facilityType, bool isSalvageIncluded,int ClientFacilityType)
        {
            return repository.LoadCurrentOverpaidReportForAllClients(facilityType, isSalvageIncluded, ClientFacilityType);
        }

        public ControlReport LoadControlReportForAllClients(Date endDate, bool isSalvageIncluded, FacilityType facilityType)
        {
            return repository.LoadControlReportForAllClients(endDate, facilityType, isSalvageIncluded);
        }

        public CreditLimitExceededReport LoadCreditLimitExceededReport(int clientId, int ClientFacilityType)
        {
            return repository.LoadCreditLimitExceededReport(clientId, ClientFacilityType);

        }
        public CreditLimitExceededReport LoadCreditLimitExceededReportForAllClients(FacilityType facilityType, bool isSalvageIncluded,int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");
           return repository.LoadCreditLimitExceededReportForAllClients(facilityType, isSalvageIncluded, ClientFacilityType);
        }

        public CreditStopSuggestionsReport LoadCreditStopSuggestionsReport(int clientId, int ClientFacilityType)
        {
            return repository.LoadCreditStopSuggestionsReport(clientId, ClientFacilityType);
        }

        public CreditStopSuggestionsReport LoadCreditStopSuggestionsReportForAllClients(FacilityType facilityType, bool isSalvageIncluded,int ClientFacilityType)
        {
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");
           return repository.LoadCreditStopSuggestionsReportForAllClients(facilityType, isSalvageIncluded, ClientFacilityType);
        }
        



        public CallsDueReport LoadCallsDueReportForAllClients(PeriodReportType reportType, BalanceRange balanceRange, FacilityType facilityType, bool isSalvageIncluded, string orderByString)
        {
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");
            ArgumentChecker.ThrowIfNull(balanceRange, "balanceRange");
           return repository.LoadCallsDueReportForAllClients(reportType,balanceRange, facilityType, isSalvageIncluded, orderByString);
        }

        public CallsDueReport LoadCallsDueReport(int clientId, PeriodReportType reportType, BalanceRange balanceRange, string orderByString)
        {
            ArgumentChecker.ThrowIfNull(balanceRange, "balanceRange");
            return repository.LoadCallsDueReport(clientId, reportType, balanceRange, orderByString);
        }

        public ClientActionReport LoadClientActionReport(int clientId)
        {
            return repository.LoadClientActionReport(clientId);
        }

        public ClientActionReport LoadClientActionReportForAllClients(BalanceRange balanceRange, FacilityType facilityType, bool isSalvageIncluded, string orderByString)
        {
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");
            ArgumentChecker.ThrowIfNull(balanceRange, "balanceRange");
            return repository.LoadClientActionReportForAllClients(balanceRange, facilityType, isSalvageIncluded, orderByString);
        }

        public CallsDueReport LoadCustomerWatchReportForAllClients(PeriodReportType reportType, BalanceRange balanceRange, FacilityType facilityType, bool isSalvageIncluded, string orderByString)
        {
            ArgumentChecker.ThrowIfNull(facilityType, "facilityType");
            ArgumentChecker.ThrowIfNull(balanceRange, "balanceRange");
            return repository.LoadCustomerWatchReportForAllClients(reportType, balanceRange, facilityType, isSalvageIncluded, orderByString);
        }

        public static IReportsService Create()
        {
            return new ReportsService(RepositoryFactory.CreateReportRepository());
        }
    }
}