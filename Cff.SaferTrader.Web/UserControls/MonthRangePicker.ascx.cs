using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class MonthRangePicker : UserControl
    {
        public event EventHandler Update;
       
        private bool updateButtonVisible = true;
        private int defaultMonthsRange = 2;//12;

        protected void Page_Init(object sender, EventArgs e)
        {
            UpdateButtonColumn.Visible = updateButtonVisible;

            if (!IsPostBack)
            {
                var today = new Date(DateTime.Today);
                for (int i = 0; i < 85; i++)
                {
                    Date date = today.MonthsAgo(i);
                    FromDropDownList.Items.Add(new ListItem(date.ToMonthYearString(), date.FirstDayOfTheMonth.ToShortDateString()));
                    ToDropDownList.Items.Add(new ListItem(date.ToMonthYearString(), date.LastDayOfTheMonth.ToShortDateString()));
                }

                FromDropDownList.SelectedValue = today.MonthsAgo(DefaultMonthsRange).FirstDayOfTheMonth.ToShortDateString();
                ToDropDownList.SelectedValue = today.LastDayOfTheMonth.ToShortDateString();

                FromDropDownList.Attributes.Add("onchange", "validateDateRange();");
                ToDropDownList.Attributes.Add("onchange", "validateDateRange();");
            }
        }

        protected void UpdateButton_Click(object sender, ImageClickEventArgs e)
        {
            if (Update != null)
            {
                Update(sender, EventArgs.Empty);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            RegisterClientScriptsForAjax();
        }

        private void RegisterClientScriptsForAjax()
        {
            const string validateDateRangeScript = @"function validateDateRange() {
                     var data = ""{'startDate':'"" + $("".fromDropDownList"").val() + ""', 'endDate':'"" + $("".toDropDownList"").val() + ""'}""
                        callValidationService(""ValidateDateRange"", data, success);
                    }
                    function success(message) {
                        $("".error"").text(message.d);
                        if (message.d != '') {
                            $("".updateButton"").attr(""disabled"", ""disabled"");
                        }
                        else {
                            $("".updateButton"").removeAttr(""disabled"");
                        }
                        setTimeout(""stopAnimate()"", 300);
                    }
                    $(document).ready(function() {
                        validateDateRange();
                    });";

            ScriptManager.RegisterClientScriptBlock(this, typeof(UpdatePanel), "DateRange", validateDateRangeScript, true);
        }

        public DateRange DateRange
        {
            get
            {
                Date startDate = new Date(DateTime.Parse(FromDropDownList.SelectedValue));
                Date endDate = new Date(DateTime.Parse(ToDropDownList.SelectedValue));
                return new DateRange(startDate, endDate);
            }
        }

        public int DefaultMonthsRange
        {
            get { return defaultMonthsRange; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Default Months range should be a positive number");
                }
                defaultMonthsRange = value;
            }
        }

        /// <summary>
        /// Shows or hides Update button - shown by default
        /// </summary>
        public bool UpdateButtonVisible
        {
            get { return updateButtonVisible; }
            set { updateButtonVisible = value; }
        }

        public FacilityType FacilityType
        {
            get { return allClientsFilter.FacilityType; }
        }

        public bool IsSalvageIncluded
        {
            get { return allClientsFilter.IsSalvageIncluded; }
        }

        public void ShowAllClientsControls()
        {
            allClientsFilter.Visible = true;
        }

        public void HideAllClientsControls()
        {
            allClientsFilter.Visible = false;
        }
    }
}