using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class LikelyRepurchasesLine
    {
        private readonly int customerid;
        private readonly int customerNumber;
        private readonly string customerName;
        private readonly string title;
        private readonly int age;
        private readonly decimal amount;
        private readonly decimal balance;
        private readonly decimal sum;

        private readonly Date dated;
        
        private readonly Date processed;
        private string transaction;
        private string reference;

        public LikelyRepurchasesLine(int custid, int custNum, string custName, string theTitle, int theAge,  
                                        decimal amt, decimal bal, decimal sum, Date thedate, Date procdate, string trans, string trxref)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(customerName, "customerName");

            this.customerid = custid;
            this.customerNumber = custNum;
            this.customerName = custName;
            this.title = theTitle;
            this.age = theAge;
            this.amount = amt;
            this.balance = bal;
            this.sum = sum;
            this.dated = thedate;
            this.processed = procdate;
            this.transaction = trans;
            this.reference = trxref;
        }


        public int CustId { get { return this.customerid;  }} 
        public int CustomerNumber {get { return this.customerNumber;  }}
        public string CustomerName { get { return this.customerName; } }
        public string Title { get { return this.title; } }
        public int Age { get { return this.age; } }
        public decimal Amount { get { return this.amount; } }
        public decimal Balance { get { return this.balance; } }
        public decimal Sum { get { return this.sum; } }

        public Date Dated { get { return this.dated; } }
        public Date Processed { get { return this.processed; } }

        public string Transaction { get { return this.transaction; } }
        public string Reference { get { return this.reference; } }

    }
}
