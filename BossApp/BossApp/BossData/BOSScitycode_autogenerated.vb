Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSScitycode

    Public Sub New( _
        ByVal pCitycodeID As Integer, _
        ByVal pCty_id As String, _
        ByVal pCty_airprt As String, _
        ByVal pCty_cty As String, _
        ByVal pCty_name As String, _
        ByVal pCty_cntry As String, _
        ByVal pCty_region As String, _
        ByVal pCty_elgkey As String, _
        ByVal pCty_cntryc As String)
        mCitycodeID = pCitycodeID
        mCty_id = pCty_id
        mCty_airprt = pCty_airprt
        mCty_cty = pCty_cty
        mCty_name = pCty_name
        mCty_cntry = pCty_cntry
        mCty_region = pCty_region
        mCty_elgkey = pCty_elgkey
        mCty_cntryc = pCty_cntryc
    End Sub

    Public Sub New( _
)
    End Sub

    Private mCitycodeID As Integer
    Private mCty_id As String
    Private mCty_airprt As String
    Private mCty_cty As String
    Private mCty_name As String
    Private mCty_cntry As String
    Private mCty_region As String
    Private mCty_elgkey As String
    Private mCty_cntryc As String

    Public Property CitycodeID() As Integer
        Get
            Return mCitycodeID
        End Get
        Set(ByVal value As Integer)
            mCitycodeID = value
        End Set
    End Property

    Public Property Cty_id() As String
        Get
            Return mCty_id
        End Get
        Set(ByVal value As String)
            mCty_id = value
        End Set
    End Property

    Public Property Cty_airprt() As String
        Get
            Return mCty_airprt
        End Get
        Set(ByVal value As String)
            mCty_airprt = value
        End Set
    End Property

    Public Property Cty_cty() As String
        Get
            Return mCty_cty
        End Get
        Set(ByVal value As String)
            mCty_cty = value
        End Set
    End Property

    Public Property Cty_name() As String
        Get
            Return mCty_name
        End Get
        Set(ByVal value As String)
            mCty_name = value
        End Set
    End Property

    Public Property Cty_cntry() As String
        Get
            Return mCty_cntry
        End Get
        Set(ByVal value As String)
            mCty_cntry = value
        End Set
    End Property

    Public Property Cty_region() As String
        Get
            Return mCty_region
        End Get
        Set(ByVal value As String)
            mCty_region = value
        End Set
    End Property

    Public Property Cty_elgkey() As String
        Get
            Return mCty_elgkey
        End Get
        Set(ByVal value As String)
            mCty_elgkey = value
        End Set
    End Property

    Public Property Cty_cntryc() As String
        Get
            Return mCty_cntryc
        End Get
        Set(ByVal value As String)
            mCty_cntryc = value
        End Set
    End Property

    Private Shared Function makeBOSScitycodeFromRow( _
            ByVal r As IDataReader _
        ) As BOSScitycode
        Return New BOSScitycode( _
               clsUseful.notInteger(r.Item("citycodeID")), _
               clsUseful.notString(r.Item("cty_id")), _
               clsUseful.notString(r.Item("cty_airprt")), _
               clsUseful.notString(r.Item("cty_cty")), _
               clsUseful.notString(r.Item("cty_name")), _
               clsUseful.notString(r.Item("cty_cntry")), _
               clsUseful.notString(r.Item("cty_region")), _
               clsUseful.notString(r.Item("cty_elgkey")), _
               clsUseful.notString(r.Item("cty_cntryc")))
    End Function

    Public Shared Function [get]( _
            ByVal pCitycodeID As Integer _
        ) As BOSScitycode
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("BOSScitycode_get", "@citycodeID", pCitycodeID)
                If Not r.Read() Then
                    Throw New Exception("No BOSScitycode with id " & pCitycodeID)
                End If
                Dim ret As BOSScitycode = makeBOSScitycodeFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of BOSScitycode)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BOSScitycode)()
            Using r As IDataReader = dbh.callSP("BOSScitycode_list")
                While r.Read()
                    ret.Add(makeBOSScitycodeFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim strRet As String = CStr(dbh.callSPSingleValue("BOSScitycode_save", "@CitycodeID", mCitycodeID, "@Cty_id", mCty_id, "@Cty_airprt", mCty_airprt, "@Cty_cty", mCty_cty, _
                                                     "@Cty_name", mCty_name, "@Cty_cntry", mCty_cntry, "@Cty_region", mCty_region, "@Cty_elgkey", mCty_elgkey, "@Cty_cntryc", mCty_cntryc))
            Return strRet
        End Using
    End Function

    Public Shared Sub delete(ByVal pCitycodeID As Integer, ByVal pCty_id As String)
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("BOSScitycode_delete", "@CitycodeID", pCitycodeID, "@Cty_id", pCty_id)
        End Using
    End Sub

End Class
