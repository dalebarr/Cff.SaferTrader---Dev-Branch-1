using System;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Services
{
    /// <summary>
    /// Redirects a redirectable view
    /// </summary>
    public class RedirectionService : IRedirectionService
    {
        private readonly IClientRepository clientRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly ISecurityManager securityManager;
        private readonly IRedirectableView view;

        public RedirectionService(IRedirectableView view, IClientRepository clientRepository, ICustomerRepository customerRepository, 
            ISecurityManager securityManager)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(clientRepository, "clientRepository");
            ArgumentChecker.ThrowIfNull(customerRepository, "customerRepository");
            ArgumentChecker.ThrowIfNull(securityManager, "securityManager");

            this.view = view;
            this.clientRepository = clientRepository;
            this.customerRepository = customerRepository;
            this.securityManager = securityManager;
        }

        public void SelectClientCustomerAndRedirectToDashboard(int clientId, int customerId)
        {
            if (securityManager.CanChangeSelectedCustomer())
            {
                ICffClient client = clientRepository.GetCffClientByClientId(clientId);
                CffCustomer customer = customerRepository.GetCffCustomerByCustomerId(customerId);

                view.Client = client;
                view.Customer = (ICffCustomer)customer;
                view.RedirectTo(DashboardUrl + "?Client=" + clientId + "&Customer=" + customerId + "&ViewID=" + QueryString.ViewIDValue);
            }
        }

        public void SelectClientAndRedirectToDashboard(int clientId)
        {
            if (securityManager.CanChangeSelectedClient())
            {
                ICffClient client = clientRepository.GetCffClientByClientId(clientId);
                if (client == null)
                {
                    throw new ArgumentException("Cannot select null client");
                }
                view.Client = client;
                view.RedirectTo(DashboardUrl + "?Client=" + clientId + "&ViewID=" + QueryString.ViewIDValue);
            }
        }

        public void SelectClientCustomerAndRedirectToInvoiceBatches(int clientId, int customerId, int batch)
        {
            if (securityManager.CanViewReleaseTab())
            {
                ICffClient client = clientRepository.GetCffClientByClientId(clientId);
                CffCustomer customer = customerRepository.GetCffCustomerByCustomerId(customerId);

                view.Client = client;
                view.Customer = (ICffCustomer)customer;
                RedirectToInvoiceBatchesWithClientCustomer(clientId, customerId, batch);
            }
        }

        public void RedirectToTransactionSearch()
        {
            view.RedirectTo(string.Format("~/{0}", Config.TransactionSearchPage));
        }

        public void RedirectToTransactionSearch(string queryString)
        {
            view.RedirectTo(string.Format("~/{0}{1}", Config.TransactionSearchPage, queryString));
        }

        public void SelectDefaultAssociationAndRedirectToDashboard(ICffUser user)
        {
            string path = string.Format("~/{0}", Config.DashboardPage);
            if (user.UserType == UserType.CustomerUser)
            {
                CustomerUser customerUser = (CustomerUser) user;
                path = string.Format("{0}?Client={1}&Customer={2}&ViewID={3}", path, customerUser.ClientId, customerUser.CustomerId, QueryString.ViewIDValue);
            }
            else
            {
                //QueryString.ClientId
                path = string.Format("{0}?Client={1}&ViewID={2}", path, user.ClientId, QueryString.ViewIDValue);
            }
            view.RedirectTo(path);
        }

        public void RedirectToInvoiceBatches(int batch)
        {
            view.RedirectTo(GenerateInvoiceBatchesUrl(batch));
        }

        private void RedirectToInvoiceBatchesWithClientCustomer(int clientId, int customerId, int batch)
        {
            view.RedirectTo(GenerateInvoiceBatchesUrlWithClientCustomer(clientId, customerId, batch));
        }

        private static string GenerateInvoiceBatchesUrl(int batch)
        {
            //return string.Format("~/{0}?Batch={1}", Config.BatchDetailsPage, batch);
            return string.Format("{0}?Batch={1}&ViewID={2}", "~/" + Config.BatchDetailsPage, batch, QueryString.ViewIDValue);
        }

        private static string GenerateInvoiceBatchesUrlWithClientCustomer(int clientId, int customerId, int batch)
        {
            //return string.Format("~/{0}?Client={1}&Customer={2}&Batch={3} ", Config.BatchDetailsPage,clientId,customerId, batch);
            return string.Format("{0}?Client={1}&Customer={2}&Batch={3}&ViewID={4} ", "~/" + Config.BatchDetailsPage, clientId, customerId, batch, QueryString.ViewIDValue);
        }

        private static string DashboardUrl
        {
            //get { return string.Format("~/{0}", Config.DashboardPage); }
            get { 
                return string.Format("{0}", Config.DashboardPage); 
            }
        }

        public static IRedirectionService Create(IRedirectableView view, ISecurityManager securityManager)
        {
            return new RedirectionService(view, RepositoryFactory.CreateClientRepository(), RepositoryFactory.CreateCustomerRepository(), securityManager);
        }
    }
}