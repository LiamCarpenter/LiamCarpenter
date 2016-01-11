Option Strict On
Option Explicit On

Imports EvoDatabaseUtils.DatabaseUtils
Imports EvoZipUtils.Zipper
Imports system.io

'this class provides a wrapper to backup databases and files easily
'and to upload them to an ftp site

Public Class BackupManager

    Public Shared Function backupDatabase(ByVal prefix As String, ByVal connectionString As String, _
        ByVal databaseName As String) As String
        Dim backupFilename As String = prefix & "-DB-" & Format(Date.Now, "dd-MM-yy hh.mm.ss") & ".bak"
        EvoDatabaseUtils.DatabaseUtils.backupDatabase(connectionString, databaseName, backupFilename)
        createZipFile(backupFilename & ".zip", backupFilename)
        File.Delete(backupFilename)
        Return backupFilename & ".zip"
    End Function

    Public Shared Function backupFiles(ByVal prefix As String, ByVal filesAndFoldersToBackup As List(Of String), _
        ByVal relativeZipRoot As String) As String
        Dim backupFilename As String = prefix & "-files-" & Format(Date.Now, "dd-MM-yy") & ".zip"
        createZipFile(backupFilename, filesAndFoldersToBackup, relativeZipRoot)
        Return backupFilename
    End Function

    Public Shared Sub backupAndUpload(ByVal backupInfo As BackupInfo)
        Dim filesToUpload As New List(Of String)
        For Each dbi As BackupInfo.DatabaseBackupInfo In backupInfo.mDatabaseBackups
            filesToUpload.Add(backupDatabase(backupInfo.mPrefix, dbi.mConnectionString, dbi.mDatabaseName))
        Next
        filesToUpload.Add(backupFiles(backupInfo.mPrefix, backupInfo.mFilesAndFolders, backupInfo.mZipRoot))
        'For Each file As String In filesToUpload
        '    SimpleFtp.upload(backupInfo.mFtpHost, backupInfo.mFtpFolder, _
        '        backupInfo.mFtpUsername, backupInfo.mFtpPassword, file)
        'Next
    End Sub

End Class
