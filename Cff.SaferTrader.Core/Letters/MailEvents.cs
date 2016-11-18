using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cff.SaferTrader.Core;

namespace Cff.SaferTrader.Core.Letters
{
    [Serializable]
    public class MailEvents
    {
        private DocumentMailer docBag;

        public MailEvents(DocumentMailer docbag)
        { 
            this.docBag = docbag;
        }

        public void updateLetterSent()
        {
            string[] strDummy = docBag.FileAttachment.Split('.');
            strDummy[0] = strDummy[0].ToString().Replace("MTH", "Month");

            string ext = "";
            if (strDummy.Count() > 1 )
            ext = strDummy[1].ToUpper().Trim();

            string ltrName = "";
            if (strDummy.Count() > 0)
            {
                string[] strDummy2 = strDummy[0].Split('_');
                for (int ix = 0; ix < strDummy2.Count(); ix++)
                {
                    ltrName += strDummy2[ix] + " ";
                    if (strDummy2[ix].ToLower().Trim().Contains("letter")) { break; }
                }

                strDummy[0] = ltrName.Trim();
            }

            if (docBag.IsLetterDetailsNotAttchedButInBody)
            {
                ext = " details in email body";
            }

            string sentTo = "";
            if (docBag.SendTo.Contains("@"))
                sentTo = " [to: " + docBag.SendTo;
            if (docBag.SendCC.Contains("@"))
                sentTo += " | cc: " + docBag.SendCC + "]";
            if (sentTo.Length > 0)
                sentTo += "]";

                if (docBag.FileAttachment.ToString().Length> 0)
                docBag.Notes = "Sent " + ltrName + ext + sentTo + " via Debtor Management.";
            else
                docBag.Notes = "Sent " + ltrName + sentTo + " via Debtor Management.";

            updateCurrentNotes(strDummy[0]);

        }

        public void updateLetterGenerated()
        {
            string[] strDummy = docBag.FileAttachment.Split('.');
            strDummy[0] = strDummy[0].ToString().Replace("MTH", "Month");

            string ltrName = "";
            if (strDummy.Count() > 0)
            {
                string[] strDummy2 = strDummy[0].Split('_');
                for (int ix = 0; ix < strDummy2.Count(); ix++)
                {
                    ltrName += strDummy2[ix] + " ";
                    if (strDummy2[ix].ToLower().Trim().Contains("letter")) { break; }
                }

                strDummy[0] = ltrName.Trim();
            }

            if (docBag.IsLetterDetailsNotAttchedButInBody)
            {
                docBag.Notes = "Generated " + ltrName + " and its details in an email via Debtor Management.";
            }
            else
                docBag.Notes = "Generated " + ltrName + strDummy[1].ToUpper().Trim() + " via Debtor Management.";

            updateCurrentNotes(strDummy[0]);
        }


        public void updateLetterPrinted()
        {
            string[] strDummy = docBag.FileAttachment.Split('.');
            strDummy[0] = strDummy[0].ToString().Replace("MTH", "Month");

            string ltrName = "";
            if (strDummy.Count() > 0)
            {
                string[] strDummy2 = strDummy[0].Split('_');
                for (int ix = 0; ix < strDummy2.Count(); ix++)
                {
                    ltrName += strDummy2[ix] + " ";
                    if (strDummy2[ix].ToLower().Trim().Contains("letter")) { break; }
                }

                strDummy[0] = ltrName.Trim();
            }


            docBag.Notes = "Printed " + ltrName + strDummy[1].ToUpper().Trim() + " via Debtor Management.";
            updateCurrentNotes(strDummy[0]);
        }


        private void updateCurrentNotes(string letterName)
        {
            string strYrMth = Convert.ToDateTime(docBag.DateStamp).Year.ToString() +
                             Convert.ToDateTime(docBag.DateStamp).Month.ToString().PadLeft(2, '0');

            List<object> objParams = new List<object>();
            objParams.Add(docBag.ClientID);
            objParams.Add(docBag.CustomerID);
            objParams.Add(Convert.ToInt16(docBag.LetterIdx));
            objParams.Add(letterName);
            objParams.Add(docBag.DateStamp);
            objParams.Add(Convert.ToInt32(strYrMth));
            objParams.Add(docBag.UserID);
            objParams.Add(docBag.FileAttachment);
            objParams.Add(docBag.Notes);        // MSarza[20160922]

            try
            {
                stpCaller stpc = new stpCaller();
                var ret = stpc.executeSP(objParams, stpCaller.stpType.InsertLetterSent);

                objParams.Clear();
                objParams.Add(1);
                objParams.Add(docBag.CustomerID);
                objParams.Add(docBag.UserID);
                objParams.Add(Convert.ToInt32(strYrMth));
                System.Data.DataSet DS = stpc.executeSPDataSet(objParams, stpCaller.stpType.GetNotesCurrent);

                int notesid = 0;

                // MSarza [20160922]: As per Marty, instead of updating notes new notes must be inserted instead
                //          like the VB app behaves. Also, as with the VB app, permanent notes must be inserted 
                //          for letters names like: (1) 7 Day, (2) Default, (3) AEL, and (4) Credit Consultants

                //bool doInsert = false;
                //if (DS == null) { doInsert = true; }
                //else if (DS.Tables.Count == 0)
                //{ doInsert = true; }
                //else if (DS.Tables[0].Rows.Count == 0)
                //{ doInsert = true; }
                //else if (DS.Tables[0].Rows.Count > 0)
                //{
                //    System.Data.DataRow DR = DS.Tables[0].Rows[0];
                //    if (Convert.ToInt32(DR["yrMth"]) == Convert.ToInt32(strYrMth))
                //    {
                //        doInsert = false;
                //        notesid = Convert.ToInt32(DR["NotesID"]);
                //        docBag.Notes = DR["notes"] + System.Environment.NewLine + "[C " + docBag.Notes + "]";
                //    }
                //    else { doInsert = true; }
                //}

                objParams.Clear();
                objParams.Add(notesid);
                objParams.Add(docBag.CustomerID);
                objParams.Add(docBag.Notes);
                objParams.Add(docBag.DateStamp);
                objParams.Add(docBag.UserID);
                //if (doInsert)
                //{ //insert current notes
                    ret = stpc.executeSP(objParams, stpCaller.stpType.InsertCurrentNotes);
                //}
                //else
                //{ //update current notes
                //    ret = stpc.executeSP(objParams, stpCaller.stpType.UpdateCurrentNotes);
                //}

                string[] letterNamesReqPermNotes = { "7 Day", "Default", "AEL", "Credit Consultants" };

                bool updatePermNotes = false;
                foreach(string lnreq in letterNamesReqPermNotes)
                {
                    if (letterName.Contains(lnreq))
                        updatePermNotes = true;
                }
                
                if (updatePermNotes)
                {
                    //MSarza[20161003]: Store proc for inserting current notes generates its own current datetime and uses it as the
                    //                  created date for inserting records; the insert permanent notes doesn't. The insert permanent
                    //                  notes uses the datetime parameter passed to it, hence, generating a current date time as here
                    //                  to be passed as the create date to the stored procedure
                    DateTime dt = DateTime.Now;
                    string x; string m; string d; int dt1; int dt2;

                    //x = "0" + docBag.DateStamp.Month.ToString();
                    x = "0" + dt.Month.ToString();
                    m = x.Substring(x.Length - 2);
                    //x = "0" + docBag.DateStamp.Day.ToString();
                    x = "0" + dt.Day.ToString();
                    d = x.Substring(x.Length - 2);                              
                    
                    dt1 = int.Parse(docBag.DateStamp.Year.ToString() + m);
                    dt2 = int.Parse(docBag.DateStamp.Year.ToString() + m + d);
                    objParams.Add(dt);
                    objParams.Add(dt1);
                    objParams.Add(dt2);

                    ret = stpc.executeSP(objParams, stpCaller.stpType.InsertNotesPermanent);
                }                
            }
            catch (Exception e)
            {
                string x = e.ToString();
            }
        }
    }
}
