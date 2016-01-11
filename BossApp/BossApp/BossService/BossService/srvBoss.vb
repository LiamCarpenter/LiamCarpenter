Option Strict On

Imports EvoUtilities.ConfigUtils
Imports EvoUtilities.CollectionUtils
Imports EvoUtilities.log4netUtils
Imports System.Threading
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Text
Imports BossData

Public Class srvBoss

    Private Shared ReadOnly log As log4net.ILog = _
        log4net.LogManager.GetLogger(System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName)
    Dim Server As Object

    Public Sub setupLogging()
        Try
            log4net.Config.XmlConfigurator.Configure( _
                New System.IO.FileInfo(getConfig("log4netconfig")))
            log4net.LogManager.GetLogger( _
                System.Reflection.MethodBase.GetCurrentMethod(). _
                DeclaringType).Debug("Application Start-1")
        Catch ex As Exception
            log.Error("setupLogging", ex)
        End Try
    End Sub

    Public Sub startIt()
        OnStart(New String() {})
    End Sub

    Public Sub stopIt()
        OnStop()
    End Sub

    '***********************************************
    'service start and stop
    Protected Overrides Sub OnStart(ByVal args() As String)
        Using New MethodLogger(log, "OnStart")
            setupLogging()
            Try
                log.Info("BOSS Service Started")
                setupdailyEveningTime()
                setupdailyEveningCashTime()
                setupdailyCustomerTime()
                setupdailyCityCodeTime()
                setupdailyProductTime()
                setupdailyProtypeTime()
                setupdailySupplierTime()
                setupdailyRegionTime()
                setupdailySaveCodeTime()
                setupMonthlyTime()
                setupTotRecvdTime()
                setupMainCommTime()
                setuppaydetTime()
                setupPaydetYearTime()
                setupCashhdrTime()
                'setup the timer - gets the interval from the config file        
                Dim strPollInterval As String = getConfig("PollSecondsInterval")
                Timer1.Interval = CInt(strPollInterval) * 1000
                'call wake up as soon as the service starts
                Timer1.Enabled = True
                doWork()

            Catch ex As Exception
                log.Error("OnStart", ex)
            End Try
        End Using
    End Sub

    Protected Overrides Sub OnStop()
        Using New MethodLogger(log, "OnStop")
            Try
                log.Info("BOSS Service Stopped")
                Timer1.Enabled = False
                'todo: save the running threads as members
                'call join on these before exiting onstop
            Catch ex As Exception
                log.Error("OnStop", ex)
            End Try
        End Using
    End Sub

    Private doHourly As Boolean = True
    Private hourlyDisabled As Boolean = False
    Private hourlyLauncherRunning As Boolean = False

    Private doPaydetHourly As Boolean = True
    Private PaydetHourlyDisabled As Boolean = False
    Private PaydetHourlyLauncherRunning As Boolean = False

    Private doCashhdr As Boolean = True
    Private CashhdrDisabled As Boolean = False
    Private CashhdrLauncherRunning As Boolean = False

    Private dailyCashhdrDisabled As Boolean = False
    Private lastdailyCashhdrDay As DateTime
    Private dailyCashhdrTime As Long
    Private dailyCashhdrLauncherRunning As Boolean = False

    Private doCash As Boolean = True
    Private CashDisabled As Boolean = False
    Private CashLauncherRunning As Boolean = False

    Private dailyEveningDisabled As Boolean = False
    Private lastdailyEveningDay As DateTime
    Private dailyEveningTime As Long
    Private dailyEveningLauncherRunning As Boolean = False

    Private dailyEveningCashDisabled As Boolean = False
    Private lastdailyEveningCashDay As DateTime
    Private dailyEveningCashTime As Long
    Private dailyEveningCashLauncherRunning As Boolean = False

    Private dailyCustomerDisabled As Boolean = False
    Private lastdailyCustomerDay As DateTime
    Private dailyCustomerTime As Long
    Private dailyCustomerLauncherRunning As Boolean = False

    Private dailyProductDisabled As Boolean = False
    Private lastdailyProductDay As DateTime
    Private dailyProductTime As Long
    Private dailyProductLauncherRunning As Boolean = False

    Private dailyProtypeDisabled As Boolean = False
    Private lastdailyProtypeDay As DateTime
    Private dailyProtypeTime As Long
    Private dailyProtypeLauncherRunning As Boolean = False

    Private dailySupplierDisabled As Boolean = False
    Private lastdailySupplierDay As DateTime
    Private dailySupplierTime As Long
    Private dailySupplierLauncherRunning As Boolean = False

    Private dailyRegionDisabled As Boolean = False
    Private lastdailyRegionDay As DateTime
    Private dailyRegionTime As Long
    Private dailyRegionLauncherRunning As Boolean = False

    Private dailySaveCodeDisabled As Boolean = False
    Private lastdailySaveCodeDay As DateTime
    Private dailySaveCodeTime As Long
    Private dailySaveCodeLauncherRunning As Boolean = False

    Private dailyCityCodeDisabled As Boolean = False
    Private lastdailyCityCodeDay As DateTime
    Private dailyCityCodeTime As Long
    Private dailyCityCodeLauncherRunning As Boolean = False

    Private monthlyDisabled As Boolean = False
    Private lastmonthlyDay As DateTime
    Private monthlyTime As Long
    Private monthlyLauncherRunning As Boolean = False

    Private mainCommDisabled As Boolean = False
    Private lastmainCommDay As DateTime
    Private mainCommTime As Long
    Private mainCommLauncherRunning As Boolean = False

    Private totRecvdDisabled As Boolean = False
    Private lasttotRecvdDay As DateTime
    Private totRecvdTime As Long
    Private totRecvdLauncherRunning As Boolean = False

    Private paydetDisabled As Boolean = False
    Private lastpaydetDay As DateTime
    Private paydetTime As Long
    Private paydetLauncherRunning As Boolean = False

    Private paydetYearDisabled As Boolean = False
    Private lastpaydetYearDay As DateTime
    Private paydetYearTime As Long
    Private paydetYearLauncherRunning As Boolean = False

    Private lastHourlyRunTicks As Long = 0
    Private lastCashRunTicks As Long = 0
    Private lastCashhdrRunTicks As Long = 0
    Private lastPayDetHourlyRunTicks As Long = 0

    Private Function minutesToTicks(ByVal minutes As Long) As Long
        Return minutes * 60 * 1000 * 1000 * 10
    End Function

    Private Function getCurrentDay() As DateTime
        Return stripTime(DateTime.Now)
    End Function

    Private Function stripTime(ByVal dt As DateTime) As DateTime
        Return New DateTime(dt.Year, dt.Month, dt.Day)
    End Function

    Private Function minutesInDay(ByVal dt As DateTime) As Long
        Return dt.Hour * 60 + dt.Minute
    End Function

    Private Function getCurrentTime() As Long
        Return minutesInDay(DateTime.Now)
    End Function

    Private Function yesterday() As DateTime
        Return stripTime(DateTime.Now) - New TimeSpan(24, 0, 0)
    End Function

    Private Sub Timer1_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer1.Elapsed
        Using New MethodLogger(log, "Timer1_Elapsed")
            Try
                doWork()
            Catch ex As Exception
                log.Error("Timer Elapsed", ex)
            End Try
        End Using
    End Sub

    Private Sub doWork()
        Using New MethodLogger(log, "doWork")

            'dailyEvening Invoice check
            Dim dailyEveningThread As Thread = Nothing
            If Not dailyEveningDisabled AndAlso lastdailyEveningDay < getCurrentDay() _
                AndAlso getCurrentTime() >= dailyEveningTime Then
                lastdailyEveningDay = getCurrentDay()
                log.Info("Starting dailyEvening thread")
                dailyEveningThread = New Thread( _
                    New ThreadStart(AddressOf updateEveningRecords))
                dailyEveningThread.Start()
            End If

            'dailyEvening Cash check
            Dim dailyEveningCashThread As Thread = Nothing
            If Not dailyEveningCashDisabled AndAlso lastdailyEveningCashDay < getCurrentDay() _
                AndAlso getCurrentTime() >= dailyEveningCashTime Then
                lastdailyEveningCashDay = getCurrentDay()
                log.Info("Starting dailyEveningCash thread")
                dailyEveningCashThread = New Thread( _
                    New ThreadStart(AddressOf updateEveningCashRecords))
                dailyEveningCashThread.Start()
            End If

            'dailyEvening Cashhdr check
            Dim dailyCashhdrThread As Thread = Nothing
            If Not dailyCashhdrDisabled AndAlso lastdailyCashhdrDay < getCurrentDay() _
                AndAlso getCurrentTime() >= dailyCashhdrTime Then
                lastdailyCashhdrDay = getCurrentDay()
                log.Info("Starting dailyCashhdr thread")
                dailyCashhdrThread = New Thread( _
                    New ThreadStart(AddressOf updateDailyCashHdr))
                dailyCashhdrThread.Start()
            End If

            'monthly invoice check
            Dim monthlyThread As Thread = Nothing
            If Not monthlyDisabled AndAlso lastmonthlyDay < getCurrentDay() Then
                lastmonthlyDay = getCurrentDay()
                    log.Info("Starting monthly thread")
                    monthlyThread = New Thread( _
                        New ThreadStart(AddressOf updateMonthlyRecords))
                    monthlyThread.Start()
            End If

            Dim nowTicks As Long = DateTime.Now.Ticks
            Dim dtDaystart As New Date(Now.Year, Now.Month, Now.Day, 7, 0, 0)
            Dim dtDayend As New Date(Now.Year, Now.Month, Now.Day, 19, 0, 0)
            'hourly invoice checks only during the working days of the week
            Dim hourlyThread As Thread = Nothing
            If doHourly Then
                If lastHourlyRunTicks + minutesToTicks(CInt(getConfig("doHourlyIntervalMinutes"))) < nowTicks Then
                    lastHourlyRunTicks = nowTicks
                    If nowTicks > dtDaystart.Ticks And nowTicks < dtDayend.Ticks Then
                        log.Info("starting hourly thread")
                        hourlyThread = New Thread( _
                            New ThreadStart(AddressOf updateHourlyRecords))
                        hourlyThread.Start()
                    End If
                End If
            End If

            'hourly cash checks only during the working days of the week
            Dim cashhdrThread As Thread = Nothing
            If doCashhdr Then
                If lastCashhdrRunTicks + minutesToTicks(CInt(getConfig("doHourlyIntervalMinutes"))) < nowTicks Then
                    lastCashhdrRunTicks = nowTicks
                    If nowTicks > dtDaystart.Ticks And nowTicks < dtDayend.Ticks Then
                        log.Info("starting cashhdr thread")
                        cashhdrThread = New Thread( _
                            New ThreadStart(AddressOf updateHourlyCashHdr))
                        cashhdrThread.Start()
                    End If
                End If
            End If

            'hourly cash checks only during the working days of the week
            Dim cashThread As Thread = Nothing
            If doCash Then
                If lastCashRunTicks + minutesToTicks(CInt(getConfig("doHourlyIntervalMinutes"))) < nowTicks Then
                    lastCashRunTicks = nowTicks
                    If nowTicks > dtDaystart.Ticks And nowTicks < dtDayend.Ticks Then
                        log.Info("starting cash thread")
                            cashThread = New Thread( _
                                New ThreadStart(AddressOf updateCashRecords))
                            cashThread.Start()
                    End If
                End If
            End If

            'hourly paydet checks only during the working days of the week
            Dim paydetHourlyThread As Thread = Nothing
            If doPaydetHourly Then
                If lastPayDetHourlyRunTicks + minutesToTicks(CInt(getConfig("doHourlyIntervalMinutes"))) < nowTicks Then
                    lastPayDetHourlyRunTicks = nowTicks
                    If nowTicks > dtDaystart.Ticks And nowTicks < dtDayend.Ticks Then
                        log.Info("starting paydetHourly thread")
                        paydetHourlyThread = New Thread( _
                            New ThreadStart(AddressOf updatePaydetHourly))
                        paydetHourlyThread.Start()
                    End If
                End If
            End If

            'dailyCustomer check
            Dim dailyCustomerThread As Thread = Nothing
            If Not dailyCustomerDisabled AndAlso lastdailyCustomerDay < getCurrentDay() _
                AndAlso getCurrentTime() >= dailyCustomerTime Then
                lastdailyCustomerDay = getCurrentDay()
                log.Info("Starting dailyCustomer thread")
                dailyCustomerThread = New Thread( _
                    New ThreadStart(AddressOf updateCustomerRecords))
                dailyCustomerThread.Start()
            End If

            'dailyCityCode check
            Dim dailyCityCodeThread As Thread = Nothing
            If Not dailyCityCodeDisabled AndAlso lastdailyCityCodeDay < getCurrentDay() _
                AndAlso getCurrentTime() >= dailyCityCodeTime Then
                lastdailyCityCodeDay = getCurrentDay()
                log.Info("Starting dailyCityCode thread")
                dailyCityCodeThread = New Thread( _
                    New ThreadStart(AddressOf updateCityCodeRecords))
                dailyCityCodeThread.Start()
            End If

            'dailyProduct check
            Dim dailyProductThread As Thread = Nothing
            If Not dailyProductDisabled AndAlso lastdailyProductDay < getCurrentDay() _
                AndAlso getCurrentTime() >= dailyProductTime Then
                lastdailyProductDay = getCurrentDay()
                log.Info("Starting dailyProduct thread")
                dailyProductThread = New Thread( _
                    New ThreadStart(AddressOf updateProductRecords))
                dailyProductThread.Start()
            End If

            ' dailyProtype check
            Dim dailyProtypeThread As Thread = Nothing
            If Not dailyProtypeDisabled AndAlso lastdailyProtypeDay < getCurrentDay() _
                AndAlso getCurrentTime() >= dailyProtypeTime Then
                lastdailyProtypeDay = getCurrentDay()
                log.Info("Starting dailyProtype thread")
                dailyProtypeThread = New Thread( _
                    New ThreadStart(AddressOf updateProtypeRecords))
                dailyProtypeThread.Start()
            End If

            ' dailySupplier check
            Dim dailySupplierThread As Thread = Nothing
            If Not dailySupplierDisabled AndAlso lastdailySupplierDay < getCurrentDay() _
                AndAlso getCurrentTime() >= dailySupplierTime Then
                lastdailySupplierDay = getCurrentDay()
                log.Info("Starting dailySupplier thread")
                dailySupplierThread = New Thread( _
                    New ThreadStart(AddressOf updateSupplierRecords))
                dailySupplierThread.Start()
            End If

            ' dailyRegion check
            Dim dailyRegionThread As Thread = Nothing
            If Not dailyRegionDisabled AndAlso lastdailyRegionDay < getCurrentDay() _
                AndAlso getCurrentTime() >= dailyRegionTime Then
                lastdailyRegionDay = getCurrentDay()
                log.Info("Starting dailyRegion thread")
                dailyRegionThread = New Thread( _
                    New ThreadStart(AddressOf updateRegionRecords))
                dailyRegionThread.Start()
            End If

            ' dailySaveCode check
            Dim dailySaveCodeThread As Thread = Nothing
            If Not dailySaveCodeDisabled AndAlso lastdailySaveCodeDay < getCurrentDay() _
                AndAlso getCurrentTime() >= dailySaveCodeTime Then
                lastdailySaveCodeDay = getCurrentDay()
                log.Info("Starting dailySaveCode thread")
                dailySaveCodeThread = New Thread( _
                    New ThreadStart(AddressOf updateSaveCodeRecords))
                dailySaveCodeThread.Start()
            End If

            ' mainComm check
            Dim mainCommThread As Thread = Nothing
            If Not mainCommDisabled AndAlso lastmainCommDay < getCurrentDay() _
                AndAlso getCurrentTime() >= mainCommTime Then
                lastmainCommDay = getCurrentDay()
                log.Info("Starting mainComm thread")
                mainCommThread = New Thread( _
                    New ThreadStart(AddressOf updateMainComm))
                mainCommThread.Start()
            End If

            ' totRecvd check
            Dim totRecvdThread As Thread = Nothing
            If Not totRecvdDisabled AndAlso lasttotRecvdDay < getCurrentDay() _
                AndAlso getCurrentTime() >= totRecvdTime Then
                lasttotRecvdDay = getCurrentDay()
                log.Info("Starting totRecvd thread")
                totRecvdThread = New Thread( _
                    New ThreadStart(AddressOf updateTotRecvd))
                totRecvdThread.Start()
            End If

            ' paydet check
            Dim paydetThread As Thread = Nothing
            If Not paydetDisabled AndAlso lastpaydetDay < getCurrentDay() _
                AndAlso getCurrentTime() >= paydetTime Then
                lastpaydetDay = getCurrentDay()
                log.Info("Starting paydet thread")
                paydetThread = New Thread( _
                    New ThreadStart(AddressOf updatePaydet))
                paydetThread.Start()
            End If

            '' paydetYear check
            'Dim paydetYearThread As Thread = Nothing
            'If Not paydetYearDisabled AndAlso lastpaydetYearDay < getCurrentDay() _
            '    AndAlso getCurrentTime() >= paydetYearTime Then
            '    lastpaydetYearDay = getCurrentDay()
            '    log.Info("Starting paydetYear thread")
            '    paydetYearThread = New Thread( _
            '        New ThreadStart(AddressOf updatePaydetYear))
            '    paydetYearThread.Start()
            'End If
        End Using
    End Sub

    Private Sub setupCashhdrTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time

            dailyCashhdrTime = getCashhdrTimeFromConfig()

            If getCurrentTime() > dailyCashhdrTime Then
                lastdailyCashhdrDay = stripTime(DateTime.Now)
                log.Info("setupCashhdrTime has passed, next setupCashhdr will be tomorrow")
            Else
                lastdailyCashhdrDay = yesterday()
                log.Info("next setupCashhdr will be today")
            End If
        Catch ex As Exception
            log.Error("Scheduled setupCashhdr have been disabled, please fix the configuration file and restart the service.", ex)
            CashhdrDisabled = True
        End Try
    End Sub

    Private Function getCashhdrTimeFromConfig() As Long
        log.Info("Cashhdr time is: " & getConfig("dailyCashhdrTime"))
        Dim m As Match = Regex.Match(getConfig("dailyCashhdrTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Sub setuppaydetYearTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time
            paydetYearTime = getpaydetYearTimeFromConfig()

            If getCurrentTime() > paydetYearTime Then
                lastpaydetYearDay = stripTime(DateTime.Now)
                log.Info("paydetYear time has passed, next paydetYear will be tomorrow")
            Else
                lastpaydetYearDay = yesterday()
                log.Info("next paydetYear will be today")
            End If
        Catch ex As Exception
            log.Error("Scheduled paydetYear have been disabled, please fix the configuration file and restart the service.", ex)
            paydetYearDisabled = True
        End Try
    End Sub

    Private Function getpaydetYearTimeFromConfig() As Long
        log.Info("paydetYear time is: " & getConfig("paydetYearTime"))
        Dim m As Match = Regex.Match(getConfig("paydetYearTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Sub setuppaydetTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time
            paydetTime = getpaydetTimeFromConfig()

            If getCurrentTime() > paydetTime Then
                lastpaydetDay = stripTime(DateTime.Now)
                log.Info("paydet time has passed, next paydet will be tomorrow")
            Else
                lastpaydetDay = yesterday()
                log.Info("next paydet will be today")
            End If
        Catch ex As Exception
            log.Error("Scheduled paydet have been disabled, please fix the configuration file and restart the service.", ex)
            paydetDisabled = True
        End Try
    End Sub

    Private Function getpaydetTimeFromConfig() As Long
        log.Info("paydet time is: " & getConfig("paydetTime"))
        Dim m As Match = Regex.Match(getConfig("paydetTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Sub setupMainCommTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time
            mainCommTime = getMainCommTimeFromConfig()

            If getCurrentTime() > mainCommTime Then
                lastmainCommDay = stripTime(DateTime.Now)
                log.Info("mainComm time has passed, next mainComm will be tomorrow")
            Else
                lastmainCommDay = yesterday()
                log.Info("next mainComm will be today")
            End If
        Catch ex As Exception
            log.Error("Scheduled mainComm have been disabled, please fix the configuration file and restart the service.", ex)
            mainCommDisabled = True
        End Try
    End Sub

    Private Function getMainCommTimeFromConfig() As Long
        log.Info("mainComm time is: " & getConfig("mainCommTime"))
        Dim m As Match = Regex.Match(getConfig("mainCommTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Sub setupTotRecvdTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time
            totRecvdTime = getTotRecvdTimeFromConfig()

            If getCurrentTime() > totRecvdTime Then
                lasttotRecvdDay = stripTime(DateTime.Now)
                log.Info("TotRecvd time has passed, next TotRecvd will be tomorrow")
            Else
                lasttotRecvdDay = yesterday()
                log.Info("next TotRecvd will be today")
            End If
        Catch ex As Exception
            log.Error("Scheduled TotRecvd have been disabled, please fix the configuration file and restart the service.", ex)
            totRecvdDisabled = True
        End Try
    End Sub

    Private Function getTotRecvdTimeFromConfig() As Long
        log.Info("TotRecvd time is: " & getConfig("totRecvdTime"))
        Dim m As Match = Regex.Match(getConfig("totRecvdTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Sub setupdailySaveCodeTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time
            dailySaveCodeTime = getdailySaveCodeTimeFromConfig()

            If getCurrentTime() > dailySaveCodeTime Then
                lastdailySaveCodeDay = stripTime(DateTime.Now)
                log.Info("dailySaveCode time has passed, next dailySaveCode will be tomorrow")
            Else
                lastdailySaveCodeDay = yesterday()
                log.Info("next dailSaveCode will be today")
            End If
        Catch ex As Exception
            log.Error("Scheduled dailySaveCode have been disabled, please fix the configuration file and restart the service.", ex)
            dailySaveCodeDisabled = True
        End Try
    End Sub

    Private Function getdailySaveCodeTimeFromConfig() As Long
        log.Info("dailySaveCode time is: " & getConfig("dailySaveCodeTime"))
        Dim m As Match = Regex.Match(getConfig("dailySaveCodeTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Sub setupdailyRegionTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time
            dailyRegionTime = getdailyRegionTimeFromConfig()

            If getCurrentTime() > dailyRegionTime Then
                lastdailyRegionDay = stripTime(DateTime.Now)
                log.Info("dailyRegion time has passed, next dailyRegion will be tomorrow")
            Else
                lastdailyRegionDay = yesterday()
                log.Info("next dailRegion will be today")
            End If
        Catch ex As Exception
            log.Error("Scheduled dailyRegion have been disabled, please fix the configuration file and restart the service.", ex)
            dailyRegionDisabled = True
        End Try
    End Sub

    Private Function getdailyRegionTimeFromConfig() As Long
        log.Info("dailyRegion time is: " & getConfig("dailyRegionTime"))
        Dim m As Match = Regex.Match(getConfig("dailyRegionTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Sub setupdailySupplierTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time
            dailySupplierTime = getdailySupplierTimeFromConfig()

            If getCurrentTime() > dailySupplierTime Then
                lastdailySupplierDay = stripTime(DateTime.Now)
                log.Info("dailySupplier time has passed, next dailySupplier will be tomorrow")
            Else
                lastdailySupplierDay = yesterday()
                log.Info("next dailSupplier will be today")
            End If
        Catch ex As Exception
            log.Error("Scheduled dailySupplier have been disabled, please fix the configuration file and restart the service.", ex)
            dailySupplierDisabled = True
        End Try
    End Sub

    Private Function getdailySupplierTimeFromConfig() As Long
        log.Info("dailySupplier time is: " & getConfig("dailySupplierTime"))
        Dim m As Match = Regex.Match(getConfig("dailySupplierTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Sub setupdailyProtypeTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time
            dailyProtypeTime = getdailyProtypeTimeFromConfig()

            If getCurrentTime() > dailyProtypeTime Then
                lastdailyProtypeDay = stripTime(DateTime.Now)
                log.Info("dailyProtype time has passed, next dailyProtype will be tomorrow")
            Else
                lastdailyProtypeDay = yesterday()
                log.Info("next dailProtype will be today")
            End If
        Catch ex As Exception
            log.Error("Scheduled dailyProtype have been disabled, please fix the configuration file and restart the service.", ex)
            dailyProtypeDisabled = True
        End Try
    End Sub

    Private Function getdailyProtypeTimeFromConfig() As Long
        log.Info("dailyProtype time is: " & getConfig("dailyProtypeTime"))
        Dim m As Match = Regex.Match(getConfig("dailyProtypeTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Sub setupdailyProductTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time
            dailyProductTime = getdailyProductTimeFromConfig()

            If getCurrentTime() > dailyProductTime Then
                lastdailyProductDay = stripTime(DateTime.Now)
                log.Info("dailyProduct time has passed, next dailyProduct will be tomorrow")
            Else
                lastdailyProductDay = yesterday()
                log.Info("next dailProduct will be today")
            End If
        Catch ex As Exception
            log.Error("Scheduled dailyProduct have been disabled, please fix the configuration file and restart the service.", ex)
            dailyProductDisabled = True
        End Try
    End Sub

    Private Function getdailyProductTimeFromConfig() As Long
        log.Info("dailyProduct time is: " & getConfig("dailyProductTime"))
        Dim m As Match = Regex.Match(getConfig("dailyProductTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Sub setupdailyCustomerTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time
            dailyCustomerTime = getdailyCustomerTimeFromConfig()

            If getCurrentTime() > dailyCustomerTime Then
                lastdailyCustomerDay = stripTime(DateTime.Now)
                log.Info("dailyCustomer time has passed, next dailyCustomer will be tomorrow")
            Else
                lastdailyCustomerDay = yesterday()
                log.Info("next dailCustomer will be today")
            End If
        Catch ex As Exception
            log.Error("Scheduled dailyCustomer have been disabled, please fix the configuration file and restart the service.", ex)
            dailyCustomerDisabled = True
        End Try
    End Sub

    Private Function getdailyCustomerTimeFromConfig() As Long
        log.Info("dailyCustomer time is: " & getConfig("dailyCustomerTime"))
        Dim m As Match = Regex.Match(getConfig("dailyCustomerTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Sub setupdailyCityCodeTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time
            dailyCityCodeTime = getdailyCityCodeTimeFromConfig()

            If getCurrentTime() > dailyCityCodeTime Then
                lastdailyCityCodeDay = stripTime(DateTime.Now)
                log.Info("dailyCityCode time has passed, next dailyCityCode will be tomorrow")
            Else
                lastdailyCityCodeDay = yesterday()
                log.Info("next dailCityCode will be today")
            End If
        Catch ex As Exception
            log.Error("Scheduled dailyCityCode have been disabled, please fix the configuration file and restart the service.", ex)
            dailyCustomerDisabled = True
        End Try
    End Sub

    Private Function getdailyCityCodeTimeFromConfig() As Long
        log.Info("dailyCityCode time is: " & getConfig("dailyCityCodeTime"))
        Dim m As Match = Regex.Match(getConfig("dailyCityCodeTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Sub setupdailyEveningTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time
            dailyEveningTime = getdailyEveningTimeFromConfig()

            If getCurrentTime() > dailyEveningTime Then
                lastdailyEveningDay = stripTime(DateTime.Now)
                log.Info("dailyEvening time has passed, next dailyEvening will be tomorrow")
            Else
                lastdailyEveningDay = yesterday()
                log.Info("next dailEvening will be today")
            End If
        Catch ex As Exception
            log.Error("Scheduled dailyEvening have been disabled, please fix the configuration file and restart the service.", ex)
            dailyEveningDisabled = True
        End Try
    End Sub

    Private Function getdailyEveningTimeFromConfig() As Long
        log.Info("dailyEvening time is: " & getConfig("dailyEveningTime"))
        Dim m As Match = Regex.Match(getConfig("dailyEveningTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Sub setupdailyEveningCashTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time
            dailyEveningCashTime = getdailyEveningCashTimeFromConfig()

            If getCurrentTime() > dailyEveningCashTime Then
                lastdailyEveningCashDay = stripTime(DateTime.Now)
                log.Info("dailyEveningCash time has passed, next dailyEveningCash will be tomorrow")
            Else
                lastdailyEveningCashDay = yesterday()
                log.Info("next dailEveningCash will be today")
            End If
        Catch ex As Exception
            log.Error("Scheduled dailyEveningCash have been disabled, please fix the configuration file and restart the service.", ex)
            dailyEveningCashDisabled = True
        End Try
    End Sub

    Private Function getdailyEveningCashTimeFromConfig() As Long
        log.Info("dailyEveningCash time is: " & getConfig("dailyEveningCashTime"))
        Dim m As Match = Regex.Match(getConfig("dailyEveningCashTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Private Sub setupMonthlyTime()
        Try
            'setup trigger stuff: if the time is greater than the trigger time
            'then set the last trigger day to today: so it will next trigger tomorrow
            'otherwise set the last trigger day to yesterday, so it will trigger
            'when it hits the trigger time
            monthlyTime = getMonthlyTimeFromConfig()

            If getCurrentTime() > monthlyTime Then
                lastmonthlyDay = stripTime(DateTime.Now)
                log.Info("Monthly time has passed, next Monthly will be tomorrow")
            Else
                lastmonthlyDay = yesterday()
                log.Info("next Monthly will be today")
            End If
        Catch ex As Exception
            log.Error("Scheduled Monthly have been disabled, please fix the configuration file and restart the service.", ex)
            dailyEveningDisabled = True
        End Try
    End Sub

    Private Function getMonthlyTimeFromConfig() As Long
        log.Info("Monthly time is: " & getConfig("MonthlyTime"))
        Dim m As Match = Regex.Match(getConfig("MonthlyTime"), "^(?<hour>\d?\d):(?<minute>\d?\d)$")
        Return CInt(m.Groups.Item("hour").Value) * 60 + CInt(m.Groups.Item("minute").Value)
    End Function

    Public Sub updateHourlyRecords()
        Try
            If hourlyLauncherRunning Then
                log.Info("Tried to call updateHourlyRecords, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for hourly started")
                    hourlyLauncherRunning = True
                    'will run all todays changes
                    runMainSelect(True, False, False, 1)
                    log.Info("checking for hourly completed")
                Finally
                    hourlyLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateHourlyRecords", ex)
        End Try
    End Sub

    Public Sub updateTotRecvd()
        Try
            If totRecvdLauncherRunning Then
                log.Info("Tried to call updateTotRecvd, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for totRecvd started")
                    totRecvdLauncherRunning = True
                    'will run all todays changes
                    selectInvtotRecvd()
                    log.Info("checking for totRecvd completed")
                Finally
                    totRecvdLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateTotRecvd", ex)
        End Try
    End Sub

    Public Sub updatePaydet()
        Try
            If paydetLauncherRunning Then
                log.Info("Tried to call  updatePaydet, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for paydet started")
                    paydetLauncherRunning = True
                    'will run all todays changes
                    runPaydet(False, True, "")
                    runPayhdr(False, True, "")
                    updateAccountsPayable()
                    updateCommissions()
                    log.Info("checking for paydet completed")
                Finally
                    paydetLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updatePaydet", ex)
        End Try
    End Sub

    Public Sub updateAccountsPayable()
        Try
            'do the BOSS thing based on all the data in the grid
            Dim oInvoices As New List(Of AccountsPayableDB)
            oInvoices = AccountsPayableDB.accountsPayableGetInvoices(Format(Now.AddYears(-1), "dd/MM/yyyy"), Format(Now, "dd/MM/yyyy"))

            For Each oInvoice As AccountsPayableDB In oInvoices
                If Not runPaydet(False, False, oInvoice.InvoiceRef) Then
                    log.Error("Error in runPaydet of updateAccountsPayable")
                End If
            Next
        Catch ex As Exception
            log.Error("updateAccountsPayable", ex)
        End Try
    End Sub

    Public Sub updateCommissions()
        Try
            'do the BOSS thing based on all the data in the grid
            Dim oInvoices As New List(Of AccountsPayableDB)
            oInvoices = AccountsPayableDB.commissionGetInvoices(Format(Now, "dd/MM/yyyy"))

            For Each oInvoice As AccountsPayableDB In oInvoices
                If oInvoice.InvoiceRef.Contains(Chr(187)) = False Then

                    If selectInvMainRecords(oInvoice.InvoiceRef) Then
                        If selectInvtotRecord(oInvoice.InvoiceRef) Then
                            If selectInvRouteRecords(oInvoice.InvoiceRef) Then

                            End If
                        End If
                    End If

                End If
            Next
        Catch ex As Exception
            log.Error("updateCommissions", ex)
        End Try
    End Sub

    Public Sub updatePaydetHourly()
        Try
            If PaydetHourlyLauncherRunning Then
                log.Info("Tried to call updatePaydetHourly, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for updatePaydetHourly started")
                    PaydetHourlyLauncherRunning = True
                    'will run all todays changes
                    runPaydet(True, False, "")
                    runPayhdr(True, False, "")
                    log.Info("checking for updatePaydetHourly completed")
                Finally
                    PaydetHourlyLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updatePaydetHourly", ex)
        End Try
    End Sub

    Public Sub updateMainComm()
        Try
            If totRecvdLauncherRunning Then
                log.Info("Tried to call updateMainComm, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for MainComm started")
                    mainCommLauncherRunning = True
                    'will run all todays changes
                    selectInvMainComm()
                    log.Info("checking for MainComm completed")
                Finally
                    mainCommLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateMainComm", ex)
        End Try
    End Sub

    Public Sub updateCashRecords()
        Try
            If CashLauncherRunning Then
                log.Info("Tried to call updateCashRecords, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for Cash started")
                    CashLauncherRunning = True
                    'will run all todays changes
                    runCashSelect(True, False, False)
                    log.Info("checking for Cash completed")
                Finally
                    CashLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateCashRecords", ex)
        End Try
    End Sub

    Public Sub updateHourlyCashHdr()
        Try

            If CashhdrLauncherRunning Then
                log.Info("Tried to call updateHourlyCashHdr, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for updateHourlyCashHdr started")
                    CashhdrLauncherRunning = True
                    'will run all todays changes
                    runCashHdr(True, False, False)
                    log.Info("checking for updateHourlyCashHdr completed")
                Finally
                    CashhdrLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateHourlyCashHdr", ex)
        End Try
    End Sub

    Public Sub updateDailyCashAll()
        Try
            runCashSelect(False, False, True)
        Catch ex As Exception
            log.Error("updateDailyCashAll", ex)
        End Try
    End Sub

    Public Sub updateDailyCashHdr()
        Try
            If dailyCashhdrLauncherRunning Then
                log.Info("Tried to call updateDailyCashHdr, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for updateDailyCashHdr started")
                    dailyCashhdrLauncherRunning = True
                    'will run all todays changes
                    runCashHdr(False, True, False)
                    log.Info("checking for updateDailyCashHdr completed")
                Finally
                    dailyCashhdrLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateCashhdr", ex)
        End Try
    End Sub

    Public Sub updateEveningCashhdrRecords()
        Try
            If CashLauncherRunning Then
                log.Info("Tried to call updateEveningCashRecords, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for EveningCash started")
                    dailyEveningCashLauncherRunning = True
                    'will run all todays changes
                    runCashHdr(False, True, False)
                    log.Info("checking for EveningCash completed")
                Finally
                    dailyEveningCashLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateEveningCashRecords", ex)
        End Try
    End Sub

    Public Sub updateEveningCashRecords()
        Try
            If CashLauncherRunning Then
                log.Info("Tried to call updateEveningCashRecords, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for EveningCash started")
                    dailyEveningCashLauncherRunning = True
                    'will run all todays changes
                    runCashSelect(False, True, False)
                    log.Info("checking for EveningCash completed")
                Finally
                    dailyEveningCashLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateEveningCashRecords", ex)
        End Try
    End Sub

    Public Sub updateEveningRecords()
        Try
            If dailyEveningLauncherRunning Then
                log.Info("Tried to call dailyEvening, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for todays dailyEvening started")
                    dailyEveningLauncherRunning = True
                    'will run all todays changes
                    runMainSelect(False, True, False, 2)
                    log.Info("checking for todays dailyEvening completed")
                Finally
                    dailyEveningLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateEveningRecords", ex)
        End Try
    End Sub

    Public Sub updateCustomerRecords()
        Try
            If dailyCustomerLauncherRunning Then
                log.Info("Tried to call dailyCustomer, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for todays dailyCustomer started")
                    dailyCustomerLauncherRunning = True
                    'will run all todays changes
                    bossCustomers()
                    log.Info("checking for todays dailyCustomer completed")
                Finally
                    dailyCustomerLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateCustomerRecords", ex)
        End Try
    End Sub

    Public Sub updateCityCodeRecords()
        Try
            If dailyCityCodeLauncherRunning Then
                log.Info("Tried to call dailyCityCode, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for todays dailyCityCode started")
                    dailyCityCodeLauncherRunning = True
                    'will run all todays changes
                    bossCityCodes()
                    log.Info("checking for todays dailyCityCode completed")
                Finally
                    dailyCityCodeLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateCityCodeRecords", ex)
        End Try
    End Sub

    Public Sub updateProductRecords()
        Try
            If dailyProductLauncherRunning Then
                log.Info("Tried to call dailyProduct, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for todays dailyProduct started")
                    dailyProductLauncherRunning = True
                    'will run all todays changes
                    bossProducts()
                    log.Info("checking for todays dailyProduct completed")
                Finally
                    dailyProductLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateProductRecords", ex)
        End Try
    End Sub

    Public Sub updateProtypeRecords()
        Try
            If dailyProtypeLauncherRunning Then
                log.Info("Tried to call dailyProtype, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for todays dailyProtype started")
                    dailyProtypeLauncherRunning = True
                    'will run all todays changes
                    bossProtypes()
                    log.Info("checking for todays dailyProtype completed")
                Finally
                    dailyProtypeLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateProtypeRecords", ex)
        End Try
    End Sub

    Public Sub updateRegionRecords()
        Try
            If dailyRegionLauncherRunning Then
                log.Info("Tried to call dailyRegion, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for todays dailyRegion started")
                    dailyRegionLauncherRunning = True
                    'will run all todays changes
                    bossRegions()
                    log.Info("checking for todays dailyRegion completed")
                Finally
                    dailyRegionLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateRegionRecords", ex)
        End Try
    End Sub

    Public Sub updateSaveCodeRecords()
        Try
            If dailySaveCodeLauncherRunning Then
                log.Info("Tried to call dailySaveCode, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for todays dailySaveCode started")
                    dailySaveCodeLauncherRunning = True
                    'will run all todays changes
                    bossSaveCodes()
                    log.Info("checking for todays dailySaveCode completed")
                Finally
                    dailySaveCodeLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateSaveCodeRecords", ex)
        End Try
    End Sub

    Public Sub updateSupplierRecords()
        Try
            If dailySupplierLauncherRunning Then
                log.Info("Tried to call dailySupplier, but it's still running from the previous wake up.")
            Else
                Try
                    log.Info("checking for todays dailySupplier started")
                    dailySupplierLauncherRunning = True
                    'will run all todays changes
                    bossSuppliers()
                    log.Info("checking for todays dailySupplier completed")
                Finally
                    dailySupplierLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateSupplierRecords", ex)
        End Try
    End Sub

    Public Sub updateMonthlyRecords()
        Try
            If monthlyLauncherRunning Then
                log.Info("Tried to call monthly, but it's still running from the previous wake up.")
            Else
                Try
                    'only run on a sunday
                    If Now.DayOfWeek = DayOfWeek.Sunday Then
                        log.Info("checking for todays monthly started")
                        monthlyLauncherRunning = True
                        'R2.0 SA 
                        log.Info("Checking for todays monthly started")
                        runMainSelect(False, False, True, 3)
                        log.Info("Checking for todays monthly completed")
                    Else
                        log.Info("It's not sunday")
                    End If
                Finally
                    monthlyLauncherRunning = False
                End Try
            End If
        Catch ex As Exception
            log.Error("updateMonthlyRecords", ex)
        End Try
    End Sub

    Private Sub bossCustomers()
        Using New MethodLogger(log, "bossCustomers")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT cus_id, cus_type, cus_grpid, cus_grpid2, cus_grpid3, cus_br, cus_coname, cus_add1, cus_add2, cus_add3, cus_add4, cus_pcode1, cus_pcode2, cus_pcode," & _
                                                        "cus_cntry, cus_cntct1, cus_cntct2, cus_cntct3, cus_cntct4, cus_cntct5, cus_tel, cus_fax, cus_email, cus_email2, cus_cdate, cus_fdate, cus_ldate, cus_onact," & _
                                                        "cus_insure, cus_insexp, cus_sref1, cus_sref2, cus_sref3, cus_sref4, cus_sref5, cus_sref6, cus_con1, cus_c1mth1, cus_c1mth2, cus_c1mth3, cus_c1pct1," & _
                                                        "cus_c1pct2, cus_c1pct3, cus_c1pct4, cus_con2, cus_c2mth1, cus_c2mth2, cus_c2mth3, cus_c2pct1, cus_c2pct2, cus_c2pct3, cus_c2pct4, cus_ptermc, cus_ptermv," & _
                                                        "cus_crep, cus_state, cus_pono, cus_costc, cus_cref1, cus_cref2, cus_ccard, cus_cconly, cus_climit, cus_debt, cus_stop, cus_2ndadd, cus_notes, cus_stadd," & _
                                                        "cus_soy, cus_conam2, cus_pop1, cus_pop2, cus_ovrdue, cus_dscpct, cus_fulreb, cus_hidecm, cus_rebcn, cus_rebfrq, cus_reb1cn, cus_rebinv, cus_incfee," & _
                                                        "cus_nocc, cus_feeid1, cus_feeid2, cus_feeid3, cus_adhoc, cus_dofee, cus_ibank, cus_feetyp, cus_atol, cus_paydd, cus_sps, cus_acas, cus_abtano, cus_mailst," & _
                                                        "cus_mailiv, cus_coninv, cus_cinvdt, cus_keyact, cus_invfrm, cus_stfrm, cus_noba, cus_slfbil, cus_exvat, cus_curncy, cus_onepdf, cus_spare2, cus_dorm," & _
                                                        "cus_mltfee, cus_print " & _
                                                        "FROM customer", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "Customer")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("Customer").Rows
                    'sp save will check if exists etc
                    Dim oCustX As New BOSScustomer(0, CStr(dr.Item("cus_id")), CStr(dr.Item("cus_type")), CStr(dr.Item("cus_grpid")), CStr(dr.Item("cus_grpid2")), CStr(dr.Item("cus_grpid3")), CStr(dr.Item("cus_br")), _
                                               CStr(dr.Item("cus_coname")), CStr(dr.Item("cus_add1")), CStr(dr.Item("cus_add2")), CStr(dr.Item("cus_add3")), CStr(dr.Item("cus_add4")), CStr(dr.Item("cus_pcode1")), _
                                               CStr(dr.Item("cus_pcode2")), CStr(dr.Item("cus_cntry")), CStr(dr.Item("cus_cntct1")), CStr(dr.Item("cus_cntct2")), CStr(dr.Item("cus_cntct3")), _
                                               CStr(dr.Item("cus_cntct4")), CStr(dr.Item("cus_cntct5")), CStr(dr.Item("cus_tel")), CStr(dr.Item("cus_fax")), CStr(dr.Item("cus_email")), CStr(dr.Item("cus_email2")), _
                                               CType(dr.Item("cus_cdate"), Date?), CType(dr.Item("cus_fdate"), Date?), CType(dr.Item("cus_ldate"), Date?), CDec(dr.Item("cus_onact")), CBool(dr.Item("cus_insure")), CType(dr.Item("cus_insexp"), Date?), _
                                               CStr(dr.Item("cus_sref1")), CStr(dr.Item("cus_sref2")), CStr(dr.Item("cus_sref3")), CStr(dr.Item("cus_sref4")), CStr(dr.Item("cus_sref5")), CStr(dr.Item("cus_sref6")), _
                                               CStr(dr.Item("cus_con1")), CInt(dr.Item("cus_c1mth1")), CInt(dr.Item("cus_c1mth2")), CInt(dr.Item("cus_c1mth3")), CDec(dr.Item("cus_c1pct1")), CDec(dr.Item("cus_c1pct2")), _
                                               CDec(dr.Item("cus_c1pct3")), CDec(dr.Item("cus_c1pct4")), CStr(dr.Item("cus_con2")), CInt(dr.Item("cus_c2mth1")), CInt(dr.Item("cus_c2mth2")), CInt(dr.Item("cus_c2mth3")), _
                                               CDbl(dr.Item("cus_c2pct1")), CDec(dr.Item("cus_c2pct2")), CDec(dr.Item("cus_c2pct3")), CDec(dr.Item("cus_c2pct4")), CStr(dr.Item("cus_ptermc")), CInt(dr.Item("cus_ptermv")), _
                                               CBool(dr.Item("cus_crep")), CBool(dr.Item("cus_state")), CBool(dr.Item("cus_pono")), CBool(dr.Item("cus_costc")), CBool(dr.Item("cus_cref1")), CBool(dr.Item("cus_cref2")), CBool(dr.Item("cus_ccard")), _
                                               CBool(dr.Item("cus_cconly")), CDec(dr.Item("cus_climit")), CDec(dr.Item("cus_debt")), CBool(dr.Item("cus_stop")), CBool(dr.Item("cus_2ndadd")), CStr(dr.Item("cus_notes")), CStr(dr.Item("cus_stadd")), _
                                               CInt(dr.Item("cus_soy")), CStr(dr.Item("cus_conam2")), CStr(dr.Item("cus_pop1")), CStr(dr.Item("cus_pop2")), CDec(dr.Item("cus_ovrdue")), CDec(dr.Item("cus_dscpct")), CBool(dr.Item("cus_fulreb")), _
                                               CBool(dr.Item("cus_hidecm")), CBool(dr.Item("cus_rebcn")), CStr(dr.Item("cus_rebfrq")), CBool(dr.Item("cus_reb1cn")), CBool(dr.Item("cus_rebinv")), CBool(dr.Item("cus_incfee")), _
                                               CBool(dr.Item("cus_nocc")), CStr(dr.Item("cus_feeid1")), CStr(dr.Item("cus_feeid2")), CStr(dr.Item("cus_feeid3")), CBool(dr.Item("cus_adhoc")), CBool(dr.Item("cus_dofee")), _
                                               CBool(dr.Item("cus_ibank")), CStr(dr.Item("cus_feetyp")), CStr(dr.Item("cus_atol")), CBool(dr.Item("cus_paydd")), CBool(dr.Item("cus_sps")), CBool(dr.Item("cus_acas")), CStr(dr.Item("cus_abtano")), _
                                               CBool(dr.Item("cus_mailst")), CBool(dr.Item("cus_mailiv")), CBool(dr.Item("cus_coninv")), CType(dr.Item("cus_cinvdt"), Date?), CBool(dr.Item("cus_keyact")), CStr(dr.Item("cus_invfrm")), _
                                               CBool(dr.Item("cus_noba")), CBool(dr.Item("cus_slfbil")), CBool(dr.Item("cus_exvat")), CStr(dr.Item("cus_curncy")), CBool(dr.Item("cus_onepdf")), _
                                               CStr(dr.Item("cus_spare2")), CBool(dr.Item("cus_dorm")), CBool(dr.Item("cus_mltfee")), CBool(dr.Item("cus_print")), CStr(dr.Item("cus_pcode")), CStr(dr.Item("cus_stfrm")))
                    oCustX.save()
                Next

                myDataSet.Clear()
                log.Info("BOSSCustomer completed successfully")
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", "BOSSCustomer had an error", ex.Message)
                log.Error("CUSTOMER ERROR: ", ex)
            End Try
        End Using
    End Sub

    Private Sub bossProducts()
        Using New MethodLogger(log, "bossProducts")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT Pro_id,Pro_type,Pro_desc,Pro_vatabl,Pro_ccrate,Pro_dccode,Pro_axcode,Pro_glcomm," & _
                                                                       "Pro_glsale,Pro_glcost,Pro_gl0com,Pro_gl0sal,Pro_gl0cos,Pro_crsprd,Pro_paycon,Pro_viacrs," & _
                                                                       "Pro_route,Pro_bsp,Pro_agchg,Pro_postbk,Pro_vicode,Pro_spare,Pro_tpcode,Pro_tpcde2,Pro_disvat " & _
                                                                        "FROM product", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "product")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("product").Rows
                    'sp save will check if exists etc
                    Dim oproduct As New BOSSproduct(0, CStr(dr.Item("Pro_id")), CStr(dr.Item("Pro_type")), CStr(dr.Item("Pro_desc")), CType(dr.Item("Pro_vatabl"), Boolean?), CStr(dr.Item("Pro_ccrate")), _
                                                    CStr(dr.Item("Pro_dccode")), CStr(dr.Item("Pro_axcode")), CStr(dr.Item("Pro_glcomm")), CStr(dr.Item("Pro_glsale")), CStr(dr.Item("Pro_glcost")), _
                                                    CStr(dr.Item("Pro_gl0com")), CStr(dr.Item("Pro_gl0sal")), CStr(dr.Item("Pro_gl0cos")), CStr(dr.Item("Pro_crsprd")), CType(dr.Item("Pro_paycon"), Boolean?), _
                                                    CType(dr.Item("Pro_viacrs"), Boolean?), CType(dr.Item("Pro_route"), Boolean?), CType(dr.Item("Pro_bsp"), Boolean?), CDec(dr.Item("Pro_agchg")), _
                                                    CType(dr.Item("Pro_postbk"), Boolean?), CStr(dr.Item("Pro_vicode")), CStr(dr.Item("Pro_spare")), CStr(dr.Item("Pro_tpcode")), CStr(dr.Item("Pro_tpcde2")), _
                                                    CType(dr.Item("Pro_disvat"), Boolean?))
                    oproduct.save()
                Next

                myDataSet.Clear()
                log.Info("BOSSproduct completed successfully")
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", "BOSSproduct had an error", ex.Message)
                log.Error("PRODUCTS ERROR: ", ex)
            End Try
        End Using
    End Sub

    Private Sub bossProtypes()
        Using New MethodLogger(log, "bossProtypes")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT typ_id,typ_desc,ap_tx,dc_tx,ax_tx,vi_tx,chksum " & _
                                                                       "FROM protype", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "Protype")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("Protype").Rows
                    'sp save will check if exists etc
                    Dim oProtype As New BOSSprotype(0, CStr(dr.Item("typ_id")), CStr(dr.Item("typ_desc")), CStr(dr.Item("ap_tx")), CStr(dr.Item("dc_tx")), CStr(dr.Item("ax_tx")), CStr(dr.Item("vi_tx")), CStr(dr.Item("chksum")))
                    oProtype.save()
                Next

                myDataSet.Clear()
                log.Info("BOSSprotype completed successfully")
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", "BOSSprotype had an error", ex.Message)
                log.Error("PROTYPES ERROR: ", ex)
            End Try
        End Using
    End Sub

    Private Sub bossRegions()
        Using New MethodLogger(log, "bossRegions")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT region,`desc`,rating,single_fee,sector_fee,sec_fee2,loclregion,longhaul,chksum " & _
                                                                       "FROM regions", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "regions")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("regions").Rows
                    'sp save will check if exists etc
                    Dim oRegion As New BOSSregion(0, CStr(dr.Item("region")), CStr(dr.Item("desc")), CDec(dr.Item("rating")), CDec(dr.Item("single_fee")), CDec(dr.Item("sector_fee")), _
                                                  CDec(dr.Item("sec_fee2")), CType(dr.Item("loclregion"), Boolean?), CType(dr.Item("longhaul"), Boolean?), CStr(dr.Item("chksum")))
                    oRegion.save()
                Next

                myDataSet.Clear()
                log.Info("BOSSRegions completed successfully")
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", "bossRegions had an error", ex.Message)
                log.Error("REGIONS ERROR: ", ex)
            End Try
        End Using
    End Sub

    Private Sub bossSaveCodes()
        Using New MethodLogger(log, "bossSaveCodes")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT sav_set,sav_id,sav_desc " & _
                                                                       "FROM savecode", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "savecode")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("savecode").Rows
                    'sp save will check if exists etc
                    Dim oSaveCode As New BOSSsavecode(0, CStr(dr.Item("sav_set")), CStr(dr.Item("sav_id")), CStr(dr.Item("sav_desc")))
                    oSaveCode.save()
                Next

                myDataSet.Clear()
                log.Info("BOSSSaveCodes completed successfully")
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", "bossSaveCodes had an error", ex.Message)
                log.Error("SAVECODES ERROR: ", ex)
            End Try
        End Using
    End Sub

    Public Shared Sub bossSuppliers()
        Using New MethodLogger(log, "bossSuppliers")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT sup_id, sup_desc, sup_type, sup_grpid, sup_comfrm, sup_name, sup_add1, sup_add2, sup_add3, sup_add4, " & _
                                                                       "sup_pcode1, sup_pcode2, sup_cntry, sup_contac, sup_tel, sup_fax, sup_email, sup_email2, sup_cdate, sup_ldate, " &
                                                                       "sup_acode, sup_dccode, sup_atol, sup_atolag, sup_prefrd, sup_paygrs, sup_selfbl, sup_docmnt, cast(sup_dcomp1 as numeric(18,2)) as sup_dcomp1, " & _
                                                                       "cast(sup_dcomp2 as numeric(18,2)) as sup_dcomp2, sup_glcode, cast(sup_pcent as numeric(18,2)) as sup_pcent, sup_xpense, " & _
                                                                       "sup_cterms, sup_bdueto, sup_bduefm, sup_pmeth, sup_pmethc, sup_notes, sup_popup, cast(sup_dppc as numeric(18,2)) as sup_dppc, " & _
                                                                       "cast(sup_dpamt as numeric(18,2)) as sup_dpamt, sup_ubtax, sup_yqtax, sup_ouracc, sup_fullcl, sup_vatabl, sup_vatno " &
                                                                       "FROM supplier", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "Supplier")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("Supplier").Rows
                    'sp save will check if exists etc
                    Dim oSupplier As New BOSSsupplier(0, CStr(dr.Item("sup_id")), CStr(dr.Item("sup_desc")), CStr(dr.Item("sup_type")), CStr(dr.Item("sup_grpid")), CStr(dr.Item("sup_comfrm")), _
                                                      CStr(dr.Item("sup_name")), CStr(dr.Item("sup_add1")), CStr(dr.Item("sup_add2")), CStr(dr.Item("sup_add3")), CStr(dr.Item("sup_add4")), _
                                                      CStr(dr.Item("sup_pcode1")), CStr(dr.Item("sup_pcode2")), CStr(dr.Item("sup_cntry")), CStr(dr.Item("sup_contac")), CStr(dr.Item("sup_tel")), _
                                                      CStr(dr.Item("sup_fax")), CStr(dr.Item("sup_email")), CStr(dr.Item("sup_email2")), CType(dr.Item("sup_cdate"), Date?), CType(dr.Item("sup_ldate"), Date?), _
                                                      CStr(dr.Item("sup_acode")), CStr(dr.Item("sup_dccode")), CStr(dr.Item("sup_atol")), CType(dr.Item("sup_atolag"), Boolean?), CType(dr.Item("sup_prefrd"), Boolean?), _
                                                      CType(dr.Item("sup_paygrs"), Boolean?), CType(dr.Item("sup_selfbl"), Boolean?), CType(dr.Item("sup_docmnt"), Boolean?), CDec(dr.Item("sup_dcomp1")), _
                                                      CDec(dr.Item("sup_dcomp2")), CDec(dr.Item("sup_glcode")), CDec(dr.Item("sup_pcent")), CStr(dr.Item("sup_xpense")), _
                                                      CDec(dr.Item("sup_cterms")), CDec(dr.Item("sup_bdueto")), CDec(dr.Item("sup_bduefm")), CStr(dr.Item("sup_pmeth")), _
                                                      CStr(dr.Item("sup_pmethc")), CStr(dr.Item("sup_notes")), CStr(dr.Item("sup_popup")), CDec(dr.Item("sup_dppc")), CDec(dr.Item("sup_dpamt")), _
                                                      CType(dr.Item("sup_ubtax"), Boolean?), CType(dr.Item("sup_yqtax"), Boolean?), CStr(dr.Item("sup_ouracc")), CStr(dr.Item("sup_fullcl")), _
                                                      CType(dr.Item("sup_vatabl"), Boolean?), CStr(dr.Item("sup_vatno")))
                    oSupplier.save()
                Next

                myDataSet.Clear()
                log.Info("BOSSSupplier completed successfully")
            Catch ex As Exception
                ' sendEmailLocal("nick.massarella@nysgroup.com", "nick.massarella@nysgroup.com", "BOSSSupplier had an error", ex.Message)
                log.Error("SUPPLIER ERROR: ", ex)
            End Try
        End Using
    End Sub

    Private Sub bossCityCodes()
        Using New MethodLogger(log, "bossCityCodes")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT cty_id,cty_airprt,cty_cty,cty_name,cty_cntry,cty_region,cty_elgkey,cty_cntryc " & _
                                                                    "FROM citycode", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "CityCodes")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("CityCodes").Rows
                    'sp save will check if exists etc
                    Dim oCity As New BOSScitycode(0, CStr(dr.Item("cty_id")), CStr(dr.Item("cty_airprt")), CStr(dr.Item("cty_cty")), CStr(dr.Item("cty_name")), _
                                                  CStr(dr.Item("cty_cntry")), CStr(dr.Item("cty_region")), CStr(dr.Item("cty_elgkey")), CStr(dr.Item("cty_cntryc")))
                    oCity.save()
                Next

                myDataSet.Clear()
                log.Info("BOSScitycode completed successfully")
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", "BOSScitycode had an error", ex.Message)
                log.Error("CITYCODE ERROR: ", ex)
            End Try
        End Using
    End Sub

    Private Sub runMainSelect(ByVal pbHourly As Boolean, ByVal pbNightly As Boolean, ByVal pbMonthly As Boolean, ByVal pType As Integer)
        Using New MethodLogger(log, "runMainSelect")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()
            Dim strCurrentRef As String = ""
            Dim strLastRef As String = ""
            Dim intCount As Integer = 0
            Dim strSubject As String = "runMainSelect hourly "

            Try
                Dim strWhere As String = ""
                If pbHourly Then
                    strWhere = "WHERE (inm_dla = DATE() or inm_invdt = DATE() or inm_cdate = DATE())"
                ElseIf pbNightly Then
                    strWhere = "WHERE (" & _
                                            "(inm_dla >= DATE()-2 and inm_dla <= DATE())" & _
                                            " or " & _
                                            "(inm_invdt >= DATE()-2 and inm_invdt <= DATE())" & _
                                            " or " & _
                                            "(inm_cdate >= DATE()-2 and inm_cdate <= DATE())" & _
                                        ")"
                ElseIf pbMonthly Then
                    strWhere = "WHERE (" & _
                                            "(inm_dla >= DATE()-30 and inm_dla <= DATE())" & _
                                            " or " & _
                                            "(inm_invdt >= DATE()-30 and inm_invdt <= DATE())" & _
                                            " or " & _
                                            "(inm_cdate >= DATE()-30 and inm_cdate <= DATE())" & _
                                        ")"
                End If

                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT DISTINCT inm_no FROM invmain " & strWhere, dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "Main")
                dBaseConnection.Close()

                'R2.0 SA 
                log.Info("Loop through every record started")
                For Each dr As DataRow In myDataSet.Tables("Main").Rows
                    If dr.Item("inm_no").ToString.Contains(Chr(187)) = False Then

                        If selectInvMainRecords(dr.Item("inm_no").ToString) Then
                            If selectInvtotRecord(dr.Item("inm_no").ToString) Then
                                If selectInvRouteRecords(dr.Item("inm_no").ToString) Then

                                End If
                            End If
                        End If

                    End If
                Next
                'R2.0 SA 
                log.Info("Loop through every record completed")

                If pType = 2 Then
                    strSubject = "runMainSelect nightly "
                ElseIf pType = 3 Then
                    strSubject = "runMainSelect sunday "
                End If

                myDataSet.Clear()
                log.Info(strSubject & "completed successfully")
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", strSubject & "had an error for:" & strCurrentRef, "Do nothing")
                log.Error("INVOICE ERROR: " & strCurrentRef, ex)
            End Try
        End Using
    End Sub

    Private Sub runCashSelect(ByVal pbHourly As Boolean, ByVal pbNightly As Boolean, ByVal pbAll As Boolean)
        Using New MethodLogger(log, "runCashSelect")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()
            Dim strCurrentRef As String = ""
            Dim strLastRef As String = ""
            Dim intCount As Integer = 0
            Dim strSubject As String = "runCashSelect hourly "

            Try
                Dim strWhere As String = ""
                If pbHourly Then
                    strWhere = "WHERE (csd_cdate >= DATE() or csd_today >= DATE())"
                ElseIf pbNightly Then
                    strWhere = "WHERE (" & _
                                            "(csd_cdate >= DATE()-2)" & _
                                            " or " & _
                                            "(csd_today >= DATE()-2)" & _
                                        ")"
                    strSubject = "runCashSelect nightly "
                ElseIf pbAll Then
                    strWhere = "where `YEAR`(csd_cdate) = 2009 or `YEAR`(csd_cdate) = 2010 or `YEAR`(csd_cdate) = 2011 or `YEAR`(csd_today) = 2009 or `YEAR`(csd_today) = 2010 or `YEAR`(csd_today) = 2011"
                    strSubject = "runCashSelect all "
                End If
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT csd_key, csd_appid, csd_to, csd_applyd, csd_appkey, csd_invkey, csd_applgr, csd_arkey, " & _
                                                                        "csd_cdate, csd_today, csd_suppid, csd_prdkey, csd_ok, csd_final, csd_note, csd_locked, csd_status, " & _
                                                                        "csd_who, csd_ukey, chksum " & _
                                                                        "FROM cash " & _
                                                                        strWhere, dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "Cash")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("Cash").Rows
                    'BOSScash.delete(0, dr.Item("Csd_ukey").ToString)
                    Dim oCash As New BOSScash(0, CStr(dr.Item("csd_key")), CStr(dr.Item("csd_appid")), CStr(dr.Item("csd_to")), CStr(dr.Item("csd_applyd")), CStr(dr.Item("csd_appkey")), CStr(dr.Item("csd_invkey")), _
                                                  CStr(dr.Item("csd_applgr")), CStr(dr.Item("csd_arkey")), CType(dr.Item("csd_cdate"), Date?), CType(dr.Item("csd_today"), Date?), CStr(dr.Item("csd_suppid")), CStr(dr.Item("csd_prdkey")), _
                                                  CStr(dr.Item("csd_ok")), CStr(dr.Item("csd_final")), CStr(dr.Item("csd_note")), CStr(dr.Item("csd_locked")), CStr(dr.Item("csd_status")), CStr(dr.Item("csd_who")), _
                                                  CStr(dr.Item("csd_ukey")), CStr(dr.Item("chksum")), Now)
                    oCash.save()
                Next

                myDataSet.Clear()
                log.Info(strSubject & "completed successfully")
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", strSubject & "had an error for:" & strCurrentRef, "Do nothing")
                log.Error("INVOICE ERROR: " & strCurrentRef, ex)
            End Try
        End Using
    End Sub

    Private Function selectInvtotRecord(ByVal pstrInvoiceRef As String) As Boolean
        Using New MethodLogger(log, "selectInvtotRecord")

            'R2.0 SA 
            log.Info("Start InvTot update")
            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()
            Dim strLocalRef As String = ""

            Try
                'R2.0 SA 
                log.Info("Invtot Get Invoice details from SQL")
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT tot_ukey,tot_custid,tot_invno,tot_change,tot_crsref,tot_br,tot_pono,tot_costc," & _
                                                                                   "tot_type,tot_invdt,tot_duedt,tot_fare,tot_tax,tot_srvchg,tot_ourchg,tot_ourvat," & _
                                                                                   "tot_supvat,tot_amtvat,tot_bilvat,tot_amount,tot_billed,tot_discnt,tot_ccamt," & _
                                                                                   "tot_recvd,tot_dposit,tot_comm,tot_comdue,tot_vtoncm,tot_disput,tot_reason," & _
                                                                                   "tot_noerrs,tot_note,tot_raddr,tot_rtelno,tot_paxs,tot_morcrs,tot_retail," & _
                                                                                   "tot_bdm,tot_print,tot_errs,tot_atol,tot_atolx,tot_cref1,tot_cref2,tot_agcomm," & _
                                                                                   "tot_agvat,tot_fileno,tot_nofee,tot_crstyp,tot_noprnt,tot_email,tot_curncy," & _
                                                                                   "tot_roe,tot_cursym " & _
                                                                                   "FROM Invtot where tot_invno = '" & pstrInvoiceRef & "'", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "Main")
                dBaseConnection.Close()

                Dim dtCreated As Date = BOSSinvmain.BOSSinvmainCdate(pstrInvoiceRef)

                For Each dr As DataRow In myDataSet.Tables("Main").Rows
                    'delete any existing record
                    'R2.0 SA 
                    log.Info("Invtot start Delete Invoice details from SQL")
                    BOSSinvtot.delete(0, CStr(dr.Item("tot_invno")))
                    log.Info("Invtot end Delete Invoice details from SQL") 'R2.0 SA
                    'save tot record
                    strLocalRef = dr.Item("tot_invno").ToString

                    If dr.Item("tot_invno").ToString.Contains(Chr(187)) = False Then
                        Dim oTot As New BOSSinvtot(0, CStr(dr.Item("tot_ukey")), CStr(dr.Item("tot_custid")), CStr(dr.Item("tot_invno")), CInt(dr.Item("tot_change")), CStr(dr.Item("tot_crsref")), _
                                              CStr(dr.Item("tot_br")), CStr(dr.Item("tot_pono")), CStr(dr.Item("tot_costc")), CStr(dr.Item("tot_type")), CDate(dr.Item("tot_invdt")), CDate(dr.Item("tot_duedt")), _
                                              CDec(dr.Item("tot_fare")), CDec(dr.Item("tot_tax")), CDec(dr.Item("tot_srvchg")), CDec(dr.Item("tot_ourchg")), CDec(dr.Item("tot_ourvat")), _
                                              CDec(dr.Item("tot_supvat")), CDec(dr.Item("tot_amtvat")), CDec(dr.Item("tot_bilvat")), CDec(dr.Item("tot_amount")), CDec(dr.Item("tot_billed")), _
                                              CDec(dr.Item("tot_discnt")), CDec(dr.Item("tot_ccamt")), CDec(dr.Item("tot_recvd")), CDec(dr.Item("tot_dposit")), CDec(dr.Item("tot_comm")), _
                                              CDec(dr.Item("tot_comdue")), CDec(dr.Item("tot_vtoncm")), CBool(dr.Item("tot_disput")), CStr(dr.Item("tot_reason")), CBool(dr.Item("tot_noerrs")), _
                                              CStr(dr.Item("tot_note")), CStr(dr.Item("tot_raddr")), CStr(dr.Item("tot_rtelno")), CInt(dr.Item("tot_paxs")), CStr(dr.Item("tot_morcrs")), _
                                              CBool(dr.Item("tot_retail")), CStr(dr.Item("tot_bdm")), CInt(dr.Item("tot_print")), CBool(dr.Item("tot_errs")), CBool(dr.Item("tot_atol")), _
                                              CStr(dr.Item("tot_atolx")), CStr(dr.Item("tot_cref1")), CStr(dr.Item("tot_cref2")), CDec(dr.Item("tot_agcomm")), CDec(dr.Item("tot_agvat")), _
                                              CStr(dr.Item("tot_fileno")), CBool(dr.Item("tot_nofee")), CStr(dr.Item("tot_crstyp")), CBool(dr.Item("tot_noprnt")), CStr(dr.Item("tot_email")), _
                                              CStr(dr.Item("tot_curncy")), CDec(dr.Item("tot_roe")), CStr(dr.Item("tot_cursym")), dtCreated)
                        'R2.0 SA 
                        log.Info("Invtot start Save Invoice details into SQL")
                        oTot.save()
                        log.Info("Invtot end Save Invoice details into SQL")

                        'R1.2 NM 
                        'need to update invref table if any values there 
                        'R2.0 SA 
                        log.Info("Invref start Invoice update details")
                        If Not selectInvRefs(CStr(dr.Item("tot_ukey")), CStr(dr.Item("tot_invno"))) Then
                            sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", "selectInvRefs had an error for:" & strLocalRef, "Do something")
                            log.Error("selectInvRefs ERROR: " & strLocalRef)
                            Return False
                        End If
                        'R2.0 SA 
                        log.Info("Invref end Invoice update details")
                    End If
                Next

                myDataSet.Clear()
                Return True
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", "selectInvtotRecord had an error for:" & strLocalRef, "Do something:" & ex.Message)
                log.Error("INVOICE ERROR: " & strLocalRef, ex)
                Return False
            End Try

            log.Info("End InvTot update")
        End Using
    End Function

    Private Function selectInvtotRecvd() As Boolean
        Using New MethodLogger(log, "selectInvtotRecvd")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()
            Dim strWhere As String = "where ("

            Try
                For i As Integer = CInt(getConfig("TotRecvdStartYear")) To CInt(getConfig("TotRecvdEndYear"))
                    strWhere = strWhere & " `YEAR`(tot_invdt) = " & i & " or "
                Next

                strWhere = Mid(strWhere, 1, Len(strWhere) - 3)
                strWhere = strWhere & ")"

                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT tot_ukey,tot_invno,tot_recvd " & _
                                                                        "FROM Invtot " & _
                                                                        strWhere, dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "selectInvtotRecvd")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("selectInvtotRecvd").Rows
                    BOSSinvtot.saveRecvd(CStr(dr.Item("tot_ukey")), CStr(dr.Item("tot_invno")), CDec(dr.Item("tot_recvd")))
                Next

                myDataSet.Clear()
                Return True
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", "selectInvtotRecvd had an error", "Do something:" & ex.Message)
                log.Error("INVTOTRECVD ERROR", ex)
                Return False
            End Try
        End Using
    End Function

    Private Function runPayhdr(ByVal pbHourly As Boolean, ByVal pbNightly As Boolean, ByVal pstrRef As String) As Boolean
        Using New MethodLogger(log, "runPayhdr")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()
            Dim strSubject As String = "runPayhdr hourly "

            Try
                Dim strWhere As String = ""
                If pbHourly Then
                    strWhere = "WHERE (pyh_cdate >= DATE() or pyh_date >= DATE())"
                ElseIf pbNightly Then
                    strWhere = "WHERE (" & _
                                            "(pyh_cdate >= DATE()-2)" & _
                                            " or " & _
                                            "(pyh_date >= DATE()-2)" & _
                                        ")"
                    strSubject = "runPayhdr nightly "
                Else
                    strWhere = "WHERE pyh_key = '" & pstrRef & "'"
                End If

                'R??? CR - cast all numeric values from BOSS, otherwise results in SQL rounding up the figure
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT pyh_key, pyh_type, pyh_cdate, pyh_date, pyh_bnkac, pyh_bnklgr, pyh_bnkref, pyh_branch, pyh_payee, pyh_suptyp, " & _
                                                                       "pyh_paynam, cast(pyh_net as numeric(18,2)) as pyh_net, cast(pyh_vat as numeric(18,2)) as pyh_vat, cast(pyh_bnkchg as numeric(18,2)) as pyh_bnkchg, cast(pyh_amt as numeric(18,2)) as pyh_amt, cast(pyh_used as numeric(18,2)) as pyh_used, cast(pyh_remain as numeric(18,2)) as pyh_remain, pyh_who, pyh_recon, pyh_recdte, pyh_note, pyh_locked, " & _
                                                                       "pyh_recur, pyh_reckey, pyh_freq, pyh_rstart, cast(pyh_howmny as numeric(18,2)) as pyh_howmny " & _
                                                                       "FROM payhdr " & _
                                                                        strWhere, dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "runPayhdr")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("runPayhdr").Rows
                    Dim oPayhdr As New BOSSpayhdr(0, CStr(dr.Item("pyh_key")), CStr(dr.Item("pyh_type")), CType(dr.Item("pyh_cdate"), Date?), CType(dr.Item("pyh_date"), Date?), _
                                                  CStr(dr.Item("pyh_bnkac")), CStr(dr.Item("pyh_bnklgr")), CStr(dr.Item("pyh_bnkref")), CStr(dr.Item("pyh_branch")), CStr(dr.Item("pyh_payee")), _
                                                  CStr(dr.Item("pyh_suptyp")), CStr(dr.Item("pyh_paynam")), CDec(dr.Item("pyh_net")), CDec(dr.Item("pyh_vat")), CDec(dr.Item("pyh_bnkchg")), _
                                                  CDec(dr.Item("pyh_amt")), CDec(dr.Item("pyh_used")), CDec(dr.Item("pyh_remain")), CStr(dr.Item("pyh_who")), CType(dr.Item("pyh_recon"), Boolean?), _
                                                  CType(dr.Item("pyh_recdte"), Date?), CStr(dr.Item("pyh_note")), CType(dr.Item("pyh_locked"), Boolean?), CType(dr.Item("pyh_recur"), Boolean?), _
                                                  CStr(dr.Item("pyh_reckey")), CStr(dr.Item("pyh_freq")), CType(dr.Item("pyh_rstart"), Date?), CDec(dr.Item("pyh_howmny")))
                    oPayhdr.save()
                Next

                myDataSet.Clear()
                log.Info("runPayhdr completed successfully")
                Return True
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", "runPayhdr had an error", "Do something:" & ex.Message)
                log.Error("PAYHDR ERROR", ex)
                Return False
            End Try
        End Using
    End Function

    Private Function runPaydet(ByVal pbHourly As Boolean, ByVal pbNightly As Boolean, ByVal pstrInvoiceRef As String) As Boolean
        Using New MethodLogger(log, "runPaydet")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()
            Dim strSubject As String = "runPaydet hourly "

            Try
                Dim strWhere As String = ""

                If pbHourly Then
                    strWhere = "WHERE (pyd_cdate >= DATE() or pyd_today >= DATE())"
                ElseIf pbNightly Then
                    strWhere = "WHERE (" & _
                                            "(pyd_cdate >= DATE()-2)" & _
                                            " or " & _
                                            "(pyd_today >= DATE()-2)" & _
                                        ")"
                    strSubject = "runPaydet nightly "
                Else
                    strWhere = "WHERE pyd_applyd = '" & pstrInvoiceRef & "'"
                End If

                'R??? CR - cast all numeric values from BOSS, otherwise results in SQL rounding up the figure
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT pyd_key, pyd_line, pyd_ctrl, pyd_cdate, pyd_today, pyd_branch, pyd_ledger, pyd_applyd, cast(pyd_dr as numeric(18,2)) as pyd_dr," & _
                                                                        "cast(pyd_cr as numeric(18,2)) as pyd_cr, pyd_apkey, pyd_ukey, pyd_locked, pyd_recon, pyd_recdte, pyd_ok, pyd_who " & _
                                                                        "FROM paydetl " & _
                                                                        strWhere, dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "selectPaydet")
                dBaseConnection.Close()

                'first run through and bin all related records as BOSS cannot be trusted!
                For Each dr As DataRow In myDataSet.Tables("selectPaydet").Rows
                    BOSSpaydetl.delete(0, "", CStr(dr.Item("pyd_applyd")))
                Next

                For Each dr As DataRow In myDataSet.Tables("selectPaydet").Rows
                    Dim oPaydet As New BOSSpaydetl(0, CStr(dr.Item("pyd_key")), CStr(dr.Item("pyd_line")), CType(dr.Item("pyd_ctrl"), Boolean?), CStr(dr.Item("pyd_cdate")), CStr(dr.Item("pyd_today")), _
                                                   CStr(dr.Item("pyd_branch")), CStr(dr.Item("pyd_ledger")), CStr(dr.Item("pyd_applyd")), CDec(dr.Item("pyd_dr")), CDec(dr.Item("pyd_cr")), _
                                                   CStr(dr.Item("pyd_apkey")), CStr(dr.Item("pyd_ukey")), CType(dr.Item("pyd_locked"), Boolean?), CType(dr.Item("pyd_recon"), Boolean?), _
                                                   CStr(dr.Item("pyd_recdte")), CType(dr.Item("pyd_ok"), Boolean?), CStr(dr.Item("pyd_who")))
                    oPaydet.save()
                    runPayhdr(False, False, dr.Item("pyd_key").ToString)
                Next

                myDataSet.Clear()
                log.Info("runPaydet completed successfully")
                Return True
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", "selectPaydet had an error", "Do something:" & ex.Message)
                log.Error("PAYDET ERROR", ex)
                Return False
            End Try
        End Using
    End Function

    Private Function selectInvMainComm() As Boolean
        Using New MethodLogger(log, "selectInvMainComm")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()
            Dim strWhere As String = "where ("

            Try
                For i As Integer = CInt(getConfig("TotRecvdStartYear")) To CInt(getConfig("TotRecvdEndYear"))
                    strWhere = strWhere & " `YEAR`(inm_invdt) = " & i & " or `YEAR`(inm_cdate) = " & i & " or "
                Next

                strWhere = Mid(strWhere, 1, Len(strWhere) - 3)
                strWhere = strWhere & ")"

                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT inm_no, cast(inm_comrcv as numeric(18,2)) as inm_comrcv, cast(inm_comdue as numeric(18,2)) as inm_comdue, inm_ukey " & _
                                                                         "FROM invmain " & _
                                                                         strWhere, dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "MainComm")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("MainComm").Rows
                    BOSSinvmain.saveComm(dr.Item("inm_no").ToString, dr.Item("inm_ukey").ToString, CDec(dr.Item("inm_comrcv")), CDec(dr.Item("inm_comdue")))
                Next

                myDataSet.Clear()
                log.Info("selectInvMainComm completed successfully")
                Return True
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", "selectInvMainComm had an error", "Do something:" & ex.Message)
                log.Error("INVMAIN COMM ERROR", ex)
                Return False
            End Try
        End Using
    End Function

    Private Function selectInvMainRecords(ByVal pstrInvoiceRef As String) As Boolean
        Using New MethodLogger(log, "selectInvMainRecords")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try
                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT inm_no, inm_line, inm_retail, inm_br, inm_type, inm_invdt, inm_entry, inm_custid, inm_costc, inm_pono, inm_cdate, inm_prod, inm_suppid, inm_supnam," & _
                                         "inm_htltel, inm_bsp, inm_docmnt, inm_etkt, inm_books, inm_exch, inm_origtk, inm_online, inm_bspdte, inm_start, inm_end, inm_ldname, inm_savecd," & _
                                         "cast(inm_mxfare as numeric(18,2)) as inm_mxfare," & _
                                         "cast(inm_lwfare as numeric(18,2)) as inm_lwfare," & _
                                         "inm_bookng, inm_fcrncy," & _
                                         "cast(inm_ffare as numeric(18,2)) as inm_ffare," & _
                                         "cast(inm_drate as numeric(18,2)) as inm_drate," & _
                                         "cast(inm_days as numeric(18,2)) as inm_days," & _
                                         "inm_curncy," & _
                                         "cast(inm_fare as numeric(18,2)) as inm_fare," & _
                                         "cast(inm_srvchg as numeric(18,2)) as inm_srvchg," & _
                                         "cast(inm_ubtax as numeric(18,2)) as inm_ubtax," & _
                                         "cast(inm_yqtax as numeric(18,2)) as inm_yqtax," & _
                                         "cast(inm_othtax as numeric(18,2)) as inm_othtax," & _
                                         "cast(inm_tax as numeric(18,2)) as inm_tax," & _
                                         "cast(inm_ourchg as numeric(18,2)) as inm_ourchg," & _
                                         "cast(inm_amtvat as numeric(18,2)) as inm_amtvat," & _
                                         "cast(inm_supvat as numeric(18,2)) as inm_supvat," & _
                                         "cast(inm_ourvat as numeric(18,2)) as inm_ourvat," & _
                                         "cast(inm_amount as numeric(18,2)) as inm_amount," & _
                                         "cast(inm_dscpct as numeric(18,2)) as inm_dscpct," & _
                                         "cast(inm_discnt as numeric(18,2)) as inm_discnt," & _
                                         "cast(inm_billed as numeric(18,2)) as inm_billed," & _
                                         "cast(inm_bilvat as numeric(18,2)) as inm_bilvat," & _
                                         "cast(inm_compct as numeric(18,2)) as inm_compct," & _
                                         "cast(inm_comvat as numeric(18,2)) as inm_comvat," & _
                                         "cast(inm_vtoncm as numeric(18,2)) as inm_vtoncm," & _
                                         "cast(inm_trucom as numeric(18,2)) as inm_trucom," & _
                                         "cast(inm_othcom as numeric(18,2)) as inm_othcom," & _
                                         "cast(inm_comamt as numeric(18,2)) as inm_comamt," & _
                                         "cast(inm_comrcv as numeric(18,2)) as inm_comrcv," & _
                                         "cast(inm_comdue as numeric(18,2)) as inm_comdue," & _
                                         "inm_paynet, inm_vatinv, inm_cominv, inm_morpax, inm_dposit, inm_depok, inm_depbr, inm_depbnk," & _
                                         "inm_baldue, inm_paytyp, inm_ccid, inm_ccno, inm_ccstdt, inm_ccexp, inm_ccauth, inm_cccvv, inm_issue, inm_merch, inm_mfee, inm_ccmeth, inm_ccamt," & _
                                         "inm_ccxmit, inm_print, inm_con1pd, inm_con2pd, inm_orig, inm_locked, inm_erflag, inm_erdesc, inm_ukey, inm_change, inm_who, inm_note, inm_crsref, inm_bdm," & _
                                         "inm_pcity, inm_ino, inm_cos, inm_domint, inm_apok, inm_arok, inm_voided, inm_agcomm, inm_agvat, inm_agdcpc, inm_atol, inm_abond, inm_afare, inm_aheads," & _
                                         "inm_nfare, inm_rebate, inm_fee, inm_feevt, inm_feebas, inm_dla, inm_bywho, inm_cinvrf, tmpfld, inm_3rdpty, inm_ourcc, inm_gcid, inm_gcno, inm_itcode," & _
                                         "inm_bkcur, inm_bkroe, inm_miles1, inm_miles2, inm_km1, inm_km2, cast(inm_disvat as numeric(18,2)) as inm_disvat, chksum " & _
                                         "FROM invmain " & _
                                         "WHERE inm_no = '" & pstrInvoiceRef & "'", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "Main")
                dBaseConnection.Close()

                'first delete any records found in SQL
                BOSSinvmain.delete(0, pstrInvoiceRef)

                For Each dr As DataRow In myDataSet.Tables("Main").Rows
                    'then save main record to SQL

                    Dim oIMain As New BOSSinvmain(0, dr.Item("inm_no").ToString, CDbl(dr.Item("inm_line")), CBool(dr.Item("inm_retail")), CStr(dr.Item("inm_br")), CStr(dr.Item("inm_type")), CDate(dr.Item("inm_invdt")), _
                                                    CStr(dr.Item("inm_entry")), CStr(dr.Item("inm_custid")), CStr(dr.Item("inm_costc")), CStr(dr.Item("inm_pono")), CDate(dr.Item("inm_cdate")), CStr(dr.Item("inm_prod")), _
                                                    CStr(dr.Item("inm_suppid")), CStr(dr.Item("inm_supnam")), CStr(dr.Item("inm_htltel")), CBool(dr.Item("inm_bsp")), CStr(dr.Item("inm_docmnt")), CStr(dr.Item("inm_etkt")), _
                                                    CDbl(dr.Item("inm_books")), CStr(dr.Item("inm_exch")), CStr(dr.Item("inm_origtk")), CBool(dr.Item("inm_online")), CDate(dr.Item("inm_bspdte")), CDate(dr.Item("inm_start")), _
                                                    CDate(dr.Item("inm_end")), CStr(dr.Item("inm_ldname")), CStr(dr.Item("inm_savecd")), CDec(dr.Item("inm_mxfare")), CDec(dr.Item("inm_lwfare")), CBool(dr.Item("inm_bookng")), _
                                                    CStr(dr.Item("inm_fcrncy")), CDec(dr.Item("inm_ffare")), CDec(dr.Item("inm_drate")), CDec(dr.Item("inm_days")), CStr(dr.Item("inm_curncy")), CDec(dr.Item("inm_fare")), _
                                                    CDec(dr.Item("inm_srvchg")), CDec(dr.Item("inm_ubtax")), CDec(dr.Item("inm_yqtax")), CDec(dr.Item("inm_othtax")), CDec(dr.Item("inm_tax")), CDec(dr.Item("inm_ourchg")), _
                                                    CDec(dr.Item("inm_amtvat")), CDec(dr.Item("inm_supvat")), CDec(dr.Item("inm_ourvat")), CDec(dr.Item("inm_amount")), CDec(dr.Item("inm_dscpct")), CDec(dr.Item("inm_discnt")), _
                                                    CDec(dr.Item("inm_billed")), CDec(dr.Item("inm_bilvat")), CDec(dr.Item("inm_compct")), CDec(dr.Item("inm_comvat")), CDec(dr.Item("inm_vtoncm")), CDec(dr.Item("inm_trucom")), _
                                                    CDec(dr.Item("inm_othcom")), CDec(dr.Item("inm_comamt")), CDec(dr.Item("inm_comrcv")), CDec(dr.Item("inm_comdue")), CBool(dr.Item("inm_paynet")), CBool(dr.Item("inm_vatinv")), CBool(dr.Item("inm_cominv")), CStr(dr.Item("inm_morpax")), CDec(dr.Item("inm_dposit")), _
                                                    CBool(dr.Item("inm_depok")), CStr(dr.Item("inm_depbr")), CStr(dr.Item("inm_depbnk")), CDate(dr.Item("inm_baldue")), CStr(dr.Item("inm_paytyp")), CStr(dr.Item("inm_ccid")), _
                                                    CStr(dr.Item("inm_ccno")), CStr(dr.Item("inm_ccstdt")), CStr(dr.Item("inm_ccexp")), CStr(dr.Item("inm_ccauth")), CDbl(dr.Item("inm_issue")), CBool(dr.Item("inm_merch")), _
                                                    CDec(dr.Item("inm_mfee")), CStr(dr.Item("inm_ccmeth")), CDec(dr.Item("inm_ccamt")), CDate(dr.Item("inm_ccxmit")), CDbl(dr.Item("inm_print")), CDate(dr.Item("inm_con1pd")), _
                                                    CDate(dr.Item("inm_con2pd")), CStr(dr.Item("inm_orig")), CBool(dr.Item("inm_locked")), CBool(dr.Item("inm_erflag")), CStr(dr.Item("inm_erdesc")), CStr(dr.Item("inm_ukey")), _
                                                    CBool(dr.Item("inm_change")), CStr(dr.Item("inm_who")), CStr(dr.Item("inm_note")), CStr(dr.Item("inm_crsref")), CStr(dr.Item("inm_bdm")), CStr(dr.Item("inm_pcity")), _
                                                    CStr(dr.Item("inm_ino")), CStr(dr.Item("inm_cos")), CStr(dr.Item("inm_domint")), CBool(dr.Item("inm_apok")), CBool(dr.Item("inm_arok")), CStr(dr.Item("inm_voided")), _
                                                    CDec(dr.Item("inm_agcomm")), CDec(dr.Item("inm_agvat")), CDec(dr.Item("inm_agdcpc")), CBool(dr.Item("inm_atol")), CStr(dr.Item("inm_abond")), CDec(dr.Item("inm_afare")), _
                                                    CDec(dr.Item("inm_aheads")), CBool(dr.Item("inm_nfare")), CDec(dr.Item("inm_rebate")), CDec(dr.Item("inm_fee")), CDec(dr.Item("inm_feevt")), CStr(dr.Item("inm_feebas")), _
                                                    CDate(dr.Item("inm_dla")), CStr(dr.Item("inm_bywho")), CStr(dr.Item("inm_cinvrf")), CStr(dr.Item("tmpfld")), CBool(dr.Item("inm_3rdpty")), CStr(dr.Item("inm_ourcc")), _
                                                    CStr(dr.Item("inm_itcode")), CStr(dr.Item("inm_bkcur")), CDec(dr.Item("inm_bkroe")), CStr(dr.Item("chksum")), Now, CStr(dr.Item("inm_miles1")), CStr(dr.Item("inm_miles2")), _
                                                    CStr(dr.Item("inm_km1")), CStr(dr.Item("inm_km2")), CDec(dr.Item("inm_disvat")), CStr(dr.Item("inm_cccvv")), CStr(dr.Item("inm_gcid")), CStr(dr.Item("inm_gcno")))
                    oIMain.save()
                Next

                myDataSet.Clear()
                Return True
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", "selectInvMainRecords had an error for:" & pstrInvoiceRef, "Do something:" & ex.Message)
                log.Error("INVOICE ERROR: " & pstrInvoiceRef, ex)
                Return False
            End Try
        End Using
    End Function

    Private Function selectInvRouteRecords(ByVal pstrInvoiceRef As String) As Boolean
        Using New MethodLogger(log, "selectInvRouteRecords")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try

                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT inr_key, inr_invno, inr_prod, inr_bookno, inr_segno, inr_from, inr_to, inr_sttime," & _
                                                                         "inr_start, inr_stterm, inr_etime, inr_end, inr_eterm, inr_flwncr, inr_flight, inr_class," & _
                                                                         "inr_bpmt, inr_fbasis, inr_fare, inr_dest, inr_miles, inr_fee, inr_feevt, inr_feebas,inr_status " & _
                                                                         "FROM invroute " & _
                                                                         "WHERE inr_invno = '" & pstrInvoiceRef & "'", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "selectInvRouteRecords")
                dBaseConnection.Close()

                'first delete any records found in SQL
                BOSSinvroute.delete(0, pstrInvoiceRef)

                For Each dr As DataRow In myDataSet.Tables("selectInvRouteRecords").Rows
                    'then save main record to SQL
                    Dim oRt As New BOSSinvroute(0, CStr(dr.Item("inr_key")), CStr(dr.Item("inr_invno")), CStr(dr.Item("inr_prod")), CInt(dr.Item("inr_bookno")), CInt(dr.Item("inr_segno")), _
                                                CStr(dr.Item("inr_from")), CStr(dr.Item("inr_to")), CStr(dr.Item("inr_sttime")), CDate(dr.Item("inr_start")), CStr(dr.Item("inr_stterm")), _
                                                CStr(dr.Item("inr_etime")), CDate(dr.Item("inr_end")), CStr(dr.Item("inr_eterm")), CStr(dr.Item("inr_flwncr")), CStr(dr.Item("inr_flight")), _
                                                CStr(dr.Item("inr_class")), CDec(dr.Item("inr_bpmt")), CStr(dr.Item("inr_fbasis")), CDec(dr.Item("inr_fare")), CBool(dr.Item("inr_dest")), _
                                                CInt(dr.Item("inr_miles")), CDec(dr.Item("inr_fee")), CDec(dr.Item("inr_feevt")), CStr(dr.Item("inr_feebas")), CStr(dr.Item("inr_status")), _
                                                Now)
                    oRt.save()
                Next

                myDataSet.Clear()
                Return True
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", "selectInvRouteRecords had an error for:" & pstrInvoiceRef, "Do something:" & ex.Message)
                log.Error("INVOICE ERROR: " & pstrInvoiceRef, ex)
                Return False
            End Try
        End Using
    End Function

    Private Function selectInvRefs(ByVal pstrUkey As String, ByVal pstrInvoiceRef As String) As Boolean
        Using New MethodLogger(log, "selectInvRefs")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()

            Try

                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT ref_ukey, ref_invno, ref_3, ref_4, ref_5, ref_6, ref_7, ref_8, ref_9 " & _
                                                                       "FROM invrefs " & _
                                                                       "WHERE ref_ukey = '" & pstrUkey & "'", dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "selectInvRefs")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("selectInvRefs").Rows
                    'then save main record to SQL
                    Dim oRt As New BOSSinvref(0, pstrUkey, pstrInvoiceRef, CStr(dr.Item("ref_3")), CStr(dr.Item("ref_4")), _
                                              CStr(dr.Item("ref_5")), CStr(dr.Item("ref_6")), CStr(dr.Item("ref_7")), CStr(dr.Item("ref_8")), CStr(dr.Item("ref_9")))
                    oRt.save()
                Next

                myDataSet.Clear()
                Return True
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", "selectInvRefs had an error", "Do something:" & ex.Message)
                log.Error("selectInvRefs ERROR", ex)
                Return False
            End Try
        End Using
    End Function

    Private Function sendEmailLocal(ByVal pstrFrom As String, _
                              ByVal pstrTo As String, _
                              ByVal pstrSubject As String, _
                              ByVal pstrMessage As String) As Boolean
        Using New MethodLogger(log, "sendEmailLocal")

            Dim MailMsg As New System.Net.Mail.MailMessage(New MailAddress(pstrFrom.Trim()), New MailAddress(pstrTo))

            MailMsg.BodyEncoding = Encoding.Default
            MailMsg.Subject = pstrSubject.Trim()

            'R21 CR
            Dim strbody As String = "<html><font face='Verdana' size='2'>" & pstrMessage & "</font></html>"
            strbody = strbody.Replace("""", "'")

            MailMsg.IsBodyHtml = True
            MailMsg.Body = strbody

            Try
                'Smtpclient to send the mail message
                Dim SmtpMail As New SmtpClient
                SmtpMail.Send(MailMsg)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Using
    End Function

    Private Sub runCashHdr(ByVal pbHourly As Boolean, ByVal pbNightly As Boolean, ByVal pbAll As Boolean)
        Using New MethodLogger(log, "runCashHdr")

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(getConfig("BOSSConnectionString"))
            Dim myDataSet As New DataSet()
            Dim strCurrentRef As String = ""
            Dim strLastRef As String = ""
            Dim intCount As Integer = 0
            Dim strSubject As String = "runCashHdr hourly "
            Dim strWhere As String = ""

            Try
                If pbHourly Then
                    strWhere = "WHERE (Csh_recdte >= DATE() or Csh_recdte >= DATE())"
                ElseIf pbNightly Then
                    strWhere = "WHERE (" & _
                                            "(Csh_recdte >= DATE()-2)" & _
                                            " or " & _
                                            "(Csh_recdte >= DATE()-2)" & _
                                        ")"
                    strSubject = "runCashHdr nightly "
                ElseIf pbAll Then
                    strWhere = "where `YEAR`(Csh_recdte) = 2009 or `YEAR`(Csh_recdte) = 2010 or `YEAR`(Csh_recdte) = 2011 or `YEAR`(Csh_rdate) = 2009 or `YEAR`(Csh_rdate) = 2010 or `YEAR`(Csh_rdate) = 2011"
                    strSubject = "runCashHdr all "
                End If

                Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT Csh_key,Csh_br,Csh_bnkdte,Csh_batch,Csh_type,Csh_from,Csh_id,Csh_ref,Csh_fop,Csh_vrfnd,Csh_rfnd," & _
                                                        "Csh_prod,Csh_bank,Csh_curncy,Csh_amount,Csh_used,Csh_remain,Csh_atol,Csh_rdate,Csh_note,Csh_locked,Csh_recon,Csh_recdte," & _
                                                        "Csh_status,Csh_who,Csh_cctxn,Csh_ccid,Csh_ccno,Csh_ccauth,Csh_ccexp,Csh_issue,Csh_merch,Csh_ccxmit,Csh_cshamt,Csh_ccamt," & _
                                                        "Csh_chqamt,Csh_vchamt,Csh_othamt,Csh_ccchg,Csh_mfee,Csh_bal,Csh_chqgte,Csh_chqno,Csh_vchid,Csh_vchref,Csh_vchnte,Csh_othid," & _
                                                        "Csh_othref,Csh_othnte,Chksum " & _
                                                        "FROM cashhdr " & _
                                                        strWhere, dBaseConnection)

                Dim myDataAdapter As New System.Data.OleDb.OleDbDataAdapter(dBaseCommand)
                myDataAdapter.Fill(myDataSet, "runCashHdr")
                dBaseConnection.Close()

                For Each dr As DataRow In myDataSet.Tables("runCashHdr").Rows
                    Dim oCashhdr As New BOSScashhdr(0, CStr(dr.Item("Csh_key")), CStr(dr.Item("Csh_br")), CType(dr.Item("Csh_bnkdte"), Date?), CStr(dr.Item("Csh_batch")), CStr(dr.Item("Csh_type")), _
                                                    CStr(dr.Item("Csh_from")), CStr(dr.Item("Csh_id")), CStr(dr.Item("Csh_ref")), CDec(dr.Item("Csh_fop")), CType(dr.Item("Csh_vrfnd"), Boolean?), _
                                                    CType(dr.Item("Csh_rfnd"), Boolean?), CStr(dr.Item("Csh_prod")), CStr(dr.Item("Csh_bank")), CStr(dr.Item("Csh_curncy")), CDec(dr.Item("Csh_amount")), _
                                                    CDec(dr.Item("Csh_used")), CDec(dr.Item("Csh_remain")), CDec(dr.Item("Csh_atol")), CType(dr.Item("Csh_rdate"), Date?), CStr(dr.Item("Csh_note")), _
                                                    CType(dr.Item("Csh_locked"), Boolean?), CType(dr.Item("Csh_recon"), Boolean?), CType(dr.Item("Csh_recdte"), Date?), CStr(dr.Item("Csh_status")), _
                                                    CStr(dr.Item("Csh_who")), CStr(dr.Item("Csh_cctxn")), CStr(dr.Item("Csh_ccid")), CStr(dr.Item("Csh_ccno")), CStr(dr.Item("Csh_ccauth")), _
                                                    CStr(dr.Item("Csh_ccexp")), CDec(dr.Item("Csh_issue")), CType(dr.Item("Csh_merch"), Boolean?), CType(dr.Item("Csh_ccxmit"), Date?), _
                                                    CDec(dr.Item("Csh_cshamt")), CDec(dr.Item("Csh_ccamt")), CDec(dr.Item("Csh_chqamt")), CDec(dr.Item("Csh_vchamt")), CDec(dr.Item("Csh_othamt")), _
                                                    CDec(dr.Item("Csh_ccchg")), CDec(dr.Item("Csh_mfee")), CDec(dr.Item("Csh_bal")), CStr(dr.Item("Csh_chqgte")), CStr(dr.Item("Csh_chqno")), _
                                                    CStr(dr.Item("Csh_vchid")), CStr(dr.Item("Csh_vchref")), CStr(dr.Item("Csh_vchnte")), CStr(dr.Item("Csh_othid")), CStr(dr.Item("Csh_othref")), _
                                                    CStr(dr.Item("Csh_othnte")), CStr(dr.Item("Chksum")))
                    oCashhdr.save()
                Next

                myDataSet.Clear()
                log.Info(strSubject & "completed successfully")
            Catch ex As Exception
                sendEmailLocal("DevelopmentTeam@nysgroup.com", "DevelopmentTeam@nysgroup.com", strSubject & "had an error for:" & strCurrentRef, "Do something")
                log.Error("INVOICE ERROR: " & strCurrentRef, ex)
            End Try
        End Using
    End Sub
End Class
