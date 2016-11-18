using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class ManagementContactDetails : UserControl, IManagementDetailsBoxView
    {
        private static ManagementDetailsBoxPresenter presenter;

        protected void Page_Load(object sender, EventArgs e)
        { //todo: investigate why this is being called 4x aside 4x call of page_load in logon.aspx

            if (presenter == null)
            {  //made static so we create, hit database and load management details only once
                presenter = ManagementDetailsBoxPresenter.Create(this);
                if (!IsPostBack)
                {
                    presenter.LoadManagementDetails();
                }
            }

            return;
        }

        public void DisplayManagementDetails(ManagementDetails managementDetails)
        {
            phoneLiteral.Text = managementDetails.Phone;
            faxLiteral.Text = managementDetails.Fax;

            emailLink.Text = managementDetails.Email;
            emailLink.NavigateUrl = managementDetails.EmailLink;

            websiteLiteral.Text = managementDetails.Website;
        }
    }
}