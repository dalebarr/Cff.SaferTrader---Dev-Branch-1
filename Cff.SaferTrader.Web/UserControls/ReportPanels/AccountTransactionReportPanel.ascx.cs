using System;
using System.Collections;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;

using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;


namespace Cff.SaferTrader.Web.UserControls.ReportPanels
{
    public partial class AccountTransactionReportPanel : ExportableGridPanel
    {
        private CffGenGridView ReportGridView;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ReportGridView = new CffGenGridView();
            ReportGridView.ID = "ReportGridView";
     
            ReportGridView.PageSize = 250;
            ReportGridView.DefaultPageSize = 250;
            ReportGridView.AllowPaging = true;
            ReportGridView.AutoGenerateColumns = false;
            ReportGridView.SetSortExpression = "tranref";
            ReportGridView.AllowPaging = true;
            ReportGridView.BorderWidth = Unit.Pixel(1);
            ReportGridView.CssClass = "cffGGV";
            ReportGridView.HeaderStyle.CssClass = "cffGGVHeader";
            ReportGridView.ShowHeaderWhenEmpty = true;
            ReportGridView.EmptyDataText = "No data to display";
            ReportGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            ReportGridView.ShowFooter = true;
            ReportGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            ReportGridView.CustomPagerSettingsMode = CffCustomPagerMode.Page | CffCustomPagerMode.Rows | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;

            reportPlaceholder.Controls.Clear();
            reportPlaceholder.Controls.Add(ReportGridView);
        }
       

        public void ConfigureGridColumns()
        {
            ReportGridView.Columns.Clear();
            ReportGridView.KeyFieldName = "tranref";

            if (SessionWrapper.Instance.IsAllClientsSelected) {
                ReportGridView.InsertDataColumn("#", "ClientNumberLabel", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
                ReportGridView.InsertBoundHyperLinkColumn("ClientName", "Client", "ClientNumber", "20%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            }
            
            InsertCommonColumns();
        }

        private void InsertCommonColumns()
        {
            ReportGridView.InsertDataColumn("TranRef", "Reference", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("TranDate", "Date", CffGridViewColumnType.Date, "20%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("TranType", "Type", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertCurrencyColumn("Debit", "Debit", "10%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertCurrencyColumn("Credit", "Credit", "10%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertDataColumn("Description", "Description", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            ReportGridView.TotalsSummarySettings.SetColumnTotals("Debit, Credit");
            ReportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Debit", "cffGGV_currencyCell");
            ReportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Credit", "cffGGV_currencyCell");

            ReportGridView.TotalsSummarySettings.SetSummaryTotals("Debit, Credit");
            ReportGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Credit", "cffGGV_currencyCell");
            ReportGridView.TotalsSummarySettings.TotalsSummaryRows = 2;
            ReportGridView.TotalsSummarySettings.TotalsSummaryText = "Closing Balance, Movement";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureGridColumns();
            if (IsPostBack) 
            {
                Display(ViewState["AccountTransactionReport"] as AccountTransactionReport);
            }
        }

        public void Clear()
        {
            ViewState.Add("AccountTransactionReport", null);
            ReportGridView.DataSource = null;
            ReportGridView.DataBind();
        }

        public override void Export()
        {
            var report = ViewState["AccountTransactionReport"] as AccountTransactionReport;

            if (report != null)
            {
                var document = new ExcelDocument();
                document.WriteTitle(report.Title);

                Hashtable hashtable = new Hashtable();
                hashtable.Add("Client", "ClientName");
                ReportGridView.WriteToExcelDocumentWithReplaceField(document, hashtable);

                document.InsertEmptyRow();

                document.AddCell("Total Debits");
                document.AddCurrencyCell(report.TotalCredit, 3); //ReportGridView.Columns["Debit"]

                document.MoveToNextRow();
                document.AddCell("Total Credits");
                document.AddCurrencyCell(report.TotalCredit, 4); //ReportGridView.Columns["Credit"].Index)

                document.MoveToNextRow();
                document.AddCell("Movement");
                document.AddCurrencyCell(report.TotalCredit, 5); //ReportGridView.Columns["Credit"].Index

                document.InsertEmptyRow();
                document.AddCell("Date Viewed");
                document.AddCell(DateTime.Now.ToShortDateString());

                WriteToResponse(document.WriteToStream(), report.ExportFileName);
            }
        }

        public void Display(AccountTransactionReport report)
        {
            ViewState.Add("AccountTransactionReport", report);

            if (report != null)
            {
                ReportGridView.DataSource = report.Records;
                if (report.Records.Count < 250)
                    ReportGridView.AllowPaging = false;
                else
                    ReportGridView.AllowPaging = true;
                ReportGridView.DataBind();
            }
        }

       /*
        * protected void ReportGridViewCustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            CallbackParameter parameter = CallbackParameter.Parse(e.Parameters);
            AccountTransactionReportRecord record = (AccountTransactionReportRecord)ReportGridView.GetRow(parameter.RowIndex);
            RedirectionParameter redirectionParameter = new RedirectionParameter(parameter.FieldName, record.ClientId, 0, 0);

            ISecurityManager securityManager = SecurityManagerFactory.Create(Context.User as CffPrincipal, SessionWrapper.Instance.Scope);
            Redirector redirector = new Redirector(RedirectionService.Create(this, securityManager));
            redirector.Redirect(redirectionParameter);
        }*/

        public override void ShowAllRecords() {
            var report = ViewState["AccountTransactionReport"] as AccountTransactionReport;
            if (report != null)
            {
                ReportGridView.DataSource = report.Records;
                ReportGridView.DataBind();
            }
        }

        public override bool IsViewAllButtonRequired() { return false;  }
        public override void ShowPager() { ReportGridView.CustomPagerSettingsMode = CffCustomPagerMode.Page | CffCustomPagerMode.Rows | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext; }
        public override void ResetPaginationAndFocus() { }

    }
}