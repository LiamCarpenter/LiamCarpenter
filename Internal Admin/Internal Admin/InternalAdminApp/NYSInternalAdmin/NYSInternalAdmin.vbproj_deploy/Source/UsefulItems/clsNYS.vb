Option Strict Off

Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Net
Imports MonoSoftware.Web.Dialogs
Imports system.net.Mail

Public Class clsNYS
    Inherits System.Web.UI.Page

    Private Shared ReadOnly className As String

    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub

    Public Shared Function toCSVCell(ByVal s As String) As String
        If s.IndexOf(",") <> -1 Or _
            s.IndexOf("""") <> -1 Or _
            s.IndexOf(vbCr) <> -1 Or _
            s.IndexOf(vbLf) <> -1 Then
            Return """" & s.Replace("""", """""") & """"
        Else
            Return s
        End If
    End Function

    Public Function setUser() As String
        Using New clslogger(log, className, "setUser")

            Dim oUser As NysDat.clsSystemNYSUser
            oUser = CType(Session.Item("loggedinuser"), NysDat.clsSystemNYSUser)

            If oUser IsNot Nothing Then
                Return "Current user: " & oUser.Systemnysuserfirstname & " " & oUser.Systemnysuserlastname
            Else
                If Session.Item("LoginType") = "CLIENT" Then
                    Return ""
                Else
                    Return "ERROR"
                End If
            End If
        End Using
    End Function

    Public Structure getBossIDs
        Public strBossID1, strBossID2 As String
        Public intCompanyID As Integer
    End Structure

    Public Shared Function checkClientName(ByVal pstrName As String) As getBossIDs
        Using New clslogger(log, className, "checkClientName")
            Dim ret As New getBossIDs
            ret.strBossID2 = "XXXXXX"
            If pstrName.ToLower = "lv=" Then
                ret.strBossID1 = "LV"
            ElseIf pstrName.ToLower = "cima" Then
                ret.strBossID1 = "CIMA"
                ret.strBossID2 = "CIMATRAVEL"
            ElseIf pstrName.ToLower = "university of york" Then
                ret.strBossID1 = "UOY"
                ret.strBossID2 = "UOYR"
            ElseIf pstrName.ToLower = "ioko" Then
                ret.strBossID1 = "IOKO"
            ElseIf pstrName.ToLower = "macmillan" Then
                ret.strBossID1 = "MACIALLAN"
                ret.strBossID2 = "MACIALLAN1"
            ElseIf pstrName.ToLower = "ogc - anchor trust" Or pstrName.ToLower = "bs - anchor trust" Then
                ret.strBossID1 = "ANCHOR"
            ElseIf pstrName.ToLower = "e-act" Then
                ret.strBossID1 = "EACT"

                'R2.17 CR - keep the old NPSA Boss Code
            ElseIf pstrName.ToLower = "ncas" Then
                ret.strBossID1 = "NPSA"

                'R2.21.3 CR - add in support for NHS Litigation Authority
                'NPSA renamed to NCAS
                'NCAS renamed to NHS Litigation Authority
                'so need to keep the old BOSS code for older data support
            ElseIf pstrName.ToLower = "nhs litigation authority" Then
                ret.strBossID1 = "NPSA"
                ret.strBossID2 = "NHSLITA"

            ElseIf pstrName.ToLower = "npsa" Then
                ret.strBossID1 = "NPSA"
            ElseIf pstrName.ToLower = "east of england" Then
                ret.strBossID1 = "EOE"
            ElseIf pstrName.ToLower = "nys demo group" Then
                ret.strBossID1 = "ANCHOR"
            ElseIf pstrName.ToLower = "fera" Then
                ret.strBossID1 = "FERA"
            ElseIf pstrName.ToLower = "de montfort university" Then
                ret.strBossID1 = "DMU"
                ret.strBossID1 = "DME"
            ElseIf pstrName.ToLower = "bs-dwp" Then
                ret.strBossID1 = "DWP"
            ElseIf pstrName.ToLower = "bs-ho" Then
                ret.strBossID1 = "HO"
            ElseIf pstrName.ToLower = "lincs council" Then
                ret.strBossID1 = "NLC"
                ret.strBossID2 = "NELC"
            ElseIf pstrName.ToLower = "herefordshire council".ToLower Then
                ret.strBossID1 = "HERECC"
            ElseIf pstrName.ToLower = "NHS Institute for Innovation and Improvement".ToLower Then
                ret.strBossID1 = "NHSINST"
            ElseIf pstrName.ToLower = "NHS Business Services Authority".ToLower Then
                ret.strBossID1 = "NHSBSA"
            ElseIf pstrName.ToLower = "University Hospital of South Manchester".ToLower Then
                ret.strBossID1 = "UHSM"
            ElseIf pstrName.ToLower = "National School of Government".ToLower Then
                ret.strBossID1 = "NSG"
            ElseIf pstrName.ToLower = "BS-TCS".ToLower Then
                ret.strBossID1 = "TCS"
            ElseIf pstrName.ToLower = "environment agency".ToLower Then
                ret.strBossID1 = "EA"
                ret.strBossID2 = "EAHRSC"

                'R2.17 CR
            ElseIf pstrName.ToLower = "natural england" Then
                ret.strBossID1 = "XXX"
                ret.strBossID2 = "XXX"

            ElseIf pstrName.ToLower = "O2".ToLower Then
                ret.strBossID1 = "O2"
            ElseIf pstrName.ToLower = "HomeGroup".ToLower Then
                ret.strBossID1 = "HOME"
                ret.strBossID1 = "HOMESTONE"
            ElseIf pstrName.ToLower = "SHA - Ambulance Radio Programme".ToLower Then
                ret.strBossID1 = "SHAARP"
            Else
                ret.strBossID1 = ""
            End If

            ret.intCompanyID = 0
            'now do One stop shop clients
            Dim strDescription As String = getConfig("OSSCompanies")
            Dim list As ICollection(Of String) = Split(strDescription, ";")
            For Each file As String In list
                If file <> "" Then
                    If file.ToLower = pstrName.ToLower Then
                        ret.intCompanyID = NysDat.clsCompanyDat.companyIDGet(file)
                        Exit For
                    End If
                End If
            Next

            Return ret
        End Using
    End Function

    Public Shared Function getMonthAsString(ByVal intmonth As Integer) As String
        If intmonth = 1 Then
            Return "January"
        ElseIf intmonth = 2 Then
            Return "February"
        ElseIf intmonth = 3 Then
            Return "March"
        ElseIf intmonth = 4 Then
            Return "April"
        ElseIf intmonth = 5 Then
            Return "May"
        ElseIf intmonth = 6 Then
            Return "June"
        ElseIf intmonth = 7 Then
            Return "July"
        ElseIf intmonth = 8 Then
            Return "August"
        ElseIf intmonth = 9 Then
            Return "September"
        ElseIf intmonth = 10 Then
            Return "October"
        ElseIf intmonth = 11 Then
            Return "November"
        ElseIf intmonth = 12 Then
            Return "December"
        Else
            Return ""
        End If
    End Function

    Public Shared Function getMonthAsShortString(ByVal intmonth As Integer) As String
        If intmonth = 1 Then
            Return "Jan"
        ElseIf intmonth = 2 Then
            Return "Feb"
        ElseIf intmonth = 3 Then
            Return "Mar"
        ElseIf intmonth = 4 Then
            Return "Apr"
        ElseIf intmonth = 5 Then
            Return "May"
        ElseIf intmonth = 6 Then
            Return "Jun"
        ElseIf intmonth = 7 Then
            Return "Jul"
        ElseIf intmonth = 8 Then
            Return "Aug"
        ElseIf intmonth = 9 Then
            Return "Sep"
        ElseIf intmonth = 10 Then
            Return "Oct"
        ElseIf intmonth = 11 Then
            Return "Nov"
        ElseIf intmonth = 12 Then
            Return "Dec"
        Else
            Return ""
        End If
    End Function

    Public Const emailRegex As String = "^((?>[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+\x20*|""((?=[\x01-\x7f])" & _
                                "[^""\\]|\\[\x01-\x7f])*""\x20*)*(?<angle><))?((?!\.)(?>\.?[a-zA-Z" & _
                                "\d!#$%&'*+\-/=?^_`{|}~]+)+|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f]" & _
                                ")*"")@(((?!-)[a-zA-Z\d\-]+(?<!-)\.)+[a-zA-Z]{2,}|\[(((?(?<!\[)\.)" & _
                                "(25[0-5]|2[0-4]\d|[01]?\d?\d)){4}|[a-zA-Z\d\-]*[a-zA-Z\d]:((?=[\" & _
                                "x01-\x7f])[^\\\[\]]|\\[\x01-\x7f])+)\])(?(angle)>)$"

    Protected Shared log As log4net.ILog = _
    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Shared logPostcode As log4net.ILog = _
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName & ".logPostcode")

    Private Shared configurationAppSettings As New System.Configuration.AppSettingsReader

    Public Shared Function getConfig(ByVal key As String) As String
        Return CType(configurationAppSettings.GetValue(key, GetType(System.String)), String)
    End Function

    'call this to add the client side script for setting textbox default buttons
    Public Shared Sub setupDefaultButtonScript(ByVal client As ClientScriptManager)
        Dim theScript As String = _
                "<SCRIPT language=""javascript"">" & vbCrLf & _
                "<!--" & vbCrLf & _
                "function fnTrapKD(btn, event){" & vbCrLf & _
                " if (document.all){" & vbCrLf & _
                "  if (event.keyCode == 13){" & vbCrLf & _
                "   event.returnValue=false;" & vbCrLf & _
                "   event.cancel = true;" & vbCrLf & _
                "   btn.click();" & vbCrLf & _
                "  }" & vbCrLf & _
                " }" & vbCrLf & _
                " else if (document.getElementById){" & vbCrLf & _
                "  if (event.which == 13){" & vbCrLf & _
                "   event.returnValue=false;" & vbCrLf & _
                "   event.cancel = true;" & vbCrLf & _
                "   btn.click();" & vbCrLf & _
                "  }" & vbCrLf & _
                " }" & vbCrLf & _
                " else if(document.layers){" & vbCrLf & _
                "  if(event.which == 13){" & vbCrLf & _
                "   event.returnValue=false;" & vbCrLf & _
                "   event.cancel = true;" & vbCrLf & _
                "   btn.click();" & vbCrLf & _
                "  }" & vbCrLf & _
                "}" & vbCrLf & _
                "}" & vbCrLf & _
                "// -->" & vbCrLf & _
                "</SCRIPT>"
        client.RegisterStartupScript(GetType(String), "ForceDefaultToScript", theScript)
    End Sub

    Public Shared Sub SetDefaultButton(ByVal textControl As TextBox, ByVal defaultButton As Button)
        textControl.Attributes.Add("onkeydown", "fnTrapKD(" + defaultButton.ClientID + ",event)")
    End Sub

    Public Shared Sub SetDefaultImageButton(ByVal textControl As TextBox, ByVal defaultButton As ImageButton)
        textControl.Attributes.Add("onkeydown", "fnTrapKD(" + defaultButton.ClientID + ",event)")
    End Sub

    Public Shared Function isPasswordValid(ByVal pstrPassword As String, Optional ByVal pstrUsername As String = "") As String

        Dim strRet As String
        Dim ascchar, i, intNoUsername, intNoChars, intnoUppercaseAZ, intnolowercaseaz, intnodigits, intnonnonalpha As Integer

        intnoUppercaseAZ = 0
        intnolowercaseaz = 0
        intnodigits = 0
        intnonnonalpha = 0
        intNoUsername = 0

        strRet = ""
        intNoChars = pstrPassword.Length()

        If intNoChars < 8 Then
            strRet = "Password must be at least 8 characters long!"
        End If

        For i = 0 To (intNoChars - 1)
            ascchar = Asc(pstrPassword.Substring(i, 1))

            If ascchar >= 65 And ascchar <= 90 Then
                'uppercase
                intnoUppercaseAZ = 1
            ElseIf ascchar >= 97 And ascchar <= 122 Then
                'Lowercase
                intnolowercaseaz = 1
            ElseIf ascchar >= 48 And ascchar <= 57 Then
                'Digits
                intnodigits = 1
            ElseIf ascchar >= 48 And ascchar <= 57 Then
                'Non alpha
                intnonnonalpha = 1
            End If

            If (pstrUsername > "") And (pstrUsername.Contains(pstrPassword.Substring(i, 1))) Then
                intNoUsername = intNoUsername + 1
            End If
        Next

        If (intnoUppercaseAZ + intnolowercaseaz + intnodigits + intnonnonalpha) < 3 Then
            strRet = "Password does not conform to policy, please review!"
        End If

        Return strRet

    End Function

    Public Shared Function labelText(ByVal pstrName As String, ByVal pintstartday As Integer, _
                                    ByVal pintstartmonth As Integer, ByVal pintstartyear As Integer, _
                                    ByVal pintendday As Integer, ByVal pintendmonth As Integer, _
                                    ByVal pintendyear As Integer) As String

        If pintstartyear = pintendyear Then
            If pintstartmonth = pintendmonth Then
                If pintstartday = pintendday Then
                    Return pstrName & " - " & CStr(pintstartday) & " " & getMonthAsShortString(pintstartmonth) & " " & CStr(pintstartyear)

                Else
                    Return pstrName & " - " & CStr(pintstartday) & " to " & _
                                       CStr(pintendday) & " " & getMonthAsShortString(pintendmonth) & " " & CStr(pintendyear)

                End If
            Else
                Return pstrName & " - " & _
                                       CStr(pintstartday) & " " & getMonthAsShortString(pintstartmonth) & " to " & _
                                        CStr(pintendday) & " " & getMonthAsShortString(pintendmonth) & " " & CStr(pintendyear)
            End If
        Else
            Return pstrName & " - " & _
                                   CStr(pintstartday) & " " & getMonthAsShortString(pintstartmonth) & " " & CStr(pintstartyear) & " to " & _
                                    CStr(pintendday) & " " & getMonthAsShortString(pintendmonth) & " " & CStr(pintendyear)
        End If


    End Function

    Public Shared Function hashpassword(ByVal pstrPassword As String, ByVal pstrEncryption As String) As String
        
        Return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pstrPassword, _
                        pstrEncryption)

    End Function

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

    Public Shared Sub handleexception(ByVal ex As Exception, ByVal pstrformname As String, ByVal ppage As System.Web.UI.Page)
        If TypeOf ex Is System.Threading.ThreadAbortException Then
            Return
        End If
        log.Error(ex.Message, ex)
        Dim strerrortext As String = ("Error in " & pstrformname & " - " & ex.Message & vbNewLine & _
            ex.StackTrace).Replace(System.Environment.NewLine, "-")

        If TypeOf ex Is EvoFriendlyException Then
            Dim efe As EvoFriendlyException = DirectCast(ex, EvoFriendlyException)
            Dialogs.ShowMessage(ppage, efe.Message, efe.title, TDlgType.mtCustom, 300)
        Else
            Dialogs.ShowMessage(ppage, strerrortext, "Check Details", TDlgType.mtCustom, 300)
        End If
    End Sub

    Public Shared Sub dumpException(ByVal ex As Exception)
        System.Diagnostics.Debug.WriteLine(ex.Message)
        System.Diagnostics.Debug.WriteLine(ex.StackTrace)
        While (Not ex.InnerException Is Nothing)
            dumpException(ex.InnerException)
        End While
    End Sub

    Public Shared Sub makeFolderExist(ByVal folder As String)
        If Not IO.Directory.Exists(folder) Then
            IO.Directory.CreateDirectory(folder)
        End If
    End Sub

    Public Shared Function readText(ByVal pstrfile As String) As String
        Using New clslogger(log, className, "readText")
            Dim ofile As New System.IO.StreamReader(pstrfile)
            readText = ofile.ReadToEnd
            ofile.Close()
        End Using
    End Function

    'R2.17 SA - not needed now
    'Public Shared Function SendInvoicesEmailMessage(ByVal emailFrom As String, ByVal emailTo As String, ByVal emailSubject As String, _
    '                                        ByVal emailMessage As String, ByVal emailcc1 As String, ByVal emailcc2 As String, _
    '                                        ByVal emailBcc As String, ByVal pAttchList As List(Of String)) As Boolean
    '    'For each to address create a mail message
    '    Dim MailMsg As New MailMessage(New MailAddress(emailFrom.Trim()), New MailAddress(emailTo))
    '    If emailcc1 <> "" Then
    '        Dim ccaddr As New MailAddress(emailcc1)
    '        MailMsg.CC.Add(ccaddr)
    '    End If
    '    If emailcc2 <> "" Then
    '        Dim ccaddr As New MailAddress(emailcc2)
    '        MailMsg.CC.Add(ccaddr)
    '    End If
    '    If emailBcc <> "" Then
    '        Dim bccaddr As New MailAddress(emailBcc)
    '        MailMsg.Bcc.Add(bccaddr)
    '    End If
    '    MailMsg.BodyEncoding = Encoding.Default
    '    MailMsg.Subject = emailSubject.Trim()
    '    MailMsg.Body = emailMessage.Trim() & vbCrLf
    '    MailMsg.IsBodyHtml = True
    '    For Each strAttachPath As String In pAttchList
    '        If strAttachPath <> "" Then
    '            Dim mailAttach As New Attachment(strAttachPath)
    '            MailMsg.Attachments.Add(mailAttach)
    '        End If
    '    Next
    '    Dim SmtpMail As New SmtpClient
    '    Try
    '        SmtpMail.Send(MailMsg)
    '        Return True
    '    Catch ex As Exception
    '        log.Error(ex.Message)
    '        'did not pass any attachment
    '        SendEmailMessageInternal(emailFrom, emailTo, "Help", "", "")
    '        Return False
    '    End Try
    'End Function

    Public Shared Function SendEmailMessage(ByVal emailFrom As String, ByVal emailTo As String, ByVal emailSubject As String, _
                                            ByVal emailMessage As String, ByVal pstrAttachPath As String, ByVal emailcc1 As String, ByVal emailcc2 As String, _
                                            ByVal emailBcc As String, ByVal pstrO2HQ As String, ByVal pstrO2UK As String, ByVal CimaFilesToAttachFolder As String) As Boolean
        'R2.20C CR - Client Statements
        'Dim MailMsg As New MailMessage(New MailAddress(emailFrom.Trim()), New MailAddress(emailTo))
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

        MailMsg.BodyEncoding = Encoding.Default
        MailMsg.Subject = emailSubject.Trim()
        MailMsg.Body = emailMessage.Trim() & vbCrLf
        MailMsg.IsBodyHtml = True

        If pstrAttachPath <> "" Then
            Dim mailAttach As New Attachment(pstrAttachPath)
            MailMsg.Attachments.Add(mailAttach)
        End If
        If pstrO2HQ <> "" Then
            Dim mailAttach1 As New Attachment(pstrO2HQ)
            MailMsg.Attachments.Add(mailAttach1)
        End If
        If pstrO2UK <> "" Then
            Dim mailAttach2 As New Attachment(pstrO2UK)
            MailMsg.Attachments.Add(mailAttach2)
        End If

        If CimaFilesToAttachFolder <> "" Then
            For Each strfileCREDITS As String In IO.Directory.GetFiles(CimaFilesToAttachFolder)
                Dim strJustFileCREDITS As String = New System.IO.FileInfo(strfileCREDITS).Name

                strJustFileCREDITS = strJustFileCREDITS.Trim

                If strJustFileCREDITS <> "" Then
                    Dim mailAttach3 As New Attachment(CimaFilesToAttachFolder & strJustFileCREDITS)
                    MailMsg.Attachments.Add(mailAttach3)
                End If
            Next
        End If

        Dim SmtpMail As New SmtpClient
        Try
            SmtpMail.Send(MailMsg)
            Return True
        Catch ex As Exception
            log.Error(ex.Message)
            SendEmailMessageInternal(emailFrom, emailTo, "Help", "", pstrAttachPath)
            Return False
        End Try

    End Function

    Public Shared Function SendEmailMessageInternal(ByVal emailFrom As String, _
            ByVal emailTo As String, ByVal emailSubject As String, _
            ByVal emailMessage As String, ByVal pstrAttachPath As String) As Boolean
        'For each to address create a mail message
        Dim MailMsg As New MailMessage(New MailAddress(emailFrom.Trim()), New MailAddress("nick.massarella@nysgroup.com"))

        MailMsg.BodyEncoding = Encoding.Default
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
            log.Error(ex.Message)
            Return False
        End Try
    End Function

    Public Shared Function getRandomPassword() As String
        Dim strPassword As String
        Try
            Dim strAlphabet As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            Dim randRandom As System.Random
            Dim intPosition, intCount As Integer

            randRandom = New System.Random()
            strPassword = ""
            For intCount = 1 To 8
                intPosition = randRandom.Next(0, 25)
                strPassword = strPassword & strAlphabet.Substring(intPosition, 1)
            Next

            strPassword = strPassword

            Return strPassword
        Catch ex As Exception
            strPassword = "Failed"
            Return strPassword
        End Try
    End Function

    Public Shared Function Marc(ByVal bf0, ByVal n, ByVal phi0, ByVal phi) As Double
        Dim dblMarc As Object

        dblMarc = bf0 * (((1 + n + ((5 / 4) * (n * n)) + ((5 / 4) * (n * n * n))) * (phi - phi0)) - (((3 * n) + (3 * (n * n)) + ((21 / 8) * (n * n * n))) * (Math.Sin(phi - phi0)) * (Math.Cos(phi + phi0))) + ((((15 / 8) * (n * n)) + ((15 / 8) * (n * n * n))) * (Math.Sin(2 * (phi - phi0))) * (Math.Cos(2 * (phi + phi0)))) - (((35 / 24) * (n * n * n)) * (Math.Sin(3 * (phi - phi0))) * (Math.Cos(3 * (phi + phi0)))))
        Return dblMarc

    End Function

    Public Shared Sub saveHTMLfile(ByVal pintgroupid As Integer, _
                            ByVal pstrgroupname As String, _
                            ByVal pintcompanyid As Integer, _
                            ByVal pstrcompanyname As String, _
                            ByVal pintcontactid As Integer, _
                            ByVal pstrlastname As String, _
                            ByVal pstrfilename As String, _
                            ByVal poServer As System.Web.HttpServerUtility, _
                            ByVal pstrvalue As String)

        Dim ofiler As System.IO.StreamWriter
        Dim strpath As String

        If pintcontactid > 0 Then
            strpath = "userdocs/" & pintgroupid & "-" & pstrgroupname & _
                        "/" & pintcompanyid & "-" & pstrcompanyname & _
                        "/" & pintcontactid & "-" & pstrlastname

        ElseIf pintcompanyid > 0 Then
            strpath = "userdocs/" & pintgroupid & "-" & pstrgroupname & _
                        "/" & pintcompanyid & "-" & pstrcompanyname
        Else
            strpath = "userdocs/" & pintgroupid & "-" & pstrgroupname
        End If

        makeFolderExist(poServer.MapPath(strpath & "/emails"))

        ofiler = New System.IO.StreamWriter(poServer.MapPath(strpath & "/emails/" & _
                                        pstrfilename), False)

        ofiler.Write(pstrvalue)
        ofiler.Flush()
        ofiler.Close()
    End Sub

    Public Function RestoreScrollPosition(ByVal pstrScrollTop As String, ByVal pstrControl As String) As String
        RestoreScrollPosition = "<script language='JavaScript'>function RestoreScrollPosition() { var a = document.getElementById('" & pstrControl & "'); a.scrollTop='" & pstrScrollTop & "';}; window.onload=RestoreScrollPosition;</script>"
    End Function

    Public Shared Function dblToString(ByVal d As Double) As String
        If d > 999.99 Then
            Return Format(d, "0,000.00")
        Else
            Return Format(d, "0.00")
        End If
    End Function

    Public Shared Function twoZeroPad(ByVal i As Integer) As String
        Return CStr(i).PadLeft(2, "0"c)
    End Function

    Public Shared Function escapeXml(ByVal text As String) As String
        Return text.Replace("&", "&amp;"). _
            Replace("<", "&lt;"). _
            Replace(">", "&gt;"). _
            Replace("'", "&apos;"). _
            Replace("""", "&quot;")
    End Function

    Public Shared Function runInvoiceNumberCheck(ByVal pstrStartDate As String, ByVal pstrEndDate As String, ByVal pstrClientName As String) As String
        Using New clslogger(log, className, "createFile")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            dBaseConnection.Open()
            Dim dinosaurs As New List(Of dataImport)
            Dim intcount As Integer = 0
            Dim dtStart As New Date(CDate(pstrStartDate).Year, CDate(pstrStartDate).Month, CDate(pstrStartDate).Day, 0, 0, 0, 0)
            Dim dtEnd As New Date(CDate(pstrEndDate).Year, CDate(pstrEndDate).Month, CDate(pstrEndDate).Day, 0, 0, 0, 0)

            Dim retDetails As New getBossIDs
            retDetails = checkClientName(pstrClientName)

            'R? CR - FIX, added to select: tot_invno like 'N%'
            'without this unprinted invoices were also coming back & causing errors in selectInvtotRecord > BOSSinvmainCdate as it isn't in SQL
            Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT tot_invno " & _
                                                            "FROM Invtot where tot_invdt between {^" & dtStart.ToString("yyyy/MM/dd") & "} and {^" & dtEnd.ToString("yyyy/MM/dd") & "} " & _
                                                            "and (tot_custid like '" & retDetails.strBossID1 & "' or tot_custid like '" & retDetails.strBossID2 & "') " & _
                                                            "and tot_invno like 'N%'", dBaseConnection)

            Dim dBaseDataReader As System.Data.OleDb.OleDbDataReader = dBaseCommand.ExecuteReader()

            While dBaseDataReader.Read
                If clsBoss.selectInvMainRecords(dBaseDataReader("tot_invno").ToString) Then
                    If clsBoss.selectInvtotRecord(dBaseDataReader("tot_invno").ToString) Then
                        If clsBoss.selectInvRouteRecords(dBaseDataReader("tot_invno").ToString) Then

                        Else
                            Return "ERROR: selectInvRouteRecords error, try again, otherwise see Dev Team"
                        End If
                    Else
                        Return "ERROR: selectInvtotRecord error, try again, otherwise see Dev Team"
                    End If
                Else
                    Return "ERROR: selectInvMainRecords error, try again, otherwise see Dev Team"
                End If
            End While

            dBaseConnection.Close()

            Return ""
        End Using
    End Function

End Class
