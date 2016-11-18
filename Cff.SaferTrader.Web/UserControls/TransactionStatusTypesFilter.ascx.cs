using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class TransactionStatusTypesFilter : System.Web.UI.UserControl
    {
        public event EventHandler Update;
        private bool enableAutoPostBack = true;
        private readonly static IList<TransactionStatus> tListItems =  TransactionStatus.TransactionFilterStatusAsListItem;

        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TransactionStatusTypeDropDownList.AutoPostBack = enableAutoPostBack;
        }

        public void PopulateTransactionStatusTypeDropdown()
        {
            try
            {
                TransactionStatusTypeDropDownList.Items.Clear();
                TransactionStatusTypeDropDownList.DataSource = tListItems;
                TransactionStatusTypeDropDownList.SelectedIndex = 0;
                TransactionStatusTypeDropDownList.DataTextField = "Status";
                TransactionStatusTypeDropDownList.DataValueField = "Id";
                TransactionStatusTypeDropDownList.DataBind();
            }
            catch (Exception  exc) 
            {
                throw new System.Exception(exc.Message);
            }
        }

        protected void TransactionStatusTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SessionWrapper.Instance.Get.SelectedTransactionFilter = TransactionStatusTypeDropDownList.SelectedValue;
            if (Update != null)
            {
                Update(sender, e);
            }
        }

        public bool EnableAutoPostBack
        {
            get { return enableAutoPostBack; }
            set { enableAutoPostBack = value; }
        }

        public TransactionStatus SelectedStatusFilter
        {
             get { return TransactionStatus.Parse(int.Parse(TransactionStatusTypeDropDownList.SelectedValue)); }
        }
    }
}