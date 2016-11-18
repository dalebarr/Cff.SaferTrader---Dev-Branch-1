using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cff.SaferTrader.Core.Builders;
using Cff.SaferTrader.Core.Common;

using DataEntry;


namespace Cff.SaferTrader.Core.Repositories
{
    public class BatchRepository : SqlManager, IBatchRepository
    {
        private readonly Batches batches = new Batches();
        private const int Action = 1;

        //start declare some event here and raise as callback
        public event EventHandler StartDataFetchEvent;
        public event EventHandler EndDataFetchEvent;
        //end declare some event here and raise as callback

        #region IBatchRepository Members

        public BatchRepository(string connectionString) : base(connectionString)
        {
        }

        public IList<InvoiceBatch> LoadInvoiceBatchesForDateRange(int clientId, BatchType batchType, DateRange dateRange)
        {
            ArgumentChecker.ThrowIfNull(batchType, "batchType");
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");
            if (!batchType.IsDateRangeDependant)
            {
                throw new InvalidOperationException("Batch type is not date range dependent");
            }

            //start ref: mariper
            //we should be able to implement call back of event functions here to somehow tell the client that the DB is still fetching as this bit of code takes too long when records exceeds 2K+
            if (StartDataFetchEvent != null)
                StartDataFetchEvent(this, (EventArgs)(new CEventArgs(200,"start")));

            DataSet theDS = batches.LoadBatches(Action,
                                                 batchType.Id,
                                                 string.Empty,
                                                 dateRange.StartDate.Value.DateTime,
                                                 dateRange.EndDate.Value.DateTime,
                                                 ref clientId,
                                                 7);
            if (theDS == null)
                return null;

            DataTableCollection tables = theDS.Tables;
            int rowCount = (tables==null)?0:(tables["Batches"]==null)?0:(tables["Batches"].Rows.Count);
            if (EndDataFetchEvent != null)
                EndDataFetchEvent(this, (EventArgs)(new CEventArgs(rowCount, "stop")));

            IList<InvoiceBatch> invoiceBatches = new InvoiceBatchBuilder().Build(tables["Batches"]);
            //end ref: mariper
            return RecordLimiter.ReturnMaximumRecords(invoiceBatches);
        }

        public IList<InvoiceBatch> LoadInvoiceBatchesFor(int clientId, BatchType batchType)
        {
            ArgumentChecker.ThrowIfNull(batchType, "batchType");
            if (batchType.IsDateRangeDependant)
            {
                throw new InvalidOperationException("Batch type is date range dependent");
            }

            try
            {
                DataSet theDS = batches.LoadBatches(Action, batchType.Id, string.Empty, Date.MinimumDate.DateTime, Date.MinimumDate.DateTime,ref clientId, 7);
                if (theDS != null) {
                    DataTableCollection tables = theDS.Tables;
                    IList<InvoiceBatch> invoiceBatches = new InvoiceBatchBuilder().Build(tables["Batches"]);
                    return RecordLimiter.ReturnMaximumRecords(invoiceBatches);
                }
            }
            catch { }
            return null;
        }

        public IList<InvoiceBatch> LoadInvoiceBatchesForBatchNumber(int clientId, string batchNumberToSearch)
        {
            IList<InvoiceBatch> invoiceBatches = new List<InvoiceBatch>();
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(connection,
                                                                      "stGetMatchedBatches",
                                                                      CreateGetMatchedBatchesParameter(clientId, batchNumberToSearch)))
                {
                    CleverReader cleverReader = new CleverReader(reader);
                    while (cleverReader.Read())
                    {
                        invoiceBatches.Add(new InvoiceBatchBuilder().Build(cleverReader));
                    }
                }
            }
            return invoiceBatches;
        }


        public IList<Invoice> LoadInvoicesFor(int clientId, int batchId)
        {//need to refactor like this so we don't get any null reference exception when call to DataEntry returns nothing
            try
            {
                DataSet ds = batches.LoadBatchTrns(1, batchId.ToString(), clientId, 7);
                DataTableCollection tables = (ds==null)?null:ds.Tables;
                return new InvoiceBuilder().Build(tables["BatchTrns"]);
            }
            catch { }
            return null;
        }

        public IList<Invoice> LoadNonFactoredInvoicesFor(int clientId, int batchId)
        {
            DataSet ds =  batches.LoadBatchTrns(-1, batchId.ToString(), clientId, 7);
            DataTableCollection tables =(ds==null)?null:ds.Tables;
            if (tables == null) return null;
            return new InvoiceBuilder().Build(tables["BatchTrns"]);
        }

        public IList<CreditLine> LoadCreditLinesFor(int clientId, int batchId)
        { 
            DataSet ds = batches.LoadBatchTrns(2, batchId.ToString(), clientId, 7);
            DataTableCollection tables = (ds==null)?null:ds.Tables;
            if (tables == null) return null;
            return new CreditLineBuilder().Build(tables["BatchTrns"]);
        }

        public IList<RepurchasesLine> LoadRepurchasesLinesFor(int clientId, int batchId)
        {
            DataSet ds =  batches.LoadBatchTrns(5, batchId.ToString(), clientId, 7);
            DataTableCollection tables = (ds==null)?null:ds.Tables;
            if (tables == null) return null;

            return new RepurchasesLineBuilder().Build(tables["BatchTrns"]);
        }

        public IList<LikelyRepurchasesLine> LoadLikelyRepurchasesLinesFor(int clientId, int batchId, int custID, int userID, string strAsAt)
        { //call the stored procedure for likely repurchases
            DataEntry.Reports reports = new DataEntry.Reports();
            DataSet ds = reports.Get_RptsTrns(21000, clientId, custID, strAsAt, userID, -1, 0, -1, 0, 0, 0, "All");
            if (ds==null) { return null; }
            else { return new LikelyRepurchasesLineBuilder().Build(ds.Tables[1]); }
        }

        public BatchSchedule LoadBatchScheduleFor(int clientId, int batchId, ChargeCollection charges)
        {
            DataSet ds = batches.LoadBatches(Action,  -1, batchId.ToString(), DateTime.Today, DateTime.Today,ref clientId,7);
            DataTableCollection tables = (ds==null)?null:ds.Tables;
            if (tables==null) return null;

            BatchScheduleFinanceInfo scheduleFinanceInfo = new BatchScheduleFinanceInfoBuilder().Build(tables["Batches"], charges);
            ClientAttribute clientAttribute = new ClientAttributeBuilder().Build(tables["Batches2"]);
            return new ScheduleBuilder().Build(tables["Batches"], scheduleFinanceInfo, clientAttribute);
        }

        public ChargeCollection LoadBatchCharges(int batchId)
        {
            ChargeCollection charges = new ChargeCollection();
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(connection,
                                                                      "stGetBatchCharges",
                                                                      CreateBatchIdParameter(batchId)))
                {
                    CleverReader cleverReader = new CleverReader(reader);
                    while (cleverReader.Read())
                    {
                        charges.Add(new BatchChargeBuilder(cleverReader).Build());
                    }
                }
            }
            return charges;
        }

        private static SqlParameter[] CreateBatchIdParameter(int batchId)
        {
            SqlParameter batchIdParameter = new SqlParameter("@BatchId", SqlDbType.Int);
            batchIdParameter.Value = batchId;
            return new[] {batchIdParameter};
        }

        private static SqlParameter[] CreateGetMatchedBatchesParameter(int clientId, string batchNumberToSearch)
        {
            SqlParameter clientIdParameter = new SqlParameter("@ClientID", SqlDbType.BigInt);
            clientIdParameter.Value = (long)clientId;
            SqlParameter batchNumberToSearchParameter = new SqlParameter("@BatchNumberToSearch", SqlDbType.VarChar,30);
            batchNumberToSearchParameter.Value = batchNumberToSearch;
            return new[] { clientIdParameter, batchNumberToSearchParameter };
        }

        #endregion
    }
}