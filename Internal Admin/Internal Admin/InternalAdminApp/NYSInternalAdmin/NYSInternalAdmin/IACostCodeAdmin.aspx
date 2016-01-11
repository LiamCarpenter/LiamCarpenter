<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IACostCodeAdmin.aspx.vb" Inherits="NYSInternalAdmin.IACostCodeAdmin" %>
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
                document.getElementById(element).innerHTML = "<img style='position: absolute; top: 301px; left: 0px;' src='images/white.gif'><img style='position: absolute; top: 310px; left: 425px;' src='images/ajax-loader.gif'><p style='position:absolute; top: 330px; left:370px; font-family: Verdana; font-size: small; font-weight: bold; font-style: normal; color: #515151; background-color: #FFFFFF;'>Please wait while the files are imported......</p>";
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
     <asp:ScriptManager ID="ScriptManager1" runat="server">
         </asp:ScriptManager>   
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
                 PopupControlID="pnNav" TargetControlID="imHover" OffsetY="-12" OffsetX="-11">
            </cc1:HoverMenuExtender> 
            <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
        </asp:Panel> 
        <asp:Label ID="lblBookingDetails" runat="server" 
             CssClass="nyslabelHeaderCentre" Style="z-index: 101; left: 4px; position: absolute;
                top: -2px; width: 1010px;" Text="Cost Code Admin"></asp:Label>
        <asp:Panel ID="pnmain" runat="server" 
            Style="left: 4px; position: absolute; z-index:100; top: 20px; height: 652px; width: 1010px;" 
            CssClass="nyspanels" >
        <asp:Panel ID="pndwp" runat="server" CssClass="nyspanels" 
             Style="left: 123px; position: absolute; top: 29px; width: 710px; height: 75px; z-index: 10000;">
        <asp:Label ID="lblinvoice" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 13px;
                position: absolute; top: 8px; width: 120px; right: 577px; height: 15px;" 
            Text="DWP file location:"></asp:Label>
        <asp:Label ID="Label1" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 13px;
                position: absolute; top: 34px; width: 120px; right: 577px; height: 15px;" 
            Text="CMEC file location:"></asp:Label>
        <asp:TextBox ID="txtdwp" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 141px; position: absolute;
                top: 8px; width: 448px;" TabIndex="1"></asp:TextBox>
        <asp:TextBox ID="txtcmec" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 141px; position: absolute;
                top: 34px; width: 448px;" TabIndex="1"></asp:TextBox>
        <asp:ImageButton ID="btnDwp" runat="server" CssClass="btnclasshand" 
             ImageUrl="~/images/run_over.gif" 
             Style="z-index: 109; left: 616px; position: absolute; top: 26px; height: 24px;" 
             TabIndex="4" BorderStyle="None" /> 
     </asp:Panel>
            <asp:Image ID="imHover" runat="server" 
                Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif"/>
        </asp:Panel>
    </div>
    </form>
</body>
</html>

