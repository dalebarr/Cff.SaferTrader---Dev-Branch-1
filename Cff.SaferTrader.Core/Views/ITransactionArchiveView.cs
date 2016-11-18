using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Views
{
    public interface ITransactionArchiveView : IRedirectableView
    {
        void ShowTransactionArchive(IList<ArchivedTransaction> archivedTransactions);
    }
}