using System;
using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class CallDueReportFilter : System.Web.UI.UserControl
    {
        public PeriodReportType ReportType
        {
            get { return PeriodReportType.Instantiate(int.Parse(ReportTypeDropDownList.SelectedValue)); }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ConfigureDropDowns();
        }

        private void ConfigureDropDowns()
        {
            ReportTypeDropDownList.DataSource = PeriodReportType.KnownTypes;
            ReportTypeDropDownList.DataTextField = "Text";
            ReportTypeDropDownList.DataValueField = "Id";
            ReportTypeDropDownList.DataBind();
        }
    }
}