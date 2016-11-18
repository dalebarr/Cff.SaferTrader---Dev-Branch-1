namespace Cff.SaferTrader.Core.SecurityManager
{
    public static class SecurityManagerFactory
    {
        public static ISecurityManager Create(CffPrincipal cffPrincipal, Scope scope)
        {
            ISecurityManager securityManager;
            UserType userType = cffPrincipal.CffUser.UserType;

            if (userType == UserType.EmployeeAdministratorUser)
            {
                securityManager = new AdministratorSecurityManager(scope);
            }
            else if (userType == UserType.EmployeeManagementUser)
            {
                securityManager = new ManagementSecurityManager(scope);
            }
            else if (userType == UserType.EmployeeStaffUser)
            {
                securityManager = new StaffSecurityManager(scope);
            }
            else if (userType == UserType.ClientStaffUser)
            {
                securityManager = new ClientStaffSecurityManager(scope);
            }
            else if (userType == UserType.ClientManagementUser)
            {
                securityManager = new ClientSecurityManager(scope);
            }
            else if (userType == UserType.CustomerUser)
            {
                securityManager = new CustomerSecurityManager();
            }
            else
            {
                throw new CffUserTypeNotFoundException("Not usertype found exception");
            }

            return securityManager;
        }
    }
}