using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class InvoiceBatch
    {
        private readonly decimal adminFee;
        private readonly string createdBy;
        private readonly decimal credit;
        private readonly Date date;
        private readonly decimal factored;
        private readonly decimal factorFee;
        private readonly string header;
        private readonly string modifiedBy;
        private readonly Date modifiedDate;
        private readonly decimal nonFactored;
        private readonly int number;
        private readonly decimal post;
        private readonly IDate released;
        private readonly decimal repurchase;
        private readonly decimal retention;
        private readonly string status;
        private readonly decimal total;
        private readonly string clientName;
        private readonly int clientId;
        private readonly int facilityType;
        private readonly decimal nonCompliant;
        private readonly decimal retnPercent;
        
        public InvoiceBatch()
        {
            this.number = 0;
            this.clientId = 0;
            this.clientName = "";
            this.total = 0;
            this.date = new Date(DateTime.Now);
            this.factored = 0;
            this.nonFactored = 0;
            this.adminFee = 0;
            this.factorFee = 0;
            this.retention = 0;
            this.repurchase = 0;
            this.credit = 0;
            this.post = 0;
            this.released = new Date(DateTime.Now);
            this.status = "";
            this.createdBy = "";
            this.modifiedDate = new Date(DateTime.Now);
            this.modifiedBy = "";
            this.header = "";
            this.facilityType = 0;
            this.nonCompliant = 0;
            this.retnPercent = 0;
        }

        public InvoiceBatch(int number, Date date, decimal factored, decimal nonFactored, decimal adminFee,
                            decimal factorFee, decimal retention, decimal repurchase, decimal credit, decimal post,
                            IDate released, string status, string createdBy, Date modifiedDate, string modifiedBy,
                            string header, decimal total, string clientName, int clientId, int facilityType, decimal nonCompliant, decimal retnPercent)
        {
            this.number = number;
            this.clientId = clientId;
            this.clientName = clientName;
            this.total = total;
            this.date = date;
            this.factored = factored;
            this.nonFactored = nonFactored;
            this.adminFee = adminFee;
            this.factorFee = factorFee;
            this.retention = retention;
            this.repurchase = repurchase;
            this.credit = credit;
            this.post = post;
            this.released = released;
            this.status = status;
            this.createdBy = createdBy;
            this.modifiedDate = modifiedDate;
            this.modifiedBy = modifiedBy;
            this.header = header;
            this.facilityType = facilityType;
            this.nonCompliant = nonCompliant;
            this.retnPercent = retnPercent;
        }

        public int ClientId
        {
            get { return clientId; }
        }

        public string ClientName
        {
            get { return clientName; }
        }

        public decimal Total
        {
            get { return total; }
        }

        public string Header
        {
            get { return header; }
        }

        public int Number
        {
            get { return number; }
        }

        public int BatchNumber
        {
            get { return number; }
        }

        public Date Date
        {
            get { return date; }
        }

        public decimal Factored
        {
            get { return factored; }
        }

        public decimal NonFactored
        {
            get { return nonFactored; }
        }

        public decimal AdminFee
        {
            get { return adminFee; }
        }

        public decimal FactorFee
        {
            get { return factorFee; }
        }

        public decimal Retention
        {
            get { return retention; }
        }

        public decimal Repurchase
        {
            get { return repurchase; }
        }

        public decimal Credit
        {
            get { return credit; }
        }

        public decimal Post
        {
            get { return post; }
        }

        public IDate Released
        {
            get { return released; }
        }

        public string Status
        {
            get { return status; }
        }

        public string CreatedBy
        {
            get { return createdBy; }
        }

        public IDate ModifiedDate
        {
            get { return modifiedDate; }
        }

        public string ModifiedBy
        {
            get { return modifiedBy; }
        }

        public decimal NonCompliant
        {
            get { return nonCompliant; }
        }

        public decimal RetnPercent
        {
            get { return retnPercent; }
        }

        public int FacilityType
        {
            get { return facilityType; }
        }

        


    }
}