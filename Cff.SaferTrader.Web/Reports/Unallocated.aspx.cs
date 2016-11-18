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
    public partial class Unallocated : ReportBasePage, IUnallocatedView
    {

        protected static string targetName = "";
        private UnallocatedPresenter presenter;

        #region IUnallocatedView Members

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
            parameterSelectorDiv.Visible = true;
            ReportPanel.ResetPaginationAndFocus();
            ReportPanel.ConfigureGridColumns();
        }

        public void ShowClientView()
        {
            parameterSelectorDiv.Visible = false;
            ReportPanel.ResetPaginationAndFocus();
            ReportPanel.ConfigureGridColumns();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new UnallocatedPresenter(this, TransactionReportsService.Create(), ReportManagerFactory.Create(this.CurrentScope(), Context.User as CffPrincipal));
            presenter.ConfigureView(this.CurrentScope());

            ICffClient xClient = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString :
                  (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString : null;

            if (xClient != null) {
                targetName = ": " + SessionWrapper.Instance.Get.ClientFromQueryString.Name;
            }

            ICffCustomer xCustomer = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.CustomerFromQueryString :
                (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString : null;
            if (xCustomer != null)
            {
                if (targetName != null || !targetName.Equals(""))
                {
                    targetName += " / ";
                    targetName = string.Concat(targetName, SessionWrapper.Instance.Get.CustomerFromQueryString.Name);
                }
                else
                {
                    targetName = ": " + SessionWrapper.Instance.Get.CustomerFromQueryString.Name;
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
            presenter.ShowReport(this.CurrentScope(), true);
        }

        protected void UpdateButtonClick(object sender, ImageClickEventArgs e)
        {
            ReportPanel.ResetPaginationAndFocus();
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