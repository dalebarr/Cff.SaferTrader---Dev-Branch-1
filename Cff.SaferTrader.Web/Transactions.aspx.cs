using System;
using System.Windows;
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
    public partial class Transactions : BasePage, ITransactionsView
    {
        protected static string targetName = "";
        private CffGenGridView TransactionGridView;
        private TransactionsPresenter presenter;

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

        #region ITransactionsView Members

        public void ShowTransactions(IList<Transaction> transactions)
        {
            // start related ref:CFF-18
            if (SessionWrapper.Instance.Get != null)
            {
                if (SessionWrapper.Instance.Get.ClientFromQueryString!=null)
                    targetName = ": " + SessionWrapper.Instance.Get.ClientFromQueryString.Name;

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
            }
            else if (QueryString.ViewIDValue != null) {
                if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString != null)
                {
                    if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString != null)
                        targetName = ": " + SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Name;

                    if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString != null)
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
                }
            }
            // end
            ViewState.Add("Transactions", transactions);
            
            TransactionGridView.DataSource = null;
            TransactionGridView.DataSource = transactions;
            TransactionGridView.DataBind();
            TransactionsPanel.Wrap = true;
        }

        #endregion

        protected void TransactionGridViewCustomCallback(object sender, ReportGridViewCustomCallbackEventArgs e)
        {
            CallbackParameter parameter = CallbackParameter.Parse(e.Parameters);

            var transaction = (Transaction) TransactionGridView.GetRow(parameter.RowIndex);
            var redirectionParameter = new RedirectionParameter(parameter.FieldName, SessionWrapper.Instance.Get.ClientFromQueryString.Id, SessionWrapper.Instance.Get.CustomerFromQueryString.Id, transaction.Batch);

            ISecurityManager securityManager = SecurityManagerFactory.Create(Context.User as CffPrincipal, SessionWrapper.Instance.Get.Scope);
            Redirector redirector = new Redirector(RedirectionService.Create(this, securityManager));
            redirector.Redirect(redirectionParameter);
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            TransactionGridView.Style.Add(HtmlTextWriterStyle.Width, "100%");
            TransactionGridView.BorderWidth = Unit.Pixel(1);

            if (Request.Browser.Browser == "IE" || Request.Browser.Browser == "Firefox")
            {
                //TransactionGridView.SettingsBehavior.ColumnResizeMode = ColumnResizeMode.NextColumn;
            }
            else
            {
                //TransactionGridView.Settings.UseFixedTableLayout = false;
                System.Web.UI.WebControls.Unit xWidth = new System.Web.UI.WebControls.Unit(100, System.Web.UI.WebControls.UnitType.Percentage);
                TransactionGridView.Width = xWidth;
                TransactionGridView.Attributes.Add("style", "table-layout:auto;width:100%;column-width: auto;");
            }

        }


        protected override void Page_Init(object sender, EventArgs e)
        {
            base.Page_Init(sender, e);

            TransactionGridView = new CffGenGridView();
            //TransactionGridView.Width = Unit.Percentage(100);
            TransactionGridView.PageSize = 250;
            TransactionGridView.AllowPaging = false;
            TransactionGridView.AutoGenerateColumns = false;
            TransactionGridView.SetSortExpression = "Date";
            //TransactionGridView.BorderColor = System.Drawing.Color.LightGray;
            TransactionGridView.HeaderStyle.CssClass = "dxgvHeader table"; // "cffGGVHeader";
         
            TransactionGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
            TransactionGridView.ShowHeaderWhenEmpty = true;
            TransactionGridView.EmptyDataText = "No data to display";
            TransactionGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            TransactionGridView.SelectedRowStyle.BackColor = System.Drawing.Color.Honeydew;
            //TransactionGridView.RowStyleHighlightColour = System.Drawing.Color.Honeydew;
            TransactionGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;

            TransactionGridView.TotalsSummarySettings.TableClass = "dxgvFooter"; //"cffGGVFooter";  //dbb
            TransactionGridView.TotalsSummarySettings.SetColumnTotals("Amount, Balance");
            TransactionGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Amount", "cffGGV_rightAlignedCell");
            TransactionGridView.TotalsSummarySettings.SetSummaryTotalColumnCssStyle("Balance", "cffGGV_rightAlignedCell");
            TransactionGridView.TotalsSummarySettings.TotalsSummaryText = "Totals";
            TransactionGridView.TotalsSummarySettings.TotalsSummaryTextStyle = "cffGGV_leftAlignedCell";   //dbb
            TransactionGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;

            TransactionGridViewPanel.Controls.Clear();
            TransactionGridViewPanel.Controls.Add(TransactionGridView);

        }

        protected void ConfigureGridColumns() 
        {
            TransactionGridView.Columns.Clear();
            TransactionGridView.InsertDataColumn("Date", "Date", CffGridViewColumnType.Date, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Left, HorizontalAlign.Center, false);
            TransactionGridView.InsertDataColumn("Processed", "Processed", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Left, HorizontalAlign.Center, false);
            TransactionGridView.InsertDataColumn("Type", "Type", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, false);
            TransactionGridView.InsertDataColumn("Number", "Number", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, false);
            TransactionGridView.InsertDataColumn("Reference", "Reference", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Left, HorizontalAlign.Center, false);
            TransactionGridView.InsertCurrencyColumn("Amount", "Amount", "8%", "cffGGV_rightAlignedCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            TransactionGridView.InsertCurrencyColumn("Balance", "Balance", "8%", "cffGGV_rightAlignedCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            TransactionGridView.InsertBoundHyperLinkColumn("Batch", "Batch", "Batch", "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            ConfigureGridColumns();
            ISecurityManager securityManager = SecurityManagerFactory.Create(Context.User as CffPrincipal, this.CurrentScope());
            presenter = new TransactionsPresenter(this, RepositoryFactory.CreateTransactionRepository(), RedirectionService.Create(this, securityManager));
            presenter.InitializeForScope(this.CurrentScope());


            int customerId = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.CustomerFromQueryString.Id : SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString.Id;
            if (IsPostBack)
            {
                if (ChkBoxTransactionInvoices.Checked) {
                    presenter.LoadCurrentTransactionsInvoices(customerId);
                } 
                else {
                    presenter.LoadCurrentTransactions(customerId);
                }
            }
            else
            {
                presenter.LoadCurrentTransactions(customerId);
            }

            TransactionCurrentLink.Visible = false;
            TransactionArchiveLink.HRef = "~/TransactionArchive.aspx" + QueryStringParameters;
            TransactionHistoryLink.HRef = "~/TransactionHistory.aspx" + QueryStringParameters;
            TransactionSearchLink.HRef = "~/TransactionSearch.aspx" + QueryStringParameters;
        }

    
        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);

            presenter.InitializeForScope(SessionWrapper.Instance.Get.Scope);

            TransactionGridView.ResetPaginationAndFocus();
            presenter.LoadCurrentTransactions(SessionWrapper.Instance.Get.CustomerFromQueryString.Id);
        }

        protected void ExportButton_Click(object sender, EventArgs e)
        {
            TransactionGridView.Export("Current Transactions for " + SessionWrapper.Instance.Get.CustomerFromQueryString.Name);
        }

        protected void ChkBoxTransactionInvoices_CheckedChanged(object sender, EventArgs e)
        {
           
        }
    }
}