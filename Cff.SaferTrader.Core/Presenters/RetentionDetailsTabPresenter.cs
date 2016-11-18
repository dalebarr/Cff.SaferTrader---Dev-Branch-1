using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class RetentionDetailsTabPresenter
    {
        private readonly IRetentionDetailsTabView view;
        private readonly IRetentionRepository retentionRepository;

        public RetentionDetailsTabPresenter(IRetentionDetailsTabView view, IRetentionRepository retentionRepository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(retentionRepository, "retentionRepository");

            this.view = view;
            this.retentionRepository = retentionRepository;
        }

        public static RetentionDetailsTabPresenter Create(IRetentionDetailsTabView view)
        {
            return new RetentionDetailsTabPresenter(view, RepositoryFactory.CreateRetentionRepository());
        }

        public void LoadRetentionDetailsFor(int retentionId)
        {
            view.DisplayRetentionDetails(retentionRepository.LoadRetentionDetails(retentionId));
        }
    }
}