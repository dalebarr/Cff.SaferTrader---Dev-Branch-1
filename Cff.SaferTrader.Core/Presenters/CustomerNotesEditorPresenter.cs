using System;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Core.Presenters
{
    public class CustomerNotesEditorPresenter
    {
        private readonly INotesRepository repository;
        private readonly ICustomerNotesEditorView view;

        public CustomerNotesEditorPresenter(ICustomerNotesEditorView view, INotesRepository repository)
        {
            this.repository = repository;
            this.view = view;
        }
        
        public static CustomerNotesEditorPresenter Create(ICustomerNotesEditorView view)
        {
            return new CustomerNotesEditorPresenter(view, RepositoryFactory.CreateCustomerNotesRepository());
        }

        public void UpdateCustomerNote(long noteId, ActivityType activityType, NoteType noteType, string comment, int modifiedBy)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(comment, "comment");
            
            CustomerNote customerNote = new CustomerNote(noteId, activityType, noteType, comment, modifiedBy);
            repository.UpdateCustomerNote(customerNote);
            view.DisplayFeedback();
        }


        public void UpdatePermanentNote(long noteId, string comment, int modifiedBy)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(comment, "comment");
            
            PermanentCustomerNote permanentCustomerNote = new PermanentCustomerNote(noteId, comment, modifiedBy);
            repository.UpdatePermanentCustomerNote(permanentCustomerNote);
            view.DisplayFeedback();
        }

        public void UpdateClientPermanentNote(long noteId, string comment, int modifiedBy)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(comment, "comment");

            PermanentClientNote permanentClientNote = new PermanentClientNote(noteId, comment, modifiedBy);
            repository.UpdatePermanentClientNote(permanentClientNote);
            view.DisplayFeedback();
        }

        public void UpdateClientNote(long noteId, ActivityType activityType, NoteType noteType, string newComment, int modifiedBy)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(newComment, "comment");
            ClientNote clientNote = new ClientNote(noteId, activityType, noteType, newComment, modifiedBy);
            repository.UpdateClientNote(clientNote);
            view.DisplayFeedback();
        }
    }
}