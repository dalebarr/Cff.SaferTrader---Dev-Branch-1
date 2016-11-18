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
    public partial class UnclaimedRepurchases : ReportBasePage, IUnclaimedRepurchasesView
    {
        protected static string targetName = "";
        private UnclaimedRepurchasesPresenter presenter;

        #region IUnclaimedRepurchasesView Members

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

        public Date LastDate()
        {
            return DatePicker.Date.LastDayOfTheMonth;
        }

        public void Clear()
        {
            this.DateViewedLiteral.Text = string.Empty;
            reportData.Visible = false;
            AllClientsReportHelpMessage.Visible = true;
            ReportPanel.Clear();
        }

        public void ShowAllClientsView()
        {
            Master.ShowReportViewer();
            ReportPanel.ConfigureGridColumns();
            AllClientsFilter.Visible = true;
            UpdateButton.Visible = true;
            DatePicker.EnableAutoPostBack = false;
        }
        public void ShowClientView()
        {
            Master.ShowReportViewer();
            ReportPanel.ConfigureGridColumns();
            DatePicker.Update += DatePickerUpdate;
            DatePicker.EnableAutoPostBack = true;
            UpdateButton.Visible = false;
            AllClientsFilter.Visible = false;
        }

        public void DisplayReport(TransactionReportBase report)
        {
            if (report != null)
                DateViewedLiteral.Text = report.DateViewed.ToDateTimeString();
            else
                DateViewedLiteral.Text = DateTime.Today.ToString();

            reportData.Visible = true ;
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

            presenter = new UnclaimedRepurchasesPresenter(this, TransactionReportsService.Create(),
             ReportManagerFactory.Create(SessionWrapper.Instance.Scope, Context.User as CffPrincipal));

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter.ConfigureView(SessionWrapper.Instance.Scope);
            
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

            // end
            if (!IsPostBack)
            {
                presenter.ShowReport(this.CurrentScope(), true);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewAllButton.Visible = (this.CurrentScope() != Scope.AllClientsScope) && ReportPanel.IsViewAllButtonRequired();
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);
            presenter.ConfigureView(this.CurrentScope());
            presenter.ShowReport(this.CurrentScope(), true);
        }
        protected void UpdateButtonClick(object sender, ImageClickEventArgs e)
        {
            presenter.ConfigureView(this.CurrentScope());
            presenter.ShowReport(this.CurrentScope(), false);
        }

        protected void DatePickerUpdate(object sender, EventArgs e)
        {
            presenter.ConfigureView(this.CurrentScope());
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
    }
}