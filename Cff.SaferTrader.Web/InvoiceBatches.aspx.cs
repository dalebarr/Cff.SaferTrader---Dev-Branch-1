using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;

using System.Web.UI.HtmlControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.Interfaces;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web
{
    public partial class InvoiceBatches : BasePage, IInvoiceBatchesView, IConvertible
    {
        public string targetName = "";
        private static bool showBatchGridView = true;
        public  CffGenGridView batchGridView;
        private InvoiceBatchesPresenter presenter;

        //private bool isPreviousOrNextButtonClicked;
        private void AddJavaScriptIncludeInHeader(string path)
        {
            try
            {
                var script = new HtmlGenericControl("script");
                script.Attributes["type"] = "text/javascript";
                script.Attributes["src"] = ResolveUrl(path);
                Page.Header.Controls.Add(script);
            }
            catch { }
        }

        public static string CustomerIdQueryString
        { //retrieve these values from sessionwrapper as we may have multiple windows/tabs/browsers open for same session id
            get {
                int custId=0;
                if (SessionWrapper.Instance.Get!=null)
                    if (SessionWrapper.Instance.Get.CustomerFromQueryString!=null)
                        custId = SessionWrapper.Instance.Get.CustomerFromQueryString.Id;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    if ((SessionWrapper.Instance.GetSession(QueryString.ViewIDValue)).CustomerFromQueryString!=null)
                        custId = (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue)).CustomerFromQueryString.Id;

                if (custId > 0)
                    return custId.ToString();

                string strcustId = System.Web.HttpContext.Current.Request.QueryString[QueryString.Customer.ToString()];
                if (!string.IsNullOrEmpty(strcustId))
                    strcustId = strcustId.Replace("(", "").Replace(")", "");
                else
                    return "";
                
                strcustId = strcustId.Split(',')[0];
                return strcustId; 
            }
        }

        public static string ClientIdQueryString
        { //retrieve these values from sessionwrapper as we may have multiple windows/tabs/browsers open for same session id
            get {
                if (SessionWrapper.Instance.Get != null)
                    return SessionWrapper.Instance.Get.ClientFromQueryString.Id.ToString();
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id.ToString();

                return System.Web.HttpContext.Current.Request.QueryString[QueryString.Client.ToString()]; 
            }
        }

        public static string CriteriaQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.Criteria.ToString()];}
        }

        public static string StartsWithQueryString
        {
            get {  return System.Web.HttpContext.Current.Request.QueryString[QueryString.StartsWith.ToString()]; }
        }

        public static string ViewIDQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.ViewID.ToString()]; }
        }

        public static string BatchNumberQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.Batch.ToString()]; }
        }

        public static string QueryStringParameters
        {
            get
            {
                if (!string.IsNullOrEmpty(ClientIdQueryString) && !string.IsNullOrEmpty(CustomerIdQueryString))
                {
                    string queryString = "?Client=" + ClientIdQueryString + "&Customer=" + CustomerIdQueryString;
                    if (!string.IsNullOrEmpty(CriteriaQueryString))
                    {
                        queryString += "&Criteria=" + CriteriaQueryString;
                    }
                    if (!string.IsNullOrEmpty(StartsWithQueryString))
                    {
                        queryString += "&StartsWith=" + StartsWithQueryString;
                    }
                    if (!string.IsNullOrEmpty(ViewIDQueryString))
                    {
                        queryString += "&ViewID=" + ViewIDQueryString;
                    }
                    if (!string.IsNullOrEmpty(BatchNumberQueryString) && (Convert.ToInt32(ClientIdQueryString)>0))
                    {
                        queryString += "&Batch=" + BatchNumberQueryString;
                    }
                    return queryString;
                }

                if (!string.IsNullOrEmpty(ClientIdQueryString) && string.IsNullOrEmpty(CustomerIdQueryString))
                {
                    string queryString = "?Client=" + ClientIdQueryString;
                    if (!string.IsNullOrEmpty(CriteriaQueryString))
                    {
                        queryString += "&Criteria=" + CriteriaQueryString;
                    }
                    if (!string.IsNullOrEmpty(StartsWithQueryString))
                    {
                        queryString += "&StartsWith=" + StartsWithQueryString;
                    }
                    if (!string.IsNullOrEmpty(ViewIDQueryString))
                    {
                        queryString += "&ViewID=" + ViewIDQueryString;
                    }
                    if (!string.IsNullOrEmpty(BatchNumberQueryString) && (Convert.ToInt32(ClientIdQueryString) > 0))
                    {
                        queryString += "&Batch=" + BatchNumberQueryString;
                    }
                    return queryString;
                }

                return "?Client=-1";
            }
        }

        #region IInvoiceBatchesView Members

        public void DisplayInvoiceBatches(IList<InvoiceBatch> invoiceBatches, bool isResetBatchNum)
        {
            ViewState.Add("InvoiceBatches", invoiceBatches);
            batchGridView.DataSource = invoiceBatches;
            if (!string.IsNullOrEmpty(BatchNumberQueryString))
                batchGridView.GridBag = invoiceBatches;

            if (isResetBatchNum && invoiceBatches != null)
            {
                if (invoiceBatches.Count > 0)
                {
                    DetailView.Visible = showBatchGridView;
                    batchGridView.SelectedIndex = 0;
                    batchGridView.FocusedRowIndex = 0;
                    ScriptManager sm = ScriptManager.GetCurrent(Page);
                    string script = (showBatchGridView) ? "showBatchGridView()" : "hideBatchGridView()";
                    if (batchGridView.PageSize == invoiceBatches.Count)
                    { //minimize batchgridview panel
                        script = "hideBatchGridView();";
                    }
                    if (sm.IsInAsyncPostBack)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showOrHideBatchGridView", script, true);
                    }
                }
            }

            batchGridView.DataBind();

            if (!DetailUpdatePanel.Visible)
                DetailUpdatePanel.Visible = true; //marty wants this visible at all times
        }

        public void ShowAllClientsView()
        {
            DateRangePicker.Visible = false;
            AllClientsDateRangePicker.Visible = true;
        }

        public void ShowClientView()
        {
            DateRangePicker.Visible = true;
            AllClientsDateRangePicker.Visible = false;
        }

        public void PopulateBatchTypeDropDown()
        {
            BatchTypeDropDownList.DataSource = BatchType.KnownTypesAsListItems;
            BatchTypeDropDownList.DataTextField = "Text";
            BatchTypeDropDownList.DataValueField = "Value";
            BatchTypeDropDownList.DataBind();
        }

        public void SelectFirstBatchRecord()
        {
            showBatchGridView = false;
            batchGridView.FocusedRowIndex++;
            batchGridView.SelectRow(batchGridView.FocusedRowIndex); //batchGridView.Selection.SelectRow(batchGridView.FocusedRowIndex);
        }

        #endregion


        #region Page Events

        protected override void Page_Init(object sender, EventArgs e)
        {
           base.Page_Init(sender, e);
           DetailUpdatePanel.Visible = false;
        
           AddJavaScriptIncludeInHeader("js/ui.1.10.4/jquery-1.10.2.js");
           AddJavaScriptIncludeInHeader("js/jquery-migrate-1.0.0.js");
           AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery-ui.js");
           AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.core.js");
           AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.button.js");
           AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.widget.js");
           AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.menu.js");
           AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.slider.js");
           AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.dialog.js");
           AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.accordion.js");
           AddJavaScriptIncludeInHeader("js/ui.1.10.4/ui/jquery.ui.datepicker.js");
          
            batchGridView = new CffGenGridView();
            batchGridView.ID = "batchGridView";
            batchGridView.KeyFieldName = "BatchNumber";
            batchGridView.DataKeyNames = new string [] {"BatchNumber"};
            
            //batchGridView.CssClass = "cffGGV";
            batchGridView.EnableSortingAndPagingCallbacks = false;
            batchGridView.ShowHeaderWhenEmpty = true;
            batchGridView.EmptyDataText = "No data to display";

            batchGridView.AutoGenerateColumns = false;
            batchGridView.ShowHeaderWhenEmpty = true;
            batchGridView.HeaderStyle.CssClass = "cffGGVHeader";

            batchGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            batchGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            batchGridView.Width = Unit.Percentage(100);
            //batchGridView.Height = Unit.Percentage(100);
            batchGridView.BorderWidth = Unit.Pixel(1);   // modified by dbb
            batchGridView.FocusedRowIndex = -1;

            batchGridView.PageSize = 10;
            batchGridView.AllowPaging = true;
            batchGridView.ShowFooter = true;

            batchGridView.CustomFooterSettings = CffCustomFooterMode.DefaultSettings | CffCustomFooterMode.ShowTotals;
            batchGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;
            batchGridView.RowsPerPageIncrement = CffRowsPerPageIncrement.DefaultPageSizeIncrement;
            
            batchGridView.EnableViewState = true;
            batchGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
            batchGridView.Visible = true;            

            batchPlaceholder.Controls.Clear();
            batchPlaceholder.Controls.Add(batchGridView);
            batchPlaceholder.Visible = true;


            RetentionSchedulesLink.HRef = "~/RetentionSchedules.aspx" + QueryStringParameters;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            DetailUpdatePanel.Visible = false;
            batchGridView.PrintEventHandler += batchGridView_PrintEventHandler;
            batchGridView.PagerCommand += batchGridView_PagerCommand;
            batchGridView.RowCommand += batchGridview_RowCommand;

            ConfigureGridColumns();
            if (!Page.IsPostBack)
            {
                showBatchGridView = true;
                FromDateRangeClient.Text = DateTime.Now.AddYears(-1).ToShortDateString();
                ToDateRangeClient.Text = DateTime.Now.ToShortDateString();

                FromDateRangeAllClients.Text = DateTime.Now.AddYears(-1).ToShortDateString();
                ToDateRangeAllClients.Text = DateTime.Now.ToShortDateString();

            }

            string script = (showBatchGridView) ? "showBatchGridView();" : "hideBatchGridView();";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showOrHideGrid", script, true);

            presenter = InvoiceBatchesPresenter.Create(this);
            presenter.InitializeForScope(this.CurrentScope());
           
            // start related ref:CFF-18
            if (SessionWrapper.Instance.Get == null)
            {
                if (((CffPrincipal)Context.User).IsInClientRole || ((CffPrincipal)Context.User).IsInCustomerRole) 
                { //??? why is the sessionwrapper content missing ??? 
                    SessionWrapper.Instance.Get.ClientFromQueryString =
                        Cff.SaferTrader.Core.Repositories.RepositoryFactory.CreateClientRepository().GetCffClientByClientId(Convert.ToInt32(((CffPrincipal)Context.User).CffUser.ClientId.ToString()));
                }
                else if (((CffPrincipal)Context.User).IsInAdministratorRole)
                {
                    SessionWrapper.Instance.Get.ClientFromQueryString =
                       Cff.SaferTrader.Core.Repositories.RepositoryFactory.CreateClientRepository().GetCffClientByClientId((int)QueryString.ClientId);
                }
            }
            else
            {
                if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
                    targetName = ": " + SessionWrapper.Instance.Get.ClientFromQueryString.Name;

                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    targetName = ": " + (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue)).ClientFromQueryString.Name;
            }
            // end

            if (IsPostBack) {
                DisplayInvoiceBatches(ViewState["InvoiceBatches"] as IList<InvoiceBatch>, false);
             } 
             else 
             {
                TabMenu.Items[0].Selected = true;
                string batchNumber = Request.QueryString["Batch"];
                if (!string.IsNullOrEmpty(batchNumber) && ((int)QueryString.ClientId > 0))
                {
                    showBatchGridView = false;
                    InvoiceBatchesNumberSearchTextBox.EncodedText = batchNumber;
                    if (SessionWrapper.Instance.Get != null)
                        presenter.SelectInvoiceBatchesForBatchNumber(SessionWrapper.Instance.Get.ClientFromQueryString.Id, batchNumber);
                    else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                            presenter.SelectInvoiceBatchesForBatchNumber(SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id, batchNumber);
                }
                else
                {
                    showBatchGridView = true;
                    if (this.CurrentScope() == Scope.AllClientsScope) {
                        panelLoadingDisplayDiv.Visible = true;
                    }
                    LoadInvoiceBatches();
                }

                if (batchGridView.Rows.Count > 0) {
                    if (batchGridView.FocusedRowIndex < 0)
                    {
                        batchGridView.FocusedRowIndex = 0;
                        batchGridView.SelectedIndex = 0;
                    }
                    LoadSelectedBatch();
                }

              }
        }

        void batchGridView_PagerCommand(object sender, GridViewCommandEventArgs e)
        {
            CffGenGridView srcGrid = (sender as CffGenGridView);
            int newPageIndex = (e.CommandSource as GridViewPageEventArgs).NewPageIndex;
            switch (e.CommandName) { 
                case "Page":
                    if (newPageIndex >= 0)
                    {
                        if ((srcGrid.PagerCommandSource == "GoToPage") || (srcGrid.PagerCommandSource == "Numeric") || (srcGrid.PagerCommandSource == "Last") || (srcGrid.PagerCommandSource == "Prev")) // dbb
                           newPageIndex += 1;

                        if (srcGrid.PagerCommandSource != "Prev")   //dbb
                        {
                            if ((newPageIndex - 1) < 0)
                            {
                                batchGridView.SelectedIndex = 0;
                                batchGridView.FocusedRowIndex = 0;
                            }
                            else
                            {
                                batchGridView.SelectedIndex = ((newPageIndex - 1)*batchGridView.PageSize);
                                batchGridView.FocusedRowIndex = ((newPageIndex - 1)*batchGridView.PageSize);
                            }
                        }
                        else
                        {
                            batchGridView.SelectedIndex = (newPageIndex * batchGridView.PageSize);
                            batchGridView.FocusedRowIndex = (newPageIndex * batchGridView.PageSize);
                        }
                    }
                    else
                    {
                        batchGridView.SelectedIndex = 0;
                        batchGridView.FocusedRowIndex = 0;
                    }
                  
                    ScriptManager sm = ScriptManager.GetCurrent(Page);
                    string script = (showBatchGridView)?"showBatchGridView();":"hideBatchGridView();";
                    if (batchGridView.PageSize == (srcGrid.DataSource as List<InvoiceBatch>).Count)
                    { //minimize batchgridview panel
                        //script = "hideBatchGridView();";  //dbb
                    }

                    if (sm.IsInAsyncPostBack)
                    {
                        this.DetailView.Visible = true;
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showOrHideBatchGridView", script, true);
                    }

                    LoadSelectedBatch("-1");
                    break;

                default:
                    break;
            }

            if (!DetailUpdatePanel.Visible)
                DetailUpdatePanel.Visible = true; 
        }

        void batchGridView_PrintEventHandler(object sender)
        {
                PrintableInvoiceBatches printable =
                new PrintableInvoiceBatches((batchGridView.DataSource as IList<InvoiceBatch>), QueryString.ViewIDValue);

                string script = PopupHelper.ShowPopup(printable, Server);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);    
        }

        void PrintEventHandler(object sender)
        {
        }

        private void BlockPage(bool setblock, int ctr=0)
        { //show/hide retrieve data info
            string script;
            ScriptManager sm = ScriptManager.GetCurrent(Page);
            if (sm.IsInAsyncPostBack) {
                if (setblock)
                {
                    if (ctr > 0)
                        script = "blockPage(" + ctr.ToString() + ");";
                    else
                        script = "blockPage();";
                }
                else
                {
                    if (ctr > 0)
                        script = "unblockPage(" + ctr.ToString() + ");";
                    else
                        script = "unblockPage();";
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "blockPageUI", script, true);
            }
            else if (this.CurrentScope() == Scope.AllClientsScope) {
                if (!setblock)
                {
                    script = "unblockPage(" + ((ctr > 0) ? ctr.ToString() : "200") + ");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "blockPageUI", script, true);
                    panelLoadingDisplayDiv.Visible = false;
                }
                else {
                    panelLoadingDisplayDiv.Visible = true;
                }
            }
        }


    
        protected void ConfigureGridColumns()
        {
            batchGridView.Columns.Clear();

            string strFooterTotals = "";
            string viewID = QueryString.ViewIDValue;

            if (this.CurrentScope() == Scope.AllClientsScope)
            {
                batchGridView.InsertBoundHyperLinkColumn("Client", "ClientName", "ClientId", "12%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
                batchGridView.InsertBoundCommandButtonColumn("Batch", "BatchNumber", "3%", "cffGGV_underlineBoundButton", HorizontalAlign.Center, "cffGGVHeader", "Click to View Batch", true);
            }
            else
                batchGridView.InsertBoundCommandButtonColumn("Batch", "BatchNumber", "5%", "cffGGV_underlineBoundButton", HorizontalAlign.Center, "cffGGVHeader", "Click to View Batch", true);
                batchGridView.InsertDataColumn("Dated", "Date", CffGridViewColumnType.Text, "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);

            if (SessionWrapper.Instance.GetSession(viewID) != null) {
                if ((SessionWrapper.Instance.GetSession(viewID)).ClientFromQueryString.ClientFacilityType != 2)
                {
                    batchGridView.InsertDataColumn("Released", "Released", CffGridViewColumnType.Text, "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
                }
            }

            //start renamed headers as per marty's suggestions
            batchGridView.InsertCurrencyColumn("Funded", "Factored", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            batchGridView.InsertCurrencyColumn("Non Funded", "NonFactored", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            batchGridView.InsertCurrencyColumn("Total", "Total", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            batchGridView.InsertCurrencyColumn("Admin Fee", "AdminFee", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            batchGridView.InsertCurrencyColumn("Funding Fee", "FactorFee", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            //end renamed headers as per marty's suggestions
     
            strFooterTotals = "Factored,NonFactored,Total,AdminFee,FactorFee";
            if (SessionWrapper.Instance.GetSession(viewID) != null)
            {
                if ((SessionWrapper.Instance.GetSession(viewID)).ClientFromQueryString.ClientFacilityType == 2)
                {
                    //batchGridView.InsertCurrencyColumn("N/C Fee", "NonCompliantFee", "6%", "cffGGV_rightAlignedCell", false, HorizontalAlign.Right, HorizontalAlign.Center);
                    batchGridView.InsertCurrencyColumn("Residual", "Retention", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
                    batchGridView.InsertCurrencyColumn("Prepayment", "Repurchase", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
                    strFooterTotals += ",NonCompliantFee,Retention,Repurchase";
                }
                else if ((SessionWrapper.Instance.GetSession(viewID)).ClientFromQueryString.ClientFacilityType == 5)
                {
                    batchGridView.InsertCurrencyColumn("Prepayment", "Repurchase", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
                    strFooterTotals += ",Repurchase";
                }
                else
                {
                    batchGridView.InsertCurrencyColumn("Retention", "Retention", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
                    batchGridView.InsertCurrencyColumn("Prepayments", "Repurchase", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
                    strFooterTotals += ",Retention,Repurchase";

                }
            }
            batchGridView.InsertCurrencyColumn("Credit", "Credit", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            batchGridView.InsertCurrencyColumn("Post", "Post", "6%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            strFooterTotals += ",Credit,Post";

            batchGridView.InsertDataColumn("Status", "Status", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);

            batchGridView.Columns[0].ItemStyle.Width = Unit.Percentage(4);

            batchGridView.TotalsSummarySettings.TableClass = "cffGGV_TotalSummaryTable";
            batchGridView.TotalsSummarySettings.SetColumnTotals(strFooterTotals);
            string[] strDummy = strFooterTotals.Split(',');
            foreach (string x in strDummy)
                batchGridView.TotalsSummarySettings.SetTotalsColumnCssStyle(x, "cffGGV_currencyCell");
        }

        protected void BatchGridViewCustomCallback(object sender, ReportGridViewCustomCallbackEventArgs e)
        {
            CallbackParameter parameter = CallbackParameter.Parse(e.Parameters);
            var batch = (InvoiceBatch)batchGridView.GetRow(parameter.RowIndex);
            var redirectionParameter = new RedirectionParameter(parameter.FieldName, batch.ClientId);

            ISecurityManager securityManager = SecurityManagerFactory.Create(Context.User as CffPrincipal, this.CurrentScope());
            Redirector redirector = new Redirector(RedirectionService.Create(this, securityManager));
            redirector.Redirect(redirectionParameter);
        }

     
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack && string.IsNullOrEmpty(Request.QueryString["Batch"]))
            {
                ///do nothing
            }
            else if (!IsPostBack && batchGridView.Rows.Count > 0)
            {
                batchGridView.FocusedRowIndex = 0;
                batchGridView.SelectedIndex = 0;
                batchGridView.PageIndex = 0;
            }

            Master.ToggleCustomerSearchControl(false);
            Master.HideRightSidePanel();
        }

        #endregion

        #region Control Events

        protected void UpdateButton_Click(object sender, ImageClickEventArgs e)
        {
            showBatchGridView = true;
            batchGridView.ResetPaginationAndFocus();
            LoadInvoiceBatches(true);
            if (!string.IsNullOrEmpty(BatchNumberQueryString) && string.IsNullOrEmpty(InvoiceBatchesNumberSearchTextBox.EncodedText))
            {
                ScriptManager sm = ScriptManager.GetCurrent(Page);
                string script = "$.query.REMOVE('Batch');";
                if (sm.IsInAsyncPostBack)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "clearBatchNumber", script, true);
                }
                LoadSelectedBatch("-1");
            }
            else
                LoadSelectedBatch();
        }

        protected void TabViews_ActiveViewChanged(object sender, EventArgs e)
        {
            if (batchGridView != null && batchGridView.FocusedRowIndex > -1 && batchGridView.Rows.Count > 0)
            {
                LoadTabInActiveTabView(BuildInvoiceBatchFromGrid());
            }
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);

            InvoiceBatchesNumberSearchTextBox.EncodedText = string.Empty;
            showBatchGridView = true;
            batchGridView.ResetPaginationAndFocus();

            if (SessionWrapper.Instance.Get!=null)
                presenter.InitializeForScope(this.CurrentScope());
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                presenter.InitializeForScope(SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).Scope);

            LoadInvoiceBatches();
        }

        protected void TabMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            if (batchGridView.Rows.Count > 0)
                TabViewsBatch.ActiveViewIndex = int.Parse(e.Item.Value);
            else
                TabViewsBatch.ActiveViewIndex = 0;
        }

        protected void NextBatchButton_Click(object sender, EventArgs e)
        {
            //isPreviousOrNextButtonClicked = true;
            batchGridView.SelectRow(batchGridView.FocusedRowIndex);
            batchGridView.FocusedRowIndex = batchGridView.FocusedRowIndex + 1;

            ScriptManager sm = ScriptManager.GetCurrent(Page);
            string script = (showBatchGridView) ? "showBatchGridView();" : "hideBatchGridView();";
            if (batchGridView.PageSize == (batchGridView.DataSource as List<InvoiceBatch>).Count)
            { //minimize batchgridview panel
                script = "hideBatchGridView();";
            }
            if (sm.IsInAsyncPostBack)
            {
                this.DetailView.Visible = true;
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showOrHideBatchGridView", script, true);
            }

            LoadSelectedBatch("-1");
        }

        protected void PreviousBatchButton_Click(object sender, EventArgs e)
        {
            //isPreviousOrNextButtonClicked = true;
            batchGridView.FocusedRowIndex--;
            batchGridView.SelectRow(batchGridView.FocusedRowIndex);
            ScriptManager sm = ScriptManager.GetCurrent(Page);
            string script = (showBatchGridView) ? "showBatchGridView();" : "hideBatchGridView();";

            if (batchGridView.PageSize == (batchGridView.DataSource as List<InvoiceBatch>).Count)
            { //minimize batchgridview panel
                script = "hideBatchGridView();";
            }

            if (sm.IsInAsyncPostBack)
            {
                this.DetailView.Visible = true;
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showOrHideBatchGridView", script, true);
            }

            LoadSelectedBatch("-1");
        }

        protected void batchGridview_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Bound" && e.CommandArgument != null)
            {
                if (batchGridView.PageSize == ((sender as CffGenGridView).DataSource as List<InvoiceBatch>).Count || batchGridView.PageSize > 10)
                { //minimize batchgridview panel
                    ScriptManager sm = ScriptManager.GetCurrent(Page);
                    string script = "hideBatchGridView();";
                    if (sm.IsInAsyncPostBack)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "showOrHideBatchGridView", script, true);
                    }
                }
                this.DetailView.Visible = true;
                LoadSelectedBatch(e.CommandArgument.ToString());
            }
            if (!DetailUpdatePanel.Visible)
                DetailUpdatePanel.Visible = true; 
        }

        #endregion

        [WebMethod]
        // ASPxGridView does not expose server side row click event that we can use.
        // Page method has been exposed so it can be called from client side on row click
        public static void ToggleBatchGridView(string show)
        {
            if (show.Equals(true.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                showBatchGridView = true;
            }
            else
            {
                showBatchGridView = false;
            }
        }

        private void ToggleBatchGridViewVisibility()
        {
            //Show or hide the retention grid view based on the value of showRetentionGridView
            string script;
            if (showBatchGridView)
            {
                script = "showBatchGridView();";
            }
            else
            {
                script = "hideBatchGridView();";
            }
             ScriptManager sm = ScriptManager.GetCurrent(Page);
             if (sm.IsInAsyncPostBack)
             {
                 ScriptManager.RegisterStartupScript(this, GetType(), "showOrHideBatchGrid", script, true);
             }
        }


        protected void PrintButton_Click(object sender, ImageClickEventArgs e)
        {
            IPrintableView tab = FindActiveTab() as IPrintableView;
            if (tab != null)
            {
                tab.Print();
            }
        }

        protected void PrintButtonBatch_Click(object sender, ImageClickEventArgs e)
        { //print batch here similar to windows output
        }

        protected void PrintBatchInvoices_Click(object sender, ImageClickEventArgs e)
        {
            batchGridView.Print();
        }

        private static string GetDocumentTitle()
        {
            string title = "Invoice Batches for {0}";
            Scope currentScope = (SessionWrapper.Instance.Get == null)
                                    ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).Scope
                                        : SessionWrapper.Instance.Get.Scope;

            if (currentScope == Scope.AllClientsScope)
            {
                title = string.Format(title, "Cashflow Funding Limited");
            }
            else if (currentScope == Scope.ClientScope)
            {
                title = string.Format(title, SessionWrapper.Instance.Get.ClientFromQueryString.Name);
            }
            else
            {
                title = string.Format(title, SessionWrapper.Instance.Get.CustomerFromQueryString.Name);
            }

            return title;
        }


        protected void InvoiceBatchesNumberSearchButton_Click(object sender, ImageClickEventArgs e)
        {
            showBatchGridView = true;
            batchGridView.ResetPaginationAndFocus();
            if (SessionWrapper.Instance.Get!=null)
                presenter.SearchInvoiceBatchesForBatchNumber(SessionWrapper.Instance.Get.ClientFromQueryString.Id,
                                                       InvoiceBatchesNumberSearchTextBox.EncodedText);
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                presenter.SearchInvoiceBatchesForBatchNumber(SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id,
                                   InvoiceBatchesNumberSearchTextBox.EncodedText);

            ToggleBatchGridViewVisibility();
            if (!string.IsNullOrEmpty(InvoiceBatchesNumberSearchTextBox.EncodedText))
            {
                LoadSelectedBatch(InvoiceBatchesNumberSearchTextBox.EncodedText);
            }
            else
                LoadSelectedBatch();
        }

        private object FindActiveTab()
        {
            object tab = null;
            foreach (Control cx in TabViewsBatch.Views[TabViewsBatch.ActiveViewIndex].Controls)
            {
                printButton.Visible = false;
                if (cx!=null)  {
                    printButton.Visible = true;
                    switch (TabViewsBatch.ActiveViewIndex)
                    { 
                        case 0:
                            tab = this.ScheduleTab;
                            break;

                        case 1:
                            tab = this.AdjustmentsTab;
                            break;

                        case 2:
                            tab = this.InvoicesTab;
                            break;

                        case 3:
                            tab = this.NonFactoredTab;
                            break;

                        case 4:
                            tab = this.CreditsTab;
                            break;

                        case 5:
                            tab = this.RepurchasesTab;
                            break;

                        default:
                            break;
                    }
                    break;
                }
            }
            return tab;
        }

        private InvoiceBatch BuildInvoiceBatchFromGrid() 
        {
            try
            {
                return (InvoiceBatch)batchGridView.GetRow(batchGridView.FocusedRowIndex);
            }
            catch {
                return null;
            }
        }

        private void ClearAllTabsData()
        {
            object xActiveTab = FindActiveTab();
            if (xActiveTab != null) {
                switch (TabViewsBatch.ActiveViewIndex)
                {
                    case 0:
                        ((Cff.SaferTrader.Web.UserControls.ReleaseTabs.ScheduleTab)xActiveTab).ClearTabData();
                        break;

                    case 2:
                        ((Cff.SaferTrader.Web.UserControls.ReleaseTabs.InvoicesTab)xActiveTab).ClearTabData();
                        break;

                    case 3:
                        ((Cff.SaferTrader.Web.UserControls.ReleaseTabs.NonFactoredTab)xActiveTab).ClearTabData();
                        break;

                    case 4:
                        ((Cff.SaferTrader.Web.UserControls.ReleaseTabs.CreditsTab)xActiveTab).ClearTabData();
                        break;

                    case 5:
                        ((Cff.SaferTrader.Web.UserControls.ReleaseTabs.RepurchasesTab)xActiveTab).ClearTabData();
                        break;

                    case 1:
                        {
                            if (xActiveTab.GetType().Name.IndexOf("batchadjustments") >= 0)
                            {
                                ((Cff.SaferTrader.Web.UserControls.ReleaseTabs.BatchAdjustmentsTab)xActiveTab).ClearTabData();
                            }
                            else
                                ((Cff.SaferTrader.Web.UserControls.ReleaseTabs.OverdueChargesTab)xActiveTab).ClearTabData();
                        }
                        break;

                    default:
                        break;
                }
            } //((IBatchTab)FindActiveTab()).ClearTabData();
        }

        private void LoadTabInActiveTabView(object parInvoiceBatch)
        {
             object activeTab = FindActiveTab();
             if (activeTab != null) {
                 switch (TabViewsBatch.ActiveViewIndex)
                 {
                     case 0: 
                        ((Cff.SaferTrader.Web.UserControls.ReleaseTabs.ScheduleTab)activeTab).LoadTab((InvoiceBatch)parInvoiceBatch);
                        break;

                     case 2:
                        ((Cff.SaferTrader.Web.UserControls.ReleaseTabs.InvoicesTab)activeTab).LoadTab((InvoiceBatch)parInvoiceBatch);
                        break;

                     case 3:
                        ((Cff.SaferTrader.Web.UserControls.ReleaseTabs.NonFactoredTab)activeTab).LoadTab((InvoiceBatch)parInvoiceBatch);
                        break;

                     case 4:
                        ((Cff.SaferTrader.Web.UserControls.ReleaseTabs.CreditsTab)activeTab).LoadTab((InvoiceBatch)parInvoiceBatch);
                        break;

                     case 5:
                        ((Cff.SaferTrader.Web.UserControls.ReleaseTabs.RepurchasesTab)activeTab).LoadTab((InvoiceBatch)parInvoiceBatch);
                        break;

                     case 1:
                        {
                            if (activeTab.GetType().Name.IndexOf("batchadjustments") >= 0)
                            {
                                ((Cff.SaferTrader.Web.UserControls.ReleaseTabs.BatchAdjustmentsTab)activeTab).LoadTab((InvoiceBatch)parInvoiceBatch);
                                //((System.Web.UI.WebControls.MultiView)((((System.Web.UI.WebControls.View)activeTab)).Parent)).Focus();
                            }
                            else
                                ((Cff.SaferTrader.Web.UserControls.ReleaseTabs.OverdueChargesTab)activeTab).LoadTab((RetentionSchedule)parInvoiceBatch);
                         }
                         break;
                
                     default:
                         break;
                 }
             }
         
        }

        private DateRange ActiveDateRangePicker
        {
            get
            {
                if (DateRangePicker.Visible) {
                    DateRange xRange =  (new DateRange(new Date(string.IsNullOrEmpty(FromDateRangeClient.Text) ? DateTime.Now.AddDays(-1) : Convert.ToDateTime(FromDateRangeClient.Text)),
                             new Date(string.IsNullOrEmpty(ToDateRangeClient.Text) ? DateTime.Now : Convert.ToDateTime(ToDateRangeClient.Text))));
                    return xRange;
                }
                else {
                    DateRange xRange2 = (new DateRange(new Date(string.IsNullOrEmpty(FromDateRangeAllClients.Text) ? DateTime.Now.AddDays(-1) : Convert.ToDateTime(FromDateRangeAllClients.Text)),
                                 new Date(string.IsNullOrEmpty(ToDateRangeAllClients.Text) ? DateTime.Now : Convert.ToDateTime(ToDateRangeAllClients.Text))));
                    return xRange2;
                }
            }
        }

        public void StartDBFetchEventHandler(object sender, EventArgs e)
        {
            if ((e as Cff.SaferTrader.Core.Common.CEventArgs).Status > 0)
                BlockPage(true, (e as Cff.SaferTrader.Core.Common.CEventArgs).Status);
            else
                BlockPage(true);
        }    


        public void EndDBFetchEventHandler(object sender, EventArgs e)
        {
            int xStat = (e as Cff.SaferTrader.Core.Common.CEventArgs).Status;
            //do something with the row counter here
            if (xStat > 0)
                BlockPage(false, xStat);
            else
            {
                BlockPage(false);
            }
        }

        private void LoadInvoiceBatches(bool isResetBatchNum=false)
        {
              
            try
            {
                if (BatchTypeDropDownList.Items.Count == 0)
                {
                    PopulateBatchTypeDropDown();
                    BatchTypeDropDownList.SelectedIndex = 0;
                }

                BatchType batchType = BatchType.Parse(int.Parse(BatchTypeDropDownList.SelectedValue));
                if (batchType.IsDateRangeDependant)
                {
                    DateRange xRange = ActiveDateRangePicker;
                    if (SessionWrapper.Instance.Get != null)
                    {
                        this.presenter.LoadInvoiceBatchesForDateRange(SessionWrapper.Instance.Get.ClientFromQueryString.Id, batchType, xRange, isResetBatchNum);
                    }
                    else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                        this.presenter.LoadInvoiceBatchesForDateRange(SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id,
                                                            batchType,
                                                            xRange);
                }
                else
                {
                    if (SessionWrapper.Instance.Get != null)
                        presenter.LoadInvoiceBatchesFor(SessionWrapper.Instance.Get.ClientFromQueryString.Id, batchType);
                    else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                        presenter.LoadInvoiceBatchesFor(SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id, batchType);

                    if (batchGridView.Rows.Count == 0 && batchGridView.FocusedRowIndex < 0)
                    {
                        DetailView.Visible = false;
                        ClearAllTabsData();
                    }
                }
            }
            catch (Exception exc) {
                string msg = exc.Message;
            }
          
        }

        private void LoadSelectedBatch(string batchNum="0")
        {
            string batchNumber = batchNum;
            if (batchNumber == "0" && Request.QueryString.ToString().Contains("Batch") && !string.IsNullOrEmpty(InvoiceBatchesNumberSearchTextBox.EncodedText))
            {
                batchNumber = Request.QueryString["Batch"];
            }

            if (Convert.ToInt32(batchNumber) > 0)
            {
                int focusedIndex =  batchGridView.GetRowIndexByValue(batchNumber);
                batchGridView.SelectedIndex = focusedIndex;
                batchGridView.FocusedRowIndex = focusedIndex;
            }

            InvoiceBatch invoiceBatch = new InvoiceBatch();
            if (batchGridView.FocusedRowIndex > -1)
            {
                invoiceBatch = BuildInvoiceBatchFromGrid();
                if (invoiceBatch != null)
                {
                    if (invoiceBatch.FacilityType == 1 || invoiceBatch.FacilityType == 3)
                    {
                        BatchLiteral.Text = "Invoice Payment Schedule/Batch:";
                    }
                    else if (invoiceBatch.FacilityType == 4)
                    {
                        BatchLiteral.Text = "Advance/Fee Batch:";
                    }
                    else if (invoiceBatch.FacilityType == 5)
                    {
                        BatchLiteral.Text = "Drawing/Fee Batch:";
                    }
                    else //2
                    {
                        BatchLiteral.Text = "Invoice Processing/Fee Batch:";
                    }

                    BatchNumberLiteral.Text = invoiceBatch.Number.ToString();
                    clientNameLiteral.Text = invoiceBatch.ClientName;
                    HeaderLiteral.Text = invoiceBatch.Header;
                    DateLiteral.Text = invoiceBatch.Date.ToString();
                    ModifiedDateLiteral.Text = invoiceBatch.ModifiedDate.ToString();

                    if (string.IsNullOrEmpty(invoiceBatch.Released.ToString()))
                    {
                        ReleasedDateLiteral.Text = "";
                        ReleasedDateLiteral.Visible = false;
                    }
                    else
                    {
                        ReleasedDateLiteral.Text = "Released: " + invoiceBatch.Released.ToString();
                        ReleasedDateLiteral.Visible = true;
                    }

                    StatusLiteral.Text = invoiceBatch.Status;

                    LoadTabInActiveTabView(invoiceBatch);

                }
            }
            else {
                LoadTabInActiveTabView(invoiceBatch);
            }

            if (batchGridView.Rows.Count > 0) {
               //Show/hide batch navigation button based on focused row index
                PreviousBatchButton.Visible = (batchGridView.FocusedRowIndex > 0);
                NextBatchButton.Visible = (batchGridView.FocusedRowIndex < ((batchGridView.PageSize * batchGridView.Rows.Count) - 1));
            }



        }

        #region IConvertible Members

        TypeCode IConvertible.GetTypeCode()
        {
            throw new NotImplementedException();
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        #endregion */

        protected void ToDateRangeClient_TextChanged(object sender, EventArgs e)
        {
            InvoiceBatchesNumberSearchTextBox.Text = "";
            InvoiceBatchesNumberSearchUpdatePanel.Update();
        }

        protected void FromDateRangeClient_TextChanged(object sender, EventArgs e)
        {
            int len = InvoiceBatchesNumberSearchTextBox.EncodedText.Length;
            InvoiceBatchesNumberSearchTextBox.Text = "";
            if (len > 0)
                InvoiceBatchesNumberSearchUpdatePanel.Update();
        }

        protected void FromDateRangeAllClients_TextChanged(object sender, EventArgs e)
        {
            InvoiceBatchesNumberSearchTextBox.Text = "";
            InvoiceBatchesNumberSearchUpdatePanel.Update();
        }

        protected void ToDateRangeAllClients_TextChanged(object sender, EventArgs e)
        {
            InvoiceBatchesNumberSearchTextBox.Text = "";
            InvoiceBatchesNumberSearchUpdatePanel.Update();
        }

    }
}