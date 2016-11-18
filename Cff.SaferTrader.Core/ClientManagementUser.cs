using System;

namespace Cff.SaferTrader.Core
{
    public class ClientManagementUser: ICffUser
    {
        private readonly Guid userId;
        private readonly string userName;
        private readonly int employeeId;
        private readonly string displayName;
        private readonly long clientId;
        private readonly long clientNumber;
        private readonly string clientName;

        public ClientManagementUser(Guid userId, string userName, int employeeId, string displayName, long clientId, long clientNumber, string clientName)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(userName,"userName");
            ArgumentChecker.ThrowIfNullOrEmpty(displayName, "displayName");
            ArgumentChecker.ThrowIfNullOrEmpty(clientName, "clientName");
            
            this.userId = userId;
            this.userName = userName;
            this.employeeId = employeeId;
            this.displayName = displayName;
            this.clientId = clientId;
            this.clientNumber = clientNumber;
            this.clientName = clientName;
        }

        public UserType UserType
        {
            get { return UserType.ClientManagementUser; }
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

        public long ClientId
        {
            get { return clientId; }
        }

        public string ClientName
        {
            get { return clientName; }
        }

        public long ClientNumber
        {
            get { return clientNumber; }
        }
    }
}
