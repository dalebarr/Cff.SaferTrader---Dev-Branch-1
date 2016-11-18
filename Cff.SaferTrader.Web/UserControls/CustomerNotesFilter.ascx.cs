using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class CustomerNotesFilter : UserControl
    {
        public event EventHandler<ImageClickEventArgs> Click;

        private string GetMonthString(int mo)
        {
            return ((mo == 1) ? "January" : (mo == 2) ? "February" : (mo == 3) ? "March" : (mo == 4) ? "April" : (mo == 5) ? "May" : (mo == 6) ? "June" : (mo == 7) ? "July" : (mo == 8) ? "August" : (mo == 9) ? "September" : (mo == 10) ? "October" : (mo == 11) ? "November" : "December");
        }

        private DateRange GetSelectedDateRange(System.Web.UI.WebControls.TextBox tBoxFrom, System.Web.UI.WebControls.TextBox tBoxTo)
        {
            try
            {
                if (string.IsNullOrEmpty(tBoxFrom.Text) || string.IsNullOrEmpty(tBoxTo.Text))
                {
                    return new DateRange(new Date(DateTime.Parse(DateTime.Now.ToShortDateString())), new Date(DateTime.Parse(DateTime.Now.ToShortDateString())));
                }

                Date startDate = new Date(DateTime.Parse(tBoxFrom.Text)).StartOfDate;
                Date endDate = new Date(DateTime.Parse(tBoxTo.Text)).EndOfDay;
                SessionWrapper.Instance.Get.SelectedDateFromInDatePicker = tBoxFrom.Text;
                SessionWrapper.Instance.Get.SelectedDateToInDatePicker = tBoxTo.Text;

                return new DateRange(startDate, endDate);
            }
            catch
            {
                return new DateRange(new Date(DateTime.Parse(DateTime.Now.ToShortDateString())), new Date(DateTime.Parse(DateTime.Now.ToShortDateString())));
            }
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.TextBox[] textBoxes = new System.Web.UI.WebControls.TextBox[] { dtCustomerNotesFilterFrom, dtCustomerNotesFilterTo };
            Util.JQueryUtils.SetDatePickerFormat(Page, "dd MM yy", textBoxes);

            if (!IsPostBack)
            {
                PopulateActivityTypes(ActivityType.KnownTypesForFilteringNotes);
                PopulateNoteTypes(NoteType.KnownTypesForFilteringNotes);
            }
        }

        public void PopulateActivityTypes(IList<ListItem> activityTypes)
        {
            ActivityTypeDropDownList.DataSource = activityTypes;
            ActivityTypeDropDownList.DataTextField = "Text";
            ActivityTypeDropDownList.DataValueField = "Value";
            ActivityTypeDropDownList.DataBind();
        }

        public void PopulateNoteTypes(IList<ListItem> noteTypes)
        {            
            NoteTypeDropDownList.DataSource = noteTypes;
            NoteTypeDropDownList.DataTextField = "Text";
            NoteTypeDropDownList.DataValueField = "Value";
            NoteTypeDropDownList.DataBind();
        }

        public ActivityType SelectedActivityType
        {
            get { return ActivityType.Parse(int.Parse(ActivityTypeDropDownList.SelectedValue)); }
        }

        public NoteType SelectedNoteType
        {
            get { return NoteType.Parse(int.Parse(NoteTypeDropDownList.SelectedValue)); }
        }
 
        public DateRange SelectedDateRange
        {
            get { return GetSelectedDateRange(dtCustomerNotesFilterFrom, dtCustomerNotesFilterTo); }
        }

        public void UpdateButton_Click(object sender, ImageClickEventArgs e)
        {
            if (Click!=null)
                Click(sender, e);
        }
    }
}