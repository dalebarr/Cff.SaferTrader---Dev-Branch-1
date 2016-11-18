using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Core.Letters;

using Calendar=Cff.SaferTrader.Core.Calendar;


namespace Cff.SaferTrader.Web
{
    public partial class SafeTrader : MasterPage, ISafeTraderView
    {
        public int cRole;
        public int timeoutWarning;

        //MSarza [20150901] : Data type changed from bool to small int for dbo.ClientFinancials.CffDebtorAdmin
        //public bool isAdminByCff; // dbb 
        public bool isClientDebtorAdmin;
        public bool isCffDebtorAdminForClient;

        private const string IsCustomerInfoInEdit = "IsCustomerInfoInEdit";
        private SafeTraderPresenter presenter;
        protected int timeout;
        protected string logOnURL;
        protected string keepAliveURL;
        private static  ClientContact origClientContactDetails;
        private static CustomerContact origCustomerContactDetails;

        #region ISafeTraderView Members

        public Scope CurrentScope()
        {
            if (SessionWrapper.Instance.Get != null)
                return SessionWrapper.Instance.Get.Scope;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).Scope;
            else
            {
                if (QueryString.CustomerId != null)
                    return Scope.CustomerScope;
                else if (QueryString.ClientId != null)
                    return Scope.ClientScope;
                return Scope.AllClientsScope;
            }
        }


        public void DisplayClientAndCustomerContacts(ClientAndCustomerContacts clientAndCustomerContacts)
        {
            if (clientAndCustomerContacts != null) 
            {

                ClientContact clientContact = clientAndCustomerContacts.ClientContact;
                clientContactName.Text = "Contact: " + clientContact.FullName;
                clientContactPhone.Text = string.Format("Phone: {0}", clientContact.Phone);
                clientContactFax.Text = string.Format("Fax: {0}", clientContact.Fax);
                clientContactMobile.Text = string.Format("Mobile: {0}", clientContact.MobilePhone);
                clientContactRole.Text = string.Format("Role: {0}", clientContact.Role);
                clientContactEmail.Text = string.Format("Email: {0}", clientContact.Email);
                clientContactAddress1.Text = clientContact.Address1;
                clientContactAddress2.Text = clientContact.Address2;
                clientContactAddress3.Text = clientContact.Address3;
                clientContactAddress4.Text = clientContact.Address4;

                if (string.IsNullOrEmpty(clientContact.ClientName))
                {
                    if (SessionWrapper.Instance.Get!=null)
                        ClientSearch.Value = SessionWrapper.Instance.Get.ClientFromQueryString.NameAndNumber;
                    else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                        ClientSearch.Value = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.NameAndNumber;
                }
                else
                    ClientSearch.Value = string.Format("{0} ({1})", clientContact.ClientName, clientContact.ClientNumber);
                
                
                CustomerContact customerContact = clientAndCustomerContacts.CustomerContact;
                if (customerContact != null) {
                    customerContactName.Text = "Contact: " + customerContact.FullName;
                    customerContactPhone.Text = string.Format("Phone: {0}", customerContact.Phone);
                    customerContactFax.Text = string.Format("Fax: {0}", customerContact.Fax);
                    customerContactMobile.Text = string.Format("Mobile: {0}", customerContact.MobilePhone);

                    customerContactRole.Text = string.Format("Role: {0}", customerContact.Role);
                    customerContactEmail.Text = string.Format("Email: {0}", customerContact.Email);
                    customerContactAddress1.Text = customerContact.Address1;
                    customerContactAddress2.Text = customerContact.Address2;
                    customerContactAddress3.Text = customerContact.Address3;
                    customerContactAddress4.Text = customerContact.Address4;
                    literalContactCustomerID.Text = customerContact.CustomerId.ToString();

                    customerContact.ContactId = clientAndCustomerContacts.CustomerContact.ContactId;
                    customerContact.Modified = DateTime.Now;
                    customerContact.ModifiedBy = clientAndCustomerContacts.CustomerContact.ModifiedBy;

                    txtCustomerContactFName.Text = customerContact.FirstName;
                    txtCustomerContactLName.Text = customerContact.LastName;
                    literalCLName.Text = customerContact.LastName;
                }
               
                origClientContactDetails = clientContact;
                origCustomerContactDetails = customerContact;
                ViewState.Add("ClientContactDetails", clientContact);
                ViewState.Add("CustContactDetails", customerContact);

                CffPrincipal cpUser = Context.User as CffPrincipal;
                if (cpUser.IsInAdministratorRole || cpUser.IsInManagementRole)
                {
                    LinkBtnEditClientContact.Visible = true;
                    LinkBtnEditCustomerContact.Visible = true;
                }
                else
                {
                    LinkBtnEditClientContact.Visible = false;
                    LinkBtnEditCustomerContact.Visible = false;
                }

                if (customerContact == null)
                    LinkBtnEditCustomerContact.Visible = false;
                
                txtClientContactFName.Text = clientContact.FirstName;
                txtClientContactLName.Text = clientContact.LastName;
                literalLName.Text = clientContact.LastName;
                hideEditClientContactTextBoxes();
                LinkBtnCancelEditContact.Visible = false;

                hideEditCustomerContactTextBoxes();
                LinkBtnCancelEditCustomerContact.Visible = true;
                LinkBtnCancelEditCustomerContact.Visible = false;

                literalContactCustomerID.Visible = false;

                if (SessionWrapper.Instance.Get != null)
                {
                    if (SessionWrapper.Instance.Get.IsEditContactDetails == true)
                            SessionWrapper.Instance.Get.IsEditContactDetails = false;


                    if (SessionWrapper.Instance.Get.IsEditCContactDetails == true)
                            SessionWrapper.Instance.Get.IsEditCContactDetails = false;

                    //MSarza [20150901]  
                    //SessionWrapper.Instance.Get.IsClientAdminByCFF = clientAndCustomerContacts.isClientAdministeredByCFF;
                    //isAdminByCff = SessionWrapper.Instance.Get.IsClientAdminByCFF;  // dbb
                    SessionWrapper.Instance.Get.IsCffDebtorAdminForClient = clientAndCustomerContacts.CffIsDebtorAdminForClient;
                    isCffDebtorAdminForClient = SessionWrapper.Instance.Get.IsCffDebtorAdminForClient; 
                    SessionWrapper.Instance.Get.IsClientDebtorAdmin = clientAndCustomerContacts.ClientIsDebtorAdmin;
                    isClientDebtorAdmin = SessionWrapper.Instance.Get.IsClientDebtorAdmin; 

                    if (customerContact != null)
                            DisplaySearchValue(customerContact);
                    else if (SessionWrapper.Instance.Get.CustomerFromQueryString != null)
                            CustomerSearch.Value = string.Format("{0} ({1})", SessionWrapper.Instance.Get.CustomerFromQueryString.Name,
                                                                    SessionWrapper.Instance.Get.CustomerFromQueryString.Number);
                }
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                {
                    if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsEditContactDetails == true)
                        SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsEditContactDetails = false;

                    if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsEditCContactDetails == true)
                        SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsEditCContactDetails = false;


                    //MSarza [20150901] : Data type changed from bool to small int for dbo.ClientFinancials.CffDebtorAdmin.
                    ////SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsClientAdminByCFF = clientAndCustomerContacts.isClientAdministeredByCFF;
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsCffDebtorAdminForClient = clientAndCustomerContacts.CffIsDebtorAdminForClient;
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsClientDebtorAdmin = clientAndCustomerContacts.ClientIsDebtorAdmin;

                    if (customerContact != null)
                        DisplaySearchValue(customerContact);
                    else if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString != null)
                        CustomerSearch.Value = string.Format("{0} ({1})", SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString.Name,
                                                                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString.Number);

                    //MSarza [20150901] : Data type changed from bool to small int for dbo.ClientFinancials.CffDebtorAdmin.
                    //isAdminByCff = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsClientAdminByCFF;  // dbb
                    isCffDebtorAdminForClient = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsCffDebtorAdminForClient;
                    isClientDebtorAdmin = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsClientDebtorAdmin;
                }
                    
            }
            else 
            {
                LinkBtnEditCustomerContact.Visible = false;
                LinkBtnCancelEditCustomerContact.Visible = false;
                //MSarza [20150901] : Data type changed from bool to small int for dbo.ClientFinancials.CffDebtorAdmin.
                //isAdminByCff = false;  // dbb
                isClientDebtorAdmin = true;
                
                if (SessionWrapper.Instance.Get != null && QueryString.CustomerId!=null) {
                    if (SessionWrapper.Instance.Get.CustomerFromQueryString != null && QueryString.CriteriaValue != null) {
                        if (QueryString.CriteriaValue < 2)
                            CustomerSearch.Value = string.Format("{0} ({1})", SessionWrapper.Instance.Get.CustomerFromQueryString.Name,
                                                            SessionWrapper.Instance.Get.CustomerFromQueryString.Number.ToString());
                    }
                }
            }
        }

        private void DisplaySearchValue(CustomerContact customerContact)
        {
            //MSarza: as per Marty H 0n 20150303, the selected customer details to be displayed will be the customer name and customer number
            //          hence, no need for the switch routine below
            ////todo:: display should be dependent on the customer dropdown selectedindex
            ////switch (QueryString.CriteriaValue)
            ////{  
            ////    case 2: //Phone/Cell/Fax
            ////        CustomerSearch.Value = string.Format("{0} (Ph. {1})", customerContact.CustomerName,
            ////            (customerContact.Phone.Length > 0 ? customerContact.Phone : customerContact.MobilePhone.Length > 0 ? customerContact.MobilePhone : customerContact.Fax));
            ////        break;
            ////    case 3://contact
            ////    case 4://email
            ////        CustomerSearch.Value = string.Format("{0} ({1})", customerContact.CustomerName, customerContact.FirstName);
            ////        break;

            ////    default: // 
            ////        CustomerSearch.Value = string.Format("{0} ({1})", customerContact.CustomerName, customerContact.CustomerNumber);
            ////        break;
            ////}
            CustomerSearch.Value = string.Format("{0} ({1})", customerContact.CustomerName, customerContact.CustomerNumber);
        }


        public void DisplayClientContactOnly(ClientContact clientContact)
        {
            //clientName.Text = clientContact.ClientName;
            try
            {
                clientContactName.Text = "Contact: " + clientContact.FullName.PadRight(80, ' ');
            }
            catch (Exception)
            {
                clientContactName.Text = "Contact: ";
            }

            clientContactPhone.Text = string.Format("Phone: {0}", clientContact.Phone);
            clientContactFax.Text = string.Format("Fax: {0}", clientContact.Fax);
            clientContactMobile.Text = string.Format("Mobile: {0}", clientContact.MobilePhone);
            clientContactRole.Text = "Role: " + clientContact.Role;
            clientContactEmail.Text = "Email: " + clientContact.Email;

            clientContactAddress1.Text = clientContact.Address1;
            clientContactAddress2.Text = clientContact.Address2;
            clientContactAddress3.Text = clientContact.Address3;
            clientContactAddress4.Text = clientContact.Address4;

            origClientContactDetails = clientContact;

            CffPrincipal cpUser = Context.User as CffPrincipal;
            if (cpUser.IsInAdministratorRole || cpUser.IsInManagementRole)
            { 
                LinkBtnEditClientContact.Visible = true;
            }
            else {
                LinkBtnEditClientContact.Visible = false; 
            }

            if (SessionWrapper.Instance.Get != null) {
                if (SessionWrapper.Instance.Get.IsEditContactDetails)
                        SessionWrapper.Instance.Get.IsEditContactDetails = false;
            } else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsEditContactDetails)
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsEditContactDetails = false;
        
            LinkBtnCancelEditContact.Visible = false;
            txtClientContactFName.Text = clientContact.FirstName;
            txtClientContactLName.Text = clientContact.LastName;
            literalLName.Text = clientContact.LastName;
            
            hideEditClientContactTextBoxes();
            hideEditCustomerContactTextBoxes();

            customerContactName.Text = string.Empty;
            customerContactPhone.Text = string.Empty;
            customerContactFax.Text = string.Empty;
            customerContactEmail.Text = string.Empty;
            customerContactAddress1.Text = string.Empty;
            customerContactAddress2.Text = string.Empty;
            customerContactAddress3.Text = string.Empty;
            customerContactAddress4.Text = string.Empty;
            LinkBtnEditCustomerContact.Visible = false;
            LinkBtnCancelEditCustomerContact.Visible = false;
            literalContactCustomerID.Visible = false;
             
        }

        public void ClearCffCustomerContactAndLeftInfomationPanel()
        {
            bool isAllClientsSelected = false;

            if (SessionWrapper.Instance.Get == null) {
                if (QueryString.ClientId == -1)
                    isAllClientsSelected = true;
            }
            else 
                isAllClientsSelected = SessionWrapper.Instance.Get.IsAllClientsSelected;
            
            if (isAllClientsSelected)
            {
                //clientName.Text = string.Empty;
                clientContactName.Text = string.Empty;
                clientContactPhone.Text = string.Empty;
                clientContactFax.Text = string.Empty;
                clientContactEmail.Text = string.Empty;
            }

            //customerName.Text = string.Empty;
            customerContactName.Text = string.Empty;
            customerContactPhone.Text = string.Empty;
            customerContactFax.Text = string.Empty;
            customerContactEmail.Text = string.Empty;

            if (stopCreditLiteral != null)
            {
                stopCreditLiteral.Text = string.Empty;
                creditLimitLiteral.Text = string.Empty;
                nextCallLiteral.Text = string.Empty;
                nextCallTextBox.Text = string.Empty;
                allowCallsLiteral.Text = string.Empty;
                lastPaidLiteral.Text = string.Empty;
                lastAmountLiteral.Text = string.Empty;
                ListDateLiteral.Text = string.Empty;
                termsLiteral.Text = string.Empty;
                clientNumberLiteral.Text = string.Empty;
                customerNumberLiteral.Text = string.Empty;
                CffIdLiteral.Text = string.Empty;
                //TODO Add Company ID and GST # which are not in the current database.
                CurrentLiteral.Text = string.Empty;
                oneMonthAgingLiteral.Text = string.Empty;
                twoMonthAgingLiteral.Text = string.Empty;
                threeMonthAgingLiteral.Text = string.Empty;
                balanceLiteral.Text = string.Empty;
                companyIDLiteral.Text = string.Empty;
                GSTLiteral.Text = string.Empty;

                // Show Add Customer Note Link
                AddCustomerNoteContentPlaceholder.Visible = false;
                EditCustomerInformationButton.Visible = false;
                customerInformationUpdatePanel.Update();
            }

            if (customerInformationPanel != null)
            {
                if (SessionWrapper.Instance.Get != null && SessionWrapper.Instance.Get.IsCustomerSelected == false)
                    customerInformationPanel.Visible = false;
                else
                    customerInformationPanel.Visible = true;
            }
        }

        public void SetFocusToCustomer()
        {
            CustomerSearch.Focus();
        }

        public void SetFocusToForm()
        {
            showContactDetailsLink.Focus();
        }

        public void DisplayClientNameAndId()
        {
            if (SessionWrapper.Instance.Get != null)
            {
                bool bValue = ClientSearch.Disabled;
                ClientSearch.Disabled = false;
                ClientSearch.Value = SessionWrapper.Instance.Get.ClientFromQueryString.NameAndNumber;
                ClientSearch.Disabled = bValue;
            }
        }

        public void DisplayClientInformationAndAgeingBalances(ClientInformationAndAgeingBalances clientInformationAndAgeingBalances, decimal limit, decimal available)
        {
            if (customerInformationUpdatePanel != null)
            {
                clientInformationPanel.Visible = true;
                customerInformationPanel.Visible = false;

                // Ageing
                AgeingBalances ageingBalances = clientInformationAndAgeingBalances.AgeingBalances;
                CurrentLiteral.Text = String.Format("{0:C}", ageingBalances.Current);
                oneMonthAgingLiteral.Text = String.Format("{0:C}", ageingBalances.OneMonthAgeing);
                twoMonthAgingLiteral.Text = String.Format("{0:C}", ageingBalances.TwoMonthAgeing);
                threeMonthAgingLiteral.Text = String.Format("{0:C}", ageingBalances.ThreeMonthPlusAgeing);
                balanceLiteral.Text = String.Format("{0:C}", ageingBalances.Balance);

                // Client information
                ClientInformation clientInformation = clientInformationAndAgeingBalances.ClientInformation;

                //ttttt CurrentACCreditLimitCust_Client CurrentACLimitClient CurrentACAvailableClient
                if (clientInformation.FacilityType == "Current A/c") //or see theFacilityType above
                {
                    CurrentACLimitClientTR.Visible = true;
                    CALimitClientLiteral.Text = String.Format("{0:C}", limit);
                    CurrentACAvailableClientTR.Visible = true;
                    CurrentACAvailableClientLiteral.Text = String.Format("{0:C}", available);
                    CurrentACCreditLimitCust_ClientTR.Visible = true;
                    //CurrentAccountCustLimitSum = the sum of the customer account limits!! shold only be 1 cust on C/A ledger!!
                    CurrentACCreditLimitCust_ClientLiteral.Text = String.Format("{0:C}", clientInformation.CurrentAccountCustLimitSum);
                }

                clientQueriesLiteral.Text = clientInformation.ClientQueries.ToString();
                //MSarza [20150901]
                //administeredByLiteral.Text = clientInformation.AdministeredBy;
                //administeredByLiteral.Text = CffDebtorAdminHelper.GetEnumDescription1((CffDebtorAdmin)clientInformation.DebtorAdministrationType);
                administeredByLiteral.Text = CffDebtorAdminHelper.GetEnumDescription2((CffDebtorAdmin)clientInformation.DebtorAdministrationType);

                createdLiteral.Text = clientInformation.Created.ToString();
                creditTermsLiteral.Text = clientInformation.CreditTerms;
                individualCustomerTermsLiteral.Text = clientInformation.IndividualCustomerTerms;
                individualInvoiceTermsLiteral.Text = clientInformation.IndividualInvoiceTerms;
                facilityTypeLiteral.Text = clientInformation.FacilityType;
                clientNumberClientPanelLiteral.Text = clientInformation.ClientNumber.ToString();
                companyNumberLiteral.Text = clientInformation.CompanyNumber.ToString();
                clientGstLiteral.Text = clientInformation.GstNumber;

                // Current month summary
                TransactionSummary transactionSummary = clientInformationAndAgeingBalances.TransacitonSummary;
                invoicesLiteral.Text = String.Format("{0:C}", transactionSummary.Invoices);
                receiptsLiteral.Text = String.Format("{0:C}", transactionSummary.Receipts);
                creditsLiteral.Text = String.Format("{0:C}", transactionSummary.Credits);
                discountsLiteral.Text = String.Format("{0:C}", transactionSummary.Discounts);
            }
        }

        public void DisplayCustomerNameAndClientNameInSearchBox()
        {
            ICffCustomer xCustomer = null;
            string sClientID = QueryString.ClientId.ToString();
            if (SessionWrapper.Instance.Get != null)
            {
                bool bValue = ClientSearch.Disabled;
                ClientSearch.Disabled = false;
                ClientSearch.Value = SessionWrapper.Instance.Get.ClientFromQueryString.NameAndNumber;
                ClientSearch.Disabled = bValue;
                xCustomer = SessionWrapper.Instance.Get.CustomerFromQueryString;
                sClientID = SessionWrapper.Instance.Get.ClientFromQueryString.Id.ToString();
            }
        
            //CustomerSearch.Value = SessionWrapper.Instance.Get..Customer.NameAndNumber;
            if (xCustomer == null)
            {
                CustomerSearch.Value = "";
            }
            else 
            {
                CustomerSearch.Value = xCustomer.NameAndNumber;
                Response.Redirect("?Client=" + sClientID + "&Customer=" + xCustomer.Id +"&ViewID=" + QueryString.ViewIDValue);
            }

        }

        public void ToggleEditNextCallDueDateButton(bool visible)
        {
            if (EditCustomerInformationButton != null)
            {
                EditCustomerInformationButton.Visible = visible;
            }
        }

        public void DisplayCustomerInformation(ClientAndCustomerInformation clientAndCustomerInformation, string facilityType, decimal limit, decimal available)
        {
            if (Page.Title.ToLower().Contains("dashboard"))
            {
                custAsAtPanel.Visible = true;
                populateAsAtList();
            }
            else {
                custAsAtPanel.Visible = false;
            }       

           if (customerInformationUpdatePanel != null)
           {
                clientInformationPanel.Visible = false;
                customerInformationPanel.Visible = true;

                if (stopCreditLiteral != null)
                {
                    //Or retrieve facility type from ClientCustomerInformation Class
                    //CffClient theClientInformation = clientAndCustomerInformation.CffClientInformation.CffClient;
                    //int theFacilityType = theClientInformation.ClientFacilityType
                    CffCustomerInformation customerInformation = clientAndCustomerInformation.CffCustomerInformation;

                    // Ageing
                    AgeingBalances ageingBalances = customerInformation.AgeingBalances;
                    CurrentLiteral.Text = String.Format("{0:C}", ageingBalances.Current);
                    oneMonthAgingLiteral.Text = String.Format("{0:C}", ageingBalances.OneMonthAgeing);
                    twoMonthAgingLiteral.Text = String.Format("{0:C}", ageingBalances.TwoMonthAgeing);
                    threeMonthAgingLiteral.Text = String.Format("{0:C}", ageingBalances.ThreeMonthPlusAgeing);
                    balanceLiteral.Text = String.Format("{0:C}", ageingBalances.Balance);

                    // Customer Information
                    if (facilityType == "Current A/c") //or see theFacilityType above
                    {
                        CurrentACLimit.Visible = true;
                        CALimit.Text = String.Format("{0:C}", limit);
                        CurrentACAvailable.Visible = true;
                        CAAvailable.Text = String.Format("{0:C}", available);
                    }
                    stopCreditLiteral.Text = customerInformation.StopCredit;
                    creditLimitLiteral.Text = String.Format("{0:C}", customerInformation.CreditLimit);
                    nextCallLiteral.Text = nextCallTextBox.Text = customerInformation.NextCall.ToString();

                    allowCallsLiteral.Text = clientAndCustomerInformation.AllowCalls;

                    lastPaidLiteral.Text = customerInformation.LastPaid.ToString();
                    lastAmountLiteral.Text = String.Format("{0:C}", Math.Abs(customerInformation.LastAmount));
                    ListDateLiteral.Text = customerInformation.ListDate.ToString();

                    termsLiteral.Text = customerInformation.ClientCustomerTerm.GetCustomerTerms();
                    clientNumberLiteral.Text =
                        clientAndCustomerInformation.CffClientInformation.CffClient.Number.ToString();

                    customerNumberLiteral.Text = customerInformation.Customer.Number.ToString();
                    CffIdLiteral.Text = customerInformation.Customer.Id.ToString();

                    //TODO Add Comapny ID and GST # which are not in the current database.
                    companyIDLiteral.Text = "#";
                    GSTLiteral.Text = "#";

                    // Show Add Customer Note Link
                    AddCustomerNoteContentPlaceholder.Visible = false;  // true   //dbb
                    customerInformationUpdatePanel.Update();
                }
            }
        }
 #endregion

        public event EventHandler<EventArgs> ScopeChanged;

        private void ValidateSessionInstance(string strRawURL="")
        {
            int clientID = 0;
            CffPrincipal cffPrincipal = Context.User as CffPrincipal;
            strRawURL= string.IsNullOrEmpty(strRawURL) ? Request.RawUrl : strRawURL;

            if (QueryString.ClientId != null)
                clientID = Convert.ToInt32(QueryString.ClientId.ToString());
            else if (strRawURL.Contains("Client"))
            {
                int cidx = strRawURL.IndexOf("Client=") + 7;
                int eidx = strRawURL.IndexOf('&', cidx);
                strRawURL = strRawURL.Substring(cidx, (eidx - cidx));
                if (int.TryParse(strRawURL, out clientID))
                    clientID = string.IsNullOrEmpty(strRawURL)?0:Convert.ToInt32(strRawURL);
            }
            else
                clientID = 0;

            if (clientID <= 0) { //QueryString is missing/deleted 
                clientID = Convert.ToInt32((System.Web.HttpContext.Current.User as CffPrincipal).CffUser.ClientId.ToString());
            }
            
            if (SessionWrapper.Instance.IsSessionValid) {
                 if ((cffPrincipal.CffUser.UserType == UserType.EmployeeStaffUser)
                          || (cffPrincipal.CffUser.UserType == UserType.EmployeeManagementUser)
                                || (cffPrincipal.CffUser.UserType == UserType.EmployeeAdministratorUser))
                 {
                    //generate a new viewID key and save this as new session 
                     string oldViewID="";
                     int oldCustomerID = 0;
                     if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                     { //get values from previous sessionview
                         oldViewID = QueryString.ViewIDValue;
                         if (QueryString.CustomerId != null)
                             oldCustomerID = (int)QueryString.CustomerId;
                         else if (SessionWrapper.Instance.GetSession(oldViewID).CustomerFromQueryString != null)
                             oldCustomerID = SessionWrapper.Instance.GetSession(oldViewID).CustomerFromQueryString.Id;
                         
                         if (SessionWrapper.Instance.GetSession(oldViewID).ClientFromQueryString!=null)
                            clientID = SessionWrapper.Instance.GetSession(oldViewID).ClientFromQueryString.Id;
                     }

                    //GENERATE NEW SESSION VIEW ID VALUE and REDIRECT
                    string viewID = SaferTrader.Core.Common.StringEnum.GenerateUniqueKey(12);
                    SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(clientID);
                    SessionWrapper.Instance.GetSession(viewID).UserIdentity = 1;
                    SessionWrapper.Instance.GetSession(viewID).IsStartsWithChecked = true;
                    SessionWrapper.Instance.GetSession(viewID).CurrentUserID = (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString();
              
                    string rUrl = ResolveClientUrl(Context.Request.Url.AbsolutePath + "?Client=" + clientID.ToString() + "&User=" + cffPrincipal.CffUser.EmployeeId + "&ViewID=" + viewID);
                    if (oldCustomerID > 0 && clientID>0)
                    {
                        if (oldCustomerID == QueryString.CustomerId)
                        {
                            CffCustomer cCustomer = RepositoryFactory.CreateCustomerRepository().GetMatchedCustomerInfo(oldCustomerID, clientID).CffCustomerInformation.Customer;
                            SessionWrapper.Instance.GetSession(viewID).CustomerFromQueryString = (ICffCustomer)(new CffCustomerExt(cCustomer.Name, cCustomer.Id, cCustomer.Number));
                            rUrl += "&Customer=" + oldCustomerID.ToString();
                        }
                    }

                    if (clientID > 0) {
                        if (QueryString.CustomerId != null)
                        {
                            if (oldCustomerID != QueryString.CustomerId)
                            {
                                CffCustomer cCustomer = RepositoryFactory.CreateCustomerRepository().GetMatchedCustomerInfo((Convert.ToInt32(QueryString.CustomerId)), clientID).CffCustomerInformation.Customer;
                                SessionWrapper.Instance.GetSession(viewID).CustomerFromQueryString = (ICffCustomer)(new CffCustomerExt(cCustomer.Name, cCustomer.Id, cCustomer.Number));
                                rUrl += "&Customer=" + QueryString.CustomerId.ToString();
                            }
                        }
                        else if (!rUrl.Contains("Customer") && this.Page.Request.QueryString.ToString().Contains("Customer"))
                        {
                            rUrl += "&Customer=" + System.Web.HttpContext.Current.Request.QueryString[QueryString.Customer.ToString()].Replace("(", "").Replace(")", "");
                        }
                    }
                
                    if (QueryString.CriteriaValue != null)
                    {
                        rUrl += "&Criteria=" + QueryString.CriteriaValue.ToString().Replace("#", "");
                    }

                    if (QueryString.ClientId == 0)
                        ClientSearch.Value = SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString.NameAndNumber;

                    if (this.Page.Request.QueryString.ToString().Contains("Batch") && clientID > 0)
                    {
                        if (System.Web.HttpContext.Current.Request.RawUrl.Contains("InvoiceBatches.aspx"))
                            rUrl += "&Batch=" + System.Web.HttpContext.Current.Request.QueryString[QueryString.Batch.ToString()];
                    }
                    this.Response.Redirect(rUrl);
                 }
                 else
                 {
                   Boolean bRetainClientID = false;
                   Boolean bIsMultipleAccounts = false;

                   if (cffPrincipal.CffUser.UserType == UserType.ClientManagementUser ||
                             cffPrincipal.CffUser.UserType == UserType.ClientStaffUser ||
                                 cffPrincipal.CffUser.UserType == UserType.CustomerUser )
                   { //validate clientid (client/user may manually input this id)
                       //if user is of multiple accounts, validate if this client id is valid for this user
                       List<UserSpecialAccounts> accounts = new List<UserSpecialAccounts>();
                       accounts = RepositoryFactory.CreateUserClientsRepository().GetSpecialAccountAccessByID((System.Web.HttpContext.Current.User as CffPrincipal).CffUser.EmployeeId);
                       if (accounts != null) {
                          bIsMultipleAccounts = true;
                          foreach (UserSpecialAccounts xAcct in accounts)
                              if (clientID == Convert.ToInt32(xAcct.cId.ToString()))
                              {
                                  bRetainClientID = true;
                                  break;
                              }
                       }

                       if ((!bRetainClientID) && (clientID != Convert.ToInt32((System.Web.HttpContext.Current.User as CffPrincipal).CffUser.ClientId.ToString())))  
                           clientID = Convert.ToInt32((System.Web.HttpContext.Current.User as CffPrincipal).CffUser.ClientId.ToString());
                   }

                   //check if there exists previous session instance
                   string viewID = SaferTrader.Core.Common.StringEnum.GenerateUniqueKey(12);
                   SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(clientID);
                   SessionWrapper.Instance.GetSession(viewID).UserIdentity = 1;
                   SessionWrapper.Instance.GetSession(viewID).IsStartsWithChecked = true;
                   SessionWrapper.Instance.GetSession(viewID).CurrentUserID = (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString();
                   SessionWrapper.Instance.GetSession(viewID).IsMultipleAccounts = bIsMultipleAccounts;

                   string rUrl = ResolveClientUrl(Context.Request.Url.AbsolutePath + "?Client=" + clientID.ToString() + "&User=" + cffPrincipal.CffUser.EmployeeId + "&ViewID=" + viewID);
                   if (QueryString.CustomerId != null)
                   {
                       ClientAndCustomerInformation matchedCustInfo = RepositoryFactory.CreateCustomerRepository().GetMatchedCustomerInfo((int)QueryString.CustomerId, clientID);
                       SessionWrapper.Instance.GetSession(viewID).CustomerFromQueryString = (ICffCustomer)(matchedCustInfo.CffCustomerInformation.Customer as IEnumerable<object>);
                       rUrl += "&Customer=" + QueryString.CustomerId.ToString();
                   }
                   else if (this.Page.Request.QueryString.ToString().Contains("Customer"))
                   {
                       rUrl += "&Customer=" + System.Web.HttpContext.Current.Request.QueryString[QueryString.Customer.ToString()].Replace("(", "").Replace(")", "");
                   }

                     
                   if (QueryString.Criteria != null)
                   {
                        rUrl += "&Criteria=" + QueryString.CriteriaValue.ToString().Replace("#", "");
                   }

                   if (this.Page.Request.QueryString.ToString().Contains("Batch"))
                       rUrl += "&Batch=" + System.Web.HttpContext.Current.Request.QueryString[QueryString.Batch.ToString()];

                   bool bValue = ClientSearch.Disabled;
                   this.ClientSearch.Disabled = false;
                   this.ClientSearch.Value = SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString.NameAndNumber;
                   this.ClientSearch.Disabled = bValue;

                   if (QueryString.ClientId == 0)
                       ClientSearch.Value = SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString.NameAndNumber;

                   this.Response.Redirect(rUrl);
                }
           }
           else
           { //redirect to logon page - allow only one window tab instance to open, unless it came from the reports tab etc
                string rUrl = ResolveClientUrl("~/Logon.aspx");
                this.Response.Redirect(rUrl);
           }
        }

        protected void Page_Init(object sender, EventArgs e)
        { //todo: investigate why page_init is called 2x
            if (!IsPostBack)
            {
                const int timeoutLeadTime = 2;
                int timeoutInMinutes;
                CffPrincipal cffPrincipal = Context.User as CffPrincipal;
                if (cffPrincipal != null)
                {
                    Session.Timeout = 3600;
                    string tUrl = this.Context.Request.RawUrl;
                    string sSessionID = "";

                    if (cffPrincipal.Identity.IsAuthenticated && SessionWrapper.Instance.Get == null)
                    { //this might be because the user have clicked on a link from the page; check if in customer scope and sessionwrapper customerid = querystringcustomerid
                        if (!(string.IsNullOrEmpty(QueryString.ViewIDValue)) && cffPrincipal.Identity.IsAuthenticated ) {
                            ValidateSessionInstance(tUrl);
                        }

                        if (string.IsNullOrEmpty(QueryString.ViewIDValue)) {
                            if ((QueryString.UserId != null) && (cffPrincipal.CffUser.EmployeeId != QueryString.UserId))
                            {
                                sSessionID = SessionWrapper.Instance.Get.SessionID.Split('_')[0];
                                if ((cffPrincipal.CffUser.EmployeeId != QueryString.UserId) &&  (sSessionID != Context.Session.SessionID))
                                {
                                    SessionWrapper.Instance.Get.ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(cffPrincipal.CffUser.ClientId));
                                    SessionWrapper.Instance.Get.UserIdentity = (short)cffPrincipal.CffUser.UserType.Id;
                                    string rUrl = ResolveClientUrl("~/Dashboard.aspx?Client=" + CurrentPrincipal.CffUser.ClientId + "&User=" + cffPrincipal.CffUser.EmployeeId 
                                                                    + "&ViewID=" + QueryString.ViewIDValue);
                                    if (QueryString.CustomerId != null)
                                        rUrl += "&CustomerId=" + QueryString.CustomerId.ToString();

                                    this.Response.Redirect(rUrl);
                                }
                            }
                        }
                        else {
                            string oldViewIDValue = QueryString.ViewIDValue;
                            if (cffPrincipal.Identity.IsAuthenticated && SessionWrapper.Instance.GetSession(oldViewIDValue) == null)
                                ValidateSessionInstance(Request.RawUrl);

                            if ((QueryString.UserId != null) && (cffPrincipal.CffUser.EmployeeId != QueryString.UserId))
                            {
                                sSessionID = SessionWrapper.Instance.GetSession(oldViewIDValue).SessionID.Split('_')[0];
                                if ((cffPrincipal.CffUser.EmployeeId != QueryString.UserId) && (sSessionID != Context.Session.SessionID))
                                {
                                    SessionWrapper.Instance.GetSession(oldViewIDValue).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(cffPrincipal.CffUser.ClientId));
                                    SessionWrapper.Instance.GetSession(oldViewIDValue).UserIdentity = (short)cffPrincipal.CffUser.UserType.Id;
                                    string rUrl = ResolveClientUrl("~/Dashboard.aspx?Client=" + CurrentPrincipal.CffUser.ClientId + "&User=" + cffPrincipal.CffUser.EmployeeId 
                                                        + "&ViewID=" + QueryString.ViewIDValue);
                                    if (QueryString.CustomerId != null)
                                        rUrl += "&CustomerId=" + QueryString.CustomerId.ToString();
                                    this.Response.Redirect(rUrl);
                                }
                            }
                        }
                    }
                }

                timeoutInMinutes = Session.Timeout;
                timeout = timeoutInMinutes * 60000;
                timeoutWarning = (timeoutInMinutes - timeoutLeadTime) * 60000;
                logOnURL = ResolveUrl("~/EndSession.aspx");
                keepAliveURL = ResolveUrl("~/KeepSessionAlive.aspx");

                string sViewID = QueryString.ViewIDValue;
                if (SessionWrapper.Instance.Get != null)
                {
                    if (SessionWrapper.Instance.Get.IsAllClientsSelected == true)
                    {
                        hideEditClientContactTextBoxes();
                        LinkBtnEditClientContact.Visible = false;
                        LinkBtnCancelEditContact.Visible = false;

                        hideEditCustomerContactTextBoxes();
                        LinkBtnEditCustomerContact.Visible = false;
                        LinkBtnCancelEditCustomerContact.Visible = false;
                        literalContactCustomerID.Visible = false;
                    }
                    else if (cffPrincipal != null)
                    {
                        string sSessionID = SessionWrapper.Instance.Get.SessionID.Split('_')[0];
                        if ((sSessionID != Context.Session.SessionID) && (Request.RawUrl.Contains("Dashboard.aspx")))
                        { //check if new tab
                            //string rUrl = ResolveClientUrl("~/Dashboard.aspx?Client=" + CurrentPrincipal.CffUser.ClientId + "&User=" + cffPrincipal.CffUser.EmployeeId + "&ViewID=" + QueryString.ViewIDValue);
                            string rUrl = ResolveClientUrl(Request.RawUrl);
                            string tUrl = this.Context.Request.RawUrl;
                        }
                    }
                } else if (!string.IsNullOrEmpty(sViewID)) {
                    if (SessionWrapper.Instance.GetSession(sViewID) != null) {
                        if (SessionWrapper.Instance.GetSession(sViewID).IsAllClientsSelected == true)
                        {
                            hideEditClientContactTextBoxes();
                            LinkBtnEditClientContact.Visible = false;
                            LinkBtnCancelEditContact.Visible = false;

                            hideEditCustomerContactTextBoxes();
                            LinkBtnEditCustomerContact.Visible = false;
                            LinkBtnCancelEditCustomerContact.Visible = false;
                            literalContactCustomerID.Visible = false;
                        }
                        else if (cffPrincipal != null)
                        {
                            string sSessionID = SessionWrapper.Instance.GetSession(sViewID).SessionID.Split('_')[0];
                            if ((sSessionID!= Context.Session.SessionID) && (Request.RawUrl.Contains("Dashboard.aspx")))
                            { //check if new tab
                                //string rUrl = ResolveClientUrl("~/Dashboard.aspx?Client=" + CurrentPrincipal.CffUser.ClientId + "&User=" + cffPrincipal.CffUser.EmployeeId + "&ViewID=" + QueryString.ViewIDValue);
                                string rUrl = ResolveClientUrl(Request.RawUrl);
                                string tUrl = this.Context.Request.RawUrl;
                            }
                        }
                    }
                
                }
                //UserClientSelection.SelectedIndexChanged += UserClientSelection_SelectedIndexChanged;
                //UserClientSelection.AutoPostBack = true;
            }
        }

        protected void UserClientSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Page.IsPostBack)
            {
                string sub = "";
                var newclient = UserClientSelection.SelectedItem;
                string oldpath = Request.Url.PathAndQuery;
                int startIndx = oldpath.IndexOf("Client=") + 7;
                int endIndx = oldpath.IndexOf('&', startIndx);
                if (endIndx > 0)
                {// this contain another params
                    sub = oldpath.Substring(startIndx, endIndx - startIndx);
                }
                else
                {// clientid is only param
                    sub = oldpath.Substring(startIndx, oldpath.Length - startIndx);
                }
                string query = oldpath.Replace(sub, newclient.Value);
                startIndx = query.IndexOf("Customer=");
                if (startIndx >= 0)
                { //clear customer query string
                    endIndx = query.IndexOf('&', startIndx);
                    if (query[startIndx - 1] == '&') startIndx -= 1;

                    sub = (endIndx > 0) ? query.Substring(startIndx, endIndx - startIndx) : query.Substring(startIndx);
                    query = query.Replace(sub, "");
                }
                string newurl = query;
                string sClientNameAndNumber = "";
                SessionWrapper.Instance.EmptyWindowHit = 0; //reset this for the jquery
                if (SessionWrapper.Instance.Get!=null) {  //change the clientid of the current session instance
                    SessionWrapper.Instance.Get.ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(newclient.Value));
                    SessionWrapper.Instance.Get.EmptyWindowHit = 0; //reset this for the jquery
                    SessionWrapper.Instance.Get.CustomerFromQueryString = null;
                    SessionWrapper.Instance.Get.CustomerFromQueryString = null;
                    SessionWrapper.Instance.Get.MultiClientSelected = true;
                    SessionWrapper.Instance.Get.IsStartsWithChecked = true;
                    SessionWrapper.Instance.Get.CurrentUserID = (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString();

                    sClientNameAndNumber = SessionWrapper.Instance.Get.ClientFromQueryString.NameAndNumber;
                } else if (!string.IsNullOrEmpty(QueryString.ViewIDValue)) {
                    //change the clientid of the current viewid value session instance
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(newclient.Value));
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).EmptyWindowHit = 0; //reset this for the jquery
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString = null;
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString = null;
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).MultiClientSelected = true;
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsStartsWithChecked = true;
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CurrentUserID = (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString();
                    sClientNameAndNumber = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.NameAndNumber;
                }
                else
                    sClientNameAndNumber = (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                                             ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.NameAndNumber
                                                : SessionWrapper.Instance.Get.ClientFromQueryString.NameAndNumber;

                bool bValue = ClientSearch.Disabled;
                this.ClientSearch.Disabled = false;
                this.ClientSearch.Value = sClientNameAndNumber;
                this.CustomerSearch.Value = "";
                this.ClientSearch.Disabled = bValue;
                
                newurl += "&clienthit";
                //make sure that clientsearch selects the correct client
                RedirectTo(newurl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bool bIsClientSelected = false;
            bool bIsAllClientsSelected = false;
            bool bIsStartsWithChecked = false;

            string sViewID = QueryString.ViewIDValue;
            int? sClientID = QueryString.ClientId;
            int? sCustID = QueryString.CustomerId;
            string sClientNameAndNumber  = (sClientID==-1)?"All Clients (-1)":"";
            bool isMultipleAccounts = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.IsMultipleAccounts :
                                                              (SessionWrapper.Instance.GetSession(sViewID) != null) ? SessionWrapper.Instance.GetSession(sViewID).IsMultipleAccounts : false;

            if (!Page.IsPostBack) 
            {
                if (SessionWrapper.Instance.Get == null && string.IsNullOrEmpty(sViewID))
                    ValidateSessionInstance(Context.Request.RawUrl);

                if (this.CurrentScope() == Scope.AllClientsScope)
                { //check if user is CFF and valid for All clients scope otherwise for to client scope 
                    if (((CffPrincipal)Context.User).IsInClientRole || ((CffPrincipal)Context.User).IsInCustomerRole)
                    {
                        sClientNameAndNumber = SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString.NameAndNumber;
                    }
                    else if ((((Cff.SaferTrader.Core.CffPrincipal)(Context.User)).CffUser).UserType.Name == "Administrator")
                    {
                        SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId((int)((QueryString.ClientId == null) ? -1 : QueryString.ClientId));
                        sClientNameAndNumber = SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString.NameAndNumber;

                        if (SessionWrapper.Instance.Get != null) {
                            SessionWrapper.Instance.Get.ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId((int)((QueryString.ClientId == null) ? -1 : QueryString.ClientId));
                        }
                    }
                }
                else
                {
                    if (sClientID != SessionWrapper.Instance.Get.ClientFromQueryString.Id)
                    {
                        if ((((Cff.SaferTrader.Core.CffPrincipal)(Context.User)).CffUser).UserType.Name == "Administrator" && sClientID != null)
                        {
                            SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId((int)sClientID);
                            sClientNameAndNumber = SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString.NameAndNumber;
                        }
                        else
                        {
                            if (isMultipleAccounts) {
                                if (SessionWrapper.Instance.Get!=null)
                                    SessionWrapper.Instance.Get.ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(sClientID));
                                else if (QueryString.ViewIDValue!=null)
                                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(sClientID));
                            }

                            sClientNameAndNumber = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString.NameAndNumber :
                                                                  (SessionWrapper.Instance.GetSession(sViewID) != null) ? SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString.NameAndNumber : "";
                        }
                   }
                   else
                        sClientNameAndNumber = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString.NameAndNumber :
                                                    (SessionWrapper.Instance.GetSession(sViewID) != null) ? SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString.NameAndNumber : "";
                }
            }
            else
            {
                var idx = UserClientSelection.SelectedIndex;
                var value = UserClientSelection.SelectedValue;

                if (SessionWrapper.Instance.Get != null)
                    SessionWrapper.Instance.Get.IsStartsWithChecked = this.StartsWith.Checked;  //re-assigned property value to sessionwrapper since querystring is readonly; 
                else if (!string.IsNullOrEmpty(sViewID))
                    SessionWrapper.Instance.GetSession(sViewID).IsStartsWithChecked = this.StartsWith.Checked;

                if (!String.IsNullOrEmpty(value.ToString()))
                { //user is a client with multiple accounts
                    bool bValue = this.ClientSearch.Disabled;

                    if (SessionWrapper.Instance.Get != null)
                    {
                        if ((((CffPrincipal)Context.User).IsInClientRole) && ((Convert.ToInt32(value)) == -1))
                        {  //throw error;
                            SessionWrapper.Instance.Get.ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(0));
                        }
                        else
                        {
                            if (SessionWrapper.Instance.Get.ClientFromQueryString.Id != Convert.ToInt32(value))
                            {
                                SessionWrapper.Instance.Get.ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(value));
                                SessionWrapper.Instance.EmptyWindowHit = 0;
                                SessionWrapper.Instance.Get.CustomerFromQueryString = null;
                                SessionWrapper.Instance.Get.MultiClientSelected = true;
                                this.CustomerSearch.Value = "";
                                sClientNameAndNumber = SessionWrapper.Instance.Get.ClientFromQueryString.NameAndNumber;
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(sViewID))
                    {
                        if ((((CffPrincipal)Context.User).IsInClientRole) && ((Convert.ToInt32(value)) == -1))
                        {  //throw error;
                            SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(0));
                        }
                        else
                        {
                            if (SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString.Id != Convert.ToInt32(value))
                            {
                                SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(value));
                                SessionWrapper.Instance.EmptyWindowHit = 0;
                                SessionWrapper.Instance.GetSession(sViewID).CustomerFromQueryString = null;
                                SessionWrapper.Instance.GetSession(sViewID).MultiClientSelected = true;
                                this.CustomerSearch.Value = "";
                            }
                        }
                    }
                  
                    bValue = this.ClientSearch.Disabled;
                    this.ClientSearch.Disabled = false;
                    this.ClientSearch.Value  = sClientNameAndNumber;
                    this.ClientSearch.Disabled = bValue;
                }
            }

            if (QueryString.CriteriaValue != null)
            {
                this.searchCriteriaOptions.SelectedIndex = (int)QueryString.CriteriaValue;
                this.SearchCriteria.Value = QueryString.CriteriaValue.ToString();
            } 
            else if (Request.Url.ToString().Contains("Criteria"))
            {
                string criterialURl= Request.Url.ToString().Substring(Request.Url.ToString().IndexOf("Criteria"));
                if (criterialURl.Length > 10) { 
                    //get the criteriavalue here
                }
            }

            try
            {
                CustomerNotesAdderModalBox.NextCallDueUpdated += CustomerNotesAdderModalBoxRefreshCustomerInformation;
                footer.Attributes.Add("title", Assembly.GetExecutingAssembly().GetName().Version.ToString());

                if (this.ClientSearch.Value != sClientNameAndNumber)
                {
                    bool bValue = this.ClientSearch.Disabled;
                    this.ClientSearch.Disabled = false;
                    this.ClientSearch.Value = sClientNameAndNumber;
                    this.ClientSearch.Disabled = bValue;
                }

                ISecurityManager securityManager = SecurityManagerFactory.Create(CurrentPrincipal, this.CurrentScope());
                presenter = new SafeTraderPresenter(this,
                                                    RepositoryFactory.CreateCustomerRepository(),
                                                    securityManager,
                                                    RedirectionService.Create(this, securityManager),
                                                    CffUserService.Create(),
                                                    CurrentPrincipal);

                //we first get the clientid and customerid from sessionwrapper as these babies needs to be attached with the existing session view id
                if (SessionWrapper.Instance.Get != null) {
                    bIsClientSelected = SessionWrapper.Instance.Get.IsClientSelected;
                    bIsAllClientsSelected = SessionWrapper.Instance.Get.IsAllClientsSelected;

                    if (((CffPrincipal)Context.User).IsInClientRole  && !isMultipleAccounts)
                    {
                        int clientID = Convert.ToInt32(((CffPrincipal)Context.User).CffUser.ClientId.ToString());
                        if (SessionWrapper.Instance.Get.ClientFromQueryString == null && QueryString.ClientId <= 0)
                        { //throw error
                            SessionWrapper.Instance.Get.ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(0);
                        }
                        if (SessionWrapper.Instance.Get.ClientFromQueryString == null && QueryString.ClientId > 0)
                        {
                            clientID = Convert.ToInt32(QueryString.ClientId.ToString());
                            SessionWrapper.Instance.Get.ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(clientID);
                        }

                        presenter.LockDown(clientID,
                            ((SessionWrapper.Instance.Get.CustomerFromQueryString == null) ? QueryString.CustomerId : SessionWrapper.Instance.Get.CustomerFromQueryString.Id));
                        
                        sClientID = clientID;
                    }
                    else {
                        presenter.LockDown(SessionWrapper.Instance.Get.ClientFromQueryString.Id,
                            ((SessionWrapper.Instance.Get.CustomerFromQueryString == null) ? QueryString.CustomerId : SessionWrapper.Instance.Get.CustomerFromQueryString.Id));
                        sClientID = SessionWrapper.Instance.Get.ClientFromQueryString.Id;
                    }

                    sClientNameAndNumber = SessionWrapper.Instance.Get.ClientFromQueryString.NameAndNumber;
                    if (string.IsNullOrEmpty(this.ClientSearch.Value))
                        this.ClientSearch.Value = sClientNameAndNumber;

                    bIsStartsWithChecked = SessionWrapper.Instance.Get.IsStartsWithChecked;
                }
                else if (!string.IsNullOrEmpty(sViewID))
                {
                    bIsClientSelected = SessionWrapper.Instance.GetSession(sViewID).IsClientSelected;
                    bIsAllClientsSelected = SessionWrapper.Instance.GetSession(sViewID).IsAllClientsSelected;

                    if (((CffPrincipal)Context.User).IsInClientRole)
                    {
                        int clientID = 0;
                        if (SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString == null && QueryString.ClientId <= 0)
                        { //throw error
                            SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(0);
                        }
                        if (SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString == null && QueryString.ClientId > 0)
                        {
                            clientID = Convert.ToInt32(QueryString.ClientId.ToString());
                            SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(clientID);
                        }
                        presenter.LockDown(clientID,
                            ((SessionWrapper.Instance.GetSession(sViewID).CustomerFromQueryString == null) ? QueryString.CustomerId : SessionWrapper.Instance.GetSession(sViewID).CustomerFromQueryString.Id));
                    }
                    else
                    {
                        presenter.LockDown(SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString.Id,
                             ((SessionWrapper.Instance.GetSession(sViewID).CustomerFromQueryString == null) ? QueryString.CustomerId : SessionWrapper.Instance.GetSession(sViewID).CustomerFromQueryString.Id));
                    }

                    sClientID = SessionWrapper.Instance.GetSession(sViewID).ClientFromQueryString.Id;
                    bIsStartsWithChecked = SessionWrapper.Instance.GetSession(sViewID).IsStartsWithChecked;
                }
                else
                    presenter.LockDown(QueryString.ClientId, QueryString.CustomerId);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
           
            if (!Page.IsPostBack)
            {
                InitializeForCurrentUser();
                RefreshCustomerInformation(bIsClientSelected); 
                InitializeEditCustomerInformation();
            }

            InitializeCurrentPathForJavaScript();

            if (nextCallTextBox != null)
            {
                nextCallTextBox.Attributes.Add("readonly", "readonly");
            }

            if (this.CurrentScope() == Scope.AllClientsScope)
            {
              HideRightSidePanel();
            }
            else if (this.CurrentScope() == Scope.CustomerScope)
            {
              customerSelection.Value = "CustomerSelected";
            }

            if (bIsAllClientsSelected == true)
            {
              hideEditClientContactTextBoxes();
              LinkBtnEditClientContact.Visible = false;
              LinkBtnCancelEditContact.Visible = false;

              hideEditCustomerContactTextBoxes();
              LinkBtnEditCustomerContact.Visible = false;
              LinkBtnCancelEditCustomerContact.Visible = false;
              literalContactCustomerID.Visible = false;
            }

            if (sClientID == -1)
            {
              showContactDetailsLink.Visible = false;
            }
           
           
            if (!Page.IsPostBack)
            {
                this.StartsWith.Checked = bIsStartsWithChecked;
            }

            UpdateMultipleClient(true);
        }

        private void UpdateMultipleClient(bool isAllowMultipleClients) 
        {
            if (!IsPostBack)
            {
                if (presenter != null && isAllowMultipleClients == true)
                {
                    List<UserSpecialAccounts> clientsSrc = presenter.LoadUserMultipleClients(RepositoryFactory.CreateUserClientsRepository());

                    if (clientsSrc.Count > 0)
                    {
                        UserClientSelection.DataTextField = "Name";
                        UserClientSelection.DataValueField = "cId";
                        UserClientSelection.DataSource = clientsSrc;
                        UserClientSelection.DataBind();

                        for (int index = 0; index < UserClientSelection.Items.Count; index++)
                        {
                            var client = UserClientSelection.Items[index];
                            if (Convert.ToInt64(client.Value) == QueryString.ClientId)
                            {
                                UserClientSelection.SelectedIndex = index;
                                break;
                            }
                        }


                        if (SessionWrapper.Instance.Get != null) {
                            if (!SessionWrapper.Instance.Get.IsMultipleAccounts)
                            {
                                string clientIDList = "";
                                for (int index = 0; index < UserClientSelection.Items.Count; index++)
                                    clientIDList += UserClientSelection.Items[index].Value.ToString() + ";";
                                SessionWrapper.Instance.Get.AccountsIDList = clientIDList;
                            }
                        }
                        else if (!string.IsNullOrEmpty(QueryString.ViewIDValue)) {
                            if (!SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsMultipleAccounts)
                            {
                                string clientIDList = "";
                                for (int index = 0; index < UserClientSelection.Items.Count; index++)
                                    clientIDList += UserClientSelection.Items[index].Value.ToString() + ";";
                                SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).AccountsIDList = clientIDList;
                            }
                        }
                    }
                    else
                    {
                        UserClientSelection.Visible = false;
                    }
                }
            }
        }

        private CffPrincipal CurrentPrincipal
        {
            get { return Context.User as CffPrincipal; }
        }

        private void CustomerNotesAdderModalBoxRefreshCustomerInformation(object sender, EventArgs e)
        {
            if (SessionWrapper.Instance.Get!=null)
                RefreshCustomerInformation(SessionWrapper.Instance.Get.IsClientSelected);
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    RefreshCustomerInformation(SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsClientSelected);

        }

        protected void AddCustomerNoteButtonClick(object sender, EventArgs e)
        {
            CustomerNotesAdderModalBox.Show();
        }


        protected void StartsWithClick(object sender, EventArgs e)
        {
            if (SessionWrapper.Instance.Get != null)
                SessionWrapper.Instance.Get.IsStartsWithChecked = this.StartsWith.Checked;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsStartsWithChecked = this.StartsWith.Checked;
        }


        protected void SearchButtonClick(object sender, ImageClickEventArgs e)
        {
            string sub = "";
            var newclient = "";
            int idx= UserClientSelection.SelectedIndex;


            if (idx >= 0) {
                newclient = UserClientSelection.Items[0].Value;
            }
            
            string oldpath = Request.Url.PathAndQuery;
            int startIndx = oldpath.IndexOf("Client=") + 7;
            int endIndx = oldpath.IndexOf('&', startIndx);
            if (endIndx > 0)
            {// this contain another params
                sub = oldpath.Substring(startIndx, endIndx - startIndx);
            }
            else
            {// clientid is only param
                sub = oldpath.Substring(startIndx, oldpath.Length - startIndx);
            }

            //string query = oldpath.Replace(sub, newclient.Value);
            //string newurl = query;
            //RedirectTo(newurl);
            ICffCustomer xCust = null;
            bool bIsMultiClientSelected = false;
            bool bIsClientSelected = false;
            if (SessionWrapper.Instance.Get != null)
            {
                xCust = SessionWrapper.Instance.Get.CustomerFromQueryString;
                bIsMultiClientSelected = SessionWrapper.Instance.Get.MultiClientSelected;
                bIsClientSelected = SessionWrapper.Instance.Get.IsClientSelected;
            } else if (!string.IsNullOrEmpty(QueryString.ViewIDValue)) {
                xCust = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString;
                bIsMultiClientSelected = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).MultiClientSelected;
                bIsClientSelected = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsClientSelected;
            }

            if (xCust!=null && bIsMultiClientSelected)
                presenter.LockDown(QueryString.ClientId, null);
            else
                presenter.LockDown(QueryString.ClientId, QueryString.CustomerId);

            RefreshCustomerInformation(bIsClientSelected);
            if (ScopeChanged != null)
            {
                ScopeChanged(this, EventArgs.Empty);
            }
        }

        protected void LogOnStatusLoggingOut(object sender, LoginCancelEventArgs e)
        {
            SessionWrapper.Instance.Clear();
        }

        public void HideRightSidePanel()
        {
            body.Attributes.Add("class", "rightSideHidden rhToggleHidden");
        }

        public void RefreshCustomerInformation(bool isClientSelected)
        {
            CffCustomer cxCust = null;
            ICffClient xClient = null;
            bool bIsCustomerSelected = false;
            bool bIsDeselectingCustomer = false;
            string sName = "";
            int ClientId = 0;

            if (SessionWrapper.Instance.Get != null) {
                bIsCustomerSelected = SessionWrapper.Instance.Get.IsCustomerSelected;
                sName = SessionWrapper.Instance.Get.ClientFromQueryString.GetType().Name;
                ClientId = SessionWrapper.Instance.Get.ClientFromQueryString.Id;
                bIsDeselectingCustomer = SessionWrapper.Instance.Get.IsDeselectingCustomer;
               
                cxCust = (CffCustomer)SessionWrapper.Instance.Get.CustomerFromQueryString;
                xClient =  SessionWrapper.Instance.Get.ClientFromQueryString;
            } 
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))  {
                bIsCustomerSelected = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsCustomerSelected;
                sName = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.GetType().Name;
                ClientId = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id;
                bIsDeselectingCustomer = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsDeselectingCustomer;

                cxCust = (CffCustomer)SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString;
                xClient = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString;
            }
           
            if (isClientSelected) //SessionWrapper.Instance.Get.IsClientSelected
            {
                if ((cxCust == null) || (xClient.Id <= 0)) {
                    presenter.LoadClientDetailsOnly(ClientId, bIsDeselectingCustomer);
                }
                else
                    presenter.LoadClientAndCustomerContact(cxCust, xClient);   // dbb
            }
            else if (bIsCustomerSelected) {
                if (sName == "AllClients") {
                    if (!(Context.User as CffPrincipal).IsInClientRole && (Context.User as CffPrincipal).IsInCustomerRole && QueryString.ClientId!=null) {
                            presenter.LoadClientAndCustomerContact(cxCust,
                                            RepositoryFactory.CreateClientRepository().GetCffClientByClientId((int)QueryString.ClientId));
                } 
                else  if (!(Context.User as CffPrincipal).IsInClientRole && (Context.User as CffPrincipal).IsInCustomerRole) 
                {
                    presenter.LoadClientAndCustomerContact(cxCust, (ICffClient)SessionWrapper.Instance.Get.ClientFromQueryString);
                }
                else //throw some error
                    presenter.LoadClientAndCustomerContact(cxCust, 
                        RepositoryFactory.CreateClientRepository().GetCffClientByClientId(0));
                }
                else
                     presenter.LoadClientAndCustomerContact(cxCust, xClient);
           }
        }

        private void InitializeEditCustomerInformation()
        {
            if (ViewState != null) ViewState[IsCustomerInfoInEdit] = false;
        }

        protected void EditCustomerInformationClick(object sender, EventArgs e)
        {
            bool bIsCustomerSelected = false;
            if (SessionWrapper.Instance.Get != null) {
                bIsCustomerSelected = SessionWrapper.Instance.Get.IsCustomerSelected;
            } else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                bIsCustomerSelected = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsCustomerSelected;

            if (bIsCustomerSelected) {
                if (ViewState != null) {
                    var isCustomerInfoInEdit = (bool) ViewState[IsCustomerInfoInEdit];
                    if (isCustomerInfoInEdit){
                        PrepareCustomerInfoForDisplay();
                        UpdateNextCallDue();
                    } else {
                        PrepareCustomerInfoForEdit();
                    }
                }
            }
        }

        protected void EditClientContactClick(object sender, EventArgs e)
        {
           // ClientContactsEditModalBox.Visible = true;
        }

        protected void EditCustomerContactClick(object sender, EventArgs e)
        {
            //CustomerContactsEditModalBox.Show();
        }

        #region Private Helper methods

        private void PrepareCustomerInfoForDisplay()
        {
            nextCallTextBox.Visible = false;
            nextCallLiteral.Visible = true;
            EditCustomerInformationButton.ImageUrl = "~/images/btn_sm_edit.png";
            if (ViewState != null) ViewState[IsCustomerInfoInEdit] = false;
        }

        private void PrepareCustomerInfoForEdit()
        {
            nextCallTextBox.Visible = true;
            nextCallLiteral.Visible = false;
            EditCustomerInformationButton.ImageUrl = "~/images/btn_sm_update.png";
            if (ViewState != null) ViewState[IsCustomerInfoInEdit] = true;
        }

        private void UpdateNextCallDue()
        {
            //int customerId = SessionWrapper.Instance.Get..Customer.Id;
            //int customerId = SessionWrapper.Instance.Get..Get.CustomerFromQueryString.Id;
            int customerId = SessionWrapper.Instance.Get.CustomerFromQueryString.Id;
            var calendar = new Calendar();
            var nextCallDue = new Date(DateTime.Parse(nextCallTextBox.Text));
            int employeeId = -1;
            var cffPrincpal = Context.User as CffPrincipal;
            if (cffPrincpal != null)
            {
                employeeId = cffPrincpal.CffUser.EmployeeId;
            }
            presenter.UpdateCustomerNextCallDue(nextCallDue, customerId, calendar.Now, employeeId);
            RefreshCustomerInformation(SessionWrapper.Instance.Get.IsClientSelected);
        }

        /// <summary>
        /// Adds a JavaScript reference to the header of the document.
        /// </summary>
        private void AddJavaScriptIncludeInHeader(string path)
        {
            var script = new HtmlGenericControl("script");
            script.Attributes["type"] = "text/javascript";
            script.Attributes["src"] = ResolveUrl(path);
            Page.Header.Controls.Add(script);
        }

        public void ToggleClientSearchControl(bool enable)
        {
            if (enable)
            {
                ClientSearch.Disabled = false;
                ClientSearch.Attributes["class"] = string.Empty;
            }
            else
            {
                ClientSearch.Disabled = true;
                ClientSearch.Attributes["class"] = "disabled";
            }
        }

        public void ToggleCustomerSearchControl(bool enable)
        {
            if (enable)
            {
                CustomerSearch.Disabled = false;
                CustomerSearch.Attributes["class"] = string.Empty;
            }
            else
            {
                CustomerSearch.Disabled = true;
                CustomerSearch.Attributes["class"] = "disabled";
            }
        }

        private void InitializeForCurrentUser()
        {
            var cffPrincipal = Context.User as CffPrincipal;
            if (cffPrincipal != null)
            {
                LogOnNameLabel.Text = cffPrincipal.CffUser.DisplayName;
            }
        }

        private void InitializeCurrentPathForJavaScript()
        {
            string relativePathToRoot = RelativePathComputer.ComputeRelativePathToRoot(Server.MapPath("~"),
                                                                                       Server.MapPath("."));
            string script = string.Format(@"var relativePathToRoot='{0}';", relativePathToRoot);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "initializeCurrentPath", script, true);
        }

        #endregion

        public ICffClient Client
        {
            get {
                if (SessionWrapper.Instance.Get != null) { 
                    return SessionWrapper.Instance.Get.ClientFromQueryString;
                } else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString;
                else
                    return null;
            }
            set {
                if (SessionWrapper.Instance.Get != null) { 
                    SessionWrapper.Instance.Get.ClientFromQueryString = value; 
                } else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString = value;
            }
        }

        //public CffCustomer Customer
        public ICffCustomer Customer
        {
            get {
                if (SessionWrapper.Instance.Get != null)
                {
                    return SessionWrapper.Instance.Get.CustomerFromQueryString;
                }
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString;
                else
                    return null;
            }
            
            set {
                if (SessionWrapper.Instance.Get != null) {
                    SessionWrapper.Instance.Get.CustomerFromQueryString = value; 
                } else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString = value;
            }
        }

        public void RedirectTo(string redirectionPath)
        {
            this.Response.Redirect(redirectionPath);
        }

        protected void ChangePassword_OnClick(object sender, EventArgs e)
        {
            ChangePassword.Show();
        }

#region "RegionEditCustomerInformationPanel"

        protected void btnEditCustomerInfo_Click(object sender, ImageClickEventArgs e)
        {
            CffPrincipal thisUser = Context.User as CffPrincipal;
            if (btnEditCustomerInfo.ImageUrl.ToLower().Contains("edit")) {
                if (thisUser.IsInAdministratorRole || thisUser.IsInManagementRole)
                {
                    if (SessionWrapper.Instance.Get == null && QueryString.ViewIDValue!=null) {
                        if (thisUser.IsInManagementRole && SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsEditContactDetails && (thisUser.CffUser.UserType.Name.ToLower().IndexOf("client") >= 0))
                        {
                            lblCENote.Text = "**Changes are subject for validation**";
                            lblCENote.Visible = true;
                        }
                    }
                    else if (thisUser.IsInManagementRole && SessionWrapper.Instance.Get.IsEditContactDetails && (thisUser.CffUser.UserType.Name.ToLower().IndexOf("client") >= 0))
                    {
                        lblCENote.Text = "**Changes are subject for validation**";
                        lblCENote.Visible = true;
                    }
                    else {
                        lblCENote.Visible = false;
                    }
                }
                else 
                { //require validation if factoring or discounting client
                    Cff.SaferTrader.Core.Repositories.ICustomerRepository repo = Cff.SaferTrader.Core.Repositories.RepositoryFactory.CreateCustomerRepository();
                    ClientInformationAndAgeingBalances cinfo = repo.GetMatchedClientInfo(SessionWrapper.Instance.Get.ClientFromQueryString.Id);
                    if (cinfo.ClientInformation.FacilityType.Trim().ToLower().Contains("factoring") ||
                           cinfo.ClientInformation.FacilityType.Trim().ToLower().Contains("discounting"))
                    {
                        lblCENote.Text = "**Changes are subject for validation**";
                        lblCENote.Visible = true;
                    }
                    else { lblCENote.Visible = false; }
                }


                btnCancelEditCustomerInfo.Visible = true;
                btnEditCustomerInfo.ImageUrl = "~/images/btn_sm_update.png";    //page_save.png";  dbb
                btnEditCustomerInfo.ToolTip = "Save Changes";
                btnEditCustomerInfo.Style.Add(HtmlTextWriterStyle.Width, "42px"); //dbb

                stopCreditDDList.Visible = true;
                for (int ix = 0; ix < stopCreditDDList.Items.Count; ix++) {
                    if (stopCreditDDList.Items[ix].Value.Trim().ToLower() == stopCreditLiteral.Text.Trim().ToLower())
                    { stopCreditDDList.SelectedIndex = ix; break;  }
                }
                stopCreditLiteral.Visible = false;
                

                creditLimitTBox.Visible = true;
                creditLimitTBox.Text = creditLimitLiteral.Text;
                creditLimitLiteral.Visible = false;
                
                nextCallTextBox.Text = nextCallLiteral.Text;
                nextCallTextBox.Visible = true;
                nextCallLiteral.Visible = false;
                
                allowCallsDDlist.Visible = true;
                for (int ix = 0; ix < allowCallsDDlist.Items.Count; ix++)
                {
                    if (allowCallsDDlist.Items[ix].Value.Trim().ToLower() == allowCallsDDlist.Text.Trim().ToLower())
                    { allowCallsDDlist.SelectedIndex = ix; break; }
                }
                allowCallsLiteral.Visible = false;

                ListDateTBox.Text = ListDateLiteral.Text;
                ListDateTBox.Visible = false;  //dbb
                ListDateLiteral.Visible = true;

                
                termsLiteral.Visible = false;
                termsDDList.Visible = true;
                if (termsDDList.SelectedIndex == 2) { termsTBox.Visible = true; }
                
                companyIDTBox.Text = companyIDLiteral.Text;
                companyIDTBox.Visible = true;
                companyIDLiteral.Visible = false;

                GSTTBox.Text = GSTLiteral.Text;
                GSTTBox.Visible = true;
                GSTLiteral.Visible = false;

            } else { //save
                btnEditCustomerInfo.ImageUrl = "~/images/btn_sm_edit.png";   //"page_edit.png";  dbb
                btnEditCustomerInfo.ToolTip = "Edit Customer Information";

                btnCancelEditCustomerInfo.Visible = false;
                stopCreditLiteral.Text = stopCreditDDList.Text;
                stopCreditDDList.Visible = false;
                stopCreditLiteral.Visible = true;

                creditLimitLiteral.Text = creditLimitTBox.Text;
                creditLimitLiteral.Visible = true;
                creditLimitTBox.Visible = false;

                nextCallLiteral.Text = nextCallTextBox.Text;
                nextCallLiteral.Visible = true;
                nextCallTextBox.Visible = false;

                allowCallsLiteral.Text = allowCallsDDlist.Text;
                allowCallsLiteral.Visible = true;
                allowCallsDDlist.Visible = false;

                ListDateLiteral.Text = ListDateTBox.Text;   
                //ListDateLiteral.Visible = true;    //dbb
                //ListDateTBox.Visible = false;

                ListDateTBox.Visible = false;
                ListDateLiteral.Visible = true;

                if (termsDDList.SelectedIndex == 2)
                {
                    termsLiteral.Text = termsTBox.Text.Trim() + " " + termsDDList.Text;
                    termsTBox.Visible = false;
                }
                else {
                    termsLiteral.Text = termsDDList.Text;
                }

                termsLiteral.Visible = true;
                termsDDList.Visible = false;

                companyIDLiteral.Text = companyIDTBox.Text;
                companyIDLiteral.Visible = true;
                companyIDTBox.Visible = false;

                GSTLiteral.Text = GSTTBox.Text;
                GSTLiteral.Visible = true;
                GSTTBox.Visible = false;
                lblGSTError.Visible = false;

                Int16 iStopCredit= 0, iAllowCalls=0, iTerms=0, iTermDays;
                
                if (stopCreditLiteral.Text.ToLower().Trim() == "yes") { iStopCredit = 1; }
                if (allowCallsLiteral.Text.ToLower().Trim() == "yes") { iAllowCalls = 1; }
                iTerms = Convert.ToInt16(termsDDList.SelectedIndex);
                if (termsDDList.SelectedIndex == 2) { 
                    iTermDays = Convert.ToInt16(termsTBox.Text);
                }

                decimal dCreditLimit=0, dGSTValue=0;

                string strDummy = creditLimitLiteral.Text.Replace("$", "");
                strDummy = strDummy.Replace(",","");
                dCreditLimit = Convert.ToDecimal(strDummy);

                strDummy = GSTLiteral.Text.Replace("#", "");
                strDummy = strDummy.Replace("$", "");
                strDummy = strDummy.Replace(",", "");
                if (strDummy.Length > 0) { dGSTValue = Convert.ToDecimal(strDummy); }
                else { dGSTValue = 0; }

                int clientId = Convert.ToInt32(SessionWrapper.Instance.Get.ClientFromQueryString.Id);
                int custId = Convert.ToInt32(SessionWrapper.Instance.Get.CustomerFromQueryString.Number);

                if (thisUser.IsInAdministratorRole || thisUser.IsInManagementRole)
                {
                    if (thisUser.IsInManagementRole && SessionWrapper.Instance.Get.IsEditContactDetails && (thisUser.CffUser.UserType.Name.ToLower().IndexOf("client") >= 0))
                    { //if user is client management and administered by cff require validation
                        presenter.InsUpdateCustomerInformation("InsValidate",
                         clientId,
                         custId,
                         iStopCredit,
                         dCreditLimit,
                         Convert.ToDateTime(nextCallLiteral.Text),
                         iAllowCalls,
                         Convert.ToDateTime(ListDateLiteral.Text),
                         iTerms,
                         companyIDLiteral.Text.Replace("#", ""),
                         dGSTValue,
                         System.DateTime.Now,
                         thisUser.CffUser.EmployeeId);

                        lblCENote.Text = "**Changes submitted for validation.**";
                    }
                    else {
                        //post changes if user is admin or management, if user is client management post changes iff not administered by cff
                        presenter.InsUpdateCustomerInformation("UpdateCust",
                            clientId,
                            custId,
                            iStopCredit,
                            dCreditLimit,
                            Convert.ToDateTime(nextCallLiteral.Text),
                            iAllowCalls,
                            Convert.ToDateTime(ListDateLiteral.Text),
                            iTerms,
                            companyIDLiteral.Text.Replace("#", ""),
                            dGSTValue,
                            System.DateTime.Now,
                            thisUser.CffUser.EmployeeId);

                        lblCENote.Text = "**Changes submitted.**";
                       
                    }
                    lblCENote.Visible = true;

                } else {
                    //otherwise require validation if factoring or discounting client
                    Cff.SaferTrader.Core.Repositories.ICustomerRepository repo = Cff.SaferTrader.Core.Repositories.RepositoryFactory.CreateCustomerRepository();
                    ClientInformationAndAgeingBalances cinfo = repo.GetMatchedClientInfo(SessionWrapper.Instance.Get.ClientFromQueryString.Id);
                    if (cinfo.ClientInformation.FacilityType.Trim().ToLower().Contains("factoring") ||
                            cinfo.ClientInformation.FacilityType.Trim().ToLower().Contains("discounting"))
                    {
                        presenter.InsUpdateCustomerInformation("InsValidate",
                          clientId,
                          custId,
                          iStopCredit,
                          dCreditLimit,
                          Convert.ToDateTime(nextCallLiteral.Text),
                          iAllowCalls,
                          Convert.ToDateTime(ListDateLiteral.Text),
                          iTerms,
                          companyIDLiteral.Text.Replace("#", ""),
                          dGSTValue,
                          System.DateTime.Now,
                          thisUser.CffUser.EmployeeId);

                        lblCENote.Text = "**Changes submitted for validation.**";
                        lblCENote.Visible = true;
                    } else { //todo: double check this specs
                        presenter.InsUpdateCustomerInformation("UpdateCust",
                           clientId,
                           custId,
                           iStopCredit,
                           dCreditLimit,
                           Convert.ToDateTime(nextCallLiteral.Text),
                           iAllowCalls,
                           Convert.ToDateTime(ListDateLiteral.Text),
                           iTerms,
                           companyIDLiteral.Text.Replace("#", ""),
                           dGSTValue,
                           System.DateTime.Now,
                           thisUser.CffUser.EmployeeId);

                        lblCENote.Text = "***Changes submitted.***";
                        lblCENote.Visible = true;
                    }
                }

                btnCancelEditCustomerInfo.Visible = false;
                btnEditCustomerInfo.Style.Add(HtmlTextWriterStyle.Width, "28px");
            }
        }

        protected void btnCancelEditCustomerInfo_Click(object sender, ImageClickEventArgs e)
        { //cancel edit customer information
            btnEditCustomerInfo.ImageUrl = "~/images/btn_sm_edit.png";  //"page_edit.png"; dbb
            btnEditCustomerInfo.ToolTip = "Edit Customer Information";
            btnCancelEditCustomerInfo.Visible = false;

            btnEditCustomerInfo.Style.Add(HtmlTextWriterStyle.Width, "28px");  //dbb

            stopCreditDDList.Visible = false;
            stopCreditLiteral.Visible = true;

            creditLimitTBox.Visible = false;
            creditLimitLiteral.Visible = true;

            nextCallTextBox.Visible = false;
            nextCallLiteral.Visible = true;

            allowCallsDDlist.Visible = false;
            allowCallsLiteral.Visible = true;

            ListDateTBox.Visible = false;
            ListDateLiteral.Visible = true;


            termsLiteral.Visible = true;
            termsDDList.Visible = false;
            termsTBox.Visible = false;

            companyIDLiteral.Visible = true;
            companyIDTBox.Visible = false;

            GSTLiteral.Visible = true;
            GSTTBox.Visible = false;
            lblGSTError.Visible = false;
            lblCENote.Visible = false;
        }

        protected void termsDDList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (termsDDList.SelectedIndex == 2) { termsTBox.Visible = true; }
            else {  termsTBox.Visible = false; }
        }

        protected void GSTTBox_TextChanged(object sender, EventArgs e)
        {
            try { 
                Decimal cValue = Convert.ToDecimal(GSTTBox.Text);
                GSTTBox.Text = String.Format("{0:c}", cValue);
                lblGSTError.Visible = false;
            } catch (Exception) {
                lblGSTError.Text = "invalid non-numeric value. ";
                lblGSTError.Visible = true;
            }
         
        }

        protected void termsTBox_TextChanged(object sender, EventArgs e)
        {
            int idummy;
            if (!Int32.TryParse(termsTBox.Text, out idummy))
            {
                termsTBox.Text = "0";
                lblCENote.Text = "invalid non-numeric value.";
                lblCENote.Visible = true;
            }
            else {
                lblCENote.Text = "";
                lblCENote.Visible = false;
            }
        }

        protected void creditLimitTBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Decimal cValue = Convert.ToDecimal(creditLimitTBox.Text);
                creditLimitTBox.Text = String.Format("{0:c}", cValue);
                lblCENote.Text = "";
                lblCENote.Visible = false;
            }
            catch (Exception)
            {
                lblCENote.Text = "invalid non-numeric value.";
                lblCENote.Visible = true;
            }
        }

#endregion

#region "RegionPopulateAsAtBalance"
        protected void ddlAsAtDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            retrieveAmounts();
        }

        protected void populateAsAtList()
        {
            List<object> objParams = new List<object>();
            objParams.Add(SessionWrapper.Instance.Get.CustomerFromQueryString.Id);
            objParams.Add(SessionWrapper.Instance.Get.ClientFromQueryString.Id);

            DateTime dateCreated;
            DateTime dateAsAt = System.DateTime.Now;
            DateTime dateNow = System.DateTime.Now;
            Date das;
            string dtShowAsAt;
            int dcMonth = 0;
            int dcYear;

            stpCaller stpc = new stpCaller();
            System.Data.DataSet DS = stpc.executeSPDataSet(objParams, stpCaller.stpType.GetCustomerInfo);
            if (DS == null) { }
            else
            {
                if (DS.Tables.Count > 1)
                {
                    System.Data.DataTable DT = DS.Tables[2];
                    if (DT.Rows.Count > 0)
                    {
                        dateCreated = Convert.ToDateTime(DT.Rows[0][3]);
                        if (dateCreated < Convert.ToDateTime("01/01/1997"))
                        {
                            dtShowAsAt = "01/01/1997";
                            dateAsAt = Convert.ToDateTime(dtShowAsAt);
                        }
                        else
                        {
                            dateAsAt = dateCreated;
                            if (dateAsAt.Month > 1)
                            {
                                dcMonth = dateAsAt.Month - 1;
                                dcYear = dateAsAt.Year;
                            }
                            else
                            { dcYear = dateAsAt.Year - 1; }


                            if (dcMonth <= 0) { dcMonth = 1; }
                            dtShowAsAt = dateAsAt.Day.ToString().PadLeft(2, '0') + "/" + dcMonth.ToString().PadLeft(2, '0') + "/" + dcYear.ToString();

                            if ((dcMonth == 2) && (dateAsAt.Day > 28))
                            {//leap year checking
                                if (dcYear % 400 == 0 || (dcYear % 100 != 0 && dcYear % 4 == 0))
                                {
                                    // dtShowAsAt = dateAsAt.Day.ToString().PadLeft(2, '0') + "/" +
                                    //             dcMonth.ToString().PadLeft(2, '0') + "/" + dcYear.ToString();
                                    dtShowAsAt = "29/" + dcMonth.ToString().PadLeft(2, '0') + "/" + dcYear.ToString();
                                }
                                else
                                {
                                    dtShowAsAt = "28/" + dcMonth.ToString().PadLeft(2, '0') + "/" + dcYear.ToString();
                                }
                            }
                            else if (dateAsAt.Day > 30)
                            {
                                if (dcMonth == 4 || dcMonth == 6 || dcMonth == 9 || dcMonth == 11)
                                {
                                    dtShowAsAt = "30/" + dcMonth.ToString().PadLeft(2, '0') + "/" + dcYear.ToString();
                                }
                            }


                            dateAsAt = Convert.ToDateTime(dtShowAsAt);

                            das = new Date(dateAsAt);
                            dtShowAsAt = das.LastDayOfTheMonth.ToDateTimeString();
                            dateAsAt = Convert.ToDateTime(dtShowAsAt);
                        }
                    }
                }

                ddlAsAtDate.Items.Add(dateNow.ToShortDateString());
                while (dateNow > dateAsAt)
                {
                    if (dateNow.Month > 1)
                    {
                        dcMonth = dateNow.Month - 1;
                        dcYear = dateNow.Year;
                    }
                    else
                    {
                        if (dateNow.Month == 1){ 
                            dcMonth = 12;
                        } 
                        dcYear = dateNow.Year - 1; 
                    }


                    if ((dcMonth == 2) && (dateNow.Day > 28))
                    { 
                        if (dcYear % 400 == 0 || (dcYear % 100 != 0 && dcYear % 4 == 0))
                        { //leap year
                            dtShowAsAt = "29/" + dcMonth.ToString().PadLeft(2, '0') + "/" + dcYear.ToString();
                            //dtShowAsAt = dateNow.Day.ToString().PadLeft(2, '0') + "/" + 
                            //             dcMonth.ToString().PadLeft(2, '0') + "/" + dcYear.ToString();
                        }
                        else
                        {
                            dtShowAsAt = "28/" + dcMonth.ToString().PadLeft(2, '0') + "/" + dcYear.ToString();
                        }

                    } 
                    else if ((dateNow.Day > 30) && (dcMonth == 4 || dcMonth == 6 || dcMonth == 9 || dcMonth == 11))
                    {
                        dtShowAsAt = "30/" + dcMonth.ToString().PadLeft(2, '0') + "/" + dcYear.ToString();
                    }
                    else {
                        dtShowAsAt = dateNow.Day.ToString().PadLeft(2, '0') + "/" +
                                        dcMonth.ToString().PadLeft(2, '0') + "/" +
                                        dcYear.ToString();
                    }

                    dateNow = Convert.ToDateTime(dtShowAsAt);
                    das = new Date(dateNow);
                    dtShowAsAt = das.LastDayOfTheMonth.ToShortDateString();
                    ddlAsAtDate.Items.Add(dtShowAsAt);

                    dtShowAsAt = das.ToDateTimeString();
                    dateNow = Convert.ToDateTime(dtShowAsAt);
                  
                }
            }

            if (ddlAsAtDate.Items.Count > 1)
            {
                ddlAsAtDate.SelectedIndex = 0;
                retrieveAmounts();
            }
        }

        protected void retrieveAmounts()
        {
            try
            {
                DateTime dtAsAt = Convert.ToDateTime(ddlAsAtDate.Text);
                string strYrMth = dtAsAt.Year.ToString() + dtAsAt.Month.ToString().PadLeft(2, '0');
                int yrMth = Convert.ToInt32(strYrMth);

                List<object> objParams = new List<object>();
                objParams.Add(SessionWrapper.Instance.Get.CustomerFromQueryString.Id);
                objParams.Add(0);
                objParams.Add(0);
                objParams.Add(yrMth);
                objParams.Add(0);
                objParams.Add(dtAsAt);

                stpCaller stpc = new stpCaller();
                System.Data.DataSet DS = stpc.executeSPDataSet(objParams, stpCaller.stpType.GetTransactions);
                if (DS == null) { }
                else
                {
                    if (DS.Tables.Count > 0)
                    {
                        System.Data.DataTable DT = DS.Tables[0];
                        if (DT.Rows.Count == 0)
                        {
                            hfCurrent.Value = "0";
                            hfMonth1.Value = "0";
                            hfMonth2.Value = "0";
                            hfMonth3.Value = "0.00";
                            currentAmtLiteral.Text = String.Format("{0:$#,##0.00;($#,##0.00)}", 0);
                            month1Literal.Text = String.Format("{0:$#,##0.00;($#,##0.00)}", 0);
                            month2Literal.Text = String.Format("{0:$#,##0.00;($#,##0.00)}", 0);
                            month3Literal.Text = String.Format("{0:$#,##0.00;($#,##0.00)}", 0);
                            totalAmountLiteral.Text = String.Format("{0:c}", 0);
                        }
                        else
                        {
                            decimal TrnBal = 0;
                            decimal RunningBal = 0;

                            decimal Current = 0;
                            decimal mth1 = 0;
                            decimal mth2 = 0;
                            decimal mth3 = 0;

                            Int64 priorTruTrnId = Convert.ToInt64(DT.Rows[0]["TrueTrnID"]);
                            short TrnType;
                            int Age = 0;

                            for (int rCtr = 0; rCtr < DT.Rows.Count; rCtr++)
                            {
                                System.Data.DataRow DR = DT.Rows[rCtr];
                                TrnType = Convert.ToInt16(DR["TransTypeID"]);

                                if (TrnType == 5)
                                {
                                    TrnBal = RunningBal;
                                }
                                else
                                {
                                    if (Convert.ToInt64(DR["TrueTrnID"]) != priorTruTrnId)
                                    {
                                        RunningBal = 0;
                                        priorTruTrnId = Convert.ToInt64(DR["TrueTrnID"]);
                                    }

                                    TrnBal = Convert.ToDecimal(DR["Amount"]);
                                    RunningBal += TrnBal;

                                    if ((TrnType == 1) || (Convert.ToInt64(DR["TrueTrnID"]) < 0))
                                    {
                                        Age = Convert.ToInt32(DR["Age"]);
                                    }

                                    switch (Age)
                                    {
                                        case 1:
                                        case 89:
                                            mth1 += TrnBal;
                                            break;
                                        case 2:
                                        case 90:
                                            mth2 += TrnBal;
                                            break;
                                        default:
                                            if (Age < 1)
                                            { Current += TrnBal; }
                                            else
                                            { mth3 += TrnBal; }
                                            break;
                                    } //end switch
                                }
                            } //end for

                            hfCurrent.Value = Current.ToString();
                            hfMonth1.Value = mth1.ToString();
                            hfMonth2.Value = mth2.ToString();
                            hfMonth3.Value = mth3.ToString();
                            currentAmtLiteral.Text = String.Format("{0:$#,##0.00;($#,##0.00)}", Current);
                            month1Literal.Text = String.Format("{0:$#,##0.00;($#,##0.00)}", mth1);
                            month2Literal.Text = String.Format("{0:$#,##0.00;($#,##0.00)}", mth2);
                            month3Literal.Text = String.Format("{0:$#,##0.00;($#,##0.00)}", mth3);
                            totalAmountLiteral.Text = String.Format("{0:$#,##0.00;($#,##0.00)}", (mth1 + mth2 + mth3 + Current));
                            //String FacilityTypeId = presenter.GetFacilityTypeByClientId(SessionWrapper.Instance.Get.ClientFromQueryString.Id);
                            //if (FacilityTypeId == "Current A/c")
                            //{
                            //    CurrentACLimitBalanceRow.Visible = true;
                            //    CALimitBalanceField.Text = String.Format("{0:C}", presenter.GetCurrentACLimitFromDrMgt(SessionWrapper.Instance.Get.ClientFromQueryString.Id, dtAsAt));
                            //}
                        } //end DT rowcount?
                    } //end DS
                }//end DSnull
            }
            catch (Exception)
            {
                //sendStatusLiteral.Text = exc.Message;
                //sendStatusLiteral.Visible = true;
            }
        }
#endregion

#region "EditClientContacts"
        protected void LinkBtnEditClientContactClick(object sender, EventArgs e)
        {
            ClientContact thisClientContact;

            if (SessionWrapper.Instance.Get.IsEditContactDetails)
            { //save changes to database 
                bool AllowEdit = false;

                ICffClient thisClient = SessionWrapper.Instance.Get.ClientFromQueryString;
                thisClientContact = getClientContactDetails(true);
                
                if (this.CurrentPrincipal.IsInManagementRole || this.CurrentPrincipal.IsInAdministratorRole)
                {
                    if (SessionWrapper.Instance.Get.IsEditContactDetails == true && (this.CurrentPrincipal.CffUser.UserType.Name.ToLower().IndexOf("client") >= 0)
                                    && (this.CurrentPrincipal.IsInManagementRole))
                    {
                        //require validation - if in client management, allow edit iff client is not administered by cff

                    }
                    else
                    {    //allow only for administrator || management
                        AllowEdit = true;
                    }

                }

                if (AllowEdit)
                {
                    if (this.presenter.UpdateClientContactDetails(thisClient.Id, thisClientContact) == false)
                    {
                        literalStatusEditClientContact.Text = "***Error: Unable to update client contact***";
                        literalStatusEditClientContact.Visible = true;
                    }
                }
                else 
                {
                    //insert into client validation table for review
                    if (this.presenter.InsertCliContactInfoDetailsForValidation(thisClient.Id, thisClientContact) == false)
                    {
                        literalStatusEditClientContact.Text = "***Error: Unable to submit changes for validation ***";
                    }
                    else
                    {
                        literalStatusEditClientContact.Text = "***Changes submitted for validation ***";
                    }
                    literalStatusEditClientContact.Visible = true;

                }

                origClientContactDetails = thisClientContact;
                SessionWrapper.Instance.Get.IsEditContactDetails = false;
                LinkBtnCancelEditContact.Visible = false;
            }
            else
            {
                thisClientContact = getClientContactDetails(false);
                SessionWrapper.Instance.Get.IsEditContactDetails = true;
                enableEditClientContactDetails();
                LinkBtnCancelEditContact.Visible = true;
            }

            displayClientContact(thisClientContact);          
        }


        private ClientContact getClientContactDetails(bool isSaveMode)
        {
            ClientContact cDetails = new ClientContact();

            cDetails.FirstName = txtClientContactFName.Text.Trim();
            cDetails.LastName = txtClientContactLName.Text.Trim();
            cDetails.ModifiedBy = CurrentPrincipal.CffUser.EmployeeId;
            cDetails.Modified = DateTime.Now;

            if (isSaveMode)
            {
                cDetails.Phone = txtClientContactPhone.Text.Trim();
                cDetails.Fax = txtClientContactFax.Text.Trim();
                cDetails.MobilePhone = txtClientContactMobile.Text.Trim();
                cDetails.FullName = cDetails.FirstName.Trim() + " " + cDetails.LastName.Trim();
                cDetails.Role = txtClientContactRole.Text.Trim();
                cDetails.Email = txtClientContactEmail.Text.Trim();
                cDetails.Address1 = txtClientContactAddress1.Text.Trim();
                cDetails.Address2 = txtClientContactAddress2.Text.Trim();
                cDetails.Address3 = txtClientContactAddress3.Text.Trim();
                cDetails.Address4 = txtClientContactAddress4.Text.Trim();
            }
            else
            {
                string[] subX = clientContactPhone.Text.Split(':');
                if (subX.Length > 1){ cDetails.Phone = subX[1].Trim(); }
                subX = clientContactFax.Text.Split(':');
                if (subX.Length > 1) { cDetails.Fax = subX[1].Trim(); }
                subX = clientContactMobile.Text.Split(':');
                if (subX.Length > 1) { cDetails.MobilePhone = subX[1].Trim(); }

                subX = clientContactName.Text.Split(':');
                if (subX.Length > 1) { cDetails.FullName = subX[1].Trim(); }
                subX = clientContactRole.Text.Split(':');
                if (subX.Length > 1) { cDetails.Role = subX[1].Trim(); }
                subX = clientContactEmail.Text.Split(':');
                if (subX.Length > 1) { cDetails.Email = subX[1].Trim(); }

                cDetails.Address1 = clientContactAddress1.Text.Trim();
                cDetails.Address2 = clientContactAddress2.Text.Trim();
                cDetails.Address3 = clientContactAddress3.Text.Trim();
                cDetails.Address4 = clientContactAddress4.Text.Trim();
            }

            return cDetails;
        }

        private void enableEditClientContactDetails()
        {
            clientContactPhone.Text = "Phone     : ";
            txtClientContactPhone.Visible = true;
            clientContactFax.Text = "Fax:";
            txtClientContactFax.Visible = true;
            clientContactMobile.Text = "Mobile:";
            txtClientContactMobile.Visible = true;

            clientContactName.Text = "First Name: ";
            txtClientContactFName.Visible = true;
            literalLName.Visible = true;
            literalLName.Text = "Last Name: ";
            txtClientContactLName.Visible = true;

            clientContactRole.Text = "Role     : ";
            txtClientContactRole.Visible = true;
            clientContactEmail.Text = "Email: ";
            txtClientContactEmail.Visible = true;

            clientContactAddress1.Text = "Address1: ";
            txtClientContactAddress1.Visible = true;
            clientContactAddress2.Text = "Address2: ";
            txtClientContactAddress2.Visible = true;

            clientContactAddress3.Text = "Address3: ";
            txtClientContactAddress3.Visible = true;
            clientContactAddress4.Text = "Address4: ";
            txtClientContactAddress4.Visible = true;


            if (this.CurrentPrincipal.IsInManagementRole || this.CurrentPrincipal.IsInAdministratorRole)
            {
                if (SessionWrapper.Instance.Get.IsEditContactDetails == true && (this.CurrentPrincipal.CffUser.UserType.Name.ToLower().IndexOf("client") >= 0)
                                && (this.CurrentPrincipal.IsInManagementRole))
                {
                    literalStatusEditClientContact.Text = "&nbsp;&nbsp;&nbsp;**Note: Changes will be subject for validation**";
                }
                else
                {
                    literalStatusEditClientContact.Text = "";
                }

            }
            else
            {
                literalStatusEditClientContact.Text = "&nbsp;&nbsp;&nbsp;**Note: Changes will be subject for validation**";
            }
            literalStatusEditClientContact.Visible = true;

        }

        private void hideEditClientContactTextBoxes()
        {
            txtClientContactPhone.Visible = false;
            txtClientContactFax.Visible = false;
            txtClientContactMobile.Visible = false;

            txtClientContactFName.Visible = false;
            txtClientContactLName.Visible = false;
            literalLName.Visible = false;
            txtClientContactRole.Visible = false;
            txtClientContactEmail.Visible = false;

            txtClientContactAddress1.Visible = false;
            txtClientContactAddress2.Visible = false;

            txtClientContactAddress3.Visible = false;
            txtClientContactAddress4.Visible = false;
        }


        private void displayClientContact(ClientContact clientContact)
        {
            if (SessionWrapper.Instance.Get.IsEditContactDetails)
            {
                txtClientContactFName.Text = clientContact.FirstName;
                txtClientContactLName.Text = clientContact.LastName;
                txtClientContactPhone.Text = clientContact.Phone;
                txtClientContactFax.Text = clientContact.Fax;
                txtClientContactMobile.Text = clientContact.MobilePhone;
                txtClientContactRole.Text = clientContact.Role;
                txtClientContactEmail.Text = clientContact.Email;

                txtClientContactAddress1.Text = clientContact.Address1;
                txtClientContactAddress2.Text = clientContact.Address2;
                txtClientContactAddress3.Text = clientContact.Address3;
                txtClientContactAddress4.Text = clientContact.Address4;
            }
            else 
            {
                if (clientContact != null)
                {
                    try
                    {
                        clientContactName.Text = "Contact: " + clientContact.FullName.PadRight(80, ' ');
                    }
                    catch (Exception)
                    {
                        clientContactName.Text = "Contact: ";
                    }

                    if (clientContact.Phone != null)
                    { //add checking for null here, sometimes when user posts and navigates concurrently data in these fields are lost - for investigation
                        clientContactPhone.Text = string.Format("Phone: {0}", clientContact.Phone);
                    }

                    if (clientContact.Fax != null)
                    {
                        clientContactFax.Text = string.Format("Fax: {0}", clientContact.Fax);
                    }

                    if (clientContact.MobilePhone != null)
                    { 
                        clientContactMobile.Text = string.Format("Mobile: {0}", clientContact.MobilePhone);
                    }

                    clientContactRole.Text = "Role: " + clientContact.Role;
                    clientContactEmail.Text = "Email: " + clientContact.Email;

                    clientContactAddress1.Text = clientContact.Address1;
                    clientContactAddress2.Text = clientContact.Address2;
                    clientContactAddress3.Text = clientContact.Address3;
                    clientContactAddress4.Text = clientContact.Address4;
                }
                
                
                CffPrincipal cpUser = Context.User as CffPrincipal;
                if (cpUser.IsInAdministratorRole || cpUser.IsInManagementRole)
                { LinkBtnEditClientContact.Visible = true; }
                else 
                { LinkBtnEditClientContact.Visible = false;}

                txtClientContactFName.Text = clientContact.FirstName;
                txtClientContactLName.Text = clientContact.LastName;
                literalLName.Text = clientContact.LastName;
                hideEditClientContactTextBoxes();

                if (SessionWrapper.Instance.Get.IsEditContactDetails== false)
                {
                    hideEditCustomerContactTextBoxes();
                }

                if ((SessionWrapper.Instance.Get.IsContactShown==true) && (SessionWrapper.Instance.Get.IsCustomerSelected==true))
                {
                    LinkBtnEditCustomerContact.Visible  = true;
                    if (SessionWrapper.Instance.Get.IsEditCContactDetails==true) {
                        LinkBtnCancelEditCustomerContact.Visible = true;
                    }
                    else { LinkBtnCancelEditCustomerContact.Visible = false; }
                }
                else
                {
                    LinkBtnCancelEditCustomerContact.Visible = false;
                    // LinkBtnEditCustomerContact.Visible = false;   // dbb [20160804]
                    LinkBtnEditCustomerContact.Visible = true;
                }
            }
        }

        protected void LinkBtnCancelEditContactClick(object sender, EventArgs e)
        {
            if (SessionWrapper.Instance.Get!=null)
                SessionWrapper.Instance.Get.IsEditContactDetails = false;
            else if (QueryString.ViewIDValue!=null)
                SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsEditContactDetails = false;
            
            bool rCustWrapper = SessionWrapper.Instance.Get.IsEditCContactDetails;

           if (origClientContactDetails == null)
               origClientContactDetails = ViewState["ClientContactDetails"] as ClientContact;

           displayClientContact(origClientContactDetails);
           //displayClientContact(getClientContactDetails(true));
        
           LinkBtnCancelEditContact.Visible = false;

           if (rCustWrapper == false)
           {
               LinkBtnCancelEditCustomerContact.Visible = false;
               if ((SessionWrapper.Instance.Get.IsContactShown == true) && (SessionWrapper.Instance.Get.IsCustomerSelected == true))
               { LinkBtnEditCustomerContact.Visible = true; }
               else {
                    //LinkBtnEditCustomerContact.Visible = false;   // dbb [20160804]
                    LinkBtnEditCustomerContact.Visible = true;
                }
           }
           else {
               //MSarza [20150901] : Data type changed from bool to small int for dbo.ClientFinancials.CffDebtorAdmin.
               //enableEditCustomerContactDetails(SessionWrapper.Instance.Get.IsClientAdminByCFF, true);
               enableEditCustomerContactDetails(SessionWrapper.Instance.Get.IsClientDebtorAdmin, true);
               LinkBtnCancelEditCustomerContact.Visible = true;
               LinkBtnEditCustomerContact.Visible = true;
           }
           literalStatusEditClientContact.Text = "";
           literalStatusEditClientContact.Visible = false;
        }

#endregion


#region "EditCustomerContact"
        protected void LinkBtnEditCustomerContactClick(object sender, EventArgs e)
        { 
            
            CustomerContact thisCustomerContact = new CustomerContact();
            bool isEditCustomerContactDetails = (SessionWrapper.Instance.Get!=null)?SessionWrapper.Instance.Get.IsEditCContactDetails:
                              (QueryString.ViewIDValue != null)?SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsEditCContactDetails:false;

            if (isEditCustomerContactDetails)
            {
                ICffClient thisClient = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString :
                              (QueryString.ViewIDValue != null) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString : null; 
                thisCustomerContact = getCustomerContactDetails(true);

                bool bAllowEdit = false;

                if (this.CurrentPrincipal.IsInAdministratorRole || this.CurrentPrincipal.IsInManagementRole)
                {   //note: allow edit only when administrator or management
                    if (this.CurrentPrincipal.CffUser.UserType.Name.ToLower().IndexOf("client") >= 0
                                && this.CurrentPrincipal.IsInManagementRole
                                    && SessionWrapper.Instance.Get.IsEditCContactDetails == true)
                    {
                        //require validation - for client management allow edit iff client is not admin by CFF.
                    }
                    else
                    {
                        bAllowEdit = true;
                    }
                }

                if (thisClient != null) {
                    if (bAllowEdit)
                    { //save customer contact details changes to database 
                        if (this.presenter.UpdateCustomerContactDetails(thisClient.Id, thisCustomerContact) == false)
                        {
                            literalStatusEditCustContact.Text = "&nbsp;&nbsp;&nbsp;***Unable to update client contact***";
                            literalStatusEditCustContact.Visible = true;
                        }
                    }
                    else
                    {  //otherwise submit for review - insert into customer validation table
                        if (this.presenter.InsertCustContactInfoDetailsForValidation(thisClient.Id, thisCustomerContact) == false)
                        {
                            literalStatusEditCustContact.Text = "&nbsp;&nbsp;&nbsp;***Unable to post changes for validation***";
                        }
                        else
                        {
                            literalStatusEditCustContact.Text = "&nbsp;&nbsp;&nbsp;***Changes submitted for validation***";
                        }
                        literalStatusEditCustContact.Visible = true;

                    }
                }
               
                origCustomerContactDetails = thisCustomerContact;
                if (SessionWrapper.Instance.Get != null)
                    SessionWrapper.Instance.Get.IsEditCContactDetails= false;
                  else if (QueryString.ViewIDValue != null) 
                       SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsEditCContactDetails= false;
                LinkBtnCancelEditCustomerContact.Visible = false;
            }
            else
            {
                thisCustomerContact = getCustomerContactDetails(false);
                if (SessionWrapper.Instance.Get != null) {
                    SessionWrapper.Instance.Get.IsEditCContactDetails = true;
                    enableEditCustomerContactDetails(SessionWrapper.Instance.Get.IsClientDebtorAdmin, false);
                }
                else if (QueryString.ViewIDValue != null) {
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsEditCContactDetails = true;
                    enableEditCustomerContactDetails(SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsClientDebtorAdmin, false);
                }
               
                LinkBtnCancelEditCustomerContact.Visible = true;
            }

            displayCustomerContact(thisCustomerContact, SessionWrapper.Instance.Get.IsEditCContactDetails);   
        }

        protected void LinkBtnCancelEditCustomerContactClick(object sender, EventArgs e)
        {
            if (SessionWrapper.Instance.Get != null)
            {
                SessionWrapper.Instance.Get.IsEditContactDetails = false;
                displayCustomerContact(origCustomerContactDetails, SessionWrapper.Instance.Get.IsEditContactDetails);
            }
            else if (QueryString.ViewIDValue != null) {
                SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsEditContactDetails = false;
                displayCustomerContact(origCustomerContactDetails, SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsEditContactDetails);
            }
            LinkBtnCancelEditCustomerContact.Visible = false;
            literalStatusEditCustContact.Text = "";
            literalStatusEditCustContact.Visible = false;
        }

        private CustomerContact getCustomerContactDetails(bool isSaveMode)
        {
           
            CustomerContact custContactDetails = new CustomerContact();
            if (origCustomerContactDetails==null)
                origCustomerContactDetails = ViewState["CustContactDetails"] as CustomerContact;

            if (origCustomerContactDetails != null) {
                custContactDetails.ContactId = origCustomerContactDetails.ContactId;
                custContactDetails.Modified = origCustomerContactDetails.Modified;
                custContactDetails.ModifiedBy = origCustomerContactDetails.ModifiedBy;
                custContactDetails.CustomerId = Convert.ToInt32(literalContactCustomerID.Text.Trim());
                custContactDetails.FirstName = txtCustomerContactFName.Text.Trim();
                custContactDetails.LastName = txtCustomerContactLName.Text.Trim();
            }
            
            if (isSaveMode)
            {
                custContactDetails.Phone = txtCustomerContactPhone.Text.Trim();
                custContactDetails.Fax = txtCustomerContactFax.Text.Trim();
                custContactDetails.MobilePhone = txtCustomerContactMobile.Text.Trim();
                custContactDetails.FullName = custContactDetails.FirstName.Trim() + " " + custContactDetails.LastName.Trim();
                custContactDetails.Role = txtCustomerContactRole.Text.Trim();
                custContactDetails.Email = txtCustomerContactEmail.Text.Trim();
                custContactDetails.Address1 = txtCustomerContactAddress1.Text.Trim();
                custContactDetails.Address2 = txtCustomerContactAddress2.Text.Trim();
                custContactDetails.Address3 = txtCustomerContactAddress3.Text.Trim();
                custContactDetails.Address4 = txtCustomerContactAddress4.Text.Trim();
            }
            else
            {
                string[] subX = customerContactPhone.Text.Split(':');
                if (subX.Length > 1) { custContactDetails.Phone = subX[1].Trim(); }

                subX = customerContactFax.Text.Split(':');
                if (subX.Length > 1) { custContactDetails.Fax = subX[1].Trim(); }
                
                subX = customerContactMobile.Text.Split(':');
                if (subX.Length > 1) { custContactDetails.MobilePhone = subX[1].Trim(); }

                subX = customerContactName.Text.Split(':');
                if (subX.Length > 1) { custContactDetails.FullName = subX[1].Trim(); }
                
                subX = customerContactRole.Text.Split(':');
                if (subX.Length > 1) { custContactDetails.Role = subX[1].Trim(); }
                subX = customerContactEmail.Text.Split(':');
                if (subX.Length > 1) { custContactDetails.Email = subX[1].Trim(); }

                custContactDetails.Address1 = customerContactAddress1.Text.Trim();
                custContactDetails.Address2 = customerContactAddress2.Text.Trim();
                custContactDetails.Address3 = customerContactAddress3.Text.Trim();
                custContactDetails.Address4 = customerContactAddress4.Text.Trim();
            }

            return custContactDetails;
        }

        private void enableEditCustomerContactDetails(bool isClientDebtorAdmin, bool isCancelEdit)
        {
            customerContactName.Text = "First Name: ";
            txtCustomerContactFName.Visible = true;
            literalCLName.Visible = true;
            literalCLName.Text = "Last Name: ";
            txtCustomerContactLName.Visible = true;

            customerContactPhone.Text = "Phone     : ";
            txtCustomerContactPhone.Visible = true;
            customerContactFax.Text = "Fax:";
            txtCustomerContactFax.Visible = true;
            customerContactMobile.Text = "Mobile:";
            txtCustomerContactMobile.Visible = true;

            customerContactRole.Text = "Role     : ";
            txtCustomerContactRole.Visible = true;
            customerContactEmail.Text = "Email: ";
            txtCustomerContactEmail.Visible = true;

            customerContactAddress1.Text = "Address1: ";
            txtCustomerContactAddress1.Visible = true;
            customerContactAddress2.Text = "Address2: ";
            txtCustomerContactAddress2.Visible = true;

            customerContactAddress3.Text = "Address3: ";
            txtCustomerContactAddress3.Visible = true;
            customerContactAddress4.Text = "Address3: ";
            txtCustomerContactAddress4.Visible = true;

            if (!isCancelEdit)
            {
                if (this.CurrentPrincipal.IsInManagementRole || this.CurrentPrincipal.IsInAdministratorRole)
                {
                    //MSarza [20150901] : Data type changed from bool to small int for dbo.ClientFinancials.CffDebtorAdmin.
                    //if (isClientAdminByCff == true && (this.CurrentPrincipal.CffUser.UserType.Name.ToLower().IndexOf("client") >= 0)
                    //                && (this.CurrentPrincipal.IsInManagementRole))
                    if (isClientDebtorAdmin == false && (this.CurrentPrincipal.CffUser.UserType.Name.ToLower().IndexOf("client") >= 0)
                                    && (this.CurrentPrincipal.IsInManagementRole))
                    {
                        literalStatusEditCustContact.Text = "&nbsp;&nbsp;&nbsp;**Note: Changes will be subject for validation**";
                    }
                    else
                    {
                        literalStatusEditCustContact.Text = "";
                    }

                }
                else
                {
                    literalStatusEditCustContact.Text = "&nbsp;&nbsp;&nbsp;**Note: Changes will be subject for validation**";
                }
                literalStatusEditCustContact.Visible = true;
            }
        }

        private void displayCustomerContact(CustomerContact customerContact, bool bIsEditContactDetails)
        {
            if (bIsEditContactDetails)
            {
                txtCustomerContactFName.Text = customerContact.FirstName;
                txtCustomerContactLName.Text = customerContact.LastName;
                txtCustomerContactFax.Text = customerContact.Fax;
                txtCustomerContactPhone.Text = customerContact.Phone;
                txtCustomerContactMobile.Text = customerContact.MobilePhone;
                txtCustomerContactRole.Text = customerContact.Role;
                txtCustomerContactEmail.Text = customerContact.Email;

                txtCustomerContactAddress1.Text = customerContact.Address1;
                txtCustomerContactAddress2.Text = customerContact.Address2;
                txtCustomerContactAddress3.Text = customerContact.Address3;
                txtCustomerContactAddress4.Text = customerContact.Address4;
            }
            else
            {
                try
                {
                    customerContactName.Text = "Contact: " + customerContact.FullName.PadRight(80, ' ');
                }
                catch (Exception)
                {
                    customerContactName.Text = "Contact: "; 
                }
                customerContactPhone.Text = string.Format("Phone: {0}", customerContact.Phone);
                customerContactFax.Text = string.Format("Fax: {0}", customerContact.Fax);
                customerContactMobile.Text = string.Format("Mobile: {0}", customerContact.MobilePhone);
                customerContactRole.Text = "Role: " + customerContact.Role;
                customerContactEmail.Text = "Email: " + customerContact.Email;

                customerContactAddress1.Text = customerContact.Address1;
                customerContactAddress2.Text = customerContact.Address2;
                customerContactAddress3.Text = customerContact.Address3;
                customerContactAddress4.Text = customerContact.Address4;

                CffPrincipal cpUser = Context.User as CffPrincipal;
                if (cpUser.IsInAdministratorRole || cpUser.IsInManagementRole)
                { LinkBtnEditCustomerContact.Visible = true; }
                else { LinkBtnEditCustomerContact.Visible = false; }

                txtCustomerContactFName.Text = customerContact.FirstName;
                txtCustomerContactLName.Text = customerContact.LastName;
                literalCLName.Text = customerContact.LastName;
                hideEditCustomerContactTextBoxes();
            }

            literalContactCustomerID.Visible = false;
            if (SessionWrapper.Instance.Get!=null)
                SessionWrapper.Instance.Get.IsEditCContactDetails = bIsEditContactDetails;
        }

        private void hideEditCustomerContactTextBoxes()
        {
            txtCustomerContactPhone.Visible = false;
            txtCustomerContactFax.Visible = false;
            txtCustomerContactMobile.Visible = false;

            txtCustomerContactFName.Visible = false;
            txtCustomerContactLName.Visible = false;
            literalCLName.Visible = false;
            txtCustomerContactRole.Visible = false;
            txtCustomerContactEmail.Visible = false;

            txtCustomerContactAddress1.Visible = false;
            txtCustomerContactAddress2.Visible = false;

            txtCustomerContactAddress3.Visible = false;
            txtCustomerContactAddress4.Visible = false;
        }

  

#endregion

    }
}