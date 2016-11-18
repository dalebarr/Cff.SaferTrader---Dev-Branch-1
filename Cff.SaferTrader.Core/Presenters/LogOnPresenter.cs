using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Core.Repositories;
using System.Collections.Specialized;
using System.Collections.Generic;
using System;

namespace Cff.SaferTrader.Core.Presenters
{
    public class LogOnPresenter
    {
        protected readonly ILogOnView view;

        public LogOnPresenter(ILogOnView view)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            this.view = view;
        }

        public static LogOnPresenter Create(ILogOnView view)
        {
            return new LogOnPresenter(view);
        }

        public LogOnPresenter(IAccountToAccess view, Int32 n)
        {
        }

        public LogOnPresenter(IAgreementPageView view)
        {
        }

        public static LogOnPresenter Create(IAccountToAccess view, Int32 n)
        {
            return new LogOnPresenter((IAccountToAccess)view, n);
        }

        public static LogOnPresenter Create(IAgreementPageView view)
        {
            return new LogOnPresenter(view);
        }

        private short GetSessionWrapperIdentity(int userTypeID)
        {
            switch (userTypeID)
            {
                case 1:
                case 4:
                case 5:
                    return (short)SessionWrapper.UserIdentity.Employee;

                case 2:
                case 6:
                    return (short)SessionWrapper.UserIdentity.Client;


                case 3:
                    return (short)SessionWrapper.UserIdentity.Customer;
            }

            return 0;
        }

        private string cleanUpClientIDAndCustomerID(string returnUrl) {
            int idx1 = returnUrl.IndexOf("ClientID");
            string url1, url2;

            if (idx1 >= 0)
            { //remove ClientID from URL
                url1 = returnUrl.Substring(0, idx1 - 1);
                url2 = returnUrl.Substring(returnUrl.IndexOf("ViewID"));
                idx1 = url2.IndexOf("&");
                if (idx1 >= 0)
                {
                    url2 = url2.Substring(idx1);
                    returnUrl = url1 + url2;
                }
                else
                    returnUrl = url1;
            }

            idx1 = returnUrl.IndexOf("CustomerID");
            if (idx1 >= 0)
            { //remove ClientID from URL
                url1 = returnUrl.Substring(0, idx1 - 1);
                url2 = returnUrl.Substring(returnUrl.IndexOf("ViewID"));
                idx1 = url2.IndexOf("&");
                if (idx1 >= 0)
                {
                    url2 = url2.Substring(idx1);
                    returnUrl = url1 + url2;
                }
                else
                    returnUrl = url1;
            }
            
            return returnUrl;
        }

        public void LogOnUser(Guid userId, string returnUrl, bool rememberMe)
        {
            bool bAddViewID = false;
            string url = "LogOnRedirection.aspx";

            ICffUserRepository repository = RepositoryFactory.CreateCffUserRepository();
            ICffUser loggedOnUser = repository.LoadCffUser(userId);
            short currentUserIdentity = GetSessionWrapperIdentity(loggedOnUser.UserType.Id);

            string viewID = Common.StringEnum.GenerateUniqueKey(12);
            SessionWrapperBase sBase = new SessionWrapperBase();

            if (SessionWrapper.Instance.Get != null && (loggedOnUser.UserType == UserType.ClientManagementUser
                                                             || loggedOnUser.UserType == UserType.ClientStaffUser
                                                             || loggedOnUser.UserType == UserType.CustomerUser))
            {
                if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
                {
                    if (SessionWrapper.Instance.Get.ClientFromQueryString.Id <= 0)
                    {
                        SessionWrapper.Instance.ClearSession(SessionWrapper.Instance.Get.SessionID);
                    }
                }
            }

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["ViewID"]))
            { //clear any remaining session keys here and generate new viewID
                    if (SessionWrapper.Instance.Get != null)
                    {
                        try
                        {
                            sBase = SessionWrapper.Instance.Get.Clone(); 
                        }
                        catch { }
                        SessionWrapper.Instance.Clear();
                        SessionWrapper.Instance.GetSession(viewID);
                        if ((currentUserIdentity == sBase.UserIdentity) && (sBase.CurrentUserID == userId.ToString()))
                        { //get last data
                            SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString = sBase.ClientFromQueryString;
                            SessionWrapper.Instance.GetSession(viewID).CustomerFromQueryString = sBase.CustomerFromQueryString;
                            SessionWrapper.Instance.GetSession(viewID).IsMultipleAccounts = sBase.IsMultipleAccounts;
                            SessionWrapper.Instance.GetSession(viewID).IsStartsWithChecked = sBase.IsStartsWithChecked;
                        }

                        if ((SessionWrapper.Instance.GetSession(viewID).UserIdentity == (short)SessionWrapper.UserIdentity.Client)
                                && (sBase.ClientFromQueryString.Id == loggedOnUser.ClientId))
                            SessionWrapper.Instance.GetSession(viewID).AccountsIDList = sBase.AccountsIDList;

                        SessionWrapper.Instance.GetSession(viewID).MultiClientSelected = false;
                        SessionWrapper.Instance.GetSession(viewID).EmptyWindowHit = 0;
                        SessionWrapper.Instance.GetSession(viewID).AccountsIDList = "";
                        SessionWrapper.Instance.GetSession(viewID).UserIdentity = currentUserIdentity;
                        SessionWrapper.Instance.GetSession(viewID).CurrentUserID = userId.ToString();
                    }
             }
             else
                bAddViewID = true;


             if (!string.IsNullOrEmpty(returnUrl)) 
             {
                    if (SessionWrapper.Instance.Get == null)
                    {
                        SessionWrapper.Instance.GetSession(viewID);
                        SessionWrapper.Instance.GetSession(viewID).IsMultipleAccounts = false;
                        SessionWrapper.Instance.GetSession(viewID).IsStartsWithChecked = true;
                        SessionWrapper.Instance.GetSession(viewID).MultiClientSelected = false;
                        SessionWrapper.Instance.GetSession(viewID).EmptyWindowHit = 0;
                        SessionWrapper.Instance.GetSession(viewID).AccountsIDList = "";
                        SessionWrapper.Instance.GetSession(viewID).UserIdentity = currentUserIdentity;
                        SessionWrapper.Instance.GetSession(viewID).CurrentUserID = userId.ToString();
                        if (loggedOnUser.ClientId == -1 && (returnUrl.Contains("Client")))
                        {  //retain to whatever scope was previously stored in the returnurl
                            string strRawURL = returnUrl;
                            int cidx = strRawURL.IndexOf("Client=") + 7;
                            int eidx = strRawURL.IndexOf('&', cidx);
                            if (eidx > 0) {
                                strRawURL = strRawURL.Substring(cidx, (eidx - cidx));
                                int clientID = string.IsNullOrEmpty(strRawURL) ? 0 : Convert.ToInt32(strRawURL);
                                if (clientID > 0)
                                    SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(clientID);
                                else
                                    SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString = RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(loggedOnUser.ClientId.ToString()));
                            }
                        }
                        else
                            SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString =
                                RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(loggedOnUser.ClientId.ToString()));

                        returnUrl = cleanUpClientIDAndCustomerID(returnUrl);
                        returnUrl += "&ClientID=" + loggedOnUser.ClientId.ToString();
                        returnUrl += "&CustomerID=0";
                    }

                    string url1 = returnUrl;
                    if (bAddViewID)
                    {
                        url = string.Format("{0}?ReturnUrl={1}&RememberMe={2}&ViewID={3}", url, returnUrl, rememberMe, viewID);
                        if (url1.IndexOf("Criteria") < 0)
                            url += "&Criteria=0";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            int idx1 = returnUrl.IndexOf("ViewID");
                            if (idx1 >= 0)
                            { //remove ViewId from URL
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
                        }
                        url = string.Format("{0}?ReturnUrl={1}&RememberMe={2}&ViewID={3}", url, returnUrl, rememberMe, viewID);
                    }
                }
                else
                {
                    if (SessionWrapper.Instance.Get == null)
                    {
                        SessionWrapper.Instance.GetSession(viewID);
                        SessionWrapper.Instance.GetSession(viewID).IsMultipleAccounts = false;
                        SessionWrapper.Instance.GetSession(viewID).IsStartsWithChecked = true;
                        SessionWrapper.Instance.GetSession(viewID).MultiClientSelected = false;
                        SessionWrapper.Instance.GetSession(viewID).EmptyWindowHit = 0;
                        SessionWrapper.Instance.GetSession(viewID).AccountsIDList = "";
                        SessionWrapper.Instance.GetSession(viewID).UserIdentity = currentUserIdentity;
                        SessionWrapper.Instance.GetSession(viewID).CurrentUserID = userId.ToString();
                        SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString =
                                RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(loggedOnUser.ClientId.ToString()));

                    }
                    url = string.Format("{0}?RememberMe={1}&ViewID={2}&Criteria=0", url, rememberMe, viewID);
                }

             if (repository.AcceptAgreement(userId, null) == false)
             {
                 view.RedirectToAgreement();
             }
             view.Redirect(url);
        }

        public bool SetAgreement(Guid UserId, Boolean? isAgreed)
        {
            ICffUserRepository repository = RepositoryFactory.CreateCffUserRepository();
            return repository.AcceptAgreement(UserId, isAgreed);
        }

        public void CreateNewUser(NameValueCollection dtaCollection, string UserKey)
        {
            // save new user to db
            ICffUserRepository repository = RepositoryFactory.CreateCffUserRepository();
            Guid uidUserKey = new Guid(UserKey);
            repository.AddNewCffUser(dtaCollection, uidUserKey);
        }

        public Boolean VerifyPasskey(string UserPassKey)
        {
            // save new user to db
            ICffUserRepository repository = RepositoryFactory.CreateCffUserRepository();
            return repository.VerifyPasskey(UserPassKey);
        }

        public Int32 VerifyIfSpecialAccount(string username, string password)
        {
            ICffUserRepository repository = RepositoryFactory.CreateCffUserRepository();
            return repository.VerifyIfSpecialAccount(username, password);
        }

        public List<UserSpecialAccounts> GetAccountAccess(string username, string password)
        {
            ICffUserRepository repository = RepositoryFactory.CreateCffUserRepository();
            return repository.GetSpecialAccountAccess(username, password);
        }

        public String GetRoleByPassKey(string UserPassKey)
        {
            ICffUserRepository repository = RepositoryFactory.CreateCffUserRepository();
            return repository.GetRoleByPassKey(UserPassKey);
        }

        public CffUserActivation ActivateUser(String uKey, String pKey)
        {
            ICffUserRepository repository = RepositoryFactory.CreateCffUserRepository();
            return repository.ActivateUser(new Guid(uKey), pKey);
        }

        public CffUserActivation UserRequestApproval(String mKey, String uKey, String action)
        {
            ICffUserRepository repository = RepositoryFactory.CreateCffUserRepository();
            if (action == "1")
            {
                // return the CffUserActivation
                return repository.ApproveUser(new Guid(mKey), new Guid(uKey));
            }
            else
            {
                // return the CffUserActivation
                return repository.DeclineUser(new Guid(mKey), new Guid(uKey));
            }
        }

        public Int32 ValidateUserAccess(String user, String accessId)
        {
            ICffUserRepository repository = RepositoryFactory.CreateCffUserRepository();
            return repository.ValidateSpecialAccess(user, new Guid(accessId));
        }

        public CffLoginAccount GetSpecialAccessAccount(String user, String accessId)
        {
            ICffUserRepository repository = RepositoryFactory.CreateCffUserRepository();
            return repository.GetSpecialAccessAccount(user, new Guid(accessId));
        }

        public bool ChangeEmployeePassword(Guid uid, String newPasswrd)
        {
            ICffUserRepository repository = RepositoryFactory.CreateCffUserRepository();
            return repository.ChangeEmployeePassword(uid, newPasswrd);
        }
    }
}
