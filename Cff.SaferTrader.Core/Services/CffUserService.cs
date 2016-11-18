using System;
using Cff.SaferTrader.Core.Repositories;

namespace Cff.SaferTrader.Core.Services
{
    public class CffUserService: ICffUserService
    {
        private readonly ICffUserRepository userRepository;
        private readonly IClientRepository clientRepository;
        private readonly ICustomerRepository customerRepository;

        public CffUserService(ICffUserRepository userRepository, IClientRepository clientRepository, ICustomerRepository customerRepository)
        {
            ArgumentChecker.ThrowIfNull(userRepository, "userRepository");
            ArgumentChecker.ThrowIfNull(clientRepository, "clientRepository");
            ArgumentChecker.ThrowIfNull(customerRepository, "customerRepository");

            this.userRepository = userRepository;
            this.clientRepository = clientRepository;
            this.customerRepository = customerRepository;
        }

        public static CffUserService Create()
         {
             return new CffUserService(RepositoryFactory.CreateCffUserRepository(), 
                                       RepositoryFactory.CreateClientRepository(), 
                                       RepositoryFactory.CreateCustomerRepository());
         }
        
        public ICffUser LoadCffUser(Guid userId)
        {
            ArgumentChecker.ThrowIfGuidEmpty(userId, "userId");
            return userRepository.LoadCffUser(userId);
        }

        public bool ValidateClientCustomerSelection(int? clientId, int? customerId, CffPrincipal principal)
        {
            bool bRet = false;
            if (clientId == principal.CffUser.ClientId || principal.CffUser.ClientId == -1)
            {
                if (!clientId.HasValue)
                {
                    // Use user's default ClientId if none supplied
                    clientId = (int)principal.CffUser.ClientId;
                }

                if (customerId.HasValue && !customerRepository.CheckCustomerBelongsToClient(clientId.Value, customerId.Value))
                {
                    // Customer and Client should be associated with each other
                    return false;
                }

                CustomerUser customerUser = principal.CffUser as CustomerUser;
                if (customerUser != null)
                {
                    // Customer can only select itself
                    return customerId.HasValue && customerUser.CustomerId == customerId.Value && clientId.Value == customerUser.ClientId;
                }

                if (principal.IsInClientRole)
                {
                    // Client can only select itself
                    return clientId == principal.CffUser.ClientId;
                }
                bRet = true;
            }
            else
            {
                // if user is allowed for this client then proceed - new implementation to allow multiple login
                if (clientId.HasValue)
                {
                    if (customerRepository.CheckClientBelongToUser(clientId.Value, principal.CffUser.UserId) == true)
                    {
                        bRet = true;
                    }
                //}
                }
                //bRet = false;
            }
            return bRet;
        }

        public ICffClient LoadCffClientAssociatedWith(ICffUser user)
        {
            ArgumentChecker.ThrowIfNull(user, "user");

            ICffClient client;

            if (user.UserType == UserType.EmployeeAdministratorUser || 
                    user.UserType == UserType.EmployeeManagementUser || 
                        user.UserType == UserType.EmployeeStaffUser)
            {
                client = AllClients.Create();
            }
            else if (user.UserType == UserType.ClientStaffUser)
            {
                client = clientRepository.GetCffClientByClientId((int)((ClientStaffUser)user).ClientId);
            }
             else if (user.UserType == UserType.ClientManagementUser)
             {
                 client = clientRepository.GetCffClientByClientId((int)((ClientManagementUser)user).ClientId);
             }
            else if (user.UserType == UserType.CustomerUser)
            {
                client = clientRepository.GetCffClientByCustomerId((int)((CustomerUser)user).CustomerId);
            }
           
            else
            {
                throw new CffUserNotFoundException("No cff user found");
            }
            return client;
        }

        public CffCustomer LoadCffCustomerAssociatedWith(ICffUser user)
        {
            ArgumentChecker.ThrowIfNull(user, "user");

            CffCustomer customer = null;
            CustomerUser customerUser = user as CustomerUser;
            if (customerUser != null)
            {
                customer = customerRepository.GetCffCustomerByCustomerId((int)customerUser.CustomerId);
            }
            return customer;
        }
    }
}
