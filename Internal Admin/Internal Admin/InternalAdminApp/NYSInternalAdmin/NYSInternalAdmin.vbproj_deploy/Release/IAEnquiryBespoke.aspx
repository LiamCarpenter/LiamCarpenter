<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IAEnquiryBespoke.aspx.vb" Inherits="NYSInternalAdmin.IABespoke" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
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

            <asp:Label ID="lblBespoke" runat="server" 
             CssClass="nyslabelHeaderCentre" Style="z-index: 101; left: 4px; position: absolute;
                top: -2px; width: 1010px;" Text="Enquiry Bespoke"></asp:Label>
            <asp:Panel ID="pnmain" runat="server" 
            Style="left: 4px; position: absolute; z-index:100; top: 20px; height: 652px; width: 1010px;" 
            CssClass="nyspanels" >       
            <rsweb:ReportViewer ID="rvBespoke" runat="server" BackColor="White" 
            CssClass="reportView" DocumentMapWidth="100%" Font-Names="Verdana" 
            Font-Size="8pt" InternalBorderStyle="None" 
            InternalBorderWidth="2px" Height="600px" ShowCredentialPrompts="False" 
            ShowDocumentMapButton="False" 
            ShowPageNavigationControls="False" ShowParameterPrompts="False" 
            ShowPromptAreaButton="False" ShowZoomControl="False" 
            Style="left: 12px; position: absolute; z-index:100; top: 37px; width: 985px;" 
            ToolBarItemHoverBackColor="Silver" 
            LinkDisabledColor="LightGray" ToolBarItemBorderStyle="None" 
                LinkActiveColor="Blue" LinkActiveHoverColor="DeepPink" 
                ShowBackButton="False" InteractiveDeviceInfos="(Collection)" 
                WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
     
                <LocalReport ReportPath="Reports\repBespokeReport.rdlc">
                 <DataSources>
                     <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" 
                         Name="BespokeReport" />
                 </DataSources>
             </LocalReport>
         </rsweb:ReportViewer>

           <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
            TypeName="NYSInternalAdmin.dsBespokeReportTableAdapters.BespokeReportTableAdapter">
        </asp:ObjectDataSource>
        <asp:Image ID="imHover" runat="server" 
            Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif"/>
        </asp:Panel>
    
              
    </div>
    </form>
</body>
</html>
