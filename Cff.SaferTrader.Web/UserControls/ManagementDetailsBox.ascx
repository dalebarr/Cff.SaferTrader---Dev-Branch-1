<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManagementDetailsBox.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.ManagementDetailsBox" %>
<%--<table class="reportHeader" style="width: 840px;margin-right: 0px; margin-top: 0px; border: none;">--%>
<%--<table class="reportHeader" style="margin-right: 0px; margin-top: 0px; border: none;">--%>
<table class="reportHeader" style="width:100%">    
    <tbody>
        <tr> 
            <td>
                <h2 id="nameSection" class="nameSection" runat="server">
                    <span><asp:Literal ID="nameLiteral" runat="server" /></span>
                    <span><asp:Literal ID="legalEntityOneLiteral" runat="server" /></span>
                    <span><asp:Literal ID="legalEntityTwoLiteral" runat="server" /></span>
                </h2>
            </td>
        </tr>

        <tr>
            <td>
                <%--<p id="gstSection" runat="server" class="gstSection">--%>
                <p id="gstSection" runat="server">
                    <strong>Tax Invoice</strong>&nbsp;
                    <span style="margin-left: 454px;">GST #:
                    <asp:Literal ID="gstLiteral" runat="server" /></span>
                </p>
            </td>
        </tr>

        <tr>
            <td>
                <p id="addressSection" runat="server">
                    <asp:Literal ID="addressLiteral" runat="server" />
                    <br />
                    <strong>Phone:</strong>
                    <asp:Literal ID="phoneLiteral" runat="server" />
                    <br />
                    <strong>Fax:</strong>
                    <asp:Literal ID="faxLiteral" runat="server" />
                    <br />
                    <strong>Web:</strong>
                    <asp:Literal ID="websiteLiteral" runat="server" />
                    <br />
                    <strong>Email:</strong>
                    <asp:HyperLink ID="emailLink" runat="server" />
                </p>
            </td>
        </tr>
    </tbody>
</table>