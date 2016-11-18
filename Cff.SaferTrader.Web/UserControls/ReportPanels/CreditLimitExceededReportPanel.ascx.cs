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
    public partial class CreditLimitExceededReportPanel : ExportableGridPanel
    {
     
        private CffGenGridView ReportGridView;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ReportGridView = new CffGenGridView();

            ReportGridView.PageSize = 250;
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

            //if (((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).Browser == "IE")
            //    ReportGridView.BorderWidth = Unit.Pixel(0);
            //else
                ReportGridView.BorderWidth = Unit.Pixel(1);
            ReportGridView.Width = Unit.Percentage(105);
            
            ReportGridView.ShowFooter = true;
            ReportGridView.TotalsSummarySettings.SetColumnTotals("CurrentBalance, MonthOldBalance, TwoMonthsOldBalance, ThreeMonthsOrOlderBalance, Balance");
            ReportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("CurrentBalance", "cffGGV_currencyCell");
            ReportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("MonthOldBalance", "cffGGV_currencyCell");
            ReportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("TwoMonthsOldBalance", "cffGGV_currencyCell");
            ReportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("ThreeMonthsOrOlderBalance", "cffGGV_currencyCell");
            ReportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Balance", "cffGGV_currencyCell");
            ReportGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            ReportGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;

            reportPlaceholder.Controls.Clear();
            reportPlaceholder.Controls.Add(ReportGridView);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureGridColumns();
            if (IsPostBack)
            {
                Display(ViewState["CreditLimitExceededReport"] as CreditLimitExceededReport);
            }
        }

        public void ConfigureGridColumns()
        {
            ReportGridView.Columns.Clear();

            if (SessionWrapper.Instance.IsAllClientsSelected)
            {
                ReportGridView.InsertBoundHyperLinkColumn("Client", "ClientName", "ClientNumber", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            }

            InsertCommonColumns();
        }

        private void InsertCommonColumns()
        {
            ReportGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "14%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            ReportGridView.InsertCurrencyColumn("Current", "CurrentBalance", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertCurrencyColumn("Month 1", "MonthOldBalance", "6%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertCurrencyColumn("Month 2", "TwoMonthsOldBalance", "7%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertCurrencyColumn("Month 3+", "ThreeMonthsOrOlderBalance", "7%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertCurrencyColumn("Balance", "Balance", "7%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertCurrencyColumn("Limit", "Limit", "7%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertDataColumn("Next Call", "NextCallDate", CffGridViewColumnType.Text, "3%","cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Contact", "Contact", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Phone", "Phone", CffGridViewColumnType.Text, "6%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Mobile Phone", "Cell", CffGridViewColumnType.Text, "6%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Email", "Email", CffGridViewColumnType.Text, "12%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);  

        }

        public override void Export()
        {
            var report = ViewState["CreditLimitExceededReport"] as CreditLimitExceededReport;

            if (report != null)
            {
                var document = new ExcelDocument();

                document.WriteTitle(report.Title);

                Hashtable hashtable = new Hashtable();
                hashtable.Add("Client", "ClientName");
                hashtable.Add("Customer", "CustomerName");

                ReportGridView.WriteToExcelDocumentWithReplaceField(document, hashtable);

                document.MoveToNextRow();
                document.MoveToNextRow();
                document.AddCell("Date Viewed");
                document.AddCell(report.DateViewed.ToDateTimeString());

                WriteToResponse(document.WriteToStream(), report.ExportFileName);
            }
        }

        public void Display(CreditLimitExceededReport report)
        {
            ViewState.Add("CreditLimitExceededReport", report);

            if (report != null)
            {
                ReportGridView.DataSource = report.Records;
                ReportGridView.DataBind();
            }
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
        public void ConfigureAllClientsGridColumns()
        {
            ReportGridView.Columns.Clear();
            //ReportGridView.InsertDataColumn("#", "ClientNumberLabel", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Center, true);
            ReportGridView.InsertBoundHyperLinkColumn("Client", "ClientName", "ClientNumber", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            InsertCommonColumns();
        }
        public void ConfigureClientGridColumns()
        {
            ReportGridView.Columns.Clear();
            InsertCommonColumns();
        }
        public void Clear()
        {
            ViewState.Add("CreditLimitExceededReport", null);
            ReportGridView.DataSource = null;
            ReportGridView.DataBind();
        }
    }
}