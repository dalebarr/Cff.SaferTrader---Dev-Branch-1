using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Cff.SaferTrader.Core.Repositories
{
    public class NotesRepository : SqlManager, INotesRepository
    {
        public NotesRepository(string connectionString)
            : base(connectionString)
        {
        }

        #region Inserting Notes

        public void InsertCustomerNote(CustomerNote customerNote)
        {
            ArgumentChecker.ThrowIfNull(customerNote, "customerNote");
            using (SqlConnection connection = CreateConnection())
            {
                SqlHelper.ExecuteNonQuery(connection,
                                          CommandType.StoredProcedure,
                                          "NotesCurrent_InsertNewNotes", CreateSaveCustomerNoteParameters(customerNote));
            }
        }

        public void InsertPermanentNote(PermanentCustomerNote permanentCustomerNote)
        {
            ArgumentChecker.ThrowIfNull(permanentCustomerNote, "permanentNote");
            using (SqlConnection connection = CreateConnection())
            {
                SqlHelper.ExecuteNonQuery(connection,
                                          CommandType.StoredProcedure,
                                          "NotePermanent_Save", CreateSavePermanentNoteParameters(permanentCustomerNote));
            }
        }

        public void InsertClientPermanentNote(PermanentClientNote permanentClientNote)
        {
            ArgumentChecker.ThrowIfNull(permanentClientNote, "clientPermanentNote");
            using (SqlConnection connection = CreateConnection())
            {
                SqlHelper.ExecuteNonQuery(connection,
                                          CommandType.StoredProcedure,
                                          "NoteClientPerm_Save",
                                          CreateSaveClientPermanentNoteParameters(permanentClientNote));
            }
        }

        public void InsertClientNote(ClientNote clientNote)
        {
            ArgumentChecker.ThrowIfNull(clientNote, "clientNote");
            using (SqlConnection connection = CreateConnection())
            {
                SqlHelper.ExecuteNonQuery(connection,
                                          CommandType.StoredProcedure,
                                          "NoteClient_Save", CreateSaveClientNoteParameters(clientNote));
            }
        }

        #endregion

        #region Update Notes

        public void UpdateCustomerNote(CustomerNote customerNote)
        {
            ArgumentChecker.ThrowIfNull(customerNote, "customerNote");

            using (SqlConnection connection = CreateConnection())
            {
                SqlHelper.ExecuteNonQuery(connection,
                                          CommandType.StoredProcedure,
                                          "NotesCurrent_Update", CreateUpdateCustomerNoteParameters(customerNote));
            }
        }

        public void UpdatePermanentCustomerNote(PermanentCustomerNote permanentCustomerNote)
        {
            ArgumentChecker.ThrowIfNull(permanentCustomerNote, "permanentNote");

            using (SqlConnection connection = CreateConnection())
            {
                SqlHelper.ExecuteNonQuery(connection,
                                          CommandType.StoredProcedure,
                                          "NotesPermanent_Update",
                                          CreateUpdatePermanentNoteParameters(permanentCustomerNote));
            }
        }
          // When updating Cff level Notes , use -1 as Client Id
        public void UpdatePermanentClientNote(PermanentClientNote permanentClientNote)
        {
            ArgumentChecker.ThrowIfNull(permanentClientNote, "clientPermanentNote");

            using (SqlConnection connection = CreateConnection())
            {
                SqlHelper.ExecuteNonQuery(connection,
                                          CommandType.StoredProcedure,
                                          "NoteClientPerm_Update",
                                          CreateUpdateClientPermanentNoteParameters(permanentClientNote));
            }
        }

        public void UpdateClientNote(ClientNote clientNote)
        {
            ArgumentChecker.ThrowIfNull(clientNote, "clientNote");

            using (SqlConnection connection = CreateConnection())
            {
                SqlHelper.ExecuteNonQuery(connection,
                                          CommandType.StoredProcedure,
                                          "NotesClient_Update", CreateUpdateClientNoteParameters(clientNote));
            }
        }

        #endregion

        #region Load Notes

        public IList<PermanentClientNote> LoadPermanentClientsNotesFor(int clientId)
        {
            IList<PermanentClientNote> notes = new List<PermanentClientNote>();

            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "NotesClientPerm_LoadForAClient",
                                                                          CreateNotesClientPerm_LoadForAClientParameters
                                                                              (clientId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        string parsedNotes = CustomerNotesParser.Parse(cleverReader.ToString("notes"));

                        PermanentClientNote note = new PermanentClientNote(cleverReader.FromBigInteger("NotesID"),
                                                                           cleverReader.FromBigInteger("ClientID"),
                                                                           cleverReader.ToDate("Created"),
                                                                           parsedNotes,
                                                                           cleverReader.ToInteger("CreatedBy"),
                                                                           cleverReader.ToString("EmployeeName"),
                                                                           cleverReader.ToInteger("ModifiedBy"),
                                                                           cleverReader.ToString(
                                                                               "ModifiedByEmployeeName"),
                                                                           cleverReader.ToDate("Modified"));
                        notes.Add(note);
                    }
                }
            }
            return notes;
        }


        public IList<AllClientsPermanentNote> LoadCffPermanentNotes()
        {
            IList<AllClientsPermanentNote> notes = new List<AllClientsPermanentNote>();

            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "NotesClientPerm_LoadAll"))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        string parsedNotes = CustomerNotesParser.Parse(cleverReader.ToString("notes"));

                        AllClientsPermanentNote note = new AllClientsPermanentNote(cleverReader.FromBigInteger("NotesID"),
                                                                           cleverReader.FromBigInteger("ClientID"),
                                                                           cleverReader.ToString("ClientName"),
                                                                           cleverReader.ToDate("Created"),
                                                                           parsedNotes,
                                                                           cleverReader.ToInteger("CreatedBy"),
                                                                           cleverReader.ToString("EmployeeName"),
                                                                           cleverReader.ToInteger("ModifiedBy"),
                                                                           cleverReader.ToString(
                                                                               "ModifiedByEmployeeName"),
                                                                           cleverReader.ToDate("Modified"));
                        notes.Add(note);
                    }
                }
            }

            return notes;
        }

        public IList<ClientNote> LoadClientNotesFor(int clientId)
        {
            IList<ClientNote> notes = new List<ClientNote>();

            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "NotesClient_LoadForAClient",
                                                                          CreateNotesClientPerm_LoadForAClientParameters
                                                                              (clientId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        string parsedNotes = CustomerNotesParser.Parse(cleverReader.ToString("notes"));

                        ClientNote note = new ClientNote(
                            cleverReader.FromBigInteger("NotesID"),
                            cleverReader.ToDate("Created"),
                            ActivityType.Parse(cleverReader.ToInteger("ActivityTypeId")),
                            NoteType.Parse(cleverReader.ToInteger("NoteTypeId")),
                            parsedNotes,
                            cleverReader.ToInteger("CreatedBy"),
                            cleverReader.ToString("EmployeeName"),
                            cleverReader.FromBigInteger("ClientID"),
                            cleverReader.ToInteger("ModifiedBy"),
                            cleverReader.ToString("ModifiedByEmployeeName"),
                            cleverReader.ToDate("Modified"));
                        notes.Add(note);
                    }
                }
            }
            return notes;
        }

        public IList<AllClientsPermanentNote> LoadCffPermanentNotesOnRange(DateRange dateRange)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "fromDate");

            SqlParameter[] sqlParameters = new[]
                                               {
                                                   CreateFromDateParameter(dateRange.StartDate),
                                                   CreateToDateParameters(dateRange.EndDate)
                                               };

            IList<AllClientsPermanentNote> notes = new List<AllClientsPermanentNote>();

            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "NotesClientPerm_LoadAllOnRange", sqlParameters))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        string parsedNotes = CustomerNotesParser.Parse(cleverReader.ToString("notes"));

                        AllClientsPermanentNote note = new AllClientsPermanentNote(cleverReader.FromBigInteger("NotesID"),
                                                                           cleverReader.FromBigInteger("ClientID"),
                                                                           cleverReader.ToString("ClientName"),
                                                                           cleverReader.ToDate("Created"),
                                                                           parsedNotes,
                                                                           cleverReader.ToInteger("CreatedBy"),
                                                                           cleverReader.ToString("EmployeeName"),
                                                                           cleverReader.ToInteger("ModifiedBy"),
                                                                           cleverReader.ToString(
                                                                               "ModifiedByEmployeeName"),
                                                                           cleverReader.ToDate("Modified"));
                        notes.Add(note);
                    }
                }
            }

            return notes;
        }

        public IList<PermanentClientNote> LoadPermanentClientsNotesForOnRange(int clientId, DateRange dateRange)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "fromDate");
            SqlParameter[] sqlParameters = new[]
                                               {
                                                   CreateClientIdParameter(clientId),
                                                   CreateFromDateParameter(dateRange.StartDate),
                                                   CreateToDateParameters(dateRange.EndDate)
                                               };
            IList<PermanentClientNote> notes = new List<PermanentClientNote>();

            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "NotesClientPerm_LoadForAClientOnRange",
                                                                          sqlParameters))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        string parsedNotes = CustomerNotesParser.Parse(cleverReader.ToString("notes"));

                        PermanentClientNote note = new PermanentClientNote(cleverReader.FromBigInteger("NotesID"),
                                                                           cleverReader.FromBigInteger("ClientID"),
                                                                           cleverReader.ToDate("Created"),
                                                                           parsedNotes,
                                                                           cleverReader.ToInteger("CreatedBy"),
                                                                           cleverReader.ToString("EmployeeName"),
                                                                           cleverReader.ToInteger("ModifiedBy"),
                                                                           cleverReader.ToString(
                                                                               "ModifiedByEmployeeName"),
                                                                           cleverReader.ToDate("Modified"));
                        notes.Add(note);
                    }
                }
            }
            return notes;
        }

        public IList<ClientNote> LoadClientNotesOnRange(int clientId, DateRange dateRange)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "fromDate");
            SqlParameter[] sqlParameters = new[]
                                               {
                                                   CreateClientIdParameter(clientId),
                                                   CreateFromDateParameter(dateRange.StartDate),
                                                   CreateToDateParameters(dateRange.EndDate)
                                               };
            IList<ClientNote> notes = new List<ClientNote>();

            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "NotesClient_LoadForAClientOnRange",
                                                                          sqlParameters))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        string parsedNotes = CustomerNotesParser.Parse(cleverReader.ToString("notes"));
                        ClientNote note = null;

                        if (clientId > 0)
                        {
                             note = new ClientNote(
                             cleverReader.FromBigInteger("NotesID"),
                             cleverReader.ToDate("Created"),
                             ActivityType.Parse(cleverReader.ToInteger("ActivityTypeId")),
                             NoteType.Parse(cleverReader.ToInteger("NoteTypeId")),
                             parsedNotes,
                             cleverReader.ToInteger("CreatedBy"),
                             cleverReader.ToString("EmployeeName"),
                             cleverReader.FromBigInteger("ClientID"),
                             cleverReader.ToInteger("ModifiedBy"),
                             cleverReader.ToString("ModifiedByEmployeeName"),
                             cleverReader.ToDate("Modified"));
                        }
                        else
                        {
                             note = new ClientNote(
                             cleverReader.FromBigInteger("NotesID"),
                             cleverReader.ToDate("Created"),
                             ActivityType.Parse(cleverReader.ToInteger("ActivityTypeId")),
                             NoteType.Parse(cleverReader.ToInteger("NoteTypeId")),
                             parsedNotes,
                             cleverReader.ToInteger("CreatedBy"),
                             cleverReader.ToString("EmployeeName"),
                             cleverReader.FromBigInteger("ClientID"),
                             cleverReader.ToInteger("ModifiedBy"),
                             cleverReader.ToString("ModifiedByEmployeeName"),
                             cleverReader.ToDate("Modified"),
                             cleverReader.ToString("ClientName"));
                        }

                        if (note!=null)
                            notes.Add(note);
                    }
                }
            }
            return notes;
        }

        //TODO Needs to retire this method when we have time , some tests depend on this method
        public IList<CustomerNote> LoadCustomerNotes(int customerId)
        {
            return ExecuteLoadCustomerNotes(new[] {CreateCustomerIdParameter(customerId)});
        }

        public IList<CustomerNote> LoadAllCustomerNotesForClientOnRange(int clientId, DateRange dateRange)
        {
            //we make use of CFF's DataEntry DLL as the stored procedure for this method is already defined, so we just reuse it.
            //where clientid=-1 : Load All Client's Customer Notes within a given date range 
            //where clientid>0  : Load All Customer notes within a given date range for this client
       
            DataEntry.Clients mClient = new DataEntry.Clients();
            ArrayList arrPar = new ArrayList();
            int yrmth = Convert.ToInt32(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString());
            if (dateRange!=null)
                yrmth = Convert.ToInt32(dateRange.EndDate.Year.ToString() + dateRange.EndDate.Month.ToString().PadLeft(2, '0'));

            arrPar.Add(1); //whichnotes
            arrPar.Add(clientId);
            arrPar.Add(yrmth);
            if (dateRange == null)
            {
                arrPar.Add(DateTime.Now.ToShortDateString() + " 11:59:59 pm");
            }
            else
            {
                arrPar.Add(String.Format("{0:MM/dd/yyyy}", dateRange.StartDate) + " 23:59:59 pm");
                //arrPar.Add(System.Convert.ToDateTime(dateRange.StartDate.ToShortDateString() + " 23:59:59 pm"));
            }

            if (dateRange == null)
            {
                arrPar.Add(DateTime.Now.ToShortDateString() + " 11:59:59 pm");
            }
            else
            {
                arrPar.Add(String.Format("{0:MM/dd/yyyy}", dateRange.EndDate) + " 23:59:59 pm");
                //arrPar.Add(System.Convert.ToDateTime(dateRange.EndDate.ToShortDateString() + " 23:59:59 pm"));
            }
            System.Data.DataSet theDS = mClient.getClientNotes(arrPar);
            IList<CustomerNote> custNoteList = new List<CustomerNote>();
            if (theDS != null) {
                for (int ix = 0; ix < theDS.Tables[0].Rows.Count; ix++)
                {
                    DataRow DR = theDS.Tables[0].Rows[ix];
                    CustomerNote custNoteDetail = new CustomerNote(Convert.ToInt64(DR["NotesId"]), new Date(Convert.ToDateTime(DR["Created"])),
                                                       ActivityType.Parse(Convert.ToInt32(DR["ActivityTypeId"])), NoteType.Parse(Convert.ToInt32(DR["NoteTypeId"])),
                                                        DR["notes"].ToString(), Convert.ToInt32(DR["CreatedBy"]), DR["CreatedByEmpName"].ToString(), Convert.ToInt32(DR["ModifiedBy"]),
                                                            DR["LastModifiedBy"].ToString(), new Date(Convert.ToDateTime(DR["Modified"])), DR["Customer"].ToString(),
                                                                    Convert.ToInt32(DR["custid"]), DR["authorRole"].ToString(),
                                                                        ((clientId>0)?clientId:Convert.ToInt32(DR["ClientID"].ToString()))
                                                                        );
                    custNoteList.Add(custNoteDetail);
                }
            }
           

            return custNoteList;
        }

        public IList<CustomerNote> LoadCustomerNotes(int customerId, DateRange dateRange)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "fromDate");

            SqlParameter[] sqlParameters = new[]
                                               {
                                                   CreateCustomerIdParameter(customerId),
                                                   CreateFromDateParameter(dateRange.StartDate),
                                                   CreateToDateParameters(dateRange.EndDate)
                                               };
            return ExecuteLoadCustomerNotes(sqlParameters);
        }

        public IList<CustomerNote> LoadCustomerNotes(int customerId, DateRange dateRange, NoteType noteType,
                                                     ActivityType activityType)
        {
            ArgumentChecker.ThrowIfNull(dateRange, "dateRange");

            SqlParameter[] sqlParameters = new[]
                                               {
                                                   CreateCustomerIdParameter(customerId),
                                                   CreateFromDateParameter(dateRange.StartDate),
                                                   CreateToDateParameters(dateRange.EndDate),
                                                   CreateNoteTypeParameter(noteType),
                                                   CreateActivityTypeParameter(activityType)
                                               };
            return ExecuteLoadCustomerNotes(sqlParameters);
        }

        public IList<PermanentCustomerNote> LoadPermanentCustomerNoteOnRange(int customerId, DateRange dateRange)
        {
            SqlParameter customerIdParameter = new SqlParameter("@CustomerId", SqlDbType.BigInt);
            customerIdParameter.Value = customerId;

            SqlParameter dateFromParameter = new SqlParameter("@DateFrom", SqlDbType.DateTime);
            dateFromParameter.Value = Convert.ToDateTime(dateRange.StartDate.ToShortDateString());

            SqlParameter dateToParameter = new SqlParameter("@DateTo", SqlDbType.DateTime);
            dateToParameter.Value = Convert.ToDateTime(dateRange.EndDate.ToShortDateString());

            SqlParameter[] paramObjects = new SqlParameter[] {customerIdParameter, dateFromParameter, dateToParameter};

            IList<PermanentCustomerNote> permanentNotes = new List<PermanentCustomerNote>();
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "NotesPermanent_LoadCustomerInRange", 
                                                                            paramObjects 
                                                                          ))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        string parsedNotes = CustomerNotesParser.Parse(cleverReader.ToString("notes"));

                        PermanentCustomerNote permanentCustomerNote =
                            new PermanentCustomerNote(cleverReader.FromBigInteger("NotesID"),
                                                      cleverReader.ToDate("Created"),
                                                      parsedNotes,
                                                      cleverReader.ToInteger("CreatedBy"),
                                                      cleverReader.ToString("EmployeeName"),
                                                      cleverReader.ToInteger("ModifiedBy"),
                                                      cleverReader.ToString("ModifiedByEmployeeName"),
                                                      cleverReader.ToDate("Modified"));
                        permanentNotes.Add(permanentCustomerNote);
                    }
                }
            }
            return permanentNotes;
        }

        public IList<PermanentCustomerNote> LoadPermanentCustomerNote(int customerId)
        {
            IList<PermanentCustomerNote> permanentNotes = new List<PermanentCustomerNote>();

            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "NotesPermanent_LoadByCustomerID",
                                                                          CreateCustomerIdParameter(customerId)))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (cleverReader.Read())
                    {
                        string parsedNotes = CustomerNotesParser.Parse(cleverReader.ToString("notes"));

                        PermanentCustomerNote permanentCustomerNote =
                            new PermanentCustomerNote(cleverReader.FromBigInteger("NotesID"),
                                                      cleverReader.ToDate("Created"),
                                                      parsedNotes,
                                                      cleverReader.ToInteger("CreatedBy"),
                                                      cleverReader.ToString("EmployeeName"),
                                                      cleverReader.ToInteger("ModifiedBy"),
                                                      cleverReader.ToString("ModifiedByEmployeeName"),
                                                      cleverReader.ToDate("Modified"));
                        permanentNotes.Add(permanentCustomerNote);
                    }
                }
            }

            return permanentNotes;
        }

        private IList<CustomerNote> ExecuteLoadCustomerNotes(SqlParameter[] sqlParameters)
        {
            IList<CustomerNote> customerNotes = new List<CustomerNote>();
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlDataReader dataReader = SqlHelper.ExecuteReader(connection,
                                                                          CommandType.StoredProcedure,
                                                                          "NotesCurrent_GetCustomerNotes",
                                                                          sqlParameters))
                {
                    CleverReader cleverReader = new CleverReader(dataReader);
                    while (!cleverReader.IsNull && cleverReader.Read())
                    {
                        string parsedNotes = CustomerNotesParser.Parse(cleverReader.ToString("notes"));

                        CustomerNote customerNote = new CustomerNote(cleverReader.FromBigInteger("NotesID"),
                                                                     cleverReader.ToDate("Created"),
                                                                     ActivityType.Parse(cleverReader.ToInteger("ActivityTypeId")),
                                                                     NoteType.Parse(cleverReader.ToInteger("NoteTypeId")),
                                                                     parsedNotes,
                                                                     cleverReader.ToInteger("CreatedBy"),
                                                                     cleverReader.ToString("EmployeeName"),
                                                                     cleverReader.ToInteger("ModifiedBy"),
                                                                     cleverReader.ToString("ModifiedByEmployeeName"),
                                                                     cleverReader.ToDate("Modified")
                            );
                        customerNotes.Add(customerNote);
                    }
                }
            }
            return customerNotes;
        }

        #endregion

        #region CreateParameters

        private static SqlParameter[] CreateUpdateClientPermanentNoteParameters(PermanentClientNote permanentClientNote)
        {
            SqlParameter noteIdParameter = new SqlParameter("@NoteId", SqlDbType.BigInt);
            SqlParameter modifiedByParameter = new SqlParameter("@ModifiedBy", SqlDbType.Int);
            SqlParameter commentParameter = new SqlParameter("@Comment", SqlDbType.Text);

            noteIdParameter.Value = permanentClientNote.NoteId;
            modifiedByParameter.Value = permanentClientNote.ModifiedBy;
            commentParameter.Value = permanentClientNote.Comment;

            return new[]
                       {
                           noteIdParameter,
                           commentParameter,
                           modifiedByParameter
                       };
        }

        private static SqlParameter[] CreateUpdatePermanentNoteParameters(PermanentCustomerNote permanentCustomerNote)
        {
            SqlParameter noteIdParameter = new SqlParameter("@NoteId", SqlDbType.BigInt);
            SqlParameter modifiedByParameter = new SqlParameter("@ModifiedBy", SqlDbType.Int);
            SqlParameter commentParameter = new SqlParameter("@Comment", SqlDbType.Text);

            noteIdParameter.Value = permanentCustomerNote.NoteId;
            modifiedByParameter.Value = permanentCustomerNote.ModifiedBy;
            commentParameter.Value = permanentCustomerNote.Comment;

            return new[]
                       {
                           noteIdParameter,
                           commentParameter,
                           modifiedByParameter
                       };
        }

        private static SqlParameter[] CreateUpdateCustomerNoteParameters(CustomerNote customerNote)
        {
            SqlParameter noteIdParameter = new SqlParameter("@NoteId", SqlDbType.BigInt);
            SqlParameter activityTypeIdParameter = new SqlParameter("@ActivityTypeId", SqlDbType.Int);
            SqlParameter noteTypeIdParameter = new SqlParameter("@NoteTypeId", SqlDbType.Int);
            SqlParameter modifiedByParameter = new SqlParameter("@ModifiedBy", SqlDbType.Int);
            SqlParameter commentParameter = new SqlParameter("@Comment", SqlDbType.Text);

            noteIdParameter.Value = customerNote.NoteId;
            activityTypeIdParameter.Value = customerNote.ActivityType.Id;
            noteTypeIdParameter.Value = customerNote.NoteType.Id;
            modifiedByParameter.Value = customerNote.ModifiedBy;
            commentParameter.Value = customerNote.Comment;

            return new[]
                       {
                           noteIdParameter, commentParameter, modifiedByParameter,
                           activityTypeIdParameter, noteTypeIdParameter
                       };
        }

        private static SqlParameter CreateClientIdParameter(long clientId)
        {
            SqlParameter clientIdParameter = new SqlParameter("@ClientId", SqlDbType.BigInt);
            clientIdParameter.Value = clientId;
            return clientIdParameter;
        }

        private static SqlParameter CreateCustomerIdParameter(long customerId)
        {
            SqlParameter customerIdParameter = new SqlParameter("@CustomerId", SqlDbType.BigInt);
            customerIdParameter.Value = customerId;
            return customerIdParameter;
        }

        private static SqlParameter CreateFromDateParameter(Date fromDate)
        {
            SqlParameter fromDateParameter = new SqlParameter("@FromDate", SqlDbType.DateTime);
            fromDateParameter.Value = fromDate.ToShortDateString();
            return fromDateParameter;
        }

        private static SqlParameter CreateToDateParameters(Date toDate)
        {
            SqlParameter toDateParameter = new SqlParameter("@ToDate", SqlDbType.DateTime);
            toDateParameter.Value = toDate.EndOfDay.ToDateTimeString();
            return toDateParameter;
        }

        private static SqlParameter CreateNoteTypeParameter(NoteType noteType)
        {
            SqlParameter noteTypeParameter = new SqlParameter("@NoteTypeId", SqlDbType.Int);
            if (noteType != null)
            {
                noteTypeParameter.Value = noteType.Id;
            }
            return noteTypeParameter;
        }

        private static SqlParameter CreateActivityTypeParameter(ActivityType activityType)
        {
            SqlParameter activityParameter = new SqlParameter("@ActivityTypeId", SqlDbType.Int);
            if (activityType != null)
            {
                activityParameter.Value = activityType.Id;
            }
            return activityParameter;
        }

        private static SqlParameter[] CreateSaveCustomerNoteParameters(CustomerNote customerNote)
        {
            SqlParameter customerIdParameter = new SqlParameter("@CustID", SqlDbType.Int);
            SqlParameter createdByParameter = new SqlParameter("@CreatedBy", SqlDbType.Int);
            SqlParameter notesParameter = new SqlParameter("@notes", SqlDbType.Text);
            SqlParameter activityTypeIdParameter = new SqlParameter("@ActivityTypeId", SqlDbType.Int);
            SqlParameter noteTypeIdParameter = new SqlParameter("@NoteTypeId", SqlDbType.Int);

            customerIdParameter.Value = customerNote.CustomerId;
            createdByParameter.Value = customerNote.AuthorId;
            notesParameter.Value = customerNote.Comment;
            activityTypeIdParameter.Value = customerNote.ActivityType.Id;
            noteTypeIdParameter.Value = customerNote.NoteType.Id;

            return new[]
                       {
                           customerIdParameter, createdByParameter, notesParameter,
                           activityTypeIdParameter, noteTypeIdParameter
                       };
        }

        private static SqlParameter[] CreateSavePermanentNoteParameters(PermanentCustomerNote permanentCustomerNote)
        {
            SqlParameter customerIdParameter = new SqlParameter("@CustomerID", SqlDbType.Int);
            SqlParameter createdByParameter = new SqlParameter("@CreatedBy", SqlDbType.Int);
            SqlParameter noteParamter = new SqlParameter("@Note", SqlDbType.Text);

            customerIdParameter.Value = permanentCustomerNote.CustomerId;
            createdByParameter.Value = permanentCustomerNote.AuthorId;
            noteParamter.Value = permanentCustomerNote.Comment;

            return new[] {customerIdParameter, createdByParameter, noteParamter};
        }

        private static SqlParameter[] CreateSaveClientPermanentNoteParameters(PermanentClientNote permanentClientNote)
        {
            SqlParameter customerIdParameter = new SqlParameter("@ClientID", SqlDbType.Int);
            SqlParameter createdByParameter = new SqlParameter("@CreatedBy", SqlDbType.Int);
            SqlParameter noteParamter = new SqlParameter("@Note", SqlDbType.Text);

            customerIdParameter.Value = permanentClientNote.ClientId;
            createdByParameter.Value = permanentClientNote.AuthorId;
            noteParamter.Value = permanentClientNote.Comment;

            return new[] {customerIdParameter, createdByParameter, noteParamter};
        }

        private static SqlParameter[] CreateUpdateClientNoteParameters(ClientNote clientNote)
        {
            SqlParameter noteIdParameter = new SqlParameter("@NoteId", SqlDbType.Int);
            SqlParameter activityTypeIdParameter = new SqlParameter("@ActivityTypeId", SqlDbType.Int);
            SqlParameter noteTypeIdParameter = new SqlParameter("@NoteTypeId", SqlDbType.Int);
            SqlParameter modifiedByParameter = new SqlParameter("@ModifiedBy", SqlDbType.Int);
            SqlParameter noteParamter = new SqlParameter("@Comment", SqlDbType.Text);
            noteIdParameter.Value = clientNote.NoteId;
            modifiedByParameter.Value = clientNote.ModifiedBy;
            noteParamter.Value = clientNote.Comment;
            activityTypeIdParameter.Value = clientNote.ActivityType.Id;
            noteTypeIdParameter.Value = clientNote.NoteType.Id;

            return new[]
                       {
                           noteIdParameter, activityTypeIdParameter, noteTypeIdParameter, modifiedByParameter,
                           noteParamter
                       };
        }

        private static SqlParameter[] CreateSaveClientNoteParameters(ClientNote note)
        {
            SqlParameter customerIdParameter = new SqlParameter("@ClientID", SqlDbType.Int);
            SqlParameter createdByParameter = new SqlParameter("@CreatedBy", SqlDbType.Int);
            SqlParameter noteParamter = new SqlParameter("@Note", SqlDbType.Text);
            SqlParameter activityTypeIdParameter = new SqlParameter("@ActivityTypeId", SqlDbType.Int);
            SqlParameter noteTypeIdParameter = new SqlParameter("@NoteTypeId", SqlDbType.Int);
            customerIdParameter.Value = note.ClientId;
            createdByParameter.Value = note.AuthorId;
            noteParamter.Value = note.Comment;
            activityTypeIdParameter.Value = note.ActivityType.Id;
            noteTypeIdParameter.Value = note.NoteType.Id;

            return new[]
                       {
                           customerIdParameter, createdByParameter, noteParamter, activityTypeIdParameter,
                           noteTypeIdParameter
                       };
        }

        private static SqlParameter[] CreateNotesClientPerm_LoadForAClientParameters(int clientId)
        {
            SqlParameter clientIdParameter = new SqlParameter("@ClientId", SqlDbType.BigInt);
            clientIdParameter.Value = clientId;
            return new[] { clientIdParameter };
        }

        #endregion
    }
}