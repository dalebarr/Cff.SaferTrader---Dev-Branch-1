using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class ClaimedRetentionRepurchase
    {
        private decimal amount;
        private readonly int batch;
        private readonly Date created;
        private readonly int customerId;
        private readonly string customerName;
        private readonly int customerNumber;
        private readonly Date dated;
        private readonly Date modified;
        private readonly string modifiedBy;
        private readonly decimal sum;
        private readonly string transaction;

        public ClaimedRetentionRepurchase(int customerId, int customerNumber, string customerName, string transaction,
                                          Date dated, decimal amount, decimal sum, int batch, Date created,
                                          Date modified, string modifiedBy)
        {
            this.customerId = customerId;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.transaction = transaction;
            this.dated = dated;
            this.amount = amount;
            this.sum = sum;
            this.batch = batch;
            this.created = created;
            this.modified = modified;
            this.modifiedBy = modifiedBy;
        }

        public int CustomerId
        {
            get { return customerId; }
        }

        public string ModifiedBy
        {
            get { return modifiedBy; }
        }

        public Date Modified
        {
            get { return modified; }
        }

        public Date Created
        {
            get { return created; }
        }

        public int Batch
        {
            get { return batch; }
        }

        public decimal Sum
        {
            get { return sum; }
        }

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public Date Dated
        {
            get { return dated; }
        }

        public string Transaction
        {
            get { return transaction; }
        }

        public string CustomerName
        {
            get { return customerName; }
        }

        public int CustomerNumber
        {
            get { return customerNumber; }
        }
    }
}