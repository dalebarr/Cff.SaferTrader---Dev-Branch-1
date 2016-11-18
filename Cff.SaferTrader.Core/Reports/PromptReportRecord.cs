using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class PromptReportRecord : BatchRecord
    {
        private readonly int id;
        private readonly int clientNumber;
        private readonly string clientName;
        private readonly int customerId;
        private readonly int customerNumber;
        private readonly string customerName;
        private readonly IDate dated;
        private readonly string reference;
        private readonly decimal amount;
        private readonly IDate factored;
        private readonly TransactionStatus transactionStatus;
        private readonly decimal balance;
        private readonly int age;
        private readonly string trnNumber;

        public PromptReportRecord(int id, int clientNumber, string clientName, int customerId, int customerNumber, string customerName, IDate dated, string reference, decimal amount, IDate factored, int batchId, TransactionStatus transactionStatus, decimal balance, int age, string trnNumber, TransactionType transactionType)
            : base(transactionType.Type, batchId)
        {
            ArgumentChecker.ThrowIfNull(transactionStatus, "transactionStatus");
            ArgumentChecker.ThrowIfNullOrEmpty(customerName, "customerName");
            ArgumentChecker.ThrowIfNullOrEmpty(trnNumber, "trnNumber");
            ArgumentChecker.ThrowIfNullOrEmpty(clientName, "clientName");
            
            this.id = id;
            this.clientNumber = clientNumber;
            this.clientName = clientName;
            this.customerId = customerId;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.dated = dated;
            this.reference = reference;
            this.amount = amount;
            this.factored = factored;
            this.transactionStatus = transactionStatus;
            this.balance = balance;
            this.age = age;
            this.trnNumber = trnNumber;
        }

        public int CustomerId
        {
            get { return customerId; }
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

        public string TrnNumber
        {
            get { return trnNumber; }
        }

        public int Age
        {
            get { return age; }
        }

        public decimal Balance
        {
            get { return balance; }
        }

        public TransactionStatus TransactionStatus
        {
            get { return transactionStatus; }
        }

        public string Status
        {
            get { return transactionStatus.Status; }
        }

        public IDate Factored
        {
            get { return factored; }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public string Reference
        {
            get { return reference; }
        }

        public IDate Dated
        {
            get { return dated; }
        }

        public string CustomerName
        {
            get { return customerName; }
        }

        public int CustomerNumber
        {
            get { return customerNumber; }
        }

        public int Id
        {
            get { return id; }
        }

        public int ClientId {
            get { return clientNumber; }
        }
    }
}
