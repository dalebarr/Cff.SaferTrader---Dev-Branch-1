using System.Data;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class ClientAttributeBuilder
    {
        public ClientAttribute Build(DataTable clientAtrributeTabel)
        {
            ClientAttribute clientAttribute = null;
            var reader = new DataRowReader(clientAtrributeTabel.Rows);
            if (reader.Read())
            {
                clientAttribute = new ClientAttribute(reader.ToDecimal("NFAdminFee"),
                                                      reader.ToDecimal("AdminFee1"),
                                                      reader.ToDecimal("FactorFee1"),
                                                      reader.ToDecimal("RetentPercent")
                    );
            }
            return clientAttribute;
        }
    }
}