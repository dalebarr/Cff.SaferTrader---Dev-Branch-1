using System;
using Cff.SaferTrader.Core;

using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.Popups
{
    public partial class PermanentCustomerNotesPopup : NotesPopup
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            lblDatePrinted.Text = "Date Printed: " + DateTime.Now.ToLongDateString();
            lblCopyRight.Text = "&copy; 2014 Cash Flow Funding Limited, 195 Main Highway Ellerslie Auckland.";

            notesGridView.AutoGenerateColumns = false;
            notesGridView.PageSize = 250;

            notesGridView.BorderWidth = Unit.Pixel(1);
            notesGridView.Width = Unit.Percentage(100);  //dbb
            //notesGridView.CssClass = "cffGGVPrintReports";
            notesGridView.HeaderStyle.CssClass = "cffGGVHeader";  // "cffGGV_PrintReportHeader";
            notesGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
            notesGridView.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(237, 237, 237);
          
            notesGridView.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            notesGridView.BorderColor = System.Drawing.Color.AliceBlue;
            notesGridView.BorderWidth = System.Web.UI.WebControls.Unit.Point(1);

            notesGridView.EmptyDataText = "No data to display";
            notesGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            notesGridView.ShowHeaderWhenEmpty = true;

            //notesGridView.ShowFooter = true;
            //notesGridView.AllowPaging = true;
            //notesGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;

            notesGridView.Columns.Clear();
            notesGridView.InsertDataColumn("Created", "Created", CffGridViewColumnType.DateTime, "10%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            notesGridView.InsertDataColumn("Created By", "createdByEmployeeName", CffGridViewColumnType.Text, "20%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            notesGridView.InsertDataColumn("Comment", "Comment", CffGridViewColumnType.Memo, "65%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left);

            //notesGridView.InsertDataColumn("Created", "Created", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left, true);
            //notesGridView.InsertDataColumn("Created By", "createdByEmployeeName", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left, true);
            //notesGridView.InsertDataColumn("Modified", "ModifiedBy", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Left, true);
            //notesGridView.InsertDataColumn("Modified By", "ModifiedByEmployeeName", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left, true);
            //notesGridView.InsertDataColumn("Comment", "Comment", CffGridViewColumnType.Memo, "50%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left);
            //notesGridView.Width = Unit.Percentage(90);

            var printable = SessionWrapper.Instance.Get.PrintBag as PrintablePermanentCustomerNotes;
            if (printable != null)
            {
                SetTitle("Customer Permanent Notes", printable.CustomerName);
                notesGridView.Caption = this.Title;
                notesGridView.DataSource = printable.Notes;
                notesGridView.DataBind();
            }
        }
    }
}