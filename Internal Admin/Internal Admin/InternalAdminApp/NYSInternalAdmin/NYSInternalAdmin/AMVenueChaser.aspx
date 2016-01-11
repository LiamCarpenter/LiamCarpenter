<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AMVenueChaser.aspx.vb" Inherits="NYSInternalAdmin.AMVenueChaser" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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

        //On scrolling of DIV tag.
        function OnDivScroll(tag) {
            var lstCollegeNames = document.getElementById(tag);

            if (lstCollegeNames.options.length > 8) {
                lstCollegeNames.size = lstCollegeNames.options.length;
            }
            else {
                lstCollegeNames.size = 8;
            }
        }

        function OnSelectFocus(divtag, tag) {

            if (document.getElementById(divtag).scrollLeft != 0) {
                document.getElementById(divtag).scrollLeft = 0;
            }

            var lstCollegeNames = document.getElementById(tag);

            if (lstCollegeNames.options.length > 8) {
                lstCollegeNames.focus();
                lstCollegeNames.size = 8;
            }
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
          <asp:Panel ID="pnTrans" runat="server" Style="left: 4px; position: absolute; z-index: 1001;
            top: 20px; width: 1010px; height: 652px; display: none;" CssClass="nyspanelwhite"
            BackColor="Transparent" BackImageUrl="~/images/trans_light.gif">
        </asp:Panel>
          <asp:Panel ID="pnNav" runat="server" Style="left: 6px; position: absolute; z-index: 1000;
            top: 2000px; width: 398px; height: 520px;" CssClass="nyspanelwhite">
            <asp:ImageButton ID="btnlogout" runat="server" CssClass="btnclasshand" ImageUrl="~/images/logout_out.gif"
                Style="z-index: 5000; left: 310px; position: absolute; top: 486px" TabIndex="3"
                BorderStyle="None" />
            <cc1:HoverMenuExtender ID="HoverMenuExtender1" runat="server" PopupControlID="pnNav"
                TargetControlID="imHover" OffsetY="-12" OffsetX="-11">
            </cc1:HoverMenuExtender>
            <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
        </asp:Panel>
                 <asp:Image ID="imHover" runat="server" 
                Style="z-index: 100; left: 9px; position: absolute;
                top: 10px;" ImageUrl="~/images/menu.gif"/>
              
              
              
              
 <asp:Panel ID="pnmain" runat="server"  Style="left: 4px; position: absolute; z-index: 100; top: 40px; height: 900px; width: 1200px; bottom: 232px;"    CssClass="nyspanels">


        <asp:Button ID="but" runat="server" Text="Go" />  
     <asp:TextBox runat="server" ID="txtStrDate" text="10-05-2014">     </asp:TextBox>   
       <asp:TextBox runat="server" ID="txtStrDateConf" text="20140510">
             
     </asp:TextBox>   
     
      <asp:TextBox runat="server" ID="txtError"> </asp:TextBox>
       <asp:Button ID="butCubitImport" runat="server" Text="Cubit Import" />

                       <asp:Button ID="butEvolviReader" runat="server" 
            Text="Evolvi Reader" />

            Group<asp:TextBox runat="server" ID="txtGroup"> </asp:TextBox>  
                 Company<asp:TextBox runat="server" ID="txtCompany"> </asp:TextBox> 
                    Rate<asp:TextBox runat="server" ID="txtRate"> </asp:TextBox>
        <asp:Button ID="butAutoInvoiceProcess" runat="server" 
            Text="AutoInvoiceProcess" />

            <br />
                       <div id='divList'   style="OVERFLOW: auto;WIDTH: 1005px;HEIGHT: 505px"   onscroll="OnDivScroll('lstEnquiry');" >

                         <asp:ListBox ID="lstEnquiry" style="HEIGHT: 500px" runat="server"  onfocus="OnSelectFocus('divList' ,'lstEnquiry');"   ></asp:ListBox>
                         <asp:ListBox ID="lstEmail" style="HEIGHT: 500px" runat="server"     ></asp:ListBox>
                    </div>


    </asp:Panel>
    </div>
    </form>
</body>
</html>
