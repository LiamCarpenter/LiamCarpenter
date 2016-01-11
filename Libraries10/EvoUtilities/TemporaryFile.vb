Imports System.IO
Imports EvoUtilities.Utils

Public Class TemporaryFile
    Inherits DisposeUtils.MyDisposable

    Public ReadOnly filename As String

    Public Sub New(Optional ByVal extension As String = Nothing)
        'just stick the extension on the end, maybe this will break
        'someday?
        filename = Path.GetTempFileName & giif(extension Is Nothing, "", extension)
    End Sub

    Public Overrides Sub disposeStuff()

    End Sub

    Public Overrides Sub disposeUnmanaged()
        If File.Exists(filename) Then
            File.Delete(filename)
        End If
    End Sub

End Class
