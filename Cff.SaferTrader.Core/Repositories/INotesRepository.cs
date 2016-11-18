using System.Collections.Generic;

namespace Cff.SaferTrader.Core.Repositories
{
    public interface INotesRepository
    {
        void InsertCustomerNote(CustomerNote customerNote);
        void InsertPermanentNote(PermanentCustomerNote permanentCustomerNote);
        
        IList<CustomerNote> LoadCustomerNotes(int customerId);
        IList<PermanentCustomerNote> LoadPermanentCustomerNote(int customerId);
        IList<PermanentCustomerNote> LoadPermanentCustomerNoteOnRange(int customerId, DateRange dateRange);

        IList<CustomerNote> LoadCustomerNotes(int customerId, DateRange dateRange);
        IList<CustomerNote> LoadCustomerNotes(int customerId, DateRange dateRange, NoteType noteType, ActivityType activityType);
        void UpdateCustomerNote(CustomerNote customerNote);
        void UpdatePermanentCustomerNote(PermanentCustomerNote permanentCustomerNote);
        
        
        IList<AllClientsPermanentNote> LoadCffPermanentNotes();
        void InsertClientPermanentNote(PermanentClientNote permanentClientNote);
        void UpdatePermanentClientNote(PermanentClientNote permanentClientNote);

        IList<PermanentClientNote> LoadPermanentClientsNotesFor(int clientId);
        void InsertClientNote(ClientNote clientNote);
        IList<ClientNote> LoadClientNotesFor(int clientId);
        void UpdateClientNote(ClientNote clientNote);

        IList<AllClientsPermanentNote> LoadCffPermanentNotesOnRange(DateRange dateRange);
        IList<PermanentClientNote> LoadPermanentClientsNotesForOnRange(int clientId, DateRange dateRange);
        IList<CustomerNote> LoadAllCustomerNotesForClientOnRange(int clientId, DateRange dateRange);
        IList<ClientNote> LoadClientNotesOnRange(int clientId, DateRange dateRange);
    }
}