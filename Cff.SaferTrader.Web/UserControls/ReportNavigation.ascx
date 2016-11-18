<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportNavigation.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.ReportNavigation" %>

<div id="reportNavigation">
    <h4 id="positionH4">
        Position Reports
    </h4>
    <div id ="divIdPositionH4">
        <ul >
        <li id="controlLi" runat="server">
            <asp:HyperLink ID="ControlLink" Text="Control"
                runat="server" />
        </li>
        <li id="agedBalancesLi" runat="server">
            <asp:HyperLink ID="AgedBalancesLink" Text="Aged Balances"
                runat="server" />
        </li>
        <li id="overdueChargesLi" runat="server">
            <asp:HyperLink ID="OverdueChargesLink" Text="Interest & Charges" runat="server" />
        </li>
        <li id="promptLi" runat="server">
            <asp:HyperLink ID="PromptLink" Text="Prompt" runat="server" />
        </li>        
        <li id="statusLi" runat="server">
            <asp:HyperLink ID="StatusLink" Text="Status" runat="server" />
        </li>
        <li id="unclaimedCreditNotesLi" runat="server">        
            <asp:HyperLink ID="UnclaimedCreditNotesLink" Text="Unclaimed Credit Notes" runat="server" />
        </li>
        <li id="unclaimedRepurchasesLi" runat="server">
            <asp:HyperLink ID="UnclaimedRepurchasesLink" Text="Unclaimed Prepayments" runat="server" />
        </li>
        <li id="CreditLimitExceededLinkLi" runat="server">
            <asp:HyperLink ID="CreditLimitExceededLink" Text="Credit Limit Exceeded" runat="server"/>
        </li>
        <li id="CreditStoppedLinkLi" runat="server">
            <asp:HyperLink ID="CreditStoppedLink" Text="Stop Credit Suggestions" runat="server"/>
        </li>
        <li id="CurrentShortPaidLi" runat="server">
            <asp:HyperLink ID="CurrentShortPaidLink" Text="Current Short Paid" runat="server" />
        </li>
        <li id="CurrentOverpaidLi" runat="server">
            <asp:HyperLink ID="CurrentOverpaidLink" Text="Current Overpaid" runat="server" />
        </li>
        <li id="CallsDueLi" runat="server">
            <asp:HyperLink ID="CallsDueLink" Text="Calls Due" runat="server" />
        </li>
        <li id="ClientActionLi" runat="server">
            <asp:HyperLink ID="ClientActionLink" Text="Client Action" runat="server" />
        </li>
        <li id="CustomerWatchLi" runat="server">
            <asp:HyperLink ID="CustomerWatchLink" Text="Customer Watch" runat="server"  />
        </li>
        <li id="StatementLi" runat="server">
            <asp:HyperLink ID="StatementLink" Text="Statement" runat="server" />
        </li>
    </ul>
    </div>
    
    <h4 id="transactionH4">
        Transaction Reports
    </h4>
    <div id="divTransactionH4">
        <ul>    
        <li id="CreditNotesLi" runat="server">
            <asp:HyperLink ID="CreditNotesLink" Text="Credit Notes" runat="server" />
        </li>
        <li id="JournalsLi" runat="server">
            <asp:HyperLink ID="JournalsLink" Text="Journals" runat="server" />
        </li>
        <li id="CreditBalanceTransfersLi" runat="server">
            <asp:HyperLink ID="CreditBalanceTransfersLink" Text="Credit Balance Transfers" runat="server" />
        </li>
        <li id="InvoicesLi" runat="server">
            <asp:HyperLink ID="InvoicesLink" Text="Invoices"
                runat="server" />
        </li>
        <li id="ReceiptsLi" runat="server">
            <asp:HyperLink ID="ReceiptsLink" Text="Receipts"
                runat="server" />
        </li>
        <li id="DiscountsLi" runat="server">
            <asp:HyperLink ID="DiscountsLink" Text="Discounts"
                runat="server" />
        </li>
        <li id="RepurchasesLi" runat="server">
            <asp:HyperLink ID="RepurchasesLink" Text="Repurchases"
                runat="server" />
        </li>
        <li id="UnallocatedLi" runat="server">
            <asp:HyperLink ID="UnallocatedLink" Text="Unallocated"
                runat="server" />
        </li>
        <li id="OverpaymentsLi" runat="server">
            <asp:HyperLink ID="OverpaymentsLink" Text="Overpayments"
                runat="server" />
        </li>
        <li id="AccountTrans" runat="server">
            <asp:HyperLink ID="AccountTransLink" Text="Account Transactions"
                runat="server" />
        </li>
    </ul>
    </div>
    <input id="customerPanelHidden" type="hidden"  value="customerPanelHidden"  />
    <input id="trxHiddenField" type="hidden" value="block" />
    <input id="posHiddenField" type="hidden" value="block" />
</div>

<script type="text/javascript">
    function getFileNameFromUrl(url) {
        return url.substring(url.lastIndexOf('/') + 1);
    }
    
    function attachClickEvents() {
        $("h4#positionH4").click(function () {
            var vdisplay = $("#posHiddenField").val();
            var t1 = window.setTimeout(function () {
                if (vdisplay.toString().toLowerCase() == "none") {
                    $("#posHiddenField").val('block');
                    $("div#divIdPositionH4").slideDown();
                }
                else {
                    $("#posHiddenField").val('none');
                    $("div#divIdPositionH4").slideUp();
                }
            }, 50);
        });

        $("h4#transactionH4").click(function () {
            var vdisplay2 = $("#trxHiddenField").val();
            var t2 = window.setTimeout(function () {
                if (vdisplay2.toString().toLowerCase() == "none") {
                    $("#trxHiddenField").val('block');
                    $("div#divTransactionH4").slideDown();
                }
                else {
                    $("#trxHiddenField").val('none');
                    $("div#divTransactionH4").slideUp();
                }
            }, 50);
        });
    }

    $(document).ready(function () {
        attachClickEvents();
    });

    //Sys.Application.add_load(attachAccordion); nb:: race condition affects postback
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
        try {
            attachClickEvents();
        } catch (Error1) {
            //alert('ERROR@@pageLoadedReportNavigation::' + Error1);
        }
    });
</script>


