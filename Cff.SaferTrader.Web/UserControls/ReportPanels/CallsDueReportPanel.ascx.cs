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
    public partial class CallsDueReportPanel : ExportableGridPanel
    {
        private CffGenGridView reportGridView;

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

            reportGridView = new CffGenGridView();
            reportGridView.PageSize = 250;
            reportGridView.DefaultPageSize = 250;
            reportGridView.AllowSorting = true;
            reportGridView.BorderWidth = Unit.Pixel(1);

            reportGridView.SetSortExpression = "CustomerName";
            reportGridView.AutoGenerateColumns = false;
            reportGridView.ShowHeaderWhenEmpty = true;
            reportGridView.EmptyDataText = "No data to display";

            //reportGridView.CssClass = "cffGGV";
            reportGridView.HeaderStyle.CssClass = "cffGGVHeader";
            reportGridView.FooterStyle.CssClass = "dxgvFooter";
            reportGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            reportGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            
            reportGridView.Width = Unit.Percentage(80);

            //if (((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).Browser == "IE")
            //    reportGridView.BorderWidth = Unit.Pixel(0);
            //else
            //    reportGridView.BorderWidth = Unit.Pixel(1);


            reportGridView.ShowFooter = true;
            reportGridView.TotalsSummarySettings.SetColumnTotals("Balance");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Balance","cffGGV_rightAlignedCell");
            reportGridView.CustomFooterSettings = CffCustomFooterMode.DefaultSettings | CffCustomFooterMode.ShowTotals;
            reportGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;

            reportPlaceholder.Controls.Clear();
            reportPlaceholder.Controls.Add(reportGridView);
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

                default: {
                    reportGridView.Columns.Clear();
                    InsertCommonColumns();
                } break;
            }

            if (IsPostBack)
            {
                Display(ViewState["CallsDueReport"] as CallsDueReport);
            }

        }

        public void Display(CallsDueReport report)
        {
            ViewState.Add("CallsDueReport", report);

            if (report != null)
            {
                reportGridView.DataSource = report.Records;
                reportGridView.DataBind();
            }
        }

        private void InsertCommonColumns()
        {
            reportGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "2%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            reportGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            reportGridView.InsertDataColumn("Title", "Title", CffGridViewColumnType.Text, "4%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            reportGridView.InsertDataColumn("Due", "Due", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            reportGridView.InsertCurrencyColumn("Balance", "Balance", "5%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            reportGridView.InsertDataColumn("Age", "Age", CffGridViewColumnType.Text, "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            reportGridView.InsertDataColumn("First Name", "FirstName", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            reportGridView.InsertDataColumn("Last Name", "LastName", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            reportGridView.InsertDataColumn("Phone", "Phone", CffGridViewColumnType.Text, "5%","cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            reportGridView.InsertDataColumn("Mobile Phone", "Cellphone", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            reportGridView.InsertDataColumn("Fax", "Fax", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
        }

        public void ConfigureAllClientsGridColumns()
        {
            reportGridView.Columns.Clear();
            reportGridView.InsertDataColumn("#", "ClientNumberLabel", CffGridViewColumnType.Text, "2%", "right dxgv");
            reportGridView.InsertBoundHyperLinkColumn("Client", "ClientName", "ClientNumber", "10%", "dxgv");
            InsertCommonColumns();
        }

        public void ConfigureClientGridColumns()
        {
            reportGridView.Columns.Clear();
            InsertCommonColumns();
        }


        public override void Export()
        {
            var report = ViewState["CallsDueReport"] as CallsDueReport;

            if (report != null)
            {
                var document = new ExcelDocument();

                document.WriteTitle(report.Title);

                Hashtable hashtable = new Hashtable();
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
        public void Clear()
        {
            ViewState.Add("CallsDueReport", null);
            reportGridView.DataSource = null;
            reportGridView.DataBind();
        }
    }
}