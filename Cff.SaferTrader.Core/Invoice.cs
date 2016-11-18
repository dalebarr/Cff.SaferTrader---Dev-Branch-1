using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class Invoice
    {
        private readonly decimal amount;
        private readonly decimal balance;
        private readonly IDate created;
        private readonly string customerName;
        private readonly int customerNumber;
        private readonly IDate date;
        private readonly IDate factoredDate;
        private readonly int id;
        private readonly int clientId;
        private readonly int customerId;
        private readonly IDate modified;
        private readonly string modifiedBy;
        private readonly string purchaseOrder;
        private readonly string reference;
        private readonly IDate repurchased;
        private readonly string transactionNumber;
        private readonly TransactionStatus transactionStatus;
        private readonly TransactionType transactionType;

        public Invoice(int id, int clientId, int customerId, int customerNumber, string customerName, string transactionNumber, string reference, IDate factoredDate, IDate date, decimal amount, decimal balance, TransactionType transactionType, TransactionStatus transactionStatus, IDate created, IDate modified, string modifiedBy, IDate repurchased, string purchaseOrder)
        {
            this.id = id;
            this.clientId = clientId;
            this.customerId = customerId;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.transactionNumber = transactionNumber;
            this.reference = reference;
            this.factoredDate = factoredDate;
            this.date = date;
            this.amount = amount;
            this.balance = balance;
            this.transactionType = transactionType;
            this.transactionStatus = transactionStatus;
            this.created = created;
            this.modified = modified;
            this.modifiedBy = modifiedBy;
            this.repurchased = repurchased;
            this.purchaseOrder = purchaseOrder;
        }

        public int CustomerId
        {
            get { return customerId; }
        }

        public int ClientId
        {
            get { return clientId; }
        }

        public int Id
        {
            get { return id; }
        }

        public int CustomerNumber
        {
            get { return customerNumber; }
        }

        public string CustomerName
        {
            get { return customerName; }
        }

        public string TransactionNumber
        {
            get { return transactionNumber; }
        }

        public string Reference
        {
            get { return reference; }
        }

        public IDate FactoredDate
        {
            get { return factoredDate; }
        }

        public IDate Date
        {
            get { return date; }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public decimal Balance
        {
            get { return balance; }
        }

        public string TransactionType
        {
            get { return transactionType.Type; }
        }

        public string TransactionStatus
        {
            get { return transactionStatus.Status; }
        }

        public IDate Created
        {
            get { return created; }
        }

        public IDate Modified
        {
            get { return modified; }
        }

        public string ModifiedBy
        {
            get { return modifiedBy; }
        }

        public IDate Repurchased
        {
            get { return repurchased; }
        }

        public string PurchaseOrder
        {
            get { return purchaseOrder; }
        }
    }
}