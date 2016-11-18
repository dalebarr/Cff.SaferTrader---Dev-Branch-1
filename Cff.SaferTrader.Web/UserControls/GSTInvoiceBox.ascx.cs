using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class GSTInvoiceBox : UserControl, IManagementDetailsBoxView 
    {
        private ManagementDetailsBoxPresenter presenter;
    
        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = ManagementDetailsBoxPresenter.Create(this);
            if (!IsPostBack)
            {
                presenter.LoadManagementDetails();
            }
        }

        public void DisplayManagementDetails(ManagementDetails managementDetails)
        {
           gstLiteral.Text = managementDetails.GstCode;
        }
    }
}