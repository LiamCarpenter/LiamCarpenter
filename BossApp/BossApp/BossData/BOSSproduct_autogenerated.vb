Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSSproduct

    Public Sub New( _
        ByVal pProductID As Integer, _
        ByVal pPro_id As String, _
        ByVal pPro_type As String, _
        ByVal pPro_desc As String, _
        ByVal pPro_vatabl As Nullable(Of Boolean), _
        ByVal pPro_ccrate As String, _
        ByVal pPro_dccode As String, _
        ByVal pPro_axcode As String, _
        ByVal pPro_glcomm As String, _
        ByVal pPro_glsale As String, _
        ByVal pPro_glcost As String, _
        ByVal pPro_gl0com As String, _
        ByVal pPro_gl0sal As String, _
        ByVal pPro_gl0cos As String, _
        ByVal pPro_crsprd As String, _
        ByVal pPro_paycon As Nullable(Of Boolean), _
        ByVal pPro_viacrs As Nullable(Of Boolean), _
        ByVal pPro_route As Nullable(Of Boolean), _
        ByVal pPro_bsp As Nullable(Of Boolean), _
        ByVal pPro_agchg As Decimal, _
        ByVal pPro_postbk As Nullable(Of Boolean), _
        ByVal pPro_vicode As String, _
        ByVal pPro_spare As String, _
        ByVal pPro_tpcode As String, _
        ByVal pPro_tpcde2 As String, _
        ByVal pPro_disvat As Nullable(Of Boolean))
        mProductID = pProductID
        mPro_id = pPro_id
        mPro_type = pPro_type
        mPro_desc = pPro_desc
        mPro_vatabl = pPro_vatabl
        mPro_ccrate = pPro_ccrate
        mPro_dccode = pPro_dccode
        mPro_axcode = pPro_axcode
        mPro_glcomm = pPro_glcomm
        mPro_glsale = pPro_glsale
        mPro_glcost = pPro_glcost
        mPro_gl0com = pPro_gl0com
        mPro_gl0sal = pPro_gl0sal
        mPro_gl0cos = pPro_gl0cos
        mPro_crsprd = pPro_crsprd
        mPro_paycon = pPro_paycon
        mPro_viacrs = pPro_viacrs
        mPro_route = pPro_route
        mPro_bsp = pPro_bsp
        mPro_agchg = pPro_agchg
        mPro_postbk = pPro_postbk
        mPro_vicode = pPro_vicode
        mPro_spare = pPro_spare
        mPro_tpcode = pPro_tpcode
        mPro_tpcde2 = pPro_tpcde2
        mPro_disvat = pPro_disvat
    End Sub

    Public Sub New( _
)
    End Sub

    Private mProductID As Integer
    Private mPro_id As String
    Private mPro_type As String
    Private mPro_desc As String
    Private mPro_vatabl As Nullable(Of Boolean)
    Private mPro_ccrate As String
    Private mPro_dccode As String
    Private mPro_axcode As String
    Private mPro_glcomm As String
    Private mPro_glsale As String
    Private mPro_glcost As String
    Private mPro_gl0com As String
    Private mPro_gl0sal As String
    Private mPro_gl0cos As String
    Private mPro_crsprd As String
    Private mPro_paycon As Nullable(Of Boolean)
    Private mPro_viacrs As Nullable(Of Boolean)
    Private mPro_route As Nullable(Of Boolean)
    Private mPro_bsp As Nullable(Of Boolean)
    Private mPro_agchg As Decimal
    Private mPro_postbk As Nullable(Of Boolean)
    Private mPro_vicode As String
    Private mPro_spare As String
    Private mPro_tpcode As String
    Private mPro_tpcde2 As String
    Private mPro_disvat As Nullable(Of Boolean)

    Public Property ProductID() As Integer
        Get
            Return mProductID
        End Get
        Set(ByVal value As Integer)
            mProductID = value
        End Set
    End Property

    Public Property Pro_id() As String
        Get
            Return mPro_id
        End Get
        Set(ByVal value As String)
            mPro_id = value
        End Set
    End Property

    Public Property Pro_type() As String
        Get
            Return mPro_type
        End Get
        Set(ByVal value As String)
            mPro_type = value
        End Set
    End Property

    Public Property Pro_desc() As String
        Get
            Return mPro_desc
        End Get
        Set(ByVal value As String)
            mPro_desc = value
        End Set
    End Property

    Public Property Pro_vatabl() As Nullable(Of Boolean)
        Get
            Return mPro_vatabl
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mPro_vatabl = value
        End Set
    End Property

    Public Property Pro_ccrate() As String
        Get
            Return mPro_ccrate
        End Get
        Set(ByVal value As String)
            mPro_ccrate = value
        End Set
    End Property

    Public Property Pro_dccode() As String
        Get
            Return mPro_dccode
        End Get
        Set(ByVal value As String)
            mPro_dccode = value
        End Set
    End Property

    Public Property Pro_axcode() As String
        Get
            Return mPro_axcode
        End Get
        Set(ByVal value As String)
            mPro_axcode = value
        End Set
    End Property

    Public Property Pro_glcomm() As String
        Get
            Return mPro_glcomm
        End Get
        Set(ByVal value As String)
            mPro_glcomm = value
        End Set
    End Property

    Public Property Pro_glsale() As String
        Get
            Return mPro_glsale
        End Get
        Set(ByVal value As String)
            mPro_glsale = value
        End Set
    End Property

    Public Property Pro_glcost() As String
        Get
            Return mPro_glcost
        End Get
        Set(ByVal value As String)
            mPro_glcost = value
        End Set
    End Property

    Public Property Pro_gl0com() As String
        Get
            Return mPro_gl0com
        End Get
        Set(ByVal value As String)
            mPro_gl0com = value
        End Set
    End Property

    Public Property Pro_gl0sal() As String
        Get
            Return mPro_gl0sal
        End Get
        Set(ByVal value As String)
            mPro_gl0sal = value
        End Set
    End Property

    Public Property Pro_gl0cos() As String
        Get
            Return mPro_gl0cos
        End Get
        Set(ByVal value As String)
            mPro_gl0cos = value
        End Set
    End Property

    Public Property Pro_crsprd() As String
        Get
            Return mPro_crsprd
        End Get
        Set(ByVal value As String)
            mPro_crsprd = value
        End Set
    End Property

    Public Property Pro_paycon() As Nullable(Of Boolean)
        Get
            Return mPro_paycon
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mPro_paycon = value
        End Set
    End Property

    Public Property Pro_viacrs() As Nullable(Of Boolean)
        Get
            Return mPro_viacrs
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mPro_viacrs = value
        End Set
    End Property

    Public Property Pro_route() As Nullable(Of Boolean)
        Get
            Return mPro_route
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mPro_route = value
        End Set
    End Property

    Public Property Pro_bsp() As Nullable(Of Boolean)
        Get
            Return mPro_bsp
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mPro_bsp = value
        End Set
    End Property

    Public Property Pro_agchg() As Decimal
        Get
            Return mPro_agchg
        End Get
        Set(ByVal value As Decimal)
            mPro_agchg = value
        End Set
    End Property

    Public Property Pro_postbk() As Nullable(Of Boolean)
        Get
            Return mPro_postbk
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mPro_postbk = value
        End Set
    End Property

    Public Property Pro_vicode() As String
        Get
            Return mPro_vicode
        End Get
        Set(ByVal value As String)
            mPro_vicode = value
        End Set
    End Property

    Public Property Pro_spare() As String
        Get
            Return mPro_spare
        End Get
        Set(ByVal value As String)
            mPro_spare = value
        End Set
    End Property

    Public Property Pro_tpcode() As String
        Get
            Return mPro_tpcode
        End Get
        Set(ByVal value As String)
            mPro_tpcode = value
        End Set
    End Property

    Public Property Pro_tpcde2() As String
        Get
            Return mPro_tpcde2
        End Get
        Set(ByVal value As String)
            mPro_tpcde2 = value
        End Set
    End Property

    Public Property Pro_disvat() As Nullable(Of Boolean)
        Get
            Return mPro_disvat
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mPro_disvat = value
        End Set
    End Property

    Private Shared Function makeBOSSproductFromRow( _
            ByVal r As IDataReader _
        ) As BOSSproduct
        Return New BOSSproduct( _
                clsUseful.notInteger(r.Item("productID")), _
                clsUseful.notString(r.Item("pro_id")), _
                clsUseful.notString(r.Item("pro_type")), _
                clsUseful.notString(r.Item("pro_desc")), _
                toNullableBoolean(r.Item("pro_vatabl")), _
                clsUseful.notString(r.Item("pro_ccrate")), _
                clsUseful.notString(r.Item("pro_dccode")), _
                clsUseful.notString(r.Item("pro_axcode")), _
                clsUseful.notString(r.Item("pro_glcomm")), _
                clsUseful.notString(r.Item("pro_glsale")), _
                clsUseful.notString(r.Item("pro_glcost")), _
                clsUseful.notString(r.Item("pro_gl0com")), _
                clsUseful.notString(r.Item("pro_gl0sal")), _
                clsUseful.notString(r.Item("pro_gl0cos")), _
                clsUseful.notString(r.Item("pro_crsprd")), _
                toNullableBoolean(r.Item("pro_paycon")), _
                toNullableBoolean(r.Item("pro_viacrs")), _
                toNullableBoolean(r.Item("pro_route")), _
                toNullableBoolean(r.Item("pro_bsp")), _
                toNullableFloat(r.Item("pro_agchg")), _
                toNullableBoolean(r.Item("pro_postbk")), _
                clsUseful.notString(r.Item("pro_vicode")), _
                clsUseful.notString(r.Item("pro_spare")), _
                clsUseful.notString(r.Item("pro_tpcode")), _
                clsUseful.notString(r.Item("pro_tpcde2")), _
                toNullableBoolean(r.Item("pro_disvat")))
    End Function

    Public Shared Function [get]( _
            ByVal pProductID As Integer _
        ) As BOSSproduct
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("BOSSproduct_get", "@productID", pProductID)
                If Not r.Read() Then
                    Throw New Exception("No BOSSproduct with id " & pProductID)
                End If
                Dim ret As BOSSproduct = makeBOSSproductFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of BOSSproduct)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BOSSproduct)()
            Using r As IDataReader = dbh.callSP("BOSSproduct_list")
                While r.Read()
                    ret.Add(makeBOSSproductFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim strRet As String = CStr(dbh.callSPSingleValue("BOSSproduct_save", "@ProductID", mProductID, "@Pro_id", mPro_id, "@Pro_type", mPro_type, "@Pro_desc", mPro_desc, _
                                                    "@Pro_vatabl", mPro_vatabl, "@Pro_ccrate", mPro_ccrate, "@Pro_dccode", mPro_dccode, "@Pro_axcode", mPro_axcode, _
                                                    "@Pro_glcomm", mPro_glcomm, "@Pro_glsale", mPro_glsale, "@Pro_glcost", mPro_glcost, "@Pro_gl0com", mPro_gl0com, _
                                                    "@Pro_gl0sal", mPro_gl0sal, "@Pro_gl0cos", mPro_gl0cos, "@Pro_crsprd", mPro_crsprd, "@Pro_paycon", mPro_paycon, _
                                                    "@Pro_viacrs", mPro_viacrs, "@Pro_route", mPro_route, "@Pro_bsp", mPro_bsp, "@Pro_agchg", mPro_agchg, "@Pro_postbk", _
                                                    mPro_postbk, "@Pro_vicode", mPro_vicode, "@Pro_spare", mPro_spare, "@Pro_tpcode", mPro_tpcode, "@Pro_tpcde2", mPro_tpcde2, _
                                                    "@Pro_disvat", mPro_disvat))
            Return strRet
        End Using
    End Function

    Public Shared Sub delete(ByVal pProductID As Integer, ByVal pPro_id As String)
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("BOSSproduct_delete", "@ProductID", pProductID, "@Pro_id", pPro_id)
        End Using
    End Sub
End Class
