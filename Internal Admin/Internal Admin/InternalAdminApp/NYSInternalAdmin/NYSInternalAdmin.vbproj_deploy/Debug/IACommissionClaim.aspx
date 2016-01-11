<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IACommissionClaim.aspx.vb" Inherits="NYSInternalAdmin.IACommissionClaim" %>
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
                top: -2px; width: 1010px;" Text="Commission Claim"></asp:Label>
        <asp:Panel ID="pnmain" runat="server" 
            Style="left: 4px; position: absolute; z-index:100; top: 20px; height: 652px; width: 1010px;" 
            CssClass="nyspanels" >
        <asp:Panel ID="pndates" runat="server" CssClass="nyspanels" 
             
                Style="left: 101px; position: absolute; top: 4px; width: 828px; height: 37px; z-index: 10000;">
            <asp:Label ID="lbltype" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 541px;
                position: absolute; top: 9px; width: 38px; height: 16px;" Text="Type:"></asp:Label>
        <asp:Label ID="lblinvoice" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 13px;
                position: absolute; top: 8px; width: 95px; right: 720px; height: 16px;" 
            Text="Invoice no(s):"></asp:Label>
        <asp:TextBox ID="txtinvoiceno" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 100px; position: absolute;
                top: 8px; width: 424px;" TabIndex="1"></asp:TextBox>
        <asp:ImageButton ID="btnrefresh" runat="server" CssClass="btnclasshand" 
             ImageUrl="~/images/run_over.gif" 
             Style="z-index: 109; left: 733px; position: absolute; top: 5px; height: 24px;" 
             TabIndex="4" BorderStyle="None" /> 
        <asp:DropDownList ID="ddtype" runat="server" Style="z-index: 99; left: 579px; position: absolute;
                top: 8px; width: 138px;" CssClass="nysdropdown">
                <asp:ListItem>Please select</asp:ListItem>
                <asp:ListItem>Initial claim</asp:ListItem>
                <asp:ListItem>First reminder</asp:ListItem>
                <asp:ListItem>Second reminder</asp:ListItem>
                <asp:ListItem>Final reminder</asp:ListItem>
                <asp:ListItem>The law</asp:ListItem>
            </asp:DropDownList>
     </asp:Panel>
            <asp:TextBox ID="txtdate" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 738px; position: absolute;
                top: 80px; width: 110px;" TabIndex="1"></asp:TextBox>
            <asp:Label ID="Label1" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 694px;
                position: absolute; top: 80px; width: 34px; height: 17px;" 
                Text="Date:"></asp:Label>
            <asp:Button ID="btnPrinter" runat="server" CssClass="btnclasshand" 
                Text="Print all files" Style="z-index: 109; left: 865px; position: absolute; top: 80px; width: 81px;" 
                TabIndex="4" />
            <asp:Button ID="btnsearch" runat="server" CssClass="btnclasshand" 
                Text="Search" Style="z-index: 109; left: 361px; position: absolute; top: 312px; width: 81px;" 
                TabIndex="4" />
            <asp:TextBox ID="txtinvoice" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 738px; position: absolute;
                top: 110px; width: 110px;" TabIndex="1"></asp:TextBox>
            <asp:Label ID="Label2" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 658px;
                position: absolute; top: 110px; width: 83px; height: 17px;" 
                Text="Invoice no:"></asp:Label>
            <asp:TextBox ID="txtemail" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 144px; position: absolute;
                top: 244px; width: 282px;" TabIndex="1"></asp:TextBox>
            <asp:Label ID="Label4" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 53px;
                position: absolute; top: 246px; width: 42px; height: 17px;" 
                Text="Email:"></asp:Label>
            <asp:Button ID="btnPrintInvoice" runat="server" CssClass="btnclasshand" 
                Text="Print invoice" Style="z-index: 109; left: 864px; position: absolute; top: 110px; width: 81px;" 
                TabIndex="4" />
            <asp:TextBox ID="txtinvoiceEmail" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 144px; position: absolute;
                top: 217px; width: 796px;" TabIndex="1"></asp:TextBox>
            <asp:TextBox ID="txtsearch" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 111px; position: absolute;
                top: 314px; width: 237px;" TabIndex="1"></asp:TextBox>
            <asp:Label ID="Label3" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 53px;
                position: absolute; top: 217px; width: 92px; height: 17px;" 
                Text="Invoice no(s):"></asp:Label>
            <asp:Label ID="Label5" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 53px;
                position: absolute; top: 314px; width: 83px; height: 17px;" 
                Text="Search:"></asp:Label>
            <asp:Button ID="btnEmail" runat="server" CssClass="btnclasshand" 
                Text="Send invoice to email" Style="z-index: 109; left: 444px; position: absolute; top: 242px; width: 137px;" 
                TabIndex="4" />
            <asp:Image ID="imHover" runat="server" 
                Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif"/>
            <asp:TextBox ID="txtResult" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 53px; position: absolute;
                top: 80px; width: 578px; height: 124px;" TabIndex="1" TextMode="MultiLine"></asp:TextBox>
             <asp:TextBox ID="txtResult2" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 51px; position: absolute;
                top: 351px; width: 891px; height: 283px;" TabIndex="1" 
                TextMode="MultiLine"></asp:TextBox>
        </asp:Panel>
    </div>
    </form>
</body>
</html>

