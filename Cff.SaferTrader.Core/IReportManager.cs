namespace Cff.SaferTrader.Core
{
    public interface IReportManager
    {
        bool CanViewControlReport();
        bool CanViewAgedBalances();
        bool CanViewOverdueChargesReport();
        bool CanViewPromptReport();
        bool CanViewStatusReport();
        bool CanViewUnclaimedCreditNotesReport();
        bool CanViewUnclaimedRepurchasesReport();
        bool CanViewRetentionReleaseEstimateReport();
        bool CanViewCreditLimitExceededReport();
        bool CanViewCreditStopSuggestionsReport();
        bool CanViewCallsDueReport();
        bool CanViewClientActionReport();
        bool CanViewCustomerWatchReport();
        bool CanViewCreditNotesReport();
        bool CanViewJournalsReport();
        bool CanViewCreditBalanceTransfersReport();
        bool CanViewInvoicesReport();
        bool CanViewReceiptsReport();
        bool CanViewDiscountsReport();
        bool CanViewRepurchaserasReport();
        bool CanViewCurrentShortPaidReport();
        bool CanViewCurrentOverpaidReport();
        bool CanViewUnallocatedReport();
        bool CanViewOverpaymentsReport();
        bool CanViewStatementReport();
        bool CanViewAccountTransReport();
    }
}