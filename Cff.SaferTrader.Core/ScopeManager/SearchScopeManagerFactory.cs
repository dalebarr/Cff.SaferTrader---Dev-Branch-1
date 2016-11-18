namespace Cff.SaferTrader.Core.ScopeManager
{
    public class SearchScopeManagerFactory
    {
        
        public static ISearchScopeManager Create(Scope scope, CffPrincipal cffPrincipal)
        {
            ISearchScopeManager reportManager;
            UserType userType = cffPrincipal.CffUser.UserType;

            if (userType == UserType.EmployeeAdministratorUser)
            {
                reportManager = new AdministratorSearchScopeManager(scope);
            }
            else if (userType == UserType.EmployeeManagementUser)
            {
                reportManager = new ManagementSearchScopeManager(scope);
            }
            else if (userType == UserType.EmployeeStaffUser)
            {
                reportManager = new StaffSearchScopeManager(scope);
            }
            else if (userType == UserType.ClientStaffUser)
            {
                reportManager = new ClientStaffSearchScopeManager(scope);
            }
            else if (userType == UserType.ClientManagementUser)
            {
                reportManager = new ClientManagementSearchScopeManager(scope);
            }
            else if (userType == UserType.CustomerUser)
            {
                reportManager = new CustomerSearchScopeManager();
            }
            else
            {
                throw new CffUserTypeNotFoundException("Not usertype found exception");
            }

            return reportManager;
        }
        
    }
}