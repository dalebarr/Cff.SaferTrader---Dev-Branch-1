using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System;

namespace Cff.SaferTrader.Core.Repositories
{
    public class UserClientsRepository : SqlManager, IUserClientsRepository
    {
        public UserClientsRepository(string connectionString)
            : base(connectionString)
        {
        }

        public Int32 VerifyIfSpecialAccountByID(int userId)
        {
            Int32 nReturn = 1;
            ArgumentChecker.ThrowIfNull(userId, "userId");
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "VerifyIfSpecialAccountByID", CreateUserIdParameter(userId)))
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

        public List<UserSpecialAccounts> GetSpecialAccountAccessByID(int userId)
        {
            // continue tomorrow - request to db all clients for this user
            ArgumentChecker.ThrowIfNull(userId, "userId");
            List<UserSpecialAccounts> userSpecialAccounts = new List<UserSpecialAccounts>();
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "GetSpecialAccountAccessByUserId", CreateUserIdParameter(userId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        Guid uid = cleverReader.ToGuid("UserID");
                        string name = cleverReader.ToString("Name");
                        int isClient = cleverReader.ToInteger("IsClient");
                        bool bisClient = isClient != 0 ? true : false;
                        Int64 id = cleverReader.FromBigInteger("ID");
                        bool bisLocked =  cleverReader.ToBoolean("IsLockedOut");
                        if (!bisLocked)
                        { //add on dropdownlist if not locked out
                            userSpecialAccounts.Add(new UserSpecialAccounts(uid, name, bisClient, id, bisLocked));
                        }
                    }
                }
            }
            return userSpecialAccounts;
        }

        private static SqlParameter[] CreateUserIdParameter(int userId)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@userId", SqlDbType.Int) { Value = userId }
            };
            return sqlParams;
        }
    }
}