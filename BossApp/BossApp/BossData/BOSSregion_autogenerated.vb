Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSSregion

    Public Sub New( _
        ByVal pRegionsID As Integer, _
        ByVal pRegion As String, _
        ByVal pDesc As String, _
        ByVal pRating As Decimal, _
        ByVal pSingle_fee As Decimal, _
        ByVal pSector_fee As Decimal, _
        ByVal pSec_fee2 As Decimal, _
        ByVal pLoclregion As Nullable(Of Boolean), _
        ByVal pLonghaul As Nullable(Of Boolean), _
        ByVal pChksum As String)
        mRegionsID = pRegionsID
        mChksum = pChksum
        mRegion = pRegion
        mDesc = pDesc
        mRating = pRating
        mSingle_fee = pSingle_fee
        mSector_fee = pSector_fee
        mSec_fee2 = pSec_fee2
        mLoclregion = pLoclregion
        mLonghaul = pLonghaul
    End Sub

    Public Sub New( _
)
    End Sub

    Private mRegionsID As Integer
    Private mChksum As String
    Private mRegion As String
    Private mDesc As String
    Private mRating As Decimal
    Private mSingle_fee As Decimal
    Private mSector_fee As Decimal
    Private mSec_fee2 As Decimal
    Private mLoclregion As Nullable(Of Boolean)
    Private mLonghaul As Nullable(Of Boolean)

    Public Property RegionsID() As Integer
        Get
            Return mRegionsID
        End Get
        Set(ByVal value As Integer)
            mRegionsID = value
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

    Public Property Region() As String
        Get
            Return mRegion
        End Get
        Set(ByVal value As String)
            mRegion = value
        End Set
    End Property

    Public Property Desc() As String
        Get
            Return mDesc
        End Get
        Set(ByVal value As String)
            mDesc = value
        End Set
    End Property

    Public Property Rating() As Decimal
        Get
            Return mRating
        End Get
        Set(ByVal value As Decimal)
            mRating = value
        End Set
    End Property

    Public Property Single_fee() As Decimal
        Get
            Return mSingle_fee
        End Get
        Set(ByVal value As Decimal)
            mSingle_fee = value
        End Set
    End Property

    Public Property Sector_fee() As Decimal
        Get
            Return mSector_fee
        End Get
        Set(ByVal value As Decimal)
            mSector_fee = value
        End Set
    End Property

    Public Property Sec_fee2() As Decimal
        Get
            Return mSec_fee2
        End Get
        Set(ByVal value As Decimal)
            mSec_fee2 = value
        End Set
    End Property

    Public Property Loclregion() As Nullable(Of Boolean)
        Get
            Return mLoclregion
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mLoclregion = value
        End Set
    End Property

    Public Property Longhaul() As Nullable(Of Boolean)
        Get
            Return mLonghaul
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mLonghaul = value
        End Set
    End Property

    Private Shared Function makeBOSSregionFromRow( _
            ByVal r As IDataReader _
        ) As BOSSregion
        Return New BOSSregion( _
                clsUseful.notInteger(r.Item("regionsID")), _
                clsUseful.notString(r.Item("region")), _
                clsUseful.notString(r.Item("desc")), _
                toNullableFloat(r.Item("rating")), _
                toNullableFloat(r.Item("single_fee")), _
                toNullableFloat(r.Item("sector_fee")), _
                toNullableFloat(r.Item("sec_fee2")), _
                toNullableBoolean(r.Item("loclregion")), _
                toNullableBoolean(r.Item("longhaul")), _
                clsUseful.notString(r.Item("chksum")))
    End Function

    Public Shared Function [get]( _
            ByVal pRegionsID As Integer _
        ) As BOSSregion
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("BOSSregions_get", "@regionsID", pRegionsID)
                If Not r.Read() Then
                    Throw New Exception("No BOSSregion with id " & pRegionsID)
                End If
                Dim ret As BOSSregion = makeBOSSregionFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of BOSSregion)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BOSSregion)()
            Using r As IDataReader = dbh.callSP("BOSSregions_list")
                While r.Read()
                    ret.Add(makeBOSSregionFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim strRet As String = CStr(dbh.callSPSingleValue("BOSSregions_save", "@RegionsID", mRegionsID, "@Region", mRegion, "@Desc", mDesc, _
                                                              "@Rating", mRating, "@Single_fee", mSingle_fee, "@Sector_fee", mSector_fee, "@Sec_fee2", mSec_fee2, _
                                                              "@Loclregion", mLoclregion, "@Longhaul", mLonghaul, "@Chksum", mChksum))
            Return strRet
        End Using
    End Function

    Public Shared Sub delete( _
            ByVal pRegionsID As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("BOSSregions_delete", "@RegionsID", pRegionsID)
        End Using
    End Sub

End Class
