using System.Data;
using System.Data.SqlClient;
using Cff.SaferTrader.Core.Builders;

namespace Cff.SaferTrader.Core.Repositories
{
    public class ManagementRepository : SqlManager, IManagementRepository
    {
        public ManagementRepository(string connectionString) : base(connectionString)
        {
        }

        public ManagementDetails LoadManagementDetails()
        {
            ManagementDetails managementDetails = null;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "ManagementDetails_Load"))
                {
                    CleverReader cleverReader = new CleverReader(reader);
                    managementDetails = new ManagementDetailsBuilder(cleverReader).Build();
                }
            }

            return managementDetails;
        }
    }
}