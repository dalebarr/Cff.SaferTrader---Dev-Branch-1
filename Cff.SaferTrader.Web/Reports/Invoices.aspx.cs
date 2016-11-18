using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters.TransactionReportPresenters;
using Cff.SaferTrader.Core.ReportManager;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.TransactionReportView;
using Cff.SaferTrader.Web.UserControls;

namespace Cff.SaferTrader.Web.Reports
{
    public partial class Invoices : ReportBasePage, IInvoicesView
    {
        protected static string targetName = "";
        protected string filterType = "";
        private ExportableGridPanel activeReportPanel;
        private InvoicesPresenter presenter;
       
        #region IInvoicesView Members


        public int ClientId()
        {
            if (SessionWrapper.Instance.Get != null)
                return SessionWrapper.Instance.Get.ClientFromQueryString.Id;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id;

            return (QueryString.ClientId == null) ? 0 : (int)QueryString.ClientId;
        }

        public int ClientFacilityType()
        {
            if (SessionWrapper.Instance.Get != null)
                return SessionWrapper.Instance.Get.ClientFromQueryString.ClientFacilityType;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.ClientFacilityType;

            return 0;
        }


        public bool IsSalvageIncluded()
        {
            return AllClientsFilter.IsSalvageIncluded;
        }

        public FacilityType FacilityType()
        {
            return AllClientsFilter.FacilityType;
        }

        public DateRange DateRange()
        { //return calendarDateRangePicker.SelectedDateRange;
            DateRange xRange = new DateRange(new Date(string.IsNullOrEmpty(FromDateTextBox.Text) ? DateTime.Now : Convert.ToDateTime(FromDateTextBox.Text)),
                        new Date(string.IsNullOrEmpty(ToDateTextBox.Text) ? DateTime.Now : Convert.ToDateTime(ToDateTextBox.Text)));
            return xRange;
        }

        public Date EndDate()
        {
            return DatePicker.Date;
        }

        public int CustomerId()
        {
            //return SessionWrapper.Instance.Customer.Id;
            return SessionWrapper.Instance.Get.CustomerFromQueryString.Id;
        }

        public void Clear()
        {
            reportData.Visible = false;
            AllClientsReportHelpMessage.Visible = true;
            reportPanel.Clear();
        }

        public void ShowAllClientsView()
        {
            Master.ShowReportViewer();
            reportPanel.Visible = true;
            activeReportPanel = reportPanel;

            TransactionStatusTypesFilterControl.Visible = true;
            TransactionStatusTypesFilterControl.Update += TransactionFilterUpdate;
            TransactionStatusTypesFilterControl.EnableAutoPostBack = true;

            AllClientsFilter.Visible = true;
            DatePicker.Visible = true;
            DatePicker.EnableAutoPostBack = false;
            DatePicker.Update -= ReportFilterUpdate;

            UpdateButton.Visible = true;
            UpdateButton.Click += ReportFilterUpdate;

            //Switch off customer report panel controls
            customerReportPanel.Visible = false;
            DateRangeDiv.Visible = false;
        }

        public void ShowClientView()
        {
            Master.ShowReportViewer();
            reportPanel.Visible = true;
            activeReportPanel = reportPanel;

            DatePicker.Visible = true;
            DatePicker.Update += ReportFilterUpdate;
            DatePicker.EnableAutoPostBack = true;

            TransactionStatusTypesFilterControl.Visible = true;
            TransactionStatusTypesFilterControl.Update += TransactionFilterUpdate;
            TransactionStatusTypesFilterControl.EnableAutoPostBack = true;

            //Switch off all clients controls
            AllClientsFilter.Visible = false;
            UpdateButton.Visible = false;
            
            //Switch off customer controls
            customerReportPanel.Visible = false;
            DateRangeDiv.Visible = false;
        }

        public void DisplayReport(TransactionReportBase report)
        {
            if (report != null) 
            {
                this.DateViewedLiteral.Text = report.DateViewed.ToDateTimeString();
                Master.ShowReportViewer();
                reportData.Visible = true;
                AllClientsReportHelpMessage.Visible = false;

                reportPanel.ConfigureGridColumns();
                reportPanel.ResetPaginationAndFocus();
                reportPanel.Display((InvoicesTransactionReport)report);
            }
        }

        public void DisplayReportForCustomer(ReportBase report)
        {
            this.DateViewedLiteral.Text = report.DateViewed.ToDateTimeString();
            Master.ShowReportViewer();
            reportData.Visible = true;
            AllClientsReportHelpMessage.Visible = false;
            customerReportPanel.ConfigureGridColumns();
            customerReportPanel.ResetPaginationAndFocus();
            customerReportPanel.Display((CustomerInvoicesTransactionReport) report);
        }

        public void ShowCustomerView()
        {
            Master.ShowReportViewer();
            customerReportPanel.Visible = true;
            activeReportPanel = customerReportPanel;

            
            DateRangeDiv.Visible = true;
            UpdateButton.Visible = true;
            UpdateButton.Click += ReportFilterUpdate;

            // Switch off all clients/client controls
            TransactionStatusTypesFilterControl.Visible = false;
            TransactionStatusTypesFilterControl.EnableAutoPostBack = false;

            AllClientsFilter.Visible = false;
            DatePicker.Visible = false;
            reportPanel.Visible = false;
        }

        #endregion

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
            dtStart = dtStart.AddDays(-61); // less than 30 days from the Date.Now (dbb)
            string dtStamp = dtStart.Day.ToString().PadLeft(2, '0') + "/" + dtStart.Month.ToString().PadLeft(2, '0') + "/" + dtStart.Year.ToString();
            string dtStampNow = DateTime.Now.Day.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year.ToString();
            FromDateTextBox.Text = dtStamp;
            ToDateTextBox.Text = dtStampNow;
        }

        protected void ReportFilterUpdate(object sender, EventArgs e)
        {
            presenter.ShowReport(this.CurrentScope(), false);
        }


        protected void TransactionFilterUpdate(object sender, EventArgs e)
        {
            if (SessionWrapper.Instance.Get!=null)
                SessionWrapper.Instance.Get.SelectedTransactionFilter = (TransactionStatusTypesFilterControl.Controls[1] as System.Web.UI.WebControls.DropDownList).Text;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).SelectedTransactionFilter = (TransactionStatusTypesFilterControl.Controls[1] as System.Web.UI.WebControls.DropDownList).Text;
            
            presenter.ShowReport(this.CurrentScope(), false);
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);
            presenter.ShowReport(this.CurrentScope(), true);
        }

        protected void UpdateButtonClick(object sender, ImageClickEventArgs e)
        {
            presenter.ShowReport(this.CurrentScope(), false);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new InvoicesPresenter(this, TransactionReportsService.Create(), ReportManagerFactory.Create(this.CurrentScope(), Context.User as CffPrincipal));
            presenter.ConfigureView(this.CurrentScope());
          
            ICffClient xClient = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString :
                                    (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString : null;
            if (xClient  != null)
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

            presenter.ShowReport(this.CurrentScope(), true);
            UpdateTitle((TransactionStatusTypesFilterControl.Controls[1] as System.Web.UI.WebControls.DropDownList).Text);
        }


        protected void ExportButton_Click(object sender, ImageClickEventArgs e)
        {
            activeReportPanel.Export();
        }

        private void UpdateTitle(String title)
        {
            if (title == null || title == "" || title == "All")
                filterType = "";
            else
                filterType = title;
            TitleDiv.InnerText = String.Format("{0} Invoices {1}", filterType, targetName);
        }
    }
}