<%@ Page Language="C#" MasterPageFile="~/SafeTrader.Master" AutoEventWireup="true"
    CodeBehind="TransactionHistory.aspx.cs" Inherits="Cff.SaferTrader.Web.TransactionHistory"
    Title="Cashflow Funding Limited | Debtor Management | Transaction History"  ValidateRequest="false" %>

<%@ Import Namespace="Cff.SaferTrader.Web.App_GlobalResources" %>
<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/MonthRangePicker.ascx" TagPrefix="uc" TagName="MonthRangePicker" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/InPageMessageBox.ascx" TagPrefix="uc" TagName="InPageMessageBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div id="contentHeader">
        <asp:UpdatePanel ID="tabPanelUpdatePanel" runat="server" UpdateMode="Always">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="SearchButton" />
            </Triggers>
            <ContentTemplate>                                                
                <ul id="tabPaneSubNav">
                    <li style="height:20px;padding-top:1px;">
                        <span id="spanCBoxTranInvoice" onmouseover="highlightMouseOver();" onmouseout ="unhighlightMouseOut();">
                            <asp:CheckBox ID="ChkBoxTransactionInvoices" autopostback="true" runat="server" Text="Invoices Only" TextAlign="Right" oncheckedchanged="ChkBoxTransactionInvoices_CheckedChanged"/>
                        </span>
                    </li>
                    <li style="height:20px;" id="currentTransactionsLink" runat="server" />
                    <li style="height:20px;" id="transactionArchiveLink" runat="server" />
                    <li style="height:20px;">
                        <span id="spanSearchLink" onmouseover="highlightMouseOver();" onmouseout ="unhighlightMouseOut();">
                            <a id="SearchLink" runat="server">Search</a>
                        </span>
                    </li>
                </ul>
            </ContentTemplate>
        </asp:UpdatePanel>
        <h3>
           <a class="toggleDescription" onclick="toggleHelp(this);return false;">
              <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info" CssClass="informationImage" Height="16px" Width="16px"/>
           </a>
           Transaction History <%=targetName%>
        </h3>
    </div>
    <asp:UpdatePanel ID="pageDescriptionUpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc:PageDescription ID="transactionHistoryPageDescription" DescriptionTitle="" DescriptionContent="Transaction History" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="contentViewerContainer" cellspacing="0" cellpadding="0" style="width:85%">
           <tr>
            <td id="contentViewer" style="width:80%">
                <asp:UpdatePanel ID="ParameterSelectorUpdatePanel" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="MonthRangePicker" />
                    </Triggers>
                    <ContentTemplate>
                        <div>
                          <uc:MonthRangePicker ID="MonthRangePicker" runat="server" DefaultMonthsRange="11" />
                          <asp:Label CssClass="instruction" ID="UpdateLabel" Text="Please select the desired parameters and click the update button" runat="server"/>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="TransactionHistoryUpdatePanel" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="MonthRangePicker" />
                        <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                        <asp:PostBackTrigger ControlID="ExportButton" />
                    </Triggers>
                    <ContentTemplate>                                                
                        <asp:Panel ID="TransactionHistoryPanel" runat="server">
                            <asp:Label ID="RDInfoLabel" Text="" runat="server" />
                            <div>
                                <asp:PlaceHolder ClientIDMode="AutoID" ID="TransactionGridViewPH" runat="server" EnableViewState="true"></asp:PlaceHolder>
                            </div>
                            <div class="right parameterSelector" style="padding:7px;" onmouseover="highlightMouseOver();" onmouseout="unhighlightMouseOut();">
                                <asp:ImageButton ID="ExportButton" runat="server" OnClick="ExportButton_Click" AlternateText="Export"  ImageUrl="~/images/btn_export.png" />
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

    <script type="text/javascript">
        function highlightMouseOver() {
            document.body.style.cursor = 'pointer';
        }
        
        function unhighlightMouseOut() {
            document.body.style.cursor = 'default';
        }

        function enableRDInfo() {
           document.body.style.cursor = 'wait';
        }

        function disableRDInfo() {
            document.body.style.cursor = 'default';
        }

  </script>
</asp:Content>
