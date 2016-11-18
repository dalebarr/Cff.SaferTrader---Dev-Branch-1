using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;


namespace Cff.SaferTrader.Web
{
    [WebService(Namespace = "http://www.cff.co.nz/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CustomerSearch : IHttpHandler, ICustomerSearchView, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string criteria = context.Request.Params["Criteria"];
            CustomerSearchPresenter presenter = CustomerSearchPresenter.Create(this);

            CffPrincipal cffPrincipal = HttpContext.Current.User as CffPrincipal;
            CffUserService cffUserService = CffUserService.Create();

           
            if (cffPrincipal != null && (cffPrincipal.IsInCustomerRole || cffPrincipal.IsInClientRole)) 
            {
                if (SessionWrapper.Instance.Get.IsMultipleAccounts && QueryString.ClientId!= -1) {
                    string rawURl = context.Request.RawUrl;
                    rawURl = rawURl.Substring(rawURl.IndexOf("Client"));
                    rawURl = rawURl.Substring(rawURl.IndexOf("=")+1);
                    int eidx  = rawURl.IndexOf("&");
                    if (eidx>0)
                        rawURl = rawURl.Substring(0,eidx);
                     int clientId = int.Parse(rawURl);
                     presenter.ShowMatchedNames(context.Request.Params["q"], clientId, System.Convert.ToInt32((criteria == "") ? "0" : criteria));
                }
                else 
                  presenter.ShowMatchedNames(context.Request.Params["q"], (cffUserService.LoadCffClientAssociatedWith(cffPrincipal.CffUser)).Id, System.Convert.ToInt32((criteria=="")?"0":criteria));
            }
            else
            {
                if (cffPrincipal.IsInClientRole || cffPrincipal.IsInCustomerRole) {
                    presenter.ShowMatchedNames(context.Request.Params["q"], SessionWrapper.Instance.Get.ClientFromQueryString.Id, System.Convert.ToInt32((criteria=="")?"0":criteria));
                }
                else 
                {
                    int clientID=-1;
                    if (context.Request.Params["Client"] != null) {
                        string contextPar = context.Request.Params["Client"].Replace("+", " ");
                        if (contextPar.Contains("All Clients")) 
                            clientID=-1;
                        else
                            clientID = System.Convert.ToInt32(context.Request.Params["Client"].ToString());
                        
                        string viewID = QueryString.ViewIDValue;
                        int? SessionClientID= null;

                        if (!string.IsNullOrEmpty(viewID))
                            SessionClientID  = (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue)).ClientFromQueryString.Id;
                        else if (SessionWrapper.Instance.Get!=null)
                            SessionClientID = SessionWrapper.Instance.Get.ClientFromQueryString.Id;

                        if (SessionClientID != null && (viewID != null))
                        {
                            if (SessionClientID != clientID
                                    && (cffPrincipal.CffUser.UserType == UserType.EmployeeAdministratorUser
                                            || cffPrincipal.CffUser.UserType == UserType.EmployeeManagementUser
                                                 || cffPrincipal.CffUser.UserType == UserType.EmployeeStaffUser))
                            { //if not in client role, we must be able to reset session wrapper's querystring details from here
                                var nameValues = HttpUtility.ParseQueryString(context.Request.QueryString.ToString());
                                nameValues.Set("Client", SessionClientID.ToString());
                                string url = context.Request.Url.AbsolutePath;
                                string updatedQueryString = "?" + nameValues.ToString();
                                //context.Response.Redirect(url + updatedQueryString);

                                SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(System.Convert.ToInt32(SessionClientID));
                                SessionWrapper.Instance.GetSession(viewID).IsDeselectingCustomer = false;
                                if (SessionWrapper.Instance.GetSession(viewID).CurrentUserID != (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString())
                                    SessionWrapper.Instance.GetSession(viewID).CurrentUserID = (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString();

                                if (SessionWrapper.Instance.Get != null) {
                                    SessionWrapper.Instance.Get.ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(System.Convert.ToInt32(SessionClientID)); ;
                                    SessionWrapper.Instance.Get.IsDeselectingCustomer = false;
                                    SessionWrapper.Instance.Get.CurrentUserID = (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString();
                                }
                                
                                clientID = (int)SessionClientID;
                            }
                        }
                    }
                    //if (context.Request.Params["Client"] == clientID.ToString())
                    //{
                        presenter.ShowMatchedNames(context.Request.Params["q"], clientID, System.Convert.ToInt32((string.IsNullOrEmpty(criteria)) ? "0" : criteria));
                    //}
                }
            }
        }

        public void DisplayMatchedSearch(string matchedCustomersJSON)
        {
            
            matchedCustomersJSON = "[" + matchedCustomersJSON + "]";
            //System.Diagnostics.Debug.WriteLine(matchedCustomersJSON);
            HttpContext.Current.Response.Write(matchedCustomersJSON);
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}