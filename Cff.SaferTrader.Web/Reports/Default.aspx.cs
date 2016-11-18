using System;
using Cff.SaferTrader.Core;
using System.Web.UI.HtmlControls;

namespace Cff.SaferTrader.Web.Reports
{
    public partial class Default : System.Web.UI.Page
    {
        protected static string targetName = "";

        private void InitializeCurrentPathForJavaScript()
        {
            try
            {
                string relativePathToRoot = RelativePathComputer.ComputeRelativePathToRoot(Server.MapPath("~"),
                                                                                           Server.MapPath("."));
                string script = string.Format(@"var relativePathToRoot='{0}';", relativePathToRoot);
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "initializeCurrentPath", script, true);
            }
            catch { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeCurrentPathForJavaScript();

            ICffClient xClient = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString :
                (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString : null;
            if (xClient != null)
            {
                targetName = ": " + xClient.Name;
            }

            ICffCustomer xCustomer = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.CustomerFromQueryString :
                      (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString : null;
            if (xCustomer != null)
            {
                if (targetName != null || !targetName.Equals(""))
                {
                    targetName += " / ";
                    targetName = string.Concat(targetName, xCustomer.Name);
                }
                else
                {
                    targetName = ": " + xCustomer.Name;
                }
            }

            Master.ShowReportViewer();
        }
    }
}
