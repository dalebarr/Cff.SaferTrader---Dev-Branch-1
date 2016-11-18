using System;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Services;

namespace Cff.SaferTrader.Web
{
    public partial class LogOnRedirection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CffPrincipal cffPrincipal = Context.User as CffPrincipal;
            if (cffPrincipal != null)
            {
                CffUserService cffUserService = CffUserService.Create();
                string returnUrl = Request.QueryString["ReturnUrl"];
                bool rememberMe = Request.QueryString["RememberMe"] != null && Request.QueryString["RememberMe"].Equals(true.ToString());
                string userName = cffPrincipal.Identity.Name;
                string viewID = Request.QueryString["ViewID"];
                string criteria = Request.QueryString["Criteria"];


                // If ReturnUrl is set, redirect to it
                // This logic shouldn't have to have been implemented according to MSDN but it doesn't seem to use ReturnUrl
                System.Web.Security.FormsAuthentication.SetAuthCookie(userName, rememberMe);
                if ((cffPrincipal.CffUser.EmployeeId == QueryString.UserId) && (!string.IsNullOrEmpty(returnUrl)))
                {
                    if (!string.IsNullOrEmpty(viewID))
                    {
                        string url1 = returnUrl;
                        int idx1 = returnUrl.IndexOf("ViewID");
                        if (idx1 >= 0)
                        {
                            url1 = returnUrl.Substring(0, idx1 - 1);
                            string url2 = returnUrl.Substring(returnUrl.IndexOf("ViewID"));
                            idx1 = url2.IndexOf("&");
                            if (idx1 >= 0)
                            {
                                url2 = url2.Substring(idx1);
                                returnUrl = url1 + url2;
                            }
                            else
                                returnUrl = url1;
                        }
                        returnUrl += "&ViewID=" + viewID;
                    }

                    if (!string.IsNullOrEmpty(criteria))
                    {
                        string url1 = returnUrl;
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            int idx1 = returnUrl.IndexOf("Criteria");
                            if (idx1 >= 0)
                            {
                                url1 = returnUrl.Substring(0, idx1 - 1);
                                string url2 = returnUrl.Substring(returnUrl.IndexOf("Criteria"));
                                idx1 = url2.IndexOf("&");
                                if (idx1 >= 0)
                                {
                                    url2 = url2.Substring(idx1);
                                    returnUrl = url1 + url2;
                                }
                                else
                                    returnUrl = url1;
                            }
                        }
                        returnUrl += "&Criteria=" + criteria.Replace("#", "");
                    }
                    Response.Redirect(returnUrl);
                }
                else
                {
                    returnUrl = "~/Dashboard.aspx";
                    if (cffUserService.LoadCffCustomerAssociatedWith(cffPrincipal.CffUser) != null)
                    {
                        returnUrl = returnUrl + "?Client=" + cffUserService.LoadCffClientAssociatedWith(cffPrincipal.CffUser).Id +
                                   "&Customer=" + cffUserService.LoadCffCustomerAssociatedWith(cffPrincipal.CffUser).Id +
                                   "&User=" + cffUserService.LoadCffCustomerAssociatedWith(cffPrincipal.CffUser).Number +
                                   "&ViewID=" + viewID;
                    }
                    else
                    {
                        returnUrl = returnUrl + "?Client=" + cffUserService.LoadCffClientAssociatedWith(cffPrincipal.CffUser).Id +
                              "&User=" + cffPrincipal.CffUser.EmployeeId + "&ViewID=" + viewID;
                    }
                    if (QueryString.Criteria != null)
                    {
                        returnUrl += "&Criteria=" + QueryString.CriteriaValue.ToString().Replace("#", "");
                    }
                    Response.Redirect(returnUrl);
                }
                //}

            }
        }

    }
}