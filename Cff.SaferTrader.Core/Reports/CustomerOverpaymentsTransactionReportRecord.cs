using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class CustomerOverpaymentsTransactionReportRecord
    {
        private readonly int id;
        private readonly Date date;
        private readonly string invoice;
        private readonly string reference;
        private readonly decimal amount;
        private readonly Date processedDate;
        private readonly decimal balance;
        private readonly ICalendar calendar;
        private readonly string type;

        public CustomerOverpaymentsTransactionReportRecord(int id, Date date, string invoice, string reference, 
            decimal amount, Date processedDate, decimal balance, ICalendar calendar, string type)
        {
            this.id = id;
            this.date = date;
            this.invoice = invoice;
            this.reference = reference;
            this.amount = amount;
            this.processedDate = processedDate;
            this.balance = balance;
            this.calendar = calendar;
            this.type = type;
        }

        public string Type
        {
            get { return type; }
        }

        public int Id
        {
            get { return id; }
        }

        public Date Date
        {
            get { return date; }
        }

        public string Invoice
        {
            get { return invoice; }
        }

        public string Reference
        {
            get { return reference; }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public Date ProcessedDate
        {
            get { return processedDate; }
        }

        public decimal Balance
        {
            get { return balance; }
        }

        public ICalendar Calendar
        {
            get { return calendar; }
        }
    }
}
