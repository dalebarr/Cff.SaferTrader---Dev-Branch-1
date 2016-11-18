namespace Cff.SaferTrader.Core.Views
{
    public interface ICustomerNotesAdderView
    {
        void DisplayFeedback();
        void TogglePermanentNoteCheckBox(bool visible);
        void ToggleNoteDescriptors(bool visible);
        void ToggleNextCallDueDate(bool visible);
    }
}