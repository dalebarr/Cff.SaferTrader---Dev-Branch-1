using System;

namespace Cff.SaferTrader.Core
{
    public interface ICffUser
    {
        string DisplayName { get; }
        int EmployeeId{ get; }
        string UserName { get; }
        Guid UserId { get; }
        UserType UserType { get; }
        long ClientId { get; }
    }

    public interface ICffLoginAccount
    {
        string Username { get; }
        string Password { get; }
    }

    public interface IUserSpecialAccounts
    {
        Guid ID { get; }
        string Name { get; }
        Boolean IsClient { get; }
    }
}
