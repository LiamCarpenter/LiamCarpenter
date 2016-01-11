Option Strict On
Option Explicit On

Imports system.net.Mail
Imports EvoUtilities.ConfigUtils
Imports microsoft.visualbasic
Imports EvoUtilities.Utils

Public Class SendEmail

    Private Shared ReadOnly log As log4net.ILog = _
        log4net.LogManager.GetLogger(System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName)

    'Public Shared Sub send(ByVal pstrFrom As String, _
    '    ByVal pstrTo As String, ByVal pstrSubject As String, _
    '    ByVal pstrMessage As String, ByVal pstrBcc As String, _
    '    ByVal pstrcc As String, ByVal pstrHomePath As String, Optional ByVal pFile As String = "")
    '    Dim oCDo As New CDO.Message
    '    Try
    '        'create the cdo object
    '        Dim oBP As CDO.IBodyPart
    '        'set the smtp server settings on the cdo object
    '        Dim strUserName As String = getConfig("EmailUserName")
    '        Dim strHomepath As String = getConfig("HomePath") 'NYS"
    '        Dim strEmailPassword As String = getConfig("EmailPassword")
    '        Dim emailServerName As String = getConfig("EmailServerName")

    '        Dim bcc As String = ""
    '        If pstrSubject.StartsWith("Feedback requested regarding") Then
    '            bcc = giif(pstrBcc Is Nothing OrElse pstrBcc = "", _
    '                        getConfig("FeedbackBCCEmailAddress"), _
    '                        pstrBcc)
    '        Else
    '            bcc = giif(pstrBcc Is Nothing OrElse pstrBcc = "", _
    '                                    getConfig("TriggerBCCEmailAddress"), _
    '                                    pstrBcc)
    '        End If


    '        If emailServerName <> "" Then
    '            Dim oCDoConf As New CDO.Configuration
    '            'Set and update fields properties
    '            With oCDoConf
    '                'Outgoing SMTP server
    '                .Fields("http://schemas.microsoft.com/cdo/configuration/smtpserver").Value = emailServerName
    '                .Fields("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate").Value = 1
    '                .Fields("http://schemas.microsoft.com/cdo/configuration/sendusername").Value = strUserName
    '                .Fields("http://schemas.microsoft.com/cdo/configuration/sendpassword").Value = strEmailPassword
    '                .Fields("http://schemas.microsoft.com/cdo/configuration/sendusing").Value = 2
    '                .Fields.Update()
    '            End With
    '            'Update the CDOSYS Configuration
    '            oCDo.Configuration = oCDoConf
    '        End If

    '        oCDo.MimeFormatted = True
    '        oCDo.To = pstrTo
    '        oCDo.CC = pstrcc
    '        oCDo.BCC = bcc
    '        oCDo.From = pstrFrom
    '        oCDo.Subject = pstrSubject
    '        oCDo.ReplyTo = pstrFrom

    '        Dim strbody As String = "<html><font face='Verdana' size='2'>" & pstrMessage & "</font></html>"
    '        Dim intFirst As Integer = 0
    '        'get first position of first image if there is one
    '        If InStr(strbody, "<img") > 0 Then
    '            intFirst = strbody.IndexOf("<img")
    '        Else
    '            intFirst = 0
    '            oBP = oCDo.BodyPart
    '        End If

    '        'cycles through img tags and replaces tags with cid: tags for embedding images

    '        Do While intFirst > 0
    '            Dim intSecond As Integer = strbody.IndexOf("src=", intFirst)
    '            Dim intThird As Integer = strbody.IndexOf("/>", intSecond)

    '            'gets full image tag
    '            Dim strimgfullstring As String = Mid(strbody, intFirst + 1, (intThird + 3) - (intFirst + 1))

    '            'checks if already replaced
    '            If InStr(strimgfullstring, "cid:") > 0 Then
    '                'no need to do anything!
    '            Else
    '                Dim strimgtosrctext As String = Mid(strbody, intFirst + 1, (intSecond + 6) - (intFirst + 1))
    '                Dim strfilepath As String = Mid(strbody, intSecond + 6, (intThird - 1) - (intSecond + 6))
    '                If InStr(strfilepath, "http://") > 0 Then

    '                Else
    '                    Dim intFourth As Integer = InStrRev(strfilepath, "/")
    '                    Dim strfilename As String = Mid(strfilepath, intFourth + 1)
    '                    strbody = strbody.Replace(strimgfullstring, "<img src=""cid:" & strfilename & """ />")
    '                    strfilepath = strfilepath.Replace("/", "\")
    '                    strfilepath = strfilepath.Replace("\" & strHomepath & "\", "")

    '                    strfilepath = pstrHomePath & strfilepath

    '                    If IO.File.Exists(strfilepath) Then
    '                        oBP = oCDo.AddRelatedBodyPart(strfilepath, strfilename, _
    '                                               CDO.CdoReferenceType.cdoRefTypeId, strUserName, strEmailPassword)
    '                        oBP.Fields.Item("urn:schemas:mailheader:Content-ID").Value = "<" & strfilename & ">"
    '                    Else
    '                        Throw New IO.FileNotFoundException
    '                    End If
    '                End If
    '            End If

    '            Dim intFifth As Integer = strbody.LastIndexOf("<img")

    '            If intFirst = intFifth Then
    '                intFirst = 0
    '            Else
    '                intFirst = strbody.IndexOf("<img", intFirst + 1)
    '                If intFirst = -1 Then 'must be same image so already been replaced
    '                    intFirst = 0
    '                End If
    '            End If
    '        Loop

    '        oCDo.HTMLBody = strbody
    '        If pFile <> "" Then
    '            oCDo.AddAttachment(pFile)
    '        End If
    '        oCDo.Send()

    '        oCDo = Nothing

    '    Catch ex As Exception
    '        oCDo = Nothing
    '    End Try
    'End Sub

    ''' <summary>
    ''' Send an email to/from the specified addresses. Any html images are converted to attachments and added to the email to force inline images to work.
    ''' New version now uses system.mail instead of old CDO coding.
    ''' </summary>
    ''' <param name="pstrFrom"></param>
    ''' <param name="pstrTo"></param>
    ''' <param name="pstrSubject"></param>
    ''' <param name="pstrMessage"></param>
    ''' <param name="pstrBcc"></param>
    ''' <param name="pstrcc"></param>
    ''' <param name="pstrHomePath"></param>
    ''' <param name="pFile"></param>
    ''' <remarks></remarks>
    Public Shared Sub send(ByVal pstrFrom As String, _
        ByVal pstrTo As String, ByVal pstrSubject As String, _
        ByVal pstrMessage As String, ByVal pstrBcc As String, _
        ByVal pstrcc As String, ByVal pstrHomePath As String, Optional ByVal pFile As String = "")
        Dim MailMsg As MailMessage
        Try

            MailMsg = New MailMessage(New MailAddress(pstrFrom.Trim()), New MailAddress(pstrTo))
            MailMsg.BodyEncoding = System.Text.Encoding.Default
            MailMsg.IsBodyHtml = True
            MailMsg.Subject = pstrSubject

            'add the cc
            If pstrcc <> "" Then
                MailMsg.CC.Add(pstrcc)
            End If

            'work out if we need to add a bcc from the config
            Dim bcc As String = ""
            If pstrSubject.StartsWith("Feedback requested regarding") Then
                bcc = giif(pstrBcc Is Nothing OrElse pstrBcc = "", _
                            getConfig("FeedbackBCCEmailAddress"), _
                            pstrBcc)
            Else
                bcc = giif(pstrBcc Is Nothing OrElse pstrBcc = "", _
                                        getConfig("TriggerBCCEmailAddress"), _
                                        pstrBcc)
            End If

            'add the BCC
            If bcc <> "" Then
                MailMsg.Bcc.Add(bcc)
            End If

            'define the body text
            Dim strbody As String = "<html><font face='Verdana' size='2'>" & pstrMessage & "</font></html>"

            Dim strHomepath As String = getConfig("HomePath") 'NYS"

            Dim intFirst As Integer = 0
            'get first position of first image if there is one
            If InStr(strbody, "<img") > 0 Then
                intFirst = strbody.IndexOf("<img")
            Else
                intFirst = 0
                'oBP = oCDo.BodyPart
            End If

            'cycles through img tags and replaces tags with cid: tags for embedding images

            Do While intFirst > 0
                Dim intSecond As Integer = strbody.IndexOf("src=", intFirst)
                Dim intThird As Integer = strbody.IndexOf("/>", intSecond)

                'gets full image tag
                Dim strimgfullstring As String = Mid(strbody, intFirst + 1, (intThird + 3) - (intFirst + 1))

                'checks if already replaced
                If InStr(strimgfullstring, "cid:") > 0 Then
                    'no need to do anything!
                Else
                    Dim strimgtosrctext As String = Mid(strbody, intFirst + 1, (intSecond + 6) - (intFirst + 1))
                    Dim strfilepath As String = Mid(strbody, intSecond + 6, (intThird - 1) - (intSecond + 6))


                    If InStr(strfilepath, "http://") > 0 Then
                        'dont care - it's over http so doesn't need attaching
                    Else
                        'check the file exists
                        Dim strPathToCheck As String = pstrHomePath & strfilepath.Replace("\" & strHomepath & "\", "").Replace("/", "\")
                        If IO.File.Exists(strPathToCheck) Then

                            Dim intFourth As Integer = InStrRev(strfilepath, "/")
                            Dim strfilename As String = Mid(strfilepath, intFourth + 1)
                            Dim strContentID As String = strfilename.Replace(".", "") & "@NYS"

                            'create an inline attachment
                            Dim oInlineAttachment As Attachment
                            oInlineAttachment = New Attachment(strfilepath)
                            oInlineAttachment.ContentDisposition.Inline = True
                            oInlineAttachment.ContentDisposition.DispositionType = System.Net.Mime.DispositionTypeNames.Inline
                            oInlineAttachment.ContentId = strContentID
                            oInlineAttachment.ContentType.Name = strfilename

                            'add the attachment to the message
                            MailMsg.Attachments.Add(oInlineAttachment)

                            'replace the img src with the content ID
                            strbody = strbody.Replace(strimgfullstring, "<img src=""cid:" & strContentID & """ />")

                        Else
                            Throw New IO.FileNotFoundException
                        End If
                    End If
                End If

                Dim intFifth As Integer = strbody.LastIndexOf("<img")

                If intFirst = intFifth Then
                    intFirst = 0
                Else
                    intFirst = strbody.IndexOf("<img", intFirst + 1)
                    If intFirst = -1 Then 'must be same image so already been replaced
                        intFirst = 0
                    End If
                End If
            Loop

            'add the body
            MailMsg.Body = strbody

            'add any other (non inline) attachments
            If pFile <> "" Then
                MailMsg.Attachments.Add(New Attachment(pFile))
            End If

            'Smtpclient to send the mail message
            Dim SmtpMail As New SmtpClient
            SmtpMail.Send(MailMsg)

            'R10 SA - need to dispose in order to move the file into another folder
            MailMsg.Dispose()

        Catch ex As Exception
            MailMsg = Nothing
        End Try
    End Sub

    Public Shared Sub safeSend(ByVal action As String, ByVal details As String, ByVal exp As Exception)
        Try
            Dim errorEmailAddress As String = _
                           EvoUtilities.ConfigUtils.getConfig("ErrorEmailAddress")
            Dim body As String = "Failure: " & action & vbNewLine & vbNewLine & _
                "Details: " & details & vbNewLine & vbNewLine & _
                "Exception: " & EvoUtilities.ExceptionUtils.getExceptionFullText(exp)
            send(errorEmailAddress, errorEmailAddress, "MEVIS Service failure: " & action, _
                body.Replace(vbNewLine, "<br>"), "", "", "")
        Catch ex As Exception
            log.Error(ex)
        End Try
    End Sub
End Class
