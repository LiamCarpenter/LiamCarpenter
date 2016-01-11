Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSSsavecode

    Public Sub New( _
        ByVal pSavecodeID As Integer, _
        ByVal pSav_set As String, _
        ByVal pSav_id As String, _
        ByVal pSav_desc As String)
        mSavecodeID = pSavecodeID
        mSav_set = pSav_set
        mSav_id = pSav_id
        mSav_desc = pSav_desc
    End Sub

    Public Sub New( _
)
    End Sub

    Private mSavecodeID As Integer
    Private mSav_set As String
    Private mSav_id As String
    Private mSav_desc As String

    Public Property SavecodeID() As Integer
        Get
            Return mSavecodeID
        End Get
        Set(ByVal value As Integer)
            mSavecodeID = value
        End Set
    End Property

    Public Property Sav_set() As String
        Get
            Return mSav_set
        End Get
        Set(ByVal value As String)
            mSav_set = value
        End Set
    End Property

    Public Property Sav_id() As String
        Get
            Return mSav_id
        End Get
        Set(ByVal value As String)
            mSav_id = value
        End Set
    End Property

    Public Property Sav_desc() As String
        Get
            Return mSav_desc
        End Get
        Set(ByVal value As String)
            mSav_desc = value
        End Set
    End Property

    Private Shared Function makeBOSSsavecodeFromRow( _
            ByVal r As IDataReader _
        ) As BOSSsavecode
        Return New BOSSsavecode( _
                clsUseful.notInteger(r.Item("savecodeID")), _
                clsUseful.notString(r.Item("sav_set")), _
                clsUseful.notString(r.Item("sav_id")), _
                clsUseful.notString(r.Item("sav_desc")))
    End Function

    Public Shared Function [get]( _
            ByVal pSavecodeID As Integer _
        ) As BOSSsavecode
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("BOSSsavecode_get", "@savecodeID", pSavecodeID)
                If Not r.Read() Then
                    Throw New Exception("No BOSSsavecode with id " & pSavecodeID)
                End If
                Dim ret As BOSSsavecode = makeBOSSsavecodeFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of BOSSsavecode)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BOSSsavecode)()
            Using r As IDataReader = dbh.callSP("BOSSsavecode_list")
                While r.Read()
                    ret.Add(makeBOSSsavecodeFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim strRet As String = CStr(dbh.callSPSingleValue("BOSSsavecode_save", "@SavecodeID", mSavecodeID, "@Sav_set", mSav_set, "@Sav_id", mSav_id, "@Sav_desc", mSav_desc))
            Return strRet
        End Using
    End Function

    Public Shared Sub delete( _
            ByVal pSavecodeID As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("BOSSsavecode_delete", "@SavecodeID", pSavecodeID)
        End Using
    End Sub

End Class
