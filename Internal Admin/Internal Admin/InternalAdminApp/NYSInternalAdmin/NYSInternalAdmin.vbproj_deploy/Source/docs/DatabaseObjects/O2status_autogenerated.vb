Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class O2status

    Public Sub New( _
        ByVal pO2statusid As Integer, _
        ByVal pO2status As String)
        mO2statusid = pO2statusid
        mO2status = pO2status
    End Sub

    Public Sub New( _
)
    End Sub

    Private mO2statusid As Integer
    Private mO2status As String

    Public Property O2statusid() As Integer
        Get
            Return mO2statusid
        End Get
        Set(ByVal value As Integer)
            mO2statusid = value
        End Set
    End Property

    Public Property O2status() As String
        Get
            Return mO2status
        End Get
        Set(ByVal value As String)
            mO2status = value
        End Set
    End Property

    Private Shared Function makeO2statuFromRow( _
            ByVal r As IDataReader _
        ) As O2status
        Return New O2status( _
                clsNYS.notInteger(r.Item("o2statusid")), _
                clsNYS.notString(r.Item("o2status")))
    End Function

    Public Shared Function [get]( _
            ByVal pO2statusid As Integer _
        ) As O2status
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("o2status_get", "@o2statusid", pO2statusid)
                If Not r.Read() Then
                    Throw New Exception("No O2statu with id " & pO2statusid)
                End If
                Dim ret As O2status = makeO2statuFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of O2status)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of O2status)()
            Using r As IDataReader = dbh.callSP("o2status_list")
                While r.Read()
                    ret.Add(makeO2statuFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mO2statusid = CInt(dbh.callSPSingleValue("o2status_save", "@O2statusid", mO2statusid, "@O2status", mO2status))
            Return mO2statusid
        End Using
    End Function

    Public Shared Sub delete( _
            ByVal pO2statusid As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("o2status_delete", "@O2statusid", pO2statusid)
        End Using
    End Sub

    Public Sub delete()
        delete(mO2statusid)
    End Sub

End Class
