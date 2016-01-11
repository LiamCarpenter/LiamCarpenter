<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReportMenu.ascx.vb" Inherits="NYSInternalAdmin.ucReportMenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
<asp:Panel ID="pnNav" runat="server" 
            Style="left: 0px; position: absolute; z-index: 1000; top: 0px; width: 398px; height: 520px;" 
            Width="100px">
    <asp:Panel ID="Panel2" runat="server" BackColor="LightGray" 
        Style="left: 5px; position: absolute; z-index: 1000; top: 5px; width: 386px; height: 508px;">
        <cc1:Accordion ID="Accordion2" runat="server" 
            FramesPerSecond="30" HeaderCssClass="" 
            HeaderSelectedCssClass="" TransitionDuration="250" FadeTransitions="True" 
            RequireOpenedPane="False">
            <Panes>
                <cc1:AccordionPane ID="AccordionPane1" runat="server">
                    <Header>
                        <div id="accp6" runat="server" style="width: 300px; height:20px; top: 4px; text-align: left;" class="nysheaderMenu" >&nbsp;
                        <a href="" id="A4" onclick="return false;" class="nysheaderText">Internal Reports</a></div>
                    </Header>
                    <Content>
                    </Content>
                </cc1:AccordionPane>
                <cc1:AccordionPane ID="AccordionPane2" runat="server">
                    <Header>
                        <div id="Div2" runat="server" style="width: 300px; height:20px; top: 4px; text-align: left;" class="nysheaderMenu" >&nbsp;
                        <a href="" id="A1" onclick="return false;" class="nysheaderText">Internal Tools</a></div>
                    </Header>
                    <Content>
                    </Content>
                </cc1:AccordionPane>
                <cc1:AccordionPane ID="AccordionPane3" runat="server">
                    <Header>
                        <div id="Div1" runat="server" style="width: 300px; height:20px; top: 4px; text-align: left;" class="nysheaderMenu" >&nbsp;
                        <a href="" id="A6" onclick="return false;" class="nysheaderText">Traveller Tracking Internal</a></div>
                    </Header>
                    <Content>
                    </Content>
                </cc1:AccordionPane>
                <cc1:AccordionPane ID="AccordionPane4" runat="server">
                    <Header>
                        <div id="accp7" runat="server" style="width: 300px; height:20px; top: 4px; text-align: left;" class="nysheaderMenu" >&nbsp;
                        <a href="" id="A5" onclick="return false;" class="nysheaderText">Admin</a></div>
                    </Header>
                    <Content>
                    </Content>
                </cc1:AccordionPane>
            </Panes>
        </cc1:Accordion>
    </asp:Panel>
</asp:Panel>
