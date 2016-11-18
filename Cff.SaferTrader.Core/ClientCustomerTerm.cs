namespace Cff.SaferTrader.Core
{
    public class ClientCustomerTerm
    {
        private short customerInvTerms;
        private short setCustTerms;
        private short customerCustTerms;
        private short termsFrom;
        private short clientInvTerms;
        private short invDays;
        private short invDaysFrom;

        public ClientCustomerTerm(short customerInvTerms, short setCustTerms, short custTerms, short termsFrom,
                                  short clientInvTerms, short invDays, short invDaysFrom)
        {
            this.customerInvTerms = customerInvTerms;
            this.setCustTerms = setCustTerms;
            customerCustTerms = custTerms;
            this.termsFrom = termsFrom;
            this.clientInvTerms = clientInvTerms;
            this.invDays = invDays;
            this.invDaysFrom = invDaysFrom;
        }

        public string GetCustomerTerms()
        {
            string customerTerms;
            string fromWhen = string.Empty;
            if (customerCustTerms == -1 || (customerInvTerms == 1 && setCustTerms == 1))
            {
                customerTerms = GetInheritedClientTerms();
            }
            else
            {
                switch (customerCustTerms)
                {
                    case -1:
                        //Not set yet..inherit from Client See above!
                        customerTerms = GetInheritedClientTerms();
                        break;
                    case 0:
                        customerTerms = "20th of month Following";
                        break;
                    case 1:
                        customerTerms = "End of month Following";
                        break;
                    default:
                        switch (termsFrom)
                        {
                            case -1:
                                //Not set yet..inherit from Client
                                return GetInheritedClientTerms();
                            case 0:
                                fromWhen = "from invoice date";
                                break;
                            case 1:
                                fromWhen = "from beginning of follwing month";
                                break;
                        }
                        string NumDays = customerCustTerms.ToString();
                        customerTerms = NumDays + " days " + fromWhen;
                        break;  
                }
            }
            return customerTerms;
        }

        private string GetInheritedClientTerms()
        {
            string customerTerms;
            switch (clientInvTerms)
            {
                case 0:
                    customerTerms = "20th of month Following";
                    break;
                case 1:
                    customerTerms = "End of month Following";
                    break;
                default:
                    string fromWhen=string.Empty;
                    switch (invDaysFrom)
                    {   
                        case -1:
                            //Not set yet..inherit from Client
                            break;
                        case 0:
                            fromWhen = "from invoice date";
                            break;
                        case 1:
                            fromWhen = "from beginning of follwing month";
                            break;
                    }
                    string NumDays = invDays.ToString();
                    customerTerms = NumDays + " days " + fromWhen;
                    break;
            }
            return customerTerms;
        }
    }
}