using System;
using System.Collections;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.UserControls.ReportPanels
{
    public partial class TransactionReportPanel : ExportableGridPanel
    {
        public CffGenGridView ReportGridView;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ReportGridView = new CffGenGridView();          
            ReportGridView.PageSize = 250;
            ReportGridView.DefaultPageSize = 250;
            ReportGridView.AllowSorting = true;
            ReportGridView.AllowPaging = true;
            ReportGridView.SetSortExpression = "CustomerName";
            ReportGridView.AutoGenerateColumns = false;
            ReportGridView.ShowHeaderWhenEmpty = true;
            ReportGridView.EmptyDataText = "No data to display";
            ReportGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
    
            //ReportGridView.CssClass = "cffGGV";
            ReportGridView.HeaderStyle.CssClass = "cffGGVHeader";
            ReportGridView.FooterStyle.CssClass = "dxgvFooter";
            ReportGridView.HoverRowCssClass = "cffGGVHoverRow";
            ReportGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            ReportGridView.EnableViewState = true;

            //if (((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).Browser == "IE")
            //    ReportGridView.BorderWidth = Unit.Pixel(0);
            //else
                ReportGridView.BorderWidth = Unit.Pixel(1);

            ReportGridView.Width = Unit.Percentage(70);
            ReportGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

            ReportGridView.ShowFooter = true;
            ReportGridView.TotalsSummarySettings.SetColumnTotals("Amount,BatchTotal");
            ReportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            ReportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("BatchTotal", "cffGGV_currencyCell");
            ReportGridView.CustomFooterSettings = CffCustomFooterMode.DefaultSettings | CffCustomFooterMode.ShowTotals;
            ReportGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;

            reportPlaceholder.Controls.Clear();
            reportPlaceholder.Controls.Add(ReportGridView);
        }

        public override void Export()
        {
            var report = ViewState["TransactionReport"] as TransactionReport;
            if (report != null) 
            {
                ExcelDocument document = new ExcelDocument();
                document.WriteTitle(report.Title);
                ReportGridView.Columns[0].Visible = false; //do not display in report
                ReportGridView.WriteToExcelDocument(document);
                document.InsertEmptyRow();
                document.MoveToNextRow();
                
                document.AddCell("Funded Total");
                document.AddCurrencyCell(report.FundedTotal);

                document.MoveToNextRow();
                document.AddCell("Non Funded Total");
                document.AddCurrencyCell(report.NonFundedTotal);

                document.InsertEmptyRow();
                document.AddCell("Date Viewed");
                document.AddCell(report.DateViewed.ToDateTimeString());

                WriteToResponse(document.WriteToStream(), report.ExportFileName);
            }
        }

        public void Display(TransactionReport report)
        {
            ViewState.Add("TransactionReport", report);

            if (report != null)
            {
                ConfigureGridColumns();
                ReportGridView.DataSource = report.Records;
                ReportGridView.DataBind();

                FundedTotalLiteral.Text = report.FundedTotal.ToString("C");
                NonFundedTotalLiteral.Text = report.NonFundedTotal.ToString("C");
            }
        }

        public void Clear()
        {
            ViewState.Add("TransactionReport", null);
            ReportGridView.DataSource = null;
            ReportGridView.DataBind();
            FundedTotalLiteral.Text = string.Empty;
            NonFundedTotalLiteral.Text = string.Empty;
        }

        public override void ShowPager()
        {
        }

        public void ConfigureGridColumns()
        {
            ReportGridView.Columns.Clear();
            ReportGridView.ShowHeaderWhenEmpty = true;
            ReportGridView.EmptyDataText = "No data to display";
            ReportGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            ReportGridView.AutoGenerateColumns = false;

            ReportGridView.PageSize = 250;  //rows per page
            if (SessionWrapper.Instance.IsAllClientsSelected) {
                ReportGridView.InsertDataColumn("#", "ClientNumberLabel", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Center, true);
                ReportGridView.InsertBoundHyperLinkColumn("Client", "ClientName", "ClientNumber", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Center);
            }

            InsertCommonColumns();
            ReportGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
            reportPlaceholder.Controls.Clear();
            reportPlaceholder.Controls.Add(ReportGridView);
        }

        private void InsertCommonColumns()
        {
            ReportGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);

            ReportGridView.InsertDataColumn("Date", "Date", CffGridViewColumnType.Date, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Processed", "ProcessedDate", CffGridViewColumnType.Date, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Invoice", "Invoice", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Ref", "Reference", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertCurrencyColumn("Amount", "Amount", "6%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertBoundHyperLinkColumn("Batch", "Batch", "Batch", "3%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, "", "cffGGVHeaderLeftAgedBal2");
            ReportGridView.InsertDataColumn("Status", "Status", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertCurrencyColumn("Batch Total", "BatchTotal", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);

        }

        public override void ShowAllRecords()
        {
            //ReportGridView.ShowAllRecords();
        }

        public override void ResetPaginationAndFocus()
        {
            //ReportGridView.ResetPaginationAndFocus();
        }

        public override bool IsViewAllButtonRequired()
        {
            return ReportGridView.IsViewAllButtonRequired;
        }

 
        protected void Page_Load(object sender, EventArgs e)
        {
            //ReportGridView.CustomCallback += ReportGridViewCustomCallback;
            ConfigureGridColumns();
            if (IsPostBack)
            {
                Display(ViewState["TransactionReport"] as TransactionReport);
            }
        }
    }
}