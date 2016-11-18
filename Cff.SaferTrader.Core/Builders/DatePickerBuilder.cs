using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Cff.SaferTrader.Core.Builders
{
    public static class DatePickerBuilder
    {
        public static List<ListItem> BuildDateItems(Date today)
        {
            List<ListItem> datePickerItems = new List<ListItem>();

            //Add current month; days upto the current day
            datePickerItems.Add(new ListItem(today.ToMonthYearString(), today.ToShortDateString()));

            //Add the remaining/previous months; days upto the last day of each month, what are the constraints?
            for (int i = 1; i < 85; i++)
            {
                Date date = today.MonthsAgo(i);
                datePickerItems.Add(new ListItem(date.ToMonthYearString(),
                                                      date.LastDayOfTheMonth.ToShortDateString()));
            }
            return datePickerItems;
        }
    }
}
