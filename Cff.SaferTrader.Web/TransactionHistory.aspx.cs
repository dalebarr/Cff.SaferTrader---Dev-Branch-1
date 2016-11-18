using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web
{
    public partial class TransactionHistory : BasePage, ITransactionHistoryView
    {
        protected static string targetName = "";
        private TransactionHistoryPresenter presenter;
        
        public CffGenGridView cffGGV_TransactionGridView;
        public CffGenGridView cffGGV_TransactionGridViewChild;
        
        public static string CustomerIdQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.Customer.ToString()]; }
        }

        public static string ClientIdQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.Client.ToString()]; }
        }

        public static string CriteriaQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.Criteria.ToString()]; }
        }

        public static string StartsWithQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.StartsWith.ToString()]; }
        }

        public static string ViewIDQueryString
        {
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.ViewID.ToString()]; }
        }

        public string QueryStringParameters
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
                    return queryString;
                }
                return "?Client=-1";
            }
        }

        protected void DetailGridViewCustomCallback(object sender, ReportGridViewCustomCallbackEventArgs e)       //ASPxGridViewCustomCallbackEventArgs e)
        {
            CffGenGridView detailGridView = (CffGenGridView)sender;
            CallbackParameter parameter = CallbackParameter.Parse(e.Parameters);
            var transaction = (Transaction)detailGridView.GetRow(parameter.RowIndex);
            var redirectionParameter = new RedirectionParameter(parameter.FieldName, SessionWrapper.Instance.Get.ClientFromQueryString.Id, SessionWrapper.Instance.Get.CustomerFromQueryString.Id, transaction.Batch);

            ISecurityManager securityManager = SecurityManagerFactory.Create(Context.User as CffPrincipal, this.CurrentScope());
            Redirector redirector = new Redirector(RedirectionService.Create(this, securityManager));
            redirector.Redirect(redirectionParameter);
        }

        protected override void Page_Init(object sender, EventArgs e)
        {
           base.Page_Init(sender, e);

           if (this.CurrentScope() == Scope.CustomerScope)
           {
               //start child grid initialization
               cffGGV_TransactionGridViewChild = new CffGenGridView();
               cffGGV_TransactionGridViewChild.AllowSorting = true;
               cffGGV_TransactionGridViewChild.AutoGenerateColumns = false;
               cffGGV_TransactionGridViewChild.SetSortExpression = "Date";
               
               cffGGV_TransactionGridViewChild.Width = Unit.Percentage(50);
               cffGGV_TransactionGridViewChild.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
               cffGGV_TransactionGridViewChild.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
               //cffGGV_TransactionGridViewChild.BorderColor = System.Drawing.Color.AliceBlue;
               //cffGGV_TransactionGridViewChild.RowStyleHighlightColour = System.Drawing.Color.Honeydew;
               //cffGGV_TransactionGridViewChild.AlternatingRowStyle.BackColor = System.Drawing.Color.AliceBlue;

               //note: for nested grid leave some space for maximize/minimize button
               cffGGV_TransactionGridViewChild.Columns.Clear();
               cffGGV_TransactionGridViewChild.InsertDataColumn("Date", "Date", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
               cffGGV_TransactionGridViewChild.InsertDataColumn("Processed", "Processed", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
               cffGGV_TransactionGridViewChild.InsertDataColumn("Type", "Type", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
               cffGGV_TransactionGridViewChild.InsertDataColumn("Number", "Number", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
               cffGGV_TransactionGridViewChild.InsertDataColumn("Reference", "Reference", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
               cffGGV_TransactionGridViewChild.InsertCurrencyColumn("Debit", "Amount", "6%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
               cffGGV_TransactionGridViewChild.InsertCurrencyColumn("Credit", "Balance", "6%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
               cffGGV_TransactionGridViewChild.InsertBoundHyperLinkColumn("Batch", "Batch", "Batch", "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, "Type:Invoice:On:IsReversed");
               cffGGV_TransactionGridViewChild.Columns[7].ItemStyle.HorizontalAlign = HorizontalAlign.Left;

               cffGGV_TransactionGridViewChild.EnableViewState = true;
               cffGGV_TransactionGridViewChild.NestedSettings.Enabled = false;
               cffGGV_TransactionGridViewChild.TotalsSummarySettings.TableClass = "dxgvFooter"; //dbb
               cffGGV_TransactionGridViewChild.TotalsSummarySettings.SetColumnTotals("Balance, Amount");
               cffGGV_TransactionGridViewChild.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Balance", "cffGGV_currencyCell");
               cffGGV_TransactionGridViewChild.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Amount", "cffGGV_currencyCell");
               cffGGV_TransactionGridViewChild.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
           }

           //start parent grid initialization
           cffGGV_TransactionGridView = new CffGenGridView();
           cffGGV_TransactionGridView.AllowSorting = true;
           cffGGV_TransactionGridView.AutoGenerateColumns = false;
           cffGGV_TransactionGridView.SetSortExpression = "YrMonth";
           cffGGV_TransactionGridView.Width = Unit.Percentage(100);
           cffGGV_TransactionGridView.BorderColor = System.Drawing.Color.LightGray;
           cffGGV_TransactionGridView.PageSize = 250;
           cffGGV_TransactionGridView.EnableViewState = true;

           if (Request.Browser.Id.ToUpper().Contains("IE"))
               cffGGV_TransactionGridView.BorderWidth = Unit.Pixel(1);

           cffGGV_TransactionGridView.HeaderStyle.CssClass = "dxgvHeader td";
           cffGGV_TransactionGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
           cffGGV_TransactionGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
           //cffGGV_TransactionGridView.RowStyleHighlightColour = System.Drawing.Color.Honeydew;

           cffGGV_TransactionGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

           if (this.CurrentScope() == Scope.CustomerScope)
           {
               cffGGV_TransactionGridView.NestedSettings.Enabled = true;
               cffGGV_TransactionGridView.NestedSettings.Expanded = false;
               cffGGV_TransactionGridView.NestedSettings.ExpandingButtonHeight = System.Web.UI.WebControls.Unit.Pixel(10);
               cffGGV_TransactionGridView.NestedSettings.ExpandingButtonWidth = System.Web.UI.WebControls.Unit.Pixel(10);
               cffGGV_TransactionGridView.NestedSettings.ExpandingColumnWidth = System.Web.UI.WebControls.Unit.Percentage(2);
               cffGGV_TransactionGridView.NestedSettings.childGrid = cffGGV_TransactionGridViewChild;
           } 
           else
               cffGGV_TransactionGridView.NestedSettings.Enabled = false;

           cffGGV_TransactionGridView.InsertDataColumn("Month", "YrMonth", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left,  HorizontalAlign.Left, false);
           cffGGV_TransactionGridView.InsertCurrencyColumn("Funding", "Factored", "8%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
           cffGGV_TransactionGridView.InsertCurrencyColumn("Non Funding", "NonFactored", "8%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
           cffGGV_TransactionGridView.InsertCurrencyColumn("Credit", "Credit", "8%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
           cffGGV_TransactionGridView.InsertCurrencyColumn("Receipt", "Receipt", "8%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
           cffGGV_TransactionGridView.InsertCurrencyColumn("Journal", "Journal", "8%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
           cffGGV_TransactionGridView.InsertCurrencyColumn("Discount", "Discount", "8%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
           cffGGV_TransactionGridView.InsertCurrencyColumn("Prepayments", "Repurchase", "8%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
           cffGGV_TransactionGridView.InsertCurrencyColumn("OverPayment", "OverPayment", "8%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
        
           cffGGV_TransactionGridView.EnableViewState = true;
           cffGGV_TransactionGridView.CaptionHeaderSettings.BoldCaption = true;

           //set summary settings
           cffGGV_TransactionGridView.TotalsSummarySettings.SetColumnTotals("Factored, NonFactored, Credit, Receipt, Journal, Discount, Repurchase, OverPayment");
           cffGGV_TransactionGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Factored", "cffGGV_currencyCell");
           cffGGV_TransactionGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("NonFactored", "cffGGV_currencyCell");
           cffGGV_TransactionGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Credit", "cffGGV_currencyCell");
           cffGGV_TransactionGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Receipt", "cffGGV_currencyCell");
           cffGGV_TransactionGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Journal", "cffGGV_currencyCell");
           cffGGV_TransactionGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Discount", "cffGGV_currencyCell");
           cffGGV_TransactionGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Repurchases", "cffGGV_currencyCell");
           cffGGV_TransactionGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("OverPayment", "cffGGV_currencyCell");
           cffGGV_TransactionGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;

           cffGGV_TransactionGridView.ShowFooter = true;
           cffGGV_TransactionGridView.AllowPaging = true;
           cffGGV_TransactionGridView.TotalsSummarySettings.TableClass = "dxgvFooter"; //dbb
           cffGGV_TransactionGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;
           cffGGV_TransactionGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

           TransactionGridViewPH.Controls.Clear();
           TransactionGridViewPH.Controls.Add(cffGGV_TransactionGridView);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            targetName = "";
            MonthRangePicker.Update += ParameterUpdate;
            ISecurityManager securityManager = SecurityManagerFactory.Create(Context.User as CffPrincipal, this.CurrentScope());
            presenter = new TransactionHistoryPresenter(this, RepositoryFactory.CreateTransactionRepository(), RedirectionService.Create(this, securityManager), securityManager);
            presenter.LockDown();
            presenter.InitializeForScope(this.CurrentScope());
            cffGGV_TransactionGridView.RowCommand += cffGGV_TransactionGridView_RowCommand;

            ChkBoxTransactionInvoices.Checked = false;
            ChkBoxTransactionInvoices.Visible = false; //removed as per marty's request

            if (IsPostBack)
            {
                // start ref:CFF-18
                if (SessionWrapper.Instance.Get == null && QueryString.ViewIDValue != null) {
                    targetName = ": " + SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Name;
                }
                else if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
                {
                    targetName = ": " + SessionWrapper.Instance.Get.ClientFromQueryString.Name;
                }

                if (SessionWrapper.Instance.Get == null && QueryString.ViewIDValue != null)
                {
                    if (targetName != null || !targetName.Equals(""))
                    {
                        targetName += " / ";
                        targetName = string.Concat(targetName, SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString.Name);
                    }
                    else
                    {
                        targetName = ": " + SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString.Name;
                    }
                } 
                else  if (SessionWrapper.Instance.Get.CustomerFromQueryString != null)
                {
                    if (targetName != null || !targetName.Equals(""))
                    {
                        targetName += " / ";
                        targetName = string.Concat(targetName, SessionWrapper.Instance.Get.CustomerFromQueryString.Name);
                    }
                    else
                    {
                        targetName = ": " + SessionWrapper.Instance.Get.CustomerFromQueryString.Name;
                    }
                }
                // end


                if (SessionWrapper.Instance.Get == null && QueryString.ViewIDValue != null)
                {
                    presenter.LoadTransactionHistory(MonthRangePicker.DateRange, SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString.Id, ChkBoxTransactionInvoices.Checked);
                }
                else if (SessionWrapper.Instance.Get.IsCustomerSelected)
                {
                    presenter.LoadTransactionHistory(MonthRangePicker.DateRange, SessionWrapper.Instance.Get.CustomerFromQueryString.Id, ChkBoxTransactionInvoices.Checked);
                }
                else
                {
                    if (ViewState["Transactions"]!=null)
                        ShowTransactionHistory(ViewState["Transactions"] as IList<HistoricalTransaction>);
                }
            }

            SearchLink.HRef = "~/TransactionSearch.aspx" + QueryStringParameters;
            currentTransactionsLink.InnerHtml = "<a href=\"Transactions.aspx" + Server.HtmlDecode(QueryStringParameters) + "\">Current</a>";
            transactionArchiveLink.InnerHtml = "<a href=\"TransactionArchive.aspx" + Server.HtmlDecode(QueryStringParameters) + "\">Archive</a>";
        }


        void cffGGV_TransactionGridView_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
             int rowIndex = Convert.ToInt32(e.CommandArgument);
             if (e.CommandName == "Expand" &&   ((((System.Web.UI.WebControls.GridView)(sender)).Rows).Count > 0) )
             {  //assign a datasource to the child and call databind
                 bool isExpanded = false;
                 if ((((System.Web.UI.WebControls.GridView)(sender)).Rows[rowIndex]).Controls.Count > 0)
                 {
                     isExpanded = ((Cff.SaferTrader.Web.UserControls.gGridViewControls.CffCommandField)((((System.Web.UI.WebControls.DataControlFieldCell)((((System.Web.UI.WebControls.GridView)(sender)).Rows[rowIndex]).Controls[0]))).ContainingField)).isExpanded;
                 }

                 HistoricalTransaction o = (HistoricalTransaction)(((IList<HistoricalTransaction>)cffGGV_TransactionGridView.DataSource)[rowIndex]);
                 Date date = new Date(Convert.ToDateTime(o.YrMonth));

                 if (SessionWrapper.Instance.Get != null && this.CurrentScope() == Scope.CustomerScope)
                 { //customer view
                    IList<Transaction> details = presenter.LoadTransactionHistoryDetails(date, SessionWrapper.Instance.Get.CustomerFromQueryString.Id);
                    cffGGV_TransactionGridView.BindNestedGrid(sender, details);
                 } 
             }
        }

        /// <summary>
        /// Load report function, if it is called in scope change event handler then reset form, otherwise call LoadTransactionHistoryForAllClient function
        /// </summary>
        /// <param name="isScopeChange"></param>
        private void LoadHistoryReport(bool isScopeChange)
        {
            // start ref: CFF-19
            UpdateLabel.Visible = false;
            ExportButton.Visible = true;
            // end

            if (this.CurrentScope() == Scope.CustomerScope)
            {
                presenter.LoadTransactionHistory(MonthRangePicker.DateRange, SessionWrapper.Instance.Get.CustomerFromQueryString.Id, ChkBoxTransactionInvoices.Checked);
            }
            else if (this.CurrentScope() == Scope.AllClientsScope)
            {
                if (isScopeChange)
                {
                    ShowTransactionHistory(null);
                }
                else
                {
                    presenter.LoadTransactionHistoryForAllClients(MonthRangePicker.DateRange, MonthRangePicker.FacilityType, MonthRangePicker.IsSalvageIncluded);    
                }
            }
            else if (this.CurrentScope() == Scope.ClientScope)
            {
                presenter.LoadTransactionHistoryForClient(MonthRangePicker.DateRange, SessionWrapper.Instance.Get.ClientFromQueryString.Id);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        public void ShowTransactionHistory(IList<HistoricalTransaction> historicalTransactions)
        {
            ViewState.Add("Transactions", historicalTransactions);
            cffGGV_TransactionGridView.DataSource = historicalTransactions;
            cffGGV_TransactionGridView.DataBind();
        }

        public void ShowCustomerView()
        {
            TransactionHistoryPanel.Visible = true;
            transactionHistoryPageDescription.Visible = true;

            MonthRangePicker.HideAllClientsControls();
            // start ref: CFF-19
            if (UpdateLabel.Visible == true)
            {
                ExportButton.Visible = false;
            }
            // end
            currentTransactionsLink.Visible = true;
            transactionArchiveLink.Visible = true;
        }

        public void ShowAllClientsView()
        {
            TransactionHistoryPanel.Visible = true;
            transactionHistoryPageDescription.Visible = true;
            //TransactionGridView.SettingsDetail.ShowDetailRow = false;
            MonthRangePicker.ShowAllClientsControls();
            // start ref: CFF-19
            if (UpdateLabel.Visible == true)
            {
                ExportButton.Visible = false;
            }
            // end
            currentTransactionsLink.Visible = false;
            transactionArchiveLink.Visible = false;
        }

        public void ShowClientView()
        {
            TransactionHistoryPanel.Visible = true;
            transactionHistoryPageDescription.Visible = true;

            MonthRangePicker.HideAllClientsControls();
            // start ref: CFF-19
            if (UpdateLabel.Visible == true)
            {
                ExportButton.Visible = false;
            }
            // end
            currentTransactionsLink.Visible = false;
            transactionArchiveLink.Visible = false;
        }


        /*
        protected void DetailGridViewBeforePerformDataSelect(object sender, EventArgs e)
        {
            if (presenter != null)
            {
                CffGenGridView detailGridView = (CffGenGridView)sender;
                Date date = new Date(DateTime.Parse(detailGridView.GetMasterRowKeyValue().ToString()));
               detailGridView.DataSource = presenter.LoadTransactionHistoryDetails(date, SessionWrapper.Instance.Get.CustomerFromQueryString.Id);
            }
        }*/

        /// <summary>
        /// Collapse any expanded row when page is changed. This is due to a bug in ASPxGrid
        /// where an exception gets thrown when it tries to export a detail grid on another page
        /// </summary>
        protected void TransactionGridView_PageIndexChanged(object sender, EventArgs e)
        {
            //TransactionGridView.DetailRows.CollapseAllRows();
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);
            presenter.InitializeForScope(this.CurrentScope());

            //TransactionGridView.ResetPaginationAndFocus();
            transactionHistoryPageDescription.Visible = true;
            LoadHistoryReport(true);
        }

        private void ParameterUpdate(object sender, EventArgs e)
        {
            //TransactionGridView.ResetPaginationAndFocus();
            string script = "enableRDInfo();";
            ScriptManager.RegisterClientScriptBlock(this,GetType(), "eRDInfo", script, true);
            RDInfoLabel.Text = "Retrieving DATA please wait...";
            ParameterSelectorUpdatePanel.Update();

            LoadHistoryReport(false);
            if (ViewState["Transactions"] != null)
                ShowTransactionHistory(ViewState["Transactions"] as IList<HistoricalTransaction>);

            RDInfoLabel.Text = "";
            ParameterSelectorUpdatePanel.Update();
            script = "disableRDInfo();";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "dRDInfo", script, true);
        }

        protected void ExportButton_Click(object sender, ImageClickEventArgs e)
        {
            cffGGV_TransactionGridView.Export(GetDocumentTitle(), MonthRangePicker.DateRange);
        }

        private static string GetDocumentTitle()
        {
            string title = "Transaction History for {0}";

            Scope currentScope = (SessionWrapper.Instance.Get == null) 
                                    ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).Scope 
                                        : SessionWrapper.Instance.Get.Scope;

            if (currentScope==Scope.AllClientsScope)
            {
                title=string.Format(title, "Cashflow Funding Limited");
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

        protected void ChkBoxTransactionInvoices_CheckedChanged(object sender, EventArgs e)
        {          
        }
    }
}