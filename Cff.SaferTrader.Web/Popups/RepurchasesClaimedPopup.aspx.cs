using System;
using Cff.SaferTrader.Core;

using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.Popups
{
    public partial class RepurchasesClaimedPopup : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CffGridRepurchasesClaimed.PageSize = 1000;
            CffGridRepurchasesClaimed.AllowPaging = false; //suggested by marty
            CffGridRepurchasesClaimed.AutoGenerateColumns = false;
            CffGridRepurchasesClaimed.SetSortExpression = "Dated";
            
            CffGridRepurchasesClaimed.BorderWidth = Unit.Pixel(1);
            CffGridRepurchasesClaimed.CssClass = "cffGGVPrintReports";
            CffGridRepurchasesClaimed.HeaderStyle.CssClass = "cffGGV_PrintReportHeader";
            CffGridRepurchasesClaimed.EmptyDataText = "No data to display";
            CffGridRepurchasesClaimed.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            CffGridRepurchasesClaimed.ShowHeaderWhenEmpty = true;

            CffGridRepurchasesClaimed.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(237, 237, 237);
            CffGridRepurchasesClaimed.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
            CffGridRepurchasesClaimed.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            CffGridRepurchasesClaimed.BorderColor = System.Drawing.Color.AliceBlue;
            CffGridRepurchasesClaimed.BorderWidth = System.Web.UI.WebControls.Unit.Point(1);

            CffGridRepurchasesClaimed.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGridRepurchasesClaimed.InsertDataColumn("Customer", "CustomerName", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGridRepurchasesClaimed.InsertDataColumn("Transaction", "Transaction", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGridRepurchasesClaimed.InsertDataColumn("Dated", "Dated", CffGridViewColumnType.Date, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            CffGridRepurchasesClaimed.InsertCurrencyColumn("Amount", "Amount", "10%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right, "cffGGVHeaderRight");

            CffGridRepurchasesClaimed.InsertDataColumn("Batch", "Batch", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGVHeaderLeftAgedBal2");
            CffGridRepurchasesClaimed.InsertDataColumn("Created", "Created", CffGridViewColumnType.Date, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            CffGridRepurchasesClaimed.TotalsSummarySettings.SetColumnTotals("Amount");
            CffGridRepurchasesClaimed.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Amount", "cffGGV_currencyCell");

            CffGridRepurchasesClaimed.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            CffGridRepurchasesClaimed.Width = Unit.Percentage(70);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var printable = SessionWrapper.Instance.Get.PrintBag as PrintableRepurchasesClaimed;
            if (printable != null)
            {
                try
                {  //Ref: CFF-13

                    Title = string.Format("Retention Prepayments Claimed Statement - End of month: {0} {1}",
                            printable.RetnSchedule.EndOfMonth, printable.RetnSchedule.ClientName);
                    EOMLiteral.Text = printable.RetnSchedule.EndOfMonth.ToString();
                    
                    clientNameLiteral.Text = printable.RetnSchedule.ClientName.ToString();
                    DatePrintedLiteral.Text = DateTime.Now.ToShortDateString();
                    YearLiteral.Text = DateTime.Now.Year.ToString();

                    if (printable.RetnSchedule.Status.Trim().ToUpper().Equals("HELD") || printable.RetnSchedule.Status.Trim().ToUpper().Equals("OK"))
                    {
                        RetnHeaderLiteral.Text = "Retention Statement";
                    }
                    else
                    {
                        RetnHeaderLiteral.Text = "Estimated Retention Release";
                    }

                    CffGridRepurchasesClaimed.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                    CffGridRepurchasesClaimed.GridLines = System.Web.UI.WebControls.GridLines.None;

                    CffGridRepurchasesClaimed.DataSource = printable.RepurchasesClaimed;
                    CffGridRepurchasesClaimed.DataBind();
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