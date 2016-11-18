using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PermanentClientNote : Note
    {
        private readonly long clientId;

        public PermanentClientNote(long noteId, string comment, int modifiedBy) : base(noteId, comment, modifiedBy)
        {
        }

        public PermanentClientNote(string comment, long clientId, int authorId) : base(comment, authorId)
        {
            this.clientId = clientId;
        }

        public PermanentClientNote(long noteId, int clientId, Date created, string comment, int authorId,
                                   string createdByEmployeeName, int modifiedBy,
                                   string modifiedByEmployeeName,
                                   Date modified)
            : base(noteId, created, comment, authorId, createdByEmployeeName, modifiedBy, modifiedByEmployeeName, modified)
        {
            this.clientId = clientId;
        }

        public long ClientId
        {
            get { return clientId; }
        }

        public object Clone()
        { //perform a shallow copy of this object
            return this.MemberwiseClone();
        }
    }


    [Serializable]
    public class AllClientsPermanentNote : Note
    {
        private readonly long       _clientId;
        private readonly string     _clientName;
   
        public AllClientsPermanentNote(long noteId, string comment, int modifiedBy)
            : base(noteId, comment, modifiedBy)
        {
        }

        public AllClientsPermanentNote(string comment, long clientId, string clientName, int authorId)
            : base(comment, authorId)
        {
            this._clientId = clientId;
            this._clientName = clientName;
        }

        public AllClientsPermanentNote(long noteId, int clientId, string clientName, 
                                    Date created, string comment, int authorId,
                                   string createdByEmployeeName, int modifiedBy,
                                   string modifiedByEmployeeName,
                                   Date modified)
            : base(noteId, created, comment, authorId, createdByEmployeeName, modifiedBy, modifiedByEmployeeName, modified)
        {
            this._clientId = clientId;
            this._clientName = clientName;
        }

        public long ClientId
        {
            get { return _clientId; }
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