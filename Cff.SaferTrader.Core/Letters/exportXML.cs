using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cff.SaferTrader.Core.Letters
{
    #pragma warning disable
    struct sHeader
    {
        public string uniquenum;
        public string companyName;
        public string dbaseVersion;
        public string driverBuilderNo;
        public string changeControl;
        public string isOpenItem;
    };

    struct sPayments
    {
        public string co_lastname;
        public string firstname;
        public string depositAcctNo; //note: this must exist in client's account table
        public string transactionID;
        public string receiptdate;
        public string invoicenumber;
        public string amountapplied;
        public string memo;
        public string currencycode;
        public string exchangerate;
        public string paymentmethod;
        public string paymentnotes;
        public string nameoncard;
        public string cardnumber;
        public string expirydate;
        public string authcode;
        public string bankandbranchl;
        public string accountnumber;
        public string accountname;
        public string checknumber;
        public string cardidentification;
        public string cardrecordid;
        public string saleID;
    };

    struct sCredits
    {
        public string typeid;
        public string co_lastname;
        public string firstname;
        public string depositAcctNo; //note: this must exist in client's account table
        public string transactionID;
        public string receiptdate;
        public string saleid;
        public string invoicenumber;
        public string creditamount;
        public string memo;
        public string currencycode;
        public string exchangerate;
        public string paymentmethod;
        public string paymentnotes;
        public string cardidentification;
        public string cardrecordid;
        public string saleID;
    };

    struct sInvoices
    {
        public string co_lastname;
        public string firstname;
        public string addressline1;
        public string addressline2;
        public string addressline3;
        public string addressline4;
        public string inclusive;
        public string invoicenumber;
        public string saledate;
        public string customernumber;
        public string shipvia;
        public string itemnumber;
        public string deliverystatus;
        public string quantity;
        public string description;
        public string extaxprice;
        public string inctaxprice;
        public string job;
        public string comment;
        public string memo;
        public string salespersonlastname;
        public string salepersonfirstname;
        public string shippingdate;
        public string taxcode;
        public string nongstctamt;
        public string gstamt;
        public string lctamt;
        public string freightextaxamt;
        public string freightinctaxamt;
        public string freighttaxcode;
        public string freightnongstlctamt;
        public string freightgstamt;
        public string freightlctamt;
        public string salestatus;
        public string currencycode;
        public string exchangerate;
        public string paymentsdue;
        public string discountdays;
        public string balanceduedays;
        public string percentdiscount;
        public string percentmonthlycharge;
        public string referralsource;
        public string amountpaid;
        public string paymentmethod;
        public string paymentnotes;
        public string nameoncard;
        public string cardnumber;
        public string expirydate;
        public string authorisationcode;
        public string drawerBSB;
        public string drawerAcctNo;
        public string drawerCheckNo;
        public string category;
        public string location;
        public string cardidentification;
        public int cardrecordid;
        public string saleid;
        
    };

    #pragma warning restore

    [Serializable]
    public class exportXML
    {
        private int clientID;
        private Int16 cardType;
        private string trxType;
        private string crdTrxFrom;
        private string crdTrxTo;

        private Int32 lastSaleID;
        private Int32 lastReceiptID;

      
        List<sPayments> s_Receipts;
        List<sPayments> s_Discounts;  //reused struct payments
        List<sInvoices> s_Invoices;
        List<sCredits>  s_Credits;
        
       
        sHeader s_Header;
        
        private bool bCredits;
        private bool bDiscounts;
        private bool bReceipts;
        private bool bAdjustments;

        private int _depositPaymentAccount;

        public exportXML()
        {}

        public exportXML(int inClientID, Int16 inCardType, string inTrxType, string inTrxFrom, string inTrxTo, string inLastIDs)
        {
            this.clientID = inClientID;
            this.cardType = inCardType;
            this.trxType = inTrxType;
            this.crdTrxFrom = inTrxFrom;
            this.crdTrxTo = inTrxTo;

            this.bReceipts = false;
            this.bDiscounts = false;
            this.bCredits = false;
            this.bAdjustments = false;

            this.lastSaleID = 0;
            this.lastReceiptID = 0;

            if (!string.IsNullOrEmpty(inLastIDs))
            {
                if (inLastIDs.IndexOf(":") > 0)
                {
                    string[] strDummy = inLastIDs.Split(':');
                    this.lastSaleID = Convert.ToInt32(strDummy[0]);

                    if (strDummy.Length > 1)
                    {
                        this.lastReceiptID = Convert.ToInt32(strDummy[1]);
                    }
                }
                else if (inLastIDs.Length > 0)
                {
                    this.lastSaleID = Convert.ToInt32(inLastIDs);
                }
            } 
        }

     
        private sPayments assignPayments(System.Data.DataRow dRow)
        { //asign payments or discount (as payments)
            sPayments sRec = new sPayments();

            sRec.co_lastname = ((dRow["LName"] != null) ? dRow["LName"].ToString() : "");               //dRow["LName"].ToString();
            sRec.firstname = ((dRow["FName"] != null) ? dRow["FName"].ToString() : "");                 //dRow["FName"].ToString();
            sRec.depositAcctNo = _depositPaymentAccount.ToString();                                     //dRow["PaymentDepositAcct"].ToString();
            sRec.transactionID = ((dRow["BatchID"] != null) ? dRow["BatchID"].ToString() : "");         //dRow["BatchID"].ToString();
            sRec.receiptdate = ((dRow["DateReceived"] != null) ? dRow["DateReceived"].ToString() : ""); //dRow["DateReceived"].ToString();
            if (sRec.receiptdate.Length > 0) { sRec.receiptdate = Convert.ToDateTime(sRec.receiptdate).ToShortDateString(); }
           
            sRec.invoicenumber = ((dRow["InvoiceNumber"] != null) ? dRow["InvoiceNumber"].ToString() : ""); //dRow["InvoiceNumber"].ToString();

            string sAmount = ((dRow["Amount"] != null) ? dRow["Amount"].ToString() : "0.00");
            sRec.amountapplied = Math.Abs(Convert.ToDecimal(sAmount)).ToString();

            sRec.currencycode = ((dRow["CurrencyCode"] != null) ? dRow["CurrencyCode"].ToString().Trim() : ""); //dRow["CurrencyCode"].ToString();
            //if (sRec.currencycode.Length > 0) { sRec.currencycode = sRec.currencycode.Trim(); }

            sRec.exchangerate = ((dRow["ExchRate"] != null) ? dRow["ExchRate"].ToString().Trim() : "");         //dRow["ExchRate"].ToString();
            sRec.paymentmethod = ((dRow["PaymentType"] != null) ? dRow["PaymentType"].ToString().Trim() : "");  //dRow["PaymentType"].ToString();
            sRec.paymentnotes = ((dRow["PytNotes"] != null) ? dRow["PytNotes"].ToString().Trim() : "");         //dRow["PytNotes"].ToString();
            sRec.nameoncard = "";
            sRec.cardnumber = "";
            sRec.expirydate = "";
            sRec.authcode = "";
            sRec.cardidentification = ((dRow["cardID"] != null) ? dRow["cardID"].ToString().Trim() : "");       //dRow["cardID"].ToString();
            sRec.cardrecordid = ((dRow["cardrecordid"] != null) ? dRow["cardrecordid"].ToString().Trim() : ""); //dRow["cardrecordid"].ToString();
            sRec.saleID = ((dRow["SaleID"] != null) ? dRow["SaleID"].ToString().Trim() : "");                   //dRow["SaleID"].ToString();
            return sRec;
        }

        private sCredits assignCredits(System.Data.DataRow dRow)
        { //asign settled or unsettled credits
            //Payments.Amount as CreditAmount, Payments.PaymentType, Payments.TypeID, Payments.TypeStatus, Payments.Reference, 
			//Payments.DateReceived, Payments.YEARMONTH, Payments.Currency, Payments.ExchRate, Payments.Created, 
            //Payments.RECNO_TEMP as SaleID, Transactions.TransRef as InvoiceNumber

            sCredits sRec = new sCredits();
            sRec.typeid = dRow["TypeID"].ToString();
            sRec.co_lastname = dRow["LName"].ToString();
            sRec.firstname = dRow["FName"].ToString();
            sRec.depositAcctNo = _depositPaymentAccount.ToString(); 
            sRec.transactionID = dRow["BatchID"].ToString();
            sRec.receiptdate = dRow["DateReceived"].ToString();
            if (sRec.receiptdate.Length > 0) { sRec.receiptdate = Convert.ToDateTime(sRec.receiptdate).ToShortDateString(); }

            sRec.saleid = dRow["SaleID"].ToString();
            sRec.invoicenumber = dRow["InvoiceNumber"].ToString();
            sRec.creditamount = Math.Abs(Convert.ToDecimal(dRow["CreditAmount"])).ToString();

            sRec.currencycode = dRow["CurrencyCode"].ToString();
            if (sRec.currencycode.Length > 0) { sRec.currencycode = sRec.currencycode.Trim(); }

            sRec.exchangerate = dRow["ExchRate"].ToString();
            sRec.paymentmethod = dRow["PaymentType"].ToString();
            sRec.paymentnotes = dRow["PytNotes"].ToString();
            sRec.cardidentification = dRow["cardID"].ToString();
            sRec.cardrecordid = dRow["cardrecordid"].ToString();
            return sRec;
        }

        private sInvoices assignNegativeInvoices(System.Data.DataRow dRow)
        { //Adjustments
            sInvoices sRec = new sInvoices();
            sRec.co_lastname = dRow["LName"].ToString();
            sRec.firstname = dRow["FirstName"].ToString();
            sRec.saledate = dRow["TransDate"].ToString();
            sRec.exchangerate = dRow["ExchRate"].ToString();
            sRec.extaxprice = dRow["TotalTax"].ToString();
            sRec.freightextaxamt = dRow["TaxExclusiveFreight"].ToString();
            sRec.freightinctaxamt = dRow["TaxInclusiveFreight"].ToString();
            sRec.cardidentification = dRow["cardID"].ToString();
            sRec.cardrecordid = Convert.ToInt32(dRow["cardrecordid"]);
            return sRec;
        }

        public bool retrieveData()
        {
            Int16 iReceipts = 0; Int16 iCredits = 0;  
            Int16 iDiscounts = 0;  Int16 iAdjustments = 0;

            try
            {
                iReceipts = Convert.ToInt16(trxType.Substring(0, 1));
                iCredits = Convert.ToInt16(trxType.Substring(1, 1));
                iDiscounts = Convert.ToInt16(trxType.Substring(2, 1));
                iAdjustments = Convert.ToInt16(trxType.Substring(3, 1));
            }
            catch { }

            //dd/mm/yyyy
            DateTime dTrxFrom = Convert.ToDateTime(crdTrxFrom.Substring(0, 2) + "/" + crdTrxFrom.Substring(2, 2) + "/" + crdTrxFrom.Substring(4, 4));
            DateTime dTrxTo = Convert.ToDateTime(crdTrxTo.Substring(0, 2) + "/" + crdTrxTo.Substring(2, 2) + "/" + crdTrxTo.Substring(4, 4));
            
            int ix = 0;
            stpCaller stpC = new stpCaller();
            List<object> arrPar = new List<object>();

            //cardType = -1 (retrieve all customers with current transactions within specified date range)
            //cardType = 0  (retrieve all customers active/inactive)
            //cardType = 1  (retrieve active customers only)

            //populate header
            arrPar.Add("Header");
            arrPar.Add(clientID);
            arrPar.Add(0);  
            arrPar.Add(cardType);
            arrPar.Add(dTrxFrom);
            arrPar.Add(dTrxTo);
            arrPar.Add(this.lastSaleID);    //last sale ID
            arrPar.Add(this.lastReceiptID); //last receipt ID

            System.Data.DataSet theDS = stpC.executeSPDataSet(arrPar, stpCaller.stpType.cff2MYOB);
            if (theDS == null) { return false; }
            System.Data.DataTable theTable = theDS.Tables[0];
            System.Data.DataRow dRow = theTable.Rows[0];
            s_Header.uniquenum = clientID.ToString();
            s_Header.companyName = dRow["ClientName"].ToString();
            s_Header.dbaseVersion = dRow["DatabaseVersion"].ToString();
            s_Header.driverBuilderNo = dRow["DriverBuildNumber"].ToString();
            _depositPaymentAccount = Convert.ToInt32(dRow["PaymentDepositAcct"]);
            s_Header.changeControl = dRow["ChangeControl"].ToString();
            s_Header.isOpenItem = dRow["isOpenItem"].ToString();
          

            if (iReceipts == 1)
            { //retrieve receipts
                arrPar[0] = "Receipts";
                theDS = stpC.executeSPDataSet(arrPar, stpCaller.stpType.cff2MYOB);
                if (theDS == null) { return false; }
                theTable = theDS.Tables[0];
                s_Receipts = new List<sPayments>();
                
                if (theTable == null) { return false; }
                if (theTable.Rows.Count > 0) { bReceipts = true; }
                
                for (ix=0; ix < theTable.Rows.Count; ix++)
                {
                    dRow = theTable.Rows[ix];
                    s_Receipts.Add(assignPayments(dRow));
                }
            }

            if (iDiscounts == 1)
            { //retrieve discounts
                arrPar[0] = "Discounts";
                theDS = stpC.executeSPDataSet(arrPar, stpCaller.stpType.cff2MYOB);
                if (theDS == null) { return false; }
                theTable = theDS.Tables[0];
                s_Discounts = new List<sPayments>();
                if (theTable == null) { return false; }
                if (theTable.Rows.Count > 0) { bDiscounts = true; }
                for (ix = 0; ix < theTable.Rows.Count; ix++)
                {
                    dRow = theTable.Rows[ix];
                    s_Discounts.Add(assignPayments(dRow));
                }
            }

            if (iCredits == 1)
            { //retrieve settled credits 
              //note that settled credits may be partially allocated
                arrPar[0] = "SettledCredits";
                theDS = stpC.executeSPDataSet(arrPar, stpCaller.stpType.cff2MYOB);
                if (theDS == null) { return false; }
                theTable = theDS.Tables[0];
                s_Credits = new List<sCredits>();
                if (theTable == null) { return false; }
                if (theTable.Rows.Count > 0) { bCredits = true; }
                for (ix = 0; ix < theTable.Rows.Count; ix++)
                {
                    dRow = theTable.Rows[ix];
                    s_Credits.Add(assignCredits(dRow));
                }

               //retrieve unsettled credits
                arrPar[0] = "UnsettledCredits";
                theDS = stpC.executeSPDataSet(arrPar, stpCaller.stpType.cff2MYOB);
                if (theDS == null) { return false; }
                theTable = theDS.Tables[0];
                if (s_Credits == null) {
                    s_Credits = new List<sCredits>();
                }
                if (theTable == null) { return false; }
                if (theTable.Rows.Count > 0) { bCredits = true; }
                for (ix = 0; ix < theTable.Rows.Count; ix++)
                {
                    dRow = theTable.Rows[ix];
                    s_Credits.Add(assignCredits(dRow));
                }

            }

            if (iAdjustments == 1)
            {//retrieve adjustsments - posted as negative invoice
                arrPar[0] = "Adjustments";
                theDS = stpC.executeSPDataSet(arrPar, stpCaller.stpType.cff2MYOB);
                if (theDS == null) { return false;  }
                theTable = theDS.Tables[0];

                //we are listing adjustments as negative invoice
                s_Invoices = new List<sInvoices>();        
                if (theTable == null) { return false; }
                
                bAdjustments = (theTable.Rows.Count > 0) ? true : false;

                for (ix = 0; ix < theTable.Rows.Count; ix++)
                {
                    dRow = theTable.Rows[ix];
                }
                return true;
            }

         
            return true;
        }

        public StringBuilder createXMLData()
        {
            StringBuilder xmlBody = new StringBuilder();
            xmlBody.Append("<Ex_MYOB>");
            xmlBody.Append(Environment.NewLine);
            xmlBody.Append("<Header>");
            xmlBody.Append("<UniqueID>" + clientID.ToString() + "</UniqueID>" + Environment.NewLine);
            xmlBody.Append("<CompanyName>" + s_Header.companyName + "</CompanyName>" + Environment.NewLine);
            xmlBody.Append("<DatabaseVersion>" + s_Header.dbaseVersion + "</DatabaseVersion>" + Environment.NewLine);
            xmlBody.Append("<DriverBuildNumber>"  + s_Header.driverBuilderNo + "</DriverBuildNumber>" + Environment.NewLine);
            xmlBody.Append("<ChangeControl>" + s_Header.changeControl + "</ChangeControl>" + Environment.NewLine);
            xmlBody.Append("<isOpenItem>" + s_Header.isOpenItem + "</isOpenItem>" + Environment.NewLine)  ;
            xmlBody.Append("</Header>");
            xmlBody.Append(Environment.NewLine);
            
            if (bReceipts)
            { //write receipts
                for (int ix=0; ix < s_Receipts.Count(); ix++)
                {
                    xmlBody.Append("<R>" + Environment.NewLine);                     
                    xmlBody.Append("<COLASTNAME>" + s_Receipts[ix].co_lastname + "</COLASTNAME>" + Environment.NewLine);
                    xmlBody.Append("<FIRSTNAME>" + s_Receipts[ix].firstname + "</FIRSTNAME>" + Environment.NewLine);
                    xmlBody.Append("<DEPOSITACCT>" + s_Receipts[ix].depositAcctNo + "</DEPOSITACCT>" + Environment.NewLine);
                    xmlBody.Append("<TRANSACTIONID>" + s_Receipts[ix].transactionID+ "</TRANSACTIONID>" + Environment.NewLine);
                    xmlBody.Append("<RECEIPTDATE>" + s_Receipts[ix].receiptdate + "</RECEIPTDATE>" + Environment.NewLine);
                    xmlBody.Append("<INVOICENUMBER>" + s_Receipts[ix].invoicenumber+ "</INVOICENUMBER>" + Environment.NewLine);
                    xmlBody.Append("<AMOUNTAPPLIED>" + s_Receipts[ix].amountapplied + "</AMOUNTAPPLIED>" + Environment.NewLine);
                    xmlBody.Append("<MEMO>" + s_Receipts[ix].memo + "</MEMO>" + Environment.NewLine);
                    xmlBody.Append("<CURRENCYCODE>" + s_Receipts[ix].currencycode + "</CURRENCYCODE>" + Environment.NewLine);
                    xmlBody.Append("<EXCHANGERATE>" + s_Receipts[ix].exchangerate + "</EXCHANGERATE>" + Environment.NewLine);
                    xmlBody.Append("<PAYMENTMETHOD>" + s_Receipts[ix].paymentmethod+ "</PAYMENTMETHOD>" + Environment.NewLine);
                    xmlBody.Append("<PAYMENTNOTES>" + s_Receipts[ix].paymentnotes + "</PAYMENTNOTES>" + Environment.NewLine);
                    xmlBody.Append("<NAMEONCARD>" + s_Receipts[ix].nameoncard+ "</NAMEONCARD>" + Environment.NewLine);
                    xmlBody.Append("<CARDNUMBER>" + s_Receipts[ix].cardnumber+ "</CARDNUMBER>" + Environment.NewLine);
                    xmlBody.Append("<EXPIRYDATE>" + s_Receipts[ix].expirydate+ "</EXPIRYDATE>" + Environment.NewLine);
                    xmlBody.Append("<AUTHCODE>" + s_Receipts[ix].authcode + "</AUTHCODE>" + Environment.NewLine);
                    xmlBody.Append("<CARDID>" + s_Receipts[ix].cardidentification+ "</CARDID>" + Environment.NewLine);
                    xmlBody.Append("<CARDRECORDID>" + s_Receipts[ix].cardrecordid + "</CARDRECORDID>" + Environment.NewLine);
                    xmlBody.Append("<SALEID>" + s_Receipts[ix].saleID + "</SALEID>" + Environment.NewLine);
                    xmlBody.Append("</R>" + Environment.NewLine);
                }
            }

            if (bDiscounts)
            { //write discounts
                for (int ix = 0; ix < s_Discounts.Count(); ix++)
                {
                    xmlBody.Append("<D>" + Environment.NewLine);
                    xmlBody.Append("<COLASTNAME>" + s_Discounts[ix].co_lastname + "</COLASTNAME>" + Environment.NewLine);
                    xmlBody.Append("<FIRSTNAME>" + s_Discounts[ix].firstname + "</FIRSTNAME>" + Environment.NewLine);
                    xmlBody.Append("<DEPOSITACCT>" + s_Discounts[ix].depositAcctNo + "</DEPOSITACCT>" + Environment.NewLine);
                    xmlBody.Append("<TRANSACTIONID>" + s_Discounts[ix].transactionID + "</TRANSACTIONID>" + Environment.NewLine);
                    xmlBody.Append("<RECEIPTDATE>" + s_Discounts[ix].receiptdate + "</RECEIPTDATE>" + Environment.NewLine);
                    xmlBody.Append("<INVOICENUMBER>" + s_Discounts[ix].invoicenumber + "</INVOICENUMBER>" + Environment.NewLine);
                    xmlBody.Append("<AMOUNTAPPLIED>" + s_Discounts[ix].amountapplied + "</AMOUNTAPPLIED>" + Environment.NewLine);
                    xmlBody.Append("<MEMO>" + s_Discounts[ix].memo + "</MEMO>" + Environment.NewLine);
                    xmlBody.Append("<CURRENCYCODE>" + s_Discounts[ix].currencycode + "</CURRENCYCODE>" + Environment.NewLine);
                    xmlBody.Append("<EXCHANGERATE>" + s_Discounts[ix].exchangerate + "</EXCHANGERATE>" + Environment.NewLine);
                    xmlBody.Append("<PAYMENTMETHOD>" + s_Discounts[ix].paymentmethod + "</PAYMENTMETHOD>" + Environment.NewLine);
                    xmlBody.Append("<PAYMENTNOTES>" + s_Discounts[ix].paymentnotes + "</PAYMENTNOTES>" + Environment.NewLine);
                    xmlBody.Append("<NAMEONCARD>" + s_Discounts[ix].nameoncard + "</NAMEONCARD>" + Environment.NewLine);
                    xmlBody.Append("<CARDNUMBER>" + s_Discounts[ix].cardnumber + "</CARDNUMBER>" + Environment.NewLine);
                    xmlBody.Append("<EXPIRYDATE>" + s_Discounts[ix].expirydate + "</EXPIRYDATE>" + Environment.NewLine);
                    xmlBody.Append("<AUTHCODE>" + s_Discounts[ix].authcode + "</AUTHCODE>" + Environment.NewLine);
                    xmlBody.Append("<CARDID>" + s_Discounts[ix].cardidentification + "</CARDID>" + Environment.NewLine);
                    xmlBody.Append("<CARDRECORDID>" + s_Discounts[ix].cardrecordid + "</CARDRECORDID>" + Environment.NewLine);
                    xmlBody.Append("<SALEID>" + s_Discounts[ix].saleID + "</SALEID>" + Environment.NewLine);
                    xmlBody.Append("</D>" + Environment.NewLine);
                }
            }
            
            if (bCredits)
            { //write credits
                for (int ix = 0; ix < s_Credits.Count(); ix++)
                {
                    xmlBody.Append("<CR>" + Environment.NewLine);
                    xmlBody.Append("<TYPEID>" + s_Credits[ix].typeid + "</TYPEID>" + Environment.NewLine);
                    xmlBody.Append("<COLASTNAME>" + s_Credits[ix].co_lastname + "</COLASTNAME>" + Environment.NewLine);
                    xmlBody.Append("<FIRSTNAME>" + s_Credits[ix].firstname + "</FIRSTNAME>" + Environment.NewLine);
                    xmlBody.Append("<DEPOSITACCT>" + s_Credits[ix].depositAcctNo + "</DEPOSITACCT>" + Environment.NewLine);
                    xmlBody.Append("<TRANSACTIONID>" + s_Credits[ix].transactionID + "</TRANSACTIONID>" + Environment.NewLine);
                    xmlBody.Append("<RECEIPTDATE>" + s_Credits[ix].receiptdate + "</RECEIPTDATE>" + Environment.NewLine);
                    xmlBody.Append("<INVOICENUMBER>" + s_Credits[ix].invoicenumber + "</INVOICENUMBER>" + Environment.NewLine);
                    xmlBody.Append("<AMOUNTAPPLIED>" + s_Credits[ix].creditamount + "</AMOUNTAPPLIED>" + Environment.NewLine);
                    xmlBody.Append("<MEMO>" + s_Credits[ix].memo + "</MEMO>" + Environment.NewLine);
                    xmlBody.Append("<CURRENCYCODE>" + s_Credits[ix].currencycode + "</CURRENCYCODE>" + Environment.NewLine);
                    xmlBody.Append("<EXCHANGERATE>" + s_Credits[ix].exchangerate + "</EXCHANGERATE>" + Environment.NewLine);
                    xmlBody.Append("<PAYMENTMETHOD>" + s_Credits[ix].paymentmethod + "</PAYMENTMETHOD>" + Environment.NewLine);
                    xmlBody.Append("<PAYMENTNOTES>" + s_Credits[ix].paymentnotes + "</PAYMENTNOTES>" + Environment.NewLine);
                    xmlBody.Append("<CARDID>" + s_Credits[ix].cardidentification + "</CARDID>" + Environment.NewLine);
                    xmlBody.Append("<CARDRECORDID>" + s_Credits[ix].cardrecordid + "</CARDRECORDID>" + Environment.NewLine);
                    xmlBody.Append("<SALEID>" + s_Credits[ix].saleID + "</SALEID>" + Environment.NewLine);
                    xmlBody.Append("</CR>" + Environment.NewLine);
                }
            }

            if (bAdjustments)
            {
                xmlBody.Append("<I>");
                xmlBody.Append(Environment.NewLine);
            
                //write invoices
                xmlBody.Append("</I>");
                xmlBody.Append(Environment.NewLine);
            }

            xmlBody.Append("</Ex_MYOB>");
            return xmlBody;
        }


}      

}
