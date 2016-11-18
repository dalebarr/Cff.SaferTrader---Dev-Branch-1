using System;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Views;
using NPOI.HSSF.Record.Formula.Functions;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class MainNavigation : System.Web.UI.UserControl, IMainNavigationView
    {
        private Scope CurrentScope()
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

        protected void Page_Load(object sender, EventArgs e)
        {
            //todo: We should be able to filter out if query string clientid=selected client id from drop down
            //if not equal redirect to dashboard

            ISecurityManager securityManager = SecurityManagerFactory.Create(CurrentPrincipal, this.CurrentScope());
            MainNavigationPresenter presenter = new MainNavigationPresenter(this, securityManager);

            int ClientFacilityType = (SessionWrapper.Instance.Get!=null)?SessionWrapper.Instance.Get.ClientFromQueryString.ClientFacilityType :
                (!string.IsNullOrEmpty(QueryString.ViewIDValue))?SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.ClientFacilityType : 0;
            if (ClientFacilityType == 5)//Current Account 
            {
                InvoiceBatchesLink.Text = "Drawing Schedules";
                RetentionSchedulesLink.Text = "Monthly Charges";
                releaseTabLabel.Text = "Drawings/Charges";
            }
            else if (ClientFacilityType == 2)//Dr Management
            {
                InvoiceBatchesLink.Text = "Invoice Batches";
                RetentionSchedulesLink.Text = "Monthly Charges";
                releaseTabLabel.Text = "Invoices/Charges";
            }
            else if (ClientFacilityType == 3)//CFSL
            {
                InvoiceBatchesLink.Text = "Funding Schedules";
                RetentionSchedulesLink.Text = "Monthly Charges";
                releaseTabLabel.Text = "Advances/Charges";
            }
            else if (ClientFacilityType == 4)//Loan
            {
                InvoiceBatchesLink.Text = "Advance Schedules";
                //RetentionSchedulesLink.Text = "Monthly Charges";  //dbb
                RetentionSchedulesLink.Visible = false;
                releaseTabLabel.Text = "Advances/Charges"; 
            }
            else // factoring
            {
                InvoiceBatchesLink.Text="Invoice Batches";
                RetentionSchedulesLink.Text= "Monthly Schedules";
                releaseTabLabel.Text = "Releases";
            }

            AppendQueryStringParameters();
            presenter.LockDown();
        }

        private void AppendQueryStringParameters()
        {
            if (UserQueryString == null)
            { //update: for autologin of different users without closing browser - 122010
                string rUrlQuery;
                if (string.IsNullOrEmpty(CustomerIdQueryString))
                {
                  rUrlQuery = "?Client=" + ClientIdQueryString + "&User=" + CurrentPrincipal.CffUser.EmployeeId;   
                  if (SessionWrapper.Instance.Get != null) {
                     rUrlQuery = "?Client=" + SessionWrapper.Instance.Get.ClientFromQueryString.Id.ToString() + "&User=" + CurrentPrincipal.CffUser.EmployeeId;
                     if (SessionWrapper.Instance.Get.CustomerFromQueryString != null)
                        rUrlQuery = "&Customer=" + SessionWrapper.Instance.Get.CustomerFromQueryString.Id.ToString();
                  } else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                  {
                     rUrlQuery = "?Client=" + SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id.ToString() + "&User=" + CurrentPrincipal.CffUser.EmployeeId;
                     if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString != null)
                        rUrlQuery = "&Customer=" + SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString.Id.ToString();
                  } 
                }
                else
                {
                    rUrlQuery = "?Client=" + ClientIdQueryString + "&Customer=" + CustomerIdQueryString + "&User=" + CurrentPrincipal.CffUser.EmployeeId;
                    if (SessionWrapper.Instance.Get != null) {
                        if (SessionWrapper.Instance.Get.CustomerFromQueryString == null && SessionWrapper.Instance.Get.MultiClientSelected)
                            rUrlQuery = "?Client=" + ClientIdQueryString + "&User=" + CurrentPrincipal.CffUser.EmployeeId;
                    }
                    else if (!string.IsNullOrEmpty(QueryString.ViewIDValue)) {
                        if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString == null && SessionWrapper.Instance.Get.MultiClientSelected)
                            rUrlQuery = "?Client=" + ClientIdQueryString + "&User=" + CurrentPrincipal.CffUser.EmployeeId;
                    }
                }

                if (!string.IsNullOrEmpty(CriteriaQueryString))
                {
                    rUrlQuery += "&Criteria=" + CriteriaQueryString.Replace("#", "");
                }

                if (!string.IsNullOrEmpty(StartsWithQueryString))
                {
                    rUrlQuery += "&StartsWith=" + StartsWithQueryString;
                }
               
                if (!string.IsNullOrEmpty(ViewIDQueryString))
                {
                    rUrlQuery += "&ViewID=" + ViewIDQueryString;
                }

                if (!string.IsNullOrEmpty(BatchIDQueryString))
                {
                    rUrlQuery += "&Batch=" + BatchIDQueryString;
                }
                
                TransactionsLink.NavigateUrl = "~/Transactions.aspx" + rUrlQuery;
                TransactionArchiveLink.NavigateUrl = "~/TransactionArchive.aspx" + rUrlQuery;
                TransactionHistoryLink.NavigateUrl = "~/TransactionHistory.aspx" + rUrlQuery;
                TransactionSearchLink.NavigateUrl = "~/TransactionSearch.aspx" + rUrlQuery;
                ReportsLink.NavigateUrl = "~/Reports/Default.aspx" + rUrlQuery;
                NotesLink.NavigateUrl = "~/Notes.aspx" + rUrlQuery;
                ViewContactsLink.NavigateUrl = "~/Contacts.aspx" + rUrlQuery;
                InvoiceBatchesLink.NavigateUrl = "~/InvoiceBatches.aspx" + rUrlQuery;
                RetentionSchedulesLink.NavigateUrl = "~/RetentionSchedules.aspx" + rUrlQuery;
                
                DashboardLink.NavigateUrl = "~/Dashboard.aspx" + rUrlQuery;
            }
            else {
                DashboardLink.NavigateUrl = "~/Dashboard.aspx" + QueryStringParameters;
                TransactionsLink.NavigateUrl = "~/Transactions.aspx" + QueryStringParameters;
                TransactionArchiveLink.NavigateUrl = "~/TransactionArchive.aspx" + QueryStringParameters;
                TransactionHistoryLink.NavigateUrl = "~/TransactionHistory.aspx" + QueryStringParameters;
                TransactionSearchLink.NavigateUrl = "~/TransactionSearch.aspx" + QueryStringParameters;
                ReportsLink.NavigateUrl = "~/Reports/Default.aspx" + QueryStringParameters;
                NotesLink.NavigateUrl = "~/Notes.aspx" + QueryStringParameters;
                ViewContactsLink.NavigateUrl = "~/Contacts.aspx" + QueryStringParameters;
                InvoiceBatchesLink.NavigateUrl = "~/InvoiceBatches.aspx" + QueryStringParameters;
                RetentionSchedulesLink.NavigateUrl = "~/RetentionSchedules.aspx" + QueryStringParameters;          
            }

            if (!string.IsNullOrEmpty(ClientIdQueryString) && !string.IsNullOrEmpty(CustomerIdQueryString))
            {
                //start Ref: BT# 61  // dbb
                if (CurrentPrincipal.IsInAdministratorRole || CurrentPrincipal.IsInManagementRole)
                {
                    if (CurrentPrincipal.CffUser.UserType == UserType.EmployeeManagementUser ||
                            CurrentPrincipal.CffUser.UserType == UserType.EmployeeAdministratorUser ||
                                 CurrentPrincipal.CffUser.UserType == UserType.EmployeeStaffUser)
                    {
                       ViewContactsLink.Text = "Contacts/Letters";
                    }
                    else
                    {
                        Cff.SaferTrader.Core.Repositories.ICustomerRepository repo = Cff.SaferTrader.Core.Repositories.RepositoryFactory.CreateCustomerRepository();
                        ClientInformationAndAgeingBalances cinfo = repo.GetMatchedClientInfo(SessionWrapper.Instance.Get.ClientFromQueryString.Id);
                        //MSarza [20150901]
                        //if (cinfo.ClientInformation.AdministeredBy == "No" && (CurrentPrincipal.CffUser.UserType == UserType.ClientManagementUser)) 
                        if (cinfo.ClientInformation.IsClientDebtorAdmin == true && (CurrentPrincipal.CffUser.UserType == UserType.ClientManagementUser)) 
                        {  ViewContactsLink.Text = "Contacts/Letters"; }
                        else { ViewContactsLink.Text = "Contacts"; }
                    }
                }
                else
                {
                    ViewContactsLink.Text = "Contacts";
                } 
                //end Ref: BT# 61


                // start: by dbb
                //ICustomerRepository repo = RepositoryFactory.CreateCustomerRepository();
                //ClientInformationAndAgeingBalances cinfo = repo.GetMatchedClientInfo(Int32.Parse(ClientIdQueryString) == null ? 0 : Int32.Parse(ClientIdQueryString));    

                //if (cinfo.ClientInformation.CffDebtorAdmin.Equals(CffDebtorAdmin.AdminByCffAsCff) // cff as cff
                //    || cinfo.ClientInformation.CffDebtorAdmin.Equals(CffDebtorAdmin.AdminByCffAsClient) // cff as client
                //    || cinfo.ClientInformation.CffDebtorAdmin.Equals(CffDebtorAdmin.BothByCffasCffAndClientAsClient) // both: depends on login (cff as cff / client as client)
                //    || cinfo.ClientInformation.CffDebtorAdmin.Equals(CffDebtorAdmin.BothByCffasClientAndClientAsClient)) // both as client (ie cff as client / client as client)
                //{
                //    ViewContactsLink.Text = "Contacts/Letters";
                //}
                //else
                //{
                //    ViewContactsLink.Text = "Contacts";
                //}
                // end: by dbb

            }
            else
            {
                ViewContactsLink.Text = "Contacts";
            }
        }


        public void ToggleCurrentTransactionsLink(bool visible)
        {
            transactionsLi.Visible = visible;
        }

        public void ToggleTransactionsArchiveLink(bool visible)
        {
            transactionArchiveLi.Visible = visible;
        }

        public void ToggleContactsLink(bool visible)
        {
            contactsLink.Visible = visible;
        }

        public void ToggleReleaseTab(bool visible)
        {
            releaseTab.Visible = visible;
        }

        public void ToggleTransactionsHistoryLink(bool visible)
        {
            TransactionHistoryLink.Visible = visible;
        }

        public CffPrincipal CurrentPrincipal
        {
            get { return Context.User as CffPrincipal; }
        }

        public static string CustomerIdQueryString
        {
            get {
                string strCustId = "";
                if (SessionWrapper.Instance.Get != null) {
                    if (SessionWrapper.Instance.Get.CustomerFromQueryString != null)
                        strCustId = SessionWrapper.Instance.Get.CustomerFromQueryString.Id.ToString();
                }
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue)) {
                    if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue) != null) {
                        if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString != null)
                            strCustId = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString.Id.ToString();
                    }
                }
                else
                    strCustId = (QueryString.CustomerId == null) ? (!string.IsNullOrEmpty(strCustId) ? strCustId : "") : QueryString.CustomerId.ToString();

                if (string.IsNullOrEmpty(strCustId))
                    strCustId = System.Web.HttpContext.Current.Request.QueryString[QueryString.Customer.ToString()];
                
                strCustId = (string.IsNullOrEmpty(strCustId))?"":strCustId;
                if (strCustId.Contains(","))
                        strCustId = strCustId.Split(',')[0];

                return strCustId; 
            }
        }

        public static string ClientIdQueryString
        {
            get {
                if (SessionWrapper.Instance.Get != null) {
                    if (SessionWrapper.Instance.Get.CustomerFromQueryString != null)
                        return SessionWrapper.Instance.Get.ClientFromQueryString.Id.ToString();
                }
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue)) {
                    if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue) != null)
                        return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id.ToString();
                }
                
                return System.Web.HttpContext.Current.Request.QueryString[QueryString.Client.ToString()]; 
            }
        }

        public static string UserQueryString
        {
            get {
                return System.Web.HttpContext.Current.Request.QueryString[QueryString.User.ToString()]; 
            }
        }

        public static string CriteriaQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.Criteria.ToString()]; }
        }

        public static string StartsWithQueryString
        {
            get {
                return System.Web.HttpContext.Current.Request.QueryString[QueryString.StartsWith.ToString()];
            }
        }

        public static string ViewIDQueryString
        {
            get {
                if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return QueryString.ViewIDValue;

                return System.Web.HttpContext.Current.Request.QueryString[QueryString.ViewID.ToString()]; 
            }
        }
        
        public static string BatchIDQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.Batch.ToString()]; }
        }


        public static string QueryStringParameters
        {
            get
            { //ref: Ron's changes - CFF-12 II,22,23,24
                if (!string.IsNullOrEmpty(ClientIdQueryString) && !string.IsNullOrEmpty(CustomerIdQueryString))
                {
                    string queryString = "?Client=" + ClientIdQueryString + "&Customer=" + CustomerIdQueryString + "&User=" + UserQueryString;

                    if (!string.IsNullOrEmpty(CriteriaQueryString))
                    {
                        queryString += "&Criteria=" + CriteriaQueryString.Replace("#","");
                    }
                    if (!string.IsNullOrEmpty(StartsWithQueryString))
                    {
                        queryString += "&StartsWith=" + StartsWithQueryString;
                    }
                    if (!string.IsNullOrEmpty(ViewIDQueryString))
                    {
                        queryString += "&ViewID=" + ViewIDQueryString;
                    }

                    if (!string.IsNullOrEmpty(BatchIDQueryString))
                    {
                        if (System.Web.HttpContext.Current.Request.RawUrl.Contains("InvoiceBatches.aspx"))
                            queryString += "&Batch=" + BatchIDQueryString;
                    }

                    return queryString;
                }
                if (!string.IsNullOrEmpty(CustomerIdQueryString))
                {
                    string queryString = "?Customer=" + CustomerIdQueryString + "&User=" + UserQueryString;
                    if (!string.IsNullOrEmpty(CriteriaQueryString))
                    {
                        queryString += "&Criteria=" + CriteriaQueryString.Replace("#", "");
                    }
                    if (!string.IsNullOrEmpty(StartsWithQueryString))
                    {
                        queryString += "&StartsWith=" + StartsWithQueryString;
                    }
                    if (!string.IsNullOrEmpty(ViewIDQueryString))
                    {
                        queryString += "&ViewID=" + ViewIDQueryString;
                    }

                    if (!string.IsNullOrEmpty(BatchIDQueryString))
                    {
                        if (System.Web.HttpContext.Current.Request.RawUrl.Contains("InvoiceBatches.aspx"))
                                queryString += "&Batch=" + BatchIDQueryString;
                    }

                    return queryString;
                }
                if (!string.IsNullOrEmpty(ClientIdQueryString) && string.IsNullOrEmpty(CustomerIdQueryString))
                {
                    string queryString = "?Client=" + ClientIdQueryString + "&User=" + UserQueryString;
                    if (!string.IsNullOrEmpty(CriteriaQueryString))
                    {
                        queryString += "&Criteria=" + CriteriaQueryString.Replace("#", "");
                    }

                    if (!string.IsNullOrEmpty(StartsWithQueryString))
                    {
                        queryString += "&StartsWith=" + StartsWithQueryString;
                    }

                    if (!string.IsNullOrEmpty(ViewIDQueryString))
                    {
                        queryString += "&ViewID=" + ViewIDQueryString;
                    }

                    if (!string.IsNullOrEmpty(BatchIDQueryString))
                    {
                         if (System.Web.HttpContext.Current.Request.RawUrl.Contains("InvoiceBatches.aspx"))
                                queryString += "&Batch=" + BatchIDQueryString;
                    }
                    return queryString;
                }
                return "?Client=-1";
            }
        }
    }
}