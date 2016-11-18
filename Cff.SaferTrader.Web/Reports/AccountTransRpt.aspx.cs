using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters.TransactionReportPresenters;
using Cff.SaferTrader.Core.ReportManager;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Core.Views.TransactionReportView;

namespace Cff.SaferTrader.Web.Reports
{
    public partial class AccountTransRpt : ReportBasePage, IAccountTransactionsView, IPrintableView
    {
        private int clientid;
        private  AccountTransactionsPresenter presenter;

        protected static string targetName = "";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.clientid = 0;
            if (SessionWrapper.Instance.Get != null)
                this.clientid = SessionWrapper.Instance.Get.ClientFromQueryString.Id;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                this.clientid = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id;

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
            Scope sScope = this.CurrentScope();
            presenter = new AccountTransactionsPresenter(this, TransactionReportsService.Create(), ReportManagerFactory.Create(sScope, Context.User as CffPrincipal));
            presenter.ConfigureView(sScope);

            ICffClient xClient = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString :
                      (!string.IsNullOrWhiteSpace(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString : null;
            
            if (xClient != null) {
                targetName = ": " + xClient.Name;
            }


            ICffCustomer xCustomer = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.CustomerFromQueryString :
                      (!string.IsNullOrWhiteSpace(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString : null;
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

            if (!IsPostBack) {
                presenter.ShowReport(sScope, true);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewAllButton.Visible =  (this.CurrentScope()!= Scope.AllClientsScope) && ReportPanel.IsViewAllButtonRequired();
        }

        public void ShowTransactionsReport(AccountTransactionReportBase transactionReport)
        {
            this.DateViewedLiteral.Text = transactionReport.DateViewed.ToDateTimeString();
            Master.ShowReportViewer();
            ReportPanel.Display((AccountTransactionReport)transactionReport);
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);
            presenter.ShowReport (this.CurrentScope(), true);
        }


#region "IAccountTransctionView"
        public int ClientId()
        {
           return this.clientid; 
        }

        public FacilityType FacilityType()
        {
           return AllClientsFilter.FacilityType;
        }

        public bool IsSalvageIncluded()
        {
            return AllClientsFilter.IsSalvageIncluded;
        }

        public Date EndDate() 
        {
            return DatePicker.Date;
        }
        
        public void Clear() 
        {
            Master.ShowReportViewer();
            reportData.Visible = false;
            AllClientsReportHelpMessage.Visible = true;
            ReportPanel.ResetPaginationAndFocus();
            ReportPanel.Clear();
        }
        
        public void ShowAllClientsView()
        {
            AllClientsFilter.Visible = true;
            UpdateButton.Visible = true;
            DatePicker.EnableAutoPostBack = false;
            ReportPanel.ConfigureGridColumns();
        }

        public void ShowClientView()
        {
            DatePicker.Update += DatePickerUpdate;
            DatePicker.EnableAutoPostBack = true;
            UpdateButton.Visible = false;
            AllClientsFilter.Visible = false;
            ReportPanel.ConfigureGridColumns();
        }

        public void DisplayReport(AccountTransactionReportBase report)
        {
            Master.ShowReportViewer();
            reportData.Visible = true;
            AllClientsReportHelpMessage.Visible = false;
            ReportPanel.ResetPaginationAndFocus();
            ReportPanel.Display((AccountTransactionReport)report);
        }

#endregion

#region "IPrintableView"
        public void Print() { 
        
        }
#endregion


#region "ButtonEvents"
        protected void DatePickerUpdate(object sender, EventArgs e)
        {
            presenter.ShowReport(this.CurrentScope(), false);
        }

        protected void UpdateButtonClick(object sender, ImageClickEventArgs e)
        {
            presenter.ShowReport(this.CurrentScope(), false);
        }

     
        protected void ExportButton_Click(object sender, ImageClickEventArgs e)
        {
            ReportPanel.Export();
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
#endregion




    }
}
