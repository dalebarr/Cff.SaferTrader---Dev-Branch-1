<%@ Page Title="Cashflow Funding Limited | Debtor Management | Notes" Language="C#"  MasterPageFile="~/SafeTrader.Master" AutoEventWireup="true" CodeBehind="Notes.aspx.cs" 
             Inherits="Cff.SaferTrader.Web.Notes" ValidateRequest="false"  EnableEventValidation="false"  %>

<%@ MasterType VirtualPath="~/SafeTrader.Master" %>

<%@ Import Namespace="Cff.SaferTrader.Core" %>
<%@ Import Namespace="NPOI.HSSF.Record.Formula.Functions" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix="uc" %>
<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls"  TagPrefix="uc"%>

<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>
<%@ Register Src="~/UserControls/CustomerNotesAdder.ascx" TagPrefix="uc" TagName="CustomerNotesAdder" %>
<%@ Register Src="~/UserControls/CustomerNotesEditModalBox.ascx" TagPrefix="uc" TagName="CustomerNotesEditModalBox" %>
<%@ Register Src="~/UserControls/MessageBox.ascx" TagPrefix="uc" TagName="MessageBox" %>
<%@ Register Src="~/UserControls/CustomerNotesFilter.ascx" TagPrefix="uc" TagName="CustomerNotesFilters" %>


<asp:Content ID="Content3"  ContentPlaceHolderID="AddCustomerNoteContentPlaceholder" runat="server" >
    <div>
            <dl id="addCustomerNote" class="collapsed">
                <dd>
                    <asp:LinkButton ID="AddCustomerNoteButton" runat="server" Text="Add Customer Note" OnClick="AddCustomerNoteButtonClick" />
                </dd>
                <dd>
                </dd>
            </dl>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
       <div>
         <div id="contentHeader">
            <h3>
                <a class="toggleDescription" onclick="toggleHelp(this);return false;">
                    <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info" CssClass="informationImage" 
                     Height="16px" Width="16px"/>
                </a> Notes <%= targetName %>
            </h3>
        </div>

       <div>
           <uc:PageDescription ID="transactionsPageDescription" DescriptionTitle="" DescriptionContent="Notes" runat="server" />
       </div>
    
        <table id="contentViewerContainer" cellspacing="0" cellpadding="0">
        <tbody>
        <tr>
            <td id="contentViewer" class="customerNotes">                
                <asp:UpdatePanel ID="NotesAdderUpdatePanel" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                    </Triggers>
                    <ContentTemplate>
                        <uc:CustomerNotesAdder ID="CustomerNotesAdder" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="EditUpdatePanel" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                    </Triggers>
                    <ContentTemplate>
                        <uc:CustomerNotesEditModalBox ID="EditCustomerNotesModalBox" runat="server" />
                        
                        <uc:MessageBox ID="CannotEditOldNoteMessageBox" runat="server" MessageTitle="Cannot edit this note"                           
                            Message="Old imported notes are designed not to be editable by anyone. Please see system administrator." />
                        
                        <uc:MessageBox ID="CannotEditNoteMessageBox" runat="server" 
                            Message="You are not the author of this note, or the note is not editable." 
                            MessageTitle="Cannot edit this note" />
                    
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
               <td id="rhToggle">
                <div id="rhToggleIcon">
                </div>
            </td>

        </tr>
       
        <tr>
            <td style="padding-top: 10px;">
                <div id="CliCustNotesDivHeader" style="<%=(this.CurrentScope()== Scope.CustomerScope) ?  "display: none": "display: block"%>" >
                  <h4 style="width:100%;background-color:#01844f;padding-left:2px;padding-top:5px;color:white;">
                    <a id="ClicustNotesToggle" onclick="toggleGrid('ClicustNotesToggle', 'CliCustNotesDiv');return false;" style="float:left;">
                    <img id="btnCliCustnotesToggle" src="images/expand.png" alt="expand" style="margin-top:0px;padding-top:0px;vertical-align:top;border:none;" /> 
                    </a>&nbsp; All Customer Notes
                  </h4>
                </div>

                <%--<div id="CliCustNotesDiv" style="<%=(this.CurrentScope()== Scope.CustomerScope) ?  "display: none": "display: block"%>" >--%>
                <div id="CliCustNotesDiv" style="display: none;">
                    <div class="parameterSelector" id="CliCustNotesDivDateFilter">
                       <table>
                            <tbody>
                              <tr>
                                <td>
                                    From: <asp:TextBox ID="dtCliCustNotesFrom" runat="server" clientidmode="AutoID" cssclass="fromDateRange" ></asp:TextBox>
                                            &nbsp; To: <asp:TextBox ID="dtCliCustNotesTo" runat="server" ClientIDMode="AutoID" CssClass="toDateRange" ></asp:TextBox>
                                    <asp:ImageButton ID="BtnUpdateCliCustNotes" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif" 
                                           OnClick="CliCustNotesUpdateButton_Click" Height="25px"  class="updateButton" OnClientClick="startAnimate();"   />
                                        
                                    <asp:Label ID="lblDateRangeValidMsg_CliCust" Visible ="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                                 </td>
                                  <td>
                                      <uc:PrintButton ID="CliCustNotesPrintButton" runat="server" OnClick="CliCustNotesPrintButton_Click" />
                                  </td>
                             </tr>
                            </tbody>
                       </table>
                       <span class="error"></span>
                    </div>
                    <asp:UpdatePanel ID="CliCustNotesUpdatePanel" runat="server" UpdateMode="Conditional" EnableViewState="true">
                       <Triggers>
                             <asp:AsyncPostBackTrigger ControlID="BtnUpdateCliCustNotes" />
                             <asp:AsyncPostBackTrigger ControlID="CliCustNotesPrintButton" />
                             <asp:AsyncPostBackTrigger ControlID="CustomerNotesAdder"/>
                       </Triggers>
                       <ContentTemplate>
                            <div class="scroll">
                                <asp:PlaceHolder ID="CliCustNotesPlaceHolder" runat="server" EnableViewState="true" />
                            </div>
                      </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>

        <!-- Filler -->
        <%--<tr>
            <td>
                <br />   
                <h4><asp:Label runat="server" ID="AspLabelWhichNotes" Text="Permanent and Client Notes"/></h4>
            </td>
        </tr>--%>  <!-- dbb -->
        
        <tr>
            <td style="padding-top: 5px;">
                  <div id="PermanentNotesPanelHeader" style='<%=(this.CurrentScope() == Scope.CustomerScope && this.custCheckBox) ?  "display: block;": "display: none;"%>'>
                          <h4 style="width:100%;background-color:#01844f;padding-left:2px;padding-top:5px;color:white;">
                                 <%--<a id="permanentNotesToggle" class="permanentNotesToggle" onclick="togglePermanentNotesGridView();return false;">--%>
                                 <a id="permanentNotesToggle" class="permanentNotesToggle" onclick="toggleGrid('permanentNotesToggle', 'PermanentNotesGridUpdatePanelDiv');return false;" style="float:left;">   
                                    <img  id="imgPermanentNotesToggle"  src="images/collapse.png" alt="collapse" />                                        
                                 </a>&nbsp; Permanent notes                           
                         </h4>
                  
                        <div id="PermanentNotesGridUpdatePanelDiv">
                            <div id="PermanentGridSelectorDiv" class="parameterSelector" style='<%=(this.CurrentScope() == Scope.CustomerScope && this.custCheckBox ) ?  "display: block;": "display: none;"%>'>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td class="invoiceBatchesmn">
                                                From: <asp:TextBox ID="dtPermanentNotesFilterFrom" runat="server" ClientIDMode="AutoID" CssClass="fromDateRange"></asp:TextBox> 
                                                               &nbsp; To: <asp:TextBox ID="dtPermanentNotesFilterTo" runat="server" ClientIDMode="AutoID" CssClass="toDateRange"></asp:TextBox>
                                                <asp:ImageButton ID="PermanentNotesUpdateButton" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif"
                                                    CssClass="updateButton" OnClick="PermanentNotesUpdateButton_Click" 
                                                    Width="80px" Height="25px"   class="updateButton" OnClientClick="startAnimate();" />
                                            </td>
                                            <td>
                                                <div id="permanentNotesButtons" runat="server">
                                                    <uc:PrintButton ID="PermanentNotesPrintButton" runat="server" OnClick="PermanentNotesPrintButton_Click" CssClass="calendarRelatedButton" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <span class="error"></span>
                            </div>
                            <div id="PermanentNotesDiv" class="PermanentNotes" style='<%=(this.CurrentScope() == Scope.CustomerScope && this.custCheckBox) ?  "display: block;": "display: none;"%>'>
                                  <div class="scroll">
                                       <asp:UpdatePanel ID="PermanentGridUpdatePanel" runat="server" UpdateMode="Conditional" EnableViewState="true">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="PermanentNotesUpdateButton" />
                                                <asp:AsyncPostBackTrigger ControlID="PermanentNotesPrintButton" />
                                                <asp:AsyncPostBackTrigger ControlID="CustomerNotesAdder"/>
                                            </Triggers>
                                            <ContentTemplate>
                                                 <asp:PlaceHolder ID="customerPermanentNotesPlaceholder" runat="server" Visible="true" EnableViewState="true"/>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                 </div>
                            </div>
                            <!-- <br />  DBB --> 
                        
                            <div id="ClientPermanentNotes" class="PermanentNotes" style='<%=(this.CurrentScope() == Scope.CustomerScope && this.custCheckBox) ?  "display: none;": "display: block;"%>'>
                                <div class="scroll">
                                    <asp:UpdatePanel ID="ClientPermanentNotesPanel" runat="server" UpdateMode="Conditional" EnableViewState="true">
                                        <Triggers>
                                           <asp:AsyncPostBackTrigger ControlID="PermanentNotesUpdateButton" />
                                            <asp:AsyncPostBackTrigger ControlID="ClientPermanentNotesPrintButton" />
                                            <asp:AsyncPostBackTrigger ControlID="CustomerNotesAdder"/>
                                        </Triggers>
                                        <ContentTemplate>
                                                <asp:PlaceHolder ID="clientPermanentNotesPlaceholder" runat="server" EnableViewState="true" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="buttons"  runat="server">
                                    <uc:PrintButton ID="ClientPermanentNotesPrintButton" runat="server" OnClick="ClientPermanentNotesPrintButton_Click" />
                                </div>
                            </div>
                        </div>
                  </div>

            </td>
        </tr> 

         <tr>
            <td style="padding-top: 5px;">
                <div id="CustomerNotesDivHeader" style="<%=(this.CurrentScope()==Scope.CustomerScope)?"display:block;":"display:none;"%>">
                       <h4  style="width:100%;background-color:#01844f;padding-left:2px;padding-top:5px;color:white;">
                            <%--<a id="customerNotesToggle" onclick="toggleCustomerNotesDivs();return false;">--%>
                            <a id="customerNotesToggle" onclick="toggleGrid('customerNotesToggle','CustomerNotesDiv');return false;" style="float: left;">
                                   <img  id="btnCustNotesToggle" src="images/collapse.png" alt="collapse" style="border:none;"/>
                             </a>&nbsp; Customer notes
                        </h4>

                        <div id="CustomerNotesDiv" style="<%=(this.CurrentScope()==Scope.CustomerScope) ? "display:block;":"display:none;"%>">
                            <div id="CustomerNotesDivParamHeader" class="parameterSelector">   <%--style="<%=(this.CurrentScope()==Scope.CustomerScope)?"display:block;":"display:none;"%>">--%>
                                <table id="CustomerNotesDivParHdrTable">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <p>
                                                    From: <asp:TextBox ID="TbxCustomerNotesFilterFrom" runat="server" ClientIDMode="AutoID" CssClass="fromDateRange" ></asp:TextBox> 
                                                    &nbsp; To:  <asp:TextBox ID="TbxCustomerNotesFilterTo" runat="server" ClientIDMode="AutoID"  CssClass="toDateRange"></asp:TextBox>
                                                    &nbsp; Activity type: &nbsp;
                                                    <asp:DropDownList ID="CbxCustomerActivityTypeDropDownList" runat="server" />
                                                    &nbsp; Note type: &nbsp;
                                                    <asp:DropDownList ID="CbxCustomerNoteTypeDropDownList" runat="server" />
                                                </p>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="btnCustomerNotesFilter" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif" 
                                                    CssClass="updateButton" OnClick="btnCustomerNotesFilter_Click" Height="25px" Width="66px"
                                                         OnClientClick="startAnimate();"   />
                                            </td>
                                            <td>
                                                <div>
                                                    <uc:PrintButton ID="CustomerNotesPrintButton" runat="server" OnClick="CustomerNotesPrintButton_Click" CssClass="calendarRelatedButton" />
                                                </div>                                                
                                            </td>
                                       </tr>
                                    </tbody>
                               </table>
                               <span class="error"></span>
                            </div>
                                <asp:UpdatePanel ID="CustomerNotesUpdatePanel" runat="server" UpdateMode="Conditional" EnableViewState="True">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnCustomerNotesFilter" />
                                        <asp:AsyncPostBackTrigger ControlID="CustomerNotesPrintButton" />
                                        <asp:AsyncPostBackTrigger ControlID="CustomerNotesAdder"/>
                                     </Triggers>
                                     <ContentTemplate>
                                         <div class="scroll">
                                            <asp:PlaceHolder ID="CffGGV_CustomerNotesPlaceHolder" runat="server" EnableViewState="true"/>             
                                         </div>
                                     </ContentTemplate>
                                </asp:UpdatePanel>
                            
                       </div>
                    </div>
                </td>
            </tr>

            <tr>
             <td style="padding-top: 5px;">
                <div id="ClientNotesToggleDiv" style="<%=(this.CurrentScope() == Scope.CustomerScope) ? "display:none;":"display:block;"%>">
                     <h4  style="background-color:#01844f;padding-left:2px;padding-top:5px;color:white;">
                          <%--<a id="clientNotesToggle" onclick="toggleClientNotesGridView();return false;">--%>
                          <a id="clientNotesToggle" onclick="toggleGrid('clientNotesToggle', 'ClientNotesDiv');return false;" style="float: left;">    
                                <img id="btnClientNotesToggle" src="images/expand.png" alt="expand" style="border:none;" />
                          </a>&nbsp; Client Notes 
                     </h4>
                </div>
                  
                <%--<div id="ClientNotesDiv" style="<%=(this.CurrentScope() == Scope.CustomerScope) ? "display:none;":"display:block;"%>" >--%>
                <div id="ClientNotesDiv" style="display:none;">
                    <div class="parameterSelector" id="ClientNotesParSelector">
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        From: <asp:TextBox ID="dtClientNotesFilterFrom" runat="server" ClientIDMode="AutoID" CssClass="fromDateRange"></asp:TextBox> 
                                                &nbsp; To: <asp:TextBox ID="dtClientNotesFilterTo" runat="server" ClientIDMode="AutoID" CssClass="toDateRange" ></asp:TextBox>
                                              <asp:ImageButton ID="btnClientNotesFilter" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif"
                                                CssClass="updateButton" OnClick="btnClientNotesFilter_Click" Width="80px" Height="25px"  class="updateButton" 
                                                    OnClientClick="startAnimate();"   />
                                   </td>
                                    <td>
                                        <uc:PrintButton ID="ClientNotesPrintButton" runat="server" OnClick="ClientNotesPrintButton_Click"   CssClass="calendarRelatedButton" />
                                    </td>
                                </tr>
                            </tbody>
                         </table>
                         <span class="error"></span>
                    </div>
                    
                    <div class="scroll">
                         <asp:UpdatePanel ID="ClientNotesDivPanel" runat="server"  UpdateMode="Conditional"  EnableViewState="true">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnClientNotesFilter" />
                                <asp:AsyncPostBackTrigger ControlID="ClientNotesPrintButton" />
                                <asp:AsyncPostBackTrigger ControlID="CustomerNotesAdder"/>
                            </Triggers>
                            <ContentTemplate>
                                  <asp:PlaceHolder ID="CffGGV_ClientNotesPlaceHolder" runat="server" EnableViewState="true" />
                             </ContentTemplate>
                         </asp:UpdatePanel>
                    </div>
                </div>
              </td>
           </tr>
       <%-- <tr>           
            <td id="rhToggle">
                <div id="rhToggleIcon">
                </div>
            </td>
        </tr>--%>
        </tbody>
    </table>
    </div>
  

    <script type="text/javascript">
        function togglePermanentNotesGridView() {
            var command = document.getElementById('imgPermanentNotesToggle').alt;
            if (command == 'collapse') {

                $('div.PermanentGridSelectorDiv').fadeOut(450);
                $('div.PermanentNotesGridUpdatePanelDiv').fadeOut(450);

                document.getElementById('PermanentGridSelectorDiv').style.visibility = 'collapse';
                document.getElementById('PermanentNotesGridUpdatePanelDiv').style.visibility = 'collapse';

                document.getElementById('imgPermanentNotesToggle').alt = 'expand';
                document.getElementById('imgPermanentNotesToggle').src = 'images/expand.png';

                //CallServiceMethod(relativePathToRoot + "UserPreferenceSetter.asmx/TogglePermanentNotesGridView", "{'show':'true'}");
            }
            else {
                $('div.PermanentGridSelectorDiv').fadeIn(450);
                $('div.PermanentNotesGridUpdatePanelDiv').fadeIn(450);

                document.getElementById('PermanentGridSelectorDiv').style.visibility = 'visible';
                document.getElementById('PermanentNotesGridUpdatePanelDiv').style.visibility = 'visible';

                document.getElementById('imgPermanentNotesToggle').alt = 'collapse';
                document.getElementById('imgPermanentNotesToggle').src = 'images/collapse.png';

                //CallServiceMethod(relativePathToRoot + "UserPreferenceSetter.asmx/TogglePermanentNotesGridView", "{'show':'false'}");
            }
        }

        function toggleClientNotesGridView() {
            var command = document.getElementById('btnClientNotesToggle').alt;

            if (command.toString() == 'collapse') {
                $('a.clientNotesToggle').children().attr('alt', 'expand').attr('src', 'images/expand.png');

                document.getElementById('btnClientNotesToggle').alt = 'expand';
                document.getElementById('btnClientNotesToggle').src = 'images/expand.png';
                $('div.ClientNotesDiv').fadeOut(450);

                document.getElementById('ClientNotesParSelector').style.visibility = 'collapse';
                document.getElementById('ClientNotesDiv').style.visibility = 'collapse';
            }
            else {
                $('a.clientNotesToggle').children().attr('alt', 'collapse').attr('src', 'images/collapse.png');

                document.getElementById('btnClientNotesToggle').alt = 'collapse';
                document.getElementById('btnClientNotesToggle').src = 'images/collapse.png';
                $('div.ClientNotesDiv').fadeIn(450);


                document.getElementById('ClientNotesParSelector').style.visibility = 'visible';
                document.getElementById('ClientNotesDiv').style.visibility = 'visible';
            }
        }

        function toggleCustomerNotesDivs() {
            var command = document.getElementById('btnCustNotesToggle').alt;

            if (command.toString() == 'collapse') {
                document.getElementById('btnCustNotesToggle').alt = 'expand';
                document.getElementById('btnCustNotesToggle').src = 'images/expand.png';

                $('div.CustomerNotesDivParamHeader').fadeOut(450);
                $('div.CustomerNotesDiv').fadeOut(450);

                /*todo: disbale for chrome*/
                document.getElementById('CustomerNotesDivParamHeader').style.visibility = 'collapse';
                document.getElementById('CustomerNotesDiv').style.visibility = 'collapse';
            }
            else {
                document.getElementById('btnCustNotesToggle').alt = 'collapse';
                document.getElementById('btnCustNotesToggle').src = 'images/collapse.png';
                $('div.CustomerNotesDivParamHeader').fadeIn(450);
                $('div.CustomerNotesDiv').fadeIn(450);

                document.getElementById('CustomerNotesDivParamHeader').style.visibility = 'visible';
                document.getElementById('CustomerNotesDiv').style.visibility = 'visible';

            }
        }


        function toggleCliCustNotesDivs() {
            var command = document.getElementById('btnCliCustNotesToggle').alt;

            if (command.toString() == 'collapse') {
                document.getElementById('btnCliCustNotesToggle').alt = 'expand';
                document.getElementById('btnCliCustNotesToggle').src = 'images/expand.png';

                $('div.CliCustNotesDivDateFilter').fadeOut(450);
                $('div.CliCustNotesDiv').fadeOut(450);

                /*todo: disbale for chrome*/
                document.getElementById('CliCustNotesDivDateFilter').style.visibility = 'collapse';
                document.getElementById('CliCustNotesDiv').style.visibility = 'collapse';
            }
            else {
                document.getElementById('btnCliCustNotesToggle').alt = 'collapse';
                document.getElementById('btnCliCustNotesToggle').src = 'images/collapse.png';
                $('div.CliCustNotesDivDateFilter').fadeIn(450);
                $('div.CliCustNotesDiv').fadeIn(450);

                document.getElementById('CliCustNotesDivDateFilter').style.visibility = 'visible';
                document.getElementById('CliCustNotesDiv').style.visibility = 'visible';
            }
        }

    </script>

</asp:Content>
