<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RetentionDetailsTab.ascx.cs"
    Inherits="Cff.SaferTrader.Web.UserControls.ReleaseTabs.RetentionDetailsTab" %>
<%@ Register TagPrefix="uc" TagName="ManagementDetailsBox" Src="~/UserControls/ManagementDetailsBox.ascx" %>
<%@ Register TagPrefix="uc" TagName="RetentionDetailsPanel" Src="~/UserControls/ReleaseTabs/RetentionDetailsPanel.ascx" %>
<%@ Register src="../GSTInvoiceBox.ascx" tagname="GSTInvoiceBox" tagprefix="uc1" %>
<div id="retentionDetails">
    <uc1:GSTInvoiceBox ID="GSTInvoiceBox1" runat="server" />
    <uc:RetentionDetailsPanel ID="retentionDetailsPanel" runat="server" />
</div>
