namespace Cff.SaferTrader.Core
{
    public class ClientInformation
    {
        private readonly int clientNumber;
        private readonly int companyNumber;
        private readonly int clientQueries;
        private readonly Date created;
        private readonly string creditTerms;
        private readonly string individualCustomerTerms;
        private readonly string individualInvoiceTerms;
        private readonly string facilityType;
        private readonly string gstNumber;
        private readonly decimal currentAccountCustLimitSum;
        private bool hasOwnLetterTemplates;

        //MSarza [2050901]
        //private readonly string administeredBy;
        private short debtorAdministrationType;
        private readonly bool isCffDebtorAdminForClient;
        private readonly bool isClientDebtorAdmin;
        
        public ClientInformation(int clientNumber, int companyNumber, int clientQueries, Date created, string creditTerms, string individualCustomerTerms,
            string individualInvoiceTerms, string facilityType, string gstNumber, decimal currentAccountCustLimitSum, //bool administeredByCff,
            bool hasOwnLetterTemplates, short debtorAdministrationType)
        {

            this.clientNumber = clientNumber;
            this.companyNumber = companyNumber;
            this.clientQueries = clientQueries;
            this.created = created;
            this.creditTerms = creditTerms;
            this.individualCustomerTerms = individualCustomerTerms;
            this.individualInvoiceTerms = individualInvoiceTerms;
            this.facilityType = facilityType;
            this.gstNumber = gstNumber;
            this.currentAccountCustLimitSum = currentAccountCustLimitSum;
            this.hasOwnLetterTemplates = hasOwnLetterTemplates;

            // client queries       int
            // administrated by     string yes/no
            // created              datetime
            // credit terms         string
            // ind customer terms   can be set / can't be set
            // ind inv terms        can be set / can't be set
            // facility type        string (or object)
            // client #             int
            // company #            int
            // gst#                 string

            //MSarza [20150901]  
            //this.administeredByCff = administeredByCff;
            this.debtorAdministrationType = debtorAdministrationType;
            this.isCffDebtorAdminForClient = CffDebtorAdminHelper.CffIsDebtorAdminForClientAsCff(debtorAdministrationType);
            this.isClientDebtorAdmin = CffDebtorAdminHelper.ClientIsDebtorAdmin(debtorAdministrationType);

        }

        public bool HasOwnLetterTemplates
        {
            get { return hasOwnLetterTemplates; }
        }

        public int ClientNumber
        {
            get { return clientNumber; }
        }

        public int CompanyNumber
        {
            get { return companyNumber; }
        }

        public int ClientQueries
        {
            get { return clientQueries; }
        }

        public Date Created
        {
            get { return created; }
        }

        public string CreditTerms
        {
            get { return creditTerms; }
        }

        public string IndividualCustomerTerms
        {
            get { return individualCustomerTerms; }
        }

        public string IndividualInvoiceTerms
        {
            get { return individualInvoiceTerms; }
        }

        public string FacilityType
        {
            get { return facilityType; }
        }

        public string GstNumber
        {
            get { return gstNumber; }
        }

        public decimal CurrentAccountCustLimitSum
        {
            get { return currentAccountCustLimitSum; }
        }


        //MSarza [20150901] : Data type changed from bool to small int for dbo.ClientFinancials.CffDebtorAdmin
        //public bool AdministeredByCff
        //{
        //    get { return administeredByCff; }
        //    //get { return CffDebtorAdminHelper.ClientDebtorAdmin(System.Convert.ToInt16(administeredBy)); }
        //}
        public short DebtorAdministrationType
        {
            get { return debtorAdministrationType; }
        }
        public bool IsCffDebtorAdminForClient
        {
            get { return isCffDebtorAdminForClient; }
        }
        public bool IsClientDebtorAdmin 
        {
            get { return isClientDebtorAdmin; }
        }


    }
}