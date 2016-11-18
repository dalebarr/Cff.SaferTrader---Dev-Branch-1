namespace Cff.SaferTrader.Core.SecurityManager
{
    public class AdministratorSecurityManager : ISecurityManager
    {
        private Scope scope;

        public AdministratorSecurityManager(Scope scope)
        {
            this.scope = scope;
        }

        #region ISecurityManager Members

        public bool CanViewContactsLink()
        {
            return true;
        }

        public bool CanViewCurrentTransactionsLink()
        {
            return scope == Scope.CustomerScope;
        }

        public bool CanViewTransactionArchiveLink()
        {
            return scope == Scope.CustomerScope;
        }

        public bool CanViewReleaseTab()
        {
            return true;
        }

        public bool CanViewCustomerPermanentNotes()
        {
            return scope == Scope.CustomerScope;
        }

        public bool CanViewClientPermanentNotes()
        {
            return scope == Scope.AllClientsScope || scope == Scope.ClientScope;
        }

        public bool CanViewClientNotes()
        {
            return scope == Scope.ClientScope;
        }

        public bool CanViewCustomerNotesAdder()
        {
            return true;
        }

        public bool CanViewCustomerNotes()
        {
            return scope == Scope.CustomerScope;
        }

        public bool CanCreatePermanentNotes()
        {
            return true;
        }

        public bool CanCreateClientNotes()
        {
            return scope == Scope.ClientScope;
        }

        public bool CanCreateCustomerNotes()
        {
            return scope == Scope.CustomerScope;
        }

        public bool CanEditNextCallDueDate()
        {
            return scope == Scope.CustomerScope;
        }

        public bool CanViewScopeSelectionInTransactionSearch()
        {
            return scope != Scope.AllClientsScope;
        }

        public bool CanViewAllClientsScopeSelectionInTransactionSearch()
        {
            return true;
        }

        public bool CanChangeSelectedCustomer()
        {
            return true;
        }

        public bool CanChangeSelectedClient()
        {
            return true;
        }

        public void UpdateScope(Scope updatedScope)
        {
            scope = updatedScope;
        }
        public bool CanViewTransactionHistoryLink()
        {
            return true;
        }
        #endregion
    }
}