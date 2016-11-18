using System.Collections.Generic;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class TransactionArchivePresenter
    {
        private readonly ITransactionArchiveView view;
        private readonly ITransactionRepository repository;
        private readonly IRedirectionService redirectionService;

        public TransactionArchivePresenter(ITransactionArchiveView view, ITransactionRepository repository, IRedirectionService redirectionService)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");
            ArgumentChecker.ThrowIfNull(redirectionService, "redirectionService");

            this.view = view;
            this.repository = repository;
            this.redirectionService = redirectionService;
        }

        public void LoadTransactionArchive(DateRange dateRange, int customerId, bool bInvoicesOnly)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");

            IList<ArchivedTransaction> archivedTransactions = repository.LoadTransactionArchive(dateRange, customerId, bInvoicesOnly);
            view.ShowTransactionArchive(archivedTransactions);
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