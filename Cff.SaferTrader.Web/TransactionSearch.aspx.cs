using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.ScopeManager;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;
using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;
using Cff.SaferTrader.Web.App_GlobalResources;


namespace Cff.SaferTrader.Web
{
    public partial class TransactionSearch : BasePage, ITransactionSearchView
    {
        protected static string targetName = "";
        private TransactionSearchPresenter presenter;

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
            get { return System.Web.HttpContext.Current.Request.QueryString[QueryString.ViewIDValue]; }
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

        #region ITransactionSearchView Members

        public void DisplayMatchedTransactions(IList<TransactionSearchResult> transactions)
        {
            ViewState.Add("transactions", transactions);
            transactionSearchGridView.DataSource = transactions;
            transactionSearchGridView.DataBind();
        }

        public void DisplayMatchedCreditNotesTransactions(IList<CreditNoteSearchResult> creditNoteSearchResults)
        {
            ViewState.Add("creditNotesTransactions", creditNoteSearchResults);
            creditSearchGridView.DataSource = creditNoteSearchResults;
            creditSearchGridView.DataBind();
        }
        
       
        #endregion

        protected void TransactionSearchGridViewCustomCallback(object sender, ReportGridViewCustomCallbackEventArgs e)
        {
            CallbackParameter parameter = CallbackParameter.Parse(e.Parameters);
            TransactionSearchResult transaction =
               (TransactionSearchResult) transactionSearchGridView.GetRow(parameter.RowIndex);
           
            RedirectionParameter redirectionParameter = new RedirectionParameter(parameter.FieldName,
                                                                                 transaction.ClientId,
                                                                                 transaction.CustomerId,
                                                                                 transaction.Batch);
            ISecurityManager securityManager = SecurityManagerFactory.Create(Context.User as CffPrincipal, this.CurrentScope());
            Redirector redirector = new Redirector(RedirectionService.Create(this, securityManager));
            redirector.Redirect(redirectionParameter);
        }

        protected void CreditSearchGridViewCustomCallback(object sender, ReportGridViewCustomCallbackEventArgs e)
        {
            CallbackParameter parameter = CallbackParameter.Parse(e.Parameters);
            
            CreditNoteSearchResult creditNote = (CreditNoteSearchResult) creditSearchGridView.GetRow(parameter.RowIndex);
            
            RedirectionParameter redirectionParameter = new RedirectionParameter(parameter.FieldName,
                                                                                 creditNote.ClientId,
                                                                                 creditNote.CustomerId, creditNote.Batch);
            ISecurityManager securityManager = SecurityManagerFactory.Create(Context.User as CffPrincipal, this.CurrentScope());
            Redirector redirector = new Redirector(RedirectionService.Create(this, securityManager));
            redirector.Redirect(redirectionParameter);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ViewState.Add("ViewID", QueryString.ViewIDValue);

            targetName = "";
            ISearchScopeManager searchScopeManager = SearchScopeManagerFactory.Create(this.CurrentScope(), Context.User as CffPrincipal);
            presenter = new TransactionSearchPresenter(this, RepositoryFactory.CreateTransactionSearchRepository(), searchScopeManager);
            
            ConfigureGrids();
            if (!IsPostBack)
            {
                PopulateTransactionTypeDropDownList();
                presenter.PopulateSearchScopeDropDownList();
                ShowHideTabNavigations(); 
                transactionSearchGridView.Visible = false;
                creditSearchGridView.Visible = false;
            }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
            else
            {
                SearchLabel.Visible = false;
                TransactionSearchType transactionSearchType =
                    ((TransactionSearchType)
                     Enum.Parse(typeof (TransactionSearchType), TransactionTypeDropDownList.SelectedValue));

                if (transactionSearchType == TransactionSearchType.Invoices)
                {
                     DisplayMatchedTransactions(ViewState["transactions"] as IList<TransactionSearchResult>);
                }
                else
                {
                    DisplayMatchedCreditNotesTransactions(
                        ViewState["creditNotesTransactions"] as IList<CreditNoteSearchResult>);
                }
            }

            currentTransactionsLink.InnerHtml = "<a href=\"Transactions.aspx" + Server.HtmlDecode(QueryStringParameters) + "\">Current</a>";
            transactionArchiveLink.InnerHtml = "<a href=\"TransactionArchive.aspx" + Server.HtmlDecode(QueryStringParameters) + "\">Archive</a>";
            transactionHistoryLink.InnerHtml = "<a href=\"TransactionHistory.aspx" + Server.HtmlDecode(QueryStringParameters) + "\">History</a>";
        }

        private void PopulateTransactionTypeDropDownList()
        {
            TransactionTypeDropDownList.DataSource = new Dictionary<TransactionSearchType, string>
                                                         {
                                                             {
                                                                 TransactionSearchType.Invoices,
                                                                 TransactionSearchType.Invoices.ToString()
                                                                 },
                                                             {
                                                                 TransactionSearchType.CreditNotes,
                                                                 "Credit Notes"
                                                                 }
                                                         };
            TransactionTypeDropDownList.DataValueField = "Key";
            TransactionTypeDropDownList.DataTextField = "Value";
            TransactionTypeDropDownList.DataBind();
        }

        protected override void Page_Init(object sender, EventArgs e)
        {
            base.Page_Init(sender, e);

            transactionSearchGridView.AllowSorting = true;
            transactionSearchGridView.BorderWidth = Unit.Pixel(1);
            transactionSearchGridView.RowStyleHighlightColour = System.Drawing.Color.Honeydew;

            transactionSearchGridView.AutoGenerateColumns = false;
            transactionSearchGridView.ShowHeaderWhenEmpty = true;
            transactionSearchGridView.EmptyDataText = "No data to display";
            transactionSearchGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
           
            transactionSearchGridView.HeaderStyle.CssClass = "cffGGVHeader";
            transactionSearchGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            
            transactionSearchGridView.AllowPaging = true;
            transactionSearchGridView.PageSize = 250;
            transactionSearchGridView.Width = Unit.Percentage(100);
            transactionSearchGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

         
            transactionSearchGridView.ShowFooter = true;
            transactionSearchGridView.TotalsSummarySettings.SetColumnTotals("InvoiceBalance,CustomerBalance");
            transactionSearchGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("InvoiceBalance","cffGGV_currencyCell");
            transactionSearchGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("CustomerBalance", "cffGGV_currencyCell");
            transactionSearchGridView.CustomFooterSettings = CffCustomFooterMode.DefaultSettings | CffCustomFooterMode.ShowTotals;
            transactionSearchGridView.CustomPagerSettingsMode = CffCustomPagerMode.Bottom | CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;

            creditSearchGridView.AllowSorting = true;
            creditSearchGridView.BorderWidth = Unit.Pixel(1);
            creditSearchGridView.RowStyleHighlightColour = System.Drawing.Color.Honeydew;

            creditSearchGridView.AutoGenerateColumns = false;
            creditSearchGridView.ShowHeaderWhenEmpty = true;
            creditSearchGridView.EmptyDataText = "No data to display";
            creditSearchGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            creditSearchGridView.Width = Unit.Percentage(100);
            creditSearchGridView.HeaderStyle.CssClass = "cffGGVHeader";
            creditSearchGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            creditSearchGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
            creditSearchGridView.ShowFooter = true;
            creditSearchGridView.AllowPaging = true;
            creditSearchGridView.PageSize = 250;

            creditSearchGridView.TotalsSummarySettings.TableClass = "dxgvFooter"; //dbb
            creditSearchGridView.TotalsSummarySettings.SetColumnTotals("CreditNoteAmount,CustomerBalance");
            creditSearchGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("CreditNoteAmount", "cffGGV_currencyCell");
            creditSearchGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("CustomerBalance", "cffGGV_currencyCell");
            creditSearchGridView.CustomFooterSettings = CffCustomFooterMode.DefaultSettings | CffCustomFooterMode.ShowTotals;
            creditSearchGridView.CustomPagerSettingsMode = CffCustomPagerMode.Bottom | CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
        }

        private void ConfigureGrids()
        {
            Scope currentScope = this.CurrentScope();

            transactionSearchGridView.Columns.Clear();
            if (currentScope == Scope.AllClientsScope) {
                transactionSearchGridView.InsertBoundHyperLinkColumn("Client", "ClientName", "ClientId", "12%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            }

            transactionSearchGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
            transactionSearchGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "30%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            transactionSearchGridView.InsertDataColumn("Title", "Title", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
            transactionSearchGridView.InsertDataColumn("Invoice", "InvoiceNumber", CffGridViewColumnType.Text, "4%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
            transactionSearchGridView.InsertDataColumn("Dated", "InvoiceDate", CffGridViewColumnType.Date, "4%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
            transactionSearchGridView.InsertCurrencyColumn("Amount", "InvoiceAmount", "7%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);

            transactionSearchGridView.InsertCurrencyColumn("Balance", "InvoiceBalance", "7%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            transactionSearchGridView.InsertDataColumn("Processed", "Processed", CffGridViewColumnType.Date, "4%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, false);
            transactionSearchGridView.InsertBoundHyperLinkColumn("Batch", "Batch", "Batch", "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center);
            transactionSearchGridView.InsertCurrencyColumn("Cust.Balance", "CustomerBalance", "7%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);

            creditSearchGridView.Columns.Clear();
            if (currentScope == Scope.AllClientsScope){
                creditSearchGridView.InsertBoundHyperLinkColumn("Client", "ClientName", "ClientId", "15%", "cffGGV_centerAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            }

            creditSearchGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
            creditSearchGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "30%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            creditSearchGridView.InsertDataColumn("Title", "Title", CffGridViewColumnType.Text, "4%", "cffGGV_centerAlignedCell", HorizontalAlign.Left, HorizontalAlign.Center, false);
            creditSearchGridView.InsertDataColumn("Transaction", "TransactionNumber", CffGridViewColumnType.Text, "4%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
            creditSearchGridView.InsertDataColumn("Dated", "Dated", CffGridViewColumnType.Date, "4%", "cffGGV_centerAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
            creditSearchGridView.InsertCurrencyColumn("Credit Note", "CreditNoteAmount", "10%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            creditSearchGridView.InsertDataColumn("Processed", "Processed", CffGridViewColumnType.Date, "7%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, false);
            creditSearchGridView.InsertDataColumn("Batch", "Batch", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Left, HorizontalAlign.Center, false);  //do not enable hyperlink for credits batch columns
            creditSearchGridView.InsertCurrencyColumn("Customer Balance", "CustomerBalance", "7%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);


            transactionSearchGridView.Columns[0].ControlStyle.BorderWidth = Unit.Pixel(1);
            transactionSearchGridView.BorderWidth = Unit.Pixel(1);
            creditSearchGridView.Columns[0].ControlStyle.BorderWidth = Unit.Pixel(1);
            creditSearchGridView.BorderWidth = Unit.Pixel(1);


            if (TransactionTypeDropDownList.Items.Count == 0)
            {
                PopulateTransactionTypeDropDownList();
                TransactionTypeDropDownList.SelectedIndex = 0;
            }

            TransactionSearchType transactionSearchType =  ((TransactionSearchType) Enum.Parse(typeof (TransactionSearchType), TransactionTypeDropDownList.SelectedValue));
            if (transactionSearchType == TransactionSearchType.Invoices) 
            {
                transactionSearchGridView.Visible = true;
                creditSearchGridView.Visible = false;
            } 
            else 
            {
                transactionSearchGridView.Visible = false;
                creditSearchGridView.Visible = true;
            }
        }

        protected void ExportButton_Click(object sender, ImageClickEventArgs e)
        {
            if (transactionSearchGridView.Visible)
                transactionSearchGridView.Export("Transaction Search for " + SessionWrapper.Instance.Get.CustomerFromQueryString.Name, TransactionDateRangePicker.SelectedDateRange);
            else if (creditSearchGridView.Visible)
                creditSearchGridView.Export("Credit Search for " + SessionWrapper.Instance.Get.CustomerFromQueryString.Name, TransactionDateRangePicker.SelectedDateRange);
        }


        public void PopulateTransactionScopeDropDownList(Dictionary<SearchScope, string> searchScope)
        {
            SearchScopeDropDownList.DataSource = searchScope;

            searchScopeLabel.Visible = SearchScopeDropDownList.Visible = searchScope.Count > 1;

            SearchScopeDropDownList.DataValueField = "Key";
            SearchScopeDropDownList.DataTextField = "Value";
            SearchScopeDropDownList.DataBind();
        }

        protected void TransactionSearchButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                ConfigureGrids();
                transactionSearchGridView.ResetPaginationAndFocus();
                creditSearchGridView.ResetPaginationAndFocus();

                PerformTransactionsSearch();
            }
        }

        private void PerformTransactionsSearch()
        {
            // start related ref:CFF-18
            if (SessionWrapper.Instance.Get == null && QueryString.ViewIDValue != null) 
            {
                targetName = ": " + SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Name;
            } 
            else  if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
            {
                targetName = ": " + SessionWrapper.Instance.Get.ClientFromQueryString.Name;
            }

            if (SessionWrapper.Instance.Get.CustomerFromQueryString != null)
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

            string batchFrom = "0";
            string batchTo = "0";
            try
            {
                Scope currentScope = this.CurrentScope();
                if (currentScope == Scope.AllClientsScope)
                {
                    presenter.CallBackHandler += presenter_CallBackHandler;
                    presenter.SearchTransactions(TransactionDateRangePicker.SelectedDateRange, InvoiceNumberTextBox.EncodedText,
                                                    (TransactionSearchType)Enum.Parse(typeof(TransactionSearchType), TransactionTypeDropDownList.SelectedValue),
                                                        SearchScope.AllClients, (CffCustomer)SessionWrapper.Instance.Get.CustomerFromQueryString, 
                                                                SessionWrapper.Instance.Get.ClientFromQueryString, batchFrom, batchTo);
                }
                else
                {
                    SearchScope xScope = (SearchScope)Enum.Parse(typeof(SearchScope), SearchScopeDropDownList.SelectedValue);
                    if (!SearchScopeDropDownList.Visible) {
                        xScope = (currentScope == Scope.ClientScope) ? SearchScope.AllCustomers : SearchScope.CurrentCustomer;
                    }

                    presenter.SearchTransactions(TransactionDateRangePicker.SelectedDateRange, InvoiceNumberTextBox.EncodedText,
                                            (TransactionSearchType)Enum.Parse(typeof(TransactionSearchType), TransactionTypeDropDownList.SelectedValue),
                                              xScope, (CffCustomer)SessionWrapper.Instance.Get.CustomerFromQueryString, SessionWrapper.Instance.Get.ClientFromQueryString, batchFrom, batchTo);
                }
            }
            catch (CffTimeoutException exception)
            {
                Logger logger = new Logger();
                logger.LogError(exception.GetBaseException());
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, GetType(), "showError",
                                                                      string.Format(
                                                                          "$('.searchError').text('{0}');",
                                                                          Cff_WebResource.transacionSearchTimeOutMessage),
                                                                      true);
            }
        }

        void presenter_CallBackHandler(object sender, EventArgs e)
        {
            //update search status bar
            if (presenter.SearchingStatus == 1)
            {
                SearchStatusPanel.Visible = true;
            }
            else
            {
                SearchStatusPanel.Visible = false;
            }
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);
            transactionSearchGridView.ResetPaginationAndFocus();
            creditSearchGridView.ResetPaginationAndFocus();
            PopulateTransactionTypeDropDownList();
            presenter.PopulateSearchScopeDropDownList();
            ShowHideTabNavigations();
            ConfigureGrids();

            //Reset Transaction Search Results To Blank
            DisplayMatchedTransactions(null);

            //Only trigger report function when user has entered invoice number, otherwise it will throw error
            Regex regex = new Regex(@"[\w\W]{3,}");
            if (regex.IsMatch(InvoiceNumberTextBox.EncodedText))
            {
                PerformTransactionsSearch();
            }
        }

        private void ShowHideTabNavigations()
        {
            bool isInCustomerScope = (this.CurrentScope() == Scope.CustomerScope);
            currentTransactionsLink.Visible = isInCustomerScope;
            transactionArchiveLink.Visible = isInCustomerScope;

            ISecurityManager securityManager = SecurityManagerFactory.Create(Context.User as CffPrincipal, this.CurrentScope());
            transactionHistoryLink.Visible = securityManager.CanViewTransactionHistoryLink();
        }

    }
}