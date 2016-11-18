using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Web
{
    public partial class AgreementPage : Page, IAgreementPageView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AcceptanceField.Visible = Request.IsAuthenticated ? true : false;
        }

        protected void OnContinueClick(object sender, EventArgs e)
        {
            MembershipUser member = Membership.GetUser();
            Guid userId = (Guid)member.ProviderUserKey;
            LogOnPresenter presenter = LogOnPresenter.Create(this);
            if (Acceptance.Checked == true) {
                if (presenter.SetAgreement(userId, true) == true) {
                    Redirect("LogOnRedirection.aspx");
                }
            }
            else {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "MessageScript", "alert(\"You need to accept the agreement before you proceed.\");", true);
            }
        }
        void Redirect(string targetUrl) {
            Response.Redirect("LogOnRedirection.aspx");
        }
    }
}
