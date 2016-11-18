namespace Cff.SaferTrader.Core.Reports
{
    public enum ReportType
    {
        InvoicesReport = 1,
        CreditNotesReport = 2,
        ReceiptsReport = 3,
        CustomerCreditBalanceTransfersReport = 4,
        RepurchaseReport = 5,
        DicountsReport = 6,
        CustoemrJournalsReport = 7,
        JournalsReport = 8,
        CreditBalanceTransfersReport = 9,
        StatusReport = 20,
        RetentionReleaseEstimateReport = 26,
        ControlReport = 30,
        PromptReportForAllInvoices = 20000,
        PromptReportForFactoredInvoices = 21000,
        UnclaimedCreditNotesReport = 203,
        OverdueCharges = 25,
        UnclaimedRepurchasesReport = 503,
        FactoredPromptReport = 21000,
        CurrentShortPaidReport=50,
        CurrentOverpaidReport=55,
        UnallocatedReport=60,
        Overpayments=10,
        CreditLimitExceededReport = 80,
        CreditStopSuggestionsReport = 85,
        ClientAction=119,
        StatementReport=91,
        AccountTransactionReport = 73
    }
}