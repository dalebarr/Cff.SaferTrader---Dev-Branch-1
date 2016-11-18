using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.UserControls.ReportPanels
{
    public partial class InvoiceTransactionReportPanel : ExportableGridPanel
    {
        public CffGenGridView ReportGridView;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ReportGridView = new CffGenGridView();

            ReportGridView.AllowSorting = true;
            ReportGridView.AllowCustomPaging = false;
            ReportGridView.RowStyleHighlightColour = System.Drawing.Color.Honeydew;

            ReportGridView.SetSortExpression = "CustomerName";
            ReportGridView.AutoGenerateColumns = false;
            ReportGridView.ShowHeaderWhenEmpty = true;
            ReportGridView.EmptyDataText = "No data to display";
            //ReportGridView.CssClass = "cffGGV";
            ReportGridView.HeaderStyle.CssClass = "cffGGVHeader";
            ReportGridView.FooterStyle.CssClass = "dxgvFooter";
            ReportGridView.HoverRowCssClass = "cffGGVHoverRow";
            ReportGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            ReportGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            ReportGridView.Width = Unit.Percentage(80);
            ReportGridView.BorderWidth = Unit.Pixel(1);

            ReportGridView.AllowPaging = false;
            ReportGridView.PageSize = 1000;
            ReportGridView.DefaultPageSize = 1000;

            ReportGridView.EnableViewState = true;
            ReportGridView.ShowFooter = true;
            ReportGridView.TotalsSummarySettings.SetColumnTotals("Amount,BatchTotal");
            ReportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            ReportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("BatchTotal", "cffGGV_currencyCell");
            ReportGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            reportPlaceholder.Controls.Clear();
            reportPlaceholder.Controls.Add(ReportGridView);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Display(ViewState["InvoicesTransactionReport"] as InvoicesTransactionReport);
            }
            else
            {
                ConfigureGridColumns();
            }
        }

        public void Display(InvoicesTransactionReport report)
        {
            ViewState.Add("InvoicesTransactionReport", report);

            if (report != null)
            {
                ReportGridView.DataSource = report.Records;
                ReportGridView.DataBind();

                FundedTotalLiteral.Text = report.FundedTotal.ToString("C");
                NonFundedTotalLiteral.Text = report.NonFundedTotal.ToString("C");
           
                MeanDebtorDaysByInvoiceLiteral.Text = report.MeanDebtorDays.ToString("#0.00");
                MeanAgeOfUnpaidByInvoiceLiteral.Text = report.MeanAgeOfUnpaidRecords.ToString("#0.00");

                MeanDebtorDaysByBOMLiteral.Text =
                    report.MeanDebtorDaysFromBeginningOfFollowingMonth.ToString("#0.00");
                MeanAgeOfUnpaidByBOMLiteralLiteral.Text =
                    report.MeanAgeOfUnpaidRecordsFromBeginningOfFollowingMonth.ToString("#0.00");

                PaidForRecordCountLiteral.Text = report.NumberOfRecordsPaidFor.ToString();
                UnpaidForRecordCountLiteral.Text = report.NumberOfRecordsNotPaidFor.ToString();

            }
        }

        public override void Export()
        {
            var report = ViewState["InvoicesTransactionReport"] as InvoicesTransactionReport;

            if (report != null)
            {
                var document = new ExcelDocument();
                
                document.WriteTitle(report.Title);

                Hashtable hashtable = new Hashtable();
                hashtable.Add("Customer", "CustomerName");

                ReportGridView.WriteToExcelDocumentWithReplaceField(document, hashtable);

                document.InsertEmptyRow();

                document.AddCell("Funded Total");
                //document.AddCurrencyCell(report.FundedTotal,
                //                                     ReportGridView.Columns["Amount"].Index);

                document.MoveToNextRow();
                document.AddCell("Non Funded Total");
                //document.AddCurrencyCell(report.NonFundedTotal,
                //                                     ReportGridView.Columns["Amount"].Index);

                document.InsertEmptyRow();

                document.AddCell("Based on");
                document.AddCell("Invoice Date");
                document.AddCell("BOM Following");
                document.AddCell("Count");

                document.MoveToNextRow();
                document.AddCell("Mean Debtor Days");
                document.AddNumericCellToCurrentRow(report.MeanDebtorDays);
                document.AddNumericCellToCurrentRow(report.MeanDebtorDaysFromBeginningOfFollowingMonth);
                document.AddNumericCellToCurrentRow(report.NumberOfRecordsPaidFor);

                document.MoveToNextRow();
                document.AddCell("Mean Age of Unpaid ");
                document.AddNumericCellToCurrentRow(report.MeanAgeOfUnpaidRecords);
                document.AddNumericCellToCurrentRow(report.MeanAgeOfUnpaidRecordsFromBeginningOfFollowingMonth);
                document.AddNumericCellToCurrentRow(report.NumberOfRecordsNotPaidFor);

                document.InsertEmptyRow();

                document.AddCell("Date Viewed");
                document.AddCell(report.DateViewed.ToDateTimeString());

                WriteToResponse(document.WriteToStream(), report.ExportFileName);
            }
        }

        public void ConfigureGridColumns()
        {
            ReportGridView.Columns.Clear();
            if (SessionWrapper.Instance.IsAllClientsSelected) 
            {
                ReportGridView.InsertDataColumn("#", "ClientNumberLabel", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
                ReportGridView.InsertBoundHyperLinkColumn("Client", " ClientName", "ClientNumber", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            }

            ReportGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            ReportGridView.InsertDataColumn("Date", "Date", CffGridViewColumnType.Date, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Processed", "ProcessedDate", CffGridViewColumnType.Date, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Invoice", "Invoice", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Ref", "Reference", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertCurrencyColumn("Amount", "Amount", "6%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertDataColumn("Status-Age", "StatusAge", CffGridViewColumnType.Text, "4%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGVHeaderLeftAgedBal2");
            ReportGridView.InsertBoundHyperLinkColumn("Batch", "Batch", "Batch", "3%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Center, HorizontalAlign.Center, "", "cffGGVHeaderLeftAgedBal2");
            ReportGridView.InsertCurrencyColumn("Batch Total", "BatchTotal", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertDataColumn("Status", "Status", CffGridViewColumnType.Text, "4%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGVHeaderLeftAgedBal2");
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
        public void Clear()
        {
            ViewState.Add("InvoicesTransactionReport", null);

                ReportGridView.DataSource = null;
                ReportGridView.DataBind();

                FundedTotalLiteral.Text =string.Empty;
                NonFundedTotalLiteral.Text = string.Empty;
          
                MeanDebtorDaysByInvoiceLiteral.Text = string.Empty;
                MeanAgeOfUnpaidByInvoiceLiteral.Text = string.Empty;

                MeanDebtorDaysByBOMLiteral.Text =
                    string.Empty;
                MeanAgeOfUnpaidByBOMLiteralLiteral.Text =
                    string.Empty;

                PaidForRecordCountLiteral.Text = string.Empty;
                UnpaidForRecordCountLiteral.Text = string.Empty;
            
        }
    }
}