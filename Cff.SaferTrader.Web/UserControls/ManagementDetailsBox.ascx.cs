using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class ManagementDetailsBox : UserControl, IManagementDetailsBoxView
    {
        private ManagementDetailsBoxPresenter presenter;
        private bool showGst;
        private bool showName = true;
        private bool showAddress = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = ManagementDetailsBoxPresenter.Create(this);
            nameSection.Visible = showName;
            addressSection.Visible = showAddress;
            gstSection.Visible = showGst;

            if (!IsPostBack)
            {
                presenter.LoadManagementDetails();
            }
        }

        public void DisplayManagementDetails(ManagementDetails managementDetails)
        {
            legalEntityOneLiteral.Text = managementDetails.LegalEntityOne;
            legalEntityTwoLiteral.Text = managementDetails.LegalEntityTwo;
            addressLiteral.Text = managementDetails.Address.ToString();

            phoneLiteral.Text = managementDetails.Phone;
            faxLiteral.Text = managementDetails.Fax;

            emailLink.Text = managementDetails.Email;
            emailLink.NavigateUrl = managementDetails.EmailLink;

            websiteLiteral.Text = managementDetails.Website;
            gstLiteral.Text = managementDetails.GstCode;
        }

        public bool ShowGst
        {
            get { return showGst; }
            set { showGst = value; }
        }

        public bool ShowName
        {
            get { return showName; }
            set { showName = value; }
        }

        public bool ShowAddress
        {
            get { return showAddress; }
            set { showAddress = value; }
        }
    }
}