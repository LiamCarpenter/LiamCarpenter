<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FeedInvoiceEmail.aspx.vb" Inherits="Cubit.FeedInvoiceEmail" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="avg" Assembly="ScrollingGrid" Namespace="AvgControls" %>
<%@ Register assembly="skmDataGrid" namespace="skmDataGrid" tagprefix="cc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>CUBIT - View</title>
    <link href="Cubit.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="ScrollingGrid.js"></script>
	
</head>
<body>
    <form id="form1" runat="server">
    <div id="mainer">
  <asp:scriptmanager ID="Scriptmanager1" runat="server">
    <Services>
        <asp:ServiceReference Path="AjaxService.asmx" />
    </Services>
  </asp:scriptmanager>
  
         <asp:Panel ID="pnmain" runat="server" Height="665px" Style="z-index: 499; left: 8px;
            position: absolute; top: 8px" Width="994px" CssClass="nyspanels">
        <asp:Label ID="lblcount" runat="server" CssClass="nysitaliclabel" 
            Style="z-index: 113; left: 7px; position: absolute; top: 629px" 
            Width="376px"></asp:Label>
        <asp:Label ID="lbluserid" runat="server" CssClass="nysitaliclabel" 
            Style="z-index: 113; left: 7px; position: absolute; top: 645px" Width="376px" Visible="false"></asp:Label>
        <asp:Label ID="lbluser" runat="server" CssClass="nysitaliclabel" 
            Style="z-index: 113; left: 7px; position: absolute; top: 645px" Width="376px"></asp:Label>
         <asp:Panel ID="Panel2" runat="server" CssClass="nyspanelwhite" Style="z-index: 500;
            left: 4px; position: absolute; top: 23px; height: 549px;" Width="982px">
                    <avg:ScrollingGrid ID="pndata" runat="server" Width="982px" Height="509px" 
                            HorizontalAlign="NotSet" ScrollBars="Auto" Visible="False" >
                            <cc1:PrettyDataGrid ID="grdData" runat="server" AutoGenerateColumns="False"
                            Width="970px" AllowSorting="True" TabIndex="3">
                                <SelectedItemStyle CssClass="gridSelected" ForeColor="Red" />
                                <AlternatingItemStyle CssClass="gridAlternate" />
                                <ItemStyle CssClass="gridItem" />
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="Dataid"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="bookingid" HeaderText="BOOKING ID" SortExpression="bookingid">
                                            <headerstyle horizontalalign="Left" width="60px" />
                                            <itemstyle width="60px" horizontalalign="Left" /></asp:BoundColumn>
                                        <asp:BoundColumn DataField="bookeddate" HeaderText="BOOKED DATE" SortExpression="bookeddate" DataFormatString="{0:dd-MM-yyyy}">
                                            <headerstyle horizontalalign="Left" width="80px" />
                                            <itemstyle width="80px" horizontalalign="Left" /></asp:BoundColumn>
                                        <asp:BoundColumn DataField="arrivaldate" HeaderText="ARRIVAL DATE" SortExpression="arrivaldate" DataFormatString="{0:dd-MM-yyyy}">
                                            <headerstyle horizontalalign="Left" width="80px" />
                                            <itemstyle width="80px" horizontalalign="Left" /></asp:BoundColumn>
                                        <asp:BoundColumn DataField="departuredate" HeaderText="DEPT DATE" SortExpression="departuredate" DataFormatString="{0:dd-MM-yyyy}">
                                            <headerstyle horizontalalign="Left" width="80px" />
                                            <itemstyle width="80px" horizontalalign="Left" /></asp:BoundColumn>
                                        <asp:BoundColumn DataField="LeadPassengerName" HeaderText="TRAVELLER" SortExpression="LeadPassengerName">
                                            <headerstyle horizontalalign="Left" width="82px" />
                                            <itemstyle width="82px" horizontalalign="Left" /></asp:BoundColumn>
                                        <asp:BoundColumn DataField="BookingReference" HeaderText="BOOKING REF" SortExpression="BookingReference">
                                            <headerstyle horizontalalign="Left" width="82px" />
                                            <itemstyle width="82px" horizontalalign="Left" /></asp:BoundColumn>
                                        <asp:BoundColumn DataField="TotalBilledInGbp" HeaderText="TOTAL BILLED" SortExpression="TotalBilledInGbp"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="SupplierName" HeaderText="HOTEL" SortExpression="SupplierName"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="HotelDetails" HeaderText="HOTEL DETAILS" SortExpression="HotelDetails">
                                            <headerstyle width="82px" horizontalalign="Left" />
                                            <itemstyle horizontalalign="Left" width="82px" /></asp:BoundColumn>
                                        <asp:BoundColumn DataField="RoomDetails" HeaderText="ROOM DETAILS" SortExpression="RoomDetails">
                                            <itemstyle width="150px" horizontalalign="Left" />
                                            <headerstyle width="150px" horizontalalign="Left" /></asp:BoundColumn>
                                        <asp:BoundColumn DataField="groupname" HeaderText="CLIENT" SortExpression="groupname">
                                            <headerstyle horizontalalign="Left" width="28px" />
                                            <itemstyle width="82px" horizontalalign="Left" /></asp:BoundColumn>
                                        <asp:BoundColumn DataField="AIBkrName" HeaderText="BOOKERS NAME" SortExpression="AIBkrName">
                                            <itemstyle width="82px" horizontalalign="Left" />
                                            <headerstyle horizontalalign="Left" width="82px" /></asp:BoundColumn>
                                        <asp:BoundColumn DataField="guestPNR" HeaderText="GUEST PNR" SortExpression="guestPNR">
                                            <headerstyle horizontalalign="Left" width="82px" />
                                            <itemstyle width="82px" horizontalalign="Left" /></asp:BoundColumn>
                                        <asp:BoundColumn DataField="unabletosendreason" HeaderText="REASON" SortExpression="unabletosendreason">
                                            <headerstyle width="82px" horizontalalign="Left" />
                                            <itemstyle horizontalalign="Left" width="82px" /></asp:BoundColumn>
                                    </Columns>
                                <HeaderStyle CssClass="gridHead" />
                            </cc1:PrettyDataGrid>
                        </avg:ScrollingGrid>
                    </asp:Panel>
        <asp:Label ID="lblGroupView" runat="server" CssClass="nyslabel" Style="z-index: 113;
            left: 7px; position: absolute; top: 6px; width: 254px;" Font-Bold="True">Unsent emails</asp:Label>
        <asp:TextBox ID="txtscrolltop" runat="server" CssClass="nystextbox" 
            MaxLength="50" Style="z-index: 113;
            left: 21px; position: absolute; top: 51px; height: 4px; width: 2px;"></asp:TextBox>
        <asp:TextBox ID="txtscrollleft" runat="server" CssClass="nystextbox" 
            MaxLength="50" Style="z-index: 113;
            left: 19px; position: absolute; top: 49px; height: 2px; width: 1px;"></asp:TextBox>
        <asp:Panel ID="pnsure" runat="server" BackColor="Transparent" 
            BackImageUrl="~/images/discount_dialogV2.gif" Height="665px" 
            Style="z-index: 512; left: 2020px; position: absolute; top: 16px" 
            Visible="False" Width="994px">
            &nbsp;&nbsp;&nbsp;<asp:Label ID="lblfeed" runat="server" BackColor="White" 
                CssClass="nyslabelcentre" Height="32px" 
                Style="z-index: 111; left: 351px; position: absolute; top: 302px; width: 296px;"></asp:Label>
            <asp:ImageButton ID="btnno" runat="server" AlternateText="No" 
                CssClass="imageButton" ImageUrl="~/images/no.gif" Style="z-index: 115;
            left: 508px; position: absolute; top: 350px; height: 24px;" TabIndex="14" />
            <asp:ImageButton ID="btnyes" runat="server" AlternateText="Yes" 
                CssClass="imageButton" ImageUrl="~/images/yes.gif" Style="z-index: 115;
            left: 411px; position: absolute; top: 350px; height: 24px; right: 504px;" 
                TabIndex="13" />
        </asp:Panel>
             <asp:Button ID="btnCheckAndSend" runat="server" Style="z-index: 115;
            left: 690px; position: absolute; top: 635px; height: 24px; width: 108px;" 
                 Text="Check and Send" />
             <asp:ImageButton ID="btnclose" runat="server" AlternateText="Close" 
                 CssClass="imageButton" ImageUrl="~/images/closeb.gif" Style="z-index: 113;
            left: 908px; position: absolute; top: 635px; height: 24px;" TabIndex="15" />
             <asp:Button ID="btnSendUnsent" runat="server" Style="z-index: 115;
            left: 808px; position: absolute; top: 635px; height: 24px; width: 92px;" 
                 Text="Send unsent" />
        </asp:Panel>
    
    </div>
    </form>
</body>
</html>
