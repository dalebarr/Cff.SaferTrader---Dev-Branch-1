using System;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class ManagementDetailsBoxPresenter
    {
        private readonly IManagementDetailsBoxView view;
        private readonly IManagementRepository repository;

        public static ManagementDetailsBoxPresenter Create(IManagementDetailsBoxView view)
        {
            return new ManagementDetailsBoxPresenter(view, RepositoryFactory.CreateManagementRepository());
        }

        public ManagementDetailsBoxPresenter(IManagementDetailsBoxView view, IManagementRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");

            this.view = view;
            this.repository = repository;
        }


        public void LoadManagementDetails()
        {
            view.DisplayManagementDetails(repository.LoadManagementDetails());
        }
    }
}