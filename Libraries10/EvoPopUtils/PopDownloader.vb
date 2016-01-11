Option Strict On
Option Explicit On

'Imports OpenPOP.POP3
Imports OpenPOP.MIMEParser
Imports system.text
Imports system.io
Imports system.collections.generic
Imports system.diagnostics.debug
Imports EvoUtilities.log4netUtils


Public Class PopDownloader

    Private Shared ReadOnly log As log4net.ILog = _
        log4net.LogManager.GetLogger(System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName)

    Private mPopserver, mUsername, mPassword, mQueueFolder As String

    Public Sub New(ByVal popServer As String, ByVal username As String, _
        ByVal password As String, Optional ByVal queueFolder As String = "")
        Using New MethodLogger(log, "New", popServer, username, password, queueFolder)
            mPopserver = popServer
            mUsername = username
            mPassword = password
            mQueueFolder = queueFolder
            If Not mQueueFolder.EndsWith("\") Then
                mQueueFolder = mQueueFolder & "\"
            End If
        End Using
    End Sub

    'Public Function downloadMessages() As Integer
    '    Using popClient As New PopClientWrapper(), _
    '            mw As New MethodLogger(log, "downloadMessages")
    '        Dim exceptions As New List(Of Exception)
    '        popClient.Disconnect()
    '        popClient.Connect(mPopserver, 110)
    '        If mUsername <> "" Then
    '            popClient.Authenticate(mUsername, mPassword)
    '        End If
    '        Dim c As Integer = popClient.GetMessageCount()
    '        For i As Integer = 1 To c
    '            Try
    '                Dim m As OpenPOP.MIMEParser.Message = popClient.GetMessage(i, False)
    '                Dim fileName As String = mQueueFolder & Guid.NewGuid().ToString() & ".eml"
    '                My.Computer.FileSystem.WriteAllText(fileName, m.RawMessage, False)
    '                popClient.DeleteMessage(i)
    '            Catch ex As Exception
    '                log.Error(ex)
    '                exceptions.Add(ex)
    '            End Try
    '        Next
    '        popClient.Disconnect()
    '        If exceptions.Count <> 0 Then
    '            Throw New DownloadEmailException(exceptions)
    '        End If
    '        Return c
    '    End Using
    'End Function

    Public Function getMessagesFromQueue() As Dictionary(Of String, OpenPOP.MIMEParser.Message)
        Using New MethodLogger(log, "getMessagesFromQueue")
            Dim ret As New Dictionary(Of String, OpenPOP.MIMEParser.Message)
            For Each s As String In Directory.GetFiles(mQueueFolder)
                ret.Item(s) = New OpenPOP.MIMEParser.Message(False, File.ReadAllText(s))
            Next
            Return ret
        End Using
    End Function

    'use this class to get popclient supporting the using keyword
    Private Class PopClientWrapper
        ' Inherits POPClient
        Implements IDisposable

        Public Sub New()
            log.Info("pop client wrapper created")
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free unmanaged resources when explicitly called
                End If

                ' TODO: free shared unmanaged resources
                log.Info("pop client wrapper disconnected")
                '  Disconnect()
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

    End Class

End Class
