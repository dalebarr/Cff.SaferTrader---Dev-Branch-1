using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cff.SaferTrader.Core.Letters;
using Cff.SaferTrader.Core.Common;

namespace Cff.SaferTrader.Core.SecurityManager
{
    [Serializable]
    public class CAuth
    {
        private int clientID;
        private Int16 cardType;
        private string trxType;
        private string passKey;
        private string xmlFileName;
        private string exportSettingsFileName;
        private string importSettingsFileName;
        private string crdTrxFrom;
        private string crdTrxTo;
        private string strIDs;

        public CAuth()
        {}

        public CAuth(int inCID, string inPkey, Int16 inCType, string inTType, string inTrxFrom, string inTrxTo, string inStrIDs)
        {
            this.clientID = inCID;
            this.passKey = inPkey;
            this.cardType = inCType;
            this.trxType = inTType;
            this.crdTrxFrom = inTrxFrom;
            this.crdTrxTo = inTrxTo;
            this.strIDs = inStrIDs;
        }

        public bool isAuthenticated() 
        {
            //authenticate user based on clientid and passkey
            return true;
        }

        public bool isAuthenticated(bool genMYOBXML) 
        {
            string ftpPath = System.Configuration.ConfigurationManager.AppSettings["ftpFolder"];
            string dateStamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0')
                                           + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.TimeOfDay.Hours.ToString().PadLeft(2, '0') +
                                           DateTime.Now.TimeOfDay.Minutes.ToString().PadLeft(2, '0');

            string strMsg = "authenticating started at [" + dateStamp + "]" + Environment.NewLine;
            if (System.Configuration.ConfigurationManager.AppSettings["DebugOn"] == "true") {
                System.IO.File.WriteAllText(ftpPath + "debug.txt", strMsg);
            }

            try {
                if (genMYOBXML)
                {   //authenticate user based on clientid and passkey
                    string strEncType = StringEnum.GetStringValue(MYOB_ENCTYPES.TRIPLE_DES);
                    string strKey = "";
                    //string strDeciphered = "";
                    string[] strCiphered=null;
                    byte[] rjkey=null;
                    byte[] rjIV=null;
                    object objKey = null;

                    Letters.stpCaller sCaller = new Letters.stpCaller();
                    List<object> arrParams = new List<object>();
                    arrParams.Add("FindPasskey");
                    arrParams.Add(clientID);
                    arrParams.Add(0); //0-Export, 1-Import
                    arrParams.Add(0);
                    arrParams.Add(passKey);
                    System.Data.DataSet theDS = sCaller.executeSPDataSet(arrParams, Letters.stpCaller.stpType.authenticateUser);
                    if (theDS == null)
                    { return false; }

                    if (theDS.Tables[0].Rows.Count == 0)
                    { return false; }

                    if (theDS.Tables.Count > 0 && theDS.Tables[1].Rows.Count > 0) {
                        strEncType = theDS.Tables[1].Rows[0]["keyTypeDesc"].ToString();

                        if (theDS.Tables[1].Rows[0]["Key1"] != null)
                        {
                            strKey = theDS.Tables[1].Rows[0]["Key1"].ToString();
                        }
                        
                        if (strEncType.ToUpper() == StringEnum.GetStringValue(MYOB_ENCTYPES.RIJNDAEL))
                        {
                            objKey = theDS.Tables[1].Rows[0]["Key2"];
                            rjkey = (byte[])objKey;

                            objKey = theDS.Tables[1].Rows[0]["Key3"];
                            rjIV = (byte[])objKey;
                        }
                    }

                    if (System.Configuration.ConfigurationManager.AppSettings["DebugOn"] == "true"){
                        System.IO.File.WriteAllText(ftpPath + "debug.txt", "passkey found for " + clientID.ToString() + Environment.NewLine);
                    }

                    //user exists, retrieve cff data
                    exportXML xmlExport = new exportXML(clientID, cardType, trxType, crdTrxFrom, crdTrxTo, strIDs);
                    if (xmlExport.retrieveData())
                    {  //generate myobxml
                        dateStamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0')
                                           + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.TimeOfDay.Hours.ToString().PadLeft(2, '0') +
                                           DateTime.Now.TimeOfDay.Minutes.ToString().PadLeft(2, '0');
                      
                        //retrieve the keys from exkeys table - always encrypt this for security reason
                        BLL.CryptoClass xCrypt = new BLL.CryptoClass(strEncType, rjkey, rjIV, "");
                        if (strEncType ==  StringEnum.GetStringValue(MYOB_ENCTYPES.TRIPLE_DES)){strEncType="0"; } 
                        else { strEncType = "1"; }
                        this.xmlFileName = clientID.ToString() + "_" + dateStamp + "_" + strEncType + ".txt"; //+ ".zip"; //
                        this.xmlFileName = this.xmlFileName.Replace(" ", "");
                        string xmlPath = ftpPath + this.xmlFileName;
                        
                        string XMLData = xmlExport.createXMLData().ToString();
                        bool isWritten = xCrypt.Encrypt(XMLData, xmlPath, ref strCiphered); //write encrypted text to file
                        xmlPath = BLL.Compression.CompressFile(xmlPath, true); //compress to zip file
                        this.xmlFileName += ".zip";
                        return isWritten;
                    }

                }
                else
                { //authenticate user without generating MYOB File
                    Letters.stpCaller sCaller = new Letters.stpCaller();
                    List<object> arrParams = new List<object>();
                    arrParams.Add("FindPasskey");
                    arrParams.Add(clientID);
                    arrParams.Add(0); //0-Export, 1-Import
                    arrParams.Add(0);
                    arrParams.Add(passKey);
                    System.Data.DataSet theDS = sCaller.executeSPDataSet(arrParams, Letters.stpCaller.stpType.authenticateUser);
                    if (theDS == null)
                    { return false; }
                        
                    if (theDS.Tables[0].Rows.Count == 0)
                    { return false; }

                    if (theDS.Tables.Count > 0 && theDS.Tables[1].Rows.Count > 0)
                    {
                        return true;
                    }
                }
            } 
            catch (Exception exc) 
            {
                if (System.Configuration.ConfigurationManager.AppSettings["DebugOn"] == "true") 
                {
                    strMsg += "Exception encountered as of [" + dateStamp + "] - " + exc.Message + Environment.NewLine;
                    System.IO.File.WriteAllText(ftpPath + "debug.txt", strMsg);
                }

                return false;
            }
           

            return true;
        }

        private Int16 retrieveRjKeyType(string trxType, ref Int16 encType)
        {//CFFWEB-6; CFFIMEX-7;
            if (trxType.ToLower().IndexOf("rj", 0) >= 0)
            {
                encType = (Int16) MYOB_ENCTYPES.RIJNDAEL;
                if (trxType.ToLower().IndexOf("rji", 0) >= 0)
                {
                    return (Int16) MYOB_TRXTYPE.IMPORT;
                }
                else
                {
                    return (Int16) MYOB_TRXTYPE.EXPORT;
                }
            }
            else {
                encType = (Int16) MYOB_ENCTYPES.TRIPLE_DES; //3des encryption
                return 0;
            }
        }

       
        public bool UpdateKey(string sAction)
        { //CFFWEB-6; CFFIMEX-7; {LogOn.aspx?ComID=...ttype=KUPDARj...}

            Int16 encType = (Int16) MYOB_ENCTYPES.TRIPLE_DES;
            Int16 importEType = (Int16) MYOB_TRXTYPE.IMPORT;
            Int16 keyType = (Int16) MYOB_TRXTYPE.EXPORT;   //export=0, import=1
            
            char ctype = trxType.Substring(4, 1)[0];
            if (char.IsNumber(ctype))
            { //3DES
                switch ((MYOB_TRXTYPE)Convert.ToInt16(ctype.ToString()))
                {
                    case MYOB_TRXTYPE.EXPORT: 
                        keyType = (Int16) MYOB_TRXTYPE.EXPORT;   
                        break;

                    case MYOB_TRXTYPE.IMPORT: 
                        keyType = (Int16) MYOB_TRXTYPE.IMPORT;
                        break;

                    default:
                        return false;
                }

                retrieveRjKeyType(trxType, ref encType);
            } 
            else if (ctype == 'A')
            { //both export and import keys
                    string[] strdummy = trxType.Split(':');
                    keyType = retrieveRjKeyType(strdummy[1], ref encType);
                    if (strdummy.Length > 2)
                    {
                        retrieveRjKeyType(strdummy[2], ref importEType);
                    }
            } 
            else 
            {
                //unsupported ctype
                return false;
            }

            //update ExKeyUpdateHist DB where clientID and keytype
            Letters.stpCaller sCaller = new Letters.stpCaller();
            List<object> arrParams = new List<object>();
            arrParams.Add("TriggerUpdateKey");
            arrParams.Add(clientID);
            arrParams.Add(keyType);     //0-Export, 1-Import
            arrParams.Add(1);           //trigger update key
            arrParams.Add(encType);     //encryption type
            arrParams.Add(DateTime.Now);
            int retVal = sCaller.executeSP(arrParams, Letters.stpCaller.stpType.TriggerUpdateKey);

            if (ctype == 'A')
            {  //if AllKeys, do update for import key as well
                System.Threading.Thread.Sleep(30); //let the stp execution above pass a bit
                sCaller = new Letters.stpCaller();
                arrParams = new List<object>();
                arrParams.Add("TriggerUpdateKey");
                arrParams.Add(clientID);
                arrParams.Add(1);           //1-Import key type
                arrParams.Add(1);           //trigger update key
                arrParams.Add(importEType); //encryption type
                arrParams.Add(DateTime.Now);
                retVal = sCaller.executeSP(arrParams, Letters.stpCaller.stpType.TriggerUpdateKey);
            }

            return true;
        }

        public bool TriggerKeySync(string strTType)
        {
            //update ExKeyUpdateHist DB where clientID and keytype
            Letters.stpCaller sCaller = new Letters.stpCaller();
            List<object> arrParams = new List<object>();
            arrParams.Add("TriggerKeySync" + strTType.Substring(5,4));
            arrParams.Add(clientID);
            arrParams.Add(-1);          //0-Export, 1-Import, -1=all
            arrParams.Add(3);           //triggerID, 3-trigger keysync
            arrParams.Add(-1);          //encryption type, -1=unknown
            arrParams.Add(DateTime.Now);
            int retVal = sCaller.executeSP(arrParams, Letters.stpCaller.stpType.TriggerUpdateKey);
            return true;
        }

        public void NotifyClientStatus(int _clientID, string status)
        {
            Letters.stpCaller sCaller = new Letters.stpCaller();
            List<object> arrParams = new List<object>();
            arrParams.Add(status);                      //status
            arrParams.Add(_clientID);                   //clientID
            arrParams.Add("");                          //fileName
            arrParams.Add(DateTime.Now);                //timeStamp
            int retVal = sCaller.executeSP(arrParams, Letters.stpCaller.stpType.ClientNotification);
        }

        public void NotifyFTPUpload(string fileName)
        {
            string[] strDummy = fileName.Split('_');
            string strDtTime = strDummy[1].Split('.')[0];
            strDtTime =  strDtTime.Substring(6, 2) + "/" + strDtTime.Substring(4, 2) + "/"  + strDtTime.Substring(0, 4) + " " + strDtTime.Substring(8, 2) + ":" + strDtTime.Substring(10, 2);

            Letters.stpCaller sCaller = new Letters.stpCaller();
            List<object> arrParams = new List<object>();
            arrParams.Add("UPLOAD");                        //status
            arrParams.Add(Convert.ToInt32(strDummy[0]));    //clientID
            arrParams.Add(fileName);                        //fileName
            arrParams.Add(Convert.ToDateTime(strDtTime));   //timeStamp
            int retVal = sCaller.executeSP(arrParams, Letters.stpCaller.stpType.ClientNotification);
        }

        public bool RetrieveKeys()
        {
            //retrieve file settings keys
            char ctype = trxType.Substring(4, 1)[0];
            byte[] bKey =null;
            object objKey = null;

            string ftpPath = System.Configuration.ConfigurationManager.AppSettings["ftpFolder"];
            string dateStamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0')
                                            + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.TimeOfDay.Hours.ToString().PadLeft(2, '0')
                                                + DateTime.Now.TimeOfDay.Minutes.ToString().PadLeft(2, '0');


            Letters.stpCaller sCaller = new Letters.stpCaller();
            List<object> arrParams = new List<object>();
            arrParams.Add("GetKeys");
            arrParams.Add(clientID);
            arrParams.Add(ctype);       //0-Export, 1-Import
            arrParams.Add(-1);          //triggerID 
            arrParams.Add(-1);          //encType
            arrParams.Add(DateTime.Now);
            System.Data.DataSet theDS = sCaller.executeSPDataSet(arrParams, Letters.stpCaller.stpType.TriggerUpdateKey);
            if (theDS != null) {
                for (int ix = 0; ix < theDS.Tables[0].Rows.Count; ix++)
                {
                    objKey = theDS.Tables[0].Rows[ix]["KeySettings"];
                    bKey = (byte[])objKey;

                    Int16 kType = Convert.ToInt16(theDS.Tables[0].Rows[ix]["KeyType"]);
                    if (kType == (Int16) MYOB_TRXTYPE.EXPORT) 
                    {
                        exportSettingsFileName = clientID.ToString() + "_E" + dateStamp + ".txt";
                        if (System.IO.File.Exists(ftpPath + exportSettingsFileName)) { System.IO.File.Delete(ftpPath + exportSettingsFileName); }
                        System.IO.File.WriteAllBytes(ftpPath + exportSettingsFileName, bKey);
                    }
                    else if (kType == (Int16) MYOB_TRXTYPE.IMPORT) 
                    {
                        importSettingsFileName = clientID.ToString() + "_I" + dateStamp + ".txt";
                        if (System.IO.File.Exists(ftpPath + importSettingsFileName)) { System.IO.File.Delete(ftpPath + importSettingsFileName); }
                        System.IO.File.WriteAllBytes(ftpPath + importSettingsFileName, bKey);
                    }
                }
            }
            
            return true;
        }

        public void DeleteFiles()
        { //note: reused variable crdTrxFom, crdTrxTo as exportsettingsfile, importsettingsfile 
            try
            {
                string ftpPath = System.Configuration.ConfigurationManager.AppSettings["ftpFolder"];
                if (System.IO.File.Exists(ftpPath + "\\" + crdTrxFrom.Trim()))
                {
                    System.IO.File.Delete(ftpPath + "\\" + crdTrxFrom.Trim());
                }

                if (System.IO.File.Exists(ftpPath + "\\" + crdTrxTo.Trim()))
                {
                    System.IO.File.Delete(ftpPath + "\\" + crdTrxTo.Trim());
                }

            } 
            catch (Exception)
            {
                return;
            }

            return;
        }

        public string getLastImportedFile(string strClientID, string theDir)
        {
            return (_searchMaxFTPFile(theDir, ".zip", strClientID));
        }

        public string getXMLFileName()
        {
            return this.xmlFileName;
        }


        public string getExportSettingsFName()
        {
            return this.exportSettingsFileName;
        }

        public string getImportSettingsFName()
        {
            return this.importSettingsFileName;
        }


        private string _searchMaxFTPFile(string directory, string ext, string strClient)
        {
            string[] strFiles = new string[]{"",""};
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(directory); 
            System.IO.FileInfo[] fiAr  = di.GetFiles(strClient + "*" + ext);
            
            if (fiAr.Length > 0)
            {
                for (int ix=0; ix<fiAr.Length; ix++)
                {
                    System.IO.FileInfo fi = fiAr[ix];
                    if (fi.Extension.Trim().ToLower() == ext)
                    {
                        DateTime dtModified = System.IO.File.GetCreationTime(fi.FullName);
                        if (string.IsNullOrEmpty(strFiles[0]))
                        {
                            strFiles[0] = fi.FullName;
                            strFiles[1] = fi.Name;
                        }
                        else
                        {
                            if (dtModified > System.IO.File.GetLastWriteTime(strFiles[0]))
                            {
                                strFiles[0] = fi.FullName;
                                strFiles[1]=fi.Name;
                            }
                        }
                    } //end if
                } //end for
            } //end if
           
            return strFiles[1];
        }

    }
}
