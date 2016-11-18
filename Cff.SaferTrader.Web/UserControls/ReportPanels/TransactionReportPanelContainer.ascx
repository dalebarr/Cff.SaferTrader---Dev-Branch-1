<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionReportPanelContainer.ascx.cs"   Inherits="Cff.SaferTrader.Web.UserControls.ReportPanels.TransactionReportPanelContainer" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>

<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/AllClientsFilter.ascx" TagPrefix="uc" TagName="AllClientsFilter" %>

<%@ Register Src="~/UserControls/DatePicker.ascx" TagPrefix="uc" TagName="DatePicker" %>

<%@ Register Src="~/UserControls/ReportPanels/TransactionReportPanel.ascx" TagPrefix="uc" TagName="TransactionReportPanel" %>
<%@ Register Src="~/UserControls/ReportPanels/CustomerTransactionReportPanel.ascx" TagPrefix="uc" TagName="CustomerTransactionReportPanel" %>
<%@ Register Src="~/UserControls/AllClientsReportHelpMessage.ascx" TagPrefix="uc" TagName="AllClientsReportHelpMessage" %>
<%@ Register Src="~/UserControls/TransactionStatusTypesFilter.ascx" TagPrefix="uc" TagName="TransactionStatusTypesFilter" %>

 <script type="text/javascript" src="js/ui.1.10.4/jquery-1.10.2.js" ></script>
 <script type="text/javascript" src="js/jquery-migrate-1.0.0.js" ></script>
 <script type="text/javascript" src="js/jquery.query-object.js" ></script>
 <script type="text/javascript" src="js/ui.1.10.4/ui/jquery-ui.js" ></script>
 <script type="text/javascript" src="js/ui.1.10.4/ui/jquery.ui.core.js" ></script>
 <script type="text/javascript" src="js/ui.1.10.4/ui/jquery.ui.button.js" ></script>
 <script type="text/javascript" src="js/ui.1.10.4/ui/jquery.ui.widget.js" ></script>
 <script type="text/javascript" src="js/ui.1.10.4/ui/jquery.ui.menu.js" ></script>
 <script type="text/javascript" src="js/ui.1.10.4/ui/jquery.ui.slider.js" ></script>
 <script type="text/javascript" src="js/ui.1.10.4/ui/jquery.ui.dialog.js" ></script>
 <script type="text/javascript" src="js/ui.1.10.4/ui/jquery.ui.accordion.js" ></script>
 <script type="text/javascript" src="js/ui.1.10.4/ui/jquery.ui.datepicker.js" ></script>
 <script type="text/javascript" src="js/cff.js" ></script>

<div id="transactionReportPanelContainer" class="parameterSelector" style="width:69.6%">
    <table>
        <tbody>
            <tr>
                <td>
                    <uc:DatePicker ID="DatePicker" runat="server" />
                </td>
                <td>
                    <uc:TransactionStatusTypesFilter ID="TransactionStatusTypesFilterControl"  runat="server" />
                </td>
                <td>
                      <div id="DateRangeDiv" runat="server" style="width:100%;">
                         <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" EnableViewState ="true">
                             <Triggers>
                                   <asp:AsyncPostBackTrigger ControlID="UpdateButton" />
                             </Triggers>
                            <ContentTemplate>
                                <div style="width:35%;float:left;">
                                    From: <asp:TextBox ID="FromDateTextBoxTR" CssClass="fromDateRange" runat="server"  ClientIDMode="Static" ViewStateMode ="Enabled" Width="150"/>
                                 </div>
                                 <div style="width:34%;float:left;padding-left:40px;padding-right:2px;">
                                    To: <asp:TextBox ID="ToDateTextBoxTR" CssClass="toDateRange" runat="server" ClientIDMode="Static" ViewStateMode ="Enabled" Width="150"/>
                                 </div>
                                 <div style="width:20%;float:right;padding-left:2px;">
                                     <asp:ImageButton ID="UpdateButton" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif" Width="80px" Height="25px"
                                        OnClientClick="startAnimate();"   OnClick="UpdateButtonClick"  CssClass="updateButton"  />
                                 </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>       
                <td>
                   <uc:AllClientsFilter ID="AllClientsFilter" runat="server" UpdateButtonVisible="false" />
                </td>
                <td style="display:none;">
                   <asp:ImageButton ID="UpdateButtonClient" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif"
                             Width="80px" Height="25px" OnClientClick="startAnimate();"   OnClick="UpdateButtonClientClick"  CssClass="updateButton" Visible ="false"  />
                </td>
          </tr>
          </tbody>
       </table>
      <span class="error"></span>
</div>
<div runat="server" id="reportData">
    <asp:UpdatePanel ID="GridUpdatePanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="DatePicker" />
            <asp:AsyncPostBackTrigger ControlID="SearchButton" />
            <asp:AsyncPostBackTrigger ControlID="UpdateButton" />
            <asp:AsyncPostBackTrigger ControlID="UpdateButtonClient" />
            <asp:PostBackTrigger ControlID="ExportButton" />
        </Triggers>
        <ContentTemplate>
            <uc:TransactionReportPanel ID="reportPanel" runat="server" />
            <uc:CustomerTransactionReportPanel ID="customerReportPanel" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div id="transactionReportPanelFooter" class="buttons" style="height: 30px; width: 68.6%">
    <div style="float: left;">
        <span>Date Viewed :
            <asp:Literal ID="DateViewedLiteral" runat="server" /></span>
    </div>
    <div style="float: right;">
        <span onmouseover="document.body.style.cursor = 'pointer';" onmouseout="document.body.style.cursor = 'default';">
            <asp:ImageButton ID="ExportButton" runat="server" OnClick="ExportButton_Click" AlternateText="Export" ImageUrl="~/images/btn_export.png" />
        </span>
    </div>
</div>

<uc:AllClientsReportHelpMessage runat="server" ID="AllClientsReportHelpMessage" Visible="false">
</uc:AllClientsReportHelpMessage>

<script type="text/javascript">
    //do not remove this part - needed as datepicker do not get reattached during postbacks
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
        var fromdateRangeOptions = {
            dateFormat: 'dd/mm/yy',
            showOn: 'both',
            buttonImage: relativePathToRoot + 'images/calendar.png',
            buttonImageOnly: true,
            buttonText: "Click to select a date",
            goToCurrent: true,
            clickInput: true,
            yearRange: "-30:+0",
            maxDate: "+0M +0D",
            prevText: "",
            nextText: "",
            changeMonth: true,
            changeYear: true,
            showMonthAfterYear: true
        };

        var todateRangeOptions = {
            dateFormat: 'dd/mm/yy',
            showOn: 'both',
            buttonImage: relativePathToRoot + 'images/calendar.png',
            buttonImageOnly: true,
            buttonText: "Click to select a date",
            goToCurrent: true,
            clickInput: true,
            yearRange: "-30:+0",
            maxDate: "+0M +0D",
            prevText: "",
            nextText: "",
            changeMonth: true,
            changeYear: true,
            showMonthAfterYear: true,
            hideIfNoPrevNext: true
        };


        $("#FromDateTextBoxTR").datepicker(fromdateRangeOptions);
        $("#ToDateTextBoxTR").datepicker(todateRangeOptions);
    });
    //do not remove this part - ref. mariper

 </script>
