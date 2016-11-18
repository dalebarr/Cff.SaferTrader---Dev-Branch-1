using System;
using System.Web.Security;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Views;
using System.Web;

namespace Cff.SaferTrader.Web
{
    public class BasePage : Page, IRedirectableView
    {
        protected virtual void Page_Init(object sender, EventArgs e)
        {
            CffPrincipal cffPrincipal = Context.User as CffPrincipal;
            if (cffPrincipal != null && Session.IsNewSession)
            {
                Cff.SaferTrader.Core.Services.CffUserService cffUserService = Cff.SaferTrader.Core.Services.CffUserService.Create();
                if (SessionWrapper.Instance.Get == null && QueryString.ViewIDValue!=null) {
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString = cffUserService.LoadCffClientAssociatedWith(cffPrincipal.CffUser);
                }
                else if (SessionWrapper.Instance.Get !=null) {
                    SessionWrapper.Instance.Get.ClientFromQueryString = cffUserService.LoadCffClientAssociatedWith(cffPrincipal.CffUser);
                }
            }

            ((SafeTrader)Master).ScopeChanged += ScopeChanged;
        }

        protected virtual void ScopeChanged(object sender, EventArgs e)
        {
        }

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

        public bool IsCustomerSessionScopeSelected {
            get {
               return (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.IsCustomerSelected :
                               (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsCustomerSelected : false;
            }
        }

        public bool IsClientSessionScopeSelected
        {
            get
            {
                return (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.IsClientSelected :
                                (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsClientSelected : false;
            }
        }

        public bool IsAllClientsSessionScopeSelected
        {
            get
            {
                return (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.IsAllClientsSelected :
                                (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsAllClientsSelected : false;
            }
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Context.Session != null)
            {
                if (Session.IsNewSession)
                {
                    string szCookieHeader = Request.Headers["Cookie"];
                    //if session expired..
                    if ((null != szCookieHeader) && (szCookieHeader.IndexOf("ASP.NET_SessionId", StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        if (Request.IsAuthenticated)
                        {
                            FormsAuthentication.SignOut();
                        }

                        // clear authentication cookie
                        HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                        cookie1.Expires = DateTime.Now.AddYears(-1);
                        Response.Cookies.Add(cookie1);

                        // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
                        HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
                        cookie2.Expires = DateTime.Now.AddYears(-1);
                        Response.Cookies.Add(cookie2);

                        FormsAuthentication.RedirectToLoginPage();
                    }
                }
            }
        }

        public void RedirectTo(string redirectionPath)
        {
            Response.Redirect(redirectionPath);
        }

        public ICffClient Client
        {
            get {
                if (SessionWrapper.Instance.Get != null)
                    return SessionWrapper.Instance.Get.ClientFromQueryString;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString;

                return null;
            }
            set {
                if (SessionWrapper.Instance.Get != null)
                    SessionWrapper.Instance.Get.ClientFromQueryString = value; 
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString = value; 
            }
        }

        //public CffCustomer Customer
        public ICffCustomer Customer
        {
            get {

                if (SessionWrapper.Instance.Get != null)
                    return SessionWrapper.Instance.Get.CustomerFromQueryString; 
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString;

                return null;
            }
            set {
                if (SessionWrapper.Instance.Get != null)
                    SessionWrapper.Instance.Get.CustomerFromQueryString = value; 
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString = value; 
            }
        }
    }
}