<Serializable()> _
Public Class SyntaxException
    Inherits Exception

    Public mLineNo As Integer

    Public Sub New()
    End Sub

    Public Sub New(ByVal lineNo As Integer, ByVal m As String)
        MyBase.new(m & " on line " & lineNo)
        mLineNo = lineNo
    End Sub
End Class