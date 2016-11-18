using System;
using System.Collections.Generic;
using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class AgeingBalances
    {
        private readonly decimal balance;
        private readonly decimal current;
        private readonly decimal oneMonthAgeing;
        private readonly decimal twoMonthAgeing;
        private readonly decimal threeMonthsAndOver;

        public AgeingBalances(decimal current, decimal oneMonthAgeing, decimal twoMonthAgeing, decimal balance) 
        {
            this.balance = balance;
            this.twoMonthAgeing = twoMonthAgeing;
            this.oneMonthAgeing = oneMonthAgeing;
            this.current = current;
        }

        public AgeingBalances(IEnumerable<StatementReportRecord> records)
        {
            foreach (StatementReportRecord record in records)
            {
                balance += record.Amount;

                if (record.IsCurrent)
                {
                    current += record.Amount;
                }
                else if (record.IsOneMonthOld)
                {
                    oneMonthAgeing += record.Amount;
                }
                else if (record.IsTwoMonthsOld)
                {
                    twoMonthAgeing += record.Amount;
                }
                else
                {
                    threeMonthsAndOver += record.Amount;
                }
            }
        }


        public decimal ThreeMonthsAndOver
        {
            get { return threeMonthsAndOver; }
        }

        public decimal TwoMonthAgeingPercentage
        {
            get { return balance == 0 ? 0 : twoMonthAgeing/balance; }
        }

        public decimal ThreeMonthPlusAgeingPercentage
        {
            get { return balance == 0 ? 0 : ThreeMonthPlusAgeing/balance; }
        }

        public decimal OneMonthAgeingPercentage
        {
            get { return balance == 0 ? 0 : oneMonthAgeing/balance; }
        }

        public decimal CurrentPercentage
        {
            get { return balance == 0 ? 0 : current/balance; }
        }

        public decimal Balance
        {
            get { return balance; }
        }

        public decimal ThreeMonthPlusAgeing
        {
            get { return balance - current - oneMonthAgeing - twoMonthAgeing; }
        }

        public decimal TwoMonthAgeing
        {
            get { return twoMonthAgeing; }
        }

        public decimal OneMonthAgeing
        {
            get { return oneMonthAgeing; }
        }

        public decimal Current
        {
            get { return current; }
        }
    }
}