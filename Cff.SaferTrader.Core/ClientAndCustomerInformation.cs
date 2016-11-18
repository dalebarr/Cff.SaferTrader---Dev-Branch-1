namespace Cff.SaferTrader.Core
{
    public class ClientAndCustomerInformation
    {
        private readonly CffCustomerInformation cffCustomerInformation;
        private readonly CffClientInformation cffClientInformation;

        private readonly CffCustomerContact cffDefaultCustContact;  //MSarza [20151001]
        private readonly CffMgtDetails cffMgtDetails;               //MSarza [20150731]

        public ClientAndCustomerInformation(CffCustomerInformation cffCustomerInformation, CffClientInformation cffClientInformation)
        {
            this.cffCustomerInformation = cffCustomerInformation;
            this.cffClientInformation = cffClientInformation;
        }

        public ClientAndCustomerInformation(CffCustomerInformation cffCustomerInformation,
                                                CffClientInformation cffClientInformation,
                                                CffCustomerContact cffDefaultCustContact,
                                                CffMgtDetails cffMgtDetails)
        {
            this.cffCustomerInformation = cffCustomerInformation;
            this.cffClientInformation = cffClientInformation;
            this.cffDefaultCustContact = cffDefaultCustContact;
            this.cffMgtDetails = cffMgtDetails;

        }

        public CffClientInformation CffClientInformation
        {
            get { return cffClientInformation; }
        }

        public CffCustomerInformation CffCustomerInformation
        {
            get { return cffCustomerInformation; }
        }
        public string AllowCalls
        {
            get
            {
                string allwoCallsText = "No";
                if (CffCustomerInformation.NoCalls == 0)
                {
                    if (CffClientInformation.NoCalls == 0)
                    {
                        allwoCallsText = "Yes";
                    }
                }
                else if (CffCustomerInformation.NoCalls == 1)
                {
                    allwoCallsText = "Yes";
                }
                return allwoCallsText;
            }
        }

        //MSarza [20150731]
        public CffMgtDetails CffMgtDetail
        {
            get { return this.cffMgtDetails; }
        }

        //MSarza [20151001]
        public CffCustomerContact CffCustomerContact
        {
            get { return this.cffDefaultCustContact; }
        }
    }
}