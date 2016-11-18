using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class CreditLimitExceededReportRecord
    {
        private readonly int id;
        private readonly int clientNumber;
        private readonly string clientName;
        private readonly int customerNumber;
        private readonly string customerName;
        private readonly decimal? currentBalance;
        private readonly decimal? monthOldBalance;
        private readonly decimal? twoMonthsOldBalance;
        private readonly decimal? threeMonthsOrOlderBalance;
        private readonly decimal balance;
        private readonly decimal limit;
        private readonly IDate nextCallDate;
        private readonly string contact;
        private readonly string phone;
        private readonly string cell;
        private readonly string email;

        public CreditLimitExceededReportRecord(int id, int clientNumber, string clientName, int customerNumber, string customerName, decimal? currentBalance, decimal? monthOldBalance, decimal? twoMonthsOldBalance, decimal? threeMonthsOrOlderBalance, decimal balance, decimal limit, IDate nextCallDate, string contact, string phone, string cell, string email)
        {
            this.id = id;
            this.clientNumber = clientNumber;
            this.clientName = clientName;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.currentBalance = currentBalance;
            this.monthOldBalance = monthOldBalance;
            this.twoMonthsOldBalance = twoMonthsOldBalance;
            this.threeMonthsOrOlderBalance = threeMonthsOrOlderBalance;
            this.balance = balance;
            this.limit = limit;
            this.nextCallDate = nextCallDate;
            this.contact = contact;
            this.phone = phone;
            this.cell = cell;
            this.email = email;
        }

        public decimal Limit
        {
            get { return limit; }
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

        public decimal Balance
        {
            get { return balance; }
        }

        public int CustomerId
        {
            get { return id; }
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

        public decimal? CurrentBalance
        {
            get { return currentBalance; }
        }

        public decimal? MonthOldBalance
        {
            get { return monthOldBalance; }
        }

        public decimal? TwoMonthsOldBalance
        {
            get { return twoMonthsOldBalance; }
        }

        public decimal? ThreeMonthsOrOlderBalance
        {
            get { return threeMonthsOrOlderBalance; }
        }

        public IDate NextCallDate
        {
            get { return nextCallDate; }
        }

        public string Contact
        {
            get { return contact; }
        }

        public string Phone
        {
            get { return phone; }
        }

        public string Cell
        {
            get { return cell; }
        }

        public string Email
        {
            get { return email; }
        }
    }
}
