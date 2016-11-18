using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Repositories
{
    public interface IContactsRepository
    {
        IList<ClientContact> LoadAClientsContacts(int clientId);
        IList<ClientContact> LoadAClientsContacts(int clientId, string sAction);   //Dbb [20160726]
        IList<CustomerContact> LoadACustomersContacts(int customerId);
        CustomerContact LoadTheDefaultCustomerContact(int customerId);  //MSarza [20150819]
        IList<ClientContact> LoadAllClientsAndTheirContacts();
        IList<CustomerContact> LoadAllCustomersAndTheirContacts();
        IList<CustomerContact> LoadAllCustomersContactsForAClient(int clientId);
        IList<ClientContact> LoadMatchedAllClientsContacts(string textToMatch);
        IList<ClientContact> LoadMatchedClientsContactsForAClient(string textToMatch, int clientId);
        IList<CustomerContact> LoadMatchedAllClientsCustomerContact(string textToMatch);
        IList<CustomerContact> LoadMatchedCustomerContactForAClient(string textToMatch, int clientId);
        IList<CustomerContact> LoadMatchedCustomerContactForACustomer(string textToMatch, int customerId);
        IList<ClientContact> LoadClientsContactsWithClientNameStartWith(string letter);
        IList<CustomerContact> LoadAllClientsCustomerContactsWithCustomerNameStartWith(string letter);
        IList<CustomerContact> LoadCustomerContactsForAClientWithCustomerNameStartWith(string letter, int clientId);
        IList<CustomerContact> LoadCustomerContactsForACustomerWithCustomerNameStartWith(string letter, int customerId);

        bool UpdateClientContactDetails(int cliID, ClientContact clientContactDetails);
        bool UpdateCustomerContactDetails(int cliID, CustomerContact customerContactDetails);
        bool InsCustContactInfoDetailsForValidation(int cliID, CustomerContact custContactInfoDetails);
        bool InsertClientContactInfoDetailsForValidation(int cliID, ClientContact clientContactDetails);
    }
}
