using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class CreditsTabPresenter
    {
        private readonly ICreditsTabView view;
        private readonly IBatchRepository repository;

        public CreditsTabPresenter(ICreditsTabView view, IBatchRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");
            
            this.view = view;
            this.repository = repository;
        }

        public static CreditsTabPresenter Create(ICreditsTabView view)
        {
            return new CreditsTabPresenter(view, RepositoryFactory.CreateBatchRepository());
        }

        public void LoadCreditLinesFor(int clientId, int batchId)
        {
            view.DisplayCredits(repository.LoadCreditLinesFor(clientId, batchId));
        }
    }
}
