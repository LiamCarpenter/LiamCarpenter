Public Class clsUseful
    Public Shared Function notString(ByVal pArg As Object) As String
        If IsDBNull(pArg) Then
            Return ""
        Else
            Return CStr(pArg)
        End If
    End Function

    Public Shared Function notInteger(ByVal pArg As Object) As Integer
        If IsDBNull(pArg) Then
            Return 0
        Else
            If IsNumeric(pArg) Then
                Return CInt(pArg)
            Else
                Return 0
            End If
        End If
    End Function

    Public Shared Function notNumber(ByVal pArg As Object) As Double
        If IsDBNull(pArg) Then
            Return 0
        Else
            If IsNumeric(pArg) Then
                Return CDbl(pArg)
            Else
                Return 0
            End If
        End If
    End Function

    Public Shared Function notDecimal(ByVal pArg As Object) As Decimal
        If IsDBNull(pArg) Then
            Return 0
        Else
            If IsNumeric(pArg) Then
                Return CDec(pArg)
            Else
                Return 0
            End If
        End If
    End Function

    Public Shared Function strToInt(ByVal s As String) As Integer
        If s = "" Then
            Return 0
        Else
            Return CInt(s)
        End If
    End Function

    Public Shared Function notBoolean(ByVal pArg As Object) As Boolean
        If IsDBNull(pArg) Then
            Return False
        Else
            Return CBool(pArg)
        End If
    End Function

    Public Shared Function sendEmailLocal(ByVal pstrFrom As String, _
                              ByVal pstrTo As String, _
                              ByVal pstrSubject As String, _
                              ByVal pstrMessage As String) As Boolean

        Dim MailMsg As New System.Net.Mail.MailMessage(New System.Net.Mail.MailAddress(pstrFrom.Trim()), New System.Net.Mail.MailAddress(pstrTo))

        MailMsg.BodyEncoding = System.Text.Encoding.Default
        MailMsg.Subject = pstrSubject.Trim()

        'R21 CR
        Dim strbody As String = "<html><font face='Verdana' size='2'>" & pstrMessage & "</font></html>"
        strbody = strbody.Replace("""", "'")

        MailMsg.IsBodyHtml = True
        MailMsg.Body = strbody

        Try
            'Smtpclient to send the mail message
            Dim SmtpMail As New System.Net.Mail.SmtpClient
            SmtpMail.Send(MailMsg)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
