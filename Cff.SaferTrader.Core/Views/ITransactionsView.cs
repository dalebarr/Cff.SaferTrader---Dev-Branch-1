using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Views
{
    public interface ITransactionsView : IRedirectableView
    {
        void ShowTransactions(IList<Transaction> transactions);
    }
}
