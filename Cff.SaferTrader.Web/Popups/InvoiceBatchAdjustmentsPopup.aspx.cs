using System;
using Cff.SaferTrader.Core;

using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;


namespace Cff.SaferTrader.Web.Popups
{
    public partial class InvoiceBatchAdjustmentsPopup : InvoiceBatchPopup
    {
        protected override void OnInit(EventArgs e)
        {
            CffGGV_BatchChargesGridView.PageSize = 1000;
            CffGGV_BatchChargesGridView.AutoGenerateColumns = false;
            CffGGV_BatchChargesGridView.SetSortExpression = "Date";

            CffGGV_BatchChargesGridView.CssClass = "cffGGVPrintReports";
            CffGGV_BatchChargesGridView.HeaderStyle.CssClass = "cffGGV_PrintReportHeader";
            CffGGV_BatchChargesGridView.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(237, 237, 237);
            
            CffGGV_BatchChargesGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
            CffGGV_BatchChargesGridView.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            CffGGV_BatchChargesGridView.BorderColor = System.Drawing.Color.AliceBlue;
            CffGGV_BatchChargesGridView.BorderWidth = System.Web.UI.WebControls.Unit.Point(1);

            CffGGV_BatchChargesGridView.ShowHeaderWhenEmpty = true;
            CffGGV_BatchChargesGridView.EmptyDataText = "No data to display";
            CffGGV_BatchChargesGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
        
            CffGGV_BatchChargesGridView.InsertDataColumn("Type", "Type", CffGridViewColumnType.Text, "20%", "cffGGV_leftAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            CffGGV_BatchChargesGridView.InsertDataColumn("Description", "Description", CffGridViewColumnType.Text, "20%", "cffGGV_leftAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            CffGGV_BatchChargesGridView.InsertCurrencyColumn("Amount", "Amount", "20%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
           
            CffGGV_BatchChargesGridView.TotalsSummarySettings.SetColumnTotals("Amount");
            CffGGV_BatchChargesGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            CffGGV_BatchChargesGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            CffGGV_BatchChargesGridView.AllowPaging = false; //removed from report - not needed
            //CffGGV_BatchChargesGridView.CustomPagerSettingsMode = CffCustomPagerMode.Page | CffCustomPagerMode.Rows | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var printable = SessionWrapper.Instance.Get.PrintBag as PrintableInvoiceBatchCharges;

            if (printable != null)
            {
                SetTitle("Adjustments", printable.InvoiceBatch);
                header.DisplayHeader(printable.InvoiceBatch);

                CffGGV_BatchChargesGridView.DataSource = printable.BatchCharges;
                CffGGV_BatchChargesGridView.DataBind();

                DatePrintedLiteral.Text = DateTime.Now.ToShortDateString();
            }
        }
    }
}