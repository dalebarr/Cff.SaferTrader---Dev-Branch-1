using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Views;
using System.Web.Security;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class ChangePassword : ModalBox, ILogOnView
    {
        private LogOnPresenter presenter;
        protected override void OnInit(EventArgs e)
        {
            Label EmailAddress = (Label)ChangePassword1.ChangePasswordTemplateContainer.FindControl("EmailAddressTxt");
            EmailAddress.Text = Membership.GetUser().Email;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Visible = false;
            }
        }
        public void Show()
        {
            Visible = true;
            ChangePassword1.Visible = true;
            BlockScreen();
        }
        protected void SaveButton_OnClick(object sender, EventArgs e)
        {
        }
        protected void CancelButton_OnClick(object sender, EventArgs e)
        {
            Visible = false;
        }
        protected void ContinueButton_OnClick(object sender, EventArgs e)
        {
            Visible = false;
        }
        protected void OnChangedPassword(object sender, EventArgs e)
        {
            //update the employee table.
            presenter = LogOnPresenter.Create(this);
            MembershipUser member = Membership.GetUser(ChangePassword1.UserName);
            Guid userId = (Guid)member.ProviderUserKey;
            presenter.ChangeEmployeePassword(userId, ChangePassword1.NewPassword.Trim());
        }

        public void Redirect(string targetUrl)
        { //do nothing
        }

        public void RedirectToAgreement()
        { //do nothing
        }

    }
}