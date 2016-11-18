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
    public partial class CreditsTab : UserControl, ICreditsTabView, IBatchTab, IRedirectableView, IPrintableView
    {
        private CreditsTabPresenter presenter;

        #region IBatchTab Members

        public void LoadTab(InvoiceBatch invoiceBatch)
        {
            ViewState.Add("InvoiceBatch", invoiceBatch);
            ViewState.Add("BatchClientId", invoiceBatch.ClientId);

            presenter = CreditsTabPresenter.Create(this);
            presenter.LoadCreditLinesFor(invoiceBatch.ClientId, invoiceBatch.Number);
        }

        public void ClearTabData()
        {
            DisplayCredits(null);
        }

        #endregion

        #region ICreditsTabView Members

        public void DisplayCredits(IList<CreditLine> credits)
        {
            ViewState.Add("Credits", credits);

            CreditsGridView.DataSource = credits;
            CreditsGridView.DataBind();
        }

        #endregion

        #region IRedirectableView Members

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


        CffGenGridView CreditsGridView;

        protected override void OnInit(EventArgs e)
        {
            CreditsGridView = new CffGenGridView();
            CreditsGridView.PageSize = 1000;
            CreditsGridView.AllowPaging = false;
            CreditsGridView.AutoGenerateColumns = false;
            CreditsGridView.ShowHeaderWhenEmpty = true;
            CreditsGridView.ShowFooter = true;
            CreditsGridView.EmptyDataText = "No data to display";
            
            //CreditsGridView.CssClass = "cffGGV";
            CreditsGridView.HeaderStyle.CssClass = "cffGGVHeader";
            CreditsGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            CreditsGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            //if (((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).Browser == "IE")
            //    CreditsGridView.BorderWidth = Unit.Pixel(0);
            //else
                CreditsGridView.BorderWidth = Unit.Pixel(1);

            CCGridViewPlaceHolder.Controls.Clear();
            CCGridViewPlaceHolder.Controls.Add(CreditsGridView);
        }

        protected void ConfigureGridColumns()
        {
            CreditsGridView.Columns.Clear();
            CreditsGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CreditsGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            CreditsGridView.InsertDataColumn("Transaction", "TransactionNumber", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CreditsGridView.InsertDataColumn("Dated", "Dated", CffGridViewColumnType.Date, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CreditsGridView.InsertCurrencyColumn("Amount", "Amount", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            CreditsGridView.InsertBoundHyperLinkColumn("Batch", "Batch", "Batch", "4%", "cffGGV_HyperLinkLeftPW10", HorizontalAlign.Left, HorizontalAlign.Left, "", "cffGGVHeaderLeftAgedBal2");
            CreditsGridView.InsertDataColumn("Created", "Created", CffGridViewColumnType.Date, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            CreditsGridView.ShowFooter = true;
            CreditsGridView.TotalsSummarySettings.SetColumnTotals("Amount");
            CreditsGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            CreditsGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            CreditsGridView.Width = Unit.Percentage(50);
            CreditsGridView.Height = Unit.Percentage(100);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureGridColumns();

            if (IsPostBack)
            {
                DisplayCredits(ViewState["Credits"] as IList<CreditLine>);
            }
        }

        public void Print()
        {
            PrintableInvoiceBatchCredits printable = new PrintableInvoiceBatchCredits(ViewState["InvoiceBatch"] as InvoiceBatch,
                                                                                        ViewState["Credits"] as IList<CreditLine>, QueryString.ViewIDValue);
            string script = PopupHelper.ShowPopup(printable, Server);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
        }
    }
}