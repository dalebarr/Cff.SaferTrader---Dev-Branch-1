using System;
using Cff.SaferTrader.Core;

using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;


namespace Cff.SaferTrader.Web.Popups
{
    public partial class RetentionLikelyRepurchasesPopup : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CffGGV_LikelyRepurchasesGridView.PageSize = 1000;
            CffGGV_LikelyRepurchasesGridView.AllowPaging = false; //suggested by marty
            CffGGV_LikelyRepurchasesGridView.AutoGenerateColumns = false;
            CffGGV_LikelyRepurchasesGridView.SetSortExpression = "Dated";

            CffGGV_LikelyRepurchasesGridView.BorderWidth = Unit.Pixel(1); //dbb
            CffGGV_LikelyRepurchasesGridView.CssClass = "cffGGVPrintReports";
            CffGGV_LikelyRepurchasesGridView.HeaderStyle.CssClass = "cffGGV_PrintReportHeader"; 
            CffGGV_LikelyRepurchasesGridView.EmptyDataText = "No data to display";
            CffGGV_LikelyRepurchasesGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            CffGGV_LikelyRepurchasesGridView.ShowHeaderWhenEmpty = true;

            CffGGV_LikelyRepurchasesGridView.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(237, 237, 237);
            CffGGV_LikelyRepurchasesGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
            CffGGV_LikelyRepurchasesGridView.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            CffGGV_LikelyRepurchasesGridView.BorderColor = System.Drawing.Color.LightGray;
            CffGGV_LikelyRepurchasesGridView.BorderWidth = System.Web.UI.WebControls.Unit.Point(3);  // dbb

            CffGGV_LikelyRepurchasesGridView.InsertDataColumn("Customer", "CustomerName", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_LikelyRepurchasesGridView.InsertDataColumn("Title", "Title", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_LikelyRepurchasesGridView.InsertDataColumn("Age", "Age", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            CffGGV_LikelyRepurchasesGridView.InsertCurrencyColumn("Amount", "Amount", "10%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right, "cffGGV_rightAlignedCellHeader");
            CffGGV_LikelyRepurchasesGridView.InsertCurrencyColumn("Balance", "Balance", "10%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right, "cffGGV_rightAlignedCellHeader");
            CffGGV_LikelyRepurchasesGridView.InsertCurrencyColumn("Sum", "Sum", "10%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right, "cffGGV_rightAlignedCellHeader");

            CffGGV_LikelyRepurchasesGridView.InsertDataColumn("Dated", "Dated", CffGridViewColumnType.Date, "10%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGVHeaderLeftAgedBal2");
            CffGGV_LikelyRepurchasesGridView.InsertDataColumn("Processed", "Processed", CffGridViewColumnType.Text, "10%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            CffGGV_LikelyRepurchasesGridView.InsertDataColumn("Transaction", "Transaction", CffGridViewColumnType.Text, "10%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            CffGGV_LikelyRepurchasesGridView.InsertDataColumn("Reference", "Reference", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            CffGGV_LikelyRepurchasesGridView.ShowFooter = true;
            CffGGV_LikelyRepurchasesGridView.TotalsSummarySettings.SetColumnTotals("Amount,Balance,Sum");
            CffGGV_LikelyRepurchasesGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Amount", "cffGGV_rightAlignedCell");
            CffGGV_LikelyRepurchasesGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Balance", "cffGGV_rightAlignedCell");
            CffGGV_LikelyRepurchasesGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Sum", "cffGGV_rightAlignedCell");
            CffGGV_LikelyRepurchasesGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            //CffGGV_LikelyRepurchasesGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var printable = SessionWrapper.Instance.Get.PrintBag as PrintableLikelyRepurchases;
            if (printable != null)
            {
                try
                {  
                    Title = string.Format("Retention Likely Repurchases Statement - End of month: {0} {1}", printable.RetentionSchedule.EndOfMonth, printable.RetentionSchedule.ClientName);
                    EOMLiteral.Text = printable.RetentionSchedule.EndOfMonth.ToString();
                    clientNameLiteral.Text = printable.RetentionSchedule.ClientName;
                    DatePrintedLiteral.Text = DateTime.Now.ToShortDateString();

                    CffGGV_LikelyRepurchasesGridView.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                    CffGGV_LikelyRepurchasesGridView.GridLines = System.Web.UI.WebControls.GridLines.None;

                    CffGGV_LikelyRepurchasesGridView.DataSource = printable.RepurchasesLineList;
                    CffGGV_LikelyRepurchasesGridView.DataBind();
            
                }
                catch (Exception ex)
                {
                    EOMLiteral.Text = ex.Message.ToString();
                    clientNameLiteral.Text = ex.Message.ToString();
                }
            }
        }
    }
}
