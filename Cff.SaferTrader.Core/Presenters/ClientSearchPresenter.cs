using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;
namespace Cff.SaferTrader.Core.Presenters
{
    public class ClientSearchPresenter
    {
        private readonly IClientSearchView view;
        private readonly IClientRepository repository;

        public ClientSearchPresenter(IClientSearchView view, IClientRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "ClientSearchView");
            ArgumentChecker.ThrowIfNull(repository, "ClientSearchView");
            this.view = view;
            this.repository = repository;
        }

        public void ShowMatchedNames(string stringToMatch)
        { //All Clients entry is added in cff.js, thus repository needs to return one less client
          
            int numberOfClientsToReturn = Config.NumberOfClientsToReturn - 1;
            view.DisplayMatchedClientNameAndNum(repository.LoadMatchedClientNameAndNum(stringToMatch, numberOfClientsToReturn));
        }

        public static ClientSearchPresenter Create(IClientSearchView view)
        {
            return new ClientSearchPresenter(view, RepositoryFactory.CreateClientRepository());
        }
    }
}