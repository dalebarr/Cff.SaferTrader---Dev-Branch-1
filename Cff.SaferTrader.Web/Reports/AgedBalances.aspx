<%@ Page Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" 
    CodeBehind="AgedBalances.aspx.cs" Inherits="Cff.SaferTrader.Web.Reports.AgedBalances"   Title="Cashflow Funding Limited | Debtor Management | Aged Balances"  %>

<%@ MasterType VirtualPath="~/Reports.Master" %>

<%@ Register Src="~/UserControls/DatePicker.ascx" TagPrefix="ucx" TagName="DatePicker" %>
<%@ Register Src="~/UserControls/AllClientsFilter.ascx" TagPrefix="ucx" TagName="AllClientsFilter" %>
<%@ Register Src="~/UserControls/AllClientsReportHelpMessage.ascx" TagPrefix="ucx" TagName="AllClientsReportHelpMessage" %>
<%@ Register Src="~/UserControls/ReportPanels/AgedBalancesReportPanelNew.ascx" TagPrefix="ucx" TagName="AgedBalancesReportPanelNew" %>
<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="ucx" TagName="PageDescription" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="ucx" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.ReportPanels" TagPrefix="ucx" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" TagPrefix="ucx" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.Reports" TagPrefix="ucx" %>

<asp:Content  ContentPlaceHolderID="ReportHeaderPlaceholder" runat="server" ClientIDMode="AutoID">
     <div id="contentHeader">
        <h3>
             <a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info" CssClass="informationImage" Height="16px" Width="16px"/>
             </a>
            Aged Balances <%= targetName %>
        </h3>
        <ucx:PageDescription runat="server"  DescriptionTitle="" ID="PageDescription" DescriptionContent="Aged Balances" />
     </div>
</asp:Content>

<asp:Content  runat="server" ContentPlaceHolderID="ReportViewerContentPlaceholder"  ClientIDMode="AutoID" EnableViewState="true">
    <div runat="server" class="parameterSelector" id="divParamSelector">
           <table width="60%;height:auto;">
               <tbody>
                <tr style="width:100%">
                    <td style="width:8%">
                        <ucx:DatePicker ID="DatePickerRpt" runat="server" EnableAutoPostBack="true" OnUpdate="DatePickerRpt_Update"  />
                    </td>
                    <td style="text-align:left;width:10%;padding-right:0;margin-right:0;">
                       <asp:UpdatePanel ID="dropdownUpdatePanel" runat="server" UpdateMode="Conditional">
                           <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="RptCheckBox" />
                                <asp:AsyncPostBackTrigger ControlID="ReportTypeDropDownList" />
                           </Triggers>
                            <ContentTemplate>
                                <label>Age:</label>&nbsp;
                                <asp:DropDownList ID="ReportTypeDropDownList" runat="server" OnSelectedIndexChanged="ReportTypeDropDownList_SelectedIndexChanged" AutoPostBack="true" />
                           </ContentTemplate>
                       </asp:UpdatePanel>
                    </td>
                    <td style="width:auto;float:left;padding:0;margin:0;">
                        <ucx:AllClientsFilter ID="AllClientsFilter" runat="server" UpdateButtonVisible="false" Visible="false" />
                    </td>
                    <td style="width:8%;margin:0;padding:0;">
                          <asp:CheckBox ID="RptCheckBox" runat="server" autopostback="false" TextAlign="Right" /> With Notes <br />
                    </td>
                    <td style="float:left;">
                         <asp:ImageButton ID="UpdateButton" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif" Width="80px" Height="25px"
                           OnClick="UpdateButtonClick"  OnClientClick="startAnimate();" CssClass="updateButton"/>
                    </td>
                </tr>
               </tbody>
           </table>
    </div>
     <div id="DivReportViewerContentPlaceHolder" runat="server" visible="false" enableviewstate="true">
           <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="ReportsUpdatePanel" EnableViewState="true">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                    <asp:AsyncPostBackTrigger ControlID="DatePickerRpt" />
                    <asp:AsyncPostBackTrigger ControlID="UpdateButton" />
                    <asp:AsyncPostBackTrigger ControlID="RptCheckBox" />
                    <asp:PostBackTrigger ControlID="ExportButton" />
                </Triggers>
                <ContentTemplate>
                    <asp:PlaceHolder ID="rptNotesPlaceHolder" runat="server" EnableViewState="true"></asp:PlaceHolder>
                    <ucx:AgedBalancesReportPanelNew ID="ReportPanel" runat="server" EnableViewState="true"/>  
                </ContentTemplate>
         </asp:UpdatePanel>
      
     </div>
     
    <div id="divButtons" class="buttons" style="height:30px;">
        <div style="float:left;">
            <span> Date Viewed: <asp:Literal ID="DateViewedLiteral" runat="server" /> </span>
        </div>
        <div style="float:right">
            <asp:ImageButton ID="ExportButton" runat="server" OnClick="ExportButton_Click" AlternateText="Export" ImageUrl="~/images/btn_export.png" />
        </div>
    </div>
       
    <div>
        <asp:UpdatePanel runat="server" ID="UpdatePanelAllClientsReportHelpMessage" UpdateMode="Conditional" >
            <ContentTemplate>
                  <ucx:AllClientsReportHelpMessage runat="server" ID="AllClientsReportHelpMessage"></ucx:AllClientsReportHelpMessage>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

