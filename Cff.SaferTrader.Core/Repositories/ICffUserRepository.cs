using System;
using System.Collections.Specialized;
using Cff.SaferTrader.Core;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Repositories
{
    public interface ICffUserRepository
    {
        ICffUser LoadCffUser(Guid userId);
        Boolean AddNewCffUser(NameValueCollection collection, Guid userId);
        Boolean VerifyPasskey(String userPassKey);
        Int32 VerifyIfSpecialAccount(String username, String password);
        List<UserSpecialAccounts> GetSpecialAccountAccess(String username, String password);
        String GetRoleByPassKey(String userPassKey);
        CffUserActivation ActivateUser(Guid sUniqueId, String pKey);
        CffUserActivation ApproveUser(Guid mKey, Guid uKey);
        CffUserActivation DeclineUser(Guid mKey, Guid uKey);
        Boolean AcceptAgreement(Guid userId, Boolean? Accept);
        String GetDashboardContent();
        Int32 ValidateSpecialAccess(String user, Guid accessId);
        CffLoginAccount GetSpecialAccessAccount(String user, Guid accessId);
        Boolean ChangeEmployeePassword(Guid uid, String newPassword);
    }
}
