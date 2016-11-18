using System;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class PermanentCustomerNote : Note
    {
        private readonly long customerId;
        
        public PermanentCustomerNote(long noteId, string comment, int modifiedBy) : base(noteId, comment, modifiedBy)
        {
        }

        public PermanentCustomerNote(string comment, long customerId, int authorId) : base(comment, authorId)
        {
            this.customerId = customerId;
        }

        public PermanentCustomerNote(long noteId, Date created, string comment, int authorId,
                                     string createdByEmployeeName, int modifiedBy, string modifiedByEmployeeName,
                                     Date modified)
            : base(noteId, created, comment, authorId, createdByEmployeeName, modifiedBy, modifiedByEmployeeName, modified)
        {
        }

        public NoteType NoteType
        {
            get
            {
                Calendar calendar = new Calendar();
                return Created < calendar.OldNotesImported ? NoteType.OldNote : NoteType.NewPermanentNote;
            }
        }

        public long CustomerId
        {
            get { return customerId; }
        }


        public string CreatedSortingDate
        {
            get
            { //we'll use this for more accurate sorting by date
                return (this.Created.Year.ToString()
                    + this.Created.Month.ToString().PadLeft(2, '0')
                        + this.Created.Day.ToString().PadLeft(2, '0'));
            }
        }

        public object Clone()
        { //perform a shallow copy of this object
            return this.MemberwiseClone();
        }
    }
}