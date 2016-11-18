using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class StatusReportRecord : BatchRecord
    {
        private readonly int clientNumber;
        private readonly string clientName;
        private readonly int customerId;
        private readonly int customerNumber;
        private readonly string customerName;
        private readonly string customerTitle;
        private readonly IDate dated;
        private readonly IDate factored;
        private readonly int age;
        private readonly string number;
        private readonly string reference;
        private readonly decimal openingBalance;
        private readonly decimal amount;
        private readonly decimal receipts;
        private readonly decimal credits;
        private readonly decimal discounts;
        private readonly decimal journals;
        private readonly decimal repurchases;
        private readonly decimal other;
        private readonly IDate lastTransaction;
        private readonly decimal balance;
        private readonly decimal retention;
        private readonly decimal charges;
        private readonly TransactionStatus status;
        private readonly IDate repurchaseDate;
        private readonly string type;

        public StatusReportRecord(int clientNumber, string clientName, int customerId, int customerNumber, string customerName, string customerTitle, IDate dated, IDate factored, int age, int batch, string number, string reference, decimal openingBalance, decimal amount, decimal receipts, decimal credits, decimal discounts, decimal journals, decimal repurchases, decimal other, IDate lastTransaction, decimal balance, decimal retention, decimal charges, TransactionStatus status, IDate repurchaseDate, string type)
            :base(type, batch)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(customerName, "customerName");
            if (customerNumber < 0){throw new ArgumentOutOfRangeException("customerNumber", "customerNumber less than zero");}
            if (batch < 0) { throw new ArgumentOutOfRangeException("batch", "Batch cannot be less than zero"); }

            this.clientNumber = clientNumber;
            this.clientName = clientName;
            this.customerId = customerId;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.customerTitle = customerTitle;
            this.dated = dated;
            this.factored = factored;
            this.age = age;
            this.number = number;
            this.reference = reference;
            this.openingBalance = openingBalance;
            this.amount = amount;
            this.receipts = receipts;
            this.credits = credits;
            this.discounts = discounts;
            this.journals = journals;
            this.repurchases = repurchases;
            this.other = other;
            this.lastTransaction = lastTransaction;
            this.balance = balance;
            this.retention = retention;
            this.charges = charges;
            this.status = status;
            this.repurchaseDate = repurchaseDate;
            this.type = type;
        }

        public int CustomerId
        {
            get { return customerId; }
        }

        public string ClientName
        {
            get { return clientName; }
        }

        /// <summary>
        /// Returns ClientNumber in string
        /// </summary>
        public string ClientNumberLabel
        {
            get { return clientNumber.ToString(); }
        }

        public int ClientNumber
        {
            get { return clientNumber; }
        }

        public int ClientId
        {
            get { return clientNumber; }
        }

        public decimal OpeningBalance
        {
            get { return openingBalance; }
        }

        public string Type
        {
            get { return type; }
        }

        public IDate RepurchaseDate
        {
            get { return repurchaseDate; }
        }

        public TransactionStatus TransactionStatus
        {
            get{ return status;}
        }

        public string Status
        {
            get { return status.Status; }
        }

        public decimal Charges
        {
            get { return charges; }
        }

        public decimal Retention
        {
            get { return retention; }
        }

        public decimal Balance
        {
            get { return balance; }
        }

        public IDate LastTransaction
        {
            get { return lastTransaction; }
        }

        public decimal Other
        {
            get { return other; }
        }

        public decimal Journals
        {
            get { return journals; }
        }

        public decimal Repurchases
        {
            get { return repurchases; }
        }

        public decimal Discounts
        {
            get { return discounts; }
        }

        public decimal Credits
        {
            get { return credits; }
        }

        public decimal Receipts
        {
            get { return receipts; }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public string Reference
        {
            get { return reference; }
        }

        public string Number
        {
            get { return number; }
        }

        public int Age
        {
            get { return age; }
        }

        public IDate Factored
        {
            get { return factored; }
        }

        public IDate Dated
        {
            get { return dated; }
        }

        public string CustomerTitle
        {
            get { return customerTitle; }
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
