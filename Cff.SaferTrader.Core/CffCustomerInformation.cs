namespace Cff.SaferTrader.Core
{
    public class CffCustomerInformation
    {
        private readonly CffCustomer customer;
        private readonly IDate lastPaid;
        private readonly decimal lastAmount;
        private readonly ClientCustomerTerm clientCustomerTerm;
        private readonly AgeingBalances ageingBalances;
        private readonly Date listDate;
        private readonly short noCalls;
        private readonly short stopCredit;
        private readonly decimal creditLimit;
        private readonly string address1;
        private readonly string address2;
        private readonly string address3;
        private readonly string address4;
        private readonly string emailStatmentsAddr;
        private readonly Date notifydate;               //MSarza [20150820]

        public CffCustomerInformation(CffCustomer customer, short stopCredit, decimal creditLimit, Date listDate, Date nextCall, 
            short customerNoCalls, IDate lastPaid, decimal lastAmount, ClientCustomerTerm clientCustomerTerm, AgeingBalances ageingBalances)
        {
            this.customer = customer;
            this.stopCredit = stopCredit;
            this.creditLimit = creditLimit;
            this.listDate = listDate;
            NextCall = nextCall;
            noCalls = customerNoCalls;
            this.lastPaid = lastPaid;
            this.lastAmount = lastAmount;
            this.clientCustomerTerm = clientCustomerTerm;
            this.ageingBalances = ageingBalances;
        }

        public CffCustomerInformation(CffCustomer customer, short stopCredit, decimal creditLimit, Date listDate, Date nextCall,
                short customerNoCalls, IDate lastPaid, decimal lastAmount, ClientCustomerTerm clientCustomerTerm, AgeingBalances ageingBalances,
                string addr1, string addr2, string addr3, string addr4, string emailStatmentsAddr, Date notifydate)
        {
            this.customer = customer;
            this.stopCredit = stopCredit;
            this.creditLimit = creditLimit;
            this.listDate = listDate;
            NextCall = nextCall;
            noCalls = customerNoCalls;
            this.lastPaid = lastPaid;
            this.lastAmount = lastAmount;
            this.clientCustomerTerm = clientCustomerTerm;
            this.ageingBalances = ageingBalances;
            this.address1 = addr1;
            this.address2 = addr2;
            this.address3 = addr3;
            this.address4 = addr4;
            this.emailStatmentsAddr = emailStatmentsAddr;
            this.notifydate = notifydate;                       //MSarza [20150820]
        }


        public AgeingBalances AgeingBalances
        {
            get { return ageingBalances; }
        }

        public decimal LastAmount
        {
            get { return lastAmount; }
        }

        public short NoCalls
        {
            get { return noCalls; }
        }

        public CffCustomer Customer
        {
            get { return customer; }
        }

        public ClientCustomerTerm ClientCustomerTerm
        {
            get { return clientCustomerTerm; }
        }

        public IDate LastPaid
        {
            get { return lastPaid; }
        }

        public Date ListDate
        {
            get { return listDate; }
        }

        public string StopCredit
        {
            get { return stopCredit == 0 ? "No" : "Yes"; }
        }

        public Date NextCall { get; set; }

        public decimal CreditLimit
        {
            get { return creditLimit; }
        }

        public string Address1
        {
            get { return address1; }
        }

        public string Address2
        {
            get { return address2; }
        }

        public string Address3
        {
            get { return address3; }
        }

        public string Address4
        {
            get { return address4; }
        }

        public string EmailStatmentsAddr
        {
            get { return emailStatmentsAddr; }
        }

        //MSarza [20150820]
        public Date NotifyDate
        {
            get { return notifydate; }
        }
    }

}