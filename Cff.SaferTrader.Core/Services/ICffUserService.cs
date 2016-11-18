using System;

namespace Cff.SaferTrader.Core.Services
{
    public interface ICffUserService
    {
        ICffUser LoadCffUser(Guid userId);
        bool ValidateClientCustomerSelection(int? clientId, int? customerId, CffPrincipal principal);
    }
}
