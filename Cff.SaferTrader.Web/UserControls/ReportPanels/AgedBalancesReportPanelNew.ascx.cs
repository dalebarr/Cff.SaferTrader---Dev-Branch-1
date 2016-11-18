using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Web.Reports;
using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;


namespace Cff.SaferTrader.Web.UserControls.ReportPanels
{
    public partial class AgedBalancesReportPanelNew : ExportableGridPanel
    {
        public int FocusedRowIndex;
        public int PageIndex;

        CffGenGridView reportGridView;      //main grid

        public bool isReportWithNotes
        {
            get
            {
                object o = ViewState["isReportWithNotes"];
                return (o == null ? false : (bool)o);
            }
            set
            {
                ViewState["isReportWithNotes"] = value;
            }
        }

        public bool isReportWithNotesInitialized
        {
            get
            {
                object o = ViewState["isReportWithNotesInitialized"];
                return (o == null ? false : (bool)o);
            }
            set
            {
                ViewState["isReportWithNotesInitialized"] = value;
            }
        }

        public GridNestingState parentGridNestingState {
            get
            {
                object o = ViewState["ParentNestingState"];
                return (o == null ? GridNestingState.None : (GridNestingState)o);
            }
            set
            {
                ViewState["ParentNestingState"] = value;
            }
        }

     
        protected override void OnInit(EventArgs e) 
        {
            base.OnInit(e);
        
            initReportGridView();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Display(ViewState["AgedBalancesReport"] as AgedBalancesReport);
            }
        }

       
        protected void initReportGridView()
        {
            //start init parent grid
            reportGridView = new CffGenGridView();
            reportGridView.ID = "reportGridView";
            reportGridView.PageSize = 250;
            reportGridView.DefaultPageSize = 250;
            reportGridView.Width = Unit.Percentage(100);
            reportGridView.BorderWidth = Unit.Pixel(1);
            reportGridView.AllowSorting = true;
      
            reportGridView.SetSortExpression = "CustomerName";
            reportGridView.AutoGenerateColumns = false;
            reportGridView.ShowHeaderWhenEmpty = true;
            reportGridView.EmptyDataText = "No data to display";
            reportGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            reportGridView.FooterStyle.CssClass = "dxgvFooter";
            reportGridView.EnableViewState = true;

            //reportGridView.CssClass = "cffGGV";
            reportGridView.HeaderStyle.CssClass = "cffGGVHeader";
            reportGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            
            reportGridView.ShowFooter = true;
            reportGridView.TotalsSummarySettings.SetColumnTotals("CurrentBalance, MonthOldBalance, TwoMonthsOldBalance, ThreeMonthsOrOlderBalance, Balance");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("CurrentBalance", "cffGGV_currencyCell");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("MonthOldBalance", "cffGGV_currencyCell");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("TwoMonthsOldBalance", "cffGGV_currencyCell");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("ThreeMonthsOrOlderBalance", "cffGGV_currencyCell");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Balance", "cffGGV_currencyCell");

            reportGridView.NestedSettings.Enabled = false;
            reportGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
            reportGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;

            reportGridView.AllowPaging = true; 
            reportGridView.CustomPagerSettingsMode = CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom | CffCustomPagerMode.Rows;

            reportPlaceholder.Controls.Clear();
            reportPlaceholder.Controls.Add(reportGridView);
        }


        private void InsertCommonColumns()
        {
            if (isReportWithNotes)
            {
                if (!this.isReportWithNotesInitialized)
                {
                    this.reportPlaceholder.Visible = false;
                    IsReportWithNotesLiteral.Text = "true";
                    IsReportWithNotesLiteral.Visible = false;
                }
           }
           else {
               this.reportPlaceholder.Visible = true;
               reportGridView.NestedSettings.Enabled = false;

               reportGridView.Columns.Clear();
               reportGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "8%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
               reportGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "8%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
               reportGridView.InsertCurrencyColumn("Current", "CurrentBalance", "10%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
               reportGridView.InsertCurrencyColumn("Month 1", "MonthOldBalance", "10%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
               reportGridView.InsertCurrencyColumn("Month 2", "TwoMonthsOldBalance", "10%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
               reportGridView.InsertCurrencyColumn("Month 3+", "ThreeMonthsOrOlderBalance", "10%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
               reportGridView.InsertCurrencyColumn("Balance", "Balance", "10%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
               reportGridView.InsertDataColumn("Next Call", "NextCallDate", CffGridViewColumnType.Text, "8%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
               reportGridView.InsertDataColumn("Email", "Email", CffGridViewColumnType.Text, "15%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
               reportGridView.InsertDataColumn("Contact", "Contact", CffGridViewColumnType.Text, "12%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
               reportGridView.InsertDataColumn("Phone", "Phone", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
               //reportGridView.InsertDataColumn("Cell", "Cell", CffGridViewColumnType.Text, "12%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGVHeaderLeftAgedBal2");
               reportGridView.InsertDataColumn("Cell", "Cell", CffGridViewColumnType.Text, "12%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true);               

               this.reportPlaceholder.Controls.Clear();
               this.reportPlaceholder.Controls.Add(reportGridView);
          }

        }

        public void Display(AgedBalancesReport report)
        {
            ViewState.Add("AgedBalancesReport", report);
            if (report != null)
            {
                if (this.isReportWithNotes)
                {
                }
                else
                {
                    reportGridView.DataSource = report.Records;
                    reportGridView.DataBind();

                    if (reportGridView.AllowSorting)
                        reportGridView.Sort("CustomerName", SortDirection.Ascending);
                }
            }
        }

        
        public void ConfigureAllClientsGridColumns()
        {
            if (reportGridView == null)
                initReportGridView();

            reportGridView.Columns.Clear();

            if (!this.isReportWithNotesInitialized || !this.isReportWithNotes) {
                reportGridView.InsertDataColumn("#", "ClientNumberLabel", CffGridViewColumnType.Text, "4%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
                reportGridView.InsertBoundHyperLinkColumn("Client", "ClientName", "ClientNumber", "12%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Center);
            }

            InsertCommonColumns();
        }

        public void ConfigureClientGridColumns()
        {
            if (!this.isReportWithNotesInitialized || !this.isReportWithNotes)
                reportGridView.Columns.Clear();
            
            InsertCommonColumns();
        }

        public void Clear()
        {
            ViewState.Add("AgedBalancesReport", null);
            reportGridView.DataSource = null;
            reportGridView.DataBind();
        }

        public override void Export()
        {
            AgedBalancesReport report = ViewState["AgedBalancesReport"] as AgedBalancesReport;
            System.Collections.Generic.IList<AgedBalancesReportRecord> exportRecordWithNotes = new System.Collections.Generic.List<AgedBalancesReportRecord>();

            foreach (AgedBalancesReportRecord rptRecord in report.Records)
            {
                System.Collections.Generic.IList<CustomerNote> cnList = rptRecord.CustNoteList;
                rptRecord.Note = "";
                foreach (CustomerNote cNote in cnList)
                {
                    if (!string.IsNullOrEmpty(cNote.Comment))
                    {
                        rptRecord.Note += "[" + cNote.CreatedByEmployeeName + "][" + cNote.Created.ToShortDateString() + "]";
                        rptRecord.Note += cNote.Comment + System.Environment.NewLine;
                    }
                }

                if (!string.IsNullOrEmpty(rptRecord.Note))
                {
                    rptRecord.CustNoteList.Clear();
                }

                exportRecordWithNotes.Add(rptRecord);
            }

            //init columns
            CffGenGridView exportGrid = this.reportGridView;
            if (this.isReportWithNotes)
            {
                exportGrid.AllowSorting = true;
                exportGrid.SetSortExpression = "CustomerName";
                exportGrid.InsertDataColumn("Customer", "CustomerName");
                exportGrid.InsertCurrencyColumn("Current", "CurrentBalance");
                exportGrid.InsertCurrencyColumn("Month 1", "MonthOldBalance");
                exportGrid.InsertCurrencyColumn("Month 2", "TwoMonthsOldBalance");
                exportGrid.InsertCurrencyColumn("Month 3+", "ThreeMonthsOrOlderBalance");
                exportGrid.InsertCurrencyColumn("Balance", "Balance") ;
                exportGrid.InsertDataColumn("Next Call", "NextCallDate", CffGridViewColumnType.Date, "10%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
                exportGrid.InsertDataColumn("Note", "Note");
                exportGrid.Visible = false;

                exportGrid.InsertDataColumn("Email", "Email");
                exportGrid.InsertDataColumn("Contact", "Contact");
                exportGrid.InsertDataColumn("Phone", "Phone");
                exportGrid.InsertDataColumn("Mobile Phone", "Cell");
                exportGrid.TotalsSummarySettings.SetColumnTotals("CurrentBalance, MonthOldBalance, TwoMonthsOldBalance, ThreeMonthsOrOlderBalance, Balance");
                exportGrid.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;

                exportGrid.DataSource = exportRecordWithNotes;
                exportGrid.DataBind();
            }


            if (report != null)
            {
                
                ExcelDocument document = new ExcelDocument();
                document._HSFFGetSheet.SetColumnWidth(8, 400);
                document.WriteTitle(report.Title);

                if (IsReportWithNotesLiteral.Text.ToLower() == "true"){
                    exportGrid.WriteToExcelDocument(document);
                }
                else {
                    reportGridView.WriteToExcelDocument(document);
                }

                document.MoveToNextRow();
                document.MoveToNextRow();
                document.AddCell("Date Viewed");
                document.AddCell(report.DateViewed.ToDateTimeString());

                WriteToResponse(document.WriteToStream(), report.ExportFileName);
            }
        }

        public override void ShowAllRecords()
        {
            //reportGridView.ShowPager();
        }

        public override bool IsViewAllButtonRequired()
        {
            return reportGridView.IsViewAllButtonRequired;
        }     

        public override void ShowPager()
        {
           // reportGridView.ShowPager();
        }

        public override void ResetPaginationAndFocus()
        {
            FocusedRowIndex = -1;
            PageIndex = 0;
        }

        public void ResetPaginationAndFocus(int focusedRow, int pgIdx)
        {
            FocusedRowIndex = focusedRow;
            PageIndex = pgIdx;
        }
    }
}