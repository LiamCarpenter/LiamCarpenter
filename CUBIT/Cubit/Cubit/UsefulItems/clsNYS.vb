Imports Microsoft.VisualBasic
Imports MonoSoftware.Web.Dialogs
Imports System
Imports System.io
Imports System.Web.ui.WebControls
Imports System.Web.Services
Imports System.Web.UI.Page
Imports System.Web.UI
Imports ReportDownloader
Imports DatabaseObjects
Imports EvoUtilities.ConfigUtils
Imports System.Net.Mail

''' <summary>
''' Class clsNYS
''' </summary>
''' <remarks>
''' Created 12/03/2009 Nick Massarella
''' Class created so all required classes can inherit for refraction of code
''' </remarks>
Public Class clsNYS
    Inherits System.Web.UI.Page

    Private Shared ReadOnly className As String

    ''' <summary>
    ''' Sub New
    ''' </summary>
    ''' <remarks>
    ''' Sets up logging for all pages
    ''' </remarks>
    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub

    Protected Shared log As log4net.ILog = _
      log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Const emailRegex As String = "^((?>[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+\x20*|""((?=[\x01-\x7f])" & _
                                 "[^""\\]|\\[\x01-\x7f])*""\x20*)*(?<angle><))?((?!\.)(?>\.?[a-zA-Z" & _
                                 "\d!#$%&'*+\-/=?^_`{|}~]+)+|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f]" & _
                                 ")*"")@(((?!-)[a-zA-Z\d\-]+(?<!-)\.)+[a-zA-Z]{2,}|\[(((?(?<!\[)\.)" & _
                                 "(25[0-5]|2[0-4]\d|[01]?\d?\d)){4}|[a-zA-Z\d\-]*[a-zA-Z\d]:((?=[\" & _
                                 "x01-\x7f])[^\\\[\]]|\\[\x01-\x7f])+)\])(?(angle)>)$"

    Public Shared Function sendemail(ByVal pstremailto As String, _
                              ByVal pstremailfrom As String, _
                              ByVal pstrsubject As String, _
                              ByVal pstrmessage As String) As String
        Using New clslogger(log, className, "sendemail")

            Dim strTo As String = pstremailto

            If getConfig("EmailTest") = "true" Then
                strTo = getConfig("EmailTestSendTo")
                pstrmessage = "Email would have been sent to: " & pstremailto & " - " & pstrmessage
            End If

            Dim MailMsg As New System.Net.Mail.MailMessage(New MailAddress(pstremailfrom), New MailAddress(strTo))
            MailMsg.BodyEncoding = Encoding.Default
            MailMsg.Subject = pstrsubject

            'R21 CR
            Dim strbody As String = "<html><font face='Verdana' size='2'>" & pstrmessage & "</font></html>"
            strbody = strbody.Replace("""", "'")
            MailMsg.IsBodyHtml = True
            MailMsg.Body = strbody.Trim() & vbCrLf

            Try
                'Smtpclient to send the mail message
                Dim SmtpMail As New SmtpClient
                SmtpMail.Send(MailMsg)
                SmtpMail = Nothing
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    log.Error("EMAIL SEND ERROR: " & ex.Message)
                    Return ex.Message
                End If
            End Try
            Return ""
        End Using
    End Function

    Public Shared Function checkBOSSReturnedFiles() As String
        Try
            Dim ch As checkBossFiles
            ch = New checkBossFiles
            Dim intStatusID As Integer = 0
            Dim strError As String = "OK"
            If Not ch.checker() Then
                strError = "ERROR"
            End If
            Return strError
        Catch ex As Exception
            log.Error(ex.Message & " - checkBOSSReturnedFiles")
            Return "ERROR"
        End Try
    End Function

    Public Shared Function readText(ByVal pstrfile As String) As String
        Using New clslogger(log, className, "readText")
            Dim ofile As New System.IO.StreamReader(pstrfile)
            readText = ofile.ReadToEnd
            ofile.Close()
        End Using
    End Function

    Public Shared Sub changeDropDowns(ByVal dd As System.Web.UI.WebControls.DropDownList, _
                                ByVal pstrvalue As String, ByVal pblnclear As Boolean, _
                                ByVal pblntext As Boolean)
        Using New clslogger(log, className, "changeDropDowns")
            If pblnclear = True Then
                For intcount As Integer = 0 To dd.Items.Count - 1
                    If dd.Items(intcount).Selected = True Then
                        dd.Items(intcount).Selected = False
                    End If
                Next
                dd.SelectedIndex = 0
            Else
                If pblntext = True Then
                    For intcount As Integer = 0 To dd.Items.Count - 1
                        If dd.Items(intcount).Text = pstrvalue Then
                            dd.SelectedIndex = intcount
                            Exit For
                        End If
                    Next
                Else
                    For intcount As Integer = 0 To dd.Items.Count - 1
                        If dd.Items(intcount).Value = pstrvalue Then
                            dd.SelectedIndex = intcount
                            Exit For
                        End If
                    Next
                End If
            End If
        End Using
    End Sub

    Public Shared Sub changelists(ByVal listControl As System.Web.UI.WebControls.ListBox, _
                                ByVal pstrvalue As String, ByVal pblnclear As Boolean)
        Using New clslogger(log, className, "clearlists")
            If pblnclear = True Then
                For intcount As Integer = 0 To listControl.Items.Count - 1
                    If listControl.Items(intcount).Selected = True Then
                        listControl.Items(intcount).Selected = False
                    End If
                Next
            Else
                For intcount As Integer = 0 To listControl.Items.Count - 1
                    If listControl.Items(intcount).Value = pstrvalue Then
                        listControl.SelectedIndex = intcount
                        Exit For
                    End If
                Next
            End If
        End Using
    End Sub

    'Public Shared Function toCSVCell(ByVal s As String) As String
    '    If s.IndexOf(",") <> -1 Or _
    '        s.IndexOf("""") <> -1 Or _
    '        s.IndexOf(vbCr) <> -1 Or _
    '        s.IndexOf(vbLf) <> -1 Then
    '        Return """" & s.Replace("""", """""") & """"
    '    Else
    '        Return s
    '    End If
    'End Function

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
            If IsNumeric(pArg) Then
                Return CDbl(pArg)
            Else
                Return 0
            End If
            Return CDbl(pArg)
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

    Public Shared Sub handleException(ByVal ex As Exception, _
            ByVal pstrformname As String, _
            Optional ByVal ppage As System.Web.UI.Page = Nothing)
        If TypeOf ex Is System.Threading.ThreadAbortException Then
            Return
        End If
        log.Error(ex.Message, ex)
        If TypeOf ex Is NYSFriendlyException Then
            Dim efe As NYSFriendlyException = DirectCast(ex, NYSFriendlyException)
            Dialogs.ShowMessage(ppage, efe.Message, efe.title, TDlgType.mtCustom, 300)
        Else
            Dim strerrortext As String = ("Error in " & pstrformname & " - " & ex.Message & vbNewLine & _
                ex.StackTrace).Replace(System.Environment.NewLine, "-")
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

    Public Shared Sub moveFolder(ByVal oldfolder As String, ByVal newfolder As String)
        If IO.Directory.Exists(oldfolder) Then
            IO.Directory.Move(oldfolder, newfolder)
        End If
    End Sub

    'Public Shared Function checkfile(ByVal fileUpload As System.Web.UI.WebControls.FileUpload, _
    '                                ByVal pbimport As Boolean) As String
    '    If pbimport = False Then
    '        Try
    '            checkfile = ""
    '            Dim strfile As String = clsFileUploadUtils.checkFilename(fileUpload)
    '        Catch ex As BadCharsInFilenameException
    '            checkfile = ex.Message
    '        Catch ex As FileNameMissingExtensionException
    '            checkfile = ex.Message
    '        Catch ex As FileNameWrongExtensionException
    '            checkfile = ex.Message
    '        End Try
    '    Else
    '        Try
    '            checkfile = ""
    '            Dim strfile As String = clsFileUploadUtils.checkImportFilename(fileUpload)
    '        Catch ex As BadCharsInFilenameException
    '            checkfile = ex.Message
    '        Catch ex As FileNameMissingExtensionException
    '            checkfile = ex.Message
    '        Catch ex As FileNameWrongImportExtensionException
    '            checkfile = ex.Message
    '        End Try
    '    End If
    'End Function

    Public Function RestoreScrollPosition(ByVal pstrScrollTop As String, _
                                            ByVal pstrControl As String) As String
        RestoreScrollPosition = "<script language='JavaScript'>function RestoreScrollPosition()" & _
                                "{ var a = document.getElementById('" & pstrControl & "'); " & _
                                "a.scrollTop='" & pstrScrollTop & "';}; " & _
                                "window.onload=RestoreScrollPosition;</script>"
    End Function

    Public Shared Function dblToString(ByVal d As Double) As String
        Return d.ToString("N2")
    End Function

    Public Shared Function twoZeroPad(ByVal i As Integer) As String
        Return CStr(i).PadLeft(2, "0"c)
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

    Public Shared Sub SetDefaultButton(ByVal textControl As WebControl, ByVal defaultButton As Button)
        textControl.Attributes.Add("onkeydown", "fnTrapKD(" + defaultButton.ClientID + ",event)")
    End Sub

    Public Shared Function checkPo(ByVal pPO As String, ByVal pblnCheck As Boolean, _
                                   ByVal pblnTryToExport As Boolean, ByVal pdblExportValue As Double) As String
        Using New clslogger(log, className, "checkPo")

            Dim dblPOValue As Double = clsPo.POValueGet(pPO)
            Dim strRet As String = ""
            Dim retVals As New clsPo.costs
            retVals = clsPo.allValuesGet(pPO)

            If retVals.bossTotal = -1 Then
                Throw New NYSFriendlyException("Could not retrieve live Boss PO value, please try again", "Info")
            End If

            If pblnTryToExport Then
                If dblPOValue >= retVals.bossTotal + pdblExportValue Then
                    strRet = ""
                Else
                    strRet = "<font color=red>PO: " & pPO & " has exceeded max value!" & "<br></font>" & _
                            "<b>PO value= " & CStr(Math.Round(dblPOValue, 2)) & "<br></b>" & _
                            "BOSS value= " & CStr(Math.Round(retVals.bossTotal, 2)) & "<br>" & _
                            "Confirmed value= " & CStr(Math.Round(retVals.confirmedMevis, 2)) & "<br>" & _
                            "Potential value= " & CStr(Math.Round(retVals.potentialMevis, 2)) & "<br>" & _
                            "Cubit Confirmed value= " & CStr(Math.Round(retVals.importedCubit, 2)) & "<br>" & _
                            "Cubit Potential value= " & CStr(Math.Round(retVals.potentialCubit, 2)) & "<br>" & _
                            "<b>Total current value= " & CStr(Math.Round(retVals.bossTotal + retVals.confirmedMevis + retVals.potentialMevis + retVals.importedCubit + retVals.potentialCubit, 2)) & "</b>"
                End If
            Else
                If dblPOValue > (retVals.bossTotal + retVals.confirmedMevis + retVals.potentialMevis + retVals.importedCubit + retVals.potentialCubit) Then
                    If Not pblnCheck Then
                        strRet = "PO: " & pPO & " is currently OK." & "<br>" & _
                                    "<b>PO value= " & CStr(Math.Round(dblPOValue, 2)) & "<br></b>" & _
                                    "BOSS value= " & CStr(Math.Round(retVals.bossTotal, 2)) & "<br>" & _
                                    "Mevis Confirmed value= " & CStr(Math.Round(retVals.confirmedMevis, 2)) & "<br>" & _
                                    "Mevis Potential value= " & CStr(Math.Round(retVals.potentialMevis, 2)) & "<br>" & _
                                    "Cubit Confirmed value= " & CStr(Math.Round(retVals.importedCubit, 2)) & "<br>" & _
                                    "Cubit Potential value= " & CStr(Math.Round(retVals.potentialCubit, 2)) & "<br>" & _
                                    "<b>Total current value= " & CStr(Math.Round(retVals.bossTotal + retVals.confirmedMevis + retVals.potentialMevis + retVals.importedCubit + retVals.potentialCubit, 2)) & "</b>"
                    Else
                        strRet = ""
                    End If
                ElseIf dblPOValue = (retVals.bossTotal + retVals.confirmedMevis + retVals.potentialMevis + retVals.importedCubit + retVals.potentialCubit) Then
                    If Not pblnCheck Then
                        strRet = "<font color=red>PO: " & pPO & " has reached max value!" & "<br></font>" & _
                                    "<b>PO value= " & CStr(Math.Round(dblPOValue, 2)) & "<br></b>" & _
                                    "BOSS value= " & CStr(Math.Round(retVals.bossTotal, 2)) & "<br>" & _
                                    "Confirmed value= " & CStr(Math.Round(retVals.confirmedMevis, 2)) & "<br>" & _
                                    "Potential value= " & CStr(Math.Round(retVals.potentialMevis, 2)) & "<br>" & _
                                    "Cubit Confirmed value= " & CStr(Math.Round(retVals.importedCubit, 2)) & "<br>" & _
                                    "Cubit Potential value= " & CStr(Math.Round(retVals.potentialCubit, 2)) & "<br>" & _
                                    "<b>Total current value= " & CStr(Math.Round(retVals.bossTotal + retVals.confirmedMevis + retVals.potentialMevis + retVals.importedCubit + retVals.potentialCubit, 2)) & "</b>"
                    Else
                        strRet = ""
                    End If
                Else
                    strRet = "<font color=red>PO: " & pPO & " has exceeded max value!" & "<br></font>" & _
                                    "<b>PO value= " & CStr(Math.Round(dblPOValue, 2)) & "<br></b>" & _
                                    "BOSS value= " & CStr(Math.Round(retVals.bossTotal, 2)) & "<br>" & _
                                    "Confirmed value= " & CStr(Math.Round(retVals.confirmedMevis, 2)) & "<br>" & _
                                    "Potential value= " & CStr(Math.Round(retVals.potentialMevis, 2)) & "<br>" & _
                                    "Cubit Confirmed value= " & CStr(Math.Round(retVals.importedCubit, 2)) & "<br>" & _
                                    "Cubit Potential value= " & CStr(Math.Round(retVals.potentialCubit, 2)) & "<br>" & _
                                    "<b>Total current value= " & CStr(Math.Round(retVals.bossTotal + retVals.confirmedMevis + retVals.potentialMevis + retVals.importedCubit + retVals.potentialCubit, 2)) & "</b>"
                End If
            End If

            Return strRet
        End Using
    End Function

   End Class
