Option Strict On
Option Explicit On

Imports system.net.Mail
Imports System.Text.RegularExpressions
Imports EvoUtilities.CollectionUtils
Imports EvoUtilities.log4netUtils
Imports EvoUtilities.Utils

Public Class SendEmailUtils

    Public Const emailRegex As String = "^((?>[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+\x20*|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f])*""\x20*)*(?<angle><))?((?!\.)(?>\.?[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+)+|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f])*"")@(((?!-)[a-zA-Z\d\-]+(?<!-)\.)+[a-zA-Z]{2,}|\[(((?(?<!\[)\.)(25[0-5]|2[0-4]\d|[01]?\d?\d)){4}|[a-zA-Z\d\-]*[a-zA-Z\d]:((?=[\x01-\x7f])[^\\\[\]]|\\[\x01-\x7f])+)\])(?(angle)>)$"

    Private Shared log As log4net.ILog = _
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Shared Sub SendEmailMessageOld(ByVal From As String, _
        ByVal [To] As List(Of String), ByVal Subject As String, _
        ByVal Message As String, Optional ByVal Bcc As List(Of String) = Nothing, _
        Optional ByVal attachments As IList(Of AttachmentInfo) = Nothing, _
        Optional ByVal CC As List(Of String) = Nothing)

        Using New MethodLogger(log, "SendEmailMessageOld")
            Dim attachmentObjects As New List(Of Attachment)
            Try
                'For each to address create a mail message

                Dim MailMsg As New MailMessage()
                MailMsg.From = New MailAddress(From.Trim())
                For Each s As String In [To]
                    MailMsg.To.Add(New MailAddress(s))
                Next

                If Bcc IsNot Nothing Then
                    For Each s As String In Bcc
                        MailMsg.Bcc.Add(New MailAddress(s))
                    Next
                End If
                If CC IsNot Nothing Then
                    For Each s As String In CC
                        MailMsg.CC.Add(New MailAddress(s))
                    Next
                End If
                MailMsg.BodyEncoding = System.Text.Encoding.Default
                MailMsg.Subject = Subject.Trim()
                MailMsg.Body = Message.Trim() & vbCrLf

                If attachments IsNot Nothing Then
                    For Each ai As AttachmentInfo In attachments
                        Dim a As New Attachment(ai.bytesFileName, ai.mimeType)
                        attachmentObjects.Add(a)
                        a.ContentDisposition.FileName = ai.attachmentFileName
                        MailMsg.Attachments.Add(a)
                    Next
                End If
                'MailMsg.IsBodyHtml = True
                'http://www.systemnetmail.com/faq/4.4.aspx

                'Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString("Here is an embedded image.<img src=cid:companylogo>", Nothing, "text/html")

                ''create the LinkedResource (embedded image)
                'Dim logo As New LinkedResource("c:\temp\logo.gif")
                'logo.ContentId = "companylogo"
                ''add the LinkedResource to the appropriate view
                'htmlView.LinkedResources.Add(logo)
                'Dim plainView As AlternateView = AlternateView.CreateAlternateViewFromString("This is my plain text content, viewable by those clients that don't support html", Nothing, "text/plain")
                ''add the views
                'mail.AlternateViews.Add(plainView)
                'mail.AlternateViews.Add(htmlView)

                'Smtpclient to send the mail message
                Dim SmtpMail As New SmtpClient
                SmtpMail.Send(MailMsg)
            Finally
                For Each a As Attachment In attachmentObjects
                    a.Dispose()
                Next
            End Try
        End Using
    End Sub

    Public Shared Sub SendEmailMessageOld(ByVal From As String, _
        ByVal [To] As String, ByVal Subject As String, _
        ByVal Message As String, Optional ByVal Bcc As String = Nothing, _
        Optional ByVal attachments As IList(Of AttachmentInfo) = Nothing, _
        Optional ByVal CC As String = Nothing)
        SendEmailMessageOld(From, makeList([To]), Subject, Message, _
            giif(Bcc Is Nothing, Nothing, makeList(Bcc)), _
            attachments, _
            giif(CC Is Nothing, Nothing, makeList(CC)))
    End Sub


    Public Class AttachmentInfo
        Public ReadOnly attachmentFileName As String
        Public ReadOnly bytesFileName As String
        Public ReadOnly mimeType As String

        Public Sub New(ByVal attachmentFilename As String, ByVal bytesFilename As String, ByVal mimeType As String)
            Me.attachmentFileName = attachmentFilename
            Me.bytesFileName = bytesFilename
            Me.mimeType = mimeType
        End Sub
    End Class

    Public Class MAddr
        Public ReadOnly emailAddress As String
        Public ReadOnly displayName As String
        Public Sub New(ByVal emailAddress As String, Optional ByVal displayName As String = Nothing)
            Me.emailAddress = emailAddress
            Me.displayName = displayName
        End Sub
    End Class

    Public Enum EmailType
        Text
        HTML
    End Enum

    Public Shared Function MAddrToMailAddress(ByVal a As MAddr) As MailAddress
        If a.displayName Is Nothing OrElse a.displayName = "" Then
            Return New MailAddress(a.emailAddress)
        Else
            Return New MailAddress(a.emailAddress, a.displayName)
        End If
    End Function

    'Private Shared Sub addAll(ByVal target As MailAddressCollection, ByVal items As List(Of MAddr))
    '    If items IsNot Nothing Then
    '        For Each item As MAddr In items
    '            target.Add(MAddrToMailAddress(item))
    '        Next
    '    End If
    'End Sub


    'Public Shared Sub SendEmail( _
    '        ByVal From As MAddr, _
    '        ByVal [To] As List(Of MAddr), _
    '        ByVal Subject As String, _
    '        ByVal Message As String, _
    '        Optional ByVal type As EmailType = EmailType.Text, _
    '        Optional ByVal CC As List(Of MAddr) = Nothing, _
    '        Optional ByVal Bcc As List(Of MAddr) = Nothing, _
    '        Optional ByVal attachments As List(Of AttachmentInfo) = Nothing, _
    '        Optional ByVal hostName As String = "Hopefully, this is well unlikely to appear in a url!£$%^*&")
    '    'check arguments
    '    If [To].Count = 0 Then
    '        Throw New Exception("Email has no to address set")
    '    End If

    '    'create the message
    '    Dim MailMsg As New MailMessage()

    '    'add the from, to, cc and bcc
    '    addAll(MailMsg.To, [To])
    '    MailMsg.From = MAddrToMailAddress(From)
    '    addAll(MailMsg.CC, CC)
    '    addAll(MailMsg.Bcc, Bcc)

    '    'set subject and body
    '    MailMsg.Subject = Subject
    '    MailMsg.Body = Message

    '    'set email encoding
    '    MailMsg.BodyEncoding = System.Text.Encoding.Default
    '    'set whether html mail
    '    MailMsg.IsBodyHtml = (type = EmailType.HTML)

    '    'if html mail then fix up embedded images
    '    If type = EmailType.HTML Then
    '        fixUpEmbeddedImages(MailMsg)
    '    End If

    '    'add attachments
    '    If attachments IsNot Nothing Then
    '        For Each ai As AttachmentInfo In attachments
    '            Dim a As New Attachment(ai.bytesFileName, ai.mimeType)
    '            a.ContentDisposition.FileName = ai.attachmentFileName
    '            MailMsg.Attachments.Add(a)
    '        Next
    '    End If
    '    'Smtpclient to send the mail message
    '    Dim SmtpMail As New SmtpClient
    '    SmtpMail.Send(MailMsg)
    'End Sub


    'Private Shared ReadOnly imageTagRegex As New Regex("<img(.(?!/>))*\s*/\s*>", RegexOptions.Multiline Or RegexOptions.IgnoreCase)
    'Private Shared ReadOnly imageSrcRegex As New Regex("^(?<pre>.*)src=(""(?<path>[^""]*)""|'(?<path>[^']*)')(?<post>.*)$", RegexOptions.Multiline Or RegexOptions.IgnoreCase)
    'Private Shared ReadOnly urlHostRegex As New Regex("^http://(?<host>[^/]*)(?<path>/.*)$")

    'Private Shared Sub fixUpEmbeddedImages(ByVal MailMsg As MailMessage, ByVal hostName As String)
    '    For Each m As Match In imageTagRegex.Matches(MailMsg.Body)
    '        Dim sm As Match = imageSrcRegex.Match(m.Value)
    '        log.Info("fixing image " & m.Value)
    '        If Not sm.Success Then
    '            log.Info("couldn't find src tag in " & m.Value)
    '        Else
    '            Dim src As String = sm.Groups.Item("path").Value
    '            'check if already replaced or http link

    '            'hack - if the link is to the current server then replace it
    '            'with a local link
    '            If isLocalUrl(src, hostName) Then
    '                src = convertToLocalUrl(src)
    '            End If

    '            If src.IndexOf("cid:") <> -1 OrElse src.IndexOf("http://") <> -1 Then
    '                log.Info("skipping img src: " & src)
    '            Else
    '                'fix up image tag
    '                'add image to email first
    '                Dim localFilepath As String = Server.MapPath(src)
    '                Dim localFilename As String = PathGetFileName(localFilepath)
    '                If Not fileExists(localFilepath) Then
    '                    Throw New FriendlyException("Couldn't find file " & src)
    '                End If
    '                oBP = oCDo.AddRelatedBodyPart(localFilepath, localFilename, _
    '                    CDO.CdoReferenceType.cdoRefTypeId, strUserName, strEmailPassword)
    '                oBP.Fields.Item("urn:schemas:mailheader:Content-ID").Value = "<" & localFilename & ">"
    '                'fix up the tag in the html
    '                Dim newTag As String = sm.Groups.Item("pre").Value & "src=""cid:" & _
    '                    localFilename & """" & sm.Groups.Item("post").Value()
    '                MailMsg.Body = MailMsg.Body.Replace(m.Value, newTag)
    '            End If
    '        End If
    '    Next
    'End Sub

    'Private Shared Function isLocalUrl(ByVal url As String, ByVal hostName As String) As Boolean
    '    Dim m As Match = urlHostRegex.Match(url)
    '    If Not m.Success Then
    '        Return False
    '    End If
    '    Return inList(m.Groups.Item("host").Value.ToLower, _
    '        "localhost", "127.0.0.1", hostName)
    'End Function

    'Private Shared Function convertToLocalUrl(ByVal url As String) As String
    '    Dim m As Match = urlHostRegex.Match(url)
    '    If Not m.Success Then
    '        Throw New Exception("Called convert to local url on " & url & " which doesn't match the url regex")
    '    End If
    '    'strip the first folder from the url since this will be the breathe url
    '    'e.g. /breathe/email_image_store/Image/stuff.jpg
    '    'we want to convert this to
    '    'email_image_store/Image/stuff.jpg
    '    'so the path is relative to the web app root url
    '    Return stripFirstFolder(m.Groups.Item("path").Value)
    'End Function

    'Private Shared Function stripFirstFolder(ByVal f As String) As String
    '    If f.Length < 1 Then
    '        'url too short, leave it alone
    '        Return f
    '    End If
    '    'skip the first "/"
    '    Dim i As Integer = f.IndexOf("/", 1)
    '    If i = -1 Then
    '        'can't find the first folder, so leave it alone
    '        Return f
    '    End If
    '    Return f.Substring(i + 1)
    'End Function

End Class
