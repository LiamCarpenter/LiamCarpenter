Option Strict On
Option Explicit On

Public Class Utils

    'visual fred: batteries included, honest.
    Public Shared Function giif(Of T)(ByVal cond As Boolean, ByVal t1 As T, ByVal t2 As T) As T
        Return DirectCast(IIf(cond, t1, t2), T)
    End Function

    Public Shared Function castNullable(Of T As Structure)(ByVal o As Object) As Nullable(Of T)
        If o Is Nothing Then
            Return Nothing
        Else
            Return DirectCast(o, T)
        End If
    End Function

    Public Shared Function castNullableValue(Of T As Structure)(ByVal o As Nullable(Of T), ByVal v As T) As T
        If o.HasValue Then
            Return o.Value
        Else
            Return v
        End If
    End Function

    Public Shared Function eq(ByVal a As Nullable(Of Boolean), ByVal b As Nullable(Of Boolean)) As Boolean
        If a.HasValue And b.HasValue Then
            Return a.Value = b.Value
        ElseIf Not a.HasValue And Not b.HasValue Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function equalsOrBothNothing(ByVal a As Object, ByVal b As Object) As Boolean
        If a Is Nothing And b Is nothing
            Return True
        End If
        If a IsNot Nothing And b IsNot Nothing Then
            Return a.Equals(b)
        End If
        Return False
    End Function

    Public Shared Function TryCast2(Of U)(ByVal e As Object) As U
        If TypeOf e Is U Then
            Return DirectCast(e, U)
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function getType2(ByVal o As Object) As Type
        Return o.GetType
    End Function

    Public Shared Function minutesToTicks(ByVal minutes As Long) As Long
        Return minutes * 60 * 1000 * 1000 * 10
    End Function

    'shortcut convertions using direct cast instead of ctype
    Public Shared Function DInt(ByVal o As Object) As Integer
        Return DirectCast(o, Integer)
    End Function

    Public Shared Function parseEnum(Of T)(ByVal s As String) As T
        Return DirectCast(System.Enum.Parse(GetType(T), s), T)
    End Function

    Public Shared Function eCoalesce(ByVal s1 As String, ByVal s2 As String) As String
        'coalesce, returns the second value iff the first is nothing or empty string
        If s1 Is Nothing OrElse s1 = "" Then
            Return s2
        Else
            Return s1
        End If
    End Function
End Class
