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
    public partial class Control : ReportBasePage, IControlView
    {
        protected static string targetName = "";
        private ControlReportPresenter presenter;

        #region IControlView Members

        public Date EndDate()
        {
            return DatePicker.Date;
        }

        public int ClientId()
        {
            return SessionWrapper.Instance.Get.ClientFromQueryString.Id;
        }

        public void DisplayReport(ControlReport report)
        {
            Master.ShowReportViewer();
            reportData.Visible = true;
            DateViewedLiteral.Text = (report==null)?"":report.DateViewed.ToDateTimeString();
            AllClientsReportHelpMessage.Visible = false;
            ReportPanel.Display(report);
        }

        public void Clear()
        {
            Master.ShowReportViewer();
            reportData.Visible = false;
            AllClientsReportHelpMessage.Visible = true;
            ReportPanel.Clear();
        }

        public bool IsSalvageIncluded()
        {
            return allClientsFilter.IsSalvageIncluded;
        }

        public FacilityType FacilityType()
        {
            return allClientsFilter.FacilityType;
        }

        public void ShowAllClientsView()
        {
            DatePicker.Visible = true;
            allClientsFilter.Visible = true;
        }

        public void ShowClientView()
        {
            allClientsFilter.Visible = false;
            DatePicker.Visible = true;
        }
        

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            allClientsFilter.Update += ControlReportAllClientsFilterUpdate;
            if (SessionWrapper.Instance.Get.Scope != Scope.AllClientsScope)
                    DatePicker.Update += DatePickerUpdate;
            
            foreach (AsyncPostBackTrigger trigger in GridUpdatePanel.Triggers) {
                if (DatePicker.UniqueID.EndsWith(trigger.ControlID, StringComparison.OrdinalIgnoreCase))
                {
                    trigger.ControlID = DatePicker.UniqueID;
                    break;
                }
            }
            presenter = new ControlReportPresenter(this, ReportsService.Create(),
                                                   ReportManagerFactory.Create(this.CurrentScope(), Context.User as CffPrincipal));
            presenter.ConfigureView(SessionWrapper.Instance.Get.Scope);
        }

        protected void DatePickerUpdate(object sender, EventArgs e)
        {
            DatePicker.Update += DatePickerUpdate;
            DatePicker.EnableAutoPostBack = true;
            presenter.ShowReport(this.CurrentScope(), false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // start related ref:CFF-18

            ICffClient xClient = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString :
                                (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString : null;
            if (xClient != null) {
                targetName = ": " + xClient.Name;
            }

            ICffCustomer xCustomer = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.CustomerFromQueryString :
                                       (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString : null;
            if (xCustomer != null) {
                if (targetName != null || !targetName.Equals(""))
                {
                    targetName += " / ";
                    targetName = string.Concat(targetName, xCustomer.Name);
                }
                else
                {
                    targetName = ": " + xCustomer.Name;
                }
            }
            // end
            
            if (!IsPostBack)
                presenter.ShowReport(this.CurrentScope(), true);
        }

        protected void ExportButton_Click(object sender, ImageClickEventArgs e)
        {
            ReportPanel.Export();
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            presenter.ConfigureView(this.CurrentScope());
            presenter.ShowReport(this.CurrentScope(), true);
        }

        private void ControlReportAllClientsFilterUpdate(object sender, EventArgs e)
        {
            allClientsFilter.Update += ControlReportAllClientsFilterUpdate;
            presenter.ShowReport(this.CurrentScope(), true);
        }
    }
}