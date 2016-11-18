using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Core;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.Popups
{
    public partial class InvoiceBatchCreditsPopup : InvoiceBatchPopup
    {
        protected override void OnInit(EventArgs e)
        {
            CffGGV_ChargesGridView.PageSize = 1000;
            CffGGV_ChargesGridView.AllowPaging = false; //as per marty's suggestions
            CffGGV_ChargesGridView.AutoGenerateColumns = false;
            CffGGV_ChargesGridView.SetSortExpression = "Dated";
            
            CffGGV_ChargesGridView.CssClass = "cffGGVPrintReports";
            CffGGV_ChargesGridView.HeaderStyle.CssClass = "cffGGV_PrintReportHeader";
            CffGGV_ChargesGridView.EmptyDataText = "No data to display";
            CffGGV_ChargesGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            CffGGV_ChargesGridView.ShowHeaderWhenEmpty = true;

            CffGGV_ChargesGridView.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(237, 237, 237);
            CffGGV_ChargesGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
            CffGGV_ChargesGridView.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            CffGGV_ChargesGridView.BorderColor = System.Drawing.Color.AliceBlue;
            CffGGV_ChargesGridView.BorderWidth = System.Web.UI.WebControls.Unit.Point(1);
            
            CffGGV_ChargesGridView.InsertDataColumn("Cust#", "CustomerNumber", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_ChargesGridView.InsertDataColumn("Customer", "CustomerName", CffGridViewColumnType.Text, "30%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_ChargesGridView.InsertDataColumn("Dated", "Dated", CffGridViewColumnType.Date, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_ChargesGridView.InsertCurrencyColumn("Amount", "Amount", "15%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right, "cffGGV_rightAlignedCellHeader");
            CffGGV_ChargesGridView.InsertDataColumn("Batch", "Batch", CffGridViewColumnType.Date, "15%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true, "cffGGV_centerAlignedCellHeader");
            
            CffGGV_ChargesGridView.TotalsSummarySettings.SetColumnTotals("Amount");
            CffGGV_ChargesGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            CffGGV_ChargesGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            CffGGV_ChargesGridView.Width = Unit.Percentage(100);

            //CffGGV_ChargesGridView.CustomPagerSettingsMode = CffCustomPagerMode.Page | CffCustomPagerMode.Rows | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var printable = SessionWrapper.Instance.Get.PrintBag as PrintableInvoiceBatchCredits;
            if (printable != null)
            {
                SetTitle("Credits", printable.InvoiceBatch);
                header.DisplayHeader(printable.InvoiceBatch);

                CffGGV_ChargesGridView.DataSource = printable.Credits;
                CffGGV_ChargesGridView.DataBind();

                DatePrintedLiteral.Text = DateTime.Now.ToShortDateString();
            }
        }

    }

}