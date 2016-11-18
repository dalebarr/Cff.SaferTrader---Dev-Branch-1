using System;
using System.Web.Security;
using System.Web.UI;

namespace Cff.SaferTrader.Web
{
    public partial class EndSession : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
            Session.Abandon();
            Response.Redirect("~/LogOn.aspx");
        }
    }
}
