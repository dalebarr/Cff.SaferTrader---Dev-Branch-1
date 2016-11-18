using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Repositories
{
    public interface IReportRepository
    {
        TransactionReport LoadCreditNotesReport(Date endDate, int clientId, int ClientFacilityType);
        TransactionReport LoadJournalsReport(Date endDate, int clientId, int ClientFacilityType);
        TransactionReport LoadReceiptsReport(Date endDate, int clientId, int ClientFacilityType);

        InvoicesTransactionReport LoadInvoicesReport(Date endDate, int clientId, int ClientFacilityType);
        InvoicesTransactionReport LoadInvoicesReport(Date endDate, int clientId, string transactionFilter, int ClientFacilityType);

        AgedBalancesReport LoadAgedBalancesReport(Date dateAsAt, int clientId, AgedBalancesReportType reportType, int ClientFacilityType);
        TransactionReport LoadDiscountsReport(Date endDate, int clientId, int ClientFacilityType);
        TransactionReport LoadRepurchasesReport(Date endDate, int clientId, int ClientFacilityType);
        TransactionReport LoadCreditBalanceTransfersReport(Date endDate, int clientId, int ClientFacilityType);
        StatusReport LoadStatusReport(Date endDate, int clientId, TransactionStatus status, int ClientFacilityType);
        StatusReport LoadStatusReportForAllClients(Date endDate, TransactionStatus status, FacilityType facilityType, bool isSalvageIncluded);
        ControlReport LoadControlReport(Date endDate, int clientId);
        RetentionReleaseEstimateReport LoadRetentionReleaseEstimateReport(int clientId);
        OverdueChargesReport LoadOverdueChargesReport(Date endDate, int clientId, TransactionStatus status, int ClientFacilityType);
        TransactionReport LoadUnclaimedCreditNotesReport(Date endDate, int clientId, int ClientFacilityType);
        PromptReport LoadPromptReportForAllInvoices(int promptDays, int clientId, int ClientFacilityType);
        PromptReport LoadPromptReportForFactoredInvoices(int promptDays, int clientId, int ClientFacilityType);
        ControlReport LoadControlReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded);
        CurrentPaymentsReport LoadCurrentShortPaidReport(int clientId, int ClientFacilityType);
        AgedBalancesReport LoadAgedBalancesReportForAllClients(AgedBalancesReportType reportType, FacilityType type, bool isSalvageIncluded, int ClientFacilityType);
        CurrentPaymentsReport LoadCurrentShortPaidReportForAllClients(FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        InvoicesTransactionReport LoadInvoicesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType, string TransactionFilter);
        CurrentPaymentsReport LoadCurrentOverpaidReport(int clientId, int ClientFacilityType);
        CurrentPaymentsReport LoadCurrentOverpaidReportForAllClients(FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        OverdueChargesReport LoadOverdueChargesReportForAllClients(Date endDate, TransactionStatus status, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        PromptReport LoadPromptReportForFactoredInvoicesForAllClients(int promptDays, FacilityType facilityType, bool isSalvageIncluded);
        PromptReport LoadPromptReportForAllInvoicesForAllClients(int promptDays, FacilityType facilityType, bool isSalvageIncluded);
        TransactionReport LoadUnallocatedReportForAllClients(FacilityType facilityType, bool isSalvageIncluded);
        TransactionReport LoadUnallocatedReport(int clientId, int ClientFacilityType);
        TransactionReport LoadOverpaymentsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded);
        TransactionReport LoadOverpaymentsReport(Date endDate, int clientId, int ClientFacilityType);
        TransactionReport LoadCreditNotesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded);
        TransactionReport LoadReceiptsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        TransactionReport LoadCreditBalanceTransfersReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded);
        TransactionReport LoadJournalsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded,int ClientFacilityType);
        TransactionReport LoadDiscountsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        TransactionReport LoadRepurchasesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);

        TransactionReport LoadUnclaimedRepurchasesReport(Date endDate, int clientId, int ClientFacilityType);
        TransactionReport LoadUnclaimedRepurchasesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        TransactionReport LoadUnclaimedCreditNotesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);

        AccountTransactionReport LoadAccountTransactionReport(int coderef, Date endDate, int clientId);
        AccountTransactionReport LoadAccountTransactionReportForAllClients(int coderef, Date endDate, FacilityType facilityType, bool isSalvageIncluded);


        CreditLimitExceededReport LoadCreditLimitExceededReport(int clientId, int ClientFacilityType);
        CreditLimitExceededReport LoadCreditLimitExceededReportForAllClients(FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);

        CreditStopSuggestionsReport LoadCreditStopSuggestionsReport(int clientId, int ClientFacilityType);
        CreditStopSuggestionsReport LoadCreditStopSuggestionsReportForAllClients(FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);

        int CalculatePromptDays(int clientId);
        CallsDueReport LoadCallsDueReport(int clientId, PeriodReportType reportType, BalanceRange balanceRange, string orderByString);
        CallsDueReport LoadCallsDueReportForAllClients(PeriodReportType reportType, BalanceRange balanceRange, FacilityType facilityType, bool isSalvageIncluded, string orderByString);
        ClientActionReport LoadClientActionReport(int clientId);
        ClientActionReport LoadClientActionReportForAllClients(BalanceRange balanceRange, FacilityType facilityType, bool isSalvageIncluded, string orderByString);
        CallsDueReport LoadCustomerWatchReportForAllClients(PeriodReportType reportType, BalanceRange balanceRange, FacilityType facilityType, bool isSalvageIncluded, string orderByString);
        CustomerInvoicesTransactionReport LoadInvoicesReportForCustomer(DateRange dateRange, int customerId);
        CustomerTransactionReport LoadReceiptReportForCustomer(DateRange dateRange, int customerId);
        CustomerTransactionReport LoadCreditNotesReportForCustomer(DateRange dateRange, int customerId);
        CustomerTransactionReport LoadJournalsReportForCustomer(DateRange dateRange, int customerId);
        CustomerTransactionReport LoadCreditBalanceTransfersReportForCustomer(DateRange dateRange, int customerId);
        CustomerTransactionReport LoadDiscountsReportForCustomer(DateRange dateRange, int customerId);
        CustomerOverpaymentsTransactionReport LoadOverpaymentsReportForCustomer(DateRange dateRange, int customerId);
        StatusReport LoadStatusReportForCustomer(Date endDate, int clientId, CffCustomer customer, TransactionStatus status, int ClientFacilityType);
        OverdueChargesReport LoadOverdueChargesReportForCustomer(Date endDate, int clientId, CffCustomer customer, TransactionStatus status, int ClientFacilityType);
        StatementReport LoadStatementReport(int clientId, int customerId, Date endDate);
    }
}