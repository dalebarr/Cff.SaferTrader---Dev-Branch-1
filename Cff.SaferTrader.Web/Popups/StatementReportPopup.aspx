<%@ Page Title="" Language="C#" MasterPageFile="~/Popups/Popup.Master" AutoEventWireup="true" CodeBehind="StatementReportPopup.aspx.cs"
    Inherits="Cff.SaferTrader.Web.Popups.StatementReportPopup" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ReportPanels/StatementReportPanel.ascx" TagPrefix="uc" TagName="StatementReportPanel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupHeaderContentPlaceholder" runat="server">
       <asp:Literal ID="reportTitleLiteral" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupBodyContentPlaceholder" runat="server">
    <uc:StatementReportPanel id="reportPanel" runat="server" ReadOnly="true" />        
</asp:Content>
