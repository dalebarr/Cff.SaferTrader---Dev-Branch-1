using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Services;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Web
{
    [WebService(Namespace = "http://www.cff.co.nz/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class CustomerAutoCompleteHelper : WebService, IScopedView
    {
        private readonly DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof (CffClient));

        [WebMethod(EnableSession = true)]
        public string SelectCustomerFromAutoCompleteDropDown(string customerId)
        {
            //assign new customer instance here based from retrieved customer number and existing clientid
            Scope xScope = Scope.AllClientsScope;
            CffCustomerExt xCustomer = null;
            CffCustomer cCustomer = null;
            if (SessionWrapper.Instance.Get != null)
            {
                if (SessionWrapper.Instance.Get.EmptyWindowHit == 0) { SessionWrapper.Instance.Get.EmptyWindowHit = 1; }
                if (xCustomer == null)
                {
                    if (customerId.Contains("("))
                    { //this is customer number! we should be able to retrieve correct customer id + client id
                        customerId = customerId.Substring(customerId.IndexOf("(") + 1);
                        customerId = customerId.Replace(")", "");
                    }

                    if (!string.IsNullOrEmpty(customerId)) {
                        cCustomer = (RepositoryFactory.CreateCustomerRepository().GetMatchedCustomerInfo(Convert.ToInt32(customerId),
                                                      SessionWrapper.Instance.Get.ClientFromQueryString.Id).CffCustomerInformation.Customer);
                        xCustomer = new CffCustomerExt(cCustomer.Name, cCustomer.Id, cCustomer.Number);
                    }
                }

                if (xCustomer != null) {
                    SessionWrapper.Instance.Get.CustomerFromQueryString = (ICffCustomer)xCustomer;
                    if (SessionWrapper.Instance.Get.CurrentUserID != (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString())
                        SessionWrapper.Instance.Get.CurrentUserID = (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString();
                }
            } else if (!string.IsNullOrEmpty(QueryString.ViewIDValue)) {
                 if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue) != null)
                 {
                     xScope = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).Scope;
                     if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).EmptyWindowHit == 0) { SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).EmptyWindowHit = 1; }

                     cCustomer = (RepositoryFactory.CreateCustomerRepository().GetMatchedCustomerInfo(Convert.ToInt32(customerId),
                                                  SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id).CffCustomerInformation.Customer);
                     xCustomer = new CffCustomerExt(cCustomer.Name, cCustomer.Id, cCustomer.Number);
                     if (xCustomer != null)
                         SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString = (ICffCustomer)xCustomer;

                     if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CurrentUserID != (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString())
                         SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CurrentUserID = (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString();
                 }
             }


            IScopeService scopeService = new ScopeService(this,
                                                         SecurityManagerFactory.Create(Context.User as CffPrincipal, xScope),
                                                         RepositoryFactory.CreateClientRepository(),
                                                         RepositoryFactory.CreateCustomerRepository());

            string jSon = string.Empty;
            if (!string.IsNullOrEmpty(customerId))
            {
                try
                {
                    scopeService.SelectCustomer(int.Parse(customerId));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        serializer.WriteObject(stream, Client);
                        jSon = Encoding.Default.GetString(stream.ToArray());
                    }
                }
                catch (FormatException)
                {
                }
            }

            return jSon;
        }

        [WebMethod(EnableSession = true)]
        public string LoadCustomerNameAndNumber(string customerNameAndNumber)
        {
            if (SessionWrapper.Instance.Get == null)
                return "";

            if (string.IsNullOrEmpty(customerNameAndNumber))
            {
                SessionWrapper.Instance.Get.CustomerFromQueryString = null;
                SessionWrapper.Instance.Get.IsDeselectingCustomer = true;
                return string.Empty;
            }
            else {
                SessionWrapper.Instance.Get.MultiClientSelected = false;
                if (SessionWrapper.Instance.Get.CurrentUserID != (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString())
                    SessionWrapper.Instance.Get.CurrentUserID = (System.Web.HttpContext.Current.User as CffPrincipal).CffUser.UserId.ToString();

            }
            return BuildReturnJason(customerNameAndNumber);
        }

        [WebMethod]
        public string GetClientIdByCustomerId(string customerId)
        {
            string clientId = "";
            try
            {
                if (customerId.Contains("(")) { 
                    //this is customer number, we should be able to get the correct customer id based from cust# and clientid
                    customerId = customerId.Substring(customerId.IndexOf("(")+1);
                    customerId = customerId.Replace(")", "");
                }

                if (!string.IsNullOrEmpty(customerId))
                {
                    CffClient cffclient = RepositoryFactory.CreateClientRepository().GetCffClientByCustomerId(Convert.ToInt32(customerId));
                    clientId = cffclient.Id.ToString();
                }
            }
            catch (HttpException ex)
            {
                Logger logger = new Logger();
                logger.LogError(ex.GetBaseException());
                clientId = "-1";
            }
            return clientId;
        }

        private static string BuildReturnJason(string returnValue)
        {
            return "{ nameAndNumber: \"" + returnValue + "\"}";
        }

        public ICffClient Client
        {
            get { return (SessionWrapper.Instance.Get==null)?null:SessionWrapper.Instance.Get.ClientFromQueryString; }
            set { }
            //get { return SessionWrapper.Instance.Get.ClientFromQueryString; }
            //set { SessionWrapper.Instance.Get.ClientFromQueryString = value;}
        }

        public ICffCustomer Customer
        {
            get { return SessionWrapper.Instance.Get.CustomerFromQueryString; }
            set { SessionWrapper.Instance.Get.CustomerFromQueryString = (ICffCustomer)value; }
            //get { return SessionWrapper.Instance.Get.Customer; }
            //set { SessionWrapper.Instance.Get.Customer = value; }
        }

        [WebMethod(EnableSession = true)]
        public string CountHitSession(string viewId, string clientId, string customerId)
        {
            string ret = string.Empty;
            try
            {
                string userId = ((CffPrincipal)(HttpContext.Current.User)).CffUser.UserName;
                string sClientId= ((CffPrincipal)(HttpContext.Current.User)).CffUser.ClientId.ToString();
                string uuid = SessionWrapper.Instance.CountSessionHit(viewId);
                ret =   (((!string.IsNullOrEmpty(uuid))? (uuid):"null") 
                            + "_" + (string.IsNullOrEmpty(clientId)?"null":clientId) 
                            + "_" + (string.IsNullOrEmpty(customerId)?"null":customerId)) ;
            }
            catch (Exception) { }
            return ret;
        }
    }
}