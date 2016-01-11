Option Strict On
Option Explicit On

Imports System.Diagnostics

<CLSCompliant(True)> _
Public Class EventLogger

    'logs to windows event log
    'every log message also gets logged using log4net

    Private Shared ReadOnly log As log4net.ILog = _
        log4net.LogManager.GetLogger(System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName)

    Public Shared Sub logError(ByVal plog As log4net.ILog, ByVal ex As Exception, _
        ByVal methodName As String)
        plog.Error(ex)
        'WriteToEventLog("Error in NYS Mailer application (" & methodName & _
        '    ") - " & ex.Message, EventLogEntryType.Error)
    End Sub

    Public Shared Sub logError(ByVal plog As log4net.ILog, ByVal message As String, _
        ByVal ex As Exception, ByVal methodName As String)
        plog.Error(message, ex)
        'WriteToEventLog("Error in NYS Mailer application (" & methodName & _
        '    ") - " & message & " - " & ex.Message, EventLogEntryType.Error)
    End Sub

    Public Shared Sub logError(ByVal plog As log4net.ILog, ByVal message As String, _
        ByVal methodName As String)
        plog.Error(message)
        'WriteToEventLog("Error in NYS Mailer application (" & methodName & _
        '    ") - " & message, EventLogEntryType.Error)
    End Sub

    Public Shared Sub logInfo(ByVal plog As log4net.ILog, ByVal message As String)
        plog.Info(message)
        'WriteToEventLog(message)
    End Sub

    '*************************************************************
    'NAME:          WriteToEventLog
    'PURPOSE:       Write to Event Log
    'PARAMETERS:    Entry - Value to Write
    '               AppName - Name of Client Application. Needed 
    '               because before writing to event log, you must 
    '               have a named EventLog source. 
    '               EventType - Entry Type, from EventLogEntryType 
    '               Structure e.g., EventLogEntryType.Warning, 
    '               EventLogEntryType.Error
    '               LogName: Name of Log (System, Application; 
    '               Security is read-only) If you 
    '               specify a non-existent log, the log will be
    '               created

    'RETURNS:       nothing
    '*************************************************************
    Private Const appName As String = "NYSMAILER"
    Private Const logName As String = "NYS"

    'Private Shared Sub WriteToEventLog(ByVal entry As String, _
    '        Optional ByVal eventType As EventLogEntryType = _
    '        EventLogEntryType.Information)
    '    Try
    '        Dim objEventLog As New EventLog
    '        'Register the Application as an Event Source
    '        If Not EventLog.SourceExists(appName) Then
    '            EventLog.CreateEventSource(appName, logName)
    '        End If
    '        'log the entry
    '        objEventLog.Source = appName
    '        objEventLog.WriteEntry(entry, eventType)
    '    Catch Ex As Exception
    '        log.Error(Ex)
    '    End Try
    'End Sub

End Class
