using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Services
{
    public interface IReportsService
    {
        AgedBalancesReport LoadAgedBalancesReport(Date dateAsAt, int clientId, AgedBalancesReportType reportType, int ClientFacilityType);
        AgedBalancesReport LoadAgedBalancesReportForAllClients(AgedBalancesReportType reportType, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        ControlReport LoadControlReport(Date endDate, int clientId);
        StatusReport LoadStatusReport(Date endDate, int clientId, TransactionStatus status, int ClientFacilityType);
        StatusReport LoadStatusReportForAllClients(Date endDate, TransactionStatus status, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        StatusReport LoadStatusReportForCustomer(Date endDate, int clientId, CffCustomer customer, TransactionStatus status, int ClientFacilityType);
        RetentionReleaseEstimateReport LoadRetentionReleaseEstimateReport(int clientId);
        OverdueChargesReport LoadOverdueChargesReport(Date endDate, int clientId, TransactionStatus status, int ClientFacilityType);
        OverdueChargesReport LoadOverdueChargesReportForCustomer(Date endDate, int clientId, CffCustomer customer, TransactionStatus status, int ClientFacilityType);
        OverdueChargesReport LoadOverdueChargesReportForAllClients(Date endDate, TransactionStatus status, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        CurrentPaymentsReport LoadCurrentShortPaidReport(int clientId, int ClientFacilityType);
        CurrentPaymentsReport LoadCurrentShortPaidReportForAllClients(FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        CurrentPaymentsReport LoadCurrentOverpaidReport(int clientId, int ClientFacilityType);
        CurrentPaymentsReport LoadCurrentOverpaidReportForAllClients(FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        ControlReport LoadControlReportForAllClients(Date endDate, bool isSalvageIncluded, FacilityType facilityType);
        CreditLimitExceededReport LoadCreditLimitExceededReport(int clientId, int ClientFacilityType);
        CreditLimitExceededReport LoadCreditLimitExceededReportForAllClients(FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        CreditStopSuggestionsReport LoadCreditStopSuggestionsReport(int clientId, int ClientFacilityType);
        CreditStopSuggestionsReport LoadCreditStopSuggestionsReportForAllClients(FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        CallsDueReport LoadCallsDueReportForAllClients(PeriodReportType reportType, BalanceRange balanceRange, FacilityType facilityType, bool isSalvageIncluded, string orderByString);
        CallsDueReport LoadCallsDueReport(int clientId, PeriodReportType reportType, BalanceRange balanceRange, string orderByString);
        ClientActionReport LoadClientActionReport(int clientId);
        ClientActionReport LoadClientActionReportForAllClients(BalanceRange balanceRange, FacilityType facilityType, bool isSalvageIncluded, string orderByString);
        CallsDueReport LoadCustomerWatchReportForAllClients(PeriodReportType reportType, BalanceRange balanceRange, FacilityType facilityType, bool isSalvageIncluded, string orderByString);
    }
}