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
    public partial class TransactionArchive : BasePage, ITransactionArchiveView
    {
        protected static string targetName = "";

        public CffGenGridView TransactionGridView;

        private TransactionArchivePresenter presenter;

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

       
        protected void TransactionGridViewCustomCallback(object sender, ReportGridViewCustomCallbackEventArgs e)
        {
            CallbackParameter parameter = CallbackParameter.Parse(e.Parameters);
            ArchivedTransaction transaction = (ArchivedTransaction)TransactionGridView.GetRow(parameter.RowIndex);

            var redirectionParameter = new RedirectionParameter(parameter.FieldName, SessionWrapper.Instance.Get.ClientFromQueryString.Id, SessionWrapper.Instance.Get.CustomerFromQueryString.Id, transaction.Batch);

            ISecurityManager securityManager = SecurityManagerFactory.Create(Context.User as CffPrincipal, SessionWrapper.Instance.Get.Scope);
            Redirector redirector = new Redirector(RedirectionService.Create(this, securityManager));
            redirector.Redirect(redirectionParameter);
        }

        protected override void Page_Init(object sender, EventArgs e)
        {
            base.Page_Init(sender, e);
            InitializeTransactionGridView();
        }

     
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureGridColumns();
            MonthRangePicker.Update += MonthRangePickerUpdate;
            ISecurityManager securityManager = SecurityManagerFactory.Create(Context.User as CffPrincipal, this.CurrentScope());
            presenter = new TransactionArchivePresenter(this, RepositoryFactory.CreateTransactionRepository(), RedirectionService.Create(this, securityManager));
            presenter.InitializeForScope(this.CurrentScope());

            int? custId = (SessionWrapper.Instance.Get == null) ? 
                             ((QueryString.ViewIDValue==null)? 0 : SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString.Id)
                                  : SessionWrapper.Instance.Get.CustomerFromQueryString.Id;
            if (IsPostBack)
            {
                presenter.LoadTransactionArchive(MonthRangePicker.DateRange,(int)custId, ChkBoxTransactionInvoices.Checked);
            }
            else
            {
                presenter.LoadTransactionArchive(MonthRangePicker.DateRange, (int)custId, ChkBoxTransactionInvoices.Checked);
            }

            TransactionsLink.HRef = "~/Transactions.aspx" + QueryStringParameters;
            TransactionHistoryLink.HRef = "~/TransactionHistory.aspx" + QueryStringParameters;
            TransactionSearchLink.HRef = "~/TransactionSearch.aspx" + QueryStringParameters;
        }

        private void InitializeTransactionGridView()
        {
            TransactionGridView = new CffGenGridView();
            TransactionGridView.ID = "TransactionGridView";
            TransactionGridView.KeyFieldName = "Id";
            TransactionGridView.AutoGenerateColumns = false;
            TransactionGridView.ShowHeaderWhenEmpty = true;
            TransactionGridView.EnableViewState = true;

            TransactionGridView.HeaderStyle.CssClass="cffGGVHeader";
            //TransactionGridView.RowStyleHighlightColour = System.Drawing.Color.Honeydew;
            TransactionGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            TransactionGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            TransactionGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

            if (Request.Browser.Id.ToString().ToUpper().Contains("IE"))
            {
            }
            else
            {
                TransactionGridView.BorderColor = System.Drawing.Color.LightGray;
            }
            TransactionGridView.PageSize = 250;
            TransactionGridView.BorderWidth = Unit.Pixel(1);
            TransactionGridView.AllowPaging = true;
            TransactionGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;
        
            TransactionGridViewPlaceHolder.Controls.Add(TransactionGridView);
        }


        protected void ConfigureGridColumns()
        {
            TransactionGridView.Columns.Clear();
            TransactionGridView.InsertDataColumn("Date", "Date", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            TransactionGridView.InsertDataColumn("Archived", "Archived", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            TransactionGridView.InsertDataColumn("Processed", "Processed", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            TransactionGridView.InsertDataColumn("Type", "Type", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            TransactionGridView.InsertDataColumn("Number", "Number", CffGridViewColumnType.Text, "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            TransactionGridView.InsertDataColumn("Reference", "Reference", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            TransactionGridView.InsertCurrencyColumn("Amount", "Amount", "6%", "cffGGV_currencyCell", true, HorizontalAlign.Right, HorizontalAlign.Right);
            TransactionGridView.InsertDataColumn("Status", "Status", CffGridViewColumnType.Text, "4%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            TransactionGridView.InsertBoundHyperLinkColumn("Batch", "Batch", "Batch", "5%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, "Type:Invoice:On:IsReversed");

            TransactionGridView.ShowFooter = true;
            TransactionGridView.TotalsSummarySettings.TableClass = "dxgvFooter"; //dbb
            TransactionGridView.TotalsSummarySettings.SetColumnTotals("Amount");
            TransactionGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyTotalTrxArch");
            TransactionGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            TransactionGridView.Style.Add(HtmlTextWriterStyle.Width, "100%");
            TransactionGridView.BorderWidth = Unit.Pixel(1);

            if (Request.Browser.Browser == "IE" || Request.Browser.Browser == "Firefox") {
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

        private void MonthRangePickerUpdate(object sender, EventArgs e)
        {
            if (SessionWrapper.Instance.Get.IsCustomerSelected)
            {
                TransactionGridView.ResetPaginationAndFocus();
                presenter.LoadTransactionArchive(MonthRangePicker.DateRange, SessionWrapper.Instance.Get.CustomerFromQueryString.Id, ChkBoxTransactionInvoices.Checked);
            }
        }

        public void ShowTransactionArchive(IList<ArchivedTransaction> archivedTransactions)
        {
            // start related ref:CFF-18
            if (SessionWrapper.Instance.Get == null && QueryString.ViewIDValue!=null) {
                targetName = ": " + SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Name;
                if (SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString != null)
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
            else if (SessionWrapper.Instance.Get != null)
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
            // end

            ViewState.Add("Transactions", archivedTransactions);

            TransactionGridView.DataSource = null;
            TransactionGridView.DataSource = archivedTransactions;
            TransactionGridView.DataBind();
            TransactionGridView.Visible = true;
            TransactionGridViewPlaceHolder.Visible = true;
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);
            TransactionGridView.ResetPaginationAndFocus();

            if (SessionWrapper.Instance.Get == null && QueryString.ViewIDValue != null) {
                if (ChkBoxTransactionInvoices.Checked)
                {
                    presenter.LoadTransactionArchive(MonthRangePicker.DateRange, SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString.Id, true);
                }
                else
                {
                    presenter.LoadTransactionArchive(MonthRangePicker.DateRange, SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString.Id, false);
                }
            } else if (SessionWrapper.Instance.Get.IsCustomerSelected)
            {
              
                if (ChkBoxTransactionInvoices.Checked)
                {
                    presenter.LoadTransactionArchive(MonthRangePicker.DateRange, SessionWrapper.Instance.Get.CustomerFromQueryString.Id, true);
                }
                else
                {
                    presenter.LoadTransactionArchive(MonthRangePicker.DateRange, SessionWrapper.Instance.Get.CustomerFromQueryString.Id, false);
                }
            }
            transactionArchivePageDescription.Visible = true;
        }

        protected void ExportButton_Click(object sender, ImageClickEventArgs e)
        {
            TransactionGridView.Export("Transaction Archive for " + SessionWrapper.Instance.Get.CustomerFromQueryString.Name, MonthRangePicker.DateRange);
        }

        protected void ChkBoxTransactionInvoices_CheckedChanged(object sender, EventArgs e)
        {
            //TransactionArchiveUpdatePanel.DataBind();
        }
    }
}