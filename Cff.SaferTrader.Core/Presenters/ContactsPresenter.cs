using System.Collections.Generic;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class ContactsPresenter
    {
        private readonly IContactsView view;
        private readonly IContactsRepository contactsRepository;

        public ContactsPresenter(IContactsView view, IContactsRepository contactsRepository)
        {
            ArgumentChecker.ThrowIfNull(contactsRepository, "contactsRepository");

            this.view = view;
            this.contactsRepository = contactsRepository;
        }

        public static ContactsPresenter Create(IContactsView view)
        {
            return new ContactsPresenter(view, RepositoryFactory.CreateContactsRepository());
        }

        public void LoadACustomersContacts(int customerId)
        {
            IList<CustomerContact> aCustomersContacts = contactsRepository.LoadACustomersContacts(customerId);
            view.DisplayCustomerContacts(aCustomersContacts);
        }

        public void LoadAClientContacts(int clientId)
        {
            IList<ClientContact> aClientsContacts = contactsRepository.LoadAClientsContacts(clientId);
            view.DisplayClientContacts(aClientsContacts);
        }

        public void LoadAClientContacts(int clientId, string sAction)      // dbb [20160726]
        {
            IList<ClientContact> aClientsContacts = contactsRepository.LoadAClientsContacts(clientId, sAction);
            view.DisplayClientContacts(aClientsContacts);
        }

        public void LoadAllClientsContacts()
        {
            IList<ClientContact> aClientsContacts = contactsRepository.LoadAllClientsAndTheirContacts();
            view.DisplayClientContacts(aClientsContacts);
        }

        public void LoadAllCustomersContacts()
        {
            IList<CustomerContact> aClientsContacts = contactsRepository.LoadAllCustomersAndTheirContacts();
            view.DisplayCustomerContacts(aClientsContacts);
        }

        public void LoadAllCustomersContactsForAClient(int clientId)
        {
            IList<CustomerContact> clientsContacts = contactsRepository.LoadAllCustomersContactsForAClient(clientId);
            view.DisplayCustomerContacts(clientsContacts);
        }

        public void SearchAllClientsContact(string textToMatch)
        {
            IList<ClientContact> aClientsContacts = contactsRepository.LoadMatchedAllClientsContacts(textToMatch);
            view.DisplayClientContacts(aClientsContacts);
        }

        public void SearchAClientsContact(string textToMatch, int clientId)
        {
            IList<ClientContact> aClientsContacts = contactsRepository.LoadMatchedClientsContactsForAClient(textToMatch, clientId);
            view.DisplayClientContacts(aClientsContacts);
        }

        public void SearchAllClientsCustomerContact(string textToMatch)
        {
            IList<CustomerContact> customerContact = contactsRepository.LoadMatchedAllClientsCustomerContact(textToMatch);
            view.DisplayCustomerContacts(customerContact);
        }

        public void SearchAClientsCustomerContact(string textToMatch, int clientId)
        {
            IList<CustomerContact> customerContact = contactsRepository.LoadMatchedCustomerContactForAClient(textToMatch, clientId);

            view.DisplayCustomerContacts(customerContact);
        }

        public void SearchCustomerContactForACustomer(string textToMatch, int customerId)
        {
            IList<CustomerContact> customerContact = contactsRepository.LoadMatchedCustomerContactForACustomer(textToMatch, customerId);
            view.DisplayCustomerContacts(customerContact);
        }

        public void LoadClientsContactsWithClientNameStartWith(string letter)
        {
            IList<ClientContact> clientContacts = contactsRepository.LoadClientsContactsWithClientNameStartWith(letter);
            view.DisplayClientContacts(clientContacts);
            view.ClearClientSearchTextBox();
        }

        public void InitializeForScope(Scope scope)
        {
            if (scope == Scope.AllClientsScope)
            {
                view.ShowAllClientsView();
            }
            else if (scope == Scope.CustomerScope)
            {
                view.ShowCustomerView();
            }
            else if (scope == Scope.ClientScope)
            {
                view.ShowClientView();
            }
        }

        public void LoadCustomerContactsForACustomerWithCustomerNameStartWith(string letter, int customerId)
        {
            view.DisplayCustomerContacts(contactsRepository.LoadCustomerContactsForACustomerWithCustomerNameStartWith(letter, customerId));
            view.ClearCustomerSearchTextBox();
        }

        public void LoadCustomerContactsForAClientWithCustomerNameStartWith(string letter, int clientId)
        {
            view.DisplayCustomerContacts(contactsRepository.LoadCustomerContactsForAClientWithCustomerNameStartWith(letter,clientId));
            view.ClearCustomerSearchTextBox();
        }

        public void LoadAllClientsCustomerContactstWithCustomerNameStartWith(string letter)
        {
            view.DisplayCustomerContacts(contactsRepository.LoadAllClientsCustomerContactsWithCustomerNameStartWith(letter));
            view.ClearCustomerSearchTextBox();
        }
    }
}