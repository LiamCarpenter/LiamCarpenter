﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IAContractsAmendSummary.aspx.vb" Inherits="NYSInternalAdmin.IAContractsAmendSummary" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
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
                top: -2px; width: 1010px;" Text="Contract Amendments Summary"></asp:Label>

        <asp:Panel ID="pnmain" runat="server" 
            Style="left: 4px; overflow: auto;  position: absolute; z-index:100; top: 20px; height: 954px; width: 1010px;" 
            CssClass="nyspanels" >
        <asp:Panel ID="pndates" runat="server" CssClass="nyspanels" 
             Style="left: 290px; position: absolute; top: 4px; width: 420px; height: 34px; z-index: 10000;">
        <asp:Label ID="lblfrom" runat="server" CssClass="nyslabel" Style="z-index: 103; left: 13px;
                position: absolute; top: 10px; width: 114px; right: 293px;" 
            Text="Select month:"></asp:Label>
        <asp:TextBox ID="txtfrom" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 127px; position: absolute;
                top: 8px; width: 40px;" TabIndex="1" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtto" runat="server" CssClass="nystextbox" Style="z-index: 99; left: 238px; position: absolute;
                top: 8px; width: 50px; " TabIndex="1" Visible="false"></asp:TextBox>
        <asp:ImageButton ID="btnrefresh" runat="server" CssClass="btnclasshand" 
             ImageUrl="~/images/refresh_out.gif" 
             Style="z-index: 109; left: 325px; position: absolute; top: 6px" 
             TabIndex="3" BorderStyle="None" /> 
            <asp:DropDownList ID="ddreport" runat="server" CssClass="nysdropdown" 
                Style="z-index: 102; left: 102px; position: absolute; top: 8px; width: 216px;" 
                tabindex="1">
            </asp:DropDownList>
     </asp:Panel>
   <asp:Image ID="imHover" runat="server" 
            Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif"/>
            
        <rsweb:ReportViewer ID="rvAmendSummaryPerc" 
            runat="server" 
            Style="left: 4px; position: absolute; top: 390px;"
                Font-Names="Verdana" Font-Size="8pt" ShowPageNavigationControls="False" 
                ShowZoomControl="False" BackColor="White" 
             ToolBarItemHoverBackColor="Silver" CssClass="reportView" 
             InternalBorderStyle="None" LinkDisabledColor="LightGray" 
                DocumentMapWidth="100%" ToolBarItemBorderStyle="None" 
                LinkActiveColor="Blue" LinkActiveHoverColor="DeepPink" Height="290px" 
                Width="342px" ShowBackButton="False">
           
            <LocalReport ReportPath="Reports\repContractsAmendSummaryPerc.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                        Name="dsContractsAmendSummaryPerc_contractsSummaryAmendPerc" />
                </DataSources>
            </LocalReport>
           
         </rsweb:ReportViewer>
        
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
                
                TypeName="NYSInternalAdmin.dsContractsAmendSummaryPercTableAdapters.contractsSummaryAmendPercTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtfrom" Name="startdate" PropertyName="Text" 
                        Type="String" />
                    <asp:ControlParameter ControlID="txtto" Name="enddate" PropertyName="Text" 
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        
         <rsweb:ReportViewer ID="rvAmendSummary" 
            runat="server" 
            Style="left: 4px; position: absolute; top: 55px;" 
                Font-Names="Verdana" Font-Size="8pt"
                ShowFindControls="True" ShowPageNavigationControls="False" 
                ShowZoomControl="False" BackColor="White" 
             ToolBarItemHoverBackColor="Silver" CssClass="reportView" 
             InternalBorderStyle="None" LinkDisabledColor="LightGray" 
                DocumentMapWidth="100%" ToolBarItemBorderStyle="None" 
                LinkActiveColor="Blue" LinkActiveHoverColor="DeepPink" Height="305px" Width="995px" ShowBackButton="False">
             
             <LocalReport ReportPath="Reports\repContractsAmendSummary.rdlc">
                 <DataSources>
                     <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" 
                         Name="dsContractsAmendSummary_contractsSummaryAmend" />
                 </DataSources>
             </LocalReport>
             
            </rsweb:ReportViewer>
            <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
                OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
                
                TypeName="NYSInternalAdmin.dsContractsAmendSummaryTableAdapters.contractsSummaryAmendTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtfrom" Name="startdate" PropertyName="Text" 
                        Type="String" />
                    <asp:ControlParameter ControlID="txtto" Name="enddate" PropertyName="Text" 
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <rsweb:ReportViewer ID="rvUsersSummary" 
            runat="server" 
            Style="left: 3px; position: absolute; top: 715px;" 
                Font-Names="Verdana" Font-Size="8pt"
                ShowFindControls="True" ShowPageNavigationControls="False" 
                ShowZoomControl="False" BackColor="White" 
             ToolBarItemHoverBackColor="Silver" CssClass="reportView" 
             InternalBorderStyle="None" LinkDisabledColor="LightGray" 
                DocumentMapWidth="100%" ToolBarItemBorderStyle="None" 
                LinkActiveColor="Blue" LinkActiveHoverColor="DeepPink" Height="194px" ShowBackButton="False" Width="814px">
             
                <LocalReport ReportPath="Reports\repContractsAmendSummaryByUser.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource3" 
                            Name="dsContractsAmendSummaryByUser_contractsSummaryAmendByUser" />
                    </DataSources>
                </LocalReport>
             
            </rsweb:ReportViewer>
            <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" 
                OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
                
                TypeName="NYSInternalAdmin.dsContractsAmendSummaryByUserTableAdapters.contractsSummaryAmendByUserTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtfrom" Name="startdate" PropertyName="Text" 
                        Type="String" />
                    <asp:ControlParameter ControlID="txtto" Name="enddate" PropertyName="Text" 
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <rsweb:ReportViewer ID="rvAmendSummaryChart" 
            runat="server" 
            Style="left: 353px; position: absolute; top: 390px;"
                Font-Names="Verdana" Font-Size="8pt"
                ShowFindControls="True" ShowPageNavigationControls="False" 
                ShowZoomControl="False" BackColor="White" 
             ToolBarItemHoverBackColor="Silver" CssClass="reportView" 
             InternalBorderStyle="None" LinkDisabledColor="LightGray" 
                DocumentMapWidth="100%" ToolBarItemBorderStyle="None" 
                LinkActiveColor="Blue" LinkActiveHoverColor="DeepPink" Height="290px" ShowBackButton="False" Width="644px">
           
                <LocalReport ReportPath="Reports\repContractsAmendSummaryChart.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                            Name="dsContractsAmendSummaryPerc_contractsSummaryAmendPerc" />
                    </DataSources>
                </LocalReport>
           
         </rsweb:ReportViewer>
        
         <asp:Label ID="lblweek1" runat="server" CssClass="nysheaderSm" Text="Contract Amendements Summary"
            Style="z-index: 101; left: 6px; position: absolute; top: 40px; width: 1001px;"></asp:Label>
         <asp:Label ID="lblweek3" runat="server" CssClass="nysheaderSm" Text="Contract Amendements By User" 
            Style="z-index: 101; left: 7px; position: absolute; top: 699px; width: 806px;"></asp:Label>
         <asp:Label ID="lblweek2" runat="server" CssClass="nysheaderSm" Text="Contract Amendements %" 
            Style="z-index: 101; left: 4px; position: absolute; top: 375px; width: 342px;"></asp:Label>
         <asp:Label ID="lblweek4" runat="server" CssClass="nysheaderSm" Text="Contract Amendements Chart"  
            
                
                Style="z-index: 101; left: 354px; position: absolute; top: 375px; width: 649px;"></asp:Label>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
