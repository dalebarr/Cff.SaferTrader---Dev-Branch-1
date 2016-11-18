using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Services
{
    public interface ITransactionReportsService
    {
        TransactionReportBase LoadCreditNotesReport(Date endDate, int clientId, int ClientFacilityType);
        TransactionReportBase LoadJournalsReport(Date endDate, int clientId, int ClientFacilityType);
        TransactionReportBase LoadReceiptsReport(Date endDate, int clientId, int ClientFacilityType);

        TransactionReportBase LoadInvoicesReport(Date endDate, int clientId, int ClientFacilityType);
        TransactionReportBase LoadInvoicesReport(Date endDate, int clientId, string transactionFilter, int ClientFacilityType);

        TransactionReportBase LoadDiscountsReport(Date endDate, int clientId, int ClientFacilityType);
        TransactionReportBase LoadRepurchasesReport(Date endDate, int clientId, int ClientFacilityType);
        TransactionReportBase LoadCreditBalanceTransfersReport(Date endDate, int clientId, int ClientFacilityType);
        TransactionReportBase LoadUnclaimedCreditNotesReport(Date endDate, int clientId, int ClientFacilityType);
        TransactionReportBase LoadInvoicesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType, string TransactionFilter);
        TransactionReportBase LoadUnallocatedReportForAllClients(FacilityType facilityType, bool isSalvageIncluded);
        TransactionReportBase LoadUnallocatedReport(int clientId, int ClientFacilityType);
        TransactionReportBase LoadOverpaymentsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        TransactionReportBase LoadOverpaymentsReport(Date endDate, int clientId, int ClientFacilityType);
        TransactionReportBase LoadCreditNotesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);

        TransactionReportBase LoadReceiptsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        TransactionReportBase LoadCreditBalanceTransfersReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        TransactionReportBase LoadJournalsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        TransactionReportBase LoadDiscountsReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        TransactionReportBase LoadRepurchasesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        TransactionReportBase LoadUnclaimedRepurchasesReport(Date endDate, int clientId, int ClientFacilityType);
        TransactionReportBase LoadUnclaimedRepurchasesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        TransactionReportBase LoadUnclaimedCreditNotesReportForAllClients(Date endDate, FacilityType facilityType, bool isSalvageIncluded, int ClientFacilityType);
        
        AccountTransactionReportBase LoadAccountTransactionReport(int coderef, Date endDate, int clientId);
        AccountTransactionReportBase LoadAccountTransactionReportForAllClients(int coderef, Date endDate, FacilityType facilityType, bool isSalvageIncluded);
        
        
        ReportBase LoadReceiptReportForCustomer(DateRange dateRange, int customerId);
        ReportBase LoadCreditNotesReportForCustomer(DateRange dateRange, int customerId);
        ReportBase LoadInvoicesReportForCustomer(DateRange dateRange, int customerId);
        ReportBase LoadJournalsReportForCustomer(DateRange dateRange, int customerId);
        ReportBase LoadCreditBalanceTransfersReportForCustomer(DateRange dateRange, int customerId);
        ReportBase LoadDiscountsReportForCustomer(DateRange dateRange, int customerId);
        ReportBase LoadOverpaymentsReportForCustomer(DateRange Date, int customerId);
    }
}