using System;

namespace Cff.SaferTrader.Core
{
    public class DateRange
    {
        private readonly Date startDate;
        private readonly Date endDate;

        public DateRange(Date startDate, Date endDate)
        {
            Validate(startDate, endDate);

            this.startDate = startDate;
            this.endDate = endDate.EndOfDay;
        }

        public static void Validate(Date startDateToValidate, Date endDateToValidate)
        {
            if (startDateToValidate > endDateToValidate)
            {
                throw new ArgumentException("Start date cannot be before end date");
            }
        }

        public Date StartDate
        {
            get { return startDate; }
        }

        public Date EndDate
        {
            get { return endDate; }
        }

        public int NumberOfMonths
        {
            get
            {
                int numberOfMonths = (endDate.Year - startDate.Year)*12 +
                                     (endDate.Month - startDate.Month);
                return numberOfMonths;
            }
        }
    }
}