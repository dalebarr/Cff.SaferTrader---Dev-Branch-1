<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgedBalancesReportPanelNew.ascx.cs" 
         Inherits="Cff.SaferTrader.Web.UserControls.ReportPanels.AgedBalancesReportPanelNew" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="ucx" %>

<div class="scroll">
    <asp:PlaceHolder ID="reportPlaceholder" runat="server"/>
</div>
<asp:Literal ID="IsReportWithNotesLiteral" runat="server" Visible="false" />
