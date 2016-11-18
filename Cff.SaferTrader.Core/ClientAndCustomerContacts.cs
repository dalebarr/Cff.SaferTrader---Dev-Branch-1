namespace Cff.SaferTrader.Core
{
    public class ClientAndCustomerContacts
    {
        private readonly ClientContact clientContact;
        private readonly CustomerContact customerContact;

        //MSarza [20150901] : Data type changed from bool to small int for dbo.ClientFinancials.CffDebtorAdmin.
        //private static bool administeredByCFF;
        private static bool cffIsDebtorAdminForClient;
        private static bool clientIsDebtorAdmin;

        public ClientAndCustomerContacts(ClientContact clientContact, CustomerContact customerContact)
        {
            if (clientContact != null)
            {
                ArgumentChecker.ThrowIfNull(clientContact, "clientContact");
                this.clientContact = clientContact;
            }

            if (customerContact != null)
            {
                ArgumentChecker.ThrowIfNull(customerContact, "customerContact");
                this.customerContact = customerContact;
            }
        }

        public ClientContact ClientContact
        {
            get { return clientContact; }
        }

        public CustomerContact CustomerContact
        {
            get { return customerContact; }
        }


        //MSarza [20150901]  
        //public bool isClientAdministeredByCFF
        //{
        //    get { return administeredByCFF; }
        //    set { administeredByCFF = value; }
        //}
        public bool ClientIsDebtorAdmin
        {
            get { return clientIsDebtorAdmin; }
            set { clientIsDebtorAdmin = value; }
        }
        public bool CffIsDebtorAdminForClient
        {
            get { return cffIsDebtorAdminForClient; }
            set { cffIsDebtorAdminForClient = value; }
        }
    }
}