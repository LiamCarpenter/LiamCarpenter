Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class FeedStatus

    Public Sub New( _
        ByVal pStatusid As Integer, _
        ByVal pStatusname As String)
        mStatusid = pStatusid
        mStatusname = pStatusname
    End Sub

    Public Sub New( _
)
    End Sub

    Private mStatusid As Integer
    Private mStatusname As String
    
    Public Property Statusid() As Integer
        Get
            Return mStatusid
        End Get
        Set(ByVal value As Integer)
            mStatusid = value
        End Set
    End Property

    Public Property Statusname() As String
        Get
            Return mStatusname
        End Get
        Set(ByVal value As String)
            mStatusname = value
        End Set
    End Property

    Private Shared Function makeFeedStatusFromRow( _
            ByVal r As IDataReader _
        ) As FeedStatus
        Return New FeedStatus( _
                clsStuff.notWholeNumber(r.Item("statusid")), _
                clsStuff.notString(r.Item("statusname")))
    End Function

    Public Shared Function [get]( _
            ByVal pStatusid As Integer _
        ) As FeedStatus
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("FeedStatus_get", "@statusid", pStatusid)
                Dim ret As New FeedStatus
                If r.Read() Then
                    ret = makeFeedStatusFromRow(r)
                End If
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function [getStatusID]( _
            ByVal pStatusName As String _
        ) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intstatusid As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValue("FeedStatusID_get", _
                                                     "@statusname", pStatusName))
            Return intstatusid
        End Using
    End Function


    Public Shared Function list() As List(Of FeedStatus)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of FeedStatus)()
            Using r As IDataReader = dbh.callSP("FeedStatus_list")
                While r.Read()
                    ret.Add(makeFeedStatusFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mStatusid = clsStuff.notWholeNumber(dbh.callSPSingleValue("FeedStatus_save", _
                                                   "@Statusid", mStatusid, _
                                                   "@Statusname", mStatusname))
            Return mStatusid
        End Using
    End Function

    Public Shared Sub delete( _
            ByVal pStatusid As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("FeedStatus_delete", "@Statusid", pStatusid)
        End Using
    End Sub

End Class
