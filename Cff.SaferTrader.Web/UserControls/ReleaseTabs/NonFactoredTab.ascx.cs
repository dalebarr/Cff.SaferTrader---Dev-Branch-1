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
    public partial class NonFactoredTab : UserControl, INonFactoredTabView, IBatchTab, IRedirectableView, IPrintableView
    {
        private NonFactoredTabPresenter presenter;

        CffGenGridView NonFactoredGridView;

        protected override void OnInit(EventArgs e)
        {
            NonFactoredGridView = new CffGenGridView();
            NonFactoredGridView.PageSize = 1000;
            NonFactoredGridView.DefaultPageSize = 1000;
            NonFactoredGridView.AllowPaging= false;
            NonFactoredGridView.AllowCustomPaging = false;
            NonFactoredGridView.AutoGenerateColumns = false;
            NonFactoredGridView.ShowHeaderWhenEmpty = true;
            NonFactoredGridView.EmptyDataText = "No data to display";

            //NonFactoredGridView.CssClass = "cffGGV";
            NonFactoredGridView.HeaderStyle.CssClass = "cffGGVHeader";
            NonFactoredGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            NonFactoredGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            
            //if (((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).Browser == "IE")
            //    NonFactoredGridView.BorderWidth = Unit.Pixel(0);
            //else
                NonFactoredGridView.BorderWidth = Unit.Pixel(1);


            NonFactoredGridView.Columns.Clear();
            NonFactoredGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            NonFactoredGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            NonFactoredGridView.InsertDataColumn("Transaction", "TransactionNumber", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            NonFactoredGridView.InsertDataColumn("Reference", "Reference", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            NonFactoredGridView.InsertDataColumn("Processed", "FactoredDate", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            NonFactoredGridView.InsertDataColumn("Date", "Date", CffGridViewColumnType.Date, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            NonFactoredGridView.InsertCurrencyColumn("Amount", "Amount", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            NonFactoredGridView.InsertCurrencyColumn("Balance", "Balance", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            NonFactoredGridView.InsertDataColumn("Status", "TransactionStatus", CffGridViewColumnType.Text, "4%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true); //, "cffGGVHeaderLeftAgedBal2");
            NonFactoredGridView.InsertDataColumn("Modified", "Modified", CffGridViewColumnType.Text, "4%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            NonFactoredGridView.InsertDataColumn("Repurchased", "Repurchased", CffGridViewColumnType.Text, "4%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            NonFactoredGridView.InsertDataColumn("Purch.Order", "PurchaseOrder", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);  //, "cffGGVHeaderWPad15");

            NonFactoredGridView.Width = Unit.Percentage(60);
            //NonFactoredGridView.Height = Unit.Percentage(100);

            NonFactoredGridView.ShowFooter = true;
            NonFactoredGridView.TotalsSummarySettings.SetColumnTotals("Amount,Balance");
            NonFactoredGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            NonFactoredGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Balance", "cffGGV_currencyCell");
            NonFactoredGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;

            GVPlaceHolder.Controls.Clear();
            GVPlaceHolder.Controls.Add(NonFactoredGridView);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                DisplayNonFactoredInvoices(ViewState["NonFactoredInvoices"] as IList<Invoice>);
            }
        }

        public void LoadTab(InvoiceBatch invoiceBatch)
        {
            ViewState.Add("InvoiceBatch", invoiceBatch);

            presenter = NonFactoredTabPresenter.Create(this);
            presenter.LoadNonFactoredInvoicesFor(invoiceBatch.ClientId, invoiceBatch.Number);
        }

        public void ClearTabData()
        {
            DisplayNonFactoredInvoices(null);
        }

        public void DisplayNonFactoredInvoices(IList<Invoice> nonFactoredInvoices)
        {
            ViewState.Add("NonFactoredInvoices", nonFactoredInvoices);

            NonFactoredGridView.DataSource = nonFactoredInvoices;
            NonFactoredGridView.DataBind();
        }

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

        public void Print()
        {
            PrintableInvoiceBatchNonFactoredInvoices printable = new PrintableInvoiceBatchNonFactoredInvoices((ViewState["InvoiceBatch"] as InvoiceBatch),
                                                                                        (ViewState["NonFactoredInvoices"] as IList<Invoice>),
                                                                                        QueryString.ViewIDValue);
            string script = PopupHelper.ShowPopup(printable, Server);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
        }
    }
}