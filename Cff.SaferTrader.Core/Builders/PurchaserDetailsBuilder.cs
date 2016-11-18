using System.Data;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class PurchaserDetailsBuilder
    {
        private readonly DataTableCollection tables;

        public PurchaserDetailsBuilder(DataTableCollection tables)
        {
            this.tables = tables;
        }

        public PurchaserDetails Build()
        {
            DataRowReader reader = new DataRowReader(tables[1].Rows);
            reader.Read();
            Address customerAddress = new AddressBuilder(reader).Build();
            CffCustomer customer = new CffCustomerBuilder(reader).Build();
            reader = new DataRowReader(tables[0].Rows);
            reader.Read();
            CffClient client = new CffClientBuilder(reader).Build();
            //CffClient client = new CffClientBuilder(CleverReader).Build();

            return new PurchaserDetails(client, customer, customerAddress);
        }
    }
}