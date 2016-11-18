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
    public partial class ClientAction : ReportBasePage, IClientActionView
    {
        #region IReportView Members

        public void ShowAllClientsView()
        {
            ReportPanel.ResetPaginationAndFocus();
            ReportPanel.ConfigureAllClientsGridColumns();
            parameterSelector.Visible = true;
        }

        public void ShowClientView()
        {
            ReportPanel.ResetPaginationAndFocus();
            ReportPanel.ConfigureClientGridColumns();
            parameterSelector.Visible = false;
        }

        public void DisplayReport(ClientActionReport report)
        {
            this.DateViewedLiteral.Text = report.DateViewed.ToDateTimeString();
            Master.ShowReportViewer();
            reportData.Visible = true;
            AllClientsReportHelpMessage.Visible = false;
            ReportPanel.Display(report);
        }

        public void Clear()
        {
            this.DateViewedLiteral.Text = string.Empty;
            Master.ShowReportViewer();
            reportData.Visible = false;
            AllClientsReportHelpMessage.Visible = true;
            ReportPanel.Clear();
        }

        public int ClientId()
        {
            return SessionWrapper.Instance.Get.ClientFromQueryString.Id;
        }

        public BalanceRange BalanceRange()
        {
            return BalanceRangeFilter.BalanceRange;
        }

        public FacilityType FacilityType()
        {
            return AllClientsFilter.FacilityType;
        }

        public bool IsSalvageIncluded()
        {
            return AllClientsFilter.IsSalvageIncluded;
        }

        public string AllClientsOrderByString()
        {
            return AllClientsOrderByFilter.OrderString;
        }

        #endregion

        protected static string targetName = "";
        private ClientActionPresenter presenter;


        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new ClientActionPresenter(this, ReportsService.Create(), ReportManagerFactory.Create(this.CurrentScope(), Context.User as CffPrincipal));
            // start related ref:CFF-18

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

            if (!IsPostBack) {
                presenter.ConfigureView(this.CurrentScope());
                presenter.ShowReport(this.CurrentScope(), true);
            }
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

        protected void ExportButton_Click(object sender, ImageClickEventArgs e)
        {
            ReportPanel.Export();
        }
    }
}