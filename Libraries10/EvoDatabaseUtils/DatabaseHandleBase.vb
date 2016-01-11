Imports EvoUtilities.log4netUtils
Imports EvoUtilities.DisposeUtils
Imports System.Data

Public MustInherit Class DatabaseHandleBase
    Inherits MyDisposable

    Private ReadOnly log As log4net.ILog = _
        log4net.LogManager.GetLogger(System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName)

    Protected mConnection As IDbConnection
    Protected connectionOpened As Boolean = False

    Protected Sub New()

    End Sub

    Protected Sub openConnection()
        Using New MethodLogger(log, "openConnection")
            If Not connectionOpened Then
                mConnection.Open()
                connectionOpened = True
            End If
        End Using
    End Sub

    Public Sub changeDatabase(ByVal databaseName As String)
        Using New MethodLogger(log, "changeDatabase", databaseName)
            openConnection()
            mConnection.ChangeDatabase(databaseName)
        End Using
    End Sub

    Public Function runSQLQuery(ByVal pSql As String, ByVal ParamArray args() As Object) As IDataReader
        Using New MethodLogger(log, "runSQLQuery", pSql)
            openConnection()
            Using cmd As IDbCommand = getIDBCommand(pSql)
                cmd.CommandTimeout = 0
                addArgs(cmd, args)
                Return cmd.ExecuteReader()
            End Using
        End Using
    End Function

    Public Sub runSQLNonQuery(ByVal pSql As String, ByVal ParamArray args() As Object)
        Using New MethodLogger(log, "runSQLNonQuery", pSql)
            openConnection()
            Using cmd As IDbCommand = getIDBCommand(pSql)
                cmd.CommandTimeout = 0
                addArgs(cmd, args)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function callSP(ByVal spName As String, _
        ByVal ParamArray args() As Object) As IDataReader
        Using New MethodLogger(log, "callSP", spName, args)
            openConnection()
            Using cmd As IDbCommand = getIDBCommand(spName)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandTimeout = 0
                addArgs(cmd, args)
#If DEBUG Then
                debugPrintCmd(cmd, args)
#End If
                Try
                    Dim xx As IDataReader
                    xx = cmd.ExecuteReader()
                    Return xx
                Catch ex As Exception
                    'Debug.Print(ex.InnerException.ToString)
                End Try
                Return cmd.ExecuteReader()
            End Using
        End Using
    End Function

    Public Sub callNonQuerySP(ByVal spName As String, _
        ByVal ParamArray args() As Object)
        Using r As IDataReader = callSP(spName, args)
        End Using
    End Sub

    Public Function callSPSingleValue(ByVal spName As String, _
        ByVal ParamArray args() As Object) As Object
        Using New MethodLogger(log, "callSPSingleValue", spName, args)
            openConnection()
            Using r As IDataReader = callSP(spName, args)
                Return singleValue(r)
            End Using
        End Using
    End Function

    Public Function callSPSingleValueCanReturnNothing(ByVal spName As String, _
        ByVal ParamArray args() As Object) As Object
        Using New MethodLogger(log, "callSPSingleValueCanReturnNothing", spName, args)
            openConnection()
            Using r As IDataReader = callSP(spName, args)
                Return singleValueCanReturnNothing(r)
            End Using
        End Using
    End Function

    Public Function runSqlQueryValue(ByVal pSql As String, _
        ByVal ParamArray args() As Object) As Object
        Using New MethodLogger(log, "runSqlQueryValue", pSql)
            openConnection()
            Using r As IDataReader = runSQLQuery(pSql, args)
                Return singleValue(r)
            End Using
        End Using
    End Function

    Public MustOverride Function getIDBCommand(ByVal sql As String) As IDbCommand

    Public MustOverride Overrides Sub disposeStuff()

    Protected MustOverride Sub addArgs(ByVal cmd As IDbCommand, ByVal args As Object())

End Class
