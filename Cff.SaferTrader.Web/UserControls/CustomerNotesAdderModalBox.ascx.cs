using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class CustomerNotesAdderModalBox : ModalBox, ICustomerNotesAdderView
    {
        private CustomerNotesAdderPresenter presenter;

        #region ICustomerNotesAdderView Members

        private Scope CurrentScope()
        {
            if (SessionWrapper.Instance.Get != null)
                return SessionWrapper.Instance.Get.Scope;
            else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                return SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).Scope;
            else
            {
                if (QueryString.CustomerId != null)
                    return Scope.CustomerScope;
                else if (QueryString.ClientId != null)
                    return Scope.ClientScope;
                return Scope.AllClientsScope;
            }
        }


        public void DisplayFeedback()
        {
            ClearControls();
            ScriptManager.RegisterClientScriptBlock(this, typeof (Page), "Saved", "alert('Note saved successfully');",
                                                    true);
            Visible = false;
        }

        public void TogglePermanentNoteCheckBox(bool visible)
        {
            AddPermanentNote.Visible = visible;
        }

        public void ToggleNoteDescriptors(bool visible)
        {
            CustomerNoteDescriptors.Visible = visible;
        }

        public void ToggleNextCallDueDate(bool visible)
        {
            NextCallDueDiv.Visible = visible;
        }

        #endregion

        public event EventHandler NextCallDueUpdated;

        protected void Page_Load(object sender, EventArgs e)
        {
            NextCallDueTextBox.Attributes.Add("readonly", "readonly");
            FeedbackLabel.Text = string.Empty;

            ISecurityManager securityManager = SecurityManagerFactory.Create(CurrentPrincipal, this.CurrentScope());
            presenter = new CustomerNotesAdderPresenter(this,
                                                       RepositoryFactory.CreateDictionaryRepository(),
                                                       RepositoryFactory.CreateCustomerNotesRepository(),
                                                       RepositoryFactory.CreateCustomerRepository(), securityManager);
            presenter.LockDown();

            if (!IsPostBack)
            {
                ClearControls();
                Visible = false;
            }
        }

        public void PopulateDropDownLists()
        {
            ActivityTypeDropDownList.DataSource = ActivityType.KnownTypesForNewNotes;
            ActivityTypeDropDownList.DataTextField = "Value";
            ActivityTypeDropDownList.DataValueField = "Key";
            ActivityTypeDropDownList.DataBind();

            NoteTypeDropDownList.DataSource = NoteType.KnownTypesForNewNotes;
            NoteTypeDropDownList.DataTextField = "Value";
            NoteTypeDropDownList.DataValueField = "Key";
            NoteTypeDropDownList.DataBind();
        }

        protected void PermanentNoteCheckBox_CheckChanged(object sender, EventArgs e)
        {
            CustomerNoteDescriptors.Visible = !PermanentNoteCheckBox.Checked;
        }

      
        public void Show()
        {
            ClearControls();
            PopulateDropDownLists();
            Visible = true;

            BlockScreen();
        }

        private void ClearControls()
        {
            ActivityTypeDropDownList.ClearSelection();
            NoteTypeDropDownList.ClearSelection();

            NextCallDueTextBox.EncodedText = string.Empty;
            CommentTextBox.EncodedText = string.Empty;
            FeedbackLabel.Text = string.Empty;

            PermanentNoteCheckBox.Checked = false;
            CustomerNoteDescriptors.Visible = true;
        }

        private CffPrincipal CurrentPrincipal
        {
            get { return Context.User as CffPrincipal; }
        }

        protected void SaveButton_Click(object sender, ImageClickEventArgs e)
        {
            int employeeId = CurrentPrincipal.CffUser.EmployeeId;
            ICffCustomer xCustomer = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.CustomerFromQueryString :
                   (!string.IsNullOrWhiteSpace(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).CustomerFromQueryString : null;

            if (PermanentNoteCheckBox.Checked)
            {
                var permanentNote = new PermanentCustomerNote(CommentTextBox.EncodedText, ((xCustomer==null)?0:xCustomer.Id), employeeId);
                presenter.SavePermanentNote(permanentNote);
            }
            else
            {
                if (!string.IsNullOrEmpty(NextCallDueTextBox.EncodedText))
                {
                    var calendar = new Calendar();
                    var nextCallDueDate = new Date(DateTime.Parse(NextCallDueTextBox.EncodedText));

                    presenter.UpdateCustomerNextCallDue(nextCallDueDate, ((xCustomer == null) ? 0 : xCustomer.Id), calendar.Now, employeeId);

                    if (NextCallDueUpdated != null)
                    {
                        NextCallDueUpdated(nextCallDueDate, new EventArgs());
                    }
                }
                ActivityType activityType = ActivityType.Parse(int.Parse(ActivityTypeDropDownList.SelectedValue));
                NoteType noteType = NoteType.Parse(int.Parse(NoteTypeDropDownList.SelectedValue));

                var customerNote = new CustomerNote(activityType, noteType, CommentTextBox.EncodedText, ((xCustomer == null) ? 0 : xCustomer.Id), employeeId);
                presenter.SaveCustomerNote(customerNote);
            }
        }

        protected void CancelButton_Click(object sender, ImageClickEventArgs e)
        {
            ClearControls();
            Visible = false;
        }

        protected void EmptyAddNotesFeedbackLabel()
        {
            FeedbackLabel.Text = "";
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            FeedbackLabel.Text = "";
        }

        protected void TextBox_TextChanged(object sender, EventArgs e)
        {
            FeedbackLabel.Text = "";
        }
    }
}