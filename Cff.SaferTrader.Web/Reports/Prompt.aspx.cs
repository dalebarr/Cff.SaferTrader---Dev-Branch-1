using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.ReportManager;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Web.Reports
{
    public partial class Prompt : ReportBasePage, IPromptReportView
    {
        protected static string targetName = "";
        private PromptReportPresenter presenter;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            foreach (AsyncPostBackTrigger trigger in GridUpdatePanel.Triggers)
            {
                if (PromptDaysPicker.UniqueID.EndsWith(trigger.ControlID, StringComparison.OrdinalIgnoreCase))
                {
                    trigger.ControlID = PromptDaysPicker.UniqueID;
                    break;
                }

                if (IsFactoredCheckBox.UniqueID.EndsWith(trigger.ControlID, StringComparison.OrdinalIgnoreCase))
                {
                    trigger.ControlID = IsFactoredCheckBox.UniqueID;
                    break;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new PromptReportPresenter(this, RepositoryFactory.CreateReportRepository(), ReportManagerFactory.Create(this.CurrentScope(), Context.User as CffPrincipal));

            ICffClient xClient = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString :
                   (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString : null;
            if (xClient != null) {
                targetName = ": " + xClient.Name;
            }

            ICffCustomer xCustomer = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.CustomerFromQueryString :
                    (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString : null;
            if (xCustomer != null)
            {
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
            
            if (!IsPostBack)
            {
                if (xClient != null)
                {
                    presenter.LoadDefaultPromptDays(xClient.Id);
                }

                LoadReport(); //as per marty's suggestions

                //if (this.CurrentScope() != Scope.AllClientsScope) {
                //    LoadReport();
                //} else {
                //    HideReportContent();
                //}
            }
        }

        private void HideReportContent()
        {
            Master.ShowReportViewer();
            reportData.Visible = false;
            AllClientsReportHelpMessage.Visible = true;
            ReportPanel.Clear();
        }

     
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        protected override void ScopeChanged(object sender, EventArgs e)
        {
            base.ScopeChanged(sender, e);

            ReportPanel.ConfigureGridColumns();
            if (this.CurrentScope() == Scope.AllClientsScope)
            {
                HideReportContent();

            }
            else
            {
                LoadReport();
            }
        }

        protected void ExportButton_Click(object sender, ImageClickEventArgs e)
        {
            ReportPanel.Export();
        }

        public void DisplayDefaultPromptDays(int promptDays)
        {
            PromptDaysPicker.SetDefaultPromptDays(promptDays);
        }

        public void ShowReport(ReportBase report)
        {
            if (report!=null)
            this.DateViewedLiteral.Text = report.DateViewed.ToDateTimeString();
            Master.ShowReportViewer();
            ReportPanel.Display((PromptReport)report);
        }

        private void LoadReport()
        {
            ReportPanel.ResetPaginationAndFocus();

            if (new ReportScopeManager(this.CurrentScope()).IsPromptReportAvailable())
            {
                bool bIsAllClientSelected = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.IsAllClientsSelected :
                    (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsAllClientsSelected : false;

                bool bIsClientSelected = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.IsClientSelected :
                    (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsClientSelected : false;

                ICffClient xClient = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString :
                     (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString : null;

                if (bIsAllClientSelected)
                {
                    reportData.Visible = true;
                    AllClientsReportHelpMessage.Visible = false;

                    if (IsFactoredCheckBox.Checked)
                    {
                        presenter.LoadFactoredPromptReportForAllClients(0, allClientsFilter.FacilityType, allClientsFilter.IsSalvageIncluded);
                    }
                    else
                    {
                        presenter.LoadAllPromptReportForAllClients(0, allClientsFilter.FacilityType, allClientsFilter.IsSalvageIncluded);
                    }
                }
                else if (bIsClientSelected)
                {
                    allClientsFilter.Visible = false;

                    if (xClient != null) {
                        if (IsFactoredCheckBox.Checked)
                        {
                            presenter.LoadFactoredPromptReport(PromptDaysPicker.SelectedPromptDay,
                                    xClient.Id, xClient.ClientFacilityType);
                        }
                        else
                        {
                            presenter.LoadAllInvoicesPromptReport(PromptDaysPicker.SelectedPromptDay,
                                    xClient.Id, xClient.ClientFacilityType);
                        }
                    }
                }
            }
            else
            {
                DisplayReportNotAvailableError();
            }
        }
    }
}