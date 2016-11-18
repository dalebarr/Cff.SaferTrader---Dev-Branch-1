using System;
using System.Collections.Generic;

namespace Cff.SaferTrader.Core
{
    public class UserType
    {
        private static readonly UserType employeeStaffUser = new UserType(1, "Staff");
        private static readonly UserType clientManagementUser = new UserType(2, "Client - Management");
        private static readonly UserType customerUser = new UserType(3, "Customer");
        private static readonly UserType employeeAdministratorUser = new UserType(4, "Administrator");
        private static readonly UserType employeeManagementUser = new UserType(5, "Management");
        private static readonly UserType clientStaffUser = new UserType(6, "Client - Staff");
        private readonly int id;
        private readonly string name;
        private static readonly Dictionary<int, UserType> userTypes = InitialiseUserTypes();

        private UserType(int id, string name)
        {
            ArgumentChecker.ThrowIfLessThanZero(id, "id");
            ArgumentChecker.ThrowIfNullOrEmpty(name, "name");

            this.id = id;
            this.name = name;
        }
        
        private static Dictionary<int, UserType> InitialiseUserTypes()
        {
            Dictionary<int, UserType> knownUserTypes = new Dictionary<int, UserType>
                                                      {
                                                          {employeeStaffUser.Id, employeeStaffUser},
                                                          {clientManagementUser.Id, clientManagementUser},
                                                          {customerUser.Id, customerUser},
                                                          {employeeAdministratorUser.Id, employeeAdministratorUser},
                                                          {employeeManagementUser.Id, employeeManagementUser},
                                                          {clientStaffUser.Id, clientStaffUser},
                                                      };
            return knownUserTypes;
        }

        public string Name{ get { return name; } }
        public int Id{ get { return id; } }
        public static UserType EmployeeStaffUser { get { return employeeStaffUser; } }
        public static UserType ClientManagementUser { get { return clientManagementUser; } }
        public static UserType CustomerUser { get { return customerUser; } }
        public static UserType EmployeeAdministratorUser { get { return employeeAdministratorUser; } }
        public static UserType EmployeeManagementUser { get { return employeeManagementUser; } }
        public static UserType ClientStaffUser { get { return clientStaffUser; } }

        public static UserType Parse(int id)
        {
            UserType userType;
            if (userTypes.ContainsKey(id))
            {
                userType = userTypes[id];
            }
            else
            {
                throw new CffUserTypeNotFoundException("No UserType found");
            }
            return userType;
        }
    }
}
