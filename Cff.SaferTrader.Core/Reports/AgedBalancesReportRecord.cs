using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class AgedBalancesReportRecord
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
        private readonly IDate nextCallDate;
        private readonly string contact;
        private readonly string phone;
        private readonly string cell;
        private readonly string email;
        private string note;
        private IList<CustomerNote> custNoteList;

        public AgedBalancesReportRecord(int id, int clientNumber, string clientName, int customerNumber, string customerName, 
                            decimal? currentBalance, decimal? monthOldBalance, decimal? twoMonthsOldBalance, decimal? threeMonthsOrOlderBalance, 
                                    decimal balance, IDate nextCallDate, string contact, string phone, string cell, string email)
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
            this.nextCallDate = nextCallDate;
            this.contact = contact;
            this.phone = phone;
            this.cell = cell;
            this.email = email;
            this.custNoteList = new List<CustomerNote>();
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

        public int ClientId
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

        public int Id
        {
            get { return id; }
        }

        public int CustomerId
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

        public string Note
        {
            get {
                string strNote = "";
                if (this.custNoteList.Count > 0)
                {
                    foreach (CustomerNote cNte in custNoteList)
                    {
                        if (cNte.Comment.Length > 0)
                        {
                            strNote += "[" + cNte.Created.ToShortDateString() + "] ";
                            strNote += cNte.Comment + System.Environment.NewLine;
                        } 
                    }

                    return strNote;
                }

                return note; 
            }
            set { this.note = value;  }
        }

        public IList<CustomerNote> CustNoteList
        {
            get { return custNoteList; }
            set { this.custNoteList = value; }
        }
    }
}