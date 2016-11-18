using System;
using System.Collections.Generic;
using System.Web;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class SessionWrapperBase
    {
        private Boolean isMultipleAccounts;
        private string currentUserID;
        
        protected ICffClient clientDetails;
        protected ICffCustomer customerDetails;
        //private CffCustomer customerDetails;

        private readonly string sessionID;
        private string accountIDList;
        private int _emptyWindowHit;
        private bool _multiClientSelected;


        public SessionWrapperBase()
        {
            IsStartsWithChecked = true;
        }

        public SessionWrapperBase(string pSessionID)
        {
            sessionID = pSessionID;
        }


        public SessionWrapperBase Clone()
        { //shallow copy
            return (SessionWrapperBase)this.MemberwiseClone();
        }

        public bool IsDeselectingCustomer
        {
            get { return GetFromSession<bool>("IsDeselectingCustomer"); }
            set { SetInSession("IsDeselectingCustomer", value); }
        }

        public bool IsHidePermanentNotesGridView
        {
            get { return GetFromSession<bool>("IsHidePermanentNotesGridView"); }
            set { SetInSession("IsHidePermanentNotesGridView", value); }
        }

        public bool IsContactShown
        {
            get { return GetFromSession<bool>("IsContactShown"); }
            set { SetInSession("IsContactShown", value); }
        }

        public bool IsStartsWithChecked
        {
            get { return GetFromSession<bool>("IsStartsWithChecked"); }
            set { SetInSession("IsStartsWithChecked", value); }
        }

        public bool IsEditContactDetails
        {
            get { return GetFromSession<bool>("IsEditContactDetails"); }
            set { SetInSession("IsEditContactDetails", value); }
        }

        public bool IsEditCContactDetails
        {
            get { return GetFromSession<bool>("IsEditCContactDetails"); }
            set { SetInSession("IsEditCContactDetails", value); }
        }

        public bool IsSessionExpiring
        {
            get { return GetFromSession<bool>("IsSessionExpiring"); }
            set { SetInSession("IsSessionExpiring", value); }
        }


        public string SelectedDateInDatePicker
        {
            get { return GetFromSession<string>("SelectedDateInDatePicker"); }
            set { SetInSession("SelectedDateInDatePicker", value); }
        }

        public string SelectedDateFromInDatePicker
        {
            get { return GetFromSession<string>("SelectedDateFromInDatePicker"); }
            set { SetInSession("SelectedDateFromInDatePicker", value); }
        }

        public string SelectedDateToInDatePicker
        {
            get { return GetFromSession<string>("SelectedDateToInDatePicker"); }
            set { SetInSession("SelectedDateToInDatePicker", value); }
        }

        public string SelectedTransactionFilter
        {
            get { return GetFromSession<string>("SelectedTransactionFilter"); }
            set { SetInSession("SelectedTransactionFilter", value); }
        }

        public IList<TransactionSearchResult> TransactionSearchResult
        {
            get { return GetFromSession<IList<TransactionSearchResult>>("TransactionSearchResult"); }
            set { SetInSession("TransactionSearchResult", value); }
        }

        private T GetFromSession<T>(string key)
        {
            object obj = HttpContext.Current.Session[key];
            if (obj == null)
            {
                return default(T);
            }
            return (T)obj;
        }

        private void SetInSession<T>(string key, T value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public ICffClient GetClientDetails()
        {
            if (clientDetails == null)
            {
                string clientId = "-1";
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Client"]))
                    clientId = HttpContext.Current.Request.QueryString["Client"];
                else if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Customer"]))
                {
                    string customerId = HttpContext.Current.Request.QueryString["Customer"];
                    clientDetails = RepositoryFactory.CreateClientRepository().GetCffClientByCustomerId(Convert.ToInt32(customerId));
                }
                else
                    clientId = "-1";

                clientDetails = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(clientId));
            }

            return clientDetails;
        }

        public ICffCustomer GetCustomerDetails()
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Customer"]))
            {
                string customerId = HttpContext.Current.Request.QueryString["Customer"];
                if (!string.IsNullOrEmpty(customerId)) {
                    customerId = customerId.Replace("(", "").Replace(")", "");

                    //cleanup - for cases where querystring duplicates
                    if (customerId.Contains(","))
                        customerId = customerId.Split(',')[0];

                    if (customerDetails == null)
                        customerDetails = (ICffCustomer)((CffCustomerExt)RepositoryFactory.CreateCustomerRepository().GetCffCustomerByCustomerIdNew((Convert.ToInt32(customerId))));
                    else
                    {
                        if (customerDetails.Id.ToString() != customerId)
                            customerDetails = (ICffCustomer)((CffCustomerExt)RepositoryFactory.CreateCustomerRepository().GetCffCustomerByCustomerIdNew((Convert.ToInt32(customerId))));
                    }
                    return customerDetails;
                }
                return null;
            }

            return null;
        }


        /// <summary>
        /// retrieve the full object of CffClient by passing client id from query string
        /// </summary>
        /// <returns></returns>
        /// public static ICffClient GetClientFromQueryString
        public ICffClient ClientFromQueryString
        {
            get
            {
                return GetClientDetails();
            }
            set
            {
                this.clientDetails = null; //nullify to be sure that we clear this out
                this.clientDetails = value;
            }
        }


        /// <summary>
        /// Get customer object from query string
        /// </summary>
        //public CffCustomer CustomerFromQueryString
        public ICffCustomer CustomerFromQueryString
        {
            get
            {
                return GetCustomerDetails();
            }
            set
            {
                this.customerDetails = null; //nullify to be sure that we clear this out
                this.customerDetails = value;
            }
        }


        // Note: DO NOT USE - this will eventually become a private property, superseded by Scope property
        //public static bool IsAllClientsSelected
        public bool IsAllClientsSelected
        {
            get
            {
                if (this.ClientFromQueryString != null && this.ClientFromQueryString.Id == -1)
                        return true;
                return false;
            }

        }


        // Note: DO NOT USE - this will eventually become a private property, superseded by Scope property
        //public static bool IsClientSelected
        public bool IsClientSelected
        {
            get {
                return (this.ClientFromQueryString != null); 
            }
        }

        // Note: DO NOT USE - this will eventually become a private property, superseded by Scope property
        public bool IsCustomerSelected
        {
            get
            {
                if (this.CustomerFromQueryString == null || this.CustomerFromQueryString.Id <= 0)
                {
                    return false;
                }
                return true;
            }
        }


        public Scope Scope
        {
            get
            {
                Scope scope = Scope.ClientScope;
                if (IsAllClientsSelected)
                {
                    scope = Scope.AllClientsScope;
                }
                else if (IsCustomerSelected)
                {
                    scope = Scope.CustomerScope;
                }
                return scope;
            }
        }

        public bool IsMultipleAccounts
        {
            get { return isMultipleAccounts; }
            set { isMultipleAccounts = value; }
        }

        public string AccountsIDList
        {
            get { return accountIDList; }
            set { accountIDList = value; }
        }

        public ISessionCache SessionCache
        {
            get { return GetFromSession<ISessionCache>("SessionCache"); }
            set { SetInSession("SessionCache", value); }
        }

        public IPrintable PrintBag
        {
            get { return GetFromSession<IPrintable>("PrintBag"); }
            set { SetInSession("PrintBag", value); }
        }

        public IDocument DocBag
        {
            get { return GetFromSession<IDocument>("DocBag"); }
            set { SetInSession("DocBag", value); }
        }
        //MSarza [20150901] 
        //public bool IsClientAdminByCFF
        //{
        //    get { return GetFromSession<bool>("IsClientAdminByCFF"); }
        //    set { SetInSession("IsClientAdminByCFF", value); }
        //}
        public bool IsClientDebtorAdmin
        {
            get { return GetFromSession<bool>("IsClientDebtorAdmin"); }
            set { SetInSession("IsClientDebtorAdmin", value); }
        }
        public bool IsCffDebtorAdminForClient
        {
            get { return GetFromSession<bool>("IsCffIsDebtorAdminForClient"); }
            set { SetInSession("IsCffIsDebtorAdminForClient", value); }
        }

        public short UserIdentity
        { //1 - cffstaff; 2 - client; 3 - customer, 4 - multiple client login
            get { return GetFromSession<short>("UserIdentity"); }
            set { SetInSession("UserIdentity", value); }
        }

        //public bool StartsWith
        //{
        //   get { return GetFromSession<bool>("StartsWith"); }
        //    set { SetInSession("StartsWith", value); }
        //}

        public string SessionID
        {
            get { return sessionID; }
        }

        public string CurrentUserID
        {
            get { return currentUserID; }
            set { currentUserID = value; }
        }

        public int EmptyWindowHit
        {
            get { return _emptyWindowHit; }
            set { _emptyWindowHit = value; }
        }

        public bool MultiClientSelected
        {
            get { return _multiClientSelected; }
            set { _multiClientSelected = value; }
        }

    }

    public sealed class SessionWrapper : SessionWrapperBase
    {
        public Dictionary<string, SessionWrapperBase> SessionIDPool;
        private static readonly SessionWrapper instance= new SessionWrapper();
        new public enum UserIdentity : short
        {
            Employee = 1,
            Client = 2,
            Customer = 3
        }

        static SessionWrapper()
        {
        }

        private SessionWrapper()
        {
            SessionIDPool = new Dictionary<string, SessionWrapperBase>();
        }

        public static SessionWrapper Instance
        {
            get
            {
                return instance;
            }
        }

        public SessionWrapperBase GetSession(string viewID)
        {
            string pViewID = viewID;
            bool isNewSession = false;
            if (string.IsNullOrEmpty(viewID))
            {
                pViewID = SaferTrader.Core.Common.StringEnum.GenerateUniqueKey(12);
                isNewSession = true;
            }

            SessionWrapperBase sBase = null;
             string pSessionID = System.Web.HttpContext.Current.Session.SessionID + '_' + pViewID;
             if (!isNewSession) {
                    try
                    {
                        if (SessionIDPool.TryGetValue(pSessionID, out sBase) == false)
                            sBase = null;
                    }
                    catch (Exception) {}
            }

             if ((sBase == null) || (SessionIDPool.Count == 0))
             { //add in sessioninstancepool
                 sBase = new SessionWrapperBase(pSessionID);
                 sBase.EmptyWindowHit = 0;
                 sBase.MultiClientSelected = false;
                 sBase.IsStartsWithChecked = true;
                 SessionIDPool.Add(pSessionID, sBase);
                 SessionIDPool.TryGetValue(pSessionID, out sBase);
             }

             return sBase;
        }

        public SessionWrapperBase Get
        {
            get
            {
                SessionWrapperBase sBase = null;
                try { 
                    string pSessionID = "";
                    string pSessionViewID = "";
                    if (HttpContext.Current.Request.QueryString["ViewID"] == null)
                    {
                        if (string.IsNullOrEmpty(QueryString.ViewIDValue))
                            return null;
                        else
                            pSessionViewID = QueryString.ViewIDValue;
                    }
                    else
                    {
                        string[] strVID = HttpContext.Current.Request.QueryString["ViewID"].Split(',');
                        pSessionViewID = strVID[0];
                    }

                    pSessionID = System.Web.HttpContext.Current.Session.SessionID + '_' + pSessionViewID; //just get the first viewid
                    try
                    {
                        if (SessionIDPool.TryGetValue(pSessionID, out sBase) == false)
                            sBase = null;
                        else
                        {
                            //check if this is still logged on as the same user
                            if (sBase.CurrentUserID != (HttpContext.Current.User as SaferTrader.Core.CffPrincipal).CffUser.UserId.ToString())
                            { //note: we may need to override HttpContext.Current.User in the future so we can support multiple logins in one session
                                SessionIDPool.Remove(pSessionID); //in the mean time we remove this old session from pool and generate new viewid 
                                sBase = null;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        sBase = null;
                    }

                    if (SessionIDPool.Count == 0)
                    { //add in sessioninstancepool
                        sBase = null;
                        if (SessionIDPool.TryGetValue(pSessionID, out sBase) == false)
                        {
                            sBase = new SessionWrapperBase(pSessionID);
                            sBase.EmptyWindowHit = 0;
                            sBase.MultiClientSelected = false;
                            sBase.IsStartsWithChecked = true;
                            SessionIDPool.Add(pSessionID, sBase);
                        }
                    }
                } catch (Exception) { }
           
                return sBase;
            }
        }

        public bool IsSessionValid
        {
            get
            {
                try
                {
                    int sCtr = 0;
                    foreach (string dKey in SessionIDPool.Keys)
                        if (dKey.Split('_')[0].ToString() == HttpContext.Current.Session.SessionID)
                            sCtr++;
                    if (sCtr > 0) return true;
                }
                catch (Exception) { }
                return false;
            }
        }

        public void Clear()
        {
            SessionWrapperBase sBase;
            string pSessionID = HttpContext.Current.Session.SessionID + "_" + QueryString.ViewIDValue;
            try
            {
                List<SessionWrapperBase> itemList = new List<SessionWrapperBase>();
                List<string> sKeyList = new List<string>();

                //try remove this key from sessionidpool
                if (SessionIDPool.TryGetValue(pSessionID, out sBase) == true)
                    SessionIDPool.Remove(pSessionID);

                //clear all other keys with the same session id and user id (since this user has logged out)
                foreach (string dKey in SessionIDPool.Keys)
                {
                    if (dKey.Split('_')[0].ToString() == HttpContext.Current.Session.SessionID)
                    {
                        SessionIDPool.TryGetValue(dKey, out sBase);
                        if (sBase.CurrentUserID == (HttpContext.Current.User as Cff.SaferTrader.Core.CffPrincipal).CffUser.UserId.ToString())
                            sKeyList.Add(dKey);
                    }
                }

                foreach (string sKey in sKeyList)
                    SessionIDPool.Remove(sKey);
            }
            catch (Exception exc)
            {
                string msg = exc.Message;
            }

            HttpContext.Current.Session.Clear();
        }


        public void ClearSession(string viewId)
        {
            try
            {
                string pSessionId = HttpContext.Current.Session.SessionID + '_' + viewId;
                SessionWrapperBase sBase;
                if (SessionIDPool.TryGetValue(pSessionId, out sBase) == true)
                    SessionIDPool.Remove(pSessionId);
            }
            catch (Exception)
            { }
        }

        public string CountSessionHit(string pViewID)
        {
            try
            {
                SessionWrapperBase sBase;
                string pSessionID = HttpContext.Current.Session.SessionID + "_" + pViewID;
                if (SessionIDPool.TryGetValue(pSessionID, out sBase))
                    if (sBase.EmptyWindowHit == 0)
                    {
                        sBase.EmptyWindowHit += 1;
                        return pViewID;
                    }
            }
            catch (Exception) { }
            return string.Empty;
        }
    }

}