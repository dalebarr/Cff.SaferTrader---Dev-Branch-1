using System;

namespace Cff.SaferTrader.Core
{
    public class CustomerUser : ICffUser
    {
        private readonly Guid userId;
        private readonly string userName;
        private readonly int employeeId;
        private readonly string displayName;
        private readonly long customerId;
        private readonly long customerNumber;
        private readonly string customerName;
        private readonly long clientId;

        public CustomerUser(Guid userId, string userName, int employeeId, string displayName, long customerId, long customerNumber, string customerName, long clientId)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(userName, "userName");
            ArgumentChecker.ThrowIfNullOrEmpty(displayName, "displayName");
            ArgumentChecker.ThrowIfNullOrEmpty(customerName, "customerName");
            
            this.userId = userId;
            this.userName = userName;
            this.employeeId = employeeId;
            this.displayName = displayName;
            this.customerId = customerId;
            this.customerNumber = customerNumber;
            this.customerName = customerName;
            this.clientId = clientId;
        }

        public UserType UserType
        {
            get { return UserType.CustomerUser; }
        }

        public string DisplayName
        {
            get { return displayName;}
        }

        public int EmployeeId
        {
            get { return employeeId;}
        }

        public string UserName
        {
            get { return userName; }
        }

        public Guid UserId
        {
            get { return userId; }
        }

        public string CustomerName
        {
            get { return customerName; }
        }

        public long CustomerNumber
        {
            get { return customerNumber; }
        }

        public long CustomerId
        {
            get { return customerId; }
        }

        public long ClientId
        {
            get { return clientId; }
        }
    }
}