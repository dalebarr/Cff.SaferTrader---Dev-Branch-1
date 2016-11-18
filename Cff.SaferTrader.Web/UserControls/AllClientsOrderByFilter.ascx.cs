using System;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class AllClientsOrderByFilter : System.Web.UI.UserControl
    {
        public string OrderString
        {
            get { return AllClientsOrderByFilterDropDownList.SelectedValue; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ConfigureDropDowns();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AllClientsOrderByFilterDropDownList.SelectedValue = AllClientsOrderByType.SortByClientCustomer.OrderString;
            }
        }

        private void ConfigureDropDowns()
        {
            AllClientsOrderByFilterDropDownList.DataSource = AllClientsOrderByType.KnownTypes;
            AllClientsOrderByFilterDropDownList.DataTextField = "Text";
            AllClientsOrderByFilterDropDownList.DataValueField = "OrderString";
            AllClientsOrderByFilterDropDownList.DataBind();
        }
    }
}