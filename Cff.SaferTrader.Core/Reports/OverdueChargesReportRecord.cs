using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class OverdueChargesReportRecord
    {
        private readonly int clientNumber;
        private readonly string clientName;
        private readonly int customerId;
        private readonly int customerNumber;
        private readonly string customerName;
        private readonly string title;
        private readonly IDate factored;
        private readonly int age;
        private readonly string number;
        private readonly string reference;
        private readonly decimal charges;
        private readonly decimal chargesWithGst;
        private readonly decimal amount;
        private readonly decimal balance;
        //private readonly TransactionStatus status;
        private readonly string status;

        public OverdueChargesReportRecord(int clientNumber, string clientName, int customerId, int customerNumber, string customerName, string title, IDate factored, int age, string number, string reference, decimal charges, decimal chargesWithGst, decimal amount, decimal balance, string status)
        //public OverdueChargesReportRecord(int clientNumber, string clientName, int customerId, int customerNumber, string customerName, string title, IDate factored, int age, string number, string reference, decimal charges, decimal chargesWithGst, decimal amount, decimal balance, TransactionStatus status)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(clientName, "clientName");
            ArgumentChecker.ThrowIfNullOrEmpty(customerName, "customerName");
            ArgumentChecker.ThrowIfNull(status, "status");

            this.clientNumber = clientNumber;
            this.clientName = clientName;
            this.customerId = customerId;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.title = title;
            this.factored = factored;
            this.age = age;
            this.number = number;
            this.reference = reference;
            this.charges = charges;
            this.chargesWithGst = chargesWithGst;
            this.amount = amount;
            this.balance = balance;
            this.status = status;
        }

        public int CustomerID
        {
            get { return customerId; }
        }

        /// <summary>
        /// Charges plus GST
        /// N.B. Admin charges are subject to GST whereas Finance charges are not
        /// </summary>
        public decimal ChargesWithGst
        {
            get { return chargesWithGst; }
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

        public string ClientName
        {
            get { return clientName; }
        }

        
        //public string StatusAsString
        //{
        //    get { return status.Status; }
        //}

        public string Status
        {
            get { return status; }
        }

        
        //public TransactionStatus Status
        //{
        //    get { return status; }
        //}

        public decimal Balance
        {
            get { return balance; }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public decimal Charges
        {
            get { return charges; }
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

        public string Title
        {
            get { return title; }
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
