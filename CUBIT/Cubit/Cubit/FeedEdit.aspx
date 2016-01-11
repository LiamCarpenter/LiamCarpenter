<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FeedEdit.aspx.vb" Inherits="Cubit.FeedEdit" %>

<%@ Register Assembly="skmDataGrid" Namespace="skmDataGrid" TagPrefix="cc1" %>
<%@ Register Assembly="ScrollingGrid" Namespace="AvgControls" TagPrefix="avg" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
        <asp:Panel ID="pnhide" runat="server" BackColor="Transparent" BackImageUrl="~/images/trans.gif"
            Style="z-index: 512; left: 1024px; position: absolute; top: 13px; height: 507px;"
            Visible="False" Width="1000px">
        </asp:Panel>
        <asp:Panel ID="pnmain" runat="server" Height="665px" Style="z-index: 103; left: 8px;
            position: absolute; top: 8px" Width="994px" CssClass="nyspanels" TabIndex="-1">
            <asp:Label ID="lbluserid" runat="server" CssClass="nysitaliclabel" Style="z-index: 113;
                left: 435px; position: absolute; top: 643px; width: 4px;" Visible="False"></asp:Label>
            <asp:Label ID="lbluser" runat="server" CssClass="nysitaliclabel" Style="z-index: 113;
                left: 7px; position: absolute; top: 645px" Width="376px"></asp:Label>
            &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:ImageButton ID="btnsave" runat="server" CssClass="imageButton" ImageUrl="~/images/save.gif"
                Style="z-index: 113; left: 822px; position: absolute; top: 638px; height: 24px;"
                AlternateText="Close" TabIndex="8" />
            <asp:ImageButton ID="btnclose" runat="server" AlternateText="Close" CssClass="imageButton"
                ImageUrl="~/images/closeb.gif" Style="z-index: 113; left: 908px; position: absolute;
                top: 638px; height: 24px;" TabIndex="9" />
            <asp:ImageButton ID="btncancel" runat="server" AlternateText="Cancel" CssClass="imageButton"
                ImageUrl="~/images/cancel_trans.gif" Style="z-index: 113; left: 907px; position: absolute;
                top: 638px; height: 24px;" TabIndex="10" />
            <asp:Panel ID="pnsearch" runat="server" Style="z-index: 116; left: 10px; position: absolute;
                top: 26px; width: 972px; right: 8px; height: 606px;" TabIndex="-1" CssClass="nyspanelwhite">
                <asp:TextBox ID="txtVenuebosscodehidden" runat="server" MaxLength="5" Visible="false"
                    Style="z-index: 115; left: 488px; position: absolute; top: 470px; right: 452px;
                    width: 24px; height: 18px;" TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txtdataid" runat="server" CssClass="nystextbox" MaxLength="5" Style="z-index: 115;
                    left: 485px; position: absolute; top: 4px; right: 395px; width: 92px; height: 16px;"
                    Visible="false"></asp:TextBox>
                <asp:Label ID="lblgroup4" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 247px; position: absolute; top: 20px; width: 112px;">Transaction line no:</asp:Label>
                <asp:Label ID="lblgroup3" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 15px; position: absolute; top: 20px; width: 93px;">Transaction no:</asp:Label>
                <asp:TextBox ID="txttransactionlinenumber" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 365px; position: absolute; top: 20px; right: 497px;
                    width: 110px;" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txttransactionnumber" runat="server" CssClass="nystextbox" MaxLength="5"
                    ReadOnly="True" Style="z-index: 115; left: 115px; position: absolute; top: 20px;
                    right: 747px; width: 110px;" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label39" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 490px;
                    position: absolute; top: 20px; width: 100px;">Booked date:</asp:Label>
                <asp:Label ID="Label1" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 724px;
                    position: absolute; top: 20px; width: 100px;">Transaction date:</asp:Label>
                <asp:TextBox ID="txttransactiondate" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 120; left: 828px; position: absolute; top: 20px; right: 22px;
                    width: 120px;" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label33" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 724px;
                    position: absolute; top: 170px; width: 93px;">Bill instructions:</asp:Label>
                <asp:Label ID="Label32" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 490px;
                    position: absolute; top: 200px; width: 54px;">Comments:</asp:Label>
                <asp:Label ID="lblfail" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 15px;
                    position: absolute; top: 330px; width: 81px;">Fail reason:</asp:Label>
                <asp:Label ID="lblaicol6" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 490px; position: absolute; top: 230px; width: 151px;"></asp:Label>
                <asp:Label ID="lblaicol7" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 490px; position: absolute; top: 260px; width: 151px;"></asp:Label>
                <asp:Label ID="lblaicol8" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 490px; position: absolute; top: 290px; width: 151px;"></asp:Label>
                <asp:Label ID="lblaicol9" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 490px; position: absolute; top: 320px; width: 151px;"></asp:Label>
                <asp:Label ID="lblaicol10" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 490px; position: absolute; top: 350px; width: 151px;"></asp:Label>
                <asp:Label ID="Label2" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 724px;
                    position: absolute; top: 200px; width: 54px;" Visible="False">Dept:</asp:Label>
                <asp:TextBox ID="txtfail" runat="server" CssClass="nystextbox" MaxLength="5" ReadOnly="True"
                    Style="z-index: 115; left: 115px; position: absolute; top: 330px; right: 497px;
                    width: 360px; height: 41px;" TabIndex="-1" TextMode="MultiLine"></asp:TextBox>
                <asp:TextBox ID="txtaicol6" runat="server" CssClass="nystextboxred" MaxLength="50"
                    Style="z-index: 115; left: 641px; position: absolute; top: 230px; right: 25px;
                    width: 306px;" TabIndex="-1"></asp:TextBox>
                <cc2:AutoCompleteExtender ID="extPoCode" runat="server" TargetControlID="txtholder"
                    ServiceMethod="anything">
                </cc2:AutoCompleteExtender>
                <asp:TextBox ID="txttravellerEmail" runat="server" CssClass="nystextboxred" MaxLength="50"
                    Style="z-index: 115; left: 641px; position: absolute; top: 407px; width: 306px;"
                    TabIndex="7"></asp:TextBox>
                <asp:Label ID="lbltravellerEmail" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 490px; position: absolute; top: 407px; width: 151px; height: 14px;">Traveller Email:</asp:Label>
                <asp:TextBox ID="txtaicol7" runat="server" CssClass="nystextboxred" MaxLength="50"
                    Style="z-index: 115; left: 641px; position: absolute; top: 260px; right: 22px;
                    width: 306px;" TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txtaicol8" runat="server" CssClass="nystextboxred" MaxLength="50"
                    Style="z-index: 115; left: 641px; position: absolute; top: 290px; right: 22px;
                    width: 306px;" TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txtaicol9" runat="server" CssClass="nystextboxred" MaxLength="50"
                    Style="z-index: 115; left: 641px; position: absolute; top: 320px; right: 22px;
                    width: 306px;" TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txtaicol10" runat="server" CssClass="nystextboxred" MaxLength="50"
                    Style="z-index: 115; left: 641px; position: absolute; top: 350px; right: 22px;
                    width: 306px;" TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txtdept" runat="server" CssClass="nystextbox" MaxLength="5" ReadOnly="True"
                    Style="z-index: 115; left: 827px; position: absolute; top: 200px; right: 24px;
                    width: 120px;" TabIndex="-1" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtref3" runat="server" CssClass="nystextbox" MaxLength="5" Style="z-index: 115;
                    left: 590px; position: absolute; top: 200px; right: 22px; width: 120px;" ReadOnly="True"
                    TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label3" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 15px;
                    position: absolute; top: 440px; width: 81px; height: 14px;">Venue name:</asp:Label>
                <asp:TextBox ID="txtEX" runat="server" CssClass="nystextbox" MaxLength="5" Style="z-index: 115;
                    left: 193px; position: absolute; top: 470px; right: 757px; width: 16px; height: 19px;"
                    ReadOnly="True" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtgroupname" runat="server" CssClass="nystextbox" MaxLength="5"
                    ReadOnly="True" Style="z-index: 115; left: 74px; position: absolute; top: 410px;
                    right: 881px; width: 17px;" Visible="False" TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txtgroupid" runat="server" CssClass="nystextbox" MaxLength="5" ReadOnly="True"
                    Style="z-index: 115; left: 74px; position: absolute; top: 410px; right: 881px;
                    width: 17px;" TabIndex="-1" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtVenuereferencehidden" runat="server" CssClass="nystextbox" MaxLength="5"
                    ReadOnly="True" Style="z-index: 115; left: 94px; position: absolute; top: 410px;
                    right: 861px; width: 17px;" TabIndex="-1" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtDD" runat="server" CssClass="nystextbox" MaxLength="5" ReadOnly="True"
                    Style="z-index: 115; left: 95px; position: absolute; top: 470px; right: 860px;
                    width: 17px;" TabIndex="-1" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtvenuenamehidden" runat="server" CssClass="nystextbox" MaxLength="5"
                    ReadOnly="True" Style="z-index: 115; left: 955px; position: absolute; top: 470px;
                    right: 5px; width: 6px;" Visible="False" TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txtvenuename" runat="server" CssClass="nystextboxred" MaxLength="200"
                    ReadOnly="True" Style="z-index: 115; left: 115px; position: absolute; top: 440px;
                    right: 134px; width: 723px;" TabIndex="3"></asp:TextBox>
                <asp:Label ID="Label5" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 490px;
                    position: absolute; top: 50px; width: 88px;">Arrival date:</asp:Label>
                <asp:TextBox ID="txtarrivaldate" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 120; left: 590px; position: absolute; top: 50px; right: 22px;
                    width: 120px;" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label6" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 724px;
                    position: absolute; top: 50px; width: 91px;">Departure date:</asp:Label>
                <asp:TextBox ID="txtdeparturedate" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 827px; position: absolute; top: 50px; right: 22px;
                    width: 120px;" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label7" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 15px;
                    position: absolute; top: 50px; width: 72px; height: 15px;">Guest name:</asp:Label>
                <asp:TextBox ID="txtguestname" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 115px; position: absolute; top: 50px; right: 497px;
                    width: 360px;" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label8" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 14px;
                    position: absolute; top: 80px; width: 69px;">Guest PNR:</asp:Label>
                <asp:Label ID="Label9" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 15px;
                    position: absolute; top: 110px; width: 64px;">Category:</asp:Label>
                <asp:TextBox ID="txtCategoryname" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 115px; position: absolute; top: 110px; right: 747px;
                    width: 110px;" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label40" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 247px;
                    position: absolute; top: 200px; width: 108px; height: 13px;">Currency:</asp:Label>
                <asp:Label ID="Label10" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 247px;
                    position: absolute; top: 110px; width: 83px; height: 13px;">Category 
            code:</asp:Label>
                <asp:TextBox ID="txtCategorybosscode" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 365px; position: absolute; top: 110px; right: 253px;
                    width: 110px;" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label11" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 15px;
                    position: absolute; top: 140px; width: 67px; height: 14px;">Item 
            nett:</asp:Label>
                <asp:TextBox ID="txtnettamount" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 115px; position: absolute; top: 140px; right: 782px;
                    width: 75px;" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label30" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 247px;
                    position: absolute; top: 170px; width: 57px;">VAT rate:</asp:Label>
                <asp:Label ID="Label12" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 15px;
                    position: absolute; top: 170px; width: 57px;">Item VAT:</asp:Label>
                <asp:TextBox ID="txtcurrency" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 365px; position: absolute; top: 200px; right: 696px;
                    width: 110px;" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txtvatrate" runat="server" CssClass="nystextbox" MaxLength="5" ReadOnly="True"
                    Style="z-index: 115; left: 365px; position: absolute; top: 170px; right: 601px;
                    width: 110px;" TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txtvatamount" runat="server" CssClass="nystextbox" MaxLength="5"
                    ReadOnly="True" Style="z-index: 115; left: 115px; position: absolute; top: 170px;
                    right: 782px; width: 75px;" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label13" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 15px;
                    position: absolute; top: 200px; width: 85px;">Item gross:</asp:Label>
                <asp:TextBox ID="txttotalamount" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 115px; position: absolute; top: 200px; right: 782px;
                    width: 75px;" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txtguestPNR" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 115px; position: absolute; top: 80px; right: 497px;
                    width: 360px;" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label14" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 490px;
                    position: absolute; top: 170px; width: 86px; height: 14px;">Last 
            modified:</asp:Label>
                <asp:TextBox ID="txtref2" runat="server" CssClass="nystextbox" MaxLength="5" Style="z-index: 115;
                    left: 827px; position: absolute; top: 170px; right: 24px; width: 120px;" ReadOnly="True"
                    TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txtref1" runat="server" CssClass="nystextbox" MaxLength="5" ReadOnly="True"
                    Style="z-index: 115; left: 590px; position: absolute; top: 170px; right: 23px;
                    width: 120px;" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label31" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 15px;
                    position: absolute; top: 230px; width: 93px; height: 14px;">Supplier 
            inv no:</asp:Label>
                <asp:Label ID="Label15" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 490px;
                    position: absolute; top: 110px; width: 93px; height: 14px;">Conferma 
            inv no:</asp:Label>
                <asp:TextBox ID="txtsupplierinvoice" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 115px; position: absolute; top: 230px; right: 497px;
                    width: 360px;" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txtconfermainvoiceno" runat="server" CssClass="nystextbox" MaxLength="5"
                    ReadOnly="True" Style="z-index: 115; left: 590px; position: absolute; top: 110px;
                    right: 23px; width: 120px;" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label16" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 724px;
                    position: absolute; top: 80px; width: 84px; height: 14px;">Bookers name:</asp:Label>
                <asp:TextBox ID="txtbooker" runat="server" CssClass="nystextbox" MaxLength="5" Style="z-index: 115;
                    left: 827px; position: absolute; top: 80px; right: 21px; width: 120px;" ReadOnly="True"
                    TabIndex="-1"></asp:TextBox>
                <asp:Label ID="lblaicostcode" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 490px; position: absolute; top: 380px; width: 151px; height: 14px;"></asp:Label>
                <asp:TextBox ID="txtaicostcode" runat="server" CssClass="nystextboxred" MaxLength="50"
                    Style="z-index: 115; left: 641px; position: absolute; top: 380px; width: 306px;"
                    TabIndex="7"></asp:TextBox>
                <asp:Label ID="Label18" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 15px;
                    position: absolute; top: 260px; width: 96px; height: 14px;">Venue 
            details:</asp:Label>
                <asp:Label ID="Label19" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 724px;
                    position: absolute; top: 110px; width: 87px; height: 14px;">Room 
            details:</asp:Label>
                <asp:TextBox ID="txtroomdetails" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 827px; position: absolute; top: 110px; right: 23px;
                    width: 120px; height: 18px;" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label20" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 490px;
                    position: absolute; top: 80px; width: 88px; height: 14px;">Booker initials:</asp:Label>
                <asp:TextBox ID="txtbookerinitials" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 590px; position: absolute; top: 80px; right: 262px;
                    width: 120px;" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label21" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 15px;
                    position: absolute; top: 410px; width: 76px; height: 14px;">Venue ref:</asp:Label>
                <asp:TextBox ID="txtVenuereference" runat="server" CssClass="nystextboxred" MaxLength="5"
                    Style="z-index: 115; left: 115px; position: absolute; top: 410px; right: 737px;
                    width: 110px;" ReadOnly="True" TabIndex="1"></asp:TextBox>
                <asp:Label ID="Label22" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 266px;
                    position: absolute; top: 470px; width: 95px; height: 14px;" TabIndex="-1">Venue bosscode:</asp:Label>
                <asp:Label ID="lblVenueComm" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 237px; position: absolute; top: 471px; width: 15px; height: 14px;" TabIndex="-1"
                    Font-Bold="True" ForeColor="Red" Visible="False">Comm</asp:Label>
                <asp:TextBox ID="txtVenuebosscode" runat="server" CssClass="nystextboxred" MaxLength="20"
                    Style="z-index: 115; left: 365px; position: absolute; top: 470px; right: 496px;
                    width: 110px;" ReadOnly="True" TabIndex="6"></asp:TextBox>
                <asp:Label ID="Label23" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 15px;
                    position: absolute; top: 470px; width: 75px; height: 14px;">Venue 
            comm:</asp:Label>
                <asp:TextBox ID="txtVenuecommision" runat="server" CssClass="nystextboxred" MaxLength="5"
                    onkeypress="return numbersonly(this, event)" Style="z-index: 115; left: 115px;
                    position: absolute; top: 470px; right: 737px; width: 110px;" TabIndex="5"></asp:TextBox>
                <asp:Label ID="Label24" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 15px;
                    position: absolute; top: 380px; width: 81px; height: 14px;">Trans code:</asp:Label>
                <asp:TextBox ID="txtTransactioncode" runat="server" CssClass="nystextbox" MaxLength="5"
                    ReadOnly="True" Style="z-index: 115; left: 115px; position: absolute; top: 380px;
                    right: 747px; width: 110px;" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="Label27" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 240px;
                    position: absolute; top: 410px; width: 118px; height: 14px;">Editable 
            trans value:</asp:Label>
                <asp:Label ID="Label25" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 240px;
                    position: absolute; top: 380px; width: 124px; height: 14px;">Trans 
            value on import:</asp:Label>
                <asp:Label ID="Label26" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 490px;
                    position: absolute; top: 140px; width: 46px; height: 14px;">Status:</asp:Label>
                <asp:TextBox ID="txtstatusname" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 590px; position: absolute; top: 140px; right: 112px;
                    width: 270px;" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txtTransactionvaluenew" runat="server" CssClass="nystextboxred"
                    MaxLength="5" onkeypress="return numbersonly(this, event)" Style="z-index: 115;
                    left: 365px; position: absolute; top: 410px; right: 584px; width: 23px;" TabIndex="2"></asp:TextBox>
                <asp:TextBox ID="txtTransactionvalue" runat="server" CssClass="nystextbox" MaxLength="5"
                    Style="z-index: 115; left: 365px; position: absolute; top: 380px; right: 584px;
                    width: 23px; bottom: 208px;" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="lbluser0" runat="server" CssClass="nysitaliclabelred" Style="z-index: 113;
                    left: 9px; position: absolute; top: 489px; width: 572px;">Red outlined textboxes 
            are editable/changeable fields, either manually or by selecting a differently 
            named venue</asp:Label>
                <asp:Label ID="lbluser1" runat="server" CssClass="nysitaliclabel" Style="z-index: 113;
                    left: 750px; position: absolute; top: 427px; width: 210px;">Click &#39;Find&#39; to 
            search VenuesDB for match</asp:Label>
                <asp:TextBox ID="txtcostcodehidden" runat="server" CssClass="nystextbox" MaxLength="5"
                    ReadOnly="True" Style="z-index: 115; left: 784px; position: absolute; top: 470px;
                    right: 161px; width: 27px;" Visible="False" TabIndex="-1"></asp:TextBox>
                <asp:Label ID="lbluser2" runat="server" CssClass="nysitaliclabel" Style="z-index: 113;
                    left: 395px; position: absolute; top: 401px; width: 70px; height: 37px; right: 507px;"
                    Font-Size="X-Small">Entering a value here will override Import on Export</asp:Label>
                <asp:ImageButton ID="btnreset" runat="server" AlternateText="Reset" CssClass="imageButton"
                    ImageUrl="~/images/reset.gif" Style="z-index: 113; left: 870px; position: absolute;
                    top: 137px; height: 24px;" TabIndex="4" Visible="False" />
                <asp:ImageButton ID="btnfind" runat="server" AlternateText="Save" CssClass="imageButton"
                    ImageUrl="~/images/find.gif" Style="z-index: 113; left: 846px; position: absolute;
                    top: 438px; height: 24px;" TabIndex="4" Visible="False" />
                <asp:Panel ID="pnboss" runat="server" Style="z-index: 116; left: 9px; position: absolute;
                    top: 505px; width: 716px; right: 243px; height: 92px;" TabIndex="-1" Visible="False"
                    CssClass="nyspanelwhite">
                    <asp:Label ID="Label34" runat="server" CssClass="nyslabel" Font-Underline="True"
                        Style="z-index: 113; left: 6px; position: absolute; top: 6px; width: 151px; height: 14px;
                        right: 159px;">BOSS 
                returned values:</asp:Label>
                    <asp:Label ID="Label37" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 180px;
                        position: absolute; top: 30px; width: 67px; height: 15px;">COMM NETT:</asp:Label>
                    <asp:Label ID="Label35" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 12px;
                        position: absolute; top: 30px; width: 54px; height: 14px; right: 491px;">BOSS 
                   ID:</asp:Label>
                    <asp:Label ID="Label38" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 180px;
                        position: absolute; top: 60px; width: 64px; height: 14px;">COMM VAT:</asp:Label>
                    <asp:Label ID="Label36" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 12px;
                        position: absolute; top: 60px; width: 69px; height: 14px;">BOSS TOTAL:</asp:Label>
                    <asp:TextBox ID="txtbosscomNett" runat="server" CssClass="nystextbox" MaxLength="5"
                        ReadOnly="True" Style="z-index: 115; left: 250px; position: absolute; top: 30px;
                        right: 221px; width: 90px;" TabIndex="-1"></asp:TextBox>
                    <asp:TextBox ID="txtbossid" runat="server" CssClass="nystextbox" MaxLength="5" ReadOnly="True"
                        Style="z-index: 115; left: 80px; position: absolute; top: 30px; right: 383px;
                        width: 90px;" TabIndex="-1"></asp:TextBox>
                    <asp:TextBox ID="txtbosscomVat" runat="server" CssClass="nystextbox" MaxLength="5"
                        ReadOnly="True" Style="z-index: 115; left: 250px; position: absolute; top: 60px;
                        right: 205px; width: 90px;" TabIndex="-1"></asp:TextBox>
                    <asp:TextBox ID="txtbosstotal" runat="server" CssClass="nystextbox" MaxLength="5"
                        ReadOnly="True" Style="z-index: 115; left: 80px; position: absolute; top: 60px;
                        right: 381px; width: 90px;" TabIndex="-1"></asp:TextBox>
                    <asp:Panel ID="pnbossinner" Style="z-index: 115; left: 358px; position: absolute;
                        top: 4px; height: 81px; width: 350px; right: 25px;" runat="server" CssClass="nyspanelwhite">
                        <avg:ScrollingGrid ID="pnBossGrid" runat="server" Height="62px" HorizontalAlign="NotSet"
                            TabIndex="-1" Width="347px">
                            <cc1:PrettyDataGrid ID="grdBoss" runat="server" AllowSorting="False" AutoGenerateColumns="False"
                                Height="16px" TabIndex="-1" Width="310px">
                                <SelectedItemStyle CssClass="gridSelected" ForeColor="Red" />
                                <AlternatingItemStyle CssClass="gridItem" />
                                <ItemStyle CssClass="gridItem" />
                                <HeaderStyle CssClass="gridHead" />
                                <Columns>
                                    <asp:BoundColumn DataField="categoryname" HeaderText="CATEGORY" SortExpression="categoryname">
                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" Width="80px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="dataitemnett" HeaderText="NETT" SortExpression="dataitemnett">
                                        <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="dataitemvat" HeaderText="VAT" SortExpression="dataitemvat">
                                        <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="dataitemgross" HeaderText="TOTAL" SortExpression="dataitemgross">
                                        <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="transactionfee" HeaderText="TRANS FEE" SortExpression="transactionfee">
                                        <HeaderStyle HorizontalAlign="Left" Width="70px" />
                                        <ItemStyle HorizontalAlign="Left" Width="70px" />
                                    </asp:BoundColumn>
                                </Columns>
                            </cc1:PrettyDataGrid></avg:ScrollingGrid>
                    </asp:Panel>
                </asp:Panel>
                <asp:TextBox ID="txtscrolltop" runat="server" CssClass="nystextbox" MaxLength="5"
                    ReadOnly="True" Style="z-index: 115; left: 869px; position: absolute; top: 27px;
                    right: 92px; width: 11px; height: 6px;"></asp:TextBox>
                <asp:TextBox ID="txtscrollleft" runat="server" CssClass="nystextbox" MaxLength="5"
                    ReadOnly="True" Style="z-index: 100; left: 870px; position: absolute; top: 53px;
                    right: 89px; width: 13px; height: 6px;"></asp:TextBox>
                <asp:Button ID="btnshowhidden" runat="server" Text="Show hidden" Style="z-index: 113;
                    left: 721px; position: absolute; top: 610px; height: 24px; width: 84px;" Visible="False"
                    TabIndex="-1" />
                <asp:Panel ID="pnvenue" runat="server" Style="z-index: 213; left: 11px; position: absolute;
                    top: 686px; height: 669px; width: 998px;" BackImageUrl="~/images/trans_grey.gif"
                    Visible="False" TabIndex="-1">
                    <asp:Panel ID="pnInnerVenue" runat="server" Style="z-index: 113; left: 11px; position: absolute;
                        top: 360px; height: 264px; width: 974px;" TabIndex="-1" CssClass="nyspanelwhite">
                        <asp:Label ID="Label29" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 9px;
                            position: absolute; top: 35px; width: 78px;">Searched on:</asp:Label>
                        <asp:Label ID="Label28" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 9px;
                            position: absolute; top: 10px; width: 78px;">Venue name:</asp:Label>
                        <asp:TextBox ID="txtDatavenuename2" runat="server" CssClass="nystextbox" MaxLength="100"
                            Style="z-index: 115; left: 369px; position: absolute; top: 34px; right: 364px;
                            width: 240px;" TabIndex="14"></asp:TextBox>
                        <asp:TextBox ID="txtDatavenuename1" runat="server" CssClass="nystextbox" MaxLength="100"
                            Style="z-index: 115; left: 89px; position: absolute; top: 34px; right: 644px;
                            width: 240px;" TabIndex="12"></asp:TextBox>
                        <asp:TextBox ID="txtDatavenuenameFind" runat="server" CssClass="nystextbox" MaxLength="5"
                            ReadOnly="True" Style="z-index: 115; left: 89px; position: absolute; top: 9px;
                            right: 364px; width: 520px;" TabIndex="11"></asp:TextBox>
                        <asp:ImageButton ID="btnnovenue" runat="server" AlternateText="No Venue" CssClass="imageButton"
                            ImageUrl="~/images/novenue.gif" Style="z-index: 113; left: 795px; position: absolute;
                            top: 32px; height: 24px;" TabIndex="6" />
                        <asp:ImageButton ID="btnfindcancel" runat="server" AlternateText="Cancel" CssClass="imageButton"
                            ImageUrl="~/images/cancel.gif" Style="z-index: 113; left: 881px; position: absolute;
                            top: 32px; height: 24px;" TabIndex="17" />
                        <asp:ImageButton ID="btnfind2" runat="server" AlternateText="Find" CssClass="imageButton"
                            ImageUrl="~/images/find.gif" Style="z-index: 113; left: 621px; position: absolute;
                            top: 30px; height: 24px;" TabIndex="15" />
                        <asp:Panel ID="Panel2" runat="server" CssClass="nyspanelwhite" Style="z-index: 115;
                            left: 10px; position: absolute; top: 62px; height: 188px; width: 949px;">
                            <avg:ScrollingGrid ID="pndata" runat="server" Height="169px" HorizontalAlign="NotSet"
                                ScrollBars="Auto" Width="949px" TabIndex="-1">
                                <cc1:PrettyDataGrid ID="grdVenue" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    Width="930px" TabIndex="-1">
                                    <SelectedItemStyle CssClass="gridSelected" ForeColor="Red" />
                                    <AlternatingItemStyle CssClass="gridAlternate" />
                                    <ItemStyle CssClass="gridItem" />
                                    <Columns>
                                        <asp:BoundColumn DataField="vereference" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="vename" HeaderText="NAME" SortExpression="vename">
                                            <HeaderStyle HorizontalAlign="Left" Width="250px" />
                                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="veaddress1" HeaderText="ADDRESS1" SortExpression="veaddress1">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="veaddress2" HeaderText="ADDRESS2" SortExpression="veaddress2">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="toname" HeaderText="TOWN" SortExpression="toname">
                                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="arname" HeaderText="AREA" SortExpression="arname">
                                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="vepostcode" HeaderText="POSTCODE" SortExpression="vepostcode">
                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                            <ItemStyle HorizontalAlign="Left" Width="60px" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="bosscode" HeaderText="BOSSCODE" SortExpression="bosscode">
                                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                            <ItemStyle HorizontalAlign="Left" Width="60px" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="veeh" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="vefb" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="verh" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="vedd" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="veex" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="vecx" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="transient" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="transientgroup" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="transientdefault" Visible="False"></asp:BoundColumn>
                                    </Columns>
                                    <HeaderStyle CssClass="gridHead" />
                                </cc1:PrettyDataGrid></avg:ScrollingGrid>
                        </asp:Panel>
                        <asp:Button ID="btnAndOr" runat="server" CssClass="nyslabel" Text="And" Style="z-index: 113;
                            left: 335px; position: absolute; top: 32px; width: 27px;" TabIndex="13" />
                    </asp:Panel>
                </asp:Panel>
                <asp:TextBox ID="txtvenuedetails" runat="server" CssClass="nystextbox" MaxLength="5"
                    ReadOnly="True" Style="z-index: 915; left: 115px; position: absolute; top: 260px;
                    right: 497px; width: 360px; height: 56px;" TextMode="MultiLine" TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txtholder" runat="server" CssClass="nystextbox" MaxLength="50" ReadOnly="True"
                    Style="z-index: 112; left: 592px; position: absolute; top: 50px; right: 350px;
                    width: 30px;" TabIndex="-1"></asp:TextBox>
                <asp:TextBox ID="txtbookeddate" runat="server" CssClass="nystextbox" MaxLength="50"
                    ReadOnly="True" Style="z-index: 120; left: 590px; position: absolute; top: 20px;
                    right: 262px; width: 120px;" TabIndex="-1"></asp:TextBox>
                <asp:CheckBox Style="position: absolute; top: 541px; left: 876px;" ID="chkExcludeExport"
                    CssClass="nyscheckbox" runat="server" />
                <asp:Label ID="lblExcludeExport" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 758px; position: absolute; top: 546px; width: 116px; height: 15px;">Exclude From Export:</asp:Label>
                <asp:Label ID="lblComplaints" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 758px; position: absolute; top: 570px; width: 183px; height: 15px;" ForeColor="Red"></asp:Label>
            </asp:Panel>
            <asp:Label ID="lblGroupView" runat="server" CssClass="nyslabel" Font-Bold="True"
                Style="z-index: 113; left: 13px; position: absolute; top: 6px; width: 276px;"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnsure" runat="server" BackColor="Transparent" BackImageUrl="~/images/discount_dialogV2.gif"
            Height="665px" Style="z-index: 512; left: 1396px; position: absolute; top: 558px"
            Visible="False" Width="994px">
            &nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblfeed" runat="server" BackColor="White" CssClass="nyslabelcentre"
                Height="32px" Style="z-index: 111; left: 351px; position: absolute; top: 302px;
                width: 296px;"></asp:Label>
            <asp:ImageButton ID="btnno" runat="server" AlternateText="No" CssClass="imageButton"
                ImageUrl="~/images/no.gif" Style="z-index: 115; left: 508px; position: absolute;
                top: 350px; height: 24px;" TabIndex="14" />
            <asp:ImageButton ID="btnyes" runat="server" AlternateText="Yes" CssClass="imageButton"
                ImageUrl="~/images/yes.gif" Style="z-index: 115; left: 411px; position: absolute;
                top: 350px; height: 24px; right: 504px;" TabIndex="13" />
        </asp:Panel>
    </div>
    </form>
</body>
</html>
