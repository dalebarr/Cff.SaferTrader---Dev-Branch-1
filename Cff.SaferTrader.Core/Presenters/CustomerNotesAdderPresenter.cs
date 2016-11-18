using System;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class CustomerNotesAdderPresenter
    {
        private readonly ICustomerNotesAdderView view;
        private readonly INotesRepository notesRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly ISecurityManager securityManager;

        public CustomerNotesAdderPresenter(ICustomerNotesAdderView view, IDictionaryRepository dictionaryRepository, INotesRepository notesRepository, ICustomerRepository customerRepository, ISecurityManager securityManager)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(dictionaryRepository, "dictionaryRepository");
            ArgumentChecker.ThrowIfNull(notesRepository, "notesRepository");
            ArgumentChecker.ThrowIfNull(customerRepository, "customerRepository");
            ArgumentChecker.ThrowIfNull(securityManager, "securityManager");


            this.view = view;
            this.notesRepository = notesRepository;
            this.customerRepository = customerRepository;
            this.securityManager = securityManager;
        }

        public void SaveCustomerNote(CustomerNote customerNote)
        {
            ArgumentChecker.ThrowIfNull(customerNote, "customerNote");
            notesRepository.InsertCustomerNote(customerNote);
            view.DisplayFeedback();
        }

        public void SavePermanentNote(PermanentCustomerNote permanentCustomerNote)
        {
            ArgumentChecker.ThrowIfNull(permanentCustomerNote, "permanentNote");
            notesRepository.InsertPermanentNote(permanentCustomerNote);
            view.DisplayFeedback();
        }

        public void SaveClientPermanentNote(PermanentClientNote permanentClientNote)
        {
            ArgumentChecker.ThrowIfNull(permanentClientNote, "clientPermanentNote");
            notesRepository.InsertClientPermanentNote(permanentClientNote);
            view.DisplayFeedback();
        }

        public void UpdateCustomerNextCallDue(Date nextCallDue, int customerId, Date modifiedDate, int authorId)
        {
            ArgumentChecker.ThrowIfNull(nextCallDue, "NextCallDue");
            ArgumentChecker.ThrowIfNull(modifiedDate, "modifiedDate");

            customerRepository.UpdateCustomerCallDue(nextCallDue, 0, customerId, modifiedDate, authorId);
        }

        public void SaveClientNote(ClientNote clientNote)
        {
            ArgumentChecker.ThrowIfNull(clientNote, "clientNote");
            notesRepository.InsertClientNote(clientNote);
            view.DisplayFeedback();
        }

        public void LockDown()
        {
            bool canCreatePermanentNotes = securityManager.CanCreatePermanentNotes();
            bool canCreateClientNotes = securityManager.CanCreateClientNotes();
            bool canCreateCustomerNotes = securityManager.CanCreateCustomerNotes();
            bool canOnlyCreatePermanentNotes = canCreatePermanentNotes && !canCreateClientNotes && !canCreateCustomerNotes;

            // TODO: Refactor this
            view.TogglePermanentNoteCheckBox(canCreatePermanentNotes && !canOnlyCreatePermanentNotes);
            view.ToggleNoteDescriptors(!canOnlyCreatePermanentNotes);
            view.ToggleNextCallDueDate(securityManager.CanEditNextCallDueDate());
        }
    }
}
