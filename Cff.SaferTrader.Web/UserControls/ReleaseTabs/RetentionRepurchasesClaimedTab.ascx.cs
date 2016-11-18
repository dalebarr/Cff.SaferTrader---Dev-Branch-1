using System;
using System.Collections.Generic;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;

using System.Web.UI.WebControls;
using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public partial class RetentionRepurchasesClaimedTab : UserControl, IRetentionRepurchasesClaimedTabView,
                                                          IRetentionTab, IRedirectableView, IPrintableView
    {
        private RetentionRepurchasesClaimedTabPresenter presenter;
        public CffGenGridView RetentionRepurchasesClaimedGridView;

        #region IRetentionRepurchasesClaimedTabView
        public void DisplayRetentionRepurchasesClaimed(IList<ClaimedRetentionRepurchase> claimedRetentionRepurchases)
        {
            ViewState.Add("claimedRetentionRepurchases", claimedRetentionRepurchases);
            RetentionRepurchasesClaimedGridView.DataSource = claimedRetentionRepurchases;
            RetentionRepurchasesClaimedGridView.DataBind();
        }
        #endregion

  
        protected override void OnInit(EventArgs e)
        {
            RetentionRepurchasesClaimedGridView = new CffGenGridView();
            RetentionRepurchasesClaimedGridView.ID = "RetentionRepurchasesGridView";
            RetentionRepurchasesClaimedGridView.AutoGenerateColumns = false;
            RetentionRepurchasesClaimedGridView.SetSortExpression = "Date";
            RetentionRepurchasesClaimedGridView.AutoGenerateColumns = false;
            RetentionRepurchasesClaimedGridView.AllowPaging = false; //suggested by marty

            RetentionRepurchasesClaimedGridView.BorderWidth = Unit.Pixel(1);
            //RetentionRepurchasesClaimedGridView.CssClass = "cffGGV";
            RetentionRepurchasesClaimedGridView.HeaderStyle.CssClass = "cffGGVHeader";
            RetentionRepurchasesClaimedGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            RetentionRepurchasesClaimedGridView.ShowHeaderWhenEmpty = true;
            RetentionRepurchasesClaimedGridView.EmptyDataText = "No data to display";
            RetentionRepurchasesClaimedGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            RetentionRepurchasesClaimedGridView.EnableViewState = true;


            RetentionRepurchasesClaimedGridView.ShowFooter = true;
            RetentionRepurchasesClaimedGridView.TotalsSummarySettings.SetColumnTotals("Amount");
            RetentionRepurchasesClaimedGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            RetentionRepurchasesClaimedGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            RetentionRepurchasesClaimedGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
         

            RRPCPlaceholder.Controls.Clear();
            RRPCPlaceholder.Controls.Add(RetentionRepurchasesClaimedGridView);
        }

        private void ConfigureGridColumns()
        {
            RetentionRepurchasesClaimedGridView.Columns.Clear();
            RetentionRepurchasesClaimedGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            RetentionRepurchasesClaimedGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "25%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            RetentionRepurchasesClaimedGridView.InsertDataColumn("Transaction", "Transaction", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            RetentionRepurchasesClaimedGridView.InsertDataColumn("Dated", "Dated", CffGridViewColumnType.Date, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true); 
            //RetentionRepurchasesClaimedGridView.InsertDataColumn("Age", "Age", CffGridViewColumnType.Text, "10%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true); //as per marty
            RetentionRepurchasesClaimedGridView.InsertCurrencyColumn("Amount", "Amount", "7%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);

            RetentionRepurchasesClaimedGridView.InsertDataColumn("Batch", "Batch", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Center, HorizontalAlign.Center, true, "cffGGVHeaderLeftAgedBal2");
            RetentionRepurchasesClaimedGridView.InsertDataColumn("Created", "Created", CffGridViewColumnType.Date, "4%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            RetentionRepurchasesClaimedGridView.Width = Unit.Percentage(50);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureGridColumns();
            
            if (IsPostBack)
            {
                DisplayRetentionRepurchasesClaimed(ViewState["claimedRetentionRepurchases"] as IList<ClaimedRetentionRepurchase>);
            }
        }

        public void LoadTab(RetentionSchedule retentionSchedule)
        {
            ViewState.Add("RetentionSchedule", retentionSchedule);
            if (retentionSchedule == null)
                return;

            if (SessionWrapper.Instance.Get.ClientFromQueryString.Id != 0)
            {
                ViewState.Add("ClientId", retentionSchedule.ClientId);

                presenter = RetentionRepurchasesClaimedTabPresenter.Create(this);
                presenter.LoadClaimedetentionRepurchasesFor(retentionSchedule, retentionSchedule.ClientId);
            }
        }

        public void ClearTabData()
        {
            DisplayRetentionRepurchasesClaimed(null);
        }

        #region IRedirectableView Members

        public void RedirectTo(string redirectionPath)
        {
            Response.Redirect(redirectionPath);
        }

        public ICffClient Client
        {
            get
            {
                if (SessionWrapper.Instance.Get != null)
                    return SessionWrapper.Instance.Get.ClientFromQueryString;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString;

                return null;
            }
            set { }
        }

        //public CffCustomer Customer
        public ICffCustomer Customer
        {
            get
            {
                if (SessionWrapper.Instance.Get != null)
                    return SessionWrapper.Instance.Get.CustomerFromQueryString;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString;

                return null;
            }

            set
            {
                if (SessionWrapper.Instance.Get != null)
                    SessionWrapper.Instance.Get.CustomerFromQueryString = (ICffCustomer)value;

                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString = (ICffCustomer)value;
            }
        }


        #endregion

        #region IPrintableView Members       
            public void Print()
            {  //Ref: CFF-13
                PrintableRepurchasesClaimed printable =
                        new PrintableRepurchasesClaimed(ViewState["claimedRetentionRepurchases"] as IList<ClaimedRetentionRepurchase>,
                            ViewState["RetentionSchedule"] as RetentionSchedule, QueryString.ViewIDValue);

                string script = PopupHelper.ShowPopup(printable, Server);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
            }
        #endregion

    }
}