Imports EvoUtilities.log4netUtils
Imports System.Data

'TODO: refactor to combine common functionality
Public Class AccessDatabaseHandle
    Inherits DatabaseHandleBase
    Implements DatabaseHandle

    Private ReadOnly log As log4net.ILog = _
    log4net.LogManager.GetLogger(System.Reflection.MethodBase. _
    GetCurrentMethod().DeclaringType.FullName)

    Protected Overrides Sub addArgs(ByVal cmd As IDbCommand, ByVal args As Object())
        Dim cmd1 As System.Data.OleDb.OleDbCommand = DirectCast(cmd, System.Data.OleDb.OleDbCommand)
        For i As Integer = 0 To args.Length - 1 Step 2
            Dim arg As Object = args(i + 1)
            If arg Is Nothing Then
                arg = DBNull.Value
            End If
            cmd1.Parameters.AddWithValue(CStr(args(i)), arg)
        Next
    End Sub

    '************************************
    'public api

    Public Sub New(ByVal connectionString As String)
        Using New MethodLogger(log, "new", connectionString)
            mConnection = New System.Data.OleDb.OleDbConnection
            mConnection.ConnectionString = connectionString
        End Using
    End Sub


    Public Overrides Sub disposeStuff()
        If mConnection IsNot Nothing Then
            mConnection.Dispose()
        End If
    End Sub

    Public Overrides Function getIDBCommand(ByVal sql As String) As System.Data.IDbCommand
        Return New System.Data.OleDb.OleDbCommand(sql, DirectCast(mConnection, System.Data.OleDb.OleDbConnection))
    End Function
End Class
