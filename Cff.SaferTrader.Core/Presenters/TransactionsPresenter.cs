using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class TransactionsPresenter
    {
        private readonly ITransactionRepository repository;
        private readonly IRedirectionService redirectionService;
        private readonly ITransactionsView view;

        public TransactionsPresenter(ITransactionsView view, ITransactionRepository repository, IRedirectionService redirectionService)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");
            ArgumentChecker.ThrowIfNull(redirectionService, "redirectionService");

            this.view = view;
            this.repository = repository;
            this.redirectionService = redirectionService;
        }

        public void LoadCurrentTransactions(int customerId)
        {
            view.ShowTransactions(repository.LoadCurrentTransactions(customerId));
        }

        public void LoadCurrentTransactionsInvoices(int customerId)
        {
            view.ShowTransactions(repository.LoadCurrentTransactionsInvoices(customerId));
        }

        public void LoadBatchDetailsAndRedirect(BatchRecord transaction)
        {
            ArgumentChecker.ThrowIfNull(transaction, "transaction");
            redirectionService.RedirectToInvoiceBatches(transaction.Batch);
        }

        public void InitializeForScope(Scope scope)
        {
            if (scope != Scope.CustomerScope)
            {
                redirectionService.RedirectToTransactionSearch();
            }
        }
    }
}