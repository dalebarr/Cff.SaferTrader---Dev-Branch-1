using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.ReleaseTabs;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web
{
    public partial class RetentionSchedules : BasePage, IRetentionSchedulesView
    {
        //protected static string targetName = "";
        protected static string varRepurchases = "";
        
        private RetentionSchedulePresenter presenter;
        public CffGenGridView RetentionGridView;
        
        private static bool showRetentionGridView = true;
        private static string _retnSchedHeader;

        private string eventTarget = "";

        public static string CustomerIdQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.Customer.ToString()]; }
        }

        public static string ClientIdQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.Client.ToString()]; }
        }

        public static string ViewIDQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.ViewID.ToString()]; }
        }

        public static string QueryStringParameters
        {
            get
            {
                if (!string.IsNullOrEmpty(ClientIdQueryString) && !string.IsNullOrEmpty(CustomerIdQueryString))
                {
                    return "?Client=" + ClientIdQueryString + "&Customer=" + CustomerIdQueryString + "&ViewID=" + ViewIDQueryString;
                }

                if (!string.IsNullOrEmpty(ClientIdQueryString) && string.IsNullOrEmpty(CustomerIdQueryString))
                {
                    return "?Client=" + ClientIdQueryString + "&ViewID=" + ViewIDQueryString;
                }

                return "?Client=-1";
            }
        }

        #region IRetentionSchedulesView Members

        public void DisplayRetentionSchedules(IList<RetentionSchedule> retentionSchedules)
        {
            ViewState.Add("RetentionSchedules", retentionSchedules);

            try
            { //Ref: CFF-21
                if (retentionSchedules != null) 
                {
                    int selectedIndex = RetentionGridView.FocusedRowIndex;


                    RetentionGridView.DataSource = retentionSchedules;
                    RetentionGridView.DataBind();
                    if (RetentionGridView.Rows.Count > 0) 
                    {
                        RetentionGridView.FocusedRowIndex = (selectedIndex<0)?0:selectedIndex;
                    }
                }
            }
            catch (Exception exc)
            {
                string excep = exc.Message;
                throw exc.InnerException;
            }
        }

        #endregion

        protected override void Page_Init(object sender, EventArgs e)
        {
            base.Page_Init(sender, e);
            RetentionGridView = new CffGenGridView();
            RetentionGridView.AutoGenerateColumns = false;
            RetentionGridView.ID = "retentionGridView";
            RetentionGridView.KeyFieldName = "Id";

            //RetentionGridView.CssClass = "cffGGV";
            RetentionGridView.HeaderStyle.CssClass = "cffGGVHeader";
            RetentionGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            RetentionGridView.ShowHeaderWhenEmpty = true;
            RetentionGridView.EmptyDataText = "No data to display";
            RetentionGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            RetentionGridView.RowCssClass = "dxgvDataRow";

            RetentionGridView.BorderWidth = Unit.Pixel(1);
            RetentionGridView.Width = Unit.Percentage(100);

            RetentionGridView.DefaultPageSize = 15;
            RetentionGridView.PageSize  = 15;
            RetentionGridView.AllowPaging = true;
            RetentionGridView.ShowFooter = false;
            RetentionGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;
            RetentionGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
            RetentionGridView.EnableViewState = true;

            gridViewPlaceholder.Controls.Clear();
            gridViewPlaceholder.Controls.Add(RetentionGridView);

            PreviousRetentionButton.Visible = true;
            NextRetentionButton.Visible = true;

            InvoiceBatchesLink.HRef = "~/InvoiceBatches.aspx" + QueryStringParameters;
        }

        public static Control GetPostBackControl(Page page, out string controlArgument)
        {
            Control control = null;
            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            controlArgument = page.Request.Params.Get("__EVENTARGUMENT");

            if (ctrlname != null && ctrlname != String.Empty) {
                control = page.FindControl(ctrlname);
            } else {
                foreach (string ctl in page.Request.Form)
                {
                    if (ctl != null) {
                        Control c = page.FindControl(ctl);
                        if (c is System.Web.UI.WebControls.Button) {
                            control = c;
                            break;
                        }
                    }
                }
            }
            return control;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int selectedIndex = RetentionGridView.FocusedRowIndex;

            if (Request.UserAgent.IndexOf("AppleWebKit") > 0) 
            {
                Request.Browser.Adapters.Clear();
            }

            ConfigureGridColumns();
            datePicker.Update += DatePickerUpdate;
            presenter = RetentionSchedulePresenter.Create(this);
            DetailView.Visible = true;

            // start related ref:CFF-18
            if (SessionWrapper.Instance.Get != null)
            {
                if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
                {
                    //targetName = ": " + SessionWrapper.Instance.Get.ClientFromQueryString.Name;
                    String LoginClientName = SessionWrapper.Instance.Get.ClientFromQueryString.Name;

                    if (SessionWrapper.Instance.Get.ClientFromQueryString.ClientFacilityType == 0)
                    {
                        RetnSchedulesLiteral.Text = "Retentions/Fees & Charges";
                        RetnSchedulesLiteral2.Text = "Retention/Fees & Charges Schedules: " + LoginClientName;
                        PageDescription.DescriptionContent = "Retention/Fees & Charges Schedules";
                        varRepurchases = "Repurch/Prepay"; //testName = "facilityType == 1"; 
                    }
                    else if (SessionWrapper.Instance.Get.ClientFromQueryString.ClientFacilityType == 1)
                    {
                        RetnSchedulesLiteral.Text = "Retentions";
                        RetnSchedulesLiteral2.Text = "Retention Schedules: " + LoginClientName;
                        PageDescription.DescriptionContent = "Retention Schedules"; //testName = "facilityType == 1"; 
                        varRepurchases = "Repurchases";
                    }
                    else
                    {
                        RetnSchedulesLiteral.Text = "Monthly Fees & Charges";
                        RetnSchedulesLiteral2.Text = "Monthly Fee Schedules: " + LoginClientName;
                        PageDescription.DescriptionContent = "Monthly Fees & Charges Schedules"; //testName = "facilityType != 1"; 
                        varRepurchases = "Prepayments";
                    }
                }
            }
            else if (QueryString.ViewIDValue!=null)
            {
                if ((SessionWrapper.Instance.GetSession(QueryString.ViewIDValue)) != null) {
                    if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString != null)
                    {
                        //targetName = ": " + SessionWrapper.Instance.Get.ClientFromQueryString.Name;
                        String LoginClientName = SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Name;

                        if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.ClientFacilityType == 0)
                        {
                            RetnSchedulesLiteral.Text = "Retentions/Fees & Charges";
                            RetnSchedulesLiteral2.Text = "Retention/Fees & Charges Schedules: " + LoginClientName;
                            PageDescription.DescriptionContent = "Retention/Fees & Charges Schedules";
                            varRepurchases = "Repurch/Prepay"; //testName = "facilityType == 1"; 
                        }
                        else if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.ClientFacilityType == 1)
                        {
                            RetnSchedulesLiteral.Text = "Retentions";
                            RetnSchedulesLiteral2.Text = "Retention Schedules: " + LoginClientName;
                            PageDescription.DescriptionContent = "Retention Schedules"; //testName = "facilityType == 1"; 
                            varRepurchases = "Repurchases";
                        }
                        else
                        {
                            RetnSchedulesLiteral.Text = "Monthly Fees & Charges";
                            RetnSchedulesLiteral2.Text = "Monthly Fee Schedules: " + LoginClientName;
                            PageDescription.DescriptionContent = "Monthly Fees & Charges Schedules"; //testName = "facilityType != 1"; 
                            varRepurchases = "Prepayments";
                        }
                    }
                }
            }
        
           
            if (IsPostBack)
            {
                string cArgs = "";
                Control c = GetPostBackControl(this.Page, out cArgs);
                if (c != null) {
                    eventTarget = c.ID;
                }

                if (eventTarget == "retentionGridView") 
                { //if this is a post back from retention grid then hide the grid
                    showRetentionGridView = false;
                }

                DisplayRetentionSchedules(ViewState["RetentionSchedules"] as IList<RetentionSchedule>);
                ToggleRetentionGridVisibility();
            }
            else
            {
                showRetentionGridView = true;
                if (presenter != null) {
                    LoadRetentionSchedules();
                }
                TabMenu.Items[0].Selected = true;
                PreviousRetentionButton.Visible = (RetentionGridView.Rows.Count == 1) ? false : ((RetentionGridView.FocusedRowIndex <= 0) ? false : true);
                NextRetentionButton.Visible = (RetentionGridView.Rows.Count == 1) ? false : ((RetentionGridView.FocusedRowIndex < (RetentionGridView.Rows.Count - 1)) ? true : false);

                if (RetentionGridView.Rows.Count > 0) {
                    RetentionGridView.ResetPaginationAndFocus();
                    RetentionGridView.FocusedRowIndex = 0;
                    LoadSelectedRetention(0);
                }
            }
            
            RetentionGridView.RowCommand += RetentionGridView_RowCommand;
            RetentionGridView.PagerCommand += RetentionGridView_PagerCommand;
            RetentionGridView.SelectionChanged += RetentionGridViewSelectionChanged;
            MonthRangePicker.Update += MonthRangePickerUpdate;
        }

        void RetentionGridView_PagerCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Contains("Page"))
            {
                CffGenGridView xGrid = sender as CffGenGridView;
                int selectedRowIndex = xGrid.PageSize * (xGrid.PageIndex);
                RetentionGridView.FocusedRowIndex = selectedRowIndex;
                RetentionGridView.SelectRow(selectedRowIndex);
                showRetentionGridView = false;
                LoadSelectedRetention(selectedRowIndex);
                //string script = "hideRetentionGridView();";  //dbb
                string script = "showRetentionGridView();";
                ScriptManager.RegisterStartupScript(this, GetType(), "hideRetGrid", script, true);
            }
        }

        protected void RetentionGridView_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Bound" && e.CommandArgument != null)
            {
                CffGenGridView xGrid = sender as CffGenGridView;
                int selectedRowIndex = (xGrid.PageSize * xGrid.PageIndex)  + Convert.ToInt32(e.CommandArgument);
                RetentionGridView.FocusedRowIndex = selectedRowIndex;
                RetentionGridView.SelectRow(selectedRowIndex);

                showRetentionGridView = false;
                LoadSelectedRetention(selectedRowIndex);
                //string script = "hideRetentionGridView();";  //dbb
                string script = "showRetentionGridView();";
                ScriptManager.RegisterStartupScript(this, GetType(), "hideRetGrid", script, true);
            }
        }

        protected void RetentionGridViewSelectionChanged(object sender, EventArgs e)
        {
            int selectedRowIndex = ((CffGenGridView)sender).SelectedRow.RowIndex;
            LoadSelectedRetention(selectedRowIndex);
        }

        private void ToggleRetentionGridVisibility()
        {
            //Show or hide the retention grid view based on the value of showRetentionGridView
            string script;
            if (showRetentionGridView)
            {
                script = "showRetentionGridView();";
            }
            else
            {
                script = "hideRetentionGridView();";
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "showOrHideGrid", script, true);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                RetentionGridView.ResetPaginationAndFocus();
            } //do not reset page index as we have pagers set

            ToggleRetentionGridVisibility();
            Master.ToggleCustomerSearchControl(false);
            Master.HideRightSidePanel();
        }
        
        private void DatePickerUpdate(object sender, EventArgs e)
        {
            showRetentionGridView = true;
            RetentionGridView.ResetPaginationAndFocus();
            RetentionGridView.FocusedRowIndex = 0;
            LoadRetentionSchedules();
            LoadSelectedRetention(-1);
        }

        protected void MonthRangePickerUpdate(object sender, EventArgs e)
        {
            showRetentionGridView = true;
            RetentionGridView.ResetPaginationAndFocus();
            RetentionGridView.FocusedRowIndex = 0;
            LoadRetentionSchedules();
            LoadSelectedRetention(-1);
        }

        protected void TabViews_ActiveViewChanged(object sender, EventArgs e)
        {
            if (RetentionGridView != null && RetentionGridView.FocusedRowIndex > -1)
            {
                LoadTabInActiveTabView(BuildRetentionItemFromGrid());
            }
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);

            showRetentionGridView = true;
            RetentionGridView.PageIndex = 0;
            RetentionGridView.FocusedRowIndex = 0;
            ConfigureGridColumns();
            LoadRetentionSchedules();
            LoadSelectedRetention(-1);
        }

        protected void TabMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            TabViews.ActiveViewIndex = int.Parse(e.Item.Value);
            
            switch (TabViews.ActiveViewIndex)
            {
                case 0:
                    { RetnHeaderLiteral.Text = _retnSchedHeader; }
                    break;

                case 1:
                    { RetnHeaderLiteral.Text = _retnSchedHeader + " - Adjustments"; }
                    break;
                
                case 2:
                    { RetnHeaderLiteral.Text = _retnSchedHeader  + " - Credits Claimed";} 
                    break;
                
                case 3:
                    { RetnHeaderLiteral.Text = _retnSchedHeader + " - Repurchases Claimed"; }
                    break;
                
                case 4:
                    { RetnHeaderLiteral.Text = _retnSchedHeader + " - Interest & Charges"; } 
                    break;
                
                case 5:
                    { RetnHeaderLiteral.Text = _retnSchedHeader + " - Likely Repurchases"; }
                    break;
                
                default:
                    { RetnHeaderLiteral.Text = "Retention Account Transactions"; }
                    break;
            }
        }

        protected void NextRetentionButton_Click(object sender, EventArgs e)
        {
            RetentionGridView.FocusedRowIndex = (RetentionGridView.FocusedRowIndex < 0)?1:RetentionGridView.FocusedRowIndex + 1;
            RetentionGridView.SelectRow(RetentionGridView.FocusedRowIndex);
            LoadSelectedRetention(-1);
        }

        protected void PreviousRetentionButton_Click(object sender, EventArgs e)
        {
            RetentionGridView.FocusedRowIndex--;
            RetentionGridView.SelectRow(RetentionGridView.FocusedRowIndex);
            LoadSelectedRetention(-1);
        }

        private void LoadSelectedRetention(int selectedRowIdx)
        {
            int focusedRowIdx = RetentionGridView.FocusedRowIndex;
            if (selectedRowIdx > 0) {
                RetentionGridView.FocusedRowIndex = selectedRowIdx;
                focusedRowIdx = selectedRowIdx;
            }

         
            if (RetentionGridView.FocusedRowIndex > -1)
            {
                // Show / hide batch navigation button based on focused row index
                RetentionSchedule retentionSchedule = BuildRetentionItemFromGrid();
                if (retentionSchedule != null)
                {
                    clientNameLiteral.Text = (retentionSchedule == null) ? "" : retentionSchedule.ClientName;
                    EndOfMonthLiteral.Text = (retentionSchedule == null) ? "" : retentionSchedule.EndOfMonth.ToString();

                    int facilityType = 0;
                    facilityType = retentionSchedule.ClientFacilityType;

                    if (facilityType == 1)
                    {
                        RetnSchedulesLiteral2.Text = "Retention Schedules: " + retentionSchedule.ClientName;
                        PageDescription.DescriptionContent = "Retention Schedules"; //testName = "facilityType == 1"; 
                    }
                    else
                    {
                        RetnSchedulesLiteral2.Text = "Monthly Fee Schedules: " + retentionSchedule.ClientName;
                        PageDescription.DescriptionContent = "Monthly Fees & Charges Schedules"; //testName = "facilityType != 1"; 
                    }

                    if (retentionSchedule.Status.Trim().ToUpper().Equals("HELD") || retentionSchedule.Status.Trim().ToUpper().Equals("OK"))
                    { //REf : CFF-21
                        if (facilityType == 1)
                        {
                            _retnSchedHeader = "Retention Statement";
                        }
                        else if (facilityType == 5)//CA
                        {
                            _retnSchedHeader = "Current Account Fees & Charges Statement";
                        }

                        else //4 or 2
                        {
                            _retnSchedHeader = "Fees & Charges Statement";
                        }
                    }
                    else
                    {
                        if (facilityType == 1)
                        {
                            _retnSchedHeader = "Estimated Retention Release";
                        }
                        else if (facilityType == 5)//CA
                        {
                            _retnSchedHeader = "Current Account Estimated Fees & Charges";
                        }

                        else //4 or 2
                        {
                            _retnSchedHeader = "Estimated Fees & Charges";
                        }
                    }

                    RetnHeaderLiteral.Text = _retnSchedHeader;
                    LoadTabInActiveTabView(retentionSchedule);
                }
            }
            else
            {
                var tab = FindActiveTab();
                if (tab != null)
                {
                    tab.ClearTabData();
                }
            }

            int RowCount = (RetentionGridView.DataSource as IList<RetentionSchedule>).Count;
            PreviousRetentionButton.Visible = (RowCount == 1) ? false : ((focusedRowIdx > 0) ? true : false);
            NextRetentionButton.Visible = (RowCount == 1) ? false : ((focusedRowIdx == 0) ? true : ((focusedRowIdx < (RowCount - 1)) ? true : false)); 
            DetailUpdatePanel.Update();
        }

        // GridView does not expose server side row click event that we can use.
        // Page method has been exposed so it can be called from client side on row click
        [WebMethod]
        public static void ToggleRetentionGridView(string show)
        {
            if (show.Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                showRetentionGridView = true;
            }
            else
            {
                showRetentionGridView = false;
            }
        }

        private void LoadRetentionSchedules()
        {
            Scope xScope = this.CurrentScope();
            if (xScope == Scope.AllClientsScope)
            {
                presenter.LoadRetentionSchedulesForAllClients(datePicker.Date);
            }
            else
            {
                ICffClient ClientFromQueryString = (SessionWrapper.Instance.Get == null)
                                ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString
                                : SessionWrapper.Instance.Get.ClientFromQueryString;
                presenter.LoadRetentionSchedules(ClientFromQueryString.Id, MonthRangePicker.DateRange);
            }
            
            if (RetentionGridView.FocusedRowIndex < 0) {
                if (RetentionGridView.Rows.Count > 0)
                    RetentionGridView.FocusedRowIndex = 0;
            }
        }

        private void LoadTabInActiveTabView(RetentionSchedule retentionSchedule)
        {
            IRetentionTab tab = FindActiveTab();
            if (tab != null) {
                tab.LoadTab(retentionSchedule);
                printButton.Visible = tab as IPrintableView != null;
            }
        }

        private RetentionSchedule BuildRetentionItemFromGrid()
        {
            return (RetentionSchedule)RetentionGridView.GetRow(RetentionGridView.FocusedRowIndex);
        }

        private void ConfigureGridColumns()
        {
            
            RetentionGridView.Columns.Clear();
           
            //removed as per marty's suggestions
            /* var commandColumn = new CffCommandField();
               commandColumn.ButtonType = ButtonType.Image;
               commandColumn.ControlStyle.CssClass = "cffGGV_centerAlignedCell";
               commandColumn.ControlStyle.Width = Unit.Pixel(50);
               commandColumn.ItemStyle.Height= Unit.Pixel(30);
 
               commandColumn.SelectText = "View Retention Statements";
               commandColumn.Visible = true;
               commandColumn.SelectImageUrl = "~/images/btn_view_retention.png";
               commandColumn.ShowSelectButton = true;
               commandColumn.VisibleIndex = 0;
               RetentionGridView.Columns.Add(commandColumn);
               RetentionGridView.Columns[0].ItemStyle.Width = Unit.Percentage(5);
            */


            //RetentionGridView.CssClass = "cffGGV";
            RetentionGridView.HeaderStyle.CssClass = "cffGGVHeader";
            RetentionGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            RetentionGridView.ShowHeaderWhenEmpty = true;
            RetentionGridView.EmptyDataText = "No data to display";
            RetentionGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            RetentionGridView.RowCssClass = "dxgvDataRow";

            RetentionGridView.InsertBoundHyperLinkColumn("Client", "ClientName", "ClientId", "20%","cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Center);
            RetentionGridView.InsertBoundCommandButtonColumn("End Of Month", "EndOfMonth", "8%", "cffGGV_underlineBoundButton", HorizontalAlign.Center, "cffGGV_centerAlignedCell", "Click to view schedule", true, true);
            RetentionGridView.InsertDataColumn("Status", "Status", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            RetentionGridView.InsertDataColumn("Released", "ReleaseDate", CffGridViewColumnType.Date, "8", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            RetentionGridView.InsertDataColumn("Notes", "Notes", CffGridViewColumnType.Text, "65%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            bool isAllClientsSelected = (SessionWrapper.Instance.Get == null)
                            ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsAllClientsSelected
                            : SessionWrapper.Instance.Get.IsAllClientsSelected;

            if (isAllClientsSelected)
            {
                //RetentionGridView.Columns["ClientName"].Visible = true;
                RetentionGridView.Columns[0].Visible = true;
                allClientsDatePickerDiv.Visible = true;
                datePicker.Visible = true;
                MonthRangePicker.Visible = false;
            }
            else
            {
                //RetentionGridView.Columns["ClientName"].Visible = false;
                RetentionGridView.Columns[0].Visible = false;
                datePicker.Visible = false;
                allClientsDatePickerDiv.Visible = false;
                MonthRangePicker.Visible = true;
            }
        }

        protected void PrintButton_Click(object sender, ImageClickEventArgs e)
        {
            var tab = FindActiveTab() as IPrintableView;
            if (tab != null)
            {
                tab.Print();
            }
        }

        private IRetentionTab FindActiveTab()
        {
            IRetentionTab tab = null;
            foreach (Control control in TabViews.Views[TabViews.ActiveViewIndex].Controls)
            {
                tab = control as IRetentionTab;
                if (tab != null)
                {
                    printButton.Visible = control as IPrintableView != null;
                    break;
                }
            }
            return tab;
        }
    }
}