using System;
using System.Collections;
using System.Collections.Generic;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;

using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters.ReportPresenters;
using Cff.SaferTrader.Core.ReportManager;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.ReportView;
using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.Reports
{
    public partial class AgedBalances : ReportBasePage, IAgedBalancesView
    {
        public CffGenGridView CffGGVRptNotes;
        protected CffGenGridView cffGGVRptNChild;
     
        protected static string targetName = "";
        private ReportPresenterTemplate presenter;

        public bool isNestedGridExpanded
        {
            get
            {
                object o = ViewState["isNestedGridExpanded"];
                return (o == null ? false : Convert.ToBoolean(o));
            }
            set
            {
                ViewState["isNestedGridExpanded"] = value;
            }
        }


        #region IAgedBalancesView
        public void DisplayReport(AgedBalancesReport report)
        {
            this.DivReportViewerContentPlaceHolder.Visible = true;
            AllClientsReportHelpMessage.Visible = false;
            
            CffGGVRptNotes.NestedSettings.ChildTableWidth = Unit.Percentage(100);
            CffGGVRptNotes.Width = Unit.Percentage(100);

            if (ReportPanel.isReportWithNotes)
            {
                rptNotesPlaceHolder.Visible = true;
                CffGGVRptNotes.RowCommand += ReportNotesGridView_RowCommand;
                if (report != null)
                {
                    ViewState.Add("AgedBalancesReportNotes", report);
                    CffGGVRptNotes.Caption = "Aged Balances Reports With Notes";

                    initReportNotesGridView(); 

                    if (ReportPanel.parentGridNestingState != GridNestingState.None)
                        CffGGVRptNotes.NestedSettings.State = ReportPanel.parentGridNestingState;

                    CffGGVRptNotes.GridBag = report.Records;
                    CffGGVRptNotes.DataSource = report.Records;
                    CffGGVRptNotes.DataBind();
                }
                else if (CffGGVRptNotes.GridBag != null)
                {
                    CffGGVRptNotes.Caption = " ";
                    CffGGVRptNotes.NestedSettings.State = GridNestingState.Nesting;
                    CffGGVRptNotes.DataSource = CffGGVRptNotes.GridBag;
                    CffGGVRptNotes.DataBind();
                }

                if (CffGGVRptNotes.AllowSorting)
                    CffGGVRptNotes.Sort("CustomerName", System.Web.UI.WebControls.SortDirection.Ascending);

                ReportPanel.parentGridNestingState = CffGGVRptNotes.NestedSettings.State;
                ReportPanel.Visible = false;
            }
            else
            {
                rptNotesPlaceHolder.Visible = false;
                ReportPanel.Display(report);
            }
        }

        public void Clear()
        {
            Master.ShowReportViewer();
            AllClientsReportHelpMessage.Visible = true;
            ReportPanel.Clear();
            rptNotesPlaceHolder.Visible = false;
        }

        public void ShowAllClientsView()
        {
            if (RptCheckBox.Checked) {
                ReportPanel.isReportWithNotes = true;
                rptNotesPlaceHolder.Visible = true;
            }
            else {
               rptNotesPlaceHolder.Visible = false;
               ReportPanel.isReportWithNotesInitialized = false;
               ReportPanel.parentGridNestingState = UserControls.gGridViewControls.GridNestingState.None;
               ReportPanel.isReportWithNotes = false;
            }

            ReportPanel.ConfigureAllClientsGridColumns();
            DatePickerRpt.Visible = false;
            AllClientsFilter.Visible = true;
        }

        void ReportNotesGridView_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "Expand" || e.CommandName == "Select")
                {
                    CffGenGridView xGV = sender as CffGenGridView;
                    AgedBalancesReportRecord abr = null;

                    if (e.CommandName == "Select" && (xGV.CurrentPageIndex > 0) && (xGV.NestedSettings.RowIndex != rowIndex))
                    {
                        if (CffGGVRptNotes.isNestedRowExpanded(xGV.NestedSettings.RowIndex))
                            xGV.RemoveExpandedIndex(xGV.NestedSettings.RowIndex);
                        rowIndex = (xGV.PageSize*xGV.CurrentPageIndex) + rowIndex;
                    }

                    bool isNestedRowExpanded = CffGGVRptNotes.isNestedRowExpanded(rowIndex);
                    CffGGVRptNotes.NestedSettings.RowIndex = rowIndex;

                    if (isNestedRowExpanded && e.CommandName == "Select" && CffGGVRptNotes.ExpandedRowIndex == rowIndex)
                    {
                        CffGGVRptNotes.NestedSettings.RowIndex = rowIndex;
                        if (CffGGVRptNotes.DataSource==null)
                            CffGGVRptNotes.DataSource =  CffGGVRptNotes.GridBag;

                        CffGGVRptNotes.DataBind();
                        CffGGVRptNotes.FocusedRowIndex = rowIndex;
                        CffGGVRptNotes.ExpandedRowIndex = -1;
                        CffGGVRptNotes.RemoveExpandedIndex(rowIndex);
                        this.isNestedGridExpanded = false;
                        ReportPanel.parentGridNestingState = UserControls.gGridViewControls.GridNestingState.Nesting;
                        CffGGVRptNotes.InitExpandedIndex();
                    }
                    else if (e.CommandName == "Select" && CffGGVRptNotes.ExpandedRowIndex != rowIndex)
                    {
                        if (xGV.DataSource == null)
                            abr = (AgedBalancesReportRecord)((ViewState["AgedBalancesReportNotes"] as AgedBalancesReport).Records[rowIndex]);
                        else
                            abr = ((xGV.DataSource) as System.Collections.Generic.List<AgedBalancesReportRecord>)[rowIndex];

                        if (((CffGenGridView)sender).NestedSettings.RowIndex >= 0)
                            CffGGVRptNotes.RemoveExpandedIndex(((CffGenGridView)sender).NestedSettings.RowIndex);

                        ((CffGenGridView)sender).DataSource = CffGGVRptNotes.GridBag;
                        ((CffGenGridView)sender).NestedSettings.State = GridNestingState.Nesting;
                        ((CffGenGridView)sender).NestedSettings.RowIndex = rowIndex;
                        CffGGVRptNotes.BindNestedGrid(sender, abr.CustNoteList, false);
                        CffGGVRptNotes.FocusedRowIndex = rowIndex;
                        CffGGVRptNotes.NestedSettings.RowIndex = rowIndex;
                        CffGGVRptNotes.ExpandedRowIndex = rowIndex;
                        CffGGVRptNotes.SetNestedRowExpanded(rowIndex, true);
                        this.isNestedGridExpanded = isNestedRowExpanded;
                        ReportPanel.parentGridNestingState = UserControls.gGridViewControls.GridNestingState.Nested;
                    }
                    else
                    {
                        if (xGV.DataSource == null)
                            abr = (AgedBalancesReportRecord)((ViewState["AgedBalancesReportNotes"] as AgedBalancesReport).Records[rowIndex]);
                        else
                            abr = ((xGV.DataSource) as System.Collections.Generic.List<AgedBalancesReportRecord>)[rowIndex];

                        CffGGVRptNotes.BindNestedGrid(sender, abr.CustNoteList, false);
                        CffGGVRptNotes.FocusedRowIndex = rowIndex;
                        CffGGVRptNotes.NestedSettings.RowIndex = rowIndex;
                        this.isNestedGridExpanded = isNestedRowExpanded;
                        ReportPanel.parentGridNestingState = UserControls.gGridViewControls.GridNestingState.Nested;
                    }

                    CffGGVRptNotes.IsRowCommandPostBack = false; 
                    ReportsUpdatePanel.Update();
                }
            }
            catch (Exception exc) {
                CffGGVRptNotes.Caption = exc.Message;
            }
        }


        public void ShowClientView()
        {
            if (RptCheckBox.Checked)
            {
                ReportPanel.isReportWithNotes = true;
            }
            else
            {
                ReportPanel.isReportWithNotesInitialized = false;
                ReportPanel.parentGridNestingState = UserControls.gGridViewControls.GridNestingState.None;
                ReportPanel.isReportWithNotes = false;
            }
            ReportPanel.ConfigureClientGridColumns();

            DatePickerRpt.Visible = true;
            AllClientsFilter.Visible = false;
        }

        public Date DateAsAt()
        {
           return DatePickerRpt.Date;
        }

        public int ClientId()
        {
            if (SessionWrapper.Instance.Get != null)
            {
                if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
                    return SessionWrapper.Instance.Get.ClientFromQueryString.Id;
            }
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
            {
                if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString != null)
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id;
            }
            
            return (QueryString.ClientId==null)?0:(int)QueryString.ClientId;
        }

        public int ClientFacilityType()
        {
            if (SessionWrapper.Instance.Get != null)
            {
                if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
                    return SessionWrapper.Instance.Get.ClientFromQueryString.ClientFacilityType;
            }
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
            {
                if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString != null)
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.ClientFacilityType;
            }

            return 0;
        }


        public AgedBalancesReportType ReportType()
        {
            string selectedReportValue = ReportTypeDropDownList.SelectedValue;
            if (ReportPanel.isReportWithNotes)
            {
                selectedReportValue = selectedReportValue.Substring(0, (selectedReportValue.Length - 1));
                selectedReportValue += "1";
            }

            return AgedBalancesReportType.Instantiate(Convert.ToInt32(selectedReportValue));
        }

        public FacilityType FacilityType()
        {
            return AllClientsFilter.FacilityType;
        }

        public bool IsSalvageIncluded()
        {
            return AllClientsFilter.IsSalvageIncluded;
        }

        #endregion


        private void AddJavaScriptIncludeInHeader(string path)
        {  /// Adds a JavaScript reference to the header of the document.
            var script = new HtmlGenericControl("script");
            script.Attributes["type"] = "text/javascript";
            script.Attributes["src"] = ResolveUrl(path);
            Page.Header.Controls.Add(script);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //initialize CffGGVRptNotes
            CffGGVRptNotes = new CffGenGridView();
            CffGGVRptNotes.AllowColumnResizing = true;
            CffGGVRptNotes.AllowPaging = true;
            CffGGVRptNotes.AllowSorting = true;
            CffGGVRptNotes.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            CffGGVRptNotes.AutoGenerateColumns = false;
            CffGGVRptNotes.AutoGenerateDeleteButton = false;
            CffGGVRptNotes.AutoGenerateEditButton = false;
            CffGGVRptNotes.AutoGenerateSelectButton = false;
            CffGGVRptNotes.PageSize = 250;
            CffGGVRptNotes.DefaultPageSize = 250;
            CffGGVRptNotes.Visible = true;
            CffGGVRptNotes.EnableViewState = true;
            CffGGVRptNotes.Caption = "Aged Balances Report with Notes";
            CffGGVRptNotes.HeaderStyle.CssClass = "cffGGVHeader";
            CffGGVRptNotes.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            CffGGVRptNotes.PagerSettings.Mode = PagerButtons.Numeric;

            CffGGVRptNotes.AutoGenerateColumns = false;
            CffGGVRptNotes.AllowSorting = true;
            CffGGVRptNotes.SetSortExpression = "YrMonth";

            CffGGVRptNotes.CaptionHeaderSettings.BoldCaption = true;
            CffGGVRptNotes.Width = Unit.Percentage(105);
            CffGGVRptNotes.BorderWidth = Unit.Pixel(1);
            CffGGVRptNotes.FooterStyle.BorderWidth = Unit.Pixel(1);

            initReportNotesGridView();
            initReportNotesGridViewNestedSettings();
            initReportNotesNestedGridView();

            rptNotesPlaceHolder.Controls.Clear();
            rptNotesPlaceHolder.Controls.Add(CffGGVRptNotes);
        }


        private void initReportNotesNestedGridView()
        {
            //start child grid initialization
            cffGGVRptNChild = new CffGenGridView();
            cffGGVRptNChild.ID = "cffGGVRptNChild";
            cffGGVRptNChild.AllowSorting = true;
            cffGGVRptNChild.AutoGenerateColumns = false;
            cffGGVRptNChild.SetSortExpression = "Date";

            cffGGVRptNChild.HeaderStyle.CssClass = "cffGGVHeader";
            cffGGVRptNChild.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            cffGGVRptNChild.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

            cffGGVRptNChild.BorderColor = System.Drawing.Color.AliceBlue;
            cffGGVRptNChild.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;

            //note: for nested grid leave some space for maximize/minimize button
            cffGGVRptNChild.Columns.Clear();
            cffGGVRptNChild.InsertDataColumn("Created By", "CreatedByEmployeeName", CffGridViewColumnType.Text, "6%", "CffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            cffGGVRptNChild.InsertDataColumn("Comment", "Comment", CffGridViewColumnType.Memo, "58%", "CffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            cffGGVRptNChild.InsertDataColumn("Created", "Created", CffGridViewColumnType.Text, "6%", "CffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            cffGGVRptNChild.EnableViewState = true;
            cffGGVRptNChild.NestedSettings.Enabled = false;
            cffGGVRptNChild.ShowFooter = false;
            cffGGVRptNChild.Width = Unit.Percentage(70);
            cffGGVRptNChild.BorderWidth = Unit.Pixel(1);
         
            //set nested settings
            CffGGVRptNotes.NestedSettings.Enabled = true;
            CffGGVRptNotes.NestedSettings.Expanded = false;
            CffGGVRptNotes.NestedSettings.ExpandingButtonHeight = System.Web.UI.WebControls.Unit.Pixel(10);
            CffGGVRptNotes.NestedSettings.ExpandingButtonWidth = System.Web.UI.WebControls.Unit.Pixel(10);
            CffGGVRptNotes.NestedSettings.ExpandingColumnWidth = System.Web.UI.WebControls.Unit.Percentage(2);
            CffGGVRptNotes.NestedSettings.childGrid = cffGGVRptNChild;
            CffGGVRptNotes.NestedSettings.childGrid.HorizontalAlign = HorizontalAlign.Left;
            CffGGVRptNotes.NestedSettings.ChildTableWidth = Unit.Percentage(100);

            //somehow this goes missing
            CffGGVRptNotes.TotalsSummarySettings.SetColumnTotals("CurrentBalance, MonthOldBalance, TwoMonthsOldBalance, ThreeMonthsOrOlderBalance, Balance");
            CffGGVRptNotes.TotalsSummarySettings.TotalsTextStyle = "cffGGV_TotalsColumnStyle";
            CffGGVRptNotes.TotalsSummarySettings.TotalColumnsRowStyle = "cffGGV_TotalsColumnStyle";
            CffGGVRptNotes.TotalsSummarySettings.SetTotalsColumnCssStyle("CurrentBalance", "cffGGV_currencyCell");
            CffGGVRptNotes.TotalsSummarySettings.SetTotalsColumnCssStyle("MonthOldBalance", "cffGGV_currencyCell");
            CffGGVRptNotes.TotalsSummarySettings.SetTotalsColumnCssStyle("TwoMonthsOldBalance", "cffGGV_currencyCell");
            CffGGVRptNotes.TotalsSummarySettings.SetTotalsColumnCssStyle("ThreeMonthsOrOlderBalance", "cffGGV_currencyCell");
            CffGGVRptNotes.TotalsSummarySettings.SetTotalsColumnCssStyle("Balance", "cffGGV_currencyCell");
            CffGGVRptNotes.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
        }

        private void initReportNotesGridView() 
        {
           //start parent grid initialization
           CffGGVRptNotes.Columns.Clear();
           CffGGVRptNotes.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
           CffGGVRptNotes.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
           CffGGVRptNotes.InsertCurrencyColumn("Current", "CurrentBalance", "6%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
           CffGGVRptNotes.InsertCurrencyColumn("Month 1", "MonthOldBalance", "6%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
           CffGGVRptNotes.InsertCurrencyColumn("Month 2", "TwoMonthsOldBalance", "6%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
           CffGGVRptNotes.InsertCurrencyColumn("Month 3+", "ThreeMonthsOrOlderBalance", "6%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
           CffGGVRptNotes.InsertCurrencyColumn("Balance", "Balance", "7%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
           //CffGGVRptNotes.InsertDataColumn("Next Call", "NextCallDate", CffGridViewColumnType.Text, "6%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGVHeaderLeftAgedBal1");
           //CffGGVRptNotes.InsertDataColumn("Email", "Email", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGVHeaderLeftAgedBal1");
           CffGGVRptNotes.InsertDataColumn("Next Call", "NextCallDate", CffGridViewColumnType.Text, "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
           CffGGVRptNotes.InsertDataColumn("Email", "Email", CffGridViewColumnType.Text, "4%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
           CffGGVRptNotes.InsertDataColumn("Contact", "Contact", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
           CffGGVRptNotes.InsertDataColumn("Phone", "Phone", CffGridViewColumnType.Text, "4%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
           CffGGVRptNotes.InsertDataColumn("Cell", "Cell", CffGridViewColumnType.Text, "6%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
           //CffGGVRptNotes.InsertDataColumn("Cell", "Cell", CffGridViewColumnType.Text, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGVHeaderLeftAgedBal1");
           CffGGVRptNotes.InsertDataColumn("Note", "Note", CffGridViewColumnType.Memo, "10%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
           CffGGVRptNotes.Columns[12].Visible = false;

           //set summary settings
           CffGGVRptNotes.TotalsSummarySettings.SetColumnTotals("CurrentBalance, MonthOldBalance, TwoMonthsOldBalance, ThreeMonthsOrOlderBalance, Balance");
           CffGGVRptNotes.TotalsSummarySettings.SetTotalsColumnCssStyle("CurrentBalance", "cffGGV_currencyTotalTrxArch");
           CffGGVRptNotes.TotalsSummarySettings.SetTotalsColumnCssStyle("MonthOldBalance", "cffGGV_currencyTotalTrxArch");
           CffGGVRptNotes.TotalsSummarySettings.SetTotalsColumnCssStyle("TwoMonthsOldBalance", "cffGGV_currencyTotalTrxArch");
           CffGGVRptNotes.TotalsSummarySettings.SetTotalsColumnCssStyle("ThreeMonthsOrOlderBalance", "cffGGV_currencyTotalTrxArch");
           CffGGVRptNotes.TotalsSummarySettings.TableClass = "cffGGV_TotalSummaryTable";
        }

        private void initReportNotesGridViewNestedSettings()
        {
            CffGGVRptNotes.NestedSettings.BoundColumnName = "Note";
            CffGGVRptNotes.NestedSettings.BoundColumnFilterObject = HtmlTextWriterStyle.Display;
            CffGGVRptNotes.NestedSettings.BoundColumnFilterValue = "none;";
            CffGGVRptNotes.NestedSettings.ExpandingButtonCssStyle = "cffGGV_AgedBalExpBtnCssStyle";
            CffGGVRptNotes.NestedSettings.ExpandingButtonAltText = "Expand to see Notes details";

            CffGGVRptNotes.EnableViewState = true;
            CffGGVRptNotes.ShowFooter = true;
            CffGGVRptNotes.AllowPaging = true;
            CffGGVRptNotes.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;
            CffGGVRptNotes.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
        }
       
        private void InitializeCurrentPathForJavaScript()
        {
            string relativePathToRoot = RelativePathComputer.ComputeRelativePathToRoot(Server.MapPath("~"),
                                                                                       Server.MapPath("."));
            string script = string.Format(@"var relativePathToRoot='{0}';", relativePathToRoot);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "initializeCurrentPath", script, true);
        }
     

        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeCurrentPathForJavaScript();
            presenter = new AgedBalancesPresenter(this, ReportsService.Create(),
                               ReportManagerFactory.Create(SessionWrapper.Instance.Scope, Context.User as CffPrincipal));

            if (!this.IsPostBack) {
                // start related ref:CFF-18
                ICffClient xClient = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString :
                    (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString : null;
                if (xClient != null)
                    targetName = ": " + xClient.Name;

                ICffCustomer xCustomer = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.CustomerFromQueryString :
                    (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString : null;
                if (xCustomer != null)
                {
                    if (targetName != null || !targetName.Equals(""))
                    {
                        targetName += " / ";
                        targetName = string.Concat(targetName, xCustomer.Name);
                    }
                    else
                    {
                        targetName = ": " + xCustomer.Name;
                    }
                }

                ReportTypeDropDownList.DataSource = AgedBalancesReportType.KnownTypes;
                ReportTypeDropDownList.DataTextField = "Text";
                ReportTypeDropDownList.DataValueField = "Id";
                ReportTypeDropDownList.DataBind();

                presenter.ConfigureView(this.CurrentScope());
                presenter.ShowReport(this.CurrentScope(), true);
                ReportPanel.isReportWithNotes = false;
                ReportPanel.isReportWithNotesInitialized = false;
            }
            else
            {
                if (RptCheckBox.Checked)
                {
                    rptNotesPlaceHolder.Visible = true;
                    ReportPanel.isReportWithNotes = true;
                    ReportPanel.isReportWithNotesInitialized = true;

                    bool bIsControlPostBack = false;
                    string pbcid = GetPostBackControlId(this.Page, ref bIsControlPostBack);
                    int expandedRowIndex = CffGGVRptNotes.ExpandedRowIndex;
                    bool isExpanded = CffGGVRptNotes.isNestedRowExpanded(expandedRowIndex);
                    this.isNestedGridExpanded = isExpanded;

                    CffGGVRptNotes.RowCommand += ReportNotesGridView_RowCommand;
                    CffGGVRptNotes.PagerCommand += CffGGVRptNotes_PagerCommand;
                    
                    if (CffGGVRptNotes.Columns.Count==0)
                        initReportNotesGridView();

                    
                    initReportNotesGridViewNestedSettings();
                    initReportNotesNestedGridView();

                    CffGGVRptNotes.Columns[(CffGGVRptNotes.Columns.Count-1)].Visible = false;
                    CffGGVRptNotes.ExpandedRowIndex = expandedRowIndex;

                    if (expandedRowIndex >=0)
                        CffGGVRptNotes.SetNestedRowExpanded(expandedRowIndex, isExpanded);

                    CffGGVRptNotes.NestedSettings.Enabled = true;
                    if (ReportPanel.parentGridNestingState != GridNestingState.None)
                    {
                        CffGGVRptNotes.NestedSettings.State = ReportPanel.parentGridNestingState;
                        CffGGVRptNotes.Width = Unit.Percentage(100);

                        bool isAsynPost = (this.Page.Request.Params.Get("__ASYNCPOST") == null) ? false : ((this.Page.Request.Params.Get("__ASYNCPOST").ToString() == "true") ? true : false);

                        if (CffGGVRptNotes.IsRowCommandPostBack && isAsynPost)
                        {
                            if (!string.IsNullOrEmpty(pbcid))
                            {
                                if (pbcid.Contains("CffGGVRptNotes"))
                                {
                                    CffGGVRptNotes.NestedSettings.RowIndex = CffGGVRptNotes.FocusedRowIndex;
                                    if (!this.isNestedGridExpanded)
                                    {
                                        ReportPanel.parentGridNestingState = GridNestingState.Nesting;
                                        CffGGVRptNotes.NestedSettings.State = ReportPanel.parentGridNestingState;

                                        if (CffGGVRptNotes.DataSource == null)
                                            CffGGVRptNotes.DataSource = CffGGVRptNotes.GridBag;

                                        CffGGVRptNotes.BindNestedGrid(CffGGVRptNotes,
                                              (((CffGGVRptNotes.DataSource) as System.Collections.Generic.List<AgedBalancesReportRecord>)[CffGGVRptNotes.FocusedRowIndex]).CustNoteList,
                                                false);
                                    }
                                    else
                                    {
                                        CffGGVRptNotes.NestedSettings.State = ReportPanel.parentGridNestingState;
                                        CffGGVRptNotes.NestedSettings.Expanded = this.isNestedGridExpanded;
                                        if (CffGGVRptNotes.DataSource == null)
                                            CffGGVRptNotes.DataSource = CffGGVRptNotes.GridBag;
                                        CffGGVRptNotes.DataBind();
                                        ReportPanel.parentGridNestingState = GridNestingState.Nesting;
                                    }

                                    if (this.Page.Request.Params.Get("__ASYNCPOST") == "true") {
                                        CffGGVRptNotes.IsRowCommandPostBack = true;
                                    } else {
                                        CffGGVRptNotes.IsRowCommandPostBack = false;
                                        this.isNestedGridExpanded = !this.isNestedGridExpanded;
                                    }
                                    ReportsUpdatePanel.Update();
                                }
                                else if (isAsynPost) {
                                    if (CffGGVRptNotes.ExpandedRowIndex >= 0)
                                        CffGGVRptNotes.InitExpandedIndex();

                                    if (CffGGVRptNotes.DataSource == null)
                                        CffGGVRptNotes.DataSource = CffGGVRptNotes.GridBag;
                                    CffGGVRptNotes.DataBind();
                                    ReportsUpdatePanel.Update();
                                }
                            }
                            else if (!pbcid.ToLower().Contains("updatebutton")) {
                                CffGGVRptNotes.ExpandedRowIndex = -1;
                                CffGGVRptNotes.InitExpandedIndex();

                                if (CffGGVRptNotes.DataSource == null)
                                    CffGGVRptNotes.DataSource = CffGGVRptNotes.GridBag;
                                
                                ReportPanel.parentGridNestingState = GridNestingState.Nesting;
                                this.isNestedGridExpanded = false;

                                if (this.Page.Request.Params.Get("__ASYNCPOST") == "true")
                                {
                                    CffGGVRptNotes.DataBind();
                                    ReportsUpdatePanel.Update();
                                }
                                else {
                                    CffGGVRptNotes.IsRowCommandPostBack = false;
                                }
                            }
                        }
                        else if (pbcid!=null) {
                            if (pbcid.ToLower().Contains("updatebutton") || pbcid.Contains("ExportButton"))
                            {
                                if (pbcid.Contains("ExportButton")) {
                                    CffGGVRptNotes.Columns[(CffGGVRptNotes.Columns.Count - 1)].Visible = true; //make notes column visible so we can include in export
                                }

                                CffGGVRptNotes.ExpandedRowIndex = -1;
                                CffGGVRptNotes.InitExpandedIndex();
                         
                                if (CffGGVRptNotes.DataSource == null)
                                    CffGGVRptNotes.DataSource = CffGGVRptNotes.GridBag;
                                CffGGVRptNotes.DataBind();
                                ReportPanel.parentGridNestingState = GridNestingState.Nesting;
                                this.isNestedGridExpanded = false;
                                CffGGVRptNotes.IsRowCommandPostBack = false;
                                ReportsUpdatePanel.Update();
                            }
                            else if (pbcid.Contains("CffGGVRptNotes") 
                                        && (this.Page.Request.Params.Get("__ASYNCPOST")=="true"))
                            {
                                if (!bIsControlPostBack) {
                                    if (CffGGVRptNotes.DataSource == null)
                                        CffGGVRptNotes.DataSource = CffGGVRptNotes.GridBag;
                                    CffGGVRptNotes.DataBind();
                                    ReportPanel.parentGridNestingState = GridNestingState.Nesting;
                                    CffGGVRptNotes.IsRowCommandPostBack = false;
                                    ReportsUpdatePanel.Update();
                                }
                            }
                            else if (isAsynPost && !CffGGVRptNotes.IsRowCommandPostBack)
                            { //check if pager postback
                                CffGGVRptNotes.NestedSettings.Enabled = true;
                                if (CffGGVRptNotes.DataSource == null)
                                    CffGGVRptNotes.DataSource = CffGGVRptNotes.GridBag;
                                ReportPanel.parentGridNestingState = GridNestingState.Nesting;

                                if (!this.isNestedGridExpanded) {
                                    this.isNestedGridExpanded = false;
                                    CffGGVRptNotes.ExpandedRowIndex = -1;
                                    CffGGVRptNotes.InitExpandedIndex();
                                    initReportNotesGridViewNestedSettings();
                                    initReportNotesNestedGridView();
                                } 
                               
                                CffGGVRptNotes.DataBind();
                                CffGGVRptNotes.IsRowCommandPostBack = false;
                                ReportsUpdatePanel.Update();
                            }
                        }
                        else if (this.Page.Request.Params.Get("__ASYNCPOST") == "true"
                                && !CffGGVRptNotes.IsRowCommandPostBack)
                        { //check if pager postback
                            CffGGVRptNotes.NestedSettings.Enabled = true;
                            if (CffGGVRptNotes.DataSource == null)
                                CffGGVRptNotes.DataSource = CffGGVRptNotes.GridBag;
                            CffGGVRptNotes.DataBind();
                            ReportPanel.parentGridNestingState = GridNestingState.Nesting;
                            this.isNestedGridExpanded = false;
                            CffGGVRptNotes.IsRowCommandPostBack = false;
                            ReportsUpdatePanel.Update();
                        }
                    }
                    
                }
                else
                {
                    rptNotesPlaceHolder.Visible = false;
                    ReportPanel.Visible = true;
                    ReportPanel.isReportWithNotesInitialized = false;
                    ReportPanel.parentGridNestingState = UserControls.gGridViewControls.GridNestingState.None;
                    ReportPanel.isReportWithNotes = false;
                }
           
                if (ReportPanel.isReportWithNotes && ReportPanel.isReportWithNotesInitialized)
                {  //no need to reconfigure the report panel grid columns
                    ReportPanel.Visible = false;
                }
                else
                {
                    if (CurrentScope() == Scope.AllClientsScope) 
                       ReportPanel.ConfigureAllClientsGridColumns();
                     else
                       ReportPanel.ConfigureClientGridColumns();
                }
             }
   
            this.DateViewedLiteral.Text = DateTime.Now.ToShortDateString();
        }

        void CffGGVRptNotes_PagerCommand(object sender, GridViewCommandEventArgs e)
        {
            string cmd = e.CommandName;
            int PageIdx = Convert.ToInt32(e.CommandArgument);
            if (cmd.Contains("Page")) {
                if (this.isNestedGridExpanded && CffGGVRptNotes.ExpandedRowIndex >= 0) {
                    this.isNestedGridExpanded = false;
                    CffGGVRptNotes.RemoveExpandedIndex(CffGGVRptNotes.ExpandedRowIndex);
                }

                (sender as CffGenGridView).PagerSettings.Mode = PagerButtons.Numeric;
                CffGGVRptNotes.PagerSettings.Mode = PagerButtons.Numeric;
                CffGGVRptNotes.InitExpandedIndex();
                if (cmd == "RowsPerPage")
                {
                    CffGGVRptNotes.IsRowCommandPostBack = false;
                    CffGGVRptNotes.PageIndex = 0;
                }
                else
                {
                    CffGGVRptNotes.PageIndex = PageIdx;
                    (sender as CffGenGridView).PageIndex = PageIdx;
                }

                if (CffGGVRptNotes.DataSource == null)
                    CffGGVRptNotes.DataSource = CffGGVRptNotes.GridBag;

                if ((sender as CffGenGridView).DataSource == null)
                    (sender as CffGenGridView).DataSource = CffGGVRptNotes.DataSource;

                if (CffGGVRptNotes.Columns.Count == 0)
                {
                    initReportNotesGridView();
                    initReportNotesNestedGridView();
                    initReportNotesGridViewNestedSettings();
                }

                CffGGVRptNotes.DataBind();
                if (cmd == "Page") {
                    (sender as CffGenGridView).PageIndex = PageIdx;
                    CffGGVRptNotes.PageIndex = PageIdx;
                }
            }
        }


        protected void ExportButton_Click(object sender, ImageClickEventArgs e)
        {
            if (ReportPanel.isReportWithNotes)
            {
                if (this.CurrentScope()==Scope.AllClientsScope) 
                    CffGGVRptNotes.Export("Aged Balances Report for All Clients ");
                else
                    CffGGVRptNotes.Export("Aged Balances Report for " + SessionWrapper.Instance.Get.ClientFromQueryString.Name);
            }
            else
                ReportPanel.Export();
        }

        protected void UpdateButtonClick(object sender, ImageClickEventArgs e)
        {
            if (RptCheckBox.Checked)
            {
                ReportPanel.isReportWithNotes = true;
                ReportTypeDropDownList.DataSource = AgedBalancesReportType.KnownTypesWithNotes;
                initReportNotesGridView();
                initReportNotesNestedGridView();
                if (CffGGVRptNotes.NestedSettings.RowIndex >= 0)  
                {
                    CffGGVRptNotes.RemoveExpandedIndex(CffGGVRptNotes.NestedSettings.RowIndex);
                }

                CffGGVRptNotes.PageIndex = 0;
                CffGGVRptNotes.ExpandedRowIndex = -1;
                CffGGVRptNotes.NestedSettings.RowIndex = -1;
                ReportPanel.parentGridNestingState = GridNestingState.Init;
            }
            else
            {
                ReportPanel.isReportWithNotesInitialized = false;
                ReportPanel.isReportWithNotes = false;
                ReportPanel.parentGridNestingState = UserControls.gGridViewControls.GridNestingState.None;
                ReportTypeDropDownList.DataSource = AgedBalancesReportType.KnownTypes;
            }

            ReportTypeDropDownList.DataTextField = "Text";
            ReportTypeDropDownList.DataValueField = "Id";
            ReportPanel.ResetPaginationAndFocus();

            presenter.ConfigureView(this.CurrentScope());
            presenter.ShowReport(this.CurrentScope(), false);


            if (RptCheckBox.Checked)
            { //so that we could expand first occurence of notes with data
                if (CffGGVRptNotes.DataSource != null)
                {
                    int firstRowWithData = 0;
                    AgedBalancesReportRecord xChildDta = null;
                    System.Array xDtaLst = ((CffGGVRptNotes.DataSource) as System.Collections.Generic.List<object>).ToArray();
                    foreach (AgedBalancesReportRecord xRpt in xDtaLst) 
                    {
                        if (xRpt != null) {
                            if (!string.IsNullOrEmpty(xRpt.Note)) {
                                xChildDta = xRpt;
                                break;
                            }
                        }
                        firstRowWithData++;
                    }

                    if (xChildDta != null) {
                        if (CffGGVRptNotes.NestedSettings.childGrid == null)
                            initReportNotesNestedGridView();

                        CffGGVRptNotes.NestedSettings.ExpandingButtonCssStyle = "cffGGV_AgedBalExpBtnCssStyle";
                        CffGGVRptNotes.NestedSettings.ExpandingButtonAltText = "Expand to see Notes details";

                        CffGGVRptNotes.RemoveExpandedIndex(firstRowWithData);
                        CffGGVRptNotes.NestedSettings.RowIndex = firstRowWithData;

                        CffGGVRptNotes.DataSource = CffGGVRptNotes.GridBag;
                        CffGGVRptNotes.NestedSettings.State = GridNestingState.Nesting;
                        CffGGVRptNotes.BindNestedGrid(CffGGVRptNotes, xChildDta.CustNoteList, false);
                        CffGGVRptNotes.FocusedRowIndex = firstRowWithData;
                        CffGGVRptNotes.NestedSettings.RowIndex = firstRowWithData;
                        CffGGVRptNotes.ExpandedRowIndex = firstRowWithData;
                        CffGGVRptNotes.SetNestedRowExpanded(firstRowWithData, true);
                        this.isNestedGridExpanded = true;
                        ReportPanel.parentGridNestingState = UserControls.gGridViewControls.GridNestingState.Nested;
                        ReportsUpdatePanel.Update();
                    }
                }
            }

            if (RptCheckBox.Checked)
                this.UpdateButton.Attributes.Add("autopostback", "true");

        }

        private void findControls(WebControl c)
        {
            foreach (WebControl cntrl in c.Controls)
            {
                if (cntrl.Controls.Count > 0)
                    findControls(cntrl);
                else if (cntrl.GetType() == typeof(Button))
                {
                    if (cntrl.ID == "Btn1")
                    {
                        //Btn1 = cntrl as Button;
                    }
                }
            }
        }

        private string GetPostBackControlId(Page pg, ref bool isControlPostBack)
        {
            if (!pg.IsPostBack)
                return string.Empty;

            isControlPostBack = false;
            System.Web.UI.Control control = null;
            
            // first we will check the "__EVENTTARGET" because if post back made by the controls
            // which used "_doPostBack" function also available in Request.Form collection.
            string controlName = pg.Request.Params["__EVENTTARGET"];
            if (!String.IsNullOrEmpty(controlName))
            {
                control = pg.FindControl(controlName);
                isControlPostBack = true;
            }
            else
            {
                // if __EVENTTARGET is null, the control is a button type and we need to
                // iterate over the form collection to find it
                string controlId;
                System.Web.UI.Control foundControl;

                foreach (string ctl in pg.Request.Form)
                {
                    if (!string.IsNullOrEmpty(ctl)) {
                        // handle ImageButton they having an additional "quasi-property" 
                        // in their Id which identifies mouse x and y coordinates
                        if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                        {
                            controlId = ctl.Substring(0, ctl.Length - 2);

                            if (controlId.Contains("CffGGVRptNotes"))
                                return "CffGGVRptNotes";

                            if (controlId.Contains("UpdateButton"))
                                return "UpdateButton";

                            foundControl = pg.FindControl(controlId);
                        }
                        else
                        {
                            foundControl = pg.FindControl(ctl);
                        }

                        if (!(foundControl is Button || foundControl is ImageButton))
                        {
                            if (foundControl == null)
                                continue;

                            if (foundControl.ClientID.Contains("ExportButton"))
                            {
                                control = foundControl;
                                break;
                            }
                            continue;
                        }

                        control = foundControl;
                        break;
                    }
                }
            }
            return control == null ? String.Empty : control.ID;
        }

        protected void RptCheckBox_CheckedChanged(Object sender, EventArgs args)
        {
            if (RptCheckBox.Checked)
            {
                ReportPanel.isReportWithNotes = true;
                ReportTypeDropDownList.DataSource = AgedBalancesReportType.KnownTypesWithNotes;
                initReportNotesGridView();
                initReportNotesNestedGridView();
                if (CffGGVRptNotes.NestedSettings.RowIndex >= 0)
                    CffGGVRptNotes.RemoveExpandedIndex(CffGGVRptNotes.NestedSettings.RowIndex);
                CffGGVRptNotes.ExpandedRowIndex = -1;
                CffGGVRptNotes.NestedSettings.RowIndex = -1;
                ReportPanel.parentGridNestingState = GridNestingState.Init;
            }
            else
            {
                ReportPanel.isReportWithNotesInitialized = false;
                ReportPanel.isReportWithNotes = false;
                ReportPanel.parentGridNestingState = UserControls.gGridViewControls.GridNestingState.None;
                ReportTypeDropDownList.DataSource = AgedBalancesReportType.KnownTypes;
            }                     
            
            ReportTypeDropDownList.DataTextField = "Text";
            ReportTypeDropDownList.DataValueField = "Id";
            ReportTypeDropDownList.DataBind();           

            ReportPanel.ResetPaginationAndFocus();
            if (presenter != null) {
                presenter.ConfigureView(SessionWrapper.Instance.Scope);
                presenter.ShowReport(SessionWrapper.Instance.Scope, false);           
            }
            
        }
        
        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);
            presenter.ConfigureView(SessionWrapper.Instance.Scope);
            presenter.ShowReport(SessionWrapper.Instance.Scope, true);
        }

        protected void ReportTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            initReportNotesGridView();
            initReportNotesNestedGridView();
            ReportPanel.parentGridNestingState = GridNestingState.Init;
            CffGGVRptNotes.NestedSettings.State = GridNestingState.Init;
            CffGGVRptNotes.NestedSettings.Expanded = false;
            CffGGVRptNotes.NestedSettings.RowIndex = -1;
            CffGGVRptNotes.ExpandedRowIndex = -1;
            CffGGVRptNotes.InitExpandedIndex();
            ScopeChanged(sender, e);
        }

        protected void DatePickerRpt_Update(object sender, EventArgs e)
        {
            bool bIsControlPostBack = false;
            string pbcid = GetPostBackControlId(this.Page, ref bIsControlPostBack);
            if (pbcid.Contains("ToDropDownList")) {
                initReportNotesGridView();
                initReportNotesNestedGridView();
                ScopeChanged(sender, e);
                ReportPanel.parentGridNestingState = GridNestingState.Nesting;
                CffGGVRptNotes.NestedSettings.State = GridNestingState.Nesting;
                CffGGVRptNotes.NestedSettings.Expanded = false;
                CffGGVRptNotes.NestedSettings.RowIndex = -1;
                CffGGVRptNotes.ExpandedRowIndex = -1;
                CffGGVRptNotes.InitExpandedIndex();
                CffGGVRptNotes.IsRowCommandPostBack = false;
                ReportsUpdatePanel.Update();
            }
        }
    }
}