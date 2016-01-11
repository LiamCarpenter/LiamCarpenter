Public Class Form1

    Private Sub btnTestInteceptor_Click(sender As System.Object, e As System.EventArgs) Handles btnTestInteceptor.Click
        Try
            Dim TestInteceptor As New NYS_MailService.srvMail

            TestInteceptor.EvolviInterceptorGuardian(tbGuardianInput.Text, tbGuardianOutput.Text)
            TestInteceptor.EvolviProcessFolder(tbInteceptorXML.Text, tbBossOutput.Text, tbSQLOutput.Text)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        
    End Sub

    Private Sub btnSSOSync_Click(sender As System.Object, e As System.EventArgs) Handles btnSSOSync.Click
        Try
            Dim TestInteceptor As New NYS_MailService.srvMail

            TestInteceptor.getSSODetails()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnTestMailing_Click(sender As Object, e As EventArgs) Handles btnTestMailing.Click
        Try
            Dim testEmailInvoices As New NYS_MailService.srvMail

            testEmailInvoices.AutoInvoiceProcessFolder()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub tbInteceptorXML_TextChanged(sender As Object, e As EventArgs) Handles tbInteceptorXML.TextChanged

    End Sub
End Class
