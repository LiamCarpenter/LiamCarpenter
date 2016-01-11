Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSScustomer

    Public Sub New( _
        ByVal pCustomerID As Integer, _
        ByVal pCus_id As String, _
        ByVal pCus_type As String, _
        ByVal pCus_grpid As String, _
        ByVal pCus_grpid2 As String, _
        ByVal pCus_grpid3 As String, _
        ByVal pCus_br As String, _
        ByVal pCus_coname As String, _
        ByVal pCus_add1 As String, _
        ByVal pCus_add2 As String, _
        ByVal pCus_add3 As String, _
        ByVal pCus_add4 As String, _
        ByVal pCus_pcode1 As String, _
        ByVal pCus_pcode2 As String, _
        ByVal pCus_cntry As String, _
        ByVal pCus_cntct1 As String, _
        ByVal pCus_cntct2 As String, _
        ByVal pCus_cntct3 As String, _
        ByVal pCus_cntct4 As String, _
        ByVal pCus_cntct5 As String, _
        ByVal pCus_tel As String, _
        ByVal pCus_fax As String, _
        ByVal pCus_email As String, _
        ByVal pCus_email2 As String, _
        ByVal pCus_cdate As Nullable(Of DateTime), _
        ByVal pCus_fdate As Nullable(Of DateTime), _
        ByVal pCus_ldate As Nullable(Of DateTime), _
        ByVal pCus_onact As Decimal, _
        ByVal pCus_insure As Boolean, _
        ByVal pCus_insexp As Nullable(Of DateTime), _
        ByVal pCus_sref1 As String, _
        ByVal pCus_sref2 As String, _
        ByVal pCus_sref3 As String, _
        ByVal pCus_sref4 As String, _
        ByVal pCus_sref5 As String, _
        ByVal pCus_sref6 As String, _
        ByVal pCus_con1 As String, _
        ByVal pCus_c1mth1 As Integer, _
        ByVal pCus_c1mth2 As Integer, _
        ByVal pCus_c1mth3 As Integer, _
        ByVal pCus_c1pct1 As Decimal, _
        ByVal pCus_c1pct2 As Decimal, _
        ByVal pCus_c1pct3 As Decimal, _
        ByVal pCus_c1pct4 As Decimal, _
        ByVal pCus_con2 As String, _
        ByVal pCus_c2mth1 As Integer, _
        ByVal pCus_c2mth2 As Integer, _
        ByVal pCus_c2mth3 As Integer, _
        ByVal pCus_c2pct1 As Double, _
        ByVal pCus_c2pct2 As Decimal, _
        ByVal pCus_c2pct3 As Decimal, _
        ByVal pCus_c2pct4 As Decimal, _
        ByVal pCus_ptermc As String, _
        ByVal pCus_ptermv As Integer, _
        ByVal pCus_crep As Boolean, _
        ByVal pCus_state As Boolean, _
        ByVal pCus_pono As Boolean, _
        ByVal pCus_costc As Boolean, _
        ByVal pCus_cref1 As Boolean, _
        ByVal pCus_cref2 As Boolean, _
        ByVal pCus_ccard As Boolean, _
        ByVal pCus_cconly As Boolean, _
        ByVal pCus_climit As Decimal, _
        ByVal pCus_debt As Decimal, _
        ByVal pCus_stop As Boolean, _
        ByVal pCus_2ndadd As Boolean, _
        ByVal pCus_notes As String, _
        ByVal pCus_stadd As String, _
        ByVal pCus_soy As Integer, _
        ByVal pCus_conam2 As String, _
        ByVal pCus_pop1 As String, _
        ByVal pCus_pop2 As String, _
        ByVal pCus_ovrdue As Decimal, _
        ByVal pCus_dscpct As Decimal, _
        ByVal pCus_fulreb As Boolean, _
        ByVal pCus_hidecm As Boolean, _
        ByVal pCus_rebcn As Boolean, _
        ByVal pCus_rebfrq As String, _
        ByVal pCus_reb1cn As Boolean, _
        ByVal pCus_rebinv As Boolean, _
        ByVal pCus_incfee As Boolean, _
        ByVal pCus_nocc As Boolean, _
        ByVal pCus_feeid1 As String, _
        ByVal pCus_feeid2 As String, _
        ByVal pCus_feeid3 As String, _
        ByVal pCus_adhoc As Boolean, _
        ByVal pCus_dofee As Boolean, _
        ByVal pCus_ibank As Boolean, _
        ByVal pCus_feetyp As String, _
        ByVal pCus_atol As String, _
        ByVal pCus_paydd As Boolean, _
        ByVal pCus_sps As Boolean, _
        ByVal pCus_acas As Boolean, _
        ByVal pCus_abtano As String, _
        ByVal pCus_mailst As Boolean, _
        ByVal pCus_mailiv As Boolean, _
        ByVal pCus_coninv As Boolean, _
        ByVal pCus_cinvdt As Nullable(Of DateTime), _
        ByVal pCus_keyact As Boolean, _
        ByVal pCus_invfrm As String, _
        ByVal pCus_noba As Boolean, _
        ByVal pCus_slfbil As Boolean, _
        ByVal pCus_exvat As Boolean, _
        ByVal pCus_curncy As String, _
        ByVal pCus_onepdf As Boolean, _
        ByVal pCus_spare2 As String, _
        ByVal pCus_dorm As Boolean, _
        ByVal pCus_mltfee As Boolean, _
        ByVal pCus_print As Boolean, _
        ByVal pCus_pcode As String, _
        ByVal pcus_stfrm As String)
        mCustomerID = pCustomerID
        mCus_id = pCus_id
        mCus_type = pCus_type
        mCus_grpid = pCus_grpid
        mCus_grpid2 = pCus_grpid2
        mCus_grpid3 = pCus_grpid3
        mCus_br = pCus_br
        mCus_coname = pCus_coname
        mCus_add1 = pCus_add1
        mCus_add2 = pCus_add2
        mCus_add3 = pCus_add3
        mCus_add4 = pCus_add4
        mCus_pcode1 = pCus_pcode1
        mCus_pcode2 = pCus_pcode2
        mCus_cntry = pCus_cntry
        mCus_cntct1 = pCus_cntct1
        mCus_cntct2 = pCus_cntct2
        mCus_cntct3 = pCus_cntct3
        mCus_cntct4 = pCus_cntct4
        mCus_cntct5 = pCus_cntct5
        mCus_tel = pCus_tel
        mCus_fax = pCus_fax
        mCus_email = pCus_email
        mCus_email2 = pCus_email2
        mCus_cdate = pCus_cdate
        mCus_fdate = pCus_fdate
        mCus_ldate = pCus_ldate
        mCus_onact = pCus_onact
        mCus_insure = pCus_insure
        mCus_insexp = pCus_insexp
        mCus_sref1 = pCus_sref1
        mCus_sref2 = pCus_sref2
        mCus_sref3 = pCus_sref3
        mCus_sref4 = pCus_sref4
        mCus_sref5 = pCus_sref5
        mCus_sref6 = pCus_sref6
        mCus_con1 = pCus_con1
        mCus_c1mth1 = pCus_c1mth1
        mCus_c1mth2 = pCus_c1mth2
        mCus_c1mth3 = pCus_c1mth3
        mCus_c1pct1 = pCus_c1pct1
        mCus_c1pct2 = pCus_c1pct2
        mCus_c1pct3 = pCus_c1pct3
        mCus_c1pct4 = pCus_c1pct4
        mCus_con2 = pCus_con2
        mCus_c2mth1 = pCus_c2mth1
        mCus_c2mth2 = pCus_c2mth2
        mCus_c2mth3 = pCus_c2mth3
        mCus_c2pct1 = pCus_c2pct1
        mCus_c2pct2 = pCus_c2pct2
        mCus_c2pct3 = pCus_c2pct3
        mCus_c2pct4 = pCus_c2pct4
        mCus_ptermc = pCus_ptermc
        mCus_ptermv = pCus_ptermv
        mCus_crep = pCus_crep
        mCus_state = pCus_state
        mCus_pono = pCus_pono
        mCus_costc = pCus_costc
        mCus_cref1 = pCus_cref1
        mCus_cref2 = pCus_cref2
        mCus_ccard = pCus_ccard
        mCus_cconly = pCus_cconly
        mCus_climit = pCus_climit
        mCus_debt = pCus_debt
        mCus_stop = pCus_stop
        mCus_2ndadd = pCus_2ndadd
        mCus_notes = pCus_notes
        mCus_stadd = pCus_stadd
        mCus_soy = pCus_soy
        mCus_conam2 = pCus_conam2
        mCus_pop1 = pCus_pop1
        mCus_pop2 = pCus_pop2
        mCus_ovrdue = pCus_ovrdue
        mCus_dscpct = pCus_dscpct
        mCus_fulreb = pCus_fulreb
        mCus_hidecm = pCus_hidecm
        mCus_rebcn = pCus_rebcn
        mCus_rebfrq = pCus_rebfrq
        mCus_reb1cn = pCus_reb1cn
        mCus_rebinv = pCus_rebinv
        mCus_incfee = pCus_incfee
        mCus_nocc = pCus_nocc
        mCus_feeid1 = pCus_feeid1
        mCus_feeid2 = pCus_feeid2
        mCus_feeid3 = pCus_feeid3
        mCus_adhoc = pCus_adhoc
        mCus_dofee = pCus_dofee
        mCus_ibank = pCus_ibank
        mCus_feetyp = pCus_feetyp
        mCus_atol = pCus_atol
        mCus_paydd = pCus_paydd
        mCus_sps = pCus_sps
        mCus_acas = pCus_acas
        mCus_abtano = pCus_abtano
        mCus_mailst = pCus_mailst
        mCus_mailiv = pCus_mailiv
        mCus_coninv = pCus_coninv
        mCus_cinvdt = pCus_cinvdt
        mCus_keyact = pCus_keyact
        mCus_invfrm = pCus_invfrm
        mCus_noba = pCus_noba
        mCus_slfbil = pCus_slfbil
        mCus_exvat = pCus_exvat
        mCus_curncy = pCus_curncy
        mCus_onepdf = pCus_onepdf
        mCus_spare2 = pCus_spare2
        mCus_dorm = pCus_dorm
        mCus_mltfee = pCus_mltfee
        mCus_print = pCus_print
        mCus_pcode = pCus_pcode
        mcus_stfrm = pcus_stfrm
    End Sub

    Public Sub New( _
)
    End Sub

    Private mCus_pcode As String
    Private mcus_stfrm As String
    Private mCustomerID As Integer
    Private mCus_id As String
    Private mCus_type As String
    Private mCus_grpid As String
    Private mCus_grpid2 As String
    Private mCus_grpid3 As String
    Private mCus_br As String
    Private mCus_coname As String
    Private mCus_add1 As String
    Private mCus_add2 As String
    Private mCus_add3 As String
    Private mCus_add4 As String
    Private mCus_pcode1 As String
    Private mCus_pcode2 As String
    Private mCus_cntry As String
    Private mCus_cntct1 As String
    Private mCus_cntct2 As String
    Private mCus_cntct3 As String
    Private mCus_cntct4 As String
    Private mCus_cntct5 As String
    Private mCus_tel As String
    Private mCus_fax As String
    Private mCus_email As String
    Private mCus_email2 As String
    Private mCus_cdate As Nullable(Of DateTime)
    Private mCus_fdate As Nullable(Of DateTime)
    Private mCus_ldate As Nullable(Of DateTime)
    Private mCus_onact As Decimal
    Private mCus_insure As Boolean
    Private mCus_insexp As Nullable(Of DateTime)
    Private mCus_sref1 As String
    Private mCus_sref2 As String
    Private mCus_sref3 As String
    Private mCus_sref4 As String
    Private mCus_sref5 As String
    Private mCus_sref6 As String
    Private mCus_con1 As String
    Private mCus_c1mth1 As Integer
    Private mCus_c1mth2 As Integer
    Private mCus_c1mth3 As Integer
    Private mCus_c1pct1 As Decimal
    Private mCus_c1pct2 As Decimal
    Private mCus_c1pct3 As Decimal
    Private mCus_c1pct4 As Decimal
    Private mCus_con2 As String
    Private mCus_c2mth1 As Integer
    Private mCus_c2mth2 As Integer
    Private mCus_c2mth3 As Integer
    Private mCus_c2pct1 As Double
    Private mCus_c2pct2 As Decimal
    Private mCus_c2pct3 As Decimal
    Private mCus_c2pct4 As Decimal
    Private mCus_ptermc As String
    Private mCus_ptermv As Integer
    Private mCus_crep As Boolean
    Private mCus_state As Boolean
    Private mCus_pono As Boolean
    Private mCus_costc As Boolean
    Private mCus_cref1 As Boolean
    Private mCus_cref2 As Boolean
    Private mCus_ccard As Boolean
    Private mCus_cconly As Boolean
    Private mCus_climit As Decimal
    Private mCus_debt As Decimal
    Private mCus_stop As Boolean
    Private mCus_2ndadd As Boolean
    Private mCus_notes As String
    Private mCus_stadd As String
    Private mCus_soy As Integer
    Private mCus_conam2 As String
    Private mCus_pop1 As String
    Private mCus_pop2 As String
    Private mCus_ovrdue As Decimal
    Private mCus_dscpct As Decimal
    Private mCus_fulreb As Boolean
    Private mCus_hidecm As Boolean
    Private mCus_rebcn As Boolean
    Private mCus_rebfrq As String
    Private mCus_reb1cn As Boolean
    Private mCus_rebinv As Boolean
    Private mCus_incfee As Boolean
    Private mCus_nocc As Boolean
    Private mCus_feeid1 As String
    Private mCus_feeid2 As String
    Private mCus_feeid3 As String
    Private mCus_adhoc As Boolean
    Private mCus_dofee As Boolean
    Private mCus_ibank As Boolean
    Private mCus_feetyp As String
    Private mCus_atol As String
    Private mCus_paydd As Boolean
    Private mCus_sps As Boolean
    Private mCus_acas As Boolean
    Private mCus_abtano As String
    Private mCus_mailst As Boolean
    Private mCus_mailiv As Boolean
    Private mCus_coninv As Boolean
    Private mCus_cinvdt As Nullable(Of DateTime)
    Private mCus_keyact As Boolean
    Private mCus_invfrm As String
    Private mCus_noba As Boolean
    Private mCus_slfbil As Boolean
    Private mCus_exvat As Boolean
    Private mCus_curncy As String
    Private mCus_onepdf As Boolean
    Private mCus_spare2 As String
    Private mCus_dorm As Boolean
    Private mCus_mltfee As Boolean
    Private mCus_print As Boolean

    Public Property CustomerID() As Integer
        Get
            Return mCustomerID
        End Get
        Set(ByVal value As Integer)
            mCustomerID = value
        End Set
    End Property

    Public Property Cus_pcode() As String
        Get
            Return mCus_pcode
        End Get
        Set(ByVal value As String)
            mCus_pcode = value
        End Set
    End Property

    Public Property cus_stfrm() As String
        Get
            Return mcus_stfrm
        End Get
        Set(ByVal value As String)
            mcus_stfrm = value
        End Set
    End Property

    Public Property Cus_id() As String
        Get
            Return mCus_id
        End Get
        Set(ByVal value As String)
            mCus_id = value
        End Set
    End Property

    Public Property Cus_type() As String
        Get
            Return mCus_type
        End Get
        Set(ByVal value As String)
            mCus_type = value
        End Set
    End Property

    Public Property Cus_grpid() As String
        Get
            Return mCus_grpid
        End Get
        Set(ByVal value As String)
            mCus_grpid = value
        End Set
    End Property

    Public Property Cus_grpid2() As String
        Get
            Return mCus_grpid2
        End Get
        Set(ByVal value As String)
            mCus_grpid2 = value
        End Set
    End Property

    Public Property Cus_grpid3() As String
        Get
            Return mCus_grpid3
        End Get
        Set(ByVal value As String)
            mCus_grpid3 = value
        End Set
    End Property

    Public Property Cus_br() As String
        Get
            Return mCus_br
        End Get
        Set(ByVal value As String)
            mCus_br = value
        End Set
    End Property

    Public Property Cus_coname() As String
        Get
            Return mCus_coname
        End Get
        Set(ByVal value As String)
            mCus_coname = value
        End Set
    End Property

    Public Property Cus_add1() As String
        Get
            Return mCus_add1
        End Get
        Set(ByVal value As String)
            mCus_add1 = value
        End Set
    End Property

    Public Property Cus_add2() As String
        Get
            Return mCus_add2
        End Get
        Set(ByVal value As String)
            mCus_add2 = value
        End Set
    End Property

    Public Property Cus_add3() As String
        Get
            Return mCus_add3
        End Get
        Set(ByVal value As String)
            mCus_add3 = value
        End Set
    End Property

    Public Property Cus_add4() As String
        Get
            Return mCus_add4
        End Get
        Set(ByVal value As String)
            mCus_add4 = value
        End Set
    End Property

    Public Property Cus_pcode1() As String
        Get
            Return mCus_pcode1
        End Get
        Set(ByVal value As String)
            mCus_pcode1 = value
        End Set
    End Property

    Public Property Cus_pcode2() As String
        Get
            Return mCus_pcode2
        End Get
        Set(ByVal value As String)
            mCus_pcode2 = value
        End Set
    End Property

    Public Property Cus_cntry() As String
        Get
            Return mCus_cntry
        End Get
        Set(ByVal value As String)
            mCus_cntry = value
        End Set
    End Property

    Public Property Cus_cntct1() As String
        Get
            Return mCus_cntct1
        End Get
        Set(ByVal value As String)
            mCus_cntct1 = value
        End Set
    End Property

    Public Property Cus_cntct2() As String
        Get
            Return mCus_cntct2
        End Get
        Set(ByVal value As String)
            mCus_cntct2 = value
        End Set
    End Property

    Public Property Cus_cntct3() As String
        Get
            Return mCus_cntct3
        End Get
        Set(ByVal value As String)
            mCus_cntct3 = value
        End Set
    End Property

    Public Property Cus_cntct4() As String
        Get
            Return mCus_cntct4
        End Get
        Set(ByVal value As String)
            mCus_cntct4 = value
        End Set
    End Property

    Public Property Cus_cntct5() As String
        Get
            Return mCus_cntct5
        End Get
        Set(ByVal value As String)
            mCus_cntct5 = value
        End Set
    End Property

    Public Property Cus_tel() As String
        Get
            Return mCus_tel
        End Get
        Set(ByVal value As String)
            mCus_tel = value
        End Set
    End Property

    Public Property Cus_fax() As String
        Get
            Return mCus_fax
        End Get
        Set(ByVal value As String)
            mCus_fax = value
        End Set
    End Property

    Public Property Cus_email() As String
        Get
            Return mCus_email
        End Get
        Set(ByVal value As String)
            mCus_email = value
        End Set
    End Property

    Public Property Cus_email2() As String
        Get
            Return mCus_email2
        End Get
        Set(ByVal value As String)
            mCus_email2 = value
        End Set
    End Property

    Public Property Cus_cdate() As Nullable(Of DateTime)
        Get
            Return mCus_cdate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mCus_cdate = value
        End Set
    End Property

    Public Property Cus_fdate() As Nullable(Of DateTime)
        Get
            Return mCus_fdate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mCus_fdate = value
        End Set
    End Property

    Public Property Cus_ldate() As Nullable(Of DateTime)
        Get
            Return mCus_ldate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mCus_ldate = value
        End Set
    End Property

    Public Property Cus_onact() As Decimal
        Get
            Return mCus_onact
        End Get
        Set(ByVal value As Decimal)
            mCus_onact = value
        End Set
    End Property

    Public Property Cus_insure() As Boolean
        Get
            Return mCus_insure
        End Get
        Set(ByVal value As Boolean)
            mCus_insure = value
        End Set
    End Property

    Public Property Cus_insexp() As Nullable(Of DateTime)
        Get
            Return mCus_insexp
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mCus_insexp = value
        End Set
    End Property

    Public Property Cus_sref1() As String
        Get
            Return mCus_sref1
        End Get
        Set(ByVal value As String)
            mCus_sref1 = value
        End Set
    End Property

    Public Property Cus_sref2() As String
        Get
            Return mCus_sref2
        End Get
        Set(ByVal value As String)
            mCus_sref2 = value
        End Set
    End Property

    Public Property Cus_sref3() As String
        Get
            Return mCus_sref3
        End Get
        Set(ByVal value As String)
            mCus_sref3 = value
        End Set
    End Property

    Public Property Cus_sref4() As String
        Get
            Return mCus_sref4
        End Get
        Set(ByVal value As String)
            mCus_sref4 = value
        End Set
    End Property

    Public Property Cus_sref5() As String
        Get
            Return mCus_sref5
        End Get
        Set(ByVal value As String)
            mCus_sref5 = value
        End Set
    End Property

    Public Property Cus_sref6() As String
        Get
            Return mCus_sref6
        End Get
        Set(ByVal value As String)
            mCus_sref6 = value
        End Set
    End Property

    Public Property Cus_con1() As String
        Get
            Return mCus_con1
        End Get
        Set(ByVal value As String)
            mCus_con1 = value
        End Set
    End Property

    Public Property Cus_c1mth1() As Integer
        Get
            Return mCus_c1mth1
        End Get
        Set(ByVal value As Integer)
            mCus_c1mth1 = value
        End Set
    End Property

    Public Property Cus_c1mth2() As Integer
        Get
            Return mCus_c1mth2
        End Get
        Set(ByVal value As Integer)
            mCus_c1mth2 = value
        End Set
    End Property

    Public Property Cus_c1mth3() As Integer
        Get
            Return mCus_c1mth3
        End Get
        Set(ByVal value As Integer)
            mCus_c1mth3 = value
        End Set
    End Property

    Public Property Cus_c1pct1() As Decimal
        Get
            Return mCus_c1pct1
        End Get
        Set(ByVal value As Decimal)
            mCus_c1pct1 = value
        End Set
    End Property

    Public Property Cus_c1pct2() As Decimal
        Get
            Return mCus_c1pct2
        End Get
        Set(ByVal value As Decimal)
            mCus_c1pct2 = value
        End Set
    End Property

    Public Property Cus_c1pct3() As Decimal
        Get
            Return mCus_c1pct3
        End Get
        Set(ByVal value As Decimal)
            mCus_c1pct3 = value
        End Set
    End Property

    Public Property Cus_c1pct4() As Decimal
        Get
            Return mCus_c1pct4
        End Get
        Set(ByVal value As Decimal)
            mCus_c1pct4 = value
        End Set
    End Property

    Public Property Cus_con2() As String
        Get
            Return mCus_con2
        End Get
        Set(ByVal value As String)
            mCus_con2 = value
        End Set
    End Property

    Public Property Cus_c2mth1() As Integer
        Get
            Return mCus_c2mth1
        End Get
        Set(ByVal value As Integer)
            mCus_c2mth1 = value
        End Set
    End Property

    Public Property Cus_c2mth2() As Integer
        Get
            Return mCus_c2mth2
        End Get
        Set(ByVal value As Integer)
            mCus_c2mth2 = value
        End Set
    End Property

    Public Property Cus_c2mth3() As Integer
        Get
            Return mCus_c2mth3
        End Get
        Set(ByVal value As Integer)
            mCus_c2mth3 = value
        End Set
    End Property

    Public Property Cus_c2pct1() As Double
        Get
            Return mCus_c2pct1
        End Get
        Set(ByVal value As Double)
            mCus_c2pct1 = value
        End Set
    End Property

    Public Property Cus_c2pct2() As Decimal
        Get
            Return mCus_c2pct2
        End Get
        Set(ByVal value As Decimal)
            mCus_c2pct2 = value
        End Set
    End Property

    Public Property Cus_c2pct3() As Decimal
        Get
            Return mCus_c2pct3
        End Get
        Set(ByVal value As Decimal)
            mCus_c2pct3 = value
        End Set
    End Property

    Public Property Cus_c2pct4() As Decimal
        Get
            Return mCus_c2pct4
        End Get
        Set(ByVal value As Decimal)
            mCus_c2pct4 = value
        End Set
    End Property

    Public Property Cus_ptermc() As String
        Get
            Return mCus_ptermc
        End Get
        Set(ByVal value As String)
            mCus_ptermc = value
        End Set
    End Property

    Public Property Cus_ptermv() As Integer
        Get
            Return mCus_ptermv
        End Get
        Set(ByVal value As Integer)
            mCus_ptermv = value
        End Set
    End Property

    Public Property Cus_crep() As Boolean
        Get
            Return mCus_crep
        End Get
        Set(ByVal value As Boolean)
            mCus_crep = value
        End Set
    End Property

    Public Property Cus_state() As Boolean
        Get
            Return mCus_state
        End Get
        Set(ByVal value As Boolean)
            mCus_state = value
        End Set
    End Property

    Public Property Cus_pono() As Boolean
        Get
            Return mCus_pono
        End Get
        Set(ByVal value As Boolean)
            mCus_pono = value
        End Set
    End Property

    Public Property Cus_costc() As Boolean
        Get
            Return mCus_costc
        End Get
        Set(ByVal value As Boolean)
            mCus_costc = value
        End Set
    End Property

    Public Property Cus_cref1() As Boolean
        Get
            Return mCus_cref1
        End Get
        Set(ByVal value As Boolean)
            mCus_cref1 = value
        End Set
    End Property

    Public Property Cus_cref2() As Boolean
        Get
            Return mCus_cref2
        End Get
        Set(ByVal value As Boolean)
            mCus_cref2 = value
        End Set
    End Property

    Public Property Cus_ccard() As Boolean
        Get
            Return mCus_ccard
        End Get
        Set(ByVal value As Boolean)
            mCus_ccard = value
        End Set
    End Property

    Public Property Cus_cconly() As Boolean
        Get
            Return mCus_cconly
        End Get
        Set(ByVal value As Boolean)
            mCus_cconly = value
        End Set
    End Property

    Public Property Cus_climit() As Decimal
        Get
            Return mCus_climit
        End Get
        Set(ByVal value As Decimal)
            mCus_climit = value
        End Set
    End Property

    Public Property Cus_debt() As Decimal
        Get
            Return mCus_debt
        End Get
        Set(ByVal value As Decimal)
            mCus_debt = value
        End Set
    End Property

    Public Property Cus_stop() As Boolean
        Get
            Return mCus_stop
        End Get
        Set(ByVal value As Boolean)
            mCus_stop = value
        End Set
    End Property

    Public Property Cus_2ndadd() As Boolean
        Get
            Return mCus_2ndadd
        End Get
        Set(ByVal value As Boolean)
            mCus_2ndadd = value
        End Set
    End Property

    Public Property Cus_notes() As String
        Get
            Return mCus_notes
        End Get
        Set(ByVal value As String)
            mCus_notes = value
        End Set
    End Property

    Public Property Cus_stadd() As String
        Get
            Return mCus_stadd
        End Get
        Set(ByVal value As String)
            mCus_stadd = value
        End Set
    End Property

    Public Property Cus_soy() As Integer
        Get
            Return mCus_soy
        End Get
        Set(ByVal value As Integer)
            mCus_soy = value
        End Set
    End Property

    Public Property Cus_conam2() As String
        Get
            Return mCus_conam2
        End Get
        Set(ByVal value As String)
            mCus_conam2 = value
        End Set
    End Property

    Public Property Cus_pop1() As String
        Get
            Return mCus_pop1
        End Get
        Set(ByVal value As String)
            mCus_pop1 = value
        End Set
    End Property

    Public Property Cus_pop2() As String
        Get
            Return mCus_pop2
        End Get
        Set(ByVal value As String)
            mCus_pop2 = value
        End Set
    End Property

    Public Property Cus_ovrdue() As Decimal
        Get
            Return mCus_ovrdue
        End Get
        Set(ByVal value As Decimal)
            mCus_ovrdue = value
        End Set
    End Property

    Public Property Cus_dscpct() As Decimal
        Get
            Return mCus_dscpct
        End Get
        Set(ByVal value As Decimal)
            mCus_dscpct = value
        End Set
    End Property

    Public Property Cus_fulreb() As Boolean
        Get
            Return mCus_fulreb
        End Get
        Set(ByVal value As Boolean)
            mCus_fulreb = value
        End Set
    End Property

    Public Property Cus_hidecm() As Boolean
        Get
            Return mCus_hidecm
        End Get
        Set(ByVal value As Boolean)
            mCus_hidecm = value
        End Set
    End Property

    Public Property Cus_rebcn() As Boolean
        Get
            Return mCus_rebcn
        End Get
        Set(ByVal value As Boolean)
            mCus_rebcn = value
        End Set
    End Property

    Public Property Cus_rebfrq() As String
        Get
            Return mCus_rebfrq
        End Get
        Set(ByVal value As String)
            mCus_rebfrq = value
        End Set
    End Property

    Public Property Cus_reb1cn() As Boolean
        Get
            Return mCus_reb1cn
        End Get
        Set(ByVal value As Boolean)
            mCus_reb1cn = value
        End Set
    End Property

    Public Property Cus_rebinv() As Boolean
        Get
            Return mCus_rebinv
        End Get
        Set(ByVal value As Boolean)
            mCus_rebinv = value
        End Set
    End Property

    Public Property Cus_incfee() As Boolean
        Get
            Return mCus_incfee
        End Get
        Set(ByVal value As Boolean)
            mCus_incfee = value
        End Set
    End Property

    Public Property Cus_nocc() As Boolean
        Get
            Return mCus_nocc
        End Get
        Set(ByVal value As Boolean)
            mCus_nocc = value
        End Set
    End Property

    Public Property Cus_feeid1() As String
        Get
            Return mCus_feeid1
        End Get
        Set(ByVal value As String)
            mCus_feeid1 = value
        End Set
    End Property

    Public Property Cus_feeid2() As String
        Get
            Return mCus_feeid2
        End Get
        Set(ByVal value As String)
            mCus_feeid2 = value
        End Set
    End Property

    Public Property Cus_feeid3() As String
        Get
            Return mCus_feeid3
        End Get
        Set(ByVal value As String)
            mCus_feeid3 = value
        End Set
    End Property

    Public Property Cus_adhoc() As Boolean
        Get
            Return mCus_adhoc
        End Get
        Set(ByVal value As Boolean)
            mCus_adhoc = value
        End Set
    End Property

    Public Property Cus_dofee() As Boolean
        Get
            Return mCus_dofee
        End Get
        Set(ByVal value As Boolean)
            mCus_dofee = value
        End Set
    End Property

    Public Property Cus_ibank() As Boolean
        Get
            Return mCus_ibank
        End Get
        Set(ByVal value As Boolean)
            mCus_ibank = value
        End Set
    End Property

    Public Property Cus_feetyp() As String
        Get
            Return mCus_feetyp
        End Get
        Set(ByVal value As String)
            mCus_feetyp = value
        End Set
    End Property

    Public Property Cus_atol() As String
        Get
            Return mCus_atol
        End Get
        Set(ByVal value As String)
            mCus_atol = value
        End Set
    End Property

    Public Property Cus_paydd() As Boolean
        Get
            Return mCus_paydd
        End Get
        Set(ByVal value As Boolean)
            mCus_paydd = value
        End Set
    End Property

    Public Property Cus_sps() As Boolean
        Get
            Return mCus_sps
        End Get
        Set(ByVal value As Boolean)
            mCus_sps = value
        End Set
    End Property

    Public Property Cus_acas() As Boolean
        Get
            Return mCus_acas
        End Get
        Set(ByVal value As Boolean)
            mCus_acas = value
        End Set
    End Property

    Public Property Cus_abtano() As String
        Get
            Return mCus_abtano
        End Get
        Set(ByVal value As String)
            mCus_abtano = value
        End Set
    End Property

    Public Property Cus_mailst() As Boolean
        Get
            Return mCus_mailst
        End Get
        Set(ByVal value As Boolean)
            mCus_mailst = value
        End Set
    End Property

    Public Property Cus_mailiv() As Boolean
        Get
            Return mCus_mailiv
        End Get
        Set(ByVal value As Boolean)
            mCus_mailiv = value
        End Set
    End Property

    Public Property Cus_coninv() As Boolean
        Get
            Return mCus_coninv
        End Get
        Set(ByVal value As Boolean)
            mCus_coninv = value
        End Set
    End Property

    Public Property Cus_cinvdt() As Nullable(Of DateTime)
        Get
            Return mCus_cinvdt
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mCus_cinvdt = value
        End Set
    End Property

    Public Property Cus_keyact() As Boolean
        Get
            Return mCus_keyact
        End Get
        Set(ByVal value As Boolean)
            mCus_keyact = value
        End Set
    End Property

    Public Property Cus_invfrm() As String
        Get
            Return mCus_invfrm
        End Get
        Set(ByVal value As String)
            mCus_invfrm = value
        End Set
    End Property

    Public Property Cus_noba() As Boolean
        Get
            Return mCus_noba
        End Get
        Set(ByVal value As Boolean)
            mCus_noba = value
        End Set
    End Property

    Public Property Cus_slfbil() As Boolean
        Get
            Return mCus_slfbil
        End Get
        Set(ByVal value As Boolean)
            mCus_slfbil = value
        End Set
    End Property

    Public Property Cus_exvat() As Boolean
        Get
            Return mCus_exvat
        End Get
        Set(ByVal value As Boolean)
            mCus_exvat = value
        End Set
    End Property

    Public Property Cus_curncy() As String
        Get
            Return mCus_curncy
        End Get
        Set(ByVal value As String)
            mCus_curncy = value
        End Set
    End Property

    Public Property Cus_onepdf() As Boolean
        Get
            Return mCus_onepdf
        End Get
        Set(ByVal value As Boolean)
            mCus_onepdf = value
        End Set
    End Property

    Public Property Cus_spare2() As String
        Get
            Return mCus_spare2
        End Get
        Set(ByVal value As String)
            mCus_spare2 = value
        End Set
    End Property

    Public Property Cus_dorm() As Boolean
        Get
            Return mCus_dorm
        End Get
        Set(ByVal value As Boolean)
            mCus_dorm = value
        End Set
    End Property

    Public Property Cus_mltfee() As Boolean
        Get
            Return mCus_mltfee
        End Get
        Set(ByVal value As Boolean)
            mCus_mltfee = value
        End Set
    End Property

    Public Property Cus_print() As Boolean
        Get
            Return mCus_print
        End Get
        Set(ByVal value As Boolean)
            mCus_print = value
        End Set
    End Property

    Private Shared Function makeBOSScustomerFromRow( _
            ByVal r As IDataReader _
        ) As BOSScustomer
        Return New BOSScustomer( _
                 clsUseful.notInteger(r.Item("customerID")), _
                 clsUseful.notString(r.Item("cus_id")), _
                 clsUseful.notString(r.Item("cus_type")), _
                 clsUseful.notString(r.Item("cus_grpid")), _
                 clsUseful.notString(r.Item("cus_grpid2")), _
                 clsUseful.notString(r.Item("cus_grpid3")), _
                 clsUseful.notString(r.Item("cus_br")), _
                 clsUseful.notString(r.Item("cus_coname")), _
                 clsUseful.notString(r.Item("cus_add1")), _
                 clsUseful.notString(r.Item("cus_add2")), _
                 clsUseful.notString(r.Item("cus_add3")), _
                 clsUseful.notString(r.Item("cus_add4")), _
                 clsUseful.notString(r.Item("cus_pcode1")), _
                 clsUseful.notString(r.Item("cus_pcode2")), _
                 clsUseful.notString(r.Item("cus_cntry")), _
                 clsUseful.notString(r.Item("cus_cntct1")), _
                 clsUseful.notString(r.Item("cus_cntct2")), _
                 clsUseful.notString(r.Item("cus_cntct3")), _
                 clsUseful.notString(r.Item("cus_cntct4")), _
                 clsUseful.notString(r.Item("cus_cntct5")), _
                 clsUseful.notString(r.Item("cus_tel")), _
                 clsUseful.notString(r.Item("cus_fax")), _
                 clsUseful.notString(r.Item("cus_email")), _
                 clsUseful.notString(r.Item("cus_email2")), _
                toNullableDate(r.Item("cus_cdate")), _
                toNullableDate(r.Item("cus_fdate")), _
                toNullableDate(r.Item("cus_ldate")), _
                toNullableFloat(r.Item("cus_onact")), _
                toNullableBoolean(r.Item("cus_insure")), _
                toNullableDate(r.Item("cus_insexp")), _
                 clsUseful.notString(r.Item("cus_sref1")), _
                 clsUseful.notString(r.Item("cus_sref2")), _
                 clsUseful.notString(r.Item("cus_sref3")), _
                 clsUseful.notString(r.Item("cus_sref4")), _
                 clsUseful.notString(r.Item("cus_sref5")), _
                 clsUseful.notString(r.Item("cus_sref6")), _
                 clsUseful.notString(r.Item("cus_con1")), _
                toNullableInteger(r.Item("cus_c1mth1")), _
                toNullableInteger(r.Item("cus_c1mth2")), _
                toNullableInteger(r.Item("cus_c1mth3")), _
                toNullableFloat(r.Item("cus_c1pct1")), _
                toNullableFloat(r.Item("cus_c1pct2")), _
                toNullableFloat(r.Item("cus_c1pct3")), _
                toNullableFloat(r.Item("cus_c1pct4")), _
                 clsUseful.notString(r.Item("cus_con2")), _
                toNullableInteger(r.Item("cus_c2mth1")), _
                toNullableInteger(r.Item("cus_c2mth2")), _
                toNullableInteger(r.Item("cus_c2mth3")), _
                toNullableFloat(r.Item("cus_c2pct1")), _
                toNullableFloat(r.Item("cus_c2pct2")), _
                toNullableFloat(r.Item("cus_c2pct3")), _
                toNullableFloat(r.Item("cus_c2pct4")), _
                 clsUseful.notString(r.Item("cus_ptermc")), _
                toNullableInteger(r.Item("cus_ptermv")), _
                toNullableBoolean(r.Item("cus_crep")), _
                toNullableBoolean(r.Item("cus_state")), _
                toNullableBoolean(r.Item("cus_pono")), _
                toNullableBoolean(r.Item("cus_costc")), _
                toNullableBoolean(r.Item("cus_cref1")), _
                toNullableBoolean(r.Item("cus_cref2")), _
                toNullableBoolean(r.Item("cus_ccard")), _
                toNullableBoolean(r.Item("cus_cconly")), _
                toNullableFloat(r.Item("cus_climit")), _
                toNullableFloat(r.Item("cus_debt")), _
                toNullableBoolean(r.Item("cus_stop")), _
                toNullableBoolean(r.Item("cus_2ndadd")), _
                 clsUseful.notString(r.Item("cus_notes")), _
                 clsUseful.notString(r.Item("cus_stadd")), _
                toNullableInteger(r.Item("cus_soy")), _
                 clsUseful.notString(r.Item("cus_conam2")), _
                 clsUseful.notString(r.Item("cus_pop1")), _
                 clsUseful.notString(r.Item("cus_pop2")), _
                toNullableFloat(r.Item("cus_ovrdue")), _
                toNullableFloat(r.Item("cus_dscpct")), _
                toNullableBoolean(r.Item("cus_fulreb")), _
                toNullableBoolean(r.Item("cus_hidecm")), _
                toNullableBoolean(r.Item("cus_rebcn")), _
                 clsUseful.notString(r.Item("cus_rebfrq")), _
                toNullableBoolean(r.Item("cus_reb1cn")), _
                toNullableBoolean(r.Item("cus_rebinv")), _
                toNullableBoolean(r.Item("cus_incfee")), _
                toNullableBoolean(r.Item("cus_nocc")), _
                 clsUseful.notString(r.Item("cus_feeid1")), _
                 clsUseful.notString(r.Item("cus_feeid2")), _
                 clsUseful.notString(r.Item("cus_feeid3")), _
                toNullableBoolean(r.Item("cus_adhoc")), _
                toNullableBoolean(r.Item("cus_dofee")), _
                toNullableBoolean(r.Item("cus_ibank")), _
                 clsUseful.notString(r.Item("cus_feetyp")), _
                 clsUseful.notString(r.Item("cus_atol")), _
                toNullableBoolean(r.Item("cus_paydd")), _
                toNullableBoolean(r.Item("cus_sps")), _
                toNullableBoolean(r.Item("cus_acas")), _
                 clsUseful.notString(r.Item("cus_abtano")), _
                toNullableBoolean(r.Item("cus_mailst")), _
                toNullableBoolean(r.Item("cus_mailiv")), _
                toNullableBoolean(r.Item("cus_coninv")), _
                toNullableDate(r.Item("cus_cinvdt")), _
                toNullableBoolean(r.Item("cus_keyact")), _
                 clsUseful.notString(r.Item("cus_invfrm")), _
                toNullableBoolean(r.Item("cus_noba")), _
                toNullableBoolean(r.Item("cus_slfbil")), _
                toNullableBoolean(r.Item("cus_exvat")), _
                 clsUseful.notString(r.Item("cus_curncy")), _
                toNullableBoolean(r.Item("cus_onepdf")), _
                 clsUseful.notString(r.Item("cus_spare2")), _
                toNullableBoolean(r.Item("cus_dorm")), _
                toNullableBoolean(r.Item("cus_mltfee")), _
                toNullableBoolean(r.Item("cus_print")), _
                clsUseful.notString(r.Item("cus_pcode")), _
                clsUseful.notString(r.Item("cus_stfrm")))
    End Function

    Public Shared Function [get]( _
            ByVal pCustomerID As Integer _
        ) As BOSScustomer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("BOSScustomer_get", "@customerID", pCustomerID)
                If Not r.Read() Then
                    Throw New Exception("No BOSScustomer with id " & pCustomerID)
                End If
                Dim ret As BOSScustomer = makeBOSScustomerFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of BOSScustomer)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BOSScustomer)()
            Using r As IDataReader = dbh.callSP("BOSScustomer_list")
                While r.Read()
                    ret.Add(makeBOSScustomerFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim strRet As String = CStr(dbh.callSPSingleValue("BOSScustomer_save", "@CustomerID", mCustomerID, "@Cus_id", mCus_id, "@Cus_type", mCus_type, "@Cus_grpid", mCus_grpid, "@Cus_grpid2", mCus_grpid2, _
                                                     "@Cus_grpid3", mCus_grpid3, "@Cus_br", mCus_br, "@Cus_coname", mCus_coname, "@Cus_add1", mCus_add1, "@Cus_add2", mCus_add2, "@Cus_add3", mCus_add3, _
                                                     "@Cus_add4", mCus_add4, "@Cus_pcode1", mCus_pcode1, "@Cus_pcode2", mCus_pcode2, "@Cus_cntry", mCus_cntry, "@Cus_cntct1", mCus_cntct1, "@Cus_cntct2", _
                                                     mCus_cntct2, "@Cus_cntct3", mCus_cntct3, "@Cus_cntct4", mCus_cntct4, "@Cus_cntct5", mCus_cntct5, "@Cus_tel", mCus_tel, "@Cus_fax", mCus_fax, "@Cus_email", _
                                                     mCus_email, "@Cus_email2", mCus_email2, "@Cus_cdate", mCus_cdate, "@Cus_fdate", mCus_fdate, "@Cus_ldate", mCus_ldate, "@Cus_onact", mCus_onact, "@Cus_insure", _
                                                     mCus_insure, "@Cus_insexp", mCus_insexp, "@Cus_sref1", mCus_sref1, "@Cus_sref2", mCus_sref2, "@Cus_sref3", mCus_sref3, "@Cus_sref4", mCus_sref4, "@Cus_sref5", _
                                                     mCus_sref5, "@Cus_sref6", mCus_sref6, "@Cus_con1", mCus_con1, "@Cus_c1mth1", mCus_c1mth1, "@Cus_c1mth2", mCus_c1mth2, "@Cus_c1mth3", mCus_c1mth3, "@Cus_c1pct1", _
                                                     mCus_c1pct1, "@Cus_c1pct2", mCus_c1pct2, "@Cus_c1pct3", mCus_c1pct3, "@Cus_c1pct4", mCus_c1pct4, "@Cus_con2", mCus_con2, "@Cus_c2mth1", mCus_c2mth1, "@Cus_c2mth2", _
                                                     mCus_c2mth2, "@Cus_c2mth3", mCus_c2mth3, "@Cus_c2pct1", mCus_c2pct1, "@Cus_c2pct2", mCus_c2pct2, "@Cus_c2pct3", mCus_c2pct3, "@Cus_c2pct4", mCus_c2pct4, _
                                                     "@Cus_ptermc", mCus_ptermc, "@Cus_ptermv", mCus_ptermv, "@Cus_crep", mCus_crep, "@Cus_state", mCus_state, "@Cus_pono", mCus_pono, "@Cus_costc", mCus_costc, _
                                                     "@Cus_cref1", mCus_cref1, "@Cus_cref2", mCus_cref2, "@Cus_ccard", mCus_ccard, "@Cus_cconly", mCus_cconly, "@Cus_climit", mCus_climit, "@Cus_debt", mCus_debt, _
                                                     "@Cus_stop", mCus_stop, "@Cus_2ndadd", mCus_2ndadd, "@Cus_notes", mCus_notes, "@Cus_stadd", mCus_stadd, "@Cus_soy", mCus_soy, "@Cus_conam2", mCus_conam2, _
                                                     "@Cus_pop1", mCus_pop1, "@Cus_pop2", mCus_pop2, "@Cus_ovrdue", mCus_ovrdue, "@Cus_dscpct", mCus_dscpct, "@Cus_fulreb", mCus_fulreb, "@Cus_hidecm", mCus_hidecm, _
                                                     "@Cus_rebcn", mCus_rebcn, "@Cus_rebfrq", mCus_rebfrq, "@Cus_reb1cn", mCus_reb1cn, "@Cus_rebinv", mCus_rebinv, "@Cus_incfee", mCus_incfee, "@Cus_nocc", mCus_nocc, _
                                                     "@Cus_feeid1", mCus_feeid1, "@Cus_feeid2", mCus_feeid2, "@Cus_feeid3", mCus_feeid3, "@Cus_adhoc", mCus_adhoc, "@Cus_dofee", mCus_dofee, "@Cus_ibank", mCus_ibank, _
                                                     "@Cus_feetyp", mCus_feetyp, "@Cus_atol", mCus_atol, "@Cus_paydd", mCus_paydd, "@Cus_sps", mCus_sps, "@Cus_acas", mCus_acas, "@Cus_abtano", mCus_abtano, _
                                                     "@Cus_mailst", mCus_mailst, "@Cus_mailiv", mCus_mailiv, "@Cus_coninv", mCus_coninv, "@Cus_cinvdt", mCus_cinvdt, "@Cus_keyact", mCus_keyact, "@Cus_invfrm", _
                                                     mCus_invfrm, "@Cus_noba", mCus_noba, "@Cus_slfbil", mCus_slfbil, "@Cus_exvat", mCus_exvat, "@Cus_curncy", mCus_curncy, "@Cus_onepdf", mCus_onepdf, "@Cus_spare2", _
                                                     mCus_spare2, "@Cus_dorm", mCus_dorm, "@Cus_mltfee", mCus_mltfee, "@Cus_print", mCus_print, "@cus_pcode", mCus_pcode, "@cus_stfrm", mcus_stfrm))
            Return strRet
        End Using
    End Function

    Public Shared Sub delete(ByVal pCustomerID As Integer, ByVal pCus_id As String)
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("BOSScustomer_delete", "@CustomerID", pCustomerID, "@Cus_id", pCus_id)
        End Using
    End Sub
End Class
