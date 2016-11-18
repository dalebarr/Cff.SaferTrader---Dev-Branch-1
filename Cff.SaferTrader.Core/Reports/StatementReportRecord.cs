using System;

namespace Cff.SaferTrader.Core.Reports
{
    [Serializable]
    public class StatementReportRecord
    {
        private readonly IDate dated;
        private readonly string description;
        private readonly string number;
        private readonly string reference;
        private readonly int age;
        private readonly decimal debits;
        private readonly decimal credits;
        private readonly decimal amount;
        private readonly int transTypeID;       //MSarza [20151007]

        public StatementReportRecord(Date dated, 
                                    string description,
                                    string number,
                                    string reference,
                                    decimal amount,
                                    int age,
                                    int transTypeID  //MSarza [20151007]
                                       )
        {
            this.dated = dated;
            this.description = description;
            this.number = number;
            this.reference = reference;
            this.age = age;
            this.amount = amount;

            if (amount > 0)
            {
                debits = amount;
            }
            else
            {
                credits = -1 * amount;
            }
        }

        public int Age
        {
            get { return age; }
        }

        public decimal Debits
        {
            get { return debits; }
        }

        public decimal Credits
        {
            get { return credits; }
        }

        public IDate Dated
        {
            get { return dated; }
        }

        public string Description
        {
            get { return description; }
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

        public bool IsCurrent
        {
            get { return age < 1; }
        }

        public bool IsOneMonthOld
        {
            get { return age == 1 || age == 89; }
        }

        public bool IsTwoMonthsOld
        {
            get { return age == 2 || age == 90; }
        }

        //MSarza [20151007]
        public int TransTypeID
        {
            get { return transTypeID; }
        }
    }
}