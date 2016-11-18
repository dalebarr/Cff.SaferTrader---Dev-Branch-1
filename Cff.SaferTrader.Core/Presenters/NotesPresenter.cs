using System.Collections.Generic;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class NotesPresenter
    {
        private readonly INotesView view;
        private readonly INotesRepository repository;
        private readonly ISecurityManager securityManager;

        public NotesPresenter(INotesView view, INotesRepository repository, ISecurityManager securityManager)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(repository, "repository");
            ArgumentChecker.ThrowIfNull(securityManager, "securityManager");

            this.view = view;
            this.repository = repository;
            this.securityManager = securityManager;
        }


        public void LoadPermanentCustomerNotes(int customerId, DateRange dateRange)
        {
            IList<PermanentCustomerNote> permanentNotes = repository.LoadPermanentCustomerNoteOnRange(customerId, dateRange);
            view.ShowPermanentNotes(permanentNotes);
        }

        public  IList<PermanentCustomerNote>  GetPermanentCustomerNotes(int customerId, DateRange dateRange)
        {
           return repository.LoadPermanentCustomerNoteOnRange(customerId, dateRange);
        }

        public void LoadCustomerNotes(int customerId, DateRange dateRange)
        {
            IList<CustomerNote> customerNotes = repository.LoadCustomerNotes(customerId, dateRange);
            view.ShowCustomerNotes(customerNotes);

            IList<PermanentCustomerNote> permanentNotes = repository.LoadPermanentCustomerNote(customerId);
            view.ShowPermanentNotes(permanentNotes);
        }

        public IList<CustomerNote> GetCustomerNotes(int customerId, DateRange dateRange, ActivityType activityType, NoteType noteType)
        {
            if (activityType == ActivityType.AllNotes) { activityType = null; }
            if (noteType == NoteType.AllNotes) { noteType = null; }
            if (noteType == NoteType.OldNote) { activityType = null; }

            return repository.LoadCustomerNotes(customerId, dateRange, noteType, activityType);
        }

        public IList<CustomerNote> GetCustomerNotes(int customerId, DateRange dateRange)
        {
            return repository.LoadCustomerNotes(customerId, dateRange);
        }


        public void LoadCustomerNotes(int customerId, DateRange dateRange, ActivityType activityType, NoteType noteType)
        {
            if (activityType == ActivityType.AllNotes) { activityType = null; }
            if (noteType == NoteType.AllNotes) { noteType = null; }
            if (noteType == NoteType.OldNote) { activityType = null; }

            IList<CustomerNote> customerNotes = repository.LoadCustomerNotes(customerId,
                                             dateRange,
                                             noteType,
                                             activityType);
            view.ShowCustomerNotes(customerNotes);

            IList<PermanentCustomerNote> permanentCustomerNotes = repository.LoadPermanentCustomerNote(customerId);
            view.ShowPermanentNotes(permanentCustomerNotes);

        }

        public void LoadAllClientsPermanentNotesOnRange(DateRange dateRange)
        {
            view.ShowAllClientsPermanentNotes(repository.LoadCffPermanentNotesOnRange(dateRange));
        }

        public void LoadAllClientsPermanentNotes()
        {
            view.ShowAllClientsPermanentNotes(repository.LoadCffPermanentNotes());
        }


        public void LoadAClientsPermanentNotes(int clientId)
        {
            IList<PermanentClientNote> permanentNotes = repository.LoadPermanentClientsNotesFor(clientId);
            view.ShowAClientsPermanentNotes(permanentNotes);

            //IList<ClientNote> clientNotes = repository.LoadClientNotesFor(clientId);
            //view.ShowClientNotes(clientNotes);
        }


        public IList<PermanentClientNote> GetPermanentClientNotesOnRange(int clientid, DateRange dateRange)
        {
            return repository.LoadPermanentClientsNotesForOnRange(clientid, dateRange);
        }

        public void LoadAllClientsPermanentNotes(DateRange dateRange)
        {
            view.ShowAllClientsPermanentNotes(repository.LoadCffPermanentNotesOnRange(dateRange));
        }

        public IList<AllClientsPermanentNote> GetAllClientsPermanentNotes(DateRange dateRange)
        {
            return repository.LoadCffPermanentNotesOnRange(dateRange);
        }


        public void LoadAClientsPermanentNotes(int clientId, DateRange dateRange)
        {
            IList<PermanentClientNote> permanentNotes = repository.LoadPermanentClientsNotesForOnRange(clientId, dateRange);
            view.ShowAClientsPermanentNotes(permanentNotes);
        }


        public void LoadAllCustomerNotesForClientOnRange(int clientId, DateRange dateRange)
        {
            //todo: refactor date to mmddyyy format
            IList<CustomerNote> custNotes = repository.LoadAllCustomerNotesForClientOnRange(clientId, dateRange);
             view.ShowAllCustomerNotesForClientOnRange(custNotes);
        }


        public IList<CustomerNote> GetAllCustomerNotes(int clientId, DateRange dateRange)
        {
            return repository.LoadAllCustomerNotesForClientOnRange(clientId, dateRange);
        }

        public void LoadClientNotes(int clientId, DateRange dateRange)
        {
            IList<ClientNote> clientNotes = repository.LoadClientNotesOnRange(clientId, dateRange);
            view.ShowClientNotes(clientNotes);
        }

        public IList<ClientNote> GetClientNotesList(int clientId, DateRange dateRange)
        {
            return repository.LoadClientNotesOnRange(clientId, dateRange);
        }

        public bool UpdateCustomerNotes(CustomerNote custNote, ref string errMsg)
        {
            try
            {
                repository.UpdateCustomerNote(custNote);
                return true;
            }
            catch (System.Exception exc)
            {
                if (exc.Message.ToLower().IndexOf("foreign key constraint", 0) > 0 &&
                        (exc.Message.ToLower().IndexOf("activitytype", 0) > 0) && custNote.ActivityType.Id >= 0)
                {
                    errMsg = "Unable to modify notes for this activity type.";
                }
                else
                {
                    errMsg = exc.Message;
                }
                return false;
            }
        }

        public bool UpdateClientNotes(ClientNote clientNote, ref string errMsg)
        {
            try
            {
                repository.UpdateClientNote(clientNote);
                return true;
            }
            catch (System.Exception exc)
            {
                if (exc.Message.ToLower().IndexOf("foreign key constraint", 0) > 0 &&
                        (exc.Message.ToLower().IndexOf("activitytype", 0) > 0) && clientNote.ActivityType.Id >= 0)
                {
                    errMsg = "Unable to modify notes for this activity type.";
                }
                else
                {
                    errMsg = exc.Message;
                }
                return false;
            }
        }

        public bool UpdatePermanentClientNotes(PermanentClientNote clientNote, ref string errMsg)
        {
            try
            {
                repository.UpdatePermanentClientNote(clientNote);
                return true;
            }
            catch (System.Exception exc)
            {
                if (exc.Message.ToLower().IndexOf("foreign key constraint", 0) > 0)
                {
                    errMsg = "Unable to modify notes for this activity type.";
                }
                else
                {
                    errMsg = exc.Message;
                }
                return false;
            }
        }


        public bool UpdatePermanentCustNote(PermanentCustomerNote permanentCustNote, ref string errMsg)
        {
            try
            {
                repository.UpdatePermanentCustomerNote(permanentCustNote);
                return true;
            }
            catch (System.Exception exc)
            {
                errMsg = exc.Message;
                return false;
            }
        }

        public void LockDown()
        {
            //view.ToggleCustomerPermanentNotes(securityManager.CanViewCustomerPermanentNotes());
            view.ToggleClientPermanentNotes(securityManager.CanViewClientPermanentNotes());
            //view.ToggleClientNotes(securityManager.CanViewClientNotes());
            view.ToggleCustomerNotesAdder(securityManager.CanViewCustomerNotesAdder());
            view.ToggleCustomerNotes(securityManager.CanViewCustomerNotes());
        }
    }
}