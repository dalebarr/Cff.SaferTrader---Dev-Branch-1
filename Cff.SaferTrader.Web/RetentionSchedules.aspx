<%@ Page Language="C#" MasterPageFile="~/SafeTrader.Master" AutoEventWireup="true"
    CodeBehind="RetentionSchedules.aspx.cs" Inherits="Cff.SaferTrader.Web.RetentionSchedules"
    Title="Cashflow Funding Limited | Debtor Management | Retention Schedules" %>

<%@ MasterType VirtualPath="~/SafeTrader.Master" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<%@ Register Src="~/UserControls/DatePicker.ascx" TagPrefix="uc" TagName="DatePicker" %>
<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/MonthRangePicker.ascx" TagPrefix="uc" TagName="MonthRangePicker" %>

<%@ Register Src="~/UserControls/ReleaseTabs/RetentionAdjustmentsTab.ascx" TagPrefix="uc" TagName="ChargesTab" %>
<%@ Register Src="~/UserControls/ReleaseTabs/RetentionDetailsTab.ascx" TagPrefix="uc" TagName="DetailsTab" %>
<%@ Register Src="~/UserControls/ReleaseTabs/CreditsClaimed.ascx" TagPrefix="uc" TagName="CreditsClaimedTab" %>
<%@ Register Src="~/UserControls/ReleaseTabs/RetentionRepurchasesClaimedTab.ascx"  TagPrefix="uc" TagName="RetentionRepurchasesClaimedTab" %>
<%@ Register Src="~/UserControls/ReleaseTabs/OverdueChargesTab.ascx" TagPrefix="uc" TagName="OverdueChargesTab" %>
<%@ Register Src="~/UserControls/ReleaseTabs/LikelyRepurchasesTab.ascx" TagPrefix="uc" TagName="LikelyRepurchasesTab" %>
<%@ Register Src="~/UserControls/ReleaseTabs/AccountTransactionsTab.ascx" TagPrefix="uc" TagName="RetentionAccountsTab" %>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div id="contentHeader">
        <ul id="tabPaneSubNav">
            <li><a id="InvoiceBatchesLink" runat="server">Invoice Batches</a></li>
            <li class="current"><span><asp:Literal ID="RetnSchedulesLiteral1" runat="server" Text="Monthly Schedules"></asp:Literal></span></li>
        </ul>
        <h3>
            <a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info" CssClass="informationImage"
                 Height="16px" Width="16px"/>
            </a>
            <asp:Literal ID="RetnSchedulesLiteral2" runat="server" Text="Retention Schedules"></asp:Literal>
        </h3>
        <input id="customerPanelHidden" type="hidden"  value="customerPanelHidden"  />        
        <input id="Hidden1" type="hidden"  value="customerPanelHidden"  />
    </div>
    <uc:PageDescription ID="PageDescription" DescriptionTitle="" DescriptionContent="Retention Schedules" runat="server" />
    <table id="contentViewerContainer" cellspacing="0" cellpadding="0">
        <tbody>
           <tr>
            <td id="contentViewer" class="invoiceBatches">
                <div>
                    <asp:UpdatePanel ID="GridUpdatePanel" runat="server" UpdateMode="Conditional">
                        <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="MonthRangePicker" />
                                <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                                <asp:AsyncPostBackTrigger ControlID="PreviousRetentionButton" />
                                <asp:AsyncPostBackTrigger ControlID="NextRetentionButton" />
                        </Triggers>
                        <ContentTemplate>                        
                                <uc:MonthRangePicker ID="MonthRangePicker" runat="server" DefaultMonthsRange="36" />
                                <div class="parameterSelector" id="allClientsDatePickerDiv" runat="server">
                                    <table>
                                        <uc:DatePicker ID="datePicker" runat="server" Visible="false" />
                                    </table>                            
                                </div>
                                <asp:Literal ID="Literal" runat="server" />
                                <div style="background-color:#01844f;padding-left:5px;padding-top:5px;padding-bottom: 5px;color:white;font-size: 15px;">
                                    <asp:Literal ID="RetnSchedulesLiteral" runat="server"></asp:Literal>
                                    <a id="retentionGridViewToggle" onclick="toggleRetentionGridView('retentionGridViewToggle', 'retentionGridView');return false;" style="float: left;padding-top:0;margin-top:1px;">
                                        <img src="./images/collapse.png" alt="collapse" style="border:none;" />&nbsp;
                                    </a>
                                    <%--<h4>                                    
                                       <asp:Literal ID="RetnSchedulesLiteral" runat="server" Text="Retentions"></asp:Literal>
                                       <a id="retentionGridViewToggle" onclick="toggleRetentionGridView('retentionGridViewToggle', 'retentionGridView');return false;">
                                           <img src="../images/collapse.png" alt="collapse" style="border:none;" />
                                       </a>
                                    </h4>--%>
                                </div>
                                <div class="scroll" id="retentionGridView" style="width:100%;">                            
                                    <asp:PlaceHolder ID="gridViewPlaceholder" runat="server" />
                                </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="DetailUpdatePanel" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="MonthRangePicker" />
                        <asp:AsyncPostBackTrigger ControlID="datePicker" />
                        <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                        <asp:AsyncPostBackTrigger ControlID="PreviousRetentionButton" />
                        <asp:AsyncPostBackTrigger ControlID="NextRetentionButton" />
                    </Triggers>
                    <ContentTemplate>
                        <div id="DetailView" runat="server" class="blockableDiv">
                            <div class="blockDiv">
                            </div>
                            <div id="RetentionHeader">
                                <uc:PrintButton ID="printButton" runat="server" OnClick="PrintButton_Click" CssClass="right" /> 
                                <div id="RetentionButtons">
                                   <span onmouseover="document.body.style.cursor='pointer';" onmouseout="document.body.style.cursor='default';">
                                       <asp:ImageButton ID="PreviousRetentionButton" runat="server" OnClick="PreviousRetentionButton_Click" ImageUrl="~/images/batch_up.png" AlternateText="Previous" />
                                   </span> 
                                   <span onmouseover="document.body.style.cursor='pointer';" onmouseout="document.body.style.cursor='default';">
                                       <asp:ImageButton ID="NextRetentionButton" runat="server" OnClick="NextRetentionButton_Click" ImageUrl="~/images/batch_down.png" AlternateText="Next" />
                                   </span> 
                                </div>                                
                                <h4>
                                    <asp:Literal ID="RetnHeaderLiteral" runat="server" />                                    
                                    &nbsp;of&nbsp;<asp:Literal ID="clientNameLiteral" runat="server" />&nbsp;for Month Ending
                                    <asp:Literal ID="EndOfMonthLiteral" runat="server" />
                                </h4>                                
                            </div>
                             <div id="RetentionTabs">
                                <asp:Menu ID="TabMenu" runat="server" OnMenuItemClick="TabMenu_MenuItemClick" Orientation="Horizontal"
                                    StaticSelectedStyle-CssClass="tabSelected">
                                    <Items>
                                        <asp:MenuItem Text="Schedule" Value="0" />
                                        <asp:MenuItem Text="Adjustments" Value="1" />
                                        <asp:MenuItem Text="Credits Claimed" Value="2" />
                                        <asp:MenuItem Text="Reclassified & Claimed" Value="3" />
                                        <asp:MenuItem Text="Interest & Charges" Value="4" />
                                        <asp:MenuItem Text="Likely to be Reclassified" Value="5" />
                                        <asp:MenuItem Text="Account Transactions" Value="6" />
                                    </Items>
                                </asp:Menu>
                                <asp:MultiView ID="TabViews" runat="server" ActiveViewIndex="0" OnActiveViewChanged="TabViews_ActiveViewChanged">
                                   <asp:View ID="DetailsView" runat="server">
                                        <uc:DetailsTab ID="RetentionDetailsTab" runat="server" />
                                   </asp:View>
                                   <asp:View ID="ChargesView" runat="server">
                                        <uc:ChargesTab ID="RetentionAdjustmentsTab" runat="server" />
                                   </asp:View>
                                   <asp:View ID="CreditsClaimedView" runat="server">
                                        <uc:CreditsClaimedTab ID="CreditsClaimedTab" runat="server" />
                                   </asp:View>
                                   <asp:View ID="RepurchasesView" runat="server">
                                        <uc:RetentionRepurchasesClaimedTab ID="RetentionRepurchasesClaimedTab" runat="server" />
                                   </asp:View>
                                   <asp:View ID="OverdueChargesView" runat="server">
                                        <uc:OverdueChargesTab ID="OverdueChargesTab" runat="server" />
                                   </asp:View>
                                   
                                   <asp:View ID="LikelyRepurchases" runat="server">
                                        <uc:LikelyRepurchasesTab ID="LikelyRepurchasesTab" runat="server" />
                                   </asp:View>
                   
                                   <!--see BT#63 - reused this tab for displaying Retention Account Transactions-->
                                   <asp:View ID="AccountTransactions" runat="server">
                                        <uc:RetentionAccountsTab ID="RetentionAccountsTransactionsTab" runat="server" />
                                    </asp:View>

                                </asp:MultiView>
                            </div>
                         
                        </div>
                    </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </td>
        </tr>
      </tbody>
    </table>
    
    <script type="text/javascript">
        function showRetentionGridView() {
            $("#retentionGridView").show();
            callPageMethod("RetentionSchedules.aspx", "ToggleRetentionGridView", "{'show':'true'}");
            var t1 = window.setTimeout(function () {
                $("#retentionGridViewToggle").children().attr('alt', 'collapse').attr('src', '../images/collapse.png');
            }, 10);
        }

        function hideRetentionGridView() {
            $("#retentionGridView").hide();
            callPageMethod("RetentionSchedules.aspx", "ToggleRetentionGridView", "{'show':'false'}");
            var t2 = window.setTimeout(function () {
                $("#retentionGridViewToggle").children().attr('alt', 'expand').attr('src', '../images/expand.png');
            }, 10);
        }

        function toggleRetentionGridView() {
            var toggleButton = $("#retentionGridViewToggle");
            var command = toggleButton.children().attr('alt');
            if (command == 'collapse') {
                hideRetentionGridView();
            }
            else {
                showRetentionGridView();
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CustomerInformationContentPlaceholder" runat="server">
</asp:Content>
