using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;
using System.Collections;

using System.Web.UI.WebControls;


namespace Cff.SaferTrader.Web.UserControls.ReportPanels
{
    public partial class StatusReportPanel : ExportableGridPanel
    {
        public CffGenGridView reportGridView;

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
            initReportGridView();
        }

        protected void initReportGridView()
        {
            reportGridView = new CffGenGridView();
            reportGridView.ID = "CffGGV_StatusReportGridView";
            reportGridView.PageSize = 250;
            reportGridView.DefaultPageSize = 250;
            reportGridView.AllowSorting = true;
            reportGridView.AllowPaging = true;
            reportGridView.AllowCustomPaging = false;
            reportGridView.EnableSortGraphic = true;
            reportGridView.AutoGenerateColumns = false;
            reportGridView.ShowHeaderWhenEmpty = true;
            
            reportGridView.SetSortExpression = "CustomerName";
            reportGridView.EmptyDataText = "No data to display";

            //reportGridView.CssClass = "cffGGV";
            reportGridView.HeaderStyle.CssClass = "cffGGVHeader";
            reportGridView.FooterStyle.CssClass = "dxgvFooter";
            reportGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            reportGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            reportGridView.Width = Unit.Percentage(150);
            reportGridView.BorderWidth = Unit.Pixel(1);

            reportGridView.ShowFooter = true;
            reportGridView.FooterStyle.BorderWidth = Unit.Pixel(1);
            reportGridView.TotalsSummarySettings.SetColumnTotals("Amount, OpeningBalance, Receipts, Credits, Discounts, Journals, Repurchases, Other, Balance, Retention, Charges");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyTotalTrxArch");  // previously cffGGV_currencyCell
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("OpeningBalance", "cffGGV_currencyTotalTrxArch");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Receipts", "cffGGV_currencyTotalTrxArch");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Credits", "cffGGV_currencyTotalTrxArch");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Discounts", "cffGGV_currencyTotalTrxArch");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Journals", "cffGGV_currencyTotalTrxArch");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Repurchases", "cffGGV_currencyTotalTrxArch");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Other", "cffGGV_currencyTotalTrxArch");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Balance", "cffGGV_currencyTotalTrxArch");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Retention", "cffGGV_currencyTotalTrxArch");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Charges", "cffGGV_currencyTotalTrxArch");

            reportGridView.EnableViewState = true;
            reportGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
            reportGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            reportGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;
            
            reportPlaceholder.Controls.Clear();
            reportPlaceholder.Controls.Add(reportGridView);
        }

        
     
        protected void Page_Load(object sender, EventArgs e)
        {
            int renderCompat = reportPlaceholder.RenderingCompatibility.Major;

            switch (this.CurrentScope()) { 
                case Scope.AllClientsScope:
                    ConfigureAllClientsGridColumns();
                    break;

                case Scope.ClientScope:
                    ConfigureClientGridColumns();
                    break;

                default:
                    ConfigureCustomerGridColumns();
                    break;
            }

            if (IsPostBack)
            {
                Display(ViewState["StatusReport"] as StatusReport);
            }
        }

   
        public void Display(StatusReport report)
        {
            ViewState.Add("StatusReport", report);

            if (report != null)
            {
                //uncomment for testing purposes only - 05112012
                reportGridView.DataSource = report.Records;
                reportGridView.DataBind();
                reportGridView.SelectedIndex = -1;
            }
        }

        public override void Export()
        {
            StatusReport report = ViewState["StatusReport"] as StatusReport;

            if (report != null)
            {
                var document = new ExcelDocument();

                document.WriteTitle(report.Title);

                Hashtable hashtable = new Hashtable();
                hashtable.Add("Client", "ClientName");
                hashtable.Add("Customer", "CustomerName");

                reportGridView.WriteToExcelDocumentWithReplaceField(document, hashtable);
                document.MoveToNextRow();
                document.MoveToNextRow();
                document.AddCell("Date Viewed");
                document.AddCell(report.DateViewed.ToDateTimeString());
                WriteToResponse(document.WriteToStream(), report.ExportFileName);
            }
        }

        public override void ShowAllRecords()
        {
            //reportGridView.ShowAllRecords();
        }

        public override void ShowPager()
        {
            //reportGridView.ShowPager();
        }

        public override void ResetPaginationAndFocus()
        {
            //reportGridView.ResetPaginationAndFocus();
        }

        public override bool IsViewAllButtonRequired()
        {
            return reportGridView.IsViewAllButtonRequired;
        }

        private void InsertCommonColumns()       
        {
            reportGridView.InsertBoundHyperLinkColumn("Batch", "Batch", "Batch", "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            reportGridView.InsertDataColumn("Dated", "Dated", CffGridViewColumnType.Date, "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            reportGridView.InsertDataColumn("Processed", "Factored", CffGridViewColumnType.Date, "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            reportGridView.InsertDataColumn("Age", "Age", CffGridViewColumnType.Text, "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            reportGridView.InsertDataColumn("Number", "Number", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            reportGridView.InsertDataColumn("Reference", "Reference", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            reportGridView.InsertCurrencyColumn("Amount", "Amount", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            reportGridView.InsertCurrencyColumn("Opening Balance", "OpeningBalance", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            reportGridView.InsertCurrencyColumn("Receipts", "Receipts", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            reportGridView.InsertCurrencyColumn("Credits", "Credits", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            reportGridView.InsertCurrencyColumn("Discounts", "Discounts", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            reportGridView.InsertCurrencyColumn("Journals", "Journals", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            reportGridView.InsertCurrencyColumn("Prepaid", "Repurchases", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            reportGridView.InsertCurrencyColumn("Other", "Other", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);

            reportGridView.InsertDataColumn("LastTrans", "LastTransaction", CffGridViewColumnType.Date, "4%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            reportGridView.InsertCurrencyColumn("Balance", "Balance", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            reportGridView.InsertCurrencyColumn("Residual", "Retention", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            reportGridView.InsertCurrencyColumn("Charges", "Charges", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);

            //reportGridView.InsertDataColumn("Status", "Status", CffGridViewColumnType.Text, "8%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGVHeaderLeftAgedBal2");
            //reportGridView.InsertDataColumn("Repurchased", "RepurchaseDate", CffGridViewColumnType.Date, "8%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Center, true, "cffGGVHeaderLeftAgedBal2");
            //reportGridView.InsertDataColumn("Type", "Type", CffGridViewColumnType.Text, "8%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGVHeaderLeftAgedBal2");     

            reportGridView.InsertDataColumn("Status", "Status", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true);
            reportGridView.InsertDataColumn("Prepayments", "RepurchaseDate", CffGridViewColumnType.Date, "3%", "cffGGV_centerAlignedCellAgedBal", HorizontalAlign.Center, HorizontalAlign.Center, true);
            reportGridView.InsertDataColumn("Type", "Type", CffGridViewColumnType.Text, "3%", "cffGGV_centerAlignedCellAgedBal", HorizontalAlign.Center, HorizontalAlign.Center, true);     
        }
        
        public void ConfigureAllClientsGridColumns()
        {
            reportGridView.Columns.Clear();
            reportGridView.InsertBoundHyperLinkColumn("Client", "ClientName", "ClientNumber", "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            reportGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            InsertCommonColumns();
        }

        public void ConfigureClientGridColumns()
        {
            reportGridView.Columns.Clear();
            reportGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Date, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            reportGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            InsertCommonColumns();
        }

        public void ConfigureCustomerGridColumns()
        {
            reportGridView.Columns.Clear();
            InsertCommonColumns();
        }

        public void Clear()
        {
            ViewState.Add("StatusReport", null);
            reportGridView.DataSource = null;
            reportGridView.DataBind();
        }
    }
}