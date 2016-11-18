using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Services;


namespace Cff.SaferTrader.Web
{
    public class Global : HttpApplication
    {
// ReSharper disable InconsistentNaming
        /// <summary>
        /// Load Client and Customer into session when a new session is created
        /// </summary>
        /// 
        public void Session_OnStart()
        // ReSharper restore InconsistentNaming
        {
            CffPrincipal cffPrincipal = Context.User as CffPrincipal;
            if (cffPrincipal != null && Session.IsNewSession)
            {
                Session.Timeout = 3600;
                CffUserService cffUserService = CffUserService.Create();
                if (SessionWrapper.Instance.Get != null) {
                    SessionWrapper.Instance.Get.ClientFromQueryString = cffUserService.LoadCffClientAssociatedWith(cffPrincipal.CffUser);
                    SessionWrapper.Instance.Get.CustomerFromQueryString = (ICffCustomer)cffUserService.LoadCffCustomerAssociatedWith(cffPrincipal.CffUser);
                }
            }
         }

        protected void Application_Start(Object sender, EventArgs e)
        {
            //System.Net.WebRequest.DefaultWebProxy = new System.Net.WebProxy("127.0.0.1", 8888);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                if (Server.GetLastError() != null)
                {
                    Logger logger = new Logger();
                    logger.LogError(Server.GetLastError().GetBaseException());

                    /// if error generated from code, redirect to error page and clear the error
                    if (Server.GetLastError() is HttpUnhandledException)
                    {
                        Server.ClearError();
                        Server.Transfer("~/Error.aspx");
                        return;
                    }
                }
            }
            catch (Exception) { }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
        protected void Application_AuthorizeRequest(object sender, EventArgs e)
        {
            IPrincipal user = Context.User;
            if (user != null && user.Identity.IsAuthenticated && user is RolePrincipal)
            {
                try
                {
                    if (Request.Url.ToString().Contains("myob"))
                    {
                        return;
                    }

                    MembershipUser membershipUser = Membership.GetUser(user.Identity.Name);
                    CffUserService cffUserService = CffUserService.Create();
                    ICffUser cffUser = cffUserService.LoadCffUser(new Guid(membershipUser.ProviderUserKey.ToString()));
                    if (cffUser != null)
                    {
                        CffPrincipal cffPrincipal = new CffPrincipal(user, cffUser);
                        Context.User = cffPrincipal;
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
          
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // If the request is for KeepSessionAlive.aspx and it's coming from a sub-folder (such as
            // /sub/admin/KeepSessionAlive.aspx, not /KeepSessionAlive.aspx) then redirect to proper location.
            //Note: server.transfer does not pass the query string parameter values to KeepSessionAlive even when preserveform option is set to true
            //we will need to find a way to work around this problem as Response.Redirect is an expensive call
            //Server.Transfer("~/KeepSessionAlive.aspx", true);
            if (Request.Url.LocalPath.IndexOf("/KeepSessionAlive.aspx") > 0)
            {
                Response.Redirect("~/KeepSessionAlive.aspx");  //Server.Transfer("~/KeepSessionAlive.aspx", true); 
            } 
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
           //Check if the user is Authenticated and been redirected to login page 
            //Response.StatusCode = 302  && (Response.RedirectLocation.ToUpper().Contains("LOGIN.ASPX"))
           //if (Request.IsAuthenticated==false && (Response.StatusCode == 200))
           //{
               //check if the user has access to the page
               //bool isUserAccess = UrlAuthorizationModule.CheckUrlAccessForPrincipal(Request.FilePath, User, "GET");
               //if (isUserAccess=false)              
               // {
                    //Pass a parameter to the login.aspx page 
                    //FormsAuthentication.RedirectToLoginPage("errCode=401");
                    
                        //Or you can redirect him to another page like AuthenticationFailed.aspx
                        //Response.Redirect("AuthenticationFaild.aspx");
                //}
            //}           
        }
    }
}