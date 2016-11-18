using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class CallsDueReportRecord
    {

        private readonly int clientNumber;
        private readonly string clientName;
        private readonly int customerId;
        private readonly int customerNumber;
        private readonly string customerName;
        private readonly string title;
        private readonly Date due;
        private readonly decimal balance;
        private readonly string age;
        private readonly string firstName;
        private readonly string lastName;
        private readonly string phone;
        private readonly string cellphone;
        private readonly string fax;

        public CallsDueReportRecord(int clientNumber, string clientName, int customerId, int customerNumber, string customerName, string title, Date due, decimal balance, string age, string firstName, string lastName, string phone, string cellphone, string fax)
        {
            this.clientNumber = clientNumber;
            this.clientName = clientName;
            this.customerId = customerId;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.title = title;
            this.due = due;
            this.balance = balance;
            this.age = age;
            this.firstName = firstName;
            this.lastName = lastName;
            this.phone = phone;
            this.cellphone = cellphone;
            this.fax = fax;
        }

        public int CustomerId
        {
            get { return customerId; }
        }

        /// <summary>
        /// Returns ClientNumber in string
        /// </summary>
        /// Fix for DM-343 where duplicate FieldName was causing Total summary label to be shown twice
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

        public int CustomerNumber
        {
            get { return customerNumber; }
        }

        public string CustomerName
        {
            get { return customerName; }
        }

        public string Title
        {
            get { return title; }
        }

        public Date Due
        {
            get { return due; }
        }

        public decimal Balance
        {
            get { return balance; }
        }

        public string Age
        {
            get { return age; }
        }

        public string FirstName
        {
            get { return firstName; }
        }

        public string LastName
        {
            get { return lastName; }
        }

        public string Phone
        {
            get { return phone; }
        }

        public string Cellphone
        {
            get { return cellphone; }
        }

        public string Fax
        {
            get { return fax; }
        }

    }
}