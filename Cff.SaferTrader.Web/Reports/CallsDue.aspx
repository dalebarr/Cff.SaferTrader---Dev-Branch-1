<%@ Page Title="Cashflow Funding Limited| Debtor Management | Calls Due" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="CallsDue.aspx.cs" Inherits="Cff.SaferTrader.Web.Reports.CallsDue" %>

<%@ MasterType VirtualPath="~/Reports.Master" %>

<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/AllClientsFilter.ascx" TagPrefix="uc" TagName="AllClientsFilter" %>
<%@ Register Src="~/UserControls/PeriodReportTypeFilter.ascx" TagPrefix="uc" TagName="PeriodReportTypeFilter" %>
<%@ Register Src="~/UserControls/AllClientsOrderByFilter.ascx"TagPrefix="uc" TagName="AllClientsOrderByFilter" %>
<%@ Register Src="~/UserControls/ClientOrderByFilter.ascx"TagPrefix="uc" TagName="ClientOrderByFilter" %>
<%@ Register Src="~/UserControls/BalanceRangeFilter.ascx"TagPrefix="uc" TagName="BalanceRangeFilter" %>
<%@ Register Src="~/UserControls/ReportPanels/CallsDueReportPanel.ascx"TagPrefix="uc"
    TagName="CallsDueReportPanel" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls"
    TagPrefix="uc" %>
    <%@ Register Src="~/UserControls/AllClientsReportHelpMessage.ascx" TagPrefix="uc"
    TagName="AllClientsReportHelpMessage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ReportHeaderPlaceholder" runat="server">
<div id="contentHeader">
  <h3>
	<a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info" CssClass="informationImage" 
                 Height="16px" Width="16px"/>
            </a>
	Calls Due <% =targetName %>
  </h3>
</div>
<uc:PageDescription ID="invoicesPageDescription" DescriptionTitle="Calls Due" DescriptionContent="Calls Due" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ReportViewerContentPlaceholder" runat="server"> 
    <div class="parameterSelector" style="width: 79.5%;">
        <table>
            <tr>
            
                 <uc:PeriodReportTypeFilter ID="PeriodReportTypeFilter" runat="server" />
                  <uc:AllClientsOrderByFilter ID="AllClientsOrderByFilter" runat="server" />
                  <uc:ClientOrderByFilter ID="ClientOrderByFilter" runat="server" />
                 <uc:BalanceRangeFilter ID="BalanceRangeFilter" runat="server" />
                    
                <uc:AllClientsFilter ID="AllClientsFilter" runat="server" DatePickerVisible="false"
                    UpdateButtonVisible="false" />
                <td>
                    <asp:ImageButton ID="UpdateButton" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif" Width="80px" Height="25px"
                        OnClientClick="startAnimate();" CssClass="updateButton" OnClick="UpdateButtonClick" />
                </td>
            </tr>
        </table>
    </div>
       <div runat="server" id="reportData">
        <asp:UpdatePanel ID="GridUpdatePanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="SearchButton" />
            <asp:PostBackTrigger ControlID="ExportButton" />
        </Triggers>
        <ContentTemplate>
            <uc:CallsDueReportPanel ID="ReportPanel" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    <div class="buttons" style="height:30px;width: 78.5%;">
          <div style="float:left;">
                <span>Date Viewed : <asp:Literal ID="DateViewedLiteral" runat="server" /></span>
           </div>
           <div style="float:right;">
              <asp:ImageButton ID="ExportButton" runat="server" OnClick="ExportButton_Click" AlternateText="Export" ImageUrl="~/images/btn_export.png" />
           </div>
    </div>
    <uc:AllClientsReportHelpMessage runat="server" ID="AllClientsReportHelpMessage" Visible="false">
    </uc:AllClientsReportHelpMessage>
</asp:Content>
