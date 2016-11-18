using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.UserControls.ReportPanels
{
    public partial class CustomerOverpaymentsTransactionReportPanel : ExportableGridPanel
    {

        CffGenGridView ReportGridView;

        protected void Page_Init(object sender, EventArgs e)
        {

            ReportGridView = new CffGenGridView();
            ReportGridView.BorderWidth = Unit.Pixel(1);
            ReportGridView.DefaultPageSize = 250;
            ReportGridView.AllowSorting = true;
            ReportGridView.SetSortExpression = "CustomerName";
            ReportGridView.AutoGenerateColumns = false;
            ReportGridView.ShowHeaderWhenEmpty = true;
            ReportGridView.EmptyDataText = "No data to display";

            //ReportGridView.CssClass = "cffGGV";
            ReportGridView.HeaderStyle.CssClass = "cffGGVHeader";
            ReportGridView.FooterStyle.CssClass = "dxgvFooter";
            ReportGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            
            ReportGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            ReportGridView.Width = Unit.Percentage(70);

            ReportGridView.ShowFooter = true;
            ReportGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;

            RptPlaceHolder.Controls.Clear();
            RptPlaceHolder.Controls.Add(ReportGridView);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Display(ViewState["CustomerOverpaymentsTransactionReport"] as CustomerOverpaymentsTransactionReport);
            }
            else
            {
                ConfigureGridColumns();
            }
        }

        public void Display(CustomerOverpaymentsTransactionReport report)
        {
            ViewState.Add("CustomerOverpaymentsTransactionReport", report);

            if (report != null)
            {
                ReportGridView.DataSource = report.Records;
                ReportGridView.DataBind();
            }
        }

        public override void Export()
        {
            var report = ViewState["CustomerOverpaymentsTransactionReport"] as CustomerOverpaymentsTransactionReport;

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
            
            ReportGridView.InsertDataColumn("Date", "Date", CffGridViewColumnType.Date, "8%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            ReportGridView.InsertDataColumn("Processed", "ProcessedDate", CffGridViewColumnType.Date, "8%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            ReportGridView.InsertDataColumn("Type", "Type", CffGridViewColumnType.Text, "8%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            ReportGridView.InsertDataColumn("Invoice", "Invoice", CffGridViewColumnType.Text, "8", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Ref", "Reference", CffGridViewColumnType.Text, "8%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            ReportGridView.InsertCurrencyColumn("Amount", "Amount", "12%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);

            ReportGridView.TotalsSummarySettings.SetColumnTotals("Amount");
            ReportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
        }

        public override void ResetPaginationAndFocus()
        {
            ReportGridView.ResetPaginationAndFocus();
        }

        public override bool IsViewAllButtonRequired()
        {
            return ReportGridView.IsViewAllButtonRequired;
        }

        public override void ShowAllRecords()
        {
            //ReportGridView.ShowAllRecords();
        }

        public override void ShowPager()
        {
            //ReportGridView.ShowPager();
        }
    }
}