<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IAManualSingleFeeder.aspx.vb" Inherits="NYSInternalAdmin.IASingleFeeder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <p>This will re-create a single xml feeder file for a specific invoice.</p>

    <p>
    <asp:Label runat="server" ID="lblError"></asp:Label>
    </p>
        Invoice Number: <asp:TextBox runat="server" id="txtInvoiceNumber"></asp:TextBox>
        <asp:Button runat="server" ID="btnCreateFile" Text="Create File" />
    </div>
    </form>
</body>
</html>
