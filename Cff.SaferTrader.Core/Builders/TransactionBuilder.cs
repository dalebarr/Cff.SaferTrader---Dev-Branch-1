using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Builders
{
    public class TransactionBuilder
    {
        private readonly DataRowReader reader;

        public TransactionBuilder(DataRowReader reader)
        {
            this.reader = reader;
        }

        public Transaction Build()
        {
           
            return new Transaction(reader.ToInteger("TrueTrnID"),
                                   reader.ToDate("Dated"),
                                   reader.ToDate("Factored"),
                                   reader.ToString("Type").Trim(),
                                   reader.ToString("Number"),
                                   reader.ToString("Reference"),
                                   reader.ToDecimal("Amount"),
                                   reader.ToDecimal("Balance"),
                                   reader.ToString("Status"),
                                   reader.ToInteger("Batch"),
                                   reader.ToString("CurrentTrnNotes")  //11/07/2012: as per marty's request add current notes
                                   );
        }

        public Transaction BuildDetailTransaction()
        {
            return new Transaction(reader.ToInteger("TrueTrnID"),
                                   reader.ToDate("Dated"),
                                   reader.ToDate("Factored"),
                                   reader.ToString("Type").Trim(),
                                   reader.ToString("Number"),
                                   reader.ToString("Reference"),
                                   reader.ToDecimal("Amount"),
                                   reader.ToDecimal("Balance"),
                                   string.Empty, 
                                   reader.ToInteger("Batch"));
                                   // TODO: Status is irrelevant in this context. 
                                   // Review the object implementation 
                                   //reader.ToString("Status"),
            //CFFWEB-8 => CurrentTrnNotes missing from storedproc
                                   //, reader.ToString("CurrentTrnNotes"));
        }
    }
}