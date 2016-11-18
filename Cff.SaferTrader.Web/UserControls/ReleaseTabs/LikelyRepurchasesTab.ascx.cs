using System;
using System.Collections.Generic;

using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;

using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;


namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public partial class LikelyRepurchasesTab : UserControl, IRetentionTab, ILikelyRepurchasesTabView, IRedirectableView, IPrintableView
    {
        private LikelyRepurchasesTabPresenter presenter;
        private CffGenGridView LikelyRepurchasesGridView;
    
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            LikelyRepurchasesGridView = new CffGenGridView();
            LikelyRepurchasesGridView.ID = "LikelyRepurchasesGridView";
            LikelyRepurchasesGridView.HeaderStyle.CssClass = "cffGGVHeader";
            LikelyRepurchasesGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            
            LikelyRepurchasesGridView.AutoGenerateColumns = false;
            LikelyRepurchasesGridView.ShowHeaderWhenEmpty = true;
            LikelyRepurchasesGridView.EmptyDataText = "No data to display";
            LikelyRepurchasesGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            LikelyRepurchasesGridView.EnableViewState = true;

            LikelyRepurchasesGridView.PageSize = 1000;
            LikelyRepurchasesGridView.ShowFooter = true;
            LikelyRepurchasesGridView.AllowPaging = false; //removed as suggested
            LikelyRepurchasesGridView.Width = Unit.Percentage(70);
            //LikelyRepurchasesGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;
            LikelyRepurchasesGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

            likelyRepurchasesPlaceHolder.Controls.Clear();
            likelyRepurchasesPlaceHolder.Controls.Add(LikelyRepurchasesGridView);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureGridColumns();

            if (IsPostBack)
            {
                DisplayLikelyRepurchases(ViewState["LikelyRepurchases"] as IList<LikelyRepurchasesLine>);
            }
           
        }

        private void ConfigureGridColumns()
        {
            LikelyRepurchasesGridView.Columns.Clear();
            LikelyRepurchasesGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "4%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            LikelyRepurchasesGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustId", "24%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            LikelyRepurchasesGridView.InsertDataColumn("Age", "Age", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            LikelyRepurchasesGridView.InsertCurrencyColumn("Amount", "Amount", "7%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            LikelyRepurchasesGridView.InsertCurrencyColumn("Balance", "Balance", "7%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            LikelyRepurchasesGridView.InsertCurrencyColumn("Sum", "Sum", "7%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);

            LikelyRepurchasesGridView.InsertDataColumn("Dated", "Dated", CffGridViewColumnType.Date, "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);
            LikelyRepurchasesGridView.InsertDataColumn("Processed", "Processed", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            LikelyRepurchasesGridView.InsertDataColumn("Transaction", "Transaction", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            LikelyRepurchasesGridView.InsertDataColumn("Reference", "Reference", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            LikelyRepurchasesGridView.TotalsSummarySettings.SetColumnTotals("Amount, Balance, Sum");
            LikelyRepurchasesGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            LikelyRepurchasesGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Balance", "cffGGV_currencyCell");
            LikelyRepurchasesGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Sum", "cffGGV_currencyCell");
            LikelyRepurchasesGridView.ShowFooter = true;
            LikelyRepurchasesGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
        }

        public void DisplayLikelyRepurchases(IList<LikelyRepurchasesLine> likelyRepurchasesLine)
        {
            ViewState.Add("LikelyRepurchases", likelyRepurchasesLine);
            LikelyRepurchasesGridView.DataSource = likelyRepurchasesLine;
            if (likelyRepurchasesLine!=null)
                LikelyRepurchasesGridView.DataBind();
        }

        public void LoadTab(RetentionSchedule retentionSchedule)
        {
            try
            {
                if (retentionSchedule != null)
                {
                    ViewState.Add("RetentionSchedule", retentionSchedule);
                    ViewState.Add("RentionClientID", retentionSchedule.ClientId);

                    presenter = LikelyRepurchasesTabPresenter.Create(this);
                    presenter.LoadLikelyRepurchasesLinesFor(retentionSchedule.ClientId, retentionSchedule.Id,
                           0, (int)QueryString.UserId, retentionSchedule.EndOfMonth.ToShortDateString());
                }
                else
                {
                    presenter = LikelyRepurchasesTabPresenter.Create(this);
                    presenter.LoadLikelyRepurchasesLinesFor((int)QueryString.ClientId, -1,
                                    0, (int)QueryString.UserId, DateTime.Now.ToShortDateString());
                }
            }
            catch (Exception exc)
            {
                string strExc = exc.Message;
            }
        }


        public void ClearTabData()
        {
            DisplayLikelyRepurchases(null);
        }


        public void Print()
        {
            PrintableLikelyRepurchases printable = new PrintableLikelyRepurchases(ViewState["LikelyRepurchases"] as IList<LikelyRepurchasesLine>, ViewState["RetentionSchedule"] as RetentionSchedule, QueryString.ViewIDValue);
           string script = PopupHelper.ShowPopup(printable, Server);

           ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
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

    }
}