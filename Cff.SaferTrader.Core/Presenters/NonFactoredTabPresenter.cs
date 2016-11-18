using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class NonFactoredTabPresenter
    {
        private readonly IBatchRepository repository;
        private readonly INonFactoredTabView view;

        public NonFactoredTabPresenter(INonFactoredTabView view, IBatchRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");

            this.view = view;
            this.repository = repository;
        }

        public static NonFactoredTabPresenter Create(INonFactoredTabView view)
        {
            return new NonFactoredTabPresenter(view, RepositoryFactory.CreateBatchRepository());
        }

        public void LoadNonFactoredInvoicesFor(int clientId, int batchId)
        {
            view.DisplayNonFactoredInvoices(repository.LoadNonFactoredInvoicesFor(clientId, batchId));
        }
    }
}
