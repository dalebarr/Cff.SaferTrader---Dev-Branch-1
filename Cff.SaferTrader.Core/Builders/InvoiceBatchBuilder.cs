using System.Collections.Generic;
using System.Data;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class InvoiceBatchBuilder
    {
        public IList<InvoiceBatch> Build(DataTable table)
        {
            IList<InvoiceBatch> invoiceBatches = new List<InvoiceBatch>();
            var reader = new DataRowReader(table.Rows);
            while (reader.Read())
            {
                invoiceBatches.Add(Build(reader));
            }
            return invoiceBatches;
        }

        public static InvoiceBatch Build(DataRowReader reader)
        {
            return new InvoiceBatch(reader.ToInteger("BatchNumber"),
                                    reader.ToDate("BatchDate"),
                                    reader.ToDecimal("FacInv"),
                                    reader.ToDecimal("NFInv"),
                                    reader.ToDecimal("Admin"),
                                    reader.ToDecimal("FactorFee"),
                                    reader.ToDecimal("Retention"),
                                    reader.ToDecimal("Repurchase"),
                                    reader.ToDecimal("Credit"),
                                    reader.ToDecimal("Post"),
                                    reader.ToNullableDate("Released"),
                                    reader.ToString("txtStatus"), // Status?
                                    reader.ToString("CreatedBy"),
                                    reader.ToDate("Modified"),
                                    reader.ToString("ModifiedBy"),
                                    reader.ToString("Header"),
                                    reader.ToDecimal("TotalInv"), 
                                    reader.ToString("ClientName"),
                                    reader.ToInteger("ClientID"),
                                    reader.ToInteger("FacilityType"),
                                    reader.ToDecimal("NonCompliantFee"),
                                    reader.ToDecimal("RetnPercent")

                                    );
        }

        public InvoiceBatch Build(CleverReader reader)
        {
            return new InvoiceBatch(reader.FromBigInteger("BatchNumber"),
                                    reader.ToDate("BatchDate"),
                                    reader.ToDecimal("FacInv"),
                                    reader.ToDecimal("NFInv"),
                                    reader.ToDecimal("Admin"),
                                    reader.ToDecimal("FactorFee"),
                                    reader.ToDecimal("Retention"),
                                    reader.ToDecimal("Repurchase"),
                                    reader.ToDecimal("Credit"),
                                    reader.ToDecimal("Post"),
                                    reader.ToNullableDate("Released"),
                                    reader.ToString("txtStatus"), // Status?
                                    reader.ToString("CreatedBy"),
                                    reader.ToDate("Modified"),
                                    reader.ToString("ModifiedBy"),
                                    reader.ToString("Header"),
                                    reader.ToDecimal("TotalInv"), 
                                    reader.ToString("ClientName"), 
                                    reader.FromBigInteger("ClientID"),
                                    reader.ToInteger("FacilityType"),
                                    reader.ToDecimal("NonCompliantFee"),
                                    reader.ToDecimal("RetnPercent")
                                    );
        }
    }
}