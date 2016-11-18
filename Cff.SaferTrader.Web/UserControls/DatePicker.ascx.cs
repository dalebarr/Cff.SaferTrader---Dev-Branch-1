using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Builders;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class DatePicker : UserControl
    {
        public event EventHandler Update;
        private bool enableAutoPostBack = true;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Date today = new Date(DateTime.Today);
                
                List<ListItem> datePickerItems = DatePickerBuilder.BuildDateItems(today);                
                foreach(ListItem listItem in datePickerItems)
                {
                    ToDropDownList.Items.Add(listItem);
                }

                if (SessionWrapper.Instance.Get != null) {
                    string selectedDate = SessionWrapper.Instance.Get.SelectedDateInDatePicker;
                    if (!string.IsNullOrEmpty(selectedDate))
                    {
                        ToDropDownList.SelectedValue = selectedDate;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ToDropDownList.AutoPostBack = enableAutoPostBack;
        }

        protected void ToDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SessionWrapper.Instance.Get.SelectedDateInDatePicker = ToDropDownList.SelectedValue;
            if (Update != null)
            {
                Update(sender, e);
            }
        }

        /// <summary>
        /// Returns last day of the selected month
        /// </summary>
        public Date Date
        {
            get { return new Date(DateTime.Parse(ToDropDownList.SelectedValue)); }
        }
        
        public bool EnableAutoPostBack
        {
            get { return enableAutoPostBack; }
            set { enableAutoPostBack = value; }
        }
    }
}