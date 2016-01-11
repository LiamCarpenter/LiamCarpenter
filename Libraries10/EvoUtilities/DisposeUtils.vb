Option Strict On
Option Explicit On

<CLSCompliant(True)> _
Public Class DisposeUtils

    Private Shared ReadOnly log As log4net.ILog = _
        log4net.LogManager.GetLogger(System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName)

    Public Shared Sub safeDispose(ByVal o As IDisposable)
        If o IsNot Nothing Then
            Try
                o.Dispose()
            Catch ex As Exception
                log.Error("Disposing", ex)
                ExceptionUtils.dumpException(ex)
            End Try
        End If
    End Sub

    'create a wrapper which calls safedispose
    'allows creating null variable, maybe setting it inside
    'the using block and having dispose on it called if it is not nothing
    Public Class Disposeriser
        Implements IDisposable

        Private mTheObject As IDisposable

        Public Sub New()
        End Sub

        Public Sub setObject(ByVal o As IDisposable)
            mTheObject = o
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free unmanaged resources when explicitly called
                End If

                ' TODO: free shared unmanaged resources
                safeDispose(mTheObject)
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

    'use this class to avoid having all that fucking junk that
    'microsoft like to dump in your class
    'can only use this if your class doesn't need to 
    'inherit from another concrete class
    <CLSCompliant(True)> _
    Public MustInherit Class MyDisposable
        Implements IDisposable

        Public MustOverride Sub disposeStuff()

        Public Overridable Sub disposeUnmanaged()
        End Sub

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' free unmanaged resources when explicitly called
                    disposeStuff()
                End If
                ' free shared unmanaged resources
                disposeUnmanaged()
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
