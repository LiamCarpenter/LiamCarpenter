Option Strict On
Option Explicit On

Imports system.diagnostics.debug

Public Class ExceptionUtils

    Private Shared ReadOnly log As log4net.ILog = _
        log4net.LogManager.GetLogger(System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName)

    Public Shared Sub dumpException(ByVal ex As Exception)
        WriteLine(ex.Message)
        WriteLine(ex.StackTrace)
        If ex.InnerException IsNot Nothing Then
            dumpException(ex.InnerException)
        End If
    End Sub


    Public Shared Function getExceptionFullText(ByVal ex As Exception) As String
        If ex Is Nothing Then
            Return ""
        Else
            Return ex.GetType.FullName & ": " & ex.Message() & vbNewLine & _
                ex.StackTrace & vbNewLine & _
                getExceptionFullText(ex.InnerException)
        End If
    End Function

    Public Shared Sub ignoreException(ByVal ex As Exception)
        log.Error(ex)
        dumpException(ex)
    End Sub

End Class
