Imports EvoUtilities.log4netUtils
Imports EvoUtilities.CollectionUtils
Imports EvoUtilities.Utils
Imports System.Data

Public Class SqlDatabaseHandle
    Inherits DatabaseHandleBase
    Implements DatabaseHandle

    Public Shared ReadOnly reservedWords As List(Of String) = makeList("default", "select")

    Private ReadOnly log As log4net.ILog = _
    log4net.LogManager.GetLogger(System.Reflection.MethodBase. _
    GetCurrentMethod().DeclaringType.FullName)

    Private mTimeOut As Integer = 90

    Public Property Timeout() As Integer
        Get
            Return mTimeOut
        End Get
        Set(ByVal value As Integer)
            mTimeOut = value
        End Set
    End Property

    Protected Overrides Sub addArgs(ByVal cmd As IDbCommand, ByVal args As Object())
        Dim cmd1 As System.Data.SqlClient.SqlCommand = DirectCast(cmd, System.Data.SqlClient.SqlCommand)
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
            mConnection = New System.Data.SqlClient.SqlConnection
            mConnection.ConnectionString = connectionString
        End Using
    End Sub

    'Public Function listTables() As List(Of String)
    '    Using r As SqlDataReader = runSQLQuery("")
    '        Dim ret As New List(Of String)

    '        Return ret
    '    End Using
    'End Function


    Private Shared ReadOnly databaseTypeMap As  _
        Dictionary(Of String, ColumnInfo.ColumnType) = makeDictionary( _
            p("bit", ColumnInfo.ColumnType.Bit), _
            p("char", ColumnInfo.ColumnType.Char), _
            p("datetime", ColumnInfo.ColumnType.DateTime), _
            p("float", ColumnInfo.ColumnType.Float), _
            p("image", ColumnInfo.ColumnType.Image), _
            p("nvarchar", ColumnInfo.ColumnType.NVarChar), _
            p("sysname", ColumnInfo.ColumnType.SysName), _
            p("timestamp", ColumnInfo.ColumnType.TimeStamp), _
            p("varchar", ColumnInfo.ColumnType.VarChar), _
            p("int", ColumnInfo.ColumnType.Int), _
            p("decimal", ColumnInfo.ColumnType.Decimal), _
            p("numeric", ColumnInfo.ColumnType.Numeric))

    Public Function listColumns(ByVal tableName As String) As IList(Of ColumnInfo)
        Using r As IDataReader = runSQLQuery( _
            "select syscolumns.name, " & _
            "systypes.name as type, " & _
            "syscolumns.length as length, " & _
            "isnullable, syscolumns.status " & _
            "   from syscolumns " & _
            "inner join sysobjects " & _
            "	on syscolumns.id = sysobjects.id " & _
            "inner join systypes " & _
            "	on syscolumns.xtype = systypes.xtype " & _
            "where sysobjects.name='" & tableName & "' " & _
            "order by colorder")

            Dim ret As New List(Of ColumnInfo)
            While r.Read()
                If Not databaseTypeMap.ContainsKey(CStr(r.Item(1))) Then
                    Throw New DatabaseUtilsException( _
                        "Don't recognise database column type " & _
                        CStr(r.Item(1)))
                End If
                Dim ci As New ColumnInfo(CStr(r.Item(0)), _
                    databaseTypeMap.Item(CStr(r.Item(1))), _
                    CInt(r.Item(2)), giif(CInt(r.Item(3)) = 1, True, False), _
                    giif((CInt(r.Item(4)) And &H80) <> 0, True, False))
                ret.Add(ci)
            End While
            Return ret
        End Using
    End Function

    Public Function getIdentityPKColumns(ByVal tableName As String) As IList(Of String)
        Using r As IDataReader = runSQLQuery( _
            "select syscolumns.name" & _
            "   from syscolumns " & _
            "inner join sysobjects " & _
            "	on syscolumns.id = sysobjects.id " & _
            "inner join systypes " & _
            "	on syscolumns.xtype = systypes.xtype " & _
            "where sysobjects.name='" & tableName & "' " & _
            " and syscolumns.status = 128 ")
            If r.Read() Then
                Return makeList(CStr(r.Item(0)))
            End If
        End Using
        'try to get primary key instead
        Using r As IDataReader = callSP("sp_pkeys", "@table_name", tableName)
            Dim ret As New List(Of String)
            While r.Read
                ret.Add(CStr(r.Item(3)))
            End While
            If ret.Count > 0 Then
                Return ret
            End If
        End Using
        Return Nothing
    End Function

    Public Function hasIdentity(ByVal tableName As String) As Boolean
        Using r As IDataReader = runSQLQuery( _
            "select syscolumns.name" & _
            "   from syscolumns " & _
            "inner join sysobjects " & _
            "	on syscolumns.id = sysobjects.id " & _
            "inner join systypes " & _
            "	on syscolumns.xtype = systypes.xtype " & _
            "where sysobjects.name='" & tableName & "' " & _
            " and syscolumns.status = 128 ")
            Return r.Read()
        End Using
    End Function

    Public Function listTables() As IList(Of String)
        Dim ret As New List(Of String)
        Using r As IDataReader = runSQLQuery( _
            "select name from sysobjects where xtype='U' and status >= 0 order by name")
            While r.Read
                ret.Add(CStr(r.Item(0)))
            End While
            Return ret
        End Using
    End Function

    Public Function listViews() As IList(Of String)
        Dim ret As New List(Of String)
        Using r As IDataReader = runSQLQuery( _
            "select name from sysobjects where xtype='V' and status >= 0 order by name")
            While r.Read
                ret.Add(CStr(r.Item(0)))
            End While
            Return ret
        End Using
    End Function

    Public Function listStoredProcedures() As IList(Of String)
        Dim ret As New List(Of String)
        Using r As IDataReader = runSQLQuery( _
            "select name from sysobjects where xtype='P' and status >= 0 order by name")
            While r.Read
                ret.Add(CStr(r.Item(0)))
            End While
            Return ret
        End Using
    End Function

    Public Function listFunctions() As IList(Of String)
        Dim ret As New List(Of String)
        Using r As IDataReader = runSQLQuery( _
            "select name from sysobjects where xtype in('FN', 'TF') and status >= 0 order by name")
            While r.Read
                ret.Add(CStr(r.Item(0)))
            End While
            Return ret
        End Using
    End Function


    Public Overrides Sub disposeStuff()
        If mConnection IsNot Nothing Then
            mConnection.Dispose()
        End If
    End Sub

    Public Overrides Function getIDBCommand(ByVal sql As String) As System.Data.IDbCommand
        Dim c As New System.Data.SqlClient.SqlCommand(sql, DirectCast(mConnection, System.Data.SqlClient.SqlConnection))
        c.CommandTimeout = mTimeOut
        Return c
    End Function
End Class
