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
    public partial class InvoicesTab : UserControl, IInvoicesTabView, IBatchTab, IRedirectableView, IPrintableView
    {
        private InvoicesTabPresenter presenter;
        CffGenGridView InvoiceGridView;

        protected override void OnInit(EventArgs e)
        {
            InvoiceGridView = new CffGenGridView();
            InvoiceGridView.PageSize = 1000;
            InvoiceGridView.ID = "InvoiceGridView";
            InvoiceGridView.AutoGenerateColumns = false;
            InvoiceGridView.ShowHeaderWhenEmpty = true;
            InvoiceGridView.EnableViewState = true;

            //if (((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).Browser == "IE")
            //    InvoiceGridView.BorderWidth = Unit.Pixel(0);
            //else
                InvoiceGridView.BorderWidth = Unit.Point(1);

            InvoiceGridView.EmptyDataText = "No data to display";
            //InvoiceGridView.CssClass = "cffGGV";
            InvoiceGridView.HeaderStyle.CssClass = "cffGGVHeader";

            InvoiceGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            InvoiceGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            InvoiceGridView.AllowPaging = false; //as per marty's suggestions
            //InvoiceGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;
            InvoiceGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

            GVPlaceHolder.Controls.Clear();
            GVPlaceHolder.Controls.Add(InvoiceGridView);
        }

        protected void ConfigureGridColumns()
        {
            InvoiceGridView.Columns.Clear();
            InvoiceGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            InvoiceGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "15%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);

            InvoiceGridView.InsertDataColumn("Transaction", "TransactionNumber", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            InvoiceGridView.InsertDataColumn("Reference", "Reference", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            InvoiceGridView.InsertDataColumn("Processed", "FactoredDate", CffGridViewColumnType.Date, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            InvoiceGridView.InsertDataColumn("Date", "Date", CffGridViewColumnType.Date, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            InvoiceGridView.InsertCurrencyColumn("Amount", "Amount", "6%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            InvoiceGridView.InsertCurrencyColumn("Balance", "Balance", "6%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            InvoiceGridView.InsertDataColumn("Status", "TransactionStatus", CffGridViewColumnType.Text, "4%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true);   //, "cffGGVHeaderLeftAgedBal2");
            InvoiceGridView.InsertDataColumn("Prepaid", "Repurchased", CffGridViewColumnType.Date, "4%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            InvoiceGridView.InsertDataColumn("Purch.Order", "PurchaseOrder", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);   // "cffGGVHeaderWPad15");

            InvoiceGridView.ShowFooter = true;
            InvoiceGridView.TotalsSummarySettings.SetColumnTotals("Amount,Balance");
            InvoiceGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            InvoiceGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Balance", "cffGGV_currencyCell");
            InvoiceGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
          
            InvoiceGridView.Width = Unit.Percentage(60);
            InvoiceGridView.Height = Unit.Percentage(100);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureGridColumns();

            if (IsPostBack)
            {
                DisplayInvoices(ViewState["Invoices"] as IList<Invoice>);
            }
        }

        public void LoadTab(InvoiceBatch invoiceBatch)
        {
            try
            {
                ViewState.Add("InvoiceBatch", invoiceBatch);

                presenter = InvoicesTabPresenter.Create(this);
                presenter.LoadInvoicesFor(invoiceBatch.ClientId, invoiceBatch.Number);
            }
            catch { }
        }

        public void ClearTabData()
        {
            DisplayInvoices(null);
        }

        public void DisplayInvoices(IList<Invoice> invoices)
        {
            ViewState.Add("Invoices", invoices);

            InvoiceGridView.DataSource = invoices;
            InvoiceGridView.DataBind();
        }

        public void RedirectTo(string redirectionPath)
        {
            Response.Redirect(redirectionPath);
        }

        public ICffClient Client
        {
            get {
                if (SessionWrapper.Instance.Get != null)
                    return SessionWrapper.Instance.Get.ClientFromQueryString;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString;
                else
                    return null;
            }
            set { }
        }

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
                    SessionWrapper.Instance.Get.CustomerFromQueryString = value;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString = value;
            }
        }

        public void Print()
        {
            PrintableInvoiceBatchInvoices printable = new PrintableInvoiceBatchInvoices(ViewState["InvoiceBatch"] as InvoiceBatch,
                                                                                        ViewState["Invoices"] as IList<Invoice>, QueryString.ViewIDValue);
            string script = PopupHelper.ShowPopup(printable, Server);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
        }
    }
}