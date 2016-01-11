<Serializable()> _
Public Class DatabaseUtilsException
    Inherits Exception

    Public Sub New(ByVal m As String)
        MyBase.new(m)
    End Sub

    Public Sub New(ByVal m As String, ByVal ex As Exception)
        MyBase.new(m, ex)
    End Sub

    Public Sub New()
    End Sub

End Class
