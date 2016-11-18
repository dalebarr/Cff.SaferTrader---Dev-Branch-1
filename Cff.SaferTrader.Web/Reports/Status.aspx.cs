using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters.ReportPresenters;
using Cff.SaferTrader.Core.ReportManager;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views.ReportView;

namespace Cff.SaferTrader.Web.Reports
{
    public partial class Status : ReportBasePage, IStatusView
    {
        protected static string targetName = "";
        private StatusPresenter presenter;

        #region IStatusView Members

        public int ClientId()
        {
            if (SessionWrapper.Instance.Get != null)
                return SessionWrapper.Instance.Get.ClientFromQueryString.Id;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id;

            return (QueryString.ClientId == null) ? 0 : (int)QueryString.ClientId;
        }


        public int ClientFacilityType()
        {
            if (SessionWrapper.Instance.Get != null)
                return SessionWrapper.Instance.Get.ClientFromQueryString.ClientFacilityType;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.ClientFacilityType;

            return 0;
        }

        public CffCustomer Customer()
        {
            if (SessionWrapper.Instance.Get != null)
                return (CffCustomer)SessionWrapper.Instance.Get.CustomerFromQueryString;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                return (CffCustomer)SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString;

            return  null;
        }


        public Date EndDate()
        {
            return DatePicker.Date;
        }

        public void DisplayReport(StatusReport report)
        {
            if (report != null) {
                this.DateViewedLiteral.Text = report.DateViewed.ToDateTimeString();
            }
            Master.ShowReportViewer();
            reportData.Visible = true;
            AllClientsReportHelpMessage.Visible = false;
            ReportPanel.Display(report);
            GridUpdatePanel.Update();
        }

        public void Clear()
        {
            this.DateViewedLiteral.Text = string.Empty;
            Master.ShowReportViewer();
            reportData.Visible = false;
            AllClientsReportHelpMessage.Visible = true;
            ReportPanel.Clear();
        }

        public bool IsSalvageIncluded()
        {
            return AllClientsFilter.IsSalvageIncluded;
        }

        public TransactionStatus transactionStatus()
        {
            return StatusPicker.Status;
        }

        public FacilityType FacilityType()
        {
            return AllClientsFilter.FacilityType;
        }

        public void ShowAllClientsView()
        {
            ReportPanel.ConfigureAllClientsGridColumns();
            ReportPanel.ResetPaginationAndFocus();
            AllClientsFilter.Visible = true;
        }

        public void ShowClientView()
        {
            ReportPanel.ConfigureClientGridColumns();
            ReportPanel.ResetPaginationAndFocus();

            AllClientsFilter.Visible = false;
        }

        public void ShowCustomerView()
        {
            ReportPanel.ConfigureCustomerGridColumns();
            ReportPanel.ResetPaginationAndFocus();

            AllClientsFilter.Visible = false;
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            UpdateButton.Enabled = true;

            foreach (AsyncPostBackTrigger trigger in GridUpdatePanel.Triggers)
            {
                if (DatePicker.UniqueID.EndsWith(trigger.ControlID, StringComparison.OrdinalIgnoreCase))
                {
                    trigger.ControlID = DatePicker.UniqueID;
                    break;
                }

                if (StatusPicker.UniqueID.EndsWith(trigger.ControlID, StringComparison.OrdinalIgnoreCase))
                {
                    trigger.ControlID = StatusPicker.UniqueID;
                    break;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // start related ref:CFF-18
            if (SessionWrapper.Instance.Get != null)
            {
                presenter = new StatusPresenter(this, ReportsService.Create(), ReportManagerFactory.Create(SessionWrapper.Instance.Get.Scope, Context.User as CffPrincipal));
                if (SessionWrapper.Instance.Get.ClientFromQueryString != null)
                {
                    targetName = ": " + SessionWrapper.Instance.Get.ClientFromQueryString.Name;

                    int facilityType = (SessionWrapper.Instance.Get.ClientFromQueryString.ClientFacilityType);

                    if (facilityType == 4 || facilityType == 5)
                    {
                        StatusPicker.Visible = false;
                    }
                    else
                    {
                        StatusPicker.Visible = true;
                    }

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
                if (!IsPostBack)
                {
                    presenter.ConfigureView(SessionWrapper.Instance.Get.Scope);
                    presenter.ShowReport(SessionWrapper.Instance.Get.Scope, true);
                }
            }
        }

     

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);
            presenter.ConfigureView(SessionWrapper.Instance.Get.Scope);
            presenter.ShowReport(SessionWrapper.Instance.Get.Scope, true);
        }

        protected void ExportButton_Click(object sender, ImageClickEventArgs e)
        {
            ReportPanel.Export();
        }

        protected void UpdateButton_Click(object sender, ImageClickEventArgs e)
        {
            presenter.ConfigureView(SessionWrapper.Instance.Get.Scope);
            presenter.ShowReport(SessionWrapper.Instance.Get.Scope, false);
        }

    }
}