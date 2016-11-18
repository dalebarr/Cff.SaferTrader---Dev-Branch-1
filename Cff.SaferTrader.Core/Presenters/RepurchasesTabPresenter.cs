using System;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class RepurchasesTabPresenter
    {
        private readonly IBatchRepository repository;
        private readonly IRepurchasesTabView view;

        public RepurchasesTabPresenter(IRepurchasesTabView view, IBatchRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");
            this.repository = repository;
            this.view = view;
        }

        public static RepurchasesTabPresenter Create(IRepurchasesTabView view)
        {
            return new RepurchasesTabPresenter(view,RepositoryFactory.CreateBatchRepository() );
        }

        public void LoadRepurchasesLinesFor(int clientId, int batchId)
        {
            view.DisplayRepurchases(repository.LoadRepurchasesLinesFor(clientId, batchId));
        }
    }
}