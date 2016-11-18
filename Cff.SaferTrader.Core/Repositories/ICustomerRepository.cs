using System;
using Microsoft.Office.Interop.Word;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Repositories
{
    public interface ICustomerRepository
    {
        string GetMatchedCustomersJSON(string matchString, long clientId, int numberOfCustomersToReturn, int criteria);
        ClientAndCustomerContacts GetCustomerClientContact(int customerId);
        ClientAndCustomerInformation GetMatchedCustomerInfo(int customerId, int clientId);
        ClientContact GetClientContactDetails(int clientId);
        void UpdateCustomerCallDue(Date nextCallDue, int callDueCheckPriority, int customerId, Date customerLastModified, int modifiedBy);
        void InsUpdateCustomerInformation(string sAction, int clientId, int CustNumber, System.Int16 stopCredit, decimal creditLimit, 
                                            System.DateTime pNextCallDue, System.Int16 allowcalls, System.DateTime listdate, System.Int16 terms,
                                                string companyID, decimal GSTvalue, System.DateTime dtModified, int iModifBy);
        CffCustomer GetCffCustomerByCustomerId(int customerId);
        ICffCustomer GetCffCustomerByCustomerIdNew(int customerId);
        ClientInformationAndAgeingBalances GetMatchedClientInfo(int clientId);
        CffCustomer GetCffCustomerByClientIdAndCustomerId(int clientId, int customerId);
        bool CheckCustomerBelongsToClient(int clientId, int customerId);
        bool CheckClientBelongToUser(int clientId, Guid userId);
        string GetPasskey(long clientId);
        Decimal GetCurrentACLimit(long clientId, int userId, DateTime date);
        List<Decimal> GetCurrentACLimitExt(long clientId, int userId, DateTime date);
    }
}
