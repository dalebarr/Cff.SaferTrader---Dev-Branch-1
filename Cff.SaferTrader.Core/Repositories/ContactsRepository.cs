using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cff.SaferTrader.Core.Builders;

namespace Cff.SaferTrader.Core.Repositories
{
    public class ContactsRepository : SqlManager, IContactsRepository
    {
        public ContactsRepository(string connectionString) : base(connectionString)
        {
        }

 #region "Private"
        private IList<ClientContact> GetAClientsContacts(int clientId)
        {
            IList<ClientContact> contacts;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "dbo.ClientContacts_GetContacts",
                                                                          GenerateClientParameters(clientId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    contacts = new ClientContactBuilder(cleverReader).BuildAll();
                }


            }
            return contacts;
        }

        // dbb [20160727]
        private IList<ClientContact> GetAClientsContacts(int clientId, string sAction)
        {
            IList<ClientContact> contacts;
            using (SqlConnection connection = CreateConnection())
            {

                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "dbo.stInsUpdateClientContacts",
                                                                          GenerateClientForValidationParameters(clientId, sAction)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    contacts = new ClientContactBuilder(cleverReader).BuildAll();
                } //

            }
            return contacts;
        }



        private IList<CustomerContact> GetACustomersContacts(int customerId)
        {
            IList<CustomerContact> contacts;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "dbo.CustomerContacts_GetContacts",
                                                                          GenerateCustomerParameters(customerId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    contacts = new CustomerContactBuilder(cleverReader).BuildAll();
                }
            }
            return contacts;
        }

        private IList<ClientContact> GetAllClientsAndTheirContacts()
        {
            IList<ClientContact> contacts;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "dbo.ClientContacts_GetAllContacts"))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    contacts = new ClientContactBuilder(cleverReader).BuildAll();
                }
            }
            return contacts;
        }

        private static bool DoesStartWith(string attribute, string textToMatch)
        {
            return (!string.IsNullOrEmpty(attribute) &&
                    attribute.StartsWith(textToMatch, StringComparison.OrdinalIgnoreCase));
        }


        private IList<CustomerContact> GetAllCustomersAndTheirContacts()
        {
            IList<CustomerContact> contacts;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "dbo.CustomerContacts_GetAllContacts"))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    contacts = new CustomerContactBuilder(cleverReader).BuildAll();
                }
            }
            return contacts;
        }

        private IList<CustomerContact> GetAllCustomersContactsForAClient(int clientId)
        {
            IList<CustomerContact> contacts;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "dbo.CustomerContacts_GetAllContactsForAClient",
                                                                          GenerateClientParameters(clientId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    contacts = new CustomerContactBuilder(cleverReader).BuildAll();
                }
            }
            return contacts;
        }

        private bool updateClientContactDetails(int cliId, ClientContact cDetails)
        {
            bool bRet = false;
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    int ret = SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                                            "dbo.stUpdateDefaultClientContact", 
                                                                GenerateClientContactParameters(cliId, cDetails));
                    bRet = ret >= 0 ? true : false;
                }
            }
            catch (Exception)
            {
                bRet = false;
            }

            return bRet;
        }

        private bool updateCustomerContactDetails(int cliId, CustomerContact custDetails)
        {
            bool bRet = false;
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    int ret = SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                                            "dbo.stUpdateDefaultCustomerContact",
                                                                GenerateCustomerContactParameters(cliId, custDetails));
                    bRet = ret >= 0 ? true : false;
                }
            }
            catch (Exception)
            {
                bRet = false;
            }
            return bRet;
        }

        private bool insertCustContactInfoDetailsForValidation(int cliId, CustomerContact custinfocontactDetails)
        {
            bool bRet = false;
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    int ret = SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                                            "dbo.stInsertCustContactInfoForValidation",
                                                                GenerateInsertCustContactInfoForValidParams(cliId, custinfocontactDetails));
                    bRet = ret >= 0 ? true : false;
                }
            }
            catch (Exception)
            {
                bRet = false;
            }

            return bRet;
            
        }

        private bool insertClientContactInfoDetailsForValidation(int cliId, ClientContact cDetails)
        {
            bool bRet = false;
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    int ret = SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure,
                                                            "dbo.stInsUpdateCliContactInfoForValidation",
                                                                GenerateInsUpdateCliContactInfoForValidParams(cliId, cDetails));
                    bRet = ret >= 0 ? true : false;
                }
            }
            catch (Exception)
            {
                bRet = false;
            }

            return bRet;
        }


 #endregion

        public IList<ClientContact> LoadAClientsContacts(int clientId)
        {
            IList<ClientContact> contacts = GetAClientsContacts(clientId);
            return RecordLimiter.ReturnMaximumRecords(contacts);
        }

        public IList<ClientContact> LoadAClientsContacts(int clientId, string sAction)
        {
            IList<ClientContact> contacts = GetAClientsContacts(clientId, sAction);
            return RecordLimiter.ReturnMaximumRecords(contacts);
        }

        public IList<CustomerContact> LoadACustomersContacts(int customerId)
        {
            IList<CustomerContact> contacts = GetACustomersContacts(customerId);
            return RecordLimiter.ReturnMaximumRecords(contacts);
        }

        public IList<ClientContact> LoadAllClientsAndTheirContacts()
        {
            IList<ClientContact> contacts = GetAllClientsAndTheirContacts();
            return RecordLimiter.ReturnMaximumRecords(contacts);
        }

        public IList<CustomerContact> LoadAllCustomersAndTheirContacts()
        {
            IList<CustomerContact> contacts = GetAllCustomersAndTheirContacts();
            return RecordLimiter.ReturnMaximumRecords(contacts);
        }

        public IList<CustomerContact> LoadAllCustomersContactsForAClient(int clientId)
        {
            IList<CustomerContact> contacts = GetAllCustomersContactsForAClient(clientId);
            return RecordLimiter.ReturnMaximumRecords(contacts);
        }

  
        public IList<ClientContact> LoadMatchedAllClientsContacts(string textToMatch)
        {
            IList<ClientContact> contacts;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "dbo.ClientContactView_GetMatchedClientContacts",
                                                                          GenerateTextToSearchParameters(textToMatch)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    contacts = new ClientContactBuilder(cleverReader).BuildAll();
                }
            }
            return RecordLimiter.ReturnMaximumRecords(contacts);
        }

        public IList<ClientContact> LoadMatchedClientsContactsForAClient(string textToMatch, int clientId)
        {
            IList<ClientContact> contacts;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "dbo.ClientContactView_GetMatchedClientContacts",
                                                                          GenerateClientIDAndtextToMatchParameters(clientId,textToMatch)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    contacts = new ClientContactBuilder(cleverReader).BuildAll();
                }
            }
            return RecordLimiter.ReturnMaximumRecords(contacts);
        }

        public IList<CustomerContact> LoadMatchedAllClientsCustomerContact(string textToMatch)
        {
            IList<CustomerContact> contacts;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "dbo.CustomerContactsView_GetMatchedCustomerContacts", GenerateTextToSearchParameters(textToMatch)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    contacts = new CustomerContactBuilder(cleverReader).BuildAll();
                }
            }
            return RecordLimiter.ReturnMaximumRecords(contacts);
        }


        public IList<CustomerContact> LoadMatchedCustomerContactForAClient(string textToMatch, int clientId)
        {
            IList<CustomerContact> contacts;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "dbo.CustomerContactsView_GetMatchedCustomerContacts",
                                                                          GenerateClientIDAndtextToMatchParameters
                                                                              (clientId, textToMatch)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    contacts = new CustomerContactBuilder(cleverReader).BuildAll();
                }
            }
            return RecordLimiter.ReturnMaximumRecords(contacts);
        }


        public IList<CustomerContact> LoadMatchedCustomerContactForACustomer(string textToMatch, int customerId)
        {
            IList<CustomerContact> contacts;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "dbo.CustomerContactsView_GetMatchedCustomerContacts",
                                                                          GenerateMatchedCustomerContactForACustomerParameters
                                                                              (customerId, textToMatch)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    contacts = new CustomerContactBuilder(cleverReader).BuildAll();
                }
            }
            return RecordLimiter.ReturnMaximumRecords(contacts);
        }



        public IList<ClientContact> LoadClientsContactsWithClientNameStartWith(string letter)
        {
            IList<ClientContact> contacts = GetAllClientsAndTheirContacts().AsQueryable()
                .Where(clientContact =>
                       DoesStartWith(clientContact.ClientName, letter))
                .OrderBy(clientContact => clientContact.ClientName)
                .ToList();
            return contacts;
        }

        public IList<CustomerContact> LoadAllClientsCustomerContactsWithCustomerNameStartWith(string letter)
        {
            IList<CustomerContact> contacts = GetAllCustomersAndTheirContacts().AsQueryable()
                .Where(customerContact =>
                       DoesStartWith(customerContact.CustomerName, letter))
                .OrderBy(customerContact => customerContact.CustomerName)
                .ToList();
            return RecordLimiter.ReturnMaximumRecords(contacts);
        }

        public IList<CustomerContact> LoadCustomerContactsForAClientWithCustomerNameStartWith(string letter, int clientId)
        {
            IList<CustomerContact> contacts = GetAllCustomersAndTheirContacts().AsQueryable()
               .Where(customerContact =>
                      DoesStartWith(customerContact.CustomerName, letter)&&customerContact.ClientId.Equals(clientId))
               .OrderBy(customerContact => customerContact.CustomerName)
               .ToList();
            return RecordLimiter.ReturnMaximumRecords(contacts);
        }

        public IList<CustomerContact> LoadCustomerContactsForACustomerWithCustomerNameStartWith(string letter, int customerId)
        {
            IList<CustomerContact> contacts = GetAllCustomersAndTheirContacts().AsQueryable()
              .Where(customerContact =>
                     DoesStartWith(customerContact.CustomerName, letter) && customerContact.CustomerId.Equals(customerId))
              .OrderBy(customerContact => customerContact.CustomerName)
              .ToList();
            return RecordLimiter.ReturnMaximumRecords(contacts);
        }

        public bool UpdateClientContactDetails(int cliID, ClientContact clientContactDetails)
        {
            return this.updateClientContactDetails(cliID, clientContactDetails);
        }

        public bool UpdateCustomerContactDetails(int cliID, CustomerContact customerContactDetails)
        {
            return this.updateCustomerContactDetails(cliID, customerContactDetails);
        }

        public bool InsCustContactInfoDetailsForValidation(int cliID, CustomerContact custContactInfoDetails)
        { 
            return this.insertCustContactInfoDetailsForValidation(cliID, custContactInfoDetails);
        }

        public bool InsertClientContactInfoDetailsForValidation(int cliId, ClientContact cDetails)
        {
            return this.insertClientContactInfoDetailsForValidation(cliId, cDetails);
        }

        //MSarza [20150819]
        public CustomerContact LoadTheDefaultCustomerContact(int customerId)      
        {
           CustomerContact defaultContact = null;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "dbo.CustomerContacts_GetContacts",
                                                                          GenerateDefaultCustomerContactParams(customerId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    if (!cleverReader.IsNull && cleverReader.Read())
                    {
                        defaultContact = new CustomerContactBuilder(cleverReader).Build();
                    }
                }
            }
            return defaultContact;
        }




 #region Create Parameters
        private static SqlParameter[] GenerateCustomerParameters(int customerId)
        {
            SqlParameter customerIdParameter = new SqlParameter("customerId", SqlDbType.Int);
            customerIdParameter.Value = customerId;

            return new[] {customerIdParameter};
        }

        private static SqlParameter[] GenerateClientParameters(int clientId)
        {
            SqlParameter clientIdParameter = new SqlParameter("clientId", SqlDbType.Int);
            clientIdParameter.Value = clientId;

            return new[] {clientIdParameter};
        }

        private static SqlParameter[] GenerateClientForValidationParameters(int clientId, string sAction)
        {
            SqlParameter clientIdParameter = new SqlParameter("clientId", SqlDbType.BigInt);
            clientIdParameter.Value = clientId;

            SqlParameter sActionParameter = new SqlParameter("sAction", SqlDbType.VarChar);
            sActionParameter.Value = sAction;

            return new[] { clientIdParameter, sActionParameter };
        }


        private static SqlParameter[] GenerateClientIDAndtextToMatchParameters(int clientId,
                                                                                         string textToMatch)
        {
            SqlParameter clientIdParameter = new SqlParameter("ClientID ", SqlDbType.Int);
            clientIdParameter.Value = clientId;
            SqlParameter searchStringParameter = new SqlParameter("SearchString ", SqlDbType.VarChar, 50);
            searchStringParameter.Value = textToMatch;
            return new[] {clientIdParameter, searchStringParameter};
        }

        private static SqlParameter[] GenerateMatchedCustomerContactForACustomerParameters(int customerId, string textToMatch)
        {
            SqlParameter customerIdParameter = new SqlParameter("customerID ", SqlDbType.Int);
            customerIdParameter.Value = customerId;
            SqlParameter searchStringParameter = new SqlParameter("SearchString ", SqlDbType.VarChar, 50);
            searchStringParameter.Value = textToMatch;
            return new[] { customerIdParameter, searchStringParameter };
        }

        private static SqlParameter[] GenerateTextToSearchParameters(string textToMatch)
        {
            SqlParameter searchStringParameter = new SqlParameter("SearchString ", SqlDbType.VarChar, 50);
            searchStringParameter.Value = textToMatch;
            return new[] { searchStringParameter };
        }

        private static SqlParameter[] GenerateClientContactParameters(int cliId, ClientContact cliDetails)
        {
            SqlParameter clientIDParameter = new SqlParameter("clientID", SqlDbType.Int);
            clientIDParameter.Value = cliId;

            SqlParameter phoneParameter = new SqlParameter("Phone", SqlDbType.VarChar, 20);
            phoneParameter.Value = cliDetails.Phone;

            SqlParameter faxParameter = new SqlParameter("Fax", SqlDbType.VarChar, 20);
            faxParameter.Value = cliDetails.Fax;
            
            SqlParameter mobileParameter = new SqlParameter("Cell", SqlDbType.VarChar, 20);
            mobileParameter.Value = cliDetails.MobilePhone;
            

            SqlParameter lastNameParameter = new SqlParameter("LName", SqlDbType.VarChar, 40);
            lastNameParameter.Value = cliDetails.LastName;

            SqlParameter firstNameParameter = new SqlParameter("FName", SqlDbType.VarChar, 40);
            firstNameParameter.Value = cliDetails.FirstName;

            SqlParameter roleParameter = new SqlParameter("Role", SqlDbType.VarChar, 30);
            roleParameter.Value = cliDetails.Role;

            SqlParameter emailParameter = new SqlParameter("Email", SqlDbType.VarChar, 40);
            emailParameter.Value = cliDetails.Email;

            SqlParameter address1Parameter = new SqlParameter("Address1", SqlDbType.VarChar, 50);
            address1Parameter.Value = cliDetails.Address1;

            SqlParameter address2Parameter = new SqlParameter("Address2", SqlDbType.VarChar, 50);
            address2Parameter.Value = cliDetails.Address2;

            SqlParameter address3Parameter = new SqlParameter("Address3", SqlDbType.VarChar, 50);
            address3Parameter.Value = cliDetails.Address3;

            SqlParameter address4Parameter = new SqlParameter("Address4", SqlDbType.VarChar, 50);
            address4Parameter.Value = cliDetails.Address4;

            return new[] { clientIDParameter, phoneParameter, faxParameter, mobileParameter, lastNameParameter, firstNameParameter,
                            roleParameter, emailParameter, address1Parameter, address2Parameter, address3Parameter,address4Parameter};
        }

        private static SqlParameter[] GenerateCustomerContactParameters(int cliId, CustomerContact custDetails)
        {
            SqlParameter clientIDParameter = new SqlParameter("clientID", SqlDbType.Int);
            clientIDParameter.Value = cliId;

            SqlParameter custIDParameter = new SqlParameter("custID", SqlDbType.Int);
            custIDParameter.Value = custDetails.CustomerId;

            /*SqlParameter custContactIDParameter = new SqlParameter("custContactsID", SqlDbType.Int);
            custContactIDParameter.Value = custDetails.ContactId;*/
          
            SqlParameter phoneParameter = new SqlParameter("Phone", SqlDbType.VarChar, 20);
            phoneParameter.Value = custDetails.Phone;

            SqlParameter faxParameter = new SqlParameter("Fax", SqlDbType.VarChar, 20);
            faxParameter.Value = custDetails.Fax;

            SqlParameter mobileParameter = new SqlParameter("Cell", SqlDbType.VarChar, 20);
            mobileParameter.Value = custDetails.MobilePhone;


            SqlParameter lastNameParameter = new SqlParameter("LName", SqlDbType.VarChar, 40);
            lastNameParameter.Value = custDetails.LastName;

            SqlParameter firstNameParameter = new SqlParameter("FName", SqlDbType.VarChar, 40);
            firstNameParameter.Value = custDetails.FirstName;

            SqlParameter roleParameter = new SqlParameter("Role", SqlDbType.VarChar, 30);
            roleParameter.Value = custDetails.Role;

            SqlParameter emailParameter = new SqlParameter("Email", SqlDbType.VarChar, 40);
            emailParameter.Value = custDetails.Email;

            SqlParameter address1Parameter = new SqlParameter("Address1", SqlDbType.VarChar, 50);
            address1Parameter.Value = custDetails.Address1;

            SqlParameter address2Parameter = new SqlParameter("Address2", SqlDbType.VarChar, 50);
            address2Parameter.Value = custDetails.Address2;

            SqlParameter address3Parameter = new SqlParameter("Address3", SqlDbType.VarChar, 50);
            address3Parameter.Value = custDetails.Address3;

            SqlParameter address4Parameter = new SqlParameter("Address4", SqlDbType.VarChar, 50);
            address4Parameter.Value = custDetails.Address4;

            return new[] { clientIDParameter, custIDParameter, phoneParameter, faxParameter,
                                mobileParameter,  lastNameParameter, firstNameParameter, roleParameter, emailParameter, 
                                    address1Parameter, address2Parameter, address3Parameter,address4Parameter};
        }

        private static SqlParameter[] GenerateInsertCustContactInfoForValidParams(int cliId, CustomerContact custinfocontactdetails)
        {
            //insert into cust contact updates for validation
            //address1-4, insert into cust info updates for validation
            SqlParameter clientIDParameter = new SqlParameter("ClientID", SqlDbType.BigInt);
            clientIDParameter.Value = cliId;

            SqlParameter custIDParameter = new SqlParameter("CustID", SqlDbType.BigInt);
            custIDParameter.Value = custinfocontactdetails.CustomerId;

            SqlParameter contactIDParameter = new SqlParameter("CustContactsID", SqlDbType.BigInt);
            contactIDParameter.Value = custinfocontactdetails.ContactId;

            SqlParameter PhonePrameter = new SqlParameter("Phone", SqlDbType.VarChar);
            PhonePrameter.Value = custinfocontactdetails.Phone;

            SqlParameter FaxParameter = new SqlParameter("Fax", SqlDbType.VarChar);
            FaxParameter.Value = custinfocontactdetails.Fax;

            SqlParameter MobilePhoneParameter = new SqlParameter("MobilePhone", SqlDbType.VarChar);
            MobilePhoneParameter.Value = custinfocontactdetails.MobilePhone;

            SqlParameter EmailAddressParameter = new SqlParameter("EmailAddress", SqlDbType.VarChar);
            EmailAddressParameter.Value = custinfocontactdetails.Email;

            SqlParameter FirstNameParameter = new SqlParameter("FirstName", SqlDbType.VarChar);
            FirstNameParameter.Value = custinfocontactdetails.FirstName;

            SqlParameter LastNameParameter = new SqlParameter("LastName", SqlDbType.VarChar);
            LastNameParameter.Value = custinfocontactdetails.LastName;

            SqlParameter Address1Parameter = new SqlParameter("Address1", SqlDbType.VarChar);
            Address1Parameter.Value = custinfocontactdetails.Address1;

            SqlParameter Address2Parameter = new SqlParameter("Address2", SqlDbType.VarChar);
            Address2Parameter.Value = custinfocontactdetails.Address2;

            SqlParameter Address3Parameter = new SqlParameter("Address3", SqlDbType.VarChar);
            Address3Parameter.Value = custinfocontactdetails.Address3;

            SqlParameter Adress4Parameter = new SqlParameter("Address4", SqlDbType.VarChar);
            Adress4Parameter.Value = custinfocontactdetails.Address4;

            SqlParameter RoleParameter= new SqlParameter("Role", SqlDbType.VarChar);
            RoleParameter.Value = custinfocontactdetails.Role;

            SqlParameter ModifiedParameter = new SqlParameter("Modified", SqlDbType.DateTime);
            ModifiedParameter.Value = custinfocontactdetails.Modified;

            SqlParameter ModifiedByParameter= new SqlParameter("ModifiedBy", SqlDbType.SmallInt);
            ModifiedByParameter.Value = custinfocontactdetails.ModifiedBy;

            return new[] {clientIDParameter, custIDParameter, contactIDParameter, PhonePrameter, FaxParameter, MobilePhoneParameter, 
                            EmailAddressParameter, FirstNameParameter, LastNameParameter, Address1Parameter, Address2Parameter, 
                            Address3Parameter, Adress4Parameter, RoleParameter, ModifiedParameter, ModifiedByParameter};
        }

        private static SqlParameter[] GenerateInsUpdateCliContactInfoForValidParams(int clientId, ClientContact cliContactDetails)
        {   //insert into clicontactinfo updates for validation
            SqlParameter sActionParameter = new SqlParameter("sAction", SqlDbType.VarChar);
            sActionParameter.Value = "Insert";

            SqlParameter clientIDParameter = new SqlParameter("ClientID", SqlDbType.BigInt);
            clientIDParameter.Value = clientId;

            SqlParameter LastNameParameter = new SqlParameter("LName", SqlDbType.VarChar);
            LastNameParameter.Value = cliContactDetails.LastName;

            SqlParameter FirstNameParameter = new SqlParameter("FName", SqlDbType.VarChar);
            FirstNameParameter.Value = cliContactDetails.FirstName;

            SqlParameter PhoneParameter = new SqlParameter("Phone", SqlDbType.VarChar);
            PhoneParameter.Value = cliContactDetails.Phone;

            SqlParameter FaxParameter = new SqlParameter("Fax", SqlDbType.VarChar);
            FaxParameter.Value = cliContactDetails.Fax;

            SqlParameter CellParameter = new SqlParameter("Cell", SqlDbType.VarChar);
            CellParameter.Value = cliContactDetails.MobilePhone;
            
            SqlParameter RoleParameter = new SqlParameter("Role", SqlDbType.VarChar);
            RoleParameter.Value = cliContactDetails.Role;

            SqlParameter EmailParameter = new SqlParameter("Email", SqlDbType.VarChar);
            EmailParameter.Value = cliContactDetails.Email;

            SqlParameter Address1Parameter = new SqlParameter("Address1", SqlDbType.VarChar);
            Address1Parameter.Value = cliContactDetails.Address1;

            SqlParameter Address2Parameter = new SqlParameter("Address2", SqlDbType.VarChar);
            Address2Parameter.Value = cliContactDetails.Address2;

            SqlParameter Address3Parameter = new SqlParameter("Address3", SqlDbType.VarChar);
            Address3Parameter.Value = cliContactDetails.Address3;

            SqlParameter Adress4Parameter = new SqlParameter("Address4", SqlDbType.VarChar);
            Adress4Parameter.Value = cliContactDetails.Address4;

            SqlParameter ModifiedParameter = new SqlParameter("Modified", SqlDbType.DateTime);
            ModifiedParameter.Value = cliContactDetails.Modified;

            SqlParameter ModifiedByParameter= new SqlParameter("ModifiedBy", SqlDbType.Int);
            ModifiedByParameter.Value = cliContactDetails.ModifiedBy;

            return new[] {sActionParameter, clientIDParameter, LastNameParameter, FirstNameParameter, PhoneParameter, FaxParameter, CellParameter, 
                            RoleParameter, EmailParameter, Address1Parameter, Address2Parameter, Address3Parameter, Adress4Parameter, 
                                ModifiedParameter, ModifiedByParameter};
        }

        //MSarza [20150819]
        private static SqlParameter[] GenerateDefaultCustomerContactParams(int customerId)  
        {
            SqlParameter customerIdParameter = new SqlParameter("CustomerID", SqlDbType.Int);
            customerIdParameter.Value = customerId;
            SqlParameter isDefaultParameter = new SqlParameter("IsDefault", SqlDbType.Bit);
            isDefaultParameter.Value = 1;

            return new[] { customerIdParameter, isDefaultParameter};
        }

#endregion
    }
}