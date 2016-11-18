<%@ Page Language="C#" MasterPageFile="~/SafeTrader.Master" AutoEventWireup="true" CodeBehind="InvoiceBatches.aspx.cs"
    Inherits="Cff.SaferTrader.Web.InvoiceBatches" Title="Cashflow Funding Limited | Debtor Management | Invoice Batches" EnableEventValidation="true" %>

<%@ MasterType VirtualPath="~/SafeTrader.Master" %>
<%@ Register TagPrefix="uc" Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" %>
<%@ Register TagPrefix="uc" Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls.gGridViewControls" %>

<%@ Register TagPrefix="uc" Src="~/Usercontrols/ReleaseTabs/InvoicesTab.ascx" TagName="InvoicesTab" %>
<%@ Register TagPrefix="uc" Src="~/Usercontrols/ReleaseTabs/NonFactoredTab.ascx" TagName="NonFactoredTab" %>
<%@ Register TagPrefix="uc" Src="~/Usercontrols/ReleaseTabs/CreditsTab.ascx" TagName="CreditsTab" %>
<%@ Register TagPrefix="uc" Src="~/Usercontrols/ReleaseTabs/RepurchasesTab.ascx" TagName="RepurchasesTab" %>
<%@ Register TagPrefix="uc" Src="~/Usercontrols/ReleaseTabs/ScheduleTab.ascx" TagName="ScheduleTab" %>
<%@ Register TagPrefix="uc" Src="~/Usercontrols/ReleaseTabs/BatchAdjustmentsTab.ascx" TagName="BatchChargesTab" %>
<%@ Register TagPrefix="uc"  Src="~/UserControls/PageDescription.ascx" TagName="PageDescription" %>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div id="contentHeader">
        <ul id="tabPaneSubNav">
            <li class="current"><span>Invoice Batches</span></li>
            <li><a id="RetentionSchedulesLink" runat="server">Monthly Schedules</a></li>
        </ul>
        <h3>
            <a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info"
                    CssClass="informationImage"  Height="16px" Width="16px"/>
            </a>Invoice Batches <% =targetName %>
        </h3>
        <input id="customerPanelHidden" type="hidden" value="customerPanelHidden" />
    </div>

    <uc:PageDescription ID="PageDescription" DescriptionTitle="" DescriptionContent="Invoice Batches" runat="server" />
    <table id="contentViewerContainer" cellspacing="0" cellpadding="0">
        <tbody>
            <tr>
            <td id="contentViewer" class="invoiceBatches" style="height:auto">
                <div id="divInvoiceBatchesGridUpdatePanel">
                    <asp:UpdatePanel ID="InvoiceBatchesGridUpdatePanel" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="UpdateButton" />
                        <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                        <asp:AsyncPostBackTrigger ControlID="PreviousBatchButton" />
                        <asp:AsyncPostBackTrigger ControlID="NextBatchButton" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="parameterSelector">
                            <div style="width:80%;float:left;">
                              <table>
                                <tbody>
                                   <tr>
                                    <td>
                                        <label>Show:</label>
                                        <asp:DropDownList ID="BatchTypeDropDownList" runat="server" CssClass="batchTypeDropDownList"  />
                                    </td>
                                    <td>
                                        <div id="DateRangePicker" runat="server" >
                                            From: <asp:TextBox ID="FromDateRangeClient" CssClass="fromDateRange" runat="server"  ClientIDMode="Static" ViewStateMode ="Enabled"
                                                       OnTextChanged ="FromDateRangeClient_TextChanged" AutoPostBack="true"/>
                                            To: <asp:TextBox ID="ToDateRangeClient" CssClass="toDateRange" runat="server" ClientIDMode="Static" ViewStateMode ="Enabled"
                                                     OnTextChanged="ToDateRangeClient_TextChanged" />
                                        </div>
                                         <div id="AllClientsDateRangePicker" runat="server" >
                                            From: <asp:TextBox ID="FromDateRangeAllClients" CssClass="fromDateRange" runat="server"  ClientIDMode="Static" ViewStateMode ="Enabled"
                                                     OnTextChanged ="FromDateRangeAllClients_TextChanged" />
                                            To: <asp:TextBox ID="ToDateRangeAllClients" CssClass="toDateRange" runat="server" ClientIDMode="Static" ViewStateMode ="Enabled"
                                                     OnTextChanged ="ToDateRangeAllClients_TextChanged"/>
                                        </div>
                                         
                                    </td>
                                    <td class="invoiceBatchesUpdateButton">
                                        <asp:ImageButton ID="UpdateButton" runat="server" AlternateText="Update" ImageUrl="~/images/btn_update.gif"
                                           CssClass="updateButton"  OnClick="UpdateButton_Click" Width="80px" Height="25px"
                                                 OnClientClick="startAnimate();$('#LoadingDisplayDiv')[0].style.display = 'block';"   />
                                    </td>
                                    <td class="invoiceBatchesNumberSearch">
                                        <asp:Panel runat="server" ID="InvoiceBatchesNumberSearchPanel" DefaultButton="InvoiceBatchesNumberSearchButton">
                                            <div style="padding-top:13px;">
                                                <asp:UpdatePanel ID="InvoiceBatchesNumberSearchUpdatePanel" runat="server" UpdateMode="Conditional">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <label>Batch #:</label>
                                                        <uc:SecureTextBox runat="server" ID="InvoiceBatchesNumberSearchTextBox"></uc:SecureTextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div>
                                                <asp:ImageButton ID="InvoiceBatchesNumberSearchButton" runat="server" AlternateText="Search" CssClass="searchButton"
                                                    ImageUrl="~/images/btn_search_blue.png" OnClick="InvoiceBatchesNumberSearchButton_Click" 
                                                        OnClientClick="startSearchButtonAnimate(); $('#LoadingDisplayDiv')[0].style.display  = 'block';" />
                                            </div>
                                        </asp:Panel>
                                    </td>
                                    <td >
                                        <div id="LoadingDisplayDiv"  style="display:none;" >
                                            <div style="margin-left:20px;padding-top:20px;">
                                                Retrieving DATA Please wait...
                                            </div>
                                        </div>
                                       <asp:Panel runat="server" ID="panelLoadingDisplayDiv" Visible ="false">
                                            <asp:Literal ID="literalPanelLoadingDisplayDiv" Text="Retrieving DATA Please Wait..." runat="server"></asp:Literal>
                                       </asp:Panel>
                                   </td>
                                </tr>
                                </tbody>
                            </table>
                            </div>
                             <div style="float:right;padding-top:12px;padding-right:20px;">
                                  <span onmouseover="document.body.style.cursor  = 'pointer';" onmouseout="document.body.style.cursor='default';">
                                        <uc:PrintButton ID="PrintBatchInvoices" runat="server" OnClick="PrintBatchInvoices_Click" CssClass="right" Visible ="true" />  
                                  </span> 
                             </div>
                            <span class="error"></span>
                        </div>

                        <div id="DivBatchesPlaceHolder" style="height:30px;padding:0;margin:0;width:100%;">
                            <%--<div id="DivBGVHeaderHolder" style="width:100%;margin:0;padding:0">--%>
                            <div id="DivBGVHeaderHolder"  style="background-color:#01844f;padding-left:5px;padding-top:5px;padding-bottom: 5px;color:white;font-size: 15px;">
                               <%--<span style="display:none;"><img id="hidAccordionIndexImg" alt="1" src="./images/expand.png"/></span>--%>
                               <%--<h4 title="Batches"> --%>
                                     <%--<a id="adivBatchesPlaceHolder" style="float:left;padding-top:0;margin-top:1px;" onclick="toggleBatchesPlaceHolder();">--%>
                                     <a id="adivBatchesPlaceHolder" onclick="toggleBatchesPlaceHolder();" style="float: left;padding-top:0;margin-top:1px;">
                                            <img id="imgDivBatchesPlaceHolder" alt="collapse" src="./images/collapse.png"/>
                                     </a>&nbsp;Batches
                               <%--</h4>--%>
                            </div>
                            <div id="DivBatchesGridViewPlaceHolder" style="width:100%;margin:0;padding:0">
                                <asp:UpdatePanel ID="BatchesGridViewPlaceHolderUpdatePanel" runat="server" UpdateMode ="Conditional" EnableViewState="true">
                                      <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="UpdateButton" />
                                            <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                                      </Triggers>
                                      <ContentTemplate>
                                            <asp:PlaceHolder ID="batchPlaceholder" runat="server" EnableViewState="true" />
                                      </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                 
                    </ContentTemplate>
                </asp:UpdatePanel>
                </div>

                <div id="divDetailUpdatePanel">
                   <asp:UpdatePanel ID="DetailUpdatePanel" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="UpdateButton" />
                            <asp:AsyncPostBackTrigger ControlID="SearchButton" />
                            <asp:AsyncPostBackTrigger ControlID="PreviousBatchButton" />
                            <asp:AsyncPostBackTrigger ControlID="NextBatchButton" />
                            <asp:AsyncPostBackTrigger ControlID="InvoiceBatchesNumberSearchButton" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="DetailView" runat="server" class="blockableDiv">
                                <div class="blockDiv">
                                </div>
                                <div id="Details"  style="background-color:#01844f;padding-left:5px;padding-top:5px;padding-bottom: 5px;color:white;font-size: 15px;">
                                    <%--<h4 title="Details"> Details--%>
                                    <%--<a id="aDivDetailsPlaceHolder" style="float:left;padding-top:0;margin-top:1px;" onclick="toggleDetailsPlaceHolder();">--%>
                                    <a id="aDivDetailsPlaceHolder" style="float:left;padding-top:0;margin-top:1px;" onclick="toggleDetailsPlaceHolder();">                                        
                                         <img id="imgDivDetailsPlaceHolder" alt="collapse" src="./images/collapse.png" />
                                    </a>&nbsp;Details
                                    <%--</h4>--%>
                                </div>

                                <div id="BatchHeader">
                                    <div>
                                        <span onmouseover="document.body.style.cursor  = 'pointer';" onmouseout="document.body.style.cursor='default';">
                                            <uc:PrintButton ID="printButton" runat="server" OnClick="PrintButton_Click" CssClass="right" Visible ="true" />        
                                        </span>  
                                    </div>
                                    <div id="BatchButtons">
                                        <span onmouseover="this.document.body.style.cursor = 'pointer';">
                                            <asp:ImageButton ID="PreviousBatchButton" runat="server" OnClick="PreviousBatchButton_Click" ImageUrl="~/images/batch_up.png" />
                                            <asp:ImageButton ID="NextBatchButton" runat="server" OnClick="NextBatchButton_Click" ImageUrl="~/images/batch_down.png"  />
                                        </span>
                                    </div>
                                    <div>
                                         <h4>
                                        <asp:Literal ID="BatchLiteral" runat="server" />&nbsp;
                                        <asp:Literal ID="BatchNumberLiteral" runat="server" />&nbsp;
                                        <asp:Literal ID="clientNameLiteral" runat="server" />
                                        </h4>
                                    </div>
                                    <div id="BatchDetails">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td><asp:Literal ID="HeaderLiteral" runat="server" /></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Dated: <span><asp:Literal ID="DateLiteral" runat="server" /></span> 
                                                        Modified: <span><asp:Literal ID="ModifiedDateLiteral" runat="server" /></span> 
                                                        <span><asp:Literal ID="ReleasedDateLiteral" runat="server" /></span> 
                                                        Status: <span><asp:Literal ID="StatusLiteral" runat="server" /></span>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>                                
                                </div>
                                <div id="BatchTabs">
                                    <asp:Menu ID="TabMenu" runat="server" OnMenuItemClick="TabMenu_MenuItemClick" Orientation="Horizontal"
                                        StaticSelectedStyle-CssClass="tabSelected" >
                                        <Items>
                                            <asp:MenuItem Text="Schedule" Value="0" />
                                            <asp:MenuItem Text="Adjustments" Value="1" />
                                            <asp:MenuItem Text="Funding" Value="2" />
                                            <asp:MenuItem Text="Non Funding" Value="3" />
                                            <asp:MenuItem Text="Credits" Value="4" />
                                            <asp:MenuItem Text="Reclassified" Value="5" />
                                        </Items>
                                    </asp:Menu>
                                </div>
                                <div id="DivTabViewsBatch">
                                <asp:MultiView ID="TabViewsBatch" runat="server" ActiveViewIndex="0" OnActiveViewChanged="TabViews_ActiveViewChanged">
                                    <asp:View ID="Schedule" runat="server" >
                                        <uc:ScheduleTab ID="ScheduleTab" runat="server" />
                                    </asp:View>
                                    <asp:View ID="Charges" runat="server">
                                        <uc:BatchChargesTab ID="AdjustmentsTab" runat="server" />
                                    </asp:View>
                                    <asp:View ID="Invoices" runat="server">
                                        <uc:InvoicesTab ID="InvoicesTab" runat="server" />
                                    </asp:View>
                                    <asp:View ID="NonFactored" runat="server">
                                        <uc:NonFactoredTab ID="NonFactoredTab" runat="server" />
                                    </asp:View>
                                    <asp:View ID="Credits" runat="server">
                                        <uc:CreditsTab ID="CreditsTab" runat="server" />
                                    </asp:View>
                                    <asp:View ID="Repurchases" runat="server">
                                        <uc:RepurchasesTab ID="RepurchasesTab" runat="server" />
                                    </asp:View>
                                </asp:MultiView>
                                </div>
                            </div>
                        </ContentTemplate>
                  </asp:UpdatePanel>             
                </div>
            </td>
        </tr>
        </tbody>
    </table>

    <script type="text/javascript">  
        var vTimeoutClearer;
        var doPageUnblock = false;
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

        function showBatchGridView() {
            $("div#divDetailUpdatePanel").hide();
            $("div#DivBatchesGridViewPlaceHolder").css("display", "block");
            $("div#DivBatchesGridViewPlaceHolder").show();

            callPageMethod("InvoiceBatches.aspx", "ToggleBatchGridView", "{'show':'true'}");
            var t1 = window.setTimeout(function () {
               $("img#imgDivBatchesPlaceHolder")[0].src = "./images/collapse.png";
               $("img#imgDivBatchesPlaceHolder")[0].alt = "collapse";
            }, 15); //put timer to handle the race condition

            $("div#divDetailUpdatePanel").css("margin-top", "" + ($("#DivBatchesGridViewPlaceHolder").height() + 20).toString() + "px");
            $("div#divDetailUpdatePanel").show();
        }

        function hideBatchGridView() {
            $("div#divDetailUpdatePanel").hide();
            $("div#DivBatchesGridViewPlaceHolder").css("display", "none");
            $("div#DivBatchesGridViewPlaceHolder").hide();
            callPageMethod("InvoiceBatches.aspx", "ToggleBatchGridView", "{'show':'false'}");

            var t2 = window.setTimeout(function () {
                $("img#imgDivBatchesPlaceHolder")[0].src = "./images/expand.png";
                $("img#imgDivBatchesPlaceHolder")[0].alt = "expand";
            }, 15);

            $("div#divDetailUpdatePanel").css("margin-top", "0px");
            $("div#divDetailUpdatePanel").show();
        }

        function toggleDateRangePicker() {
                //document.getElementById("BatchTypeDropDownList");
                var dropDownList = $('.batchTypeDropDownList');
                if (dropDownList.find(':selected').text() !== 'Started') {
                    $('td.fromDate').show();
                    $('td.toDate').show();
                } else {
                    $('td.fromDate').hide();
                    $('td.toDate').hide();
                }
            }

            function attachToggleDateRangePicker() {
                $(".batchGridView").change(toggleDateRangePicker);
            }        

            function toggleBatchesPlaceHolder() {
                if ($("img#imgDivBatchesPlaceHolder")[0].alt =="collapse")
                {
                    $("div#divDetailUpdatePanel").hide();
                    $("div#DivBatchesGridViewPlaceHolder").css("display", "none");
                    $("div#DivBatchesGridViewPlaceHolder").hide(); //.fadeOut(10);      
                    callPageMethod("InvoiceBatches.aspx", "ToggleBatchGridView", "{'show':'true'}");

                    var t1 = window.setTimeout(function () {
                        $("img#imgDivBatchesPlaceHolder")[0].src = "./images/expand.png";
                        $("img#imgDivBatchesPlaceHolder")[0].alt = "expand";
                    }, 15);

                    $("div#divDetailUpdatePanel").css("margin-top", "0px");
                    $("div#divDetailUpdatePanel").show();
                } else {
                   
                    $("div#DivBatchesGridViewPlaceHolder").css("display", "block");
                    $("div#DivBatchesGridViewPlaceHolder").show(); //.fadeIn(10);
                    callPageMethod("InvoiceBatches.aspx", "ToggleBatchGridView", "{'show':'false'}");

                    var t2 = window.setTimeout(function () {
                        $("img#imgDivBatchesPlaceHolder")[0].src = "./images/collapse.png";
                        $("img#imgDivBatchesPlaceHolder")[0].alt = "collapse";
                    }, 15);
                  
                    $("div#divDetailUpdatePanel").css("margin-top", "" + ($("#DivBatchesGridViewPlaceHolder").height() + 20).toString() + "px");
                    $("div#divDetailUpdatePanel").show();
                }
            }

            function toggleDetailsPlaceHolder() {
                if ($("img#imgDivDetailsPlaceHolder")[0].alt == "collapse") {
                    $("img#imgDivDetailsPlaceHolder")[0].src = "./images/expand.png";
                    $("img#imgDivDetailsPlaceHolder")[0].alt = "expand";
                    $("div#BatchHeader").hide(); 
                    $("div#BatchTabs").hide();
                    $("div#DivTabViewsBatch").hide();
                } else {
                    $("img#imgDivDetailsPlaceHolder")[0].src = "./images/collapse.png";
                    $("img#imgDivDetailsPlaceHolder")[0].alt = "collapse";
                    $("div#BatchHeader").show();
                    $("div#BatchTabs").show();
                    $("div#DivTabViewsBatch").show();
                }
            }

            function adjustDetailsPlaceHolder()
            {
                $("div#divDetailUpdatePanel").css("margin-top", "" + ($("#DivBatchesGridViewPlaceHolder").height() + 20).toString() + "px");
            }

            function toggleDivDetailsPlaceHolderImage() {
                if ($("img#imgDivDetailsPlaceHolder")[0].alt == "collapse") {
                    $("img#imgDivDetailsPlaceHolder")[0].alt = "expand";
                    $("img#imgDivDetailsPlaceHolder")[0].src = "./images/expand.png";
                } else {
                    $("img#imgDivDetailsPlaceHolder")[0].alt = "collapse";
                    $("img#imgDivDetailsPlaceHolder")[0].src = "./images/collapse.png";
                }
            }

            function blockPage(ctr) {
                document.body.style.cursor = 'wait';
                try {
                    clearTimeout(vTimeoutClearer);
                } catch (errory) { }
                $('#LoadingDisplayDiv')[0].setAttribute("visible", "true");
                $('#LoadingDisplayDiv')[0].style.display = "block";
                //$.blockUI({                                               // modified by dbb
                //    message: "Retrieving data please wait..",
                //    css: {
                //        border: 'none',
                //        height: '100px',
                //        'cursor': 'auto',
                //        'width': '200px',
                //        'top': 200,
                //        'left': 300
                //    }
                //});

                $.blockUI({
                    message: $('#LoadingDisplayDiv'),
                    css: {
                        border: 'none',
                        height: '100px',
                        padding: '15px',
                        backgroundColor: '#000',
                        '-webkit-border-radius': '10px',
                        '-moz-border-radius': '10px',
                        opacity: .5,
                        color: '#FFFF00'
                    }
                });


                if (ctr>0)
                    setTimeout($.unblockUI, ctr);
                else
                    setTimeout($.unblockUI, 3000);

                try {
                    clearTimeout(vTimeoutClearer);
                } catch (errory) { }
            }

            function unblockPage(ctr) {
                document.body.style.cursor = 'default';
                vTimeoutClearer = setTimeout($.unblockUI, ctr);
                $('#LoadingDisplayDiv')[0].setAttribute("visible", "false");
                $('#LoadingDisplayDiv')[0].style.display = "none";
                try {
                    clearTimeout(vTimeoutClearer);
                } catch (errory) { }
            }

            function unblockPageBlock(sender, args) {
                vTimeoutClearer = setTimeout($.unblockUI, 2000);
                document.body.style.cursor = 'default';
                $('#LoadingDisplayDiv')[0].setAttribute("visible", "false");
                $('#LoadingDisplayDiv')[0].style.display = "none";
                try {
                    clearTimeout(vTimeoutClearer);
                } catch (errory) { }
            }

            function attachDateRangePickers() {
                $("#FromDateRangeClient").datepicker(fromdateRangeOptions);
                $("#ToDateRangeClient").datepicker(todateRangeOptions);
                $("#FromDateRangeAllClients").datepicker(fromdateRangeOptions);
                $("#ToDateRangeAllClients").datepicker(todateRangeOptions);
            }

            $(document).ready(function() {
                attachDateRangePickers();

                attachToggleDateRangePicker();
                toggleDateRangePicker();

                $("div#divDetailUpdatePanel").css("margin-top", "" + ($("#DivBatchesGridViewPlaceHolder").height() + 2).toString() + "px");
            });

            Sys.Application.add_load(attachToggleDateRangePicker);
            Sys.Application.add_load(toggleDateRangePicker);

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(unblockPageBlock);
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
                attachDateRangePickers();
            });
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                attachDateRangePickers();
            });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CustomerInformationContentPlaceholder" runat="server">
</asp:Content>
