using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Reports;
using System.Web.UI.HtmlControls;

namespace Cff.SaferTrader.Web.UserControls.ReportPanels
{
    /// <summary>
    /// Container for TransactionReportPanel and CustomerTransactionPanel
    /// </summary>
    public partial class TransactionReportPanelContainer : UserControl
    {
        protected ExportableGridPanel activeReportPanel;
        public event EventHandler<EventArgs> ReportParameterUpdated;

        private void AddJavaScriptIncludeInHeader(string path)
        {
            try
            {
                var script = new HtmlGenericControl("script");
                script.Attributes["type"] = "text/javascript";
                script.Attributes["src"] = ResolveUrl(path);
                Page.Header.Controls.Add(script);
            }
            catch { }
        }

        public void DisplayTransactionsReport(TransactionReportBase transactionReport)
        {
            if (transactionReport!=null)
                this.DateViewedLiteral.Text = transactionReport.DateViewed.ToDateTimeString();
            reportPanel.ResetPaginationAndFocus();
            reportData.Visible = true;
            AllClientsReportHelpMessage.Visible = false;
            reportPanel.Display((TransactionReport) transactionReport);
        }

        public void DisplayTransactionsReportForCustomer(ReportBase transactionReport)
        {
            this.DateViewedLiteral.Text = transactionReport.DateViewed.ToDateTimeString();
            customerReportPanel.ConfigureGridColumns();
            customerReportPanel.ResetPaginationAndFocus();
            reportData.Visible = true;
            AllClientsReportHelpMessage.Visible = false;
            customerReportPanel.Display((CustomerTransactionReport) transactionReport);
        }

        public void ShowAllClientsView()
        {
            reportPanel.Visible = true;
            activeReportPanel = reportPanel;

            AllClientsFilter.Visible = true;
            DatePicker.Visible = true;
            UpdateButton.Visible = true;

            DatePicker.EnableAutoPostBack = false;
            DateRangeDiv.Visible = false;
            customerReportPanel.Visible = false;
        }

        public void ShowClientView()
        {
            reportPanel.Visible = true;
            activeReportPanel = reportPanel;

            DatePicker.Visible = true;
            TransactionStatusTypesFilterControl.Visible = true;

            AllClientsFilter.Visible = false;
            DateRangeDiv.Visible = false;
            customerReportPanel.Visible = false;
            UpdateButtonClient.Visible = true;
        }

        public void ShowCustomerView()
        {
            customerReportPanel.Visible = true;
            activeReportPanel = customerReportPanel;

            DateRangeDiv.Visible = true;
            UpdateButton.Visible = true;
            FromDateTextBoxTR.Visible = true;
            ToDateTextBoxTR.Visible = true;

            TransactionStatusTypesFilterControl.Visible = false;
            TransactionStatusTypesFilterControl.EnableAutoPostBack = false;
            DatePicker.EnableAutoPostBack = false;
            DatePicker.Visible = false;
            AllClientsFilter.Visible = false;
            reportPanel.Visible = false;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            foreach (AsyncPostBackTrigger trigger in GridUpdatePanel.Triggers)
            {
                if (DatePicker.UniqueID.EndsWith(trigger.ControlID, StringComparison.OrdinalIgnoreCase))
                {
                    trigger.ControlID = DatePicker.UniqueID;
                    break;
                }
            }

            DateTime dtStart = DateTime.Now;
            dtStart = dtStart.AddDays(-31); // less than 30 days from the Date.Now (dbb)
            //string dtStamp = DateTime.Now.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + (DateTime.Now.Year).ToString(); //dbb
            string dtStamp = dtStart.Day.ToString().PadLeft(2, '0') + "/" + dtStart.Month.ToString().PadLeft(2, '0') + "/" + dtStart.Year.ToString();
            string dtStampNow = DateTime.Now.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year.ToString();

            FromDateTextBoxTR.Text = dtStamp;
            ToDateTextBoxTR.Text = dtStampNow;
            
        }

  
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            AddJavaScriptIncludeInHeader("js/ui.1.10.4/jquery-1.10.2.js");
            AddJavaScriptIncludeInHeader("js/jquery-migrate-1.0.0.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery-ui.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.core.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.button.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.widget.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.menu.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.slider.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.dialog.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.accordion.js");
            AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.datepicker.js");
        }

        protected void DatePickerUpdate(object sender, EventArgs e)
        {
            if (ReportParameterUpdated != null)
            {
                ReportParameterUpdated(sender, EventArgs.Empty);
            }
        }

        protected void TransactionFilterUpdate(object sender, EventArgs e)
        {
            SessionWrapper.Instance.SelectedTransactionFilter = (TransactionStatusTypesFilterControl.Controls[1] as System.Web.UI.WebControls.DropDownList).Text;
            if (ReportParameterUpdated != null)
            {
                ReportParameterUpdated(sender, EventArgs.Empty);
            }
        }

        protected void UpdateButtonClick(object sender, ImageClickEventArgs e)
        {
            if (ReportParameterUpdated != null)
            {
                ReportParameterUpdated(sender, EventArgs.Empty);
            }
        }

        protected void UpdateButtonClientClick(object sender, ImageClickEventArgs e)
        {
            if (ReportParameterUpdated != null)
            {
                ReportParameterUpdated(sender, EventArgs.Empty);
            }
        }

        protected void ExportButton_Click(object sender, ImageClickEventArgs e)
        {
            activeReportPanel.Export();
        }

        public void ResetPaginationAndFocus()
        {
            activeReportPanel.ResetPaginationAndFocus();
        }

        public void Clear()
        {
            reportData.Visible = false;
            AllClientsReportHelpMessage.Visible = true;
            reportPanel.Clear();
        }

        public void HideExportButton()
        {
            ExportButton.Visible = false;
        }

        public void ShowExportButton()
        {
            ExportButton.Visible = true;
        }

        public DateRange SelectedDateRange
        {
           
            get {
                DateRange xRange = new DateRange(new Date(string.IsNullOrEmpty(FromDateTextBoxTR.Text) ? DateTime.Now : Convert.ToDateTime(FromDateTextBoxTR.Text)),
                        new Date(string.IsNullOrEmpty(ToDateTextBoxTR.Text) ? DateTime.Now : Convert.ToDateTime(ToDateTextBoxTR.Text)));
                return xRange;
            }
        }

        public Date Date
        {
            get { return DatePicker.Date; }
        }

        public FacilityType FacilityType
        {
            get { return AllClientsFilter.FacilityType; }
        }

        public bool IsSalvageIncluded
        {
            get { return AllClientsFilter.IsSalvageIncluded; }
        }
    }
}