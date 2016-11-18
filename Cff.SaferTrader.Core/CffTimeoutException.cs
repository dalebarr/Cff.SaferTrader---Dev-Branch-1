using System;
using System.Runtime.Serialization;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class CffTimeoutException : ApplicationException
    {
        public CffTimeoutException(string message) : base(message)
        {
        }

        public CffTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CffTimeoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}