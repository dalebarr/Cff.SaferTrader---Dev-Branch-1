using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class AccountTransactionReportBuilder 
    {
        decimal creditTotal;
        decimal debitTotal;
        decimal movementAmt;
        decimal closingbalance;
        decimal openingbalance;
        IList<AccountTransactionReportRecord> records;

        public AccountTransactionReportBuilder()
        {
            creditTotal = 0;
            debitTotal = 0;
            movementAmt = 0;
            closingbalance = 0;
            openingbalance = 0;
        }

        public AccountTransactionReportBuilder(DataRowCollection rows, DataRowCollection summary)
        { 
            AccountTransactionReportRecord dummyRecord;

            var reader = new DataRowReader(rows);
            this.records = new List<AccountTransactionReportRecord>();
           
            if (summary.Count > 0)
            {
                var sumReader = new DataRowReader(summary);
                sumReader.Read();
                this.creditTotal = sumReader.ToDecimal("creditAmt");
                this.debitTotal = sumReader.ToDecimal("debitAmt");
                this.closingbalance = sumReader.ToDecimal("Balance");
                this.movementAmt = sumReader.ToDecimal("SumAmountMth");
                this.openingbalance = this.closingbalance - this.movementAmt;
            }

            //read the first record and add the balance amount to 1st record
            reader.Read();
            dummyRecord = new AccountTransactionReportRecord(reader.ToInteger("clientid"), this.openingbalance, "", "Opening Balance", null, null);
            this.records.Add(dummyRecord);
            
            //add current read record as 2nd record
            this.records.Add(Build(reader));

            //proceed with the rest
            while (reader.Read())
            {
                this.records.Add(Build(reader));
            }            
        }

        public AccountTransactionReportRecord Build(DataRowReader reader)
        {
           return new AccountTransactionReportRecord(reader.ToInteger("clientid"),
                                    reader.ToDecimal("value"),
                                    reader.ToString("description"),
                                    reader.ToString("tranref"),
                                    (IDate)reader.ToDate("date"),
                                    TransactionType.ParseRetnStatus(reader.ToInteger("type")));
        }

        
        public IList<AccountTransactionReportRecord> Records() { return this.records;  }
        public decimal CreditTotal() { return this.creditTotal;  }
        public decimal DebitTotal() { return this.debitTotal;  }
        public decimal MovementAmt() { return this.movementAmt;  }
        public decimal ClosingBalance() { return this.closingbalance; }
        public decimal OpeningBalance() { return this.openingbalance;  }

    }
}
