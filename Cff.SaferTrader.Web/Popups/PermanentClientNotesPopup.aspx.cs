using System;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cff.SaferTrader.Web.Popups
{
    public partial class PermanentClientNotesPopup : NotesPopup
    {
        CffGenGridView notesGridView;

        protected void Page_Init(object sender, EventArgs e)
        {
            notesGridView = new CffGenGridView();
            notesGridView.PageSize = 250;
            notesGridView.AllowSorting = true;
            notesGridView.AutoGenerateColumns = false;
            notesGridView.EnableViewState = true;

            notesGridView.BorderWidth = Unit.Pixel(1);
            notesGridView.CssClass = "cffGGVPrintReports";
            notesGridView.HeaderStyle.CssClass = "cffGGV_PrintReportHeader";

            notesGridView.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(237, 237, 237);
            notesGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
            notesGridView.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            notesGridView.BorderColor = System.Drawing.Color.AliceBlue;
            notesGridView.BorderWidth = System.Web.UI.WebControls.Unit.Point(1);

            notesGridView.ShowFooter = true;
            notesGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
            notesGridView.Width = Unit.Percentage(55);

            notesGridViewPlaceHolder.Controls.Add(notesGridView);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblDatePrinted.Text = "Date Printed: " + DateTime.Now.ToLongDateString();
            lblCopyRight.Text = "&copy;2014 Cash Flow Funding Limited, 195 Main Highway Ellerslie Auckland.";

            notesGridView.InsertDataColumn("Created", "Created", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left, true);
            notesGridView.InsertDataColumn("Created By", "createdByEmployeeName", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left, true);
            notesGridView.InsertDataColumn("Modified", "ModifiedBy", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left, true);
            notesGridView.InsertDataColumn("Modified By", "ModifiedByEmployeeName", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left, true);
            notesGridView.InsertDataColumn("Comment", "Comment", CffGridViewColumnType.Memo, "25%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left);
  

            var printable = SessionWrapper.Instance.Get.PrintBag as PrintablePermanentClientNotes;
            if (printable != null)
            {
                this.ClientNameLabel.Text = printable.ClientName;
                SetTitle("Client Permanent Notes", printable.ClientName);
                notesGridView.Caption = this.Title;
                notesGridView.DataSource = printable.Notes;
                notesGridView.DataBind();
            }
        }
    }
}
