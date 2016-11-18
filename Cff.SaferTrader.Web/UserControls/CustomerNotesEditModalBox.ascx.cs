using System;
using System.Collections.Generic;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class CustomerNotesEditModalBox : ModalBox, ICustomerNotesEditorView
    {
        private CustomerNotesEditorPresenter presenter;
        public event EventHandler NoteSaved;
        private const string SelectedNote = "SelectedNote";
      

        protected void Page_Load(object sender, EventArgs e)
        {            
            presenter = CustomerNotesEditorPresenter.Create(this);
            
            if(!IsPostBack)
            {
                Clear();
                Visible = false;
            }
        }

        protected void CancelButton_Click(object sender, ImageClickEventArgs e)
        {
            Close();
        }


        public string EncodedText(string value = "")
        {//TODO: is ugly, please  me in an interface
           if (string.IsNullOrEmpty(value))
           {
                // Ensures that Text returned to server is HTML encoded so that any control can render it safely
                return System.Web.HttpUtility.HtmlEncode(this.CommentEditTextBox.Text);
           }
           else
           {
                // Ensures that Text displayed inside the control is HTML decoded for correct rendering
                return System.Web.HttpUtility.HtmlDecode(value);
           }
        }

        protected void SaveButton_Click(object sender, ImageClickEventArgs e)
        {
            //TODO FOR Tommorrow,when user click save I need to be able to update the client note.
            
            CffPrincipal cffPrincipal = Context.User as CffPrincipal;
            if (cffPrincipal != null && ViewState != null)
            {
                int modifiedBy = cffPrincipal.CffUser.EmployeeId;
                object orginalNote = ViewState[SelectedNote];

                //TODO Interface ME!
                if (orginalNote.GetType() == typeof(CustomerNote))
                {
                    UpdateCustomerNote(orginalNote, modifiedBy, EncodedText(CommentEditTextBox.Text));
                }
                else if (orginalNote.GetType() == typeof(PermanentCustomerNote))
                {
                    UpdatePermanentNote(orginalNote, modifiedBy, EncodedText(CommentEditTextBox.Text));
                }
                else if (orginalNote.GetType() == typeof(ClientNote))
                {
                    UpdateClientNote(orginalNote, modifiedBy, EncodedText(CommentEditTextBox.Text));
                }
                else if (orginalNote.GetType() == typeof(PermanentClientNote))
                {
                    UpdateClientPermanentNote(orginalNote, modifiedBy, EncodedText(CommentEditTextBox.Text));
                }
            }
        }

        private void UpdateClientNote(object orginalNote, int modifiedBy, string newComment)
        {
            newComment = newComment.Replace("&amp;", "&");
            NoteType noteType = NoteType.Parse(int.Parse(NoteTypeDropDownList.SelectedValue));
            ActivityType activityType = ActivityType.Parse(int.Parse(ActivityTypeDropDownList.SelectedValue));

            ClientNote originalCustomerNote = orginalNote as ClientNote;
            if (originalCustomerNote != null)
            {
                presenter.UpdateClientNote(originalCustomerNote.NoteId, activityType, noteType, newComment, modifiedBy);
            }
        }

        private void UpdateCustomerNote(object orginalNote, int modifiedBy, string newComment)
        {
            newComment = newComment.Replace("&amp;", "&");
            NoteType noteType = NoteType.Parse(int.Parse(NoteTypeDropDownList.SelectedValue));
            ActivityType activityType = ActivityType.Parse(int.Parse(ActivityTypeDropDownList.SelectedValue));

            CustomerNote originalCustomerNote = orginalNote as CustomerNote;
            if (originalCustomerNote != null)
            {
                presenter.UpdateCustomerNote(originalCustomerNote.NoteId, activityType, noteType, newComment, modifiedBy);
            }
        }

        private void UpdatePermanentNote(object orginalNote, int modifiedBy, string newComment)
        { //clean up the &amp before saving to DB
            newComment = newComment.Replace("&amp;", "&");
            PermanentCustomerNote originalPermanentCustomerNote = orginalNote as PermanentCustomerNote;
            if (originalPermanentCustomerNote != null)
            {
                presenter.UpdatePermanentNote(originalPermanentCustomerNote.NoteId, newComment, modifiedBy);
            }
        }

        private void UpdateClientPermanentNote(object orginalNote, int modifiedBy, string newComment)
        {
            newComment = newComment.Replace("&amp;", "&");
            PermanentClientNote originalPermanentClientNote = orginalNote as PermanentClientNote;
            if (originalPermanentClientNote != null)
            {
                presenter.UpdateClientPermanentNote(originalPermanentClientNote.NoteId, newComment, modifiedBy);
            }
        }
        
        private void Close()
        {
            Clear();
            Visible = false;
        }

        public void Show(CustomerNote selectedCustomerNote)
        {
            PopulateCustomerNotesControls(selectedCustomerNote);
            ViewState.Add(SelectedNote, selectedCustomerNote);
            CustomerNoteDescriptors.Visible = true;
            Visible = true;

            BlockScreen();
        }

        public void Show(PermanentCustomerNote selectedPermanentCustomerNote)
        {
            CommentEditTextBox.Text = EncodedText(CustomerNotesParser.RemoveBr(selectedPermanentCustomerNote.Comment));
            CommentEditTextBox.Focus();
            ViewState.Add(SelectedNote, selectedPermanentCustomerNote);
            CustomerNoteDescriptors.Visible = false;
            Visible = true;

            BlockScreen();
        }

        public void Show(PermanentClientNote selectedPermanentClientNote)
        {
            CommentEditTextBox.Text = EncodedText(CustomerNotesParser.RemoveBr(selectedPermanentClientNote.Comment));
            CommentEditTextBox.Focus();
            ViewState.Add(SelectedNote, selectedPermanentClientNote);
            CustomerNoteDescriptors.Visible = false;
            Visible = true;

            BlockScreen();
        }

        public void Show(ClientNote selectedClientNote)
        {
            PopulateClientNotesControls(selectedClientNote);
            CommentEditTextBox.Text = EncodedText(CustomerNotesParser.RemoveBr(selectedClientNote.Comment));
            CommentEditTextBox.Focus();
            ViewState.Add(SelectedNote, selectedClientNote);
            CustomerNoteDescriptors.Visible = true;
            Visible = true;
            BlockScreen();
        }

        private void PopulateClientNotesControls(ClientNote selectedClientNote)
        {
            PopulateNoteTypes(NoteType.KnownTypesForNewNotes);
            PopulateActivityTypes(ActivityType.KnownTypesForNewNotes);

            CommentEditTextBox.Text = EncodedText(CustomerNotesParser.RemoveBr(selectedClientNote.Comment));
            ActivityTypeDropDownList.SelectedValue = selectedClientNote.ActivityType.Id.ToString();
            NoteTypeDropDownList.SelectedValue = selectedClientNote.NoteType.Id.ToString(); 
        }

        public void PopulateCustomerNotesControls(CustomerNote selectedCustomerNote)
        {
            PopulateNoteTypes(NoteType.KnownTypesForNewNotes);
            PopulateActivityTypes(ActivityType.KnownTypesForNewNotes);

            CommentEditTextBox.Text = EncodedText(CustomerNotesParser.RemoveBr(selectedCustomerNote.Comment));
            ActivityTypeDropDownList.SelectedValue = selectedCustomerNote.ActivityType.Id.ToString();
            NoteTypeDropDownList.SelectedValue = selectedCustomerNote.NoteType.Id.ToString();
        }

        private void Clear()
        {
            ViewState.Remove("EditNoteId");
            ActivityTypeDropDownList.ClearSelection();
            NoteTypeDropDownList.ClearSelection();
            CommentEditTextBox.Text = string.Empty;
            FeedbackLabel.Text = string.Empty;
            CustomerNoteDescriptors.Visible = true;
        }

        public void PopulateActivityTypes(Dictionary<int, string> activityTypes)
        {
            ActivityTypeDropDownList.DataSource = activityTypes;
            ActivityTypeDropDownList.DataTextField = "Value";
            ActivityTypeDropDownList.DataValueField = "Key";
            ActivityTypeDropDownList.DataBind();
        }

        public void PopulateNoteTypes(Dictionary<int, string> noteTypes)
        {
            NoteTypeDropDownList.DataSource = noteTypes;
            NoteTypeDropDownList.DataTextField = "Value";
            NoteTypeDropDownList.DataValueField = "Key";
            NoteTypeDropDownList.DataBind();
        }

        public void DisplayFeedback()
        {
            Close();
            if (NoteSaved != null)
            {
                NoteSaved(this, EventArgs.Empty);
            }
        }


    }
}