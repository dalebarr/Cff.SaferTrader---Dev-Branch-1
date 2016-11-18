using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public partial class RetentionDetailsTab : UserControl, IRetentionDetailsTabView, IRetentionTab, IPrintableView
    {
        private RetentionDetailsTabPresenter presenter;

        #region IRetentionDetailsTabView Members

        public void DisplayRetentionDetails(RetentionDetails retentionDetails)
        {
            ViewState.Add("RetentionDetails", retentionDetails);

            if (retentionDetails != null)
            {
                retentionDetailsPanel.DisplayRetentionDetails(retentionDetails);
            }
        }

        #endregion

        public void LoadTab(RetentionSchedule retentionSchedule)
        {
            if (retentionSchedule == null)
                return;

            if (SessionWrapper.Instance.Get.IsClientSelected)
            {
                ViewState.Add("RetentionSchedule", retentionSchedule);

                presenter = RetentionDetailsTabPresenter.Create(this);
                presenter.LoadRetentionDetailsFor(retentionSchedule.Id);
            }
        }

        public void ClearTabData()
        {
            retentionDetailsPanel.Clear();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                DisplayRetentionDetails(ViewState["RetentionDetails"] as RetentionDetails);
            }
        }

        public void Print()
        {
            PrintableRetentionSchedule printable = new PrintableRetentionSchedule(ViewState["RetentionSchedule"] as RetentionSchedule,
                                                                                  ViewState["RetentionDetails"] as RetentionDetails,
                                                                                  QueryString.ViewIDValue);
            string script = PopupHelper.ShowPopup(printable, Server);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
        }
    }
}