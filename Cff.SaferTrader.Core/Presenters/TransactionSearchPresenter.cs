using System;
using System.Collections.Generic;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.ScopeManager;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class TransactionSearchPresenter
    {
        private readonly ITransactionSearchRepository transactionSearchRepository;
        private readonly ITransactionSearchView view;
        private readonly ISearchScopeManager searchScopeManager;

        public Int16 SearchingStatus;
        public event EventHandler CallBackHandler;

        public TransactionSearchPresenter(ITransactionSearchView view, ITransactionSearchRepository transactionSearchRepository, ISearchScopeManager searchScopeManager)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(searchScopeManager, "searchScopeManager");
            ArgumentChecker.ThrowIfNull(transactionSearchRepository, "transactionSearchRepository");

            this.transactionSearchRepository = transactionSearchRepository;
            this.view = view;
            this.searchScopeManager = searchScopeManager;

        }

        public void SearchTransactions(DateRange dateRange, string invoiceNumber, TransactionSearchType transactionType,
                                       SearchScope searchScope, CffCustomer customer, ICffClient client, string batchFrom, string batchTo)
        {
            if (transactionType == TransactionSearchType.Invoices)
            {
                SearchingStatus = 1;
                if (CallBackHandler != null)
                    this.CallBackHandler(this, new EventArgs());

                IList<TransactionSearchResult> transactions = transactionSearchRepository.SearchTransactions(dateRange,
                                                                                                              invoiceNumber,
                                                                                                              transactionType,
                                                                                                              searchScope,
                                                                                                              customer,
                                                                                                              client,
                                                                                                              batchFrom,
                                                                                                              batchTo);

                SearchingStatus = 2;
                if (CallBackHandler != null)
                    this.CallBackHandler(this, new EventArgs());

                view.DisplayMatchedTransactions(transactions);
            }
            else
            {
                IList<CreditNoteSearchResult> transactions =
                    transactionSearchRepository.SearchCreditNotesTransactions(dateRange,
                                                                              invoiceNumber,
                                                                              transactionType,
                                                                              searchScope, customer,
                                                                              client,
                                                                              batchFrom,
                                                                              batchTo);
                view.DisplayMatchedCreditNotesTransactions(transactions);
            }
        }
        

        public void PopulateSearchScopeDropDownList()
        {
            Dictionary<SearchScope, string> dictionary = searchScopeManager.LoadSearchScope();
            view.PopulateTransactionScopeDropDownList(dictionary);
        }

    }
}