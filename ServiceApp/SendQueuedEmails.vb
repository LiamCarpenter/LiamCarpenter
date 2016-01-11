Option Strict On
Option Explicit On

Imports NYS_MailService.eventlogger
Imports EvoUtilities.ConfigUtils
Imports system.text.regularexpressions
Imports microsoft.VisualBasic

Public Class SendQueuedEmails

    Private Shared ReadOnly log As log4net.ILog = _
        log4net.LogManager.GetLogger(System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName)

    Private Shared ReadOnly TestingToAddress As String = getOptionalConfig("testingOutgoingEmailAddress")

    Private Shared dateRegex As New Regex( _
        "^(?<day>\d\d?)/(?<month>\d\d?)/(?<year>\d\d(\d\d)?)$")

    Private Shared Function toDate(ByVal s As String) As Date
        'work around for when the f'ing computer
        'decides to parse dates in american format

        Dim m As Match = dateRegex.Match(s)
        If Not m.Success Then
            Throw New Exception("couldn't parse date '" & s & "'" & _
                ", expecting dd/mm/yyyy or dd/mm/yy")
        End If
        Return New Date(CInt(m.Groups.Item("year").Value()), _
            CInt(m.Groups.Item("month").Value()), _
            CInt(m.Groups.Item("day").Value()))
    End Function

    Public Shared Sub checkEmails()
        Try
            log.Debug("Processing emails")
            Dim oemailCol As New NYSDat.clsEmailPollDat
            Dim oEmails As Collection = oemailCol.Populate(0)
            Dim strHomePath As String = EvoUtilities.ConfigUtils.getConfig("HomeAbsolutepath")
            Dim intcount As Integer = 0
            For Each oEmail As NYSDat.clsEmailPollDat In oEmails
                Try
                    If Date.Now.DayOfWeek = DayOfWeek.Saturday Or Date.Now.DayOfWeek = DayOfWeek.Sunday Then
                        'don't bother to send
                        'to do add check to see how many were trying to go as there should be none
                        intcount = intcount + 1
                    Else
                        If toDate(oEmail.emailpollduedate) <= _
                            Microsoft.VisualBasic.Now() Then
                            If oEmail.mstremailpolladdressee = "" Then
                                Throw New Exception("To addess is empty")
                            End If
                            Dim sendTo As String = oEmail.mstremailpolladdressee
                            Dim body As String = oEmail.mstremailpollbody

                            Dim ocompany As New NysDat.clsCompanyDat
                            ocompany.Populate(oEmail.mintcompanyid, 0, "")
                            Dim strfrom As String = ocompany.mstrcompanyNYSemailbox
                            ocompany = Nothing

                            log.Info("sending email (" & oEmail.mintemailpollid & ") " & _
                                oEmail.mstremailpollsubject & _
                                " to " & sendTo)
                            SendEmail.send(strfrom, _
                                    sendTo, _
                                    oEmail.mstremailpollsubject, _
                                    body, "", "", _
                                    strHomePath)
                            If oEmail.mstremailpollsubject.StartsWith("Feedback requested regarding") Then
                                'need to update feedback sent date
                                saveFeedbackSent(oEmail.mintenquiryid, Format(Now, "dd/MM/yyyy"))
                            End If
                            Dim strdescription As String = "Trigger email sent to: " & oEmail.mstremailpolladdressee & " subject: " & oEmail.mstremailpollsubject
                            saveTriggerHistory(oEmail.mintenquiryid, oEmail.mintgroupid, oEmail.mintcompanyid, _
                                                oEmail.mstrgroupname, oEmail.mstrcompanyname, oEmail.mstremailpollbody, _
                                                strdescription)
                            If oEmail.delete() = False Then
                                Throw New Exception("clsEmailPollDat returned false")
                            End If
                        End If
                    End If
                Catch ex As Exception
                    'catch and log exceptions seperately for each email
                    logError(log, "email id " & oEmail.mintemailpollid, ex, "sendEmails")
                    'try to send an email alert
                    SendEmail.safeSend("sending email", oEmail.mstremailpollsubject & _
                        vbNewLine & oEmail.mstremailpollbody, ex)
                    'and then continue to try and send next email
                End Try
            Next
            If intcount > 0 Then
                log.Debug("Weekend email count = " & CStr(intcount))
            End If
            log.Debug("Finished Processing emails")
        Catch ex As Exception
            logError(log, ex, "checkEmails")
            SendEmail.safeSend("sending emails", "", ex)
        End Try
    End Sub

    Public Shared Sub saveFeedbackSent(ByVal pintenquiryid As Integer, _
                                    ByVal pstrdate As String)

        Dim oFeedback As New NysDat.clsFeedbackDat

        oFeedback.mintenquiryid = pintenquiryid
        oFeedback.mstrfeedbackdatewebformsent = pstrdate
        oFeedback.sent()

        oFeedback = Nothing

    End Sub

    Public Shared Sub saveTriggerHistory(ByVal pintenquiryid As Integer, _
                                    ByVal pintgroupid As Integer, _
                                    ByVal pintcompanyid As Integer, _
                                    ByVal pstrgroupname As String, _
                                    ByVal pstrcompanyname As String, _
                                    ByVal pstrbody As String, _
                                    ByVal pstrdescription As String)

        Dim oHistory As New NysDat.clsHistoryDat

        oHistory.minthistoryid = 0
        oHistory.mintareaid = pintenquiryid
        oHistory.mstrhistorydatecreated = Format(Date.Now, "dd/MM/yyyy")
        oHistory.minthistorytimehour = Date.Now.TimeOfDay.Hours
        oHistory.minthistorytimemin = Date.Now.TimeOfDay.Minutes
        oHistory.mstrhistorytype = "note"
        oHistory.mstrhistorydesription = pstrdescription
        oHistory.mstrhistorydueday = "0"
        oHistory.mstrhistoryuser = ""
        oHistory.mstrhistorystatus = "Complete"
        oHistory.mstrhistorydocumentname = ""
        oHistory.minthistoryarea = 1
        oHistory.mstrhistorybody = "1"
        oHistory.save()

        Dim strpath As String = EvoUtilities.ConfigUtils.getConfig("HomeAbsolutepath")
        strpath = strpath & "userdocs/" & CStr(pintgroupid) & "-" & _
                                        pstrgroupname & "/" & CStr(pintcompanyid) & _
                                        "-" & CStr(pstrcompanyname) & "/emails/body/"
        'save body doc
        Dim ofiler As New IO.StreamWriter(strpath & CStr(oHistory.minthistoryid) & ".htm", False)
        ofiler.Write("<html>" & pstrbody & "</html>")
        ofiler.Flush()
        ofiler.Close()
        oHistory = Nothing

    End Sub

End Class
