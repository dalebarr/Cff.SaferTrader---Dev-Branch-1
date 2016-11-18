<%@ Page Language="C#" MasterPageFile="~/SafeTrader.Master" AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs" Inherits="Cff.SaferTrader.Web.Dashboard" Title="Cashflow Funding Limited | Debtor Management | Dashboard" 
    ValidateRequest="false" %>

<%@ Register Assembly="Cff.SaferTrader.Web" Namespace="Cff.SaferTrader.Web.UserControls" TagPrefix ="uc" %>
<%@ Register Src="~/UserControls/PageDescription.ascx" TagPrefix="uc" TagName="PageDescription" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div id="contentHeader">
        <h3>
            <a class="toggleDescription" onclick="toggleHelp(this);return false;">
                <asp:Image ID="informationImage" runat="server" ImageUrl="~/images/icon_information_grey.png" AlternateText="Show page info"  Height="16px" Width="16px"/>
            </a>
            Welcome!
        </h3>
    </div>
    <uc:PageDescription ID="pageDescription" DescriptionTitle="" DescriptionContent="Dashboard" runat="server" />
    <table id="contentViewerContainer" cellspacing="0" cellpadding="0">
        <tr>
            <td id="contentViewer">
                <div class="blob" id="content" runat="server">
                    <%-- <p>Say hello to our new client web site.</p>
                    <p>
                        As you will have noticed, we&#8217;ve given our client web site a major facelift. 
                        In addition to the new site being easier to read and navigate, it is also much easier for us to update. 
                        This means that we will be able to add fresh news and information about Cashflow Funding Limited on a regular basis. 
                        Rest assured, all key reporting information is still available &#8211; albeit via a more user friendly interface. 
                        As a result we have a web site that is better than before, including the ability to export many of the reports into Excel.
                    </p>
                    <p>
                        Each client will receive a User Guide to help explain the new website in greater detail but we encourage you to &#8220;experiment and explore.&#8221; 
                        Before you start; if you want specific customer/debtor information type the customer name into the &#8220;customer&#8221; field followed by <u>Enter</u> (on your keyboard).
                        When the customer field is left blank/empty information displayed is relative to the total ledger and not any specific customer.
                    </p>
                    <p>
                        You will notice that some menu options differ according to whether you are viewing specific customers or the generic ledger.
                        As mentioned all clients will receive a detailed User Guide, but please, keep checking back often, and feel free in giving feedback.
                    </p> --%>
                </div>
            </td>
            <td id="rhToggle">
                <div id="rhToggleIcon">
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
