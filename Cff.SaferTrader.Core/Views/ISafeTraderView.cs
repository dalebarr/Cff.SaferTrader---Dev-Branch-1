namespace Cff.SaferTrader.Core.Views
{
    public interface ISafeTraderView: IRedirectableView
    {
        void DisplayCustomerInformation(ClientAndCustomerInformation clientAndCustomerInformation, string facilityType, decimal limit, decimal available);
        void DisplayClientAndCustomerContacts(ClientAndCustomerContacts clientAndCustomerContacts);
        void ClearCffCustomerContactAndLeftInfomationPanel();
        void DisplayClientContactOnly(ClientContact clientContact);
        void SetFocusToCustomer();
        void SetFocusToForm();
        void DisplayClientNameAndId();
        void DisplayClientInformationAndAgeingBalances(ClientInformationAndAgeingBalances clientInformationAndAgeingBalances, decimal limit, decimal available);
        void DisplayCustomerNameAndClientNameInSearchBox();
        void ToggleEditNextCallDueDateButton(bool visible);
        void ToggleClientSearchControl(bool enable);
        void ToggleCustomerSearchControl(bool enable);
    }
}