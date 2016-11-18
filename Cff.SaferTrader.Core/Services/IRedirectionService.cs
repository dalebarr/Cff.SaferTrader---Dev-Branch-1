namespace Cff.SaferTrader.Core.Services
{
    public interface IRedirectionService
    {
        void RedirectToInvoiceBatches(int batch);
        void SelectClientAndRedirectToDashboard(int clientId);
        void SelectClientCustomerAndRedirectToDashboard(int clientId, int customerId);
        void SelectClientCustomerAndRedirectToInvoiceBatches(int clientId, int customerId, int batch);
        void RedirectToTransactionSearch();
        void RedirectToTransactionSearch(string queryString);
        void SelectDefaultAssociationAndRedirectToDashboard(ICffUser user);
    }
}