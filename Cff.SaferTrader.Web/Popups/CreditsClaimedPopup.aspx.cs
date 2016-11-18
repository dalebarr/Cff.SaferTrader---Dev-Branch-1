using System;
using System.Web.UI;
using Cff.SaferTrader.Core;

using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.Popups
{
    public partial class CreditsClaimedPopup : Page
    {
        protected override void OnInit(EventArgs e)
        {
            CffGGV_CreditsClaimed.PageSize = 1000;
            CffGGV_CreditsClaimed.AutoGenerateColumns = false;
            CffGGV_CreditsClaimed.SetSortExpression = "Dated";

            CffGGV_CreditsClaimed.ShowHeaderWhenEmpty = true;
            CffGGV_CreditsClaimed.EmptyDataText = "No data to display";
            CffGGV_CreditsClaimed.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            CffGGV_CreditsClaimed.CssClass = "cffGGVPrintReports";
            CffGGV_CreditsClaimed.HeaderStyle.CssClass = "cffGGV_PrintReportHeader";
            CffGGV_CreditsClaimed.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(237, 237, 237);

            CffGGV_CreditsClaimed.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
            CffGGV_CreditsClaimed.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            CffGGV_CreditsClaimed.BorderColor = System.Drawing.Color.AliceBlue;
            CffGGV_CreditsClaimed.BorderWidth = System.Web.UI.WebControls.Unit.Point(1);

            CffGGV_CreditsClaimed.InsertDataColumn("Cust#", "CustomerNumber", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_CreditsClaimed.InsertDataColumn("Customer", "CustomerName", CffGridViewColumnType.Text, "20%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_CreditsClaimed.InsertDataColumn("Transaction", "Transaction", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_CreditsClaimed.InsertDataColumn("Dated", "Dated", CffGridViewColumnType.Date, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            CffGGV_CreditsClaimed.InsertCurrencyColumn("Amount", "Amount", "15%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            CffGGV_CreditsClaimed.InsertCurrencyColumn("Sum", "Sum", "15%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);

            CffGGV_CreditsClaimed.InsertDataColumn("Batch", "Batch", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGVHeaderLeftAgedBal2");
            CffGGV_CreditsClaimed.InsertDataColumn("Created", "Created", CffGridViewColumnType.Date, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
     
            CffGGV_CreditsClaimed.TotalsSummarySettings.SetColumnTotals("Totals,Amount,Sum");
            CffGGV_CreditsClaimed.TotalsSummarySettings.SetTotalsColumnCssStyle("Totals", "cffGGV_currencyCell");
            CffGGV_CreditsClaimed.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            CffGGV_CreditsClaimed.TotalsSummarySettings.SetTotalsColumnCssStyle("Sum", "cffGGV_currencyCell");

            CffGGV_CreditsClaimed.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            CffGGV_CreditsClaimed.AllowPaging = false; //as per marty's suggestions
            //CffGGV_CreditsClaimed.CustomPagerSettingsMode = CffCustomPagerMode.Page | CffCustomPagerMode.Rows | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
            CffGGV_CreditsClaimed.Width = Unit.Percentage(75); 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.YearLiteral.Text = DateTime.Now.Year.ToString();

            var printable = SessionWrapper.Instance.Get.PrintBag as PrintableCreditsClaimed;
            
            if (printable != null)
            {
                try
                {  //Ref: CFF-13
                   Title = string.Format("Retention Credits Claimed Statement - End of month: {0} {1}", printable.RetnSchedule.EndOfMonth, printable.RetnSchedule.ClientName);
                   if (printable.RetnSchedule.Status.Trim().ToUpper().Equals("HELD") || printable.RetnSchedule.Status.Trim().ToUpper().Equals("OK"))
                   {
                       RetnHeaderLiteral.Text = "Retention Statement";
                   }
                   else
                   {
                       RetnHeaderLiteral.Text = "Estimated Retention Release";
                   }

                   clientNameLiteral.Text = printable.RetnSchedule.ClientName.ToString();
                   EOMLiteral.Text = printable.RetnSchedule.EndOfMonth.ToString();
                   DatePrintedLiteral.Text = DateTime.Now.ToShortDateString();
                   
                   CffGGV_CreditsClaimed.DataSource = printable.ClaimedCredits;
                   CffGGV_CreditsClaimed.DataBind();
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