using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters.ReportPresenters;
using Cff.SaferTrader.Core.ReportManager;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Web.Reports
{
    public partial class OverdueCharges : ReportBasePage, IOverdueChargesView
    {
        protected static string targetName = "";
        private OverdueChargesPresenter presenter;

        #region IOverdueChargesView Members

        public Date EndDate()
        {
            return DatePicker.Date;
        }

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
                
        public void DisplayReport(OverdueChargesReport report)
        {
            Master.ShowReportViewer();
            reportData.Visible = true;
            AllClientsReportHelpMessage.Visible = false;
            DateViewedLiteral.Text = (report==null)?"":report.DateViewed.ToDateTimeString();
            ReportPanel.Display(report);
        }
        public void Clear()
        {
            Master.ShowReportViewer();
            reportData.Visible = false;
            AllClientsReportHelpMessage.Visible = true;
            ReportPanel.Clear();
        }
        public bool IsSalvageIncluded()
        {
            return allClientsFilter.IsSalvageIncluded;
        }
        public TransactionStatus TransactionStatus()
        {
            return StatusPicker.Status;
        }
        public FacilityType FacilityType()
        {
            return allClientsFilter.FacilityType;
        }

        public void ShowAllClientsView()
        {
            ReportPanel.ConfigureAllClientsGridColumns();
            ReportPanel.ResetPaginationAndFocus();
        }

        public void ShowClientView()
        {
            ReportPanel.ConfigureClientGridColumns();
            ReportPanel.ResetPaginationAndFocus();
        }

        public void ShowCustomerView()
        {
            ReportPanel.ConfigureCustomerGridColumns();
            ReportPanel.ResetPaginationAndFocus();
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

                if (StatusPicker.UniqueID.EndsWith(trigger.ControlID, StringComparison.OrdinalIgnoreCase))
                {
                    trigger.ControlID = StatusPicker.UniqueID;
                    break;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            IReportManager reportManager = ReportManagerFactory.Create(this.CurrentScope(), Context.User as CffPrincipal);
            presenter = new OverdueChargesPresenter(this, ReportsService.Create(), reportManager);

            ICffClient xClient = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString :
                        (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString : null;
            if (xClient != null) {
                targetName = ": " + xClient.Name;
            }
            if (SessionWrapper.Instance.Get.CustomerFromQueryString != null)
            {
                if (targetName != null || !targetName.Equals(""))
                {
                    targetName += " / ";
                    targetName = string.Concat(targetName, xClient.Name);
                }
                else
                {
                    targetName = ": " + xClient.Name;
                }
            }
            // end
            if (!IsPostBack)
            {
                presenter.ConfigureView(this.CurrentScope());
                presenter.ShowReport(this.CurrentScope(), true);
            }
        }

        protected void DatePickerUpdate(object sender, EventArgs e)
        {
            presenter.ConfigureView(this.CurrentScope());
            presenter.ShowReport(this.CurrentScope(), false);
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);
            presenter.ConfigureView(this.CurrentScope());
            presenter.ShowReport(this.CurrentScope(), true);
        }

      

        protected void ExportButton_Click(object sender, ImageClickEventArgs e)
        {
            ReportPanel.Export();
        }

        protected void UpdateButton_Click(object sender, ImageClickEventArgs e)
        {
            presenter.ConfigureView(this.CurrentScope());
            presenter.ShowReport(this.CurrentScope(), false);
        }

        public ICffCustomer Customer
        {
            get { 
                if (SessionWrapper.Instance.Get!=null)
                    return SessionWrapper.Instance.Get.CustomerFromQueryString;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString;

                return null;
            }

            set {
                if (SessionWrapper.Instance.Get != null)
                    SessionWrapper.Instance.Get.CustomerFromQueryString = value; 

                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString = value; 
            }
        }

        public ICffClient Client
        {
            get {

                if (SessionWrapper.Instance.Get != null)
                    return SessionWrapper.Instance.Get.ClientFromQueryString; 
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString;

                return null;
            }
            set { }
        }

        public void RedirectTo(string redirectionPath)
        {
            Response.Redirect(redirectionPath);
        }
    }
}