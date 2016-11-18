using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Reports;
using Cff.SaferTrader.Core.Repositories;
using System.Data.SqlClient;

namespace Cff.SaferTrader.Core.Builders
{
    public class TransactionReportRecordBuilder
    {
        private ICustomerRepository customerRepository;
        public IList<TransactionReportRecord> BuildForAllClients(DataRowCollection rows, string trxFilter)
        {
            IList<TransactionReportRecord> records = new List<TransactionReportRecord>();

            decimal batchTotal = 0;
            var reader = new DataRowReader(rows);
            while (reader.Read())
            {

                bool bContinue = false;
                int tStat = reader.ToInteger("TypeStatusID");
                switch (trxFilter)
                {
                    case "Funding":
                        //if (tStat == 0) { bContinue = true;}
                        if (tStat == 0 || tStat == 2 || tStat == 3) { bContinue = true; }
                        break;

                    case "Non-Funding":
                        if (tStat == 1) { bContinue = true; }
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

        public IList<TransactionReportRecord> Build(DataRowCollection rows, int clientNumber, string clientName)
        {
            IList<TransactionReportRecord> records = new List<TransactionReportRecord>();

            decimal batchTotal = 0;
            var reader = new DataRowReader(rows);
            while (reader.Read())
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

        public IList<TransactionReportRecord> Build(DataRowCollection rows, int clientNumber, string clientName, string trxFilter, string yearMonth)
        {
            IList<TransactionReportRecord> records = new List<TransactionReportRecord>();

            decimal batchTotal = 0;
            var reader = new DataRowReader(rows);
            customerRepository = RepositoryFactory.CreateCustomerRepository();
            while (reader.Read())
            {
                var yrMoPick = reader.ToString("YearMonth");
                var customerId = reader.ToInteger("CustomerID");
                var isCustBelongsToClient = customerRepository.CheckCustomerBelongsToClient(clientNumber, customerId);
                if (yrMoPick == yearMonth && isCustBelongsToClient)
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
            }
            return records;
        }




        public IList<TransactionReportRecord> Build(DataRowCollection rows, int clientNumber, string clientName, string trxFilter)
        {
            IList<TransactionReportRecord> records = new List<TransactionReportRecord>();

            decimal batchTotal = 0;
            var reader = new DataRowReader(rows);
            while (reader.Read())
            {
                bool bContinue = false;
                int tStat = reader.ToInteger("TypeStatusID");
                switch (trxFilter)
                { 
                    case "Funding":
                        //if (tStat == 0) { bContinue = true;}
                        if (tStat == 0 || tStat == 2 || tStat == 3) { bContinue = true; }
                        break;

                    case "Non-Funding":
                        if (tStat == 1){ bContinue = true; }
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

        private static TransactionReportRecord Build(DataRowReader reader, decimal? batchTotal, int clientNumber,
                                                     string clientName)
        {
            return new TransactionReportRecord(reader.ToInteger("TrnID"), clientNumber,
                                               clientName, reader.ToInteger("CustomerID"), reader.ToInteger("CustNum"),
                                               reader.ToString("Customer"), reader.ToDate("Dated"),
                                               reader.ToString("TrnNumber"), reader.ToString("Reference"),
                                               reader.ToDecimal("Amount"), reader.ToDate("Factored"),
                                               reader.ToInteger("BatchID"), batchTotal,
                                               TransactionStatus.Parse(reader.ToInteger("TypeStatusID")),
                                               TransactionType.Parse(reader.ToInteger("TransTypeID")));
        }

    }
}