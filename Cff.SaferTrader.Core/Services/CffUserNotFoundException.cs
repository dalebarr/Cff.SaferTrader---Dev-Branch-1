using System;

namespace Cff.SaferTrader.Core.Services
{
    [Serializable]
    public class CffUserNotFoundException : Exception
    {
        private readonly string message;
     
        public CffUserNotFoundException(string message)
        {
            this.message = message;
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}