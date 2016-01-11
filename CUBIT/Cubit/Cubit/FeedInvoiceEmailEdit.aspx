<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FeedInvoiceEmailEdit.aspx.vb" Inherits="Cubit.FeedInvoiceEmailEdit" %>
<%@ Register assembly="skmDataGrid" namespace="skmDataGrid" tagprefix="cc1" %>
<%@ Register assembly="ScrollingGrid" namespace="AvgControls" tagprefix="avg" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>CUBIT - Edit</title>
    <link href="Cubit.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/calendar.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="AjaxService.asmx" />
        </Services>
    </asp:ScriptManager>
    <div>
        <asp:Panel ID="pnhide" runat="server" BackColor="Transparent" 
            BackImageUrl="~/images/trans.gif" 
            Style="z-index: 512; left: 1024px; position: absolute; top: 13px; height: 507px;" 
            Visible="False" Width="1000px">
        </asp:Panel>
    <asp:Panel ID="pnmain" runat="server" Height="665px" Style="z-index: 103; left: 8px;
            position: absolute; top: 8px" Width="994px" CssClass="nyspanels" 
            TabIndex="-1">
        <asp:Label ID="lbluser" runat="server" CssClass="nysitaliclabel" Style="z-index: 113;
            left: 7px; position: absolute; top: 645px" Width="376px"></asp:Label>
        <asp:Label ID="lbluserid" runat="server" CssClass="nysitaliclabel" Style="z-index: 113;
            left: 7px; position: absolute; top: 645px" Width="376px" Visible="false"></asp:Label>
        <asp:ImageButton ID="btnsave" 
            runat="server" CssClass="imageButton" 
            ImageUrl="~/images/save.gif" Style="z-index: 113;
            left: 822px; position: absolute; top: 638px; height: 24px;" 
            AlternateText="Close" TabIndex="3" />
        <asp:ImageButton ID="btncancel" runat="server" AlternateText="Cancel" 
            CssClass="imageButton" ImageUrl="~/images/cancel_trans.gif" Style="z-index: 113;
            left: 908px; position: absolute; top: 638px; height: 24px;" 
            TabIndex="4" />
        <asp:Panel ID="pnsearch" runat="server" 
            style="z-index: 116; left: 10px; position: absolute; top: 26px; width:972px; right: 8px; height: 606px;" 
            tabindex="-1" CssClass="nyspanelwhite">
            <asp:TextBox ID="txtdataid" runat="server" CssClass="nystextbox" MaxLength="5" 
                Style="z-index: 115; left: 485px; position: absolute; top: 4px; right: 411px; width: 76px; height: 16px;" 
                Visible="false"></asp:TextBox>
            <asp:Label ID="lblgroup3" runat="server" CssClass="nyslabel" Style="z-index: 113;
                left: 15px; position: absolute; top: 20px; width: 93px;">Booking ID:</asp:Label>
            <asp:Label ID="Label10" runat="server" CssClass="nyslabel" Style="z-index: 113;
                left: 268px; position: absolute; top: 20px; width: 93px;">Supplier invoice:</asp:Label>
            <asp:TextBox ID="txtbookingid" runat="server" CssClass="nystextbox" 
                MaxLength="5" ReadOnly="True" 
                Style="z-index: 115; left: 115px; position: absolute; top: 20px; right: 747px; width: 110px;" 
                TabIndex="-1"></asp:TextBox>
             <asp:TextBox ID="txtsupplerinvoice" runat="server" CssClass="nystextbox" 
                MaxLength="200"  
                Style="z-index: 115; left: 365px; position: absolute; top: 20px; right: 497px; width: 110px;" 
                TabIndex="-1"></asp:TextBox>
            <asp:TextBox ID="txtsupplerinvoice_hidden" runat="server" CssClass="nystextbox" 
                MaxLength="5" Visible="false"  
                Style="z-index: 100; left: 365px; position: absolute; top: 20px; right: 497px; width: 10px;" 
                TabIndex="-1"></asp:TextBox>
            
            <asp:Label ID="Label39" runat="server" CssClass="nyslabel" Style="z-index: 113;
            left: 15px; position: absolute; top: 172px; width: 100px;">Booked date:</asp:Label>
            <asp:Label ID="lblfail" runat="server" CssClass="nyslabel" Style="z-index: 113;
            left: 15px; position: absolute; top: 360px; width: 81px;">Fail reason:</asp:Label>
            <asp:TextBox ID="txtfail" runat="server" CssClass="nystextbox" MaxLength="5" 
                ReadOnly="True" 
                Style="z-index: 115; left: 115px; position: absolute; top: 360px; right: 497px; width: 360px; height: 41px;" 
                TabIndex="-1" TextMode="MultiLine"></asp:TextBox>

            
            <asp:Label ID="Label3" runat="server" CssClass="nyslabel" Style="z-index: 113;
            left: 15px; position: absolute; top: 440px; width: 81px; height: 14px;">Venue name:</asp:Label>
            <asp:Label ID="Label4" runat="server" CssClass="nyslabel" Style="z-index: 113;
            left: 15px; position: absolute; top: 470px; width: 108px; height: 15px;">Accounts email:</asp:Label>
            <asp:TextBox ID="txtvenuename" runat="server" CssClass="nystextboxred" 
                MaxLength="200" ReadOnly="True" 
                Style="z-index: 115; left: 115px; position: absolute; top: 440px; right: 134px; width: 723px;" 
                TabIndex="3"></asp:TextBox>
            <asp:TextBox ID="txtoriginalvenuename" runat="server" CssClass="nystextboxred" 
                MaxLength="200" ReadOnly="True" 
                Style="z-index: 115; left: 238px; position: absolute; top: 411px; right: 622px; width: 112px;" 
                TabIndex="3" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtbosscode" runat="server" CssClass="nystextboxred" 
                MaxLength="200" ReadOnly="True" 
                Style="z-index: 115; left: 404px; position: absolute; top: 414px; right: 456px; width: 112px;" 
                TabIndex="3" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtemail" runat="server" CssClass="nystextboxred" 
                MaxLength="100" 
                Style="z-index: 115; left: 115px; position: absolute; top: 470px; right: 134px; width: 723px;" 
                TabIndex="2"></asp:TextBox>
            <asp:Label ID="Label5" runat="server" CssClass="nyslabel" Style="z-index: 113;
            left: 15px; position: absolute; top: 202px; width: 88px;">Arrival date:</asp:Label>
            <asp:TextBox ID="txtarrivaldate" runat="server" CssClass="nystextbox" MaxLength="5" 
                
                
                Style="z-index: 120; left: 115px; position: absolute; top: 200px; right: 750px; width: 120px;" 
                ReadOnly="True" TabIndex="-1"></asp:TextBox>
            <asp:Label ID="Label6" runat="server" CssClass="nyslabel" Style="z-index: 113;
            left: 250px; position: absolute; top: 202px; width: 91px;">Departure date:</asp:Label>
            <asp:TextBox ID="txtdeparturedate" runat="server" CssClass="nystextbox" MaxLength="5" 
                Style="z-index: 115; left: 355px; position: absolute; top: 200px; right: 497px; width: 120px;" 
                ReadOnly="True" TabIndex="-1"></asp:TextBox>
            <asp:Label ID="Label7" runat="server" CssClass="nyslabel" Style="z-index: 113;
            left: 15px; position: absolute; top: 50px; width: 72px; height: 15px;">Traveller</asp:Label>
            <asp:TextBox ID="txttraveller" runat="server" CssClass="nystextbox" MaxLength="5" 
                Style="z-index: 115; left: 115px; position: absolute; top: 50px; right: 497px; width: 360px;" 
                ReadOnly="True" TabIndex="-1"></asp:TextBox>
            <asp:Label ID="Label8" runat="server" CssClass="nyslabel" Style="z-index: 113;
            left: 14px; position: absolute; top: 80px; width: 69px;">Guest PNR:</asp:Label>
            <asp:Label ID="Label9" runat="server" CssClass="nyslabel" Style="z-index: 113;
            left: 15px; position: absolute; top: 110px; width: 93px;">Booking ref:</asp:Label>
            <asp:TextBox ID="txtbookingref" runat="server" CssClass="nystextbox" MaxLength="5" 
                Style="z-index: 115; left: 115px; position: absolute; top: 110px; right: 497px; width: 360px;" 
                ReadOnly="True" TabIndex="-1"></asp:TextBox>
            <asp:TextBox ID="txtclientname" runat="server" CssClass="nystextbox" MaxLength="5" 
                Style="z-index: 115; left: 115px; position: absolute; top: 140px; right: 497px; width: 360px;" 
                ReadOnly="True" TabIndex="-1"></asp:TextBox>
            <asp:Label ID="Label11" runat="server" CssClass="nyslabel" Style="z-index: 113;
            left: 15px; position: absolute; top: 140px; width: 93px; height: 14px;">Client name:</asp:Label>
            <asp:Label ID="Label1" runat="server" CssClass="nyslabel" Style="z-index: 113;
            left: 250px; position: absolute; top: 172px; width: 67px; height: 14px;">Total billed:</asp:Label>
            <asp:TextBox ID="txttotal" runat="server" CssClass="nystextbox" MaxLength="5" 
                
                
                Style="z-index: 115; left: 355px; position: absolute; top: 170px; right: 497px; width: 120px;" 
                ReadOnly="True" TabIndex="-1"></asp:TextBox>
            <asp:TextBox ID="txtguestPNR" runat="server" CssClass="nystextbox" 
                MaxLength="5" 
                
                Style="z-index: 115; left: 115px; position: absolute; top: 80px; right: 497px; width: 360px;" 
                ReadOnly="True" TabIndex="-1"></asp:TextBox>
            <asp:Label ID="Label31" runat="server" CssClass="nyslabel" Style="z-index: 113;
                left: 15px; position: absolute; top: 230px; width: 93px; height: 14px;">Room details:</asp:Label>
            <asp:Label ID="Label2" runat="server" CssClass="nyslabel" Style="z-index: 113;
                left: 15px; position: absolute; top: 260px; width: 93px; height: 14px;">Bookers name:</asp:Label>
            <asp:TextBox ID="txtroomdetails" runat="server" CssClass="nystextbox" MaxLength="5" 
                Style="z-index: 115; left: 115px; position: absolute; top: 230px; right: 497px; width: 360px;" 
                ReadOnly="True" TabIndex="-1"></asp:TextBox>
            <asp:TextBox ID="txtbooker" runat="server" CssClass="nystextbox" MaxLength="5" 
                Style="z-index: 115; left: 115px; position: absolute; top: 260px; right: 497px; width: 360px;" 
                ReadOnly="True" TabIndex="-1"></asp:TextBox>
            <asp:Label ID="Label18" runat="server" CssClass="nyslabel" Style="z-index: 113;
                left: 15px; position: absolute; top: 290px; width: 96px; height: 14px;">Venue 
            details:</asp:Label>
            <asp:Label ID="Label21" runat="server" CssClass="nyslabel" Style="z-index: 113;
                left: 15px; position: absolute; top: 410px; width: 76px; height: 14px;">Venue ref:</asp:Label>
            <asp:TextBox ID="txtVenuereference" runat="server" CssClass="nystextboxred" MaxLength="5" 
                
                
                Style="z-index: 115; left: 115px; position: absolute; top: 410px; right: 737px; width: 110px;" 
                ReadOnly="True" TabIndex="1"></asp:TextBox>
            <asp:Label ID="lbluser1" runat="server" CssClass="nysitaliclabel" Style="z-index: 113;
            left: 712px; position: absolute; top: 419px; width: 210px;">Click &#39;Find&#39; to 
            search VenuesDB for match</asp:Label>
            
            <asp:ImageButton ID="btnfind" runat="server" AlternateText="Save" 
                CssClass="imageButton" ImageUrl="~/images/find.gif" Style="z-index: 113;
            left: 846px; position: absolute; top: 438px; height: 24px;" TabIndex="1" />
            
        <asp:TextBox ID="txtscrolltop" runat="server" CssClass="nystextbox" 
            MaxLength="5" ReadOnly="True" 
            Style="z-index: 114; left: 454px; position: absolute; top: 57px; right: 507px; width: 11px; height: 6px;"></asp:TextBox>
        <asp:TextBox ID="txtscrollleft" runat="server" CssClass="nystextbox" 
            MaxLength="5" ReadOnly="True" 
            Style="z-index: 100; left: 426px; position: absolute; top: 54px; right: 533px; width: 13px; height: 6px;"></asp:TextBox>
        <asp:Panel ID="pnvenue" runat="server" 
            Style="z-index: 213;left: 11px; position: absolute; top: 686px; height: 669px; width: 998px;" 
            BackImageUrl="~/images/trans_grey.gif" Visible="False" TabIndex="-1">
        <asp:Panel ID="pnInnerVenue" runat="server" 
            Style="z-index: 113;left: 11px; position: absolute; top: 360px; height: 264px; width: 974px;" 
                TabIndex="-1" CssClass="nyspanelwhite">
            <asp:Label ID="Label29" runat="server" 
                CssClass="nyslabel" Style="z-index: 113;
            left: 9px; position: absolute; top: 35px; width: 78px;">Searched on:</asp:Label>
            <asp:Label ID="Label28" runat="server" 
                CssClass="nyslabel" Style="z-index: 113;
            left: 9px; position: absolute; top: 10px; width: 78px;">Venue name:</asp:Label>
            <asp:TextBox ID="txtDatavenuename2" runat="server" 
                CssClass="nystextbox" MaxLength="100" 
                
                   Style="z-index: 115; left: 369px; position: absolute; top: 34px; right: 364px; width: 240px;" 
                   TabIndex="14"></asp:TextBox>
            <asp:TextBox ID="txtDatavenuename1" runat="server" CssClass="nystextbox" 
                MaxLength="100"  
                   Style="z-index: 115; left: 89px; position: absolute; top: 34px; right: 644px; width: 240px;" 
                   TabIndex="12"></asp:TextBox>
            <asp:TextBox ID="txtDatavenuenameFind" runat="server" CssClass="nystextbox" 
                MaxLength="5" ReadOnly="True" 
                
                   Style="z-index: 115; left: 89px; position: absolute; top: 9px; right: 364px; width: 520px;" 
                   TabIndex="11"></asp:TextBox>
            <asp:ImageButton ID="btnnovenue" runat="server" 
                AlternateText="No Venue" CssClass="imageButton" 
                   ImageUrl="~/images/novenue.gif" Style="z-index: 113;
            left: 795px; position: absolute; top: 32px; height: 24px;" TabIndex="6" />
               <asp:ImageButton ID="btnfindcancel" runat="server" AlternateText="Cancel" 
                   CssClass="imageButton" ImageUrl="~/images/cancel.gif" Style="z-index: 113;
            left: 881px; position: absolute; top: 32px; height: 24px;" TabIndex="17" />
               <asp:ImageButton ID="btnfind2" runat="server" AlternateText="Find" 
                   CssClass="imageButton" ImageUrl="~/images/find.gif" Style="z-index: 113;
            left: 621px; position: absolute; top: 30px; height: 24px;" TabIndex="15" />
               <asp:Panel ID="Panel2" runat="server" CssClass="nyspanelwhite" Style="z-index: 115;
            left: 10px; position: absolute; top: 62px; height: 188px; width: 949px;">
                   <avg:ScrollingGrid ID="pndata" runat="server" Height="169px" 
                       HorizontalAlign="NotSet" ScrollBars="Auto" Width="949px" TabIndex="-1"><cc1:PrettyDataGrid 
                       ID="grdVenue" runat="server" AllowSorting="True" 
                           AutoGenerateColumns="False" Width="930px" TabIndex="-1" 
                       ><SelectedItemStyle CssClass="gridSelected" ForeColor="Red" /><AlternatingItemStyle CssClass="gridAlternate" /><ItemStyle CssClass="gridItem" /><Columns><asp:BoundColumn DataField="vereference" Visible="False"></asp:BoundColumn><asp:BoundColumn DataField="vename" HeaderText="NAME" 
                                   SortExpression="vename"><headerstyle horizontalalign="Left" width="250px" /><itemstyle horizontalalign="Left" width="250px" /></asp:BoundColumn><asp:BoundColumn DataField="veaddress1" HeaderText="ADDRESS1" 
                                   SortExpression="veaddress1"><headerstyle horizontalalign="Left" width="150px" /><itemstyle horizontalalign="Left" width="150px" /></asp:BoundColumn><asp:BoundColumn DataField="veaddress2" HeaderText="ADDRESS2" 
                                   SortExpression="veaddress2"><headerstyle horizontalalign="Left" width="150px" /><itemstyle horizontalalign="Left" width="150px" /></asp:BoundColumn><asp:BoundColumn DataField="toname" HeaderText="TOWN" 
                                   SortExpression="toname"><headerstyle horizontalalign="Left" width="80px" /><itemstyle horizontalalign="Left" width="80px" /></asp:BoundColumn><asp:BoundColumn DataField="arname" HeaderText="AREA" 
                                   SortExpression="arname"><headerstyle horizontalalign="Left" width="80px" /><itemstyle horizontalalign="Left" width="80px" /></asp:BoundColumn><asp:BoundColumn DataField="vepostcode" HeaderText="POSTCODE" 
                                   SortExpression="vepostcode"><headerstyle horizontalalign="Left" width="60px" /><itemstyle horizontalalign="Left" width="60px" /></asp:BoundColumn><asp:BoundColumn DataField="bosscode" HeaderText="BOSSCODE" 
                                   SortExpression="bosscode"><headerstyle horizontalalign="Left" width="60px" /><itemstyle horizontalalign="Left" width="60px" /></asp:BoundColumn><asp:BoundColumn DataField="veeh" Visible="False"></asp:BoundColumn><asp:BoundColumn DataField="vefb" Visible="False"></asp:BoundColumn><asp:BoundColumn DataField="verh" Visible="False"></asp:BoundColumn><asp:BoundColumn DataField="vedd" Visible="False"></asp:BoundColumn><asp:BoundColumn DataField="veex" Visible="False"></asp:BoundColumn><asp:BoundColumn DataField="vecx" Visible="False"></asp:BoundColumn><asp:BoundColumn 
                           DataField="transient" Visible="False"></asp:BoundColumn>
                           <asp:BoundColumn DataField="transientgroup" Visible="False"></asp:BoundColumn>
                           <asp:BoundColumn DataField="transientdefault" Visible="False"></asp:BoundColumn>
                           </Columns><HeaderStyle CssClass="gridHead" /></cc1:PrettyDataGrid></avg:ScrollingGrid>
               </asp:Panel>
               <asp:Button ID="btnAndOr" runat="server" CssClass="nyslabel" Text="And" Style="z-index: 113;
            left: 335px; position: absolute; top: 32px; width: 27px;" TabIndex="13"/>
        </asp:Panel>  
         </asp:Panel>  
        <asp:TextBox ID="txtvenuedetails" runat="server" CssClass="nystextbox" 
            MaxLength="5" ReadOnly="True" 
            
            Style="z-index: 115; left: 115px; position: absolute; top: 290px; right: 497px; width: 360px; height: 56px;" 
            TextMode="MultiLine" TabIndex="-1"></asp:TextBox>
            <asp:TextBox ID="txtbookeddate" runat="server" CssClass="nystextbox" 
                MaxLength="50" ReadOnly="True" 
                Style="z-index: 120; left: 115px; position: absolute; top: 170px; right: 747px; width: 120px;" 
                TabIndex="-1"></asp:TextBox>
       </asp:Panel>
        <asp:Label ID="lblGroupView" runat="server" CssClass="nyslabel" 
            Font-Bold="True" Style="z-index: 113;
            left: 13px; position: absolute; top: 6px; width: 276px;"></asp:Label>
    </asp:Panel>
         
        <asp:Panel ID="pnsure" runat="server" BackColor="Transparent" 
            BackImageUrl="~/images/discount_dialogV2.gif" Height="665px" 
            Style="z-index: 512; left: 1042px; position: absolute; top: 550px" 
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
         
    </div>
     
    </form>
</body>
</html>
