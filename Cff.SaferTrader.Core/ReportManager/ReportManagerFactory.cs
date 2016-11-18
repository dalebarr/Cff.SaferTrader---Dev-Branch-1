namespace Cff.SaferTrader.Core.ReportManager
{
    public static class ReportManagerFactory
    {
        public static IReportManager Create(Scope scope, CffPrincipal cffPrincipal)
        {
            IReportManager reportManager;
            UserType userType = cffPrincipal.CffUser.UserType;

            if (userType == UserType.EmployeeAdministratorUser)
            {
                reportManager = new AdministratorReportManager();
            }
            else if (userType == UserType.EmployeeManagementUser)
            {
                reportManager = new ManagementReportManager();
            }
            else if (userType == UserType.EmployeeStaffUser)
            {
                reportManager = new StaffReportManager(scope);
            }
            else if (userType == UserType.ClientStaffUser)
            {
                reportManager = new ClientStaffReportManager();
            }
            else if (userType == UserType.ClientManagementUser)
            {
                reportManager = new ClientManagementReportManager();
            }
            else if (userType == UserType.CustomerUser)
            {
                reportManager = new CustomerReportManager();
            }
            else
            {
                throw new CffUserTypeNotFoundException("Not usertype found exception");
            }

            return reportManager;
        }
    }
}