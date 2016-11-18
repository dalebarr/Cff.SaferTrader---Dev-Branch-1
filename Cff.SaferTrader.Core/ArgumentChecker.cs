using System;

namespace Cff.SaferTrader.Core
{
    public static class ArgumentChecker
    {
        public static void ThrowIfLessThanZero(int value, string name)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        public static void ThrowIfLessThanOne(int value, string name)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        public static void ThrowIfNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void ThrowIfNullOrEmpty(string value, string name)
        {
            try {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(name);
                }
            }
            catch  (Exception) {}
        }

        public static void ThrowIfGuidEmpty(Guid value, string name)
        {
            if(value == Guid.Empty)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}