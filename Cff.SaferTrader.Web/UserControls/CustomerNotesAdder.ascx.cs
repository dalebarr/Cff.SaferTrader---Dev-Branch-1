using System;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.SecurityManager;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Web.UserControls
{
    public partial class CustomerNotesAdder : UserControl, ICustomerNotesAdderView
    {
        private CustomerNotesAdderPresenter presenter;
       
        #region ICustomerNotesAdderView Members

        public void DisplayFeedback()
        {
            ClearControls();
            FeedbackLabel.Text = "Note saved successfully.";

            if (SaveSuccessful != null)
            {
                SaveSuccessful(this, EventArgs.Empty);
            }
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

        public event EventHandler Cancel;
        public event EventHandler SaveSuccessful;
        public event EventHandler NextCallDueUpdated;
        public bool PermCheckBoxEnable;


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


        protected void Page_Load(object sender, EventArgs e)
        {
            NextCallDueTextBox.Attributes.Add("readonly", "readonly");
            ISecurityManager securityManager = SecurityManagerFactory.Create(CurrentPrincipal, this.CurrentScope());
            presenter = new CustomerNotesAdderPresenter(this,
                                                       RepositoryFactory.CreateDictionaryRepository(),
                                                       RepositoryFactory.CreateCustomerNotesRepository(),
                                                       RepositoryFactory.CreateCustomerRepository(), securityManager);
            presenter.LockDown();

            if (!IsPostBack)
            {
                ClearControls();
                PopulateDropDownLists();
            }



        }

        public bool PermanentCheckBoxEnable()
        {
            ISecurityManager securityManager = SecurityManagerFactory.Create(CurrentPrincipal, this.CurrentScope());
            PermCheckBoxEnable = securityManager.CanCreatePermanentNotes();
            return PermCheckBoxEnable;
        }


        protected void PermanentNoteCheckBox_CheckChanged(object sender, EventArgs e)
        {
            CustomerNoteDescriptors.Visible = !PermanentNoteCheckBox.Checked;
            bool bIsClientSelected = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.IsClientSelected :
                (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsClientSelected : false;

            bool bIsCustomerSelected = (SessionWrapper.Instance.Get!=null)?SessionWrapper.Instance.Get.IsCustomerSelected :
                (!string.IsNullOrEmpty(QueryString.ViewIDValue))?SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsCustomerSelected:false;

            if (bIsClientSelected && !bIsCustomerSelected) {
                ActivityTypeDropDownList.Visible = true;
                NoteTypeDropDownList.Visible = true;
                NextCallDueDiv.Visible = false;
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            ClearControls();
            if (Cancel != null)
            {
                Cancel(sender, e);
            }
        }

        public string EncodedText(string value)
        {
            // Ensures that Text returned to server is HTML encoded so that any control can render it safely
            return System.Web.HttpUtility.HtmlEncode(value);
        }


        protected void SaveButton_Click(object sender, EventArgs e)
        {
            int employeeId = CurrentPrincipal.CffUser.EmployeeId;
            string comment = string.IsNullOrEmpty(CommentTextBox.Text) ? "" : CommentTextBox.Text;
            if (!string.IsNullOrEmpty(comment))
                //comment = comment.Replace("\n", ";");  //dbb
                comment = comment.Replace("\n", " ");

            ICffClient xClient = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.ClientFromQueryString :
                  (!string.IsNullOrWhiteSpace(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).ClientFromQueryString : null;

            bool bIsAllClientsSelected = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.IsAllClientsSelected :
                                        (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsAllClientsSelected : false;

            bool bIsClientSelected = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.IsClientSelected :
                                        (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsClientSelected : false;

            bool bIsCustomerSelected = (SessionWrapper.Instance.Get != null) ? SessionWrapper.Instance.Get.IsCustomerSelected :
                                        (!string.IsNullOrEmpty(QueryString.ViewIDValue)) ? SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsCustomerSelected : false;

            if (bIsAllClientsSelected)
            {
                int clientId = SessionWrapper.Instance.Get.ClientFromQueryString.Id;
                var permanentNote = new PermanentClientNote(comment, clientId, employeeId);
                presenter.SaveClientPermanentNote(permanentNote);
            }
            else if (bIsCustomerSelected)
            {
                //int customerId = SessionWrapper.Instance.Get.Customer.Id;
                int customerId = SessionWrapper.Instance.Get.CustomerFromQueryString.Id;
                string nextCallDueText = EncodedText(NextCallDueTextBox.Text);
                ActivityType activityType = ActivityType.Parse(int.Parse(ActivityTypeDropDownList.SelectedValue));
                NoteType noteType = NoteType.Parse(int.Parse(NoteTypeDropDownList.SelectedValue));

                SaveNoteForCustomer(comment, employeeId, customerId, nextCallDueText, activityType, noteType);
            }
            else if (bIsClientSelected)
            {
                if (PermanentNoteCheckBox.Checked)
                {
                    int clientId = xClient.Id;
                    var permanentNote = new PermanentClientNote(comment, clientId, employeeId);
                    presenter.SaveClientPermanentNote(permanentNote);
                }
                else
                {
                    int clientId = xClient.Id;
                    ActivityType activityType = ActivityType.Parse(int.Parse(ActivityTypeDropDownList.SelectedValue));
                    NoteType noteType = NoteType.Parse(int.Parse(NoteTypeDropDownList.SelectedValue));
                    var clientNote = new ClientNote(activityType,noteType, comment, clientId, employeeId);
                    presenter.SaveClientNote(clientNote);
                }
            }
        }

        private void SaveNoteForCustomer(string comment, int employeeId, int customerId, string nextCallDueText,
                                         ActivityType activityType, NoteType noteType)
        {
            if (!string.IsNullOrEmpty(comment))
                //comment = comment.Replace("\n", ";"); //dbb
                comment = comment.Replace("\n", " ");  

            if (PermanentNoteCheckBox.Checked)
            {
               
                var permanentNote = new PermanentCustomerNote(comment, customerId, employeeId);
                presenter.SavePermanentNote(permanentNote);
            }
            else
            {
                if (!string.IsNullOrEmpty(nextCallDueText))
                {
                    var nextCallDueDate = new Date(DateTime.Parse(nextCallDueText));
                    UpdateNextCallDueDate(nextCallDueDate, customerId, new Calendar().Now, employeeId);
                }

                SaveCustomerNote(employeeId, comment, customerId, activityType, noteType);
            }
        }

        private void SaveCustomerNote(int employeeId, string comment, int customerId, ActivityType activityType,
                                      NoteType noteType)
        {
            if (!string.IsNullOrEmpty(comment))
                //comment = comment.Replace("\n", ";"); //dbb
                comment = comment.Replace("\n", " ");

            var customerNote = new CustomerNote(activityType, noteType, comment, customerId, employeeId);
            presenter.SaveCustomerNote(customerNote);
        }

        private void UpdateNextCallDueDate(Date nextCallDueDate, int customerId, Date now, int employeeId)
        {
            presenter.UpdateCustomerNextCallDue(nextCallDueDate, customerId, now, employeeId);

            if (NextCallDueUpdated != null)
            {
                NextCallDueUpdated(nextCallDueDate, new EventArgs());
            }
        }


        private void ClearControls()
        {
            ActivityTypeDropDownList.ClearSelection();
            NoteTypeDropDownList.ClearSelection();

            NextCallDueTextBox.Text = System.Web.HttpUtility.HtmlDecode(string.Empty);
            CommentTextBox.Text = string.Empty;
            FeedbackLabel.Text = string.Empty;
            CustomerNoteDescriptors.Visible = !SessionWrapper.Instance.Get.IsAllClientsSelected;
            PermanentNoteCheckBox.Checked = false;

        }

        private void PopulateDropDownLists()
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

        private CffPrincipal CurrentPrincipal
        {
            get { return Context.User as CffPrincipal; }
        }
    }
}