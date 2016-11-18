using System;

namespace Cff.SaferTrader.Core
{
    //MSarza [20150731]
    [Serializable]
    public class CffMgtDetails
    {
        private readonly string postalAddress;
        private readonly string physicalAddress;

        public string Name { get; private set; }
        public string LegalEntity1 { get; private set; }
        public string LegalEntity2 { get; private set; }
        public string LegalEntity3 { get; private set; }
        public string Phone { get; private set; }
        public string Fax { get; private set; }
        public string Web { get; private set; }
        public string Email { get; private set; }
        public string Bank { get; private set; }
        public string Branch { get; private set; }
        public string BankAccount { get; private set; }
        public string EnquiryEmailContact { get; private set; }

        public string PostalAddress1 { get; private set; }
        public string PostalAddress2 { get; private set; }
        public string PostalAddress3 { get; private set; }
        public string PostalAddress4 { get; private set; }
        public string PhysicalAddress1 { get; private set; }
        public string PhysicalAddress2 { get; private set; }
        public string PhysicalAddress3 { get; private set; }
        public string PhysicalAddress4 { get; private set; }
        public string PhysicalAddress5 { get; private set; }

        public string PostalAddress { get; private set; }
        public string PhysicalAddress { get; private set; }

        public CffMgtDetails(string name,
                            string legalEntity1,
                            string legalEntity2,
                            string legalEntity3,
                            //string postalAddress,
                            string postalAddress1,
                            string postalAddress2,
                            string postalAddress3,
                            string postalAddress4,
                            //string physicalAddress,
                            string physicalAddress1,
                            string physicalAddress2,
                            string physicalAddress3,
                            string physicalAddress4,
                            string physicalAddress5,
                            string phone,
                            string fax,
                            string web,
                            string email,
                            string bank,
                            string branch,
                            string bankAccount,
                            string enquiryEmailContact)
        {
            this.Name = name;
            this.LegalEntity1 = legalEntity1;
            this.LegalEntity2 = legalEntity2;
            this.LegalEntity3 = legalEntity3;
            this.PostalAddress1 = postalAddress1;
            this.PostalAddress2 = postalAddress2;
            this.PostalAddress3 = postalAddress3;
            this.PostalAddress4 = postalAddress4;
            this.PhysicalAddress1 = physicalAddress1;
            this.PhysicalAddress2 = physicalAddress2;
            this.PhysicalAddress3 = physicalAddress3;
            this.PhysicalAddress4 = physicalAddress4;
            this.PhysicalAddress5 = physicalAddress5;
            this.Phone = phone;
            this.Fax = fax;
            this.Web = web;
            this.Email = email;
            this.Bank = bank;
            this.Branch = branch;
            this.BankAccount = bankAccount;
            this.EnquiryEmailContact = enquiryEmailContact;

            postalAddress = "";

            if (postalAddress1.Trim().Length > 1)
            {
                if ((postalAddress2.Trim().Length + postalAddress3.Trim().Length + postalAddress4.Trim().Length) > 0 )
                {
                    postalAddress = postalAddress1.Trim() + ", ";
                }
                else { postalAddress = postalAddress1.Trim(); }
            }

            if (postalAddress2.Trim().Length > 1)
            {
                if ((postalAddress3.Trim().Length + postalAddress4.Trim().Length) > 0)
                {
                    postalAddress += postalAddress2.Trim() + ", ";
                }
                else { postalAddress += postalAddress2.Trim(); }
            }

            if (postalAddress3.Trim().Length > 1)
            {
                if (postalAddress4.Trim().Length > 0)
                {
                    postalAddress += postalAddress3.Trim() + ", ";
                }
                else { postalAddress += postalAddress3.Trim(); }
            }
            else
            {
                if (postalAddress4.Trim().Length > 0)
                {
                    postalAddress += postalAddress4.Trim();
                }
            }


            //
            physicalAddress = "";

            if (physicalAddress1.Trim().Length > 1)
            {
                if ((physicalAddress2.Trim().Length + physicalAddress3.Trim().Length + physicalAddress4.Trim().Length + physicalAddress5.Trim().Length) > 0)
                {
                    physicalAddress = physicalAddress1.Trim() + ", ";
                }
                else { physicalAddress = physicalAddress1.Trim(); }
            }

            if (physicalAddress2.Trim().Length > 1)
            {
                if ((physicalAddress3.Trim().Length + physicalAddress4.Trim().Length + physicalAddress5.Trim().Length) > 0)
                {
                    physicalAddress += physicalAddress2.Trim() + ", ";
                }
                else { physicalAddress += physicalAddress2.Trim(); }
            }

            if (physicalAddress3.Trim().Length > 1)
            {
                if ((physicalAddress4.Trim().Length  + physicalAddress5.Trim().Length) > 0)
                {
                    physicalAddress += physicalAddress3.Trim() + ", "; 
                }
                else { physicalAddress += physicalAddress3.Trim(); }
            }

            if (physicalAddress4.Trim().Length > 1)
            {
                if (physicalAddress5.Trim().Length > 1)
                {
                    physicalAddress += physicalAddress4.Trim() + ", ";
                }
                else { physicalAddress += physicalAddress4.Trim(); }
            }

            if (physicalAddress5.Trim().Length > 1)
            {
                physicalAddress += physicalAddress5.Trim();
            }

            this.PostalAddress = postalAddress;
            this.PhysicalAddress = physicalAddress;

          
        }
    }
}
