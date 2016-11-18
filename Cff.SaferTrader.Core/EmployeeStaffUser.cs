using System;

namespace Cff.SaferTrader.Core
{
    public class EmployeeStaffUser: ICffUser
    {
        private readonly Guid userId;
        private readonly string userName;
        private readonly int employeeId;
        private readonly string displayName;
        private readonly long clientId;

        public EmployeeStaffUser(Guid userId, string userName, int employeeId, string displayName, long clientId)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(userName, "userName");
            ArgumentChecker.ThrowIfNullOrEmpty(displayName, "displayName");

            this.userId = userId;
            this.clientId = clientId;
            this.userName = userName;
            this.employeeId = employeeId;
            this.displayName = displayName;
        }

        public UserType UserType
        {
            get { return UserType.EmployeeStaffUser; }
        }

        public long ClientId
        {
            get { return clientId; }
        }

        public string DisplayName
        {
            get { return displayName; }
        }

        public int EmployeeId
        {
            get { return employeeId; }
        }

        public string UserName
        {
            get { return userName; }
        }

        public Guid UserId
        {
            get { return userId; }
        }
    }
}
