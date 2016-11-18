using System;
using System.Data;
using Cff.SaferTrader.Core;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Repositories
{
    public class CffUserRepository : SqlManager, ICffUserRepository
    {
        public CffUserRepository(string connectionString) : base (connectionString)
        {}

        public ICffUser LoadCffUser(Guid userId)
        {
            ArgumentChecker.ThrowIfGuidEmpty(userId, "userId");
            ICffUser user = null;

            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, 
                                                                            CommandType.StoredProcedure, 
                                                                            "CffUserView_LoadUserByUserId", 
                                                                            CreateUserKeyParameter(userId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    
                    if(!cleverReader.IsNull && cleverReader.Read())
                    {
                        UserType userType = UserType.Parse(cleverReader.ToInteger("UserTypeId"));
                        Guid returnedUserId = cleverReader.ToGuid("UserId");
                        string userName = cleverReader.ToString("UserName");
                        int employeeId = cleverReader.ToInteger("EmployeeId");
                        string displayName = cleverReader.ToString("DisplayName");

                        int clientId = cleverReader.FromBigInteger("ClientId");
                        if(userType == UserType.EmployeeStaffUser)
                        {
                            user = new EmployeeStaffUser(returnedUserId, userName, employeeId, displayName, clientId);
                        }
                        else if (userType == UserType.EmployeeManagementUser)
                        {
                            user = new EmployeeManagementUser(returnedUserId, userName, employeeId, displayName, clientId);
                        }
                        else if (userType == UserType.EmployeeAdministratorUser)
                        {
                            user = new EmployeeAdministratorUser(returnedUserId, userName, employeeId, displayName, clientId);
                        }
                        else if (userType == UserType.ClientStaffUser)
                        {
                            user = new ClientStaffUser(returnedUserId,
                                                     userName,
                                                     employeeId,
                                                     displayName,
                                                     clientId,
                                                     cleverReader.FromBigInteger("ClientNum"),
                                                     cleverReader.ToString("ClientName"));
                        }
                        else if (userType == UserType.ClientManagementUser)
                        {
                            user = new ClientManagementUser(returnedUserId,
                                                     userName,
                                                     employeeId,
                                                     displayName,
                                                     clientId,
                                                     cleverReader.FromBigInteger("ClientNum"),
                                                     cleverReader.ToString("ClientName"));
                        }
                        else if (userType == UserType.CustomerUser)
                        {
                            user = new CustomerUser(returnedUserId,
                                                    userName,
                                                    employeeId,
                                                    displayName,
                                                    cleverReader.FromBigInteger("CustomerId"),
                                                    cleverReader.FromBigInteger("CustNum"),
                                                    cleverReader.ToString("Customer"), 
                                                    clientId);
                        }
                    }
                }
            }
            return user;
        }
        public Boolean AddNewCffUser(NameValueCollection collection, Guid userId)
        {
            Boolean bReturn = false;
            ArgumentChecker.ThrowIfNull(collection, "collection");
            ArgumentChecker.ThrowIfGuidEmpty(userId, "userId");
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "AddNewCffUser", CreateInvestorParameters(userId, collection)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        bReturn = true;
                    }
                }
            }
            return bReturn;
        }

        public Boolean VerifyPasskey(String userPassKey)
        {
            Boolean bReturn = false;
            ArgumentChecker.ThrowIfNull(userPassKey, "userPassKey");
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "VerifyPassKey", CreateUserPassKeyParameter(userPassKey)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        if (cleverReader.ToBoolean("RESULT") == true) 
                            bReturn = true;
                    }
                }
            }
            return bReturn;
        }

        public Int32 VerifyIfSpecialAccount(String username, String password)
        {
            Int32 nReturn = 1;
            ArgumentChecker.ThrowIfNull(username, "username");
            ArgumentChecker.ThrowIfNull(password, "password");
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "VerifyIfSpecialAccount", CreateAccountParameter(username, password)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        nReturn = cleverReader.ToInteger("RESULT");
                    }
                }
            }
            return nReturn;
        }

        public List<UserSpecialAccounts> GetSpecialAccountAccess(string username, string password)
        {
            List<UserSpecialAccounts> userSpecialAccounts = new List<UserSpecialAccounts>();
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "GetSpecialAccountAccess", CreateAccountParameter(username, password)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        Guid uid = cleverReader.ToGuid("UserID");
                        string name = cleverReader.ToString("Name");
                        int isClient = cleverReader.ToInteger("IsClient");
                        bool bisClient = isClient != 0 ? true : false;
                        int id = cleverReader.ToInteger("ID");
                        bool  isLocked = cleverReader.ToBoolean("IsLockedOut");
                        userSpecialAccounts.Add(new UserSpecialAccounts(uid, name, bisClient, id, isLocked));
                    }
                }
            }
            return userSpecialAccounts;
        }

        public String GetRoleByPassKey(String userPassKey)
        {
            ArgumentChecker.ThrowIfNull(userPassKey, "userPassKey");
            String sRole = "Staff";
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "GetRoleByPassKey", CreateUserPassKeyParameter(userPassKey)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        sRole = cleverReader.ToString("ROLE");
                    }
                }
            }
            return sRole;
        }

        public CffUserActivation ActivateUser(Guid uid, String pKey)
        {
            ArgumentChecker.ThrowIfNull(pKey, "pKey");
            ArgumentChecker.ThrowIfGuidEmpty(uid, "uid");
            CffUserActivation record = null;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "ActivateNewUser", CreateActivationParameter(uid, pKey)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        int nStatus = cleverReader.ToInteger("Status");
                        if (nStatus == 1)
                        { //valid - UKey must pickup from customer-id
                            record = new CffUserActivation(cleverReader.ToString("Name"), cleverReader.ToString("MngtEmail"), cleverReader.ToGuid("UKey").ToString(), cleverReader.ToString("USERMAIL"), nStatus);
                        }
                        else
                        { //invalid
                            record = new CffUserActivation("", "", "", "", nStatus);
                        }
                    }
                }
            }
            return record;
        }

        public CffUserActivation ApproveUser(Guid mKey, Guid uKey)
        {
            ArgumentChecker.ThrowIfGuidEmpty(mKey, "mKey");
            ArgumentChecker.ThrowIfGuidEmpty(uKey, "uKey");
            CffUserActivation record = null;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "ApproveUserAccess", CreateAccessActionParameter(mKey, uKey)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        int nStatus = cleverReader.ToInteger("Status");
                        if (nStatus > 0)
                        { //valid - UKey must pickup from customer-id
                            record = new CffUserActivation(cleverReader.ToString("Name"), "", "", cleverReader.ToString("USERMAIL"), nStatus);
                        }
                        else
                        { //invalid
                            record = new CffUserActivation("", "", "", "", nStatus);
                        }
                    }
                }
            }
            return record;
        }

        public CffUserActivation DeclineUser(Guid mKey, Guid uKey)
        {
            ArgumentChecker.ThrowIfGuidEmpty(mKey, "mKey");
            ArgumentChecker.ThrowIfGuidEmpty(uKey, "uKey");
            CffUserActivation record = null;
            String userEmployeeName = "";
            String userEmail = "";

            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "GetEmployeeDetailsByUID", CreateEmployeeParameter(uKey, "ALL")))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        userEmployeeName = cleverReader.ToString("EmployeeName");
                        userEmail = cleverReader.ToString("EmailAddress");
                    }
                }
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "DeclineUserAccess", CreateAccessActionParameter(mKey, uKey)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        int nStatus = cleverReader.ToInteger("Status");
                        if (nStatus >= 0)
                        { //valid - UKey must pickup from customer-id
                            record = new CffUserActivation(userEmployeeName, "", "", userEmail, nStatus);
                        }
                        else
                        { //invalid
                            record = new CffUserActivation("", "", "", "", nStatus);
                        }
                    }
                }
                
            }
            return record;
        }

        public Boolean AcceptAgreement(Guid userId, Boolean? isAccept)
        {
            ArgumentChecker.ThrowIfGuidEmpty(userId, "userId");
            Boolean bRet = false;
            if (isAccept == null)
            {
                // query the agreement
                using (SqlConnection connection = CreateConnection())
                {
                    using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "IsAcceptAgreement", CreateUserKeyParameter(userId)))
                    {
                        CleverReader cleverReader = new CleverReader(dataReader);
                        while (cleverReader.Read())
                        {
                            bRet = cleverReader.ToBoolean("AcceptAgreement");
                        }
                    }
                }
            }
            else
            { 
                // set the agreement
                using (SqlConnection connection = CreateConnection())
                {
                    using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "SetAcceptAgreement", CreateUserAgreementParameter(userId, isAccept)))
                    {
                        CleverReader cleverReader = new CleverReader(dataReader);
                        while (cleverReader.Read())
                        {
                            bRet = cleverReader.ToBoolean("AcceptAgreement");
                        }
                    }
                }
            }
            return bRet;
        }

        public String GetDashboardContent()
        {
            String content = "";
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "GetCFFContent", CreateContentIdparameter(1)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        content = cleverReader.ToString("MsgContent");
                    }
                }
            }
            return content;
        }


        public Int32 ValidateSpecialAccess(String owner, Guid accessId)
        {
            // -1 failed, 1 pass, 0 blocked (we need to limit to 3 times validation and block the access for this user)
            ArgumentChecker.ThrowIfNull(owner, "owner");
            ArgumentChecker.ThrowIfGuidEmpty(accessId, "accessId");
            Int32 nRet = -1;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "ValidateSpecialAccess", CreateSpecialAccountParameter(owner, accessId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        nRet = cleverReader.ToInteger("RESULT");
                    }
                }
            }
            return nRet;
        }

        public CffLoginAccount GetSpecialAccessAccount(String owner, Guid accessId)
        {
            ArgumentChecker.ThrowIfNull(owner, "owner");
            ArgumentChecker.ThrowIfGuidEmpty(accessId, "accessId");

            CffLoginAccount account = null;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "GetSpecialMapAccount", CreateSpecialAccountParameter(owner, accessId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        account = new CffLoginAccount(cleverReader.ToString("USERNAME"), cleverReader.ToString("PASSWORD"));
                    }
                }
            }
            return account;
        }

        public Boolean ChangeEmployeePassword(Guid uid, String newPassword)
        {
            Boolean bRet = false;
            ArgumentChecker.ThrowIfGuidEmpty(uid, "uid");
            ArgumentChecker.ThrowIfNull(newPassword, "newPassword");
            using (SqlConnection connection = CreateConnection())
            {
                SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "ChangeEmployeePassword", CreateChangePassParameter(uid, newPassword));
                if (dataReader != null)
                    bRet = true;
                else
                    bRet = false;
            }
            return bRet;
        }
        private SqlParameter[] CreateChangePassParameter(Guid uid, String newPassword)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = uid },
                new SqlParameter("@NewPassword", SqlDbType.VarChar) { Value = newPassword }
            };
            return sqlParams;
        }
        private SqlParameter[] CreateEmployeeParameter(Guid uid, String action)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = uid },
                new SqlParameter("@sAction", SqlDbType.VarChar) { Value = action }
            };
            return sqlParams;
        }

        private SqlParameter[] CreateAccessActionParameter(Guid mKey, Guid uKey)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@mkey", SqlDbType.UniqueIdentifier) { Value = mKey },
                new SqlParameter("@ukey", SqlDbType.UniqueIdentifier) { Value = uKey }
            };
            return sqlParams;
        }

        private static SqlParameter[] CreateActivationParameter(Guid uid, String pKey)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@uid", SqlDbType.UniqueIdentifier) { Value = uid },
                new SqlParameter("@passkey", SqlDbType.VarChar) { Value = pKey }
            };
            return sqlParams;
        }

        private static SqlParameter[] CreateUserAgreementParameter(Guid userId, bool? isAccept)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = userId },
                new SqlParameter("@isAccept", SqlDbType.Bit) { Value = isAccept }
            };
            return sqlParams;
        }

        private static SqlParameter[] CreateAccountParameter(String username, String password)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@username", SqlDbType.VarChar) { Value = username },
                new SqlParameter("@password", SqlDbType.VarChar) { Value = password }
            };
            return sqlParams;
        }

        private static SqlParameter CreateUserKeyParameter(Guid userId)
        {
            SqlParameter userKeyParameter = new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) {Value = userId};
            return userKeyParameter;
        }

        private static SqlParameter CreateUserPassKeyParameter(String userPassKey)
        {
            SqlParameter userKeyParameter = new SqlParameter("@PassKey", SqlDbType.VarChar) { Value = userPassKey };
            return userKeyParameter;
        }

        private static SqlParameter[] CreateInvestorParameters(Guid userId, NameValueCollection collection)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@EmployeeName", SqlDbType.VarChar) { Value = collection["FullName"] },
                new SqlParameter("@Password", SqlDbType.VarChar) { Value = collection["Password"] },
                new SqlParameter("@Signature", SqlDbType.VarChar) { Value = collection["Signature"] },
                new SqlParameter("@EmailAddress", SqlDbType.VarChar) { Value = collection["Email"] },
                new SqlParameter("@UserId", SqlDbType.UniqueIdentifier) { Value = userId },
                new SqlParameter("@UserTypeId", SqlDbType.Int) { Value = Convert.ToInt16(collection["UserTypeId"]) }
            };
            return sqlParams;
        }

        private static SqlParameter[] CreateSpecialAccountParameter(String username, Guid accessId)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@User", SqlDbType.VarChar) { Value = username },
                new SqlParameter("@AccessId", SqlDbType.VarChar) { Value = accessId.ToString() }
            };
            return sqlParams;
        }

        private static SqlParameter CreateContentIdparameter(Int32 contentId)
        {
            SqlParameter IdParameter = new SqlParameter("@ContentId", SqlDbType.Int);
            IdParameter.Value = contentId;
            
            return IdParameter;
        }
    }
}
