using System;
using Cff.SaferTrader.Core;

using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.Popups
{
    public partial class InvoiceBatchRepurchasesPopup : InvoiceBatchPopup
    {
        protected override void OnInit(EventArgs e)
        {
            CffGGV_RepurchasesGridView.PageSize = 250;
            CffGGV_RepurchasesGridView.AllowPaging = true;
            CffGGV_RepurchasesGridView.AutoGenerateColumns = false;
            CffGGV_RepurchasesGridView.SetSortExpression = "Dated";
            
            CffGGV_RepurchasesGridView.CssClass = "cffGGVPrintReports";
            CffGGV_RepurchasesGridView.HeaderStyle.CssClass = "cffGGV_PrintReportHeader";
            CffGGV_RepurchasesGridView.EmptyDataText = "No data to display";
            CffGGV_RepurchasesGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            CffGGV_RepurchasesGridView.ShowHeaderWhenEmpty = true;

            CffGGV_RepurchasesGridView.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(237, 237, 237);
            CffGGV_RepurchasesGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
            CffGGV_RepurchasesGridView.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            CffGGV_RepurchasesGridView.BorderColor = System.Drawing.Color.AliceBlue;
            CffGGV_RepurchasesGridView.BorderWidth = System.Web.UI.WebControls.Unit.Point(1);

            CffGGV_RepurchasesGridView.InsertDataColumn("Cust#", "CustomerNumber", CffGridViewColumnType.Text, "20%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_RepurchasesGridView.InsertDataColumn("Customer", "CustomerName", CffGridViewColumnType.Text, "20%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_RepurchasesGridView.InsertDataColumn("Transaction", "TransactionNumber", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_RepurchasesGridView.InsertDataColumn("Dated", "Dated", CffGridViewColumnType.Date, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            CffGGV_RepurchasesGridView.InsertCurrencyColumn("Amount", "Amount", "20%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            CffGGV_RepurchasesGridView.InsertDataColumn("Batch", "Batch", CffGridViewColumnType.Text, "20%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_RepurchasesGridView.InsertDataColumn("Created", "Created", CffGridViewColumnType.Date, "20%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            CffGGV_RepurchasesGridView.TotalsSummarySettings.SetColumnTotals("Amount");
            CffGGV_RepurchasesGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Amount", "cffGGV_currencyCell");

            CffGGV_RepurchasesGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            CffGGV_RepurchasesGridView.CustomPagerSettingsMode = CffCustomPagerMode.Page | CffCustomPagerMode.Rows | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var printable = SessionWrapper.Instance.Get.PrintBag as PrintableInvoiceBatchRepurchases;

            if (printable != null)
            {
                SetTitle("Prepayments", printable.InvoiceBatch);
                header.DisplayHeader(printable.InvoiceBatch);

                CffGGV_RepurchasesGridView.DataSource = printable.Repurchases;
                CffGGV_RepurchasesGridView.DataBind();

                DatePrintedLiteral.Text = DateTime.Now.ToShortDateString();
            }
        }
    }
}