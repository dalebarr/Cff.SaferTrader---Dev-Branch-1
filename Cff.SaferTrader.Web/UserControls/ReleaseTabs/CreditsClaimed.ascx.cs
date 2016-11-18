using System;
using System.Collections.Generic;

using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;


namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public partial class CreditsClaimed : UserControl, IRetentionTab, ICreditsClaimedTabView, IRedirectableView, IPrintableView
    {
        private CreditsClaimedTabPresenter presenter;


        CffGenGridView CffGGV_CreditsClaimedGridView;

        protected override void OnInit(EventArgs e)
        {

            CffGGV_CreditsClaimedGridView = new CffGenGridView();
            CffGGV_CreditsClaimedGridView.AllowPaging = false; //removed as suggested
            CffGGV_CreditsClaimedGridView.AutoGenerateColumns = false;
            CffGGV_CreditsClaimedGridView.SetSortExpression = "tranref";
            CffGGV_CreditsClaimedGridView.BorderWidth = Unit.Pixel(1);
            
            CffGGV_CreditsClaimedGridView.HeaderStyle.CssClass = "cffGGVHeader";
            CffGGV_CreditsClaimedGridView.ShowHeaderWhenEmpty = true;
            CffGGV_CreditsClaimedGridView.EmptyDataText = "No data to display";
            CffGGV_CreditsClaimedGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
          
            CffGGV_CreditsClaimedGridView.ShowFooter = true;
            CffGGV_CreditsClaimedGridView.TotalsSummarySettings.SetColumnTotals("Amount, Sum");
            CffGGV_CreditsClaimedGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            CffGGV_CreditsClaimedGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Sum", "cffGGV_currencyCell");
            CffGGV_CreditsClaimedGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
            //CffGGV_CreditsClaimedGridView.CustomPagerSettingsMode = CffCustomPagerMode.Page | CffCustomPagerMode.Rows | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext;
         
            CCGridViewPlaceHolder.Controls.Clear();
            CCGridViewPlaceHolder.Controls.Add(CffGGV_CreditsClaimedGridView);
        }

        protected void ConfigureGridColumns() 
        {
            CffGGV_CreditsClaimedGridView.Columns.Clear();
            CffGGV_CreditsClaimedGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_CreditsClaimedGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerId", "18%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            CffGGV_CreditsClaimedGridView.InsertDataColumn("Transaction", "Transaction", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_CreditsClaimedGridView.InsertDataColumn("Dated", "Dated", CffGridViewColumnType.Date, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            CffGGV_CreditsClaimedGridView.InsertCurrencyColumn("Amount", "Amount", "7%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            CffGGV_CreditsClaimedGridView.InsertCurrencyColumn("Sum", "Sum", "7%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);

            CffGGV_CreditsClaimedGridView.InsertBoundHyperLinkColumn("Batch", "Batch", "Batch", "5%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, "", "cffGGVHeaderLeftAgedBal2");
            CffGGV_CreditsClaimedGridView.InsertDataColumn("Created", "Created", CffGridViewColumnType.Date, "4%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_CreditsClaimedGridView.Width = Unit.Percentage(50);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureGridColumns();

            if (IsPostBack)
            {
                DisplayCreditsClaimed(ViewState["CreditsClaimed"] as IList<ClaimedCredit>);
            }
        }

        public void LoadTab(RetentionSchedule retentionSchedule)
        {
            ViewState.Add("RetentionSchedule", retentionSchedule);
            if (retentionSchedule == null)
                return;

            ViewState.Add("ClientId", retentionSchedule.ClientId);

            presenter = CreditsClaimedTabPresenter.Create(this);
            presenter.LoadCreditsClaimed(retentionSchedule, retentionSchedule.ClientId);
        }

        public void ClearTabData()
        {
            DisplayCreditsClaimed(null);
        }

        public void DisplayCreditsClaimed(IList<ClaimedCredit> creditsClaimed)
        {
            ViewState.Add("CreditsClaimed", creditsClaimed);

            try
            {
                CffGGV_CreditsClaimedGridView.DataSource = creditsClaimed;
                CffGGV_CreditsClaimedGridView.DataBind();
            }
            catch (Exception exc)
            {
                string msg = exc.Message;
            }
        }

        #region IRedirectableView Members

        public void RedirectTo(string redirectionPath)
        {
            Response.Redirect(redirectionPath);
        }

        public ICffClient Client
        {
            get {
                if (SessionWrapper.Instance.Get != null)
                    return SessionWrapper.Instance.Get.ClientFromQueryString;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString;
                return null;                
            }
            set { }
        }

        public ICffCustomer Customer
        {
            get {
                if (SessionWrapper.Instance.Get != null)
                    return SessionWrapper.Instance.Get.CustomerFromQueryString;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString;
                return null;
            }

            set {
                if (SessionWrapper.Instance.Get != null)
                    SessionWrapper.Instance.Get.CustomerFromQueryString = (ICffCustomer)value;

                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString = (ICffCustomer)value; 
            }
        }

        #endregion

        #region IPrintableView Members
            public void Print()
            { //Ref: CFF-13
                PrintableCreditsClaimed printable =
                    new PrintableCreditsClaimed(ViewState["CreditsClaimed"] as IList<ClaimedCredit>,
                                                      ViewState["RetentionSchedule"] as RetentionSchedule, QueryString.ViewIDValue);
                string script = PopupHelper.ShowPopup(printable, Server);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
            }
        #endregion



    }
}