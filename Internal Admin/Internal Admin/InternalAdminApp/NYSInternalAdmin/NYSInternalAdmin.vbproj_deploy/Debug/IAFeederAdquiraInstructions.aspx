<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IAFeederAdquiraInstructions.aspx.vb" Inherits="NYSInternalAdmin.IAFeederAdquiraInstructions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:725px; height:500px;overflow-y:scroll;">
        Please follow these steps for Uploading a feeder file to Adquira for O2:

        <ol>
            <li>Sign in to <a href="https://mkpes.adquira.com/">https://mkpes.adquira.com/</a></li>
            <li>Click on 'Raised Invoices'</li>
            <li>Click on 'New Invoice'</li>
            <li>Choose 'Create invoices WITHOUT purchase order reference' > 'Load invoice file' > 'Load CSV invoice file'</li>
            <li>Click 'Browse' then paste this path into the file browser and select the CSV file from the folder: <asp:literal runat="server" ID="litAdquiraFilePath"></asp:literal></li>
            <li>Press the Send button to begin processing the file</li>
            <li>Now go to 'Raised Invoices' again and check all invoices have been uploaded</li>
            <li>If an exclaimation mark is against any invoice it means there is an error - these need correcting now</li>
            <li>Still on 'Raised Invoices', tick all invoices and press the 'Sign' button</li>
            <li>This will now show you all signed invoices waiting to be sent, tick all the invoices and press the 'Send' button</li>
            <li>The invoices will now be marked as "Sending"</li>
        </ol>

        If an invoice fails to send an email will be received in the O2 Invoicing inbox detailing the problem. This can take a few hours to come through.
    </div>
    </form>
</body>
</html>
