using System;

namespace Cff.SaferTrader.Core
{
     [Serializable]
    public class CffUserTypeNotFoundException : Exception
    {
        private readonly string _exceptionMessage;
        public CffUserTypeNotFoundException(string exceptionMessage)
        {
            _exceptionMessage = exceptionMessage;
        }

        public string ExceptionMessage
        {
            get { return _exceptionMessage; }
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}