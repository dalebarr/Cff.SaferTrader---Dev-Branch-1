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
    public partial class CreditLimitExceeded : ReportBasePage, ICreditLimitExceededView
    {
        protected static string targetName = "";
        private CreditLimitExceededPresenter presenter;
        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new CreditLimitExceededPresenter(this, ReportsService.Create(), ReportManagerFactory.Create(this.CurrentScope(), Context.User as CffPrincipal));
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
            
            if (!IsPostBack)
            {
                presenter.ShowReport(this.CurrentScope(), true);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewAllButton.Visible = (this.CurrentScope() != Scope.AllClientsScope) && ReportPanel.IsViewAllButtonRequired();
        }

        protected void UpdateButtonClick(object sender, EventArgs e)
        {
            ReportPanel.ResetPaginationAndFocus();
            presenter.ConfigureView(this.CurrentScope());
            presenter.ShowReport(this.CurrentScope(), false);
        }

        protected void ViewAllButton_Click(object sender, ImageClickEventArgs e)
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

        #region ICreditLimitExceededView Members

        public int ClientId()
        {
            if (SessionWrapper.Instance.Get!=null)
                 return SessionWrapper.Instance.Get.ClientFromQueryString.Id;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id;

            return (QueryString.ClientId == null)?0:(int)QueryString.ClientId;
        }

        public int ClientFacilityType()
        {
            if (SessionWrapper.Instance.Get != null)
                return SessionWrapper.Instance.Get.ClientFromQueryString.ClientFacilityType;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.ClientFacilityType;

            return 0;
        }

        public void Clear()
        {
            this.DateViewedLiteral.Text = string.Empty;
            Master.ShowReportViewer();
            reportData.Visible = false;
            AllClientsReportHelpMessage.Visible = true;
            ReportPanel.Clear();
        }

        public bool IsSalvageIncluded()
        {
            return AllClientsFilter.IsSalvageIncluded;
        }
        public FacilityType FacilityType()
        {
            return AllClientsFilter.FacilityType;
        }
        public void ShowAllClientsView()
        {
            ReportPanel.ResetPaginationAndFocus();
            parameterSelectorDiv.Visible = true;
            ReportPanel.ConfigureAllClientsGridColumns();
            AllClientsFilter.Update += UpdateButtonClick;
        }

        public void ShowClientView()
        {
            ReportPanel.ResetPaginationAndFocus();
            parameterSelectorDiv.Visible = false;
            ReportPanel.ConfigureClientGridColumns();
        }

        public void DisplayReport(CreditLimitExceededReport report)
        {
            this.DateViewedLiteral.Text = report.DateViewed.ToDateTimeString();
            Master.ShowReportViewer();
            reportData.Visible = true;
            AllClientsReportHelpMessage.Visible = false;
            ReportPanel.Display(report);
        }

        #endregion
    }
}