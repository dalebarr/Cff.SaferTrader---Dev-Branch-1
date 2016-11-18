using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class CustomerNote : Note, ICloneable
    {
        private readonly ActivityType activityType;
        private readonly NoteType noteType;
        private readonly long customerId;
        private readonly string customerName;
        private readonly string authorRole;
        private readonly int _clientID;

        public CustomerNote(long noteId, ActivityType activityType, NoteType noteType, string comment, int modifiedBy)
            : base(noteId, comment, modifiedBy)
        {
            this.activityType = activityType;
            this.noteType = noteType;
        }

        public CustomerNote(ActivityType activityType, NoteType noteType, string comment, int customerId, int authorId)
            : base(comment, authorId)
        {
            this.customerId = customerId;
            this.activityType = activityType;
            this.noteType = noteType;
        }

        public CustomerNote(long noteId, Date created, ActivityType activityType, NoteType noteType, string comment,
                          int authorId, string createdByEmployeeName, int modifiedBy, string modifiedByEmployeeName,
                          Date modified, string custName, int customerid, string authorRole, int clientID=0)
            : base(noteId, created, comment, authorId, createdByEmployeeName, modifiedBy, modifiedByEmployeeName, modified)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(createdByEmployeeName, "createdByEmployeeName");

            this.activityType = activityType;
            this.noteType = noteType;
            this.customerId = customerid;
            this.customerName = custName;
            this.authorRole = authorRole;
            if (clientID==0 && SessionWrapper.Instance.Get!=null)
                this._clientID = SessionWrapper.Instance.Get.ClientFromQueryString.Id;
            else
                this._clientID = clientID;
        }


        public CustomerNote(long noteId, Date created, ActivityType activityType, NoteType noteType, string comment,
                            int authorId, string createdByEmployeeName, int modifiedBy, string modifiedByEmployeeName,
                            Date modified)
            : base(noteId, created, comment, authorId, createdByEmployeeName, modifiedBy, modifiedByEmployeeName, modified)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(createdByEmployeeName, "createdByEmployeeName");

            this.activityType = activityType;
            this.noteType = noteType;
        }

        public CustomerNote(string comment, Date created, string createdbyemployeename) : base(comment, created, createdbyemployeename)
        {
        }

        public NoteType NoteType
        {
            get { return noteType; }
        }

        public ActivityType ActivityType
        {
            get { return activityType; }
        }

        public string ActivityTypeName
        {
            get { return ActivityType.Name; }
        }

        public string NoteTypeName
        {
            get { return noteType.Name; }
        }

        public long CustomerId
        {
            get { return customerId; }
        }

        public string CustomerName
        {
            get { return customerName; }
        }

        public string AuthorRole
        {
            get { return authorRole; }
        }

        public int ClientId
        { //needed for hyperlinks @All Clients Scope 
            get { return this._clientID; }
        }

        public string CreatedSortingDate {
            get
            { //we'll use this for more accurate sorting by date
                return (this.Created.Year.ToString() 
                    + this.Created.Month.ToString().PadLeft(2,'0') 
                        + this.Created.Day.ToString().PadLeft(2,'0'));
            }
        }

        public object Clone()
        { //perform a shallow copy of this object
           return this.MemberwiseClone();
        }

       

    }
}