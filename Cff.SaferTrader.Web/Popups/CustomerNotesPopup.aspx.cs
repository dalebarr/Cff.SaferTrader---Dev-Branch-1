using System;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.Popups
{
    public partial class CustomerNotesPopup : NotesPopup
    {
        CffGenGridView notesGridView;

        protected void Page_Init(object sender, EventArgs e)
        {
            notesGridView = new CffGenGridView();
            notesGridView.PageSize = 100;
            notesGridView.Width = Unit.Percentage(100);
            notesGridView.AllowSorting = true;
            notesGridView.AllowGroupBy = true;
            notesGridView.AutoGenerateColumns = false;
            notesGridView.EnableViewState = true;
            
            //notesGridView.AllowPaging = true;
            notesGridView.SetSortExpression = "Created";

            //notesGridView.CssClass = "cffGGVPrintReports";
            notesGridView.HeaderStyle.CssClass = "cffGGVHeader";    // "cffGGV_PrintReportHeader";
            notesGridView.ShowHeaderWhenEmpty = true;
            notesGridView.EmptyDataText = "No data to display";
            notesGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            notesGridView.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(237, 237, 237);
            notesGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;
            notesGridView.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            notesGridView.BorderColor = System.Drawing.Color.AliceBlue;
            notesGridView.BorderWidth = System.Web.UI.WebControls.Unit.Point(1);

            //notesGridView.ShowFooter = true;
            //notesGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;

            notesGridViewPlaceHolder.Controls.Add(notesGridView);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            lblDatePrinted.Text = "Date Printed: " + DateTime.Now.ToLongDateString();
            lblCopyRight.Text = "&copy;" + DateTime.Now.Year.ToString() +" Cash Flow Funding Limited, 195 Main Highway Ellerslie Auckland.";
            notesGridView.Columns.Clear();

            //notesGridView.InsertDataColumn("Created", "Created", CffGridViewColumnType.Date, "10%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left, true);
            //notesGridView.InsertDataColumn("Created By", "createdByEmployeeName", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left, true);
            //notesGridView.InsertDataColumn("Modified", "ModifiedBy", CffGridViewColumnType.Date, "10%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left, true);
            //notesGridView.InsertDataColumn("Modified By", "ModifiedByEmployeeName", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left, true);
            //notesGridView.InsertDataColumn("Activity Type", "ActivityTypeName", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left, true);
            //notesGridView.InsertDataColumn("Note Type", "NoteTypeName", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left, true);
            //notesGridView.InsertDataColumn("Comment", "Comment", CffGridViewColumnType.Memo, "30%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Left);

            var printable = SessionWrapper.Instance.Get.PrintBag as PrintableCustomerNotes;
            HeaderLiteral.Text = "Customer Notes";

            if (printable != null)
            {

                if (!string.IsNullOrEmpty(printable.Reference))
                {
                    SetTitle("Current Notes", printable.CustomerName);
                    HeaderLiteral.Text = "Current Notes - (Reference -  " + printable.Reference + ")";
                }
                else {

                    if (printable.ClientName == null)
                        SetTitle("Customer Notes", printable.CustomerName);
                    else
                        SetTitle("Customer Notes", printable.ClientName);

                    //if (SessionWrapper.Instance.Get.IsCustomerSelected)   //dbb
                    //    SetTitle("Customer Notes", printable.CustomerName);
                    //else
                    //    SetTitle("Customer Notes", printable.ClientName);
                }

                try
                {
                    if (printable.IsGroupByCustomerName)
                    {
                        HeaderLiteral.Text = "All Customer Notes";
                        SetTitle("", "");
                        HeaderLiteral.Text += "  For " + printable.ClientName;
                        //notesGridView.GroupBySettings.GroupByExpression = "Customer";
                        notesGridView.SetSortExpression = "CustomerName";
                        notesGridView.SetSortDirection = SortDirection.Ascending;
                        notesGridView.GroupBySettings.GroupByExpression = "CustomerName";
                        notesGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "10%", "cffGGV_leftAlignedCell");
                    }
                    else
                    {
                        notesGridView.Caption = this.Title;                        
                    }

                    notesGridView.InsertDataColumn("Created", "Created", CffGridViewColumnType.DateTime, "12%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center);
                    notesGridView.InsertDataColumn("Created By", "createdByEmployeeName", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Center, System.Web.UI.WebControls.HorizontalAlign.Center);
                    notesGridView.InsertDataColumn("Comment", "Comment", CffGridViewColumnType.Memo, "60%", "cffGGV_leftAlignedCell", System.Web.UI.WebControls.HorizontalAlign.Left, System.Web.UI.WebControls.HorizontalAlign.Center);

                    notesGridView.DataSource = printable.Notes;
                    notesGridView.DataBind();
                }
                catch { }
            }
        }
    }
}