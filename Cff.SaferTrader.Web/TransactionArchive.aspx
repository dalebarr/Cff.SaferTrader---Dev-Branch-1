<%@ Page Language="C#" MasterPageFile="~/SafeTrader.Master" AutoEventWireup="true"
    CodeBehind="TransactionArchive.aspx.cs" Inherits="Cff.SaferTrader.Web.TransactionArchive"
    Title="Cashflow Funding Limited | Debtor Management | Transaction Archive" ValidateRequest="false" %>

<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/MonthRangePicker.ascx" TagPrefix="uc" TagName="MonthRangePicker" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/InPageMessageBox.ascx" TagPrefix="uc" TagName="InPageMessageBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <script type="text/javascript">
        function highlightMouseOver() {
            document.body.style.cursor = 'pointer';
        }

        function unhighlightMouseOut() {
            document.body.style.cursor = 'default';
        }
   </script>

    <div id="contentHeader">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
          <ContentTemplate>
            <ul id="tabPaneSubNav">
                <li style="height:20px;padding-top:2px;">
                    <span onmouseover="highlightMouseOver();" onmouseout="unhighlightMouseOut();">
                        <asp:CheckBox ID="ChkBoxTransactionInvoices" autopostback="true" 
                                runat="server" Text="Invoices Only"  TextAlign="Right" oncheckedchanged="ChkBoxTransactionInvoices_CheckedChanged"/>
                    </span>
                </li>
                <li style="height:20px;"><a id="TransactionsLink" runat="server">Current</a></li>
                <li style="height:20px;"><a id="TransactionHistoryLink" runat="server">History</a></li>
                <li style="height:20px;"><a id="TransactionSearchLink" runat="server">Search</a></li>
            </ul>
         </ContentTemplate>
        </asp:UpdatePanel>
      
        <h3>
            <a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info" CssClass="informationImage" Height="16px" Width="16px"/>
            </a>
            Transaction Archive <% =targetName %>
        </h3>
    </div>
    <asp:UpdatePanel ID="pageDescriptionUpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc:PageDescription ID="transactionArchivePageDescription" DescriptionTitle="" DescriptionContent="TransactionArchive" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdateProgress ID="Progress" runat="server" AssociatedUpdatePanelID="ContactDetailsUpdatePanel" Visible="true">
        <ProgressTemplate>                    
            <div class="progressIndicator"></div>
        </ProgressTemplate>
    </asp:UpdateProgress>         

    <table id="contentViewerContainer" cellspacing="0" cellpadding="0" style="width: 75%">
        <tr>
            <td id="contentViewer">
                <asp:UpdatePanel ID="TransactionArchiveUpdatePanel" runat="server" UpdateMode="Always">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="MonthRangePicker" />
                        <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                        <asp:PostBackTrigger ControlID="ExportButton" />
                    </Triggers>
                    <ContentTemplate>                        
                        <asp:Panel ID="TransactionArchivePanel" runat="server">
                            <%--<div class="scroll" style="width:70%">--%>
                            <div>
                                <uc:MonthRangePicker ID="MonthRangePicker" runat="server" DefaultMonthsRange="3" />
                                <asp:PlaceHolder ID="TransactionGridViewPlaceHolder" runat="server" EnableViewState="true" />
                            </div>
                            <%--<div class="buttons" onmouseover="highlightMouseOver();" onmouseout="unhighlightMouseOut();" style="width:68.5%">--%>
                            <div class="buttons" onmouseover="highlightMouseOver();" onmouseout="unhighlightMouseOut();">
                                <asp:ImageButton ID="ExportButton" runat="server" OnClick="ExportButton_Click" AlternateText="Export"
                                    ImageUrl="~/images/btn_export.png" />
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdateProgress ID="TransProgress" runat="server" AssociatedUpdatePanelID="TransactionArchiveUpdatePanel">
                    <ProgressTemplate>
                        <div class="progressIndicator"></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

            </td>
            <%--<td id="rhToggle">
                <div id="rhToggleIcon">
                </div>
            </td>--%>
        </tr>
    </table>
</asp:Content>
