using System;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters.TransactionReportPresenters;
using Cff.SaferTrader.Core.ReportManager;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.TransactionReportView;

namespace Cff.SaferTrader.Web.Reports
{
    public partial class Receipts : ReportBasePage, IReceiptsView
    {
        protected static string filterType = "";
        protected static string targetName = "";
        private ReceiptsPresenter presenter;

        #region IReceiptsView Members

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

        public int CustomerId()
        {
            //return SessionWrapper.Instance.Customer.Id;
            return SessionWrapper.Instance.Get.CustomerFromQueryString.Id;
        }

        public void Clear()
        {
            Master.ShowReportViewer();
            reportPanelContainer.HideExportButton();
            reportPanelContainer.Clear();
        }

        public void ShowAllClientsView()
        {
            reportPanelContainer.ShowAllClientsView();
        }

        public void ShowClientView()
        {
            reportPanelContainer.ShowClientView();
        }

        public void DisplayReport(TransactionReportBase report)
        {
            Master.ShowReportViewer();
            reportPanelContainer.ShowExportButton();
            reportPanelContainer.DisplayTransactionsReport(report);
        }

        public void DisplayReportForCustomer(ReportBase report)
        {
            Master.ShowReportViewer();
            reportPanelContainer.ShowExportButton();
            reportPanelContainer.DisplayTransactionsReportForCustomer(report);
        }

        public void ShowCustomerView()
        {
            reportPanelContainer.ShowCustomerView();
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            presenter = new ReceiptsPresenter(this, TransactionReportsService.Create(), ReportManagerFactory.Create(SessionWrapper.Instance.Scope, Context.User as CffPrincipal));
            presenter.ConfigureView(SessionWrapper.Instance.Scope);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            reportPanelContainer.ReportParameterUpdated += ReportParameterUpdated;
            // start related ref:CFF-18
            if (SessionWrapper.Instance.Get != null) {
                if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
                {
                    targetName = ": " + SessionWrapper.Instance.Get.ClientFromQueryString.Name;
                }
                if (SessionWrapper.Instance.Get.CustomerFromQueryString != null)
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
            }
            else if (QueryString.ViewIDValue != null) {

                if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString != null)
                {
                    targetName = ": " + SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Name;
                }
                if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString != null)
                {
                    if (targetName != null || !targetName.Equals(""))
                    {
                        targetName += " / ";
                        targetName = string.Concat(targetName, SessionWrapper.Instance.Get.CustomerFromQueryString.Name);
                    }
                    else
                    {
                        targetName = ": " + SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString.Name;
                    }
                }
            }
           
            // end
            
            //if (IsPostBack)
            //{ marty wants this loaded on select 
             presenter.ShowReport(SessionWrapper.Instance.Scope, true);
             UpdateTitle((reportPanelContainer.FindControl("TransactionStatusTypesFilterControl").Controls[1] as System.Web.UI.WebControls.DropDownList).Text);
            //}
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);
            reportPanelContainer.ResetPaginationAndFocus();
            presenter.ShowReport(SessionWrapper.Instance.Scope, true);
        }

        private void ReportParameterUpdated(object sender, EventArgs e)
        {
            presenter.ShowReport(SessionWrapper.Instance.Scope, false);
            UpdateTitle((reportPanelContainer.FindControl("TransactionStatusTypesFilterControl").Controls[1] as System.Web.UI.WebControls.DropDownList).Text);
        }

        private void UpdateTitle(String title)
        {
            if (title == null || title == "" || title == "All")
            {
                filterType = "";
                TitleDiv.InnerText = String.Format("Receipts {0}", targetName);
            }
            else
            {
                filterType = title;
                TitleDiv.InnerText = String.Format("{0} Receipts {1}", filterType, targetName);
            }
        }
    }
}