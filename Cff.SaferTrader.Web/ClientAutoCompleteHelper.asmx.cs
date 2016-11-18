using System;
using System.Web.Services;
using System.Web.SessionState;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;

using System.Web.Script.Services;
using System.Collections.Generic;

namespace Cff.SaferTrader.Web
{
    /// <summary>
    /// Summary description for ClientAutoCompleteHelper
    /// </summary>
    [WebService(Namespace = "http://www.cff.co.nz/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class ClientAutoCompleteHelper : WebService, IRequiresSessionState, IScopedView
    {
        [WebMethod(EnableSession = true)]
        public string SelectClientFromAutoCompleteDropDown(string clientId)
        {
           try
           {
               string[] strDummy = clientId.Split('_');
               clientId = (strDummy[0].Contains("("))?parseClientID(strDummy[0]):strDummy[0];
               string viewID = strDummy[1];

                if (!string.IsNullOrEmpty(clientId))
                {
                    if ((((CffPrincipal)Context.User).IsInClientRole == true || ((CffPrincipal)Context.User).IsInCustomerRole == true) && (clientId == "-1")) 
                    {  //force to zero to return an error so we do not allow allclients scope for client or customer role
                        clientId = "0";
                    }

                    SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(clientId));
                    SessionWrapper.Instance.GetSession(viewID).IsDeselectingCustomer = false;
                    if (SessionWrapper.Instance.GetSession(viewID).CurrentUserID != (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString())
                        SessionWrapper.Instance.GetSession(viewID).CurrentUserID = (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString();

                    if (SessionWrapper.Instance.Get!=null)
                        SessionWrapper.Instance.Get.ClientFromQueryString = SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString;
                }

                IScopeService scopeService = new ScopeService(this,
                    SecurityManagerFactory.Create(Context.User as CffPrincipal, SessionWrapper.Instance.GetSession(viewID).Scope),
                    RepositoryFactory.CreateClientRepository(),
                    RepositoryFactory.CreateCustomerRepository()
                    );

               scopeService.SelectClient(int.Parse(clientId));
            }
            catch (FormatException)
            {

            }
            return string.Empty;
        }


        [WebMethod(EnableSession = true)]
        public string LoadClientNameAndNumber(string clientNameAndNumber)
        {
            string[] strDummy = clientNameAndNumber.Split('_');
            clientNameAndNumber = strDummy[0];
            string viewID = strDummy[1];
            string userID = (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString();
          
            if (clientNameAndNumber.ToUpper().Equals(SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString.NameAndNumber.ToUpper()) || clientNameAndNumber.Equals("All Clients"))
            {
                SessionWrapper.Instance.GetSession(viewID).CurrentUserID = userID;
                return BuildReturnJason(string.Empty);
            }
            else {
                SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(parseClientID(clientNameAndNumber)));
                SessionWrapper.Instance.GetSession(viewID).CurrentUserID = userID;
            }
            
            return BuildReturnJason(SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString.NameAndNumber);
        }


        [WebMethod(EnableSession = true)]
        public string CountHitSession(string viewId)
        {
            try
            {
                if (SessionWrapper.Instance.Get != null)
                {
                    string uuid = SessionWrapper.Instance.CountSessionHit(viewId);
                    return uuid;
                }
            }
            catch (Exception) {}
            return string.Empty;
        }

   
        private static string BuildReturnJason(string returnValue)
        {
            return "{ nameAndNumber: \"" + returnValue + "\"}";
        }

        private string parseClientID(string value)
        { //parse the client id here and reassign to session wrapper
            string clientID = "";
            int endIdx = value.IndexOf(')');
            string[] xDummy = value.Split('(');
            if (xDummy.Length > 2)
            {
                endIdx = xDummy[xDummy.Length - 1].ToString().IndexOf(')');
                clientID = xDummy[xDummy.Length - 1].ToString().Substring(0, endIdx);
            }
            else
            {
                endIdx = xDummy[xDummy.Length - 1].ToString().IndexOf(')');
                clientID = xDummy[1].ToString().Substring(0, endIdx);
            }
            return clientID;
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

        public ICffCustomer Customer
        {
            get {
                if (SessionWrapper.Instance.Get != null)
                    return SessionWrapper.Instance.Get.CustomerFromQueryString;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString;

                return null;
            }
            set
            {
                if (SessionWrapper.Instance.Get != null)
                    SessionWrapper.Instance.Get.CustomerFromQueryString = value;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString = value;
            }
        }


        [WebMethod(EnableSession = true)]
        public string ClearSessionIDPool(string viewId)
        {
            try
            {
                SessionWrapper.Instance.ClearSession(viewId);
            } catch (Exception){}
            return string.Empty;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public IList<ICffClient> GetClientNameAndNum()
        {
            int clientID = (this.Client!=null)?this.Client.Id:(QueryString.ClientId==null)?0:(int)QueryString.ClientId;
            int numberOfClientsToReturn = Config.NumberOfClientsToReturn - 1;

            IClientRepository cliRepo = RepositoryFactory.CreateClientRepository();
            return cliRepo.GetClientNameAndNum(clientID.ToString(), numberOfClientsToReturn);
        }


    }
}