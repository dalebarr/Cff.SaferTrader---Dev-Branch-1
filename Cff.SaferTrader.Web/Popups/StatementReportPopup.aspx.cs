using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Web.Popups
{
    public partial class StatementReportPopup : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StatementReport report =  SessionWrapper.Instance.Get.PrintBag as StatementReport;

            if (report != null)
            {
                Title = report.Title;
                reportTitleLiteral.Text = report.Title;

                reportPanel.ShowReport(report);
            }
        }
    }
}