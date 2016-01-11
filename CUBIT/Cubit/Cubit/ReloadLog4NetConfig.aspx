<%@ Page Language="vb" %>
<%@ Import Namespace="System.IO" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>ReloadLog4NetConfig</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	</head>
	<body  ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		
		<%
		    lblout.Text = Cubit.NSLogUtils.reloadConfig().Replace("&", "&amp;"). _
    Replace("<", "&lt;").Replace(vbNewLine, "<br />")
		%>
			<asp:Label id="lblout" runat="server"></asp:Label>
		</form>
	</body>
</html>
