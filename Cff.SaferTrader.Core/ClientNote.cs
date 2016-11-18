using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class ClientNote : Note
    {
        private readonly ActivityType activityType;
        private readonly NoteType noteType;
        private readonly string _clientName;
        private readonly long clientId;

        //For Retrieving
        public ClientNote(long noteId, Date created, ActivityType activityType, NoteType noteType, string comment,
                          int authorId, string createdByEmployeeName, long clientId, int modifiedBy,
                          string modifiedByEmployeeName,
                          Date modified)
            : base(noteId, created, comment, authorId, createdByEmployeeName, modifiedBy, modifiedByEmployeeName, modified)
        {
            this.clientId = clientId;
            this.activityType = activityType;
            this.noteType = noteType;
        }

        public ClientNote(long noteId, Date created, ActivityType activityType, NoteType noteType, string comment,
                        int authorId, string createdByEmployeeName, long clientId, int modifiedBy,
                        string modifiedByEmployeeName,
                        Date modified, string clientName)
            : base(noteId, created, comment, authorId, createdByEmployeeName, modifiedBy, modifiedByEmployeeName, modified)
        {
            this._clientName = clientName;
            this.clientId = clientId;
            this.activityType = activityType;
            this.noteType = noteType;
        }


        //For Inserting
        public ClientNote(ActivityType activityType, NoteType noteType, string comment, int clientId, int authorId)
            : base(comment, authorId)
        {
            this.clientId = clientId;
            this.activityType = activityType;
            this.noteType = noteType;
        }

        //For Updating
        public ClientNote(long noteId, ActivityType activityType, NoteType noteType, string comment, int modifiedBy)
            : base(noteId, comment, modifiedBy)
        {
            this.activityType = activityType;
            this.noteType = noteType;
        }

        public ActivityType ActivityType
        {
            get { return activityType; }
        }

        public NoteType NoteType
        {
            get { return noteType; }
        }

        public long ClientId
        {
            get { return clientId; }
        }

        public string ActivityTypeName
        {
            get { return ActivityType.Name; }
        }

        public string NoteTypeName
        {
            get { return noteType.Name; }
        }

        public string ClientName
        {
            get { return _clientName; }
        }

        public object Clone()
        { //perform a shallow copy of this object
            return this.MemberwiseClone();
        }
    }
}