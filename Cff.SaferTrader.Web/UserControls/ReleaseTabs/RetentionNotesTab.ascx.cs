using System;
using System.Collections.Generic;
using System.Web.UI;
using Cff.SaferTrader.Core;
using Cff.SaferTrader.Core.Presenters;
using Cff.SaferTrader.Core.Views;

namespace Cff.SaferTrader.Web.UserControls.ReleaseTabs
{
    public partial class RetentionNotesTab : UserControl, IRetentionTab, IRetentionNotesTabView, IPrintableView
    {
        private RetentionNotesTabPresenter presenter;
        private IList<ClientNote> retentionNotes;
        private String retnClientName;

        #region IRetentionDetailsTabView Members

        public void DisplayRetentionNotes(IRetentionNote retentionNote)
        {
            retentionNotes = new List<ClientNote>();

            RetentionNotesLiteral.Text = retentionNote.GetNote();
            ClientNote retnClientNote = new ClientNote(ActivityType.AllNotes, NoteType.AllNotes, 
                        retentionNote.GetNote(), 0, 0);
            retentionNotes.Add(retnClientNote);
        }

        #endregion

        public void LoadTab(RetentionSchedule retentionSchedule)
        {
            retnClientName = retentionSchedule.ClientName;

            if (SessionWrapper.Instance.Get.IsClientSelected)
            {
                presenter = RetentionNotesTabPresenter.Create(this);
                presenter.LoadRetentionNotesFor(retentionSchedule.Id);
            }
        }
        public void ClearTabData()
        {
            RetentionNotesLiteral.Text = string.Empty;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region IPrintableView Members
            public void Print()
            { //Ref: CFF-13
                PrintableClientNotes printable = new PrintableClientNotes(retnClientName, retentionNotes, QueryString.ViewIDValue);
                String script = PopupHelper.ShowPopup(printable, Server);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Popup", script, true);
            }
        #endregion

    }
}