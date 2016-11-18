<%@ Page Language="C#" MasterPageFile="~/SafeTrader.Master" AutoEventWireup="true"
    CodeBehind="LogOnRedirection.aspx.cs" Inherits="Cff.SaferTrader.Web.LogOnRedirection"
    Title="Cashflow Funding Limited | Debtor Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CustomerInformationContentPlaceholder"
    runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="AddCustomerNoteContentPlaceholder"
    runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div id="contentHeader">
        <h3>
            Redirection</h3>
    </div>
    <table id="contentViewerContainer" cellspacing="0" cellpadding="0">
        <tr>
            <td id="contentViewer">
                <div class="blob">
                    Please wait. You are now logging on..
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
