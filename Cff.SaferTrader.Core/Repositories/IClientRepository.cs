using System.Collections;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Repositories
{
    public interface IClientRepository
    {
        string LoadMatchedClientNameAndNum(string searchString, int numberOfCustomersToReturn);
        CffClient GetCffClientByCustomerId(int customerId);
        ICffClient GetCffClientByClientId(int clientId);
        IList<ICffClient> GetClientNameAndNum(string searchString, int numberOfCustomersToReturn);
        int GetCffDebtorAdmin(int clientid);   // added by dbb
    }

    public interface IUserClientsRepository
    {
        System.Int32 VerifyIfSpecialAccountByID(int userId);
        List<UserSpecialAccounts> GetSpecialAccountAccessByID(int userId);
    }
}

