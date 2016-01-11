Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSSprotype

    Public Sub New( _
        ByVal pProtypeID As Integer, _
        ByVal pTyp_id As String, _
        ByVal pTyp_desc As String, _
        ByVal pAp_tx As String, _
        ByVal pDc_tx As String, _
        ByVal pAx_tx As String, _
        ByVal pVi_tx As String, _
        ByVal pChksum As String)
        mProtypeID = pProtypeID
        mTyp_id = pTyp_id
        mTyp_desc = pTyp_desc
        mAp_tx = pAp_tx
        mDc_tx = pDc_tx
        mAx_tx = pAx_tx
        mVi_tx = pVi_tx
        mChksum = pChksum
    End Sub

    Public Sub New( _
)
    End Sub

    Private mProtypeID As Integer
    Private mTyp_id As String
    Private mTyp_desc As String
    Private mAp_tx As String
    Private mDc_tx As String
    Private mAx_tx As String
    Private mVi_tx As String
    Private mChksum As String

    Public Property ProtypeID() As Integer
        Get
            Return mProtypeID
        End Get
        Set(ByVal value As Integer)
            mProtypeID = value
        End Set
    End Property

    Public Property Typ_id() As String
        Get
            Return mTyp_id
        End Get
        Set(ByVal value As String)
            mTyp_id = value
        End Set
    End Property

    Public Property Typ_desc() As String
        Get
            Return mTyp_desc
        End Get
        Set(ByVal value As String)
            mTyp_desc = value
        End Set
    End Property

    Public Property Ap_tx() As String
        Get
            Return mAp_tx
        End Get
        Set(ByVal value As String)
            mAp_tx = value
        End Set
    End Property

    Public Property Dc_tx() As String
        Get
            Return mDc_tx
        End Get
        Set(ByVal value As String)
            mDc_tx = value
        End Set
    End Property

    Public Property Ax_tx() As String
        Get
            Return mAx_tx
        End Get
        Set(ByVal value As String)
            mAx_tx = value
        End Set
    End Property

    Public Property Vi_tx() As String
        Get
            Return mVi_tx
        End Get
        Set(ByVal value As String)
            mVi_tx = value
        End Set
    End Property

    Public Property Chksum() As String
        Get
            Return mChksum
        End Get
        Set(ByVal value As String)
            mChksum = value
        End Set
    End Property

    Private Shared Function makeBOSSprotypeFromRow( _
            ByVal r As IDataReader _
        ) As BOSSprotype
        Return New BOSSprotype( _
                clsUseful.notInteger(r.Item("protypeID")), _
                clsUseful.notString(r.Item("typ_id")), _
                clsUseful.notString(r.Item("typ_desc")), _
                clsUseful.notString(r.Item("ap_tx")), _
                clsUseful.notString(r.Item("dc_tx")), _
                clsUseful.notString(r.Item("ax_tx")), _
                clsUseful.notString(r.Item("vi_tx")), _
                clsUseful.notString(r.Item("chksum")))
    End Function

    Public Shared Function [get]( _
            ByVal pProtypeID As Integer _
        ) As BOSSprotype
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("BOSSprotype_get", "@protypeID", pProtypeID)
                If Not r.Read() Then
                    Throw New Exception("No BOSSprotype with id " & pProtypeID)
                End If
                Dim ret As BOSSprotype = makeBOSSprotypeFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of BOSSprotype)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BOSSprotype)()
            Using r As IDataReader = dbh.callSP("BOSSprotype_list")
                While r.Read()
                    ret.Add(makeBOSSprotypeFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim strRet As String = CStr(dbh.callSPSingleValue("BOSSprotype_save", "@ProtypeID", mProtypeID, "@Typ_id", mTyp_id, "@Typ_desc", mTyp_desc, "@Ap_tx", mAp_tx, "@Dc_tx", mDc_tx, _
                                                    "@Ax_tx", mAx_tx, "@Vi_tx", mVi_tx, "@Chksum", mChksum))
            Return strRet
        End Using
    End Function

    Public Shared Sub delete(ByVal pProtypeID As Integer, ByVal pTyp_id As String)
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("BOSSprotype_delete", "@ProtypeID", pProtypeID, "@Typ_id", pTyp_id)
        End Using
    End Sub

End Class
