using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Web.UserControls.Interfaces;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public partial class RepurchasesTab : UserControl, IRepurchasesTabView, IBatchTab, IRedirectableView, IPrintableView
    {
        private RepurchasesTabPresenter presenter;
        CffGenGridView RepurchasesGridView;

        protected override void OnInit(EventArgs e)
        {
            RepurchasesGridView = new CffGenGridView();
            RepurchasesGridView.BorderWidth = Unit.Pixel(1);     //modified by dbb
            RepurchasesGridView.AutoGenerateColumns = false;
            RepurchasesGridView.ShowHeaderWhenEmpty = true;
            
            //RepurchasesGridView.CssClass = "cffGGV";
            RepurchasesGridView.HeaderStyle.CssClass = "cffGGVHeader";
            RepurchasesGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            RepurchasesGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            RepurchasesGridView.EmptyDataText = "No data to display";
            RepurchasesGridView.EnableViewState = true;
            RepurchasesGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            RepurchasesGridView.Columns.Clear();
            RepurchasesGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            RepurchasesGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            RepurchasesGridView.InsertDataColumn("Transaction", "TransactionNumber", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            RepurchasesGridView.InsertDataColumn("Dated", "Dated", CffGridViewColumnType.Date, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            RepurchasesGridView.InsertCurrencyColumn("Amount", "Amount", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            RepurchasesGridView.InsertBoundHyperLinkColumn("Batch", "Batch", "Batch", "4%", "cffGGV_HyperLinkLeftPW10", HorizontalAlign.Left, HorizontalAlign.Left, "", "cffGGVHeaderLeftAgedBal2");
            RepurchasesGridView.InsertDataColumn("Created", "Created", CffGridViewColumnType.Date, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            RepurchasesGridView.ShowFooter = true;
            RepurchasesGridView.TotalsSummarySettings.SetColumnTotals("Amount");
            RepurchasesGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            RepurchasesGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            RepurchasesGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

            RepurchasesGridView.Width = Unit.Percentage(50);
            RepurchasesGridView.Height = Unit.Percentage(100);

            GVPlaceHolder.Controls.Clear();
            GVPlaceHolder.Controls.Add(RepurchasesGridView);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                DisplayRepurchases(ViewState["Repurchases"] as IList<RepurchasesLine>);
            }
        }

        public void LoadTab(InvoiceBatch invoiceBatch)
        {
            ViewState.Add("InvoiceBatch", invoiceBatch);
            ViewState.Add("BatchClientId", invoiceBatch.ClientId);

            presenter = RepurchasesTabPresenter.Create(this);
            presenter.LoadRepurchasesLinesFor(invoiceBatch.ClientId, invoiceBatch.Number);
        }

        public void ClearTabData()
        {
            DisplayRepurchases(null);
        }

        public void DisplayRepurchases(IList<RepurchasesLine> repurchasesLine)
        {
            ViewState.Add("Repurchases", repurchasesLine);

            RepurchasesGridView.DataSource = repurchasesLine;
            RepurchasesGridView.DataBind();
        }

        #region IRedirectableVsiew Members

        public void RedirectTo(string redirectionPath)
        {
            Response.Redirect(redirectionPath);
        }

        public ICffClient Client
        {
            get
            {
                if (SessionWrapper.Instance.Get != null)
                    return SessionWrapper.Instance.Get.ClientFromQueryString;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString;
                return null;
            }
            set { }
        }

        public ICffCustomer Customer
        {
            get
            {
                if (SessionWrapper.Instance.Get != null)
                    return SessionWrapper.Instance.Get.CustomerFromQueryString;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString;
                return null;
            }

            set
            {
                if (SessionWrapper.Instance.Get != null)
                    SessionWrapper.Instance.Get.CustomerFromQueryString = (ICffCustomer)value;

                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString = (ICffCustomer)value;
            }
        }


        #endregion

        public void Print()
        {
            PrintableInvoiceBatchRepurchases printable = new PrintableInvoiceBatchRepurchases(ViewState["InvoiceBatch"] as InvoiceBatch, ViewState["Repurchases"] as IList<RepurchasesLine>, QueryString.ViewIDValue);
            string script = PopupHelper.ShowPopup(printable, Server);

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
        }
    }
}