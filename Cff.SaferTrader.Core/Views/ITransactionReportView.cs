using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Views
{
    public interface ITransactionReportView
    {
        void ShowTransactionsReport(TransactionReportBase transactionReport);
        void ShowTransactionsReportForCustomer(ReportBase transactionReport);
        void ShowAllClientsView();
        void ShowClientView();
        void ShowCustomerView();
    }
}