using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.UserControls.ReportPanels
{
    public partial class CustomerTransactionReportPanel : ExportableGridPanel
    {
        CffGenGridView ReportGridView;
        protected ExportableGridPanel activeReportPanel;
        public event EventHandler<EventArgs> ReportParameterUpdated;
       
        protected void Page_Init(object sender, EventArgs e)
        {
            ReportGridView = new CffGenGridView();
            ReportGridView.ID = "CffGGV_CustomerTrxReportGridView";
            ReportGridView.PageSize = 250;
            ReportGridView.DefaultPageSize = 250;   // added by dbb
            ReportGridView.BorderWidth = Unit.Pixel(1);
            ReportGridView.AllowSorting = true;
            ReportGridView.SetSortExpression = "CustomerName";
            ReportGridView.AutoGenerateColumns = false;
            ReportGridView.ShowHeaderWhenEmpty = true;
            ReportGridView.EmptyDataText = "No data to display";
            ReportGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            ReportGridView.HeaderStyle.CssClass = "cffGGVHeader";
            ReportGridView.FooterStyle.CssClass = "dxgvFooter";
            ReportGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            ReportGridView.Width = Unit.Percentage(70);

            ReportGridView.CustomPagerSettingsMode = CffCustomPagerMode.Bottom | CffCustomPagerMode.Rows | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
            RptPlaceHolder.Controls.Clear();
            RptPlaceHolder.Controls.Add(ReportGridView);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Display(ViewState["CustomerTransactionReport"] as CustomerTransactionReport);
            }
            else
            {
                ConfigureGridColumns();
            }
        }

        public void Display(CustomerTransactionReport report)
        {
            ViewState.Add("CustomerTransactionReport", report);

            if (report != null)
            {
                ReportGridView.DataSource = report.Records;
                ReportGridView.DataBind();
            }
        }

        public override void Export()
        {
            var report = ViewState["CustomerTransactionReport"] as CustomerTransactionReport;
            if (report != null)  
            {
                var document = new ExcelDocument();
                document.WriteTitle(report.Title);
                ReportGridView.WriteToExcelDocument(document);

                document.InsertEmptyRow();

                document.AddCell("Date Viewed");
                document.AddCell(report.DateViewed.ToDateTimeString());
                WriteToResponse(document.WriteToStream(), report.ExportFileName);
            }
        }

        public void ConfigureGridColumns()
        {
            ReportGridView.Columns.Clear();
            ReportGridView.InsertDataColumn("Date", "Date", CffGridViewColumnType.Date, "8%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center,true);
            ReportGridView.InsertDataColumn("Processed", "ProcessedDate", CffGridViewColumnType.Date, "8%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            ReportGridView.InsertDataColumn("Type", "Type", CffGridViewColumnType.Text, "8%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            ReportGridView.InsertDataColumn("Invoice", "Invoice", CffGridViewColumnType.Text, "8", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertCurrencyColumn("Amount", "Amount", "12%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertDataColumn("Ref", "Reference", CffGridViewColumnType.Text, "9%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
          
            ReportGridView.ShowFooter = true;
            ReportGridView.TotalsSummarySettings.SetColumnTotals("Amount");
            ReportGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
        }

        public override void ResetPaginationAndFocus()
        {
        }

        public override bool IsViewAllButtonRequired()
        {
            return ReportGridView.IsViewAllButtonRequired;
        }

        public override void ShowAllRecords()
        {
        }

        public override void ShowPager()
        {
        }

    }
}