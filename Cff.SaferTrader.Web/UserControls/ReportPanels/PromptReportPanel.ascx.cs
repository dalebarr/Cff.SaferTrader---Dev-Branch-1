using System;
using System.Web.UI.WebControls;
using System.Collections;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.UserControls.ReportPanels
{
    public partial class PromptReportPanel : ExportableGridPanel
    {
        protected CffGenGridView ReportGridView;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);          
        }

        protected void Page_Init(object sender, EventArgs e) {
            ReportGridView = new CffGenGridView();
            ReportGridView.PageSize = 250;
            ReportGridView.DefaultPageSize = 250;
            ReportGridView.AllowSorting = true;
            ReportGridView.AllowPaging = true;
            ReportGridView.AutoGenerateColumns = false;
            ReportGridView.ShowHeaderWhenEmpty = true;
            ReportGridView.Width = Unit.Percentage(80);

            //if (((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).Browser == "IE")
            //    ReportGridView.BorderWidth = Unit.Pixel(0);
            //else
                ReportGridView.BorderWidth = Unit.Pixel(1);

            ReportGridView.SetSortExpression = "CustomerName";
            ReportGridView.EmptyDataText = "No data to display";
            ReportGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            //ReportGridView.CssClass = "cffGGV";
            ReportGridView.HeaderStyle.CssClass = "cffGGVHeader";
            ReportGridView.FooterStyle.CssClass = "dxgvFooter";
            ReportGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            ReportGridView.Style.Add(System.Web.UI.HtmlTextWriterStyle.Width, "80%");

            ReportGridView.ShowFooter = true;
            ReportGridView.TotalsSummarySettings.SetColumnTotals("Amount, Balance");
            ReportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_rightAlignedCell");
            ReportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Balance", "cffGGV_rightAlignedCell");

            ReportGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;
            ReportGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;

            ReportGridView.EnableViewState = true;
            ReportGridView.Visible = true;
            reportPlaceholder.Visible = true;

            reportPlaceholder.Controls.Clear();
            reportPlaceholder.Controls.Add(ReportGridView);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) {
                Display(ViewState["PromptReport"] as PromptReport);
            }
        }

        public override void Export()
        {
            var report = ViewState["PromptReport"] as PromptReport;
            if (report != null) {
                ExcelDocument document = new ExcelDocument();

                document.WriteTitle(report.Title);

                Hashtable hashtable = new Hashtable();
                hashtable.Add("Client", "ClientName");
                hashtable.Add("Customer", "CustomerName");
                
                ReportGridView.WriteToExcelDocumentWithReplaceField(document, hashtable);

                document.InsertEmptyRow();
                document.AddCell("Funded Balance");
                //document.AddCurrencyCell(report.FundedBalanceTotal, ReportGridView.Columns["Amount"].Index);

                document.MoveToNextRow();
                document.AddCell("Non Funded Balance");
                //document.AddCurrencyCell(report.NonFundedBalancedTotal, ReportGridView.Columns["Amount"].Index); 

                document.InsertEmptyRow();

                document.AddCell("Date Viewed");
                document.AddCell(report.DateViewed.ToDateTimeString());

                WriteToResponse(document.WriteToStream(), report.ExportFileName);
            }
        }

        public void Display(PromptReport report) 
        {
            ViewState.Add("PromptReport", report);

            if (report != null) {
                ConfigureGridColumns();
                ReportGridView.DataSource = report.Records;
                ReportGridView.DataBind();

                FundedBalanceLiteral.Text = report.FundedBalanceTotal.ToString("C");
                NonFundedBalance.Text = report.NonFundedBalancedTotal.ToString("C");
            }
        }

        public void ConfigureGridColumns() 
        {
            ReportGridView.Columns.Clear();

            if (SessionWrapper.Instance.IsAllClientsSelected) {
                ReportGridView.InsertBoundHyperLinkColumn("Client", "ClientName", "ClientNumber", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            }

            ReportGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Date, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            ReportGridView.InsertDataColumn("Dated", "Dated", CffGridViewColumnType.Date, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Processed", "Factored", CffGridViewColumnType.Date, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Invoice", "TrnNumber", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Ref", "Reference", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertCurrencyColumn("Amount", "Amount", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertCurrencyColumn("Balance", "Balance", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertDataColumn("Age", "Age", CffGridViewColumnType.Text, "2%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            ReportGridView.InsertDataColumn("Status", "Status", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertBoundHyperLinkColumn("Batch", "Batch", "Batch", "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
        }

        public override void ShowAllRecords()
        {
            //ReportGridView.ShowAllRecords();
        }

        public override void ShowPager()
        {
            //ReportGridView.ShowPager();
        }

        public override void ResetPaginationAndFocus()
        {
            //ReportGridView.ResetPaginationAndFocus();
        }

        public override bool IsViewAllButtonRequired()
        {
            return ReportGridView.IsViewAllButtonRequired;
        }
        public void Clear()
        {
            ViewState.Add("PromptReport", null);
            ReportGridView.DataSource = null;
            ReportGridView.DataBind();

            FundedBalanceLiteral.Text = string.Empty;
            NonFundedBalance.Text = string.Empty;
        }
    }
}