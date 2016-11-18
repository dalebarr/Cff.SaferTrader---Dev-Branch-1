using System;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class Date : IDate, IComparable
    {
        private readonly DateTime dateTime;

        public Date(DateTime dateTime)
        {
            this.dateTime = dateTime;
        }

        public Date(int year, int month, int day)
        {
            dateTime = new DateTime(year, month, day);
        }

        /// <summary>
        /// Parses year month string in yyyy MM format and returns a IDate 
        /// for the start of the specified month
        /// </summary>
        public static Date Parse(string yearMonthString)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(yearMonthString, "yearMonthString");
            if (!new Regex(@"^[0-9]{4}\s[0-9]{2}$").IsMatch(yearMonthString))
            {
                throw new ArgumentException("Year Month string is in an incorrect format");
            }

            string[] yearMonthComponents = yearMonthString.Split(' ');
            int yearComponent = Convert.ToInt32(yearMonthComponents[0]);
            int monthComponent = Convert.ToInt32(yearMonthComponents[1]);

            return new Date(new DateTime(yearComponent, monthComponent, 1));
        }

        public Date MonthsAgo(int numberOfMonths)
        {
            ArgumentChecker.ThrowIfLessThanZero(numberOfMonths, "numberOfMonths");
            return new Date(dateTime.AddMonths(-numberOfMonths));
        }

        public Date MonthsAfter(int numberOfMonths)
        {
            ArgumentChecker.ThrowIfLessThanZero(numberOfMonths, "numberOfMonths");
            return new Date(dateTime.AddMonths(numberOfMonths));
        }

        public Date YearsAgo(int numberOfYears)
        {
            ArgumentChecker.ThrowIfLessThanZero(numberOfYears, "numberOfYears");
            return new Date(dateTime.AddYears(-numberOfYears));
        }

        /// <summary>
        /// Returns IDate in yyyyMM format
        /// e.g. 200901
        /// </summary>
        public int ToYearMonthValue()
        {
            string yearComponent = dateTime.Year.ToString();
            string monthComponent = dateTime.Month.ToString();
            if (monthComponent.Length < 2)
            {
                monthComponent = "0" + monthComponent;
            }
            return Convert.ToInt32(yearComponent + monthComponent);
        }

        /// <summary>
        /// Returns IDate in yyyyMMdd format
        /// e.g. 20090129
        /// </summary>
        public int ToYearMonthDayValue()
        {
            string yearComponent = dateTime.Year.ToString();
            string monthComponent = dateTime.Month.ToString();
            if (monthComponent.Length < 2)
            {
                monthComponent = "0" + monthComponent;
            }
            string dayComponent = dateTime.Day.ToString();
            if (dayComponent.Length < 2)
            {
                dayComponent = "0" + dayComponent;
            }
            return Convert.ToInt32(yearComponent + monthComponent + dayComponent);
        }

        /// <summary>
        /// Returns IDate in MMMMMMMM yyyy format 
        /// e.g. January 2009
        /// </summary>
        public string ToMonthYearString()
        {
            return dateTime.ToString("MMMMMMMM yyyy");
        }

        /// <summary>
        /// Returns IDate in dd/MM/yyyy format 
        /// e.g. 01/01/2009
        /// </summary>
        public override string ToString()
        {
            return dateTime.ToString("dd/MM/yyyy");
        }

        public string ToDateTimeString()
        {
            return dateTime.ToString();
        }

        public string ToShortDateString()
        {
            return dateTime.ToShortDateString();
        }

        public bool Equals(Date obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj.DateTime.Equals(dateTime);
        }

        public bool HasValue
        {
            get { return true; }
        }

        public static bool operator >(Date date1, Date date2)
        {
            return date1.DateTime > date2.DateTime;
        }

        public static bool operator <(Date date1, Date date2)
        {
            return date1.DateTime < date2.DateTime;
        }

        public static bool operator >=(Date date1, Date date2)
        {
            return date1.DateTime >= date2.DateTime;
        }

        public static bool operator <=(Date date1, Date date2)
        {
            return date1.DateTime <= date2.DateTime;
        }

        public static bool operator==(Date date1, Date date2)
        {
            if (date1 != null && date2 != null)
                return (date1.DateTime == ((date2 == null) ? DateTime.Now : (date2.DateTime)));
            else
                return false;
        }

        public static bool operator !=(Date date1, Date date2)
        {
            if (date1 == null || date2 == null)
                return true;

            return !(date1.DateTime == date2.dateTime);
        }

        public static TimeSpan operator -(Date date1, Date date2)
        {
            return date1.DateTime - date2.DateTime;
        }

        public override int GetHashCode()
        {
            return dateTime.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            IDate date = (IDate)obj;
            if (date.HasValue)
            {
                return dateTime.CompareTo(date.Value.DateTime);
            }
            return 1;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Date)) return false;
            return Equals(((Date) obj));
        }

        // Reads nicely but is only used in tests :(
        public bool IsWithin(DateRange dateRange)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            return this >= dateRange.StartDate && this <= dateRange.EndDate;
        }

        public Date FirstDayOfNextMonth
        {
            get { return MonthsAfter(1).FirstDayOfTheMonth; }
        }

        public Date FirstDayOfTheMonth
        {
            get { return new Date(new DateTime(dateTime.Year, dateTime.Month, 1)); }
        }

        public Date LastDayOfTheMonth
        {
            get
            {
                int year = dateTime.Year;
                int month = dateTime.Month;
                return new Date(new DateTime(year, month, DateTime.DaysInMonth(year, month)));
            }
        }

        public Date EndOfDay
        {
            get
            {               
                int year = dateTime.Year;
                int month = dateTime.Month;
                int day = dateTime.Day;

                return new Date(new DateTime(year, month, day, 23, 59, 59));
            }
        }

        public Date Value
        {
            get { return this; }
        }

        public int Year
        {
            get { return dateTime.Year; }
        }

        public int Month
        {
            get { return dateTime.Month; }
        }

        public int Day
        {
            get { return dateTime.Day; }
        }

        public Date Today
        {
            get { return new Date(Year, Month, Day); }
        }

        public DateTime DateTime
        {
            get { return dateTime; }
        }

        /// <summary>
        /// Equivalent to SqlDateTime.MinValue
        /// </summary>
        public static Date MinimumDate
        {
            get { return new Date(SqlDateTime.MinValue.Value);}
        }

        public Date StartOfDate
        {
            get
            {
                int year = dateTime.Year;
                int month = dateTime.Month;
                int day = dateTime.Day;

                return new Date(new DateTime(year, month, day, 00, 00, 00));
            }

        }

        public string ToYearDateHourMinuteString()
        {
            return dateTime.ToString("yyyyMMddhhmm");
        }

        public int IsThisMonth(ICalendar calendar)
        {
            int isThisMonth = -1;
            if (IsWithin(new DateRange(calendar.Today.FirstDayOfTheMonth, calendar.Today.LastDayOfTheMonth.EndOfDay)))
            {
                isThisMonth = 0;
            }
            return isThisMonth;
        }
    }
}