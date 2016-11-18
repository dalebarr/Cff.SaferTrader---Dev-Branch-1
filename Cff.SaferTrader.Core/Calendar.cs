using System;

namespace Cff.SaferTrader.Core
{
    /// <summary>
    /// The calendar the system goes by
    /// </summary>
    [Serializable]
    public class Calendar : ICalendar
    {
        #region ICalendar Members

        public Date Now
        {
            get { return new Date(DateTime.Now); }
        }

        public Date Today
        {
            get { return new Date(DateTime.Today); }
        }

        public Date OldNotesImported
        {
            get { return new Date(2009, 5, 1); }
        }

        #endregion
    }
}