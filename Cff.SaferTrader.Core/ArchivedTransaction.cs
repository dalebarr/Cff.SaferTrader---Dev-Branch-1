using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class ArchivedTransaction : BatchRecord
    {
        private readonly decimal amount;
        private readonly int batch;
        private readonly IDate date;
        private readonly IDate archived;
        private readonly int id;
        private readonly string number;
        private readonly string reference;
        private readonly string status;
        private readonly string type;
        private readonly IDate processed;
        private readonly string currentNotes;
        private readonly int _customerId;

        public ArchivedTransaction(int id, IDate date, IDate archived, IDate processed, string type, string number, 
            string reference, decimal amount, string status, int batch, string currNotes, int iCustID=-1) : base(type, batch)
        {
            this.id = id;
            this.processed = processed;
            this.date = date;
            this.archived = archived;
            this.type = type;
            this.number = number;
            this.reference = reference;
            this.amount = amount;
            this.status = status;
            this.batch = batch;
            this.currentNotes = currNotes;
            this._customerId = iCustID;
        }

        public IDate Archived
        {
            get { return archived; }
        }

        public IDate Processed
        {
            get { return processed; }
        }
        public int Id
        {
            get { return id; }
        }

        public IDate Date
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

        public string Status
        {
            get { return status; }
        }

        public string CurrentNotes
        {
            get { return currentNotes; }
        }

        public int CustomerId
        {
            get { return _customerId; }
        }
    }
}