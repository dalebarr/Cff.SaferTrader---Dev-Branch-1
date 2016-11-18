using System;
using System.Web.UI;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class CalendarDateRangePicker : UserControl
    {
        private int defaultMonthsRange = 2;

        public void Hide()
        {
            Visible = false;
        }

        public void Show()
        {
            Visible = true;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            RegisterClientScriptsForAjax();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (FromDateTextBox == null)
                FromDateTextBox = new System.Web.UI.WebControls.TextBox();

            if (ToDateTextBox == null)
                ToDateTextBox = new System.Web.UI.WebControls.TextBox();

            if (!IsPostBack && SessionWrapper.Instance.Get != null)
            {

              string selectedDateFrom = SessionWrapper.Instance.Get.SelectedDateFromInDatePicker;
              if (!string.IsNullOrEmpty(selectedDateFrom))
                FromDateTextBox.Text = selectedDateFrom.ToString();
              else
                SetDefaultDateRange(1);
              
              string selectedDateTo = SessionWrapper.Instance.Get.SelectedDateToInDatePicker;
              if (!string.IsNullOrEmpty(selectedDateTo))
                ToDateTextBox.Text = selectedDateTo.ToString();
              else
                SetDefaultDateRange(2);
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (SessionWrapper.Instance.Get != null))
            {
                string selectedDateFrom = SessionWrapper.Instance.Get.SelectedDateFromInDatePicker;
                if (!string.IsNullOrEmpty(selectedDateFrom))
                {
                    FromDateTextBox.Text = selectedDateFrom.ToString();
                }
                else
                {
                    SetDefaultDateRange(1);
                }
                string selectedDateTo = SessionWrapper.Instance.Get.SelectedDateToInDatePicker;
                if (!string.IsNullOrEmpty(selectedDateTo))
                {
                    ToDateTextBox.Text = selectedDateTo.ToString();
                }
                else
                {
                    SetDefaultDateRange(2);
                }
            }
            FromDateTextBox.Attributes.Add("readonly", "readonly");
            ToDateTextBox.Attributes.Add("readonly", "readonly");
        }

        private void SetDefaultDateRange(int targetType)
        {
            try
            {
                Calendar calendar = new Calendar();
                if (targetType == 1)
                {
                    Date defaultFromDate = calendar.Today.MonthsAgo(defaultMonthsRange);
                    // If range is zero (i.e. one month), from date should default to first day of the month
                    if (defaultMonthsRange == 0)
                    {
                        defaultFromDate = defaultFromDate.FirstDayOfTheMonth;
                    }

                    FromDateTextBox.Text = defaultFromDate.ToString();
                }
                else
                {
                    Date defaultToDate = calendar.Today;
                    ToDateTextBox.Text = defaultToDate.ToString();
                }
            }
            catch { }
        }

        public DateRange SelectedDateRange
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(FromDateTextBox.Text) || string.IsNullOrEmpty(ToDateTextBox.Text))
                    {
                        return null;
                    }
                    Date startDate = new Date(DateTime.Parse(FromDateTextBox.Text)).StartOfDate;
                    Date endDate = new Date(DateTime.Parse(ToDateTextBox.Text)).EndOfDay;
                    SessionWrapper.Instance.Get.SelectedDateFromInDatePicker = FromDateTextBox.Text;
                    SessionWrapper.Instance.Get.SelectedDateToInDatePicker = ToDateTextBox.Text;

                    return new DateRange(startDate, endDate);
                }
                catch {
                    return new DateRange(new Date(DateTime.Parse(DateTime.Now.ToShortDateString())), new Date(DateTime.Parse(DateTime.Now.ToShortDateString())));
                }
            }
        }


        private void RegisterClientScriptsForAjax()
        {
            const string validateDateRangeScript = @"function validateDateRange() 
                {
                    var data = ""{'startDate':'"" + $('FromDateTextBox').val() + ""', 'endDate':'"" + $('ToDateTextBox').val() + ""'}""
                    callValidationService(""ValidateDateRange"", data, success);
                }

                $(document).ready(function() {
                    validateDateRange();
                });
                  function success(message) {
                    $("".error"").text(message.d);
                    if (message.d != '') {
                        $('input.calendarRelatedButton').attr(""disabled"", ""disabled"");
                    }
                    else {
                        $('input.calendarRelatedButton').removeAttr(""disabled"");
                    }
                }";

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, typeof(UpdatePanel), "DateRange", validateDateRangeScript, true);
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

        public string TriggerButtonId { get; set; }
    }
}