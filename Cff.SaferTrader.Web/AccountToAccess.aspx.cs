using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core;


namespace Cff.SaferTrader.Web
{
    public partial class AccountToAccess : Page, IAccountToAccess
    {
        private LogOnPresenter presenter;
        public string ParamRefUserQueryString { get { return Request.QueryString[QueryString.User.ToString()]; } }
        public string ParamRefIdQueryString { get { return Request.QueryString[QueryString.LogonParamId.ToString()]; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((ParamRefUserQueryString != "" && ParamRefIdQueryString != "") && (ParamRefUserQueryString != null && ParamRefIdQueryString != null))
            {
                presenter = LogOnPresenter.Create((IAccountToAccess)this, 0);
                Int32 n = presenter.ValidateUserAccess(ParamRefUserQueryString, ParamRefIdQueryString);
                switch (n)
                {
                    case 1: //pass
                        CffLoginAccount account = presenter.GetSpecialAccessAccount(ParamRefUserQueryString, ParamRefIdQueryString);
                         MembershipUser membershipUser = Membership.GetUser(new Guid(ParamRefIdQueryString));
                        if (Membership.ValidateUser(account.Username, account.Password))
                        {
                            FormsAuthentication.SetAuthCookie(account.Username, false);
                            string viewID = Request.QueryString["ViewID"];
                            if (string.IsNullOrEmpty(viewID))
                                    viewID = SaferTrader.Core.Common.StringEnum.GenerateUniqueKey(12);

                            Cff.SaferTrader.Core.Repositories.ICffUserRepository repository = Cff.SaferTrader.Core.Repositories.RepositoryFactory.CreateCffUserRepository();
                            ICffUser loggedOnUser = repository.LoadCffUser(new Guid(ParamRefIdQueryString));

                            SessionWrapper.Instance.GetSession(viewID);        //Generate a new instance of this session
                            SessionWrapper.Instance.GetSession(viewID).IsStartsWithChecked = true;
                            SessionWrapper.Instance.GetSession(viewID).MultiClientSelected = false;
                            SessionWrapper.Instance.GetSession(viewID).IsMultipleAccounts = true;
                            SessionWrapper.Instance.GetSession(viewID).UserIdentity = GetSessionWrapperIdentity(loggedOnUser.UserType.Id);
                            SessionWrapper.Instance.GetSession(viewID).CurrentUserID = loggedOnUser.UserId.ToString();
                            SessionWrapper.Instance.GetSession(viewID).IsDeselectingCustomer = false;
                            SessionWrapper.Instance.GetSession(viewID).ClientFromQueryString = Cff.SaferTrader.Core.Repositories.RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(loggedOnUser.ClientId.ToString()));

                            string returnUrl = Request.QueryString["ReturnUrl"];
                            bool rememberMe = Request.QueryString["RememberMe"] != null && Request.QueryString["RememberMe"].Equals(true.ToString());
                            string url = string.Format("{0}?RememberMe={1}&ViewID={2}&Criteria=0&ClientID={3}&User={4}&ReturnUrl=", 
                                                "LogOnRedirection.aspx", rememberMe, viewID, loggedOnUser.ClientId, loggedOnUser.EmployeeId); //LogOnRedirection.aspx
                            
                            Response.Redirect(url);
                        }
                        else {
                            Response.Redirect("LogOn.aspx");
                        }
                        break;
                    case 0: // blocked
                        break;
                    default: // failed
                        Response.Redirect("LogOn.aspx");
                        break;
                }
            }
            else {
                Response.Redirect("LogOn.aspx");
            }
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
     
    }
}
