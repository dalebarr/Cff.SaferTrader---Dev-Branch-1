using System;
using System.Web.UI;
using Cff.SaferTrader.Core.Views.ReportView;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.Reports
{
    public class ReportBasePage : Page, IReportView
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            ((ReportsMaster)Master).Master.ScopeChanged += ScopeChanged;
        }

        protected virtual void ScopeChanged(object sender, EventArgs e)
        {
        }

        public Scope CurrentScope()
        {
            if (SessionWrapper.Instance.Get != null)
                return SessionWrapper.Instance.Get.Scope;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).Scope;
            else
            {
                if (QueryString.CustomerId != null)
                    return Scope.CustomerScope;
                else if (QueryString.ClientId != null)
                    return Scope.ClientScope;
                return Scope.AllClientsScope;
            }
        }


        public void DisplayAccessDeniedError()
        {
            ((ReportsMaster)Master).HideReportViewer("You are not authorised to view the report.");
        }

        public void DisplayReportNotAvailableError()
        {
            ((ReportsMaster)Master).HideReportViewer("Report is not available in this scope.");
        }
    }
}