<%@ Page Language="C#" MasterPageFile="~/SafeTrader.Master" AutoEventWireup="true"
    CodeBehind="Transactions.aspx.cs" Inherits="Cff.SaferTrader.Web.Transactions"
    Title="Cashflow Funding Limited | Debtor Management | Current Transactions" ValidateRequest="false" %>

<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/InPageMessageBox.ascx" TagPrefix="uc" TagName="InPageMessageBox" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div id="contentHeader">
        <asp:UpdatePanel runat="server" UpdateMode="Always">
          <ContentTemplate>
             <ul id="tabPaneSubNav">
                <li style="height:20px;padding-top:2px;"><asp:CheckBox ID="ChkBoxTransactionInvoices"  autopostback="true" runat="server" 
                        Text="Invoices Only" TextAlign="Right"
                        oncheckedchanged="ChkBoxTransactionInvoices_CheckedChanged"/></li>
                <li style="height:20px;"><a id="TransactionCurrentLink" runat="server"><span>Current</span></a></li>
                <li style="height:20px;"><a id="TransactionArchiveLink" runat="server">Archive</a></li>
                <li style="height:20px;"><a id="TransactionHistoryLink" runat="server">History</a></li>
                <li style="height:20px;"><a id="TransactionSearchLink" runat="server" >Search</a></li>
            </ul>
            </ContentTemplate>
        </asp:UpdatePanel>
        <h3>
            <a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info"
                CssClass="informationImage" Height="16px" Width="16px"/>
            </a>
            Current Transactions <% =targetName %>
         </h3>
         <asp:Literal ID="testLiteral" runat="server" />
    </div>
    <asp:UpdatePanel ID="pageDescriptionUpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
        <uc:PageDescription ID="transactionsPageDescription" DescriptionTitle="" DescriptionContent="Transactions" runat="server" />     
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="contentViewerContainer" cellspacing="0" cellpadding="0" style="width:75%">
        <tr>
            <td id="contentViewer">                
                <asp:UpdatePanel ID="GridUpdatePanel" runat="server" UpdateMode="Always">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                        <asp:PostBackTrigger ControlID="ExportButton" />
                    </Triggers>
                    <ContentTemplate>                                                
                        <asp:Panel ID="TransactionsPanel" runat="server" >                                       
                            <div>
                                <asp:PlaceHolder ClientIDMode="AutoID" ID="TransactionGridViewPanel" runat="server" EnableViewState="true"/>
                            </div>
                            <div class="buttons">
                                <asp:ImageButton ID="ExportButton" runat="server" OnClick="ExportButton_Click" AlternateText="Export"
                                    ImageUrl="~/images/btn_export.png" />
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <%--<td id="rhToggle">
                <div id="rhToggleIcon">
                </div>
            </td>--%>
        </tr>
    </table>    
</asp:Content>
