Option Strict On
Option Explicit On

Public Class DownloadEmailException
    Inherits Exception
    Public Sub New(ByVal exceptions As List(Of Exception))
        MyBase.new("Error downloading emails")
        mExceptions = exceptions
    End Sub

    Private mExceptions As List(Of Exception)

    Public ReadOnly Property Exceptions() As List(Of Exception)
        Get
            Return mExceptions
        End Get
    End Property

End Class
