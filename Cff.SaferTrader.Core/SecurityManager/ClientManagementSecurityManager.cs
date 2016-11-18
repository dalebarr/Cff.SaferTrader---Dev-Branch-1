using System;

namespace Cff.SaferTrader.Core.SecurityManager
{
    public class ClientSecurityManager : ISecurityManager
    {
        private Scope scope;

        public ClientSecurityManager(Scope scope)
        {
            ArgumentChecker.ThrowIfNull(scope, "scope");
            this.scope = scope;
        }

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
            return false;
        }

        public bool CanViewClientPermanentNotes()
        {
            return false;
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
            return false;
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
            return true;
        }
        public bool CanViewAllClientsScopeSelectionInTransactionSearch()
        {
            return false;
        }

        public bool CanChangeSelectedCustomer()
        {
            return true;
        }

        public bool CanChangeSelectedClient()
        {
            return false;
        }

        public void UpdateScope(Scope updatedScope)
        {
            if (updatedScope == Scope.AllClientsScope)
            {
                throw new InvalidOperationException("Client is not able to change to All Clients");
            }
            scope = updatedScope;
        }

        public bool CanViewTransactionHistoryLink()
        {
            return true;
        }
    }
}