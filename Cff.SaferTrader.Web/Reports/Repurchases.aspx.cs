using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters.TransactionReportPresenters;
using Cff.SaferTrader.Core.ReportManager;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.TransactionReportView;

namespace Cff.SaferTrader.Web.Reports
{
    public partial class Repurchases : ReportBasePage, IRepurchasesView
    {
        protected static string filterType = "";
        protected static string targetName = "";
        private RepurchasesPresenter presenter;

        #region IRepurchasesView Members

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

        public Date EndDate()
        {
            return DatePicker.Date;
        }

        public void Clear()
        {
            this.DateViewedLiteral.Text = string.Empty;
            Master.ShowReportViewer();
            reportData.Visible = false;
            AllClientsReportHelpMessage.Visible = true;
            ReportPanel.Clear();
        }

        public void ShowAllClientsView()
        {
            AllClientsFilter.Visible = true;
            UpdateButton.Visible = true;
            DatePicker.EnableAutoPostBack = false;
        }

        public void ShowClientView()
        {
            DatePicker.Update += DatePickerUpdate;
            DatePicker.EnableAutoPostBack = true;

            TransactionStatusTypesFilterControl.Visible = true;
            TransactionStatusTypesFilterControl.Update += TransactionFilterUpdate;
            TransactionStatusTypesFilterControl.EnableAutoPostBack = true;

            UpdateButton.Visible = false;
            AllClientsFilter.Visible = false;
        }

        public void DisplayReport(TransactionReportBase report)
        {
            if (report!=null)
                this.DateViewedLiteral.Text = report.DateViewed.ToDateTimeString();
            Master.ShowReportViewer();
            reportData.Visible = true;
            AllClientsReportHelpMessage.Visible = false;
            ReportPanel.Display((TransactionReport) report);
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new RepurchasesPresenter(this, TransactionReportsService.Create(), ReportManagerFactory.Create(this.CurrentScope(), Context.User as CffPrincipal));
            presenter.ConfigureView(this.CurrentScope());

            ICffClient xClient = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString :
                     (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString : null;
            if (xClient != null) {
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
            
            if (!IsPostBack)
            {
                presenter.ShowReport(this.CurrentScope(), true);
            }

            UpdateTitle((TransactionStatusTypesFilterControl.Controls[1] as System.Web.UI.WebControls.DropDownList).Text);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewAllButton.Visible = (this.CurrentScope() != Scope.AllClientsScope) && ReportPanel.IsViewAllButtonRequired();
        }

        protected void DatePickerUpdate(object sender, EventArgs e)
        {
            presenter.ShowReport(this.CurrentScope(), false);
        }

        protected void TransactionFilterUpdate(object sender, EventArgs e)
        {
            SessionWrapper.Instance.SelectedTransactionFilter = (TransactionStatusTypesFilterControl.Controls[1] as System.Web.UI.WebControls.DropDownList).Text;
            presenter.ShowReport(this.CurrentScope(), false);
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);
            ReportPanel.ConfigureGridColumns();
            presenter.ShowReport(this.CurrentScope(), true);
        }


        protected void UpdateButtonClick(object sender, ImageClickEventArgs e)
        {
            presenter.ShowReport(this.CurrentScope(), false);
        }

        protected void ViewAllButton_Click(object sender, EventArgs e)
        {
            if (ViewAllButton.WasShowingViewPagesImage())
            {
                ReportPanel.ShowPager();
            }
            else
            {
                ReportPanel.ShowAllRecords();
            }
        }

        protected void ExportButton_Click(object sender, ImageClickEventArgs e)
        {
            ReportPanel.Export();
        }

        private void UpdateTitle(String title)
        {
            if (title == null || title == "" || title == "All")
            {
                filterType = "";
                TitleDiv.InnerText = String.Format("Prepayments {0}", targetName);
            }
            else
            {
                filterType = title;
                TitleDiv.InnerText = String.Format("{0} Prepayments {1}", filterType, targetName);
            }
        }
    }
}