namespace Cff.SaferTrader.Core.Builders
{
    public class CffCustomerBuilder
    {
        private readonly IReader reader;


        public CffCustomerBuilder(IReader reader)
        {
            this.reader = reader;
        }

        public CffCustomer Build()
        {
            //System.Diagnostics.Debug.Write("CffCustomer[Build()]: CustNum passed by: ");
            //System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            //System.Diagnostics.Debug.WriteLine(stackTrace.GetFrame(1).GetMethod().Name);

            return Build(reader.FromBigInteger("customerID"));
        }

        public CffCustomer Build(int customerId)
        {
            //System.Diagnostics.Debug.Write("CffCustomer[Build(x)]: CustNum passed by: ");
            //System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            //System.Diagnostics.Debug.WriteLine(stackTrace.GetFrame(1).GetMethod().Name);

            return new CffCustomer(reader.ToString("Customer"), customerId,
                                    reader.FromBigInteger("CustNum"));
                                  //reader.FromBigInteger("CustomerID"));
        }

        //public CffCustomer Build2()
        //{
        //    //System.Diagnostics.Debug.Write("CffCustomer[Build2()]: CustNum passed by: ");
        //    //System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        //    //System.Diagnostics.Debug.WriteLine(stackTrace.GetFrame(1).GetMethod().Name);
        //    return Build2(reader.FromBigInteger("customerID"));
        //}
        //public CffCustomer Build2(int customerId)
        //{
        //    //System.Diagnostics.Debug.Write("CffCustomer[Build2(x)]: CustNum passed by: ");
        //    //System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
        //    //System.Diagnostics.Debug.WriteLine(stackTrace.GetFrame(1).GetMethod().Name);
        //    return new CffCustomer(reader.ToString("Customer"), customerId,
        //                            //reader.FromBigInteger("CustNum"));
        //                            reader.FromBigInteger("CustomerID"));
        //}



    }
}