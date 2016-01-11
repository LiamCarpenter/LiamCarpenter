Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSSsupplier

    Public Sub New( _
        ByVal pSupplierID As Integer, _
        ByVal pSup_id As String, _
        ByVal pSup_desc As String, _
        ByVal pSup_type As String, _
        ByVal pSup_grpid As String, _
        ByVal pSup_comfrm As String, _
        ByVal pSup_name As String, _
        ByVal pSup_add1 As String, _
        ByVal pSup_add2 As String, _
        ByVal pSup_add3 As String, _
        ByVal pSup_add4 As String, _
        ByVal pSup_pcode1 As String, _
        ByVal pSup_pcode2 As String, _
        ByVal pSup_cntry As String, _
        ByVal pSup_contac As String, _
        ByVal pSup_tel As String, _
        ByVal pSup_fax As String, _
        ByVal pSup_email As String, _
        ByVal pSup_email2 As String, _
        ByVal pSup_cdate As Nullable(Of DateTime), _
        ByVal pSup_ldate As Nullable(Of DateTime), _
        ByVal pSup_acode As String, _
        ByVal pSup_dccode As String, _
        ByVal pSup_atol As String, _
        ByVal pSup_atolag As Nullable(Of Boolean), _
        ByVal pSup_prefrd As Nullable(Of Boolean), _
        ByVal pSup_paygrs As Nullable(Of Boolean), _
        ByVal pSup_selfbl As Nullable(Of Boolean), _
        ByVal pSup_docmnt As Nullable(Of Boolean), _
        ByVal pSup_dcomp1 As Decimal, _
        ByVal pSup_dcomp2 As Decimal, _
        ByVal pSup_glcode As Decimal, _
        ByVal pSup_pcent As Decimal, _
        ByVal pSup_xpense As String, _
        ByVal pSup_cterms As Decimal, _
        ByVal pSup_bdueto As Decimal, _
        ByVal pSup_bduefm As Decimal, _
        ByVal pSup_pmeth As String, _
        ByVal pSup_pmethc As String, _
        ByVal pSup_notes As String, _
        ByVal pSup_popup As String, _
        ByVal pSup_dppc As Decimal, _
        ByVal pSup_dpamt As Decimal, _
        ByVal pSup_ubtax As Nullable(Of Boolean), _
        ByVal pSup_yqtax As Nullable(Of Boolean), _
        ByVal pSup_ouracc As String, _
        ByVal pSup_fullcl As String, _
        ByVal pSup_vatabl As Nullable(Of Boolean), _
        ByVal pSup_vatno As String)
        mSupplierID = pSupplierID
        mSup_id = pSup_id
        mSup_desc = pSup_desc
        mSup_type = pSup_type
        mSup_grpid = pSup_grpid
        mSup_comfrm = pSup_comfrm
        mSup_name = pSup_name
        mSup_add1 = pSup_add1
        mSup_add2 = pSup_add2
        mSup_add3 = pSup_add3
        mSup_add4 = pSup_add4
        mSup_pcode1 = pSup_pcode1
        mSup_pcode2 = pSup_pcode2
        mSup_cntry = pSup_cntry
        mSup_contac = pSup_contac
        mSup_tel = pSup_tel
        mSup_fax = pSup_fax
        mSup_email = pSup_email
        mSup_email2 = pSup_email2
        mSup_cdate = pSup_cdate
        mSup_ldate = pSup_ldate
        mSup_acode = pSup_acode
        mSup_dccode = pSup_dccode
        mSup_atol = pSup_atol
        mSup_atolag = pSup_atolag
        mSup_prefrd = pSup_prefrd
        mSup_paygrs = pSup_paygrs
        mSup_selfbl = pSup_selfbl
        mSup_docmnt = pSup_docmnt
        mSup_dcomp1 = pSup_dcomp1
        mSup_dcomp2 = pSup_dcomp2
        mSup_glcode = pSup_glcode
        mSup_pcent = pSup_pcent
        mSup_xpense = pSup_xpense
        mSup_cterms = pSup_cterms
        mSup_bdueto = pSup_bdueto
        mSup_bduefm = pSup_bduefm
        mSup_pmeth = pSup_pmeth
        mSup_pmethc = pSup_pmethc
        mSup_notes = pSup_notes
        mSup_popup = pSup_popup
        mSup_dppc = pSup_dppc
        mSup_dpamt = pSup_dpamt
        mSup_ubtax = pSup_ubtax
        mSup_yqtax = pSup_yqtax
        mSup_ouracc = pSup_ouracc
        mSup_fullcl = pSup_fullcl
        mSup_vatabl = pSup_vatabl
        mSup_vatno = pSup_vatno
    End Sub

    Public Sub New( _
)
    End Sub

    Private mSupplierID As Integer
    Private mSup_id As String
    Private mSup_desc As String
    Private mSup_type As String
    Private mSup_grpid As String
    Private mSup_comfrm As String
    Private mSup_name As String
    Private mSup_add1 As String
    Private mSup_add2 As String
    Private mSup_add3 As String
    Private mSup_add4 As String
    Private mSup_pcode1 As String
    Private mSup_pcode2 As String
    Private mSup_cntry As String
    Private mSup_contac As String
    Private mSup_tel As String
    Private mSup_fax As String
    Private mSup_email As String
    Private mSup_email2 As String
    Private mSup_cdate As Nullable(Of DateTime)
    Private mSup_ldate As Nullable(Of DateTime)
    Private mSup_acode As String
    Private mSup_dccode As String
    Private mSup_atol As String
    Private mSup_atolag As Nullable(Of Boolean)
    Private mSup_prefrd As Nullable(Of Boolean)
    Private mSup_paygrs As Nullable(Of Boolean)
    Private mSup_selfbl As Nullable(Of Boolean)
    Private mSup_docmnt As Nullable(Of Boolean)
    Private mSup_dcomp1 As Decimal
    Private mSup_dcomp2 As Decimal
    Private mSup_glcode As Decimal
    Private mSup_pcent As Decimal
    Private mSup_xpense As String
    Private mSup_cterms As Decimal
    Private mSup_bdueto As Decimal
    Private mSup_bduefm As Decimal
    Private mSup_pmeth As String
    Private mSup_pmethc As String
    Private mSup_notes As String
    Private mSup_popup As String
    Private mSup_dppc As Decimal
    Private mSup_dpamt As Decimal
    Private mSup_ubtax As Nullable(Of Boolean)
    Private mSup_yqtax As Nullable(Of Boolean)
    Private mSup_ouracc As String
    Private mSup_fullcl As String
    Private mSup_vatabl As Nullable(Of Boolean)
    Private mSup_vatno As String

    Public Property SupplierID() As Integer
        Get
            Return mSupplierID
        End Get
        Set(ByVal value As Integer)
            mSupplierID = value
        End Set
    End Property

    Public Property Sup_id() As String
        Get
            Return mSup_id
        End Get
        Set(ByVal value As String)
            mSup_id = value
        End Set
    End Property

    Public Property Sup_desc() As String
        Get
            Return mSup_desc
        End Get
        Set(ByVal value As String)
            mSup_desc = value
        End Set
    End Property

    Public Property Sup_type() As String
        Get
            Return mSup_type
        End Get
        Set(ByVal value As String)
            mSup_type = value
        End Set
    End Property

    Public Property Sup_grpid() As String
        Get
            Return mSup_grpid
        End Get
        Set(ByVal value As String)
            mSup_grpid = value
        End Set
    End Property

    Public Property Sup_comfrm() As String
        Get
            Return mSup_comfrm
        End Get
        Set(ByVal value As String)
            mSup_comfrm = value
        End Set
    End Property

    Public Property Sup_name() As String
        Get
            Return mSup_name
        End Get
        Set(ByVal value As String)
            mSup_name = value
        End Set
    End Property

    Public Property Sup_add1() As String
        Get
            Return mSup_add1
        End Get
        Set(ByVal value As String)
            mSup_add1 = value
        End Set
    End Property

    Public Property Sup_add2() As String
        Get
            Return mSup_add2
        End Get
        Set(ByVal value As String)
            mSup_add2 = value
        End Set
    End Property

    Public Property Sup_add3() As String
        Get
            Return mSup_add3
        End Get
        Set(ByVal value As String)
            mSup_add3 = value
        End Set
    End Property

    Public Property Sup_add4() As String
        Get
            Return mSup_add4
        End Get
        Set(ByVal value As String)
            mSup_add4 = value
        End Set
    End Property

    Public Property Sup_pcode1() As String
        Get
            Return mSup_pcode1
        End Get
        Set(ByVal value As String)
            mSup_pcode1 = value
        End Set
    End Property

    Public Property Sup_pcode2() As String
        Get
            Return mSup_pcode2
        End Get
        Set(ByVal value As String)
            mSup_pcode2 = value
        End Set
    End Property

    Public Property Sup_cntry() As String
        Get
            Return mSup_cntry
        End Get
        Set(ByVal value As String)
            mSup_cntry = value
        End Set
    End Property

    Public Property Sup_contac() As String
        Get
            Return mSup_contac
        End Get
        Set(ByVal value As String)
            mSup_contac = value
        End Set
    End Property

    Public Property Sup_tel() As String
        Get
            Return mSup_tel
        End Get
        Set(ByVal value As String)
            mSup_tel = value
        End Set
    End Property

    Public Property Sup_fax() As String
        Get
            Return mSup_fax
        End Get
        Set(ByVal value As String)
            mSup_fax = value
        End Set
    End Property

    Public Property Sup_email() As String
        Get
            Return mSup_email
        End Get
        Set(ByVal value As String)
            mSup_email = value
        End Set
    End Property

    Public Property Sup_email2() As String
        Get
            Return mSup_email2
        End Get
        Set(ByVal value As String)
            mSup_email2 = value
        End Set
    End Property

    Public Property Sup_cdate() As Nullable(Of DateTime)
        Get
            Return mSup_cdate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mSup_cdate = value
        End Set
    End Property

    Public Property Sup_ldate() As Nullable(Of DateTime)
        Get
            Return mSup_ldate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mSup_ldate = value
        End Set
    End Property

    Public Property Sup_acode() As String
        Get
            Return mSup_acode
        End Get
        Set(ByVal value As String)
            mSup_acode = value
        End Set
    End Property

    Public Property Sup_dccode() As String
        Get
            Return mSup_dccode
        End Get
        Set(ByVal value As String)
            mSup_dccode = value
        End Set
    End Property

    Public Property Sup_atol() As String
        Get
            Return mSup_atol
        End Get
        Set(ByVal value As String)
            mSup_atol = value
        End Set
    End Property

    Public Property Sup_atolag() As Nullable(Of Boolean)
        Get
            Return mSup_atolag
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSup_atolag = value
        End Set
    End Property

    Public Property Sup_prefrd() As Nullable(Of Boolean)
        Get
            Return mSup_prefrd
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSup_prefrd = value
        End Set
    End Property

    Public Property Sup_paygrs() As Nullable(Of Boolean)
        Get
            Return mSup_paygrs
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSup_paygrs = value
        End Set
    End Property

    Public Property Sup_selfbl() As Nullable(Of Boolean)
        Get
            Return mSup_selfbl
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSup_selfbl = value
        End Set
    End Property

    Public Property Sup_docmnt() As Nullable(Of Boolean)
        Get
            Return mSup_docmnt
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSup_docmnt = value
        End Set
    End Property

    Public Property Sup_dcomp1() As Decimal
        Get
            Return mSup_dcomp1
        End Get
        Set(ByVal value As Decimal)
            mSup_dcomp1 = value
        End Set
    End Property

    Public Property Sup_dcomp2() As Decimal
        Get
            Return mSup_dcomp2
        End Get
        Set(ByVal value As Decimal)
            mSup_dcomp2 = value
        End Set
    End Property

    Public Property Sup_glcode() As Decimal
        Get
            Return mSup_glcode
        End Get
        Set(ByVal value As Decimal)
            mSup_glcode = value
        End Set
    End Property

    Public Property Sup_pcent() As Decimal
        Get
            Return mSup_pcent
        End Get
        Set(ByVal value As Decimal)
            mSup_pcent = value
        End Set
    End Property

    Public Property Sup_xpense() As String
        Get
            Return mSup_xpense
        End Get
        Set(ByVal value As String)
            mSup_xpense = value
        End Set
    End Property

    Public Property Sup_cterms() As Decimal
        Get
            Return mSup_cterms
        End Get
        Set(ByVal value As Decimal)
            mSup_cterms = value
        End Set
    End Property

    Public Property Sup_bdueto() As Decimal
        Get
            Return mSup_bdueto
        End Get
        Set(ByVal value As Decimal)
            mSup_bdueto = value
        End Set
    End Property

    Public Property Sup_bduefm() As Decimal
        Get
            Return mSup_bduefm
        End Get
        Set(ByVal value As Decimal)
            mSup_bduefm = value
        End Set
    End Property

    Public Property Sup_pmeth() As String
        Get
            Return mSup_pmeth
        End Get
        Set(ByVal value As String)
            mSup_pmeth = value
        End Set
    End Property

    Public Property Sup_pmethc() As String
        Get
            Return mSup_pmethc
        End Get
        Set(ByVal value As String)
            mSup_pmethc = value
        End Set
    End Property

    Public Property Sup_notes() As String
        Get
            Return mSup_notes
        End Get
        Set(ByVal value As String)
            mSup_notes = value
        End Set
    End Property

    Public Property Sup_popup() As String
        Get
            Return mSup_popup
        End Get
        Set(ByVal value As String)
            mSup_popup = value
        End Set
    End Property

    Public Property Sup_dppc() As Decimal
        Get
            Return mSup_dppc
        End Get
        Set(ByVal value As Decimal)
            mSup_dppc = value
        End Set
    End Property

    Public Property Sup_dpamt() As Decimal
        Get
            Return mSup_dpamt
        End Get
        Set(ByVal value As Decimal)
            mSup_dpamt = value
        End Set
    End Property

    Public Property Sup_ubtax() As Nullable(Of Boolean)
        Get
            Return mSup_ubtax
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSup_ubtax = value
        End Set
    End Property

    Public Property Sup_yqtax() As Nullable(Of Boolean)
        Get
            Return mSup_yqtax
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSup_yqtax = value
        End Set
    End Property

    Public Property Sup_ouracc() As String
        Get
            Return mSup_ouracc
        End Get
        Set(ByVal value As String)
            mSup_ouracc = value
        End Set
    End Property

    Public Property Sup_fullcl() As String
        Get
            Return mSup_fullcl
        End Get
        Set(ByVal value As String)
            mSup_fullcl = value
        End Set
    End Property

    Public Property Sup_vatabl() As Nullable(Of Boolean)
        Get
            Return mSup_vatabl
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSup_vatabl = value
        End Set
    End Property

    Public Property Sup_vatno() As String
        Get
            Return mSup_vatno
        End Get
        Set(ByVal value As String)
            mSup_vatno = value
        End Set
    End Property

    Private Shared Function makeBOSSsupplierFromRow( _
            ByVal r As IDataReader _
        ) As BOSSsupplier
        Return New BOSSsupplier( _
                clsUseful.notInteger(r.Item("supplierID")), _
                clsUseful.notString(r.Item("sup_id")), _
                clsUseful.notString(r.Item("sup_desc")), _
                clsUseful.notString(r.Item("sup_type")), _
                clsUseful.notString(r.Item("sup_grpid")), _
                clsUseful.notString(r.Item("sup_comfrm")), _
                clsUseful.notString(r.Item("sup_name")), _
                clsUseful.notString(r.Item("sup_add1")), _
                clsUseful.notString(r.Item("sup_add2")), _
                clsUseful.notString(r.Item("sup_add3")), _
                clsUseful.notString(r.Item("sup_add4")), _
                clsUseful.notString(r.Item("sup_pcode1")), _
                clsUseful.notString(r.Item("sup_pcode2")), _
                clsUseful.notString(r.Item("sup_cntry")), _
                clsUseful.notString(r.Item("sup_contac")), _
                clsUseful.notString(r.Item("sup_tel")), _
                clsUseful.notString(r.Item("sup_fax")), _
                clsUseful.notString(r.Item("sup_email")), _
                clsUseful.notString(r.Item("sup_email2")), _
                toNullableDate(r.Item("sup_cdate")), _
                toNullableDate(r.Item("sup_ldate")), _
                clsUseful.notString(r.Item("sup_acode")), _
                clsUseful.notString(r.Item("sup_dccode")), _
                clsUseful.notString(r.Item("sup_atol")), _
                toNullableBoolean(r.Item("sup_atolag")), _
                toNullableBoolean(r.Item("sup_prefrd")), _
                toNullableBoolean(r.Item("sup_paygrs")), _
                toNullableBoolean(r.Item("sup_selfbl")), _
                toNullableBoolean(r.Item("sup_docmnt")), _
                toNullableFloat(r.Item("sup_dcomp1")), _
                toNullableFloat(r.Item("sup_dcomp2")), _
                toNullableInteger(r.Item("sup_glcode")), _
                toNullableFloat(r.Item("sup_pcent")), _
                clsUseful.notString(r.Item("sup_xpense")), _
                toNullableInteger(r.Item("sup_cterms")), _
                toNullableInteger(r.Item("sup_bdueto")), _
                toNullableInteger(r.Item("sup_bduefm")), _
                clsUseful.notString(r.Item("sup_pmeth")), _
                clsUseful.notString(r.Item("sup_pmethc")), _
                clsUseful.notString(r.Item("sup_notes")), _
                clsUseful.notString(r.Item("sup_popup")), _
                toNullableFloat(r.Item("sup_dppc")), _
                toNullableFloat(r.Item("sup_dpamt")), _
                toNullableBoolean(r.Item("sup_ubtax")), _
                toNullableBoolean(r.Item("sup_yqtax")), _
                clsUseful.notString(r.Item("sup_ouracc")), _
                clsUseful.notString(r.Item("sup_fullcl")), _
                toNullableBoolean(r.Item("sup_vatabl")), _
                clsUseful.notString(r.Item("sup_vatno")))
    End Function

    Public Shared Function [get]( _
            ByVal pSupplierID As Integer _
        ) As BOSSsupplier
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("BOSSsupplier_get", "@supplierID", pSupplierID)
                If Not r.Read() Then
                    Throw New Exception("No BOSSsupplier with id " & pSupplierID)
                End If
                Dim ret As BOSSsupplier = makeBOSSsupplierFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of BOSSsupplier)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BOSSsupplier)()
            Using r As IDataReader = dbh.callSP("BOSSsupplier_list")
                While r.Read()
                    ret.Add(makeBOSSsupplierFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim strRet As String = CStr(dbh.callSPSingleValue("BOSSsupplier_save", "@SupplierID", mSupplierID, "@Sup_id", mSup_id, "@Sup_desc", mSup_desc, "@Sup_type", mSup_type, "@Sup_grpid", mSup_grpid, _
                                                     "@Sup_comfrm", mSup_comfrm, "@Sup_name", mSup_name, "@Sup_add1", mSup_add1, "@Sup_add2", mSup_add2, "@Sup_add3", mSup_add3, "@Sup_add4", mSup_add4, _
                                                     "@Sup_pcode1", mSup_pcode1, "@Sup_pcode2", mSup_pcode2, "@Sup_cntry", mSup_cntry, "@Sup_contac", mSup_contac, "@Sup_tel", mSup_tel, "@Sup_fax", _
                                                     mSup_fax, "@Sup_email", mSup_email, "@Sup_email2", mSup_email2, "@Sup_cdate", mSup_cdate, "@Sup_ldate", mSup_ldate, "@Sup_acode", mSup_acode, _
                                                     "@Sup_dccode", mSup_dccode, "@Sup_atol", mSup_atol, "@Sup_atolag", mSup_atolag, "@Sup_prefrd", mSup_prefrd, "@Sup_paygrs", mSup_paygrs, "@Sup_selfbl", _
                                                     mSup_selfbl, "@Sup_docmnt", mSup_docmnt, "@Sup_dcomp1", mSup_dcomp1, "@Sup_dcomp2", mSup_dcomp2, "@Sup_glcode", mSup_glcode, "@Sup_pcent", mSup_pcent, _
                                                     "@Sup_xpense", mSup_xpense, "@Sup_cterms", mSup_cterms, "@Sup_bdueto", mSup_bdueto, "@Sup_bduefm", mSup_bduefm, "@Sup_pmeth", mSup_pmeth, "@Sup_pmethc", _
                                                     mSup_pmethc, "@Sup_notes", mSup_notes, "@Sup_popup", mSup_popup, "@Sup_dppc", mSup_dppc, "@Sup_dpamt", mSup_dpamt, "@Sup_ubtax", mSup_ubtax, "@Sup_yqtax", _
                                                     mSup_yqtax, "@Sup_ouracc", mSup_ouracc, "@Sup_fullcl", mSup_fullcl, "@Sup_vatabl", mSup_vatabl, "@Sup_vatno", mSup_vatno))
            Return strRet
        End Using
    End Function

    Public Shared Sub delete( _
            ByVal pSupplierID As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("BOSSsupplier_delete", "@SupplierID", pSupplierID)
        End Using
    End Sub

    Public Shared Function getSupplierBoss(ByVal pstrInvoiceRef As String) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim strRet As String = clsUseful.notString(dbh.callSPSingleValueCanReturnNothing("bossSupplier_getID", "@invoiceref", pstrInvoiceRef))
            Return strRet
        End Using
    End Function

End Class
