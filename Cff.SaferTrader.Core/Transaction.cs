using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class Transaction : BatchRecord
    {
        private readonly decimal amount;
        private readonly decimal balance;
        private readonly Date date;
        private readonly int id;
        private readonly string number;
        private readonly Date processed;
        private readonly string reference;
        private readonly string status;
        private readonly string type;
        private readonly string currentnotes;


        public Transaction(int id, Date date, Date processed, string type, string number, string reference,
                        decimal amount, decimal balance, string status, int batch)
            : base(type, batch)
        {
            this.id = id;
            this.processed = processed;
            this.date = date;
            this.type = type;
            this.number = number;
            this.reference = reference;
            this.amount = amount;
            this.balance = balance;
            this.status = status;
            this.currentnotes = ""; //"**missing from storedproc**" //CFFWEB-8 
        }

        public Transaction(int id, Date date, Date processed, string type, string number, string reference,
                           decimal amount, decimal balance, string status, int batch, string currnotes) : base(type, batch)
        {
            this.id = id;
            this.processed = processed;
            this.date = date;
            this.type = type;
            this.number = number;
            this.reference = reference;
            this.amount = amount;
            this.balance = balance;
            this.status = status;
            this.currentnotes = currnotes;
        }

        public Transaction(int id, Date date, string type, string number, string reference, decimal amount,
                           decimal balance, string status, int batch) : base(type, batch)
        {
            this.id = id;
            this.date = date;
            this.type = type;
            this.number = number;
            this.reference = reference;
            this.amount = amount;
            this.balance = balance;
            this.status = status;
        }

        public int Id
        {
            get { return id; }
        }

        public Date Date
        {
            get { return date; }
        }

        public string Type
        {
            get { return type; }
        }

        public string Number
        {
            get { return number; }
        }

        public string Reference
        {
            get { return reference; }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public decimal Balance
        {
            get { return balance; }
        }

        public string Status
        {
            get { return status; }
        }

        public Date Processed
        {
            get { return processed; }
        }

        public String CurrentNotes
        {
            get { return currentnotes; }
        }
    }
}