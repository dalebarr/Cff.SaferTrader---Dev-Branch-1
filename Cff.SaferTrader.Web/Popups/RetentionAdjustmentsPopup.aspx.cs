using System;
using Cff.SaferTrader.Core;

using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.Popups
{
    public partial class RetentionAdjustmentsPopup : Page
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            cffGridRetnAdjustments.PageSize = 1000;
            cffGridRetnAdjustments.AllowPaging = false; //suggested by marty
            cffGridRetnAdjustments.AutoGenerateColumns = false;
            cffGridRetnAdjustments.SetSortExpression = "Type";

            cffGridRetnAdjustments.BorderWidth = Unit.Pixel(1);
            cffGridRetnAdjustments.CssClass = "cffGGVPrintReports";
            cffGridRetnAdjustments.HeaderStyle.CssClass = "cffGGV_PrintReportHeader";
            cffGridRetnAdjustments.ShowHeaderWhenEmpty = true;
            cffGridRetnAdjustments.EmptyDataText = "No data to display";
            cffGridRetnAdjustments.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            cffGridRetnAdjustments.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(237, 237, 237);
            cffGridRetnAdjustments.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
            cffGridRetnAdjustments.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            cffGridRetnAdjustments.BorderColor = System.Drawing.Color.LightGray;
            cffGridRetnAdjustments.BorderWidth = System.Web.UI.WebControls.Unit.Point(2); // dbb

            cffGridRetnAdjustments.InsertDataColumn("Type", "Type", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            cffGridRetnAdjustments.InsertCurrencyColumn("Amount", "Amount", "10%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);

            cffGridRetnAdjustments.TotalsSummarySettings.SetColumnTotals("Amount");
            cffGridRetnAdjustments.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Amount", "cffGGV_currencyCell");

            cffGridRetnAdjustments.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var printable = SessionWrapper.Instance.Get.PrintBag as PrintableRetentionAdjustments;
            
            if (printable != null)
            {
                try
                {  //CFF-13
                    Title = string.Format("Retention Adjustment Statement - End of month: {0} {1}", printable.RetentionSchedule.EndOfMonth, printable.RetentionSchedule.ClientName);
                    if (printable.RetentionSchedule.Status.Trim().ToUpper().Equals("HELD") || printable.RetentionSchedule.Status.Trim().ToUpper().Equals("OK"))
                    {
                        RetnHeaderLiteral.Text = "Retention Statement";
                    }
                    else
                    {
                        RetnHeaderLiteral.Text = "Estimated Retention Release";
                    }


                    EOMLiteral.Text = printable.RetentionSchedule.EndOfMonth.ToString();
                    clientNameLiteral.Text = printable.RetentionSchedule.ClientName;
                    DatePrintedLiteral.Text = DateTime.Now.ToShortDateString();

                    cffGridRetnAdjustments.DataSource = printable.Charges;
                    cffGridRetnAdjustments.DataBind();
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