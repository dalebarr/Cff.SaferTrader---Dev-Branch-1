using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters.TransactionReportPresenters;
using Cff.SaferTrader.Core.ReportManager;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.TransactionReportView;
using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.Reports
{
    public partial class Overpayments : ReportBasePage, IOverpaymentsView
    {
        protected static string filterType = "";
        protected static string targetName = "";
        private ExportableGridPanel activeReportPanel;
        private OverpaymentsPresenter presenter;

        #region IOverpaymentsView Members

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
            //Master.ShowReportViewer();
            reportData.Visible = false;
            AllClientsReportHelpMessage.Visible = true;
            //ReportPanel.ResetPaginationAndFocus();
            ReportPanel.Clear();
        }

        public void ShowAllClientsView()
        {
            Master.ShowReportViewer();
            ReportPanel.Visible = true;
            activeReportPanel = ReportPanel;

            AllClientsFilter.Visible = true;
            DatePicker.Visible = true;
            DatePicker.EnableAutoPostBack = false;
            DatePicker.Update -= ReportFilterUpdate;

            UpdateButton.Visible = true;
            UpdateButton.Click += ReportFilterUpdate;

            TransactionStatusTypesFilterControl.Visible = true;
            TransactionStatusTypesFilterControl.Update += TransactionFilterUpdate;
            TransactionStatusTypesFilterControl.EnableAutoPostBack = true;

            // Switch off customer controls
            customerReportPanel.Visible = false;
            DateRangeDiv.Visible = false;
        }

        public void ShowClientView()
        {
            Master.ShowReportViewer();
            ReportPanel.Visible = true;
            activeReportPanel = ReportPanel;

            DatePicker.Visible = true;
            DatePicker.Update += ReportFilterUpdate;
            DatePicker.EnableAutoPostBack = true;

            AllClientsFilter.Visible = false;
            UpdateButton.Visible = false;

            // Switch off customer controls
            customerReportPanel.Visible = false;
            DateRangeDiv.Visible = false;
        }

        public void DisplayReport(TransactionReportBase report)
        {
            if (report!=null)
                this.DateViewedLiteral.Text = report.DateViewed.ToDateTimeString();
            Master.ShowReportViewer();
            reportData.Visible = true;
            AllClientsReportHelpMessage.Visible = false;

            ReportPanel.ConfigureGridColumns();
            ReportPanel.ResetPaginationAndFocus();
            ReportPanel.Display((TransactionReport) report);
        }

        public void DisplayReportForCustomer(ReportBase report)
        {
            this.DateViewedLiteral.Text = report.DateViewed.ToDateTimeString();
            Master.ShowReportViewer();
            reportData.Visible = true;
            ReportPanel.Visible = false;
            customerReportPanel.Visible = true;
            AllClientsReportHelpMessage.Visible = false;
            customerReportPanel.ConfigureGridColumns();
            customerReportPanel.ResetPaginationAndFocus();
            customerReportPanel.Display((CustomerOverpaymentsTransactionReport)report);
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
            ReportPanel.Visible = false;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new OverpaymentsPresenter(this, TransactionReportsService.Create(), ReportManagerFactory.Create(this.CurrentScope(), Context.User as CffPrincipal));
            presenter.ConfigureView(this.CurrentScope());

            ICffClient xClient = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString :
                        (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString : null;
            if (xClient != null) {
                targetName = ": " + xClient.Name;
            }

            ICffCustomer xCustomer = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.CustomerFromQueryString :
                        (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString : null;
            if (xCustomer != null) {
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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewAllButton.Visible = (this.CurrentScope() != Scope.AllClientsScope) && ReportPanel.IsViewAllButtonRequired();
        }

        protected void ViewAllButton_Click(object sender, EventArgs e)
        {
            if (ViewAllButton.WasShowingViewPagesImage())
            {
                activeReportPanel.ShowPager();
            }
            else
            {
                activeReportPanel.ShowAllRecords();
            }
        }

        protected void ExportButton_Click(object sender, ImageClickEventArgs e)
        {
            ReportPanel.Export();
        }

        public void ShowTransactionsReport(TransactionReportBase transactionReport)
        {
            Master.ShowReportViewer();
            ReportPanel.Display((TransactionReport) transactionReport);
            UpdateTitle((TransactionStatusTypesFilterControl.Controls[1] as System.Web.UI.WebControls.DropDownList).Text);
        }

        private void UpdateTitle(String title)
        {
            if (title == null || title == "" || title == "All")
            {
                filterType = "";
                TitleDiv.InnerText = String.Format("Overpayments {0}", targetName);
            }
            else
            {
                filterType = title;
                TitleDiv.InnerText = String.Format("{0} Overpayments {1}", filterType, targetName);
            }
        }
    }
}