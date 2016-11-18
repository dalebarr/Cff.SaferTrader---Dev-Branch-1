using System.Security.Principal;

namespace Cff.SaferTrader.Core
{
    public class CffPrincipal: IPrincipal
    {
        private readonly IPrincipal rolePrincipal;
        private readonly ICffUser cffUser;

        public CffPrincipal(IPrincipal rolePrincipal, ICffUser cffUser)
        {
            ArgumentChecker.ThrowIfNull(rolePrincipal, "rolePrincipal");
            ArgumentChecker.ThrowIfNull(cffUser, "cffUser");
            
            this.rolePrincipal = rolePrincipal;
            this.cffUser = cffUser;
        }

        public IIdentity Identity
        {
            get { return rolePrincipal.Identity; }
        }

        public bool IsInRole(string role)
        {
            return rolePrincipal.IsInRole(role);
        }

        /// <summary>
        /// Remove this in Phase 3
        /// </summary>
        public bool IsInAdministratorRole
        { //ref: DM112
            //get { return IsInRole("Administrator") || IsInRole("Management") || IsInRole("Staff"); }
            get { return IsInRole("Administrator"); }
        }

        public bool IsInManagementRole
        { //ref: DM112
            get { return IsInRole("Management") || IsInRole("Client - Management");  }
        }

        public bool IsInCustomerRole
        {
            get { return IsInRole("Customer"); }
        }

        public bool IsInClientRole
        { //ref: DM112
            //get { return IsInRole("Client - Staff") || IsInRole("Staff"); }
            get { return IsInRole("Client - Management") || IsInRole("Client - Staff"); } 
        }

        public ICffUser CffUser
        {
            get { return cffUser; }
        }
    }
}