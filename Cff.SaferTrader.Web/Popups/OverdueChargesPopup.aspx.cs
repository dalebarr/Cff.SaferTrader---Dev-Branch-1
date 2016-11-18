using System;
using Cff.SaferTrader.Core;

using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;


namespace Cff.SaferTrader.Web.Popups
{
    public partial class OverdueChargesPopup : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CffGridOverdueCharges.PageSize = 1000;
            CffGridOverdueCharges.AllowPaging = false; //as per marty's suggestions
            CffGridOverdueCharges.AutoGenerateColumns = false;
            CffGridOverdueCharges.SetSortExpression = "Processed";

            CffGridOverdueCharges.CssClass = "cffGGVPrintReports";
            CffGridOverdueCharges.HeaderStyle.CssClass = "cffGGV_PrintReportHeader";
            CffGridOverdueCharges.ShowHeaderWhenEmpty = true;
            CffGridOverdueCharges.EmptyDataText = "No data to display";
            CffGridOverdueCharges.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            CffGridOverdueCharges.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
            CffGridOverdueCharges.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            CffGridOverdueCharges.BorderColor = System.Drawing.Color.AliceBlue;
            CffGridOverdueCharges.BorderWidth = System.Web.UI.WebControls.Unit.Point(1);
            CffGridOverdueCharges.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(237, 237, 237);

            CffGridOverdueCharges.InsertDataColumn("Cust#", "CustomerNumber", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGridOverdueCharges.InsertDataColumn("Customer", "CustomerName", CffGridViewColumnType.Text, "12%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGridOverdueCharges.InsertDataColumn("Title", "Title", CffGridViewColumnType.Text, "8%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGridOverdueCharges.InsertDataColumn("Processed", "Factored", CffGridViewColumnType.Date, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true, "cffGGV_centerAlignedCellHeader");
            CffGridOverdueCharges.InsertDataColumn("Age", "Age", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true, "cffGGV_centerAlignedCellHeader");
            CffGridOverdueCharges.InsertDataColumn("Number", "Number", CffGridViewColumnType.Text, "8%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGridOverdueCharges.InsertDataColumn("Reference", "Reference", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            CffGridOverdueCharges.InsertCurrencyColumn("Charges", "Charges", "8%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right, "cffGGV_rightAlignedCellHeader");
            CffGridOverdueCharges.InsertCurrencyColumn("Charges With GST", "ChargesWithGst", "8%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right, "cffGGV_rightAlignedCellHeader");
            CffGridOverdueCharges.InsertCurrencyColumn("Amount", "Amount", "12%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right, "cffGGV_rightAlignedCellHeader");
            CffGridOverdueCharges.InsertCurrencyColumn("Balance", "Balance", "12%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right, "cffGGV_rightAlignedCellHeader");

            CffGridOverdueCharges.InsertDataColumn("Status", "Status", CffGridViewColumnType.Text, "12%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
          
            CffGridOverdueCharges.TotalsSummarySettings.SetColumnTotals("Charges,ChargesWithGst,Amount,Balance");
            CffGridOverdueCharges.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Charges", "cffGGV_currencyCell");
            CffGridOverdueCharges.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("ChargesWithGst", "cffGGV_currencyCell");
            CffGridOverdueCharges.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Amount", "cffGGV_currencyCell");
            CffGridOverdueCharges.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Balance", "cffGGV_currencyCell");

            CffGridOverdueCharges.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            //CffGridOverdueCharges.CustomPagerSettingsMode = CffCustomPagerMode.Page | CffCustomPagerMode.Rows | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
            CffGridOverdueCharges.Width = Unit.Percentage(100);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            var printable = SessionWrapper.Instance.Get.PrintBag as PrintableOverdueCharges;
            
            if (printable != null)
            {
                try
                {  //Ref: CFF-13 
                    Title = string.Format("Retention Interest & Charges Statement - End of month: {0} {1}",
                            printable.rtnSchedule.EndOfMonth, printable.rtnSchedule.ClientName);
                    EOMLiteral.Text = printable.rtnSchedule.EndOfMonth.ToString();
                    clientNameLiteral.Text = printable.rtnSchedule.ClientName.ToString();
                    if (printable.rtnSchedule.Status.Trim().ToUpper().Equals("HELD") || printable.rtnSchedule.Status.Trim().ToUpper().Equals("OK"))
                    {
                        RetnHeaderLiteral.Text = "Retention Statement";
                    }
                    else
                    {
                        RetnHeaderLiteral.Text = "Estimated Retention Release";
                    }
                    DatePrintedLiteral.Text = DateTime.Now.ToShortDateString();
                    YearLiteral.Text = DateTime.Now.Year.ToString();
                    CffGridOverdueCharges.DataSource = printable.ODCReportRecords;
                    CffGridOverdueCharges.DataBind();
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