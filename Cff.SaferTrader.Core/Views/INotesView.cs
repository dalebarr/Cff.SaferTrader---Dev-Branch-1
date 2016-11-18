using System.Collections.Generic;
namespace Cff.SaferTrader.Core.Views
{
    public interface INotesView
    {
        void ShowCustomerNotes(IList<CustomerNote> customerNotes);
        void ShowPermanentNotes(IList<PermanentCustomerNote> permanentNotes);
        void ShowAllClientsPermanentNotes(IList<AllClientsPermanentNote> clientPermanentNotes);
        void ShowAClientsPermanentNotes(IList<PermanentClientNote> clientPermanentNotes);
        void ShowClientNotes(IList<ClientNote> clientNotes);
        void ShowAllCustomerNotesForClientOnRange(IList<CustomerNote> customerNotes);
        void ToggleCustomerPermanentNotes(bool visible);
        void ToggleClientPermanentNotes(bool visible);
        void ToggleCustomerNotesAdder(bool visible);
        void ToggleCustomerNotes(bool visible);
    }
}
