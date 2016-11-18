using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class StatusPicker : UserControl
    {
        public event EventHandler Update;
        private bool enableAutoPostBack = true;
        private bool selectAllByDefault;

        protected void Page_Init(object sender, EventArgs e)
        {
            PopulateStatusPicker();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            StatusSelectorDropDownList.AutoPostBack = enableAutoPostBack;
        }

        private void PopulateStatusPicker()
        {
            if(!IsPostBack)
            {
                StatusSelectorDropDownList.Items.Add(new ListItem(TransactionStatus.All.Status, TransactionStatus.All.Id.ToString()));
                StatusSelectorDropDownList.Items.Add(new ListItem(TransactionStatus.Funded.Status, TransactionStatus.Funded.Id.ToString()));
                StatusSelectorDropDownList.Items.Add(new ListItem(TransactionStatus.NonFunded.Status, TransactionStatus.NonFunded.Id.ToString()));
                
                StatusSelectorDropDownList.SelectedValue = TransactionStatus.All.Id.ToString();
            }
        }

        public void StatusSelectorDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Update != null)
            {
                Update(sender, e);
            }
        }

        public TransactionStatus Status
        {
            get { return TransactionStatus.Parse(int.Parse(StatusSelectorDropDownList.SelectedValue)); }
        }

        public bool EnableAutoPostBack
        {
            get { return enableAutoPostBack; }
            set { enableAutoPostBack = value; }
        }

        public bool SelectAllByDefault
        {
            get { return selectAllByDefault; }
            set { selectAllByDefault = value; }
        }
    }
}