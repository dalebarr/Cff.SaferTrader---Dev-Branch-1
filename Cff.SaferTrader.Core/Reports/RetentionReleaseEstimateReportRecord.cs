using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class RetentionReleaseEstimateReportRecord
    {
        private readonly int id;
        private readonly int customerId;
        private readonly int customerNumber;
        private readonly string customerName;
        private readonly IDate date;
        private readonly string number;
        private readonly string reference;
        private readonly decimal amount;
        private readonly decimal balance;
        private readonly decimal currentTransaction;
        private readonly decimal openingBalance;
        private readonly decimal retention;
        private readonly TransactionStatus status;

        public RetentionReleaseEstimateReportRecord(int id, int customerId, int customerNumber, string customerName, IDate date, string number, string reference, decimal amount, decimal openingBalance, decimal balance, decimal retention, TransactionStatus status)
        {
            this.id = id;
            this.customerId = customerId;
            this.retention = retention;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.date = date;
            this.number = number;
            this.reference = reference;
            this.amount = amount;
            this.balance = balance;
            this.openingBalance = openingBalance;
            this.status = status;

            currentTransaction = openingBalance - balance;
        }

        public int CustomerId
        {
            get { return customerId; }
        }

        public string Status
        {
            get { return status.Status; }
        }

        public decimal CurrentTransaction
        {
            get { return currentTransaction; }
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

        public IDate Date
        {
            get { return date; }
        }

        public string Number
        {
            get { return number; }
        }

        public string Reference
        {
            get { return reference; }
        }

        public decimal Balance
        {
            get { return balance; }
        }

        public decimal OpeningBalance
        {
            get { return openingBalance; }
        }

        public decimal Amount
        {
            get { return amount; }
        }
        
        public decimal Retention
        {
            get { return retention; }
        }

        public TransactionStatus TransactionStatus
        {
            get { return status; }
        }
    }
}