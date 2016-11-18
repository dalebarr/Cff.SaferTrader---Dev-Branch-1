using System;
using Cff.SaferTrader.Core;

using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.Popups
{
    public partial class InvoiceBatchInvoicesPopup : InvoiceBatchPopup
    {
        protected override void OnInit(EventArgs e)
        {
            CffGGV_InvoiceGridView.PageSize = 1000;
            CffGGV_InvoiceGridView.AutoGenerateColumns = false;
            CffGGV_InvoiceGridView.SetSortExpression = "Date";

            //CffGGV_InvoiceGridView.CssClass = "cffGGVPrintReports";
            CffGGV_InvoiceGridView.HeaderStyle.CssClass = "cffGGV_PrintReportHeader";

            CffGGV_InvoiceGridView.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(237, 237, 237);
            CffGGV_InvoiceGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
            CffGGV_InvoiceGridView.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            CffGGV_InvoiceGridView.BorderColor = System.Drawing.Color.AliceBlue;
            CffGGV_InvoiceGridView.BorderWidth = System.Web.UI.WebControls.Unit.Point(2);

            CffGGV_InvoiceGridView.ShowHeaderWhenEmpty = true;
            CffGGV_InvoiceGridView.EmptyDataText = "No data to display";
            CffGGV_InvoiceGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            CffGGV_InvoiceGridView.InsertDataColumn("Customer", "CustomerName", CffGridViewColumnType.Text, "12%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGV_leftAlignedCellHeader");
            CffGGV_InvoiceGridView.InsertDataColumn("Transaction", "TransactionNumber", CffGridViewColumnType.Text, "8%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGV_leftAlignedCellHeader");
            CffGGV_InvoiceGridView.InsertDataColumn("Reference", "Reference", CffGridViewColumnType.Text, "8%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGV_leftAlignedCellHeader");
            CffGGV_InvoiceGridView.InsertDataColumn("Processed", "FactoredDate", CffGridViewColumnType.Date, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGV_leftAlignedCellHeader");
            CffGGV_InvoiceGridView.InsertDataColumn("Date", "Date", CffGridViewColumnType.Date, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGV_leftAlignedCellHeader");
            CffGGV_InvoiceGridView.InsertCurrencyColumn("Balance", "Balance", "6%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right, "cffGGV_rightAlignedCellHeader");
            CffGGV_InvoiceGridView.InsertCurrencyColumn("Amount", "Amount", "6%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right, "cffGGV_rightAlignedCellHeader");
            CffGGV_InvoiceGridView.InsertDataColumn("Status", "TransactionStatus", CffGridViewColumnType.Date, "8%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGVHeaderLeftAgedBal2");
            CffGGV_InvoiceGridView.InsertDataColumn("Repurchased", "Repurchased", CffGridViewColumnType.Date, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGV_leftAlignedCellHeader");
            CffGGV_InvoiceGridView.InsertDataColumn("Purchase Order", "PurchaseOrder", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGV_leftAlignedCellHeader");

            CffGGV_InvoiceGridView.ShowFooter = true;
            CffGGV_InvoiceGridView.TotalsSummarySettings.SetColumnTotals("Balance,Amount");
            CffGGV_InvoiceGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Balance", "cffGGV_currencyCell");
            CffGGV_InvoiceGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            CffGGV_InvoiceGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            CffGGV_InvoiceGridView.Width = Unit.Percentage(100); //from 80 to 100 // dbb
            CffGGV_InvoiceGridView.AllowPaging = false; //as per marty's suggestions
            //CffGGV_InvoiceGridView.CustomPagerSettingsMode = CffCustomPagerMode.Page | CffCustomPagerMode.Rows | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var printable = SessionWrapper.Instance.Get.PrintBag as PrintableInvoiceBatchInvoices;

            if (printable != null)
            {
                SetTitle("Invoices", printable.InvoiceBatch);
                header.DisplayHeader(printable.InvoiceBatch);

                CffGGV_InvoiceGridView.DataSource = printable.BatchInvoices;
                CffGGV_InvoiceGridView.DataBind();

                DatePrintedLiteral.Text = DateTime.Now.ToShortDateString();
            }
        }
    }
}