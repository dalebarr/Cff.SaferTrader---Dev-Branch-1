using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Core;
using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;



namespace Cff.SaferTrader.Web.Popups
{
    public partial class InvoiceBatchesPopup : Page
    {

        protected override void OnInit(EventArgs e)
        {
            CffGGV_InvoiceBatchesGridView.PageSize = 250;
            CffGGV_InvoiceBatchesGridView.AllowPaging = false;
            CffGGV_InvoiceBatchesGridView.AutoGenerateColumns = false;
            CffGGV_InvoiceBatchesGridView.SetSortExpression = "Date";

            //CffGGV_InvoiceBatchesGridView.CssClass = "cffGGVPrintReports";   //dbb
            //CffGGV_InvoiceBatchesGridView.HeaderStyle.CssClass = "cffGGV_PrintReportHeader";
            CffGGV_InvoiceBatchesGridView.HeaderStyle.CssClass = "cffGGVHeader";
            CffGGV_InvoiceBatchesGridView.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(237, 237, 237);

            CffGGV_InvoiceBatchesGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
            CffGGV_InvoiceBatchesGridView.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            CffGGV_InvoiceBatchesGridView.BorderColor = System.Drawing.Color.AliceBlue;
            CffGGV_InvoiceBatchesGridView.BorderWidth = System.Web.UI.WebControls.Unit.Point(2);

            CffGGV_InvoiceBatchesGridView.ShowHeaderWhenEmpty = true;
            CffGGV_InvoiceBatchesGridView.EmptyDataText = "No data to display";
            CffGGV_InvoiceBatchesGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            CffGGV_InvoiceBatchesGridView.ShowFooter = true;
            CffGGV_InvoiceBatchesGridView.TotalsSummarySettings.SetColumnTotals("Factored, NonFactored, AdminFee, FactorFee, Total");
            CffGGV_InvoiceBatchesGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Factored", "cffGGV_currencyCell");
            CffGGV_InvoiceBatchesGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("NonFactored", "cffGGV_currencyCell");
            CffGGV_InvoiceBatchesGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("AdminFee", "cffGGV_currencyCell");
            CffGGV_InvoiceBatchesGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("FactorFee", "cffGGV_currencyCell");
            CffGGV_InvoiceBatchesGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Total", "cffGGV_currencyCell");
            CffGGV_InvoiceBatchesGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
         
            //CffGGV_InvoiceBatchesGridView.CustomPagerSettingsMode = CffCustomPagerMode.Page | CffCustomPagerMode.Rows | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;  //dbb
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            var printable = SessionWrapper.Instance.Get.PrintBag as PrintableInvoiceBatches;

            //add grid columns here
            CffGGV_InvoiceBatchesGridView.Columns.Clear();
            if (SessionWrapper.Instance.Get.Scope == Scope.AllClientsScope)
                CffGGV_InvoiceBatchesGridView.InsertDataColumn("Client", "ClientName", CffGridViewColumnType.Text, "20%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);

            CffGGV_InvoiceBatchesGridView.InsertDataColumn("Batch", "BatchNumber", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
            CffGGV_InvoiceBatchesGridView.InsertDataColumn("Dated", "Date", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
            CffGGV_InvoiceBatchesGridView.InsertDataColumn("Released", "Released", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
            CffGGV_InvoiceBatchesGridView.InsertCurrencyColumn("Factored", "Factored", "10%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            CffGGV_InvoiceBatchesGridView.InsertCurrencyColumn("Non Factored", "NonFactored", "10%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            CffGGV_InvoiceBatchesGridView.InsertCurrencyColumn("Admin Fee", "AdminFee", "10%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            CffGGV_InvoiceBatchesGridView.InsertCurrencyColumn("Factor Fee", "FactorFee", "10%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            CffGGV_InvoiceBatchesGridView.InsertCurrencyColumn("Total", "Total", "10%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);

            if (printable != null)
            {
                ClientNameLiteral.Text =SessionWrapper.Instance.Get.ClientFromQueryString.Name;
               
                CffGGV_InvoiceBatchesGridView.DataSource = printable.InvoiceBatches;
                CffGGV_InvoiceBatchesGridView.DataBind();

                DatePrintedLiteral.Text = DateTime.Now.ToShortDateString();
            }
          
        }


    }
}