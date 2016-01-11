<Serializable()> _
Partial Public Class clsStuff

    Public Sub New()
    End Sub

    Public Shared Function notString(ByVal pArg As Object) As String
        If IsDBNull(pArg) Then
            Return ""
        Else
            Return CStr(pArg)
        End If
    End Function

    Public Shared Function notWholeNumber(ByVal pArg As Object) As Integer
        If IsDBNull(pArg) Then
            Return 0
        Else
            If IsNumeric(pArg) Then
                Return CInt(pArg)
            Else
                Return 0
            End If
            Return CInt(pArg)
        End If
    End Function

    'R2.9 SA 
    Public Shared Function notTheWholeNumber(ByVal pArg As Object) As Long
        If IsDBNull(pArg) Then
            Return 0
        Else
            If IsNumeric(pArg) Then
                Return CLng(pArg)
            Else
                Return 0
            End If
            Return CInt(pArg)
        End If
    End Function

    Public Shared Function notDouble(ByVal pArg As Object) As Double
        If IsDBNull(pArg) Then
            Return 0
        Else
            If IsNumeric(pArg) Then
                Return CDbl(pArg)
            Else
                Return 0
            End If
            Return CDbl(pArg)
        End If
    End Function

    Public Shared Function notDecimal(ByVal pArg As Object) As Decimal
        If IsDBNull(pArg) Then
            Return 0
        Else
            If IsNumeric(pArg) Then
                Return CDec(pArg)
            Else
                Return 0
            End If
            Return CDec(pArg)
        End If
    End Function

    Public Shared Function notBoolean(ByVal pArg As Object) As Boolean
        If IsDBNull(pArg) Then
            Return False
        Else
            If CBool(pArg) Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
End Class
