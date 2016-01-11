Option Strict On
Option Explicit On

Imports EvoUtilities.CollectionUtils

Public Class log4netUtils

    Public Class MethodLogger
        Implements IDisposable
        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free unmanaged resources when explicitly called
                End If
                mLog.Debug("end method")
                log4net.ThreadContext.Stacks.Item("NDC").Pop()
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

        Public Sub New(ByVal log As log4net.ILog, ByVal methodName As String)
            mLog = log
            mName = methodName
            log4net.ThreadContext.Stacks.Item("NDC").Push(methodName)
            mLog.Debug("start method")
        End Sub

        Public Sub New(ByVal log As log4net.ILog, ByVal methodName As String, _
            ByVal ParamArray params As Object())
            mLog = log
            mName = methodName
            log4net.ThreadContext.Stacks.Item("NDC").Push(methodName)
            If mLog.IsDebugEnabled Then
                mLog.Debug("start method")
                Dim sb As New System.Text.StringBuilder
                sb.Append("params: [")
                sb.Append(joinWith(New List(Of Object)(params), _
                    ",", AddressOf objToStringPlusType))
                sb.Append("]")
                mLog.Debug(sb.ToString)
            End If
        End Sub

    End Class

    Private Shared Function objToStringPlusType(ByVal o As Object) As String
        If o Is Nothing Then
            Return "Nothing"
        Else
            Return o.ToString & ":" & o.GetType.FullName
        End If
    End Function

End Class
