Public Class VariableConversions
    Public Shared Function notString(pArg As Object) As String
        If IsDBNull(pArg) OrElse pArg = Nothing Then
            Return ""
        Else
            Return CStr(pArg)
        End If
    End Function

    Public Shared Function notInteger(pArg As Object) As Integer
        If IsDBNull(pArg) OrElse pArg = Nothing Then
            Return 0
        Else
            Return CInt(pArg)
        End If
    End Function

    Public Shared Function notDouble(pArg As Object) As Double
        If IsDBNull(pArg) OrElse pArg = Nothing Then
            Return 0
        Else
            Return CDbl(pArg)
        End If
    End Function

    Public Shared Function notBoolean(pArg As Object) As Boolean
        If IsDBNull(pArg) OrElse pArg = Nothing Then
            Return False
        Else
            Return CBool(pArg)
        End If
    End Function
End Class
