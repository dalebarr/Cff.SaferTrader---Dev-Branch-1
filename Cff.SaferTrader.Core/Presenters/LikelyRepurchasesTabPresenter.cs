using System;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;


namespace Cff.SaferTrader.Core.Presenters
{
    public class LikelyRepurchasesTabPresenter
    {
        private readonly IBatchRepository repository;
        private readonly ILikelyRepurchasesTabView view;

        public LikelyRepurchasesTabPresenter(ILikelyRepurchasesTabView view, IBatchRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");
            this.repository = repository;
            this.view = view;
        }

        public static LikelyRepurchasesTabPresenter Create(ILikelyRepurchasesTabView view)
        {
            return new LikelyRepurchasesTabPresenter(view, RepositoryFactory.CreateBatchRepository());
        }

        public void LoadLikelyRepurchasesLinesFor(int clientId, int batchId, int custID, int userid, string strAsAt)
        {
            view.DisplayLikelyRepurchases(repository.LoadLikelyRepurchasesLinesFor(clientId, batchId, custID, userid, strAsAt));
        }
    }
}
