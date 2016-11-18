<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RetentionReleaseEstimateReportPanel.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.ReportPanels.RetentionReleaseEstimateReportPanel" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<div class="scroll">
   <asp:PlaceHolder ID="GridViewReportPanel" runat="server"></asp:PlaceHolder>
</div>

<table class="totalSummary">
    <tbody>
        <tr>
            <td>
                Release from Funded Transactions:
            </td>
            <td class="right">
                <asp:Literal ID="FundedTransactionReleaseLiteral" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Release from Non Funded Transactions:
            </td>
            <td class="right">
                <asp:Literal ID="NonFundedTransactionReleaseLiteral" runat="server" />
            </td>
        </tr>
        <tr class="total">
            <td>
                Release Total:
            </td>
            <td class="right">
                <asp:Literal ID="ReleaseTotal" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                Unclaimed Credits:
            </td>
            <td class="right">
                <asp:Literal ID="UnclaimedCreditsLiteral" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Unclaimed Prepayments:
            </td>
            <td class="right">
                <asp:Literal ID="UnclaimedRepurchasesLiteral" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Unclaimed Discounts:
            </td>
            <td class="right">
                <asp:Literal ID="UnclaimedDiscountsLiteral" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
               <asp:Literal id="LikelyRepurchasesText" runat="server"> Likely Repurchases (65 days):</asp:Literal>
            </td>
            <td class="right">
                <asp:Literal ID="LikelyRepurchasesLiteral" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Overdue Charges:
            </td>
            <td class="right">
                <asp:Literal ID="OverdueChargesLiteral" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Cheque Fees (<asp:Literal ID="NumberOfChequesLiteral" runat="server" />
                @
                <asp:Literal ID="ChequeFeeLiteral" runat="server" />):
            </td>
            <td class="right">
                <asp:Literal ID="ChequeFeesLiteral" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Postage Fees (<asp:Literal ID="NumberOfPostsLiteral" runat="server" />
                @
                <asp:Literal ID="PostageFeeLiteral" runat="server" />):
            </td>
            <td class="right">
                <asp:Literal ID="PostageFeesLiteral" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Letter Fees (<asp:Literal ID="NumberOfLettersLiteral" runat="server" />
                @
                <asp:Literal ID="LetterFeeLiteral" runat="server" />):
            </td>
            <td class="right">
                <asp:Literal ID="LetterFeesLiteral" runat="server" />
            </td>
        </tr>
        <tr class="total">
            <td>
                Total:
            </td>
            <td class="right">
                <asp:Literal ID="DeductablesTotalLiteral" runat="server" />
            </td>
        </tr>
        <tr class="total">
            <td>
                Estimated Release:
            </td>
            <td class="right">
                <asp:Literal ID="EstimatedReleaseLiteral" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <span>Date Viewed</span>:
            </td>
            <td class="right">
                <asp:Literal ID="DateViewedLiteral" runat="server" />
            </td>
        </tr>
    </tbody>
</table>
