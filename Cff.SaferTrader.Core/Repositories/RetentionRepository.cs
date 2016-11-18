using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Cff.SaferTrader.Core.Builders;
using DataEntry;

namespace Cff.SaferTrader.Core.Repositories
{
    public class RetentionRepository : SqlManager, IRetentionRepository
    {
        private readonly Batches batches = new Batches();
        
        public RetentionRepository(string connectionString) :base (connectionString)
        {
        }

        public IList<RetentionSchedule> LoadRetentionSchedules(int clientId, DateRange dateRange)
        {
            IList<RetentionSchedule> retentionSchedules = new List<RetentionSchedule>();
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(connection, 
                                                                      "stGetRetentionSchedule",
                                                                      CreateRetentionScheduleParameters(clientId, dateRange)))
                {
                    CleverReader cleverReader = new CleverReader(reader);
                    while (cleverReader.Read())
                    {
                        retentionSchedules.Add(new RetentionScheduleBuilder(cleverReader).Build());
                    }

                }
            }
            return RecordLimiter.ReturnMaximumRecords(retentionSchedules);
        }

        public ChargeCollection LoadCharges(int retentionId)
        {
            ChargeCollection charges = new ChargeCollection();
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(connection,
                                                                      "stGetRetentionCharges",
                                                                      CreateRetentionIdParameter(retentionId)))
                {
                    CleverReader cleverReader = new CleverReader(reader);
                    while (cleverReader.Read())
                    {
                        charges.Add(new ChargeBuilder(cleverReader).Build());
                    }

                }
            }
            return charges;
        }

        public IList<ClaimedCredit> LoadCreditsClaimed(int retentionItemDate, int clientId)
        {
            DataTableCollection tables = null;
            DataSet theDS =  batches.LoadBatchTrns(2, retentionItemDate.ToString(), clientId, 7);
            if (theDS != null)
            {
                tables = theDS.Tables;
                return new CreditsClaimedBuilder().Build(tables["BatchTrns"]);
            }

            return null;
        }

        public IList<ClaimedRetentionRepurchase> LoadClaimedRetentionRepurchase(int retentionItemDate, int clientId)
        {
            DataTableCollection tables = null;
            DataSet theDS = batches.LoadBatchTrns(5, retentionItemDate.ToString(), clientId, 7);
            if (theDS != null) {
                tables = theDS.Tables;
                return new ClaimedRetentionRepurchaseBuilder().Build(tables["BatchTrns"]);
            }
            return null;
        }

        public IList<RetentionSchedule> LoadRetentionSchedulesForAllClients(Date date)
        {
            IList<RetentionSchedule> retentionSchedules = new List<RetentionSchedule>();
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(connection,
                                                                      "stGetRetentionScheduleForAllClients",
                                                                      CreateRetentionScheduleForAllClientsParameter(date)))
                {
                    CleverReader cleverReader = new CleverReader(reader);
                    while (cleverReader.Read())
                    {
                        retentionSchedules.Add(new RetentionScheduleBuilder(cleverReader).Build());
                    }

                }
            }
            return RecordLimiter.ReturnMaximumRecords(retentionSchedules);
        }

        public RetentionDetails LoadRetentionDetails(int retentionId)
        {
            RetentionDetails retentionDetails = null;
            ChargeCollection charges = LoadCharges(retentionId);

            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(connection,
                                                                      "stGetRetentionDetails",
                                                                      CreateRetentionIdParameter(retentionId)))
                {
                    CleverReader cleverReader = new CleverReader(reader);
                    if (cleverReader.Read())
                    {
                        RetentionInfo retentionInfo = new RetentionInfoBuilder(cleverReader).Build();
                        RetentionDeductable retentionDeductable = new RetentionDeductableBuilder(cleverReader).Build();
                        TransactionsAfterEndOfMonth transactionsAfterEndOfMonth = new TransactionsAfterEndOfMonthBuilder(cleverReader).Build();
                        RetentionSummary retentionSummary = new RetentionSummaryBuilder(cleverReader).Build();
                        OverdueFee overdueFee = new OverdueFeeBuidler(cleverReader).Build();

                        retentionDetails = new RetentionDetailsBuilder(cleverReader).Build(retentionInfo, 
                                                                                           retentionDeductable, 
                                                                                           transactionsAfterEndOfMonth, 
                                                                                           retentionSummary, 
                                                                                           charges, 
                                                                                           overdueFee);
                    }
                }
            }
            return retentionDetails;
        }

        private static SqlParameter[] CreateRetentionScheduleParameters(int clientId, DateRange dateRange)
        {
            SqlParameter clientIdParameter = new SqlParameter("@ClientID", SqlDbType.Int);
            SqlParameter startDateParameter = new SqlParameter("@MinYrMthGet", SqlDbType.BigInt);
            SqlParameter endDateParameter = new SqlParameter("@MaxYrMthGet", SqlDbType.BigInt);

            clientIdParameter.Value = clientId;
            startDateParameter.Value = dateRange.StartDate.ToYearMonthValue();
            endDateParameter.Value = dateRange.EndDate.ToYearMonthValue();

            return new[]{
                           clientIdParameter,
                           startDateParameter,
                           endDateParameter
                        };
        }

        private static SqlParameter[] CreateRetentionIdParameter(int retentionId)
        {
            SqlParameter retentionIdParameter = new SqlParameter("@RetnID", SqlDbType.Int);
            retentionIdParameter.Value = retentionId;

            return new[]{ retentionIdParameter };
        }
        private static SqlParameter[] CreateRetentionScheduleForAllClientsParameter(Date date)
        {
            SqlParameter yearMonthParameter = new SqlParameter("@YrMthGet", SqlDbType.BigInt);
            yearMonthParameter.Value = date.ToYearMonthValue();

            return new[] { yearMonthParameter };
        }
    }
}