namespace Cff.SaferTrader.Core
{
    public interface ISecurityManager
    {
        void UpdateScope(Scope scope);
        bool CanViewContactsLink();
        bool CanViewCurrentTransactionsLink();
        bool CanViewTransactionArchiveLink();
        bool CanViewTransactionHistoryLink();
        bool CanViewReleaseTab();
        bool CanViewCustomerPermanentNotes();
        bool CanViewClientPermanentNotes();
        bool CanViewClientNotes();
        bool CanViewCustomerNotesAdder();
        bool CanViewCustomerNotes();
        bool CanCreatePermanentNotes();
        bool CanCreateClientNotes();
        bool CanCreateCustomerNotes();
        bool CanEditNextCallDueDate();
        bool CanViewScopeSelectionInTransactionSearch();
        bool CanViewAllClientsScopeSelectionInTransactionSearch();

        bool CanChangeSelectedCustomer();
        bool CanChangeSelectedClient();
    }
}