<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduleTab.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.ReleaseTabs.ScheduleTab" %>

<%@ Register TagPrefix="uc" TagName="BatchSchedulePanel" Src="~/UserControls/ReleaseTabs/BatchSchedulePanel.ascx" %>
<%@ Register TagPrefix="uc" TagName="ManagementDetailsBox" Src="~/UserControls/ManagementDetailsBox.ascx" %>

<div id="divSchedulesTab" style="overflow:hidden;">
    <uc:ManagementDetailsBox ID="managementDetailsBox" runat="server" ShowName="false" ShowAddress="false" ShowGst="true" />
    <uc:BatchSchedulePanel id="batchSchedulePanel" runat="server" />
</div>