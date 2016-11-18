using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class CustomerContact
    {
        private readonly int clientId;
        private readonly int clientNumber;
        private readonly string clientName;
        private int contactId;
        private int customerId;
        private string fullName;
        private string phone;
        private string fax;
        private string email;
        private string mobilePhone;
        private string customerName;
        private int customerNumber;
        private string lastName;
        private string firstName;
        private string role;
        private Boolean isDefault;
        private Boolean attn;
        private DateTime? modified;
        private Int16 modifiedby;
        private string address1;
        private string address2;
        private string address3;
        private string address4;
        private Boolean emailstatement;

        private Int16 emailReceipt;           //MSarza - added line
        
        public CustomerContact()
        {

            this.contactId = 0;
            this.clientId = 0;
            this.clientNumber = 0;
            this.clientName = "";
            this.customerId = 0;
            this.customerNumber = 0;
            this.customerName = "";
            this.lastName = "";
            this.firstName = "";
            this.phone = "";
            this.fax = "";
            this.email = "";
            this.mobilePhone = "";
            this.role = "";
            this.isDefault = false;

            this.attn = false;
            this.modified = DateTime.Now;
            this.modifiedby = 0;

            this.address1 = "";
            this.address2 = "";
            this.address3 = "";
            this.address4 = "";
            this.emailstatement = false;

            this.emailReceipt = 2;          //MSarza - added line 
                                            // values: 0-Never; 1-Alway; 2-Inherit from Client's setting
        }

        public CustomerContact(int contactid, int clientId, int clientNumber, string clientName, int customerid, 
            int customerNumber, string customerName,string lastName, string firstName, string phone, string fax, string email, 
            string mobilePhone, string strRole, Boolean isDefaultContact, Boolean attention, DateTime? modif, Int16 modifby,
            string addr1, string addr2, string addr3, string addr4, Boolean eStatement
             , Int16 emailReceipt  //MSarza
            )
        {
            this.contactId = contactid;
            this.clientId = clientId;
            this.clientNumber = clientNumber;
            this.clientName = clientName;
            this.customerId = customerid;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.lastName = lastName;
            this.firstName = firstName;
            this.phone = phone;
            this.fax = fax;
            this.email = email;
            this.mobilePhone = mobilePhone;
            this.role = strRole;
            this.isDefault = isDefaultContact;

            this.attn = attention;
            this.modified = modif;
            this.modifiedby = modifby;

            this.address1 = addr1;
            this.address2 = addr2;
            this.address3 = addr3;
            this.address4 = addr4;
            this.emailstatement = eStatement;

            this.emailReceipt = emailReceipt;       //MSarza -- added line

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

        public CustomerContact(int contactId, int clientId, int clientNumber, string clientName, 
            int customerId, int customerNumber, string customerName, string lastName, string firstName, string phone,
            string fax, string email, string mobilePhone, Boolean estatement
            , Int16 emailReceipt       //MSarza - added line
        )
        {
            this.contactId = contactId;
            this.clientId = clientId;
            this.clientNumber = clientNumber;
            this.clientName = clientName;
            this.customerId = customerId;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.lastName = lastName;
            this.firstName = firstName;
            this.phone = phone;
            this.fax = fax;
            this.email = email;
            this.mobilePhone = mobilePhone;
            this.emailstatement = estatement;

            this.emailReceipt = emailReceipt;       //MSarza - added line

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

        public int ClientId
        {
            get { return clientId; }
        }

        public int CustomerId
        {
            get { return customerId; }
            set { this.customerId = value; }
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

        public int ContactId
        {
            get { return contactId; }
            set { this.contactId = value; }
        }

        public string MobilePhone
        {
            get { return mobilePhone; }
            set { this.mobilePhone = value; }
        }

        public string FullName
        {
            get { return fullName; }
            set { this.fullName = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { this.phone = value; }
        }

        public string Fax
        {
            get { return fax; }
            set { this.fax= value; }
        }

        public string Email
        {
            get { return email; }
            set { this.email = value; }
        }

        public string CustomerName
        {
            get { return customerName; }
        }

        public int CustomerNumber
        {
            get { return customerNumber; }
        }

        public int ClientNumber
        {
            get { return clientNumber; }
        }

        public string ClientName
        {
            get { return clientName; }
        }

        public string Role
        {
            get { return role; }
            set { this.role = value; }
        }

        public Boolean IsDefault
        {
            get { return isDefault; }
            set { this.isDefault = value; }
        }

        public Boolean Attn
        {
            get { return attn; }
            set { this.attn= value; }
        }

        public DateTime? Modified
        {
            get { return modified; }
            set { this.modified = value; }
        }

        public Int16 ModifiedBy
        {
            get { return modifiedby; }
            set { this.modifiedby = value; }
        }

        public string Address1
        {
            get { return this.address1; }
            set { this.address1 = value; }
        }

        public string Address2
        {
            get { return this.address2; }
            set { this.address2 = value; }
        }

        public string Address3
        {
            get { return this.address3; }
            set { this.address3 = value; }
        }

        public string Address4
        {
            get { return this.address4; }
            set { this.address4 = value; }
        }

        public Boolean EmailStatement
        {
            get { return this.emailstatement;  }
            set { this.emailstatement = value; }
        }

        //MSarza - inserted code block
        public Int16 EmailReceipt
        {
            get { return this.emailReceipt; }
            set { this.emailReceipt = value; }
        }
     }
}