using System;
using Cff.SaferTrader.Core;

using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;



namespace Cff.SaferTrader.Web.Popups
{
    public partial class RetnAccountTransactionsPopup : Page
    {
        protected override void OnInit(EventArgs e)
        {
            CffGGV_GridAccountTransactions.PageSize = 1000;
            CffGGV_GridAccountTransactions.AutoGenerateColumns = false;

            CffGGV_GridAccountTransactions.BorderWidth = Unit.Pixel(1);
            CffGGV_GridAccountTransactions.CssClass = "cffGGVPrintReports";
            CffGGV_GridAccountTransactions.HeaderStyle.CssClass = "cffGGV_PrintReportHeader";

            CffGGV_GridAccountTransactions.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(237, 237, 237);
            CffGGV_GridAccountTransactions.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
            CffGGV_GridAccountTransactions.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            CffGGV_GridAccountTransactions.BorderColor = System.Drawing.Color.LightGray;
            CffGGV_GridAccountTransactions.BorderWidth = System.Web.UI.WebControls.Unit.Point(1);  // dbb

            CffGGV_GridAccountTransactions.EmptyDataText = "No data to display";
            CffGGV_GridAccountTransactions.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            CffGGV_GridAccountTransactions.ShowHeaderWhenEmpty = true;

            CffGGV_GridAccountTransactions.ShowFooter = true;
            CffGGV_GridAccountTransactions.TotalsSummarySettings.SetColumnTotals("Debit,Credit");
            CffGGV_GridAccountTransactions.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Debit", "cffGGV_rightAlignedCell");
            CffGGV_GridAccountTransactions.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Credit", "cffGGV_rightAlignedCell");
            CffGGV_GridAccountTransactions.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            CffGGV_GridAccountTransactions.Width = Unit.Percentage(100);
            CffGGV_GridAccountTransactions.AllowPaging = false; //removed as not necessary here
            //CffGGV_GridAccountTransactions.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            CffGGV_GridAccountTransactions.InsertDataColumn("Reference", "TranRef", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGV_leftAlignedCellHeader");
            CffGGV_GridAccountTransactions.InsertDataColumn("TransDate", "TranDate", CffGridViewColumnType.Date, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGV_leftAlignedCellHeader");
            CffGGV_GridAccountTransactions.InsertDataColumn("TransType", "TranType", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGV_leftAlignedCellHeader");
            CffGGV_GridAccountTransactions.InsertCurrencyColumn("Debit", "Debit", "15%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right, "cffGGV_rightAlignedCellHeader");
            CffGGV_GridAccountTransactions.InsertCurrencyColumn("Credit", "Credit", "15%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right, "cffGGV_rightAlignedCellHeader");
            CffGGV_GridAccountTransactions.InsertDataColumn("Description", "Description", CffGridViewColumnType.Text, "25%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true, "cffGGV_centerAlignedCellHeader");

            var printable = SessionWrapper.Instance.Get.PrintBag as PrintableAccountTransactions;
            if (printable != null) {
                try
                {
                    Title = string.Format("Printable Account Transactions - End of month: {0} {1}", printable.RetentionSchedule.EndOfMonth, printable.RetentionSchedule.ClientName);
                    EOMLiteral.Text = printable.RetentionSchedule.EndOfMonth.ToString();
                    clientNameLiteral.Text = printable.RetentionSchedule.ClientName;
                    DatePrintedLiteral.Text = DateTime.Now.ToShortDateString();
                    YearLiteral.Text = DateTime.Now.Year.ToString();
                    ClosingBalanceLiteral.Text = printable.ClosingBalance.ToString("C");
                    MovementLiteral.Text = printable.Movement.ToString("C");

                    CffGGV_GridAccountTransactions.DataSource = printable.AccountTransReportRecords;
                    CffGGV_GridAccountTransactions.DataBind();
                   
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
