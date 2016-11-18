using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public abstract class BatchRecord
    {
        private readonly string type;
        private readonly int batch;

        protected BatchRecord(string type, int batch)
        {
            this.type = type;
            this.batch = batch;
        }

        public bool IsInvoice 
        {
            get { return type.Equals(TransactionType.Invoice.Type); }
        }

        public int Batch 
        { 
            get { return batch; }
        }
    }
}