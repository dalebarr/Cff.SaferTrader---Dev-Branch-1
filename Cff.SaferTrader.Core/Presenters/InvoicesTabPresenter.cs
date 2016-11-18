using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class InvoicesTabPresenter
    {
        private readonly IInvoicesTabView view;
        private readonly IBatchRepository repository;

        public InvoicesTabPresenter(IInvoicesTabView view, IBatchRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");

            this.view = view;
            this.repository = repository;
        }

        public static InvoicesTabPresenter Create(IInvoicesTabView view)
        {
            return new InvoicesTabPresenter(view, RepositoryFactory.CreateBatchRepository());
        }

        public void LoadInvoicesFor(int clientId, int batchId)
        {
            view.DisplayInvoices(repository.LoadInvoicesFor(clientId, batchId));
        }
    }
}