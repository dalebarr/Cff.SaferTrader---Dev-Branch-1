<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceBatchHeader.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.ReleaseTabs.InvoiceBatchHeader" %>
<div id="BatchHeader">
    <div id="divBH">
        <h4>
            <asp:Literal ID="clientNameLiteral" runat="server" />&nbsp;
            <asp:Literal ID="BatchLiteral" runat="server" />&nbsp;
            <asp:Literal ID="BatchNumberLiteral" runat="server" />
        </h4>
    </div>
    <div id="BatchDetails">
        <p>
            <span><asp:Literal ID="HeaderLiteral" runat="server" /></span>
        </p>
        <p>
            Dated: <span><asp:Literal ID="DateLiteral" runat="server" /></span> 
            Modified: <span><asp:Literal ID="ModifiedDateLiteral" runat="server" /></span> 
            <span><asp:Literal ID="ReleasedDateLiteral" runat="server" /></span> 
            Status: <span><asp:Literal ID="StatusLiteral" runat="server" /></span>
        </p>
    </div>
</div>
