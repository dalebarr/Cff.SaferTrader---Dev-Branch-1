using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Cff.SaferTrader.Web
{
    public partial class ReportsMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (((SaferTrader.Core.CffPrincipal)Context.User).IsInClientRole && SaferTrader.Core.SessionWrapper.Instance.Get.ClientFromQueryString.Id == -1)
            { //TODO: resolve this - temporary solution for client error:: let it throw an error
                SaferTrader.Core.SessionWrapper.Instance.Get.ClientFromQueryString = 
                        SaferTrader.Core.Repositories.RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(0));
            }

            if (!this.IsPostBack)
            {
                try
                {
                    Master.HideRightSidePanel();
                    ScriptManager sr = Master.FindControl("ScriptManager1") as ScriptManager;
                    if (sr != null) {
                        sr.AjaxFrameworkMode = AjaxFrameworkMode.Explicit;
                        ScriptManager.RegisterStartupScript(GridUpdatePanel, typeof(UpdatePanel), "selectReportTab", @"selectParentTab($('.sf-menu li a.reportsTab'));", true);
                    }
                }
                catch (Exception exc)
                {
                    string error = exc.Message;
                }
            }
        }

        public void ShowReportViewer()
        {
            NoReport.Visible = false;
            if (!this.IsPostBack) {
                this.ReportViewerContentPlaceholder.Visible = true;
            }
        }

        public void HideReportViewer(string warningMessage)
        {
            NoReport.Visible = true;
            NoReportLiteral.Text = warningMessage;
            if (!Page.IsPostBack) {
                this.ReportViewerContentPlaceholder.Visible = false;
            }
        }

    }
}