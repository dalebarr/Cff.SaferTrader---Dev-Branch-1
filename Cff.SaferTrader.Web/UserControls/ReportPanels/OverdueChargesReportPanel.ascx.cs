using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;
using System.Collections;

namespace Cff.SaferTrader.Web.UserControls.ReportPanels
{
    public partial class OverdueChargesReportPanel : ExportableGridPanel
    { 
        public CffGenGridView reportGridView; // main grid 
    
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
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            reportGridView = new CffGenGridView();
            reportGridView.PageSize = 250;
            reportGridView.DefaultPageSize = 250;
            reportGridView.AllowSorting = true;
            reportGridView.AllowPaging = true;
            reportGridView.AllowCustomPaging = false;
            reportGridView.AutoGenerateColumns = false;
            reportGridView.ShowHeaderWhenEmpty = true;
           
            reportGridView.SetSortExpression = "CustomerName";
            reportGridView.EmptyDataText = "No data to display";
            //reportGridView.CssClass = "cffGGV";
            reportGridView.HeaderStyle.CssClass = "cffGGVHeader";
            reportGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            reportGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            reportGridView.RowCssClass = "dxgvDataRow";
            reportGridView.FooterStyle.CssClass = "dxgvFooter";
            reportGridView.Width = Unit.Percentage(68);
            reportGridView.Style.Add(HtmlTextWriterStyle.Width, "68%");
            //reportGridView.Height = Unit.Percentage(100);
         
            //if (((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).Browser == "IE")
            //    reportGridView.BorderWidth = Unit.Pixel(0);
            //else
                reportGridView.BorderWidth = Unit.Pixel(1);

            reportGridView.ShowFooter = true;
            reportGridView.TotalsSummarySettings.SetColumnTotals("Charges, ChargesWithGst, Amount, Balance");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Charges", "cffGGV_currencyCell");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("ChargesWithGst", "cffGGV_currencyCell");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            reportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Balance", "cffGGV_currencyCell");
            reportGridView.TotalsSummarySettings.TotalsText = "Totals:";

            reportGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            reportGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows |  CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;
            
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

                default:
                    ConfigureCustomerGridColumns();
                    break;
            }

            if (IsPostBack)
            {
                Display(ViewState["OverdueChargesReport"] as OverdueChargesReport);
            }
        }

        public void Display(OverdueChargesReport report)
        {
            ViewState.Add("OverdueChargesReport", report);

            if (report != null)
            {
                reportGridView.DataSource = report.Records;
                reportGridView.DataBind();
            }
        }

        public override void Export()
        {
            OverdueChargesReport report = ViewState["OverdueChargesReport"] as OverdueChargesReport;

            if (report != null)
            {
                var document = new ExcelDocument();
                document.WriteTitle(report.Title);

                if (this.CurrentScope() == Scope.AllClientsScope)
                {
                    reportGridView.Columns[0].Visible = false; //remove the client number from report
                    reportGridView.Columns[2].Visible = false; //remove the customer number from report
                }
                else if (this.CurrentScope() == Scope.ClientScope)
                    reportGridView.Columns[0].Visible = false; //remove the customer number from report
                
                reportGridView.WriteToExcelDocument(document);
                document.MoveToNextRow();
                document.MoveToNextRow();
                document.AddCell("Date Viewed");
                document.AddCell(report.DateViewed.ToDateTimeString());

                WriteToResponse(document.WriteToStream(), report.ExportFileName);
            }
        }

        private void InsertCommonColumns()
        {
            reportGridView.InsertDataColumn("Processed", "Factored", CffGridViewColumnType.Date, "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            reportGridView.InsertDataColumn("Age", "Age", CffGridViewColumnType.Text, "2%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            reportGridView.InsertDataColumn("Transactions", "Number", CffGridViewColumnType.Text, "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            reportGridView.InsertDataColumn("Reference", "Reference", CffGridViewColumnType.Text, "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            reportGridView.InsertCurrencyColumn("Charges", "Charges", "4%", "cffGGV_rightAlignedCell", true, HorizontalAlign.Right, HorizontalAlign.Right );
            reportGridView.InsertCurrencyColumn("With GST", "ChargesWithGst", "4%", "cffGGV_rightAlignedCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            reportGridView.InsertCurrencyColumn("Amount", "Amount", "4%", "cffGGV_rightAlignedCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            reportGridView.InsertCurrencyColumn("Balance", "Balance", "4%", "cffGGV_rightAlignedCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            reportGridView.InsertDataColumn("Status", "Status", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGVHeaderLeftAgedBal2");
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

        public void ConfigureAllClientsGridColumns()
        {
            reportGridView.Columns.Clear();
            reportGridView.InsertDataColumn("#", "ClientNumberLabel", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Center, true);
            reportGridView.InsertBoundHyperLinkColumn("Client", "ClientName", "ClientNumber", "12%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Center);
            reportGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Center, true);
            reportGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerID", "13%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Center);
            reportGridView.InsertDataColumn("Title", "Title", CffGridViewColumnType.Text, "8%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Center, true);
            InsertCommonColumns();
        }

        public void ConfigureClientGridColumns()
        {
            reportGridView.Columns.Clear();
            reportGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            reportGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerID", "12%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            reportGridView.InsertDataColumn("Title", "Title", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            InsertCommonColumns();
        }

        public void ConfigureCustomerGridColumns()
        {
            reportGridView.Columns.Clear();
            InsertCommonColumns();
        }

        public void Clear()
        {
            ViewState.Add("OverdueChargesReport", null);
            reportGridView.DataSource = null;
            reportGridView.DataBind();
        }
    }
}