using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace Cff.SaferTrader.Core.Repositories
{
    public class DictionaryRepository : SqlManager, IDictionaryRepository
    {
        public DictionaryRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IDictionary<int, string> LoadActivityTypes()
        {
            IDictionary<int, string> activityTypes = new Dictionary<int, string>();
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "ActivityType_GetActivityTypes"))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (!cleverReader.IsNull && cleverReader.Read())
                    {
                        activityTypes.Add(cleverReader.ToInteger("ActivityTypeId"),
                                          cleverReader.ToString("ActivityType"));
                    }
                }
            }
            return activityTypes;
        }

        public IDictionary<int, string> LoadNoteTypes()
        {
            IDictionary<int, string> activityTypes = new Dictionary<int, string>();
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "NoteType_GetNoteTypes"))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (!cleverReader.IsNull && cleverReader.Read())
                    {
                        activityTypes.Add(cleverReader.ToInteger("NoteTypeId"),
                                          cleverReader.ToString("NoteType"));
                    }
                }
            }
            return activityTypes;
        }
    }
}