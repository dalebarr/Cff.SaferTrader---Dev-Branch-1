<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="AgedBalNotesDetailRowTemplateNew.aspx.cs" 
    Inherits="Cff.SaferTrader.Web.Reports.AgedBalNotesDetailRowTemplateNew" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix ="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="uc" %>

<div id="scroll">
    <uc:CffGenGridView ID="detailGridView" runat="server"/>
</div>

