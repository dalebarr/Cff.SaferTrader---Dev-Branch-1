using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cff.SaferTrader.Core.Letters
{
    [Serializable]
    public class DocumentMailer:IDocument
    {
        private string fileAttachment;
        private string subject;
        private string sendTo;
        private string sendCC;
        private string sendBCC;
        private string sendBody;
        private string popupPageName;

        
        private int clientID;
        private int customerID;
        private int userID;

        private Int16 letterIdx;
        private Int16 letterType; //0- letter, 1-statement, 2-banking details
        private DateTime dateStamp;

        private string notes;

        //  MSarza: Added properties primarily to detect if customer letter
        //          was sent where details are in the body itself and not as an attached file
        private bool isLetterDetailsNotAttchedButInBody;                       


        public DocumentMailer(Int16 ltrType, Int16 ltrIdx, DateTime dtStamp, int cliID, int custID, int uID,
                                string attachment, string thesubject, string sendto, string sendcc, string sendbcc, 
                                    string strbody, string viewID, bool letterDetailsNotAttchedButInBody)
        {
            this.letterType = ltrType;
            this.letterIdx = ltrIdx;
            this.dateStamp = dtStamp;
            this.clientID = cliID;
            this.customerID = custID;
            this.userID = uID;

            this.fileAttachment = attachment;
            this.subject = thesubject;
            this.sendTo = sendto;
            this.sendCC = sendcc;
            this.sendBCC = sendbcc;
            this.sendBody = strbody;
            this.popupPageName = "checkEmail.aspx?ViewID=" + viewID;

            this.isLetterDetailsNotAttchedButInBody = letterDetailsNotAttchedButInBody;
        }


        public int HashCode
        {
            get { return this.GetHashCode(); }
        }

        public string FileAttachment
        {
            get { return this.fileAttachment; }        
        }

        public string Subject
        {
            get { return this.subject; }
        }

        public string  SendTo
        {
            get { return this.sendTo; }
            //MSarza[20160929]:  Added to support updating of notes whenever a variable with a reference from this object's instance is updated
            set { this.sendTo = value; }       
        }

        public string  SendCC
        {
            get { return this.sendCC; }
            //MSarza[20160929]:  Added to support updating of notes whenever a variable with a reference from this object's instance is updated
            set { this.sendCC = value; }
        }

        public string SendBCC
        {
            get { return this.sendBCC; }
        }

        public string SendBody
        {
            get { return this.sendBody; }
        }

        public string PopupPageName
        {
            get { return this.popupPageName; }
        }

        public int ClientID
        {
            get { return this.clientID; }
        }

        public int CustomerID
        {
            get { return this.customerID; }
        }

        public int UserID
        {
            get { return this.userID; }
        }


        public int LetterType
        {
            get { return this.letterType; }
        }

        public int LetterIdx
        {
            get { return this.letterIdx; }
        }

        public DateTime DateStamp
        {
            get { return this.dateStamp; }
        }

        public string Notes
        {
            get { return this.notes; }
            set { this.notes = value; }
        }

        public bool IsLetterDetailsNotAttchedButInBody
        {
            get { return this.isLetterDetailsNotAttchedButInBody; }
        }


        

    }
}
