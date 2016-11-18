using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class ClaimedCredit
    {
        private readonly int _customerId;
        private readonly int customerNumber;
        private readonly string customerName;
        private readonly string transaction;
        private readonly Date dated;
        private readonly decimal amount;
        private readonly decimal sum;
        private readonly int batch;
        private readonly Date created;
        private readonly Date modified;
        private readonly string modifiedBy;

        public ClaimedCredit(int customerId, int customerNumber, string customerName, string transaction, 
            Date dated, decimal amount, decimal sum, int batch, Date created, Date modified, string modifiedBy)
        {
            this._customerId = customerId;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.transaction = transaction;
            this.dated = dated;
            this.amount = amount;
            this.sum = sum;
            this.batch = batch;
            this.created = created;
            this.modified = modified;
            this.modifiedBy = modifiedBy;
        }

        public int CustomerID
        {
            get { return _customerId; }
        }

       // public int CustomerId
       // {
       //     get { return _customerId; }
       // }

        public string ModifiedBy
        {
            get { return modifiedBy; }
        }

        public Date Modified
        {
            get { return modified; }
        }

        public Date Created
        {
            get { return created; }
        }

        public int Batch
        {
            get { return batch; }
        }

        public decimal Sum
        {
            get { return sum; }
        }

        /// <summary>
        /// Absolute value of claimed credit amount
        /// </summary>
        public decimal Amount
        {
            get { return Math.Abs(amount); }
        }

        public Date Dated
        {
            get { return dated; }
        }

        public string Transaction
        {
            get { return transaction; }
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
