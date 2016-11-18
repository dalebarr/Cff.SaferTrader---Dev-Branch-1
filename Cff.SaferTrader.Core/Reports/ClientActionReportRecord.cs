using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class ClientActionReportRecord
    {
        private readonly int clientNumber;
        private readonly string clientName;
        private readonly int customerNumber;
        private readonly string customerName;
        private readonly string title;
        private readonly decimal balance;
        private readonly string details;
        private readonly string age;
        private readonly Date due;
        private readonly int customerId;

        public ClientActionReportRecord(int clientNumber, string clientName, int customerId, int customerNumber, string customerName, string title, decimal balance, string details, string age, Date due)
        {
            this.clientNumber = clientNumber;
            this.clientName = clientName;
            this.customerId = customerId;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.title = title;
            this.balance = balance;
            this.details = details;
            this.age = age;
            this.due = due;
        }

        public Date Due
        {
            get { return due; }
        }

        public string Age
        {
            get { return age; }
        }

        public string Details
        {
            get { return details; }
        }

        public decimal Balance
        {
            get { return balance; }
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

        public int CustomerId
        {
            get { return customerId; }
        }
    }
}