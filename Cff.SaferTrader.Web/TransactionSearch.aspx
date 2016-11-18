<%@ Page Language="C#" MasterPageFile="~/SafeTrader.Master" AutoEventWireup="true"
    CodeBehind="TransactionSearch.aspx.cs" Inherits="Cff.SaferTrader.Web.TransactionSearch"
    Title="Cashflow Funding Limited | Debtor Management | Transaction Search" ValidateRequest="false" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<%@ Register Src="~/UserControls/CalendarDateRangePicker.ascx" TagPrefix="uc" TagName="DateRangePicker" %>
<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div id="contentHeader">
        <asp:UpdatePanel ID="tabPanelUpdatePanel" runat="server" UpdateMode="Always">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="SearchButton" />
            </Triggers>
            <ContentTemplate>   
                <ul id="tabPaneSubNav">
                    <li style="height:20px;" id="currentTransactionsLink" runat="server"></li>
                    <li style="height:20px;" id="transactionArchiveLink" runat="server"></li>
                    <li style="height:20px;" id="transactionHistoryLink" enableviewstate="false" runat="server"></li>
                </ul>
                <h3>
                    <a class="toggleDescription" onclick="toggleHelp(this);return false;">
                        <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info"
                        CssClass="informationImage" Height="16px" Width="16px"/>
                    </a>
                    Transaction Search <% =targetName %>
                </h3>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <uc:PageDescription ID="transactionsPageDescription" DescriptionTitle="" DescriptionContent="Transaction Search" runat="server" />
    <div class="scrollReport">
        <table id="contentViewerContainer" cellspacing="0" cellpadding="0">
            <tr>
                <td id="contentViewer">                    
                    <asp:Panel ID="TransactionSearchPanel" runat="server" DefaultButton="TransactionSearchImageButton">
                        <asp:UpdatePanel ID="TransactionSearchGridUpdatePanel" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="TransactionSearchImageButton" />
                                <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                            </Triggers>
                            <ContentTemplate>
                                <div id="transactionSearchPanel">
                                    <table>
                                        <tr>
                                            <td>
                                                <label for="<%=InvoiceNumberTextBox.ClientID %>">
                                                    Invoice #:</label>
                                                <uc:SecureTextBox ID="InvoiceNumberTextBox" runat="server"></uc:SecureTextBox>
                                            </td>
                                            <td>
                                                <label for="<%=TransactionTypeDropDownList.ClientID %>">
                                                    Transaction type:</label>
                                                <asp:DropDownList ID="TransactionTypeDropDownList" runat="server" />
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <label ID="searchScopeLabel" runat="server">Search scope:</label>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="SearchScopeDropDownList" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <uc:DateRangePicker ID="TransactionDateRangePicker" TriggerButtonId="TransactionSearchImageButton" ClientIDMode="AutoID" runat="server" DefaultMonthsRange="12" />
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="TransactionSearchImageButton" AlternateText="Search" runat="server"
                                                    OnClick="TransactionSearchButton_Click" ImageUrl="~/images/btn_search_blue.png"
                                                    CssClass="searchButton" ValidationGroup="InvoiceNumber" OnClientClick="startSearchButtonAnimate()" />
                                            </td>
                                            <td>
                                                <asp:Panel ID="SearchStatusPanel" runat="server" Visible="false">
                                                    <asp:Label ID="lblSearchStatusPanel" runat="server" Text="Searching Please Wait..."></asp:Label>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <div>
                                        <asp:RequiredFieldValidator ID="InvoiceNumberRequiredFieldValidator" ControlToValidate="InvoiceNumberTextBox"
                                            runat="server" ErrorMessage="Please enter at least 3 numbers for invoice number."
                                            Display="Dynamic" ValidationGroup="InvoiceNumber"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ControlToValidate="InvoiceNumberTextBox" ID="InvoiceNumberRegularExpressionValidator"
                                            runat="server" ErrorMessage="Please enter at least 3 numbers for invoice number."
                                            ValidationExpression="[\w\W]{3,}" Display="Dynamic" ValidationGroup="InvoiceNumber"></asp:RegularExpressionValidator>
                                        <span class="error" ></span>
                                        <span class="searchError" ></span>
                                    </div>
                                </div>
                                <asp:Label CssClass="instruction" ID="SearchLabel" Text="Please select the desired parameters and click the search button" runat="server"/>
                                <div class="scroll">
                                    <uc:CffGenGridView ID="transactionSearchGridView" runat="server" CssClass="cffGGV" 
                                         KeyFieldName="Id" AllowColumnResizing="true" AllowCustomPaging="true" AllowPaging="true" AllowSorting="true" AlternatingRowStyle-BackColor="Honeydew"
                                                    AutoGenerateColumns="false"  PageSize="10" Visible="true" EnableViewState="true"  Caption="" HeaderStyle-CssClass="cffGGVHeader">
                                             <Columns>
                                                    <uc:CffCommandField ButtonType="Button" SelectText="..."></uc:CffCommandField>
                                             </Columns>
                                    </uc:CffGenGridView>
                                    
                                     <uc:CffGenGridView ID="creditSearchGridView" runat="server" CssClass="cffGGV" 
                                         KeyFieldName="Id" AllowColumnResizing="true" AllowCustomPaging="true" AllowPaging="true" AllowSorting="true" AlternatingRowStyle-BackColor="Honeydew"
                                                    AutoGenerateColumns="false"  PageSize="10" Visible="true" EnableViewState="true" Caption="" HeaderStyle-CssClass="cffGGVHeader">
                                             <Columns>
                                                    <uc:CffCommandField ButtonType="Button" SelectText="..."></uc:CffCommandField>
                                             </Columns>
                                    </uc:CffGenGridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
                <td id="rhToggle">
                    <div id="rhToggleIcon">
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="buttons">
        <asp:ImageButton ID="ExportButton" runat="server" OnClick="ExportButton_Click" AlternateText="Export" ImageUrl="~/images/btn_export.png"  />
    </div>  
</asp:Content>
