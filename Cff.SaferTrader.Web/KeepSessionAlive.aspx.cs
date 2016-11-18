using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web
{
    public partial class KeepSessionAlive : System.Web.UI.Page
    {
        protected String WindowStatusText;
        protected System.Web.UI.HtmlControls.HtmlControl metaRefresh;

        protected void Page_Load(object sender, EventArgs e)
        {
           if (User.Identity.IsAuthenticated)
            {
                CffPrincipal cffPrincipal = Context.User as CffPrincipal;
                if (cffPrincipal != null)
                {  
                    String logonUrl = ResolveUrl("~/KeepSessionAlive.aspx");
                    if (cffPrincipal.IsInAdministratorRole) {
                        metaRefresh.Attributes["http-equiv"] = "refresh";
                        //Refresh this page 60 seconds before session timeout, to reset the session timeout counter. //";url=KeepSessionAlive.aspx?q=" 
                        metaRefresh.Attributes["content"] = Convert.ToString((Session.Timeout * 60) - 60) + ";url=" + logonUrl + "?q=" + DateTime.Now.Ticks;
                    }
                    else if (cffPrincipal.IsInManagementRole)
                    {
                        metaRefresh.Attributes["http-equiv"] = "refresh";
                        metaRefresh.Attributes["content"] = Convert.ToString((Session.Timeout * 60) - 5) + ";url=" + logonUrl + "?q=" + DateTime.Now.Ticks;
                    }
                    else if (cffPrincipal.IsInClientRole)
                    {
                        metaRefresh.Attributes["http-equiv"] = "refresh";
                        metaRefresh.Attributes["content"] = Convert.ToString((Session.Timeout * 60) + 1) + ";url=" + logonUrl + "?q=" + DateTime.Now.Ticks;
                    }
                }
                WindowStatusText = "Last refresh " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            }
        }
    }
}
