using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Repositories
{
    public interface ITransactionSearchRepository
    {
        IList<TransactionSearchResult> SearchTransactions(DateRange dateRange, string invoiceNumber, TransactionSearchType transactionType, SearchScope searchScope, CffCustomer customer, ICffClient client, string batchFrom, string batchTo);
        IList<CreditNoteSearchResult> SearchCreditNotesTransactions(DateRange dateRange, string invoiceNumber, TransactionSearchType transactionSearchType, SearchScope searchScope, CffCustomer customer, ICffClient client, string batchFrom, string batchTo);
    }
}