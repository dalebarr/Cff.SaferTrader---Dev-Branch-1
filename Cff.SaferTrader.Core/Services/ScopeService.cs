using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Services
{
    public class ScopeService : IScopeService
    {
        private readonly IClientRepository clientRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly ISecurityManager securityManager;
        private readonly IScopedView view;
        private const string DashBoardUri = "~/Dashboard.aspx";

        public ScopeService(IScopedView view, ISecurityManager securityManager, IClientRepository clientRepository, ICustomerRepository customerRepository)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(securityManager, "securityManager");
            ArgumentChecker.ThrowIfNull(clientRepository, "clientRepository");
            ArgumentChecker.ThrowIfNull(customerRepository, "customerRepository");

            this.view = view;
            this.securityManager = securityManager;
            this.clientRepository = clientRepository;
            this.customerRepository = customerRepository;
        }

        public void SelectCustomer(int customerId)
        {
            if (securityManager.CanChangeSelectedCustomer())
            {
                if (securityManager.CanChangeSelectedClient())
                {
                    CffClient client = clientRepository.GetCffClientByCustomerId(customerId);
                    CffCustomer customer = customerRepository.GetCffCustomerByCustomerId(customerId);

                    if (client != null && customer != null)
                    {
                       // view.Client = client;
                       // view.Customer = customer;
                    }
                }
                else
                {
                    CffCustomer customer = customerRepository.GetCffCustomerByClientIdAndCustomerId(view.Client.Id, customerId);
                    if (customer != null)
                    {
                       // view.Customer = customer;
                    }
                }
            }
        }

        public void SelectCustomerClientAtCustomerScope(int clientId, int customerId)
        {
            if (securityManager.CanChangeSelectedClient())
            {
                ICffClient client = clientRepository.GetCffClientByClientId(clientId);
                CffCustomer customer = customerRepository.GetCffCustomerByCustomerId(customerId);
                if (client != null)
                {
                    view.Client = client;
                    CffCustomerExt xC = new CffCustomerExt(customer.Name, customer.Id, customer.Number);
                    view.Customer = (ICffCustomer)xC;
                }
 
            }
        }


        public void CheckClientAndCustomerMatchAndSelectCustomer(int clientId, int customerId)
        {
            if (securityManager.CanChangeSelectedCustomer())
            {
                if (securityManager.CanChangeSelectedClient())
                {

                    ICffClient client = clientRepository.GetCffClientByClientId(clientId);
                    CffClient clientBycustomerId = clientRepository.GetCffClientByCustomerId(customerId);


                    if (SessionWrapper.Instance.Get != null)
                    {
                        if (SessionWrapper.Instance.Get.UserIdentity == 1)
                        {
                            if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
                            {
                                System.Web.HttpContext.Current.Response.Redirect(DashBoardUri + "?Client=" + SessionWrapper.Instance.Get.ClientFromQueryString.Id + "&Customer=" + customerId);
                                return;
                            }
                        }
                        else
                        {
                            if (client != null && clientBycustomerId != null)
                            {
                                if (client.Id != clientBycustomerId.Id)
                                {
                                    System.Web.HttpContext.Current.Response.Redirect(DashBoardUri + "?Client=" + clientBycustomerId.Id + "&Customer=" + customerId);
                                }
                            }
                        }
                    }
                    else {
                        if (client != null && clientBycustomerId != null)
                        {
                            if (client.Id != clientBycustomerId.Id)
                            {
                                System.Web.HttpContext.Current.Response.Redirect(DashBoardUri + "?Client=" + clientBycustomerId.Id + "&Customer=" + customerId);
                            }
                        }
                    }
                }
                else
                {
                    CffCustomer customer = customerRepository.GetCffCustomerByClientIdAndCustomerId(view.Client.Id, customerId);
                    if (customer != null)
                    {
                        // view.Customer = customer;
                    }
                }
            }
        }

        public void SelectClient(int clientId)
        {
            if (securityManager.CanChangeSelectedClient())
            {
                ICffClient client = clientRepository.GetCffClientByClientId(clientId);
                if (client != null)
                {
                    view.Client = client;
                    view.Customer = null;
                }
            }
        }


        public void SelectCustomerByClientAndCustomer(int clientId, int customerId)
        {
            if (securityManager.CanChangeSelectedCustomer())
            {
                if (securityManager.CanChangeSelectedClient())
                {
                    CffClient client = clientRepository.GetCffClientByCustomerId(customerId);
                    CffCustomer customer = customerRepository.GetCffCustomerByCustomerId(customerId);
                    ICffClient clientByClientId = clientRepository.GetCffClientByClientId(clientId);

                    if (client != null && customer != null && clientByClientId !=null)
                    {
                       // view.Client = clientByClientId;
                        //view.Customer = customer;
                    }
                }
                else
                {
                    CffCustomer customer = customerRepository.GetCffCustomerByClientIdAndCustomerId(view.Client.Id, customerId);
                    if (customer != null)
                    {
                       // view.Customer = customer;
                    }
                }
            }
        }
    }
}