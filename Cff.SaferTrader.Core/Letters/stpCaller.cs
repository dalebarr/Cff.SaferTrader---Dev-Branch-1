using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Letters
{
    [Serializable]
    public class stpCaller
    {
        public enum stpType
        {
            GetClientSelectedData = 0,
            GetCustomerDetails=1,
            GetCustomerInfo = 2,
            GetTransactions = 3,
            GetNotesCurrent = 4,
            GetUserDetails = 5,
            InsUpdCustContForValid = 6,
            InsertLetterSent = 7,
            InsertCurrentNotes = 8,
            UpdateCurrentNotes = 9,
            authenticateUser = 10,
            cff2MYOB = 11,
            TriggerUpdateKey = 12,
            ClientNotification = 13,
            CashFlowFundingRequest = 14,
            CashFlowFundingInterestReq = 15,
            otherStoredProc = 16,
            smsMessageIn = 17,
            UpdClientContacts = 18,
            InsUpdateCliContact = 19,
            AddCffCustomerApplication = 20,
            AddCffCustomerApplicationDirectors = 21,
            Letters_GetCustomerLetterDetails = 22,
            InsertNotesPermanent = 23
        };

        private static readonly string _connectionString =
              System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

        public stpCaller()
        { }

        public CleverReader executeSPReader(List<object> stpParameters, stpType storedProcType)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          System.Data.CommandType.StoredProcedure,
                                                                          storedProcName(storedProcType),
                                                                          GenerateClientParameters(stpParameters, storedProcType)))
                {
                    CleverReader reader = new CleverReader(dataReader);
                    return reader;
                }
            }
        }

        public System.Data.DataSet executeSPDataSet(List<object> stpParameters, stpType storedProcType)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (System.Data.DataSet DS = SqlHelper.ExecuteDataset(connection,
                                                                          System.Data.CommandType.StoredProcedure,
                                                                          storedProcName(storedProcType),
                                                                          GenerateClientParameters(stpParameters, storedProcType)))
                {
                    return DS;
                }
            }
        }

        public int executeSP(List<object> stpParameters, stpType storedProcType) {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                int retVal = SqlHelper.ExecuteNonQuery(connection, storedProcName(storedProcType), 
                                                                        GenerateClientParameters(stpParameters, storedProcType));
                return retVal;
            }
        }
        
        private static SqlParameter[] GenerateClientParameters(List<object> arrParameter, stpType storedProcType)
        {
            SqlParameter clientIdParameter;
            SqlParameter customerIDParameter;
            SqlParameter yrMthParameter;
            SqlParameter userID;
            SqlParameter sAction;
            SqlParameter notesid;
            SqlParameter notes;
            SqlParameter sendr;
            SqlParameter recvr;
            SqlParameter msg;
            SqlParameter op;
            SqlParameter msgType;
            SqlParameter reference;
            SqlParameter stats;
            SqlParameter empId;
            SqlParameter createDate;             //MSarza[20160922]
            SqlParameter modifyDate;             //MSarza[20160922]
            SqlParameter yrMthDayParameter;     //MSarza[20160922]
            //SqlParameter modi;
            

            switch (storedProcType)
            {
                case stpType.GetClientSelectedData:
                    SqlParameter getWhatParameter = new SqlParameter("getWhat", System.Data.SqlDbType.VarChar);
                    getWhatParameter.Value = arrParameter[0];

                    clientIdParameter = new SqlParameter("clientId", System.Data.SqlDbType.Int);
                    clientIdParameter.Value = arrParameter[1];

                    yrMthParameter = new SqlParameter("yrMth", System.Data.SqlDbType.VarChar);
                    yrMthParameter.Value = arrParameter[2];

                    return new[] { getWhatParameter, clientIdParameter, yrMthParameter };

                case stpType.GetCustomerDetails:
                    customerIDParameter = new SqlParameter("CustID", System.Data.SqlDbType.Int);
                    customerIDParameter.Value = arrParameter[0];

                    clientIdParameter = new SqlParameter("ClientID", System.Data.SqlDbType.Int);
                    clientIdParameter.Value = arrParameter[1];
                    return new[] { clientIdParameter, customerIDParameter };

                case stpType.GetCustomerInfo:
                    customerIDParameter = new SqlParameter("customerId", System.Data.SqlDbType.Int);
                    customerIDParameter.Value = arrParameter[0];
                    return new[] { customerIDParameter};

                case stpType.GetTransactions:
                    customerIDParameter = new SqlParameter("CustId", System.Data.SqlDbType.Int);
                    customerIDParameter.Value = arrParameter[0];

                    SqlParameter whichTrns = new SqlParameter("whichTrns", System.Data.SqlDbType.SmallInt);
                    whichTrns.Value = arrParameter[1];

                    SqlParameter AsAt = new SqlParameter("AsAt", System.Data.SqlDbType.Int);
                    AsAt.Value = arrParameter[2];

                    SqlParameter YrMthNow = new SqlParameter("YrMthNow", System.Data.SqlDbType.Int);
                    YrMthNow.Value = arrParameter[3];

                    SqlParameter ArchiveLen = new SqlParameter("ArchiveLen", System.Data.SqlDbType.SmallInt);
                    ArchiveLen.Value = arrParameter[4];

                    SqlParameter dtDateAsAt = new SqlParameter("dtDateAsAt", System.Data.SqlDbType.DateTime);
                    dtDateAsAt.Value = arrParameter[5];
                    return new[] { customerIDParameter, whichTrns, AsAt, YrMthNow, ArchiveLen, dtDateAsAt};

                case stpType.GetNotesCurrent:
                    SqlParameter whichNotes = new SqlParameter("WhichNotes", System.Data.SqlDbType.SmallInt);
                    whichNotes.Value = arrParameter[0];

                    customerIDParameter = new SqlParameter("CustID", System.Data.SqlDbType.BigInt);
                    customerIDParameter.Value = arrParameter[1];

                    userID = new SqlParameter("userID", System.Data.SqlDbType.Int);
                    userID.Value = arrParameter[2];

                    yrMthParameter = new SqlParameter("YrMth", System.Data.SqlDbType.Int);
                    yrMthParameter.Value = arrParameter[3];

                    return new[] { whichNotes, customerIDParameter, userID, yrMthParameter };

                case stpType.GetUserDetails:
                    userID = new SqlParameter("userID", System.Data.SqlDbType.BigInt);
                    userID.Value = arrParameter[0];

                    sAction = new SqlParameter("sAction", System.Data.SqlDbType.VarChar);
                    sAction.Value = arrParameter[1];

                    return new[] { userID, sAction};

                case stpType.InsUpdCustContForValid:
                    sAction = new SqlParameter("sAction", System.Data.SqlDbType.VarChar);
                    sAction.Value = arrParameter[0];

                    SqlParameter custContactsID = new SqlParameter("custContactsID", System.Data.SqlDbType.BigInt);
                    custContactsID.Value = arrParameter[1];

                    customerIDParameter = new SqlParameter("CustID", System.Data.SqlDbType.BigInt);
                    customerIDParameter.Value = arrParameter[2];

                    SqlParameter LName = new SqlParameter("LName", System.Data.SqlDbType.VarChar);
                    LName.Value = arrParameter[3];

                    SqlParameter FName = new SqlParameter("FName", System.Data.SqlDbType.VarChar);
                    FName.Value = arrParameter[4];

                    SqlParameter Role = new SqlParameter("Role", System.Data.SqlDbType.VarChar);
                    Role.Value = arrParameter[5];

                    SqlParameter Phone = new SqlParameter("Phone", System.Data.SqlDbType.VarChar);
                    Phone.Value = arrParameter[6];
                    
                    SqlParameter Fax = new SqlParameter("Fax", System.Data.SqlDbType.VarChar);
                    Fax.Value = arrParameter[7];
                    
                    SqlParameter Email = new SqlParameter("Email", System.Data.SqlDbType.VarChar);
                    Email.Value = arrParameter[8];
                    
                    SqlParameter Cell = new SqlParameter("Cell", System.Data.SqlDbType.VarChar);
                    Cell.Value = arrParameter[9];

                    SqlParameter isDefault = new SqlParameter("isDefaultContact", System.Data.SqlDbType.Bit);
                    isDefault.Value = arrParameter[10];

                    SqlParameter modified  = new SqlParameter("Modified", System.Data.SqlDbType.DateTime);
                    modified.Value = arrParameter[11];

                    SqlParameter attn = new SqlParameter("Attn", System.Data.SqlDbType.Bit);
                    attn.Value = arrParameter[12];

                    SqlParameter modifiedBy = new SqlParameter("ModifiedBy", System.Data.SqlDbType.SmallInt);
                    modifiedBy.Value = arrParameter[13];

                    SqlParameter emailStatement = new SqlParameter("emailStatement", System.Data.SqlDbType.Bit);
                    emailStatement.Value = arrParameter[14];

                    SqlParameter emailReceipt = new SqlParameter("emailReceipt", System.Data.SqlDbType.SmallInt);
                    emailReceipt.Value = arrParameter[15];

                    return new[] { sAction, custContactsID, customerIDParameter, LName, FName, Role, Phone, Fax, Email, 
                                    Cell, isDefault, modified, attn, modifiedBy, emailStatement, emailReceipt };

                case stpType.InsertLetterSent: /*insert letter sent*/
                    clientIdParameter = new SqlParameter("clientId", System.Data.SqlDbType.Int);
                    clientIdParameter.Value = arrParameter[0];

                    customerIDParameter = new SqlParameter("custID", System.Data.SqlDbType.BigInt);
                    customerIDParameter.Value = arrParameter[1];

                    SqlParameter ltrType = new SqlParameter("letterType", System.Data.SqlDbType.SmallInt);
                    ltrType.Value = arrParameter[2];

                    SqlParameter ltrName = new SqlParameter("letterName", System.Data.SqlDbType.VarChar);
                    ltrName.Value = arrParameter[3];

                    SqlParameter dated = new SqlParameter("dated", System.Data.SqlDbType.DateTime);
                    dated.Value = arrParameter[4];

                    yrMthParameter = new SqlParameter("yrMth", System.Data.SqlDbType.BigInt);
                    yrMthParameter.Value = arrParameter[5];

                    modifiedBy = new SqlParameter("modifiedBy", System.Data.SqlDbType.BigInt);
                    modifiedBy.Value = arrParameter[6];

                    SqlParameter fileName = new SqlParameter("fileName", System.Data.SqlDbType.VarChar);
                    fileName.Value = arrParameter[7].ToString().Trim();

                    notes = new SqlParameter("currentNotes", System.Data.SqlDbType.Text);       //MSarza[20160922
                    notes.Value = arrParameter[8];                                              //MSarza[20160922

                    SqlParameter retVal = new SqlParameter("retVal", System.Data.SqlDbType.VarChar);
                    retVal.Value = "";

                    return new[] { clientIdParameter, customerIDParameter, ltrType, ltrName, dated, yrMthParameter, modifiedBy, fileName, notes, retVal};

                case stpType.InsertCurrentNotes:
                    notesid = new SqlParameter("NotesID", System.Data.SqlDbType.BigInt);
                    notesid.Value = arrParameter[0];

                    customerIDParameter = new SqlParameter("@CustID", System.Data.SqlDbType.BigInt);
                    customerIDParameter.Value = arrParameter[1];

                    notes = new SqlParameter("NotesID", System.Data.SqlDbType.Text);
                    notes.Value = arrParameter[2];

                    yrMthParameter  = new SqlParameter("NotesID", System.Data.SqlDbType.DateTime);
                    yrMthParameter.Value = arrParameter[3];

                    userID = new SqlParameter("NotesID", System.Data.SqlDbType.Int);
                    userID.Value = arrParameter[4];

                    return new[] { notesid, customerIDParameter, notes, yrMthParameter, userID };

                case stpType.UpdateCurrentNotes:
                    notesid = new SqlParameter("NotesID", System.Data.SqlDbType.BigInt);
                    notesid.Value = arrParameter[0];

                    customerIDParameter = new SqlParameter("@CustID", System.Data.SqlDbType.BigInt);
                    customerIDParameter.Value = arrParameter[1];

                    notes = new SqlParameter("NotesID", System.Data.SqlDbType.Text);
                    notes.Value = arrParameter[2];

                    yrMthParameter = new SqlParameter("NotesID", System.Data.SqlDbType.DateTime);
                    yrMthParameter.Value = arrParameter[3];

                    userID = new SqlParameter("NotesID", System.Data.SqlDbType.Int);
                    userID.Value = arrParameter[4];

                    return new[] { notesid, customerIDParameter, notes, yrMthParameter, userID };

                case stpType.authenticateUser:
                    sAction = new SqlParameter("sAction", System.Data.SqlDbType.VarChar);
                    sAction.Value = arrParameter[0];
                    
                    clientIdParameter = new SqlParameter("ClientID", System.Data.SqlDbType.BigInt);
                    clientIdParameter.Value = arrParameter[1];

                    customerIDParameter = new SqlParameter("CustID", System.Data.SqlDbType.BigInt);
                    customerIDParameter.Value = arrParameter[2];

                    SqlParameter Mgt = new SqlParameter("Mgt", System.Data.SqlDbType.Bit);
                    Mgt.Value = arrParameter[3];

                    SqlParameter passKey = new SqlParameter("passKey", System.Data.SqlDbType.VarChar);
                    passKey.Value = arrParameter[4];

                    return new[] { sAction, clientIdParameter, customerIDParameter, Mgt, passKey };

                case stpType.cff2MYOB:
                    sAction = new SqlParameter("sAction", System.Data.SqlDbType.VarChar);
                    sAction.Value = arrParameter[0];

                    clientIdParameter = new SqlParameter("clientID", System.Data.SqlDbType.BigInt);
                    clientIdParameter.Value = arrParameter[1];

                    customerIDParameter = new SqlParameter("custID", System.Data.SqlDbType.BigInt);
                    customerIDParameter.Value = arrParameter[2];

                    if (arrParameter.Count == 3)
                    {
                        return (new[] { sAction, clientIdParameter, customerIDParameter });
                    }
                    else {
                        SqlParameter cardType = new SqlParameter("cardType", System.Data.SqlDbType.SmallInt);
                        cardType.Value = arrParameter[3];

                        SqlParameter dateFrom = new SqlParameter("dtFrom", System.Data.SqlDbType.DateTime);
                        dateFrom.Value = arrParameter[4];

                        SqlParameter dateTo = new SqlParameter("dtTo", System.Data.SqlDbType.DateTime);
                        dateTo.Value = arrParameter[5];
                        return new[] { sAction, clientIdParameter, customerIDParameter, cardType, dateFrom, dateTo };
                    }

                case stpType.TriggerUpdateKey:
                    sAction = new SqlParameter("sAction", System.Data.SqlDbType.VarChar);
                    sAction.Value = arrParameter[0];

                    clientIdParameter = new SqlParameter("clientID", System.Data.SqlDbType.BigInt);
                    clientIdParameter.Value = arrParameter[1];

                    SqlParameter keyType = new SqlParameter("keyType", System.Data.SqlDbType.SmallInt);
                    keyType.Value = arrParameter[2];

                    SqlParameter triggerID = new SqlParameter("triggerID", System.Data.SqlDbType.SmallInt);
                    triggerID.Value = arrParameter[3];

                    SqlParameter encType = new SqlParameter("encType", System.Data.SqlDbType.SmallInt);
                    encType.Value = arrParameter[4];

                    SqlParameter dateupdated = new SqlParameter("dateupdated", System.Data.SqlDbType.DateTime);
                    dateupdated.Value = arrParameter[5];
                    return new[] { sAction, clientIdParameter, keyType, triggerID, encType, dateupdated };


                case stpType.ClientNotification:
                    sAction = new SqlParameter("Status", System.Data.SqlDbType.VarChar);
                    sAction.Value = arrParameter[0];

                    clientIdParameter = new SqlParameter("clientID", System.Data.SqlDbType.BigInt);
                    clientIdParameter.Value = arrParameter[1];

                    SqlParameter filename = new SqlParameter("filename", System.Data.SqlDbType.SmallInt);
                    filename.Value = arrParameter[2];

                    SqlParameter timestamp = new SqlParameter("timestamp", System.Data.SqlDbType.SmallInt);
                    timestamp.Value = arrParameter[3];

                    return new[] { sAction, clientIdParameter, filename, timestamp };


                case stpType.CashFlowFundingRequest:
                    sAction = new SqlParameter("sAction", System.Data.SqlDbType.VarChar);
                    sAction.Value = arrParameter[0];

                    SqlParameter queryString = new SqlParameter("queryString", System.Data.SqlDbType.VarChar);
                    queryString.Value = arrParameter[1];

                    return new[] { sAction, queryString };

                case stpType.CashFlowFundingInterestReq:
                    sAction = new SqlParameter("sAction", System.Data.SqlDbType.VarChar);
                    sAction.Value = arrParameter[0];

                    return new[] { sAction };

                case stpType.smsMessageIn:      // dbb [2015/10/13]
                    sendr = new SqlParameter("sender", System.Data.SqlDbType.VarChar);
                    sendr.Value = arrParameter[0];
                    recvr = new SqlParameter("receiver", System.Data.SqlDbType.VarChar);
                    recvr.Value = arrParameter[1];
                    msg = new SqlParameter("msg", System.Data.SqlDbType.VarChar);
                    msg.Value = arrParameter[2];
                    op = new SqlParameter("operator", System.Data.SqlDbType.VarChar);
                    op.Value = arrParameter[3];
                    msgType = new SqlParameter("msgType", System.Data.SqlDbType.VarChar);
                    msgType.Value = arrParameter[4];
                    reference = new SqlParameter("reference", System.Data.SqlDbType.VarChar);
                    reference.Value = arrParameter[5];
                    stats = new SqlParameter("status", System.Data.SqlDbType.VarChar);
                    stats.Value = arrParameter[6];
                    clientIdParameter = new SqlParameter("ClientID", System.Data.SqlDbType.BigInt);
                    clientIdParameter.Value = arrParameter[7];
                    customerIDParameter = new SqlParameter("CustID", System.Data.SqlDbType.BigInt);
                    customerIDParameter.Value = arrParameter[8];
                    empId = new SqlParameter("EmployeeID", System.Data.SqlDbType.Int);
                    empId.Value = arrParameter[9];
                    yrMthParameter = new SqlParameter("yrmth", System.Data.SqlDbType.Int);
                    yrMthParameter.Value = arrParameter[10];
                    retVal = new SqlParameter("retVal", System.Data.SqlDbType.VarChar);
                    retVal.Value = arrParameter[11];

                    return new[] {sendr, recvr, msg, op, msgType, reference, stats, clientIdParameter, customerIDParameter, empId, yrMthParameter, retVal};
                
                case stpType.InsUpdateCliContact:
                    sAction = new SqlParameter("sAction", System.Data.SqlDbType.VarChar);
                    sAction.Value = arrParameter[0];
                    SqlParameter clientId = new SqlParameter("clientId", System.Data.SqlDbType.BigInt);
                    clientId.Value = arrParameter[1];
                    SqlParameter clientContactsId = new SqlParameter("clientContactsId", System.Data.SqlDbType.BigInt);
                    clientContactsId.Value = arrParameter[2];
                    SqlParameter contactFName = new SqlParameter("firstName", System.Data.SqlDbType.VarChar);
                    contactFName.Value = arrParameter[3];
                    SqlParameter contactLName = new SqlParameter("lastName", System.Data.SqlDbType.VarChar);
                    contactLName.Value = arrParameter[4];
                    SqlParameter phone = new SqlParameter("phone", System.Data.SqlDbType.VarChar);
                    phone.Value = arrParameter[5];
                    SqlParameter fax = new SqlParameter("fax", System.Data.SqlDbType.VarChar);
                    fax.Value = arrParameter[6];
                    SqlParameter cell = new SqlParameter("mobilePhone", System.Data.SqlDbType.VarChar);
                    cell.Value = arrParameter[7];                    
                    SqlParameter role = new SqlParameter("role", System.Data.SqlDbType.VarChar);
                    role.Value = arrParameter[8];
                    SqlParameter email = new SqlParameter("email", System.Data.SqlDbType.VarChar);
                    email.Value = arrParameter[9];
                    SqlParameter modDate = new SqlParameter("modified", System.Data.SqlDbType.BigInt);
                    modDate.Value = arrParameter[10];
                    SqlParameter modBy = new SqlParameter("modifiedBy", System.Data.SqlDbType.BigInt);
                    modBy.Value = arrParameter[11];

                    return new[] { sAction, clientId, clientContactsId, contactFName, contactLName, phone, fax, cell, role, email, modDate, modBy };

                case stpType.AddCffCustomerApplication:
                    sAction = new SqlParameter("sAction", System.Data.SqlDbType.VarChar);
                    sAction.Value = arrParameter[0];
                    SqlParameter id = new SqlParameter("id", System.Data.SqlDbType.BigInt);
                    id.Value = arrParameter[1];
                    SqlParameter applicationType = new SqlParameter("applicantType", System.Data.SqlDbType.SmallInt);
                    applicationType.Value = arrParameter[2];
                    SqlParameter type = new SqlParameter("type", System.Data.SqlDbType.SmallInt);
                    type.Value = arrParameter[3];
                    SqlParameter name = new SqlParameter("name", System.Data.SqlDbType.VarChar);
                    name.Value = arrParameter[4];
                    SqlParameter companyNumber = new SqlParameter("companyNumber", System.Data.SqlDbType.VarChar);
                    companyNumber.Value = arrParameter[5];
                    SqlParameter nzbn = new SqlParameter("nzbn", System.Data.SqlDbType.VarChar);
                    nzbn.Value = arrParameter[6];
                    SqlParameter phoneNumber = new SqlParameter("phoneNumber", System.Data.SqlDbType.VarChar);
                    phoneNumber.Value = arrParameter[7];
                    email = new SqlParameter("email", System.Data.SqlDbType.NVarChar);
                    email.Value = arrParameter[8];
                    SqlParameter profitable = new SqlParameter("profitable", System.Data.SqlDbType.Bit);
                    profitable.Value = arrParameter[9];
                    SqlParameter liabilities = new SqlParameter("liabilities", System.Data.SqlDbType.Bit);
                    liabilities.Value = arrParameter[10];
                    SqlParameter paymentPlan = new SqlParameter("paymentPlan", System.Data.SqlDbType.VarChar);
                    paymentPlan.Value = arrParameter[11];
                    SqlParameter numActiveCustomers = new SqlParameter("numActiveCustomers", System.Data.SqlDbType.Int);
                    numActiveCustomers.Value = arrParameter[12];
                    SqlParameter lastMonthSales = new SqlParameter("lastMonthSales", System.Data.SqlDbType.Decimal);
                    lastMonthSales.Value = arrParameter[13];
                    SqlParameter lastYearSales = new SqlParameter("lastYearSales", System.Data.SqlDbType.Decimal);
                    lastYearSales.Value = arrParameter[14];
                    SqlParameter totalDebtor = new SqlParameter("totalDebtor", System.Data.SqlDbType.Decimal);
                    totalDebtor.Value = arrParameter[15];
                    SqlParameter valueStockOnHand = new SqlParameter("valueStockOnHand", System.Data.SqlDbType.Decimal);
                    valueStockOnHand.Value = arrParameter[16];
                    SqlParameter hasPrevConvictionsOrObligations = new SqlParameter("hasPrevConvictionsOrObligations", System.Data.SqlDbType.Bit);
                    hasPrevConvictionsOrObligations.Value = arrParameter[17];
                    SqlParameter google = new SqlParameter("google", System.Data.SqlDbType.Bit);
                    google.Value = arrParameter[18];
                    SqlParameter radioStation = new SqlParameter("radioStation", System.Data.SqlDbType.VarChar);
                    radioStation.Value = arrParameter[19];
                    SqlParameter paperAds = new SqlParameter("paperAds", System.Data.SqlDbType.VarChar);
                    paperAds.Value = arrParameter[20];
                    SqlParameter referral = new SqlParameter("referral", System.Data.SqlDbType.VarChar);
                    referral.Value = arrParameter[21];
                    SqlParameter interestCoNz = new SqlParameter("interestCoNz", System.Data.SqlDbType.Bit);
                    interestCoNz.Value = arrParameter[22];
                    SqlParameter other = new SqlParameter("other", System.Data.SqlDbType.VarChar);
                    other.Value = arrParameter[23];
                    SqlParameter nameSignature = new SqlParameter("nameSignature", System.Data.SqlDbType.VarChar);
                    nameSignature.Value = arrParameter[24];
                    SqlParameter dateSigned = new SqlParameter("dateSigned", System.Data.SqlDbType.DateTime);
                    dateSigned.Value = arrParameter[25];
                    SqlParameter signatureFile = new SqlParameter("signatureFile", System.Data.SqlDbType.VarBinary);
                    signatureFile.Value = arrParameter[26];
                    SqlParameter randomId = new SqlParameter("randomId", System.Data.SqlDbType.VarChar);
                    randomId.Value = arrParameter[27];

                    return new[] {sAction, id, type, applicationType, name, companyNumber, nzbn, phoneNumber, email, profitable, liabilities, paymentPlan, numActiveCustomers, lastMonthSales, lastYearSales,
                                    totalDebtor, valueStockOnHand, hasPrevConvictionsOrObligations, google, radioStation, paperAds, referral, interestCoNz, other, nameSignature, dateSigned, signatureFile, randomId};

                case stpType.AddCffCustomerApplicationDirectors:
                    sAction = new SqlParameter("sAction", System.Data.SqlDbType.VarChar);
                    sAction.Value = arrParameter[0];
                    SqlParameter fullName = new SqlParameter("fullName", System.Data.SqlDbType.VarChar);
                    fullName.Value = arrParameter[1];
                    SqlParameter dob = new SqlParameter("dob", System.Data.SqlDbType.DateTime);
                    dob.Value = arrParameter[2];
                    SqlParameter hasTrust = new SqlParameter("hasTrust", System.Data.SqlDbType.Bit);
                    hasTrust.Value = arrParameter[3];
                    SqlParameter trusts = new SqlParameter("trusts", System.Data.SqlDbType.VarChar);
                    trusts.Value = arrParameter[4];
                    SqlParameter financials = new SqlParameter("financials", System.Data.SqlDbType.VarChar);
                    financials.Value = arrParameter[5];
                    SqlParameter otherAsset = new SqlParameter("otherAsset", System.Data.SqlDbType.Decimal);
                    otherAsset.Value = arrParameter[6];
                    SqlParameter otherLiab = new SqlParameter("otherLiab", System.Data.SqlDbType.Decimal);
                    otherLiab.Value = arrParameter[7];
                    randomId = new SqlParameter("randomId", System.Data.SqlDbType.VarChar);
                    randomId.Value = arrParameter[8];

                    return new[] { sAction, fullName, dob, hasTrust, trusts, financials, otherAsset, otherLiab, randomId };

                case stpType.InsertNotesPermanent:
                    notesid = new SqlParameter("notesID", System.Data.SqlDbType.BigInt);
                    notesid.Value = -1;
                    customerIDParameter = new SqlParameter("CustID", System.Data.SqlDbType.BigInt);
                    customerIDParameter.Value = arrParameter[1];
                    createDate = new SqlParameter("Created", System.Data.SqlDbType.DateTime);
                    createDate.Value = arrParameter[5];
                    notes = new SqlParameter("notes", System.Data.SqlDbType.Text);
                    notes.Value = arrParameter[2];
                    modifyDate = new SqlParameter("Modified", System.Data.SqlDbType.DateTime);
                    modifyDate.Value = arrParameter[3];
                    userID = new SqlParameter("ModifiedBy", System.Data.SqlDbType.DateTime);
                    userID.Value = arrParameter[4];
                    yrMthParameter = new SqlParameter("yrMth", System.Data.SqlDbType.BigInt);
                    yrMthParameter.Value = arrParameter[6];
                    yrMthDayParameter = new SqlParameter("yrMthdaymodified", System.Data.SqlDbType.BigInt);
                    yrMthDayParameter.Value = arrParameter[7];
                    return new[] { notesid, customerIDParameter, createDate, notes, modifyDate, userID, yrMthParameter, yrMthDayParameter };

                default:
                    clientIdParameter = new SqlParameter("clientId", System.Data.SqlDbType.BigInt);
                    clientIdParameter.Value = arrParameter[0];
                    return new[] { clientIdParameter };
            }
        }

        private string storedProcName(stpType storedProcType)
        {
            switch (storedProcType)
            {
                case stpType.GetClientSelectedData:
                    return "dbo.stGetClientSelectedData";

                case stpType.GetCustomerDetails:
                    return "dbo.stGetCustomer";

                case stpType.GetCustomerInfo:
                    return "dbo.GetCustomerInfo";

                case stpType.GetTransactions:
                    return "dbo.stGetTransactionsMart";
                
                case stpType.GetNotesCurrent:
                    return "dbo.stGetNotesCurrent";

                case stpType.GetUserDetails:
                    return "dbo.StGetEmployeeDetails";

                case stpType.InsUpdCustContForValid:
                    return "dbo.stInsUpdateCustContactsForValidation";

                case stpType.InsertLetterSent:
                    return "dbo.stInsertLetterSent";

                case stpType.InsertCurrentNotes:
                    return "dbo.stUpdateCustomerNotesI";

                case stpType.UpdateCurrentNotes:
                    return "dbo.stUpdateCustomerNotesU";

                case stpType.authenticateUser:
                    return "dbo.StUpdatePassKey";

                case stpType.cff2MYOB:
                    return "dbo.stCFF2MYOB";

                case stpType.TriggerUpdateKey:
                    return "dbo.stEditGetExKeyUpdateHist";

                case stpType.ClientNotification:
                    return "dbo.stClientNotification";

                case stpType.CashFlowFundingRequest:
                    return "dbo.stParseCFundingRequest";

                case stpType.CashFlowFundingInterestReq:
                    return "dbo.stGetCFFBankRates";

                case stpType.smsMessageIn:
                    return "stSMSmessageoutAdd";

                case stpType.UpdClientContacts:
                    return "stUpdateWebClientContacts";

                case stpType.InsUpdateCliContact:
                    return "stInsUpdateClientContacts";

                case stpType.AddCffCustomerApplication:
                    return "stAddCffCustomerApplication";

                case stpType.AddCffCustomerApplicationDirectors:
                    return "stAddCffCustomerApplicantDirectors";

                case stpType.Letters_GetCustomerLetterDetails:
                    return "dbo.Letters_GetCustomerLetterDetails";

                case stpType.InsertNotesPermanent:
                    return "dbo.stUpdateCustomerNotesPermanentI";

                default:
                    return "unknownStoredProcedure";
            
            }
        }
    }
}
