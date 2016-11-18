<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountTransactionsTab.ascx.cs"
         Inherits="Cff.SaferTrader.Web.UserControls.ReleaseTabs.AccountTransactionsTab" %>
         
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<div ID="acctTrxPlaceHolderDiv" style="overflow:hidden;">
    <asp:PlaceHolder ID="AccountTransactionsPlaceHolder" runat="server" />
</div>

<div id="DivSummaryBlock" class="summary" style="display:inline-block;width:70%;">
    <table style="width:50%;">
        <tbody>
            <tr>
                <td class="cffGGVHeader" style="width:5%;">Closing Balance = <asp:Literal ID="closingBalanceliteral" runat="server" /></td>
                <td style="width:1%;border:none;"></td>
                <td class="cffGGVHeader" style="width:5%;">Movement  =  <asp:Literal ID="movementLiteral" runat="server" /></td>
            </tr>
        </tbody>
    </table>
</div>