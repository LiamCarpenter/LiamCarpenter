Public Class IAFeederAdquiraInstructions
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        litAdquiraFilePath.Text = "\\nysapps\XMLFILES\O2 Adquira\" & Date.Now.ToString("dd-MM-yyyy") & "\"
    End Sub

End Class