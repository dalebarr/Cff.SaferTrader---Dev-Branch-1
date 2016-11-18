using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class RetentionNotesTabPresenter 
    {
        private readonly IRetentionNotesTabView view;
        private readonly IRetentionNotesTabRepository repository;
        public RetentionNotesTabPresenter(IRetentionNotesTabView view, IRetentionNotesTabRepository repository)
        {
            this.view = view;
            this.repository = repository;
        }
        public static RetentionNotesTabPresenter Create(IRetentionNotesTabView view)
        {
            return new RetentionNotesTabPresenter(view, RepositoryFactory.CreateRetentionNotesTabRepository());
        }
        public void LoadRetentionNotesFor(int retentionScheduleID)
        {
            view.DisplayRetentionNotes(repository.LoadRetentionNotesFor(retentionScheduleID));
        }
    }
}