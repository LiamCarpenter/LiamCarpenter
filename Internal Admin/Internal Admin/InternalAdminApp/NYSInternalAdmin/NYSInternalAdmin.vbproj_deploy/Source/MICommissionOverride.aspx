<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MICommissionOverride.aspx.vb" Inherits="NYSInternalAdmin.MICommissionOverride" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
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
                top: -2px; width: 1010px;" Text="Commission Override"></asp:Label>
       
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
            TypeName="NYSInternalAdmin.dsCommissionOverrideTableAdapters.BossCommissionOverride_ListTableAdapter">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtDue" Name="due" PropertyName="Text" 
                    Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:Panel ID="pnmain" runat="server" 
            Style="left: 4px; position: absolute; z-index:100; top: 20px; height: 652px; width: 1010px;" 
            CssClass="nyspanels" >
         <rsweb:ReportViewer ID="rvDue" runat="server" BackColor="White" Font-Names="Verdana" 
             Font-Size="8pt"
             ShowPageNavigationControls="False" ShowZoomControl="False" 
             Style="left: 26px; position: absolute; z-index:100; top: 71px; height: 557px; width: 956px;"
             ToolBarItemHoverBackColor="Silver" CssClass="reportView" 
             DocumentMapWidth="100%" InternalBorderStyle="None" 
             LinkDisabledColor="LightGray" ToolBarItemBorderStyle="None" 
                LinkActiveColor="Blue" LinkActiveHoverColor="DeepPink" 
                ShowBackButton="False" InteractiveDeviceInfos="(Collection)" 
                WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="Reports\repCommissionOverride.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                        Name="dsCommissionOverride" />
                </DataSources>
            </LocalReport>
            
        </rsweb:ReportViewer>
        <asp:Panel ID="pndates" runat="server" CssClass="nyspanels" 
             Style="left: 335px; position: absolute; top: 4px; width: 317px; height: 37px; z-index: 10000;">
        <asp:Label ID="lblinvoice" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 13px;
                position: absolute; top: 8px; width: 75px; right: 564px; height: 16px;" 
            Text="Invoice no:"></asp:Label>
        <asp:TextBox ID="txtinvoiceno" runat="server" CssClass="nystextbox" Style="z-index: 299; left: 94px; position: absolute;
                top: 8px; width: 110px;" TabIndex="1"></asp:TextBox>
        <asp:TextBox ID="txtDue" runat="server" CssClass="nystextbox" Style="z-index: 100; left: 97px; position: absolute;
                top: 8px; width: 1px;" TabIndex="1"></asp:TextBox>
        <asp:ImageButton ID="btnrefresh" runat="server" CssClass="btnclasshand" 
             ImageUrl="~/images/run_over.gif" 
             Style="z-index: 109; left: 225px; position: absolute; top: 5px; height: 24px;" 
             TabIndex="4" BorderStyle="None" /> 
        </asp:Panel>
        <asp:Panel ID="Panel1" runat="server" CssClass="nyspanels" 
             
                Style="left: 10px; position: absolute; top: 61px; width: 990px; height: 579px; z-index: 10000;">
        <asp:Panel ID="pnBoss" runat="server" CssClass="nyspanels" 
             Style="left: 115px; position: absolute; top: 90px; width: 749px; height: 88px; z-index: 10000;">
             <asp:Label ID="Label6" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 11px;
                position: absolute; top: 12px; width: 113px; height: 17px;" 
                 Text="Commission nett:"></asp:Label>
             <asp:Label ID="Label1" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 11px;
                position: absolute; top: 37px; width: 113px; height: 17px;" 
                 Text="Commission vat:"></asp:Label>
             <asp:TextBox ID="txtBossCommNett" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 130px; position: absolute;
                top: 10px; width: 110px;" TabIndex="-1" ReadOnly="True"></asp:TextBox>
             <asp:Label ID="Label7" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 12px;
                position: absolute; top: 62px; width: 113px; height: 17px;" 
                 Text="Commission total:"></asp:Label>
             <asp:TextBox ID="txtBossCommVat" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 130px; position: absolute;
                top: 35px; width: 110px;" TabIndex="-1" ReadOnly="True"></asp:TextBox>
             <asp:Label ID="Label9" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 267px;
                position: absolute; top: 24px; width: 119px; height: 17px;" 
                 Text="Commission recvd:"></asp:Label>
             <asp:Label ID="Label10" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 267px;
                position: absolute; top: 49px; width: 121px; height: 17px;" 
                 Text="Commission due:"></asp:Label>
             <asp:Label ID="Label11" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 527px;
                position: absolute; top: 24px; width: 83px; height: 17px;" 
                 Text="Pay net:"></asp:Label>
             <asp:TextBox ID="txtBossCommRec" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 385px; position: absolute;
                top: 22px; width: 110px;" TabIndex="-1" ReadOnly="True"></asp:TextBox>
             <asp:TextBox ID="txtBossCommDue" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 385px; position: absolute;
                top: 47px; width: 110px;" TabIndex="-1" ReadOnly="True"></asp:TextBox>
             <asp:TextBox ID="txtBossPay" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 619px; position: absolute;
                top: 22px; width: 110px;" TabIndex="-1" ReadOnly="True"></asp:TextBox>
             <asp:Label ID="Label12" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 527px;
                position: absolute; top: 49px; width: 83px; height: 17px;" 
                 Text="Invoice total:"></asp:Label>
             <asp:TextBox ID="txtBossTot" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 619px; position: absolute;
                top: 47px; width: 110px;" TabIndex="-1" ReadOnly="True"></asp:TextBox>
             <asp:TextBox ID="txtBossCommTot" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 130px; position: absolute;
                top: 60px; width: 110px;" TabIndex="-1" ReadOnly="True"></asp:TextBox>
        </asp:Panel>
        <asp:Panel ID="pnSql" runat="server" CssClass="nyspanels" 
             Style="left: 115px; position: absolute; top: 226px; width: 749px; height: 88px; z-index: 10000;">
             <asp:Label ID="Label2" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 11px;
                position: absolute; top: 12px; width: 113px; height: 17px;" 
                 Text="Commission nett:"></asp:Label>
             <asp:Label ID="Label3" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 11px;
                position: absolute; top: 37px; width: 113px; height: 17px;" 
                 Text="Commission vat:"></asp:Label>
             <asp:TextBox ID="txtSqlCommNet" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 130px; position: absolute;
                top: 10px; width: 110px;" TabIndex="-1" ReadOnly="True"></asp:TextBox>
             <asp:Label ID="Label13" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 12px;
                position: absolute; top: 62px; width: 113px; height: 17px;" 
                 Text="Commission total:"></asp:Label>
             <asp:TextBox ID="txtSqlCommVat" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 130px; position: absolute;
                top: 35px; width: 110px;" TabIndex="-1" ReadOnly="True"></asp:TextBox>
             <asp:Label ID="Label14" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 267px;
                position: absolute; top: 24px; width: 119px; height: 17px;" 
                 Text="Commission recvd:"></asp:Label>
             <asp:Label ID="Label15" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 267px;
                position: absolute; top: 49px; width: 121px; height: 17px;" 
                 Text="Commission due:"></asp:Label>
             <asp:Label ID="Label16" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 527px;
                position: absolute; top: 24px; width: 83px; height: 17px;" 
                 Text="Pay net:"></asp:Label>
             <asp:TextBox ID="txtSqlCommRec" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 385px; position: absolute;
                top: 22px; width: 110px;" TabIndex="-1" ReadOnly="True"></asp:TextBox>
             <asp:TextBox ID="txtSqlCommDue" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 385px; position: absolute;
                top: 47px; width: 110px;" TabIndex="-1" ReadOnly="True"></asp:TextBox>
             <asp:TextBox ID="txtSqlPay" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 619px; position: absolute;
                top: 22px; width: 110px;" TabIndex="-1" ReadOnly="True"></asp:TextBox>
             <asp:Label ID="Label17" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 527px;
                position: absolute; top: 49px; width: 83px; height: 17px;" 
                 Text="Invoice total:"></asp:Label>
             <asp:TextBox ID="txtSqlTot" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 619px; position: absolute;
                top: 47px; width: 110px;" TabIndex="-1" ReadOnly="True"></asp:TextBox>
             <asp:TextBox ID="txtSqlCommTot" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 130px; position: absolute;
                top: 60px; width: 110px;" TabIndex="-1" ReadOnly="True"></asp:TextBox>
        </asp:Panel>
        <asp:Panel ID="pnOverride" runat="server" CssClass="nyspanels" 
             
                Style="left: 115px; position: absolute; top: 361px; width: 749px; height: 114px; z-index: 10000;">
              <cc1:ConfirmButtonExtender runat="server" ID="cbedelete"
                TargetControlID="btnDelete"
                ConfirmText="Are you sure you would like to delete the Override values?"></cc1:ConfirmButtonExtender>
              <asp:TextBox ID="txtCurrency" runat="server" CssClass="nystextbox" 
                  MaxLength="3" 
                  Style="z-index: 99; left: 130px; position: absolute; top: 85px; width: 53px;" 
                  TabIndex="4"></asp:TextBox>
             <asp:Label ID="Label18" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 11px;
                position: absolute; top: 12px; width: 113px; height: 17px;" 
                 Text="Commission nett:"></asp:Label>
             <asp:Label ID="Label19" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 11px;
                position: absolute; top: 37px; width: 113px; height: 17px;" 
                 Text="Commission vat:"></asp:Label>
             <asp:TextBox ID="txtOverCommNet" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 130px; position: absolute;
                top: 10px; width: 110px;" TabIndex="2"></asp:TextBox>
             <asp:Label ID="Label20" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 12px;
                position: absolute; top: 62px; width: 113px; height: 17px;" 
                 Text="Commission total:"></asp:Label>
             <asp:TextBox ID="txtOverCommVat" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 130px; position: absolute;
                top: 35px; width: 110px;" TabIndex="3"></asp:TextBox>
             <asp:Label ID="Label21" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 267px;
                position: absolute; top: 24px; width: 119px; height: 17px;" 
                 Text="Commission recvd:"></asp:Label>
             <asp:Label ID="Label22" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 267px;
                position: absolute; top: 49px; width: 121px; height: 17px;" 
                 Text="Commission due:"></asp:Label>
             <asp:Label ID="Label23" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 527px;
                position: absolute; top: 24px; width: 83px; height: 17px;" 
                 Text="Pay net:"></asp:Label>
             <asp:TextBox ID="txtOverCommRec" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 385px; position: absolute;
                top: 22px; width: 110px;" TabIndex="5"></asp:TextBox>
             <asp:TextBox ID="txtOverCommDue" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 385px; position: absolute;
                top: 47px; width: 110px;" TabIndex="6"></asp:TextBox>
             <asp:Label ID="Label24" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 526px;
                position: absolute; top: 49px; width: 84px; height: 17px;" 
                 Text="Invoice total: (GBP)"></asp:Label>
             <asp:TextBox ID="txtOverTot" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 619px; position: absolute;
                top: 51px; width: 110px;" TabIndex="8" ReadOnly="True"></asp:TextBox>
             <asp:TextBox ID="txtOverCommTot" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 130px; position: absolute;
                top: 60px; width: 110px;" TabIndex="4" ReadOnly="True"></asp:TextBox>
             <asp:Button ID="btnSave" runat="server" CssClass="btnclasshand" 
                 Style="z-index: 109; left: 536px; position: absolute; top: 83px; width: 100px;" 
                 TabIndex="9" Text="Save Override" />
             <asp:Button ID="btnPaid" runat="server" CssClass="nyshyperlink" 
                 Style="z-index: 109; left: 466px; position: absolute; top: 66px; width: 34px;" 
                 TabIndex="9" Text="Paid" />
             <asp:Button ID="btnDelete" runat="server" CssClass="btnclasshand" 
                 Style="z-index: 109; left: 643px; position: absolute; top: 83px; width: 100px;" 
                 TabIndex="9" Text="Delete Override" />
             <asp:DropDownList ID="ddOverPay" runat="server" CssClass="nysdropdown" 
                 Style="z-index: 99; left: 619px; position: absolute; top: 22px; width: 116px;">
                 <asp:ListItem>Select</asp:ListItem>
                 <asp:ListItem>Yes</asp:ListItem>
                 <asp:ListItem>No</asp:ListItem>
                 <asp:ListItem>Yes &amp; No</asp:ListItem>
             </asp:DropDownList>
              <asp:Label ID="Label25" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 12px;
                position: absolute; top: 87px; width: 113px; height: 17px;" 
                  Text="Currency Code:"></asp:Label>
        </asp:Panel>
        <asp:Label ID="Label8" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 115px;
                position: absolute; top: 345px; width: 192px; height: 17px;" 
                Text="Values from BOSS override" Font-Bold="True"></asp:Label>
            <asp:Label ID="Label4" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 116px;
                position: absolute; top: 75px; width: 137px; height: 17px;" 
                Text="Values from BOSS" Font-Bold="True"></asp:Label>
            <asp:Label ID="Label5" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 116px;
                position: absolute; top: 210px; width: 215px; height: 17px;" 
                Text="Values from SQL copy of BOSS" Font-Bold="True"></asp:Label>
        </asp:Panel>
            
            <asp:Image ID="imHover" runat="server" 
                Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif"/>
            <asp:Button ID="btnDue" runat="server" CssClass="btnclasshand" 
                Style="z-index: 109; left: 853px; position: absolute; top: 10px; width: 133px;" 
                TabIndex="4" Text="Due Override Invoices" />
            <asp:Button ID="btnAll" runat="server" CssClass="btnclasshand" 
                Style="z-index: 109; left: 710px; position: absolute; top: 10px; width: 133px;" 
                TabIndex="4" Text="Override Invoices" />
        </asp:Panel>
    </div>
    </form>
</body>
</html>

