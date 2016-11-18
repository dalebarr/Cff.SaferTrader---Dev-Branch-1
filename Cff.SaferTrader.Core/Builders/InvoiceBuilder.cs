using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class InvoiceBuilder
    {
        public IList<Invoice> Build(DataTable table)
        {
            IList<Invoice> invoices = new List<Invoice>();

            var reader = new DataRowReader(table.Rows);
            while (reader.Read())
            {
                invoices.Add(Build(reader));
            }

            return invoices;
        }

        private static Invoice Build(DataRowReader reader)
        {
            return new Invoice(reader.ToInteger("TrueTrnID"), reader.ToInteger("ClientID"), reader.ToInteger("CustID"),
                               reader.ToInteger("CustNum"),
                               reader.ToString("Customer"),
                               reader.ToString("TrnNum"),
                               reader.ToString("Reference"),
                               reader.ToDate("FactorDate"),
                               reader.ToDate("TransDate"),
                               reader.ToDecimal("TransAmount"),
                               reader.ToDecimal("Balance"),
                               TransactionType.Parse(reader.ToInteger("TransTypeID")),
                               TransactionStatus.Parse(reader.ToInteger("TypeStatus")),
                               reader.ToDate("Created"),
                               reader.ToDate("Modified"),
                               reader.ToString("ModifiedBy"),
                               reader.ToNullableDate("RepDate"), //??
                               reader.ToString("PurchOrder"));
        }
    }
}