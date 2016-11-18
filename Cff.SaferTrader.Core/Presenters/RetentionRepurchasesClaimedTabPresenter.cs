using System.Collections.Generic;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class RetentionRepurchasesClaimedTabPresenter
    {
        private readonly IRetentionRepurchasesClaimedTabView view;
        private readonly IRetentionRepository repository;

        public static RetentionRepurchasesClaimedTabPresenter Create(IRetentionRepurchasesClaimedTabView view)
        {
            return new RetentionRepurchasesClaimedTabPresenter(view, RepositoryFactory.CreateRetentionRepository());
        }

        public RetentionRepurchasesClaimedTabPresenter(IRetentionRepurchasesClaimedTabView view, IRetentionRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");
            this.view = view;
            this.repository = repository;
        }

        public void LoadClaimedetentionRepurchasesFor(RetentionSchedule retentionSchedule, int clientId)
        {
            //5
            int retentionItemDate = EndOfMonthHelper.ParseRententionEndOfMonth(retentionSchedule.EndOfMonth);

            IList<ClaimedRetentionRepurchase> repurchaseClaimed = repository.LoadClaimedRetentionRepurchase(retentionItemDate, clientId);
            view.DisplayRetentionRepurchasesClaimed(repurchaseClaimed);
        }
    }
}