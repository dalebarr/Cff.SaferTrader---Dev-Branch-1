
using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OpenXmlPowertoolsSubset;
//using OxmlOxml = DocumentFormat.OpenXml;
using OxmlWP = DocumentFormat.OpenXml.Wordprocessing;
using OxmlPkg = DocumentFormat.OpenXml.Packaging;
using OxmlDrw =DocumentFormat.OpenXml.Drawing;
using InterOPWord = Microsoft.Office.Interop.Word;


namespace Cff.SaferTrader.Core.Letters
{
    //[Serializable]
    public class IOutputNew
    {
        string returnStatus ="";
        LetterDetails letterDetails;

        string letterheadFilePath = "";
        string signatureLogoFilePath = "";
        short NumImagesToReplaceInDocumentBody = 0;
        string dataInsertionReferenceTagType; 

        string printPdfLibToUse = "";
        string textDelimeter;

        
        OxmlPkg.WordprocessingDocument theDocument;
        
        public string Result
        {
            get { return returnStatus; }
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docBodyDataUpdateReference">The tag item type on the main document that will replaced by LetterDetails members, e.g., merge fields or customer properties</param>
        /// <param name="letterDetails"></param>
        /// <param name="templateFile"></param>
        /// <param name="outputFileNameNoTypeExtension"></param>
        public IOutputNew(LetterDetails letterDetails,//
                            string libToUse,//
                            string letterOutputType, //
                            string outputFileNameNoExt,  //                                                
                            string outputFileDirectory,//
                            string letterTemplateFullPath,//
                            string lettersLibInsertionRefTagType = "",   //                          
                            string htmlOuputDirectory = "",//
                            string letterheadFilePath = "",//
                            string signatureLogoFilePath = "",
                            string printPdfLibToUse = ""//
                            )
        {
            this.letterDetails = letterDetails;
            this.letterheadFilePath = letterheadFilePath;
            this.printPdfLibToUse = printPdfLibToUse;

            if (letterheadFilePath != "" && !letterheadFilePath.ToLower().Contains("do not update"))
            {
                if (File.Exists(letterheadFilePath))
                {
                    this.letterheadFilePath = letterheadFilePath;
                    this.NumImagesToReplaceInDocumentBody += 1;
                }
                //else this.letterheadFilePath = "Error: Letterhead file does not exist.";
                else this.returnStatus  = "Error: Letterhead file does not exist."; 
            }

            if (signatureLogoFilePath != ""  &&  !signatureLogoFilePath.ToLower().Contains("do not update"))
            {
                if (File.Exists(letterheadFilePath))
                {
                    this.signatureLogoFilePath = signatureLogoFilePath;
                    this.NumImagesToReplaceInDocumentBody += 1;
                }
                //else this.signatureLogoFilePath = "Error: Signature logo file does not exist.";
                else this.returnStatus  = "Error: Signature logo file does not exist."; 
            }

            if (lettersLibInsertionRefTagType.ToLower().Contains("textdelimited") || lettersLibInsertionRefTagType.ToLower().Contains("mergefield"))
            {
                this.textDelimeter = lettersLibInsertionRefTagType.Substring(lettersLibInsertionRefTagType.Length - 1);
                this.dataInsertionReferenceTagType = lettersLibInsertionRefTagType.Replace(textDelimeter, "");
            }

            if (!File.Exists(letterTemplateFullPath))
                this.returnStatus  = "Error: Template file does not exist.";

            if (!this.returnStatus.ToLower().Contains("error"))
            switch (libToUse.ToLower())
            {
                case "oxml":
                    this.returnStatus = "";
                    string oxmlDocxOutputFile = outputFileDirectory + outputFileNameNoExt + ".docx";

                    if (System.IO.Path.GetFileName(letterTemplateFullPath).ToLower().Contains(".docx"))
                    {
                        try { File.Copy(letterTemplateFullPath, oxmlDocxOutputFile); }
                        catch (Exception e) { this.returnStatus += "Error: " + e.ToString(); }

                        if (File.Exists(oxmlDocxOutputFile) && (this.returnStatus == ""))
                        {
                            FileInfo fileInfo = new System.IO.FileInfo(oxmlDocxOutputFile);
                            if (fileInfo.IsReadOnly == true)
                                fileInfo.IsReadOnly = false;

                            try { this.theDocument = OxmlPkg.WordprocessingDocument.Open(oxmlDocxOutputFile, true); }
                            catch (Exception e)
                            { 
                                this.returnStatus += "Error: Failed to complete output file; template may be malformed.";
                                break;
                            }

                            this.returnStatus = GenerateLetter_OXML();
                            this.theDocument.MainDocumentPart.Document.Save();
                            this.theDocument.Dispose();

                            if ((this.returnStatus == "") && (letterOutputType.ToLower().Contains("pdf")))
                                this.returnStatus = PrintPdfOutput(oxmlDocxOutputFile);

                            if ((this.returnStatus == "") && (letterOutputType.ToLower().Contains("html")))
                                this.returnStatus = OxmlCreateHtmlOutput(oxmlDocxOutputFile, htmlOuputDirectory);

                            //Make file read only after manipulation? Comment out if not necessary
                            fileInfo.IsReadOnly = true;
                        }

                        else
                            this.returnStatus = "Error: File to update does not exist: " + oxmlDocxOutputFile;

                    }
                    else
                        this.returnStatus += "Error: Incorrect template type. ";
         
                    break;

                case "interop":
                    this.returnStatus = "";
                    this.returnStatus = InteropHelper_CompileDataSource();

                     if (!this.returnStatus.ToLower().Contains("error"))
                         this.returnStatus = GenerateLetter_Interop(this.returnStatus, letterTemplateFullPath, outputFileDirectory, outputFileNameNoExt, letterOutputType, htmlOuputDirectory);                        
                    break;

                default:
                    this.returnStatus = "Error: Library to use not found/implemented.";
                    break;
            }


            //if (this.returnStatus.ToLower().Contains("error"))
            //{
            //    Exception ex = new Exception(this.returnStatus);
            //    throw ex;
            //}  
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GenerateLetter_OXML()
        {
            string result = "";        

            #region Update letterhead
            if (result == "" && this.letterheadFilePath != "" && !this.letterheadFilePath.ToLower().Contains("do not update"))
            {
                if (this.letterheadFilePath.ToLower().Contains("error"))
                    result = this.letterheadFilePath;
                else
                {
                    result = OxmlReplaceImageInHeader();
                    if (result.Contains("No header image to replace was found."))
                    {
                        result = OxmlReplaceImageInBody(this.letterheadFilePath);
                        if (!result.ToLower().Contains("error"))
                        {
                            this.NumImagesToReplaceInDocumentBody += -1;                //20160714 [MSarza]: fixed bug when letterhead image was found in body
                            result = "";
                        }                         
                                                   
                    }
                    else 
                        this.NumImagesToReplaceInDocumentBody += -1;
                } 
            }
            #endregion

            #region Update signature logo, if any
            if (result == "" && this.signatureLogoFilePath != "" && !this.signatureLogoFilePath.ToLower().Contains("do not update"))
            {
                if (this.signatureLogoFilePath.ToLower().Contains("error"))
                    result = this.signatureLogoFilePath;
                else 
                    result = OxmlReplaceImageInBody(this.signatureLogoFilePath, this.NumImagesToReplaceInDocumentBody);
            }
            #endregion

            #region Update tables in the document
            //  Using the table's caption or title property as data insertion point reference.
            //      This reference can be set in the template by going to Table Tools > Layout > Alt Text > Title
            if(result =="") 
            {
                try
                {
                    IEnumerable<OxmlWP.TableProperties> tableProperties = this.theDocument.MainDocumentPart.Document.Body.Descendants<OxmlWP.TableProperties>().Where(tp => tp.TableCaption != null);

                    var parSpacingBL = new OxmlWP.SpacingBetweenLines();
                    parSpacingBL.After = "0";
                    parSpacingBL.Before = "0";
                    parSpacingBL.LineRule = OxmlWP.LineSpacingRuleValues.Auto;

                    var justfctnLft = new OxmlWP.Justification();
                    justfctnLft.Val = OxmlWP.JustificationValues.Left;

                    var justfctnRt = new OxmlWP.Justification();
                    justfctnRt.Val = OxmlWP.JustificationValues.Right;

                    var justfctnCtr = new OxmlWP.Justification();
                    justfctnCtr.Val = OxmlWP.JustificationValues.Center;            
                    

                    foreach (OxmlWP.TableProperties tProp in tableProperties)
                    {
                        #region Update CurrentNotes table
                        // Customer Notes
                        // Customer notes are in a 2 column resultset format on the document, hence implemented here as such.
                        //      The columns are "Created" and "Notes"
                        // As a rule, the table caption to be used as reference is "CurrentNotes"
                        if ((letterDetails.CurrentNotes != null) && (tProp.TableCaption.Val == "CurrentNotes"))
                        {
                            OxmlWP.Table currentNotesTable = (OxmlWP.Table)tProp.Parent;
                            //OxmlWP.TableRow tr = new OxmlWP.TableRow();   // if declared here will not append properly

                            if (letterDetails.CurrentNotes.Rows.Count > 0)
                                foreach (DataRow dr in letterDetails.CurrentNotes.Rows)
                                {
                                    OxmlWP.TableRow tr = new OxmlWP.TableRow();
                                    tr.Append(new OxmlWP.TableCell(new OxmlWP.Paragraph(
                                                                new OxmlWP.ParagraphProperties(justfctnCtr.CloneNode(true), parSpacingBL.CloneNode(true)), 
                                                                new OxmlWP.Run(new OxmlWP.Text(dr["Created"].ToString()))
                                                                )));
                                    tr.Append(new OxmlWP.TableCell(new OxmlWP.Paragraph(
                                                                new OxmlWP.ParagraphProperties(justfctnLft.CloneNode(true), parSpacingBL.CloneNode(true)), 
                                                                new OxmlWP.Run(new OxmlWP.Text(dr["Notes"].ToString()))
                                                                )));
                                    currentNotesTable.AppendChild(tr);
                                    tr.ClearAllAttributes();
                                }
                        }
                        #endregion

                        #region Update Transactions table (Customer Transactions)
                        // Customer transactions are in a 5 column resultset format on the document, hence implemented here as such.
                        //      The columns are "Dated", "Type", "Number" (for the Transaction column), "Reference", and "Amount"
                        // As a rule, the table caption to be used as reference is "Transactions"
                        if ((letterDetails.Transactions != null) && (tProp.TableCaption.Val == "Transactions"))
                        {
                            OxmlWP.Table currentTrnTable = (OxmlWP.Table)tProp.Parent;
                            //OxmlWP.TableRow tr = new OxmlWP.TableRow();

                            if (letterDetails.Transactions.Rows.Count > 0)
                            {
                                DateTime dtt; string sd;
                                foreach (DataRow dr in letterDetails.Transactions.Rows)
                                {
                                    if (Convert.ToInt16(dr["TransTypeID"]) != 5)
                                    {
                                        try
                                        {
                                            dtt = Convert.ToDateTime(dr["Dated"].ToString());
                                            sd = dtt.ToShortDateString();
                                        }
                                        catch (Exception) { 
                                            sd = (dr["Dated"].ToString() + "          ").Substring(0,10);
                                        }
                                       
                                        OxmlWP.TableRow tr = new OxmlWP.TableRow();
                                        tr.Append(new OxmlWP.TableCell(new OxmlWP.Paragraph(
                                                                new OxmlWP.ParagraphProperties(justfctnCtr.CloneNode(true), parSpacingBL.CloneNode(true)),
                                                                new OxmlWP.Run(new OxmlWP.Text(sd))
                                                                )));
                                        tr.Append(new OxmlWP.TableCell(new OxmlWP.Paragraph(
                                                                new OxmlWP.ParagraphProperties(justfctnCtr.CloneNode(true), parSpacingBL.CloneNode(true)),
                                                                new OxmlWP.Run(new OxmlWP.Text(dr["Type"].ToString()))
                                                                )));
                                        tr.Append(new OxmlWP.TableCell(new OxmlWP.Paragraph(
                                                                new OxmlWP.ParagraphProperties(justfctnCtr.CloneNode(true), parSpacingBL.CloneNode(true)),
                                                                new OxmlWP.Run(new OxmlWP.Text(dr["Number"].ToString()))
                                                                )));
                                        tr.Append(new OxmlWP.TableCell(new OxmlWP.Paragraph(
                                                                new OxmlWP.ParagraphProperties(justfctnCtr.CloneNode(true), parSpacingBL.CloneNode(true)),
                                                                new OxmlWP.Run(new OxmlWP.Text(dr["Reference"].ToString()))
                                                                )));
                                        tr.Append(new OxmlWP.TableCell(new OxmlWP.Paragraph(
                                                                new OxmlWP.ParagraphProperties(justfctnRt.CloneNode(true), parSpacingBL.CloneNode(true)),
                                                                //new OxmlWP.Justification() { Val = OxmlWP.JustificationValues.Right},
                                                                new OxmlWP.Run(new OxmlWP.Text(String.Format("{0:c}", dr["Amount"])))
                                                               )));
                                        currentTrnTable.AppendChild(tr);
                                        tr.ClearAllAttributes();
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Update StatementReport table
                        // Statement reports are in a 6 column resultset format on the document, hence implemented here as such.
                        //      The columns are "Dated", "Description", "Invoice", "Reference", "Debits and "Credits"
                        // As a rule, the table caption to be used as reference is "StatementReport"
                        
                        if ((letterDetails.StatementReportRecords != null) && (tProp.TableCaption.Val == "StatementReport"))
                        {
                            OxmlWP.Table currentStatementTable = (OxmlWP.Table)tProp.Parent;
                            //OxmlWP.TableRow tr = new OxmlWP.TableRow();
                            #region Populate table
                                if (letterDetails.StatementReportRecords.Count > 0)                                
                                    for (int i = 0; i < letterDetails.StatementReportRecords.Count(); i++ )
                                    {
                                        // MSarza: Check for TransTypeID <> 5  already done in the StatementReportRecordBuilder class' Build method
                                        if (Convert.ToInt16(letterDetails.StatementReportRecords[i].TransTypeID) != 5)  
                                        {
                                            OxmlWP.TableRow tr = new OxmlWP.TableRow();
                                            tr.Append(new OxmlWP.TableCell(new OxmlWP.Paragraph(
                                                                                new OxmlWP.ParagraphProperties(justfctnCtr.CloneNode(true), parSpacingBL.CloneNode(true)),
                                                                                new OxmlWP.Run(new OxmlWP.Text(letterDetails.StatementReportRecords[i].Dated.Value.ToShortDateString()))                                                                                
                                                                                )));
                                            tr.Append(new OxmlWP.TableCell(new OxmlWP.Paragraph(
                                                                                new OxmlWP.ParagraphProperties(justfctnCtr.CloneNode(true), parSpacingBL.CloneNode(true)),
                                                                                new OxmlWP.Run(new OxmlWP.Text(letterDetails.StatementReportRecords[i].Description.ToString()))
                                                                                )));
                                            tr.Append(new OxmlWP.TableCell(new OxmlWP.Paragraph(
                                                                                new OxmlWP.ParagraphProperties(justfctnCtr.CloneNode(true), parSpacingBL.CloneNode(true)),
                                                                                new OxmlWP.Run(new OxmlWP.Text(letterDetails.StatementReportRecords[i].Number.ToString()))
                                                                                )));      // This is Invoice Number
                                            tr.Append(new OxmlWP.TableCell(new OxmlWP.Paragraph(
                                                                                new OxmlWP.ParagraphProperties(justfctnCtr.CloneNode(true), parSpacingBL.CloneNode(true)),
                                                                                new OxmlWP.Run(new OxmlWP.Text(letterDetails.StatementReportRecords[i].Reference.ToString()))
                                                                                )));
                                            tr.Append(new OxmlWP.TableCell(new OxmlWP.Paragraph(
                                                                                new OxmlWP.ParagraphProperties(justfctnRt.CloneNode(true), parSpacingBL.CloneNode(true)),
                                                                                new OxmlWP.Run(new OxmlWP.Text(String.Format("{0:c}", letterDetails.StatementReportRecords[i].Debits)))
                                                                                )));

                                            tr.Append(new OxmlWP.TableCell(new OxmlWP.Paragraph(
                                                                                new OxmlWP.ParagraphProperties(justfctnRt.CloneNode(true), parSpacingBL.CloneNode(true)), 
                                                                                new OxmlWP.Run(new OxmlWP.Text(String.Format("{0:c}", letterDetails.StatementReportRecords[i].Credits)))
                                                                                )));
                                            currentStatementTable.AppendChild(tr);
                                            tr.ClearAllAttributes();
                                        }
                                    }
                            #endregion

                            #region specify line space adjustment for StatementRecords Template

                                int lsa1 = 0; int lsa2 = 0;
                                int lsa3 = 0; int lsa4 = 0;                                
                                int recsRef = letterDetails.StatementReportRecords.Count;
                                int numP = 1;  
                                //Adjust when template changes
                                int spcrGrp1 = 33; 
                                int spcrGrp2 = 52;
                                int recsPage1 = 13; //Max num of records to breach first page  
                                //-------------------------

                                //determine number of pages
                                if (recsRef > recsPage1) 
                                    numP += 1;
                                while(recsRef > recsPage1)
                                {
                                    if (recsRef > (recsPage1 + spcrGrp2)) //spcrGrp2 is the number of records to breach succeeding pages
                                    {
                                        numP += 1;
                                        recsRef += -spcrGrp2;
                                    }
                                    else break;
                                }

                                letterDetails.EstNumPages = numP.ToString();

                                if (numP > 1)
                                    letterDetails.FooterStatement1 = "Please find and refer to remittance advice on the last page.";
                                

                                //determine the base number of records that can be used to determine spacing group set
                                recsRef = letterDetails.StatementReportRecords.Count;
                                while(recsRef > spcrGrp1) 
                                {
                                    if (recsRef > (spcrGrp2 + spcrGrp1))  
                                    {
                                        recsRef += -spcrGrp2;
                                    }
                                    else break;
                                }


                                //Adjust when template changes
                                if (recsRef < 11)
                                    { lsa1 = 1; lsa2 = 0; lsa3 = 1; lsa4 = 1; }
                                else if (recsRef < 14)
                                    { lsa1 = 0; lsa2 = 0; lsa3 = 0; lsa4 = 0; }
                                else if (recsRef < 26)
                                { lsa1 = 1; lsa2 = 1; lsa3 = 1; lsa4 = 29 - recsRef - lsa1 - lsa2 - lsa3; }
                                else if (recsRef < 29)
                                { lsa1 = 0; lsa2 = 0; lsa3 = 0; lsa4 = 1; }
                                else if (recsRef < 33)
                                { lsa1 = 1; lsa2 = 0; lsa3 = 1; lsa4 = 0; }
                                else if (recsRef < 34)
                                { lsa1 = 0; lsa2 = 0; lsa3 = 1; lsa4 = 0; }

                                else if (recsRef < 63)
                                { lsa1 = 1; lsa2 = 0; lsa3 = 1; lsa4 = 1; }
                                else if (recsRef < 66)
                                { lsa1 = 0; lsa2 = 0; lsa3 = 0; lsa4 = 0; }
                                else if (recsRef < 78)
                                { lsa1 = 1; lsa2 = 1; lsa3 = 1; lsa4 = 81 - recsRef - lsa1 - lsa2 - lsa3; }
                                else if (recsRef < 81)         
                                { lsa1 = 0; lsa2 = 0; lsa3 = 0; lsa4 = 1; }
                                else if (recsRef < 85)
                                { lsa1 = 1; lsa2 = 0; lsa3 = 1; lsa4 = 0; }
                                else if (recsRef < 86)
                                { lsa1 = 0; lsa2 = 0; lsa3 = 1; lsa4 = 0; }

                                else
                                    { lsa1 = 0; lsa2 = 0; lsa3 = 0; lsa4 = 0; }
                                //----------------------

                                letterDetails.LineSpaces1 = lsa1;
                                letterDetails.LineSpaces2 = lsa2;
                                letterDetails.LineSpaces3 = lsa3;
                                letterDetails.LineSpaces4 = lsa4;
                                


                            #endregion
                        }

                        #endregion // Statement Report
                    }
                    //this.theDocument.MainDocumentPart.Document.Save();

                }
                catch (Exception e) { result = "Error: " + e.Message; }
            }
            #endregion

            #region Populate/Insert text data into main document
            if (result == "")
            {
                switch (this.dataInsertionReferenceTagType.ToUpper())
                {
                    case "MERGEFIELD":
                        result = OxmlPopulateMergeFields();
                        //Merge field values are not retained once updated thorough OpenXML due to it not getting persisted
                        //in headers and footers. Hence, as a business rule, text to be replaced in the headers and foorters
                        // will be identified by text delimeters in the config. See constructor code on how delimeter is determined.
                        if (this.textDelimeter == "")
                        {
                            result = "Error: Text delimeter is not set";
                            break;
                        }
                        if (result == "")
                            result = OxmlReplaceDelimitedTextInHeader();
                        if (result == "")
                            result = OxmlReplaceDelimitedTextInFooter();
                        break;

                    case "TEXTDELIMITED":
                        if (this.textDelimeter == "")
                        {
                            this.returnStatus = "Error: Text delimeter is not set";
                            break;
                        }
                        result = OxmlReplaceDelimitedTextInBody();
                        if (result == "")
                            result = OxmlReplaceDelimitedTextInHeader();
                        if (result == "")
                            result = OxmlReplaceDelimitedTextInFooter();
                        break;

                    // Add implementation for other types here when required

                    default:
                        result = "Error: Document data to replace reference data type not defined.";
                        break;
                }
            }
            #endregion

            return result;
        }

        private string OxmlAdjustLineSpacers(string spacerMergeFieldTag, short numSpaces)
        {

            return "";
        }



        private string OxmlCreateHtmlOutput(string sourceFile, string outputDirectory)
        {  
            if (System.IO.Path.GetFileName(sourceFile).ToLower().Contains(".docx"))
            {
                DocxToHtmlConverter dthc = new DocxToHtmlConverter(sourceFile, outputDirectory + System.IO.Path.GetFileName(sourceFile).ToString().Replace(".docx", "") + ".htm");
                if (dthc.ConversionStatus.ToLower().Contains("fault"))
                    return "Error: " + dthc.ConversionStatus;
                else
                    return "";
            }
            else
                return "Error: File to convert to html not of .docx type.";
        }

        private string PrintPdfOutput(string fileToPrint)
        {
            string tmpString;

            if (this.printPdfLibToUse.ToLower() == "foxit")
            {
                // As at 20150910
                //  The way foxit printing works using OpenXmlPowertoolsSubset.PdfPrinting is that it 
                //  should be installed in the machine this application is to run. As below, it prints the
                //  file passed into it to an output directory that is set in the printer's settings itself. In
                //  this application's case, it should be the web.config key=ReportsFolder.
                PdfPrinting dtp = new PdfPrinting();
                tmpString = dtp.PrintUsingFoxIt(fileToPrint);
                if (!tmpString.ToLower().Contains("fault:"))
                    return "";
                else
                    return "Error: " + tmpString;
            }
            else if (this.printPdfLibToUse.ToLower() == "interop")
            {
                Object objFileToPrint = fileToPrint;
                Object objFalse = false;
                //Object objTrue = true;
                Object objMiss = System.Reflection.Missing.Value;
                Object objSaveFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                //Object objWordOpenFormat;          

                if (fileToPrint.Contains(".docx"))
                {
                    //objWordOpenFormat = Microsoft.Office.Interop.Word.WdOpenFormat.wdOpenFormatXMLDocument;
                    tmpString = fileToPrint.Replace(".docx", ".pdf");
                }   
                else if (fileToPrint.Contains(".doc"))
                {
                    //objWordOpenFormat = Microsoft.Office.Interop.Word.WdOpenFormat.wdOpenFormatDocument;
                    tmpString = fileToPrint.Replace(".doc", ".pdf");
                }
                    
                else
                    return "Error: Cannot print pdf using interop; incorrect file type.";

                Object objOutputDocument = tmpString;
                try
                {
                    Microsoft.Office.Interop.Word._Application wordApp = new Microsoft.Office.Interop.Word.Application();
                    Microsoft.Office.Interop.Word._Document wordDoc = wordApp.Documents.Open(ref objFileToPrint,
                                    ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                                    ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss);
                    wordApp.ActiveDocument.SaveAs(ref objOutputDocument, ref objSaveFormat,
                                ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                                ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss);

                    wordDoc.Close(ref objFalse, ref objMiss, ref objMiss);

                    //((Microsoft.Office.Interop.Word._Document)wordApp.ActiveDocument).Close(ref objFalse, ref objMiss, ref objMiss);
                    wordApp.Quit(ref objFalse, ref objMiss, ref objMiss);
                    tmpString = "";
                }
                catch (Exception wp)
                { tmpString = "Error: " + wp.ToString(); }
                
                return tmpString;
            }

            else // use interop as it is defined as the default
                return "Error: For implementation";            
        }

        private string OxmlPopulateMergeFields()
        {
            string fieldDelimeter = " MERGEFIELD ";
            string returnString = "";

            try
            {
                //this.theDocument.ChangeDocumentType(WordprocessingDocumentType.Document);  //not sure if necessary

                //Loop through body of document
                foreach (OxmlWP.FieldCode field in this.theDocument.MainDocumentPart.RootElement.Descendants<OxmlWP.FieldCode>().Where(a => a.Text.Contains(fieldDelimeter)))
                {
                    var fieldNameStart = field.Text.LastIndexOf(fieldDelimeter, System.StringComparison.Ordinal);
                    var fieldname = field.Text.Substring(fieldNameStart + fieldDelimeter.Length).Trim();
                    //var fieldValue = GetMailMergeFieldValues(FieldName: fieldname);
                    string fieldValue = this.letterDetails.GetPropertyValues(fieldname);

                    // Go through all of the Run elements and replace the Text Elements Text Property
                    foreach (OxmlWP.Run run in this.theDocument.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Run>())
                    {
                        foreach (OxmlWP.Text txtFromRun in run.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>().Where(a => a.Text == "«" + fieldname + "»"))
                        {
							if (fieldname.ToString().ToLower().Contains("linespacer"))
							{
								int x;
								switch (fieldname.ToString().ToLower())
								{
								case "linespacer1":
                                    x = this.letterDetails.LineSpaces1;
									break;
								case "linespacer2":
                                    x = this.letterDetails.LineSpaces2;
									break;
								case "linespacer3":
                                    x = this.letterDetails.LineSpaces3;
									break;
								case "linespacer4":
                                    x = this.letterDetails.LineSpaces4;
									break;
								default:
									x = 0;
									break;
								}
								
								while(x > 0)
								{
									run.AppendChild(new OxmlWP.Break());
									x += -1;
								}	

								txtFromRun.Text = fieldValue;								
							}
							else
							{
								if (fieldValue.Contains("\n\b"))
								{
									txtFromRun.Text = fieldValue.Replace("\n\b", "");
									run.AppendChild(new OxmlWP.Break());
								}
								else txtFromRun.Text = fieldValue;
							}
                        }
                    }

                    if (fieldValue.ToLower().Contains("error"))
                        returnString = fieldValue;
                }

                #region Deprecated header and footer mail merge code
                ////////Replacing mail merge elements in header footer does not work when printing or converting as
                ////////it requires re-validation with the data source by word itself. There being no data source, the 
                ////////replacement text gets lost. The alternative is to replace a text directly as implemented in
                //////// new methods called below
                ////////
                ////////Loop through Header (each header on document as it may have different first page header)
                //////foreach (HeaderPart header in theDoc.MainDocumentPart.HeaderParts)
                //////{
                //////    //Loop through each header field
                //////    foreach (FieldCode hdrField in header.RootElement.Descendants<FieldCode>())
                //////    {
                //////        var hdrfieldNameStart = hdrField.Text.LastIndexOf(fieldDelimeter, System.StringComparison.Ordinal);
                //////        var hdrfieldname = hdrField.Text.Substring(hdrfieldNameStart + fieldDelimeter.Length).Trim();
                //////        //var hdrfieldValue = GetMailMergeFieldValues(FieldName: hdrfieldname);
                //////        var hdrfieldValue = this.letterDetails.GetPropertyValues(hdrfieldname);

                //////        try
                //////        {
                //////            foreach (DocumentFormat.OpenXml.Wordprocessing.Run run in header.RootElement.Descendants<DocumentFormat.OpenXml.Wordprocessing.Run>())
                //////            {
                //////                foreach (DocumentFormat.OpenXml.Wordprocessing.Text txtFromRun in run.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>().Where(a => a.Text == "«" + hdrfieldname + "»"))
                //////                {
                //////                    txtFromRun.Text = hdrfieldValue;
                //////                }
                //////            }
                //////        }
                //////        catch (Exception e) { return "Error: " + e.Message + returnStatus; }
                //////    }

                //////    header.Header.Save();
                //////}

                ////////Loop through Footer (each footer on document as it may have different first page footer or odd and even page footers)
                //////foreach (FooterPart footer in theDoc.MainDocumentPart.FooterParts)
                //////{
                //////    //IEnumerable<FieldCode> ftr = footer.RootElement.Descendants<FieldCode>();
                //////    foreach (FieldCode ftrField in footer.RootElement.Descendants<FieldCode>())
                //////    {
                //////        var ftrfieldNameStart = ftrField.Text.LastIndexOf(fieldDelimeter, System.StringComparison.Ordinal);
                //////        var ftrfieldname = ftrField.Text.Substring(ftrfieldNameStart + fieldDelimeter.Length).Trim();
                //////        //var ftrfieldValue = GetMailMergeFieldValues(FieldName: ftrfieldname);
                //////        var ftrfieldValue = this.letterDetails.GetPropertyValues(ftrfieldname);

                //////        try
                //////        {
                //////            //IEnumerable<FooterPart> fp = docGenerated.MainDocumentPart.FooterParts;
                //////            foreach (DocumentFormat.OpenXml.Wordprocessing.Run run in footer.RootElement.Descendants<DocumentFormat.OpenXml.Wordprocessing.Run>())
                //////            {
                //////                //IEnumerable<DocumentFormat.OpenXml.Wordprocessing.Run> hrun = ftrField.Descendants<DocumentFormat.OpenXml.Wordprocessing.Run>();
                //////                foreach (DocumentFormat.OpenXml.Wordprocessing.Text txtFromRun in run.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>().Where(a => a.Text == "«" + ftrfieldname + "»"))
                //////                {
                //////                    txtFromRun.Text = ftrfieldValue;
                //////                }
                //////            }
                //////        }
                //////        catch (Exception e) { return "Error: " + e.Message + returnStatus; }
                //////    }

                //////    footer.Footer.Save();
                //////}
                #endregion

                // If the Document has settings remove them so the end user doesn't get prompted to use the data source
                OxmlPkg.DocumentSettingsPart settingsPart = this.theDocument.MainDocumentPart.GetPartsOfType<OxmlPkg.DocumentSettingsPart>().First();
                var oxeSettings = settingsPart.Settings.Where(a => a.LocalName == "mailMerge").FirstOrDefault();
                if (oxeSettings != null)
                {
                    settingsPart.Settings.RemoveChild(oxeSettings);
                    settingsPart.Settings.Save();
                }

                //Save the document
                //this.theDocument.MainDocumentPart.Document.Save();
                return returnString;
            }
            catch (Exception e) { return "Error: " + e.Message; }
        }

        private string OxmlReplaceDelimitedTextInBody()
        {
            string textToReplace;
            string textReplacement;
            string returnString = "";
            
            try
            {
                foreach (OxmlWP.Document dPart in this.theDocument.MainDocumentPart.Document)
                {
                    foreach (OxmlWP.Text currText in dPart.Descendants<OxmlWP.Text>())
                    {
                        if ((currText.Text.Length > 1) && (currText.Text.Substring(0, 1) == textDelimeter) && (currText.Text.Substring(currText.Text.Length - 1) == textDelimeter))
                        {
                            textToReplace = currText.Text.ToString();
                            currText.Text.Substring(1, currText.Text.Length - 1);
                            textReplacement = this.letterDetails.GetPropertyValues(PropertyName: textToReplace.Replace(textDelimeter, ""));
                            currText.Text = currText.Text.Replace(textToReplace, textReplacement);
                            if (textReplacement.ToLower().Contains("error"))
                                returnString += textReplacement;
                        }
                    }
                }
            }
            catch (Exception e) { return "Error: " + e.Message; } 

            return returnString;
        }

        private string OxmlReplaceDelimitedTextInHeader()
        {           
            string textToReplace;
            string textReplacement;
            string returnString = "";
            short leadIndex = 10;
            short lagIndex = 10;

            try
            {
                foreach (OxmlPkg.HeaderPart hPart in this.theDocument.MainDocumentPart.HeaderParts)
                {
                    foreach (OxmlWP.Text currText in hPart.RootElement.Descendants<OxmlWP.Text>())
                    {
                        if (currText.Text.Length > 2)
                        {
                            if (currText.Text.Substring(0, 1) == this.textDelimeter)
                                leadIndex = 1;
                            if (currText.Text.Substring(1, 1) == this.textDelimeter)
                                leadIndex = 2;
                            if (currText.Text.Substring(currText.Text.Length - 1, 1) == this.textDelimeter)
                                lagIndex = 1;
                            if (currText.Text.Substring(currText.Text.Length - 2, 1) == this.textDelimeter)
                                lagIndex = 2;

                            if (leadIndex < 3 && lagIndex < 3)
                            {
                                textToReplace = currText.Text.Substring(leadIndex, currText.Text.Length - (leadIndex + lagIndex));
                                textReplacement = this.letterDetails.GetPropertyValues(PropertyName: textToReplace);

                                if (textReplacement.ToLower().Contains("error"))
                                    returnString += textReplacement;
                                else
                                    currText.Text = currText.Text.Replace(this.textDelimeter + textToReplace + this.textDelimeter, textReplacement);

                                leadIndex = 10;
                                lagIndex = 10;
                            }
                        }


                        //if (currText.Text.Length > 2) 
                        //{
                        //    if ((currText.Text.Substring(0, 1) == this.textDelimeter) && (currText.Text.Substring(currText.Text.Length - 1, 1) == this.textDelimeter))
                        //    {
                        //        textToReplace = currText.Text.Substring(1, currText.Text.Length - 2);
                        //        textReplacement = this.letterDetails.GetPropertyValues(PropertyName: textToReplace);
                        //        currText.Text = currText.Text.Replace(this.textDelimeter + textToReplace + this.textDelimeter, textReplacement);
                        //    }                           
                        //}
                    }
                }
             }
            catch (Exception e) { return "Error: " + e.Message; } 
            return returnString;
        }

        private string OxmlReplaceDelimitedTextInFooter()
        {
            //string regexMatchRef = this.textDelimeter + "(.*?)" + this.textDelimeter;
            string textToReplace;
            string textReplacement;
            string returnString = "";
            short leadIndex = 10;
            short lagIndex = 10;

            try
            {
                foreach (OxmlPkg.FooterPart fPart in this.theDocument.MainDocumentPart.FooterParts)
                {
                    foreach (OxmlWP.Text currText in fPart.RootElement.Descendants<OxmlWP.Text>())
                    {
                        if (currText.Text.Length > 2)
                        {
                            if (currText.Text.Substring(0, 1) == this.textDelimeter)
                                leadIndex = 1;
                            if (currText.Text.Substring(1, 1) == this.textDelimeter)
                                leadIndex = 2;
                            if (currText.Text.Substring(currText.Text.Length - 1, 1) == this.textDelimeter)
                                lagIndex = 1;
                            if (currText.Text.Substring(currText.Text.Length - 2, 1) == this.textDelimeter)
                                lagIndex = 2;

                            if (leadIndex < 3 && lagIndex < 3)
                            {
                                textToReplace = currText.Text.Substring(leadIndex, currText.Text.Length - (leadIndex + lagIndex));
                                textReplacement = this.letterDetails.GetPropertyValues(PropertyName: textToReplace);

                                if (textReplacement.ToLower().Contains("error"))
                                    returnString += textReplacement;
                                else
                                    currText.Text = currText.Text.Replace(this.textDelimeter + textToReplace + this.textDelimeter, textReplacement);

                                leadIndex = 10;
                                lagIndex = 10;
                            }
                        }



                        //if (currText.Text.Length > 2)
                        //{
                        //    if ((currText.Text.Substring(0, 1) == this.textDelimeter) && (currText.Text.Substring(currText.Text.Length - 1, 1) == this.textDelimeter))
                        //    {
                        //        textToReplace = currText.Text.Substring(1, currText.Text.Length - 2);
                        //        textReplacement = this.letterDetails.GetPropertyValues(PropertyName: textToReplace);
                        //        currText.Text = currText.Text.Replace(this.textDelimeter + textToReplace + this.textDelimeter, textReplacement);
                        //    }
                        //}
                    }
                }
            }
            catch (Exception e) { return "Error: " + e.Message; }
            return returnString;
        }

        private string OxmlReplaceImageInHeader()
        {
            string result = "";
            try
            {
                OxmlPkg.ImagePart imagePart = this.theDocument.MainDocumentPart.HeaderParts.Select(p => p.ImageParts.SingleOrDefault()).FirstOrDefault();
                string x = imagePart.Uri.ToString();

                if (!(string.IsNullOrWhiteSpace(imagePart.Uri.ToString())))
                {
                    //ImagePart imagePart = (ImagePart)theDoc.MainDocumentPart.GetPartById(imageId);
                    byte[] imageBytes = File.ReadAllBytes(this.letterheadFilePath);
                    BinaryWriter writer = new BinaryWriter(imagePart.GetStream());
                    writer.Write(imageBytes);
                    writer.Close();
                }
                else result = "No header image to replace was found.";
            }
            catch (Exception e) 
            { 
                if (e.Message == "Object reference not set to an instance of an object.")
                    return "No header image to replace was found.";
                else
                    return "Error: " + e.Message; 
            }
            return result;
        }

        private string OxmlReplaceImageInBody(string theImageFile, short imgIndexInDoc = 1)
        {
            //index convention used: start or first is 1; if 1 replace first image found, if 2 replace 2nd image, etc.;
            try
            {
                // Get the drawing elements in the document.
                var drawingElements = from run in this.theDocument.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Run>()
                                      where run.Descendants<OxmlWP.Drawing>().Count() != 0
                                      select run.Descendants<OxmlWP.Drawing>().First();

                // Get the blip elements in the drawing elements.
                var blipElements = from drawing in drawingElements
                                   where drawing.Descendants<OxmlDrw.Blip>().Count() > 0
                                   select drawing.Descendants<OxmlDrw.Blip>().First();

                if (blipElements.Count() > 0)
                {
                    //Get the first blip element
                    //Blip blipElement = blipElements.First();
                    OxmlDrw.Blip blipElement = blipElements.Skip(imgIndexInDoc - 1).First();

                    //Set image reference in doc, use it and update the image
                    string imageId = blipElement.Embed.Value;
                    OxmlPkg.ImagePart imagePart = (OxmlPkg.ImagePart)this.theDocument.MainDocumentPart.GetPartById(imageId);
                    byte[] imageBytes = File.ReadAllBytes(theImageFile);
                    BinaryWriter writer = new BinaryWriter(imagePart.GetStream());
                    writer.Write(imageBytes);
                    writer.Close();
                }

                else
                {
                    return "Error: Image to replace in the document was not found.";
                }
            }
            catch (Exception e) { return "Error: " + e.Message + returnStatus; }

            return "";
           
        }

        private string OxmlInsertDataIntoTaggedTable(string theDocFile, string theTableTag) 
        {
            //For implementation
            return "For implementation";
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateFile"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public string GenerateLetter_Interop(string dataSource, string templateFile, string outputFilePath, string fileNameNoExt, string outputType, string htmlOutPath)
        {
            Object objFalse = false;
            Object objTrue = true;
            Object objMiss = System.Reflection.Missing.Value;
            Object objWordSaveFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
            Object objWordOpenFormat = Microsoft.Office.Interop.Word.WdOpenFormat.wdOpenFormatText;
            Object objTemplateFilePath = templateFile;
            Object objOutputDocument;

            Microsoft.Office.Interop.Word._Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word._Document wordDoc = wordApp.Documents.Open(ref objTemplateFilePath,
                            ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                            ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss);

            wordDoc.MailMerge.OpenDataSource(dataSource, ref objWordOpenFormat,
                                    ref objFalse, ref objTrue, ref objFalse, ref objFalse,
                                    ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                                    ref objMiss, ref objMiss, ref objMiss, ref objMiss);


            #region Update letterhead image
            //additional formatting for statements with record merge fields
            bool readStatRec = false;
            bool readCNotes = false;
            string fRecord = "";
            string fNotesRecord = "";
            Microsoft.Office.Interop.Word.Field mField;
            int ix = 1;

            for (; ix < wordDoc.InlineShapes.Count + 1; ix++)
            {
                if (wordDoc.InlineShapes[ix].Type == InterOPWord.WdInlineShapeType.wdInlineShapePicture)
                {
                    if (this.letterheadFilePath.ToLower() != "do not update")
                    {
                        Microsoft.Office.Interop.Word.Range rng = wordDoc.InlineShapes[1].Range;
                        object rngObj = rng;

                        wordDoc.InlineShapes[ix].Select();
                        wordApp.Selection.TypeBackspace();

                        Microsoft.Office.Interop.Word.Shape shp = wordDoc.Shapes.AddPicture(this.letterheadFilePath, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref rngObj);
                    }
                    break;
                }
            }
            #endregion

            #region  // check and update document through comment insertion reference points
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
            }

            if (readStatRec) 
            {
                int iRows = this.letterDetails.Transactions.Rows.Count;
                if (iRows > 0)
                {
                    for (int trows = 1; trows < iRows + 1; trows++)
                    {
                        wordDoc.Tables[1].Rows.Add(ref objMiss);
                    }

                    for (int trows = 1; trows < iRows + 1; trows++)
                    {
                        System.Data.DataRow dr = this.letterDetails.Transactions.Rows[trows - 1];
                        wordDoc.Tables[1].Cell(trows + 1, 1).Range.InsertAfter((Convert.ToDateTime(dr["Dated"].ToString())).ToShortDateString());
                        wordDoc.Tables[1].Cell(trows + 1, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                        wordDoc.Tables[1].Cell(trows + 1, 2).Range.InsertAfter(Convert.ToString(dr["Type"]));
                        wordDoc.Tables[1].Cell(trows + 1, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                        wordDoc.Tables[1].Cell(trows + 1, 3).Range.InsertAfter(Convert.ToString(dr["Number"]));
                        wordDoc.Tables[1].Cell(trows + 1, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                        wordDoc.Tables[1].Cell(trows + 1, 4).Range.InsertAfter(Convert.ToString(dr["Reference"]));
                        wordDoc.Tables[1].Cell(trows + 1, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

                        wordDoc.Tables[1].Cell(trows + 1, 5).Range.InsertAfter(Convert.ToString(dr["Amount"]));
                        wordDoc.Tables[1].Cell(trows + 1, 5).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                    }
                    wordDoc.Tables[1].Rows[1].Range.Bold = 1;

                }
            }


            if (readCNotes)
            { //read Notes record
                for (int trows = 1; trows < this.letterDetails.CurrentNotes.Rows.Count + 1; trows++)
                {
                    wordDoc.Tables[3].Rows.Add(ref objMiss);
                }

                for (int trows = 1; trows < this.letterDetails.CurrentNotes.Rows.Count + 1; trows++)
                {
                    System.Data.DataRow dr = this.letterDetails.CurrentNotes.Rows[trows - 1];
                    wordDoc.Tables[3].Cell(trows + 1, 1).Range.InsertAfter(Convert.ToString(dr["Created"]));
                    wordDoc.Tables[3].Cell(trows + 1, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

                    wordDoc.Tables[3].Cell(trows + 1, 2).Range.InsertAfter(Convert.ToString(dr["Notes"]));
                    wordDoc.Tables[3].Cell(trows + 1, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                }
                wordDoc.Tables[3].Rows[1].Range.Bold = 1;
            }
            #endregion

            #region Generate statement based on old code
            if (fileNameNoExt.ToLower().Contains("statement"))
            {
                for (int sRows = 0; sRows < this.letterDetails.StatementReportRecords.Count(); sRows++)
                {
                    wordDoc.Tables[2].Rows.Add(ref objMiss);
                }

                for (int sr = 0; sr < this.letterDetails.StatementReportRecords.Count(); sr++)
                {
                    if (this.letterDetails.StatementReportRecords[sr].Dated != null)
                    {
                        wordDoc.Tables[2].Cell(sr + 2, 1).Range.InsertAfter(this.letterDetails.StatementReportRecords[sr].Dated.Value.ToShortDateString());
                        wordDoc.Tables[2].Cell(sr + 2, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                    else { wordDoc.Tables[2].Cell(sr + 2, 1).Range.InsertAfter(""); }

                    if (this.letterDetails.StatementReportRecords[sr].Description != null)
                    {
                        wordDoc.Tables[2].Cell(sr + 2, 2).Range.InsertAfter(this.letterDetails.StatementReportRecords[sr].Description);
                        wordDoc.Tables[2].Cell(sr + 2, 2).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                    else { wordDoc.Tables[2].Cell(sr + 2, 2).Range.InsertAfter(""); }


                    if (this.letterDetails.StatementReportRecords[sr].Description.ToLower().Contains("invoice"))
                    {
                        if (this.letterDetails.StatementReportRecords[sr].Number != null)
                        {
                            wordDoc.Tables[2].Cell(sr + 2, 3).Range.InsertAfter(this.letterDetails.StatementReportRecords[sr].Number); //invoice
                            wordDoc.Tables[2].Cell(sr + 2, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        }
                        else { wordDoc.Tables[2].Cell(sr + 2, 3).Range.InsertAfter(""); }

                        wordDoc.Tables[2].Cell(sr + 2, 4).Range.InsertAfter(""); //reference
                        wordDoc.Tables[2].Cell(sr + 2, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                    else
                    {
                        wordDoc.Tables[2].Cell(sr + 2, 3).Range.InsertAfter(""); //invoice 
                        wordDoc.Tables[2].Cell(sr + 2, 3).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                        if (this.letterDetails.StatementReportRecords[sr].Reference != null)
                        {
                            wordDoc.Tables[2].Cell(sr + 2, 4).Range.InsertAfter(this.letterDetails.StatementReportRecords[sr].Reference);
                            wordDoc.Tables[2].Cell(sr + 2, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        }
                        else { wordDoc.Tables[2].Cell(sr + 2, 4).Range.InsertAfter(""); }
                    }


                    wordDoc.Tables[2].Cell(sr + 2, 5).Range.InsertAfter(System.String.Format("{0:c}", this.letterDetails.StatementReportRecords[sr].Debits));
                    wordDoc.Tables[2].Cell(sr + 2, 5).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    wordDoc.Tables[2].Cell(sr + 2, 6).Range.InsertAfter(System.String.Format("{0:c}", this.letterDetails.StatementReportRecords[sr].Credits));
                    wordDoc.Tables[2].Cell(sr + 2, 6).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                }
            }

            #endregion

            wordDoc.MailMerge.SuppressBlankLines = true;
            wordDoc.MailMerge.Destination = Microsoft.Office.Interop.Word.WdMailMergeDestination.wdSendToNewDocument;
            wordDoc.MailMerge.Execute(ref objFalse);

            object documentFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatFilteredHTML;

            if (outputType.ToUpper().Contains("WORD") || outputType.ToUpper().Contains("FAX"))
            {
                objOutputDocument = outputFilePath + fileNameNoExt + ".doc";
                wordApp.ActiveWindow.ActivePane.View.Type = InterOPWord.WdViewType.wdNormalView;    //Makes the word app open the document in Normal view instead of say html view,
                                                                                                    //so as not to hide the header and footer when word file generated is opened?
                wordApp.ActiveDocument.SaveAs(ref objOutputDocument, ref objWordSaveFormat,
                                ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                                ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss);

                if (outputType.ToUpper().Contains("PRINT") || outputType.ToUpper().Contains("FAX"))
                {
                    int htmldivctr = wordApp.ActiveDocument.HTMLDivisions.Count;

                    ix = 1; object objCnt = ix;
                    wordApp.ActiveDocument.Paragraphs[1].Range.Delete(ref objMiss, ref objCnt);
                }

                //Object htmlFilePath = htmlOutPath + fileNameNoExt + ".htm";
                objOutputDocument = htmlOutPath + fileNameNoExt + ".htm";
                wordApp.ActiveDocument.SaveAs(ref objOutputDocument, ref documentFormat,
                            ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                            ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss);
            }
            else if (outputType.ToUpper().Contains("PDF"))
            {
                objOutputDocument = outputFilePath + fileNameNoExt + ".pdf";
                documentFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                wordApp.ActiveDocument.SaveAs(ref objOutputDocument, ref documentFormat,
                            ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss,
                            ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss, ref objMiss);
            }

            wordDoc.Close(ref objFalse, ref objMiss, ref objMiss);

            ((Microsoft.Office.Interop.Word._Document)wordApp.ActiveDocument).Close(ref objFalse, ref objMiss, ref objMiss);
            wordApp.Quit(ref objFalse, ref objMiss, ref objMiss);

            return "";


        }

        private string InteropHelper_CompileDataSource()
        {
            string datasourceDir = System.Configuration.ConfigurationManager.AppSettings["TemplatesFilePathRecs"];
            string interOpSource = datasourceDir + "LetterSource.doc";

            try
            {
                if (File.Exists(interOpSource))
                    File.Delete(interOpSource);
            }
            catch (Exception e)
            {
                return ("Error: " + e.Message);
            }

            FileStream fs = new FileStream(interOpSource, FileMode.Create, FileAccess.Write);
            fs.SetLength(0);
            StreamWriter sw = new StreamWriter(fs);

            try
            {
                sw.BaseStream.Seek(0, SeekOrigin.Begin);

                //setup headers
                sw.Write('\u0022'); sw.Write("custNumber"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("custNum"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("CustName"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("customerName"); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write("CustAddress1"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("CustAddress2"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("CustAddress3"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("CustAddress4"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("Address1"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("Address2"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("Address3"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("Address4"); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write("custContact"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("custPhone"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("custFax"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("custCell"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("custEmail"); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write("clientNum"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("clientId"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("ClientName"); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write("Title_ClientName"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("ClientFNameAndLName"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("FName_LName"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("ClientFName"); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write("ClientAddress1"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("ClientAddress2"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("ClientAddress3"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("ClientAddress4"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("ClientFax"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("ClientPhone"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("Cell"); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write("CollectionsBankAccount"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("EmailStatmentsAddr"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("UserSignature"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("ClientSignature"); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write("legalEntity"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("mgtPhone"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("mgtFax"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("mgtEmail"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("mgtWeb"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("mgtPhysicalAddr"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("mgtPostalAddr"); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write("mgtPostalAddr1"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("mgtPostalAddr2"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("mgtPostalAddr3"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("mgtPostalAddr4"); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write("Mth1"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("Mth2"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("Mth3"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("Bal"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("Month1"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("Month2"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("Month3"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("Balance"); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write("Current"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("Current_CurrentOrODue"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("ODue_CurrentOrODue"); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write("dateNotified"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("AttnHdr"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("DearHdr"); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write("Date_Now"); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write("monthEnd"); sw.WriteLine('\u0022');


                //setup records
                sw.Write('\u0022'); sw.Write(this.letterDetails.CustNumber); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.CustNumber); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.CustName); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.CustName); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write(this.letterDetails.CustomerAddress1); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.CustomerAddress2); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.CustomerAddress3); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.CustomerAddress4); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.CustomerAddress1); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.CustomerAddress2); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.CustomerAddress3); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.CustomerAddress4); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write(this.letterDetails.CustContactName); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.CustContactPhone); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.CustContactFax); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.CustContactCell); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.CustContactEmail); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write(this.letterDetails.ClientId.ToString()); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.ClientId.ToString()); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.ClientName); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022');
                if ((this.letterDetails.ClientTitle.Trim()).Length > 0)
                    sw.Write(this.letterDetails.ClientTitle + " " + this.letterDetails.ClientName);
                else sw.Write(this.letterDetails.ClientName);
                sw.Write('\u0022');
                sw.Write(',');

                sw.Write('\u0022');
                if (this.letterDetails.ClientFName.Length > 0)
                    sw.Write(this.letterDetails.ClientFName + " " + this.letterDetails.ClientLName);
                else sw.Write(this.letterDetails.ClientName);
                sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write(this.letterDetails.ClientFName + " " + this.letterDetails.ClientLName); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.ClientFName); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write(this.letterDetails.ClientAddress1); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.ClientAddress2); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.ClientAddress3); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.ClientAddress4); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.ClientFax); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.ClientPhone); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.ClientCell); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write(this.letterDetails.CollectionsBankAccount); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.EmailStatmentsAddr); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.UserSignature); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.ClientSignature); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write(this.letterDetails.CffLegalEntity); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.MgtPhone); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.MgtFax); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.MgtEmail); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.MgtWeb); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.MgtPhysicalAddr); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.MgtPostalAddr); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write(this.letterDetails.MgtPostalAddr1); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.MgtPostalAddr2); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.MgtPostalAddr3); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.MgtPostalAddr4); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022');
                if (this.letterDetails.Month1 == 0)
                    sw.Write("$0.00");
                else sw.Write(String.Format("{0:c}", this.letterDetails.Month1));
                sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022');
                if (this.letterDetails.Month2 == 0)
                    sw.Write("$0.00");
                else sw.Write(String.Format("{0:c}", this.letterDetails.Month2));
                sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022');
                if (this.letterDetails.Month3 == 0)
                    sw.Write("$0.00");
                else sw.Write(String.Format("{0:c}", this.letterDetails.Month3));
                sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022');
                if (this.letterDetails.Balance == 0)
                    sw.Write("$0.00");
                else sw.Write(String.Format("{0:c}", this.letterDetails.Balance));
                sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022');
                if (this.letterDetails.Month1 == 0)
                    sw.Write("$0.00");
                else sw.Write(String.Format("{0:c}", this.letterDetails.Month1));
                sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022');
                if (this.letterDetails.Month2 == 0)
                    sw.Write("$0.00");
                else sw.Write(String.Format("{0:c}", this.letterDetails.Month2));
                sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022');
                if (this.letterDetails.Month3 == 0)
                    sw.Write("$0.00");
                else sw.Write(String.Format("{0:c}", this.letterDetails.Month3));
                sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022');
                if (this.letterDetails.Balance == 0)
                    sw.Write("$0.00");
                else sw.Write(String.Format("{0:c}", this.letterDetails.Balance));
                sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022');
                if (this.letterDetails.Current == 0)
                    sw.Write("$0.00");
                else sw.Write(String.Format("{0:c}", this.letterDetails.Current));
                sw.Write('\u0022'); sw.Write(',');

                //Current_CurrentOrODue
                decimal total;
                sw.Write('\u0022');
                if (DateTime.Now.Day < 20)
                {
                    total = this.letterDetails.Current + this.letterDetails.Month1;
                    sw.Write(String.Format("{0:c}", total));
                }
                else { sw.Write(this.letterDetails.Current); }
                sw.Write('\u0022'); sw.Write(',');

                //ODue_CurrentOrODue
                sw.Write('\u0022');
                if (DateTime.Now.Day < 20)
                {
                    total = this.letterDetails.Month2 + this.letterDetails.Month3;
                    sw.Write(String.Format("{0:c}", total));
                }
                else
                {
                    total = this.letterDetails.Month1 + this.letterDetails.Month2 + this.letterDetails.Month3;
                    sw.Write(String.Format("{0:c}", total));
                }
                sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write(this.letterDetails.CustNotifyDate.ToString()); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.AttnHeader); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.DearHeader); sw.Write('\u0022'); sw.Write(',');

                sw.Write('\u0022'); sw.Write(DateTime.Now.ToShortDateString()); sw.Write('\u0022'); sw.Write(',');
                sw.Write('\u0022'); sw.Write(this.letterDetails.DateAsAt.ToShortDateString()); sw.WriteLine('\u0022');

                //end write
                sw.Flush();
                sw.Close();

            }
            catch (Exception e)
            {
                return ("Error: " + e.Message);
            }

            return interOpSource;
        }

        private int InterOpHelper_GetNumLinesInFile(string filename)
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
            }
            catch (Exception)
            {

            }

            return iRows;
        }

        private bool InterOpHelper_GetTableFromRec(ref string[,] dTable, string fRec, bool getHeader, char delim)
        {
            bool bRet = false;
            int iRows, iCols;
            string[] tColumns;

            try
            {
                System.IO.StreamReader oRead = System.IO.File.OpenText(fRec);
                string dStr = "";
                if (getHeader == false)
                {
                    dStr = oRead.ReadLine();
                    dStr = oRead.ReadLine();
                    iRows = -1;
                    while (dStr != null)
                    {
                        if (dStr.Length > 0)
                        {
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
                                else
                                {
                                    dTable[iRows, iCols] = "";
                                }
                            }
                        }
                        dStr = oRead.ReadLine();
                        if (oRead.Peek() < 0)
                        {
                            break;
                        }
                    } //end while
                }
                else
                {
                    iRows = 0;
                    while (oRead.Peek() > 0)
                    {
                        iCols = 0;
                        dStr = oRead.ReadLine();
                        if (dStr == null)
                        {
                            int ix = oRead.Peek();
                            break;
                        }
                        int cLen = dStr.Length - 1;
                        int eIdx = 0;

                        dStr = dStr.Substring(1);
                        while (cLen > 0)
                        {
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
            }
            catch (Exception)
            {
                bRet = false;
            }
            return bRet;
        }

      



    }
}
