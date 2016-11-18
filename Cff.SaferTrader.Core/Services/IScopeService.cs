namespace Cff.SaferTrader.Core.Services
{
    public interface IScopeService
    {
        void SelectCustomer(int customerId);
        void SelectClient(int clientId);
        void SelectCustomerByClientAndCustomer(int clientId, int customerId);
        void CheckClientAndCustomerMatchAndSelectCustomer(int clientId, int customerId);
        void SelectCustomerClientAtCustomerScope(int clientId, int customerId);
    }
}