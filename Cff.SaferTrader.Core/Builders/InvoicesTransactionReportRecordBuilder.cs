using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class InvoicesTransactionReportRecordBuilder
    {
        private readonly ICalendar calendar;

        public InvoicesTransactionReportRecordBuilder(ICalendar calendar)
        {
            this.calendar = calendar;
        }

        public IList<InvoicesTransactionReportRecord> BuildForAllClients(DataRowCollection rows, string transactionFilter)
        {
            IList<InvoicesTransactionReportRecord> records = new List<InvoicesTransactionReportRecord>();

            decimal batchTotal = 0;
            DataRowReader reader = new DataRowReader(rows);
            while (reader.Read())
            {

                bool bContinue = false;
                int status = reader.ToInteger("TypeStatusID");
                int TransType = reader.ToInteger("TransTypeID");

                switch (transactionFilter)
                {
                    case "Funding":
                        if (TransType == 1)//Invoice 
                        {
                            if (status == 0 || status == 4)
                            {
                                bContinue = true;
                            }
                        }
                        else if (status == 0 || status == 2 || status == 3)
                        {
                            bContinue = true;
                        }
                        break;

                    case "Non-Funding":
                        if (TransType == 1)//Invoice 
                        {
                            if (status == 1 || status == 2)
                            {
                                bContinue = true;
                            }
                        }
                        else if (status == 1)
                        {
                            bContinue = true;
                        }

                        break;

                    default: //All
                        bContinue = true;
                        break;
                }


                if (bContinue)
                {
                    batchTotal += reader.ToDecimal("Amount");
                    if (IsLastRecordInBatch(reader))
                    {
                        // Show batch total to last record in batch
                        records.Add(Build(reader, batchTotal, reader.ToInteger("ClientNum"), reader.ToString("ClientName")));
                        batchTotal = 0;
                    }
                    else
                    {
                        records.Add(Build(reader, null, reader.ToInteger("ClientNum"), reader.ToString("ClientName")));
                    }
                }
            }
            return records;
        }

        public IList<InvoicesTransactionReportRecord> Build(DataRowCollection rows, int clientNumber, string clientName)
        {
            IList<InvoicesTransactionReportRecord> records = new List<InvoicesTransactionReportRecord>();

            decimal batchTotal = 0;
            DataRowReader reader = new DataRowReader(rows);
            while(reader.Read())
            {
                batchTotal += reader.ToDecimal("Amount");
                if (IsLastRecordInBatch(reader))
                {
                    // Show batch total to last record in batch
                    records.Add(Build(reader, batchTotal, clientNumber, clientName));
                    batchTotal = 0;
                }
                else
                {
                    records.Add(Build(reader, null, clientNumber, clientName));
                }
            }
            return records;
        }

        public IList<InvoicesTransactionReportRecord> Build(DataRowCollection rows, int clientNumber, string clientName, string transactionFilter)
        {
            IList<InvoicesTransactionReportRecord> records = new List<InvoicesTransactionReportRecord>();

            decimal batchTotal = 0;
            DataRowReader reader = new DataRowReader(rows);
            while (reader.Read())
            {
                bool bContinue=false;
                int status = reader.ToInteger("TypeStatusID");
                int TransType = reader.ToInteger("TransTypeID");

                switch (transactionFilter)
                {
                    case "Funding":
                        if (TransType == 1)//Invoice 
                        {
                            if (status == 0 || status == 4)
                            {
                                bContinue = true;
                            }
                        }
                        else if (status == 0 || status == 2 || status == 3)
                        {
                            bContinue = true;
                        }
                        break;

                    case "Non-Funding":
                        if (TransType == 1)//Invoice 
                        {
                            if (status == 1 || status == 2)
                            {
                                bContinue = true;
                            }
                        }
                        else if (status == 1 )
                            {
                                bContinue = true;
                            }

                        break;

                    default: //All
                        bContinue = true;
                        break;
                }


                //switch (transactionFilter)
                //{
                //    case "Funding":
                //        if (status==0) { 
                //            bContinue=true; 
                //        }
                //        break;

                //    case "Non-Funding":
                //        if (status==1) { 
                //            bContinue=true; 
                //        }
                //        break;
                    
                //    default: //All
                //        bContinue=true;
                //        break;
                //}

               if (bContinue) {
                batchTotal += reader.ToDecimal("Amount");
                if (IsLastRecordInBatch(reader))
                {
                    // Show batch total to last record in batch
                    records.Add(Build(reader, batchTotal, clientNumber, clientName));
                    batchTotal = 0;
                }
                else
                {
                    records.Add(Build(reader, null, clientNumber, clientName));
                }
               } 
               
            }
            return records;
        }


        private static bool IsLastRecordInBatch(DataRowReader reader)
        {
            bool isLastRecordInBatch = true;
            if (reader.HasNext)
            {
                int batchId = reader.ToInteger("BatchID");
                reader.Read();
                int nextBatchId = reader.ToInteger("BatchID");
                reader.PreviousRow();
                isLastRecordInBatch = batchId != nextBatchId;
            }
            return isLastRecordInBatch;
        }

        private InvoicesTransactionReportRecord Build(DataRowReader reader, decimal? batchTotal, int clientNumber, string clientName)
        {
            return new InvoicesTransactionReportRecord(reader.ToInteger("TrnID"), clientNumber, clientName, 
                                                   reader.ToInteger("CustomerId"),
                                                   reader.ToInteger("CustNum"),
                                                   reader.ToString("Customer"),
                                                   reader.ToDate("Dated"),
                                                   reader.ToString("TrnNumber"),
                                                   reader.ToString("Reference"),
                                                   reader.ToDecimal("Amount"),
                                                   reader.ToDate("Factored"),
                                                   reader.ToInteger("BatchID"),
                                                   batchTotal,
                                                   TransactionStatus.Parse(reader.ToInteger("TypeStatusID")),
                                                   reader.ToDecimal("Balance"),
                                                   reader.ToNullableDate("ReceiptDate"), 
                                                   calendar);
        }
    }
}