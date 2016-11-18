using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System;

namespace Cff.SaferTrader.Core.Repositories
{
    public class ClientRepository : SqlManager, IClientRepository
    {
        public ClientRepository(string connectionString) : base(connectionString)
        {
        }

        #region IClientRepository Members

        public int GetCffDebtorAdmin(int clientid)   // added by dbb
        {
            int clientDebtorAdminStatus = 0;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure,
                    "stUpdateClientDebtorAdminStatus",
                    CreateClientDebtorAdminStatusParameters("SearchClientID", clientid, "", 0, 0)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    if (!cleverReader.IsNull)
                    {
                        while (cleverReader.Read())
                        {
                            clientDebtorAdminStatus = cleverReader.ToSmallInteger("IsCffAdministered");
                        }
                    }
                }
            }
            return clientDebtorAdminStatus;
        }

        private static SqlParameter[] CreateClientDebtorAdminStatusParameters(string actionType, int clientid,
            string clientName, int isAdminByCff, int hasLetterTemplates)     // added by dbb                   
        {
            var actionTypeParam = new SqlParameter("@actionType", SqlDbType.NVarChar, 32);
            var clientidParam = new SqlParameter("@clientID", SqlDbType.BigInt);
            var clientNameParam = new SqlParameter("@clientName", SqlDbType.NVarChar, 200);
            var isAdminByCffParam = new SqlParameter("@isAdminByCff", SqlDbType.Bit);
            var hasLetterTemplatesParam = new SqlParameter("@hasLetterTemplates", SqlDbType.Bit);

            actionTypeParam.Value = actionType;
            clientidParam.Value = clientid;
            clientNameParam.Value = clientName;
            isAdminByCffParam.Value = isAdminByCff;
            hasLetterTemplatesParam.Value = hasLetterTemplates;

            return new[] {actionTypeParam, clientidParam, clientNameParam, isAdminByCffParam, hasLetterTemplatesParam};
        }

        public string LoadMatchedClientNameAndNum(string searchString, int numberOfCustomersToReturn)
        {
            IList<ICffClient> clientsList = new List<ICffClient>();

            using (SqlConnection connection = CreateConnection())
            {
                using (
                    SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure,
                                                                       "GetMatchedClient",
                                                                       CreateShowMatchedNamesParameters(searchString,
                                                                                                        numberOfCustomersToReturn))
                    )
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    if (!cleverReader.IsNull)
                    {
                        while (cleverReader.Read())
                        {
                            if (cleverReader.FromBigInteger("ClientID") == -1)
                            {
                                clientsList.Add(AllClients.Create());
                            }
                            else
                            {
                                clientsList.Add(new CffClient(cleverReader.ToString("ClientName"),
                                                  cleverReader.FromBigInteger("ClientID"),
                                                  cleverReader.FromBigInteger("ClientNum"),
                                                  cleverReader.ToSmallInteger("FacilityType"),
                                                  cleverReader.ToString("CollectionsBankAccount"),
                                                  cleverReader.ToSmallInteger("CFFDebtorAdmin"),  //MSazra [20151006]
                                                  cleverReader.ToBoolean("ClientHasLetterTemplates")  //MSazra [20151006]
                                                  ));
                            }
                        }
                    }
                }
            }

            return GenreateJSONForClient(clientsList); 
        }

        public IList<ICffClient> GetClientNameAndNum(string searchString, int numberOfCustomersToReturn)
        {
            IList<ICffClient> clientsList = new List<ICffClient>();

            using (SqlConnection connection = CreateConnection())
            {
                using (
                    SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure,
                                                                       "GetMatchedClient",
                                                                       CreateShowMatchedNamesParameters(searchString,
                                                                                                        numberOfCustomersToReturn))
                    )
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    if (!cleverReader.IsNull)
                    {
                        while (cleverReader.Read())
                        {
                            if (cleverReader.FromBigInteger("ClientID") == -1)
                            {
                                clientsList.Add(AllClients.Create());
                            }
                            else
                            {
                                clientsList.Add(new CffClient(cleverReader.ToString("ClientName"),
                                                  cleverReader.FromBigInteger("ClientID"),
                                                  cleverReader.FromBigInteger("ClientNum"),
                                                  cleverReader.ToSmallInteger("FacilityType"),
                                                  cleverReader.ToString("CollectionsBankAccount"),
                                                  cleverReader.ToSmallInteger("CFFDebtorAdmin"),  //MSazra [20151006]
                                                  cleverReader.ToBoolean("ClientHasLetterTemplates") //MSazra [20151006]
                                                  ));
                            }
                        }
                    }
                }
            }

            return clientsList;
        }

        private static string GenreateJSONForClient(ICollection<ICffClient> clientsList)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append("[");
            int x = 0;

            foreach (ICffClient client in clientsList)
            {
                if (client.Id != -1)
                {
                    if (x > 0) {
                        stringBuilder.Append(","); 
                    }

                    stringBuilder.Append(client.NameAndNumberJSON());
                    x=1;
                }
            }

            if (clientsList.Count != 0)
            {
                if (x > 0) { 
                    stringBuilder.Append(","); 
                }

                string allClientSelection = "{\"label\": \"All Clients\", \"value\": \"-1\"}";
                /* string allClientSelection = "{\"clientName\":" + "<span class=\"AllClients\">All Clients</span>" +
                                            "\", \"clientId\": \"" +
                                            -1 +
                                            "\"}";*/

                stringBuilder.Append(allClientSelection);
            }

            return stringBuilder.ToString();
        }

        public CffClient GetCffClientByCustomerId(int customerId)
        {
            CffClient cffClient = null;
            using (SqlConnection connection = CreateConnection())
            {
                using (
                    SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure,
                                                                       "Customers_GetClientNameID",
                                                                       CreateCustomerIdParameter(customerId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    if (!cleverReader.IsNull && cleverReader.Read())
                    {
                        cffClient = new CffClient(cleverReader.ToString("ClientName"),
                                                  cleverReader.FromBigInteger("ClientID"),
                                                  cleverReader.FromBigInteger("ClientNum"),
                                                  cleverReader.ToSmallInteger("FacilityType"),
                                                  cleverReader.ToString("CollectionsBankAccount"),
                                                  cleverReader.ToSmallInteger("CFFDebtorAdmin"),
                                                  cleverReader.ToBoolean("ClientHasLetterTemplates")    //MSazra [20151006]
                                                  );
                    }
                }
            }
            return cffClient;
        }

        public ICffClient GetCffClientByClientId(int clientId)
        {
            ICffClient cffClient = null;
            using (SqlConnection connection = CreateConnection())
            {
                using (
                    SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure,
                                                                       "Client_GetClientByCleintID",
                                                                       CreateClientIdParameter(clientId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    if (!cleverReader.IsNull && cleverReader.Read())
                    {
                        if (cleverReader.FromBigInteger("ClientID") == -1)
                        {
                            cffClient = AllClients.Create();
                        }
                        else
                        {
                            cffClient = new CffClient(cleverReader.ToString("ClientName"),
                                                      cleverReader.FromBigInteger("ClientID"),
                                                      cleverReader.FromBigInteger("ClientNum"),
                                                      cleverReader.ToSmallInteger("FacilityType"),
                                                      cleverReader.ToString("CollectionsBankAccount"),
                                                      cleverReader.ToSmallInteger("CFFDebtorAdmin"),        //MSazra [20151006]
                                                      cleverReader.ToBoolean("ClientHasLetterTemplates")    //MSazra [20151006]
                                                      );
                        }
                    }
                }
            }
            return cffClient;
        }

        #endregion

        private static SqlParameter[] CreateCustomerIdParameter(long customerId)
        {
            var customerIdParameter = new SqlParameter("@CustomerID", SqlDbType.BigInt);
            customerIdParameter.Value = customerId;
            return new[] {customerIdParameter};
        }

        private static SqlParameter[] CreateClientIdParameter(long customerId)
        {
            var customerIdParameter = new SqlParameter("@CleintID", SqlDbType.BigInt);
            customerIdParameter.Value = customerId;
            return new[] {customerIdParameter};
        }

        private static SqlParameter[] CreateShowMatchedNamesParameters(string matchString, int numberOfCustomersToReturn)
        {
            var matchStringParameter = new SqlParameter("@MatchString", SqlDbType.NVarChar, 40);
            var numberOfCustomersToReturnParameter = new SqlParameter("@numberOfCustomersToReturn", SqlDbType.Int);

            matchStringParameter.Value = matchString;
            numberOfCustomersToReturnParameter.Value = numberOfCustomersToReturn;

            return new[] {matchStringParameter, numberOfCustomersToReturnParameter};
        }        
    }
}