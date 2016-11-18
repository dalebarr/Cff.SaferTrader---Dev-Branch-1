using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Web.UserControls.Interfaces;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;


namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public partial class BatchAdjustmentsTab : UserControl, IBatchChargesTabView, IBatchTab, IPrintableView
    {
        private BatchChargesTabPresenter presenter;

        protected void Page_Init(object sender, EventArgs e)
        {
            InsertColumns();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BatchChargesGridView.Columns.Clear();
            BatchChargesGridView.InsertDataColumn("Type", "Type", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            BatchChargesGridView.InsertCurrencyColumn("Amount", "Amount", "5%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            BatchChargesGridView.InsertDataColumn("Description", "Description", CffGridViewColumnType.Text, "30%", "cffGGV_leftAlignedCell",
                                                        HorizontalAlign.Left, HorizontalAlign.Left, true);  //reused
           
            if (IsPostBack)
            {
                DisplayBatchCharges(ViewState["BatchCharges"] as IList<Charge>);
            }
        }

        public void LoadTab(InvoiceBatch invoiceBatch)
        {
            ViewState.Add("InvoiceBatch", invoiceBatch);

            presenter = BatchChargesTabPresenter.Create(this);
            presenter.LoadBatchChargesFor(invoiceBatch.Number);
        }

        public void ClearTabData()
        {
            DisplayBatchCharges(null);
        }

        public void DisplayBatchCharges(IList<Charge> charges)
        {
            ViewState.Add("BatchCharges", charges);
            BatchChargesGridView.DataSource = charges;
            BatchChargesGridView.DataBind();
        }

        public void Print()
        {
            PrintableInvoiceBatchCharges printable = new PrintableInvoiceBatchCharges(ViewState["InvoiceBatch"] as InvoiceBatch,
                                                                                        ViewState["BatchCharges"] as IList<Charge>, QueryString.ViewIDValue);
            string script = PopupHelper.ShowPopup(printable, Server);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
        }

        private void InsertColumns()
        {
            BatchChargesGridView.AutoGenerateColumns = false;
            BatchChargesGridView.PageSize = 1000;
            //BatchChargesGridView.CssClass = "cffGGV";
            BatchChargesGridView.HeaderStyle.CssClass = "cffGGVHeader";
            BatchChargesGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            BatchChargesGridView.EmptyDataText = "No data to display";
            BatchChargesGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            //if (((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).Browser == "IE")
            //    BatchChargesGridView.BorderWidth = Unit.Pixel(0);
            //else
                BatchChargesGridView.BorderWidth = Unit.Pixel(1);

            BatchChargesGridView.ShowHeaderWhenEmpty = true;
            BatchChargesGridView.ShowFooter = true;
            BatchChargesGridView.AllowCustomPaging = true;

            BatchChargesGridView.TotalsSummarySettings.SetColumnTotals("Amount");
            BatchChargesGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            BatchChargesGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;

            BatchChargesGridView.Width = Unit.Percentage(50);
            BatchChargesGridView.Height = Unit.Percentage(100);
            BatchChargesGridView.HorizontalAlign = HorizontalAlign.Left;
            //BatchChargesGridView.Style.Add(HtmlTextWriterStyle.MarginTop, "10px");   // dbb
            //BatchChargesGridView.Style.Add(HtmlTextWriterStyle.MarginLeft, "10px");
        }
    }
}