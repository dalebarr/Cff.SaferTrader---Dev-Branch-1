using System;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters.TransactionReportPresenters;
using Cff.SaferTrader.Core.ReportManager;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.TransactionReportView;

namespace Cff.SaferTrader.Web.Reports
{
    public partial class Journals : ReportBasePage, IJournalsView
    {
        protected static string filterType = "";
        protected static string targetName = "";
        private JournalsPresenter presenter;

        #region IJournalsView Members

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

        public int CustomerId()
        {
            if (SessionWrapper.Instance.Get != null)
                return SessionWrapper.Instance.Get.CustomerFromQueryString.Id;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString.Id;

            return (QueryString.CustomerId == null) ? 0 : (int)QueryString.CustomerId;
        }


        public bool IsSalvageIncluded()
        {
            return reportPanelContainer.IsSalvageIncluded;
        }

        public FacilityType FacilityType()
        {
            return reportPanelContainer.FacilityType;
        }

        public DateRange DateRange()
        {
            return reportPanelContainer.SelectedDateRange;
        }

        public Date EndDate()
        {
            return reportPanelContainer.Date;
        }

        
        public void Clear()
        {
            reportPanelContainer.HideExportButton();
            reportPanelContainer.Clear();
        }

        public void ShowAllClientsView()
        {
            Master.ShowReportViewer();
            reportPanelContainer.ShowAllClientsView();
            reportPanelContainer.ResetPaginationAndFocus();
        }


        public void ShowClientView()
        {
            Master.ShowReportViewer();
            reportPanelContainer.ShowClientView();
            reportPanelContainer.ResetPaginationAndFocus();
        }

        public void ShowCustomerView()
        {
            Master.ShowReportViewer();
            reportPanelContainer.ShowCustomerView();
            reportPanelContainer.ResetPaginationAndFocus();
        }

        public void DisplayReport(TransactionReportBase report)
        {
            reportPanelContainer.ShowExportButton();
            reportPanelContainer.DisplayTransactionsReport(report);
            reportPanelContainer.ResetPaginationAndFocus();
        }

        public void DisplayReportForCustomer(ReportBase report)
        {
            reportPanelContainer.ShowExportButton();
            reportPanelContainer.DisplayTransactionsReportForCustomer(report);
            reportPanelContainer.ResetPaginationAndFocus();
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            reportPanelContainer.ReportParameterUpdated += ReportParameterUpdated;

            presenter = new JournalsPresenter(this, TransactionReportsService.Create(), ReportManagerFactory.Create(this.CurrentScope(), Context.User as CffPrincipal));
            presenter.ConfigureView(this.CurrentScope());

            ICffClient xClient = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString :
                      (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString : null;
            if (xClient != null)
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
            // end
            //if (!IsPostBack)
            //{
                presenter.ShowReport(this.CurrentScope(), true);
                UpdateTitle((reportPanelContainer.FindControl("TransactionStatusTypesFilterControl").Controls[1] as System.Web.UI.WebControls.DropDownList).Text);
            //}
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);
            presenter.ConfigureView(this.CurrentScope());
            presenter.ShowReport(this.CurrentScope(), true);
        }

        private void ReportParameterUpdated(object sender, EventArgs e)
        {
            presenter.ConfigureView(this.CurrentScope());
            presenter.ShowReport(this.CurrentScope(), false);
            UpdateTitle((reportPanelContainer.FindControl("TransactionStatusTypesFilterControl").Controls[1] as System.Web.UI.WebControls.DropDownList).Text);
        }
        private void UpdateTitle(String title)
        {
            if (title == null || title == "" || title == "All")
            {
                filterType = "";
                TitleDiv.InnerText = String.Format("Journals {0}", targetName);
            }
            else
            {
                filterType = title;
                TitleDiv.InnerText = String.Format("{0} Journals {1}", filterType, targetName);
            }
        }
    }
}