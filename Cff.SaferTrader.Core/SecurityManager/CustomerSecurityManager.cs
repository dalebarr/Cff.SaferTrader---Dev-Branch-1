using System;

namespace Cff.SaferTrader.Core.SecurityManager
{
    public class CustomerSecurityManager: ISecurityManager
    {
        private Scope scope = Scope.CustomerScope;

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
            return false;
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
            return false;
        }

        public bool CanViewCustomerNotesAdder()
        {
            return true;
        }

        public bool CanViewCustomerNotes()
        {
            return true;
        }

        public bool CanCreatePermanentNotes()
        {
            return false;
        }

        public bool CanCreateClientNotes()
        {
            return false;
        }

        public bool CanCreateCustomerNotes()
        {
            return true;
        }

        public bool CanEditNextCallDueDate()
        {
            return false;
        }
        public bool CanViewScopeSelectionInTransactionSearch()
        {
         
            return false;  
        }
        public bool CanViewAllClientsScopeSelectionInTransactionSearch()
        {
            return false;
        }

        public bool CanChangeSelectedCustomer()
        {
            return false;
        }

        public bool CanChangeSelectedClient()
        {
            return false;
        }

        public void UpdateScope(Scope updatedScope)
        {
            if (updatedScope == Scope.AllClientsScope)
            {
                throw new InvalidOperationException("Customer is not able to change to All Clients");
            }
            if (updatedScope == Scope.ClientScope)
            {
                throw new InvalidOperationException("Customer is not able to change Client");
            }
            scope = updatedScope;
        }
        public bool CanViewTransactionHistoryLink()
        {
            return true;
        }
    }
}