using System;
using Cff.SaferTrader.Core;

using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.Popups
{
    public partial class ClientNotesPopup : NotesPopup
    {

        CffGenGridView notesGridView;

        protected void Page_Init(object sender, EventArgs e)
        {
            notesGridView = new CffGenGridView();
            notesGridView.PageSize = 250;
            notesGridView.AllowSorting = true;
            notesGridView.AutoGenerateColumns = false;
            notesGridView.EnableViewState = true;

            //notesGridView.AllowPaging = true;
            notesGridView.SetSortExpression = "Created";
            notesGridView.Width = Unit.Percentage(100);
            //notesGridView.AllowPaging = true;
            //notesGridView.CssClass = "cffGGVPrintReports";
            notesGridView.HeaderStyle.CssClass = "cffGGVHeader";   //"cffGGV_PrintReportHeader";
            notesGridView.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(237, 237, 237);
            notesGridView.RowCssClass = "dxgvDataRow";

            notesGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
            notesGridView.ShowHeaderWhenEmpty = true;
            notesGridView.EmptyDataText = "No data to display";
            notesGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            notesGridView.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            notesGridView.BorderColor = System.Drawing.Color.AliceBlue;
            notesGridView.BorderWidth = System.Web.UI.WebControls.Unit.Point(1);

            //notesGridView.ShowFooter = false;   // dbb
            //notesGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
            notesGridViewPlaceHolder.Controls.Add(notesGridView);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblDatePrinted.Text = "Date Printed: " + DateTime.Now.ToLongDateString();
            lblCopyRight.Text = "&copy; " + DateTime.Now.Year.ToString() +" CashFlow Funding Limited, 195 Main Highway Ellerslie, Auckland.";

            notesGridView.Columns.Clear();

            notesGridView.InsertDataColumn("Created", "Created", CffGridViewColumnType.DateTime, "15%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            notesGridView.InsertDataColumn("Created By", "createdByEmployeeName", CffGridViewColumnType.Text, "15%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center, true);
            notesGridView.InsertDataColumn("Comment", "Comment", CffGridViewColumnType.Memo, "70%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left);
            //notesGridView.Width = Unit.Percentage(90);

            var printable = SessionWrapper.Instance.Get.PrintBag as PrintableClientNotes;

            if (printable != null)
            {
                SetTitle("Client Notes", printable.ClientName);
                HeaderLiteral.Text = "Client Notes for " + printable.ClientName;
                notesGridView.Caption = this.Title;
                notesGridView.DataSource = printable.Notes;
                notesGridView.DataBind();
            }
        }
    }
}