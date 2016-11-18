using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public abstract class Note
    {
        private readonly long noteId;
        private readonly Date created;
        private readonly int authorId;
        private readonly string createdByEmployeeName;
        private string modifiedByEmployeeName;

        private string comment;
        private int modifiedBy;
        private Date modified;

        protected Note(long noteId, Date created, string comment, int authorId,
                                   string createdByEmployeeName, int modifiedBy,
                                   string modifiedByEmployeeName,
                                   Date modified) : this(comment)
        {
            this.noteId = noteId;
            this.created = created;
            this.createdByEmployeeName = createdByEmployeeName;
            this.modifiedBy = modifiedBy;
            this.modifiedByEmployeeName = modifiedByEmployeeName;
            this.modified = modified;
            this.authorId = authorId;
        }

        protected Note(string comment, Date created, string createdbyemployeename) : this(comment)
        {
            this.created = created;
            this.createdByEmployeeName = createdbyemployeename;
        }

        protected Note(long noteId, string comment, int modifiedBy) : this(comment)
        {
            this.noteId = noteId;
            this.modifiedBy = modifiedBy;
        }

        private Note(string comment)
        {
            if (!string.IsNullOrEmpty(comment)) {
                this.comment = System.Web.HttpUtility.HtmlEncode(comment);
            }
            this.comment = comment;
        }

        protected Note(string comment, int authorId) : this(comment)
        {
            this.authorId = authorId;
        }

       

        public int ModifiedBy
        {
            get { return modifiedBy; }
            set { this.modifiedBy = value; }
        }

        public long NoteId
        {
            get { return noteId; }
        }

        public string CreatedByEmployeeName
        {
            get { return createdByEmployeeName; }
        }

        public Date Created
        {
            get { return created; }
        }

        public string Comment
        {
            get {  return comment; }
            set { this.comment = value; }
        }

        public int AuthorId
        {
            get { return authorId; }
        }

        public string ModifiedByEmployeeName
        {
            get { return modifiedByEmployeeName; }
            set { this.modifiedByEmployeeName = value; }
        }

        public Date Modified
        {
            get { return modified; }
            set { this.modified = value;  }
        }

    }
}
