namespace Cff.SaferTrader.Core.Builders
{
    public class CffCustomerInformationBuilder
    {
        private readonly CleverReader reader;

        public CffCustomerInformationBuilder(CleverReader reader)
        {
            this.reader = reader;
        }

        public CffCustomerInformation Build(CffCustomer customer, IDate lastPaid, decimal lastAmount, 
                                            ClientCustomerTerm clientCustomerTerm, AgeingBalances ageingBalances)
        {
            return new CffCustomerInformation(customer,
                                              reader.ToSmallInteger("StopCredit"),
                                              reader.ToDecimal("CreditLimit"),
                                              reader.ToDate("Created"),
                                              reader.ToDate("NextCall"),
                                              reader.ToSmallInteger("customerNoCalls"),
                                              lastPaid, lastAmount,
                                              clientCustomerTerm, ageingBalances,
                                              reader.ToString("Address1"),
                                              reader.ToString("Address2"),
                                              reader.ToString("Address3"),
                                              reader.ToString("Address4"),
                                              reader.ToString("EmailStatmentsAddr"),
                                              reader.ToDate("NotifyDate")
                                              );
        }

   
    }
}