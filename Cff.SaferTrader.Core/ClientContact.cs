using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class ClientContact
    {
        private readonly long contactId;
        private readonly int clientId;
        private readonly int clientNumber;
        private readonly string clientName;
        private string fullName;
        private string phone;
        private string fax;
        private string email;
        private string mobilePhone;
        private string lastName;
        private string firstName;
        private string role;
        private string address1;
        private string address2;
        private string address3;
        private string address4;
        private DateTime modified;
        private int modifiedby;


        public int ClientId
        {
            get { return clientId; }
        }

        public bool IsMatched { get; set; }

        public ClientContact()
        { 
        }

        public ClientContact(long contactId, int clientId, int clientNumber, string clientName, string lastName, string firstName, 
                                string phone, string fax, string email, string mobilePhone, string strRole, string addr1,
                                    string addr2, string addr3, string addr4)
        {
            this.contactId = contactId;
            this.clientId = clientId;
            this.clientNumber = clientNumber;
            this.clientName = clientName;
            this.lastName = lastName;
            this.firstName = firstName;
            this.phone = phone;
            this.fax = fax;
            this.email = email;
            this.mobilePhone = mobilePhone;
            this.role = strRole;
            this.address1 = addr1;
            this.address2 = addr2;
            this.address3 = addr3;
            this.address4 = addr4;

            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                fullName = firstName + " " + lastName;
            }
            else if (!string.IsNullOrEmpty(firstName))
            {
                fullName = firstName;
            }
            else if (!string.IsNullOrEmpty(lastName))
            {
                fullName = lastName;
            }
        }

        public ClientContact(long contactId, int clientId, int clientNumber, string clientName, string lastName, string firstName,
                           string phone, string fax, string email, string mobilePhone, string strRole, string addr1,
                               string addr2, string addr3, string addr4, DateTime modified, int modifiedby)
        {
            this.contactId = contactId;
            this.clientId = clientId;
            this.clientNumber = clientNumber;
            this.clientName = clientName;
            this.lastName = lastName;
            this.firstName = firstName;
            this.phone = phone;
            this.fax = fax;
            this.email = email;
            this.mobilePhone = mobilePhone;
            this.role = strRole;
            this.address1 = addr1;
            this.address2 = addr2;
            this.address3 = addr3;
            this.address4 = addr4;
            this.modified = modified;
            this.modifiedby = modifiedby;

            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                fullName = firstName + " " + lastName;
            }
            else if (!string.IsNullOrEmpty(firstName))
            {
                fullName = firstName;
            }
            else if (!string.IsNullOrEmpty(lastName))
            {
                fullName = lastName;
            }
        }

        public int ClientNumber
        {
            get { return clientNumber; }
        }

        public string ClientName
        {
            get { return clientName; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { this.firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { this.lastName = value; }
        }

        public long ContactId
        {
            get { return contactId; }
        }

        public string MobilePhone
        {
            get { return mobilePhone; }
            set { mobilePhone = value; }
        }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Role
        {
            get { return role; }
            set { role = value; }
        }

        public string Address1
        {
            get { return address1; }
            set { address1 = value; }
        }

        public string Address2
        {
            get { return address2; }
            set { address2 = value; }
        }

        public string Address3
        {
            get { return address3; }
            set { address3 = value; }
        }

        public string Address4
        {
            get { return address4; }
            set { address4 = value; }
        }

        public DateTime Modified
        {
            get { return this.modified; }
            set { this.modified = value; }
        }

        public int ModifiedBy
        {
            get { return this.modifiedby; }
            set { this.modifiedby = value; }
        }
    }
}