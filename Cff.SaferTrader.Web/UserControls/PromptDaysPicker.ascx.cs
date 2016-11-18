using System;
using System.Web.UI;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class PromptDaysPicker : UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                for (int i = 10; i < 150; i++)
                {
                    PromptDaysDropDownList.Items.Add(i.ToString());
                }   
            }
        }

        public void SetDefaultPromptDays(int promptDays)
        {
            PromptDaysDropDownList.SelectedValue = promptDays.ToString();
        }

        public int SelectedPromptDay
        {
            get { return int.Parse(PromptDaysDropDownList.SelectedValue); }
        }
    }
}