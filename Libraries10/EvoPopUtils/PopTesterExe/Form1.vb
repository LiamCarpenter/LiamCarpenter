Option Strict On
Option Explicit On

Imports EvoUtilities.ExceptionUtils
Imports EvoPopUtils

Public Class Form1

    Private Sub handleexception(ByVal ex As Exception)
        txtLog.Text &= getExceptionFullText(ex)
    End Sub

    Private Sub btnDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        Try
            Dim p As New PopDownloader(txtServer.Text, txtUsername.Text, _
                txtPassword.Text, txtVar.Text)
            Dim n As Integer = p.downloadMessages()
            txtLog.Text &= n & " messages downloaded" & vbNewLine
        Catch ex As Exception
            handleexception(ex)
        End Try
    End Sub
End Class
