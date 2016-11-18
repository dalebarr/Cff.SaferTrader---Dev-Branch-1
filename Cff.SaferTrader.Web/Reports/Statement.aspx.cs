using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.ReportManager;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Web.UserControls;

namespace Cff.SaferTrader.Web.Reports
{
    public partial class Statement : ReportBasePage, IStatementView, IPrintableView
    {
        protected static string targetName = "";
        private StatementPresenter presenter;

        #region IStatementView Members

        public int ClientId
        {
            get {
                if (SessionWrapper.Instance.Get != null)
                    return SessionWrapper.Instance.Get.ClientFromQueryString.Id;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id;

                return (QueryString.ClientId == null) ? 0 : (int)QueryString.ClientId;
            }
        }

        public int CustomerId
        {
            get {
                if (SessionWrapper.Instance.Get != null)
                    return SessionWrapper.Instance.Get.CustomerFromQueryString.Id;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString.Id;

                return (QueryString.CustomerId == null) ? 0 : (int)QueryString.CustomerId;
            }
        }

        public void HideReportViewer()
        {
            Master.HideReportViewer("Please select a customer to view the report.");
        }

        public void ShowCustomerView()
        {
            Master.ShowReportViewer();
        }

        public void ShowReport(StatementReport report)
        {
            if (report != null)
            {
                this.DateViewedLiteral.Text = report.DateViewed.ToDateTimeString();
                ViewState.Add("StatementReport", report);
                stReportPanel.ShowReport(report);
            }
        }


        public Date EndDate
        {
            get { return DatePicker.Date; }
        }

        public Scope Scope
        {
            get { return this.CurrentScope(); }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            DatePicker.Update += DatePickerUpdated;

            presenter = new StatementPresenter(this, RepositoryFactory.CreateReportRepository(),
                                               ReportManagerFactory.Create(SessionWrapper.Instance.Scope,
                                                                           Context.User as CffPrincipal));

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
            // end
            if (!IsPostBack)
            {
                presenter.InitializeForScope();
                presenter.LoadStatementReport();
            }
            else
            {
                ShowReport(ViewState["StatementReport"] as StatementReport);
            }
        }


        private void DatePickerUpdated(object sender, EventArgs e)
        {
            presenter.LoadStatementReport();
            if (this.presenter.getReportMgr().CanViewStatementReport())
            {
                ShowReport(ViewState["StatementReport"] as StatementReport);
            }
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);
            presenter.InitializeForScope();
            presenter.LoadStatementReport();
        }


        protected void PrintButton_Click(object sender, ImageClickEventArgs e)
        {
            Print();
        }

        public void Print()
        {
            string script = PopupHelper.ShowPopup(ViewState["StatementReport"] as StatementReport, Server);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
        }

    }

}