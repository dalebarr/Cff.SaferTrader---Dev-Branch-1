using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Cff.SaferTrader.Core.Builders;
using DataEntry;


namespace Cff.SaferTrader.Core.Repositories
{
    public class CustomerRepository : SqlManager, ICustomerRepository
    {
        private readonly DataEntry.Clients clients;
        public CustomerRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void UpdateCustomerCallDue(Date nextCallDue, int callDueCheckPriority, int customerId,
                                          Date customerLastModified, int modifiedBy)
        {
            ArgumentChecker.ThrowIfNull(nextCallDue, "nextCallDue");
            ArgumentChecker.ThrowIfNull(customerLastModified, "customerLastModified");
            using (SqlConnection connection = CreateConnection())
            {
                SqlHelper.ExecuteNonQuery(connection,
                                          CommandType.StoredProcedure,
                                          "stUpdateCustomerCallDue",
                                          CreateUpdateCustomerCallDueParameters(nextCallDue,
                                                                                callDueCheckPriority,
                                                                                customerId,
                                                                                customerLastModified,
                                                                                modifiedBy));
            }
        }

        public void InsUpdateCustomerInformation(string sAction, int ClientId, int CustID, Int16 stopCredit, decimal creditLimit, System.DateTime pNextCallDue,
                                                    Int16 allowcalls, System.DateTime listdate, Int16 terms, string companyID, decimal GSTvalue, System.DateTime dtModified, int iModifBy)
        {
            using (SqlConnection connection = CreateConnection())
            {
                SqlHelper.ExecuteNonQuery(connection,
                                          CommandType.StoredProcedure,
                                          "stInsUpdateCustInfoForValidation",
                                          CreateInsUpdateCustomerInfoParameters(sAction, ClientId, CustID, stopCredit, creditLimit, pNextCallDue,
                                                    allowcalls, listdate, terms, companyID, GSTvalue, dtModified, iModifBy));
            }
        }


        public ICffCustomer GetCffCustomerByCustomerIdNew(int customerId)
        {
            ICffCustomer customer = null;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "Customers_GetCustomerByID",
                                                                          CreateCustomerIdParameter(customerId, false)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    if (!cleverReader.IsNull && cleverReader.Read())
                    {
                        customer = new CffCustomerBuilderNew(cleverReader).BuildNew();
                    }
                }
            }
            return customer;
        }

        public CffCustomer GetCffCustomerByCustomerId(int customerId)
        {
            CffCustomer customer = null;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "Customers_GetCustomerByID",
                                                                          CreateCustomerIdParameter(customerId, false)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    if (!cleverReader.IsNull && cleverReader.Read())
                    {
                        customer = new CffCustomerBuilder(cleverReader).Build();
                    }
                }
            }
            return customer;
        }

        public CffCustomer GetCffCustomerByClientIdAndCustomerId(int clientId, int customerId)
        {
            CffCustomer customer = null;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "Customers_GetCustomerByClientAndCustomerID",
                                                                          CreateClientAndCustomerIdParameters(clientId, customerId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    if (!cleverReader.IsNull && cleverReader.Read())
                    {
                        customer = new CffCustomerBuilder(cleverReader).Build();
                    }
                }
            }
            return customer;
        }

        public bool CheckCustomerBelongsToClient(int clientId, int customerId)
        {
            if (clientId == AllClients.Create().Id)
            {
                return true;
            }
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "CheckCustomerBelongsToClient",
                                                                          CreateClientAndCustomerIdParameters(clientId, customerId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    if (!cleverReader.IsNull && cleverReader.Read())
                    {
                        return cleverReader.ToBoolean("Result");
                    }
                }

            }
            return false;
        }

        public bool CheckClientBelongToUser(int clientId, Guid userId)
        {
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "CheckClientBelongToUser",
                                                                          CreateClientIdAndUserUIDParameters(clientId, userId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    if (!cleverReader.IsNull && cleverReader.Read())
                    {
                        return cleverReader.ToBoolean("Result");
                    }
                }

            }
            return false;
        }

        public string GetMatchedCustomersJSON(string matchString, long clientId, int numberOfCustomersToReturn, int criteria)
        {
            List<CffCustomer> customers = new List<CffCustomer>();
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "stGetCustomersSearch",
                                                                          ExCreateGetMatchedCustomerParameters(matchString, clientId, criteria)
                                                                          ))
                {
                    int ix = 0;

                    //System.Diagnostics.Debug.Write("Call to GetMatchedCustomersJSON by: ");
                    //System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                    //System.Diagnostics.Debug.WriteLine(stackTrace.GetFrame(1).GetMethod().Name);

                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (!cleverReader.IsNull && cleverReader.Read() && ix <= numberOfCustomersToReturn)
                    {
                        // MSarza: Line below  causes issues at the CleverReader class due to differing field names passed. Providing a conditional
                        //          code herein to identify and correct the field name appropriately proved to be buggy, hence, fix was instead done on the 
                        //          stored procedure stGetCustomersSearch.
                        customers.Add(new CffCustomerBuilder(cleverReader).Build()); 
                            // MSarza: deprecated conditional code initially applied to fix issue on the preceeding line
                            //if (matchString == "%" && criteria == 0)
                            //{
                            //    //System.Diagnostics.Debug.WriteLine("Using Build2()");
                            //    customers.Add(new CffCustomerBuilder(cleverReader).Build2());
                            //}
                            //else
                            //{
                            //    //System.Diagnostics.Debug.WriteLine("Using Build()");
                            //    customers.Add(new CffCustomerBuilder(cleverReader).Build());
                            //}
                        ix++;
                    }

                    //System.Diagnostics.Debug.WriteLine("Called stGetCustomersSearch matchString: " + matchString + ", clientId: " + clientId.ToString() + ", criteria: " + criteria.ToString());

                }
            }
            return GenerateJSONForClient(customers);
        }

        private static string GenerateJSONForClient(IEnumerable<CffCustomer> matchedCustomers)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int ix = 0;
            //System.Diagnostics.Debug.WriteLine("Prior string builder: " + DateTime.Now);
            foreach (CffCustomer clientCustomer in matchedCustomers)
            {
                if (ix > 0) { stringBuilder.Append(","); }
                stringBuilder.Append(clientCustomer.NameAndNumberJSON());
                ix=1;
            }
            //System.Diagnostics.Debug.WriteLine("After string builder returning: " + ix.ToString() + " customers! - " + DateTime.Now);
            return stringBuilder.ToString();
        }

        public ClientAndCustomerContacts GetCustomerClientContact(int customerId)
        {
            if (customerId == int.MinValue || customerId == 0)
            {
                return null;
            }
            ClientAndCustomerContacts clientAndCustomerContacts = null;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "GetCustomerAndClientContact",
                                                                          CreateCustomerIdParameter(customerId, true)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    if (!cleverReader.IsNull && cleverReader.Read())
                    {
                        ClientContact clientContact = new ClientContactBuilder(cleverReader).Build();

                        cleverReader.NextResult();
                        cleverReader.Read();

                        CustomerContact customerContact = new CustomerContactBuilder(cleverReader).Build();
                        clientAndCustomerContacts = new ClientAndCustomerContacts(clientContact, customerContact);
                    }
                }
            }
            return clientAndCustomerContacts;
        }

        public ClientInformationAndAgeingBalances GetMatchedClientInfo(int clientId)
        {
            ClientInformationAndAgeingBalances information = null;

            //ArrayList arrClist = new ArrayList();
            //arrClist.Add(clientId);

            //DataSet theDs = clients.CACrLimitFromDrMgt(arrClist);
            //if (theDs != null)
            //{
            //     //theDs.Tables.
            //}

            
            using (SqlConnection connection = CreateConnection())
            {
                //djhjhdfjhdfh
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "stGetClientAgingAndCurrent",
                                                                          CreateClientIdAndUserParameters(clientId, 7)))
                {
                    CleverReader reader = new CleverReader(dataReader);
                    if (reader.Read())
                    {
                        ClientInformation clientInformation = new ClientInformationBuilder(reader).Build();
                        reader.NextResult();
                        TransactionSummary transactionSummary = new TransactionSummaryBuilder(reader).Build();
                        reader.NextResult();
                        AgeingBalances ageingBalances = new AgeingBalancesBuilder(reader).Build();

                        information = new ClientInformationAndAgeingBalances(ageingBalances, clientInformation,
                                                                             transactionSummary);
                    }
                }
            }

            return information;
        }

        public ClientAndCustomerInformation GetMatchedCustomerInfo(int customerId, int clientId)
        {
            if (customerId == int.MinValue || customerId == 0)
            {
                return null;
            }

            ClientAndCustomerInformation clientAndCustomerInformation = null;
            using (SqlConnection connection = CreateConnection())
            {
                //DataTable schema = null;
                //using (var schemaCommand = new SqlCommand("exec GetCustomerInfo 910823", connection))
                //{
                //    connection.Open();
                //    using (var reader = schemaCommand.ExecuteReader(CommandBehavior.SchemaOnly))
                //    {
                //        reader.NextResult();
                //        reader.Read();
                //        schema = reader.GetSchemaTable();
                //        reader.NextResult();
                //        reader.Read();
                //        schema = reader.GetSchemaTable();
                //        reader.NextResult();
                //        reader.Read();
                //        schema = reader.GetSchemaTable();
                //    }
                //}
                //DataTable schema = null;

                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "GetCustomerInfo",
                                                                          CreateCustomerIdParameter(customerId, false)))

                {
                    CleverReader reader = new CleverReader(dataReader);
                    if (!reader.IsNull && reader.Read())
                    {
                        IDate lastPaid = reader.ToNullableDate("lastRecDt");
                        decimal lastAmount = reader.ToDecimal("lastRecAmt");

                        reader.NextResult();

                        AgeingBalances ageingBalances = new AgeingBalancesBuilder(reader).Build();

                        reader.NextResult();
                        reader.Read();

                        CffClient client = new CffClientBuilder(reader).Build();

                        //CffClient client = new CffClientBuilderCleverReader(reader).Build();
                        CffClientInformation cffClientInformation = new CffClientInformationBuilder(reader).Build(client);
                        ClientCustomerTerm clientCustomerTerm = new ClientCustomerTermBuilder(reader).Build();

                        CffCustomer customer = new CffCustomerBuilder(reader).Build(customerId);
                        CffCustomerInformation customerInformation =
                            new CffCustomerInformationBuilder(reader).Build(customer, lastPaid, lastAmount,
                                                                            clientCustomerTerm, ageingBalances);

                        //MSarza [20151001]
                        reader.NextResult();
                        reader.Read();
                        CffCustomerContact defaultCustContact = new CffCustomerContactBuilder(reader).Build();

                        //MSarza [20150731]
                        reader.NextResult();
                        reader.Read();                        
                        CffMgtDetails cffMgtDetails = new CffMgtDetailsBuilder(reader).Build();

                        clientAndCustomerInformation = new ClientAndCustomerInformation(customerInformation, 
                                                                                        cffClientInformation,
                                                                                        defaultCustContact,         //MSarza [20150731]
                                                                                        cffMgtDetails);             //MSarza [20150731]
                    }
                }
            }
            return clientAndCustomerInformation;
        }

        public ClientContact GetClientContactDetails(int clientId)
        {
            ClientContact clientContact = null;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "Clients_GetClientContactOnly",
                                                                          CreateClientIdParameter(clientId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    if (!cleverReader.IsNull && cleverReader.Read())
                    {
                        clientContact = new ClientContactBuilder(cleverReader).Build();
                    }
                }
            }
            return clientContact;
        }

        public string GetPasskey(long clientId)
        {
            string key = string.Empty;
            ArgumentChecker.ThrowIfNull(clientId, "clientId");
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, "GetPassKey", CreateClientIdParameter(clientId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        key = cleverReader.ToString("PASSKEY");
                    }
                }
            }
            return key;
        }

        public Decimal GetCurrentACLimit(long clientId, int userId, DateTime date)
        {
            ArgumentChecker.ThrowIfNull(clientId, "clientId");
            ArgumentChecker.ThrowIfNull(userId, "userId");

            DataEntry.Clients mClient = new DataEntry.Clients();
            System.Collections.ArrayList arrPar = new System.Collections.ArrayList();
            Int16 isNow = 0;
            if (DateTime.Compare(date, DateTime.Today) != 0) isNow = -1;
            int yrmth = Convert.ToInt32(date.Year.ToString() + date.Month.ToString().PadLeft(2, '0'));
            //int yrmth = Convert.ToInt32(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0'));
            arrPar.Add(0);
            arrPar.Add(clientId);
            arrPar.Add(yrmth);
            arrPar.Add(date);
            arrPar.Add(userId);
            arrPar.Add(isNow);
            System.Data.DataSet theDS = mClient.CACrLimitFromDrMgt(arrPar);
            if (theDS != null)
            {
                DataRow DR = theDS.Tables[0].Rows[0];
                return Convert.ToDecimal(DR["limit"]);
            }
            else return 0;
        }

        public List<Decimal> GetCurrentACLimitExt(long clientId, int userId, DateTime date)
        {
            ArgumentChecker.ThrowIfNull(clientId, "clientId");
            ArgumentChecker.ThrowIfNull(userId, "userId");
            List<Decimal> record = new List<decimal>();
            DataEntry.Clients mClient = new DataEntry.Clients();
            System.Collections.ArrayList arrPar = new System.Collections.ArrayList();
            Int16 isNow = 0;
            if (DateTime.Compare(date, DateTime.Today) != 0) isNow = -1;
            int yrmth = Convert.ToInt32(date.Year.ToString() + date.Month.ToString().PadLeft(2, '0'));
            int fType = 5;
            string dateNow = date.ToString("dd/M/yyyy");
            //int yrmth = Convert.ToInt32(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0'));
            arrPar.Add(0);
            arrPar.Add(clientId);
            arrPar.Add(yrmth);
            //arrPar.Add(date);
            arrPar.Add(dateNow);
            arrPar.Add(userId);
            arrPar.Add(isNow);
            arrPar.Add(fType);
            System.Data.DataSet theDS = mClient.CACrLimitFromDrMgt(arrPar);
            if (theDS != null)
            {
                DataRow DR = theDS.Tables[0].Rows[0];
                record.Add(Convert.ToDecimal(DR["limit"]));
                record.Add(Convert.ToDecimal(DR["IntAndChargesDrMgt"]));
                record.Add(Convert.ToDecimal(DR["IntAndChargesCA"]));
                record.Add(Convert.ToDecimal(DR["IntAndChargesDrMgtPriorMth"]));
                record.Add((Convert.ToDecimal(DR["IntAndChargesCAPriorMth"])));
            }
            return record;
        }

        #region Parameter Helper methods

        private static SqlParameter[] CreateClientAndCustomerIdParameters(int clientId, int customerId)
        {
            SqlParameter clientIdParameter = new SqlParameter("@ClientID", SqlDbType.BigInt);
            clientIdParameter.Value = clientId;
            SqlParameter customerIdParameter = new SqlParameter("@CustomerID", SqlDbType.BigInt);
            customerIdParameter.Value = customerId;

            return new[] { clientIdParameter, customerIdParameter };
        }

        private static SqlParameter[] CreateClientIdAndUserParameters(int clientId, int userId)
        {
            SqlParameter clientIdParameter = new SqlParameter("@ClientID", SqlDbType.BigInt);
            clientIdParameter.Value = clientId;
            SqlParameter userIdParameter = new SqlParameter("@UserId", SqlDbType.Int);
            userIdParameter.Value = userId;

            return new[] {clientIdParameter, userIdParameter};
        }

        private static SqlParameter[] CreateClientIdAndUserUIDParameters(int clientId, Guid userId)
        {
            SqlParameter clientIdParameter = new SqlParameter("@ClientID", SqlDbType.BigInt);
            clientIdParameter.Value = clientId;
            SqlParameter userIdParameter = new SqlParameter("@UserId", SqlDbType.UniqueIdentifier);
            userIdParameter.Value = userId;

            return new[] { clientIdParameter, userIdParameter };
        }

        private static SqlParameter[] CreateCustomerIdParameter(long customerId, bool isGetDefault)
        {
            SqlParameter customerIdParameter = new SqlParameter("@customerId", SqlDbType.BigInt);
            customerIdParameter.Value = customerId;

            if (isGetDefault == true)
            {
                SqlParameter getDefault = new SqlParameter("@isDefault", SqlDbType.Bit);
                getDefault.Value = 1;

                return new[] { customerIdParameter, getDefault };
            }
            else 
            {
                return new[]{customerIdParameter};
            }
        }

        private static SqlParameter[] CreateClientIdParameter(long clientrId)
        {
            SqlParameter clientIdParameter = new SqlParameter("@ClientId", SqlDbType.BigInt);
            clientIdParameter.Value = clientrId;
            return new[]
                       {
                           clientIdParameter
                       };
        }

        private static SqlParameter[] CreateGetMatchedCustomerParameters(string matchString, long clientId,
                                                                         int numberOfCustomersToReturn)
        {
            SqlParameter matchStringParameter = new SqlParameter("@MatchString", SqlDbType.NVarChar, 40);
            SqlParameter clientIdParameter = new SqlParameter("@ClientID", SqlDbType.BigInt);
            SqlParameter numberOfCustomersToReturnParameter = new SqlParameter("@numberOfCustomersToReturn",
                                                                               SqlDbType.Int);
            matchStringParameter.Value = matchString;
            clientIdParameter.Value = clientId;
            numberOfCustomersToReturnParameter.Value = numberOfCustomersToReturn;
            return new[]
                       {
                           matchStringParameter,
                           clientIdParameter,
                           numberOfCustomersToReturnParameter
                       };
        }

        private static SqlParameter[] ExCreateGetMatchedCustomerParameters(string matchString, long clientId,
                                                                         int searchBy)
        {
            SqlParameter matchStringParameter = new SqlParameter("@Seek", SqlDbType.NVarChar, 40);
            SqlParameter clientIdParameter = new SqlParameter("@ClientID", SqlDbType.BigInt);
            SqlParameter searchByParameter = new SqlParameter("@SearchBy", SqlDbType.Int);
            
            matchStringParameter.Value = matchString;
            clientIdParameter.Value = clientId;
            searchByParameter.Value = searchBy;
            return new[]
                       {
                           matchStringParameter,
                           clientIdParameter,
                           searchByParameter
                       };
        }

        private static SqlParameter[] CreateUpdateCustomerCallDueParameters(Date nextCallDue, int callDueCheckPriority,
                                                                            int customerId, Date customerLastModified,
                                                                            int modifiedBy)
        {
            SqlParameter nextCallDueParameter = new SqlParameter("@NextCall", SqlDbType.DateTime);
            SqlParameter callDueCheckPriorityParameter = new SqlParameter("@CallDueChkdPriority", SqlDbType.Int);
            SqlParameter customerIdParameter = new SqlParameter("@CustomerId", SqlDbType.Int);
            SqlParameter customerLastModifiedParameter = new SqlParameter("@CustLastModified", SqlDbType.DateTime);
            SqlParameter modifiedByParameter = new SqlParameter("@ModifiedBy", SqlDbType.Int);
            SqlParameter returnValueParameter = new SqlParameter("@retVal", SqlDbType.VarChar);
            nextCallDueParameter.Value = nextCallDue.ToShortDateString();
            callDueCheckPriorityParameter.Value = callDueCheckPriority;
            customerIdParameter.Value = customerId;
            customerLastModifiedParameter.Value = customerLastModified.ToShortDateString();
            modifiedByParameter.Value = modifiedBy;
            return new[]
                       {
                           nextCallDueParameter,
                           callDueCheckPriorityParameter,
                           customerIdParameter,
                           customerLastModifiedParameter,
                           modifiedByParameter,
                           returnValueParameter
                       };
        }

        private static SqlParameter[] CreateInsUpdateCustomerInfoParameters(string strAction, int iClientID, int iCustNum,
                                    Int16 iStopCredit, decimal dCreditLimit, System.DateTime pNextCallDue, Int16 iAllowCalls, 
                                        System.DateTime dtListDate, Int16 iTerms, string sCompID, decimal dGSTValue, System.DateTime dtModified, int iModifBy)
        { //TODO:
            SqlParameter sAction = new SqlParameter("@sAction", SqlDbType.VarChar);
            SqlParameter clientID = new SqlParameter("@ClientID", SqlDbType.BigInt);
            SqlParameter custID = new SqlParameter("@CustID", SqlDbType.BigInt);
            SqlParameter stopCredit = new SqlParameter("@stopCredit", SqlDbType.SmallInt);
            SqlParameter creditLimit = new SqlParameter("@creditLimit", SqlDbType.Decimal);
            SqlParameter nextCall = new SqlParameter("@nextCall", SqlDbType.DateTime);
            SqlParameter nocalls = new SqlParameter("@noCalls", SqlDbType.SmallInt);
            SqlParameter listdate = new SqlParameter("@listdate", SqlDbType.DateTime);
            SqlParameter custterms = new SqlParameter("@custTerms", SqlDbType.SmallInt);
            SqlParameter companyID = new SqlParameter("@companyID", SqlDbType.VarChar);
            SqlParameter gstValue = new SqlParameter("@GSTValue", SqlDbType.Decimal);
            SqlParameter modified = new SqlParameter("@Modified", SqlDbType.DateTime);
            SqlParameter modifiedby = new SqlParameter("@ModifiedBy", SqlDbType.BigInt);


            sAction.Value = strAction;
            clientID.Value = iClientID;
            custID.Value = iCustNum;
            stopCredit.Value = iStopCredit;
            creditLimit.Value = dCreditLimit;
            nextCall.Value = pNextCallDue;
            nocalls.Value = iAllowCalls;
            listdate.Value = dtListDate;
            custterms.Value = iTerms;
            companyID.Value = sCompID;
            gstValue.Value = dGSTValue;
            modified.Value = dtModified;
            modifiedby.Value = iModifBy;

            return new[] { sAction, clientID, custID, stopCredit, creditLimit, nextCall, nocalls, listdate, custterms, companyID, gstValue, modified, modifiedby};
        }

        #endregion
    }
}