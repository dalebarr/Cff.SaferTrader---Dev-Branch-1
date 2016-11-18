using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class RepurchasesLine
    {
         private readonly int transactionId;
        private readonly int customerId;
        private readonly int customerNumber;
        private readonly string customerName;
        private readonly string transactionNumber;
        private readonly Date dated;
        private readonly decimal amount;
        private readonly decimal sum;
        private readonly int batchId;
        private readonly Date created;
        private readonly Date modified;
        private readonly string modifiedBy;

        public RepurchasesLine(int transactionId, int customerId, int customerNumber, string customerName, string transactionNumber, Date dated, decimal amount, decimal sum, int batchId, Date created, Date modified, string modifiedBy)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(customerName, "customerName");
            ArgumentChecker.ThrowIfNullOrEmpty(modifiedBy, "modifiedBy");
            
            this.transactionId = transactionId;
            this.customerId = customerId;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.transactionNumber = transactionNumber;
            this.dated = dated;
            this.amount = Math.Abs(amount);
            this.sum = sum;
            this.batchId = batchId;
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
            get { return batchId; }
        }

        public decimal Sum
        {
            get { return sum; }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public Date Dated
        {
            get { return dated; }
        }

        public string TransactionNumber
        {
            get { return transactionNumber; }
        }

        public string CustomerName
        {
            get { return customerName; }
        }

        public int CustomerNumber
        {
            get { return customerNumber; }
        }

        public int TransactionId
        {
            get { return transactionId; }
        }
    }
}