<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IAO2Invoice.aspx.vb" Inherits="NYSInternalAdmin.IAO2Invoice" %>
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
                document.getElementById(element).innerHTML = "<img style='position: absolute; top: 301px; left: 0px;' src='images/white.gif'><img style='position: absolute; top: 310px; left: 425px;' src='images/ajax-loader.gif'><p style='position:absolute; top: 330px; left:370px; font-family: Verdana; font-size: small; font-weight: bold; font-style: normal; color: #515151; background-color: #FFFFFF;'>Please wait while the files are built......</p>";
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
                top: -2px; width: 1010px;" Text="O2 Invoice Admin"></asp:Label>
        <asp:Panel ID="pnmain" runat="server" 
            Style="left: 4px; position: absolute; z-index:100; top: 20px; height: 652px; width: 1010px;" 
            CssClass="nyspanels" >
        <asp:Panel ID="pndates" runat="server" CssClass="nyspanels" 
             Style="left: 181px; position: absolute; top: 4px; width: 710px; height: 37px; z-index: 10000;">
        <asp:Label ID="lbltype" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 110px;
                position: absolute; top: 9px; width: 52px; height: 16px;" 
            Text="Status:"></asp:Label>
        <asp:ImageButton ID="btnrefresh" runat="server" CssClass="btnclasshand" 
             ImageUrl="~/images/run_over.gif" 
             Style="z-index: 109; left: 518px; position: absolute; top: 5px; height: 24px;" 
             TabIndex="4" BorderStyle="None" /> 
        <asp:DropDownList ID="ddtype" runat="server" Style="z-index: 99; left: 165px; position: absolute;
                top: 8px; width: 325px;" CssClass="nysdropdown">
            </asp:DropDownList>
     </asp:Panel>
            <asp:Image ID="imHover" runat="server" 
                Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif"/>
            <asp:Label ID="Label1" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 81px;
                position: absolute; top: 46px; width: 52px; height: 16px;" 
                Text="Results:"></asp:Label>
            <asp:Label ID="Label6" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 81px;
                position: absolute; top: 385px; width: 221px; height: 16px;" 
                Text="Invoices waiting to be sent to O2:"></asp:Label>
            <asp:TextBox ID="txtinvoiceref" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 708px; position: absolute;
                top: 80px; width: 219px;" TabIndex="1" ReadOnly="True"></asp:TextBox>
            <asp:TextBox ID="txtpo" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 708px; position: absolute;
                top: 110px; width: 219px;" TabIndex="1" ReadOnly="True"></asp:TextBox>
            <asp:DropDownList ID="ddstatus" runat="server" Style="z-index: 99; left: 708px; position: absolute;
                top: 140px; width: 222px;" CssClass="nysdropdown">
            </asp:DropDownList>
            <asp:TextBox ID="txtdatesent" runat="server" 
                CssClass="nystextbox" Style="z-index: 99; left: 708px; position: absolute;
                top: 170px; width: 219px;" TabIndex="1" ReadOnly="True"></asp:TextBox>
            <asp:Label ID="Label2" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 632px;
                position: absolute; top: 80px; width: 82px; height: 18px;" 
                Text="Invoice ref:"></asp:Label>
            <asp:Label ID="Label3" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 632px;
                position: absolute; top: 110px; width: 82px; height: 18px;" 
                Text="PO:"></asp:Label>
            <asp:Label ID="Label4" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 632px;
                position: absolute; top: 140px; width: 82px; height: 18px;" 
                Text="Status:"></asp:Label>
            <asp:Label ID="Label5" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 632px;
                position: absolute; top: 170px; width: 82px; height: 18px;" 
                Text="Date sent:"></asp:Label>
            <asp:ListBox ID="lstInvoicesGone" runat="server"  Style="z-index: 99; left: 78px; position: absolute;
                top: 61px; width: 537px; height: 316px;" TabIndex="1" 
                CssClass="nysdropdown" AutoPostBack="True" ></asp:ListBox>
            <asp:TextBox ID="txtInvoicesAwaiting" runat="server" 
                CssClass="nystextbox" Style="z-index: 99; left: 78px; position: absolute;
                top: 401px; width: 537px; height: 241px;" TabIndex="1" ReadOnly="True" 
                TextMode="MultiLine"></asp:TextBox>
            <asp:Button ID="btnUpdate" runat="server" CssClass="btnclasshand" 
                Style="z-index: 109; left: 856px; position: absolute; top: 204px; width: 77px;" 
                TabIndex="4" Text="Update" />
        </asp:Panel>
    </div>
    </form>
</body>
</html>

