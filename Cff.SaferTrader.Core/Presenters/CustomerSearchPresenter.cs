using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;
namespace Cff.SaferTrader.Core.Presenters
{
    public class CustomerSearchPresenter
    {
        private readonly ICustomerSearchView view;
        private readonly ICustomerRepository repository;

        public CustomerSearchPresenter(ICustomerSearchView view, ICustomerRepository repository)
        {
            ArgumentChecker.ThrowIfNull(view, "customerSearchView");
            ArgumentChecker.ThrowIfNull(repository, "CustomerSearchRepository");
            this.view = view;
            this.repository = repository;
        }

        public void ShowMatchedNames(string stringToMatch, long clientId, int criteria)
        {
            bool bSearchOthers = false;

            if (SessionWrapper.Instance.Get != null)
            {
                if (SessionWrapper.Instance.Get.IsStartsWithChecked)
                    bSearchOthers = true;
            }
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
            {
                if ((SessionWrapper.Instance.GetSession(QueryString.ViewIDValue)).IsStartsWithChecked)
                    bSearchOthers = true;
            }

            if (bSearchOthers || string.IsNullOrEmpty(stringToMatch))
            {
                switch (criteria) {
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        {
                            if (string.IsNullOrEmpty(stringToMatch))
                                stringToMatch += '%';
                            else
                                stringToMatch = '%' + stringToMatch + '%';
                        }
                        break;

                    default:
                        stringToMatch += '%';
                        break;
                
                }
            }

            string MatchedCustSearchJSON = repository.GetMatchedCustomersJSON(stringToMatch, clientId, Config.NumberOfCustomersToReturn, criteria);
            view.DisplayMatchedSearch(MatchedCustSearchJSON);
        }

        public static CustomerSearchPresenter Create(ICustomerSearchView view)
        {
            return new CustomerSearchPresenter(view, RepositoryFactory.CreateCustomerRepository());
        }
    }
}