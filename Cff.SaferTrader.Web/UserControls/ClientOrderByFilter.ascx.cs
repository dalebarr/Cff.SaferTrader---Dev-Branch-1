using System;
using System.Web.UI;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class ClientOrderByFilter : UserControl
    {
        public event EventHandler Update;
        public string OrderString
        {
            get { return ClientOrderByFilterDropDownList.SelectedValue; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {

            ConfigureDropDowns();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClientOrderByFilterDropDownList.SelectedValue = ClientOrderByType.SortByCustomer.OrderString;
            }
        }
        private void ConfigureDropDowns()
        {
            ClientOrderByFilterDropDownList.DataSource = ClientOrderByType.KnownTypes;
            ClientOrderByFilterDropDownList.DataTextField = "Text";
            ClientOrderByFilterDropDownList.DataValueField = "OrderString";
            ClientOrderByFilterDropDownList.DataBind();
        }

        protected void ClientOrderByFilterDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Update != null)
            {
                Update(sender, e);
            }
        }


    }
}