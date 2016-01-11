<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IABoss2.aspx.vb" Inherits="NYSInternalAdmin.IABoss2" %>
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
        <asp:scriptmanager ID="ScriptManager1" runat="server">
   
    </asp:scriptmanager> 
        <asp:Panel ID="pnTrans" runat="server" Style="left: 4px; position: absolute; 
            z-index: 1001; top: 20px; width: 1010px; height: 652px; display: none;" 
            CssClass="nyspanelwhite" BackColor="Transparent" 
             BackImageUrl="~/images/trans_light.gif">
        </asp:Panel> 
        <asp:Panel ID="pnNav" runat="server" Style="left: 6px; position: absolute; 
            z-index: 1000; top: 2000px; width: 398px; height: 520px;" 
            CssClass="nyspanelwhite">
            <asp:ImageButton ID="btnlogout" runat="server" CssClass="btnclasshand" 
                 ImageUrl="~/images/logout_out.gif" 
                  Style="z-index: 5000; left: 310px; position: absolute; top: 486px"  
                 TabIndex="3" BorderStyle="None" />
            <cc1:HoverMenuExtender ID="HoverMenuExtender1" runat="server" 
                 PopupControlID="pnNav" TargetControlID="imHover" OffsetY="-12" 
                OffsetX="-11" PopDelay="1">
            </cc1:HoverMenuExtender> 
            <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
        </asp:Panel> 
        <asp:Label ID="lblBookingDetails" runat="server" 
             CssClass="nyslabelHeaderCentre" Style="z-index: 101; left: 4px; position: absolute;
                top: -2px; width: 1010px;" Text="BOSS Tables Updater - All Users"></asp:Label>
        <asp:Panel ID="pnmain" runat="server" 
            Style="left: 4px; position: absolute; z-index:100; top: 20px; height: 652px; width: 1010px;" 
            CssClass="nyspanels" >
            <asp:TextBox ID="txtresult" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 12px; position: absolute;
                top: 101px; width: 986px; height: 533px;" TabIndex="5" 
                TextMode="MultiLine"></asp:TextBox>
            <asp:TextBox ID="txtInvRef" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 435px; position: absolute;
                top: 46px; width: 108px;" TabIndex="5"></asp:TextBox>
            <asp:Image ID="imHover" runat="server" ImageUrl="~/images/menu.gif" Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" />
            <asp:Button ID="btnInvoice" runat="server" CssClass="btnclasshand" 
                Style="z-index: 109; left: 571px; position: absolute; top: 44px; width: 104px;" 
                Text="Update tables" />
            <asp:Label ID="lblfrom" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 279px;
                position: absolute; top: 48px; width: 149px; right: 582px;" 
                Text="Enter BOSS Invoice no:"></asp:Label>
           
           </asp:Panel>
        
    </div>
    </form>
</body>
</html>
