Option Strict On
Option Explicit On

Public Class BackupInfo

    Public Class DatabaseBackupInfo
        Public mConnectionString As String
        Public mDatabaseName As String
        Public Sub New(ByVal connectionString As String, ByVal databaseName As String)
            mConnectionString = connectionString
            mDatabaseName = databaseName
        End Sub
    End Class

    Public mDatabaseBackups As New List(Of DatabaseBackupInfo)

    Public mPrefix As String
    Public mFilesAndFolders As New List(Of String)
    Public mZipRoot As String

    Public mFtpHost As String
    Public mFtpFolder As String
    Public mFtpUsername As String
    Public mFtpPassword As String

End Class
