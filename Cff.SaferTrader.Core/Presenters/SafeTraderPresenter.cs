using System;
using Cff.SaferTrader.Core.Repositories;
using Cff.SaferTrader.Core.Services;
using Cff.SaferTrader.Core.Views;
using System.Collections.Generic;
using NPOI.HSSF.Record.Formula.Functions;

namespace Cff.SaferTrader.Core.Presenters
{
    public class SafeTraderPresenter
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ISecurityManager securityManager;
        private readonly IRedirectionService redirectionService;
        private readonly ICffUserService userService;
        private readonly CffPrincipal principal;
        private readonly ISafeTraderView view;
      
        public SafeTraderPresenter(ISafeTraderView view, ICustomerRepository customerRepository, 
                        ISecurityManager securityManager, IRedirectionService redirectionService,
                                ICffUserService userService, CffPrincipal principal)
        {
            ArgumentChecker.ThrowIfNull(view, "view");
            ArgumentChecker.ThrowIfNull(customerRepository, "customerRepository");

            this.view = view;
            this.customerRepository = customerRepository;
            this.securityManager = securityManager;
            this.redirectionService = redirectionService;
            this.userService = userService;
            this.principal = principal;
        }
        
        public void LoadClientAndCustomerContact(CffCustomer customer, ICffClient client)
        {
            if (customer == null) {
                view.ClearCffCustomerContactAndLeftInfomationPanel();
                view.DisplayCustomerNameAndClientNameInSearchBox();
                return;
            }

            ClientAndCustomerContacts clientAndCustomerContacts = customerRepository.GetCustomerClientContact(customer.Id);
            int clientID = (client == null) ? clientAndCustomerContacts.ClientContact.ClientId : client.Id;

            ClientAndCustomerInformation cffCustomer = customerRepository.GetMatchedCustomerInfo(customer.Id, clientID);
            ClientInformation clientInfo = customerRepository.GetMatchedClientInfo(clientID).ClientInformation;
              
            if (clientAndCustomerContacts != null)
            {
                    //MSarza [20150901]
                    //if (clientInfo.AdministeredBy.ToLower() == "no")
                    //{ clientAndCustomerContacts.isClientAdministeredByCFF = false; }
                    //else
                    //{
                    //    clientAndCustomerContacts.isClientAdministeredByCFF = true;
                    //}
                    if (clientInfo.IsClientDebtorAdmin)
                    { clientAndCustomerContacts.ClientIsDebtorAdmin = false; }
                    else
                    {
                        clientAndCustomerContacts.ClientIsDebtorAdmin = true;
                    }
                    if (clientInfo.IsCffDebtorAdminForClient)
                    { clientAndCustomerContacts.CffIsDebtorAdminForClient = false; }
                    else
                    {
                        clientAndCustomerContacts.CffIsDebtorAdminForClient = true;
                    }
            }

            view.SetFocusToForm();

            if (clientAndCustomerContacts == null)
                clientAndCustomerContacts = new ClientAndCustomerContacts(customerRepository.GetClientContactDetails(clientID), null);
           
            view.DisplayClientAndCustomerContacts(clientAndCustomerContacts);
            //view.DisplayCustomerInformation(cffCustomer);

            decimal limit = 0;
            decimal available = 0;
            if (clientInfo.FacilityType == "Current A/c")
            {
                limit =  GetCurrentACLimitFromDrMgt(client.Id, System.DateTime.Today);
                if (cffCustomer.CffCustomerInformation.CreditLimit <= limit)
                {
                    available = (cffCustomer.CffCustomerInformation.CreditLimit - cffCustomer.CffCustomerInformation.AgeingBalances.Balance);
                }
                else
                {
                    available = (limit - cffCustomer.CffCustomerInformation.AgeingBalances.Balance);
                }
            }

            view.DisplayCustomerInformation(cffCustomer, clientInfo.FacilityType, limit, available);
        }

        public void LoadClientDetailsOnly(int clientId, bool isDeselectingCustomer)
        {
            ClientContact clientContact = customerRepository.GetClientContactDetails(clientId);
            ClientInformationAndAgeingBalances clientInformation = customerRepository.GetMatchedClientInfo(clientId);

            //MSarza [20150901] : Data type changed from bool to small int for dbo.ClientFinancials.CffDebtorAdmin
            //if (clientInformation.ClientInformation.AdministeredBy.ToLower() == "yes")
            //if (clientInformation.ClientInformation.AdministeredByCff)
            //{
            //    if (SessionWrapper.Instance.Get != null)
            //        SessionWrapper.Instance.Get.IsClientAdminByCFF = true;
            //    else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
            //        SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsClientAdminByCFF = true;
            //}

            if (clientInformation.ClientInformation.IsCffDebtorAdminForClient)
            {
                if (SessionWrapper.Instance.Get != null)
                    SessionWrapper.Instance.Get.IsCffDebtorAdminForClient = true;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsCffDebtorAdminForClient = true;
            }

            if (clientInformation.ClientInformation.IsClientDebtorAdmin)
            {
                if (SessionWrapper.Instance.Get != null)
                    SessionWrapper.Instance.Get.IsClientDebtorAdmin = true;
                else if (!string.IsNullOrEmpty(QueryString.ViewIDValue))
                    SessionWrapper.Instance.GetSession(QueryString.ViewIDValue).IsClientDebtorAdmin = true;
            }


            if (clientContact != null)
            {
                view.DisplayClientContactOnly(clientContact);
            }
            
            decimal limit = 0;
            decimal available = 0;

            if (clientInformation.ClientInformation.FacilityType == "Current A/c")
            {

                //
                List<System.Decimal> result = GetCurrentACLimitFromDrMgtExt(clientId, System.DateTime.Today);
                limit = result[0];
                if (clientInformation.ClientInformation.CurrentAccountCustLimitSum < limit && clientInformation.ClientInformation.CurrentAccountCustLimitSum > 0)
                {
                    //available = (clientInformation.ClientInformation.CurrentAccountCustLimitSum - clientInformation.AgeingBalances.Balance);
                    available = clientInformation.ClientInformation.CurrentAccountCustLimitSum -
                                clientInformation.AgeingBalances.Balance -
                                (result[1] + result[2] + result[3] + result[4]);
                }
                else
                {
                    if (result.Count > 0)
                    {
                        available = result[0] - clientInformation.AgeingBalances.Balance - (result[1] + result[2] + result[3] + result[4]);
                    } else {

                        limit = GetCurrentACLimitFromDrMgt(clientId, System.DateTime.Today);
                        available = (limit - clientInformation.AgeingBalances.Balance);
                    }
                }
            }

            view.DisplayClientNameAndId();
            view.ClearCffCustomerContactAndLeftInfomationPanel();
            view.DisplayClientInformationAndAgeingBalances(clientInformation,limit,available);

            if (!isDeselectingCustomer)
            {
                view.SetFocusToCustomer();
            }
        }

        public string GetFacilityTypeByClientId(int clientId)
        {
            ClientInformationAndAgeingBalances clientInformation = customerRepository.GetMatchedClientInfo(clientId);
            return clientInformation.ClientInformation.FacilityType;
        }

        public System.Decimal GetCurrentACLimitFromDrMgt(int clientId, System.DateTime date)
        {
            return customerRepository.GetCurrentACLimit(clientId, principal.CffUser.EmployeeId, date);
        }

        public List<Decimal> GetCurrentACLimitFromDrMgtExt(int clientId, System.DateTime date)
        {
            return customerRepository.GetCurrentACLimitExt(clientId, principal.CffUser.EmployeeId, date);
        }

        public void UpdateCustomerNextCallDue(Date nextCallDue, int customerId, Date modifiedDate, int employeeId)
        {
            ArgumentChecker.ThrowIfNull(nextCallDue, "NextCallDue");
            ArgumentChecker.ThrowIfNull(modifiedDate, "modifiedDate");
            customerRepository.UpdateCustomerCallDue(nextCallDue, 0, customerId, modifiedDate, employeeId);
        }


        public bool InsertCustContactInfoDetailsForValidation(int clientId, CustomerContact custContactInfoDetails)
        {
            IContactsRepository contactRepo = RepositoryFactory.CreateContactsRepository();
            return (contactRepo.InsCustContactInfoDetailsForValidation(clientId, custContactInfoDetails));
        }


        public bool InsertCliContactInfoDetailsForValidation(int clientId, ClientContact cliInfoDetails)
        {
            IContactsRepository contactRepo = RepositoryFactory.CreateContactsRepository();
            return (contactRepo.InsertClientContactInfoDetailsForValidation(clientId, cliInfoDetails));
        }


        public bool UpdateClientContactDetails(int clientId, ClientContact clientContactDetails)
        {
            IContactsRepository contactRepo = RepositoryFactory.CreateContactsRepository();
            return (contactRepo.UpdateClientContactDetails(clientId, clientContactDetails));
        }

        public bool UpdateCustomerContactDetails(int clientId, CustomerContact custContactDetails)
        {
            IContactsRepository contactRepo = RepositoryFactory.CreateContactsRepository();
            return (contactRepo.UpdateCustomerContactDetails(clientId, custContactDetails));
        }

        public void InsUpdateCustomerInformation(string sAction, int clientID, int custID, System.Int16 stopCredit, 
                                                        decimal creditLimit, System.DateTime pNextCallDue, System.Int16 allowcalls, 
                                                                System.DateTime listdate, System.Int16 terms, string companyID, 
                                                                    decimal GSTvalue, System.DateTime dtModified, int iModifBy)
        {
            customerRepository.InsUpdateCustomerInformation(sAction, clientID, custID, stopCredit, 
                                                                creditLimit, pNextCallDue, allowcalls, listdate, terms, companyID, GSTvalue, dtModified, iModifBy);
        }


        public void LockDown(int? clientId, int? customerId)
        {
            if (!userService.ValidateClientCustomerSelection(clientId, customerId, principal))
            {
                redirectionService.SelectDefaultAssociationAndRedirectToDashboard(principal.CffUser);
            }

            view.ToggleEditNextCallDueDateButton(securityManager.CanEditNextCallDueDate());
            view.ToggleClientSearchControl(securityManager.CanChangeSelectedClient());
            view.ToggleCustomerSearchControl(securityManager.CanChangeSelectedCustomer());
        }

        public List<UserSpecialAccounts> VerifyIfSpecialAccountByUserId(IUserClientsRepository userClientRepository, int UserId)
        {
            return userClientRepository.GetSpecialAccountAccessByID(UserId);
        }

        public List<UserSpecialAccounts> LoadUserMultipleClients(IUserClientsRepository userClientRepository)
        {
            List<UserSpecialAccounts> accounts = new List<UserSpecialAccounts>();
            if (userClientRepository.VerifyIfSpecialAccountByID(principal.CffUser.EmployeeId) == 1)
            {
                accounts = userClientRepository.GetSpecialAccountAccessByID(principal.CffUser.EmployeeId);
            }
            return accounts;
        }
    }
}