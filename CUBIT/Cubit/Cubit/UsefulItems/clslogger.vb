Imports Microsoft.VisualBasic
Imports System

''' <summary>
''' Class clslogger
''' </summary>
''' <remarks>
''' Created 12/03/2009 Nick Massarella
''' Set up class for logging
''' </remarks>
Public Class clslogger
    Implements IDisposable

    Private disposedValue As Boolean       ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free unmanaged resources when explicitly called
            End If
            mLog.Info("end " & mName)
            ' TODO: free shared unmanaged resources
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

    Private mLog As log4net.ILog
    Private mName As String

    Public Sub New(ByVal log As log4net.ILog, ByVal className As String, ByVal methodName As String)
        mLog = log
        mName = className & "." & methodName
        mLog.Info("start " & mName)
    End Sub

    Public Sub New(ByVal log As log4net.ILog, ByVal methodName As String)
        mLog = log
        mName = methodName
        mLog.Info("start " & mName)
    End Sub

End Class
