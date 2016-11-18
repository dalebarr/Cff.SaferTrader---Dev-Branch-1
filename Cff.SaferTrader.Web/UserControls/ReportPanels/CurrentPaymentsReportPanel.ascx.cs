using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.UserControls.ReportPanels
{
    public partial class CurrentPaymentsReportPanel : ExportableGridPanel
    {
        private CffGenGridView ReportGridView;

        private Scope CurrentScope()
        {
            if (SessionWrapper.Instance.Get != null)
                return SessionWrapper.Instance.Get.Scope;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).Scope;
            else
            {
                if (QueryString.CustomerId != null)
                    return Scope.CustomerScope;
                else if (QueryString.ClientId != null)
                    return Scope.ClientScope;
                return Scope.AllClientsScope;
            }

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ReportGridView = new CffGenGridView();
            ReportGridView.PageSize = 250;
            ReportGridView.AllowSorting = true;
            ReportGridView.EnableSortGraphic = true;

            ReportGridView.SetSortExpression = "CustomerName";
            ReportGridView.AutoGenerateColumns = false;
            ReportGridView.ShowHeaderWhenEmpty = true;
            ReportGridView.EmptyDataText = "No data to display";
            ReportGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            //ReportGridView.CssClass = "cffGGV";
            ReportGridView.HeaderStyle.CssClass = "cffGGVHeader";
            ReportGridView.FooterStyle.CssClass = "dxgvFooter";
            ReportGridView.Style.Add(HtmlTextWriterStyle.Width, "80%");
            ReportGridView.Width = Unit.Percentage(80);
            ReportGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            ReportGridView.BorderWidth = Unit.Pixel(1);

            ReportGridView.TotalsSummarySettings.SetColumnTotals("Amount, Balance");
            ReportGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Amount", "cffGGV_currencyCell");
            ReportGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Balance", "cffGGV_currencyCell");
            ReportGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            ReportGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;         
            reportPlaceholder.Controls.Clear();
            reportPlaceholder.Controls.Add(ReportGridView);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.CurrentScope())
            {
                case Scope.AllClientsScope:
                    ConfigureAllClientsGridColumns();
                    break;

                case Scope.ClientScope:
                    ConfigureClientGridColumns();
                    break;

                default:
                    {
                        ReportGridView.Columns.Clear();
                        InsertCommonColumns();
                    } break;
            }

            if (IsPostBack)
            {
                Display(ViewState["CurrentPaymentsReport"] as CurrentPaymentsReport);
            }
        }


        public void Display(CurrentPaymentsReport report)
        {
            ViewState.Add("CurrentPaymentsReport", report);

            if (report != null)
            {
                ReportGridView.DataSource = report.Records;
                ReportGridView.DataBind();

                FundedTotalLiteral.Text = report.FundedTotal.ToString("C");
                NonFundedTotalLiteral.Text = report.NonFundedTotal.ToString("C");
            }
        }

        private void InsertCommonColumns()
        {
            ReportGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "2%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            ReportGridView.InsertDataColumn("Date", "Date", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Processed", "ProcessedDate", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Invoice", "Invoice", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Ref", "Reference", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertCurrencyColumn("Amount", "Amount", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertCurrencyColumn("Balance", "Balance", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertDataColumn("Status", "Status", CffGridViewColumnType.Text, "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            ReportGridView.InsertBoundHyperLinkColumn("Batch", "Batch", "Batch", "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center);
        }

        public override void Export()
        {
            var report = ViewState["CurrentPaymentsReport"] as CurrentPaymentsReport;

            if (report != null)
            {
                var document = new ExcelDocument();
                document.WriteTitle(report.Title);

                Hashtable hashtable = new Hashtable();
                hashtable.Add("Customer", "CustomerName");

                ReportGridView.WriteToExcelDocumentWithReplaceField(document, hashtable);
                document.InsertEmptyRow();

                document.AddCell("Funded Total");
                //document.AddCurrencyCell(report.FundedTotal, ReportGridView.Columns["Amount"].Index);

                document.MoveToNextRow();
                document.AddCell("Non Funded Total");
                //document.AddCurrencyCell(report.NonFundedTotal, ReportGridView.Columns["Amount"].Index);

                document.InsertEmptyRow();
                document.AddCell("Date Viewed");
                document.AddCell(report.DateViewed.ToDateTimeString());

                WriteToResponse(document.WriteToStream(), report.ExportFileName);
            }
        }

        public override void ResetPaginationAndFocus()
        {
            //ReportGridView.ResetPaginationAndFocus();
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

        public void ConfigureAllClientsGridColumns()
        {
            ReportGridView.Columns.Clear();
            ReportGridView.InsertDataColumn("#", "ClientNumberLabel", CffGridViewColumnType.Text, "3%", "right dxgv");
            ReportGridView.InsertBoundHyperLinkColumn("Client", "ClientName", "ClientNumber", "15%", "dxgv");
            InsertCommonColumns();
        }

        public void ConfigureClientGridColumns()
        {
            ReportGridView.Columns.Clear();
            InsertCommonColumns();
        }
        public void Clear()
        {
            ViewState.Add("CurrentPaymentsReport", null);
            ReportGridView.DataSource = null;
            ReportGridView.DataBind();
            FundedTotalLiteral.Text = string.Empty;
            NonFundedTotalLiteral.Text = string.Empty;
        }
    }
}