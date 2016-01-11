Imports Microsoft.VisualBasic
Imports System.Net.Mail

Public Class ClsNys
    
    Public Shared Function notString(ByVal pArg As Object) As String
        If IsDBNull(pArg) Then
            Return ""
        Else
            Return CStr(pArg)
        End If
    End Function

    Public Shared Function notNumber(ByVal pArg As Object) As Double
        If IsDBNull(pArg) Then
            Return 0
        Else
            Return CDbl(pArg)
        End If
    End Function

    Public Shared Function notInteger(ByVal pArg As Object) As Integer
        If IsDBNull(pArg) Then
            Return 0
        Else
            Return CInt(pArg)
        End If
    End Function

    Public Shared Function notDouble(ByVal pArg As Object) As Double
        If IsDBNull(pArg) Then
            Return 0
        Else
            Return CDbl(pArg)
        End If
    End Function

    Public Shared Function notBoolean(ByVal pArg As Object) As Boolean
        If IsDBNull(pArg) Then
            Return False
        Else
            Return CBool(pArg)
        End If
    End Function

    Public Shared Function SendEmailMessage(ByVal emailFrom As String, ByVal emailTo As String, ByVal emailSubject As String, _
                                            ByVal emailMessage As String, ByVal pstrAttachPath As String, ByVal emailcc1 As String, ByVal emailcc2 As String, _
                                            ByVal emailBcc As String) As Boolean
        'For each to address create a mail message
        Dim MailMsg As New MailMessage
        MailMsg.From = New MailAddress(emailFrom.Trim)

        'do this for multiple email address handling
        If emailTo.Contains(";") Then
            For Each strSingleEmail As String In emailTo.Split(";")
                If strSingleEmail <> "" Then
                    MailMsg.To.Add(New MailAddress(strSingleEmail.Trim))
                End If
            Next
        Else
            MailMsg.To.Add(New MailAddress(emailTo.Trim))
        End If


        If emailcc1 <> "" Then
            Dim ccaddr As New MailAddress(emailcc1)
            MailMsg.CC.Add(ccaddr)
        End If
        If emailcc2 <> "" Then
            Dim ccaddr As New MailAddress(emailcc2)
            MailMsg.CC.Add(ccaddr)
        End If
        If emailBcc <> "" Then
            Dim bccaddr As New MailAddress(emailBcc)
            MailMsg.Bcc.Add(bccaddr)
        End If

        MailMsg.BodyEncoding = System.Text.Encoding.Default
        MailMsg.Subject = emailSubject.Trim()
        MailMsg.Body = emailMessage.Trim() & vbCrLf
        MailMsg.IsBodyHtml = True

        If pstrAttachPath <> "" Then
            Dim mailAttach As New Attachment(pstrAttachPath)
            MailMsg.Attachments.Add(mailAttach)
        End If

        Dim SmtpMail As New SmtpClient
        Try
            SmtpMail.Send(MailMsg)
            Return True
        Catch ex As Exception
            'log.Error(ex.Message)
            SendEmailMessageInternal(emailFrom, emailTo, "Help", "", pstrAttachPath)
            Return False
        End Try

    End Function

    Public Shared Function SendEmailMessageInternal(ByVal emailFrom As String, _
            ByVal emailTo As String, ByVal emailSubject As String, _
            ByVal emailMessage As String, ByVal pstrAttachPath As String) As Boolean
        'For each to address create a mail message
        Dim MailMsg As New MailMessage(New MailAddress(emailFrom.Trim()), New MailAddress("nick.massarella@nysgroup.com"))

        MailMsg.BodyEncoding = System.Text.Encoding.Default
        MailMsg.Subject = emailSubject.Trim()
        MailMsg.Body = "Email did not send to " & emailTo & vbCrLf
        MailMsg.IsBodyHtml = True

        If pstrAttachPath <> "" Then
            Dim mailAttach As New Attachment(pstrAttachPath)
            MailMsg.Attachments.Add(mailAttach)
        End If

        Dim SmtpMail As New SmtpClient
        Try
            SmtpMail.Send(MailMsg)
            Return True
        Catch ex As Exception
            'log.Error(ex.Message)
            Return False
        End Try
    End Function
End Class
