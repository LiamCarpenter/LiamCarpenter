<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IAControl.aspx.vb" Inherits="NYSInternalAdmin.IAControl" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
     <title>NYS Corporate - Management Information</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" media="all" />
        <script language="JavaScript" type="text/javascript">
        function toggleDiv(element) {
            if (document.getElementById(element).style.display === 'none') {
                document.getElementById(element).style.display = 'block';
                document.getElementById(element).innerHTML = "<img style='position: absolute; top: 301px; left: 0px;' src='images/white.gif'><img style='position: absolute; top: 310px; left: 425px;' src='images/ajax-loader.gif'><p style='position:absolute; top: 330px; left:370px; font-family: Verdana; font-size: small; font-weight: bold; font-style: normal; color: #515151; background-color: #FFFFFF;'>Please wait while the report is compiled......</p>";
            }
            else if (document.getElementById(element).style.display === 'block') {
                document.getElementById(element).style.display = 'none';
            }
        }
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        *---------------------------------------------------------------------------------------------<asp:ScriptManager ID="ScriptManager1" runat="server">
         </asp:ScriptManager>   

        <asp:Panel ID="pnTrans" runat="server" Style="left: 4px; position: absolute; 
            z-index: 1001; top: 20px; width: 1010px; height: 652px; display: none;" 
            CssClass="nyspanelwhite" BackColor="Transparent" 
             BackImageUrl="~/images/trans_light.gif">
        </asp:Panel> 
        <asp:Panel ID="pnNav" runat="server" Style="left: 20px; position: absolute; 
            z-index: 1000; top: 40px; width: 398px; height: 520px;" 
            CssClass="nyspanelwhite">
            <asp:ImageButton ID="btnlogout" runat="server" CssClass="btnclasshand" 
                 ImageUrl="~/images/logout_out.gif" 
                 Style="z-index: 5000; left: 310px; position: absolute; top: 486px" 
                 TabIndex="3" BorderStyle="None" />
            <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
        </asp:Panel>  

       <asp:Panel ID="pnmain" runat="server" CssClass="nyspanels" Height="652px" 
        Style="left: 4px; position: absolute; top: 20px" Width="1010px">
                <asp:Label ID="lblMonthTotals" runat="server" CssClass="nysheaderSmLeft" Style="z-index: 101; left: 16px; position: absolute;
                top: 1px; width: 401px;" 
              Text="Click a Report header below to view report list:"></asp:Label>  
          <asp:Label ID="Label6" runat="server" CssClass="nyslabellogonlg" 
              Font-Bold="False" Style="z-index: 103; left: 461px;
                position: absolute; top: 97px; width: 472px;" 
              Text="2. When searching, inputting a greater date range will take the report longer to load."></asp:Label>
          <asp:Label ID="Label5" runat="server" CssClass="nyslabellogonlg" 
              Font-Bold="False" Style="z-index: 103; left: 459px;
                position: absolute; top: 55px; width: 472px;" 
              Text="1. For all Traveller Tracking reports, clearing the Location box will search for All Areas"></asp:Label>
          <asp:Label ID="Label4" runat="server" CssClass="nyslabellogonlg" 
              Font-Bold="False" Style="z-index: 103; left: 457px;
                position: absolute; top: 29px; width: 153px;" Text="Notes:"></asp:Label>
          </asp:Panel>
    </div>
    </form>
</body>
</html>

