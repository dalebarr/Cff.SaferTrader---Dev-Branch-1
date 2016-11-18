using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Views
{
    public interface IContactsView : IRedirectableView
    {
        void DisplayClientContacts(IList<ClientContact> clientContacts);
        void DisplayCustomerContacts(IList<CustomerContact> customerContacts);
        void ClearClientSearchTextBox();
        void ShowAllClientsView();
        void ShowCustomerView();
        void ShowClientView();
        void ClearCustomerSearchTextBox();
        CffPrincipal CurrentPrincipal { get; }
    }
}
