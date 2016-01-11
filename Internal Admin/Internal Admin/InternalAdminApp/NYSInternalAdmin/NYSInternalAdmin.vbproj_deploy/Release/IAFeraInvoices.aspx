<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IAFeraInvoices.aspx.vb" Inherits="NYSInternalAdmin.IAFeraInvoices" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">



<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">

    <title>NYS Corporate - Management Information</title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" media="all" />

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
        <asp:Panel runat="server" ID="pnlMain"
        Style="left: 4px; position: absolute; z-index:100; top: 20px; height: 652px; width: 1010px;" 
            CssClass="nyspanels">
            <asp:Image ID="imHover" runat="server" 
            Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif"/>
         <asp:Panel runat="server" ID="pnlInputs" 
                style="position:absolute;top:76px; left:8px;" CssClass="nyspanels">
        <asp:Literal runat="server"  ID="ltrOutputString"></asp:Literal>
        <p>
            Date From: <asp:TextBox runat="server" ID="txtDateFrom"></asp:TextBox> to today (<%=FormatDateTime(Date.Now(), DateFormat.ShortDate)%>)
            <br />
            <asp:Button runat="server" ID="btnGetResults" Text="Get Results" />
        </p>
        <p>
            Total Number of BOSS Records: <asp:Literal runat="server" ID="litBossInvoices"></asp:Literal>
            <br />
            Number of BOSS records with matching invoice file: <asp:Literal runat="server" ID="litBossMatching"></asp:Literal>
        </p>
        <p>
            <asp:Label runat="server" AssociatedControlID="txtRecordsMissingFiles" 
                ID="lblRecordsMissingFiles" CssClass="nyslabel">Invoice ID's that are missing files:</asp:Label>
            <br />
            <asp:TextBox runat="server" ID="txtRecordsMissingFiles" TextMode="MultiLine" ReadOnly="true" Columns="50" Rows="20"></asp:TextBox>
        </p>
        </asp:Panel>
            <asp:Label ID="lblBookingDetails" runat="server" CssClass="nyslabelHeaderLeft" Style="z-index: 101; left: 8px; position: absolute;
                top: 46px; width: 213px;" Text="Fera BOSS Invoices"></asp:Label>
        </asp:Panel>
        
        <cc1:CalendarExtender ID="cexFrom" runat="server" TargetControlID="txtDateFrom">
        </cc1:CalendarExtender> 
    </div>
    </form>
</body>
</html>
