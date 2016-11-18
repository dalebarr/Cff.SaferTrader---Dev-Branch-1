using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters.ReportPresenters;
using Cff.SaferTrader.Core.ReportManager;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.ReportView;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;


namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public partial class OverdueChargesTab : UserControl, IRetentionTab, IOverdueChargesView,  Cff.SaferTrader.Core.Views.IPrintableView
    {
        private OverdueChargesPresenter presenter;
        private RetentionSchedule _retentionSchedule;

        CffGenGridView ReportGridView;
        protected override void OnInit(EventArgs e)
        {
            InitializeGridView();
        }

        private void InitializeGridView()
        {

            ReportGridView = new CffGenGridView();
           
            ReportGridView.AutoGenerateColumns = false;
            ReportGridView.SetSortExpression = "Date";

            //ReportGridView.CssClass = "cffGGV";
            ReportGridView.HeaderStyle.CssClass = "cffGGVHeader";
            ReportGridView.ShowHeaderWhenEmpty = true;
            ReportGridView.EmptyDataText = "No data to display";
            ReportGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";
            ReportGridView.EnableViewState = true;
            ReportGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;

            ReportGridView.BorderWidth = Unit.Pixel(1);
            ReportGridView.Width = Unit.Percentage(70);

            ReportGridView.Columns.Clear();
            ReportGridView.InsertDataColumn("#", "CustomerNumber", CffGridViewColumnType.Text, "4%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertBoundHyperLinkColumn("Customer", "CustomerName", "CustomerID", "19%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left);
            ReportGridView.InsertDataColumn("Title", "Title", CffGridViewColumnType.Text, "5%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            ReportGridView.InsertDataColumn("Factored", "Factored", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            ReportGridView.InsertDataColumn("Aged", "Age", CffGridViewColumnType.Text, "2%", "cffGGV_rightAlignedCell", HorizontalAlign.Right, HorizontalAlign.Right, true);
            ReportGridView.InsertDataColumn("Transaction", "Number", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCellAgedBal", HorizontalAlign.Left, HorizontalAlign.Left, true, "cffGGVHeaderLeftAgedBal2");
            ReportGridView.InsertDataColumn("Reference", "Reference", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);

            ReportGridView.InsertCurrencyColumn("Charges", "Charges", "7%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertCurrencyColumn("GST Charges", "ChargesWithGst", "7%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertCurrencyColumn("Amount", "Amount", "7%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);
            ReportGridView.InsertCurrencyColumn("Balance", "Balance", "7%", "cffGGV_currencyCell", false, HorizontalAlign.Right, HorizontalAlign.Right);

            ReportGridView.InsertDataColumn("Status", "Status", CffGridViewColumnType.Text, "3%", "cffGGV_centerAlignedCell", HorizontalAlign.Center, HorizontalAlign.Center, true);

            ReportGridView.ShowFooter = true;
            ReportGridView.TotalsSummarySettings.SetColumnTotals("Charges,ChargesWithGst,Amount,Balance");
            string[] strDummy = "Charges,ChargesWithGst,Amount,Balance".Split(',');
            foreach (string x in strDummy)
                ReportGridView.TotalsSummarySettings.SetTotalsColumnCssStyle(x, "cffGGV_currencyCell");
            ReportGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;

            ReportGridView.PageSize = 1000;
            ReportGridView.DefaultPageSize = 1000;
            ReportGridView.AllowPaging = false; //removed as per marty's suggestions
            //ReportGridView.CustomPagerSettingsMode = CffCustomPagerMode.Rows | CffCustomPagerMode.Page | CffCustomPagerMode.FirstLast | CffCustomPagerMode.PreviousNext | CffCustomPagerMode.Bottom;
            ReportGridView.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;

            GVPlaceHolder.Controls.Clear();
            GVPlaceHolder.Controls.Add(ReportGridView);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            StatusPicker.Update += StatusPickerUpdate;

            if (IsPostBack)
            {
                DisplayReport(ViewState["OverdueChargesReport"] as OverdueChargesReport);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        protected void StatusPickerUpdate(object sender, EventArgs e)
        {
            RetentionSchedule retentionSchedule = ViewState["RetentionSchedule"] as RetentionSchedule;
            LoadTab(retentionSchedule);
        }

        public void LoadTab(RetentionSchedule retentionSchedule)
        {
            bool bIsClientSelected = (SessionWrapper.Instance.Get!=null)?SessionWrapper.Instance.Get.IsClientSelected:
                                          (!string.IsNullOrEmpty(QueryString.ViewIDValue))?SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsClientSelected:false;

            if (bIsClientSelected)
            {
                _retentionSchedule = retentionSchedule;
                ViewState.Add("RetentionSchedule", retentionSchedule);

                // In Retention Schedules, the tab is always opened in Client scope
                IReportManager reportManager = ReportManagerFactory.Create(Scope.ClientScope, Context.User as CffPrincipal);
                presenter = new OverdueChargesPresenter(this, ReportsService.Create(), reportManager);
                presenter.ShowReport(Scope.ClientScope, false);
            }
        }

        public void ClearTabData()
        {
            ReportGridView.DataSource = null;
            ReportGridView.DataBind();
            DateViewedLiteral.Text = string.Empty;
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

        #region IOverdueChargesView Member

        public Date EndDate()
        {
            if (_retentionSchedule == null)
                return new Date(DateTime.Now);

            return _retentionSchedule.EndOfMonth;
        }

        public int ClientId()
        {
            if (_retentionSchedule == null)
                return 0;

            return _retentionSchedule.ClientId;
        }

        public int ClientFacilityType()
        {
            if (_retentionSchedule == null)
                return 0;

            return _retentionSchedule.ClientFacilityType;
        }

        public void DisplayReport(OverdueChargesReport report)
        {
            ViewState.Add("OverdueChargesReport", report);

            OverdueChargesReport overdueChargesReport = report;
            if (report != null)
            {
                DateViewedLiteral.Text = report.DateViewed.ToDateTimeString();

                ReportGridView.DataSource = overdueChargesReport.Records;
                ReportGridView.DataBind();
            }
        }
        public void Clear()
        {
            throw new NotImplementedException();
        }
        public bool IsSalvageIncluded()
        {
            throw new NotImplementedException();
        }
        public TransactionStatus TransactionStatus()
        {
            return StatusPicker.Status;
        }
        public FacilityType FacilityType()
        {
            throw new NotImplementedException();
        }
        public void ShowAllClientsView()
        {
            throw new NotImplementedException();
        }
        public void ShowClientView()
        {
            throw new NotImplementedException();
        }
        public void ShowCustomerView()
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Everyone with access to Retention Schedules has access
        /// </summary>
        public void DisplayAccessDeniedError()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The tab is to be always available
        /// </summary>
        public void DisplayReportNotAvailableError()
        {
            throw new NotImplementedException();
        }

        #region IPrintableViews member
            public void Print()
            { //CFF-13
                PrintableOverdueCharges printable =
                   new PrintableOverdueCharges(ViewState["OverdueChargesReport"] as OverdueChargesReport,
                                     ViewState["RetentionSchedule"] as RetentionSchedule, QueryString.ViewIDValue);

                string script = PopupHelper.ShowPopup(printable, Server);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
            }
        #endregion

    }
}