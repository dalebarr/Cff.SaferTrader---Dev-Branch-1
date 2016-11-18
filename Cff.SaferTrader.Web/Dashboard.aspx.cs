using System;
using System.Web.Security;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Web
{
    public partial class Dashboard : BasePage, IDashboardView
    {
        private DashboardPresenter presenter;

        public string CustomerIdQueryString
        {
            get { return Request.QueryString[QueryString.Customer.ToString()]; }
        }

        public string ClientIdQueryString
        {
            get { return Request.QueryString[QueryString.Client.ToString()]; }
        }

        public string CriteriaQueryString
        {
            get { return Request.QueryString[QueryString.Criteria.ToString()]; }
        }

        public string UserQueryString
        {
            get { return Request.QueryString[QueryString.User.ToString()]; }
        }

        public string ViewIDQueryString
        {
            get { return Request.QueryString[QueryString.ViewIDValue]; }
        }

        private string retrieveSelectedClientID(string value)
        {
            string strClientID = "";
            if (value.Length > 0)
            {
                string[] strDummy = value.Split('(');
                if (strDummy.Length > 2)
                    strClientID = strDummy[strDummy.Length].Substring(0, (strDummy[strDummy.Length].Length - strDummy[strDummy.Length].IndexOf(')')));
                else if (strDummy.Length == 2)
                    strClientID = strDummy[1].Substring(0, strDummy[1].IndexOf(')'));
            }
            return strClientID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CffPrincipal cPrincipal = (CffPrincipal)Context.User;

            // verify if this user agreed on the CFF Agreement
            if (SessionWrapper.Instance.Get == null)
            {
                if ((cPrincipal.CffUser.UserType == UserType.EmployeeStaffUser) 
                        || (cPrincipal.CffUser.UserType == UserType.EmployeeManagementUser)
                            || (cPrincipal.CffUser.UserType == UserType.EmployeeAdministratorUser))
                {
                    string viewID = SaferTrader.Core.Common.StringEnum.GenerateUniqueKey(12);
                    SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(QueryString.ClientId));
                    SessionWrapper.Instance.GetSession(viewID).UserIdentity = 1;

                    string rUrl = ResolveClientUrl(Context.Request.Url.AbsolutePath + "?Client=" + QueryString.ClientId.ToString() + "&User=" + cPrincipal.CffUser.EmployeeId.ToString() + "&ViewID=" + viewID);
                    
                    string tUrl = this.Context.Request.RawUrl;
                    this.Response.Redirect(rUrl);
                }
                else
                { //redirect to logon page - allow only one window tab instance to open, unless it came from the reports tab etc
                    string rUrl = ResolveClientUrl("~/Logon.aspx");
                    string tUrl = this.Context.Request.RawUrl;
                    this.Response.Redirect(rUrl);
                }
            }

            if ((QueryString.ClientId != cPrincipal.CffUser.ClientId) && (cPrincipal.IsInClientRole || cPrincipal.IsInCustomerRole)) {
                SessionWrapper.Instance.Get.ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId((int)cPrincipal.CffUser.ClientId);
            }

            if (Page.IsPostBack) {
                int ClientId = (this.Client != null) ? this.Client.Id : 0;
                if (ClientId == 0)
                {
                    if (QueryString.ClientId == null)
                        ClientId = Convert.ToInt32(cPrincipal.CffUser.ClientId.ToString());
                    else
                        ClientId = (int)QueryString.ClientId;
                }

                if (SessionWrapper.Instance.Get != null) {
                    if ((SessionWrapper.Instance.Get.ClientFromQueryString.Id != ClientId) 
                            && (!string.IsNullOrEmpty(SessionWrapper.Instance.Get.AccountsIDList)))
                        SessionWrapper.Instance.Get.ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(ClientId);

                    else if (SessionWrapper.Instance.Get.ClientFromQueryString.Id != ClientId) {
                        
                        if (cPrincipal.CffUser.UserType == UserType.EmployeeAdministratorUser || 
                                cPrincipal.CffUser.UserType == UserType.EmployeeManagementUser || cPrincipal.CffUser.UserType == UserType.EmployeeStaffUser)
                                   SessionWrapper.Instance.Get.ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(ClientId);
                        else if (cPrincipal.IsInClientRole || cPrincipal.IsInCustomerRole)
                            SessionWrapper.Instance.Get.ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(cPrincipal.CffUser.ClientId.ToString()));
                    
                    }
                }
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue)) {
                    if ((SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id != ClientId)
                            && (!string.IsNullOrEmpty(SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).AccountsIDList)))
                        SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(ClientId);

                    else if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id != ClientId)
                    {

                        if (cPrincipal.CffUser.UserType == UserType.EmployeeAdministratorUser ||
                                cPrincipal.CffUser.UserType == UserType.EmployeeManagementUser || cPrincipal.CffUser.UserType == UserType.EmployeeStaffUser)
                            SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(ClientId);
                        else if (cPrincipal.IsInClientRole || cPrincipal.IsInCustomerRole)
                            SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(cPrincipal.CffUser.ClientId.ToString()));
                    }
                }
            }

            IScopeService scopeService = new ScopeService(this,
                                          SecurityManagerFactory.Create(Context.User as CffPrincipal, SessionWrapper.Instance.Get.Scope),
                                          RepositoryFactory.CreateClientRepository(),
                                          RepositoryFactory.CreateCustomerRepository());

            presenter = new DashboardPresenter(this, scopeService);

            MembershipUser membershipUser = Membership.GetUser(User.Identity.Name);
            
            if ( presenter.IsReadAgreement((Guid)membershipUser.ProviderUserKey) == false)
                RedirectTo("AgreementPage.aspx");
            else
            {
                if (!IsPostBack)
                {
                    presenter.InitialiseView();
                }
                content.InnerHtml = presenter.DashboardMainContent();
            }
        }
    }
}