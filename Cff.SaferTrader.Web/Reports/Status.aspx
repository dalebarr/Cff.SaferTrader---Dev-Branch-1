<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Status.aspx.cs" Inherits="Cff.SaferTrader.Web.Reports.Status" MasterPageFile="~/Reports.Master" 
Title="Cashflow Funding Limited | Debtor Management | Status" %>

<%@ MasterType VirtualPath="~/Reports.Master" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web" TagPrefix="uc" %>

<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/DatePicker.ascx" TagPrefix="uc" TagName="DatePicker" %>
<%@ Register Src="~/UserControls/StatusPicker.ascx" TagPrefix="uc" TagName="StatusPicker" %>
<%@ Register Src="~/UserControls/AllClientsFilter.ascx" TagPrefix="uc" TagName="AllClientsFilter" %>
<%@ Register Src="~/UserControls/ReportPanels/StatusReportPanel.ascx" TagPrefix="uc" TagName="StatusReportPanel" %>
<%@ Register Src="~/UserControls/AllClientsReportHelpMessage.ascx" TagPrefix="uc" TagName="AllClientsReportHelpMessage" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="ReportHeaderPlaceholder" runat="server">
    <div id="contentHeader">
		<h3>
			<a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info"
                CssClass="informationImage" Height="16px" Width="16px"/>
            </a>
			Status <%= targetName %> 
		</h3>    
	</div>
	<uc:PageDescription ID="promptPageDescription" DescriptionTitle="" DescriptionContent="Status"  runat="server" />
</asp:Content>
<asp:Content ID="ReportContent" ContentPlaceHolderID="ReportViewerContentPlaceholder" runat="server">        
    <div class="parameterSelector">    
        <table>
            <tr style="text-decoration: underline">
                <td>
                    <uc:DatePicker ID="DatePicker" runat="server" EnableAutoPostBack="false" />
                </td>
                <td>
                    <uc:StatusPicker ID="StatusPicker" runat="server" EnableAutoPostBack="false" />
                </td>
                <td>
                    <uc:AllClientsFilter ID="AllClientsFilter" runat="server" UpdateButtonVisible="false" Visible="false"/>
                </td>
                <td>
                    <asp:ImageButton ID="UpdateButton"  OnClientClick="startAnimate();"
                                 CssClass="updateButton" OnClick="UpdateButton_Click" Enabled = "true"  
                                    runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif" Width="80px" Height="25px"/>
                </td>
            </tr>
        </table>        
    </div>    
     <div runat="server" id="reportData" style="width:auto;">
        <div>
          <asp:UpdatePanel ID="GridUpdatePanel" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DatePicker" />
                    <asp:AsyncPostBackTrigger ControlID="StatusPicker" />
                    <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                    <asp:PostBackTrigger ControlID="ExportButton" />
                </Triggers>
                <ContentTemplate>   
                    <%--<div class="scroll" style="overflow:auto;">--%>
                    <div class="scroll">
                        <uc:StatusReportPanel id="ReportPanel" runat="server" />
                    </div>         
                </ContentTemplate>
         </asp:UpdatePanel>
        </div>
        <div class="buttons" style="height:30px;">
            <div style="float:left;">
                <span>Date Viewed : <asp:Literal ID="DateViewedLiteral" runat="server" /></span>
            </div>
            <div style="float:right;">
                <asp:ImageButton ID="ExportButton" runat="server" OnClick="ExportButton_Click" AlternateText="Export" ImageUrl="~/images/btn_export.png" />
            </div>
        </div>   
    </div>   
    <uc:AllClientsReportHelpMessage runat="server" ID="AllClientsReportHelpMessage" Visible="false" />
</asp:Content>