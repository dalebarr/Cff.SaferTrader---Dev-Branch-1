using System;
using System.Collections;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;


namespace Cff.SaferTrader.Web.UserControls.ReportPanels
{
    public partial class AgedBalancesReportPanel : ExportableGridPanel
    {
        protected CffGridView reportGridView;
        protected bool isReportWithNotes;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            initReportGridView();
            this.isReportWithNotes = false;
        }

        protected void initReportGridView()
        {
            reportGridView = new CffGridView(250);
            reportGridView.KeyFieldName = "Id";

            reportGridView.SettingsBehavior.AllowSort = true;
            reportGridView.SettingsBehavior.AllowFocusedRow = true;
            

            reportGridView.SettingsPager.AlwaysShowPager = true;
            reportGridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
            reportGridView.ShowPager(250);

            reportGridView.Settings.ShowFooter = true;
            reportGridView.TotalSummary.Clear();

            reportPlaceholder.Controls.Clear();
            reportPlaceholder.Controls.Add(reportGridView);
        }

  
        protected void ReportGridViewCustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            CallbackParameter parameter = CallbackParameter.Parse(e.Parameters);
            AgedBalancesReportRecord record = (AgedBalancesReportRecord) reportGridView.GetRow(parameter.RowIndex);
            RedirectionParameter redirectionParameter = new RedirectionParameter(parameter.FieldName,
                                                                                 record.ClientNumber, record.Id);

            ISecurityManager securityManager = SecurityManagerFactory.Create(Context.User as CffPrincipal, SessionWrapper.Instance.Scope);
            Redirector redirector = new Redirector(RedirectionService.Create(this, securityManager));
            redirector.Redirect(redirectionParameter);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            reportGridView.CustomCallback += ReportGridViewCustomCallback;
            reportGridView.PageIndexChanged += reportGridView_PageIndexChanged;
            reportGridView.FocusedRowChanged += reportGridView_FocusedRowChanged;
            reportGridView.DetailRowExpandedChanged += reportGridView_DetailRowExpandedChanged;
            if (IsPostBack)
            {
                Display(ViewState["AgedBalancesReport"] as AgedBalancesReport);
            }
        }

        private void InsertCommonColumns()
        {
            reportGridView.SettingsBehavior.AllowSort = true;
            reportGridView.InsertRightAlignedDataColumn("CustomerNumber", "#");
            reportGridView.InsertHyperlinkColumn("CustomerName", "Customer", "Id", LinkHelper.NavigateUrlFormatToDashboardForCustomer);
            reportGridView.InsertCurrencyColumn("CurrentBalance", "Current");
            reportGridView.InsertCurrencyColumn("MonthOldBalance", "Month 1");
            reportGridView.InsertCurrencyColumn("TwoMonthsOldBalance", "Month 2");
            reportGridView.InsertCurrencyColumn("ThreeMonthsOrOlderBalance", "Month 3+");
            reportGridView.InsertCurrencyColumn("Balance");
            reportGridView.InsertDataColumn("NextCallDate", "Next Call");
            reportGridView.InsertMemoColumn("Note", "Note");
            reportGridView.Columns["Note"].Visible = false;
            
            if (isReportWithNotes)
            {
                reportGridView.Columns["Note"].Visible = true;
        
                IsReportWithNotesLiteral.Text = "true";
                IsReportWithNotesLiteral.Visible = false;
            }
        
            reportGridView.SettingsBehavior.ProcessFocusedRowChangedOnServer = false;
            reportGridView.SettingsBehavior.ProcessSelectionChangedOnServer = false;
            reportGridView.EnableCallBacks = true;

            reportGridView.SettingsDetail.ShowDetailRow = false;
            reportGridView.SettingsDetail.ShowDetailButtons = false;
            
            reportGridView.InsertDataColumn("Email");
            reportGridView.InsertDataColumn("Contact");
            reportGridView.InsertDataColumn("Phone");
            reportGridView.InsertDataColumn("Cell", "Mobile Phone");
      
    
            reportGridView.TotalSummary.Clear();
            reportGridView.InsertTotalSummaryLabelColumn(0);
            reportGridView.InsertTotalSummarySumColumn("CurrentBalance");
            reportGridView.InsertTotalSummarySumColumn("MonthOldBalance");
            reportGridView.InsertTotalSummarySumColumn("TwoMonthsOldBalance");
            reportGridView.InsertTotalSummarySumColumn("ThreeMonthsOrOlderBalance");
            reportGridView.InsertTotalSummarySumColumn("Balance");
        }


        protected void reportGridView_DetailRowExpandedChanged(object sender, EventArgs e)
        {
            reportGridView.CurrentFocusedRow = ((ASPxGridViewDetailRowEventArgs)e).VisibleIndex;
            reportGridView.FocusedRowIndex = reportGridView.CurrentFocusedRow;
            if (IsReportWithNotesLiteral.Text.ToLower() == "true" && ((ASPxGridViewDetailRowEventArgs)e).Expanded)
            {
                try
                {
                    reportGridView.DetailRows.CollapseAllRows();
                    reportGridView.FindDetailRowTemplateControl(reportGridView.CurrentFocusedRow, ((sender as CffGridView).ID));
                    reportGridView.Templates.DetailRow = new AgedBalancesNotesDetailRowTemplate("Created",
                                ((sender as CffGridView).GetRow(reportGridView.CurrentFocusedRow) as AgedBalancesReportRecord).CustNoteList,
                                   ((sender as CffGridView).ID), reportGridView.CurrentFocusedRow);
                    reportGridView.DetailRows.ExpandRow(reportGridView.CurrentFocusedRow);
                }
                catch { }
            }
            else if (IsReportWithNotesLiteral.Text.ToLower() == "true")
            {
                reportGridView.FindVisibleIndexByKeyValue(reportGridView.CurrentFocusedRow);
                reportGridView.FindDetailRowTemplateControl(reportGridView.CurrentFocusedRow, ((sender as CffGridView).ID));
                reportGridView.DetailRows.CollapseRow(reportGridView.CurrentFocusedRow);
            }
            else {
                reportGridView.DetailRows.CollapseRow(reportGridView.CurrentFocusedRow);
            }
        }

       
        protected void reportGridView_PageIndexChanged(object sender, EventArgs e)
        {
            reportGridView.CurrentPageIndex = reportGridView.PageIndex+1;
            
            if (reportGridView.FocusedRowIndex < 0 && reportGridView.PageIndex==0)
            {
                ((Cff.SaferTrader.Web.UserControls.CffGridView)sender).FocusedRowIndex = 0;
            }
           
            try
            {
                reportGridView.FindVisibleIndexByKeyValue(reportGridView.FocusedRowIndex);
            }
            catch
            {
                if ((reportGridView.CurrentPageIndex != reportGridView.PageIndex+1) && (reportGridView.CurrentPageIndex >=0))
                {
                    reportGridView.FocusedRowIndex = (reportGridView.PageIndex+1)*reportGridView.defaultPageSize;
                    reportGridView.CurrentFocusedRow = reportGridView.FocusedRowIndex;
                }
                else {
                    reportGridView.FocusedRowIndex = reportGridView.CurrentFocusedRow;
                }
              
                reportGridView.FindVisibleIndexByKeyValue(reportGridView.FocusedRowIndex);
            } finally {
            }

            reportGridView.Caption = "Page " + reportGridView.CurrentPageIndex;
            reportGridView.Focus();
            reportGridView.Enabled = true;
        }

        protected void reportGridView_FocusedRowChanged(object sender, EventArgs e)
        {
              
            reportGridView.CurrentFocusedRow = ((Cff.SaferTrader.Web.UserControls.CffGridView)sender).FocusedRowIndex;
            if (IsReportWithNotesLiteral.Text.ToLower().Trim() == "true")
            { //do something
                reportGridView.CurrentPageIndex = reportGridView.PageIndex + 1;
            }
        }

        public void ConfigureAllClientsGridColumns()
        {
            reportGridView.Columns.Clear();
            reportGridView.InsertRightAlignedDataColumn("ClientNumberLabel", "#");
            reportGridView.InsertHyperlinkColumn("ClientName", "Client", "ClientNumber", LinkHelper.NavigateUrlFormatToDashboardForClient);
            InsertCommonColumns();
        }

        public void ConfigureClientGridColumns()
        {
            reportGridView.Columns.Clear();
            InsertCommonColumns();
        }

        public void setReportWithNotes(bool value)
        {
            this.isReportWithNotes = value;
        }

        public override void Export()
        {
            AgedBalancesReport report = ViewState["AgedBalancesReport"] as AgedBalancesReport;
            System.Collections.Generic.IList<AgedBalancesReportRecord> exportRecordWithNotes = new System.Collections.Generic.List<AgedBalancesReportRecord>();

            foreach (AgedBalancesReportRecord rptRecord in report.Records)
            {
                System.Collections.Generic.IList<CustomerNote> cnList = rptRecord.CustNoteList;
                rptRecord.Note = "";
                foreach (CustomerNote cNote in cnList)
                {
                    if (!string.IsNullOrEmpty(cNote.Comment)) {
                        rptRecord.Note += "[" + cNote.CreatedByEmployeeName + "][" + cNote.Created.ToShortDateString() + "]";
                        rptRecord.Note += cNote.Comment + System.Environment.NewLine;
                    }
                }
                
                if (!string.IsNullOrEmpty(rptRecord.Note)) {
                    rptRecord.CustNoteList.Clear();
                }

                exportRecordWithNotes.Add(rptRecord);
            }

            //init columns
            CffGridView exportGrid = new CffGridView(250);
            if (IsReportWithNotesLiteral.Text.ToLower() == "true"){
                exportGrid.SettingsBehavior.AllowSort = true;
                exportGrid.InsertRightAlignedDataColumn("CustomerNumber", "#");
                exportGrid.InsertHyperlinkColumn("CustomerName", "Customer", "Id", LinkHelper.NavigateUrlFormatToDashboardForCustomer);
                exportGrid.InsertCurrencyColumn("CurrentBalance", "Current");
                exportGrid.InsertCurrencyColumn("MonthOldBalance", "Month 1");
                exportGrid.InsertCurrencyColumn("TwoMonthsOldBalance", "Month 2");
                exportGrid.InsertCurrencyColumn("ThreeMonthsOrOlderBalance", "Month 3+");
                exportGrid.InsertCurrencyColumn("Balance");
                exportGrid.InsertDataColumn("NextCallDate", "Next Call");
                exportGrid.InsertMemoColumn("Note", "Note");
                exportGrid.Visible = false;
                
                exportGrid.InsertDataColumn("Email");
                exportGrid.InsertDataColumn("Contact");
                exportGrid.InsertDataColumn("Phone");
                exportGrid.InsertDataColumn("Cell", "Mobile Phone");
                
                exportGrid.TotalSummary.Clear();
                exportGrid.InsertTotalSummaryLabelColumn(0);
                exportGrid.InsertTotalSummarySumColumn("CurrentBalance");
                exportGrid.InsertTotalSummarySumColumn("MonthOldBalance");
                exportGrid.InsertTotalSummarySumColumn("TwoMonthsOldBalance");
                exportGrid.InsertTotalSummarySumColumn("ThreeMonthsOrOlderBalance");
                exportGrid.InsertTotalSummarySumColumn("Balance");

                exportGrid.DataSource = exportRecordWithNotes;
                exportGrid.DataBind();
            }
           
           
            
            if (report != null)
            {
                ExcelDocument document = new ExcelDocument();
                document.HSFFGetSheet.SetColumnWidth(8, 400);
                document.WriteTitle(report.Title);

                Hashtable hashtable = new Hashtable();
                hashtable.Add("Client", "ClientName");
                hashtable.Add("Customer", "CustomerName");

                 if (IsReportWithNotesLiteral.Text.ToLower() == "true") {
                    exportGrid.WriteToExcelDocumentWithReplaceField(document, hashtable);
                 }
                 else {
                    reportGridView.WriteToExcelDocumentWithReplaceField(document, hashtable);
                 }

                document.MoveToNextRow();
                document.MoveToNextRow();
                document.AddCell("Date Viewed");
                document.AddCell(report.DateViewed.ToDateTimeString());

                WriteToResponse(document.WriteToStream(), report.ExportFileName);
            }
        }

      
        void detailGridView_BeforePerformDataSelect(object sender, EventArgs e)
        {
            CffGridView masterGrid = sender as CffGridView;
        }

        public void Display(AgedBalancesReport report)
        {
            if (report != null)
            {
                ViewState.Add("AgedBalancesReport", report);
                reportGridView.DataSource = report.Records;
                if (IsReportWithNotesLiteral.Text.ToLower().Trim() == "true" || isReportWithNotes)
                {
                    reportGridView.Templates.DetailRow = new AgedBalancesNotesDetailRowTemplate("Created");
                    reportGridView.SettingsDetail.ShowDetailRow = true;
                    reportGridView.SettingsDetail.ShowDetailButtons = true;
                    //reportGridView.SettingsDetail.AllowOnlyOneMasterRowExpanded = false;
                }
                else
                {
                    reportGridView.SettingsDetail.ShowDetailRow = false;
                    reportGridView.SettingsDetail.ShowDetailButtons = false;
                }

                reportGridView.DataBind();
                DateViewedLiteral.Text = report.DateViewed.ToDateTimeString();
            }
        }

        public override void ShowAllRecords()
        {
            reportGridView.ShowAllRecords();
        }

        public override void ShowPager()
        {
            reportGridView.ShowPager();
        }

        public override void ResetPaginationAndFocus()
        {
            reportGridView.ResetPaginationAndFocus();
        }

        public override bool IsViewAllButtonRequired()
        {
            return reportGridView.IsViewAllButtonRequired;
        }

        public void Clear()
        {
            ViewState.Add("AgedBalancesReport", null);
            reportGridView.DataSource = null;
            reportGridView.DataBind();
            DateViewedLiteral.Text = string.Empty;
        }
    }
}