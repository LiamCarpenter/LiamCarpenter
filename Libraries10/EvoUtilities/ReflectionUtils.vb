Option Strict On
Option Explicit On

Imports system.reflection

Public Class ReflectionUtils
    Public Shared Function getProperty(ByVal o As Object, ByVal prop As String) As Object
        Dim pi As PropertyInfo = o.GetType.GetProperty(prop)
        If pi Is Nothing Then
            Throw New Exception("property named " & prop & " not found in object of type " & o.GetType.FullName)
        End If
        Return pi.GetValue(o, Nothing)
    End Function

    Public Shared Sub setProperty(ByVal o As Object, ByVal prop As String, ByVal value As Object)
        Dim pi As PropertyInfo = o.GetType.GetProperty(prop)
        If pi Is Nothing Then
            Throw New Exception("property named " & prop & " not found in object of type " & o.GetType.FullName)
        End If
        'check for numbers
        If pi.PropertyType Is GetType(Nullable(Of Integer)) Then
            pi.SetValue(o, CType(CInt(value), Nullable(Of Integer)), Nothing)
        Else
            pi.SetValue(o, value, Nothing)
        End If

    End Sub

    Public Shared Function isBoolean(ByVal o As Object, ByVal prop As String) As Boolean
        Dim pi As PropertyInfo = o.GetType.GetProperty(prop)
        Return pi.PropertyType.FullName.IndexOf("System.Boolean") <> -1
    End Function

    Public Shared Function propertyType(ByVal o As Object, ByVal prop As String) As Type
        Dim pi As PropertyInfo = o.GetType.GetProperty(prop)
        Return pi.PropertyType
    End Function

End Class
