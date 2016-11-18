using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class CreditsClaimedTabPresenter
    {
        private readonly IRetentionRepository repository;
        private readonly ICreditsClaimedTabView view;

        public CreditsClaimedTabPresenter(ICreditsClaimedTabView view, IRetentionRepository repository)
        {
            this.view = view;
            this.repository = repository;
        }

        public static CreditsClaimedTabPresenter Create(ICreditsClaimedTabView view)
        {
            return new CreditsClaimedTabPresenter(view, RepositoryFactory.CreateRetentionRepository());
        }

        public void LoadCreditsClaimed(RetentionSchedule retentionSchedule, int clientId)
        {
            int retentionItemDate = EndOfMonthHelper.ParseRententionEndOfMonth(retentionSchedule.EndOfMonth);
            
            IList<ClaimedCredit> creditsClaimed = repository.LoadCreditsClaimed(retentionItemDate, clientId);
            view.DisplayCreditsClaimed(creditsClaimed);
        }

        
    }
}
