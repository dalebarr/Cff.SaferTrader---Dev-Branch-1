using System;
using System.Data;
using System.Data.SqlClient;

namespace Cff.SaferTrader.Core.Repositories
{
    public class RetentionNotesTabRepository :  SqlManager,IRetentionNotesTabRepository
    {
        public RetentionNotesTabRepository(string connectionString)
            : base(connectionString)
        {

        }
        public IRetentionNote LoadRetentionNotesFor(int retentionScheduleId)
        {
            IRetentionNote retentionNote ;

            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "RetnNotes_LoadNote",
                                                                          CreateRetnNotes_LoadNoteParameters
                                                                              (retentionScheduleId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    if (cleverReader.Read())
                    {
                        string parsedNotes = CustomerNotesParser.Parse(cleverReader.ToString("notes"));
                        retentionNote = new RetentionNote(parsedNotes);
                    }
                    else
                    {
                        retentionNote = new NullRetentionNote();
                    }
                }
            }
            return retentionNote;
        }
        private SqlParameter[] CreateRetnNotes_LoadNoteParameters(int retentionScheduleId)
        {
            SqlParameter retentionScheduleIdParameter = new SqlParameter("@RetnID", SqlDbType.BigInt);
            retentionScheduleIdParameter.Value = retentionScheduleId;
            return new[] { retentionScheduleIdParameter };
        }
    }
}