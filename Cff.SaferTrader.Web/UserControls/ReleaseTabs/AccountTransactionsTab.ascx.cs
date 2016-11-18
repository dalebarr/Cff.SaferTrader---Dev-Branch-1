using System;
using System.Collections.Generic;

using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Core.Views.TransactionReportView;
using Cff.SaferTrader.Core.Reports;

using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;


namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public partial class AccountTransactionsTab : UserControl, IRetentionTab, IAccountTransactionsView, IRedirectableView , IPrintableView
    {
        private Date eomDate;
        private int clientID;

        private CffGenGridView AccountTransactionsGridView;
        private AccountTransactionTabPresenter presenter;
        
        private static decimal closingBalance;
        private static decimal movementAmount;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AccountTransactionsGridView = new CffGenGridView();
            AccountTransactionsGridView.ID = "CffGGV_AcctTrxGridView";
            AccountTransactionsGridView.AllowPaging = false;
            AccountTransactionsGridView.AutoGenerateColumns = false;
            
            //if (((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).Browser == "IE")
            //    AccountTransactionsGridView.BorderWidth = Unit.Pixel(0);
            //else
                AccountTransactionsGridView.BorderWidth = Unit.Pixel(1);

            //AccountTransactionsGridView.CssClass = "cffGGV";
            AccountTransactionsGridView.HeaderStyle.CssClass = "cffGGVHeader";
            AccountTransactionsGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            AccountTransactionsGridView.ShowHeaderWhenEmpty = true;
            AccountTransactionsGridView.EmptyDataText = "No data to display";
            AccountTransactionsGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            AccountTransactionsGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
            
            AccountTransactionsGridView.Width = Unit.Percentage(70);

            AccountTransactionsPlaceHolder.Controls.Clear();
            AccountTransactionsPlaceHolder.Controls.Add(AccountTransactionsGridView);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureGridColumns();

            if (IsPostBack)
            {
                DisplayReport(ViewState["AccountTransactions"] as AccountTransactionReportBase);
            }
        }

        private void ConfigureGridColumns()
        {
            //this.totalDebitLiteral.Text = string.Empty;
            //this.totalCreditLiteral.Text = string.Empty;
            this.movementLiteral.Text = string.Empty;
            this.closingBalanceliteral.Text = string.Empty;

            AccountTransactionsGridView.Columns.Clear();
            AccountTransactionsGridView.InsertDataColumn("Reference", "TranRef", CffGridViewColumnType.Text, "6%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            AccountTransactionsGridView.InsertDataColumn("TransDate", "TranDate", CffGridViewColumnType.Date, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            AccountTransactionsGridView.InsertDataColumn("TransType", "TranType", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            AccountTransactionsGridView.InsertCurrencyColumn("Debit", "Debit", "6%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            AccountTransactionsGridView.InsertCurrencyColumn("Credit", "Credit", "6%", "cffGGV_currencyCellAgedBal", true, HorizontalAlign.Right, HorizontalAlign.Right);
            AccountTransactionsGridView.InsertDataColumn("Description", "Description", CffGridViewColumnType.Text, "34%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGVHeaderLeftAgedBal1");

            AccountTransactionsGridView.ShowFooter = true;
            AccountTransactionsGridView.TotalsSummarySettings.SetColumnTotals("Debit,Credit");
            AccountTransactionsGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Debit", "cffGGV_rightAlignedCell");
            AccountTransactionsGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Credit", "cffGGV_rightAlignedCell");
            AccountTransactionsGridView.CustomFooterSettings = CffCustomFooterMode.DefaultSettings | CffCustomFooterMode.ShowTotals;
        }

        public void DisplayReport(AccountTransactionReportBase accountTransactionsReport)
        {

           if (accountTransactionsReport != null)
           {
                ViewState.Add("AccountTransactions", accountTransactionsReport);
            
                closingBalance = ((AccountTransactionReport)accountTransactionsReport).ClosingBalance;
                movementAmount = Math.Abs(((AccountTransactionReport)accountTransactionsReport).Movement);

                AccountTransactionsGridView.DataSource = ((AccountTransactionReport)accountTransactionsReport).Records;
                AccountTransactionsGridView.DataBind();

                //this.totalDebitLiteral.Text = accountTransactionsReport.TotalDebit.ToString("C");
                //this.totalCreditLiteral.Text = Math.Abs(accountTransactionsReport.TotalCredit).ToString("C");
                this.movementLiteral.Text = accountTransactionsReport.Movement.ToString("C");
                this.closingBalanceliteral.Text = Math.Abs(accountTransactionsReport.ClosingBalance).ToString("C");
           }

        }

        public void LoadTab(RetentionSchedule retentionSchedule)
        {
            try
            {
                if (retentionSchedule != null)
                {
                    ViewState.Add("RetentionSchedule", retentionSchedule);
                    ViewState.Add("RentionClientID", retentionSchedule.ClientId);
                    this.eomDate = retentionSchedule.EndOfMonth;
                    this.clientID = retentionSchedule.ClientId;
                }

              
                presenter = AccountTransactionTabPresenter.Create(this);
                presenter.LoadAccountTransactions(5000 + retentionSchedule.ClientId, retentionSchedule.EndOfMonth, retentionSchedule.ClientId);
         
            }
            catch (Exception exc)
            {
                string strExc = exc.Message;
            }
        }

        public void ClearTabData()
        {
            DisplayReport(null);
        }

        public void Print()
        {
            PrintableAccountTransactions printable = new PrintableAccountTransactions(ViewState["AccountTransactions"] as AccountTransactionReportBase, ViewState["RetentionSchedule"] as RetentionSchedule, QueryString.ViewIDValue);
            string script = PopupHelper.ShowPopup(printable, Server);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
        }

#region "IAccountTransactionsView"
        public void DisplayAccessDeniedError() { }
        public void DisplayReportNotAvailableError() { }
        
        public Date EndDate() { return eomDate;  }
        public int ClientId() { return clientID; }
        public FacilityType FacilityType() { return null;  }
        public bool IsSalvageIncluded() { return false; }
        
        public void Clear() { DisplayReport(null);  }
        public void ShowAllClientsView() { }
        public void ShowClientView() { }

#endregion


#region IRedirectableView Members

        public void RedirectTo(string redirectionPath)
        {
            Response.Redirect(redirectionPath);
        }

        public ICffClient Client
        {
            get { 
                if (SessionWrapper.Instance.Get!=null)
                    return SessionWrapper.Instance.Get.ClientFromQueryString; 
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString;
                
                return null;
            }
            set { }
        }

        //public CffCustomer Customer
        public ICffCustomer Customer
        {
            get {
                if (SessionWrapper.Instance.Get != null)
                    return SessionWrapper.Instance.Get.CustomerFromQueryString; 
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString;

                return null;
            }

            set {
                if (SessionWrapper.Instance.Get != null)
                    SessionWrapper.Instance.Get.CustomerFromQueryString = (ICffCustomer)value; 

                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString = (ICffCustomer)value; 
            }
        }

#endregion


    }
}