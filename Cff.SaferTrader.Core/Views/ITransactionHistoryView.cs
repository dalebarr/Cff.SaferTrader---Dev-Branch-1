using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Views
{
    public interface ITransactionHistoryView : IRedirectableView
    {
        void ShowTransactionHistory(IList<HistoricalTransaction> historicalTransactions);
        void ShowCustomerView();
        void ShowAllClientsView();
        void ShowClientView();

        string QueryStringParameters { get; }
    }
}