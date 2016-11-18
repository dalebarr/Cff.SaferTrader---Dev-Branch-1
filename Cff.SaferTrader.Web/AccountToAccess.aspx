<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountToAccess.aspx.cs" Inherits="Cff.SaferTrader.Web.AccountToAccess" %>
<%@ Register TagPrefix="uc" TagName="ManagementContactDetails" Src="~/UserControls/ManagementContactDetails.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cashflow Funding Limited | Debtor Management</title>
    <%--<script src="js/jquery-1.2.6.min.js" type="text/javascript"></script>
    <script src="js/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>--%>
    <% 
        if (Request.ServerVariables["HTTPS"] == "off" || Request.ServerVariables["HTTPS"] == "")
       {%>
            <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/themes/base/jquery-ui.css" rel="stylesheet" type="text/css"/>
            <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
            <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/jquery-ui.min.js"></script>
        <% }
       else
       { %>
            <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/themes/base/jquery-ui.css" rel="stylesheet" type="text/css"/>
            <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
            <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/jquery-ui.min.js"></script>
    <% } %>
    
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="css/superfish.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function($) {
            $('#tab').tabs();
        });
    </script>
</head>
<body>
    <form id="form2" runat="server">
        <div id="mywrapper" class="access">
            <div id="access_content">
                <h1>Cashflow Funding Limited</h1>
                <div id="tab" style="height:auto;">
                    <ul>
                        <li>
                            <a href="#Client">Client</a>
                        </li>
                        <li>
                            <a href="#Customer">Customer</a>
                        </li>
                    </ul>
                    <div id="Client">
                        <button class="cupid-green">Cashflow Funding Limited</button>
                        <button class="cupid-green">Company A</button>
                        <button class="cupid-green">The Company Limited</button>
                        <button class="cupid-green">Business Essential Ltd.</button>
                    </div>
                    <div id="Customer">
                        <button class="cupid-green">Cashflow Funding Limited</button>
                        <button class="cupid-green">Google Limited</button>
                        <button class="cupid-green">The Company Incorporated</button>
                        <button class="cupid-green">Yahoo Essential Ltd.</button>
                    </div>
                   </div>
               <div class="loginFooter">
                    <p>
                        <uc:ManagementContactDetails id="managementContactDetailsBox" runat="server" />
                    </p>
                </div>
            </div>
        </div>
        <%--<div id="mywrapper" class="access">
            <div id="access_content">
                <h1>Commercial Factors and Finance</h1>
                 <div id="tab" style="height:500px;">
                    <ul class="sf-menu" style="left: 35px;">
                        <li>
                            <a href="#Client">Client</a>
                        </li>
                        <li>
                            <a href="#Customer">Customer</a>
                        </li>
                    </ul>
                    <div class="content_body" id="Client">
                        <button class="cupid-green">Commercial Factors & Finance</button>
                        <button class="cupid-green">Company A</button>
                        <button class="cupid-green">The Company Limited</button>
                        <button class="cupid-green">Business Essential Ltd.</button>
                    </div>
                    <div class="content_body" id="Customer">
                        <button class="cupid-green">Factors & Finance</button>
                        <button class="cupid-green">Google Limited</button>
                        <button class="cupid-green">The Company Incorporated</button>
                        <button class="cupid-green">Yahoo Essential Ltd.</button>
                    </div>
                </div>
                <div class="loginFooter">
                    <p>
                        <uc:ManagementContactDetails id="managementContactDetailsBox" runat="server" />
                    </p>
                </div>
            </div>
        </div>--%>
    </form>
</body>
</html>

