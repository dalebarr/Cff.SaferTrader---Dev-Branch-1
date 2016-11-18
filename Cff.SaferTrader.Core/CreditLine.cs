using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class CreditLine : BatchRecord
    {
        private readonly int transactionId;
        private readonly int customerId;
        private readonly int customerNumber;
        private readonly string customerName;
        private readonly string transactionNumber;
        private readonly IDate dated;
        private readonly decimal amount;
        private readonly decimal sum;
        private readonly IDate created;
        private readonly IDate modified;
        private readonly string modifiedBy;

        public CreditLine(int transactionId, int customerId, int customerNumber, string customerName, string transactionNumber, IDate dated, decimal amount, decimal sum, int batch, IDate created, IDate modified, string modifiedBy)
            : base (TransactionType.Credit.Type, batch)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(customerName, "customerName");
            ArgumentChecker.ThrowIfNullOrEmpty(transactionNumber, "transactionNumber");
            ArgumentChecker.ThrowIfNullOrEmpty(modifiedBy, "modifiedBy");

            this.transactionId = transactionId;
            this.customerId = customerId;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.transactionNumber = transactionNumber;
            this.dated = dated;
            this.amount = amount;
            this.sum = sum;
            this.created = created;
            this.modified = modified;
            this.modifiedBy = modifiedBy;
        }

        public string ModifiedBy
        {
            get { return modifiedBy; }
        }

        public IDate Modified
        {
            get { return modified; }
        }

        public IDate Created
        {
            get { return created; }
        }
        
        public decimal Sum
        {
            get { return sum; }
        }

        public decimal Amount
        {
            get { return Math.Abs(amount); }
        }

        public IDate Dated
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

        public int CustomerId
        {
            get { return customerId; }
        }
    }
}