using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cff.SaferTrader.Core.Reports;

namespace Cff.SaferTrader.Core.Letters
{
    [Serializable]
    public class LetterDetails
    {
        private int iYrMth;
        private int userID;
        private string  letterName;
        private string userSignature;

        private Date    created;
        private DateTime dateStamp;
        private DateTime dateAsAt;

        private int clientId;
        private string clientName;

        private string attnHeader = "";
        private string dearHeader = "";

        private string clientTitle = "";
        private string clientFax = "";
        private string clientPhone = "";
        private string clientCell = "";
        private string clientFName = "";
        private string clientLName = "";
        private string clientAddress1 = "";
        private string clientAddress2 = "";
        private string clientAddress3 = "";
        private string clientAddress4 = "";
        private string collectionsBankAccount;

        private int custId;
        private string custName;
        private int custNumber;
        private int custContactId;

        private string custContactName;
        private string custContactFName;
        private string custContactLName;
        private string custContactPhone;
        private string custContactFax;
        private string custContactCell;
        private string custContactEmail;

        private string customerAddress1;
        private string customerAddress2;
        private string customerAddress3;
        private string customerAddress4;
        private string emailStatmentsAddr;

        private DateTime custNotifyDate;

        private decimal month1;
        private decimal month2;
        private decimal month3;
        private decimal current;
        private decimal balance;

        //MSarza [20150731]: New letter templates data; used only for interop templates - delete when no longer supported
        private string mgtPhone; 
        private string mgtFax;
        private string mgtEmail;
        private string mgtWeb;
        private string mgtPhysicalAddr;
        private string mgtPostalAddr;
        private string mgtPostalAddr1;
        private string mgtPostalAddr2;
        private string mgtPostalAddr3;
        private string mgtPostalAddr4;

        private IList<StatementReportRecord> statReportList;

        public System.Data.DataTable CurrentNotes;

        public LetterDetails()
        { }

        public LetterDetails(IList<StatementReportRecord> parList)
        {
            this.statReportList = parList;
        }

        public IList<StatementReportRecord> StatementReportRecords
        {
            get { return this.statReportList; }
        }
    
        public int UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }

        public string LetterName
        {
            get { return this.letterName; }
            set { this.letterName = value; }
        }

        public string UserSignature
        {
            get { return this.userSignature; }
            set { this.userSignature = value; }
        }

        public Date DateCreated
        {
            get { return this.created; }
            set { this.created = value; }
        }

        public DateTime DateStamp
        {
            get { return this.dateStamp; }
            set { this.dateStamp = value; }        
        }

        public int YrMth
        {
            get { return this.iYrMth; }
            set { this.iYrMth = value; }
        }

        public decimal Month1
        {
            get { return this.month1; }
            set { this.month1 = value; }
        }

        public decimal Month2
        {
            get { return this.month2; }
            set { this.month2 = value; }
        }

        public decimal Month3
        {
            get { return this.month3; }
            set { this.month3 = value; }
        }

        public decimal Current
        {
            get { return this.current; }
            set { this.current = value; }
        }

        public decimal Balance
        {
            get { return this.balance; }
            set { this.balance = value; }
        }

        public string AttnHeader
        {
            get { return this.attnHeader; }
            set { this.attnHeader = value; }
        }

        public string DearHeader
        {
            get { return this.dearHeader; }
            set { this.dearHeader = value; }
        }

        public int CustId
        {
            get { return this.custId; }
            set { this.custId = value; }
        }

        public int CustContactID
        {
            get { return this.custContactId; }
            set { this.custContactId = value; }
        }

        public int CustNumber
        {
            get { return this.custNumber; }
            set { this.custNumber = value; }
        }

        public string CustName
        {
            get { return this.custName; }
            set { this.custName = value; }
        }

        //client details
        public int ClientId
        {
            get { return this.clientId; }
            set { this.clientId = value; }
        }

        public string ClientTitle
        {
            get {
                if (this.clientTitle == null)
                    return "";
                else
                    return this.clientTitle; 
            }
            set { this.clientTitle = value; }
        }

        public string ClientName
        {
            get { return this.clientName; }
            set { this.clientName = value; }
        }

        public string ClientAddress1
        {
            get { return this.clientAddress1; }
            set { this.clientAddress1 = value; }
        }

        public string ClientAddress2
        {
            get { return this.clientAddress2; }
            set { this.clientAddress2 = value; }        
        }

        public string ClientAddress3
        {
            get { return this.clientAddress3; }
            set { this.clientAddress3 = value; }        
        }

        public string ClientAddress4
        {
            get { return this.clientAddress4; }
            set { this.clientAddress4 = value; }
        }

        public string ClientFax
        {
            get { return this.clientFax ; }
            set { this.clientFax = value; }        
        }

        public string ClientPhone
        {
            get { return this.clientPhone; }
            set { this.clientPhone = value; }                
        }

        public string ClientCell
        {
            get { return this.clientCell ; }
            set { this.clientCell = value; }        
        }

        public string ClientFName
        {
            get {
                if (this.clientFName == null)
                    return "";
                else
                    return this.clientFName; 
            }
            set { this.clientFName = value; }
            //get { return this.clientFName; }
            //set { this.clientFName = value; }        
        }

        public string ClientLName
        {
            get { return this.clientLName; }
            set { this.clientLName = value; }                
        }

        public string CollectionsBankAccount
        {
            get { return this.collectionsBankAccount; }
            set { this.collectionsBankAccount = value; }
        }

        
        //customer details
        public string CustomerAddress1
        {
            get { return this.customerAddress1; }
            set { this.customerAddress1 = value; }
        }

        public string CustomerAddress2
        {
            get { return this.customerAddress2; }
            set { this.customerAddress2 = value; }
        }

        public string CustomerAddress3
        {
            get { return this.customerAddress3; }
            set { this.customerAddress3 = value; }
        }

        public string CustomerAddress4
        {
            get { return this.customerAddress4; }
            set { this.customerAddress4 = value; }
        }

        public DateTime CustNotifyDate
        {
            get { return this.custNotifyDate; }
            set { this.custNotifyDate = value; }
        }

        public string CustContactName
        {
            get { return this.custContactName; }
            set { this.custContactName = value; }
        }

        public string CustContactFName 
        {
            get { return this.custContactFName; }
            set { this.custContactFName = value; }
        }

        public string CustContactLName
        {
            get { return this.custContactLName; }
            set { this.custContactLName = value; }
        }

        public string CustContactPhone 
        {
            get { return this.custContactPhone; }
            set { this.custContactPhone = value; }
        }

        public string CustContactFax 
        {
            get { return this.custContactFax; }
            set { this.custContactFax = value; }
        }

        public string CustContactCell 
        {
            get { return this.custContactCell; }
            set { this.custContactCell = value; }
        }

        public string CustContactEmail 
        {
            get { return this.custContactEmail; }
            set { this.custContactEmail = value; }
        }

        public string EmailStatmentsAddr 
        {
            get { return this.emailStatmentsAddr; }
            set { this.emailStatmentsAddr = value; }
        }

        
        public DateTime DateAsAt
        {
            get { return this.dateAsAt; }
            set { this.dateAsAt = value; }
        }


        //MSarza [20150731]: New letter templates data
        //CffManagement Details
        public string MgtPhone
        {
            get { return this.mgtPhone; }
            set { this.mgtPhone = value; }
        }
        public string MgtFax
        {
            get { return this.mgtFax; }
            set { this.mgtFax = value; }
        }
        public string MgtEmail
        {
            get { return this.mgtEmail; }
            set { this.mgtEmail = value; }
        }
        public string MgtWeb
        {
            get { return this.mgtWeb; }
            set { this.mgtWeb = value; }
        }

        public string MgtPhysicalAddr
        {
            get { return this.mgtPhysicalAddr; }
            set { this.mgtPhysicalAddr = value; }
        }
        
        public string MgtPostalAddr
        {
            get { return this.mgtPostalAddr; }
            set { this.mgtPostalAddr = value; }
        }

        //MSarza[20150824] - added to support IOutpoutOXML
        public System.Data.DataTable Transactions;

        //MSarza [20150810]: New letter templates data
        public string CffLegalEntity { get; set; }                  // Cff's legal entitites as defined
        public string CffIncLegalEntity { get; set; }               // Either empty string or Cff legal entity prefixed by "Incorporating"
        public string UserLegalEntity { get; set; }                 // should depend on logged in user 
        public string ClientSignature { get; set; }
        public string OnBehalf { get; set; }
        public int ClientNumber { get; set; }
        public string UserEntityLH { get; set; }

        
        //MSarza [20151104]: To support line spaces
        private int lineSpaces1 = 0;
        private int lineSpaces2 = 0;
        private int lineSpaces3 = 0;
        private int lineSpaces4 = 0;

        public int LineSpaces1 
        {
            get { return this.lineSpaces1; }
            set { this.lineSpaces1 = value; }
        }
        public int LineSpaces2 
        {
            get { return this.lineSpaces2; }
            set { this.lineSpaces2 = value; }
        }
        public int LineSpaces3 
        {
            get { return this.lineSpaces3; }
            set { this.lineSpaces3 = value; }
        }
        public int LineSpaces4
        {
            get { return this.lineSpaces4; }
            set { this.lineSpaces4 = value; }
        }

        //MSarza [20151102]: To support statement reports
        private string stmtHdrL1a = "";
        private string stmtHdrL1b = "";
        private string stmtHdrL1c = "";
        private string stmtPeriodV1 = "";    
        private string stmtHdrL2a = "";
        private string stmtHdrL2b = "";
        private string estNumPages = "";
        private string emailCopySentTo = "";
        private string footerStatement1 = "";

        public string StmtHdrL1a
        {
            get { return this.stmtHdrL1a; }
            set { this.stmtHdrL1a = value; }
        }
        public string StmtHdrL1b
        {
            get { return this.stmtHdrL1b; }
            set { this.stmtHdrL1b = value; }
        }
        public string StmtHdrL1c
        {
            get { return this.stmtHdrL1c; }
            set { this.stmtHdrL1c = value; }
        }
        public string StmtPeriodV1
        {
            get { return this.stmtPeriodV1; }
            set { this.stmtPeriodV1 = value; }
        }
        public string StmtHdrL2a
        {
            get { return this.stmtHdrL2a; }
            set { this.stmtHdrL2a = value; }
        }
        public string StmtHdrL2b
        {
            get { return this.stmtHdrL2b; }
            set { this.stmtHdrL2b = value; }
        }
        public string EstNumPages
        {
            get { return this.estNumPages; }
            set { this.estNumPages = value; }
        }
        public string EmailCopySentTo
        {
            get { return this.emailCopySentTo; }
            set { this.emailCopySentTo = value; }
        }
        public string FooterStatement1
        {
            get { return this.footerStatement1; }
            set { this.footerStatement1 = value; }
        }   
   
        //MSarza [20151023]: New letter templates data
        private string userPhone = "Null or not set";
        private string qlfdUserPhoneV1 = "";
        private string qlfdUserPhoneV2 = "";
        private string qlfdUserPhoneV3 = "";
        //private string qlfdUserPhoneV4 = "";
        //private string qlfdUserPhoneV5 = "";
        //private string qlfdUserPhoneV6 = "";

        private string userMobile = "Null or not set";
        private string qlfdUserMobileV1 = "";
        private string qlfdUserMobileV2 = "";
        private string qlfdUserMobileV3 = "";
        //private string qlfdUserMobileV4 = "";
        //private string qlfdUserMobileV5 = "";
        //private string qlfdUserMobileV6 = "";

        private string userFax = "Null or not set";
        private string qlfdUserFaxV1 = "";
        private string qlfdUserFaxV2 = "";
        private string qlfdUserFaxV3 = "";
        //private string qlfdUserFaxV4 = "";
        //private string qlfdUserFaxV5 = "";
        //private string qlfdUserFaxV6 = "";

        private string userEmail = "Null or not set";
        private string qlfdUserEmailV1 = "";
        private string qlfdUserEmailV2 = "";
        private string qlfdUserEmailV3 = "";
        //private string qlfdUserEmailV4 = "";
        //private string qlfdUserEmailV5 = "";
        //private string qlfdUserEmailV6 = "";

        private string userWeb = "Null or not set";
        private string qlfdUserWebV1 = "";
        private string qlfdUserWebV2 = "";
        private string qlfdUserWebV3 = "";
        private string qlfdUserWebV4 = "";
        private string qlfdUserWebV5 = "";
        //private string qlfdUserWebV6 = "";
        //private string qlfdUserWebV7 = "";
        //private string qlfdUserWebV8 = "";

        private string userPhysicalAddr = "Null or not set";
        private string qlfdUserPhysicalAddrV1 = "";
        private string qlfdUserPhysicalAddrV2 = "";
        private string qlfdUserPhysicalAddrV3 = "";
        private string qlfdUserPhysicalAddrV4 = "";
        private string qlfdUserPhysicalAddrV5 = "";
        private string qlfdUserPhysicalAddrV6 = "";
        //private string qlfdUserPhysicalAddrV7 = "";
        //private string qlfdUserPhysicalAddrV8 = "";
        //private string qlfdUserPhysicalAddrV9 = "";
        //private string qlfdUserPhysicalAddrV10 = "";
        //private string qlfdUserPhysicalAddrV11 = "";
        //private string qlfdUserPhysicalAddrV12 = "";
        //private string qlfdUserPhysicalAddrV13 = "";

        private string userPostalAddr = "Null or not set";
        private string qlfdUserPostalAddrV1 = "";
        private string qlfdUserPostalAddrV2 = "";
        private string qlfdUserPostalAddrV3 = "";
        private string qlfdUserPostalAddrV4 = "";
        private string qlfdUserPostalAddrV5 = "";
        private string qlfdUserPostalAddrV6 = "";
        //private string qlfdUserPostalAddrV7 = "";
        //private string qlfdUserPostalAddrV8 = "";
        //private string qlfdUserPostalAddrV9 = "";
        //private string qlfdUserPostalAddrV10 = "";
        //private string qlfdUserPostalAddrV11 = "";
        //private string qlfdUserPostalAddrV12 = "";
        //private string qlfdUserPostalAddrV13 = "";

        public string UserPhone 
        { 
            get { return this.userPhone; }
            set
            {
                this.userPhone = value;
                if (!((value == "Null or not set") | (value == "")))
                {
                    // if value is not "Null or not set" or "", supply the following:
                    this.qlfdUserPhoneV1 = "P: " + value;
                    this.qlfdUserPhoneV2 = "; P: " + value;
                        // word or open xml does not persist leading and (sometimes) lagging whitespace
                        // \u2004 is an xml escape sequence to insert moderate non breaking space space
                        //      range for these escape sequence is u2001 - u2009
                        // \n\b is an xml escape sequence to insert a carriage return after the current line,
                        //      hence, even when placed at the front, the new line will be inserted at 
                        //      the end of the current line, e.g.,  "P: " + value + "\n\b"  is same
                        //      as "\n\bP: " + value;
                    this.qlfdUserPhoneV3 = "\u2004|\u2004P: " + value;  
                    //this.qlfdUserPhoneV4 = "\n\bP: " + value;
                    //this.qlfdUserPhoneV5 = "\n\b; P: " + value;
                    //this.qlfdUserPhoneV6 = "\n\b | P:" + value;
                }
            }
        }

        public string UserMobile 
        { 
            get { return this.userMobile; }
            set
            {
                this.userMobile = value;
                if (!((value == "Null or not set") | (value == "")))
                {
                    // if value is not "Null or not set" or "", supply the following:
                    this.qlfdUserMobileV1 = "M: " + value;
                    this.qlfdUserMobileV2 = "; M: " + value;
                    this.qlfdUserMobileV3 = "\u2004|\u2004M: " + value;
                    //this.qlfdUserMobileV4 = "\n\bM: " + value;
                    //this.qlfdUserMobileV5 = "\n\b; M: " + value;
                    //this.qlfdUserMobileV6 = "\n\b | M:" + value;
                }
            }
        }

        public string UserFax
        {
            get { return this.userFax; }
            set
            {
                this.userFax = value;
                if (!((value == "Null or not set") | (value == "")))
                {
                    // if value is not "Null or not set" or "", supply the following:
                    this.qlfdUserFaxV1 = "F: " + value;
                    this.qlfdUserFaxV2 = "; F: " + value;
                    this.qlfdUserFaxV3 = "\u2004|\u2004F:" + value; 
                    //this.qlfdUserFaxV4 = "\n\bF: " + value;
                    //this.qlfdUserFaxV5 = "\n\b; F: " + value;
                    //this.qlfdUserFaxV6 = "\n\b | F:" + value;
                }
            }
        }

        public string UserEmail
        { 
            get { return this.userEmail; }
            set
            {
                this.userEmail = value;
                if (!((value == "Null or not set") | (value == "")))
                {
                    // if value is not "Null or not set" or "", supply the following:
                    this.qlfdUserEmailV1 = "E: " + value;
                    this.qlfdUserEmailV2 = "; E: " + value;
                    this.qlfdUserEmailV3 = "\u2004|\u2004E: " + value;
                    //this.qlfdUserEmailV4 = "\n\bE: " + value;
                    //this.qlfdUserEmailV5 = "\n\b; E: " + value;
                    //this.qlfdUserEmailV6 = "\n\b | E:" + value;
                }
            }
        }

        public string UserWeb
        { 
            get { return this.userWeb; }
            set
            {
                this.userWeb = value;
                if (!((value == "Null or not set") | (value == "")))
                {
                    // if value is not "Null or not set" or "", supply the following:
                    this.qlfdUserWebV1 = "; " + value;
                    this.qlfdUserWebV2 = "\u2004|\u2004" + value;
                    this.qlfdUserWebV3 = "W: " + value;
                    this.qlfdUserWebV4 = "; W: " + value;
                    this.qlfdUserWebV5 = "\u2004|\u2004W: " + value;
                    //this.qlfdUserWebV6 = "\n\bW: " + value;
                    //this.qlfdUserWebV7 = "\n\b; W: " + value;
                    //this.qlfdUserWebV8 = "\n\b | W:" + value;
                }
            }
        }
        public string UserPhysicalAddr
        {
            get { return this.userPhysicalAddr; }
            set
            {
                this.userPhysicalAddr = value;
                if (!((value == "Null or not set") | (value == "")))
                {
                    // if value is not "Null or not set" or "", supply the following:   this.qlfdUserPhysicalAddrV1 = "; " + value;
                    this.qlfdUserPhysicalAddrV2 = " | " + value;
                                 
                    this.qlfdUserPhysicalAddrV3 = "Main: " + value;
                    this.qlfdUserPhysicalAddrV4 = "\u2004Main: " + value;
                    this.qlfdUserPhysicalAddrV5 = "; Main: " + value;
                    this.qlfdUserPhysicalAddrV6 = "\u2004|\u2004Main: " + value;
                                 
                    //this.qlfdUserPhysicalAddrV7 = "\n\b" + value;
                    //this.qlfdUserPhysicalAddrV8 = "\n\b; " + value;
                    //this.qlfdUserPhysicalAddrV9 = "\n\b | " + value;
                                 
                    //this.qlfdUserPhysicalAddrV10 = "\n\bMain: " + value;
                    //this.qlfdUserPhysicalAddrV11 = "\n\b Main: " + value;
                    //this.qlfdUserPhysicalAddrV12 = "\n\b; Main: " + value;
                    //this.qlfdUserPhysicalAddrV13 = "\n\b | Main: " + value;
                }
            }
        }
        public string UserPostalAddr
        {
            get { return this.userPostalAddr; }
            set
            {
                this.userPostalAddr = value;
                if (!((value == "Null or not set") | (value == "")))
                {
                    // if value is not "Null or not set" or "", supply the following:
                    this.qlfdUserPostalAddrV1 = "; " + value;
                    this.qlfdUserPostalAddrV2 = " | " + value;

                    this.qlfdUserPostalAddrV3 = "Postal: " + value;
                    this.qlfdUserPostalAddrV4 = "\u2004Postal: " + value;
                    this.qlfdUserPostalAddrV5 = "; Postal: " + value;
                    this.qlfdUserPostalAddrV6 = "\u2004|\u2004Postal: " + value;

                    //this.qlfdUserPostalAddrV7 = "\n\b" + value;
                    //this.qlfdUserPostalAddrV8 = "\n\b; " + value;
                    //this.qlfdUserPostalAddrV9 = "\n\b | " + value;

                    //this.qlfdUserPostalAddrV10 = "\n\bPostal: " + value;
                    //this.qlfdUserPostalAddrV11 = "\n\b Postal: " + value;
                    //this.qlfdUserPostalAddrV12 = "\n\b; Postal: " + value;
                    //this.qlfdUserPostalAddrV13 = "\n\b | Postal: " + value;
                }
            }
        }
        public string MgtPostalAddr1
        {
            get { return this.mgtPostalAddr1; }
            set { this.mgtPostalAddr1 = value; }
        }
        public string MgtPostalAddr2
        {
            get { return this.mgtPostalAddr2; }
            set { this.mgtPostalAddr2 = value; }
        }
        public string MgtPostalAddr3
        {
            get { return this.mgtPostalAddr3; }
            set { this.mgtPostalAddr3 = value; }
        }
        public string MgtPostalAddr4
        {
            get { return this.mgtPostalAddr4; }
            set { this.mgtPostalAddr4 = value; }
        }

        private string date_now = DateTime.Now.Day.ToString() + " " +  //DateTime.Now.ToString("dd") + " " +
                                    DateTime.Now.ToString("MMMM") + " " +
                                    DateTime.Now.Year.ToString();
        

        // MSarza [20160615]
        private string oneLineClientAddressFormat()
        {
            string cltAddressLine;
            cltAddressLine = "";
            //string[] cltAddressLines = new string[] { this.clientAddress1, this.clientAddress2, this.clientAddress3, this.clientAddress4 };
            string[] cltAddressLines = new string[4];
            cltAddressLines[0] = this.clientAddress1;
            cltAddressLines[1] = this.clientAddress2;
            cltAddressLines[2] = this.clientAddress3;
            cltAddressLines[3] = this.clientAddress4;

            int x; 
            for (int i = 0; i < 3; i++)
            {
                x = 0;
                if (cltAddressLines[i].Length > 0)
                {
                    for (int j = i + 1; j < 4; j++)
                    {
                        x += cltAddressLines[j].Length;
                    }
                    if (x > 0)
                        cltAddressLine += cltAddressLines[i] + ", ";
                    else
                        cltAddressLine += cltAddressLines[i];
                }
            }

            return cltAddressLine;
        }
        
        //MSarza [201901]: Oxml merge helper
        public string GetPropertyValues(string PropertyName)
        {
            switch (PropertyName.ToLower()) //these are the mail merge property or document property reference name on the templates
            {
                case "date_now":                return this.date_now; // DateTime.Now.ToShortDateString();
                case "userid":                  return this.userID.ToString();
                case "lettername":              return this.letterName;
                
                case "datecreated":             return this.DateCreated.ToString();
                case "datestamp":               return this.dateStamp.ToString();
                case "dateasat":                return this.dateAsAt.ToString();
                case "yrmth":                   return this.YrMth.ToString();
                case "attnheader":              return this.attnHeader;
                case "attnhdr":                 return this.attnHeader;
                case "dearheader":              return this.dearHeader;
                case "dearhdr":                 return this.dearHeader;
                case "emailstatmentsaddr":      return this.emailStatmentsAddr;

                case "clientid":                return this.clientId.ToString();
                case "clienttitle":             return this.clientTitle;
                case "clientname":              return this.clientName;
                case "clientaddress1":          return this.clientAddress1;
                case "clientaddress2":          return this.clientAddress2;
                case "clientaddress3":          return this.clientAddress3;
                case "clientaddress4":          return this.clientAddress4;

                // 1 line client address format
                case "clientqlfdaddr1v1":       return oneLineClientAddressFormat();

                case "clientfax":               return this.clientFax;
                case "clientphone":             return this.clientPhone;
                case "clientcell":              return this.clientCell;
                case "cell":                    return this.clientCell;
                case "clientfname":             return this.clientFName;
                case "clientlname":             return this.clientLName;
                case "clientsignature":         return this.ClientSignature;                
                case "clientnumber":            return this.ClientNumber.ToString();
                case "clientnum":               return this.ClientNumber.ToString();
                case "title_clientname":
                    {   
                        if (this.clientTitle.Length > 0)
                            return this.clientTitle + " " + this.clientName;
                        else
                            return this.clientName;
                    }
                case "clientfnameandlname":
                    {
                        if (this.clientFName.Length > 0)
                            return (this.clientFName + " " + this.clientLName);
                        else
                            return "";
                    }
                case "fname_lname": return (this.clientFName + " " + this.clientLName);

                //case "custcontactid":           return this.custContactID.ToString();
                case "custid":                  return this.custId.ToString();
                case "custnumber":              return this.custNumber.ToString();
                case "custnum":                 return this.custNumber.ToString();
                case "customernumber": return this.custNumber.ToString();
                case "custname":                return this.custName;
                case "customername":            return this.custName;
                case "custnotifydate":          return this.custNotifyDate.ToString();
                case "datenotified":            return this.custNotifyDate.ToString("dd MMMM yyyy");
                case "custcontact":             return this.custContactName;
                case "custcontactname":         return this.custContactName;                
                case "custcontactfname":        return this.custContactFName;
                case "custcontactlname":        return this.custContactLName;
                case "custcontactphone":        return this.custContactPhone;
                case "custphone":               return this.custContactPhone;
                case "custcontactfax":          return this.custContactFax;
                case "custfax":                 return this.custContactFax;
                case "custcontactcell":         return this.custContactCell;
                case "custcell":                return this.custContactCell;
                case "custcontactemail":        return this.custContactEmail;
                case "custemail":               return this.custContactEmail;

                case "customeraddress1":        return this.customerAddress1;
                case "customeraddress2":        return this.customerAddress2;
                case "customeraddress3":        return this.customerAddress3;
                case "customeraddress4":        return this.customerAddress4;
                case "custaddress1":            return this.customerAddress1;
                case "custaddress2":            return this.customerAddress2;
                case "custaddress3":            return this.customerAddress3;
                case "custaddress4":            return this.customerAddress4;
                case "address1":                return this.customerAddress1;
                case "address2":                return this.customerAddress2;
                case "address3":                return this.customerAddress3;
                case "address4":                return this.customerAddress4;
                case "custqlfdaddr1v1":
                        if ((this.customerAddress2.Length
                                + this.customerAddress3.Length
                                + this.customerAddress4.Length) > 1)
                            return "\n\b" + this.customerAddress1;
                        else return this.customerAddress1;
                case "custqlfdaddr2v1":
                        if ((this.customerAddress3.Length
                                + this.customerAddress4.Length) > 1)
                            return "\n\b" + this.customerAddress2;
                        else return this.customerAddress2;
                case "custqlfdaddr3v1":
                        if (this.customerAddress4.Length > 1)
                            return "\n\b" + this.customerAddress3;
                        else return this.customerAddress3;
                case "custqlfdaddr4v1":         return this.customerAddress4;

                // 2 line customer address format
                case "custqlfdaddr1v2":
                    if (this.customerAddress1.Length > 1)
                        if (this.customerAddress2.Length > 1)
                            return this.customerAddress1 + ", " + this.customerAddress2;
                        else return this.customerAddress1;
                    else return this.customerAddress2;
                case "custqlfdaddr2v2":
                    if (this.customerAddress3.Length > 1)
                        if (this.customerAddress4.Length > 1)
                            return this.customerAddress3 + ", " + this.customerAddress4;
                        else return this.customerAddress3;
                    else return this.customerAddress4;

                case "cffinclegalentity":       return this.CffIncLegalEntity;                  
                case "legalentity":             return this.CffLegalEntity;                     
                case "mgtphone":                return this.mgtPhone;
                case "mgtfax":                  return this.mgtFax;
                case "mgtemail":                return this.mgtEmail;
                case "mgtweb":                  return this.mgtWeb;
                case "mgtphysicaladdr":         return this.mgtPhysicalAddr;
                case "mgtpostaladdr":           return this.mgtPostalAddr;
                case "mgtpostaladdr1":          return this.mgtPostalAddr1;
                case "mgtpostaladdr2":          return this.mgtPostalAddr2;
                case "mgtpostaladdr3":          return this.mgtPostalAddr3;
                case "mgtpostaladdr4":          return this.mgtPostalAddr4;

                case "usersignature":           return this.userSignature;
                case "onbehalf":                return this.OnBehalf;
                case "userentity":              return this.UserLegalEntity;
                case "userentitylh":            return this.UserEntityLH;

                case "userphone":               return this.userPhone;
                case "qlfduserphonev1":         return this.qlfdUserPhoneV1;
                case "qlfduserphonev2":         return this.qlfdUserPhoneV2;
                case "qlfduserphonev3":         return this.qlfdUserPhoneV3;
                //case "qlfduserphonev4":         return this.qlfdUserPhoneV4;
                //case "qlfduserphonev5":         return this.qlfdUserPhoneV5;
                //case "qlfduserphonev6":         return this.qlfdUserPhoneV6;

                case "userfax":                 return this.userFax;
                case "qlfduserfaxv1":           return this.qlfdUserFaxV1;
                case "qlfduserfaxv2":           return this.qlfdUserFaxV2;
                case "qlfduserfaxv3":           return this.qlfdUserFaxV3;
                //case "qlfduserfaxv4":           return this.qlfdUserFaxV4;
                //case "qlfduserfaxv5":           return this.qlfdUserFaxV5;
                //case "qlfduserfaxv6":           return this.qlfdUserFaxV6;

                case "useremail":               return this.userEmail;
                case "qlfduseremailv1":         return this.qlfdUserEmailV1;
                case "qlfduseremailv2":         return this.qlfdUserEmailV2;
                case "qlfduseremailv3":         return this.qlfdUserEmailV3;
                //case "qlfduseremailv4":         return this.qlfdUserEmailV4;
                //case "qlfduseremailv5":         return this.qlfdUserEmailV5;
                //case "qlfduseremailv6":         return this.qlfdUserEmailV6;
                
                case "userweb":                 return this.userWeb;
                case "qlfduserwebv1":           return this.qlfdUserWebV1;
                case "qlfduserwebv2":           return this.qlfdUserWebV2;
                case "qlfduserwebv3":           return this.qlfdUserWebV3;
                case "qlfduserwebv4":           return this.qlfdUserWebV4;
                case "qlfduserwebv5":           return this.qlfdUserWebV5;
                //case "qlfduserwebv6":           return this.qlfdUserWebV6;
                //case "qlfduserwebv7":           return this.qlfdUserWebV6;
                //case "qlfduserwebv8":           return this.qlfdUserWebV6;

                case "usermobile":              return this.userMobile;
                case "qlfdusermobilev1":        return this.qlfdUserMobileV1;
                case "qlfdusermobilev2":        return this.qlfdUserMobileV2;
                case "qlfdusermobilev3":        return this.qlfdUserMobileV3;
                //case "qlfdusermobilev4":        return this.qlfdUserMobileV4;
                //case "qlfdusermobilev5":        return this.qlfdUserMobileV5;
                //case "qlfdusermobilev6":        return this.qlfdUserMobileV6;

                case "userphysicaladdr":        return this.userPhysicalAddr;
                case "qlfduserphysicaladdrv1":  return this.qlfdUserPhysicalAddrV1;
                case "qlfduserphysicaladdrv2":  return this.qlfdUserPhysicalAddrV2;
                case "qlfduserphysicaladdrv3":  return this.qlfdUserPhysicalAddrV3;
                case "qlfduserphysicaladdrv4":  return this.qlfdUserPhysicalAddrV4;
                case "qlfduserphysicaladdrv5":  return this.qlfdUserPhysicalAddrV5;
                case "qlfduserphysicaladdrv6":  return this.qlfdUserPhysicalAddrV6;
                //case "qlfduserphysicaladdrv7":  return this.qlfdUserPhysicalAddrV7;
                //case "qlfduserphysicaladdrv8":  return this.qlfdUserPhysicalAddrV8;
                //case "qlfduserphysicaladdrv9":  return this.qlfdUserPhysicalAddrV9;
                //case "qlfduserphysicaladdrv10": return this.qlfdUserPhysicalAddrV10;
                //case "qlfduserphysicaladdrv11": return this.qlfdUserPhysicalAddrV11;
                //case "qlfduserphysicaladdrv12": return this.qlfdUserPhysicalAddrV12;
                //case "qlfduserphysicaladdrv13": return this.qlfdUserPhysicalAddrV13;

                case "qlfduserphysicaladdrv21":
                    if (!((this.userPostalAddr == "Null or not set") | (this.userPostalAddr == "")))
                        return "\n\b" + this.userPhysicalAddr;
                    else return this.userPhysicalAddr;


                case "userpostaladdr":          return this.userPostalAddr;
                case "qlfduserpostaladdrv1":    return this.qlfdUserPostalAddrV1;
                case "qlfduserpostaladdrv2":    return this.qlfdUserPostalAddrV2;
                case "qlfduserpostaladdrv3":    return this.qlfdUserPostalAddrV3;
                case "qlfduserpostaladdrv4":    return this.qlfdUserPostalAddrV4;
                case "qlfduserpostaladdrv5":    return this.qlfdUserPostalAddrV5;
                case "qlfduserpostaladdrv6":    return this.qlfdUserPostalAddrV6;
                //case "qlfduserpostaladdrv7": return this.qlfdUserPostalAddrV7;
                //case "qlfduserpostaladdrv8": return this.qlfdUserPostalAddrV8;
                //case "qlfduserpostaladdrv9": return this.qlfdUserPostalAddrV9;
                //case "qlfduserpostaladdrv10": return this.qlfdUserPostalAddrV10;
                //case "qlfduserpostaladdrv11": return this.qlfdUserPostalAddrV11;
                //case "qlfduserpostaladdrv12": return this.qlfdUserPostalAddrV12;
                //case "qlfduserpostaladdrv13": return this.qlfdUserPostalAddrV13;

                ////case "accountwith"       return this.AccountWith; //if Cff: Your account with ClientName; else ""

                case "monthend":                return this.dateAsAt.ToShortDateString();
                case "collectionsbankaccount":  return this.collectionsBankAccount;
                case "month1":                  //return this.month1.ToString();
                    {
                        if (this.month1 == 0)
                            return ("$0.00");
                        else
                            return (String.Format("{0:c}", this.month1));
                    }
                case "mth1":
                    {
                        if (this.month1 == 0)
                            return ("$0.00");
                        else
                            return (String.Format("{0:c}", this.month1));
                    }
                case "month2":                  //return this.month2.ToString();
                    {
                        if (this.month2 == 0)
                            return ("$0.00");
                        else
                            return (String.Format("{0:c}", this.month2));
                    }
                case "mth2":
                    {
                        if (this.month2 == 0)
                            return ("$0.00");
                        else
                            return (String.Format("{0:c}", this.month2));
                    }
                case "month3":                  //return this.month3.ToString();
                    {
                        if (this.month3 == 0)
                            return ("$0.00");
                        else
                            return (String.Format("{0:c}", this.month3));
                    }
                case "mth3":
                    {
                        if (this.month3 == 0)
                            return ("$0.00");
                        else
                            return (String.Format("{0:c}", this.month3));
                    }
                case "current":                 //return this.current.ToString();
                    {
                        if (this.current == 0)
                            return ("$0.00");
                        else
                            return (String.Format("{0:c}", this.current));
                    }
                case "balance":                 //return this.balance.ToString();
                    {
                        if (this.balance == 0)
                            return ("$0.00");
                        else
                            return (String.Format("{0:c}", this.balance));
                    }
                case "bal":                     //return this.balance.ToString();
                    {
                        if (this.balance == 0)
                            return ("$0.00");
                        else
                            return (String.Format("{0:c}", this.balance));
                    }

                //This is as per IOuput.cs definition and as per the commented out
                //      definition in the IOutput.vb files
                case "current_currentorodue":
                    {
                        if (DateTime.Now.Day < 20)
                            {
                            if ((this.current + this.month1) == 0)
                                return ("$0.00");
                            else
                                return (String.Format("{0:c}", (this.current + this.month1)));
                            }
                        else 
                        {
                            if (this.current == 0)
                                return ("$0.00");
                            else
                                return (String.Format("{0:c}", this.current));
                        }
                    }
                case "odue_currentorodue":
                    {
                        if (DateTime.Now.Day < 20)
                        {
                            if ((this.month2 + this.month3) == 0)
                                return ("$0.00");
                            else
                                return (String.Format("{0:c}", (this.month2 + this.month3)));
                        }
                        else
                        {
                            if ((this.month1 + this.month2 + this.month3) == 0)
                                return ("$0.00");
                            else
                                return (String.Format("{0:c}", (this.month1 + this.month2 + this.month3)));
                        }
                    }

                case "linespacer1":         return "";
                case "linespacer2":         return "";
                case "linespacer3":         return "";
                case "linespacer4":         return "";

                case "stmthdrl1a":          return this.stmtHdrL1a;
                case "stmthdrl1b":          return this.stmtHdrL1b;
                case "stmthdrl1c":          return this.stmtHdrL1c;
                case "stmthdrl2a":          return this.stmtHdrL2a;
                case "stmthdrl2b":          return this.stmtHdrL2b;
                case "stmtperiodv1":          return this.stmtPeriodV1;
                case "estnumpages":         return this.estNumPages;
                case "emailcopysentto":       return this.emailCopySentTo;
                case "footerstatement1":     return this.footerStatement1;

                default:
                    //throw new Exception(message: "FieldName (" + FieldName + ") was not found");
                    return "Error: FieldName (" + PropertyName + ") not found.";
            }

        }

        
    }
}
