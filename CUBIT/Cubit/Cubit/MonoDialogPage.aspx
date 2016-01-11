<%@ Assembly Name="MonoSoftware.Web.Dialogs" %>
<%@ Page smartNavigation="False" AutoEventWireup="false" Inherits="MonoSoftware.Web.Dialogs.MonoDLGPage" enableViewState="False" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
		<base target="_self"/>
		<title id="PageCaption" runat="server"></title>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<link href="MonoDialogs.css" type="text/css" rel="stylesheet"/>
 </head>
<body <asp:literal Runat="server" id="lblBody"></ASP:literal> >
<script type="text/javascript" language="javascript">
		function inputQueryResult(keys, ctrls){
			//debugger;
		
			//call eventual validation
			if (typeof(Page_ClientValidate) == 'function') {if (!(Page_ClientValidate())) {return false;};} 
		
			//keys and ctrls have the same length
			var retArr = new Array(2 * (-1 + ctrls.length));

			for (var i=0;i<ctrls.length;i++) {
				retArr[i] = keys[i]; //control's key == friendly name
				var element = document.getElementById(ctrls[i]);
				
				switch(element.tagName.toLowerCase())
				{
					case "input":
						if ((element.type.toLowerCase() == "checkbox") || (element.type.toLowerCase() == "radio"))
							{retArr[ctrls.length + i] = element.checked;}
						else 
							{retArr[ctrls.length + i] = element.value;}
						break;
					case "select":
						var res = "";
						if (element.multiple)
						{
							for (var j=0;j<element.options.length; j++) 
							{
								if (element.options[j].selected)
								{
									res = res + element.options[j].value + ", ";
								}
							}
							if (res.length!=0){res = res.substr(0,res.lastIndexOf(", "));}
							
							retArr[ctrls.length + i] = res;
						}
						else
						{
							retArr[ctrls.length + i] = element.value;	
						}
						break;
					case "table": //ASP.NET::RadioButtonList, DropDownList
						var cbs = element.getElementsByTagName("input");
						var res = "";
						for (var j = 0; j <= cbs.length-1; j++)
						{
							if (cbs[j].checked)
							{
								if (cbs[j].type.toLowerCase()== "checkbox") // has a <label for ...> after <input ...>
								{
									res = res + cbs[j].nextSibling.innerHTML + ", ";
								}
								else
								{
									res = res + cbs[j].value + ", ";
								}
							}
						}
						if (res.length!=0){res = res.substr(0,res.lastIndexOf(", "));}
						retArr[ctrls.length + i] = res;
						break;
					default:
						retArr[ctrls.length + i] = element.value;
						
						break;
				}//switch
			}
			
			window.returnValue = retArr;
			
			window.close();
		} //InputBoxExResult
		
		function noPostBack(){return false;}
		

function ClipBoard() 
{
	var holdtext = document.getElementById("holdtext");
	//holdtext.innerText = o.innerText;
	Copied = holdtext.createTextRange();
	Copied.execCommand("Copy");
}


		function resizeDialog(dlg, docenter) {
			//remove ASP.NET postback's
			//__doPostBack = noPostBack;
		
			//BEGIN set mesage
			var MonoDlg = window.dialogArguments;
			var o = document.getElementById("MonoMsg");
			if (o != null){o.innerHTML = MonoDlg.msg;};
			//END set mesage
		
			var fr = document.getElementById("tblMonoDialog");
			fr.Left = "4px";
			fr.Top = "4px";
			
			if (dlg == 'MessageDlg') {
				window.dialogWidth = (fr.offsetWidth + 10) + "px";
			}
			if (dlg == 'ShowMessage') {
				window.dialogWidth = (fr.offsetWidth ) + "px";
			}

			var textmessage = document.getElementById("holdtext");
			textmessage.value = o.innerHTML;
			//alert(textmessage.value.indexOf('Error'));
			if (textmessage.value.indexOf('Error') == -1){
			   document.getElementById("link").style.visibility = 'hidden';
			}

			window.dialogHeight = (fr.offsetHeight + 60) + "px";
			//alert(window.dialogHeight)
			//extract number from "123px"
			var dlgLeft = window.dialogLeft.substring(0, window.dialogLeft.length - 2)
			var dlgTop = window.dialogTop.substring(0, window.dialogTop.length - 2)
			var dlgWidth = window.dialogWidth.substring(0, window.dialogWidth.length - 2)
			var dlgHeight = window.dialogHeight.substring(0, window.dialogHeight.length - 2)
            docenter=2
			if (docenter == 1) {
			//alert(5);
					window.dialogLeft = (Math.floor(screen.width / 2) - Math.floor(dlgWidth / 2)) + "px";
					window.dialogTop = (Math.floor(screen.height / 2) - Math.floor(dlgHeight / 2)) + "px";
			}
			else {
			//alert(9);
				if (dlgLeft > (screen.width - dlgWidth)) {window.dialogLeft = "10px"};
				if (dlgTop > (screen.height - dlgHeight)) {window.dialogTop = "10px"};
			}
			
			//set input focus to txtInputBox and select all the text inside
			if (dlg == 'InputBox') {
				document.getElementById("txtInputBox").focus();
				document.getElementById("txtInputBox").select();
			}
		}//resizeDialog()
		</script>
		<form id="MonoDialogForm" runat="server">
			<a id="link" class='evolink' href="JavaScript:ClipBoard();">Copy to Clipboard</a> 
			<asp:PlaceHolder  id="MonoDialogHolder"  runat="server" Visible="True">
				</asp:PlaceHolder>
                
				<textarea id="holdtext" style="display:none;"></textarea>

				
		</form>
	</body>
</html>
