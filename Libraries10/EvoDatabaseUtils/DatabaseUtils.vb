Option Strict On
Option Explicit On

'Imports System.data.sqlclient
'Imports system.data.OleDb
Imports System.Diagnostics.Debug
Imports EvoUtilities
Imports System.Text.RegularExpressions
Imports EvoUtilities.log4netUtils
Imports EvoUtilities.DisposeUtils

Imports EvoUtilities.CollectionUtils
Imports EvoUtilities.Utils
Imports System.Data

Public Module DatabaseUtils

    Private ReadOnly log As log4net.ILog = _
        log4net.LogManager.GetLogger(System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName)

    Private ReadOnly connectionRegex As New Regex("^.*database=(?<database>.*)$")

    Public Function getDatabaseName(ByVal connectionString As String) As String
        Dim m As Match = connectionRegex.Match(connectionString)
        If m.Success Then
            Return m.Groups.Item("database").Value
        Else
            Throw New DatabaseUtilsException("couldn't find database name")
        End If
    End Function

    Public Function singleValueCanReturnNothing(ByVal r As IDataReader) As Object
        If Not r.Read Then
            Return DBNull.Value
        End If
        'If r.FieldCount <> 1 Then
        '    Throw New DatabaseUtilsException("expected one field in the result set but got " & r.FieldCount)
        'End If
        Dim ret As Object = r.Item(0)
        'If r.Read Then
        '    Throw New DatabaseUtilsException("expected one row but got at least two")
        'End If
        Return ret
    End Function

    Public Function singleValue(ByVal r As IDataReader) As Object
        If Not r.Read Then
            Throw New DatabaseUtilsException("got no results back, expected some")
        End If
        If r.FieldCount <> 1 Then
            Throw New DatabaseUtilsException("expected one field in the result set but got " & r.FieldCount)
        End If
        Dim ret As Object = r.Item(0)
        If r.Read Then
            Throw New DatabaseUtilsException("expected one row but got at least two")
        End If
        Return ret
    End Function

    Public Sub backupDatabase(ByVal connectionString As String, _
        ByVal databaseName As String, ByVal backupFilename As String)
        Using New MethodLogger(log, "backupDatabase")
            Using h As New SqlDatabaseHandle(connectionString)
                h.runSQLNonQuery("backup database " & databaseName & " to disk='" & _
                    backupFilename & "'")
            End Using
        End Using
    End Sub

    Public Sub createDatabase(ByVal connectionString As String, _
        ByVal databaseName As String)
        Using h As New SqlDatabaseHandle(connectionString)
            h.runSQLNonQuery("create database " & databaseName)
        End Using
    End Sub

    Public Function listDatabases(ByVal connectionString As String) _
        As List(Of String)
        Using h As New SqlDatabaseHandle(connectionString)
            Using r As IDataReader = h.runSQLQuery("select name from sysdatabases")
                Dim ret As New List(Of String)
                While r.Read()
                    ret.Add(r.GetString(0))
                End While
                Return ret
            End Using
        End Using
    End Function

    Public Sub restoreDatabase(ByVal connectionString As String, _
        ByVal databaseName As String, ByVal backupFileName As String)

        'point of all the excessive code in this method is to use
        'the database and log file names from the database we
        'are restoring to instead of the ones stored in the backup

        Using h As New SqlDatabaseHandle(connectionString)
            h.changeDatabase("master")
            'get the database file name
            Dim databaseFile As String
            Using r As IDataReader = h.runSQLQuery( _
                "select filename from sysdatabases where name = '" & _
                databaseName & "'")
                r.Read()
                databaseFile = r.GetString(0)
            End Using

            'get the log file name
            h.changeDatabase(databaseName)
            Dim logFile As String
            Using r As IDataReader = h.runSQLQuery( _
                "select filename from sysfiles where name like '%_log%'")
                r.Read()
                logFile = r.GetString(0).Trim
            End Using

            'get the logical names from the backup
            h.changeDatabase("master")
            Dim logicalDBName, logicalDBLog As String
            Using r As IDataReader = h.runSQLQuery( _
                "restore filelistonly from disk = '" & backupFileName & "'")
                r.Read()
                If CStr(r.Item("Type")) <> "D" Then
                    Throw New DatabaseUtilsException("First line in restore filelistonly was type " & _
                        CStr(r.Item("Type")) & ", expected D")
                End If
                logicalDBName = CStr(r.Item("LogicalName"))
                r.Read()
                If CStr(r.Item("Type")) <> "L" Then
                    Throw New DatabaseUtilsException("Second line in restore filelistonly was type " & _
                        CStr(r.Item("Type")) & ", expected L")
                End If
                logicalDBLog = CStr(r.Item("LogicalName"))
                If r.Read() Then
                    Throw New DatabaseUtilsException("more than two lines returned from restore filelistonly")
                End If
            End Using

            'do the restore
            Dim restoreSql As String = _
                "RESTORE DATABASE " & databaseName & vbNewLine & _
                "   FROM DISK = '" & backupFileName & "'" & vbNewLine & _
                "   WITH RECOVERY, " & vbNewLine & _
                "      MOVE '" & logicalDBName & "' TO '" & _
                    databaseFile & "', " & vbNewLine & _
                "      MOVE '" & logicalDBLog & "' TO '" & logFile & "', replace"
            h.Timeout = 500
            h.runSQLNonQuery(restoreSql)
        End Using
    End Sub

    'add setup users/logins, permissions

    'strip dodgy characters from s: used for cleaning data in database
    'or for imports
    'Private Function whitelistFilter(ByVal s As String) As String
    '    Return Regex.Replace(s, "[^A-Za-z0-9!""£$%^&*()_+-={}[]~#@':;?/>.<,|\\\n\t ]", "")
    'End Function

    Public Sub debugPrintSP(ByVal oCommand As System.Data.SqlClient.SqlCommand)
        'purpose of this function is to write out the arguments of a stored procedure call in
        'a way which can be run from sql query analyzer
        'replace empty strings with null
        'surround strings with ''
        'when ' appears in string escape it by doubling it e.g. candidate's becomes candidate''s
        WriteLine(oCommand.CommandText)
        Dim unescaped As New List(Of String)
        Dim tmp As New List(Of String)
        For Each oparam As System.Data.SqlClient.SqlParameter In oCommand.Parameters
            unescaped.Add(CStr(oparam.Value))
            If oparam.Value.ToString = "" Then
                tmp.Add("null")
            Else
                If oparam.DbType = DbType.String Then
                    tmp.Add("'" & oparam.Value.ToString.Replace("'", "''") & "'")
                Else
                    tmp.Add(oparam.Value.ToString)
                End If
            End If
            'tmp.Add(oparam.Value)
        Next
        WriteLine(CollectionUtils.join(unescaped, ","))
        WriteLine(CollectionUtils.join(tmp, ","))
    End Sub

    Public Function IsDBNull(ByVal o As Object) As Boolean
        Return Microsoft.VisualBasic.IsDBNull(o)
    End Function

    Public Function toNullableInteger(ByVal o As Object) As Nullable(Of Integer)
        If o Is Nothing OrElse IsDBNull(o) Then
            Return Nothing
        Else
            Return CInt(o)
        End If
    End Function

    'R2.9 SA 
    Public Function toNullableLong(ByVal o As Object) As Nullable(Of Long)
        If o Is Nothing OrElse IsDBNull(o) Then
            Return Nothing
        Else
            Return CLng(o)
        End If
    End Function

    Public Function toNullableBoolean(ByVal o As Object) As Nullable(Of Boolean)
        If o Is Nothing OrElse IsDBNull(o) Then
            Return Nothing
        Else
            Return CBool(o)
        End If
    End Function

    Public Function toStr(ByVal o As Object) As String
        If o Is Nothing OrElse IsDBNull(o) Then
            Return ""
        Else
            Return CStr(o)
        End If
    End Function

    Public Function toNullableDate(ByVal o As Object) As Nullable(Of Date)
        If o Is Nothing OrElse IsDBNull(o) Then
            Return Nothing
        Else
            Return CDate(o)
        End If
    End Function

    Public Function toNullableFloat(ByVal o As Object) As Nullable(Of Double)
        If o Is Nothing OrElse IsDBNull(o) Then
            Return Nothing
        Else
            Return CDbl(o)
        End If
    End Function

    Public Sub debugPrintCmd(ByVal cmd As IDbCommand, ByVal ParamArray args As Object())
        Debug.WriteLine(cmd.CommandText)
        For i As Integer = 0 To args.Length - 1 Step 2
            Dim arg As Object = args(i + 1)
            Debug.Write(CStr(args(i)) & " = ")
            If arg Is Nothing Then
                Debug.WriteLine("null,")
            Else
                Debug.WriteLine("'" & arg.ToString & "',")
            End If
        Next

    End Sub

End Module
