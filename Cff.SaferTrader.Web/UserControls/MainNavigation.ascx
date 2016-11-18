<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainNavigation.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.MainNavigation" %>
<div id="mainNavigation">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="SearchButton" />
        </Triggers>
        <ContentTemplate>
            <ul class="sf-menu">
                <li>
                   <asp:HyperLink ID="DashboardLink" Text="Dashboard" runat="server" />
                </li>
                <li><a>Transactions</a>
                    <ul>
                        <li id="transactionsLi" runat="server" visible="false">
                            <asp:HyperLink ID="TransactionsLink" Text="Current Transactions" runat="server" />
                        </li>
                        <li id="transactionArchiveLi" runat="server" visible="false">
                            <asp:HyperLink ID="TransactionArchiveLink" Text="Transaction Archive"
                                runat="server" />
                        </li>
                        <li>
                            <asp:HyperLink ID="TransactionHistoryLink" Text="Transaction History" runat="server" />
                        </li>
                        <li>
                            <asp:HyperLink ID="TransactionSearchLink" Text="Transaction Search" runat="server" />
                        </li>
                    </ul>
                </li>
                <li>
                    <asp:HyperLink ID="ReportsLink" Text="Reports" runat="server" CssClass="reportsTab" />
                </li>
                <li>
                    <asp:HyperLink ID="NotesLink" Text="Notes" runat="server" /></li>
                <li id="contactsLink" runat="server">
                    <asp:HyperLink ID="ViewContactsLink" runat="server" Text="Contacts"/>
                </li>
                <li id="releaseTab" runat="server"><a><asp:Label ID="releaseTabLabel" Text="Releases" runat="server" /></a>
                    <ul>
                        <li>
                            <asp:HyperLink ID="InvoiceBatchesLink" Text="Invoice Batches" runat="server" onclick="blockPage();" />
                        </li>
                        <li>
                            <asp:HyperLink ID="RetentionSchedulesLink" Text="Retention Schedules" runat="server" />
                        </li>
                    </ul>
                </li>
            </ul>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">  
         function blockPage() {
            $.blockUI({
                        message: "Retrieving data please wait..",
                        css: {
                            border: 'none',
                            height: '50px',
                            'cursor': 'auto',
                            'width': '200px',
                            'top': 200,
                            'left': 400
                        }
            });
            setTimeout($.unblockUI, 2000);
         }

         function unblockPage() {
            setTimeout($.unblockUI, 2000);
         }
   </script>
</div>


