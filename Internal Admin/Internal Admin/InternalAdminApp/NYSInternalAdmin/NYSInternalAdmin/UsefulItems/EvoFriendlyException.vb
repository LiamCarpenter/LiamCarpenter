Option Strict On
Option Explicit On

Public Class EvoFriendlyException
    Inherits Exception

    Public ReadOnly title As String

    Public Sub New(ByVal message As String, ByVal title As String)
        MyBase.new(message)
        Me.title = title
    End Sub

    Public Sub New(ByVal message As String, ByVal title As String, ByVal ex As Exception)
        MyBase.new(message, ex)
        Me.title = title
    End Sub
End Class
