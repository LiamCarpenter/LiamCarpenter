<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FeedAdmin.aspx.vb" Inherits="Cubit.FeedAdmin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="avg" Assembly="ScrollingGrid" Namespace="AvgControls" %>
<%@ Register Assembly="skmDataGrid" Namespace="skmDataGrid" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CUBIT - Admin</title>
    <link href="Cubit.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/calendar.js"></script>
    <style type="text/css">
        .auto-style1 {
            width: 99%;
            border: 1px solid #000000;
        }
        .auto-style2 {
            font-weight: bold;
        }
        .auto-style3 {
            font-weight: bold;
            width: 117px;
        }
        .auto-style4 {
            width: 117px;
        }
        .auto-style5 {
            height: 15px;
        }
        .auto-style6 {
            width: 117px;
            height: 15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
        <asp:Panel ID="pnmain" runat="server" Height="665px" Style="z-index: 123; left: 8px;
            position: absolute; top: 8px" Width="994px" CssClass="nyspanels">
            <asp:DropDownList ID="ddgroup" runat="server" AutoPostBack="True" CssClass="nysdropdown"
                Style="z-index: 105; left: 83px; position: absolute; top: 32px" TabIndex="1"
                Width="320px">
            </asp:DropDownList>
            <asp:Label ID="lblgroup" runat="server" CssClass="nyslabel" Style="z-index: 113;
                left: 15px; position: absolute; top: 31px; width: 50px; height: 22px;">Group:</asp:Label>
            <asp:Label ID="lblgroup0" runat="server" CssClass="nyslabel" Style="z-index: 113;
                left: 15px; position: absolute; top: 68px; width: 50px; height: 13px;">Parameters</asp:Label>
            <asp:ListBox ID="lstparameters" runat="server" CssClass="nysList" Style="z-index: 105;
                left: 82px; position: absolute; top: 66px; width: 320px; height: 306px;" TabIndex="2"
                AutoPostBack="True"></asp:ListBox>
            &nbsp;&nbsp;&nbsp;
            <asp:Panel ID="pnedit" runat="server" Style="z-index: 116; left: 569px; position: absolute;
                top: 389px; width: 318px; right: 103px; height: 131px;" TabIndex="2" Visible="False"
                CssClass="nyspanelwhite">
                <label class="nyslabel" style="z-index: 115; left: 15px; position: absolute; top: 3px;
                    width: 106px; right: 197px;">
                    Booked Date:
                </label>
                <input id="from" class="nyslink" onclick="displayCalendar(document.forms[0].txtfrom,'dd/mm/yyyy',this,1)"
                    style="z-index: 115; left: 14px; position: absolute; top: 20px; width: 40px;
                    right: 264px;" tabindex="4" type="button" value="From:" />
                <input id="txtfrom" runat="server" class="nystextbox" readonly="readonly" style="z-index: 116;
                    position: absolute; top: 20px; width: 75px; right: 181px; left: 56px;" tabindex="2"
                    type="text" value="" maxlength="10" />
                <input id="to" class="nyslink" onclick="displayCalendar(document.forms[0].txtto,'dd/mm/yyyy',this,1)"
                    style="z-index: 114; left: 150px; position: absolute; top: 21px; width: 24px;
                    height: 18px; right: 144px;" tabindex="5" type="button" value="To:" />
                <input id="txtto" runat="server" class="nystextbox" readonly="readonly" style="z-index: 113;
                    left: 179px; position: absolute; top: 21px; width: 75px" tabindex="4" type="text"
                    value="" maxlength="10" />
                <asp:DropDownList ID="ddtransactions" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                    left: 13px; position: absolute; top: 63px; width: 290px;" TabIndex="6">
                </asp:DropDownList>
                <asp:ImageButton ID="btncancel" runat="server" CssClass="imageButton" ImageUrl="~/images/cancel.gif"
                    Style="z-index: 113; left: 224px; position: absolute; top: 93px; height: 24px;"
                    AlternateText="Cancel" TabIndex="8" />
                <asp:ImageButton ID="btnsave" runat="server" AlternateText="Save" CssClass="imageButton"
                    ImageUrl="~/images/save.gif" Style="z-index: 113; left: 138px; position: absolute;
                    top: 93px; height: 24px; bottom: 14px;" TabIndex="7" />
                <asp:Label ID="lblgroup2" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 15px; position: absolute; top: 48px; width: 115px;">Transaction types:</asp:Label>
            </asp:Panel>
            <asp:Label ID="lbluser" runat="server" CssClass="nysitaliclabel" Style="z-index: 113;
                left: 7px; position: absolute; top: 645px" Width="376px"></asp:Label>
            <asp:Button runat="server" ID="btnMissingInvoices" CssClass="Button" Text="Missing Invoices"
                Style="z-index: 113; position: absolute; left: 633px; top: 636px; width: 94px;" />
            <asp:Button ID="btnCopyFromMevis" runat="server" CssClass="Button" Style="z-index: 113;
                position: absolute; left: 734px; top: 636px; width: 162px;" Text="Copy New Groups From Mevis" />
            <asp:ImageButton ID="btnclose" runat="server" CssClass="imageButton" ImageUrl="~/images/closeb.gif"
                Style="z-index: 113; left: 908px; position: absolute; top: 635px; height: 24px;"
                AlternateText="Close" TabIndex="15" />
               
            <asp:Label ID="lblgroup1" runat="server" CssClass="nyslabel" Style="z-index: 113;
                left: 456px; position: absolute; top: 33px; width: 115px;">Transaction types:</asp:Label>
            <asp:DropDownList ID="ddtransactionsedit" runat="server" AutoPostBack="True" CssClass="nysdropdown"
                Style="z-index: 105; left: 566px; position: absolute; top: 31px; width: 320px;"
                TabIndex="17">
            </asp:DropDownList>
            <asp:ImageButton ID="btntransadd" runat="server" AlternateText="Add" CssClass="imageButton"
                ImageUrl="~/images/add.gif" Style="z-index: 113; left: 806px; position: absolute;
                top: 61px; height: 24px;" TabIndex="19" />
            <asp:Panel ID="pntransactionedit" runat="server" Style="z-index: 116; left: 567px;
                position: absolute; top: 60px; width: 318px; right: 105px; height: 178px;" TabIndex="2"
                Visible="False" CssClass="nyspanelwhite">
                <asp:TextBox ID="txtvalue" runat="server" CssClass="nystextbox" MaxLength="5" onkeypress="return numbersonly(this, event)"
                    Style="z-index: 115; left: 115px; position: absolute; top: 36px; right: 8px;
                    width: 193px;" TabIndex="20"></asp:TextBox>
                <asp:Label ID="lblgroup16" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 8px; position: absolute; top: 62px; width: 106px;">Transaction type:</asp:Label>
                <asp:Label ID="lblgroup17" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 8px; position: absolute; top: 36px; width: 106px;">Transaction value:</asp:Label>
                <asp:Label ID="lblgroup18" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 7px; position: absolute; top: 10px; width: 115px;">Transaction code:</asp:Label>
                <asp:DropDownList ID="ddtype" runat="server" CssClass="nysdropdown" Style="z-index: 115;
                    left: 115px; position: absolute; top: 62px; width: 193px;" TabIndex="21">
                    <asp:ListItem>room</asp:ListItem>
                    <asp:ListItem>meals/beverages</asp:ListItem>
                    <asp:ListItem>all extras</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddcode" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                    left: 115px; position: absolute; top: 10px; width: 193px;" TabIndex="24">
                    <asp:ListItem>online</asp:ListItem>
                    <asp:ListItem>offline</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblInternational" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 8px; position: absolute; top: 90px; width: 106px;">Country:</asp:Label>
                <asp:DropDownList ID="ddCountry" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                    left: 115px; position: absolute; top: 90px; width: 193px;" TabIndex="24">
                    <asp:ListItem>GB</asp:ListItem>
                    <asp:ListItem>Intl</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblNonCom" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 8px; position: absolute; top: 120px; width: 106px;">Non Comm:</asp:Label>
                <asp:DropDownList ID="ddCommissionable" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                    left: 115px; position: absolute; top: 120px; width: 193px;" TabIndex="24">
                    <asp:ListItem>Commissionable</asp:ListItem>
                    <asp:ListItem>None Commissionable</asp:ListItem>
                </asp:DropDownList>
                <asp:ImageButton ID="btntranssave" runat="server" AlternateText="Save" CssClass="imageButton"
                    ImageUrl="~/images/save.gif" Style="z-index: 116; left: 138px; position: absolute;
                    top: 150px; height: 24px;" TabIndex="23" />
                <asp:ImageButton ID="btntranscancel" runat="server" AlternateText="Cancel" CssClass="imageButton"
                    ImageUrl="~/images/cancel.gif" Style="z-index: 116; left: 224px; position: absolute;
                    top: 150px; height: 24px;" TabIndex="22" />
            </asp:Panel>
            <asp:Panel ID="pnoptionsouter" runat="server" Height="661px" Style="z-index: 123;
                left: 8px; position: absolute; top: 700px" Width="990px" BackImageUrl="~/images/trans_grey.gif"
                BorderStyle="None" Visible="False">
                <asp:Panel ID="pnoptions" runat="server" CssClass="nyspanelwhite" Style="z-index: 116;
                    left: 90px; position: absolute; top: 88px; width: 802px; right: 94px; height: 455px;"
                    TabIndex="2">
                    <asp:ImageButton ID="btncanceloptions" runat="server" AlternateText="Cancel" CssClass="imageButton"
                        ImageUrl="~/images/cancel.gif" Style="z-index: 113; left: 705px; position: absolute;
                        top: 416px;" TabIndex="27" />
                    <asp:ImageButton ID="btnsaveoptions" runat="server" AlternateText="Save" CssClass="imageButton"
                        ImageUrl="~/images/save.gif" Style="z-index: 113; left: 613px; position: absolute;
                        top: 416px;" TabIndex="26" />
                    <asp:Label ID="lblgroup8" runat="server" CssClass="nyslabel" Style="z-index: 113;
                        left: 10px; position: absolute; top: 54px; width: 104px;">AICol6 value:</asp:Label>
                    <asp:Label ID="Label2" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 333px;
                        position: absolute; top: 34px; width: 104px;">Cost centre:</asp:Label>
                    <asp:Label ID="Label6" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 629px;
                        position: absolute; top: 13px; width: 151px;" Font-Bold="True">Default values all invoices</asp:Label>
                    <asp:Label ID="lblgroup7" runat="server" CssClass="nyslabel" Style="z-index: 113;
                        left: 10px; position: absolute; top: 74px; width: 101px;">AICol7 value:</asp:Label>
                    <asp:Label ID="Label3" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 333px;
                        position: absolute; top: 54px; width: 101px;">PO:</asp:Label>
                    <asp:Label ID="lblcostcentre" runat="server" CssClass="nyslabel" Style="z-index: 113;
                        left: 10px; position: absolute; top: 34px; width: 102px;">Cost centre value:</asp:Label>
                    <asp:Label ID="Label1" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 416px;
                        position: absolute; top: 13px; width: 105px;" Font-Bold="True">BOSS Mappings</asp:Label>
                    <asp:Label ID="Label21" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 118px;
                        position: absolute; top: 13px; width: 105px;" Font-Bold="True">Label values</asp:Label>
                    <asp:Label ID="lblgroup10" runat="server" CssClass="nyslabel" Style="z-index: 113;
                        left: 10px; position: absolute; top: 300px; width: 115px;">Transaction charge:</asp:Label>
                    <asp:Label ID="Label22" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 338px;
                        position: absolute; top: 325px; width: 86px;">Additional fee:</asp:Label>
                    <asp:Label ID="Label27" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 381px;
                        position: absolute; top: 355px; width: 39px;">CX Fee:</asp:Label>
                    <asp:Label ID="Label12" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 11px;
                        position: absolute; top: 355px; width: 86px;">OOH fee:</asp:Label>
                    <asp:Label ID="Label24" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 11px;
                        position: absolute; top: 384px; width: 86px;">APT fee:</asp:Label>
                    <asp:Label ID="Label13" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 160px;
                        position: absolute; top: 354px; width: 103px;">OOH fee value:</asp:Label>
                    <asp:Label ID="Label25" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 160px;
                        position: absolute; top: 384px; width: 103px;">APT fee value:</asp:Label>
                    <asp:Label ID="Label11" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 511px;
                        position: absolute; top: 350px; width: 64px;">Invoice fee:</asp:Label>
                    <asp:Label ID="Label23" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 460px;
                        position: absolute; top: 325px; width: 118px;">Additional fee value:</asp:Label>
                    <asp:Label ID="Label10" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 634px;
                        position: absolute; top: 326px; width: 163px;" ForeColor="Red">Please ensure the value in this box is INC/EX VAT as per the Transaction Ex VAT check box to the left.</asp:Label>
                    <asp:Label ID="Label14" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 315px;
                        position: absolute; top: 355px; width: 31px;" ForeColor="Red"> + VAT</asp:Label>
                    <asp:Label ID="Label26" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 315px;
                        position: absolute; top: 385px; width: 31px;" ForeColor="Red"> + VAT</asp:Label>
                    <asp:Label ID="Label5" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 333px;
                        position: absolute; top: 95px; width: 115px;">Cref2:</asp:Label>
                    <asp:Label ID="Label7" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 333px;
                        position: absolute; top: 115px; width: 75px;">Cref3:</asp:Label>
                    <asp:Label ID="Label15" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 333px;
                        position: absolute; top: 135px; width: 75px;">Cref4:</asp:Label>
                    <asp:Label ID="Label16" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 333px;
                        position: absolute; top: 155px; width: 75px;">Cref5:</asp:Label>
                    <asp:Label ID="Label17" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 333px;
                        position: absolute; top: 175px; width: 75px;">Cref6:</asp:Label>
                    <asp:Label ID="Label18" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 333px;
                        position: absolute; top: 195px; width: 75px;">Cref7:</asp:Label>
                    <asp:Label ID="Label19" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 333px;
                        position: absolute; top: 215px; width: 75px;">Cref8:</asp:Label>
                    <asp:Label ID="Label20" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 333px;
                        position: absolute; top: 235px; width: 75px;">Cref9:</asp:Label>
                    <asp:Label ID="lblgroup12" runat="server" CssClass="nyslabel" Style="z-index: 113;
                        left: 10px; position: absolute; top: 325px; width: 115px;">Export gross only:</asp:Label>
                    <asp:Label ID="lblgroup13" runat="server" CssClass="nyslabel" Style="z-index: 113;
                        left: 160px; position: absolute; top: 324px; width: 104px; height: 14px;">Transaction Ex VAT:</asp:Label>
                    <asp:Label ID="lblgroup9" runat="server" CssClass="nyslabel" Style="z-index: 113;
                        left: 10px; position: absolute; top: 411px; width: 104px; height: 14px;">Extras charges:</asp:Label>
                    <asp:Label ID="lblgroup5" runat="server" CssClass="nyslabel" Style="z-index: 113;
                        left: 10px; position: absolute; top: 94px; width: 97px;">AICol8 value:</asp:Label>
                    <asp:Label ID="Label8" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 10px;
                        position: absolute; top: 114px; width: 97px;">AICol9 value:</asp:Label>
                    <asp:Label ID="Label9" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 10px;
                        position: absolute; top: 134px; width: 97px;">AICol10 value:</asp:Label>
                    <asp:Label ID="Label4" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 333px;
                        position: absolute; top: 74px; width: 97px;">Cref1:</asp:Label>
                    <asp:TextBox ID="txtoptionsid" runat="server" CssClass="nystextbox" Style="z-index: 115;
                        left: 127px; position: absolute; top: 36px; right: 549px; height: 9px;" TabIndex="-1"
                        Visible="False"></asp:TextBox>
                    <asp:TextBox ID="txtmappingsid" runat="server" CssClass="nystextbox" Style="z-index: 115;
                        left: 127px; position: absolute; top: 36px; right: 549px; height: 9px;" TabIndex="-1"
                        Visible="False"></asp:TextBox>
                    <asp:TextBox ID="txtAICol8value" runat="server" CssClass="nystextbox" MaxLength="50"
                        Style="z-index: 115; left: 118px; position: absolute; top: 93px; right: 9px;
                        width: 193px;" TabIndex="12"></asp:TextBox>
                    <asp:TextBox ID="txtAICol9value" runat="server" CssClass="nystextbox" MaxLength="50"
                        Style="z-index: 115; left: 118px; position: absolute; top: 113px; right: 9px;
                        width: 193px;" TabIndex="12"></asp:TextBox>
                    <asp:TextBox ID="txtAICol10value" runat="server" CssClass="nystextbox" MaxLength="50"
                        Style="z-index: 115; left: 118px; position: absolute; top: 133px; right: 9px;
                        width: 193px;" TabIndex="13"></asp:TextBox>
                    <asp:TextBox ID="txtAICol7value" runat="server" CssClass="nystextbox" MaxLength="50"
                        Style="z-index: 115; left: 118px; position: absolute; top: 73px; right: 10px;
                        width: 193px;" TabIndex="11"></asp:TextBox>
                    <asp:TextBox ID="txtAICol6value" runat="server" CssClass="nystextbox" MaxLength="50"
                        Style="z-index: 115; left: 118px; position: absolute; top: 53px; right: 10px;
                        width: 193px;" TabIndex="10"></asp:TextBox>
                    <asp:TextBox ID="txtccdefault" runat="server" CssClass="nystextbox" MaxLength="200"
                        Style="z-index: 115; left: 628px; position: absolute; top: 33px; right: 15px;
                        width: 144px;" TabIndex="22"></asp:TextBox>
                    <asp:TextBox ID="txtpodefault" runat="server" CssClass="nystextbox" MaxLength="200"
                        Style="z-index: 115; left: 628px; position: absolute; top: 53px; right: 15px;
                        width: 144px;" TabIndex="23"></asp:TextBox>
                    <asp:TextBox ID="txtcref1default" runat="server" CssClass="nystextbox" MaxLength="200"
                        Style="z-index: 115; left: 628px; position: absolute; top: 73px; right: 15px;
                        width: 144px;" TabIndex="24"></asp:TextBox>
                    <asp:TextBox ID="txtcref2default" runat="server" CssClass="nystextbox" MaxLength="200"
                        Style="z-index: 115; left: 628px; position: absolute; top: 93px; right: 15px;
                        width: 144px;" TabIndex="25"></asp:TextBox>
                    <asp:TextBox ID="txtcref3default" runat="server" CssClass="nystextbox" MaxLength="200"
                        Style="z-index: 115; left: 628px; position: absolute; top: 113px; right: 15px;
                        width: 144px;" TabIndex="25"></asp:TextBox>
                    <asp:TextBox ID="txtcref4default" runat="server" CssClass="nystextbox" MaxLength="200"
                        Style="z-index: 115; left: 628px; position: absolute; top: 133px; right: 15px;
                        width: 144px;" TabIndex="25"></asp:TextBox>
                    <asp:TextBox ID="txtcref5default" runat="server" CssClass="nystextbox" MaxLength="200"
                        Style="z-index: 115; left: 628px; position: absolute; top: 153px; right: 15px;
                        width: 144px;" TabIndex="25"></asp:TextBox>
                    <asp:TextBox ID="txtcref6default" runat="server" CssClass="nystextbox" MaxLength="200"
                        Style="z-index: 115; left: 628px; position: absolute; top: 173px; right: 15px;
                        width: 144px;" TabIndex="25"></asp:TextBox>
                    <asp:TextBox ID="txtcref7default" runat="server" CssClass="nystextbox" MaxLength="200"
                        Style="z-index: 115; left: 628px; position: absolute; top: 193px; right: 15px;
                        width: 144px;" TabIndex="25"></asp:TextBox>
                    <asp:TextBox ID="txtcref8default" runat="server" CssClass="nystextbox" MaxLength="200"
                        Style="z-index: 115; left: 628px; position: absolute; top: 213px; right: 15px;
                        width: 144px;" TabIndex="25"></asp:TextBox>
                    <asp:TextBox ID="txtcref9default" runat="server" CssClass="nystextbox" MaxLength="200"
                        Style="z-index: 115; left: 628px; position: absolute; top: 233px; right: 15px;
                        width: 144px;" TabIndex="25"></asp:TextBox>
                    <asp:TextBox ID="txtCostCentreValue" runat="server" CssClass="nystextbox" MaxLength="50"
                        Style="z-index: 115; left: 118px; position: absolute; top: 33px; right: 7px;
                        width: 193px;" TabIndex="9"></asp:TextBox>
                    <asp:DropDownList ID="ddcharge" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                        left: 118px; position: absolute; top: 300px; width: 193px;" TabIndex="13">
                        <asp:ListItem Value="BKG">Per booking</asp:ListItem>
                        <asp:ListItem Value="PP">Per person</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddcc" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                        left: 415px; position: absolute; top: 33px; width: 193px;" TabIndex="17">
                        <asp:ListItem>N/A</asp:ListItem>
                        <asp:ListItem>Costcode</asp:ListItem>
                        <asp:ListItem>Booker</asp:ListItem>
                        <asp:ListItem>AIcol6</asp:ListItem>
                        <asp:ListItem>AIcol7</asp:ListItem>
                        <asp:ListItem>AIcol8</asp:ListItem>
                        <asp:ListItem>AIcol9</asp:ListItem>
                        <asp:ListItem>AIcol10</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddpo" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                        left: 415px; position: absolute; top: 53px; width: 193px;" TabIndex="18">
                        <asp:ListItem>N/A</asp:ListItem>
                        <asp:ListItem>Costcode</asp:ListItem>
                        <asp:ListItem>Booker</asp:ListItem>
                        <asp:ListItem>AIcol6</asp:ListItem>
                        <asp:ListItem>AIcol7</asp:ListItem>
                        <asp:ListItem>AIcol8</asp:ListItem>
                        <asp:ListItem>AIcol9</asp:ListItem>
                        <asp:ListItem>AIcol10</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddcref1" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                        left: 415px; position: absolute; top: 73px; width: 193px;" TabIndex="19">
                        <asp:ListItem>N/A</asp:ListItem>
                        <asp:ListItem>Costcode</asp:ListItem>
                        <asp:ListItem>Booker</asp:ListItem>
                        <asp:ListItem>AIcol6</asp:ListItem>
                        <asp:ListItem>AIcol7</asp:ListItem>
                        <asp:ListItem>AIcol8</asp:ListItem>
                        <asp:ListItem>AIcol9</asp:ListItem>
                        <asp:ListItem>AIcol10</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddcref2" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                        left: 415px; position: absolute; top: 93px; width: 193px;" TabIndex="20">
                        <asp:ListItem>N/A</asp:ListItem>
                        <asp:ListItem>Costcode</asp:ListItem>
                        <asp:ListItem>Booker</asp:ListItem>
                        <asp:ListItem>AIcol6</asp:ListItem>
                        <asp:ListItem>AIcol7</asp:ListItem>
                        <asp:ListItem>AIcol8</asp:ListItem>
                        <asp:ListItem>AIcol9</asp:ListItem>
                        <asp:ListItem>AIcol10</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddcref3" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                        left: 415px; position: absolute; top: 113px; width: 193px;" TabIndex="20">
                        <asp:ListItem>N/A</asp:ListItem>
                        <asp:ListItem>Costcode</asp:ListItem>
                        <asp:ListItem>Booker</asp:ListItem>
                        <asp:ListItem>AIcol6</asp:ListItem>
                        <asp:ListItem>AIcol7</asp:ListItem>
                        <asp:ListItem>AIcol8</asp:ListItem>
                        <asp:ListItem>AIcol9</asp:ListItem>
                        <asp:ListItem>AIcol10</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddcref4" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                        left: 415px; position: absolute; top: 133px; width: 193px;" TabIndex="20">
                        <asp:ListItem>N/A</asp:ListItem>
                        <asp:ListItem>Costcode</asp:ListItem>
                        <asp:ListItem>Booker</asp:ListItem>
                        <asp:ListItem>AIcol6</asp:ListItem>
                        <asp:ListItem>AIcol7</asp:ListItem>
                        <asp:ListItem>AIcol8</asp:ListItem>
                        <asp:ListItem>AIcol9</asp:ListItem>
                        <asp:ListItem>AIcol10</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddcref5" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                        left: 415px; position: absolute; top: 153px; width: 193px;" TabIndex="20">
                        <asp:ListItem>N/A</asp:ListItem>
                        <asp:ListItem>Costcode</asp:ListItem>
                        <asp:ListItem>Booker</asp:ListItem>
                        <asp:ListItem>AIcol6</asp:ListItem>
                        <asp:ListItem>AIcol7</asp:ListItem>
                        <asp:ListItem>AIcol8</asp:ListItem>
                        <asp:ListItem>AIcol9</asp:ListItem>
                        <asp:ListItem>AIcol10</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddcref6" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                        left: 415px; position: absolute; top: 173px; width: 193px;" TabIndex="20">
                        <asp:ListItem>N/A</asp:ListItem>
                        <asp:ListItem>Costcode</asp:ListItem>
                        <asp:ListItem>Booker</asp:ListItem>
                        <asp:ListItem>AIcol6</asp:ListItem>
                        <asp:ListItem>AIcol7</asp:ListItem>
                        <asp:ListItem>AIcol8</asp:ListItem>
                        <asp:ListItem>AIcol9</asp:ListItem>
                        <asp:ListItem>AIcol10</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddcref7" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                        left: 415px; position: absolute; top: 193px; width: 193px;" TabIndex="20">
                        <asp:ListItem>N/A</asp:ListItem>
                        <asp:ListItem>Costcode</asp:ListItem>
                        <asp:ListItem>Booker</asp:ListItem>
                        <asp:ListItem>AIcol6</asp:ListItem>
                        <asp:ListItem>AIcol7</asp:ListItem>
                        <asp:ListItem>AIcol8</asp:ListItem>
                        <asp:ListItem>AIcol9</asp:ListItem>
                        <asp:ListItem>AIcol10</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddcref8" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                        left: 415px; position: absolute; top: 213px; width: 193px;" TabIndex="20">
                        <asp:ListItem>N/A</asp:ListItem>
                        <asp:ListItem>Costcode</asp:ListItem>
                        <asp:ListItem>Booker</asp:ListItem>
                        <asp:ListItem>AIcol6</asp:ListItem>
                        <asp:ListItem>AIcol7</asp:ListItem>
                        <asp:ListItem>AIcol8</asp:ListItem>
                        <asp:ListItem>AIcol9</asp:ListItem>
                        <asp:ListItem>AIcol10</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddcref9" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                        left: 415px; position: absolute; top: 233px; width: 193px;" TabIndex="20">
                        <asp:ListItem>N/A</asp:ListItem>
                        <asp:ListItem>Costcode</asp:ListItem>
                        <asp:ListItem>Booker</asp:ListItem>
                        <asp:ListItem>AIcol6</asp:ListItem>
                        <asp:ListItem>AIcol7</asp:ListItem>
                        <asp:ListItem>AIcol8</asp:ListItem>
                        <asp:ListItem>AIcol9</asp:ListItem>
                        <asp:ListItem>AIcol10</asp:ListItem>
                    </asp:DropDownList>
                    <asp:CheckBox ID="chkexvat" runat="server" CssClass="nyscheckbox" Style="z-index: 105;
                        left: 267px; position: absolute; top: 322px; width: 20px; right: 513px;" TabIndex="15"
                        Text=" " />
                    <asp:CheckBox ID="chkgross" runat="server" CssClass="nyscheckbox" Style="z-index: 105;
                        left: 118px; position: absolute; top: 321px; width: 20px;" TabIndex="14" Text=" " />
                    <asp:CheckBox ID="chkadditional" runat="server" CssClass="nyscheckbox" Style="z-index: 105;
                        left: 428px; position: absolute; top: 321px; width: 20px;" TabIndex="14" Text=" " />
                    <asp:CheckBox ID="chkCX" runat="server" CssClass="nyscheckbox" Style="z-index: 105;
                        left: 428px; position: absolute; top: 351px; width: 20px;" TabIndex="14" Text=" " />
                    <asp:CheckBox ID="chkOOH" runat="server" CssClass="nyscheckbox" Style="z-index: 105;
                        left: 118px; position: absolute; top: 351px; width: 20px;" TabIndex="14" Text=" " />
                    <asp:CheckBox ID="chkAPT" runat="server" CssClass="nyscheckbox" Style="z-index: 105;
                        left: 118px; position: absolute; top: 381px; width: 20px;" TabIndex="14" Text=" " />
                    <asp:RadioButtonList ID="rdextras" runat="server" CssClass="nyslabel" RepeatDirection="Horizontal"
                        Style="z-index: 105; left: 9px; position: absolute; top: 426px; width: 267px;"
                        TabIndex="16">
                        <asp:ListItem Selected="True" Value="0">None</asp:ListItem>
                        <asp:ListItem Value="1">Meals &amp; Beverages</asp:ListItem>
                        <asp:ListItem Value="2">All extras</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:Label ID="lblgroup15" runat="server" CssClass="nyslabel" ForeColor="#33CC33"
                        Style="z-index: 113; left: 338px; position: absolute; top: 300px; width: 68px;
                        height: 14px;">BOSS Code:</asp:Label>
                    <asp:Label ID="lblor" runat="server" CssClass="nyslabel" ForeColor="#33CC33" Style="z-index: 113;
                        left: 594px; position: absolute; top: 300px; width: 68px; height: 14px;">OR:</asp:Label>
                    <asp:DropDownList ID="ddBossCode" runat="server" CssClass="nysdropdown" Style="z-index: 105;
                        left: 626px; position: absolute; top: 300px; width: 150px;" TabIndex="20">
                        <asp:ListItem>N/A</asp:ListItem>
                        <asp:ListItem Value="Costcode">CostCode</asp:ListItem>
                        <asp:ListItem Value="Booker">Booker</asp:ListItem>
                        <asp:ListItem Value="AICol6">AICol6</asp:ListItem>
                        <asp:ListItem Value="AICol7">AICol7</asp:ListItem>
                        <asp:ListItem Value="AICol8">AICol8</asp:ListItem>
                        <asp:ListItem Value="AICol9">AICol9</asp:ListItem>
                        <asp:ListItem Value="AICol10">AICol10</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtBossCode" runat="server" CssClass="nystextbox" MaxLength="50"
                        Style="z-index: 115; left: 428px; position: absolute; top: 300px; right: 225px;
                        width: 150px;" TabIndex="21"></asp:TextBox>
                    <asp:TextBox ID="txtadditionalfee" runat="server" CssClass="nystextbox" MaxLength="50"
                        Style="z-index: 115; left: 582px; position: absolute; top: 325px; right: 175px;
                        width: 39px;" TabIndex="21"></asp:TextBox>
                    <asp:TextBox ID="txtinvoicefee" runat="server" CssClass="nystextbox" MaxLength="50"
                        Style="z-index: 115; left: 582px; position: absolute; top: 350px; right: 175px;
                        width: 39px;" TabIndex="21"></asp:TextBox>
                    <asp:TextBox ID="txtOOHfee" runat="server" CssClass="nystextbox" MaxLength="50" Style="z-index: 115;
                        left: 267px; position: absolute; top: 351px; right: 490px; width: 39px;" TabIndex="21"></asp:TextBox>
                    <asp:TextBox ID="txtAPTFee" runat="server" CssClass="nystextbox" MaxLength="50" Style="z-index: 115;
                        left: 267px; position: absolute; top: 381px; right: 490px; width: 39px;" TabIndex="21"></asp:TextBox>
                    <hr style="z-index: 116; left: 72px; position: absolute; top: 269px; width: 663px;
                        right: 67px; height: 2px;" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnAnchor" runat="server" CssClass="nyspanelwhite" Style="z-index: 116;
                left: 565px; position: absolute; top: 237px; width: 318px; right: 107px; height: 131px;"
                TabIndex="2">
                <asp:ImageButton ID="btnexport" runat="server" AlternateText="Save" CssClass="imageButton"
                    ImageUrl="~/images/export.gif" Style="z-index: 113; left: 225px; position: absolute;
                    top: 97px; bottom: 10px;" TabIndex="7" />
                <asp:Label ID="lblgroup14" runat="server" CssClass="nyslabel" Style="z-index: 113;
                    left: 15px; position: absolute; top: 19px; width: 115px;">Transaction types:</asp:Label>
                <asp:FileUpload ID="flUpload" runat="server" Style="z-index: 105; left: 17px; position: absolute;
                    top: 40px; width: 292px;" CssClass="nysdropdown" />
                <asp:Button ID="btnBookedDownLoad" runat="server" Text="Booked" Style="z-index: 113;
                    left: 119px; position: absolute; top: 97px; bottom: 8px; width: 70px;" TabIndex="7" />
                <asp:Button ID="btnTransDownLoad" runat="server" Text="Trans" Style="z-index: 113;
                    left: 25px; position: absolute; top: 97px; bottom: 8px; width: 70px;" TabIndex="7"
                    Visible="False" />
            </asp:Panel>
            <asp:Panel ID="pnSubClientOptionsOuter" runat="server" Height="661px" Style="z-index: 123;
                left: 8px; position: absolute; top: 1412px" Width="990px" BackImageUrl="~/images/trans_grey.gif"
                BorderStyle="None" Visible="False">
                <asp:TextBox ID="txtscrolltop" runat="server" CssClass="nystextbox" MaxLength="50"
                Style="z-index: 113; left: 85px; position: absolute; top: 32px; height: 1px;
                width: 1px; bottom: 624px;"></asp:TextBox>
            <asp:TextBox ID="txtscrollleft" runat="server" CssClass="nystextbox" MaxLength="50"
                Style="z-index: 113; left: 87px; position: absolute; top: 32px; height: 1px;
                width: 1px;"></asp:TextBox>
                <asp:Panel ID="pnSubClientOptions" runat="server" CssClass="nyspanelwhite" Style="z-index: 116;
                    left: 98px; position: absolute; width: 798px; right: 90px; height: 441px; top: 90px;"
                    TabIndex="2">
                    <avg:ScrollingGrid ID="pndata" runat="server" Width="798px" Height="437px" HorizontalAlign="NotSet">
                        <cc1:PrettyDataGrid ID="grdData" runat="server" AutoGenerateColumns="False" Width="785px"
                            AllowSorting="True" TabIndex="3" RowHighlightColor="" Height="135px">
                            <SelectedItemStyle CssClass="gridSelected" ForeColor="Red" />
                            <AlternatingItemStyle CssClass="gridAlternate" />
                            <ItemStyle CssClass="gridItem" />
                            <Columns>
                                <asp:BoundColumn Visible="False" DataField="SubClientID"></asp:BoundColumn>
                                <asp:BoundColumn DataField="SubClientName" HeaderText="SubClient Name" SortExpression="SubClientName">
                                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                    <ItemStyle Width="200px" HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="SubClientBossCode" HeaderText="SubClient BossCode" SortExpression="SubClientBossCode">
                                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                    <ItemStyle Width="200px" HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="GroupID" HeaderText="Group ID" SortExpression="GroupID"
                                    Visible="false">
                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="SubClientFee" HeaderText="Fee" SortExpression="SubClientFee">
                                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="SubClientActive" HeaderText="Active" SortExpression="SubClientActive">
                                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                    <ItemStyle Width="60px" HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:ButtonColumn CommandName="edit" Text="Edit"></asp:ButtonColumn>
                            </Columns>
                            <HeaderStyle CssClass="gridHead" />
                        </cc1:PrettyDataGrid>
                    </avg:ScrollingGrid>
                    <asp:ImageButton ID="btnSubClientAdd" runat="server" AlternateText="Save" CssClass="imageButton"
                        ImageUrl="~/images/add.gif" Style="z-index: 113; position: absolute; top: 318px;
                        left: 581px;" TabIndex="26" />
                    <asp:ImageButton ID="btnSubClientClose" runat="server" AlternateText="Save" CssClass="imageButton"
                        ImageUrl="~/images/closeb.gif" Style="z-index: 113; position: absolute; top: 318px;
                        left: 683px;" TabIndex="26" />
                    <asp:Panel ID="pnSubClientAdd" runat="server" CssClass="nyspanelwhite" Style="z-index: 116;
                        left: 18px; position: absolute; top: 355px; width: 758px; right: 18px; height: 67px;"
                        TabIndex="2" Visible="false">
                        <asp:TextBox ID="txtSubClientName" runat="server" CssClass="nystextbox" MaxLength="50"
                            Style="z-index: 115; left: 140px; position: absolute; width: 150px; top: 12px;"
                            TabIndex="21"></asp:TextBox>
                        <asp:TextBox ID="txtSubclientBossCode" runat="server" CssClass="nystextbox" MaxLength="50"
                            Style="z-index: 115; left: 140px; position: absolute; width: 150px; top: 43px;"
                            TabIndex="21"></asp:TextBox>
                        <asp:TextBox ID="txtSubClientFee" runat="server" CssClass="nystextbox" MaxLength="50"
                            Style="z-index: 115; left: 435px; position: absolute; width: 55px; top: 12px;"
                            TabIndex="21"></asp:TextBox>
                        <asp:TextBox ID="txtsubclientID" runat="server" CssClass="nystextbox" MaxLength="50"
                            Style="z-index: 115; left: 733px; position: absolute; width: 8px; top: 8px;"
                            TabIndex="21" Visible="false"></asp:TextBox>
                        <asp:Label ID="Label28" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 29px;
                            position: absolute; top: 15px;">Sub-Client Name:</asp:Label>
                        <asp:Label ID="Label29" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 10px;
                            position: absolute; top: 44px;">Sub-Client BossCode:</asp:Label>
                        <asp:Label ID="Label30" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 335px;
                            position: absolute; top: 15px; height: 14px;">Sub-Client Fee:</asp:Label>
                        <asp:Label ID="Label32" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 504px;
                            position: absolute; top: 14px; width: 31px;" ForeColor="Red"> + VAT</asp:Label>
                        <asp:Label ID="Label31" runat="server" CssClass="nyslabel" Style="z-index: 113; left: 323px;
                            position: absolute; top: 44px;">Sub-Client Active:</asp:Label>
                        <asp:CheckBox ID="chkSubClientActive" runat="server" CssClass="nyscheckbox" Style="z-index: 105;
                            left: 436px; position: absolute; width: 20px; top: 39px;" TabIndex="14" Text=" " />
                        <asp:ImageButton ID="btnSubClientSave" runat="server" AlternateText="Save" CssClass="imageButton"
                            ImageUrl="~/images/save.gif" Style="z-index: 113; position: absolute; top: 35px;
                            left: 561px;" TabIndex="26" />
                        <asp:ImageButton ID="btnSubClientCancel" runat="server" AlternateText="Save" CssClass="imageButton"
                            ImageUrl="~/images/cancel.gif" Style="z-index: 113; position: absolute; top: 35px;
                            left: 663px;" TabIndex="26" />
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            
            <asp:Panel ID="pnFilters" runat="server" CssClass="nyspanelwhite" Style="z-index: 116;
                left: 22px; position: absolute; top: 390px; width: 516px; right: 452px; height: 224px;" TabIndex="2">
                <table cellspacing="1" class="auto-style1">
                    <tr>
                        <td class="auto-style2">Active</td>
                        <td class="auto-style2">Booking Type</td>
                        <td class="auto-style2">On or Off</td>
                        <td class="auto-style2">Country</td>
                        <td class="auto-style3">Comm</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="FiltersActiveList" runat="server" Height="106px">
                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                <asp:ListItem Value="Inactive">Inactive</asp:ListItem>
                                <asp:ListItem Value="All" Selected="True">All</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="FiltersBookingType" runat="server" Height="106px">
                                <asp:ListItem>Room</asp:ListItem>
                                <asp:ListItem>Food/Beverages</asp:ListItem>
                                <asp:ListItem>Extras</asp:ListItem>
                                <asp:ListItem Selected="True">All</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="FilterOnlineOffline" runat="server" Height="106px">
                                <asp:ListItem>Online</asp:ListItem>
                                <asp:ListItem>Offline</asp:ListItem>
                                <asp:ListItem Selected="True">All</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="FiltersCountry" runat="server" Height="106px">
                                <asp:ListItem>GB</asp:ListItem>
                                <asp:ListItem>International</asp:ListItem>
                                <asp:ListItem Selected="True">All</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="auto-style4">
                            <asp:RadioButtonList ID="FiltersCommission" runat="server" Height="106px">
                                <asp:ListItem>Comm</asp:ListItem>
                                <asp:ListItem>NonComm</asp:ListItem>
                                <asp:ListItem Selected="True">All</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style5">&nbsp;</td>
                        <td class="auto-style5"></td>
                        <td class="auto-style5"></td>
                        <td class="auto-style5"></td>
                        <td class="auto-style6">
                            
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnFilter" runat="server" Text="Filter" Height="33px" Width="71px" />

                <br />
                <asp:Label ID="lblSelectedFee" runat="server" CssClass="nyslabel" ForeColor="#003300"></asp:Label>

            </asp:Panel>
            
            <asp:ImageButton ID="btnadd" runat="server" AlternateText="Add" CssClass="imageButton" ImageUrl="~/images/add.gif" Style="z-index: 113; left: 408px; position: absolute; top: 345px; height: 24px;" TabIndex="3" />
            
        </asp:Panel>
    </div>
    </form>
</body>
</html>
