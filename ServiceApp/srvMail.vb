Option Strict Off
Option Explicit On

Imports System.Data
Imports System.IO
Imports System.Linq
Imports System.Net.Mail
Imports System.Reflection
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Timers
Imports System.Xml
Imports System.Xml.Linq
Imports DatabaseObjects
Imports EvoUtilities
Imports log4net
Imports log4net.Config
Imports Microsoft.VisualBasic
Imports NysDat
Imports NYS_MailService.com.nyscorporate.onlineclient
Imports Renci.SshNet
Imports System.Xml.Serialization
Imports System.Collections.Generic
Imports Renci.SshNet.Messages.Authentication

'Imports ReportDownloader

'R2.23 CR

Public Class SrvMail
    '***********************************************
    'logging

    Private Shared ReadOnly Log As ILog =
                                LogManager.GetLogger(MethodBase.
                                                        GetCurrentMethod().DeclaringType.FullName)

    Dim server As Object

    Public Sub SetupLogging()
        Try
            XmlConfigurator.Configure(
                New FileInfo(ConfigUtils.getConfig("log4netconfig")))
            LogManager.GetLogger(
                MethodBase.GetCurrentMethod().
                                    DeclaringType).Info("Application Start-02-07-2014")
        Catch ex As Exception
            EventLogger.logError(Log, ex, "setupLogging")
        End Try
    End Sub

    '***********************************************
    'service start and stop
    Protected Overrides Sub OnStart(args() As String)
        Using New log4netUtils.MethodLogger(Log, "OnStart")
            SetupLogging()
            Try
                EventLogger.logInfo(Log, "Mail Service Started")

                'app.config
                '<add key="NYSDBBackupTime" value="11:54" />               '<add key="CUBITBackupTime" value="11:51" />
                '<add key="GIDSBackupTime" value="11:54" />                 '<add key="SSOBackupTime" value="11:51" />
                '<add key="ArchiveTime" value="21:00" />                    '<add key="CubitTime" value="07:30"/>
                '<add key="CubitFinishTime" value="12:00"/>                 '<add key="CubitAfternoonTime" value="16:30"/>
                '<add key="TriggerTime" value="00:10"/>

                If ConfigUtils.getConfig("FeraInvoiceOn") = "True" Then
                    EventLogger.logInfo(Log, "Fera Invoicing On")
                Else
                    EventLogger.logInfo(Log, "Fera Invoicing Off")
                End If

                SetupTriggerTime() 'triggerTime = 00:10 getConfig("TriggerTime"))

                'setupCubitTime() 'S3 ---- 3 times ----  07:30, 12:00, 16:30   (all going into variables)

                SetupArchiveTime() 'S2 ---- getConfig("ArchiveTime"))  21:00

                'setupCubitInvoiceTime()

                'R2.2 CR
                SetupRssExRatesTime()

                'R2.4 CR
                SetupCorpFeedbackTime()

                'R2.6 CR
                SetupInvoiceCheckTime()

                'R2.23 CR
                SetupLvUserReportTime()

                'check the mail settings
                If ConfigUtils.getConfig("EmailServerName").Trim = "" Then
                    sendEmails = False
                    Log.Warn("No email server set, sending emails disabled")
                End If

                If ConfigUtils.getConfig("EnquiryPopServer").Trim = "" Then
                    getEnquiries = False
                    Log.Warn("No enquiry pop server set, downloading enquiries disabled")
                End If

                If ConfigUtils.getConfig("FeedbackPopServer").Trim = "" Then
                    getFeedback = False
                    Log.Warn("No feedback pop server set, downloading feedback disabled")
                End If

                'setup the timer - gets the interval from the config file
                Dim strPollInterval As String = ConfigUtils.getConfig("PollSecondsInterval")
                Timer1.Interval = CInt(strPollInterval) * 1000
                'call wake up as soon as the service starts
                Timer1.Enabled = True

                DoWork()

            Catch ex As Exception
                EventLogger.logError(Log, ex, "OnStart")
                SendEmail.safeSend("starting email service", "", ex)
            End Try
        End Using
    End Sub

    Private sendEmails As Boolean = True
    Private ReadOnly checkEvolvi As Boolean = True
    Private ReadOnly checkEvolviP As Boolean = True
    Private getEnquiries As Boolean = True
    Private getFeedback As Boolean = True

    'R2.21 CR
    Private ssoSync As Boolean = True

    Protected Overrides Sub OnStop()
        Using New log4netUtils.MethodLogger(Log, "OnStop")
            Try
                EventLogger.logInfo(Log, "Mail Service Stopped")
                Timer1.Enabled = False
                'todo: save the running threads as members
                'call join on these before exiting onstop
            Catch ex As Exception
                EventLogger.logError(Log, ex, "OnStop")
            End Try
        End Using
    End Sub

    Public Sub StartIt()
        OnStart(New String() {})
    End Sub

    Public Sub StopIt()
        OnStop()
    End Sub

    '***********************************************
    'timer handler

    Private Sub Timer1_Elapsed(sender As Object, e As ElapsedEventArgs) Handles Timer1.Elapsed
        Using New log4netUtils.MethodLogger(Log, "Timer1_Elapsed")
            Try
                DoWork()
            Catch ex As Exception
                EventLogger.logError(Log, ex, "Timer Elapsed")
                SendEmail.safeSend("running service", "", ex)
            End Try
        End Using
    End Sub

    Private Sub SetupArchiveTime()
        Try
            archiveTime = GetArchiveTimeFromConfig()

            If GetCurrentTime() > archiveTime Then
                lastArchiveDay = StripTime(DateTime.Now)
                Log.Info("archive time has passed, next archive will be tomorrow")
            Else
                lastArchiveDay = Yesterday()
                Log.Info("next archive will be today")
            End If
        Catch ex As Exception
            EventLogger.logError(Log, ex, "setupArchiveTime")
            EventLogger.logError(Log, "Scheduled archives have been disabled, please fix " &
                                      "the configuration file and restart the service.", "setupArchiveTime")
            archivesDisabled = True
            SendEmail.safeSend("archives disabled",
                               "Scheduled archives have been disabled, please fix " &
                               "the configuration file and restart the service.", ex)
        End Try
    End Sub

    Private Function GetArchiveTimeFromConfig() As Long
        Log.Debug("archive time is: " & ConfigUtils.getConfig("ArchiveTime"))
        Dim m As Match = Regex.Match(ConfigUtils.getConfig("ArchiveTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Sub SetupTriggerTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time
            triggerTime = GetTriggerTimeFromConfig()

            If GetCurrentTime() > triggerTime Then
                lasttriggerDay = StripTime(DateTime.Now)
                Log.Info("trigger time has passed, next trigger will be tomorrow")
            Else
                lasttriggerDay = Yesterday()
                Log.Info("next trigger will be today")
            End If
        Catch ex As Exception
            EventLogger.logError(Log, ex, "setuptriggerTime")
            EventLogger.logError(Log, "Scheduled triggers have been disabled, please fix " &
                                      "the configuration file and restart the service.", "setuptriggerTime")
            triggerDisabled = True
            SendEmail.safeSend("triggers disabled",
                               "Scheduled triggers have been disabled, please fix " &
                               "the configuration file and restart the service.", ex)
        End Try
    End Sub

    Private Function GetTriggerTimeFromConfig() As Long
        Log.Debug("trigger time is: " & ConfigUtils.getConfig("TriggerTime"))
        Dim m As Match = Regex.Match(ConfigUtils.getConfig("TriggerTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    'do work
    '****************************************
    Private Sub DoWork()
        Using New log4netUtils.MethodLogger(Log, "doWork")

            'S2 ARCHIVING ----> archive nysapps files older than 6 months
            Dim archiveThread As Thread = Nothing
            If Not archivesDisabled AndAlso lastArchiveDay < GetCurrentDay() _
               AndAlso GetCurrentTime() >= archiveTime Then
                lastArchiveDay = GetCurrentDay()
                Log.Info("Starting archive thread")
                archiveThread = New Thread(
                    New ThreadStart(AddressOf GetCompanyListAndArchiveFiles))
                archiveThread.Start()
            End If

            'S10 trigger check ------- includes updates to Enquiry Data which will eventually be ported to BOSS
            Dim triggerThread As Thread = Nothing
            If Not triggerDisabled AndAlso lasttriggerDay < GetCurrentDay() _
               AndAlso GetCurrentTime() >= triggerTime Then
                lasttriggerDay = GetCurrentDay()
                Log.Info("Starting trigger thread")
                triggerThread = New Thread(
                    New ThreadStart(AddressOf TriggerLauncher))
                triggerThread.Start()
            End If

            Dim nowTicks As Long = DateTime.Now.Ticks

            '---------------------------- S1 ----------------------------------------------------
            'S1.1 check Evolvi directories every 15
            Dim checkEvolviThread As Thread = Nothing
            If checkEvolvi Then
                If lastCheckEvolviRunTicks +
                   MinutesToTicks(CInt(ConfigUtils.getConfig("checkEvolviMinutesInterval"))) _
                   < nowTicks Then
                    lastCheckEvolviRunTicks = nowTicks
                    'log.Info("starting check Evolvi thread")
                    checkEvolviThread = New Thread(
                        New ThreadStart(AddressOf CheckEvolviLauncher))
                    checkEvolviThread.Start()
                End If
            End If

            'S1.2 check EvolviProcess directories every 1
            Dim checkEvolviProcessThread As Thread = Nothing
            If checkEvolviP Then
                If CInt(ConfigUtils.getConfig("checkEvolviProcessSecondsInterval")) > 0 Then
                    If lastCheckEvolviProcessTicks +
                       MinutesToTicks(CInt(ConfigUtils.getConfig("checkEvolviProcessSecondsInterval"))) _
                       < nowTicks Then
                        lastCheckEvolviProcessTicks = nowTicks
                        'log.Info("starting check EvolviProcess thread")
                        checkEvolviProcessThread = New Thread(
                            New ThreadStart(AddressOf CheckEvolviProcess))
                        checkEvolviProcessThread.Start()
                    End If
                End If
            End If

            Dim sendEmailThread As Thread = Nothing
            Dim sendSlaEmailThread As Thread = Nothing
            If sendEmails Then
                If lastSendEmailRunTicks +
                   MinutesToTicks(CInt(ConfigUtils.getConfig("SendEmailMinutesInterval"))) _
                   < nowTicks Then
                    lastSendEmailRunTicks = nowTicks
                    Log.Info("starting send emails thread")
                    sendEmailThread = New Thread(
                        New ThreadStart(AddressOf GetEmailsAndSendLauncher))
                    sendEmailThread.Start()

                    'R2.9 NM
                    sendSlaEmailThread = New Thread(
                        New ThreadStart(AddressOf CheckSlaEnquiries))
                    sendSlaEmailThread.Start()
                End If
            End If

            'S6
            'R2.2 CR R2.5 NM added a quick check on Cubit too
            'get new exchange rates & update nysdb
            Dim rssExchangeRates As Thread = Nothing
            'Dim CubitCheck As Thread = Nothing
            'Dim CubitMissingCheck As Thread = Nothing
            If lastRssDay < GetCurrentDay() _
               AndAlso GetCurrentTime() >= rssTime Then
                lastRssDay = GetCurrentDay()
                Log.Info("Starting RSS Exchange Rates thread")

                rssExchangeRates = New Thread(
                    New ThreadStart(AddressOf GetRssFeedAndChangeExRates))
                rssExchangeRates.Start()

                'CubitCheck = New Thread(
                '    New ThreadStart(AddressOf getCubitCheck))
                'CubitCheck.Start()

                'R2.21 CR - no longer need to check for missing reconciled data, Kath picks them up on seperate reports
                'CubitMissingCheck = New Thread( _
                '   New ThreadStart(AddressOf getCubitMissingCheck))
                'CubitMissingCheck.Start()

            End If

            'S7 ----- AM removed for next rollout April 23 if redesigned to fit in IA
            'R2.4 CR
            'Send corporate feedback emails
            Dim corporateFeedbackEmails As Thread = Nothing
            If lastCorpFeedbackDay < GetCurrentDay() _
               AndAlso GetCurrentTime() >= corpFeedbackTime Then
                lastCorpFeedbackDay = GetCurrentDay()
                Log.Info("Starting Corporate Feedback Email thread")

                corporateFeedbackEmails = New Thread(
                    New ThreadStart(AddressOf CheckCorporateFeedback))
                corporateFeedbackEmails.Start()

            End If

            'S8

            'S9
            'R2.23 CR - Automate generation of LV= user extract report
            Dim lvUserReportGenerator As Thread = Nothing
            If StripTime(Date.Now) = StripTime(lvUserReportNextRunDate) _
               AndAlso GetCurrentTime() >= lvUserReportTime Then

                lvUserReportNextRunDate = StripTime(Date.Now.AddDays(7))
                Log.Info("Starting LV User Report Generation")

                lvUserReportGenerator = New Thread(
                    New ThreadStart(AddressOf GenerateLvUserReport))
                lvUserReportGenerator.Start()
            End If

        End Using
    End Sub

    'R2.23 CR
    'For button click of tester
    Public Sub GenerateLvUserReportGo()
        GenerateLvUserReport()
    End Sub

    'R2.23 CR
    Private Sub GenerateLvUserReport()
        Try
            Dim lstUserReport As New List(Of clsUserReport)
            lstUserReport = clsUserReport.GetUsers_LV

            Dim strCsvData As New StringBuilder

            If lstUserReport.Count > 0 Then
                'add the headers to the file
                strCsvData.Append("FirstName,LastName,Status,Role,Email,ACKCode,EmployeeNumber" & vbCrLf)
            End If

            'loop through the users and create the csv data
            For Each oUser As clsUserReport In lstUserReport
                strCsvData.Append(oUser.FirstName & ",")
                strCsvData.Append(oUser.LastName & ",")
                strCsvData.Append(oUser.Status & ",")
                strCsvData.Append(oUser.Role & ",")
                strCsvData.Append(oUser.Email & ",")
                strCsvData.Append(oUser.CostCentre & ",")
                strCsvData.Append(oUser.PO & vbCrLf)
            Next

            Try
                Log.Debug("Starting SSH connection")
                'now try to open an SFTP connection & send the file to the server
                Dim sshExternalServer As New SftpClient(host:=ConfigUtils.getConfig("UserReportLVSFTPHost"),
                                                        port:=
                                                           notInteger(ConfigUtils.getConfig("UserReportLVSFTPPort")),
                                                        username:=ConfigUtils.getConfig("UserReportLVUsername"),
                                                        password:=ConfigUtils.getConfig("UserReportLVPassword"))
                sshExternalServer.Connect()

                Log.Debug("SSH connection established, writing file...")
                sshExternalServer.WriteAllText(ConfigUtils.getConfig("UserReportLVOutputName"), strCsvData.ToString)

                Log.Debug("File has been written, diconnecting from SSH")
                sshExternalServer.Disconnect()
                Log.Debug("Disconnected from SSH connection")
            Catch ex As Exception
                SendEmail.send("service@nysgroup.com", "development@nysgroup.com", "LV User File Generation failed",
                               "SFTP connection failed, please manually generate the file ready for collection Tuesday at 10AM then debug problem.",
                               "", "", "")
            End Try

            'all done!
            Log.Info("Finished LV User Report Generation")
        Catch ex As Exception
            EventLogger.logError(Log, ex, "GenerateLVUserReport")
        End Try
    End Sub

    'R2.6 CR
    Private Function GetBossEmailLog(pblnOldFile As Boolean) As String
        Dim strFilePath = ""
        Dim strTempFilePath = ""

        If pblnOldFile Then
            strFilePath = ConfigUtils.getConfig("BossEmailLog")
            strTempFilePath = ConfigUtils.getConfig("BossEmailLogTemp")
        Else
            strFilePath = ConfigUtils.getConfig("BossOldEmailLog")
            strTempFilePath = ConfigUtils.getConfig("BossOldEmailLogTemp")
        End If

        If Not Directory.Exists(strTempFilePath.Substring(0, strTempFilePath.LastIndexOf("\"))) Then
            Directory.CreateDirectory(strTempFilePath.Substring(0, strTempFilePath.LastIndexOf("\") + 1))
        End If

        'Remove the old temporary file - if there is one
        If File.Exists(strTempFilePath) Then
            File.Delete(strTempFilePath)
        End If

        'give server a chance to delete the file
        Thread.Sleep(1000)

        'copy the file to a temp location in case there are any issues when reading which could cause a file lock
        If File.Exists(strFilePath) Then
            File.Copy(strFilePath, strTempFilePath)

            Dim strEmailLog = ""
            Dim emailLogReader As New StreamReader(strTempFilePath)

            GetBossEmailLog = emailLogReader.ReadToEnd

            emailLogReader.Close()
            emailLogReader = Nothing

        Else
            GetBossEmailLog = ""
        End If
    End Function

    'R2.2 CR
    Private Sub SetupRssExRatesTime()
        Try
            rssTime = GetRssExRatesTimeFromConfig()

            If GetCurrentTime() > rssTime Then
                lastRssDay = StripTime(DateTime.Now)
                Log.Info("EX Rates time has passed, next update will be tomorrow")
            Else
                lastRssDay = Yesterday()
                Log.Info("next EX Rates will be today")
            End If
        Catch ex As Exception
            EventLogger.logError(Log, ex, "setupRSSExRatesTime")
            EventLogger.logError(Log, "Exchange Rates RSS Feed import has been disabled, please fix " &
                                      "the configuration file and restart the service.", "setupRSSExRatesTime")
            archivesDisabled = True
            SendEmail.safeSend("Exchange Rates RSS disabled",
                               "Exchange Rates RSS Feed import has been disabled, please fix " &
                               "the configuration file and restart the service.", ex)
        End Try
    End Sub

    'R2.4 CR ****
    Private Sub CheckCorporateFeedback()

        'Get all clients set up for corporate feedback sending
        Dim oClients As New List(Of clsCorporateFeedbackLimitDat)
        oClients = clsCorporateFeedbackLimitDat.listWithValues

        For Each oClient As clsCorporateFeedbackLimitDat In oClients
            Dim strBossIDs = ""
            strBossIDs = clsCorporateFeedbackLimitDat.getClientBossCodes(oClient.ClientID)

            If strBossIDs IsNot Nothing AndAlso strBossIDs <> "" Then
                Dim strBossIdWhere = ""

                If strBossIDs.Contains(",") Then
                    Dim strSplitIds As String() = strBossIDs.Split(",")
                    Dim x = 0
                    For x = 0 To strSplitIds.Length - 1
                        If strSplitIds(x).Trim <> "" Then
                            If strBossIdWhere <> "" Then
                                strBossIdWhere &= " or "
                            End If
                            strBossIdWhere &= "inm_custid LIKE '" & strSplitIds(x).Trim & "'"
                        End If
                    Next
                Else
                    strBossIdWhere = "inm_custid LIKE '" & strBossIDs.Trim & "'"
                End If

                If oClient.AirPercentage > 0 Then
                    CheckCorporateInvoices(strBossIdWhere, oClient.ClientID, "A", oClient.AirPercentage,
                                           oClient.AirLastInvoice, oClient.SendAfterAir)
                End If

                If oClient.RailPercentage > 0 Then
                    CheckCorporateInvoices(strBossIdWhere, oClient.ClientID, "RR", oClient.RailPercentage,
                                           oClient.RailLastInvoice, oClient.SendAfterRail)
                End If

                If oClient.HotelPercentage > 0 Then
                    CheckCorporateInvoices(strBossIdWhere, oClient.ClientID, "H", oClient.HotelPercentage,
                                           oClient.HotelLastInvoice, oClient.SendAfterHotel)
                End If
            End If
        Next
    End Sub

    'R2.4 CR
    Private Sub CheckCorporateInvoices(pstrClientBossId As String,
                                       pintClientId As Integer,
                                       pstrProductCode As String,
                                       pdblPercentage As Double,
                                       pstrLastInvoiceNoProcessed As String,
                                       pintSendAfter As Integer)
        Dim oInvoices As New List(Of clsInvoicingDat)
        oInvoices = clsInvoicingDat.checkFeedbackInvoice(pstrClientBossId, pstrProductCode, pstrLastInvoiceNoProcessed)

        'R2.5 CR - use passed in param pintSendAfter instead of hard-coded 50
        If oInvoices.Count >= pintSendAfter Then
            Dim intTotalInvoices As Integer = oInvoices.Count
            Dim strLastInvoiceId = ""

            'Work out how many emails we need to send as a whole int
            Dim intTotalEmailsToSend As Integer = Math.Ceiling((intTotalInvoices / 100) * pdblPercentage)

            'choose that many random numbers between 1 (& including) and intTotalEmailsToSend + 1 (only less than)
            Dim intRandomList As New List(Of Integer)
            GetRandomNumbers(1, intTotalInvoices + 1, intTotalEmailsToSend, intRandomList)

            'run through invoices and only send when the current count hits one of the random numbers selected before
            Dim intCount = 0
            For Each oInvoice As clsInvoicingDat In oInvoices
                intCount += 1

                If intRandomList.Contains(intCount) Then
                    Dim strTo = ""
                    Dim strBookerFirstname = ""
                    Dim strBookerLastname = ""

                    'get the group name, not just id
                    'Dim strGroupName As String = ""
                    'strGroupName = Replace(Replace(clsGroupDat.GetGroupName(0, ).ToLower, "bs-", ""), "bs - ", "")

                    'try to find an email address for that booker
                    If pstrProductCode = "RR" Then
                        '1st check - Evolvi (only if rail booking)
                        Dim oEvolviContact As clsEvolviData
                        oEvolviContact = clsEvolviData.getBooker(notInteger(oInvoice.mstrboss))
                        ' search on booking agent firstname/lastname - get back bookingagentemailaddress

                        If oEvolviContact.BookerEmail <> "" Then
                            strTo = oEvolviContact.BookerEmail
                            FormatBookerName(oEvolviContact.BookerName, strBookerFirstname, strBookerLastname)
                        ElseIf oEvolviContact.BookerName <> "" Then
                            'Not found an email address but has found a name, so check SSO for possible email address
                            FormatBookerName(oEvolviContact.BookerName, strBookerFirstname, strBookerLastname)

                            Dim oBookerDetails As New clsCorporateFeedbackDat
                            oBookerDetails = clsCorporateFeedbackDat.checkSSO(strBookerFirstname, strBookerLastname,
                                                                              oInvoice.mstrbossid)
                            strTo = oBookerDetails.mstrBookerEmail
                            If oBookerDetails.mstrBookerName.Trim <> "" Then
                                strBookerFirstname = oBookerDetails.mstrBookerName.Substring(0,
                                                                                             oBookerDetails.
                                                                                                mstrBookerName.IndexOf(
                                                                                                    " "))
                                strBookerLastname =
                                    oBookerDetails.mstrBookerName.Substring(
                                        oBookerDetails.mstrBookerName.IndexOf(" ") + 1)
                            End If
                            oBookerDetails = Nothing
                        End If

                        oEvolviContact = Nothing
                    ElseIf pstrProductCode = "H" Then
                        '2nd check - CUBIT (only if hotel booking)
                        Dim oCubitContact As New clsCorporateFeedbackDat
                        oCubitContact = clsCorporateFeedbackDat.getCubitBooker(oInvoice.mstrinvoicenumber)

                        If oCubitContact.mstrBookerName <> "" Then
                            FormatBookerName(oCubitContact.mstrBookerName, strBookerFirstname, strBookerLastname)

                            Dim oBookerDetails As New clsCorporateFeedbackDat
                            oBookerDetails = clsCorporateFeedbackDat.checkSSO(strBookerFirstname, strBookerLastname,
                                                                              oInvoice.mstrbossid)
                            strTo = oBookerDetails.mstrBookerEmail
                            If oBookerDetails.mstrBookerName.Trim <> "" Then
                                strBookerFirstname = oBookerDetails.mstrBookerName.Substring(0,
                                                                                             oBookerDetails.
                                                                                                mstrBookerName.IndexOf(
                                                                                                    " "))
                                strBookerLastname =
                                    oBookerDetails.mstrBookerName.Substring(
                                        oBookerDetails.mstrBookerName.IndexOf(" ") + 1)
                            End If
                            oBookerDetails = Nothing
                        End If

                        oCubitContact = Nothing
                    ElseIf pstrProductCode = "A" Then
                        '3rd check - GIDS (only if Air booking)
                        Dim oGids As New clsCorporateFeedbackDat
                        oGids = clsCorporateFeedbackDat.getGIDSBooker(oInvoice.mstrboss)

                        If oGids.mstrBookerName <> "" Then
                            FormatBookerName(oGids.mstrBookerName, strBookerFirstname, strBookerLastname)

                            Dim oBookerDetails As New clsCorporateFeedbackDat
                            oBookerDetails = clsCorporateFeedbackDat.checkSSO(strBookerFirstname, strBookerLastname,
                                                                              oInvoice.mstrbossid)
                            strTo = oBookerDetails.mstrBookerEmail
                            If oBookerDetails.mstrBookerName.Trim <> "" Then
                                strBookerFirstname = oBookerDetails.mstrBookerName.Substring(0,
                                                                                             oBookerDetails.
                                                                                                mstrBookerName.IndexOf(
                                                                                                    " "))
                                strBookerLastname =
                                    oBookerDetails.mstrBookerName.Substring(
                                        oBookerDetails.mstrBookerName.IndexOf(" ") + 1)
                            End If
                            oBookerDetails = Nothing
                        End If

                        oGids = Nothing
                    End If

                    If strTo <> "" Then
                        'if contact email is found then

                        'R2.17 CR
                        'check to see if they have requested no emails (blacklisted)
                        If clsCorporateFeedbackBlacklist.checkEmail(strTo) > 0 Then
                            'they are on the list
                            'skip this email
                        Else
                            'send the email
                            Dim strParams = ""
                            Dim strBookerFullName = ""

                            strBookerFullName = strBookerFirstname & " " & strBookerLastname

                            strParams = "?InvoiceRef=" & oInvoice.mstrinvoicenumber
                            strParams &= "&fao=" & strBookerFirstname & strBookerLastname

                            If strBookerFirstname = "" And strBookerLastname = "" Then
                                strBookerFullName = "Sir/Madam"
                            ElseIf strBookerFirstname <> "" And strBookerLastname = "" Then
                                strBookerFullName = strBookerFirstname
                            End If

                            Dim strOriginalEmail = ""
                            Dim blnTesting = False
                            If ConfigUtils.getConfig("CorporateFeedbackTesting") = "1" Then
                                'we are in test mode
                                blnTesting = True

                                'put the email address it would have been sent to into the email body
                                strOriginalEmail = strTo

                                'Reset the email address to the testing one
                                strTo = ConfigUtils.getConfig("CorporateFeedbackTestingEmail")
                            End If

                            Dim oMail As New MailMessage(ConfigUtils.getConfig("CorporateFeedbackEmail"), strTo)
                            oMail.IsBodyHtml = True
                            oMail.BodyEncoding = Encoding.Default
                            oMail.Subject = "NYS Corporate Feedback Regarding Booking Ref: " &
                                            oInvoice.mstrinvoicenumber

                            Dim strProductDesc = ""
                            If pstrProductCode = "RR" Then
                                strProductDesc = "Rail Travel"
                            ElseIf pstrProductCode = "H" Then
                                strProductDesc = "Hotel Stay"
                            ElseIf pstrProductCode = "A" Then
                                strProductDesc = "Flight booking"
                            End If

                            oMail.Body = "Reference " & oInvoice.mstrinvoicenumber & "<br /><br />Dear " &
                                         strBookerFullName & "<br/><br/>" &
                                         "You recently made a booking through NYS Corporate, details of the travel are below.<br/><br/>" &
                                         "<strong>" & strProductDesc & " for " & oInvoice.mstrleadname &
                                         " travelling on " & oInvoice.mstrstartdate & "</strong><br/><br/>" &
                                         "We would be grateful if you could spend a few moments of your time completing the questionnaire found by following the link below.<br/><br/><a href=""" &
                                         ConfigUtils.getConfig("CorporateFeedbackURL") & strParams &
                                         """>NYS FEEDBACK QUESTIONNAIRE</a><br/><br/>" &
                                         "If the link above does not work please can you copy and paste the following into a web browser to enable you to complete the NYS Feedback questionnaire:<br />" &
                                         ConfigUtils.getConfig("CorporateFeedbackURL") & strParams & "<br/><br/>" &
                                         "Please do not hesitate to contact us if you have any queries.<br/><br/>" &
                                         "Kind Regards,<br /><br />" &
                                         "Quality Department<br />" &
                                         "NYS Corporate<br />" &
                                         "Quantum House<br />" &
                                         "Innovation Way<br />" &
                                         "York YO10 5BR<br />" &
                                         "Tel: 01904 420227<br />" &
                                         "Fax: 0870 4582 757<br />"

                            If blnTesting Then
                                oMail.Body = "THIS IS A TEST EMAIL, original would have been sent to: " &
                                             strOriginalEmail & "<br/><br/>" & oMail.Body
                            End If

                            Dim oClient As New SmtpClient

                            Try
                                oClient.Send(oMail)

                                'save the intial feedback form to db to record send
                                Dim oCorpFeedback As New clsCorporateFeedbackDat
                                oCorpFeedback.saveInitial(oInvoice.mstrinvoicenumber, pintClientId)
                            Catch ex As Exception
                                'email sending failed for some reason
                            End Try

                            'intRandomList.Remove(intCount)
                        End If

                    Else
                        'try to get another random number in the remaining invoices, if there are any remaining
                        If intCount + 1 < intTotalInvoices + 1 Then
                            GetRandomNumbers(intCount + 1, intTotalInvoices + 1, 1, intRandomList)
                        End If

                        intRandomList.Remove(intCount)
                    End If
                End If

                If intCount = oInvoices.Count Then
                    strLastInvoiceId = oInvoice.mstrinvoicenumber
                End If
            Next

            If strLastInvoiceId <> "" Then
                'save the last invoice id to the limit db table
                Dim oFeedbackLimit As New clsCorporateFeedbackLimitDat
                oFeedbackLimit = clsCorporateFeedbackLimitDat.getByClient(pintClientId)
                If pstrProductCode = "A" Then
                    oFeedbackLimit.AirLastInvoice = strLastInvoiceId
                ElseIf pstrProductCode = "H" Then
                    oFeedbackLimit.HotelLastInvoice = strLastInvoiceId
                ElseIf pstrProductCode = "RR" Then
                    oFeedbackLimit.RailLastInvoice = strLastInvoiceId
                End If

                oFeedbackLimit.saveLastInvoices()
            End If

            strLastInvoiceId = Nothing
            oInvoices = Nothing
        End If
    End Sub

    'R2.4 CR
    Private Sub FormatBookerName(pstrBookerName As String, ByRef strFirstname As String, ByRef strLastname As String)
        'mess around with the booker name
        pstrBookerName = Replace(pstrBookerName, ".", " ")

        'Dim strFirstname As String = ""
        'Dim strLastname As String = ""

        If pstrBookerName.Contains(" ") Then
            If CollectionUtils.split(pstrBookerName, " ").Count > 2 Then
                'they've added title!
                strFirstname = CollectionUtils.split(pstrBookerName, " ")(1)
                strLastname = CollectionUtils.split(pstrBookerName, " ")(2)
            Else
                strFirstname = CollectionUtils.split(pstrBookerName, " ")(0)
                strLastname = CollectionUtils.split(pstrBookerName, " ")(1)
            End If
        ElseIf pstrBookerName.Length = 2 Then
            'try as initials
            strFirstname = pstrBookerName.Substring(0, 1)
            strLastname = pstrBookerName.Substring(1)
        Else
            'either going to be:
            'full firstname + full lastname
            'firstname initial + full last name
            'firstname only
            'random string that is not a booker name

            'check to see if any of the characters after the 1st are upper case, then split on next occurrance of an upper character
            Dim x As Integer
            Dim intSplitNumber = 0
            Dim blnUpperSeperate = False
            For x = 0 To pstrBookerName.Length - 1
                Dim strCurrentLetter As String = pstrBookerName(x)
                If strCurrentLetter = strCurrentLetter.ToUpper Then
                    'the letter is upper case
                    If x = 0 Then
                        blnUpperSeperate = True
                    Else
                        blnUpperSeperate = True
                        intSplitNumber = x + 1
                        Exit For
                    End If
                End If
            Next

            If blnUpperSeperate Then
                If intSplitNumber = 0 Then
                    intSplitNumber = 1
                End If
                strFirstname = pstrBookerName.Substring(0, intSplitNumber)
                strLastname = pstrBookerName.Substring(intSplitNumber)

            Else
                'if no uppers then just put the string in firstname as can't split fullname
                strFirstname = pstrBookerName
                strLastname = ""
            End If
        End If

        'Capitalise the first letter of both firstname and surname for presentation in email
        Dim strFirstletter = ""
        If strFirstname.Length > 1 Then
            strFirstletter = strFirstname.Substring(0, 1)
            strFirstname = strFirstletter.ToUpper & strFirstname.Remove(0, 1).ToLower
        ElseIf strFirstname.Length = 1 Then
            strFirstname = strFirstname.ToUpper
        End If

        strFirstletter = ""

        If strLastname.Length > 1 Then
            strFirstletter = strLastname.Substring(0, 1)
            strLastname = strFirstletter.ToUpper & strLastname.Remove(0, 1).ToLower

            If strLastname.Contains("-") Then
                'double barrelled last name
                Dim strFirstLastname = ""
                Dim strSecondLastname = ""
                strFirstLastname = strLastname.Substring(0, strLastname.IndexOf("-") + 1)
                strSecondLastname = strLastname.Substring(strLastname.IndexOf("-") + 1)

                Dim intSubstringValue = 0
                strFirstletter = strSecondLastname.Substring(intSubstringValue, 1)
                If strFirstletter = " " Then
                    intSubstringValue = 1
                    strFirstletter = strSecondLastname.Substring(intSubstringValue, 1)
                End If

                strLastname = strFirstLastname & strFirstletter.ToUpper &
                              strSecondLastname.Remove(intSubstringValue, 1).ToLower

            End If
        ElseIf strLastname.Length = 1 Then
            strLastname = strLastname.ToUpper
        End If
    End Sub

    'R2.4 CR
    Private Sub GetRandomNumbers(pintMinNumber As Integer, pintMaxNumber As Integer, intTotalNumbersNeeded As Integer,
                                 ByRef lstRandomNumbers As List(Of Integer))
        Dim rdmNumber As New Random
        Dim x = 0

        'First check to see if there any numbers remaining that are unselected, need to do this to avoid infinate loop
        Dim a = 0
        Dim blnRemainingFreeNumbers = False
        For a = pintMinNumber To pintMaxNumber - 1
            If a > pintMinNumber Then
                If Not lstRandomNumbers.Contains(a) Then
                    blnRemainingFreeNumbers = True
                    Exit For
                End If
            End If
        Next

        If blnRemainingFreeNumbers Then
            For x = 0 To intTotalNumbersNeeded - 1
                Dim intNewRandomNumber As Integer = rdmNumber.Next(pintMinNumber, pintMaxNumber)

                Do Until Not lstRandomNumbers.Contains(intNewRandomNumber)
                    intNewRandomNumber = rdmNumber.Next(pintMinNumber, pintMaxNumber)
                Loop

                lstRandomNumbers.Add(intNewRandomNumber)
            Next
        End If
    End Sub

    'R2.4 CR
    Private Sub SetupCorpFeedbackTime()
        Try
            'R2.6 CR - FIX: rssTime was previously getting set instead of CorpFeedbackTime
            corpFeedbackTime = GetCorpFeedbackTimeFromConfig()

            If GetCurrentTime() > corpFeedbackTime Then
                lastCorpFeedbackDay = StripTime(DateTime.Now)
                Log.Info("Corporate Feedback time has passed, next attempt will be tomorrow")
            Else
                lastCorpFeedbackDay = Yesterday()
                Log.Info("next Corporate Feedback attempt will be today")
            End If
        Catch ex As Exception
            EventLogger.logError(Log, ex, "setupCorpFeedbackTime")
            EventLogger.logError(Log, "Corporate Feedback Emailing has been disabled, please fix " &
                                      "the configuration file and restart the service.", "setupCorpFeedbackTime")
            archivesDisabled = True
            SendEmail.safeSend("Corporate Feedback disabled",
                               "Corporate Feedback Emailing has been disabled, please fix " &
                               "the configuration file and restart the service.", ex)
        End Try
    End Sub

    'R2.6 CR
    Private Sub SetupInvoiceCheckTime()
        Try
            invoiceCheckTime = GetInvoiceCheckTimeFromConfig()

            If GetCurrentTime() > invoiceCheckTime Then
                lastInvoiceCheckDay = StripTime(DateTime.Now)
                Log.Info("Invoice Check time has passed, next attempt will be tomorrow")
            Else
                lastInvoiceCheckDay = Yesterday()
                Log.Info("next Invoice Check attempt will be today")
            End If
        Catch ex As Exception
            EventLogger.logError(Log, ex, "setupRsaInvoiceCheckTime")
            EventLogger.logError(Log, "Invoice Check has been disabled, please fix " &
                                      "the configuration file and restart the service.", "setupInvoiceCheckTime")
            archivesDisabled = True
            SendEmail.safeSend("Invoice Check disabled",
                               "Invoice Check has been disabled, please fix " &
                               "the configuration file and restart the service.", ex)
        End Try
    End Sub

    'R2.23 CR
    Private Function GetLvUserReportTimeFromConfig() As Long
        Log.Debug("LV User Report time is: " & ConfigUtils.getConfig("WeeklyLVUserReportTime"))
        Dim m As Match = Regex.Match(ConfigUtils.getConfig("WeeklyLVUserReportTime"),
                                     "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    'R2.23 CR
    Private Function GetLvUserReportWeekdayFromConfig() As Long
        Log.Debug("LV User Report day is: " & ConfigUtils.getConfig("WeeklyLVUserReportDay"))
        Return ConfigUtils.getConfig("WeeklyLVUserReportDay")
    End Function

    'R2.23 CR
    Private Sub SetupLvUserReportTime()
        Try
            'get the time from the config
            lvUserReportTime = GetLvUserReportTimeFromConfig()

            'get the day of the week from the config
            Dim lvUserReportDay As Integer = notInteger(GetLvUserReportWeekdayFromConfig())

            'check the day of the week first
            If notInteger(Date.Now.DayOfWeek) = lvUserReportDay Then
                'today is the correct day
                'only time to worry about now
                Log.Info("Correct day to generate LV User Report, now checking time.")
                lvUserReportNextRunDate = StripTime(Date.Now)
            Else
                'today isn't the correct day
                'work out the next run date & log it for debugging
                If notInteger(Date.Now.DayOfWeek) > lvUserReportDay Then
                    'run will be next week
                    lvUserReportNextRunDate =
                        StripTime(Date.Now.AddDays((7 - notInteger(Date.Now.DayOfWeek)) + lvUserReportDay))
                Else
                    'run will be this week
                    lvUserReportNextRunDate =
                        StripTime(Date.Now.AddDays(lvUserReportDay - notInteger(Date.Now.DayOfWeek)))
                End If
                Log.Info(
                    "Today is not the correct day to generate LV User Report, next attempt will be " &
                    StripTime(lvUserReportNextRunDate).ToString("dd/MM/yyyy") & " at " & MinutesToTime(lvUserReportTime))
            End If

            'then check current time
            If StripTime(Date.Now) = StripTime(lvUserReportNextRunDate) Then
                If GetCurrentTime() > lvUserReportTime Then
                    lvUserReportNextRunDate = StripTime(lvUserReportNextRunDate.AddDays(7))
                    Log.Info(
                        "LV User Report time has passed. Next run will be " &
                        StripTime(lvUserReportNextRunDate).ToString("dd/MM/yyyy") & " at " &
                        MinutesToTime(lvUserReportTime) & ".")
                Else
                    Log.Info("Next LV User Report attempt will be today at " & MinutesToTime(lvUserReportTime))
                End If
            End If

        Catch ex As Exception
            EventLogger.logError(Log, ex, "setupLVUserReportTime")
            EventLogger.logError(Log, "LV User Report has been disabled, please fix " &
                                      "the configuration file and restart the service.", "setupLVUserReportTime")
            SendEmail.safeSend("LV User Report disabled",
                               "LV User Report has been disabled, please fix " &
                               "the configuration file and restart the service.", ex)
        End Try
    End Sub

    'R2.2 CR
    Public Sub GetRssFeedAndChangeExRates()
        Dim rssSourceLocation = ""
        rssSourceLocation = ConfigUtils.getConfig("ExRatesRSSURL")

        Dim rssData As New DataSet
        Try
            rssData.ReadXml(rssSourceLocation)
        Catch ex As Exception
            SendEmail.send("service@nysgroup.com", "mike.kirk@nysgroup.com", "RSS currency",
                           "RSS feed for exchange rates failed:" & ex.Message, "nick.massarella@nysgroup.com", "", "",
                           "")
            Exit Sub
        End Try

        For Each rssRow As DataRow In rssData.Tables("item").Rows
            'get the currency code
            Dim strCurrencyCode = ""
            strCurrencyCode = rssRow.Item("title").ToString
            strCurrencyCode = strCurrencyCode.Substring(0, strCurrencyCode.IndexOf("/"))

            'check db for code
            Dim oCurrency As New clsSystemparameter
            oCurrency = clsSystemparameter.findCurrency(strCurrencyCode)

            If oCurrency.Systemparameterid > 0 Then
                'found!

                'get currency value to 1 British Pound Sterling
                Dim strCurrencyValue = ""
                strCurrencyValue = rssRow.Item("description").ToString
                strCurrencyValue = strCurrencyValue.Substring(strCurrencyValue.IndexOf("=") + 1)
                strCurrencyValue = strCurrencyValue.Trim
                strCurrencyValue = strCurrencyValue.Substring(0, strCurrencyValue.IndexOf(" "))
                strCurrencyValue = strCurrencyValue.Trim

                'Round value to 2dp
                Dim dblCurrencyValue As Double = 0
                dblCurrencyValue = Math.Round(notDouble(strCurrencyValue), 2)

                'change rates & set update datetime
                oCurrency.Systemparametercolour = dblCurrencyValue.ToString
                If oCurrency.changeCurrencyRate("NYS-Service") > 0 Then
                    'saved ok
                End If
            End If
        Next
    End Sub

    'R2.2 CR
    Private Function GetRssExRatesTimeFromConfig() As Long
        Log.Debug("Exchange Rates RSS Feed time is: " & ConfigUtils.getConfig("ExRatesRSSTime"))
        Dim m As Match = Regex.Match(ConfigUtils.getConfig("ExRatesRSSTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    'R2.4 CR
    Private Function GetCorpFeedbackTimeFromConfig() As Long
        Log.Debug("Corporate Feedback Emailing time is: " & ConfigUtils.getConfig("CorpFeedbackTime"))
        Dim m As Match = Regex.Match(ConfigUtils.getConfig("CorpFeedbackTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    'R2.6 CR
    Private Function GetInvoiceCheckTimeFromConfig() As Long
        Log.Debug("Invoice Check time is: " & ConfigUtils.getConfig("InvoiceCheckTime"))
        Dim m As Match = Regex.Match(ConfigUtils.getConfig("InvoiceCheckTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

#Region "Latch items --- to trigger etc"
    'R2.21 SA
    Private lastcheckAutoInvoiceRunTicks As Long = 0

    'Private lastcheckFeraRunTicks As Long = 0
    'Private lastcheckFeraAwaitingRunTicks As Long = 0
    Private lastSendEmailRunTicks As Long = 0
    Private lastCheckEvolviRunTicks As Long = 0
    Private lastCheckEvolviProcessTicks As Long = 0
    Private lastDownloadEmailRunTicks As Long = 0

    'R2.21 CR
    Private lastSsoSyncTicks As Long = 0

    Private backupsSsoDisabled As Boolean = False
    Private lastSsoBackupDay As DateTime
    Private backupTimeSso As Long

    Private backupsDisabled As Boolean = False
    Private archivesDisabled As Boolean = False
    Private cubitInvoiceDisabled As Boolean = ConfigUtils.getConfig("cubitInvoiceDisabled") 'False
    Private lastBackupDay As DateTime
    Private lastArchiveDay As DateTime
    Private lastcubitInvoiceDay As DateTime
    Private backupTime As Long
    Private archiveTime As Long
    Private cubitInvoiceTime As Long
    Private backupsDisabledGids As Boolean = False
    Private lastGidsBackupDay As DateTime
    Private gidsBackupTime As Long

    Private cubitbackupsDisabled As Boolean = False
    Private lastCubitBackupDay As DateTime
    Private cubitbackupTime As Long
    Private cubitMorningbackupsDisabled As Boolean = False
    Private lastCubitMorningBackupDay As DateTime
    Private cubitMorningbackupTime As Long

    Private triggerDisabled As Boolean = False
    Private lasttriggerDay As DateTime
    Private triggerTime As Long

    Private cubitBookedDisabled As Boolean = False
    Private cubitTransDisabled As Boolean = False
    Private cubitDisabled As Boolean = False
    Private lastCubitDay As DateTime
    Private cubitTime As Long

    'R2.21.7 CR
    Private cubitAfternoonTime As Long
    Private cubitBookedAfternoonDisabled As Boolean = False

    Private cubitFinishTime As Long

    'R2.2 CR
    Private lastRssDay As DateTime
    Private rssTime As Long

    'R2.4 CR
    Private lastCorpFeedbackDay As DateTime
    Private corpFeedbackTime As Long

    'R2.6 CR
    Private lastInvoiceCheckDay As DateTime
    Private invoiceCheckTime As Long

    'R2.17 SA
    Private commisionEmailTime As Long
    Private lastCommisionEmailDay As DateTime

    'R2.23 CR
    Private lvUserReportNextRunDate As Date
    Private lvUserReportTime As Long

#End Region

    Private Function GetCurrentDay() As DateTime
        Return StripTime(DateTime.Now)
    End Function

    Private Function StripTime(dt As DateTime) As DateTime
        Return New DateTime(dt.Year, dt.Month, dt.Day)
    End Function

    Private Function Yesterday() As DateTime
        Return StripTime(DateTime.Now) - New TimeSpan(24, 0, 0)
    End Function

    Private Function GetCurrentTime() As Long
        Return MinutesInDay(DateTime.Now)
    End Function

    Private Function MinutesInDay(dt As DateTime) As Long
        Return dt.Hour * 60 + dt.Minute
    End Function

    Private Function MinutesToTicks(minutes As Long) As Long
        Return minutes * 60 * 1000 * 1000 * 10
    End Function

    Private Function SecondsToTicks(seconds As Long) As Long
        Return seconds * 60 * 60 * 1000 * 1000 * 10
    End Function

    'R2.22 CR
    Private Function MinutesToTime(pminutes As Long) As String
        Dim tsMintues As New TimeSpan(MinutesToTicks(pminutes))
        Dim ret As String = tsMintues.Hours.ToString & ":" & tsMintues.Minutes.ToString
        Return ret
    End Function

    '***********************************************
    'outgoing emails

    Private getEmailsAndSendLauncherRunning As Boolean = False

    Private Sub GetEmailsAndSendLauncher()
        Try
            If getEmailsAndSendLauncherRunning Then
                EventLogger.logError(Log, "Tried to call getEmailsAndSendLauncher, but " &
                                          "it's still running from the previous wake up.",
                                     "getEmailsAndSendLauncher")
            Else
                Try 'use try finally to make sure
                    'getEmailsAndSendLauncherRunning is set to false when
                    'finished
                    getEmailsAndSendLauncherRunning = True
                    SendQueuedEmails.checkEmails()
                Finally
                    getEmailsAndSendLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            EventLogger.logError(Log, ex, "getEmailsAndSendLauncher")
            SendEmail.safeSend("sending emails", "", ex)
        End Try
    End Sub

    Public Shared Sub MakeFolderExist(folder As String)
        If Not Directory.Exists(folder) Then
            Directory.CreateDirectory(folder)
        End If
    End Sub

    Private getCompanyListAndArchiveFilesRunning As Boolean = False

    Public Sub GetCompanyListAndArchiveFiles()
        Try
            If getCompanyListAndArchiveFilesRunning Then
                EventLogger.logError(Log, "Tried to call getCompanyListAndArchiveFilesRunning, but " &
                                          "it's still running from the previous wake up.",
                                     "getCompanyListAndArchiveFilesRunning")
            Else
                Try 'use try finally to make sure
                    getCompanyListAndArchiveFilesRunning = True
                    ArchiveFiles()

                    'R2.21 CR - moved the SSO Sync to seperate schedule
                    'NM 2.14 added to retrieve database updates from SSO
                    'getSSODetails()
                Finally
                    getCompanyListAndArchiveFilesRunning = False
                End Try
            End If
        Catch ex As Exception
            EventLogger.logError(Log, ex, "getCompanyListAndArchiveFiles")
        End Try
    End Sub

    Private Sub ArchiveFiles()
        Try
            Dim oCompanieslist, oCompanieslists As New clsCompanyDat
            Dim oGroups As List(Of NysDat.clsGroup)
            oGroups = NysDat.clsGroup.List()

            For Each gr As NysDat.clsGroup In oGroups

                oCompanieslists.Populate(0, gr.Groupid)

                For Each oCompanieslist In oCompanieslists.mColCompanies
                    Dim strLivePath As String = ConfigUtils.getConfig("HomeAbsolutepath") & "userdocs\" &
                                                CStr(gr.Groupid) & "-" & gr.Groupname &
                                                "\" & CStr(oCompanieslist.mintcompanyid) & "-" &
                                                oCompanieslist.mstrcompanyname

                    Dim strArchivePath As String = ConfigUtils.getConfig("ArchiveDirectory") & CStr(gr.Groupid) & "-" &
                                                   gr.Groupname &
                                                   "\" & CStr(oCompanieslist.mintcompanyid) & "-" &
                                                   oCompanieslist.mstrcompanyname

                    If Not Directory.Exists(strArchivePath) Then
                        MakeFolderExist(strArchivePath)
                    End If
                    If Not Directory.Exists(strArchivePath & "\emails\body") Then
                        MakeFolderExist(strArchivePath & "\emails\body")
                    End If
                    If Not Directory.Exists(strArchivePath & "\emails\attachments") Then
                        MakeFolderExist(strArchivePath & "\emails\attachments")
                    End If
                    If Not Directory.Exists(strArchivePath & "\documents") Then
                        MakeFolderExist(strArchivePath & "\documents")
                    End If

                    ''move emails
                    'If IO.Directory.Exists(strLivePath & "\emails") Then
                    '    For Each filename As String In Directory.GetFiles(strLivePath & "\emails")
                    '        If IO.File.GetLastWriteTime(filename) < Now.AddMonths(-8) Then
                    '            File.Move(filename, strArchivePath & "\emails\" & Path.GetFileName(filename))
                    '        End If
                    '    Next
                    'End If
                    'move email bodys

                    If Directory.Exists(strLivePath & "\emails\body") Then
                        For Each filename As String In Directory.GetFiles(strLivePath & "\emails\body")
                            If _
                                File.GetLastWriteTime(filename) <
                                Now.AddMonths(notInteger(ConfigUtils.getConfig("ArchiveMonthLimit"))) Then
                                If Not File.Exists(strArchivePath & "\emails\body\" & Path.GetFileName(filename)) Then
                                    Try
                                        File.Move(filename,
                                                  strArchivePath & "\emails\body\" & Path.GetFileName(filename))
                                    Catch ex As Exception

                                    End Try
                                Else
                                    Try
                                        File.Delete(filename)
                                    Catch ex As Exception

                                    End Try
                                End If
                            End If
                        Next
                    End If
                    'move email attachments
                    If Directory.Exists(strLivePath & "\emails\attachments") Then
                        For Each filename As String In Directory.GetFiles(strLivePath & "\emails\attachments")
                            If _
                                File.GetLastWriteTime(filename) <
                                Now.AddMonths(notInteger(ConfigUtils.getConfig("ArchiveMonthLimit"))) Then
                                If Not File.Exists(strArchivePath & "\emails\attachments\" & Path.GetFileName(filename)) _
                                    Then
                                    Try
                                        File.Move(filename,
                                                  strArchivePath & "\emails\attachments\" & Path.GetFileName(filename))
                                    Catch ex As Exception

                                    End Try
                                Else
                                    Try
                                        File.Delete(filename)
                                    Catch ex As Exception

                                    End Try
                                End If
                            End If
                        Next
                    End If
                    'move docs
                    If Directory.Exists(strLivePath & "\documents") Then
                        For Each filename As String In Directory.GetFiles(strLivePath & "\documents")
                            If _
                                File.GetLastWriteTime(filename) <
                                Now.AddMonths(notInteger(ConfigUtils.getConfig("ArchiveMonthLimit"))) Then
                                If Not File.Exists(strArchivePath & "\documents\" & Path.GetFileName(filename)) Then
                                    Try
                                        File.Move(filename, strArchivePath & "\documents\" & Path.GetFileName(filename))
                                    Catch ex As Exception

                                    End Try
                                Else
                                    Try
                                        File.Delete(filename)
                                    Catch ex As Exception

                                    End Try
                                End If
                            End If
                        Next
                    End If
                Next
            Next
        Catch ex As Exception
            EventLogger.logError(Log, ex, "ArchiveFiles")
        End Try
    End Sub

    Private checkEvolviLauncherRunning As Boolean = False

    Private Sub CheckEvolviLauncher()
        Try
            If checkEvolviLauncherRunning Then
                EventLogger.logError(Log, "Tried to call checkEvolviLauncher, but " &
                                          "it's still running from the previous wake up.",
                                     "checkEvolviLauncher")
            Else
                Try 'use try finally to make sure
                    'getEmailsAndSendLauncherRunning is set to false when
                    'finished
                    checkEvolviLauncherRunning = True
                    Dim strDay As String = Mid(Now.DayOfWeek.ToString, 1, 3)
                    EvolviReader(strDay)
                Finally
                    checkEvolviLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            EventLogger.logError(Log, ex, "checkEvolviLauncherRunning")
        End Try
    End Sub

    Private checkEvolviProcessRunning As Boolean = False

    Private Sub CheckEvolviProcess()
        Try
            If checkEvolviProcessRunning Then
                EventLogger.logError(Log, "Tried to call checkEvolviProcess, but " &
                                          "it's still running from the previous wake up.",
                                     "checkEvolviProcess")
            Else
                Try
                    checkEvolviProcessRunning = True
                    Dim strDay As String = Mid(Now.DayOfWeek.ToString, 1, 3)

                    'R2.16 CR - Guardian runs first to sort out the file splitting for new handoff file versions
                    'Guardian's input folder should be the EvolviHandoffs folder
                    ''Guardian's output folder should be Interceptor's input folder
                    EvolviInterceptorGuardian(ConfigUtils.getConfig("EvolviGuardianInputFolder"),
                                              ConfigUtils.getConfig("EvolviGuardianOutputFolder"),
                                              ConfigUtils.getConfig("EvolviProcessSqlOutput"))

                    'EvolviProcessFolder(getConfig("EvolviProcessInput"), getConfig("EvolviProcessBossOutput"), getConfig("EvolviProcessSqlOutput"))
                    'CL 20/02/15 - This procedure will now take in the temporary BOSS prep folder to limit the amount of locks.
                    'EvolviProcessFolder(getConfig("EvolviGuardianInputFolder"), getConfig("EvolviProcessTemporaryBossOutput"), getConfig("EvolviProcessSqlOutput"))
                Finally
                    checkEvolviProcessRunning = False
                End Try
            End If
        Catch ex As Exception
            EventLogger.logError(Log, ex, "checkEvolviProcess")
        End Try
    End Sub

    Private triggerLauncherRunning As Boolean = False

    Public Sub TriggerLauncher()
        Try
            If triggerLauncherRunning Then
                EventLogger.logError(Log, "Tried to call triggerLauncher, but " &
                                          "it's still running from the previous wake up.",
                                     "triggerLauncher")
            Else
                Try
                    Log.Info("checking for todays triggers")
                    triggerLauncherRunning = True
                    'first roll over any enquiries that have taken place
                    CheckEnquiries() '--------> includes the calculation of Admin Fee
                    CheckEtPenquiries()
                    CheckContractsEnquiries()
                    'R2.9 NM
                    CheckContractsOutReach()
                    'R2.8 NM
                    CheckNonCommEnquiries()
                    'R2.9 NM
                    CheckBilledAsBookedEnquiries()
                    'now go get all triggers that apply
                    Dim strwhathappened As String = clsTriggerDat.triggerCreate()
                    Log.Info(strwhathappened)
                    'now clear all files for email temp folder so it doesn't get too big
                    ClearEmailTemp()
                Finally
                    triggerLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            EventLogger.logError(Log, ex, "triggerLauncher")
            SendEmail.safeSend("running triggers", "", ex)
        End Try
    End Sub

    'Public Sub ConfermaLauncher()
    'End Sub

    'Public Sub ConfermaBookedLauncher()
    'End Sub

    'Public Sub ConfermaBookedAftLauncher()
    'End Sub

    'Public Sub ConfermaTransLauncher()
    'End Sub

    Private Sub ClearEmailTemp()
        Try
            Log.Info("Trying to clear old files")
            For Each s As String In Directory.GetDirectories(ConfigUtils.getConfig("EmailTempFolder"))
                For Each s2 As String In Directory.GetFiles(s)
                    File.Delete(s2)
                Next s2
                Directory.Delete(s, True)
            Next s

            For Each sX As String In Directory.GetDirectories(ConfigUtils.getConfig("RequirementsFolder"))
                For Each s2X As String In Directory.GetFiles(sX)
                    File.Delete(s2X)
                Next s2X
                Directory.Delete(sX, True)
            Next sX
        Catch ex As Exception
            EventLogger.logError(Log, ex, "clearEmailTemp")
        End Try
    End Sub

    Public Sub CheckContractsEnquiries()
        Dim oContracts As List(Of clsContractsDat)
        oContracts = clsContractsDat.contractsNotReceived(0)

        For Each oContract As clsContractsDat In oContracts
            If SendContractsNotReceivedEmail(oContract.EnquiryID,
                                             oContract.enquirynysref,
                                             oContract.bookingconfirmeddate,
                                             oContract.companyname,
                                             oContract.enquiryeventdate,
                                             oContract.venuename,
                                             oContract.venuecontactemail,
                                             oContract.CompanyID,
                                             oContract.GroupID,
                                             oContract.GroupName) Then
                clsContractsDat.contractsNotReceivedSent(oContract.EnquiryID)
            End If
        Next
    End Sub

    'R2.9 NM
    Public Sub CheckSlaEnquiries()
        Dim oEmails As List(Of clsEnquiryNew)
        oEmails = clsEnquiryNew.enquirySLAEmail

        For Each oEmail As clsEnquiryNew In oEmails
            If SendSlaEmail(oEmail.enquiryid,
                            oEmail.companyid,
                            oEmail.companyname,
                            oEmail.groupid,
                            oEmail.groupname,
                            oEmail.enquiryeventdate,
                            oEmail.contactfirstname,
                            oEmail.contactemail,
                            oEmail.companyNYSemailbox,
                            oEmail.companyNYSteamtel,
                            oEmail.enquirynysref) Then
                clsEnquiryNew.enquirySLAUpdate(oEmail.enquiryid)
            End If
        Next
    End Sub

    Private Function SendSlaEmail(penquiryid As Integer,
                                  pcompanyid As Integer,
                                  pcompanyname As String,
                                  pgroupid As Integer,
                                  pgroupname As String,
                                  penquiryeventdate As String,
                                  pcontactfirstname As String,
                                  pcontactemail As String,
                                  pcompanyNySemailbox As String,
                                  pcompanyNySteamtel As String,
                                  penquirynysref As String) As Boolean
        Try

            Dim ofile As New StreamReader(ConfigUtils.getConfig("HomeAbsolutepath") & "userdocs\NYS\SLA.htm")

            Dim strreadtest As String = ofile.ReadToEnd & "<p>"
            ofile.Close()

            strreadtest = strreadtest.Replace("#contactfirstname#", pcontactfirstname)
            strreadtest = strreadtest.Replace("#eventdate#", penquiryeventdate)
            strreadtest = strreadtest.Replace("#nysref#", penquirynysref)
            strreadtest = strreadtest.Replace("#nysteamemail#", pcompanyNySemailbox)
            strreadtest = strreadtest.Replace("#nysteamtel#", pcompanyNySteamtel)

            Dim strfrom As String = pcompanyNySemailbox

            Dim strSavePath As String = ConfigUtils.getConfig("HomeAbsolutepath") & "userdocs\" & CStr(pgroupid) & "-" &
                                        CStr(pgroupname) & "\" & CStr(pcompanyid) &
                                        "-" & CStr(pcompanyname) & "\emails\body\"

            Dim strMessage As String = strreadtest
            Dim strTo As String = pcontactemail

            If strTo = "" Then
                SendEmail.send(strfrom, "nick.massarella@nysgroup.com", "Auto SLA error: No contact email", strMessage,
                               "", "", "")
                Return False
            End If

            If ConfigUtils.getConfig("ReleaseEmailTest") = "true" Then
                strTo = ConfigUtils.getConfig("ReleaseEmailTestSend")
                strMessage = "Email would have been sent to: " & pcontactemail & " - " & strMessage
            End If

            SendEmail.send(strfrom, strTo, "Your recent enquiry", strMessage, "", "", "")

            SaveHistory(penquiryid, Format(Date.Now, "dd/MM/yyyy"),
                        "SLA email sent to:" & strTo, "", "email", "", "Auto email",
                        "Complete", 1, strMessage, strSavePath)
            Return True
        Catch ex As Exception
            Log.Error("SLA EMAIL ERROR: " & ex.Message)
            Return False
        End Try
    End Function

    Public Sub CheckContractsOutReach()
        Dim oContracts As List(Of clsContractsDat)
        oContracts = clsContractsDat.contractsOutReach

        For Each oContract As clsContractsDat In oContracts
            clsContractsDat.saveNoContracts(oContract.EnquiryID)
        Next
    End Sub

    Public Sub CheckNonCommEnquiries()
        'R2.8 NM look for non comm bookings older than 2 days at ETP
        Dim oNonComs As List(Of clsEnquiryNew)
        oNonComs = clsEnquiryNew.enquiryCheckNonCom
        For Each oNonCom As clsEnquiryNew In oNonComs
            Dim oEnquiryList2 As New clsEnquiryDat
            oEnquiryList2.saveauto(oNonCom.enquirystatus, oNonCom.enquiryid, 5)
        Next
    End Sub

    Public Sub CheckBilledAsBookedEnquiries()

        'R2.9 NM

        Dim oBabs As New List(Of clsEnquiryNew)
        oBabs = clsEnquiryNew.enquiryBookedAsBilledCheck
        For Each oBab As clsEnquiryNew In oBabs
            Dim oEnquiryList3 As New clsEnquiryDat
            oEnquiryList3.saveauto(oBab.enquirystatus, oBab.enquiryid, 8)
        Next
    End Sub

    Private Function SendContractsNotReceivedEmail(penquiryid As Integer,
                                                   penquirynysref As String,
                                                   pbookingconfirmeddate As String,
                                                   pcompanyname As String,
                                                   penquiryeventdate As String,
                                                   pvenuename As String,
                                                   pvenuecontactemail As String,
                                                   pcompanyid As Integer,
                                                   pgroupid As Integer,
                                                   pgroupname As String) As Boolean
        Try

            Dim ofile As New StreamReader(ConfigUtils.getConfig("HomeAbsolutepath") & "userdocs\NYS\ContractsChase.htm")

            Dim strreadtest As String = ofile.ReadToEnd & "<p>"
            ofile.Close()

            strreadtest = strreadtest.Replace("#nysref#", penquirynysref)
            strreadtest = strreadtest.Replace("#confirmdate#", pbookingconfirmeddate)
            strreadtest = strreadtest.Replace("#clientname#", pcompanyname)
            strreadtest = strreadtest.Replace("#venuename#", pvenuename)
            strreadtest = strreadtest.Replace("#eventdate#", penquiryeventdate)

            If Date.Now.TimeOfDay.Hours >= 12 Then
                strreadtest = strreadtest.Replace("#timeofday#", "afternoon")
            Else
                strreadtest = strreadtest.Replace("#timeofday#", "morning")
            End If

            Dim strfrom = "contracts@nysgroup.com"

            Dim strSavePath As String = ConfigUtils.getConfig("HomeAbsolutepath") & "userdocs\" & CStr(pgroupid) & "-" &
                                        CStr(pgroupname) & "\" & CStr(pcompanyid) &
                                        "-" & CStr(pcompanyname) & "\emails\body\"

            Dim strMessage As String = strreadtest
            Dim strTo As String = pvenuecontactemail

            If strTo = "" Then
                SendEmail.send(strfrom, "nick.massarella@nysgroup.com",
                               "Auto contracts not received error: No venue email", strMessage, "", "", "")
                Return False
            End If

            If ConfigUtils.getConfig("ReleaseEmailTest") = "true" Then
                strTo = ConfigUtils.getConfig("ReleaseEmailTestSend")
                strMessage = "Email would have been sent to: " & pvenuecontactemail & " - " & strMessage
            End If

            SendEmail.send(strfrom, strTo, "Venue Chase For Contract/ Confirmation", strMessage, "", "", "")

            SaveHistory(penquiryid, Format(Date.Now, "dd/MM/yyyy"),
                        "Contracts not received notification email sent to:" & strTo, "", "email", "", "Auto email",
                        "Complete", 1, strMessage, strSavePath)
            Return True
        Catch ex As Exception
            Log.Error("AUTO INVOICE EMAIL ERROR: " & ex.Message)
            Return False
        End Try
    End Function

    Public Sub CheckEnquiries()
        Dim oEnquirys, oEnquiry As New clsEnquiryDat
        'first check to see if new year so can reset ref number
        Dim dtcheck = CDate(clsEnquiryDat.checkdate())
        Log.Info("last check date was " & Format(dtcheck, "dd/MM/yyyy"))
        If dtcheck.Year < Date.Now.Year Then 'implies last year so need to reset ref number
            clsEnquiryDat.resetreffornewyear()
            Log.Info("references reset for new year")
        End If

        'R2.9 NM
        'first get all ETP bookings where booking value = 0 so we can update
        Dim oChecks As New List(Of clsEnquiryNew)
        oChecks = clsEnquiryNew.enquiryZeroBookingValueCheck
        For Each oCheck As clsEnquiryNew In oChecks
            Dim ddr = False
            If oCheck.Typer <> 0 Then
                If oCheck.Typer = 1 Then
                    ddr = True
                End If
                UpdateVenueCosts(oCheck.enquiryvenueid, ddr, oCheck.CanReclaimVat)
            End If
        Next

        oEnquirys.checkenquiry(Format(Date.Now.AddDays(-1), "dd/MM/yyyy"), Format(Date.Now, "dd/MM/yyyy"), 0)
        Dim intcount = 0
        For Each oEnquiry In oEnquirys.mcolEnquiry
            Dim oEnquiryList2 As New clsEnquiryDat
            oEnquiryList2.saveauto(oEnquiry.mintenquirystatus, oEnquiry.mintenquiryid, 1)
            Log.Info("Enquiry: " & CStr(oEnquiry.mintenquiryid) & " rolled over to 'Event Takes Place'")

            'R2.10 NM
            Dim intRet = 0 'Admin Charge  999
            If ConfigUtils.getConfig("NYSNonCommClients").ToString.Contains(CStr(oEnquiry.mintgroupid)) Then

                Log.Info("Testing IsConfigRateWithVenue CompanyID Get : " & oEnquiry.companyid)
                If _
                    NSConfigUtils.IsConfigRateWithVenue(oEnquiry.mintenquiryvenueid, CStr(oEnquiry.mintgroupid),
                                                        CStr(oEnquiry.companyid)) Then
                    SaveOtherFee(oEnquiry.mintenquiryid, oEnquiry.mintenquiryvenueid, oEnquiry.mintgroupid)
                End If

            End If
        Next
    End Sub

    'R2.10 NM
    Private Function SaveOtherFee(pintenquiryid As Integer, pintenquiryvenueid As Integer, pintgroupid As Integer) _
        As Boolean

        Dim oCosts As New List(Of clsEnquiryVenue)
        oCosts = clsEnquiryVenue.enquiryVenueValueGet(pintenquiryvenueid)
        Dim decCalcdNet As Decimal = 0
        Dim decCalcdVat As Decimal = 0

        For Each oCost As clsEnquiryVenue In oCosts

            'Dim FeePerc As Decimal = CDec(getConfig("TCSAdmin"))

            Try
                Dim feePerc As Decimal = NSConfigUtils.ReadConfigRateWithVenueEnquiry(pintenquiryvenueid, pintenquiryid)

                decCalcdNet = CDec(Math.Round((oCost.nett / 100) * feePerc, 2))
                decCalcdVat = (decCalcdNet / 100) * clsEnquiryVenueDat.getVenueVatRate(pintenquiryvenueid, False)

                Log.Info(
                    "OK  -- saveOtherFee: venue id " + pintenquiryvenueid.ToString() + " EnquiryID " +
                    pintenquiryid.ToString() + " Admin Fee based on % " + feePerc.ToString())
            Catch ex As Exception
                Log.Error(
                    "ERR -- saveOtherFee: venue id " + pintenquiryvenueid.ToString() + " EnquiryID " +
                    pintenquiryid.ToString())

            End Try

        Next
        Dim oOther As New clsOtherDat

        'R2.17a CR - work out what kind of fee we should be adding, previously just adding as "NYS Admin Fee"
        Dim intRequirementId = 0
        Dim strComments = "NYS Admin fee"
        If ConfigUtils.getConfig("NYSNonCommClients").ToString.Contains(CStr(pintgroupid)) Then
            'this is a non comm fee - not an admin fee
            Dim oSysParam As New clsSystemparameter
            oSysParam = clsSystemparameter.populateByNameAndValue("Other requirement",
                                                                  "NYS Corporate non commissionable fee")
            intRequirementId = oSysParam.Systemparameterid
            strComments = oSysParam.Systemparametervalue
        End If

        oOther.mintenquiryvenueid = pintenquiryvenueid
        oOther.mintotherid = 0
        oOther.mstrotherdaterequired = clsEnquiryNew.enquiryDateGet(pintenquiryid)
        oOther.mintotherqty = 1
        oOther.mstrothercosttype = "AC"
        oOther.mintotherdays = 0 'CInt(txtotherdays.Text)

        oOther.mdblotherratenet = decCalcdNet
        oOther.mdblotherVAT = decCalcdVat
        oOther.mbotherdeleinc = False
        oOther.mbotherquote = True
        oOther.mdblothersavings = CDbl(0)

        'R2.17a CR - set the comments to what we have worked out above
        oOther.mstrothercomments = strComments

        oOther.mstrotherinstructions = ""
        oOther.mstrotherconfirmed = ""
        oOther.mbotherpaiddirect = False

        'R2.17a CR - set the requirement
        oOther.mintotherrequirement = intRequirementId

        'R2.17a CR - set the supplier!
        'Otherwise accounts can have problems reconciling the amounts.
        'oOther.mintothersupplier = 0
        Dim oSysParamSupplier As New clsSystemparameter
        oSysParamSupplier = clsSystemparameter.populateByNameAndValue("Other supplier", "NYS Corporate")
        oOther.mintothersupplier = oSysParamSupplier.Systemparameterid

        oOther.mbothercommision = False
        oOther.mintothertype = 0

        'R2.10 NM
        oOther.mbotheroverride = False
        oOther.mbserviceFlag = True

        If oOther.save(-1) = True Then
            If clsEnquiryVenueDat.enquiryVenueCostsUpdate(pintenquiryvenueid, decCalcdNet, decCalcdVat) > 0 Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    ''' <summary>
    '''     R21 NM
    ''' </summary>
    ''' <remarks>Checks for enquiries that have been at ETP only for 2 weeks or more, then for a week following that</remarks>
    Private Sub CheckEtPenquiries()
        'do all 2 week ones first
        If ConfigUtils.getConfig("ETP") = "ON" Then
            Dim oEnqs As New List(Of clsEnquiryDat)
            oEnqs = clsEnquiryDat.checkETPenquiryAM(0)

            For Each oEnq As clsEnquiryDat In oEnqs
                'R2.4 NM need to check if all paiddirect
                'send email
                Dim blnFirstSend = True
                If oEnq.venueQuoteType = 0 Then 'then ddr
                    If clsEnquiryDat.paidDirectCheckDdr(oEnq.enquiryvenueid) = 0 Then
                        blnFirstSend = False
                    End If
                ElseIf oEnq.venueQuoteType = 1 Then 'the room & cat
                    If clsEnquiryDat.paidDirectCheckRc(oEnq.enquiryvenueid) = 0 Then
                        blnFirstSend = False
                    End If
                Else 'error!
                    blnFirstSend = False
                End If

                'Added 27th 11 - This field is the Pay Direct Field in Mevis
                If _
                    oEnq.mbenquirycomissiononly = True Or
                    ConfigUtils.getConfig("PaidDirectClients").ToString.ToLower.Contains(oEnq.groupname.ToLower) Then
                    blnFirstSend = False
                End If

                If blnFirstSend Then
                    'R2.6 CR - added oEnq.companyaccountstel
                    If SendEtPemailToVenue(oEnq.enquiryid,
                                           oEnq.groupid,
                                           oEnq.groupname,
                                           oEnq.eventname,
                                           oEnq.companyid,
                                           oEnq.companyname,
                                           oEnq.companyaccountsemail,
                                           oEnq.venueaccountsemail,
                                           oEnq.eventdate,
                                           oEnq.nysref,
                                           oEnq.companyaccountstel) Then

                        'update invoice record
                        clsInvoicingDat.invoiceAutoSave(oEnq.enquiryid, Now)
                    Else
                        'update invoice record so can find later
                        clsInvoicingDat.invoiceAutoSave(oEnq.enquiryid, CDate("01/01/2999"))
                    End If
                Else
                    'update invoice record so can find later
                    clsInvoicingDat.invoiceAutoSave(oEnq.enquiryid, CDate("01/01/2999"))
                End If
            Next
            'now do weekly ones
            Dim oWeekEnqs As New List(Of clsEnquiryDat)
            oWeekEnqs = clsEnquiryDat.checkETPWeeklyenquiry(0)

            For Each oWeekEnq As clsEnquiryDat In oEnqs
                ' R2.4 NM need to check if all paiddirect
                'send email
                Dim blnSecondSend = True
                If oWeekEnq.venueQuoteType = 0 Then 'then ddr
                    If clsEnquiryDat.paidDirectCheckDdr(oWeekEnq.enquiryvenueid) = 0 Then
                        blnSecondSend = False
                    End If
                ElseIf oWeekEnq.venueQuoteType = 1 Then 'the room & cat
                    If clsEnquiryDat.paidDirectCheckRc(oWeekEnq.enquiryvenueid) = 0 Then
                        blnSecondSend = False
                    End If
                Else 'error!
                    blnSecondSend = False
                End If

                If _
                    oWeekEnq.mbenquirycomissiononly = True Or
                    ConfigUtils.getConfig("PaidDirectClients").ToString.ToLower.Contains(oWeekEnq.groupname.ToLower) _
                    Then
                    blnSecondSend = False
                End If
                'send email
                If blnSecondSend Then
                    'R2.6 CR - added oWeekEnq.companyaccountstel
                    If SendEtPemailToVenue(oWeekEnq.enquiryid,
                                           oWeekEnq.groupid,
                                           oWeekEnq.groupname,
                                           oWeekEnq.eventname,
                                           oWeekEnq.companyid,
                                           oWeekEnq.companyname,
                                           oWeekEnq.companyaccountsemail,
                                           oWeekEnq.venueaccountsemail,
                                           oWeekEnq.eventdate,
                                           oWeekEnq.nysref,
                                           oWeekEnq.companyaccountstel) Then

                        'update invoice record
                        clsInvoicingDat.invoiceAutoSave(oWeekEnq.enquiryid, Now)
                    Else
                        'this shouldn't happen as we sent one a week ago!!!!
                        'update invoice record so can find later
                        clsInvoicingDat.invoiceAutoSave(oWeekEnq.enquiryid, CDate("01/01/2999"))
                    End If
                Else
                    'this shouldn't happen as we sent one a week ago!!!!
                    'update invoice record so can find later
                    clsInvoicingDat.invoiceAutoSave(oWeekEnq.enquiryid, CDate("01/01/2999"))
                End If
            Next
        End If
    End Sub

    Private Function SendEtPemailToVenue(penquiryid As Integer,
                                         pgroupid As Integer,
                                         pgroupname As String,
                                         peventname As String,
                                         pcompanyid As Integer,
                                         pcompanyname As String,
                                         pCompanyAccountsEmailFrom As String,
                                         pVenueAccountsEmailTo As String,
                                         peventdate As String,
                                         pnysref As String,
                                         pcompanyaccountstel As String) As Boolean
        Try

            Dim ofile As New StreamReader(ConfigUtils.getConfig("HomeAbsolutepath") & "userdocs\NYS\ETPVenue.htm")

            Dim strreadtest As String = ofile.ReadToEnd & "<p>"
            ofile.Close()

            strreadtest = strreadtest.Replace("#nysref#", pnysref)
            strreadtest = strreadtest.Replace("#eventname#", peventname)
            strreadtest = strreadtest.Replace("#groupname#", pgroupname)
            strreadtest = strreadtest.Replace("#eventdate#", peventdate)

            'R2.6 CR
            strreadtest = strreadtest.Replace("#companyaccountstel#", pcompanyaccountstel)

            Dim strfrom As String = pCompanyAccountsEmailFrom
            If strfrom = "" Then
                strfrom = "accounts@nysgroup.com"
            End If

            Dim strSavePath As String = ConfigUtils.getConfig("HomeAbsolutepath") & "userdocs\" & CStr(pgroupid) & "-" &
                                        CStr(pgroupname) & "\" & CStr(pcompanyid) &
                                        "-" & CStr(pcompanyname) & "\emails\body\"

            Dim strMessage As String = strreadtest
            Dim strTo As String = pVenueAccountsEmailTo

            If strTo = "" Then
                SendEmail.send(strfrom, "mike.kirk@nysgroup.com", "Auto invoice error: No venue email", strMessage, "",
                               "", "")
                Return False
            End If

            If ConfigUtils.getConfig("ReleaseEmailTest") = "true" Then
                strTo = ConfigUtils.getConfig("ReleaseEmailTestSend")
                strMessage = "Email would have been sent to: " & pVenueAccountsEmailTo & " - " & strMessage
            End If

            SendEmail.send(strfrom, strTo, ConfigUtils.getConfig("ETPSubject"), strMessage, "", "", "")

            SaveHistory(penquiryid, Format(Date.Now, "dd/MM/yyyy"),
                        ConfigUtils.getConfig("ETPSubject") & " email sent to:" & strTo, "", "email", "", "Auto email",
                        "Complete", 3, strMessage, strSavePath)
            Return True
        Catch ex As Exception
            Log.Error("AUTO INVOICE EMAIL ERROR: " & ex.Message)
            Return False
        End Try
    End Function

    Public Function SaveHistory(pintareaid As Integer,
                                pstrdatecreated As String,
                                pstrdescription As String,
                                pstrdocument As String,
                                pstrtype As String,
                                pstrdays As String,
                                pstruser As String,
                                pstrstatus As String,
                                pintarea As Integer,
                                pstrbody As String,
                                pstrpath As String) As Integer

        Dim oDat As New clsHistoryDat
        oDat.minthistoryid = 0
        oDat.mintareaid = pintareaid
        oDat.mstrhistorydatecreated = pstrdatecreated
        oDat.minthistorytimehour = Date.Now.Hour
        oDat.minthistorytimemin = Date.Now.Minute
        oDat.mstrhistorytype = pstrtype
        oDat.mstrhistorydesription = pstrdescription
        oDat.mstrhistorydueday = pstrdays
        oDat.mstrhistoryuser = pstruser
        oDat.mstrhistorystatus = pstrstatus
        oDat.mstrhistorydocumentname = pstrdocument
        oDat.minthistoryarea = pintarea
        If pstrtype = "email" Then
            oDat.mstrhistorybody = "1"
        Else
            oDat.mstrhistorybody = "0"
        End If

        SaveHistory = oDat.save()

        If pstrpath <> "" And pstrbody <> "0" Then
            Dim ofiler As New StreamWriter(pstrpath & CStr(oDat.minthistoryid) & ".htm", False)
            ofiler.Write(pstrbody)
            ofiler.Flush()
            ofiler.Close()
        End If
    End Function

    Public Sub EvolviReader(pDayOfWeek As String)
        Try
            Dim strEvolviFilesLocation As String = ConfigUtils.getConfig("EvolviHandoffs") _
            ' \\nysmgmt\EvolviHandoffs\intercept\SQL\
            Dim strDay As String = pDayOfWeek
            If ConfigUtils.getConfig("EvolviDay") <> "" Then
                strDay = ConfigUtils.getConfig("EvolviDay") 'but in the app it is ""
            End If
            Dim fileCount As Integer = Directory.GetFiles(strEvolviFilesLocation, "*.xml").Length _
            'System.IO.Directory.GetFiles(strEvolviFilesLocation).Length   '  \\nysmgmt\EvolviHandoffs\intercept\SQL\
            Dim arrFiles() As String = Directory.GetFiles(strEvolviFilesLocation, "*.xml") ' into an array to manage

            For i = 0 To fileCount - 1
                Dim ref = 0
                Dim externalRef = ""
                Dim issueRef = 0
                Dim transactionType = ""
                Dim transactionDate = ""
                Dim machineType = 0
                Dim machineNumber = ""
                Dim currencyCode = ""
                Dim personalCcUsed = ""
                Dim fulfilmentType = ""
                Dim accountRef = 0
                Dim accountExternalRef = ""
                Dim accountType = ""
                Dim period = ""
                Dim immediate = ""
                Dim evolviInvoice = ""
                Dim evolviPayment = ""
                Dim evolviFinancial = ""
                Dim bookingAgentRef = 0
                Dim bookingAgentExternalRef = ""
                Dim branchExternalRef = ""
                Dim unit = ""
                Dim bookingAgentFirstName = ""
                Dim bookingAgentLastName = ""
                Dim bookingAgentTitle = ""
                Dim bookingAgentEmailAddress = ""
                Dim onBehalfRef = 0
                Dim onBehalfExternalRef = ""
                Dim onBehalfBranchExternalRef = ""
                Dim onBehalfUnit = ""
                Dim onBehalfFirstName = ""
                Dim onBehalfLastName = ""
                Dim onBehalfTitle = ""
                Dim onBehalfEmailAddress = ""

                Dim ticketingAgentRef = 0
                Dim ticketingAgentExternalRef = ""
                Dim ticketingAgentBranchExternalRef = ""
                Dim ticketingAgentUnit = ""
                Dim leadRef = 0
                Dim accountContactRef = 0
                Dim accountContactFirstName = ""
                Dim accountContactLastName = ""
                Dim accountContactTitle = ""
                Dim accountContactNumber = ""
                Dim accountContactEmail = ""
                Dim accountContactOrganisation = ""
                Dim accountContactAddress1 = ""
                Dim accountContactAddress2 = ""
                Dim accountContactAddress3 = ""
                Dim accountContactCity = ""
                Dim accountContactCounty = ""
                Dim accountContactPostcode = ""
                Dim deliveryContactRef = 0
                Dim deliveryContactFirstName = ""
                Dim deliveryContactLastName = ""
                Dim deliveryContactTitle = ""
                Dim deliveryContactNumber = ""
                Dim deliveryContactEmail = ""
                Dim deliveryContactOrganisation = ""
                Dim deliveryContactAddress1 = ""
                Dim deliveryContactAddress2 = ""
                Dim deliveryContactAddress3 = ""
                Dim deliveryContactCity = ""
                Dim deliveryContactCounty = ""
                Dim deliveryContactPostcode = ""
                Try
                    Dim odoc As New XmlDocument

                    odoc.Load(arrFiles(i))

                    ' XML Check through to load variables above
                    Try
                        'R2.23 CR - BUG FIX: apparently the new "SignOnToken" tag isn't ALWAYS there...grrr
                        'so, we take the firstchild name and check it - if it's wrong then twe take the next sibling name and check it
                        'then if BOTH fail we error
                        Dim ndeFirstChild As XmlNode = odoc.LastChild.FirstChild
                        If ndeFirstChild.Name <> "Agency" Then
                            ndeFirstChild = odoc.LastChild.FirstChild.NextSibling
                        End If

                        'R2.23 CR - BUG FIX: added .nextsibling to skip new "SignOnToken" tag
                        If ndeFirstChild.Name = "Agency" Then
                            For Each at As XmlAttribute In ndeFirstChild.Attributes
                                If at.Name = "Ref" Then
                                    ref = notInteger(at.Value)
                                ElseIf at.Name = "ExternalRef" Then
                                    externalRef = at.Value
                                End If
                            Next
                        Else
                            Log.Error("No Agency as first child:" & arrFiles(i).ToString)
                        End If

                        'R2.21.4 CR - BUG FIX: OK, so "FeeOrderRule" isn't always there!! So below comment is wrong
                        'now check to see if the last child is FeeOrderRule, if it is then use the previous sibling
                        'if it's not then just carry on with lastchild.lastchild
                        Dim oNodeToProcess As XmlNode
                        If odoc.LastChild.LastChild.Name = "FeeOrderRule" Then
                            oNodeToProcess = odoc.LastChild.LastChild.PreviousSibling
                        Else
                            oNodeToProcess = odoc.LastChild.LastChild
                        End If

                        'R2.23 CR - BUG FIX: ImmediateDetail is no longer last child, now there is "FeeOrderRule"
                        'PreviousSibling added; should move it back to the correct node
                        If oNodeToProcess.Name = "ImmediateDetail" Then
                            For Each at As XmlAttribute In oNodeToProcess.Attributes
                                If at.Name = "IssueRef" Then
                                    issueRef = notInteger(at.Value)
                                ElseIf at.Name = "TransactionType" Then
                                    transactionType = at.Value
                                ElseIf at.Name = "TransactionDate" Then
                                    transactionDate = Mid(at.Value, 1, 10)
                                    transactionDate = Format(CDate(transactionDate), "dd/MM/yyyy")
                                ElseIf at.Name = "MachineType" Then
                                    machineType = notInteger(at.Value)
                                ElseIf at.Name = "MachineNumber" Then
                                    machineNumber = at.Value
                                ElseIf at.Name = "CurrencyCode" Then
                                    currencyCode = at.Value
                                ElseIf at.Name = "PersonalCCUsed" Then
                                    personalCcUsed = at.Value
                                ElseIf at.Name = "FulfilmentType" Then
                                    fulfilmentType = at.Value
                                End If
                            Next
                            If oNodeToProcess.HasChildNodes Then
                                For iI = 0 To oNodeToProcess.ChildNodes.Count - 1
                                    If oNodeToProcess.ChildNodes(iI).Name = "Account" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                accountRef = notInteger(at.Value)
                                            ElseIf at.Name = "ExternalRef" Then
                                                accountExternalRef = at.Value
                                            ElseIf at.Name = "Type" Then
                                                accountType = at.Value
                                            ElseIf at.Name = "Period" Then
                                                period = at.Value
                                            ElseIf at.Name = "Immediate" Then
                                                immediate = at.Value
                                            ElseIf at.Name = "EvolviInvoice" Then
                                                evolviInvoice = at.Value
                                            ElseIf at.Name = "EvolviPayment" Then
                                                evolviPayment = at.Value
                                            ElseIf at.Name = "EvolviFinancial" Then
                                                evolviFinancial = at.Value
                                            End If
                                        Next
                                    ElseIf oNodeToProcess.ChildNodes(iI).Name = "BookingAgent" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                bookingAgentRef = notInteger(at.Value)
                                            ElseIf at.Name = "ExternalRef" Then
                                                bookingAgentExternalRef = at.Value
                                            ElseIf at.Name = "BranchExternalRef" Then
                                                branchExternalRef = at.Value
                                            ElseIf at.Name = "Unit" Then
                                                unit = at.Value
                                            End If
                                        Next

                                        'R1
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            For iII = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Person" Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "FirstName" Then
                                                            bookingAgentFirstName = at.Value
                                                        ElseIf at.Name = "LastName" Then
                                                            bookingAgentLastName = at.Value
                                                        ElseIf at.Name = "Title" Then
                                                            bookingAgentTitle = at.Value
                                                        End If
                                                    Next
                                                End If
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Email" Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Address" Then
                                                            bookingAgentEmailAddress = at.Value
                                                        End If
                                                    Next
                                                End If
                                            Next
                                        End If
                                        'end of R1

                                        'new bit r2
                                    ElseIf oNodeToProcess.ChildNodes(iI).Name = "OnBehalfOf" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                onBehalfRef = notInteger(at.Value)
                                            ElseIf at.Name = "ExternalRef" Then
                                                onBehalfExternalRef = at.Value
                                            ElseIf at.Name = "BranchExternalRef" Then
                                                onBehalfBranchExternalRef = at.Value
                                            ElseIf at.Name = "Unit" Then
                                                onBehalfUnit = at.Value
                                            End If
                                        Next
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            For iII = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Person" Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "FirstName" Then
                                                            onBehalfFirstName = at.Value
                                                        ElseIf at.Name = "LastName" Then
                                                            onBehalfLastName = at.Value
                                                        ElseIf at.Name = "Title" Then
                                                            onBehalfTitle = at.Value
                                                        End If
                                                    Next
                                                End If
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Email" Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Address" Then
                                                            onBehalfEmailAddress = at.Value
                                                        End If
                                                    Next
                                                End If
                                            Next
                                        End If
                                        'end of R2

                                    ElseIf oNodeToProcess.ChildNodes(iI).Name = "TicketingAgent" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                ticketingAgentRef = notInteger(at.Value)
                                            ElseIf at.Name = "ExternalRef" Then
                                                ticketingAgentExternalRef = at.Value
                                            ElseIf at.Name = "BranchExternalRef" Then
                                                ticketingAgentBranchExternalRef = at.Value
                                            ElseIf at.Name = "Unit" Then
                                                ticketingAgentUnit = at.Value
                                            End If
                                        Next
                                    ElseIf oNodeToProcess.ChildNodes(iI).Name = "PassengerGroup" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "LeadRef" Then
                                                leadRef = notInteger(at.Value)
                                            End If
                                        Next
                                    ElseIf oNodeToProcess.ChildNodes(iI).Name = "AccountContact" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                accountContactRef = notInteger(at.Value)
                                            End If
                                        Next
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            For iII = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Person" Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "FirstName" Then
                                                            accountContactFirstName = at.Value
                                                        ElseIf at.Name = "LastName" Then
                                                            accountContactLastName = at.Value
                                                        ElseIf at.Name = "Title" Then
                                                            accountContactTitle = at.Value
                                                        End If
                                                    Next
                                                ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Phone" Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Number" Then
                                                            accountContactNumber = at.Value
                                                        End If
                                                    Next
                                                ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Email" Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Address" Then
                                                            accountContactEmail = at.Value
                                                        End If
                                                    Next
                                                ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Address" _
                                                    Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Organisation" Then
                                                            accountContactOrganisation = at.Value
                                                        ElseIf at.Name = "Address1" Then
                                                            accountContactAddress1 = at.Value
                                                        ElseIf at.Name = "Address2" Then
                                                            accountContactAddress2 = at.Value
                                                        ElseIf at.Name = "Address3" Then
                                                            accountContactAddress3 = at.Value
                                                        ElseIf at.Name = "City" Then
                                                            accountContactCity = at.Value
                                                        ElseIf at.Name = "County" Then
                                                            accountContactCounty = at.Value
                                                        ElseIf at.Name = "Postcode" Then
                                                            accountContactPostcode = at.Value
                                                        End If
                                                    Next
                                                End If
                                            Next
                                        End If
                                    ElseIf oNodeToProcess.ChildNodes(iI).Name = "DeliveryContact" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                deliveryContactRef = notInteger(at.Value)
                                            End If
                                        Next
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            For iII = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Person" Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "FirstName" Then
                                                            deliveryContactFirstName = at.Value
                                                        ElseIf at.Name = "LastName" Then
                                                            deliveryContactLastName = at.Value
                                                        ElseIf at.Name = "Title" Then
                                                            deliveryContactTitle = at.Value
                                                        End If
                                                    Next
                                                ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Phone" Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Number" Then
                                                            deliveryContactNumber = at.Value
                                                        End If
                                                    Next
                                                ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Email" Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Address" Then
                                                            deliveryContactEmail = at.Value
                                                        End If
                                                    Next
                                                ElseIf oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Address" _
                                                    Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Organisation" Then
                                                            deliveryContactOrganisation = at.Value
                                                        ElseIf at.Name = "Address1" Then
                                                            deliveryContactAddress1 = at.Value
                                                        ElseIf at.Name = "Address2" Then
                                                            deliveryContactAddress2 = at.Value
                                                        ElseIf at.Name = "Address3" Then
                                                            deliveryContactAddress3 = at.Value
                                                        ElseIf at.Name = "City" Then
                                                            deliveryContactCity = at.Value
                                                        ElseIf at.Name = "County" Then
                                                            deliveryContactCounty = at.Value
                                                        ElseIf at.Name = "Postcode" Then
                                                            deliveryContactPostcode = at.Value
                                                        End If
                                                    Next
                                                End If
                                            Next
                                        End If
                                    End If
                                Next
                            End If
                        Else
                            Log.Error("No Immediate as Last child:" & arrFiles(i).ToString)
                        End If

                        'first save all main data
                        ' load clsEvolvi in MEVIS --- NYSDAT
                        Dim intEvolviId As Integer = clsEvolvi.saveEvolvi(ref, externalRef,
                                                                          issueRef, transactionType, transactionDate _
                                                                          , machineType, machineNumber, currencyCode,
                                                                          personalCcUsed, fulfilmentType _
                                                                          , accountRef, accountExternalRef, accountType,
                                                                          period, immediate _
                                                                          , evolviInvoice, evolviPayment,
                                                                          evolviFinancial, bookingAgentRef _
                                                                          , bookingAgentExternalRef, branchExternalRef,
                                                                          unit, bookingAgentFirstName _
                                                                          , bookingAgentLastName, bookingAgentTitle,
                                                                          bookingAgentEmailAddress _
                                                                          , onBehalfRef, onBehalfExternalRef,
                                                                          onBehalfBranchExternalRef _
                                                                          , onBehalfUnit, onBehalfFirstName,
                                                                          onBehalfLastName, onBehalfTitle _
                                                                          , onBehalfEmailAddress, ticketingAgentRef _
                                                                          , ticketingAgentExternalRef,
                                                                          ticketingAgentBranchExternalRef _
                                                                          , ticketingAgentUnit, leadRef,
                                                                          accountContactRef, accountContactFirstName _
                                                                          , accountContactLastName, accountContactTitle,
                                                                          accountContactNumber _
                                                                          , accountContactEmail,
                                                                          accountContactOrganisation,
                                                                          accountContactAddress1 _
                                                                          , accountContactAddress2,
                                                                          accountContactAddress3, accountContactCity _
                                                                          , accountContactCounty, accountContactPostcode,
                                                                          deliveryContactRef _
                                                                          , deliveryContactFirstName,
                                                                          deliveryContactLastName, deliveryContactTitle _
                                                                          , deliveryContactNumber, deliveryContactEmail,
                                                                          deliveryContactOrganisation _
                                                                          , deliveryContactAddress1,
                                                                          deliveryContactAddress2,
                                                                          deliveryContactAddress3 _
                                                                          , deliveryContactCity, deliveryContactCounty,
                                                                          deliveryContactPostcode)

                        odoc = Nothing

                        'now do Custom fields
                        Dim odocCustom As New XmlDocument
                        odocCustom.Load(arrFiles(i))

                        'R2.21.4 CR - BUG FIX: OK, so "FeeOrderRule" isn't always there!!
                        'now check to see if the last child is FeeOrderRule, if it is then use the previous sibling
                        'if it's not then just carry on with lastchild.lastchild
                        If odocCustom.LastChild.LastChild.Name = "FeeOrderRule" Then
                            oNodeToProcess = odocCustom.LastChild.LastChild.PreviousSibling
                        Else
                            oNodeToProcess = odocCustom.LastChild.LastChild
                        End If

                        If oNodeToProcess.Name = "ImmediateDetail" Then
                            If oNodeToProcess.HasChildNodes Then
                                For iI = 0 To oNodeToProcess.ChildNodes.Count - 1
                                    If oNodeToProcess.ChildNodes(iI).Name = "CustomField" Then
                                        Dim customCode = ""
                                        Dim customValue = ""
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Code" Then
                                                customCode = at.Value
                                            ElseIf at.Name = "Value" Then
                                                customValue = at.Value
                                            End If
                                        Next
                                        clsEvolvi.saveCustom(intEvolviId, customCode, customValue)
                                    End If
                                Next
                            End If
                        End If

                        odocCustom = Nothing
                        'now do Passengers
                        Dim odocPassenger As New XmlDocument
                        odocPassenger.Load(arrFiles(i))

                        'R2.21.4 CR - BUG FIX: OK, so "FeeOrderRule" isn't always there!!
                        'now check to see if the last child is FeeOrderRule, if it is then use the previous sibling
                        'if it's not then just carry on with lastchild.lastchild
                        If odocPassenger.LastChild.LastChild.Name = "FeeOrderRule" Then
                            oNodeToProcess = odocPassenger.LastChild.LastChild.PreviousSibling
                        Else
                            oNodeToProcess = odocPassenger.LastChild.LastChild
                        End If

                        If oNodeToProcess.Name = "ImmediateDetail" Then
                            If oNodeToProcess.HasChildNodes Then
                                For iI = 0 To oNodeToProcess.ChildNodes.Count - 1
                                    If oNodeToProcess.ChildNodes(iI).Name = "PassengerGroup" Then
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            For iII = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                Dim passengerRef = 0
                                                Dim adultChild = ""
                                                Dim firstName = ""
                                                Dim lastName = ""
                                                Dim title = ""
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Passenger" Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Ref" Then
                                                            passengerRef = notInteger(at.Value)
                                                        ElseIf at.Name = "AdultChild" Then
                                                            adultChild = at.Value
                                                        End If
                                                    Next
                                                End If
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).HasChildNodes Then
                                                    Dim passengerId = 0
                                                    For iIII = 0 To _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes.Count -
                                                        1
                                                        If _
                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(
                                                                iIII).Name = "Person" Then
                                                            For Each at As XmlAttribute In _
                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(
                                                                    iIII).Attributes
                                                                If at.Name = "FirstName" Then
                                                                    firstName = at.Value
                                                                ElseIf at.Name = "LastName" Then
                                                                    lastName = at.Value
                                                                ElseIf at.Name = "Title" Then
                                                                    title = at.Value
                                                                End If
                                                            Next
                                                            passengerId = clsEvolvi.savePassenger(intEvolviId, leadRef,
                                                                                                  passengerRef,
                                                                                                  adultChild,
                                                                                                  firstName, lastName,
                                                                                                  title)
                                                        ElseIf _
                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(
                                                                iIII).Name = "CustomField" Then
                                                            Dim customCode = ""
                                                            Dim customLabel = ""
                                                            Dim customValue = ""
                                                            For Each at As XmlAttribute In _
                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(
                                                                    iIII).Attributes
                                                                If at.Name = "Code" Then
                                                                    customCode = at.Value
                                                                ElseIf at.Name = "Label" Then
                                                                    customLabel = at.Value
                                                                ElseIf at.Name = "Value" Then
                                                                    customValue = at.Value
                                                                End If
                                                            Next
                                                            clsEvolvi.savePassengerCustom(intEvolviId, passengerId,
                                                                                          customCode, customLabel,
                                                                                          customValue)
                                                        End If
                                                    Next
                                                End If

                                            Next
                                        End If
                                    End If
                                Next
                            End If
                        End If

                        odocPassenger = Nothing

                        'now do segments

                        Dim odocSegment As New XmlDocument
                        Dim segmentRef = 0
                        Dim distance As Double = 0
                        Dim distanceUnits = ""
                        Dim journeyTime As Double = 0

                        odocSegment.Load(arrFiles(i))

                        'R2.21.4 CR - BUG FIX: OK, so "FeeOrderRule" isn't always there!!
                        'now check to see if the last child is FeeOrderRule, if it is then use the previous sibling
                        'if it's not then just carry on with lastchild.lastchild
                        If odocSegment.LastChild.LastChild.Name = "FeeOrderRule" Then
                            oNodeToProcess = odocSegment.LastChild.LastChild.PreviousSibling
                        Else
                            oNodeToProcess = odocSegment.LastChild.LastChild
                        End If

                        If oNodeToProcess.Name = "ImmediateDetail" Then
                            If oNodeToProcess.HasChildNodes Then
                                For iI = 0 To oNodeToProcess.ChildNodes.Count - 1
                                    If oNodeToProcess.ChildNodes(iI).Name = "Segment" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                segmentRef = notInteger(at.Value)
                                            ElseIf at.Name = "Distance" Then
                                                distance = notNumber(at.Value)
                                            ElseIf at.Name = "DistanceUnits" Then
                                                distanceUnits = notString(at.Value)
                                            ElseIf at.Name = "JourneyTime" Then
                                                journeyTime = notNumber(at.Value)
                                            End If
                                        Next
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            Dim originUicCode = ""
                                            Dim originNlcCode = ""
                                            Dim originCrsCode = ""
                                            Dim originName = ""
                                            Dim destinationUicCode = ""
                                            Dim destinationNlcCode = ""
                                            Dim destinationCrsCode = ""
                                            Dim destinationName = ""
                                            For iII = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Origin" Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "UICCode" Then
                                                            originUicCode = at.Value
                                                        ElseIf at.Name = "NLCCode" Then
                                                            originNlcCode = at.Value
                                                        ElseIf at.Name = "CRSCode" Then
                                                            originCrsCode = at.Value
                                                        ElseIf at.Name = "Name" Then
                                                            originName = at.Value
                                                        End If
                                                    Next
                                                ElseIf _
                                                    oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Destination" _
                                                    Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "UICCode" Then
                                                            destinationUicCode = at.Value
                                                        ElseIf at.Name = "NLCCode" Then
                                                            destinationNlcCode = at.Value
                                                        ElseIf at.Name = "CRSCode" Then
                                                            destinationCrsCode = at.Value
                                                        ElseIf at.Name = "Name" Then
                                                            destinationName = at.Value
                                                        End If
                                                    Next
                                                End If
                                            Next
                                            clsEvolvi.saveSegment(segmentRef, distance, distanceUnits, journeyTime,
                                                                  intEvolviId, originUicCode, originNlcCode,
                                                                  originCrsCode, originName, destinationUicCode,
                                                                  destinationNlcCode, destinationCrsCode,
                                                                  destinationName)
                                        End If
                                    End If
                                Next
                            End If
                        End If
                        odocSegment = Nothing

                        'now do legs in segments
                        Dim odocLeg As New XmlDocument
                        Dim segmentRefLeg = 0

                        odocLeg.Load(arrFiles(i))

                        'R2.21.4 CR - BUG FIX: OK, so "FeeOrderRule" isn't always there!!
                        'now check to see if the last child is FeeOrderRule, if it is then use the previous sibling
                        'if it's not then just carry on with lastchild.lastchild
                        If odocLeg.LastChild.LastChild.Name = "FeeOrderRule" Then
                            oNodeToProcess = odocLeg.LastChild.LastChild.PreviousSibling
                        Else
                            oNodeToProcess = odocLeg.LastChild.LastChild
                        End If

                        If oNodeToProcess.Name = "ImmediateDetail" Then
                            If oNodeToProcess.HasChildNodes Then
                                For iI = 0 To oNodeToProcess.ChildNodes.Count - 1
                                    If oNodeToProcess.ChildNodes(iI).Name = "Segment" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                segmentRefLeg = notInteger(at.Value)
                                            End If
                                        Next
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            Dim legRef = 0
                                            Dim direction = ""
                                            Dim departure = ""
                                            Dim arrival = ""
                                            Dim transportMode = ""
                                            Dim legOriginUicCode = ""
                                            Dim legOriginNlcCode = ""
                                            Dim legOriginCrsCode = ""
                                            Dim legOriginName = ""
                                            Dim legDestinationUicCode = ""
                                            Dim legDestinationNlcCode = ""
                                            Dim legDestinationCrsCode = ""
                                            Dim legDestinationName = ""
                                            Dim tocCode = ""
                                            Dim tocName = ""
                                            Dim passengerRef = ""
                                            Dim accomodationUnit = ""
                                            Dim trainRouteRef = ""
                                            Dim trainRouteOriginDeparture = ""
                                            Dim trainRouteDestinationArrival = ""
                                            Dim trainRouteOriginUicCode = ""
                                            Dim trainRouteOriginNlcCode = ""
                                            Dim trainRouteOriginCrsCode = ""
                                            Dim trainRouteOriginName = ""
                                            Dim trainRouteDestinationUicCode = ""
                                            Dim trainRouteDestinationNlcCode = ""
                                            Dim trainRouteDestinationCrsCode = ""
                                            Dim trainRouteDestinationName = ""

                                            For iII = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Leg" Then
                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Ref" Then
                                                            legRef = notInteger(at.Value)
                                                        ElseIf at.Name = "Direction" Then
                                                            direction = at.Value
                                                        ElseIf at.Name = "Departure" Then
                                                            departure = Mid(at.Value, 1, 10)
                                                            departure = Format(CDate(departure), "dd/MM/yyyy")
                                                        ElseIf at.Name = "Arrival" Then
                                                            arrival = Mid(at.Value, 1, 10)
                                                            arrival = Format(CDate(arrival), "dd/MM/yyyy")
                                                        ElseIf at.Name = "TransportMode" Then
                                                            transportMode = at.Value
                                                        End If
                                                    Next
                                                    If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).HasChildNodes Then
                                                        For iIII = 0 To _
                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes.
                                                                Count - 1
                                                            If _
                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(
                                                                    iIII).Name = "Origin" Then
                                                                For Each at As XmlAttribute In _
                                                                    oNodeToProcess.ChildNodes(iI).ChildNodes(iII).
                                                                        ChildNodes(iIII).Attributes
                                                                    If at.Name = "UICCode" Then
                                                                        legOriginUicCode = at.Value
                                                                    ElseIf at.Name = "NLCCode" Then
                                                                        legOriginNlcCode = at.Value
                                                                    ElseIf at.Name = "CRSCode" Then
                                                                        legOriginCrsCode = at.Value
                                                                    ElseIf at.Name = "Name" Then
                                                                        legOriginName = at.Value
                                                                    End If
                                                                Next
                                                            ElseIf _
                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(
                                                                    iIII).Name = "Destination" Then
                                                                For Each at As XmlAttribute In _
                                                                    oNodeToProcess.ChildNodes(iI).ChildNodes(iII).
                                                                        ChildNodes(iIII).Attributes
                                                                    If at.Name = "UICCode" Then
                                                                        legDestinationUicCode = at.Value
                                                                    ElseIf at.Name = "NLCCode" Then
                                                                        legDestinationNlcCode = at.Value
                                                                    ElseIf at.Name = "CRSCode" Then
                                                                        legDestinationCrsCode = at.Value
                                                                    ElseIf at.Name = "Name" Then
                                                                        legDestinationName = at.Value
                                                                    End If
                                                                Next
                                                            ElseIf _
                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(
                                                                    iIII).Name = "TOC" Then
                                                                For Each at As XmlAttribute In _
                                                                    oNodeToProcess.ChildNodes(iI).ChildNodes(iII).
                                                                        ChildNodes(iIII).Attributes
                                                                    If at.Name = "Code" Then
                                                                        tocCode = at.Value
                                                                    ElseIf at.Name = "Name" Then
                                                                        tocName = at.Value
                                                                    End If
                                                                Next
                                                            ElseIf _
                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(
                                                                    iIII).Name = "Reservation" Then
                                                                For Each at As XmlAttribute In _
                                                                    oNodeToProcess.ChildNodes(iI).ChildNodes(iII).
                                                                        ChildNodes(iIII).Attributes
                                                                    If at.Name = "PassengerRef" Then
                                                                        passengerRef = at.Value
                                                                    ElseIf at.Name = "AccomodationUnit" Then
                                                                        accomodationUnit = at.Value
                                                                    End If
                                                                Next
                                                            ElseIf _
                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(
                                                                    iIII).Name = "TrainRoute" Then
                                                                For Each at As XmlAttribute In _
                                                                    oNodeToProcess.ChildNodes(iI).ChildNodes(iII).
                                                                        ChildNodes(iIII).Attributes
                                                                    If at.Name = "Ref" Then
                                                                        trainRouteRef = at.Value
                                                                    ElseIf at.Name = "OriginDeparture" Then
                                                                        trainRouteOriginDeparture = Mid(at.Value, 1, 10)
                                                                        trainRouteOriginDeparture =
                                                                            Format(CDate(trainRouteOriginDeparture),
                                                                                   "dd/MM/yyyy")
                                                                    ElseIf at.Name = "DestinationArrival" Then
                                                                        trainRouteDestinationArrival = Mid(at.Value, 1,
                                                                                                           10)
                                                                        trainRouteDestinationArrival =
                                                                            Format(CDate(trainRouteOriginDeparture),
                                                                                   "dd/MM/yyyy")
                                                                    End If
                                                                Next
                                                                If _
                                                                    oNodeToProcess.ChildNodes(iI).ChildNodes(iII).
                                                                        ChildNodes(iIII).HasChildNodes Then
                                                                    For iIIII = 0 To _
                                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).
                                                                            ChildNodes(iIII).ChildNodes.Count - 1
                                                                        If _
                                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII) _
                                                                                .ChildNodes(iIII).ChildNodes(iIIII).Name =
                                                                            "Origin" Then
                                                                            For Each at As XmlAttribute In _
                                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(
                                                                                    iII).ChildNodes(iIII).ChildNodes(
                                                                                        iIIII).Attributes
                                                                                If at.Name = "UICCode" Then
                                                                                    trainRouteOriginUicCode = at.Value
                                                                                ElseIf at.Name = "NLCCode" Then
                                                                                    trainRouteOriginNlcCode = at.Value
                                                                                ElseIf at.Name = "CRSCode" Then
                                                                                    trainRouteOriginCrsCode = at.Value
                                                                                ElseIf at.Name = "Name" Then
                                                                                    trainRouteOriginName = at.Value
                                                                                End If
                                                                            Next
                                                                        ElseIf _
                                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII) _
                                                                                .ChildNodes(iIII).ChildNodes(iIIII).Name =
                                                                            "Destination" Then
                                                                            For Each at As XmlAttribute In _
                                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(
                                                                                    iII).ChildNodes(iIII).ChildNodes(
                                                                                        iIIII).Attributes
                                                                                If at.Name = "UICCode" Then
                                                                                    trainRouteDestinationUicCode =
                                                                                        at.Value
                                                                                ElseIf at.Name = "NLCCode" Then
                                                                                    trainRouteDestinationNlcCode =
                                                                                        at.Value
                                                                                ElseIf at.Name = "CRSCode" Then
                                                                                    trainRouteDestinationCrsCode =
                                                                                        at.Value
                                                                                ElseIf at.Name = "Name" Then
                                                                                    trainRouteDestinationName = at.Value
                                                                                End If
                                                                            Next
                                                                        End If
                                                                    Next
                                                                End If
                                                            End If
                                                        Next
                                                        clsEvolvi.saveSegmentLeg(legRef, segmentRefLeg, intEvolviId,
                                                                                 direction, departure, arrival,
                                                                                 transportMode,
                                                                                 legOriginUicCode, legOriginNlcCode,
                                                                                 legOriginCrsCode,
                                                                                 legOriginName, legDestinationUicCode,
                                                                                 legDestinationNlcCode,
                                                                                 legDestinationCrsCode,
                                                                                 legDestinationName, tocCode, tocName,
                                                                                 passengerRef, accomodationUnit,
                                                                                 trainRouteRef,
                                                                                 trainRouteOriginDeparture,
                                                                                 trainRouteDestinationArrival,
                                                                                 trainRouteOriginUicCode,
                                                                                 trainRouteOriginNlcCode,
                                                                                 trainRouteOriginCrsCode,
                                                                                 trainRouteOriginName,
                                                                                 trainRouteDestinationUicCode,
                                                                                 trainRouteDestinationNlcCode,
                                                                                 trainRouteDestinationCrsCode,
                                                                                 trainRouteDestinationName)
                                                    End If
                                                End If
                                            Next
                                        End If
                                    End If
                                Next
                            End If
                        End If

                        odocLeg = Nothing

                        'now do emissions in segments
                        Dim oEmissionLeg As New XmlDocument
                        Dim segmentRefEmission = 0
                        Dim transportType = ""
                        Dim emissions As Double = 0

                        oEmissionLeg.Load(arrFiles(i))

                        'R2.21.4 CR - BUG FIX: OK, so "FeeOrderRule" isn't always there!!
                        'now check to see if the last child is FeeOrderRule, if it is then use the previous sibling
                        'if it's not then just carry on with lastchild.lastchild
                        If oEmissionLeg.LastChild.LastChild.Name = "FeeOrderRule" Then
                            oNodeToProcess = oEmissionLeg.LastChild.LastChild.PreviousSibling
                        Else
                            oNodeToProcess = oEmissionLeg.LastChild.LastChild
                        End If

                        If oNodeToProcess.Name = "ImmediateDetail" Then
                            If oNodeToProcess.HasChildNodes Then
                                For iI = 0 To oNodeToProcess.ChildNodes.Count - 1
                                    If oNodeToProcess.ChildNodes(iI).Name = "Segment" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                segmentRefEmission = notInteger(at.Value)
                                            End If
                                        Next
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            For iII = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If _
                                                    oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name =
                                                    "CarbonEmissionDetails" Then
                                                    If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).HasChildNodes Then
                                                        For iIII = 0 To _
                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes.
                                                                Count - 1
                                                            If _
                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(
                                                                    iIII).Name = "CarbonEmissions" Then
                                                                For Each at As XmlAttribute In _
                                                                    oNodeToProcess.ChildNodes(iI).ChildNodes(iII).
                                                                        ChildNodes(iIII).Attributes
                                                                    If at.Name = "TransportType" Then
                                                                        transportType = at.Value
                                                                    ElseIf at.Name = "Emissions" Then
                                                                        emissions = notNumber(at.Value)
                                                                    End If
                                                                Next
                                                                clsEvolvi.saveSegmentEmissions(intEvolviId,
                                                                                               segmentRefEmission,
                                                                                               transportType, emissions)
                                                            End If
                                                        Next
                                                    End If
                                                End If
                                            Next
                                        End If
                                    End If
                                Next
                            End If
                        End If

                        oEmissionLeg = Nothing
                        'now do tickets in segments
                        Dim odocTicket As New XmlDocument
                        Dim segmentRefTicket = 0

                        odocTicket.Load(arrFiles(i))

                        'R2.21.4 CR - BUG FIX: OK, so "FeeOrderRule" isn't always there!!
                        'now check to see if the last child is FeeOrderRule, if it is then use the previous sibling
                        'if it's not then just carry on with lastchild.lastchild
                        If odocTicket.LastChild.LastChild.Name = "FeeOrderRule" Then
                            oNodeToProcess = odocTicket.LastChild.LastChild.PreviousSibling
                        Else
                            oNodeToProcess = odocTicket.LastChild.LastChild
                        End If

                        If oNodeToProcess.Name = "ImmediateDetail" Then
                            If oNodeToProcess.HasChildNodes Then
                                For iI = 0 To oNodeToProcess.ChildNodes.Count - 1
                                    If oNodeToProcess.ChildNodes(iI).Name = "Segment" Then
                                        For Each at As XmlAttribute In oNodeToProcess.ChildNodes(iI).Attributes
                                            If at.Name = "Ref" Then
                                                segmentRefTicket = notInteger(at.Value)
                                            End If
                                        Next
                                        If oNodeToProcess.ChildNodes(iI).HasChildNodes Then
                                            For iII = 0 To oNodeToProcess.ChildNodes(iI).ChildNodes.Count - 1
                                                If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Name = "Ticket" Then
                                                    Dim ticketRef = 0
                                                    Dim ticketPassengerRef = 0
                                                    Dim ticketTcn = 0
                                                    Dim ticketAdultChild = ""
                                                    Dim ticketCode = ""
                                                    Dim ticketName = ""
                                                    Dim ticketClass = ""
                                                    Dim ticketSingleReturn = ""
                                                    Dim ticketRouteCode = ""
                                                    Dim ticketRoute = ""
                                                    Dim ticketDistance As Double = 0
                                                    Dim fareTotalAmount As Double = 0
                                                    Dim fareVatAmount As Double = 0
                                                    Dim fareVatCode = 0
                                                    Dim discountTotalAmount As Double = 0
                                                    Dim discountVatAmount As Double = 0
                                                    Dim discountVatCode = 0
                                                    Dim transactionChargeTotalAmount As Double = 0
                                                    Dim transactionChargeVatAmount As Double = 0
                                                    Dim transactionChargeVatCode = 0
                                                    Dim fulfilmentFeeTotalAmount As Double = 0
                                                    Dim fulfilmentFeeVatAmount As Double = 0
                                                    Dim fulfilmentFeeVatCode = 0
                                                    Dim creditCardChargeTotalAmount As Double = 0
                                                    Dim creditCardChargeVatAmount As Double = 0
                                                    Dim creditCardChargeVatCode = 0
                                                    Dim normalFare As Double = 0
                                                    Dim offeredFare As Double = 0
                                                    Dim railcardCode = ""
                                                    Dim railcardName = ""
                                                    Dim grossRefundTotalAmount As Double = 0
                                                    Dim grossRefundVatAmount As Double = 0
                                                    Dim grossRefundVatCode = 0
                                                    Dim discountReclaimedTotalAmount As Double = 0
                                                    Dim discountReclaimedVatAmount As Double = 0
                                                    Dim discountReclaimedVatCode = 0
                                                    Dim atocCancellationChargeTotalAmount As Double = 0
                                                    Dim atocCancellationChargeVatAmount As Double = 0
                                                    Dim atocCancellationChargeVatCode = 0
                                                    Dim agencyCancellationChargeTotalAmount As Double = 0
                                                    Dim agencyCancellationChargeVatAmount As Double = 0
                                                    Dim agencyCancellationChargeVatCode = 0
                                                    Dim exgratiaPaymentTotalAmount As Double = 0
                                                    Dim exgratiaPaymentVatAmount As Double = 0
                                                    Dim exgratiaPaymentVatCode = 0

                                                    For Each at As XmlAttribute In _
                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).Attributes
                                                        If at.Name = "Ref" Then
                                                            ticketRef = notInteger(at.Value)
                                                        ElseIf at.Name = "PassengerRef" Then
                                                            ticketPassengerRef = notInteger(at.Value)
                                                        ElseIf at.Name = "TCN" Then
                                                            ticketTcn = notInteger(at.Value)
                                                        ElseIf at.Name = "AdultChild" Then
                                                            ticketAdultChild = at.Value
                                                        ElseIf at.Name = "Code" Then
                                                            ticketCode = at.Value
                                                        ElseIf at.Name = "Name" Then
                                                            ticketName = at.Value
                                                        ElseIf at.Name = "Class" Then
                                                            ticketClass = at.Value
                                                        ElseIf at.Name = "SingleReturn" Then
                                                            ticketSingleReturn = at.Value
                                                        ElseIf at.Name = "RouteCode" Then
                                                            ticketRouteCode = at.Value
                                                        ElseIf at.Name = "Route" Then
                                                            ticketRoute = at.Value
                                                        ElseIf at.Name = "Distance" Then
                                                            ticketDistance = notInteger(at.Value)
                                                        End If
                                                    Next
                                                    If oNodeToProcess.ChildNodes(iI).ChildNodes(iII).HasChildNodes Then
                                                        For iIII = 0 To _
                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes.
                                                                Count - 1
                                                            If _
                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(
                                                                    iIII).Name = "Sale" Then
                                                                If _
                                                                    oNodeToProcess.ChildNodes(iI).ChildNodes(iII).
                                                                        ChildNodes(iIII).HasChildNodes Then
                                                                    For iIIII = 0 To _
                                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).
                                                                            ChildNodes(iIII).ChildNodes.Count - 1
                                                                        If _
                                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII) _
                                                                                .ChildNodes(iIII).ChildNodes(iIIII).Name =
                                                                            "Fare" Then
                                                                            For Each at As XmlAttribute In _
                                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(
                                                                                    iII).ChildNodes(iIII).ChildNodes(
                                                                                        iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    fareTotalAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    fareVatAmount = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    fareVatCode = notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf _
                                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII) _
                                                                                .ChildNodes(iIII).ChildNodes(iIIII).Name =
                                                                            "Discount" Then
                                                                            For Each at As XmlAttribute In _
                                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(
                                                                                    iII).ChildNodes(iIII).ChildNodes(
                                                                                        iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    discountTotalAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    discountVatAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    discountVatCode =
                                                                                        notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf _
                                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII) _
                                                                                .ChildNodes(iIII).ChildNodes(iIIII).Name =
                                                                            "TransactionCharge" Then
                                                                            For Each at As XmlAttribute In _
                                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(
                                                                                    iII).ChildNodes(iIII).ChildNodes(
                                                                                        iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    transactionChargeTotalAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    transactionChargeVatAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    transactionChargeVatCode =
                                                                                        notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf _
                                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII) _
                                                                                .ChildNodes(iIII).ChildNodes(iIIII).Name =
                                                                            "FulfilmentFee" Then
                                                                            For Each at As XmlAttribute In _
                                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(
                                                                                    iII).ChildNodes(iIII).ChildNodes(
                                                                                        iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    fulfilmentFeeTotalAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    fulfilmentFeeVatAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    fulfilmentFeeVatCode =
                                                                                        notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf _
                                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII) _
                                                                                .ChildNodes(iIII).ChildNodes(iIIII).Name =
                                                                            "CreditCardCharge" Then
                                                                            For Each at As XmlAttribute In _
                                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(
                                                                                    iII).ChildNodes(iIII).ChildNodes(
                                                                                        iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    creditCardChargeTotalAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    creditCardChargeVatAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    creditCardChargeVatCode =
                                                                                        notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        End If
                                                                    Next
                                                                End If
                                                            ElseIf _
                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(
                                                                    iIII).Name = "Railcard" Then
                                                                For Each at As XmlAttribute In _
                                                                    oNodeToProcess.ChildNodes(iI).ChildNodes(iII).
                                                                        ChildNodes(iIII).Attributes
                                                                    If at.Name = "Code" Then
                                                                        railcardCode = at.Value
                                                                    ElseIf at.Name = "Name" Then
                                                                        railcardName = at.Value
                                                                    End If
                                                                Next
                                                            ElseIf _
                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(
                                                                    iIII).Name = "Refund" Then
                                                                If _
                                                                    oNodeToProcess.ChildNodes(iI).ChildNodes(iII).
                                                                        ChildNodes(iIII).HasChildNodes Then
                                                                    For iIIII = 0 To _
                                                                        oNodeToProcess.ChildNodes(iI).ChildNodes(iII).
                                                                            ChildNodes(iIII).ChildNodes.Count - 1
                                                                        If _
                                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII) _
                                                                                .ChildNodes(iIII).ChildNodes(iIIII).Name =
                                                                            "GrossRefund" Then
                                                                            For Each at As XmlAttribute In _
                                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(
                                                                                    iII).ChildNodes(iIII).ChildNodes(
                                                                                        iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    grossRefundTotalAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    grossRefundVatAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    grossRefundVatCode =
                                                                                        notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf _
                                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII) _
                                                                                .ChildNodes(iIII).ChildNodes(iIIII).Name =
                                                                            "DiscountReclaimed" Then
                                                                            For Each at As XmlAttribute In _
                                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(
                                                                                    iII).ChildNodes(iIII).ChildNodes(
                                                                                        iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    discountReclaimedTotalAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    discountReclaimedVatAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    discountReclaimedVatCode =
                                                                                        notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf _
                                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII) _
                                                                                .ChildNodes(iIII).ChildNodes(iIIII).Name =
                                                                            "ATOCCancellationCharge" Then
                                                                            For Each at As XmlAttribute In _
                                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(
                                                                                    iII).ChildNodes(iIII).ChildNodes(
                                                                                        iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    atocCancellationChargeTotalAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    atocCancellationChargeVatAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    atocCancellationChargeVatCode =
                                                                                        notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf _
                                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII) _
                                                                                .ChildNodes(iIII).ChildNodes(iIIII).Name =
                                                                            "AgencyCancellationCharge" Then
                                                                            For Each at As XmlAttribute In _
                                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(
                                                                                    iII).ChildNodes(iIII).ChildNodes(
                                                                                        iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    agencyCancellationChargeTotalAmount _
                                                                                        = notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    agencyCancellationChargeVatAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    agencyCancellationChargeVatCode =
                                                                                        notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        ElseIf _
                                                                            oNodeToProcess.ChildNodes(iI).ChildNodes(iII) _
                                                                                .ChildNodes(iIII).ChildNodes(iIIII).Name =
                                                                            "ExgratiaPayment" Then
                                                                            For Each at As XmlAttribute In _
                                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(
                                                                                    iII).ChildNodes(iIII).ChildNodes(
                                                                                        iIIII).Attributes
                                                                                If at.Name = "TotalAmount" Then
                                                                                    exgratiaPaymentTotalAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATAmount" Then
                                                                                    exgratiaPaymentVatAmount =
                                                                                        notNumber(at.Value)
                                                                                ElseIf at.Name = "VATCode" Then
                                                                                    exgratiaPaymentVatCode =
                                                                                        notInteger(at.Value)
                                                                                End If
                                                                            Next
                                                                        End If
                                                                    Next
                                                                End If
                                                            ElseIf _
                                                                oNodeToProcess.ChildNodes(iI).ChildNodes(iII).ChildNodes(
                                                                    iIII).Name = "FareException" Then
                                                                For Each at As XmlAttribute In _
                                                                    oNodeToProcess.ChildNodes(iI).ChildNodes(iII).
                                                                        ChildNodes(iIII).Attributes
                                                                    If at.Name = "NormalFare" Then
                                                                        normalFare = notNumber(at.Value)
                                                                    ElseIf at.Name = "OfferedFare" Then
                                                                        offeredFare = notNumber(at.Value)
                                                                    End If
                                                                Next
                                                            End If
                                                        Next
                                                    End If
                                                    clsEvolvi.saveTicket(segmentRefTicket, intEvolviId, ticketRef,
                                                                         ticketPassengerRef,
                                                                         ticketTcn, ticketAdultChild, ticketCode,
                                                                         ticketName,
                                                                         ticketClass, ticketSingleReturn,
                                                                         ticketRouteCode,
                                                                         ticketRoute, ticketDistance, fareTotalAmount,
                                                                         fareVatAmount, fareVatCode, discountTotalAmount,
                                                                         discountVatAmount, discountVatCode,
                                                                         transactionChargeTotalAmount,
                                                                         transactionChargeVatAmount,
                                                                         transactionChargeVatCode,
                                                                         fulfilmentFeeTotalAmount,
                                                                         fulfilmentFeeVatAmount, fulfilmentFeeVatCode,
                                                                         creditCardChargeTotalAmount,
                                                                         creditCardChargeVatAmount,
                                                                         creditCardChargeVatCode, normalFare,
                                                                         offeredFare, railcardCode, railcardName,
                                                                         grossRefundTotalAmount,
                                                                         grossRefundVatAmount, grossRefundVatCode,
                                                                         discountReclaimedTotalAmount,
                                                                         discountReclaimedVatAmount,
                                                                         discountReclaimedVatCode,
                                                                         atocCancellationChargeTotalAmount,
                                                                         atocCancellationChargeVatAmount,
                                                                         atocCancellationChargeVatCode,
                                                                         agencyCancellationChargeTotalAmount,
                                                                         agencyCancellationChargeVatAmount,
                                                                         agencyCancellationChargeVatCode,
                                                                         exgratiaPaymentTotalAmount,
                                                                         exgratiaPaymentVatAmount,
                                                                         exgratiaPaymentVatCode)
                                                End If
                                            Next
                                        End If
                                    End If
                                Next
                            End If
                        End If
                        'move the file so it doesn't get read again
                        If File.Exists(arrFiles(i)) Then
                            Dim strFileName As String = Path.GetFileName(arrFiles(i))
                            File.Move(arrFiles(i), strEvolviFilesLocation & strDay & "-PASS\" & strFileName)
                        End If
                    Catch ex As Exception
                        If File.Exists(arrFiles(i)) Then
                            Dim strFileName As String = Path.GetFileName(arrFiles(i))
                            File.Move(arrFiles(i), strEvolviFilesLocation & strDay & "-FAIL\" & strFileName)
                        End If
                    End Try
                Catch ex As Exception
                    If File.Exists(arrFiles(i)) Then
                        Dim strFileName As String = Path.GetFileName(arrFiles(i))
                        File.Move(arrFiles(i), strEvolviFilesLocation & strDay & "-FAIL\" & strFileName)
                    End If
                End Try
            Next
        Catch ex As Exception
            Dim strEvolviFilesLocation As String = ConfigUtils.getConfig("EvolviHandoffs") _
            ' \\nysmgmt\EvolviHandoffs\intercept\SQL\
            Dim fileCount As Integer = Directory.GetFiles(strEvolviFilesLocation).Length _
            '  \\nysmgmt\EvolviHandoffs\intercept\SQL\
            Log.Error("Evolvi file load error:" & ex.Message)
            Log.Error(
                "Regards Above - File count=" + fileCount.ToString() + " likely no xml files in directory " +
                strEvolviFilesLocation + " at this specific moment")
        End Try
    End Sub

    Public Sub MoveFile(filefrom As String, fileto As String)
        Using New log4netUtils.MethodLogger(Log, "Move File - from " & filefrom & " to " & fileto)

            Try

                If File.Exists(fileto) Then

                End If
                If File.Exists(fileto) Then
                    File.Copy(filefrom, fileto, True)
                    File.Delete(filefrom)
                Else
                    File.Move(filefrom, fileto)
                End If
            Catch ex As Exception
                'R2.21 SA - log per comany name not just FERA
                Log.Error("Move File- " & filefrom & ": " & fileto)

            Finally 'close file logs

            End Try
        End Using
    End Sub

    Public Sub AutoInvoicesProcessFolderAwaiting()
        Try

            Dim strDatePath As String = "_" & CStr(Date.Now.Year) &
                                        IIf(Date.Now.Month < 10, "0" & CStr(Date.Now.Month), CStr(Date.Now.Month))
            Dim strSentFolder As String = ConfigUtils.getConfig("InvoiceSent").Replace("###", strDatePath)
            Dim strNotSentFolder As String = ConfigUtils.getConfig("InvoiceNotSent").Replace("###", strDatePath)
            Dim strErrorFolder As String = ConfigUtils.getConfig("InvoiceError").Replace("###", strDatePath)
            Dim strAwaitingFolder As String = ConfigUtils.getConfig("InvoiceAwaiting")

            Dim diAwaitingFolder As New DirectoryInfo(strAwaitingFolder)
            Dim diInputFolder1 As New DirectoryInfo(strSentFolder)
            Dim diInputFolder2 As New DirectoryInfo(strNotSentFolder)
            Dim diInputFolder3 As New DirectoryInfo(strErrorFolder)

            If Not diAwaitingFolder.Exists Then
                diAwaitingFolder.Create()
            End If

            'R2.21 SA - invoices for YDH as well as FERA
            Dim strCompanyName = ""

            If diAwaitingFolder.Exists Then
                Dim count = 0
                'loop through all files in folder
                For Each aFile As FileSystemInfo In diAwaitingFolder.GetFiles
                    Dim strFileName As String = aFile.Name
                    Dim oInvoices As New List(Of clsAutoInvoice)
                    Dim oEvolviEmails As New List(Of clsAutoInvoice)
                    Dim oAirEmails As New List(Of clsAutoInvoice)

                    'R?? SA file name no longer contains M - after boss update 29/10/2013
                    'If strFileName.ToLower.Contains("m.pdf") Then
                    If strFileName.ToLower.Contains(".pdf") Then

                        'oInvoices = clsAutoInvoice.checkInvoice(strFileName.ToLower.Replace("m.pdf", ""))
                        oInvoices = clsAutoInvoice.checkInvoice(strFileName.ToLower.Replace(".pdf", ""))

                        If oInvoices.Count > 0 Then 'should actually only be one!
                            If File.Exists(strSentFolder & strFileName) Then _
'file has already been sent so don't bother again
                                File.Copy(strAwaitingFolder & strFileName, strSentFolder & strFileName, True)
                                File.Delete(strAwaitingFolder & strFileName)
                            Else
                                For Each oInvoice As clsAutoInvoice In oInvoices
                                    Try
                                        'R2.21 SA
                                        strCompanyName = oInvoice.Client.ToUpper.Trim

                                        If strCompanyName = "FERA" Or strCompanyName = "YDH R" Or strCompanyName = "YDH" _
                                            Then
                                            Dim strFirstName = ""
                                            Dim strLastName = ""
                                            Dim strEmailTo = ""

                                            'R2.21 SA - only check Po for FERA
                                            If oInvoice.PONumber.Trim <> "" AndAlso strCompanyName = "FERA" Then
                                                Try
                                                    If IsNumeric(oInvoice.PONumber.Trim.ToLower.Replace("p", "")) Then
                                                        'all good so send to proc email
                                                        strEmailTo = ConfigUtils.getConfig("FeraProcurementEmail")
                                                    End If
                                                    'Dim inttest As Integer = CInt(oFera.PONumber.Trim.ToLower.Replace("p", ""))
                                                Catch ex As Exception
                                                    'can't do it
                                                End Try
                                            Else
                                                'R2.21 SA - check locator is not empty
                                                If oInvoice.RecordLocator.Trim <> "" Then
                                                    If IsNumeric(oInvoice.RecordLocator.Trim) Then 'RAIL
                                                        strEmailTo = CheckEvolviEmail(oInvoice.RecordLocator.Trim)
                                                    Else
                                                        'CHECK AIR
                                                        strEmailTo = CheckAirEmail(oInvoice.RecordLocator.Trim)
                                                        'CHECK HOTEL
                                                        If strEmailTo = "" Then
                                                            strEmailTo = CheckConfermaEmail(oInvoice.RecordLocator.Trim)
                                                        End If
                                                    End If
                                                ElseIf _
                                                    oInvoice.RecordLocator.Trim = "" AndAlso strCompanyName = "" AndAlso
                                                    oInvoice.Supplier = "" Then
                                                    strEmailTo = ConfigUtils.getConfig("YDHRPostChargeEmail")
                                                Else
                                                    strEmailTo = ""
                                                End If
                                            End If

                                            'make sure email is in the correct format
                                            Dim mRegExp As New Regex(SendEmailUtils.emailRegex)
                                            If Not mRegExp.IsMatch(strEmailTo) Then
                                                strEmailTo = "" ' getConfig("FeraProcurementEmail")
                                            End If

                                            If strEmailTo <> "" Then
                                                Dim blnSent As Boolean = SendAutoInvoiceEmail(strEmailTo,
                                                                                              ConfigUtils.getConfig(
                                                                                                  "AutoInvoiceEmailFrom"),
                                                                                              strAwaitingFolder &
                                                                                              strFileName, strFirstName,
                                                                                              False)
                                                If blnSent Then
                                                    'move to sent folder
                                                    MoveFile(strAwaitingFolder & strFileName,
                                                             strSentFolder & strFileName)
                                                Else
                                                    'R2.21 SA - log per company name not just FERA
                                                    Log.Error(
                                                        "Failure sending email - " & strCompanyName & ": " & strFileName)
                                                    SendAutoInvoiceEmail("", "", strFileName, "", True)
                                                    'move to error folder
                                                    MoveFile(strAwaitingFolder & strFileName,
                                                             strErrorFolder & strFileName)
                                                End If
                                            Else
                                                'R2.21 SA - log per company name not just FERA
                                                Log.Error("No email - " & strCompanyName & ": " & strFileName)
                                                SendAutoInvoiceEmail("", "", strFileName, "", True)
                                                'move to error folder
                                                MoveFile(strAwaitingFolder & strFileName, strErrorFolder & strFileName)
                                            End If

                                        Else
                                            'definately not Fera so move
                                            MoveFile(strAwaitingFolder & strFileName, strNotSentFolder & strFileName)
                                        End If
                                    Catch ex As Exception
                                        'R2.21 SA - log per comany name not just FERA
                                        Log.Error("Failure retrieving email - " & strCompanyName & ": " & strFileName)

                                        SendAutoInvoiceEmail("", "", strFileName, "", True)
                                        'move to error folder
                                        MoveFile(strAwaitingFolder & strFileName, strErrorFolder & strFileName)
                                    Finally 'close file logs

                                    End Try
                                Next
                            End If
                        Else
                            'just leave in directory to be checked again later
                        End If
                    Else
                        'definately not Fera so move
                        MoveFile(strAwaitingFolder & strFileName, strNotSentFolder & strFileName)
                    End If
                Next
            Else
                Log.Error("Fera/YDH/YDH R - One of the specified folders does not exist")
            End If
        Catch ex As Exception
            Log.Error("Read Failure - Fera/YDH/YDH R: " & ex.Message)
        End Try
    End Sub

    'R2.21 SA
    Public Sub AutoInvoiceProcessFolder()

        Log.Info("AUTO Invoice Process Folder - Starting")
        Try

            Dim strDatePath As String = "_" & CStr(Date.Now.Year) &
                                        IIf(Date.Now.Month < 10, "0" & CStr(Date.Now.Month), CStr(Date.Now.Month))
            Dim strReadFromFolder As String = ConfigUtils.getConfig("InvoiceInput")
            Dim strSentFolder As String = ConfigUtils.getConfig("InvoiceSent").Replace("###", strDatePath)
            Dim strNotSentFolder As String = ConfigUtils.getConfig("InvoiceNotSent").Replace("###", strDatePath)
            Dim strErrorFolder As String = ConfigUtils.getConfig("InvoiceError").Replace("###", strDatePath)
            Dim strAwaitingFolder As String = ConfigUtils.getConfig("InvoiceAwaiting")

            Dim diInputFolder As New DirectoryInfo(strReadFromFolder)
            Dim diInputFolder1 As New DirectoryInfo(strSentFolder)
            Dim diInputFolder2 As New DirectoryInfo(strNotSentFolder)
            Dim diInputFolder3 As New DirectoryInfo(strErrorFolder)
            Dim diInputFolder4 As New DirectoryInfo(strAwaitingFolder)

            If Not diInputFolder.Exists Then
                diInputFolder.Create()
            End If
            If Not diInputFolder1.Exists Then
                diInputFolder1.Create()
            End If
            If Not diInputFolder2.Exists Then
                diInputFolder2.Create()
            End If
            If Not diInputFolder3.Exists Then
                diInputFolder3.Create()
            End If
            If Not diInputFolder4.Exists Then
                diInputFolder4.Create()
            End If

            Dim strCompanyName = ""

            If _
                diInputFolder.Exists And diInputFolder1.Exists And diInputFolder2.Exists And diInputFolder3.Exists And
                diInputFolder4.Exists Then
                Dim count = 0
                'loop through all files in folder

                'MK
                'Appears to be locking the files?
                'But this shouldn't be
                'Im going to loop through it first and save the results into a new list and run from that list as it will have finished with the file by then

                Dim fileList As New List(Of FileSystemInfo)

                For Each aFile As FileSystemInfo In diInputFolder.GetFiles
                    fileList.Add(aFile)
                Next

                For Each aFile As FileSystemInfo In fileList

                    Log.Info("Pickup up file " & aFile.Name)

                    Dim strFileName As String = aFile.Name
                    Dim oInvoices As New List(Of clsAutoInvoice)
                    'Dim oEvolviEmails As New List(Of clsAutoInvoice)
                    Dim oAirEmails As New List(Of clsAutoInvoice)
                    'R2.21
                    Dim oConfermaEmails As New List(Of clsAutoInvoice)

                    'R?? SA file name dosen't contain M any more- after boss update on 29/10/2013
                    ' If strFileName.ToLower.Contains("m.pdf") Then
                    If strFileName.ToLower.Contains(".pdf") Then

                        'TODO: update SQL with BOSS values before running - make sure we have the SQL data!
                        'will need the server to have the foxpro driver installed

                        'R?? SA file name dosen't contain M any more- after boss update on 29/10/2013
                        'oInvoices = clsAutoInvoice.checkInvoice(strFileName.ToLower.Replace("m.pdf", ""))
                        oInvoices = clsAutoInvoice.checkInvoice(strFileName.ToLower.Replace(".pdf", ""))

                        If oInvoices.Count > 0 Then 'should actually only be one!
                            If File.Exists(strSentFolder & strFileName) Then _
'file has already been sent so don't bother again

                                Log.Info(aFile.Name & " File has already been sent Moving file")
                                Try
                                    File.Copy(strReadFromFolder & strFileName, strSentFolder & strFileName, True)
                                    File.Delete(strReadFromFolder & strFileName)
                                Catch ex As Exception
                                    Log.Info(aFile.Name & " Copy and delete failed")
                                End Try
                            Else
                                For Each oInvoice As clsAutoInvoice In oInvoices
                                    Try
                                        'R2.21 SA
                                        strCompanyName = oInvoice.Client.ToUpper.Trim

                                        If strCompanyName = "FERA" Or strCompanyName = "YDH R" Or strCompanyName = "YDH" _
                                            Then
                                            Dim strFirstName = ""
                                            Dim strLastName = ""
                                            Dim strEmailTo = ""

                                            If _
                                                strCompanyName = "FERA" AndAlso oInvoice.PONumber.Trim <> "" AndAlso
                                                IsNumeric(oInvoice.PONumber.Trim.ToLower.Replace("p", "")) Then
                                                Try
                                                    'Dim inttest As Integer = CInt(oInvoice.PONumber.Trim.ToLower.Replace("p", ""))
                                                    'Dim strtest As String = oInvoice.PONumber.Trim.ToLower.Replace("p", "")
                                                    ' If IsNumeric(strtest) Then
                                                    'all good so send to proc email
                                                    strEmailTo = ConfigUtils.getConfig("FeraProcurementEmail")
                                                    'End If

                                                Catch ex As Exception
                                                    'rail
                                                    strEmailTo = CheckEvolviEmail(oInvoice.RecordLocator.Trim)
                                                    'Air
                                                    If strEmailTo = "" Then
                                                        strEmailTo = CheckAirEmail(oInvoice.RecordLocator.Trim)
                                                    End If

                                                End Try
                                            Else
                                                If oInvoice.RecordLocator.Trim <> "" Then

                                                    'R2.21 SA
                                                    If IsNumeric(oInvoice.RecordLocator.Trim) Then 'RAIL
                                                        strEmailTo = CheckEvolviEmail(oInvoice.RecordLocator.Trim)
                                                    Else

                                                        Log.Info("Checking with EF 2")
                                                        Try
                                                            Using database = New NYSDBEntities

                                                                Dim tempInvoiceNumber =
                                                                        strFileName.ToLower.Replace(".pdf", "")

                                                                Dim cref2 As String =
                                                                        (From x In database.BOSSinvtotEF
                                                                        Where x.tot_invno = tempInvoiceNumber
                                                                        Select x.tot_cref2).FirstOrDefault
                                                                Log.Info("Cref2 = " & cref2)
                                                                If cref2 <> "" Then
                                                                    'We Have a name lets split it up
                                                                    Dim forename = CollectionUtils.split(cref2, " ")(0)
                                                                    Dim surname = CollectionUtils.split(cref2, " ")(1)

                                                                    Dim tempEvolviData =
                                                                            (From x In database.EvolviDataEF
                                                                            Where _
                                                                            x.DeliveryContactFirstName = forename And
                                                                            x.DeliveryContactLastName = surname And
                                                                            x.DeliveryContactOrganisation = "Fera"
                                                                            Select x.DeliveryContactEmail).
                                                                            FirstOrDefault

                                                                    If tempEvolviData <> "" Then
                                                                        strEmailTo = tempEvolviData
                                                                    End If

                                                                End If

                                                            End Using
                                                        Catch ex As Exception
                                                            Log.Info("Failed on EF - " & ex.Message)
                                                        End Try

                                                        If strEmailTo = "" Then
                                                            'CHECK AIR
                                                            strEmailTo = CheckAirEmail(oInvoice.RecordLocator.Trim)
                                                        End If

                                                        'See if we can use the booker name to get a email address from Evolvi Database
                                                        'CHECK HOTEL
                                                        If strEmailTo = "" Then
                                                            strEmailTo = CheckConfermaEmail(oInvoice.RecordLocator.Trim)
                                                        End If
                                                    End If
                                                ElseIf _
                                                    oInvoice.RecordLocator.Trim = "" AndAlso strCompanyName = "YDH R" AndAlso
                                                    oInvoice.Supplier = "GENPOST" Then
                                                    strEmailTo = ConfigUtils.getConfig("YDHRPostChargeEmail")
                                                Else

                                                    Using database = New NYSDBEntities

                                                        Dim tempInvoiceNumber = strFileName.ToLower.Replace(".pdf", "")

                                                        Dim cref2 As String =
                                                                (From x In database.BOSSinvtotEF
                                                                Where x.tot_invno = tempInvoiceNumber Select x.tot_cref2) _
                                                                .FirstOrDefault
                                                        Log.Info("Cref2 = " & cref2)
                                                        If cref2 <> "" Then
                                                            'We Have a name lets split it up
                                                            Dim forename = CollectionUtils.split(cref2, " ")(0)
                                                            Dim surname = CollectionUtils.split(cref2, " ")(1)

                                                            Dim tempEvolviData =
                                                                    (From x In database.EvolviDataEF
                                                                    Where _
                                                                    x.DeliveryContactFirstName = forename And
                                                                    x.DeliveryContactLastName = surname And
                                                                    x.DeliveryContactOrganisation =
                                                                    "York District Hospital"
                                                                    Select x.DeliveryContactEmail).FirstOrDefault

                                                            If tempEvolviData <> "" Then
                                                                strEmailTo = tempEvolviData
                                                            End If

                                                        End If

                                                    End Using

                                                End If
                                            End If

                                            Log.Info(aFile.Name & " will be sent to " & strEmailTo)
                                            'make sure email is in the correct format
                                            Dim mRegExp As New Regex(SendEmailUtils.emailRegex)
                                            If Not mRegExp.IsMatch(strEmailTo) Then
                                                strEmailTo = "" ' getConfig("FeraProcurementEmail")
                                            End If

                                            If strEmailTo <> "" Then

                                                Dim blnSent As Boolean = SendAutoInvoiceEmail(strEmailTo,
                                                                                              ConfigUtils.getConfig(
                                                                                                  "AutoInvoiceEmailFrom"),
                                                                                              strReadFromFolder &
                                                                                              strFileName, strFirstName,
                                                                                              False)
                                                'Dim blnSent As Boolean = sendAutoInvoiceEmail(strEmailTo, getConfig("AutoInvoiceEmailFrom"), "\\nys-boss\BOSS\32bitboss\PDFfiles\CopyToSend\" & strFileName, strFirstName, False)
                                                'Dim blnSent As Boolean = sendAutoInvoiceEmail("mike.kirk@nysgroup.com", getConfig("AutoInvoiceEmailFrom"), "\\nys-boss\BOSS\32bitboss\PDFfiles\CopyToSend\" & strFileName, strFirstName, False)

                                                If blnSent Then
                                                    Log.Info(aFile.Name & " has been sent correctly moving files")
                                                    'move to sent folder
                                                    Try
                                                        If File.Exists(strSentFolder & strFileName) Then
                                                            File.Copy(strReadFromFolder & strFileName,
                                                                      strSentFolder & strFileName, True)
                                                            File.Delete(strReadFromFolder & strFileName)
                                                        Else
                                                            Log.Info("Attempting to move the file " & aFile.Name)
                                                            File.Move(strReadFromFolder & strFileName,
                                                                      strSentFolder & strFileName)
                                                            'IO.File.Delete(strReadFromFolder & strFileName)
                                                            Log.Info("Success")
                                                        End If
                                                    Catch ex As Exception
                                                        Log.Info(aFile.Name & " Failed to move files - " & ex.Message)
                                                    End Try
                                                Else
                                                    'R2.21 SA - log per company name
                                                    Log.Error(
                                                        "Send Email failed - " & strCompanyName & ": " & strFileName)
                                                    SendAutoInvoiceEmail("", "", strFileName, "", True)
                                                    'move to error folder
                                                    If File.Exists(strErrorFolder & strFileName) Then
                                                        File.Copy(strReadFromFolder & strFileName,
                                                                  strErrorFolder & strFileName, True)
                                                        File.Delete(strReadFromFolder & strFileName)
                                                    Else
                                                        File.Move(strReadFromFolder & strFileName,
                                                                  strErrorFolder & strFileName)
                                                        ' IO.File.Delete(strReadFromFolder & strFileName)
                                                    End If
                                                End If
                                            Else
                                                'R2.21 SA - log per company name
                                                Log.Error("No email - " & strCompanyName & ": " & strFileName)
                                                SendAutoInvoiceEmail("", "", strFileName, "", True)
                                                'move to error folder
                                                If File.Exists(strErrorFolder & strFileName) Then
                                                    File.Copy(strReadFromFolder & strFileName,
                                                              strErrorFolder & strFileName, True)
                                                    File.Delete(strReadFromFolder & strFileName)
                                                Else
                                                    File.Move(strReadFromFolder & strFileName,
                                                              strErrorFolder & strFileName)
                                                    'IO.File.Delete(strReadFromFolder & strFileName)
                                                End If
                                            End If
                                        Else
                                            'R2.16 CR - only do this late at night!! just in case the file is NOT FERA and is being appended to by BOSS
                                            If _
                                                DateTime.Now.Hour >=
                                                notInteger(ConfigUtils.getConfig("StartMovePDFFilesHour")) Then
                                                'definately not Fera so move
                                                If File.Exists(strNotSentFolder & strFileName) Then
                                                    File.Copy(strReadFromFolder & strFileName,
                                                              strNotSentFolder & strFileName, True)
                                                    File.Delete(strReadFromFolder & strFileName)
                                                Else
                                                    File.Move(strReadFromFolder & strFileName,
                                                              strNotSentFolder & strFileName)

                                                    ' IO.File.Delete(strReadFromFolder & strFileName)
                                                End If
                                            End If
                                        End If

                                    Catch ex As Exception
                                        'R2.21 SA - log for company name instead of just for FERA
                                        Log.Error(
                                            "Failure retrieving email - " & strCompanyName & ": " & strFileName &
                                            " **Ex.Message=" & ex.Message)
                                        SendAutoInvoiceEmail("", "", strFileName, "", True)
                                        'move to error folder
                                        If File.Exists(strErrorFolder & strFileName) Then
                                            File.Copy(strReadFromFolder & strFileName, strErrorFolder & strFileName,
                                                      True)
                                            File.Delete(strReadFromFolder & strFileName)
                                        Else
                                            File.Move(strReadFromFolder & strFileName, strErrorFolder & strFileName)
                                            ' IO.File.Delete(strReadFromFolder & strFileName)
                                        End If
                                    End Try
                                Next
                            End If
                        Else
                            'could still be Fera / YDH , but record not yet transfered to SQL from BOSS
                            'need to move to diff folder so can poll that at a later date

                            'R2.16 CR - only do this late at night!! just in case the file is NOT FERA and is being appended to by BOSS
                            If DateTime.Now.Hour >= notInteger(ConfigUtils.getConfig("StartMovePDFFilesHour")) Then
                                If File.Exists(strAwaitingFolder & strFileName) Then
                                    File.Delete(strAwaitingFolder & strFileName)
                                    File.Move(strReadFromFolder & strFileName, strAwaitingFolder & strFileName)
                                    'IO.File.Delete(strReadFromFolder & strFileName)
                                Else
                                    File.Move(strReadFromFolder & strFileName, strAwaitingFolder & strFileName)
                                    'IO.File.Delete(strReadFromFolder & strFileName)
                                End If
                            End If
                        End If
                    Else
                        'R2.16 CR - only do this late at night!! just in case the file is NOT FERA and is being appended to by BOSS
                        If DateTime.Now.Hour >= notInteger(ConfigUtils.getConfig("StartMovePDFFilesHour")) Then
                            'definately not Fera so move
                            If File.Exists(strNotSentFolder & strFileName) Then
                                File.Copy(strReadFromFolder & strFileName, strNotSentFolder & strFileName, True)
                                File.Delete(strReadFromFolder & strFileName)
                            Else
                                File.Move(strReadFromFolder & strFileName, strNotSentFolder & strFileName)
                                'IO.File.Delete(strReadFromFolder & strFileName)
                            End If
                        End If

                    End If

                Next
            Else
                Log.Error("Fera/YDH/YDH R - One of the specified folders does not exist")
            End If
        Catch ex As Exception
            Log.Error("Read Failure - Fera/YDH/YDH R: " & ex.Message)
            Log.Error("AUTO Invoice Process Folder - exception thrown: " & ex.Message)
        End Try

        Log.Info("AUTO Invoice Process Folder - Ending")
    End Sub

    Private Function SendAutoInvoiceEmail(pEmailTo As String,
                                          pEmailFrom As String,
                                          pFile As String,
                                          pFirstName As String,
                                          pError As Boolean) As Boolean

        If Not pError Then
            Dim _
                ofile As _
                    New StreamReader(ConfigUtils.getConfig("HomeAbsolutepath") & "userdocs\NYS\AutoInvoiceEmail.htm")

            Dim strreadtest As String = ofile.ReadToEnd & "<p>"
            ofile.Close()

            If pFirstName <> "" Then
                strreadtest = strreadtest.Replace("#firstname#", pFirstName)
            Else
                strreadtest = strreadtest.Replace("Dear #firstname#&nbsp;<br />", "")
            End If

            Dim strMessage As String = strreadtest

            Dim strfrom As String = pEmailFrom
            Dim strto As String = pEmailTo
            If strfrom = "" Then
                strfrom = "rail@nysgroup.com"
            End If

            If ConfigUtils.getConfig("ReleaseEmailTest") = "true" Then
                strto = ConfigUtils.getConfig("ReleaseEmailTestSend")
                strMessage = "Email would have been sent to: " & pEmailTo & " - " & strMessage
            End If

            Try
                Log.Info("Attempting to send email")
                SendEmail.send(strfrom, strto, "PDF Invoice from NYS Corporate", strMessage, "conference3@nysgroup.com",
                               "", "", pFile)
                Log.Info("Email Sent")
                Return True
            Catch ex As Exception
                Log.Error("ERROR IN SENDAUTOINVOICEEMAIL: " & ex.Message)
                Return False
            End Try
        Else
            Try
                Dim strProduct As String = clsAutoInvoice.GetInvoiceType(pFile.Replace(".pdf", ""))
                If strProduct = "Rail" Then
                    SendEmail.send("RailBookings@nysgroup.com", "mike.kirk@nysgroup.com",
                                   "FERA/YDH " & strProduct & " PDF Invoice could not be sent to the booker",
                                   "Invoice: " & pFile, "", "", "", "")
                Else
                    SendEmail.send("RailBookings@nysgroup.com", "mike.kirk@nysgroup.com",
                                   "FERA/YDH " & strProduct & " PDF Invoice could not be sent to the booker",
                                   "Invoice: " & pFile, "", "", "", "")
                End If

                Return True
            Catch ex As Exception
                Log.Error("ERROR IN SENDAUTOINVOICEEMAIL: " & ex.Message)
                Return False
            End Try
        End If
    End Function

    'R2.21 SA
    Private Function CheckEvolviEmail(pLocator As String) As String
        Dim strFirstName = ""
        Dim strLastName = ""
        Dim strEmailTo = ""
        Dim oEvolviEmails As New List(Of clsAutoInvoice)

        oEvolviEmails = clsAutoInvoice.EvolviEmail(pLocator)
        If oEvolviEmails.Count > 0 Then
            For Each oEmail As clsAutoInvoice In oEvolviEmails
                strFirstName = oEmail.FirstName.Trim
                strLastName = oEmail.LastName.Trim
                strEmailTo = oEmail.EmailAddress.Trim
            Next
            If strEmailTo = "" Then
                'try fera/YDH  email table next
                strEmailTo = clsAutoInvoice.EmailCheck(strFirstName, strLastName)
            End If
        End If

        Return strEmailTo
    End Function

    'R2.21 SA
    Private Function CheckAirEmail(pLocator As String) As String
        Dim strFirstName = ""
        Dim strLastName = ""
        Dim strEmailTo = ""
        Dim oAirEmails As New List(Of clsAutoInvoice)

        oAirEmails = clsAutoInvoice.AirEmail(pLocator)
        If oAirEmails.Count > 0 Then
            For Each oEmail As clsAutoInvoice In oAirEmails
                Dim oSplit As Object
                If oEmail.FullName.Trim <> "" Then
                    oSplit = oEmail.FullName.Split(" ")
                    strFirstName = oSplit(0).ToString.Trim
                    strLastName = oSplit(1).ToString.Trim
                End If
            Next
            If strFirstName <> "" Or strLastName <> "" Then
                strEmailTo = clsAutoInvoice.EmailCheck(strFirstName, strLastName)
                'if stll nothing then try switching
                If strEmailTo = "" Then
                    strEmailTo = clsAutoInvoice.EmailCheck(strLastName, strFirstName)
                End If
            End If
        End If

        Return strEmailTo
    End Function

    'R2.21 SA
    Private Function CheckConfermaEmail(pLocator As String) As String

        Try
            Dim ret As String = NSConfigUtils.ConfermaEmail(pLocator)
            Log.Info("CheckConfermaEmail: " & ret & " for invoice " & pLocator)
            Return ret
        Catch
            Log.Error("CheckConfermaEmail: error for invoice " & pLocator)
        End Try

        'Return strEmailTo
    End Function

    'for test purposes to simplify mail service created by LC
    Public Sub EvolviInterceptorGuardian(pstrReadFromFolder As String, pstrWriteToFolder As String,
                                         pstrWriteToSqlFolder As String)
        Dim dblSchemaVersion As Double = 0
        Dim intNumberOfBookings = 0
        Dim strOrderRef = ""
        Dim strFileName = ""
        Dim fulfilmentType = ""
        Dim intRefunds = 0
        Dim blnInsideRefund = False
        Dim blnIsValidRefund = False
        Dim lstInvalidRefunds As New List(Of Integer)
        Dim intImmediateDetailCount = 0
        Dim strSplitNode = ""
        Dim strSplitFieldName = ""
        Dim strSplitFieldValue = ""
        Dim strSplitUniqueFieldName = ""
        Dim intTotalTickets As Integer
        Dim interceptorClient As Object
        Dim strBossCode = ""
        Dim blnAddOfflineFee = False
        Dim strMachineNumber = ""
        Dim dblSpecialDelivery As Double = 0
        Dim strCustomfieldCc = ""
        Dim strCustomfieldCref1 = ""
        Dim blnSkipDeliveryCharge = False
        Dim strCurrentFulfilmentRef = ""
        Dim blnChangeAgencyCancellation = False
        Dim dblRefundCancellationCharge As Double = 0
        Dim strCustomFieldPo = ""
        Dim strIssueRef = ""
        Dim strExternalRefValue = ""
        Dim blnIsDataError = False
        Dim blnLvTravellerProvided = False
        Dim blnLvProjectCode = False
        Dim strMckTravellerCostCentre = ""
        Dim strMckTravellerHrNumber = ""
        Dim blnMckOptionalCostCentre = False
        Dim strSystemCTravellerCostCentre = ""
        Dim strSystemCTravellerHrNumber = ""
        Dim blnSystemCOptionalCostCentre = False
        Dim strChiesiTravellerDepartmentCode = ""
        Dim strChiesiTravellerAreaCode = ""
        Dim strChiesiTravellerCostCode = ""
        Dim strPikselTravellerEmployeeNumber = ""
        Dim strPikselTravellerPo = ""
        Dim strPikselTravellerBussinessUnit = ""
        Dim strPikselTravellerDepartment = ""
        Dim strPikselTravellerLegalEntity = ""
        Dim strPikselTravellerLocation = ""
        Dim strPikselTravellerGlCode = ""
        Dim strKcomTravellerEmployeeNumber = ""
        Dim blnNonIssue = False
        Dim strIssueDate As String = Date.Now.ToString(ConfigUtils.getConfig("RailIssueDateFormat"))
        Dim intTotalPassengers = 0
        Dim blnFeeAdded = False
        Dim currentTraveller As New clsTravellerProfiles
        'Get A List Of All Custom Fields within the booking
        Dim customFields As IEnumerable(Of XElement)
        Dim strTravellerEmail = ""
        Dim strLvPassengerCustomFields As New StringBuilder
        Dim strChiesiPassengerCustomFields As New StringBuilder
        Dim intEmailExist = 0
        Dim travelerClient = ""
        Dim chiesiGlCodeAndDescription = ""
        Dim chiesiGlCode = ""
        Dim chiesiIoCode = ""
        Dim strChiesiApprovedByField = ""
        Dim strKcomPassengerCustomFields = ""
        Dim customCode1 = False
        Dim customCode2 = False
        Dim customCode3 = False
        Dim customCode4 = False
        Dim customCode5 = False
        Dim customCode6 = False
        Dim customCode7 = False
        Dim customCode8 = False
        Dim customCode9 = False
        Dim customCode10 = False
        Dim transactionType = ""

        Try

            Dim diInputFolder As New DirectoryInfo(pstrReadFromFolder)
            Dim diOutputFolder As New DirectoryInfo(pstrWriteToFolder)
            Dim diOutputSqlFolder As New DirectoryInfo(pstrWriteToSqlFolder)

            If diInputFolder.Exists And diOutputFolder.Exists And diOutputSqlFolder.Exists Then

                'loop through all the files in the folder
                For Each file As FileSystemInfo In diInputFolder.GetFiles
                    'open the file and read it's content

                    Dim guardianIOnputFile As XElement = XElement.Load(file.FullName)

                    strOrderRef = guardianIOnputFile.Attribute("OrderRef").Value

                    If guardianIOnputFile.Attribute("SchemaVersion") IsNot Nothing Then

                        dblSchemaVersion = notDouble(guardianIOnputFile.Attribute("SchemaVersion").Value)

                        'Change scheme version if higher than 4.0 although boss no longer requires this functionality????????????????????
                        If dblSchemaVersion >= 4.0 Then
                            guardianIOnputFile.SetAttributeValue("SchemaVersion", "3.9")
                        End If

                    End If

                    'Get The Client Boss Code
                    If guardianIOnputFile.Descendants("Account").Attributes("ExternalRef") IsNot Nothing Then

                        strBossCode =
                            guardianIOnputFile.Descendants("Account").Attributes("ExternalRef").FirstOrDefault().Value

                    End If
                    'loop through each element and set the value to the empty string
                    For Each item In guardianIOnputFile.Descendants("FeeOrderItemRuleId")

                        item.SetValue("")

                    Next
                    'loop through each element and set the value to the empty string
                    For Each item In guardianIOnputFile.Descendants("FeeOrderRuleId")

                        item.SetValue("")

                    Next
                    'loop through each element and set the value to the empty string
                    For Each item In guardianIOnputFile.Descendants("Name")

                        item.SetValue("")

                    Next
                    'loop through each element and set the value to the empty string
                    For Each item In guardianIOnputFile.Descendants("PercentageValue")

                        item.SetValue("")

                    Next
                    'loop through each element and set the value to the empty string
                    For Each item In guardianIOnputFile.Descendants("PercentageMinValue")

                        item.SetValue("")

                    Next
                    'loop through each element and set the value to the empty string
                    For Each item In guardianIOnputFile.Descendants("PercentageMaxValue")

                        item.SetValue("")

                    Next
                    'create new entity framework
                    Using nysdb = New NYSDBEntities()

                        'Retrieve The Current client details
                        interceptorClient =
                            (From interceptorFee In nysdb.interceptorClients Where interceptorFee.bossCode = strBossCode) _
                                .FirstOrDefault()

                        If interceptorClient IsNot Nothing Then
                            'assign client details to variable
                            strSplitNode = interceptorClient.SplitNodeName
                            strSplitFieldName = interceptorClient.SplitFieldName
                            strSplitFieldValue = interceptorClient.SplitFieldValue
                            strSplitUniqueFieldName = interceptorClient.SplitUniqueFieldName
                        End If

                    End Using

                    'Get A List Of All bookings
                    If guardianIOnputFile.Descendants("ImmediateDetail") IsNot Nothing Then

                        Dim bookings As IEnumerable(Of XElement)
                        Dim bookingsXElements As New List(Of XElement)
                        Dim bookingAgent As IEnumerable(Of XElement)
                        Dim bookingAgents As New List(Of XElement)
                        Dim passengers As IEnumerable(Of XElement)
                        Dim passengersXElements As New List(Of XElement)
                        Dim passengerSegmentList As IEnumerable(Of XElement)
                        Dim allTicketsXElements As New List(Of XElement)
                        Dim ticketsXElements As New List(Of XElement)
                        Dim reservationsXElements As New List(Of XElement)
                        Dim passengerReservationsXElements As New List(Of XElement)
                        Dim allSegments As IEnumerable(Of XElement)
                        Dim allSegmentsXElements As New List(Of XElement)
                        Dim currentPassengerSegmentsXElements As New List(Of XElement)
                        Dim segmentXElements As New List(Of XElement)
                        Dim distinctpassengerSegmentList As New List(Of XElement)
                        Dim customFieldsXElements As New List(Of XElement)
                        Dim distinctPassengers As New List(Of XElement)
                        'Declare ListOf custom values
                        Dim distinctCustomFieldValues As IEnumerable(Of String)
                        Dim deepCopyOfSegments As New List(Of XElement)
                        Dim deepCopyOfPassengers As New List(Of XElement)

                        bookings = guardianIOnputFile.Descendants("ImmediateDetail")
                        'check that a booking exsists
                        If bookings IsNot Nothing Then
                            'add bookings to the list
                            bookingsXElements.AddRange(bookings)
                            'Remove each booking from the file to create a generic template for all invoices.

                            For Each booking In bookingsXElements

                                'Increment number of bookings
                                intNumberOfBookings += 1

                                If booking.Attribute("TransactionType") IsNot Nothing Then

                                    transactionType = booking.Attribute("TransactionType").Value.ToLower()

                                    If transactionType.ToLower = "nonissue" Then
                                        blnNonIssue = True

                                        'take a copy of the file, we need to see it in it's original form
                                        IO.File.Copy(file.FullName,
                                                     ConfigUtils.getConfig("InterceptorErrorsFolder") &
                                                     file.Name, True)
                                        Exit For
                                    End If

                                    'reset the booleans
                                    blnInsideRefund = False

                                    blnIsValidRefund = False

                                End If

                                If booking.Attribute("OrderItemRef") IsNot Nothing Then

                                    'Rename OrderItemRef To IssueRef
                                    booking.SetAttributeValue("IssueRef", booking.Attribute("OrderItemRef").Value)

                                    booking.Attributes("OrderItemRef").Remove()
                                    'retrieve handoff issue reference
                                    strIssueRef = booking.Attributes("IssueRef").FirstOrDefault().Value

                                End If

                                If booking.Attribute("MachineNumber") IsNot Nothing Then
                                    'Retrieve Machine number
                                    strMachineNumber = booking.Attribute("MachineNumber").Value

                                End If

                                'Retrieve the current date time and set the transacation date of each booking
                                Dim d As DateTime = Now

                                Dim strDate As String = (d.ToString("yyyy-MM-dd")) & "T" & (d.ToString("hh:mm:ss"))

                                booking.SetAttributeValue("TransactionDate", strDate)
                                'check there is a booking agent node present
                                If booking.Descendants("BookingAgent") IsNot Nothing Then
                                    'retrieve booking agent
                                    bookingAgent = booking.Descendants("BookingAgent")
                                    'cast each booking agent node into a list of booking agenet elements
                                    bookingAgents = If(TryCast(bookingAgent, List(Of XElement)), bookingAgent.ToList())

                                    'check bvosscode of booker and decide whether to apply fee
                                    For Each agent In bookingAgents

                                        If _
                                            agent.Attributes("ExternalRef") IsNot Nothing AndAlso
                                            agent.Attributes("ExternalRef").First().Value <> "EV" And
                                            agent.Attributes("ExternalRef") IsNot Nothing AndAlso
                                            agent.Attributes("ExternalRef").First().Value <> "UNK" _
                                            Then
                                            blnAddOfflineFee = True
                                        End If

                                        'Check if the booking agent details need adding to the invoice if not remove accordingly
                                        If interceptorClient.AddBookerToInvoice = False Then
                                            agent.Remove()
                                        End If

                                    Next

                                End If
                                'loop through custom fields

                                'Get A List Of All Passengers within the booking in reverse order
                                passengers = booking.Descendants("Passenger").Reverse()
                                passengersXElements.AddRange(passengers)
                                'Get A List Of All segments within the booking
                                allSegments = booking.Descendants("Segment")
                                allSegmentsXElements.AddRange(allSegments)
                                'check a passenger exsists
                                If passengersXElements IsNot Nothing Then

                                    'loop through passengers
                                    For Each passenger In passengersXElements

                                        'Get passenger reference
                                        If passenger.Attribute("Ref") IsNot Nothing Then
                                            'assign passenger ref value to string to represent the current passenger
                                            Dim strCurrentPassengerRef As String = passenger.Attribute("Ref").Value

                                            'Get A List Of All Passenger Custom Fields
                                            passenger.Descendants("CustomField").Where(Function(x) x.Attribute("Value").Value = "").Remove()
                                            'create a collection on cuastomfield xelements
                                            customFields = passenger.Descendants("CustomField")
                                            'add collection of customfield elements to the list
                                            customFieldsXElements.AddRange(customFields)
                                            'loop through all the segments to find each segment containing a ticket belonging to the passenger
                                            segmentXElements = allSegmentsXElements.Where(Function(x) x.Descendants("Ticket").Attributes("PassengerRef").Any(Function(y) y.Value = strCurrentPassengerRef)).ToList()

                                            'loop through each segments
                                            For Each segment As XElement In segmentXElements
                                                'Get A List Of All segment Passenger Tickets
                                                ticketsXElements = segment.Descendants("Ticket").Where(Function(x) x.Attributes("PassengerRef").Any(Function(y) y.Value = strCurrentPassengerRef)).ToList()
                                                'loop through each ticket in list of tickets
                                                For Each ticket In ticketsXElements
                                                    'increment ticket count
                                                    intTotalTickets += 1

                                                    'Check to see if transaction is a refund and if there is a refund node to prove this is a valid refund
                                                    If _
                                                        booking.Attribute("TransactionType") IsNot Nothing AndAlso
                                                        booking.Attribute("TransactionType").Value.ToLower() = "refund" Then
                                                        'increment rtefund count
                                                        intRefunds += 1

                                                        Dim refundAdded = False
                                                        'check that a refund node exsists within the booking
                                                        If booking.Descendants("Refund") IsNot Nothing Then
                                                            'retieve refund element
                                                            Dim refunds = booking.Descendants("Refund")

                                                            blnChangeAgencyCancellation = False

                                                            dblRefundCancellationCharge = 0.0

                                                            strCurrentFulfilmentRef = strCurrentPassengerRef
                                                            'loop through each refund element
                                                            For Each refund As XElement In refunds

                                                                lstInvalidRefunds.Add(intNumberOfBookings - 1)

                                                                refundAdded = True

                                                                If _
                                                                    refund.Descendants("DiscountReclaimed").Attributes(
                                                                        "TotalAmount").First().Value IsNot Nothing Then
                                                                    dblRefundCancellationCharge +=
                                                                        notDouble(
                                                                            refund.Descendants("DiscountReclaimed").
                                                                                     Attributes("TotalAmount").First().Value)

                                                                ElseIf _
                                                                    refund.Descendants("ATOCCancellationCharge").Attributes(
                                                                        "TotalAmount").First().Value IsNot Nothing Then
                                                                    dblRefundCancellationCharge +=
                                                                        notDouble(
                                                                            refund.Descendants("ATOCCancellationCharge").
                                                                                     Attributes("TotalAmount").First().Value)

                                                                ElseIf _
                                                                    refund.Descendants("ExgratiaPayment").Attributes(
                                                                        "TotalAmount").First().Value IsNot Nothing Then
                                                                    dblRefundCancellationCharge +=
                                                                        notDouble(
                                                                            refund.Descendants("ExgratiaPayment").Attributes(
                                                                                "TotalAmount").First().Value)

                                                                ElseIf _
                                                                    refund.Descendants("TOCExchangeFee").Attributes(
                                                                        "TotalAmount").First().Value IsNot Nothing Then
                                                                    dblRefundCancellationCharge +=
                                                                        notDouble(
                                                                            refund.Descendants("TOCExchangeFee").Attributes(
                                                                                "TotalAmount").First().Value)

                                                                ElseIf _
                                                                    refund.Descendants("AgencyCancellationCharge") IsNot _
                                                                    Nothing Then

                                                                    If _
                                                                        refund.Descendants("AgencyCancellationCharge").
                                                                            Attributes("TotalAmount").First().Value = "0.01" _
                                                                        Then

                                                                        If booking.Descendants("Account") IsNot Nothing Then

                                                                            booking.Descendants("Account").First().
                                                                                SetAttributeValue("externalref",
                                                                                                  notString(
                                                                                                      ConfigUtils.getConfig(
                                                                                                          "RefundExternalRefReplacement")))

                                                                        End If

                                                                        refund.Descendants("AgencyCancellationCharge").First() _
                                                                            .SetAttributeValue("TotalAmount",
                                                                                               ConfigUtils.getConfig(
                                                                                                   "RefundChargeReplacement"))

                                                                        If _
                                                                            passenger.Descendants("CustomField").First(
                                                                                Function(n) n.Attribute("Code").Value = "1") IsNot _
                                                                            Nothing Then

                                                                            passenger.Descendants("CustomField").First(
                                                                                Function(n) n.Attribute("Code").Value = "1") _
                                                                                .SetAttributeValue("Value", "NYS")

                                                                        End If

                                                                        dblRefundCancellationCharge = 0

                                                                        blnChangeAgencyCancellation = True

                                                                    End If

                                                                ElseIf _
                                                                    refund.Descendants("AgencyCancellationCharge").
                                                                        Attributes("TotalAmount") IsNot Nothing AndAlso
                                                                    refund.Descendants("AgencyCancellationCharge").
                                                                        Attributes("TotalAmount").First().Value Is Nothing _
                                                                    Then

                                                                    dblRefundCancellationCharge +=
                                                                        notDouble(
                                                                            refund.Descendants("AgencyCancellationCharge").
                                                                                     First().Attribute("TotalAmount").Value)

                                                                    refund.Descendants().First.AddAfterSelf(
                                                                        New XElement("AgencyCancellationCharge",
                                                                                     New XAttribute("VatCode", "1"),
                                                                                     New XAttribute("VATAmount", "0.00"),
                                                                                     New XAttribute("TotalAmount",
                                                                                                    dblRefundCancellationCharge)))

                                                                ElseIf _
                                                                    refund.Descendants("AgencyExchangeFee").Attributes(
                                                                        "TotalAmount") IsNot Nothing AndAlso
                                                                    refund.Descendants("AgencyExchangeFee").Attributes(
                                                                        "TotalAmount").First().Value IsNot Nothing Then

                                                                    dblRefundCancellationCharge +=
                                                                        notDouble(
                                                                            refund.Descendants("AgencyExchangeFee").
                                                                                     Attributes("TotalAmount").First().Value)

                                                                    refund.Descendants("AgencyCancellationCharge").First().
                                                                        SetAttributeValue("TotalAmount",
                                                                                          dblRefundCancellationCharge)

                                                                    dblRefundCancellationCharge = 0

                                                                End If

                                                            Next

                                                        End If

                                                        If refundAdded Then
                                                            'take a copy of the file, we need to see it in it's original form
                                                            IO.File.Copy(file.FullName,
                                                                         ConfigUtils.getConfig("EvolviGuardianRefundCopies") &
                                                                         file.Name, True)

                                                        End If
                                                    End If

                                                Next
                                                'clear list of elements
                                                ticketsXElements = Nothing

                                            Next
                                            'cleart list of segments
                                            segmentXElements = Nothing

                                        End If
                                        'If the temporary list of custom elements is not empty
                                        If customFieldsXElements IsNot Nothing Then

                                            customCode1 = passenger.Descendants("CustomField").Any(Function(n) n.Attribute("Code").Value = "1")
                                            customCode2 = passenger.Descendants("CustomField").Any(Function(n) n.Attribute("Code").Value = "2")
                                            customCode3 = passenger.Descendants("CustomField").Any(Function(n) n.Attribute("Code").Value = "3")
                                            customCode4 = passenger.Descendants("CustomField").Any(Function(n) n.Attribute("Code").Value = "4")
                                            customCode5 = passenger.Descendants("CustomField").Any(Function(n) n.Attribute("Code").Value = "5")
                                            customCode6 = passenger.Descendants("CustomField").Any(Function(n) n.Attribute("Code").Value = "6")
                                            customCode7 = passenger.Descendants("CustomField").Any(Function(n) n.Attribute("Code").Value = "7")
                                            customCode8 = passenger.Descendants("CustomField").Any(Function(n) n.Attribute("Code").Value = "8")
                                            customCode9 = passenger.Descendants("CustomField").Any(Function(n) n.Attribute("Code").Value = "9")
                                            customCode10 = passenger.Descendants("CustomField").Any(Function(n) n.Attribute("Code").Value = "10")

                                            'loop through custom fields
                                            For Each field In customFieldsXElements
                                                'declare code element
                                                Dim code
                                                'if the code value of the current field is not blank then
                                                If field.Attribute("Code") IsNot Nothing Then
                                                    'assign value to code element
                                                    code = field.Attribute("Code").Value.ToLower
                                                    'declare value
                                                    Dim value
                                                    'check the value attribute
                                                    If field.Attribute("Value") IsNot Nothing Then
                                                        value = field.Attribute("Value").Value
                                                    End If
                                                    'assign value as a trimmed string
                                                    strTravellerEmail = value.Trim()
                                                    'Check The code value of each custom field
                                                    Select Case code

                                                        Case "1"
                                                            'Check Boss Codes And Apply Changes
                                                            If strBossCode.ToUpper() = "HERECC" Then
                                                                'Check first char of bosscode and apply change
                                                                If value.StartsWith("j") Then
                                                                    strExternalRefValue = "HOOPLE"
                                                                Else
                                                                    strExternalRefValue = "HERECC"
                                                                End If

                                                                If booking.Descendants("Account") IsNot Nothing Then

                                                                    'set the external reference within the account node
                                                                    booking.Descendants("Account").First().
                                                                        SetAttributeValue("externalref",
                                                                                          strExternalRefValue)

                                                                End If

                                                            ElseIf strBossCode.ToUpper() = "LV" Then
                                                                'check lv traveller is active
                                                                If _
                                                                    ConfigUtils.getConfig("LVTravellerEmailActive") =
                                                                    "true" Then
                                                                    'Declare that traveler is p[rovided
                                                                    blnLvTravellerProvided = True

                                                                    strTravellerEmail = value
                                                                    strTravellerEmail = strTravellerEmail.Trim
                                                                    currentTraveller =
                                                                        clsTravellerProfiles.getByEmail(
                                                                            strTravellerEmail)

                                                                    'ensure traveller email exsists
                                                                    If _
                                                                        currentTraveller.Email Is Nothing OrElse
                                                                        currentTraveller.Email = "" Then

                                                                        blnIsDataError = True
                                                                        'assign the traverller company for the error email
                                                                        travelerClient = "LV Traveller"

                                                                    End If

                                                                End If

                                                            ElseIf strBossCode.ToUpper = "KCOM0" Then
                                                                'get traveller email
                                                                intEmailExist =
                                                                    clsTravellerProfiles.checkEmail(strTravellerEmail)

                                                                If intEmailExist = 1 Then
                                                                    strMckTravellerCostCentre =
                                                                        clsTravellerProfiles.getCode1(strTravellerEmail)
                                                                    strMckTravellerHrNumber =
                                                                        clsTravellerProfiles.getCode2(strTravellerEmail)
                                                                Else
                                                                    blnIsDataError = True
                                                                    'assign the traverller company for the error email
                                                                    travelerClient = "Kcom Traveller"

                                                                End If

                                                            ElseIf strBossCode.ToUpper = "MCKESSON" Then

                                                                intEmailExist =
                                                                    clsTravellerProfiles.checkEmail(strTravellerEmail)

                                                                If intEmailExist = 1 Then
                                                                    Dim dbTraveller =
                                                                            clsTravellerProfiles.getByEmail(
                                                                                strTravellerEmail)
                                                                    strKcomTravellerEmployeeNumber = dbTraveller.Code1
                                                                Else

                                                                    blnIsDataError = True
                                                                    'assign the traverller company for the error email
                                                                    travelerClient = "McKesson Traveller"

                                                                End If

                                                            ElseIf strBossCode.ToUpper = "SYSTEMC0" Then

                                                                intEmailExist =
                                                                    clsTravellerProfiles.checkEmail(strTravellerEmail)

                                                                If intEmailExist = 1 Then
                                                                    strSystemCTravellerCostCentre =
                                                                        clsTravellerProfiles.getCode1(strTravellerEmail)
                                                                    strSystemCTravellerHrNumber =
                                                                        clsTravellerProfiles.getCode2(strTravellerEmail)
                                                                Else

                                                                    blnIsDataError = True
                                                                    'assign the traverller company for the error email
                                                                    travelerClient = "SystemC Traveller"

                                                                End If

                                                            ElseIf _
                                                                strBossCode.ToUpper = "PIKSELUK0".ToUpper Or
                                                                strBossCode.ToUpper = "PIKSELFR0".ToUpper Or
                                                                strBossCode.ToUpper = "PIKSELIT0".ToUpper Then

                                                                intEmailExist =
                                                                    clsTravellerProfiles.checkEmail(strTravellerEmail)

                                                                If intEmailExist = 1 Then
                                                                    Dim oTraveller As New clsTravellerProfiles
                                                                    oTraveller.populate(0, strTravellerEmail)

                                                                    strPikselTravellerEmployeeNumber = oTraveller.Code1
                                                                    strPikselTravellerBussinessUnit = oTraveller.Code3
                                                                    strPikselTravellerDepartment = oTraveller.Code5
                                                                    strPikselTravellerLegalEntity = oTraveller.Code6
                                                                    strPikselTravellerLocation = oTraveller.Location
                                                                Else

                                                                    blnIsDataError = True
                                                                    'assign the traverller company for the error email
                                                                    travelerClient = "Piksel Traveller"

                                                                End If

                                                            ElseIf strBossCode = "UHSM" Then
                                                                strCustomfieldCref1 = value
                                                                strCustomfieldCref1 =
                                                                    clsInterceptorCostCentre.findDivisionName(0,
                                                                                                              strCustomfieldCref1 _
                                                                                                                 .
                                                                                                                 Substring(
                                                                                                                     0,
                                                                                                                     1))

                                                            ElseIf strBossCode = "UOYR" Then
                                                                'do not charge delivery for Archaeology department
                                                                If value = "ARCHAEOL" Then
                                                                    blnSkipDeliveryCharge = True
                                                                End If
                                                            ElseIf strBossCode = "CHIESI0" Then

                                                                chiesiGlCode =
                                                                    chiesiGlCodeAndDescription.Substring(
                                                                        InStrRev(chiesiGlCodeAndDescription, " "),
                                                                        chiesiGlCodeAndDescription.Length -
                                                                        InStrRev(chiesiGlCodeAndDescription, " "))

                                                                Log.Info(
                                                                    "CHIESI Ticket - Getting GL Code = " & chiesiGlCode)
                                                                Try

                                                                    Using database = New NYSDBEntities
                                                                        Log.Info("CHIESI Ticket - Getting GL Code")

                                                                        Dim chiesiGlDescription As String =
                                                                                chiesiGlCodeAndDescription.Substring(0,
                                                                                                                     InStrRev(
                                                                                                                         chiesiGlCodeAndDescription,
                                                                                                                         " - ") -
                                                                                                                     1)
                                                                        Log.Info(
                                                                            "CHIESI Ticket - Getting GL Code Description " &
                                                                            chiesiGlDescription)

                                                                        Try
                                                                            Log.Info("Before ef call")

                                                                            Dim counter As Integer =
                                                                                    (From _
                                                                                    t In database.TravellerProfiles).
                                                                                    Count()
                                                                            'log.Info("Test Count: " & counter.ToString())

                                                                            Dim test =
                                                                                    database.ChiesiGLIOCodes.SqlQuery(
                                                                                        "SELECT * FROM ChiesiGLIOCodes") _
                                                                                    .Count()
                                                                            Log.Info("Test: " & test.ToString())
                                                                            Dim glCode As New ChiesiGLIOCodes
                                                                            'Go get the IO Code
                                                                            glCode =
                                                                                (From c In database.ChiesiGLIOCodes
                                                                                    Where _
                                                                                        c.NYSDescription =
                                                                                        chiesiGlDescription).
                                                                                    FirstOrDefault

                                                                            Log.Info(
                                                                                "CHIESI Ticket - Looking for IO Code")

                                                                            If Not IsNothing(glCode) Then
                                                                                Log.Info("GL Code is not nothing")
                                                                                chiesiIoCode =
                                                                                    IIf(IsNothing(glCode.IOCode), ".",
                                                                                        glCode.IOCode)
                                                                            Else

                                                                                'Error?

                                                                                chiesiIoCode =
                                                                                    "Unable to trace GI code in database"
                                                                            End If
                                                                        Catch ex As Exception
                                                                            Log.Info("Caught null exception")
                                                                            Log.Info(ex.Message.ToString())
                                                                            Log.Info(ex.InnerException.ToString())
                                                                            blnIsDataError = True
                                                                            'GoTo skipfile
                                                                        End Try

                                                                    End Using
                                                                Catch ex As Exception
                                                                    Log.Info("context instantiation error")
                                                                    Log.Info(ex.Message.ToString())
                                                                End Try

                                                            End If

                                                            blnSkipDeliveryCharge = False
                                                        Case "2"

                                                            If strBossCode.ToUpper() = "LV" Then

                                                                If value <> "" Then

                                                                    currentTraveller.ProjectCode =
                                                                        field.Attribute("Value").Value

                                                                End If

                                                                If _
                                                                    ConfigUtils.getConfig("LVTravellerEmailActive") =
                                                                    "true" Then

                                                                    blnLvProjectCode = True

                                                                End If

                                                            ElseIf strBossCode.ToUpper() = "CHIESI0" Then

                                                                intEmailExist =
                                                                    clsTravellerProfiles.checkEmail(strTravellerEmail)

                                                                If intEmailExist = 1 Then
                                                                    Dim dbTraveller =
                                                                            clsTravellerProfiles.getByEmail(
                                                                                strTravellerEmail)
                                                                    strChiesiTravellerAreaCode = dbTraveller.Code3
                                                                    strChiesiTravellerDepartmentCode = dbTraveller.Code2
                                                                    strChiesiTravellerCostCode = dbTraveller.Code1
                                                                Else
                                                                    blnIsDataError = True

                                                                    travelerClient = "Cheisi Traveller"

                                                                End If

                                                            ElseIf _
                                                                strBossCode.ToUpper = "PIKSELUK0".ToUpper Or
                                                                strBossCode.ToUpper = "PIKSELFR0".ToUpper Or
                                                                strBossCode.ToUpper = "PIKSELIT0".ToUpper Then

                                                                strPikselTravellerPo = value

                                                            End If

                                                        Case "3"

                                                            If _
                                                                strBossCode.ToUpper = "PIKSELUK0".ToUpper Or
                                                                strBossCode.ToUpper = "PIKSELFR0".ToUpper Or
                                                                strBossCode.ToUpper = "PIKSELIT0".ToUpper Then

                                                                strPikselTravellerGlCode = value

                                                            ElseIf currentTraveller.ProjectCode <> "" Then

                                                                currentTraveller.Code1 = value

                                                            ElseIf strBossCode.ToUpper() = "CHIESI0" Then
                                                                'Need to save this for later
                                                                strChiesiApprovedByField =
                                                                    strChiesiApprovedByField.Replace("Code=""3""",
                                                                                                     "Code=""5""")
                                                            End If

                                                        Case "po"

                                                            strCustomFieldPo =
                                                                clsInterceptorClient.getCustomPO(strBossCode)

                                                            If strCustomFieldPo <> "" Then

                                                                field.SetAttributeValue("value", strCustomFieldPo)

                                                            End If

                                                        Case "cc"

                                                            strCustomfieldCc = value.First().Value

                                                            If _
                                                                strBossCode <> "" AndAlso
                                                                notString(
                                                                    ConfigUtils.getConfig("CostCentreSuffixClients")).
                                                                    Contains(strBossCode) Then

                                                                field.SetAttributeValue("value",
                                                                                        strCustomfieldCc &
                                                                                        notString(
                                                                                            ConfigUtils.getConfig(
                                                                                                "CostCentreSuffix")))
                                                                ' "11001"

                                                            End If

                                                        Case "cref1"

                                                            If strCustomfieldCref1 <> "" Then

                                                                field.Value = strCustomfieldCref1

                                                            End If

                                                            If code.StartsWith("SD") Then

                                                                If value.Length > 0 Then

                                                                    Try

                                                                        Dim dblTest = CDbl(code.Replace("SD", ""))

                                                                        dblSpecialDelivery = dblSpecialDelivery +
                                                                                             dblTest

                                                                    Catch ex As Exception

                                                                        dblSpecialDelivery = dblSpecialDelivery

                                                                        Log.Error("SPECIAL DELIVERY IS NOT A NUMBER!")

                                                                    End Try

                                                                End If

                                                            End If

                                                    End Select

                                                End If
                                                'check whether there has been an error
                                                If blnIsDataError Then
                                                    'send error email
                                                    Email(file.Name, strTravellerEmail, strIssueRef, travelerClient, "")

                                                    'take a copy of the file, we need to see it in it's original form
                                                    IO.File.Copy(file.FullName,
                                                                 ConfigUtils.getConfig("InterceptorErrorsFolder") &
                                                                 file.Name, True)
                                                    Exit For
                                                End If

                                            Next
                                            If blnIsDataError Then Exit For
                                        End If
                                        'check boss code
                                        Dim bossCode = strBossCode.ToUpper
                                        'check boss code
                                        Select Case bossCode

                                            Case "LV"

                                                If _
                                                    ConfigUtils.getConfig("LVTravellerEmailActive") = "true" AndAlso
                                                    blnLvTravellerProvided AndAlso customCode1 AndAlso customCode2 AndAlso customCode3 Or blnLvProjectCode = False AndAlso customCode1 Then
                                                    'clear it first
                                                    strLvPassengerCustomFields.Clear()

                                                    'Add new node with attributes

                                                    If customCode2 Then
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "2").SetAttributeValue("Label", "Employee Number")
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "2").SetAttributeValue("Value", currentTraveller.Code2)
                                                    Else
                                                        passenger.Descendants("CustomField").First.AddAfterSelf(
                                                        New XElement("CustomField", New XAttribute("Code", "2"),
                                                                     New XAttribute("Label", "Employee Number"),
                                                                     New XAttribute("Value", currentTraveller.Code2)))
                                                    End If

                                                    If currentTraveller.ProjectCode <> "" Then
                                                        'Add new node with attributes

                                                        If customCode3 Then

                                                            passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "3").SetAttributeValue("Label", "Project Code")
                                                            passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "3").SetAttributeValue("Value", currentTraveller.ProjectCode)

                                                        Else
                                                            passenger.Descendants("CustomField").First.AddAfterSelf(
                                                          New XElement("CustomField", New XAttribute("Code", "3"),
                                                                       New XAttribute("Label", "Project Code"),
                                                                       New XAttribute("Value",
                                                                                      currentTraveller.ProjectCode)))
                                                        End If

                                                    End If

                                                    'Add new node with attributes

                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "1").SetAttributeValue("Label", "ACK Code")
                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "1").SetAttributeValue("Value", currentTraveller.Code1)

                                                    strLvPassengerCustomFields.Clear()

                                                End If
                                            Case "MCKESSON"

                                                If customCode1 AndAlso customCode2 Or blnMckOptionalCostCentre = False AndAlso customCode1 Then
                                                    'Add new node with attributes

                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "1").SetAttributeValue("Label", "Cost Centre")
                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "1").SetAttributeValue("Value", strMckTravellerCostCentre)

                                                    If passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "2") IsNot Nothing Then
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "2").SetAttributeValue("Label", "HR Number")
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "2").SetAttributeValue("Value", strMckTravellerHrNumber)
                                                    Else
                                                        'Add new node with attributes
                                                        passenger.Descendants("CustomField").First.Add(
                                                            New XElement("CustomField", New XAttribute("Code", "2"),
                                                                         New XAttribute("Label", "HR Number"),
                                                                         New XAttribute("Value", strMckTravellerHrNumber)))
                                                    End If

                                                End If

                                            Case "CHIESI0"

                                                'Dim customFieldAttribute = passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "1").Attributes("Value").First().Value
                                                'Dim customFieldAttribute = (From custom In passenger.Descendants("CustomField") Where custom.Attribute("Code").Value = "1" Select custom.Attribute("Value").Value)

                                                If customCode1 And customCode2 And customCode3 Then
                                                    strChiesiPassengerCustomFields.Clear()

                                                    Log.Info("CHIESI Ticket - Recreate Fields")

                                                    If customCode7 Then
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "7").SetAttributeValue("Label", "GL")
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "7").SetAttributeValue("Value", chiesiGlCode)
                                                    Else
                                                        passenger.Descendants("CustomField").First.Add(
                                                            New XElement("CustomField", New XAttribute("Code", "7"),
                                                                         New XAttribute("Label", "GL"),
                                                                         New XAttribute("Value", chiesiGlCode)))
                                                    End If

                                                    If customCode8 Then
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "8").SetAttributeValue("Label", "IO")
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "8").SetAttributeValue("Value", chiesiIoCode)
                                                    Else
                                                        passenger.Descendants("CustomField").First.Add(
                                                            New XElement("CustomField", New XAttribute("Code", "8"),
                                                                         New XAttribute("Label", "IO"),
                                                                         New XAttribute("Value", chiesiIoCode)))
                                                    End If

                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "1").SetAttributeValue("Label", "Cost Centre")
                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "1").SetAttributeValue("Value", strChiesiTravellerCostCode)

                                                    strChiesiPassengerCustomFields.Clear()
                                                    Log.Info("CHIESI Ticket - Ended - Recreate Fields")
                                                End If
                                            Case "KCOM0"
                                                Dim employeeNumber = ""

                                                Log.Error(strKcomPassengerCustomFields.ToString() & "-Before I hit")

                                                If customCode1 Then

                                                    Dim dbTraveller =
                                                            clsTravellerProfiles.getByEmail(
                                                                passenger.Descendants("CustomField").First(
                                                                    Function(n) n.Attribute("Code").Value = "1").
                                                                                               Attributes("Value").First() _
                                                                                               .Value.Trim())

                                                    employeeNumber = dbTraveller.Code1

                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "1").SetAttributeValue("Label", "Employee Number")
                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "1").SetAttributeValue("Value", employeeNumber)

                                                    Log.Info(strKcomPassengerCustomFields.ToString())
                                                End If
                                            Case "SYSTEMC0"

                                                If customCode1 AndAlso customCode3 Or
                                                   blnSystemCOptionalCostCentre = False AndAlso customCode1 Then

                                                    'Get A List Of All Passenger Custom Fields
                                                    If customCode2 Then
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "2").SetAttributeValue("Label", "HR Number")
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "2").SetAttributeValue("Value", strSystemCTravellerHrNumber)
                                                    Else
                                                        passenger.Descendants("CustomField").First.AddAfterSelf(
                                                            New XElement("CustomField", New XAttribute("Code", "2"),
                                                                         New XAttribute("Label", "HR Number"),
                                                                         New XAttribute("Value", strSystemCTravellerHrNumber)))
                                                    End If

                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "1").SetAttributeValue("Label", "Cost Centre")
                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "1").SetAttributeValue("Value", strSystemCTravellerCostCentre)

                                                End If
                                            Case "PIKSELUK0", "PIKSELFR0", "PIKSELIT0"

                                                If customCode1 AndAlso customCode2 AndAlso customCode3 Then

                                                    If customCode5 Then
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "5").SetAttributeValue("Label", "Department")
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "5").SetAttributeValue("Value", strPikselTravellerDepartment)
                                                    Else
                                                        passenger.Descendants("CustomField").First.AddAfterSelf(
                                                            New XElement("CustomField", New XAttribute("Code", "5"),
                                                                         New XAttribute("Label", "Department"),
                                                                         New XAttribute("Value",
                                                                                        strPikselTravellerDepartment)))
                                                    End If

                                                    If customCode6 Then
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "6").SetAttributeValue("Label", "Legal Entity")
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "6").SetAttributeValue("Value", strPikselTravellerLegalEntity)
                                                    Else
                                                        passenger.Descendants("CustomField").First.AddAfterSelf(
                                                       New XElement("CustomField", New XAttribute("Code", "6"),
                                                                    New XAttribute("Label", "Legal Entity"),
                                                                    New XAttribute("Value", strPikselTravellerLegalEntity)))
                                                    End If

                                                    If customCode7 Then
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "7").SetAttributeValue("Label", "Location")
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "7").SetAttributeValue("Value", strPikselTravellerLocation)
                                                    Else
                                                        passenger.Descendants("CustomField").First.AddAfterSelf(
                                                            New XElement("CustomField", New XAttribute("Code", "7"),
                                                                         New XAttribute("Label", "Location"),
                                                                         New XAttribute("Value", strPikselTravellerLocation)))
                                                    End If

                                                    If passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "8") IsNot Nothing Then
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "8").SetAttributeValue("Label", "GL Code")
                                                        passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "8").SetAttributeValue("Value", strPikselTravellerGlCode)
                                                    Else
                                                        passenger.Descendants("CustomField").First.AddAfterSelf(
                                                            New XElement("CustomField", New XAttribute("Code", "8"),
                                                                         New XAttribute("Label", "GL Code"),
                                                                         New XAttribute("Value", strPikselTravellerGlCode)))
                                                    End If

                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "3").SetAttributeValue("Label", "Business Unit")
                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "3").SetAttributeValue("Value", strPikselTravellerBussinessUnit)

                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "2").SetAttributeValue("Label", "Project Code")
                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "2").SetAttributeValue("Value", strPikselTravellerPo)

                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "1").SetAttributeValue("Label", "Employee Number")
                                                    passenger.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "1").SetAttributeValue("Value", strPikselTravellerEmployeeNumber)

                                                End If

                                        End Select
                                        'if the client has fees activated and ticket type is issue then
                                        If _
                                            booking.Attribute("TransactionType") IsNot Nothing AndAlso
                                            interceptorClient.InterceptorFeesActive = True AndAlso
                                            booking.Attribute("TransactionType").Value.ToUpper = "ISSUE" Then
                                            'get the fulfilment type
                                            If booking.Attribute("DeliveryMethod") IsNot Nothing Then

                                                fulfilmentType = booking.Attribute("DeliveryMethod").Value

                                            End If

                                            Dim dblTotalFee = 0.0
                                            Dim dblTotalPerTicket = 0.0
                                            Dim dblTotalPerPax = 0.0
                                            Dim dblTotalPerOrderItem = 0.0
                                            Dim interceptorCharge As New InterceptorCharges

                                            Using nysdb = New NYSDBEntities
                                                'create a new charge item to allocate fees to

                                                If interceptorCharge Is Nothing Then
                                                    interceptorCharge.IssueRef = strIssueRef
                                                    interceptorCharge.TotalTickets = intTotalTickets
                                                    interceptorCharge.TransactionFee = 0.0
                                                    interceptorCharge.TransactionFeeTotal = 0.0
                                                    interceptorCharge.PostageFee = 0.0
                                                    interceptorCharge.PostageFeeTotal = 0.0
                                                    interceptorCharge.OfflineFee = 0.0
                                                    interceptorCharge.OfflineFeeTotal = 0.0
                                                    interceptorCharge.SpecialDeliveryFee = 0.0
                                                    interceptorCharge.SpecialDeliveryFeeTotal = 0.0
                                                    interceptorCharge.CancellationFee = 0.0
                                                    interceptorCharge.CancellationFeeTotal = 0.0
                                                    interceptorCharge.IssueType = "ISSUE"
                                                    interceptorCharge.IssueDate = strIssueDate
                                                    interceptorCharge.ClientBossCode = strBossCode
                                                    interceptorCharge.ToDFee = 0.0
                                                    interceptorCharge.ToDFeeTotal = 0.0
                                                    interceptorCharge.KioskFee = 0.0
                                                    interceptorCharge.KioskFeeTotal = 0.0
                                                    interceptorCharge.SaturdaySDFee = 0.0
                                                    interceptorCharge.SaturdaySDFeeTotal = 0.0
                                                    interceptorCharge.CourierFee = 0.0
                                                    interceptorCharge.CourierFeeTotal = 0.0
                                                    interceptorCharge.PlainPaperTicketingFee = 0.0
                                                    interceptorCharge.PlainPaperTicketingFeeTotal = 0.0
                                                End If

                                                'check fulfilment type
                                                Select Case fulfilmentType.ToLower()

                                                    Case "ticket on departure"

                                                        dblTotalPerTicket = IIf(blnAddOfflineFee,
                                                                                interceptorClient.ToDOfflineFeePerTicket *
                                                                                intTotalTickets,
                                                                                interceptorClient.ToDOnlineFeePerTicket *
                                                                                intTotalTickets)
                                                        dblTotalPerPax = IIf(blnAddOfflineFee,
                                                                             interceptorClient.ToDOfflineFeePerTraveller *
                                                                             intTotalPassengers,
                                                                             interceptorClient.ToDOnlineFeePerTraveller *
                                                                             intTotalPassengers)
                                                        dblTotalPerOrderItem = IIf(blnAddOfflineFee,
                                                                                   interceptorClient.
                                                                                      ToDOfflineFeePerOrderItem,
                                                                                   interceptorClient.
                                                                                      ToDOnlineFeePerOrderItem)
                                                        interceptorCharge.ToDFeeTotal = dblTotalPerTicket +
                                                                                        dblTotalPerPax +
                                                                                        dblTotalPerOrderItem
                                                        interceptorCharge.ToDFee = dblTotalPerTicket + dblTotalPerPax +
                                                                                   dblTotalPerOrderItem

                                                        blnFeeAdded = True

                                                    Case "first class post"

                                                        dblTotalPerTicket = IIf(blnAddOfflineFee,
                                                                                interceptorClient.
                                                                                    PostageOfflineFeePerTicket *
                                                                                intTotalTickets,
                                                                                interceptorClient.
                                                                                    PostageOnlineFeePerTicket *
                                                                                intTotalTickets)
                                                        dblTotalPerPax = IIf(blnAddOfflineFee,
                                                                             interceptorClient.
                                                                                 PostageOfflineFeePerTraveller *
                                                                             intTotalPassengers,
                                                                             interceptorClient.
                                                                                 PostageOnlineFeePerTraveller *
                                                                             intTotalPassengers)
                                                        dblTotalPerOrderItem = IIf(blnAddOfflineFee,
                                                                                   interceptorClient.
                                                                                      PostageOfflineFeePerOrderItem,
                                                                                   interceptorClient.
                                                                                      PostageOnlineFeePerOrderItem)
                                                        interceptorCharge.PostageFee = dblTotalPerTicket +
                                                                                       dblTotalPerPax +
                                                                                       dblTotalPerOrderItem
                                                        interceptorCharge.PostageFeeTotal = dblTotalPerTicket +
                                                                                            dblTotalPerPax +
                                                                                            dblTotalPerOrderItem

                                                        blnFeeAdded = True

                                                    Case "1pm courier"

                                                        dblTotalPerTicket = IIf(blnAddOfflineFee,
                                                                                interceptorClient.
                                                                                    CourierOfflineFeePerTicket *
                                                                                intTotalTickets,
                                                                                interceptorClient.
                                                                                    CourierOnlineFeePerTicket *
                                                                                intTotalTickets)
                                                        dblTotalPerPax = IIf(blnAddOfflineFee,
                                                                             interceptorClient.
                                                                                 CourierOfflineFeePerTraveller *
                                                                             intTotalPassengers,
                                                                             interceptorClient.
                                                                                 CourierOnlineFeePerTraveller *
                                                                             intTotalPassengers)
                                                        dblTotalPerOrderItem = IIf(blnAddOfflineFee,
                                                                                   interceptorClient.
                                                                                      CourierOfflineFeePerOrderItem,
                                                                                   interceptorClient.
                                                                                      CourierOnlineFeePerOrderItem)
                                                        interceptorCharge.CourierFee = dblTotalPerTicket +
                                                                                       dblTotalPerPax +
                                                                                       dblTotalPerOrderItem
                                                        interceptorCharge.CourierFeeTotal = dblTotalPerTicket +
                                                                                            dblTotalPerPax +
                                                                                            dblTotalPerOrderItem

                                                        blnFeeAdded = True

                                                    Case "special delivery"

                                                        dblTotalPerTicket = IIf(blnAddOfflineFee,
                                                                                interceptorClient.
                                                                                    SpecialDeliveryOfflineFeePerTicket *
                                                                                intTotalTickets,
                                                                                interceptorClient.
                                                                                    SpecialDeliveryOnlineFeePerTicket *
                                                                                intTotalTickets)
                                                        dblTotalPerPax = IIf(blnAddOfflineFee,
                                                                             interceptorClient.
                                                                                 SpecialDeliveryOfflineFeePerTraveller *
                                                                             intTotalPassengers,
                                                                             interceptorClient.
                                                                                 SpecialDeliveryOnlineFeePerTraveller *
                                                                             intTotalPassengers)
                                                        dblTotalPerOrderItem = IIf(blnAddOfflineFee,
                                                                                   interceptorClient.
                                                                                      SpecialDeliveryOfflineFeePerOrderItem,
                                                                                   interceptorClient.
                                                                                      SpecialDeliveryOnlineFeePerOrderItem)
                                                        interceptorCharge.SpecialDeliveryFee = dblTotalPerTicket +
                                                                                               dblTotalPerPax +
                                                                                               dblTotalPerOrderItem
                                                        interceptorCharge.SpecialDeliveryFeeTotal = dblTotalPerTicket +
                                                                                                    dblTotalPerPax +
                                                                                                    dblTotalPerOrderItem

                                                        blnFeeAdded = True

                                                    Case "saturday special delivery"

                                                        dblTotalPerTicket = IIf(blnAddOfflineFee,
                                                                                interceptorClient.
                                                                                    SaturdaySpecialDeliveryOfflineFeePerTicket *
                                                                                intTotalTickets,
                                                                                interceptorClient.
                                                                                    SaturdaySpecialDeliveryOnlineFeePerTicket *
                                                                                intTotalTickets)
                                                        dblTotalPerPax = IIf(blnAddOfflineFee,
                                                                             interceptorClient.
                                                                                 SaturdaySpecialDeliveryOfflineFeePerTraveller *
                                                                             intTotalPassengers,
                                                                             interceptorClient.
                                                                                 SaturdaySpecialDeliveryOnlineFeePerTraveller *
                                                                             intTotalPassengers)
                                                        dblTotalPerOrderItem = IIf(blnAddOfflineFee,
                                                                                   interceptorClient.
                                                                                      SaturdaySpecialDeliveryOfflineFeePerOrderItem,
                                                                                   interceptorClient.
                                                                                      SaturdaySpecialDeliveryOnlineFeePerOrderItem)
                                                        interceptorCharge.SaturdaySDFee = dblTotalPerTicket +
                                                                                          dblTotalPerPax +
                                                                                          dblTotalPerOrderItem
                                                        interceptorCharge.SaturdaySDFeeTotal = dblTotalPerTicket +
                                                                                               dblTotalPerPax +
                                                                                               dblTotalPerOrderItem

                                                        blnFeeAdded = True

                                                    Case "self printing"

                                                        dblTotalPerTicket = IIf(blnAddOfflineFee,
                                                                                interceptorClient.
                                                                                    PlainPaperTicketingOfflineFeePerTicket *
                                                                                intTotalTickets,
                                                                                interceptorClient.
                                                                                    PlainPaperTicketingOnlineFeePerTicket *
                                                                                intTotalTickets)
                                                        dblTotalPerPax = IIf(blnAddOfflineFee,
                                                                             interceptorClient.
                                                                                 PlainPaperTicketingOfflineFeePerTraveller *
                                                                             intTotalPassengers,
                                                                             interceptorClient.
                                                                                 PlainPaperTicketingOnlineFeePerTraveller *
                                                                             intTotalPassengers)
                                                        dblTotalPerOrderItem = IIf(blnAddOfflineFee,
                                                                                   interceptorClient.
                                                                                      PlainPaperTicketingOfflineFeePerOrderItem,
                                                                                   interceptorClient.
                                                                                      PlainPaperTicketingOnlineFeePerOrderItem)
                                                        interceptorCharge.PlainPaperTicketingFee = dblTotalPerTicket +
                                                                                                   dblTotalPerPax +
                                                                                                   dblTotalPerOrderItem
                                                        interceptorCharge.PlainPaperTicketingFeeTotal =
                                                            dblTotalPerTicket +
                                                            dblTotalPerPax +
                                                            dblTotalPerOrderItem

                                                        blnFeeAdded = True
                                                End Select
                                                'save charge item to the database
                                                nysdb.SaveChanges()

                                            End Using

                                        End If
                                    Next
                                    'if either boolean true exit the loop
                                    If blnIsDataError Or blnNonIssue Then Exit For
                                End If

                                passengers = Nothing
                                passengersXElements.Clear()
                                'Get A List Of All segments within the booking
                                allSegments = Nothing
                                allSegmentsXElements.Clear()
                                'check a passenger exsists
                                'Check hyandoff for a custom field wehich is outside the passenger node with the stated values
                                If _
                                    booking.Descendants("CustomField").Any(
                                        Function(n) n.Attribute("Code").Value = "ORDERREF") Then
                                    'if the field exsists update the value attribute
                                    booking.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "ORDERREF").SetAttributeValue("Label", "")
                                    booking.Descendants("CustomField").First(Function(n) n.Attribute("Code").Value = "ORDERREF").SetAttributeValue("Value", strOrderRef)
                                Else
                                    ''if the custom field does not exsist then create a new custom field.
                                    booking.Descendants("Segment").First.AddAfterSelf(New XElement("CustomField", New XAttribute("Code", "ORDERREF"), New XAttribute("Value", strOrderRef)))
                                End If

                                '=======================================Split======================================
                                'if neither boolean true continue to split the invoices
                                If blnIsDataError = False And blnNonIssue = False Then

                                    Dim currentBooking As ImmediateDetail = booking.CreateReader().Deserialize(Of ImmediateDetail)()
                                    Dim currentBookingElement = currentBooking.ToXElement(Of ImmediateDetail)()

                                    Dim emailErrorMessage As String
                                    'Remove each booking from the file to create a generic template for all invoices.
                                    guardianIOnputFile.Descendants("ImmediateDetail").Remove()
                                    'declare count
                                    Dim splitCount As Integer

                                    'loop through each complete booking and prepare for the split
                                    intImmediateDetailCount += 1

                                    'if there are invalid refunds then add the position number of the invalid refund to allow for later checks to ensure they are ommited  the final invoice
                                    If lstInvalidRefunds.Count > 0 AndAlso
                                       lstInvalidRefunds.Contains(intImmediateDetailCount - 1) Then
                                        'do nothing
                                    Else
                                        strFileName = file.Name
                                        'Add the booking back to the origional file
                                        'GuardianIOnputFile.Descendants("Agency").First.AddAfterSelf(booking)
                                        'Remove all tickets

                                        If currentBooking.PassengerGroup.Passenger.ToList() IsNot Nothing Then

                                            deepCopyOfPassengers.AddRange(currentBookingElement.Descendants("Passenger"))

                                            deepCopyOfPassengers.Distinct()

                                            If strSplitFieldValue.ToLower() = "traveller's email" Then
                                                strSplitFieldValue = "Employee Number"
                                            End If

                                            'For each distinct custom field find passengers that match criteria
                                            distinctCustomFieldValues =
                                                (From p In deepCopyOfPassengers
                                                    Where _
                                                        p.Descendants("CustomField").Attributes("Label") IsNot Nothing AndAlso
                                                        p.Descendants("CustomField").Attributes("Label").FirstOrDefault() _
                                                            .Value = strSplitFieldValue
                                                    Select _
                                                        p.Descendants("CustomField").Attributes("Value").FirstOrDefault() _
                                                            .Value).ToList().Distinct()

                                            allSegments = booking.Descendants("Segment").Reverse().ToList()
                                            allSegmentsXElements = If(TryCast(allSegments, List(Of XElement)), allSegments.ToList())

                                            deepCopyOfSegments.AddRange(currentBookingElement.Descendants("Segment"))

                                            deepCopyOfSegments.Reverse()

                                        End If

                                        Dim successFullSplit = False

                                        'Check which node is to be used for the splitting of the invoices
                                        Select Case strSplitNode
                                            'If the invoice should be split on the passenger then
                                            Case "Passenger"
                                                'loop through list of distinct passengers
                                                For Each passenger In deepCopyOfPassengers
                                                    allSegmentsXElements = deepCopyOfSegments
                                                    Dim passengerRef = passenger.Attribute("Ref").Value
                                                    'remove elements from booking
                                                    booking.Descendants("Passenger").Remove()
                                                    booking.Descendants("Segment").Remove()
                                                    'Increment split count
                                                    splitCount += 1

                                                    'Add the current passenger to the booking
                                                    booking.Descendants("PassengerGroup").First.Add(passenger)

                                                    'Get A List Of All Passenger Tickets
                                                    currentPassengerSegmentsXElements = allSegmentsXElements.Where(Function(x) x.Descendants("Ticket").Attributes("PassengerRef").Any(Function(y) y.Value = passengerRef)).ToList()

                                                    'loop through segments containing ticket for current passenger
                                                    For Each segment In allSegments

                                                        segment.Descendants("Ticket").Remove()
                                                        segment.Descendants("Reservation").Remove()

                                                        For Each ticket In deepCopyOfSegments.Descendants("Ticket").Where(Function(x) x.Attribute("PassengerRef").Value = passengerRef).ToList()

                                                            segment.Descendants("Destination").First.AddAfterSelf(ticket)

                                                        Next

                                                        For Each reservation In deepCopyOfSegments.Descendants("Reservation").Where(Function(x) x.Attribute("PassengerRef").Value = passengerRef).ToList()

                                                            segment.Descendants("TOC").First.AddAfterSelf(reservation)

                                                        Next

                                                        'Add ticket to booking
                                                        booking.Descendants("DeliveryContact").First.AddAfterSelf(segment)

                                                    Next

                                                    currentPassengerSegmentsXElements.Clear()

                                                    'Add the booking top the original hand off
                                                    guardianIOnputFile.Descendants("Agency").First.AddAfterSelf(booking)

                                                    'Split invoice and save new files to location
                                                    SplitInvoice(strFileName, splitCount, guardianIOnputFile, pstrWriteToFolder)

                                                    'Remove each booking from the file to create a generic template for all invoices.
                                                    guardianIOnputFile.Descendants("ImmediateDetail").Remove()

                                                    If successFullSplit = False Then
                                                        successFullSplit = True
                                                    End If
                                                Next
                                                distinctPassengers.Clear()

                                                emailErrorMessage = "Interceptor Guardian - There are no Passengers to split on"

                                                splitCount = 0
                                                'If the invoice should be split on the custom field then
                                            Case "CustomField"
                                                booking.Descendants("Passenger").Remove()
                                                booking.Descendants("Segment").Remove()
                                                'If the split value is on custom field then
                                                For Each customfield In distinctCustomFieldValues

                                                    'increment split count
                                                    splitCount += 1
                                                    'search for all passengers with the unique custom field value
                                                    Dim passengerWithCustomField =
                                                            (From p In deepCopyOfPassengers
                                                            Where _
                                                            p.Descendants("CustomField").Attributes("Label").FirstOrDefault() _
                                                                .Value = strSplitFieldValue AndAlso
                                                            p.Descendants("CustomField").Attributes("Value").Any(
                                                                Function(x) x.Value = customfield))

                                                    For Each passenger In passengerWithCustomField
                                                        allSegmentsXElements = deepCopyOfSegments
                                                        passengerSegmentList = (From tic In allSegmentsXElements Where tic.Descendants("Ticket").Attributes("PassengerRef").First().Value = passenger.Attribute("Ref").Value).ToList()

                                                        'Add the current passenger to the booking
                                                        booking.Descendants("PassengerGroup").First.Add(passenger)
                                                    Next

                                                    distinctpassengerSegmentList.AddRange(passengerSegmentList.Distinct())

                                                    booking.Descendants("Segment").Remove()

                                                    For Each segment In distinctpassengerSegmentList
                                                        ''For each distinct passenger reference find first matching passenger from list of passengers and add to the booking
                                                        'Dim passengerXElement As XElement = (From p In passengersXElements Where p.Attribute("Ref").Value = passengerId).FirstOrDefault()
                                                        'booking.Descendants("PassengerGroup").First.AddAfterSelf(passenger)
                                                        'Find all tickets for current passenger
                                                        Dim passengerSegmentTicketList = (From x In segment.Descendants("Ticket") Where x.Attribute("PassengerRef").Value = passengerWithCustomField.Any(Function(n) n.Attribute("Ref").Value))
                                                        currentPassengerSegmentsXElements.AddRange(passengerSegmentTicketList)

                                                        segment.Descendants("Ticket").Remove()

                                                        'For each ticket belonging to passenger within this booking
                                                        For Each ticket In currentPassengerSegmentsXElements
                                                            'Add ticket to booking
                                                            segment.Descendants("Destination").First.AddAfterSelf(ticket)
                                                        Next

                                                        'Find all reservations belonging to passenger
                                                        Dim passengerSegmentReservations = (From r In segment.Descendants("Reservation") Where r.Attribute("PassengerRef").Value = passengerWithCustomField.Any(Function(n) n.Attribute("Ref").Value))
                                                        passengerReservationsXElements.AddRange(passengerSegmentReservations)

                                                        segment.Descendants("Reservation").Remove()
                                                        'For each reservation belonging to passenger within this booking
                                                        For Each reservation In passengerReservationsXElements
                                                            'Add reservation to booking
                                                            segment.Descendants("TOC").First.AddAfterSelf(reservation)
                                                        Next

                                                        'Add ticket to booking
                                                        booking.Descendants("DeliveryContact").First.AddAfterSelf(segment)

                                                        passengerSegmentTicketList = Nothing
                                                        currentPassengerSegmentsXElements.Clear()
                                                        passengerSegmentReservations = Nothing
                                                        passengerReservationsXElements.Clear()

                                                    Next
                                                    passengerWithCustomField = Nothing
                                                    distinctpassengerSegmentList.Clear()

                                                    guardianIOnputFile.Descendants("Agency").First.AddAfterSelf(booking)
                                                    'split invoice and save new files to location
                                                    SplitInvoice(strFileName, splitCount, guardianIOnputFile, pstrWriteToFolder)
                                                    'Remove passenger node
                                                    booking.Descendants("Passenger").Remove()
                                                    'Remove passenger node
                                                    booking.Descendants("Segment").Remove()
                                                    'Remove passenger node
                                                    booking.Descendants("Reservation").Remove()
                                                    'Remove each booking from the file to create a generic template for all invoices.
                                                    guardianIOnputFile.Descendants("ImmediateDetail").Remove()

                                                    successFullSplit = True
                                                Next

                                                distinctCustomFieldValues = ""
                                                splitCount = 0

                                                If successFullSplit = False Then
                                                    emailErrorMessage = "Interceptor Guardian - There are no unique custom fields to split on"
                                                End If

                                            Case Else

                                                guardianIOnputFile.Descendants("Agency").First.AddAfterSelf(booking)
                                                'split invoice and save new files to location
                                                SplitInvoice(strFileName, intImmediateDetailCount, guardianIOnputFile,
                                                             pstrWriteToFolder)

                                                'Remove each booking from the file to create a generic template for all invoices.
                                                guardianIOnputFile.Descendants("ImmediateDetail").Remove()
                                        End Select

                                        If successFullSplit = False Then

                                            Email(file.Name, strTravellerEmail, strIssueRef, strBossCode, emailErrorMessage)

                                            IO.File.Copy(file.FullName,
                                                  ConfigUtils.getConfig("InterceptorErrorsFolder") &
                                                  file.Name, True)
                                        End If

                                    End If
                                    'clear lists
                                    deepCopyOfSegments.Clear()
                                    deepCopyOfPassengers.Clear()
                                    distinctCustomFieldValues = Nothing
                                    distinctPassengers.Clear()
                                End If
                            Next
                            'clear lists
                            bookings = Nothing
                            bookingsXElements.Clear()
                            'reset count
                            intImmediateDetailCount = 0
                            'delete current file when done
                            IO.File.Delete(file.FullName)
                            'clear lists
                            blnIsDataError = False
                            blnNonIssue = False
                        Else
                            Log.Error("Interceptor Guardian - One of the specified folders does not exist")
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Info("Exception error message " & ex.Message)
            Log.Error("Read Failure - Interceptor Guardian: " & ex.Message)
        End Try
    End Sub

    Public Sub SplitInvoice(strFileName As String, splitCount As Integer, guardianIOnputFile As XElement,
                            pstrWriteToFolder As String)

        Select Case splitCount
            Case 1
                'first file, keep the name the same!
            Case Else
                'work out the file name, need to alter the sequence no
                Dim strFileLetter As String =
                        GetInterceptorFileLetter(splitCount - 1)

                'place the letter at the start of the sequence number
                Dim strNewSequenceNo =
                        Replace(
                            strFileLetter &
                            strFileName.Substring(
                                (strFileName.LastIndexOf("_", StringComparison.Ordinal) + 1) +
                                strFileLetter.Length), ".xml", "")

                strFileName =
                    strFileName.Substring(0,
                                          strFileName.LastIndexOf("_", StringComparison.Ordinal) +
                                          1) & strNewSequenceNo & ".xml"

                guardianIOnputFile.SetAttributeValue("SequenceNumber", strNewSequenceNo)
        End Select

        guardianIOnputFile.Save(pstrWriteToFolder & strFileName)

    End Sub

    Private Function cvtXDocumentToXElement(xDoc As XDocument) As XElement
        Dim xmlOut As XElement = XElement.Parse(xDoc.ToString())
        Return xmlOut
    End Function

    ''' <summary>
    '''     Pass in a number, upper case letter of the alphabet is returned. eg: 0 = "", 1 = "A", 2 = "B"
    '''     For values over 26 double letters are returned.
    '''     For Values over 52 "error" is returned.
    ''' </summary>
    ''' <param name="pintNumber"></param>
    ''' <returns>Sequence letter OR string "error"</returns>
    ''' <remarks></remarks>
    Private Function GetInterceptorFileLetter(pintNumber As Integer) As String
        If pintNumber <= 52 Then

            'Create a collection to hold the letters
            Dim lstLetters(52) As String
            lstLetters(0) = ""

            Dim intTotalLoops = 1
            If pintNumber > 26 Then
                intTotalLoops = 2
            End If

            Dim intSkip = 0
            Dim i = 0
            Dim y = 0

            'loop for number of characters needed
            For y = 1 To intTotalLoops
                intSkip = ((y - 1) * 26)
                'go through all 26 letters
                For i = 1 To 26
                    'add the letter, or if on second loop add an additional letter
                    lstLetters(i + intSkip) += lstLetters(i) & (Convert.ToChar(i + 96)).ToString.ToUpper
                Next
            Next

            Return lstLetters(pintNumber)
        Else
            Return "error"
        End If
    End Function

    'R2.10 CR
    'Private Function EvolviWorkOutFee(ByVal dblTargetTotal As Double, ByVal intNumberToSplit As Integer, ByVal intCurrentStage As Integer, ByRef dblRunningTot As Double)

    '    Dim dblTest As Double = 0

    '    If intNumberToSplit <> intCurrentStage Then
    '        dblTest = Math.Round((dblTargetTotal / intNumberToSplit), 2)
    '    Else
    '        'on last stage
    '        'the value will be the difference between the target total and the running tot
    '        dblTest = dblTargetTotal - dblRunningTot
    '    End If

    '    dblRunningTot += dblTest

    '    Return dblTest
    'End Function

    Public Sub Email(fileName As String, strTravellerEmail As String, strIssueRef As String, travelerGroup As String, strCustomMessage As String)

        Dim message As String

        If strCustomMessage Is Nothing Then

            message = "<br/>Traveller Email in the Handoff file was not found as an " & travelerGroup & ".<br/>"
        Else
            message = strCustomMessage
        End If

        Log.Error(travelerGroup & " not found in file " & fileName & ", email provided was: " & strTravellerEmail)

        'for live version:
        'SendEmail.send("service@nysgroup.com",
        SendEmail.send("Development@nysgroup.com",
     "Development@nysgroup.com",
     "Interceptor - " & travelerGroup & " Not Found",
     "Interceptor has encountered an input error." & message &
     "<p>Traveller Email in handoff file is: " & strTravellerEmail & "<br/>Ticket Issue Ref is: " &
     strIssueRef & "</p><p>File name is: " & fileName & "</p>" &
     "Handoff will be moved to error files folder until corrected, please either chase the client for new traveller details or correct the incorrect email in the file (CustomCode 1)." &
     "<br/>Once corrected please move the handoff file back into the 'awaiting' directory for processing.",
     "mike.kirk@nysgroup.com", "", "")
    End Sub

    Public Sub CubitInvoicesGo()
        CubitInvoiceEmails(1)
        CubitInvoiceEmails(2)
        CubitInvoiceEmails(3)
    End Sub

    'R2.4 CR
    Public Sub CorporateFeedbackGox()
        CheckCorporateFeedback()
    End Sub

    Public Sub CubitInvoiceEmails(ptype As Integer)
        Log.Info("cubitInvoiceEmails start")
        Dim intTest = 1
        Try

            Dim exs As New List(Of clsFeedBookedData)
            If ptype = 1 Then
                exs = clsFeedBookedData.initialInvoiceEmailList(CInt(ConfigUtils.getConfig("InitialDays")))
            ElseIf ptype = 2 Then
                exs = clsFeedBookedData.repeatInvoiceEmailList(CInt(ConfigUtils.getConfig("RepeatDays")))
            ElseIf ptype = 3 Then
                exs = clsFeedBookedData.missingInvoiceEmailCheckandsend
            End If

            intTest = 2
            Dim strCurrentSupplier = ""
            Dim strLastSupplier = ""
            Dim strreason = ""
            Dim stremailto = ""
            Dim strMessage = ""
            Dim intDataId = 0

            'R2.11 SA
            Dim intCheck As Integer
            Dim td As New TransactionData

            For Each ex As clsFeedBookedData In exs
                'R2.11 SA
                intCheck = td.check(ex.BookingId)
                If intCheck > 0 Then

                    'try and match venue name with venuesdb
                    intDataId = ex.Dataid
                    intTest = 3
                    strCurrentSupplier = ex.SupplierName
                    strreason = ""
                    If strLastSupplier = "" Then
                        strLastSupplier = strCurrentSupplier
                    End If

                    If strCurrentSupplier <> strLastSupplier Then
                        'do send
                        If stremailto <> "" Then
                            Dim strFinalMessage As String =
                                    ReadText(ConfigUtils.getConfig("CubitDocsPath") & "InvoiceMain.htm")
                            strFinalMessage = strFinalMessage.Replace("#body#", strMessage)

                            Dim strResult = ""
                            Try
                                SendEmail.send(ConfigUtils.getConfig("CubitEmailFrom"), stremailto,
                                               "Urgent request for invoice/s", strFinalMessage,
                                               "conference3@nysgroup.com", "", "", "")
                                strResult = ""
                            Catch ex1 As Exception
                                Log.Error("ERROR IN cubitInvoiceEmails: " & ex1.Message)
                                strResult = "ERROR IN cubitInvoiceEmails: " & ex1.Message
                            End Try

                            If strResult <> "" Then
                                Dim intRet As Integer = clsFeedBookedData.invoiceEmailsave(ex.Dataid, True, True,
                                                                                           strResult, "")
                            Else
                                Dim intRet As Integer = clsFeedBookedData.invoiceEmailsave(ex.Dataid, False, False, "",
                                                                                           Format(Now, "dd/MM/yyyy"))
                            End If
                        End If
                        stremailto = ""
                        strMessage = ""
                    End If
                    Dim strAlternativeName As String = clsStuff.notString(
                        clsVenue.venueAlternativenameCheck _
                                                                             (strCurrentSupplier))
                    If strAlternativeName <> "" Then
                        strCurrentSupplier = strAlternativeName
                    End If

                    'now go and see if the name exists in VenuesDB if it does bring back values
                    Dim venues As List(Of clsVenue)
                    venues = clsVenue.venueExactNameFind(strCurrentSupplier.Replace("'", "''''"), CStr(0))
                    intTest = 4
                    If venues.Count = 0 Then
                        strreason = "No match to any venue"

                    ElseIf venues.Count = 1 Then
                        strreason = ""
                        For Each venue As clsVenue In venues
                            stremailto = clsFeedBookedData.venueInvoiceEmailGet(CStr(venue.vereference))
                        Next
                        'check email format
                        If stremailto <> "" Then
                            Dim mRegExp As New Regex(SendEmailUtils.emailRegex)
                            If Not mRegExp.IsMatch(stremailto) Then
                                strreason = "Email address in not in the correct format"
                                stremailto = ""
                            End If
                        Else
                            strreason = "No email address present for venue"
                        End If
                    ElseIf venues.Count > 1 Then
                        'see if we can match the ID before we give up
                        Dim strAlternativeRef As String = clsStuff.notString(
                            clsVenue.venueAlternativeRefCheck _
                                                                                (ex.SupplierName))

                        If strAlternativeRef <> "" And strAlternativeRef <> "0" Then
                            For Each venue As clsVenue In venues
                                If venue.vereference = CDbl(strAlternativeRef) Then
                                    stremailto = clsFeedBookedData.venueInvoiceEmailGet(CStr(venue.vereference))
                                    Exit For
                                End If
                            Next
                        End If

                        If stremailto = "" Then
                            'too many matches
                            strreason = "Venue name has matched to more than one venue: refs="
                            For Each venue As clsVenue In venues
                                strreason = strreason & venue.vereference & ";"
                            Next
                        End If
                    End If
                    If strreason <> "" Then
                        'save reason to db record
                        intTest = 5
                        Dim intRet As Integer = clsFeedBookedData.invoiceEmailsave(ex.Dataid, True, True, strreason, "")
                    Else
                        intTest = 5
                        Dim intRet As Integer = clsFeedBookedData.invoiceEmailsave(ex.Dataid, False, False, "",
                                                                                   Format(Now, "dd/MM/yyyy"))
                    End If
                    If stremailto <> "" Then
                        intTest = 7
                        'build the message
                        strMessage = strMessage & ReadText(ConfigUtils.getConfig("CubitDocsPath") & "InvoiceBody.htm")

                        strMessage = strMessage.Replace("#traveller#", ex.LeadPassengerName)
                        strMessage = strMessage.Replace("#venuename#", strCurrentSupplier)
                        strMessage = strMessage.Replace("#arrivaldate#", Format(ex.ArrivalDate, "dd/MM/yyyy"))
                        strMessage = strMessage.Replace("#departuredate#", Format(ex.DepartureDate, "dd/MM/yyyy"))
                        strMessage = strMessage.Replace("#bookingreference#", ex.BookingReference)
                        strMessage = strMessage.Replace("#guestpnr#", ex.GuestPNR)
                        strMessage = strMessage.Replace("#conferma#", CStr(ex.BookingId))
                        strMessage = strMessage.Replace("#lastfour#", ex.Last4Digits)
                        strMessage = strMessage.Replace("#totalcost#", "£" & Format(ex.TotalBilledInGbp, "0.00"))
                        intTest = 8
                    End If
                    strLastSupplier = ex.SupplierName
                End If
            Next

            'need to do a finally for last record
            If exs.Count > 0 Then
                intTest = 9
                If stremailto <> "" Then
                    Dim strFinalMessage As String = ReadText(ConfigUtils.getConfig("CubitDocsPath") & "InvoiceMain.htm")
                    intTest = 10
                    strFinalMessage = strFinalMessage.Replace("#body#", strMessage)
                    intTest = 11
                    Dim strResult = ""
                    Try
                        SendEmail.send(ConfigUtils.getConfig("CubitEmailFrom"), stremailto,
                                       "Urgent request for invoice/s", strFinalMessage, "conference3@nysgroup.com", "",
                                       "", "")
                        strResult = ""
                    Catch ex1 As Exception
                        Log.Error("ERROR IN cubitInvoiceEmails: " & ex1.Message)
                        strResult = "ERROR IN cubitInvoiceEmails: " & ex1.Message
                    End Try

                    If strResult <> "" Then
                        Dim intRet As Integer = clsFeedBookedData.invoiceEmailsave(intDataId, True, True, strResult, "")
                    Else
                        Dim intRet As Integer = clsFeedBookedData.invoiceEmailsave(intDataId, False, False, "",
                                                                                   Format(Now, "dd/MM/yyyy"))
                    End If
                End If
            End If

        Catch ex As Exception
            Log.Error("CUBIT ERROR: " & intTest & "-" & ex.Message)
        End Try
    End Sub

    Public Shared Function ReadText(pstrfile As String) As String
        Dim ofile As New StreamReader(pstrfile)
        ReadText = ofile.ReadToEnd
        ofile.Close()
    End Function

    Private Structure GetAccomret
        Public Table As DataTable

        Public DblTotalVatall,
               DblTotalNettall,
               DblTotalVatnotinc,
               DblTotalNettnotinc,
               Dblpaidcost,
               Dblpaidvat,
               Dblpaiddeleinccost,
               Dblpaiddeleincvat As Double
    End Structure

    Private Function GetAccomDetailsTable(pintenquiryvenueid As Integer) As GetAccomret

        Dim dblTotalCost As Double = 0
        Dim dblTotalVat As Double = 0
        Dim intcount = 0
        Dim ret As New GetAccomret
        Dim oAccom As List(Of clsAccommodationDat)

        oAccom = clsAccommodationDat.Populate(0, pintenquiryvenueid)

        For Each ac As clsAccommodationDat In oAccom
            Dim accommodationrooms = CInt(ac.Accommodationrooms)
            Dim accommodationnights = CInt(ac.Accommodationnights)
            Dim accommodationratenet = CDbl(ac.Accommodationratenet)
            Dim accommodationvat = CDbl(ac.AccommodationVAT)
            Dim accommodationdeleinc = CBool(ac.Accommodationdeleinc)

            If accommodationdeleinc = False Then
                ret.DblTotalNettnotinc = ret.DblTotalNettnotinc +
                                         ((accommodationratenet * accommodationrooms) * accommodationnights)
                ret.DblTotalVatnotinc = ret.DblTotalVatnotinc +
                                        ((accommodationvat * accommodationrooms) * accommodationnights)
            End If
            ret.DblTotalNettall = ret.DblTotalNettall + ((accommodationratenet * accommodationrooms) * accommodationnights)
            ret.DblTotalVatall = ret.DblTotalVatall + ((accommodationvat * accommodationrooms) * accommodationnights)
            If CBool(ac.Accommodationpaiddirect) Then
                If accommodationdeleinc Then
                    ret.Dblpaiddeleinccost = ret.Dblpaiddeleinccost +
                                             ((accommodationratenet * accommodationrooms) * accommodationnights)
                    ret.Dblpaiddeleincvat = ret.Dblpaiddeleincvat +
                                            ((accommodationvat * accommodationrooms) * accommodationnights)
                Else
                    ret.Dblpaidcost = ret.Dblpaidcost + ((accommodationratenet * accommodationrooms) * accommodationnights)
                    ret.Dblpaidvat = ret.Dblpaidvat + ((accommodationvat * accommodationrooms) * accommodationnights)
                End If
            End If
            intcount = intcount + 1
        Next

        Return ret
    End Function

    Private Structure GetCateringret
        Public Table As DataTable

        Public DblTotalVatall,
               DblTotalNettall,
               DblTotalVatnotinc,
               DblTotalNettnotinc,
               Dblpaidcost,
               Dblpaidvat,
               Dblpaiddeleinccost,
               Dblpaiddeleincvat As Double
    End Structure

    Private Function GetCateringDetailsTable(pintenquiryvenueid As Integer) As GetCateringret

        Dim dblTotalCost, dblTotalVat As Double
        Dim oCateringlist As New clsCateringDat
        Dim strtime1, strtime2, strtime3, strtime4 As String
        Dim intservings = 0
        Dim ret As New GetCateringret

        oCateringlist.Populate(0, pintenquiryvenueid)
        dblTotalCost = 0
        dblTotalVat = 0

        For Each oCateringlist In oCateringlist.mcolCatering
            Dim intcateringdelegates As Integer = oCateringlist.mintcateringdelegates
            Dim intcateringtime1Hour As Integer = oCateringlist.mintcateringtime1hour
            Dim intcateringtime1Min As Integer = oCateringlist.mintcateringtime1min
            Dim intcateringtime2Hour As Integer = oCateringlist.mintcateringtime2hour
            Dim intcateringtime2Min As Integer = oCateringlist.mintcateringtime2min
            Dim intcateringtime3Hour As Integer = oCateringlist.mintcateringtime3hour
            Dim intcateringtime3Min As Integer = oCateringlist.mintcateringtime3min
            Dim intcateringtime4Hour As Integer = oCateringlist.mintcateringtime4hour
            Dim intcateringtime4Min As Integer = oCateringlist.mintcateringtime4min

            intservings = 0

            If oCateringlist.mintcateringtime1hour = -2 Then
                strtime1 = "unltd"
                strtime2 = "--"
                strtime3 = "--"
                strtime4 = "--"
                intservings = 1
            Else
                If intcateringtime1Hour > -1 And intcateringtime1Min > -1 Then
                    intservings = 1
                End If
                If intcateringtime2Hour > -1 And intcateringtime2Min > -1 Then
                    intservings = intservings + 1
                End If
                If intcateringtime3Hour > -1 And intcateringtime3Min > -1 Then
                    intservings = intservings + 1
                End If
                If intcateringtime4Hour > -1 And intcateringtime4Min > -1 Then
                    intservings = intservings + 1
                End If
            End If
            If intservings = 0 Then
                intservings = 1
            End If
            Dim dblcateringdays = CDbl(oCateringlist.mstrcateringdays)
            Dim dblcateringratenet As Double = oCateringlist.mdblcateringratenet
            Dim dblcateringvat As Double = oCateringlist.mdblcateringVAT
            Dim bcateringdeleinc As Boolean = oCateringlist.mbcateringdeleinc

            If intservings = 0 Then
                intservings = 1
            End If
            If bcateringdeleinc = False Then
                If oCateringlist.mstrdurationtype = "days" Then
                    ret.DblTotalNettnotinc = ret.DblTotalNettnotinc + (((dblcateringratenet * intservings) *
                                                                        intcateringdelegates) * dblcateringdays)
                    ret.DblTotalVatnotinc = ret.DblTotalVatnotinc + (((dblcateringvat * intservings) *
                                                                      intcateringdelegates) * dblcateringdays)
                Else
                    ret.DblTotalNettnotinc = ret.DblTotalNettnotinc + ((dblcateringratenet * intservings) *
                                                                       intcateringdelegates)
                    ret.DblTotalVatnotinc = ret.DblTotalVatnotinc + ((dblcateringvat * intservings) *
                                                                     intcateringdelegates)
                End If
            End If
            If oCateringlist.mstrdurationtype = "days" Then
                ret.DblTotalNettall = ret.DblTotalNettall + (((dblcateringratenet * intservings) *
                                                              intcateringdelegates) * dblcateringdays)
                ret.DblTotalVatall = ret.DblTotalVatall + (((dblcateringvat * intservings) *
                                                            intcateringdelegates) * dblcateringdays)
            Else
                ret.DblTotalNettall = ret.DblTotalNettall + ((dblcateringratenet * intservings) *
                                                             intcateringdelegates)
                ret.DblTotalVatall = ret.DblTotalVatall + ((dblcateringvat * intservings) *
                                                           intcateringdelegates)
            End If
            If oCateringlist.mbcateringpaiddirect = True Then
                If bcateringdeleinc Then
                    If oCateringlist.mstrdurationtype = "days" Then
                        ret.Dblpaiddeleinccost = ret.Dblpaiddeleinccost + (((dblcateringratenet * intservings) *
                                                                            intcateringdelegates) * dblcateringdays)
                        ret.Dblpaiddeleincvat = ret.Dblpaiddeleincvat + (((dblcateringvat * intservings) *
                                                                          intcateringdelegates) * dblcateringdays)
                    Else
                        ret.Dblpaiddeleinccost = ret.Dblpaiddeleinccost + ((dblcateringratenet * intservings) *
                                                                           intcateringdelegates)
                        ret.Dblpaiddeleincvat = ret.Dblpaiddeleincvat + ((dblcateringvat * intservings) *
                                                                         intcateringdelegates)
                    End If
                Else
                    If oCateringlist.mstrdurationtype = "days" Then
                        ret.Dblpaidcost = ret.Dblpaidcost + (((dblcateringratenet * intservings) *
                                                              intcateringdelegates) * dblcateringdays)
                        ret.Dblpaidvat = ret.Dblpaidvat + (((dblcateringvat * intservings) *
                                                            intcateringdelegates) * dblcateringdays)
                    Else
                        ret.Dblpaidcost = ret.Dblpaidcost + ((dblcateringratenet * intservings) *
                                                             intcateringdelegates)
                        ret.Dblpaidvat = ret.Dblpaidvat + ((dblcateringvat * intservings) *
                                                           intcateringdelegates)
                    End If
                End If
            End If
        Next

        Return ret
    End Function

    Private Structure GetMeetingRoomsret
        Public Table As DataTable

        Public DblTotalVatall,
               DblTotalNettall,
               DblTotalVatnotinc,
               DblTotalNettnotinc,
               Dblpaidcost,
               Dblpaidvat,
               Dblpaiddeleinccost,
               Dblpaiddeleincvat As Double
    End Structure

    Private Function GetMeetingRoomsDetailsTable(pintenquiryvenueid As Integer) As GetMeetingRoomsret

        Dim omls As New clsMeetingRoomsDat
        Dim dblTotalCost, dblTotalVenueCost, dblTotalVat, dblTotalVenueVat As Double
        Dim ret As New GetMeetingRoomsret

        omls.Populate(0, pintenquiryvenueid)
        dblTotalCost = 0
        dblTotalVat = 0
        dblTotalVenueCost = 0
        dblTotalVenueVat = 0

        Dim strfromhour, strfrommin, strtohour, strtomin As String
        Dim retvalue As GetMeetingRoomssetupret

        For Each ml As clsMeetingRoomsDat In omls.mcolMeetingRooms

            retvalue = New GetMeetingRoomssetupret
            retvalue = PopulateMeetingRoomssetup(0, ml.mintmeetingroomsid)

            Dim intsetup As Integer = retvalue.Intid

            If ml.mintmeetingroomsfromtimehour < 10 Then
                strfromhour = "0" & CStr(ml.mintmeetingroomsfromtimehour)
            Else
                strfromhour = CStr(ml.mintmeetingroomsfromtimehour)
            End If
            If ml.mintmeetingroomsfromtimemin < 10 Then
                strfrommin = "0" & CStr(ml.mintmeetingroomsfromtimemin)
            Else
                strfrommin = CStr(ml.mintmeetingroomsfromtimemin)
            End If
            If ml.mintmeetingroomstotimehour < 10 Then
                strtohour = "0" & CStr(ml.mintmeetingroomstotimehour)
            Else
                strtohour = CStr(ml.mintmeetingroomstotimehour)
            End If
            If ml.mintmeetingroomstotimemin < 10 Then
                strtomin = "0" & CStr(ml.mintmeetingroomstotimemin)
            Else
                strtomin = CStr(ml.mintmeetingroomstotimemin)
            End If

            If Not ml.mbmeetingroomsdeleinc Then
                If ml.mstrmeetingroomsdurationtype = "days" Then
                    ret.DblTotalNettnotinc = ret.DblTotalNettnotinc +
                                             (ml.mdblmeetingroomsratenet * CDbl(ml.mstrmeetingroomsdays))
                    ret.DblTotalVatnotinc = ret.DblTotalVatnotinc +
                                            (ml.mdblmeetingroomsVAT * CDbl(ml.mstrmeetingroomsdays))
                Else
                    ret.DblTotalNettnotinc = ret.DblTotalNettnotinc + ml.mdblmeetingroomsratenet 'total venue cost
                    ret.DblTotalVatnotinc = ret.DblTotalVatnotinc + ml.mdblmeetingroomsVAT
                End If
            End If
            If ml.mstrmeetingroomsdurationtype = "days" Then
                ret.DblTotalNettall = ret.DblTotalNettall + (ml.mdblmeetingroomsratenet * CDbl(ml.mstrmeetingroomsdays))
                ret.DblTotalVatall = ret.DblTotalVatall + (ml.mdblmeetingroomsVAT * CDbl(ml.mstrmeetingroomsdays))
                If ml.mbmeetingroomspaiddirect = True Then
                    If ml.mbmeetingroomsdeleinc Then
                        ret.Dblpaiddeleinccost = ret.Dblpaiddeleinccost +
                                                 (ml.mdblmeetingroomsratenet * CDbl(ml.mstrmeetingroomsdays))
                        ret.Dblpaiddeleincvat = ret.Dblpaiddeleincvat +
                                                (ml.mdblmeetingroomsVAT * CDbl(ml.mstrmeetingroomsdays))
                    Else
                        ret.Dblpaidcost = ret.Dblpaidcost + (ml.mdblmeetingroomsratenet * CDbl(ml.mstrmeetingroomsdays))
                        ret.Dblpaidvat = ret.Dblpaidvat + (ml.mdblmeetingroomsVAT * CDbl(ml.mstrmeetingroomsdays))
                    End If
                End If
            Else
                ret.DblTotalNettall = ret.DblTotalNettall + ml.mdblmeetingroomsratenet 'total venue cost
                ret.DblTotalVatall = ret.DblTotalVatall + ml.mdblmeetingroomsVAT
                If ml.mbmeetingroomspaiddirect = True Then
                    If ml.mbmeetingroomsdeleinc Then
                        ret.Dblpaiddeleinccost = ret.Dblpaiddeleinccost + ml.mdblmeetingroomsratenet
                        ret.Dblpaiddeleincvat = ret.Dblpaiddeleincvat + ml.mdblmeetingroomsVAT
                    Else
                        ret.Dblpaidcost = ret.Dblpaidcost + ml.mdblmeetingroomsratenet
                        ret.Dblpaidvat = ret.Dblpaidvat + ml.mdblmeetingroomsVAT
                    End If
                End If
            End If
            'now add setup costs too
            ret.DblTotalNettall = ret.DblTotalNettall + retvalue.DblTotalNettall
            ret.DblTotalVatall = ret.DblTotalVatall + retvalue.DblTotalVatall
            ret.DblTotalNettnotinc = ret.DblTotalNettnotinc + retvalue.DblTotalNettall
            ret.DblTotalVatnotinc = ret.DblTotalVatnotinc + retvalue.DblTotalVatall
        Next

        Return ret
    End Function

    Private Structure GetMeetingRoomssetupret
        Public DblTotalVatall,
               DblTotalNettall,
               DblTotalVatnotinc,
               DblTotalNettnotinc,
               Dblpaidcost,
               Dblpaidvat As Double

        Public Intid As Integer
    End Structure

    Private Function PopulateMeetingRoomssetup(pintmeetingroomssetupid As Integer,
                                               pintmeetingroomsid As Integer) As GetMeetingRoomssetupret

        Dim oMeetinglist As New clsMeetingRoomsDat
        Dim ret As New GetMeetingRoomssetupret

        oMeetinglist.Populatesetup(pintmeetingroomssetupid, pintmeetingroomsid)

        ret.Intid = oMeetinglist.mintmeetingroomssetupid
        ret.DblTotalNettall = oMeetinglist.mdblmeetingroomssetupratenet
        ret.DblTotalVatall = oMeetinglist.mdblmeetingroomssetupVAT

        Return ret
    End Function

    Private Structure GetEquipmentret
        Public Table As DataTable

        Public DblTotalVatall,
               DblTotalNettall,
               DblTotalVatnotinc,
               DblTotalNettnotinc,
               Dblpaidcost,
               Dblpaidvat,
               Dblpaiddeleinccost,
               Dblpaiddeleincvat As Double
    End Structure

    Private Function GetEquipmentDetailsTable(pintenquiryvenueid As Integer) As GetEquipmentret

        Dim oEquipment As New clsEquipmentDat
        Dim colEquipment As New Collection
        Dim ret As New GetEquipmentret

        colEquipment = oEquipment.Populate(0, pintenquiryvenueid)

        For Each oEquipment In oEquipment.mcolEquipment
            Dim intequipmentqty As Integer = oEquipment.mintequipmentqty
            Dim dblequipmentratenet As Double = oEquipment.mdblequipmentratenet
            Dim dblequipmentvat As Double = oEquipment.mdblequipmentVAT
            Dim bequipmentdeleinc As Boolean = oEquipment.mbequipmentdeleinc
            Dim strdurationtype As String = oEquipment.mstrequipmentdurationtype

            If bequipmentdeleinc = False Then
                If strdurationtype = "days" Then
                    ret.DblTotalNettnotinc = ret.DblTotalNettnotinc +
                                             ((dblequipmentratenet * intequipmentqty) * CDbl(oEquipment.mstrequipmentdays))
                    ret.DblTotalVatnotinc = ret.DblTotalVatnotinc +
                                            ((dblequipmentvat * intequipmentqty) * CDbl(oEquipment.mstrequipmentdays))
                Else
                    ret.DblTotalNettnotinc = ret.DblTotalNettnotinc + (dblequipmentratenet * intequipmentqty)
                    ret.DblTotalVatnotinc = ret.DblTotalVatnotinc + (dblequipmentvat * intequipmentqty)
                End If
            End If
            If strdurationtype = "days" Then
                ret.DblTotalNettall = ret.DblTotalNettall +
                                      ((dblequipmentratenet * intequipmentqty) * CDbl(oEquipment.mstrequipmentdays))
                ret.DblTotalVatall = ret.DblTotalVatall +
                                     ((dblequipmentvat * intequipmentqty) * CDbl(oEquipment.mstrequipmentdays))
            Else
                ret.DblTotalNettall = ret.DblTotalNettall + (dblequipmentratenet * intequipmentqty)
                ret.DblTotalVatall = ret.DblTotalVatall + (dblequipmentvat * intequipmentqty)
            End If
            If oEquipment.mbequipmentpaiddirect = True Then
                If bequipmentdeleinc Then
                    If strdurationtype = "days" Then
                        ret.Dblpaiddeleinccost = ret.Dblpaiddeleinccost +
                                                 ((dblequipmentratenet * intequipmentqty) *
                                                  CDbl(oEquipment.mstrequipmentdays))
                        ret.Dblpaiddeleincvat = ret.Dblpaiddeleincvat +
                                                ((dblequipmentvat * intequipmentqty) * CDbl(oEquipment.mstrequipmentdays))
                    Else
                        ret.Dblpaidcost = ret.Dblpaidcost + (dblequipmentratenet * intequipmentqty)
                        ret.Dblpaidvat = ret.Dblpaidvat + (dblequipmentvat * intequipmentqty)
                    End If
                Else
                    If strdurationtype = "days" Then
                        ret.Dblpaidcost = ret.Dblpaidcost +
                                          ((dblequipmentratenet * intequipmentqty) * CDbl(oEquipment.mstrequipmentdays))
                        ret.Dblpaidvat = ret.Dblpaidvat +
                                         ((dblequipmentvat * intequipmentqty) * CDbl(oEquipment.mstrequipmentdays))
                    Else
                        ret.Dblpaidcost = ret.Dblpaidcost + (dblequipmentratenet * intequipmentqty)
                        ret.Dblpaidvat = ret.Dblpaidvat + (dblequipmentvat * intequipmentqty)
                    End If
                End If
            End If
        Next

        Return ret
    End Function

    Private Structure GetDdRret
        Public DblTotalVat, DblTotalNett, Dblpaidcost, Dblpaidvat As Double
    End Structure

    Private Function Populateddr(pintenquiryvenueid As Integer) As GetDdRret

        Dim oDdRlist As New clsDDRDat
        Dim colDdr As New Collection
        Dim dblTotalNett, dblTotalVat, dblpaidcost, dblpaidvat As Double
        Dim ret As New GetDdRret

        colDdr = oDdRlist.Populate(0, pintenquiryvenueid)

        For Each oDdRlist In oDdRlist.mcolDDR
            Dim intddrdelegates As Integer = oDdRlist.mintddrdelegates
            Dim dblddrratenet As Double = oDdRlist.mdblddrratenet
            Dim dblddrvat As Double = oDdRlist.mdblddrVAT

            If oDdRlist.mstrdurationtype = "days" Then
                dblTotalNett = dblTotalNett + ((intddrdelegates * dblddrratenet) * CDbl(oDdRlist.mstrddrdays))
                dblTotalVat = dblTotalVat + ((intddrdelegates * dblddrvat) * CDbl(oDdRlist.mstrddrdays))
                If oDdRlist.mbddrpaiddirect = True Then
                    dblpaidcost = dblpaidcost + ((intddrdelegates * dblddrratenet) * CDbl(oDdRlist.mstrddrdays))
                    dblpaidvat = dblpaidvat + ((intddrdelegates * dblddrvat) * CDbl(oDdRlist.mstrddrdays))
                End If
            Else
                dblTotalNett = dblTotalNett + (intddrdelegates * dblddrratenet)
                dblTotalVat = dblTotalVat + (intddrdelegates * dblddrvat)
                If oDdRlist.mbddrpaiddirect = True Then
                    dblpaidcost = dblpaidcost + (intddrdelegates * dblddrratenet)
                    dblpaidvat = dblpaidvat + (intddrdelegates * dblddrratenet)
                End If
            End If
        Next

        ret.DblTotalNett = dblTotalNett
        ret.DblTotalVat = dblTotalVat
        ret.Dblpaidcost = dblpaidcost
        ret.Dblpaidvat = dblpaidvat

        Return ret
    End Function

    Private Structure GetOtherret
        Public DblTotalVat, DblTotalNett, Dblpaidcost, Dblpaidvat As Double
    End Structure

    Private Function PopulateOther(pintenquiryvenueid As Integer) As GetOtherret

        Dim oOther As New clsOtherDat
        Dim colOther As New Collection
        Dim dblTotalCost As Double = 0
        Dim dblTotalVat As Double = 0
        Dim dblpaidcost As Double = 0
        Dim dblpaidvat As Double = 0
        Dim ret As New GetOtherret

        colOther = oOther.Populate(0, pintenquiryvenueid)

        For Each oOther In oOther.mcolother

            'R2.10 NM
            'Need to check for AC type and if booking is NOT GBP so will need to convert to make booking value stored in database correct
            Dim localNet = CDec(oOther.mdblotherratenet * oOther.mintotherqty)
            Dim localVat = CDec(oOther.mdblotherVAT * oOther.mintotherqty)

            If oOther.mstrothercosttype.ToUpper = "AC" Then
                Dim oVenues As New clsEnquiryVenueDat
                Dim oVenue As New clsEnquiryVenueDat
                oVenues.Populate(0, pintenquiryvenueid)
                Dim dblrate As Decimal = 1
                Dim currency = ""
                For Each oVenue In oVenues.mcolEnquiryVenue
                    Try
                        dblrate = CDec(oVenue.mdblvenueEXRate)
                    Catch ex As Exception
                        dblrate = 1
                    End Try

                    currency = oVenue.mstrVenuecurrency

                    If currency.ToUpper <> "GBP" And currency <> "" Then
                        If dblrate = 1 Or dblrate = 0 Then
                            dblrate = CDec(getrate(currency))
                        End If
                    End If
                Next
                localNet = localNet * dblrate
                localVat = localVat * dblrate
            End If

            If oOther.mbotherquote = True Then
                dblTotalCost = dblTotalCost + localNet
                dblTotalVat = dblTotalVat + localVat
            End If
            If oOther.mbotherpaiddirect = True Then
                dblpaidcost = dblpaidcost + localNet
                dblpaidvat = dblpaidvat + localVat
            End If
        Next

        ret.DblTotalNett = dblTotalCost
        ret.DblTotalVat = dblTotalVat
        ret.Dblpaidcost = dblpaidcost
        ret.Dblpaidvat = dblpaidvat

        Return ret
    End Function

    'This method is to handle if element is missing <System.Runtime.CompilerServices.Extension> _
    Public Shared Function ElementValueNull(element As XElement) As String
        If element IsNot Nothing Then
            Return element.Value
        End If

        Return ""
    End Function

    'This method is to handle if attribute is missing <System.Runtime.CompilerServices.Extension> _
    Public Shared Function AttributeValueNull(element As XElement, attributeName As String) As String
        If element Is Nothing Then
            Return ""
        Else
            Dim attr As XAttribute = element.Attribute(attributeName)
            Return If(attr Is Nothing, "", attr.Value)
        End If
    End Function

    Private Sub UpdateVenueCosts(pintEnquiryVenueId As Integer, pblnType As Boolean, pblnCanReclaimVat As Boolean)
        'if pblnType = true then DDR else RC
        Dim retvalues As GetAccomret = GetAccomDetailsTable(pintEnquiryVenueId)
        Dim dblaccomall As Double = retvalues.DblTotalNettall
        Dim dblaccomnotinc As Double = retvalues.DblTotalNettnotinc
        Dim dblaccomvatall As Double = retvalues.DblTotalVatall
        Dim dblaccomvatnotinc As Double = retvalues.DblTotalVatnotinc
        Dim dblaccompaidvat As Double = retvalues.Dblpaidvat
        Dim dblaccompaidcost As Double = retvalues.Dblpaidcost
        'get catering values
        Dim retcatvalues As GetCateringret = GetCateringDetailsTable(pintEnquiryVenueId)
        Dim dblcatall As Double = retcatvalues.DblTotalNettall
        Dim dblcatnotinc As Double = retcatvalues.DblTotalNettnotinc
        Dim dblcatvatall As Double = retcatvalues.DblTotalVatall
        Dim dblcatvatnotinc As Double = retcatvalues.DblTotalVatnotinc
        Dim dblcatpaidvat As Double = retcatvalues.Dblpaidvat
        Dim dblcatpaidcost As Double = retcatvalues.Dblpaidcost
        'now meeting rooms
        Dim retmeetingvalues As GetMeetingRoomsret = GetMeetingRoomsDetailsTable(pintEnquiryVenueId)
        Dim dblmeetingall As Double = retmeetingvalues.DblTotalNettall
        Dim dblmeetingnotinc As Double = retmeetingvalues.DblTotalNettnotinc
        Dim dblmeetingvatall As Double = retmeetingvalues.DblTotalVatall
        Dim dblmeetingvatnotinc As Double = retmeetingvalues.DblTotalVatnotinc
        Dim dblmeetingpaidvat As Double = retmeetingvalues.Dblpaidvat
        Dim dblmeetingpaidcost As Double = retmeetingvalues.Dblpaidcost

        ''now equipment
        Dim retequipvalues As GetEquipmentret = GetEquipmentDetailsTable(pintEnquiryVenueId)
        Dim dblequipall As Double = retequipvalues.DblTotalNettall
        Dim dblequipnotinc As Double = retequipvalues.DblTotalNettnotinc
        Dim dblequipvatall As Double = retequipvalues.DblTotalVatall
        Dim dblequipvatnotinc As Double = retequipvalues.DblTotalVatnotinc
        Dim dblequipmentpaidvat As Double = retequipvalues.Dblpaidvat
        Dim dblequipmentpaidcost As Double = retequipvalues.Dblpaidcost

        Dim retddrvalues As GetDdRret = Populateddr(pintEnquiryVenueId)
        Dim dblddrnett As Double = retddrvalues.DblTotalNett
        Dim dblddrvat As Double = retddrvalues.DblTotalVat
        Dim dblddrpaidnett As Double = retddrvalues.Dblpaidcost
        Dim dblddrpaidvat As Double = retddrvalues.Dblpaidvat

        Dim retothervalues As GetOtherret = PopulateOther(pintEnquiryVenueId)
        Dim dblothernett As Double = retddrvalues.DblTotalNett
        Dim dblothervat As Double = retddrvalues.DblTotalVat
        Dim dblotherpaidnett As Double = retddrvalues.Dblpaidcost
        Dim dblotherpaidvat As Double = retddrvalues.Dblpaidvat

        Dim dblrc As Double = dblcatall + dblaccomall + dblmeetingall + dblequipall + dblothernett

        Dim dblddr As Double = dblddrnett + dblaccomnotinc +
                               dblcatnotinc + dblmeetingnotinc + dblequipnotinc + dblothernett

        Dim odat As New clsEnquiryVenueDat
        Dim enquirypaidnett As Double = 0
        Dim enquirypaidvat As Double = 0
        If pblnType Then
            enquirypaidnett = dblddrpaidnett +
                              dblotherpaidnett +
                              retvalues.Dblpaidcost +
                              retcatvalues.Dblpaidcost +
                              retmeetingvalues.Dblpaidcost +
                              retequipvalues.Dblpaidcost
            If Not pblnCanReclaimVat Then
                enquirypaidvat = dblddrpaidvat +
                                 dblotherpaidvat +
                                 retvalues.Dblpaidvat +
                                 retcatvalues.Dblpaidvat +
                                 retmeetingvalues.Dblpaidvat +
                                 retequipvalues.Dblpaidvat
            Else
                enquirypaidvat = 0
            End If
        Else
            enquirypaidnett = dblotherpaidnett +
                              (retvalues.Dblpaidcost + retvalues.Dblpaiddeleinccost) +
                              (retcatvalues.Dblpaidcost + retcatvalues.Dblpaiddeleinccost) +
                              (retmeetingvalues.Dblpaidcost + retmeetingvalues.Dblpaiddeleinccost) +
                              (retequipvalues.Dblpaidcost + retequipvalues.Dblpaiddeleinccost)
            If Not pblnCanReclaimVat Then
                enquirypaidvat = dblotherpaidvat +
                                 (retvalues.Dblpaidvat + retvalues.Dblpaiddeleincvat) +
                                 (retcatvalues.Dblpaidvat + retcatvalues.Dblpaiddeleincvat) +
                                 (retmeetingvalues.Dblpaidvat + retmeetingvalues.Dblpaiddeleincvat) +
                                 (retequipvalues.Dblpaidvat + retequipvalues.Dblpaiddeleincvat)
            Else
                enquirypaidvat = 0
            End If
        End If
        odat.saveCosts(pintEnquiryVenueId,
                       (dblddrnett + dblaccomnotinc + dblcatnotinc + dblequipnotinc + dblmeetingnotinc + dblothernett),
                       (dblaccomvatnotinc + dblcatvatnotinc + dblequipvatnotinc + dblmeetingvatnotinc + dblddrvat +
                        dblothervat),
                       (dblaccomall + dblcatall + dblequipall + dblmeetingall + dblothernett),
                       (dblaccomvatall + dblcatvatall + dblequipvatall + dblmeetingvatall + dblothervat),
                       enquirypaidnett,
                       enquirypaidvat)
        odat = Nothing
    End Sub

    Public Sub GetSsoDetails()
        Try
            'first get all the clients
            Log.Info("SSO Client Sync Started")
            Dim tabClients As DataTable
            Dim oWebClients As New srvEmail

            tabClients = oWebClients.getClientData("googlehammer")

            For Each rowClient As DataRow In tabClients.Rows
                Dim _
                    oClient As _
                        New SsoClient(ClsNys.notInteger(rowClient(0)), ClsNys.notString(rowClient(1)),
                                      ClsNys.notString(rowClient(2)), ClsNys.notString(rowClient(3)),
                                      ClsNys.notString(rowClient(4)),
                                      ClsNys.notString(rowClient(5)), ClsNys.notString(rowClient(6)),
                                      ClsNys.notString(rowClient(7)), ClsNys.notString(rowClient(8)),
                                      ClsNys.notString(rowClient(9)),
                                      ClsNys.notString(rowClient(10)), ClsNys.notInteger(rowClient(11)),
                                      ClsNys.notString(rowClient(12)), ClsNys.notString(rowClient(13)),
                                      ClsNys.notBoolean(rowClient(14)))

                Dim iRetId As Integer = oClient.save()
                If iRetId <> ClsNys.notInteger(rowClient(0)) Then
                    SendEmail.send("service@nysgroup.com", "nick.massarella@nysgroup.com", "SSO Database",
                                   "SSO Database is out of sync, do a back up and restore", "mike.kirk@nysgroup.com", "",
                                   "", "")
                    Exit Sub
                End If
            Next
            Log.Info("SSO Client Sync Complete")

            'now go get all users for all clients
            Log.Info("SSO User Sync Started")
            Dim tabUsers As DataTable
            Dim oWebUsers As New srvEmail
            tabUsers = oWebUsers.getUserData(0, "googlehammer")

            For Each rowUser As DataRow In tabUsers.Rows
                Dim _
                    oUser As _
                        New SsoUser(ClsNys.notInteger(rowUser(0)), ClsNys.notString(rowUser(1)),
                                    ClsNys.notString(rowUser(2)), ClsNys.notString(rowUser(3)),
                                    ClsNys.notString(rowUser(4)),
                                    ClsNys.notBoolean(rowUser(5)), ClsNys.notBoolean(rowUser(6)),
                                    ClsNys.notBoolean(rowUser(7)), ClsNys.notBoolean(rowUser(8)),
                                    ClsNys.notBoolean(rowUser(9)),
                                    ClsNys.notInteger(rowUser(10)), ClsNys.notBoolean(rowUser(11)),
                                    ClsNys.notBoolean(rowUser(12)), ClsNys.notString(rowUser(13)),
                                    ClsNys.notString(rowUser(14)), ClsNys.notString(rowUser(15)),
                                    ClsNys.notString(rowUser(16)), ClsNys.notString(rowUser(17)),
                                    ClsNys.notString(rowUser(18)),
                                    ClsNys.notString(rowUser(19)), ClsNys.notString(rowUser(20)),
                                    ClsNys.notString(rowUser(21)), ClsNys.notBoolean(rowUser(22)),
                                    ClsNys.notBoolean(rowUser(23)), ClsNys.notBoolean(rowUser(24)),
                                    ClsNys.notBoolean(rowUser(25)), ClsNys.notBoolean(rowUser(26)))
                oUser.save()
            Next
            Log.Info("SSO User Sync Complete")

            'R2.21 CR
            'now go get all codes in search list
            Log.Info("SSO CodeList Sync Started")
            Dim tabCodes As DataTable
            Dim oWebCodes As New srvEmail
            tabCodes = oWebCodes.getCodeList("googlehammer")

            'R2.22.2 CR BUG FIX
            'delete all the codes we are going to sync from the DB first - this is to fix the duplicate codes issue
            For Each rowCode As DataRow In tabCodes.Rows
                SsoCodeList.delete(ClsNys.notString(rowCode(1)))
            Next

            For Each rowCode As DataRow In tabCodes.Rows
                Dim _
                    oSsoCode As _
                        New SsoCodeList(ClsNys.notInteger(rowCode(0)), ClsNys.notString(rowCode(1)),
                                        ClsNys.notString(rowCode(2)),
                                        ClsNys.notString(rowCode(3)), ClsNys.notInteger(rowCode(4)),
                                        ClsNys.notBoolean(rowCode(5)))
                oSsoCode.saveSync()
            Next
            Log.Info("SSO CodeList Sync Complete")

        Catch ex As Exception
            Log.Error("SSO Sync Error - " & ex.Message)
        End Try
    End Sub

End Class

Public Class InterceptorClientFeeDetails
    Public Property DblTotalFee As String

    Public Property DblTotalPerOrderItem As String

    Public Property DblTotalPerPax As String

    Public Property DblTotalPerTicket As String
End Class

Public Class InterceptorChargedetails
    Public Property StrIssueRef As Decimal

    Public Property IntTotalTickets As Integer
    Public Property TransactionFee As Decimal
    Public Property TransactionFeeTotal As Decimal
    Public Property PostageFee As Decimal
    Public Property PostageFeeTotal As Decimal
    Public Property OfflineFee As Decimal
    Public Property OfflineFeeTotal As Decimal
    Public Property SpecialDeliveryFee As Decimal
    Public Property SpecialDeliveryFeeTotal As Decimal
    Public Property CancellationFee As Decimal
    Public Property CancellationFeeTotal As Decimal
    Public Property IssueType As String
    Public Property IssueDate As String
    Public Property StrBossCode As String
    Public Property ToDFee As Decimal
    Public Property ToDFeeTotal As Decimal
    Public Property KioskFee As Decimal
    Public Property KioskFeeTotal As Decimal
    Public Property SaturdaySdFee As Decimal
    Public Property SaturdaySdFeeTotal As Decimal
    Public Property CourierFee As Decimal
    Public Property CourierFeeTotal As Decimal
    Public Property PlainPaperTicketingFee As Decimal
    Public Property PlainPaperTicketingFeeTotal As Decimal
End Class