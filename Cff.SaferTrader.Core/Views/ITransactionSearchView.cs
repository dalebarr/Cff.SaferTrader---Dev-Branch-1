using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Views
{
    public interface ITransactionSearchView : IRedirectableView
    {
        void DisplayMatchedTransactions(IList<TransactionSearchResult> transactions);
        void DisplayMatchedCreditNotesTransactions(IList<CreditNoteSearchResult> creditNoteSearchResults);
        void PopulateTransactionScopeDropDownList(Dictionary<SearchScope, string> searchScope);
    }
}