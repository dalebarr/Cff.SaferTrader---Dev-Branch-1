using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public abstract class ReportBase
    {
        private readonly Date dateViewed;
        private readonly string name;
        private readonly string companyName;

        /// <summary>
        /// Creates a ReportBase
        /// </summary>
        /// <param name="calendar">ICalendar the report is keeps</param>
        /// <param name="name">Name of the report e.g. Invoices</param>
        /// <param name="companyName">Name of company the report is about - can be a client or a customer</param>
        protected ReportBase(ICalendar calendar, string name, string companyName)
        {
            ArgumentChecker.ThrowIfNull(calendar, "calendar");
            ArgumentChecker.ThrowIfNullOrEmpty(name, "name");
            ArgumentChecker.ThrowIfNullOrEmpty(companyName, "companyName");

            this.name = name;
            this.companyName = companyName;
            
            dateViewed = calendar.Now;
        }

        public string ExportFileName
        {
            get
            {
                return string.Format("{0}_for_{1}_{2}", 
                                    Name,
                                    companyName,
                                    DateViewed.ToYearDateHourMinuteString()).Replace(" ", "_");
            }
        }

        public Date DateViewed
        {
            get { return dateViewed; }
        }

        private string Name
        {
            get { return name; }
        }
    }
}