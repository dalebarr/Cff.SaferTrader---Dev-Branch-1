namespace Cff.SaferTrader.Core.Builders
{
    public class CffCustomerBuilderNew
    {
        private readonly IReader readerNew;
        //private readonly int id;
        //private readonly string name;
        //private readonly int number;

        public CffCustomerBuilderNew(IReader readerNew)
        {
            this.readerNew = readerNew;
        }

        public ICffCustomer BuildNew()
        {
            return BuildNew(readerNew.FromBigInteger("customerID"));
        }

        public ICffCustomer BuildNew(int customerId)
        {
            return new CffCustomerExt(readerNew.ToString("Customer"),
                                   customerId,
                                   readerNew.FromBigInteger("CustNum"));
        }

        //public int CustomerId {
        //    get { return id; }
        //}

        //public string Customer {
        //    get { return name; }
        //}

        //public int CustomerNumber {
        //    get { return number; }
        //}
       
    }
}
