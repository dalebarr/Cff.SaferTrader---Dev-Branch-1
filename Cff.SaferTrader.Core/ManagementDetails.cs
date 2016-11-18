using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class ManagementDetails
    {
        private readonly string name;
        private readonly string legalEntityOne;
        private readonly string legalEntityTwo;
        private readonly string phone;
        private readonly string fax;
        private readonly string email;
        private readonly string website;
        private readonly string emailLink;
        private readonly Address address;
        private readonly BankDetails bankDetails;
        private readonly string gstCode;

        public ManagementDetails(string name, string legalEntityOne, string legalEntityTwo, string phone, string fax, string email, 
            string website, Address address, BankDetails bankDetails, string gstCode)
        {
            ArgumentChecker.ThrowIfNull(address, "address");
            ArgumentChecker.ThrowIfNullOrEmpty(gstCode, "gstCode");

            this.address = address;
            this.bankDetails = bankDetails;
            this.gstCode = gstCode;
            this.name = name;
            this.legalEntityOne = legalEntityOne;
            this.legalEntityTwo = legalEntityTwo;
            this.phone = phone;
            this.fax = fax;
            this.email = email;
            this.website = website;
            
            if (!string.IsNullOrEmpty(email))
            {
                emailLink = "mailto:" + email;
            }
        }

        public string GstCode
        {
            get { return gstCode; }
        }

        public BankDetails BankDetails
        {
            get { return bankDetails; }
        }

        public string Phone
        {
            get { return phone; }
        }

        public string Fax
        {
            get { return fax; }
        }

        public string Email
        {
            get { return email; }
        }

        public string Website
        {
            get { return website; }
        }

        public string EmailLink
        {
            get { return emailLink; }
        }

        public string Name
        {
            get {
                return name;
            }
        }

        public string LegalEntityOne
        {
            get { return legalEntityOne; }
        }

        public string LegalEntityTwo
        {
            get { return legalEntityTwo; }
        }

        public Address Address
        {
            get { return address; }
        }
    }
}