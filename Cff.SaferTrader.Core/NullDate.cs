using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class NullDate : IDate, IComparable
    {
        public bool HasValue
        {
            get { return false; }
        }

        public Date Value
        {
            get { return null; }
        }

        public override string ToString()
        {
            return string.Empty;
        }

        public int CompareTo(object obj)
        {
            IDate date = (IDate) obj;
            if (date.HasValue)
            {
                return -1;
            }
            return 0;
        }
    }
}