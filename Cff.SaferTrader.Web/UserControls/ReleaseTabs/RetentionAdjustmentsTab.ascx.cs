using System;
using System.Collections.Generic;

using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Views;

using System.Web.UI;
using System.Web.UI.WebControls;

using Cff.SaferTrader.Web.UserControls;
using Cff.SaferTrader.Web.UserControls.gGridViewControls;

namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public partial class RetentionAdjustmentsTab : UserControl, IRetentionTab, IChargesTabView, IPrintableView
    {
        private ChargesTabPresenter presenter;

        CffGenGridView CffGGV_ChargesGridView;

        protected override void OnInit(EventArgs e)
        {
             base.OnInit(e);

            CffGGV_ChargesGridView = new CffGenGridView();
            CffGGV_ChargesGridView.ID = "CffGGV_ChargesGridView";
            CffGGV_ChargesGridView.AllowPaging = false; //as suggested by marty
            CffGGV_ChargesGridView.AutoGenerateColumns = false;
            CffGGV_ChargesGridView.BorderWidth = Unit.Pixel(1);
            CffGGV_ChargesGridView.Width = Unit.Percentage(50);

            //CffGGV_ChargesGridView.CssClass = "cffGGV";
            CffGGV_ChargesGridView.HeaderStyle.CssClass = "cffGGVHeader";
            CffGGV_ChargesGridView.AlternatingRowStyle.BackColor = System.Drawing.Color.Honeydew;
            CffGGV_ChargesGridView.ShowHeaderWhenEmpty = true;
            CffGGV_ChargesGridView.EmptyDataText = "No data to display";
            CffGGV_ChargesGridView.EmptyDataRowStyle.CssClass = "dxgvEmptyDataRow td";

            GVPlaceHolder.Controls.Clear();
            GVPlaceHolder.Controls.Add(CffGGV_ChargesGridView);
        }

        protected void ConfigureGridColumns()
        {
            CffGGV_ChargesGridView.Columns.Clear();
            CffGGV_ChargesGridView.InsertDataColumn("Type", "Type", CffGridViewColumnType.Text, "3%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_ChargesGridView.InsertDataColumn("Description", "Description", CffGridViewColumnType.Text, "38%", "cffGGV_leftAlignedCell", HorizontalAlign.Left, HorizontalAlign.Left, true);
            CffGGV_ChargesGridView.InsertCurrencyColumn("Amount", "Amount", "6%", "cffGGV_currencyCellAgedBal", false, HorizontalAlign.Right, HorizontalAlign.Right);

            CffGGV_ChargesGridView.ShowFooter = true;
            CffGGV_ChargesGridView.TotalsSummarySettings.SetColumnTotals("Amount");
            CffGGV_ChargesGridView.TotalsSummarySettings.SetTotalsColumnCssStyle("Amount", "cffGGV_currencyCell");
            CffGGV_ChargesGridView.CustomFooterSettings = CffCustomFooterMode.ShowTotals | CffCustomFooterMode.DefaultSettings;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureGridColumns();
            if (IsPostBack)
            {
                DisplayCharges(ViewState["Charges"] as IList<Charge>);
            }
        }

        public void DisplayCharges(IList<Charge> charges)
        {
            ViewState.Add("Charges", charges);
            CffGGV_ChargesGridView.DataSource = charges;
            CffGGV_ChargesGridView.DataBind();
        }

        public void LoadTab(RetentionSchedule retentionSchedule)
        {
            ViewState.Add("RetentionSchedule", retentionSchedule);

            presenter = ChargesTabPresenter.Create(this);
            presenter.LoadCharges(retentionSchedule);
        }

        public void ClearTabData()
        {
            DisplayCharges(null);
        }

        #region IPrintableView Members
            public void Print()
            { //CFF-13
                PrintableRetentionAdjustments printable =
                    new PrintableRetentionAdjustments((ViewState["Charges"] as IList<Charge>),
                                                      (ViewState["RetentionSchedule"] as RetentionSchedule),
                                                      QueryString.ViewIDValue);

                string script = PopupHelper.ShowPopup(printable, Server);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
            }
        #endregion

    }
}