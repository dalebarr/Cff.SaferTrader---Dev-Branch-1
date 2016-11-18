using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Repositories;
using Microsoft.Office.Interop.Word;

namespace Cff.SaferTrader.Core.Letters
{
    [Serializable]
    public class IOutput
    {
        private CustomerContact custContact;
        private LetterDetails theLetterDetails;
        private int     clientID;
        private string  clientName;
        private string  letterHead = "";
        private string  templateFilePathRec = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePathRecs"];
        private bool    administeredByCFF;
        private bool    bCFFStaff;
        private bool clientHasLetterTemplates = false;

        public IOutput() 
        {
            theLetterDetails = new LetterDetails();
            custContact = new CustomerContact();
            administeredByCFF = false;
            bCFFStaff = true;
        }

        public IOutput(int clientid, string clientname)
        {
            this.clientID = clientid;
            this.clientName = clientname;
        }

        public IOutput(bool isAdminByCFF, bool isCFFStaff)
        {
            theLetterDetails = new LetterDetails();
            custContact = new CustomerContact();
            administeredByCFF = isAdminByCFF;
            bCFFStaff = isCFFStaff;
        }

        public IOutput(CustomerContact custcontact)
        {
          this.custContact = custcontact;
        }

        public IOutput(LetterDetails lDetails)
        {
            this.theLetterDetails = lDetails;
        }

        public IOutput(CustomerContact custcontact, bool isAdminByCFF, bool isCFFStaff)
        {
            this.custContact = custcontact;
            administeredByCFF = isAdminByCFF;
            bCFFStaff = isCFFStaff;
        }

        public IOutput(LetterDetails lDetails, bool isAdminByCFF, bool isCFFStaff)
        {
            this.theLetterDetails = lDetails;
            administeredByCFF = isAdminByCFF;
            bCFFStaff = isCFFStaff;
        }

        public void setClientID(int value)
        {
            this.clientID = value;
        }

        public void setClientNAme(string value)
        {
            this.clientName = value;
        }

        public void setClientHasLetterTemplates(bool value)
        {
            this.clientHasLetterTemplates = value;
        }

        public string generateReportStatement(string strOutput)
        {
            string strDoc = "";
            strDoc = generateWordReportStatement(compileFileStatRpt(), strOutput);
            return strDoc;
        }

        public string generateLetterStatement(string strOutput, string serverMapPath)
        {
            string strDoc = "";
            string strDtaSrc = "";

            try
            {
                if (theLetterDetails.LetterName.ToUpper().Contains("CLIENT ACTION"))
                { 
                    strDtaSrc = compileFileDetails("forCustDetails", theLetterDetails.YrMth, "CustDetailsNotes"); 
                }
                else 
                { 
                    strDtaSrc = compileFileDetails("forCustDetails", theLetterDetails.YrMth, "CustDetails"); 
                }

                if ((theLetterDetails.LetterName.ToUpper().Contains("CLIENT ACTION")) || (theLetterDetails.LetterName.ToUpper().Contains("CREDIT")))
                { //retrieve transaction details for statRec.doc
                    compileFileTrns();
                }

                /*if (bCFFStaff == false && administeredByCFF == false) {*/
                if (administeredByCFF == false)
                {
                    stpCaller stpc = new stpCaller();
                    List<object> arrParam = new List<object>();
                    arrParam.Add("LetterHead");
                    arrParam.Add(this.clientID);
                    arrParam.Add(0);
                    System.Data.DataSet DS = stpc.executeSPDataSet(arrParam, stpCaller.stpType.GetClientSelectedData);
                    if (DS.Tables[0].Rows.Count > 0)
                        this.letterHead = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePathClientLetterHeads"].ToString() + DS.Tables[0].Rows[0]["letterheadFileName"].ToString();
                    else
                        this.letterHead = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePathClientLetterHeads"].ToString() + "defaultLetterHead.jpg";
                }

                strDoc = generateWordStatement(strDtaSrc, strOutput, serverMapPath);
            }
            catch (Exception exc)
            {
                strDoc = "ERR: " + exc.Message;
            }
           
            return strDoc;
        }

        private string generateWordReportStatement(string datasource, string strOutput)
        { //generate report statement in word document
            Object objFalse = false;
            Object objTrue = true;
            Object objMiss = System.Reflection.Missing.Value;
            Object objWordSaveFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
            Object objWordOpenFormat = Microsoft.Office.Interop.Word.WdOpenFormat.wdOpenFormatText;

            //MSarza [20150805]
                //string templatesFilePathCust = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePathCustomer"];
                //string reportsPath = System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"];
                //Object objFilePath = templatesFilePathCust + "SpecialCases\\StatementReports.doc";    

            string reportsPath = System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"];
            string templatesFilePathCust = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePathCustomer"];
            templatesFilePathCust += "SpecialCases\\";   // This is the folder structure as per existing rules
            string templateName = "StatementReports";

            Object objFilePath = null;
            if (System.IO.File.Exists(templatesFilePathCust + templateName + ".doc"))
            { objFilePath = templatesFilePathCust + templateName + ".doc"; }
            else if (System.IO.File.Exists(templatesFilePathCust + templateName + ".docx"))
            { objFilePath = templatesFilePathCust + templateName + ".docx"; }
            else
            {
                templateName = "Statement";
                if (System.IO.File.Exists(templatesFilePathCust + templateName + ".doc"))
                { objFilePath = templatesFilePathCust + templateName + ".doc"; }
                else 
                { objFilePath = templatesFilePathCust + templateName + ".docx";  }            }
            //---

            
            string strFileName = "StatementReport_" + cleanFileName(theLetterDetails.CustName);
            strFileName += "_" + theLetterDetails.DateAsAt.Year.ToString() +
                            theLetterDetails.DateAsAt.Month.ToString().PadLeft(2, '0') + theLetterDetails.DateAsAt.Day.ToString().PadLeft(2, '0') +
                            theLetterDetails.DateAsAt.Hour.ToString().PadLeft(2, '0') + theLetterDetails.DateAsAt.Minute.ToString().PadLeft(2, '0') +
                            theLetterDetails.DateAsAt.Second.ToString().PadLeft(2, '0');
            Object objDocName = reportsPath + strFileName + ".doc";
            try
            {
                Microsoft.Office.Interop.Word._Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word._Document wordDoc = wordApp.Documents.Open(ref objFilePath,
                                ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                                ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss);

                wordDoc.MailMerge.OpenDataSource(datasource, ref objWordOpenFormat,
                                        ref objFalse, ref objTrue, ref objFalse, ref objFalse,
                                        ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                                        ref objMiss, ref objMiss, ref objMiss, ref objMiss);

               
                for (int ix = 0; ix < theLetterDetails.StatementReportRecords.Count(); ix++) 
                {
                    wordDoc.Tables[2].Rows.Add(ref objMiss); 
                }

                for (int ix = 0; ix < theLetterDetails.StatementReportRecords.Count(); ix++) 
                {
                    if (theLetterDetails.StatementReportRecords[ix].Dated != null)
                    {
                        wordDoc.Tables[2].Cell(ix + 2, 1).Range.InsertAfter(theLetterDetails.StatementReportRecords[ix].Dated.Value.ToShortDateString());
                        wordDoc.Tables[2].Cell(ix + 2, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                    else { wordDoc.Tables[2].Cell(ix + 2, 1).Range.InsertAfter(""); }

                    if (theLetterDetails.StatementReportRecords[ix].Description != null)
                    {
                        wordDoc.Tables[2].Cell(ix + 2, 2).Range.InsertAfter(theLetterDetails.StatementReportRecords[ix].Description);
                        wordDoc.Tables[2].Cell(ix + 2, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                    else { wordDoc.Tables[2].Cell(ix + 2, 2).Range.InsertAfter(""); }


                    if (theLetterDetails.StatementReportRecords[ix].Description.ToLower().Contains("invoice"))
                    {
                        if (theLetterDetails.StatementReportRecords[ix].Number != null)
                        {
                            wordDoc.Tables[2].Cell(ix + 2, 3).Range.InsertAfter(theLetterDetails.StatementReportRecords[ix].Number); //invoice
                            wordDoc.Tables[2].Cell(ix + 2, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        }
                        else { wordDoc.Tables[2].Cell(ix + 2, 3).Range.InsertAfter(""); }
                    
                        wordDoc.Tables[2].Cell(ix + 2, 4).Range.InsertAfter(""); //reference
                        wordDoc.Tables[2].Cell(ix + 2, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    } else {
                      wordDoc.Tables[2].Cell(ix + 2, 3).Range.InsertAfter(""); //invoice 
                      wordDoc.Tables[2].Cell(ix + 2, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                      if (theLetterDetails.StatementReportRecords[ix].Reference != null)
                      {
                          wordDoc.Tables[2].Cell(ix + 2, 4).Range.InsertAfter(theLetterDetails.StatementReportRecords[ix].Reference);
                          wordDoc.Tables[2].Cell(ix + 2, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                      }
                      else { wordDoc.Tables[2].Cell(ix + 2, 4).Range.InsertAfter(""); }
                    }

                   
                    wordDoc.Tables[2].Cell(ix + 2, 5).Range.InsertAfter(System.String.Format("{0:c}", theLetterDetails.StatementReportRecords[ix].Debits)); 
                    wordDoc.Tables[2].Cell(ix + 2, 5).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    wordDoc.Tables[2].Cell(ix + 2, 6).Range.InsertAfter(System.String.Format("{0:c}", theLetterDetails.StatementReportRecords[ix].Credits)); 
                    wordDoc.Tables[2].Cell(ix + 2, 6).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                }

                wordDoc.MailMerge.SuppressBlankLines = true;
                wordDoc.MailMerge.Destination = Microsoft.Office.Interop.Word.WdMailMergeDestination.wdSendToNewDocument;
                wordDoc.MailMerge.Execute(ref objFalse);

                wordApp.ActiveDocument.SaveAs(ref objDocName, ref objWordSaveFormat,
                                ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                                ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss);

                wordDoc.Close(ref objFalse, ref objMiss, ref objMiss);
                ((Microsoft.Office.Interop.Word._Document)wordApp.ActiveDocument).Close(ref objFalse, ref objMiss, ref objMiss);
                wordApp.Quit(ref objFalse, ref objMiss, ref objMiss);

                strFileName = "OK: " + strFileName;

            }
            catch (Exception exc)
            {
                strFileName = "ERR: " + exc.Message;
            }

            return strFileName;
        }

        private string generateWordStatement(string datasource, string strOutput, string serverMapPath)
        {
            Object objFalse = false;
            Object objTrue = true;
            Object objMiss = System.Reflection.Missing.Value;
            Object objWordSaveFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
            Object objWordOpenFormat = Microsoft.Office.Interop.Word.WdOpenFormat.wdOpenFormatText;
            string strFileName = "";


            // For refactoring? Immediate code below (setting of ClientID may be redundant as there is currently only one call to this method
            //      and there is already clientID private member that is set up prior to calling the method calling this method.
            int ClientID = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString.Id :
               (!string.IsNullOrWhiteSpace(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString.Id : 
                           (QueryString.ClientId==null)?0:(int)QueryString.ClientId;
            try
            {
                string templatesFilePathCust = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePathCustomer"];
                string reportsPath = System.Configuration.ConfigurationManager.AppSettings["ReportsFolder"];
                string strDummy = cleanFileName(theLetterDetails.LetterName); //theLetterDetails.LetterName.Replace(" ", "_");

                if (strDummy.Contains("Notification"))
                {
                    if (administeredByCFF == false)
                    {
                        //templatesFilePathCust = templatesFilePathCust.Substring(0, templatesFilePathCust.Length - 1);
                        if (strDummy.Contains("Client_Notification"))
                        {
                            templatesFilePathCust += clientID.ToString() + "\\Notification\\ClientNotification\\";
                        }
                        else
                        {
                            templatesFilePathCust += clientID.ToString() + "\\Notification\\";
                        }
                    }
                    else 
                    {
                        if (strDummy.Contains("Client_Notification"))
                        {
                            templatesFilePathCust += "Notification\\ClientNotification\\";
                        }
                        else
                        {
                            templatesFilePathCust += "Notification\\";
                        }
                    }

                    if (System.IO.File.Exists(templatesFilePathCust + ClientID.ToString() + "_" + strDummy + ".doc"))
                    {
                        templatesFilePathCust += ClientID.ToString() + "_" + strDummy + ".doc";
                    }
                    else if (System.IO.File.Exists(templatesFilePathCust + ClientID.ToString() + "_" + strDummy + ".docx"))
                    {
                        templatesFilePathCust += ClientID.ToString() + "_" + strDummy + ".docx";
                    }
                    else if (System.IO.File.Exists(templatesFilePathCust.Replace(ClientID.ToString() + "\\", "") + "000_" + strDummy + ".doc"))
                    {
                        templatesFilePathCust = templatesFilePathCust.Replace(ClientID.ToString() + "\\", "") + "000_" + strDummy + ".doc";
                    }
                    else
                    {
                        templatesFilePathCust = templatesFilePathCust.Replace(ClientID.ToString() + "\\", "") + "000_" + strDummy + ".docx";
                    }
                }
                else
                {

                    if (this.clientHasLetterTemplates)
                    {
                        templatesFilePathCust += clientID.ToString() + "\\" + strDummy;
                    }
                    else 
                    {
                        templatesFilePathCust += strDummy;
                    }

                    if (templatesFilePathCust.Contains("Month"))
                    {
                        if (System.IO.File.Exists(templatesFilePathCust + ".doc"))
                        {
                            templatesFilePathCust += ".doc";
                        }
                        else if (System.IO.File.Exists(templatesFilePathCust.Replace("Month", "MTH") + ".doc"))
                        {
                            templatesFilePathCust = templatesFilePathCust.Replace("Month", "MTH") + ".doc";
                        }
                        else if (System.IO.File.Exists(templatesFilePathCust.Replace("Month", "MTH") + ".docx"))
                        {
                            templatesFilePathCust = templatesFilePathCust.Replace("Month", "MTH") + ".docx";
                        }
                        else
                        {
                            templatesFilePathCust += ".docx";
                        }
                    }
                    else
                    {
                        if (System.IO.File.Exists(templatesFilePathCust + ".doc"))
                        {
                            templatesFilePathCust += ".doc";
                        }
                        else
                        {
                            templatesFilePathCust += ".docx";
                        }
                    }                    
                }

                Object objFilePath = templatesFilePathCust;

                strFileName += strDummy;
                strDummy = cleanFileName(theLetterDetails.CustName); //theLetterDetails.CustName.Replace(" ", "_");
                //MSarza
                //strFileName += "_" + strDummy + "_" + theLetterDetails.DateStamp.Year.ToString() +
                //                theLetterDetails.DateStamp.Month.ToString().PadLeft(2, '0') + theLetterDetails.DateStamp.Day.ToString().PadLeft(2, '0') +
                //                theLetterDetails.DateStamp.Hour.ToString().PadLeft(2, '0') + theLetterDetails.DateStamp.Minute.ToString().PadLeft(2, '0') +
                //                theLetterDetails.DateStamp.Second.ToString().PadLeft(2, '0');
                strFileName += "_" + strDummy + "_" +
                                theLetterDetails.CustId + "_" +
                                theLetterDetails.DateStamp.Year.ToString() +
                                theLetterDetails.DateStamp.Month.ToString().PadLeft(2, '0') + theLetterDetails.DateStamp.Day.ToString().PadLeft(2, '0') +
                                theLetterDetails.DateStamp.Hour.ToString().PadLeft(2, '0') + theLetterDetails.DateStamp.Minute.ToString().PadLeft(2, '0') +
                                theLetterDetails.DateStamp.Second.ToString().PadLeft(2, '0');

                Object objDocName = reportsPath + strFileName + ".doc";

                Microsoft.Office.Interop.Word._Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word._Document wordDoc = wordApp.Documents.Open(ref objFilePath,
                                ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                                ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss);
    
                wordDoc.MailMerge.OpenDataSource(datasource, ref objWordOpenFormat,
                                        ref objFalse, ref objTrue, ref objFalse, ref objFalse,
                                        ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                                        ref objMiss, ref objMiss, ref objMiss, ref objMiss);

                //additional formatting for statements with record merge fields
                bool readStatRec = false;
                bool readCNotes = false;
                string fRecord = "";
                string fNotesRecord = "";
                Microsoft.Office.Interop.Word.Field mField;
                int ix = 1;

                for (; ix < wordDoc.InlineShapes.Count + 1; ix++)
                {
                    if (wordDoc.InlineShapes[ix].Type == WdInlineShapeType.wdInlineShapePicture)
                    {   //the letterhead, check if this user is in client role
                        /*if (administeredByCFF == false && bCFFStaff == false)
                        {   //if letter is administered by client, search for the 1st image in this template 
                            //replace CFF letterhead with that of the client's letterhead.
                        */
                        if (administeredByCFF == false)
                        {
                            if (this.letterHead.Length > 0)
                            {
                                Microsoft.Office.Interop.Word.Range rng = wordDoc.InlineShapes[1].Range;
                                object rngObj = rng;

                                wordDoc.InlineShapes[ix].Select();
                                wordApp.Selection.TypeBackspace();

                                Microsoft.Office.Interop.Word.Shape shp = wordDoc.Shapes.AddPicture(this.letterHead, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref rngObj);
                                                              
                            }
                        }
                        break;
                    }
                }

                for (ix = 1; ix < wordDoc.Fields.Count + 1; ix++)
                { //check if template is credit/client action letters
                    mField = wordDoc.Fields[ix];
                    Microsoft.Office.Interop.Word.Range rangeFldCode = mField.Code;
                    string fieldText = rangeFldCode.Text;

                    if (fieldText.Contains("COMMENTS"))
                    {
                        string fieldName = fieldText.Substring(9).Trim();
                        if (fieldName == "StatRec.doc")
                        {
                            fRecord = fieldName;
                            mField.Select();
                            wordApp.Selection.TypeText(" ");
                            readStatRec = true;
                        }

                        if (fieldName == "CNotes.doc")
                        {
                            fNotesRecord = fieldName;
                            mField.Select();
                            wordApp.Selection.TypeText(" ");
                            readCNotes = true;
                        }
                    }
                } //end check template


                if (readStatRec)
                { //read status record
                    string fRec = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePathRecs"] + fRecord;
                    int iRows = getNumLines(fRec);
                    if (iRows > 0)
                    {
                        string[,] dTable = new string[iRows - 1, 4];

                        if (getTableFromRec(ref dTable, fRec, false, ','))
                        {
                            for (int trows = 1; trows < iRows - 1; trows++)
                            { wordDoc.Tables[1].Rows.Add(ref objMiss); }

                            for (int trows = 1; trows < iRows - 1; trows++)
                            {
                                wordDoc.Tables[1].Cell(trows + 1, 1).Range.InsertAfter(Convert.ToString(dTable[trows - 1, 0]));
                                wordDoc.Tables[1].Cell(trows + 1, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                                wordDoc.Tables[1].Cell(trows + 1, 2).Range.InsertAfter(Convert.ToString(dTable[trows - 1, 1]));
                                wordDoc.Tables[1].Cell(trows + 1, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                                wordDoc.Tables[1].Cell(trows + 1, 3).Range.InsertAfter(Convert.ToString(dTable[trows - 1, 2]));
                                wordDoc.Tables[1].Cell(trows + 1, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                                wordDoc.Tables[1].Cell(trows + 1, 4).Range.InsertAfter(Convert.ToString(dTable[trows - 1, 3]));
                                wordDoc.Tables[1].Cell(trows + 1, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                            }
                            wordDoc.Tables[1].Rows[1].Range.Bold = 1;
                        }
                    }
                }


                if (readCNotes)
                { //read Notes record
                    for (int trows = 1; trows < theLetterDetails.CurrentNotes.Rows.Count + 1; trows++)
                    {
                        wordDoc.Tables[3].Rows.Add(ref objMiss);
                    }

                    for (int trows = 1; trows < theLetterDetails.CurrentNotes.Rows.Count + 1; trows++)
                    {
                        System.Data.DataRow DR = theLetterDetails.CurrentNotes.Rows[trows - 1];
                        wordDoc.Tables[3].Cell(trows + 1, 1).Range.InsertAfter(Convert.ToString(DR["Created"]));
                        wordDoc.Tables[3].Cell(trows + 1, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

                        wordDoc.Tables[3].Cell(trows + 1, 2).Range.InsertAfter(Convert.ToString(DR["Notes"]));
                        wordDoc.Tables[3].Cell(trows + 1, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    }
                    wordDoc.Tables[3].Rows[1].Range.Bold = 1;
                }


                wordDoc.MailMerge.SuppressBlankLines = true;
                wordDoc.MailMerge.Destination = Microsoft.Office.Interop.Word.WdMailMergeDestination.wdSendToNewDocument;
                wordDoc.MailMerge.Execute(ref objFalse);



                object documentFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatFilteredHTML; //8

                if (strOutput.ToUpper().Contains("WORD") || strOutput.ToUpper().Contains("FAX"))
                {  //Save the word document as HTML file - later we need this for display
                    wordApp.ActiveWindow.ActivePane.View.Type = WdViewType.wdNormalView;    //Makes the word app open the document in Normal view instead of say html view,
                                                                                            //so as not to hide the header and footer when word file generated is opened?
                    wordApp.ActiveDocument.SaveAs(ref objDocName, ref objWordSaveFormat,
                                    ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                                    ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss);

                    if (strOutput.ToUpper().Contains("PRINT") || strOutput.ToUpper().Contains("FAX"))
                    {
                        int htmldivctr = wordApp.ActiveDocument.HTMLDivisions.Count;

                        ix = 1; object objCnt = ix;
                        wordApp.ActiveDocument.Paragraphs[1].Range.Delete(ref objMiss, ref objCnt);
                    }
                    

                    Object htmlFilePath = serverMapPath + strFileName + ".htm";
                    //object SaveAsAOCLetter = true;

                    wordApp.ActiveDocument.SaveAs(ref htmlFilePath, ref documentFormat, 
                                ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                                ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss);

                    /*Object htmlFilePathBak = "C:\\source\\CFL\\GeneratedFiles\\" + strFileName + ".htm";
                    wordApp.ActiveDocument.SaveAs(ref htmlFilePathBak, ref documentFormat, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                                   ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss);

                
                     */
                }
                else if (strOutput.ToUpper().Contains("PDF"))
                { //Save the word document as PDF File 
                    Object pdfFilePath = reportsPath + strFileName + ".pdf";
                    //Object pdfFilePath = serverMapPath + strFileName + ".pdf";
                    documentFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                    wordApp.ActiveDocument.SaveAs(ref pdfFilePath, ref documentFormat, 
                                ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                                ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss);
                }

                wordDoc.Close(ref objFalse, ref objMiss, ref objMiss);

                ((Microsoft.Office.Interop.Word._Document)wordApp.ActiveDocument).Close(ref objFalse, ref objMiss, ref objMiss);
                wordApp.Quit(ref objFalse, ref objMiss, ref objMiss);

                strFileName = "OK: " + strFileName;
            }
            catch (Exception exc)
            {
                
                strFileName = "ERR: " + exc.Message;

            }

            return strFileName;
        }

        private bool compileFileTrns()
        {
           try {
            List<object> arrParam = new List<object>();
            arrParam.Add(theLetterDetails.CustId); 
            arrParam.Add(0);                        //whichtrns
            arrParam.Add(0);                        //AsAt //theLetterDetails.YrMth
            arrParam.Add(theLetterDetails.YrMth);   //YrMth
            arrParam.Add(-1);
            arrParam.Add(theLetterDetails.DateAsAt);

            string theFileName = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePathRecs"].ToString()  + "StatRec.doc";
            if  (File.Exists(theFileName)) { File.Delete(theFileName); }

            FileStream fs = new FileStream(theFileName, FileMode.Create, FileAccess.Write);
            fs.SetLength(0);

            StreamWriter w = new StreamWriter(fs);

            w.Write('\u0022');
            w.Write("Dated");
            w.Write('\u0022');
            w.Write(',');
            w.Write('\u0022');
            w.Write("Type");
            w.Write('\u0022');
            w.Write(',');
            w.Write('\u0022');
            w.Write("TransNumOrReference");
            w.Write('\u0022');
            w.Write(',');
            w.Write('\u0022');
            w.Write("Amount");
            w.WriteLine('\u0022');

            //call stGetTransactions
            stpCaller stpc = new stpCaller();
            System.Data.DataSet DS = stpc.executeSPDataSet(arrParam, stpCaller.stpType.GetTransactions);
            if (DS == null) { }
            else {
                if (DS.Tables.Count == 0) { }
                else {
                    System.Data.DataView DV = DS.Tables[0].DefaultView;
                    DV.RowFilter = "TransTypeID <> 5";
                    for (int ix=0; ix<DV.Count; ix++) {
                        System.Data.DataRow DR = DV[ix].Row;
                        w.Write('\u0022');
                        w.Write(Convert.ToDateTime(DR["Dated"]).ToShortDateString());
                        w.Write('\u0022');
                        w.Write(',');
                        
                        w.Write('\u0022');
                        w.Write(Convert.ToString(DR["Type"]).Trim());
                        w.Write('\u0022');
                        w.Write(',');

                        w.Write('\u0022');
                        if (Convert.ToInt32(DR["TransTypeID"]) == 1)
                        {
                            w.Write(Convert.ToString(DR["Number"]).Trim()); 
                        }
                        else { 
                            w.Write(Convert.ToString(DR["Reference"]).Trim());
                        }
                        w.Write('\u0022');
                        w.Write(',');
                        
                        w.Write('\u0022');
                        w.Write(Convert.ToString(DR["Amount"]).Trim());
                        w.WriteLine('\u0022');
                    }
                }
            }
            
               w.Flush();
               fs.Close();
            
            } catch (Exception) {
                return false;
            }

            return true;
        }

        private string compileFileDetails(string getWhat, int yrMth, string whichFile)
        {
            List<object> arrParam = new List<object>();
            arrParam.Add(getWhat);
            arrParam.Add(this.clientID);
            arrParam.Add(yrMth);
            if (theLetterDetails.LetterName.ToUpper().Contains("CLIENT")) {
                retrieveClientDetails(arrParam);
            }

            arrParam.Clear();
            //todo: should use contact id here as well for customers with multiple contact
            arrParam.Add(this.theLetterDetails.CustId);
            arrParam.Add(this.theLetterDetails.ClientId);
            retrieveCustomerDetails(arrParam);

            arrParam.Clear();
            arrParam.Add(this.theLetterDetails.UserID);
            arrParam.Add("ALL");
            retrieveUserDetails(arrParam);

            string theFileName="";
            switch (whichFile)
            {
                case "CustDetails":
                    theFileName = templateFilePathRec + "StatHdr.doc";
                    break;

                case "CustDetailsNotes":
                    theFileName = templateFilePathRec + "CNotes.doc";
                    retrieveCustDetailNotes();
                    compileFileNotes();
                    break;
            }

            try
            {
                if (File.Exists(theFileName)) {
                    File.Delete(theFileName);
                }
            }
            catch (Exception exc1)
            {
                return ("ERR: " + exc1.Message);
            }

            FileStream FS  = new FileStream(theFileName, FileMode.Create, FileAccess.Write);
            FS.SetLength(0);
            StreamWriter w  = new StreamWriter(FS);
            
            try
            {
                w.BaseStream.Seek(0, SeekOrigin.Begin);
                //headers
                w.Write('\u0022');
                w.Write("Mth2");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("Mth3");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("Bal");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("Current");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("Mth1");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("Title_ClientName");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("FName_LName");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("Date_Now");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("CustName");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("CustAddress1");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("CustAddress2");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("CustAddress3");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("CustAddress4");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("dateNotified");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("custContact");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("custPhone");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("custFax");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("custCell");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("clientNum");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("custNumber");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("Cell");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("ClientAddress1");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("ClientAddress2");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("ClientAddress3");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("ClientAddress4");
                w.Write('\u0022');
                w.Write(',');
                
                w.Write('\u0022');
                w.Write("ClientFax");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("ClientPhone");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022'); 
                w.Write("CollectionsBankAccount"); 
                w.Write('\u0022'); 
                w.Write(',');

                w.Write('\u0022'); 
                w.Write("EmailStatmentsAddr"); 
                w.Write('\u0022'); 
                w.Write(',');

                w.Write('\u0022');
                w.Write("UserSignature");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("ClientFName");
                w.Write('\u0022');
                w.Write(',');
                
                w.Write('\u0022');
                w.Write("ClientFNameAndLName");
                w.Write('\u0022');
                w.Write(',');

                if (whichFile=="CustDetailsNotes") 
                {
                    w.Write('\u0022');
                    w.Write("Notes");
                    w.Write('\u0022');
                    w.Write(',');
                }

                w.Write('\u0022');
                w.Write("Current_CurrentOrODue");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("ODue_CurrentOrODue");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("custEmail");
                w.Write('\u0022');
                w.Write(',');
                    
                //MSarza [20150730]
                w.Write('\u0022');
                w.Write("legalEntity");
                w.Write('\u0022');
                w.Write(',');

                //MSarza [20150731]
                w.Write('\u0022');
                w.Write("mgtPhone");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("mgtFax");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("mgtEmail");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("mgtWeb");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("mgtPhysicalAddr");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("mgtPostalAddr");
                w.Write('\u0022');
                w.Write(',');
                //--

                w.Write('\u0022');              //MSarza [20150810]
                w.Write("ClientSignature"); 
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("AttnHdr");
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write("DearHdr");
                w.WriteLine('\u0022');



                //records
                w.Write('\u0022');
                if ( theLetterDetails.Month2  == 0 ) { w.Write("$0.00"); }
                else { w.Write(String.Format("{0:c}",theLetterDetails.Month2));}
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                if (theLetterDetails.Month3 == 0 ) {w.Write("$0.00"); } 
                else { w.Write(String.Format("{0:c}", theLetterDetails.Month3)); }
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                if (theLetterDetails.Balance == 0 )  { w.Write("$0.00"); }
                else { w.Write(String.Format("{0:c}", theLetterDetails.Balance)); }
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                if (theLetterDetails.Current == 0)  {  w.Write("$0.00"); }
                else  { w.Write(String.Format("{0:c}", theLetterDetails.Current)); }
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                if (theLetterDetails.Month1 == 0 ) {  w.Write("$0.00"); }
                else { w.Write(String.Format("{0:c}", theLetterDetails.Month1)); }
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                if (theLetterDetails.ClientTitle == null)
                { w.Write(theLetterDetails.ClientName); }
                else if (theLetterDetails.ClientTitle.Length > 0 ) 
                {  w.Write(theLetterDetails.ClientTitle + " " + theLetterDetails.ClientName); }
                else { w.Write(theLetterDetails.ClientName); }
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.ClientFName + " " + theLetterDetails.ClientLName);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                string strx = theLetterDetails.DateStamp.ToShortDateString();
                w.Write(strx);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.CustName);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.CustomerAddress1);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.CustomerAddress2);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.CustomerAddress3);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.CustomerAddress4);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.CustNotifyDate.ToShortDateString());
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.CustContactName);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.CustContactPhone);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.CustContactFax);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.CustContactCell);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.ClientId.ToString());
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.CustNumber.ToString());
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.ClientCell);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.ClientAddress1);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.ClientAddress2);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.ClientAddress3);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.ClientAddress4);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.ClientFax);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.ClientPhone);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022'); 
                w.Write(theLetterDetails.CollectionsBankAccount); 
                w.Write('\u0022'); 
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.EmailStatmentsAddr);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.UserSignature);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.ClientFName);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                if (theLetterDetails.ClientFName == null)
                {}
                else if (theLetterDetails.ClientFName.Length>0)
                { w.Write(theLetterDetails.ClientFName + " "); }
                w.Write(theLetterDetails.ClientLName);
                w.Write('\u0022');
                w.Write(',');


               if (whichFile == "CustDetailsNotes")
               {
                    string note = "";
                    w.Write('\u0022');
                    if (theLetterDetails.CurrentNotes != null)
                    {
                        for (int ix = 0; ix < theLetterDetails.CurrentNotes.Rows.Count; ix++)
                        {
                            System.Data.DataRow DR = theLetterDetails.CurrentNotes.Rows[ix];
                            note += Convert.ToString(DR["Created"]) + "::" + Convert.ToString(DR["Notes"]);
                        }
                    }
                    w.Write(note);
                    w.Write('\u0022');
                    w.Write(',');               
               }

                //Current_CurrentOrODue
                decimal total;
                w.Write('\u0022');
                if (DateTime.Now.Day < 20) {
                    total = theLetterDetails.Current + theLetterDetails.Month1;
                    w.Write(String.Format("{0:c}", total)); 
                }
                else {  w.Write(theLetterDetails.Current); }
                w.Write('\u0022');
                w.Write(',');

                //ODue_CurrentOrODue
                w.Write('\u0022');
                if (DateTime.Now.Day < 20) {
                    total = theLetterDetails.Month2 + theLetterDetails.Month3;
                    w.Write(String.Format("{0:c}", total)); 
                } 
                else {
                    total = theLetterDetails.Month1 + theLetterDetails.Month2 + theLetterDetails.Month3;
                    w.Write(String.Format("{0:c}", total)); 
                }
                w.Write('\u0022');
                w.Write(',');
                
                w.Write('\u0022');
                w.Write(theLetterDetails.CustContactEmail);
                w.Write('\u0022');
                w.Write(',');

                //MSarza [20150730]
                w.Write('\u0022');
                w.Write(theLetterDetails.CffLegalEntity);
                w.Write('\u0022');
                w.Write(',');

                //MSarza [20150731]
                w.Write('\u0022');
                w.Write(theLetterDetails.MgtPhone);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.MgtFax);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.MgtEmail);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.MgtWeb);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.MgtPhysicalAddr);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.MgtPostalAddr);
                w.Write('\u0022');
                w.Write(',');
                //--

                w.Write('\u0022');                              //MSarza [20150810]
                w.Write(theLetterDetails.ClientSignature);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.AttnHeader);
                w.Write('\u0022');
                w.Write(',');

                w.Write('\u0022');
                w.Write(theLetterDetails.DearHeader);
                w.WriteLine('\u0022');

                //end write
                w.Flush();
                w.Close();

                //MSarza [20150806] - commented out; purpose unknown (previous testing maybe)as just outputs a duplicate file
                //if (whichFile == "CustDetails") 
                //{
                //    if (File.Exists(templateFilePathRec + "2MTH.doc"))
                //    { File.Delete(templateFilePathRec + "2MTH.doc"); }
                //    File.Copy(theFileName, templateFilePathRec + "2MTH.doc");
                //    //strDataSrc = theFileName;
                //}

            } catch (Exception) {
                
            }

            return theFileName;
        }

        private void retrieveClientDetails(List<object> arrParam)
        {
            stpCaller stpC = new stpCaller();
            System.Data.DataSet theDS = stpC.executeSPDataSet(arrParam, stpCaller.stpType.GetClientSelectedData);
            if (theDS == null) { }
            else
            {
                System.Data.DataTable DT = theDS.Tables[0];
                if (DT.Rows.Count > 0)
                {
                    theLetterDetails.ClientAddress1 = Convert.ToString(DT.Rows[0]["Address1"]);
                    theLetterDetails.ClientAddress2 = Convert.ToString(DT.Rows[0]["Address2"]);
                    theLetterDetails.ClientAddress3 = Convert.ToString(DT.Rows[0]["Address3"]);
                    theLetterDetails.ClientAddress4 = Convert.ToString(DT.Rows[0]["Address4"]);
                    theLetterDetails.ClientFax = Convert.ToString(DT.Rows[0]["Fax"]);
                    theLetterDetails.ClientPhone = Convert.ToString(DT.Rows[0]["Phone"]);
                    theLetterDetails.ClientCell = Convert.ToString(DT.Rows[0]["Cell"]);
                    theLetterDetails.ClientFName = Convert.ToString(DT.Rows[0]["FName"]);
                    theLetterDetails.ClientLName = Convert.ToString(DT.Rows[0]["LName"]);
                    theLetterDetails.ClientTitle = Convert.ToString(DT.Rows[0]["Title"]);
                }
            }
        }

        private void retrieveCustomerDetails(List<object> arrParam)
        {
            stpCaller stpC = new stpCaller();
            System.Data.DataSet theDS = stpC.executeSPDataSet(arrParam, stpCaller.stpType.GetCustomerDetails);
            if (theDS == null) { }
            else {
                System.Data.DataTable DT = theDS.Tables[0];
                System.Data.DataRow DR;
                if (DT.Rows.Count > 0)
                {
                    DR = DT.Rows[0];
                    this.theLetterDetails.CustomerAddress1 = Convert.ToString(DR["Address1"]);
                    this.theLetterDetails.CustomerAddress2 = Convert.ToString(DR["Address2"]);
                    this.theLetterDetails.CustomerAddress3 = Convert.ToString(DR["Address3"]);
                    this.theLetterDetails.CustomerAddress4 = Convert.ToString(DR["Address4"]);
                    this.theLetterDetails.CustNotifyDate = Convert.ToDateTime(DR["NotifyDate"]);
                }

                if (theDS.Tables.Count > 4)
                { 
                    DT = theDS.Tables[4];
                    DR = DT.Rows[0];
                    this.theLetterDetails.CustContactName = Convert.ToString(DR["ContactName"]);
                    this.theLetterDetails.CustContactFName = Convert.ToString(DR["FName"]);
                    this.theLetterDetails.CustContactLName = Convert.ToString(DR["LName"]);
                    this.theLetterDetails.CustContactPhone = Convert.ToString(DR["Phone"]);
                    this.theLetterDetails.CustContactFax = Convert.ToString(DR["Fax"]);
                    this.theLetterDetails.CustContactCell = Convert.ToString(DR["Cell"]);
                    this.theLetterDetails.CustContactEmail = Convert.ToString(DR["Email"]);
                }
            }
        }

        private void retrieveUserDetails(List<object> arrParam)
        {
            stpCaller stpC = new stpCaller();
            System.Data.DataSet theDS = stpC.executeSPDataSet(arrParam, stpCaller.stpType.GetUserDetails);
            if (theDS == null) { }
            else
            {
                System.Data.DataTable DT = theDS.Tables[0];
                if (DT.Rows.Count > 0)
                {
                    this.theLetterDetails.UserSignature = Convert.ToString(DT.Rows[0]["Signature"]);
                }

            }
        }
       
        private void retrieveCustDetailNotes()
        {
            List<object> arrParam = new List<object>();
            arrParam.Clear();
            arrParam.Add(1);
            arrParam.Add(theLetterDetails.CustId);
            arrParam.Add(theLetterDetails.UserID);
            arrParam.Add(theLetterDetails.YrMth);

            stpCaller stpC = new stpCaller();
            System.Data.DataSet theDS = stpC.executeSPDataSet(arrParam, stpCaller.stpType.GetNotesCurrent);
            if (theDS == null) { }
            else
            {
               System.Data.DataTable DT = theDS.Tables[0];
               theLetterDetails.CurrentNotes = DT;
            }
        }

        private string compileFileStatRpt()
        {
            string theFileName = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePathRecs"] + "StatRpt.doc";
            try
            {
                if (File.Exists(theFileName)) { File.Delete(theFileName); }

                FileStream fs = new FileStream(theFileName, FileMode.Create, FileAccess.Write);
                fs.SetLength(0);

                StreamWriter w = new StreamWriter(fs); //create a Char writer 
                try
                {
                    w.BaseStream.Seek(0, SeekOrigin.Begin);//set the file pointer to the start

                    //Headers/Fields
                    w.Write('\u0022'); w.Write("ClientName"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("customerName"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("Address1"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("Address2"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("Address3"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("Address4"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("custNum"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("monthEnd"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("clientId"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("Balance"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("Month1"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("Month2"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("Month3"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("CollectionsBankAccount"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("Current"); w.Write('\u0022'); w.Write(',');
                    //Msarza [20150805]
                    w.Write('\u0022'); w.Write("legalEntity"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("mgtPhysicalAddr"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("mgtPostalAddr"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("mgtPhone"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("mgtFax"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("mgtWeb"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("mgtEmail"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("mgtPostalAddr1"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("mgtPostalAddr2"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("mgtPostalAddr3"); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write("mgtPostalAddr4"); w.WriteLine('\u0022');              //MSarza - Last heading/field element must terminate with a WriteLine
                    
                    //--

                    //Records
                    w.Write('\u0022'); w.Write(theLetterDetails.ClientName); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.CustName); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.CustomerAddress1); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.CustomerAddress2); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.CustomerAddress3); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.CustomerAddress4); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.CustNumber.ToString()); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.DateAsAt.ToShortDateString()); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.ClientId.ToString()); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(System.String.Format("{0:c}", theLetterDetails.Balance)); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(System.String.Format("{0:c}", theLetterDetails.Month1)); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(System.String.Format("{0:c}", theLetterDetails.Month2)); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(System.String.Format("{0:c}", theLetterDetails.Month3)); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(System.String.Format("{0:c}", theLetterDetails.CollectionsBankAccount)); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(System.String.Format("{0:c}", theLetterDetails.Current)); w.Write('\u0022'); w.Write(',');
                    //MSarza [20150805]
                    w.Write('\u0022'); w.Write(theLetterDetails.CffLegalEntity); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.MgtPhysicalAddr); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.MgtPostalAddr); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.MgtPhone); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.MgtFax); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.MgtWeb); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.MgtEmail); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.MgtPostalAddr1); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.MgtPostalAddr2); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.MgtPostalAddr3); w.Write('\u0022'); w.Write(',');
                    w.Write('\u0022'); w.Write(theLetterDetails.MgtPostalAddr4); w.WriteLine('\u0022');           //MSarza - Last record element must terminate with a WriteLine
                    //--
                    w.Flush();
                    w.Close();
                }
                catch (Exception) { }
                //finally
                //{
                //    w.Close();
                //    fs = null;
                //}
            }
            catch
            { 
                // path may not exist
            }
            return theFileName;
        }

        private void compileFileNotes() 
        {
            string theFileName = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePathRecs"] + "CNotes.doc";
            try
            {
                if (File.Exists(theFileName)) { File.Delete(theFileName); }

                FileStream fs = new FileStream(theFileName, FileMode.Create, FileAccess.Write);
                fs.SetLength(0);

                StreamWriter w = new StreamWriter(fs); //create a Char writer 

                try
                {
                    w.BaseStream.Seek(0, SeekOrigin.Begin);//set the file pointer to the start
                    w.Write('\u0022');
                    w.Write("Dated");
                    w.Write('\u0022');
                    w.Write(',');
                    w.Write('\u0022');
                    w.Write("Notes");
                    w.WriteLine('\u0022');

                    if (theLetterDetails.CurrentNotes.Rows.Count > 0)
                    {
                        for (int ix = 0; ix < theLetterDetails.CurrentNotes.Rows.Count; ix++)
                        {
                            System.Data.DataRow DR = theLetterDetails.CurrentNotes.Rows[ix];
                            w.Write('\u0022');
                            w.Write(Convert.ToString(DR["Created"]));
                            w.Write('\u0022');
                            w.Write(',');
                            w.Write('\u0022');
                            w.Write(Convert.ToString(DR["Notes"]));
                            w.WriteLine('\u0022');
                        }
                    }
                    w.Flush();
                }
                catch (Exception)
                {
                }
                finally
                {
                    w.Close();
                    fs = null;
                }
            }
            catch { 
                // path not exist
            }
        }

        private int getNumLines(string filename) 
        {
            int iRows = 0;
            try 
            { //function only applicable to UTF8/txt files
                System.IO.StreamReader oRead = System.IO.File.OpenText(filename);
                string strLine = oRead.ReadLine(); //remove header
                while (oRead.Peek() != -1)
                {
                    strLine = oRead.ReadLine();
                    iRows += 1;
                }
                oRead.Close();
            } catch (Exception) {
                
            } 
        
            return iRows;
        }

        private bool getTableFromRec(ref string[,] dTable, string fRec, bool getHeader, char delim)
        {
         bool bRet = false;
         int iRows, iCols;
         string[] tColumns;

         try {
            System.IO.StreamReader oRead  = System.IO.File.OpenText(fRec);
            string dStr = "";   
            if (getHeader == false)
            { 
                dStr = oRead.ReadLine();
                dStr = oRead.ReadLine(); 
                iRows = -1;
                while (dStr!=null) {    
                    if (dStr.Length > 0) {
                        iRows += 1;
                        tColumns = dStr.Split(delim);
                        for (iCols = 0; iCols < tColumns.Count(); iCols++)
                        {
                            dStr = tColumns[iCols];
                            if (dStr.Length > 2)
                            {
                                dStr = tColumns[iCols].Substring(1, tColumns[iCols].Length - 2);
                                dTable[iRows, iCols] = dStr.Trim();
                            }
                            else {
                                dTable[iRows, iCols] = ""; 
                            }
                        }
                    }
                    dStr = oRead.ReadLine();
                    if (oRead.Peek() < 0) {
                        break;
                    }
                } //end while
            }
            else
            {
                iRows = 0;
                while (oRead.Peek()>0) {
                    iCols = 0;
                    dStr = oRead.ReadLine();
                    if (dStr == null) {
                        int ix = oRead.Peek();
                        break;
                    }
                    int cLen = dStr.Length - 1;
                    int eIdx = 0;

                    dStr = dStr.Substring(1);
                    while (cLen > 0) {
                        eIdx = dStr.IndexOf("\"\"");
                        dTable[iRows, iCols] = dStr.Substring(0, eIdx);
                        if ((eIdx + 3) > dStr.Length) { break; }
                        dStr = dStr.Substring(eIdx + 3);
                        cLen -= dTable[iRows, iCols].Length + 3;
                        iCols += 1;
                    }
                    iRows += 1;                
                }
            }                
                oRead.Close();
                bRet = true;
          } catch (Exception) {
              bRet = false;
          } 
          return bRet;
        }

        private string cleanFileName(string fileName) 
        {
            fileName = fileName.Replace("&", "and");
            fileName = fileName.Replace("/", "_");
            fileName = fileName.Replace("\\", "");
            fileName = fileName.Replace("(", "");
            fileName = fileName.Replace(")", "");
            fileName = fileName.Replace(":", "");
            fileName = fileName.Replace(",", "");
            fileName = fileName.Replace("?", "");
            fileName = fileName.Replace("@", "at");
            fileName = fileName.Replace(" - ", "_");
            fileName = fileName.Replace(" ", "_");
            fileName = fileName.Replace("__", "_");
            fileName = fileName.Replace(">", "");
            fileName = fileName.Replace("=", "");
            fileName = fileName.Replace("'", "");
            fileName = fileName.Replace("#", "");
            return fileName;
         }
    }
}
