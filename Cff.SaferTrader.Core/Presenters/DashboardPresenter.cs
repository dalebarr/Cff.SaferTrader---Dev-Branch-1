using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Presenters
{
    public class DashboardPresenter
    {
        private readonly IDashboardView view;
        private readonly IScopeService scopeService;

        public DashboardPresenter(IDashboardView view, IScopeService scopeService)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(scopeService, "scopeService");

            this.view = view;
            this.scopeService = scopeService;
        }

        /// <summary>
        /// Checks query string.
        /// If Customer ID is specified, the Customer will be selected . If Client ID is specified, the Client will be selected.
        /// </summary>
        public void InitialiseView()
        {
            // Check Customer query string
            if (view != null)
            {
                string customerIdQueryString = view.CustomerIdQueryString;
                string clientIdQueryString = view.ClientIdQueryString;

                if (QueryString.IsInteger(customerIdQueryString) && QueryString.IsInteger(clientIdQueryString))
                {
                    //logic here : if both client and customer id exist in the query string then check whether these two are matching?
                    //if not, direct user to the dashboard and replace id of the client that the customer associates with
                    if (SessionWrapper.Instance.Get!=null)
                        if (SessionWrapper.Instance.Get.UserIdentity == 1)
                        {
                            scopeService.SelectCustomerClientAtCustomerScope(int.Parse(clientIdQueryString), int.Parse(customerIdQueryString));

                        }
                        else
                            scopeService.CheckClientAndCustomerMatchAndSelectCustomer(int.Parse(clientIdQueryString), int.Parse(customerIdQueryString));
                    //these two methods call used to set the client and customer id in session
                    //scopeService.SelectCustomer(int.Parse(customerIdQueryString));
                    //scopeService.SelectCustomerByClientAndCustomer(int.Parse(clientIdQueryString), int.Parse(customerIdQueryString));
                }
                //else
                //{
                //    // If Customer query string is not found, try Client query string
                //    string clientIdQueryString = view.ClientIdQueryString;
                //    if (QueryString.IsInteger(clientIdQueryString))
                //    {
                //        scopeService.SelectClient(int.Parse(clientIdQueryString));
                //    }
                //}
            }
        }

        public bool IsReadAgreement(System.Guid userId)
        {
            bool bRet = true;
            ICffUserRepository repository = RepositoryFactory.CreateCffUserRepository();
            if (repository.AcceptAgreement(userId, null) == false)
            {
                bRet = false;
            }
            return bRet;
        }

        public string DashboardMainContent()
        {
            ICffUserRepository repository = RepositoryFactory.CreateCffUserRepository();
            return repository.GetDashboardContent();
        }
    }
}