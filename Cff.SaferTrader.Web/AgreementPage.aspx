﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgreementPage.aspx.cs" 
Inherits="Cff.SaferTrader.Web.AgreementPage" Title="Cashflow Funding Limited | Debtor Management | Dashboard" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title>Cashflow Funding Limited | Debtor Management</title>
    <link rel="stylesheet" href="~/css/style.css" type="text/css" />
    <% 
        if (Request.ServerVariables["HTTPS"] == "off" || Request.ServerVariables["HTTPS"] == "")
       {%>
            <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
       <% } else { %>
            <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
    <% } %>    
    <script type="text/javascript">
    </script>
</head>
<body>
    <form id="form2" runat="server">
    <center><br />
        <div>
            <table border="0" width="60%" style="background-color: White; padding: 10px;">
                    <h1>Cashflow Funding Limited</h1>
                </div>
               
    </center>
    </form>
</body>
</html>
