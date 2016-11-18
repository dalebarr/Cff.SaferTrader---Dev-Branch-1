<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BatchAdjustmentsTab.ascx.cs" Inherits="Cff.SaferTrader.Web.UserControls.ReleaseTabs.BatchAdjustmentsTab" %>
<%@ Register TagPrefix="uc" Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" %>
<%@ Register TagPrefix="uc" Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" %>

<div id="divBatchChargesGV" runat="server" style="overflow:hidden;">
     <uc:CffGenGridView ID="BatchChargesGridView" runat="server"/>
</div>