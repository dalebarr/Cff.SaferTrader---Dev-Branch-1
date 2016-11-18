using System.Data;
using System.Data.SqlClient;
using Cff.SaferTrader.Core.Builders;

namespace Cff.SaferTrader.Core.Repositories
{
    //MSarzea [20150731]
    public class CffMgtDetailsRepository : SqlManager, ICffMgtDetails
    {
        public CffMgtDetailsRepository(string connectionString)
            : base(connectionString)
        {
        }

        public CffMgtDetails LoadCffMgtDetails()
        {
            CffMgtDetails cffMgtDetails = null;
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "ManagementDetails_Load"))
                {
                    CleverReader cleverReader = new CleverReader(reader);
                    cleverReader.Read();        //Msarza  - added
                    cffMgtDetails = new CffMgtDetailsBuilder(cleverReader).Build();
                }
            }

            return cffMgtDetails;
        }
    }
}
