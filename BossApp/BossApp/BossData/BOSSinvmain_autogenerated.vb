Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSSinvmain

    Public Sub New( _
        ByVal pInvmainID As Integer, _
        ByVal pInm_no As String, _
        ByVal pInm_line As Nullable(Of Double), _
        ByVal pInm_retail As Nullable(Of Boolean), _
        ByVal pInm_br As String, _
        ByVal pInm_type As String, _
        ByVal pInm_invdt As Nullable(Of DateTime), _
        ByVal pInm_entry As String, _
        ByVal pInm_custid As String, _
        ByVal pInm_costc As String, _
        ByVal pInm_pono As String, _
        ByVal pInm_cdate As Nullable(Of DateTime), _
        ByVal pInm_prod As String, _
        ByVal pInm_suppid As String, _
        ByVal pInm_supnam As String, _
        ByVal pInm_htltel As String, _
        ByVal pInm_bsp As Nullable(Of Boolean), _
        ByVal pInm_docmnt As String, _
        ByVal pInm_etkt As String, _
        ByVal pInm_books As Nullable(Of Double), _
        ByVal pInm_exch As String, _
        ByVal pInm_origtk As String, _
        ByVal pInm_online As Nullable(Of Boolean), _
        ByVal pInm_bspdte As Nullable(Of DateTime), _
        ByVal pInm_start As Nullable(Of DateTime), _
        ByVal pInm_end As Nullable(Of DateTime), _
        ByVal pInm_ldname As String, _
        ByVal pInm_savecd As String, _
        ByVal pInm_mxfare As Decimal, _
        ByVal pInm_lwfare As Decimal, _
        ByVal pInm_bookng As Nullable(Of Boolean), _
        ByVal pInm_fcrncy As String, _
        ByVal pInm_ffare As Decimal, _
        ByVal pInm_drate As Decimal, _
        ByVal pInm_days As Decimal, _
        ByVal pInm_curncy As String, _
        ByVal pInm_fare As Decimal, _
        ByVal pInm_srvchg As Decimal, _
        ByVal pInm_ubtax As Decimal, _
        ByVal pInm_yqtax As Decimal, _
        ByVal pInm_othtax As Decimal, _
        ByVal pInm_tax As Decimal, _
        ByVal pInm_ourchg As Decimal, _
        ByVal pInm_amtvat As Decimal, _
        ByVal pInm_supvat As Decimal, _
        ByVal pInm_ourvat As Decimal, _
        ByVal pInm_amount As Decimal, _
        ByVal pInm_dscpct As Decimal, _
        ByVal pInm_discnt As Decimal, _
        ByVal pInm_billed As Decimal, _
        ByVal pInm_bilvat As Decimal, _
        ByVal pInm_compct As Decimal, _
        ByVal pInm_comvat As Decimal, _
        ByVal pInm_vtoncm As Decimal, _
        ByVal pInm_trucom As Decimal, _
        ByVal pInm_othcom As Decimal, _
        ByVal pInm_comamt As Decimal, _
        ByVal pInm_comrcv As Decimal, _
        ByVal pInm_comdue As Decimal, _
        ByVal pInm_paynet As Nullable(Of Boolean), _
        ByVal pInm_vatinv As Nullable(Of Boolean), _
        ByVal pInm_cominv As Nullable(Of Boolean), _
        ByVal pInm_morpax As String, _
        ByVal pInm_dposit As Decimal, _
        ByVal pInm_depok As Nullable(Of Boolean), _
        ByVal pInm_depbr As String, _
        ByVal pInm_depbnk As String, _
        ByVal pInm_baldue As Nullable(Of DateTime), _
        ByVal pInm_paytyp As String, _
        ByVal pInm_ccid As String, _
        ByVal pInm_ccno As String, _
        ByVal pInm_ccstdt As String, _
        ByVal pInm_ccexp As String, _
        ByVal pInm_ccauth As String, _
        ByVal pInm_issue As Nullable(Of Double), _
        ByVal pInm_merch As Nullable(Of Boolean), _
        ByVal pInm_mfee As Decimal, _
        ByVal pInm_ccmeth As String, _
        ByVal pInm_ccamt As Decimal, _
        ByVal pInm_ccxmit As Nullable(Of DateTime), _
        ByVal pInm_print As Nullable(Of Double), _
        ByVal pInm_con1pd As Nullable(Of DateTime), _
        ByVal pInm_con2pd As Nullable(Of DateTime), _
        ByVal pInm_orig As String, _
        ByVal pInm_locked As Nullable(Of Boolean), _
        ByVal pInm_erflag As Nullable(Of Boolean), _
        ByVal pInm_erdesc As String, _
        ByVal pInm_ukey As String, _
        ByVal pInm_change As Nullable(Of Boolean), _
        ByVal pInm_who As String, _
        ByVal pInm_note As String, _
        ByVal pInm_crsref As String, _
        ByVal pInm_bdm As String, _
        ByVal pInm_pcity As String, _
        ByVal pInm_ino As String, _
        ByVal pInm_cos As String, _
        ByVal pInm_domint As String, _
        ByVal pInm_apok As Nullable(Of Boolean), _
        ByVal pInm_arok As Nullable(Of Boolean), _
        ByVal pInm_voided As String, _
        ByVal pInm_agcomm As Decimal, _
        ByVal pInm_agvat As Decimal, _
        ByVal pInm_agdcpc As Decimal, _
        ByVal pInm_atol As Nullable(Of Boolean), _
        ByVal pInm_abond As String, _
        ByVal pInm_afare As Decimal, _
        ByVal pInm_aheads As Decimal, _
        ByVal pInm_nfare As Nullable(Of Boolean), _
        ByVal pInm_rebate As Decimal, _
        ByVal pInm_fee As Decimal, _
        ByVal pInm_feevt As Decimal, _
        ByVal pInm_feebas As String, _
        ByVal pInm_dla As Nullable(Of DateTime), _
        ByVal pInm_bywho As String, _
        ByVal pInm_cinvrf As String, _
        ByVal pTmpfld As String, _
        ByVal pInm_3rdpty As Nullable(Of Boolean), _
        ByVal pInm_ourcc As String, _
        ByVal pInm_itcode As String, _
        ByVal pInm_bkcur As String, _
        ByVal pInm_bkroe As Decimal, _
        ByVal pChksum As String, _
        ByVal pDatecreated As Nullable(Of DateTime), _
        ByVal pInm_miles1 As String, _
        ByVal pInm_miles2 As String, _
        ByVal pInm_km1 As String, _
        ByVal pInm_km2 As String, _
        ByVal pInm_disvat As Decimal, _
        ByVal pinm_cccvv As String, _
        ByVal pinm_gcid As String, _
        ByVal pinm_gcno As String)
        mInvmainID = pInvmainID
        mInm_no = pInm_no
        mInm_line = pInm_line
        mInm_retail = pInm_retail
        mInm_br = pInm_br
        mInm_type = pInm_type
        mInm_invdt = pInm_invdt
        mInm_entry = pInm_entry
        mInm_custid = pInm_custid
        mInm_costc = pInm_costc
        mInm_pono = pInm_pono
        mInm_cdate = pInm_cdate
        mInm_prod = pInm_prod
        mInm_suppid = pInm_suppid
        mInm_supnam = pInm_supnam
        mInm_htltel = pInm_htltel
        mInm_bsp = pInm_bsp
        mInm_docmnt = pInm_docmnt
        mInm_etkt = pInm_etkt
        mInm_books = pInm_books
        mInm_exch = pInm_exch
        mInm_origtk = pInm_origtk
        mInm_online = pInm_online
        mInm_bspdte = pInm_bspdte
        mInm_start = pInm_start
        mInm_end = pInm_end
        mInm_ldname = pInm_ldname
        mInm_savecd = pInm_savecd
        mInm_mxfare = pInm_mxfare
        mInm_lwfare = pInm_lwfare
        mInm_bookng = pInm_bookng
        mInm_fcrncy = pInm_fcrncy
        mInm_ffare = pInm_ffare
        mInm_drate = pInm_drate
        mInm_days = pInm_days
        mInm_curncy = pInm_curncy
        mInm_fare = pInm_fare
        mInm_srvchg = pInm_srvchg
        mInm_ubtax = pInm_ubtax
        mInm_yqtax = pInm_yqtax
        mInm_othtax = pInm_othtax
        mInm_tax = pInm_tax
        mInm_ourchg = pInm_ourchg
        mInm_amtvat = pInm_amtvat
        mInm_supvat = pInm_supvat
        mInm_ourvat = pInm_ourvat
        mInm_amount = pInm_amount
        mInm_dscpct = pInm_dscpct
        mInm_discnt = pInm_discnt
        mInm_billed = pInm_billed
        mInm_bilvat = pInm_bilvat
        mInm_compct = pInm_compct
        mInm_comvat = pInm_comvat
        mInm_vtoncm = pInm_vtoncm
        mInm_trucom = pInm_trucom
        mInm_othcom = pInm_othcom
        mInm_comamt = pInm_comamt
        mInm_comrcv = pInm_comrcv
        mInm_comdue = pInm_comdue
        mInm_paynet = pInm_paynet
        mInm_vatinv = pInm_vatinv
        mInm_cominv = pInm_cominv
        mInm_morpax = pInm_morpax
        mInm_dposit = pInm_dposit
        mInm_depok = pInm_depok
        mInm_depbr = pInm_depbr
        mInm_depbnk = pInm_depbnk
        mInm_baldue = pInm_baldue
        mInm_paytyp = pInm_paytyp
        mInm_ccid = pInm_ccid
        mInm_ccno = pInm_ccno
        mInm_ccstdt = pInm_ccstdt
        mInm_ccexp = pInm_ccexp
        mInm_ccauth = pInm_ccauth
        mInm_issue = pInm_issue
        mInm_merch = pInm_merch
        mInm_mfee = pInm_mfee
        mInm_ccmeth = pInm_ccmeth
        mInm_ccamt = pInm_ccamt
        mInm_ccxmit = pInm_ccxmit
        mInm_print = pInm_print
        mInm_con1pd = pInm_con1pd
        mInm_con2pd = pInm_con2pd
        mInm_orig = pInm_orig
        mInm_locked = pInm_locked
        mInm_erflag = pInm_erflag
        mInm_erdesc = pInm_erdesc
        mInm_ukey = pInm_ukey
        mInm_change = pInm_change
        mInm_who = pInm_who
        mInm_note = pInm_note
        mInm_crsref = pInm_crsref
        mInm_bdm = pInm_bdm
        mInm_pcity = pInm_pcity
        mInm_ino = pInm_ino
        mInm_cos = pInm_cos
        mInm_domint = pInm_domint
        mInm_apok = pInm_apok
        mInm_arok = pInm_arok
        mInm_voided = pInm_voided
        mInm_agcomm = pInm_agcomm
        mInm_agvat = pInm_agvat
        mInm_agdcpc = pInm_agdcpc
        mInm_atol = pInm_atol
        mInm_abond = pInm_abond
        mInm_afare = pInm_afare
        mInm_aheads = pInm_aheads
        mInm_nfare = pInm_nfare
        mInm_rebate = pInm_rebate
        mInm_fee = pInm_fee
        mInm_feevt = pInm_feevt
        mInm_feebas = pInm_feebas
        mInm_dla = pInm_dla
        mInm_bywho = pInm_bywho
        mInm_cinvrf = pInm_cinvrf
        mTmpfld = pTmpfld
        mInm_3rdpty = pInm_3rdpty
        mInm_ourcc = pInm_ourcc
        mInm_itcode = pInm_itcode
        mInm_bkcur = pInm_bkcur
        mInm_bkroe = pInm_bkroe
        mChksum = pChksum
        mDatecreated = pDatecreated
        mInm_miles1 = pInm_miles1
        mInm_miles2 = pInm_miles2
        mInm_km1 = pInm_km1
        mInm_km2 = pInm_km2
        mInm_disvat = pInm_disvat
        minm_cccvv = pinm_cccvv
        minm_gcid = pinm_gcid
        minm_gcno = pinm_gcno
    End Sub

    Public Sub New( _
)
    End Sub

    Private minm_cccvv As String
    Private minm_gcid As String
    Private minm_gcno As String
    Private mInvmainID As Integer
    Private mInm_no As String
    Private mInm_line As Nullable(Of Double)
    Private mInm_retail As Nullable(Of Boolean)
    Private mInm_br As String
    Private mInm_type As String
    Private mInm_invdt As Nullable(Of DateTime)
    Private mInm_entry As String
    Private mInm_custid As String
    Private mInm_costc As String
    Private mInm_pono As String
    Private mInm_cdate As Nullable(Of DateTime)
    Private mInm_prod As String
    Private mInm_suppid As String
    Private mInm_supnam As String
    Private mInm_htltel As String
    Private mInm_bsp As Nullable(Of Boolean)
    Private mInm_docmnt As String
    Private mInm_etkt As String
    Private mInm_books As Nullable(Of Double)
    Private mInm_exch As String
    Private mInm_origtk As String
    Private mInm_online As Nullable(Of Boolean)
    Private mInm_bspdte As Nullable(Of DateTime)
    Private mInm_start As Nullable(Of DateTime)
    Private mInm_end As Nullable(Of DateTime)
    Private mInm_ldname As String
    Private mInm_savecd As String
    Private mInm_mxfare As Decimal
    Private mInm_lwfare As Decimal
    Private mInm_bookng As Nullable(Of Boolean)
    Private mInm_fcrncy As String
    Private mInm_ffare As Decimal
    Private mInm_drate As Decimal
    Private mInm_days As Decimal
    Private mInm_curncy As String
    Private mInm_fare As Decimal
    Private mInm_srvchg As Decimal
    Private mInm_ubtax As Decimal
    Private mInm_yqtax As Decimal
    Private mInm_othtax As Decimal
    Private mInm_tax As Decimal
    Private mInm_ourchg As Decimal
    Private mInm_amtvat As Decimal
    Private mInm_supvat As Decimal
    Private mInm_ourvat As Decimal
    Private mInm_amount As Decimal
    Private mInm_dscpct As Decimal
    Private mInm_discnt As Decimal
    Private mInm_billed As Decimal
    Private mInm_bilvat As Decimal
    Private mInm_compct As Decimal
    Private mInm_comvat As Decimal
    Private mInm_vtoncm As Decimal
    Private mInm_trucom As Decimal
    Private mInm_othcom As Decimal
    Private mInm_comamt As Decimal
    Private mInm_comrcv As Decimal
    Private mInm_comdue As Decimal
    Private mInm_paynet As Nullable(Of Boolean)
    Private mInm_vatinv As Nullable(Of Boolean)
    Private mInm_cominv As Nullable(Of Boolean)
    Private mInm_morpax As String
    Private mInm_dposit As Decimal
    Private mInm_depok As Nullable(Of Boolean)
    Private mInm_depbr As String
    Private mInm_depbnk As String
    Private mInm_baldue As Nullable(Of DateTime)
    Private mInm_paytyp As String
    Private mInm_ccid As String
    Private mInm_ccno As String
    Private mInm_ccstdt As String
    Private mInm_ccexp As String
    Private mInm_ccauth As String
    Private mInm_issue As Nullable(Of Double)
    Private mInm_merch As Nullable(Of Boolean)
    Private mInm_mfee As Decimal
    Private mInm_ccmeth As String
    Private mInm_ccamt As Decimal
    Private mInm_ccxmit As Nullable(Of DateTime)
    Private mInm_print As Nullable(Of Double)
    Private mInm_con1pd As Nullable(Of DateTime)
    Private mInm_con2pd As Nullable(Of DateTime)
    Private mInm_orig As String
    Private mInm_locked As Nullable(Of Boolean)
    Private mInm_erflag As Nullable(Of Boolean)
    Private mInm_erdesc As String
    Private mInm_ukey As String
    Private mInm_change As Nullable(Of Boolean)
    Private mInm_who As String
    Private mInm_note As String
    Private mInm_crsref As String
    Private mInm_bdm As String
    Private mInm_pcity As String
    Private mInm_ino As String
    Private mInm_cos As String
    Private mInm_domint As String
    Private mInm_apok As Nullable(Of Boolean)
    Private mInm_arok As Nullable(Of Boolean)
    Private mInm_voided As String
    Private mInm_agcomm As Decimal
    Private mInm_agvat As Decimal
    Private mInm_agdcpc As Decimal
    Private mInm_atol As Nullable(Of Boolean)
    Private mInm_abond As String
    Private mInm_afare As Decimal
    Private mInm_aheads As Decimal
    Private mInm_nfare As Nullable(Of Boolean)
    Private mInm_rebate As Decimal
    Private mInm_fee As Decimal
    Private mInm_feevt As Decimal
    Private mInm_feebas As String
    Private mInm_dla As Nullable(Of DateTime)
    Private mInm_bywho As String
    Private mInm_cinvrf As String
    Private mTmpfld As String
    Private mInm_3rdpty As Nullable(Of Boolean)
    Private mInm_ourcc As String
    Private mInm_itcode As String
    Private mInm_bkcur As String
    Private mInm_bkroe As Decimal
    Private mChksum As String
    Private mDatecreated As Nullable(Of DateTime)
    Private mInm_miles1 As String
    Private mInm_miles2 As String
    Private mInm_km1 As String
    Private mInm_km2 As String
    Private mInm_disvat As Decimal

    Public Property inm_gcid() As String
        Get
            Return minm_gcid
        End Get
        Set(ByVal value As String)
            minm_gcid = value
        End Set
    End Property

    Public Property inm_gcno() As String
        Get
            Return minm_gcno
        End Get
        Set(ByVal value As String)
            minm_gcno = value
        End Set
    End Property

    Public Property inm_cccvv() As String
        Get
            Return minm_cccvv
        End Get
        Set(ByVal value As String)
            minm_cccvv = value
        End Set
    End Property

    Public Property InvmainID() As Integer
        Get
            Return mInvmainID
        End Get
        Set(ByVal value As Integer)
            mInvmainID = value
        End Set
    End Property

    Public Property Inm_no() As String
        Get
            Return mInm_no
        End Get
        Set(ByVal value As String)
            mInm_no = value
        End Set
    End Property

    Public Property Inm_line() As Nullable(Of Double)
        Get
            Return mInm_line
        End Get
        Set(ByVal value As Nullable(Of Double))
            mInm_line = value
        End Set
    End Property

    Public Property Inm_retail() As Nullable(Of Boolean)
        Get
            Return mInm_retail
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_retail = value
        End Set
    End Property

    Public Property Inm_br() As String
        Get
            Return mInm_br
        End Get
        Set(ByVal value As String)
            mInm_br = value
        End Set
    End Property

    Public Property Inm_type() As String
        Get
            Return mInm_type
        End Get
        Set(ByVal value As String)
            mInm_type = value
        End Set
    End Property

    Public Property Inm_invdt() As Nullable(Of DateTime)
        Get
            Return mInm_invdt
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mInm_invdt = value
        End Set
    End Property

    Public Property Inm_entry() As String
        Get
            Return mInm_entry
        End Get
        Set(ByVal value As String)
            mInm_entry = value
        End Set
    End Property

    Public Property Inm_custid() As String
        Get
            Return mInm_custid
        End Get
        Set(ByVal value As String)
            mInm_custid = value
        End Set
    End Property

    Public Property Inm_costc() As String
        Get
            Return mInm_costc
        End Get
        Set(ByVal value As String)
            mInm_costc = value
        End Set
    End Property

    Public Property Inm_pono() As String
        Get
            Return mInm_pono
        End Get
        Set(ByVal value As String)
            mInm_pono = value
        End Set
    End Property

    Public Property Inm_cdate() As Nullable(Of DateTime)
        Get
            Return mInm_cdate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mInm_cdate = value
        End Set
    End Property

    Public Property Inm_prod() As String
        Get
            Return mInm_prod
        End Get
        Set(ByVal value As String)
            mInm_prod = value
        End Set
    End Property

    Public Property Inm_suppid() As String
        Get
            Return mInm_suppid
        End Get
        Set(ByVal value As String)
            mInm_suppid = value
        End Set
    End Property

    Public Property Inm_supnam() As String
        Get
            Return mInm_supnam
        End Get
        Set(ByVal value As String)
            mInm_supnam = value
        End Set
    End Property

    Public Property Inm_htltel() As String
        Get
            Return mInm_htltel
        End Get
        Set(ByVal value As String)
            mInm_htltel = value
        End Set
    End Property

    Public Property Inm_bsp() As Nullable(Of Boolean)
        Get
            Return mInm_bsp
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_bsp = value
        End Set
    End Property

    Public Property Inm_docmnt() As String
        Get
            Return mInm_docmnt
        End Get
        Set(ByVal value As String)
            mInm_docmnt = value
        End Set
    End Property

    Public Property Inm_etkt() As String
        Get
            Return mInm_etkt
        End Get
        Set(ByVal value As String)
            mInm_etkt = value
        End Set
    End Property

    Public Property Inm_books() As Nullable(Of Double)
        Get
            Return mInm_books
        End Get
        Set(ByVal value As Nullable(Of Double))
            mInm_books = value
        End Set
    End Property

    Public Property Inm_exch() As String
        Get
            Return mInm_exch
        End Get
        Set(ByVal value As String)
            mInm_exch = value
        End Set
    End Property

    Public Property Inm_origtk() As String
        Get
            Return mInm_origtk
        End Get
        Set(ByVal value As String)
            mInm_origtk = value
        End Set
    End Property

    Public Property Inm_online() As Nullable(Of Boolean)
        Get
            Return mInm_online
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_online = value
        End Set
    End Property

    Public Property Inm_bspdte() As Nullable(Of DateTime)
        Get
            Return mInm_bspdte
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mInm_bspdte = value
        End Set
    End Property

    Public Property Inm_start() As Nullable(Of DateTime)
        Get
            Return mInm_start
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mInm_start = value
        End Set
    End Property

    Public Property Inm_end() As Nullable(Of DateTime)
        Get
            Return mInm_end
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mInm_end = value
        End Set
    End Property

    Public Property Inm_ldname() As String
        Get
            Return mInm_ldname
        End Get
        Set(ByVal value As String)
            mInm_ldname = value
        End Set
    End Property

    Public Property Inm_savecd() As String
        Get
            Return mInm_savecd
        End Get
        Set(ByVal value As String)
            mInm_savecd = value
        End Set
    End Property

    Public Property Inm_mxfare() As Decimal
        Get
            Return mInm_mxfare
        End Get
        Set(ByVal value As Decimal)
            mInm_mxfare = value
        End Set
    End Property

    Public Property Inm_lwfare() As Decimal
        Get
            Return mInm_lwfare
        End Get
        Set(ByVal value As Decimal)
            mInm_lwfare = value
        End Set
    End Property

    Public Property Inm_bookng() As Nullable(Of Boolean)
        Get
            Return mInm_bookng
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_bookng = value
        End Set
    End Property

    Public Property Inm_fcrncy() As String
        Get
            Return mInm_fcrncy
        End Get
        Set(ByVal value As String)
            mInm_fcrncy = value
        End Set
    End Property

    Public Property Inm_ffare() As Decimal
        Get
            Return mInm_ffare
        End Get
        Set(ByVal value As Decimal)
            mInm_ffare = value
        End Set
    End Property

    Public Property Inm_drate() As Decimal
        Get
            Return mInm_drate
        End Get
        Set(ByVal value As Decimal)
            mInm_drate = value
        End Set
    End Property

    Public Property Inm_days() As Decimal
        Get
            Return mInm_days
        End Get
        Set(ByVal value As Decimal)
            mInm_days = value
        End Set
    End Property

    Public Property Inm_curncy() As String
        Get
            Return mInm_curncy
        End Get
        Set(ByVal value As String)
            mInm_curncy = value
        End Set
    End Property

    Public Property Inm_fare() As Decimal
        Get
            Return mInm_fare
        End Get
        Set(ByVal value As Decimal)
            mInm_fare = value
        End Set
    End Property

    Public Property Inm_srvchg() As Decimal
        Get
            Return mInm_srvchg
        End Get
        Set(ByVal value As Decimal)
            mInm_srvchg = value
        End Set
    End Property

    Public Property Inm_ubtax() As Decimal
        Get
            Return mInm_ubtax
        End Get
        Set(ByVal value As Decimal)
            mInm_ubtax = value
        End Set
    End Property

    Public Property Inm_yqtax() As Decimal
        Get
            Return mInm_yqtax
        End Get
        Set(ByVal value As Decimal)
            mInm_yqtax = value
        End Set
    End Property

    Public Property Inm_othtax() As Decimal
        Get
            Return mInm_othtax
        End Get
        Set(ByVal value As Decimal)
            mInm_othtax = value
        End Set
    End Property

    Public Property Inm_tax() As Decimal
        Get
            Return mInm_tax
        End Get
        Set(ByVal value As Decimal)
            mInm_tax = value
        End Set
    End Property

    Public Property Inm_ourchg() As Decimal
        Get
            Return mInm_ourchg
        End Get
        Set(ByVal value As Decimal)
            mInm_ourchg = value
        End Set
    End Property

    Public Property Inm_amtvat() As Decimal
        Get
            Return mInm_amtvat
        End Get
        Set(ByVal value As Decimal)
            mInm_amtvat = value
        End Set
    End Property

    Public Property Inm_supvat() As Decimal
        Get
            Return mInm_supvat
        End Get
        Set(ByVal value As Decimal)
            mInm_supvat = value
        End Set
    End Property

    Public Property Inm_ourvat() As Decimal
        Get
            Return mInm_ourvat
        End Get
        Set(ByVal value As Decimal)
            mInm_ourvat = value
        End Set
    End Property

    Public Property Inm_amount() As Decimal
        Get
            Return mInm_amount
        End Get
        Set(ByVal value As Decimal)
            mInm_amount = value
        End Set
    End Property

    Public Property Inm_dscpct() As Decimal
        Get
            Return mInm_dscpct
        End Get
        Set(ByVal value As Decimal)
            mInm_dscpct = value
        End Set
    End Property

    Public Property Inm_discnt() As Decimal
        Get
            Return mInm_discnt
        End Get
        Set(ByVal value As Decimal)
            mInm_discnt = value
        End Set
    End Property

    Public Property Inm_billed() As Decimal
        Get
            Return mInm_billed
        End Get
        Set(ByVal value As Decimal)
            mInm_billed = value
        End Set
    End Property

    Public Property Inm_bilvat() As Decimal
        Get
            Return mInm_bilvat
        End Get
        Set(ByVal value As Decimal)
            mInm_bilvat = value
        End Set
    End Property

    Public Property Inm_compct() As Decimal
        Get
            Return mInm_compct
        End Get
        Set(ByVal value As Decimal)
            mInm_compct = value
        End Set
    End Property

    Public Property Inm_comvat() As Decimal
        Get
            Return mInm_comvat
        End Get
        Set(ByVal value As Decimal)
            mInm_comvat = value
        End Set
    End Property

    Public Property Inm_vtoncm() As Decimal
        Get
            Return mInm_vtoncm
        End Get
        Set(ByVal value As Decimal)
            mInm_vtoncm = value
        End Set
    End Property

    Public Property Inm_trucom() As Decimal
        Get
            Return mInm_trucom
        End Get
        Set(ByVal value As Decimal)
            mInm_trucom = value
        End Set
    End Property

    Public Property Inm_othcom() As Decimal
        Get
            Return mInm_othcom
        End Get
        Set(ByVal value As Decimal)
            mInm_othcom = value
        End Set
    End Property

    Public Property Inm_comamt() As Decimal
        Get
            Return mInm_comamt
        End Get
        Set(ByVal value As Decimal)
            mInm_comamt = value
        End Set
    End Property

    Public Property Inm_comrcv() As Decimal
        Get
            Return mInm_comrcv
        End Get
        Set(ByVal value As Decimal)
            mInm_comrcv = value
        End Set
    End Property

    Public Property Inm_comdue() As Decimal
        Get
            Return mInm_comdue
        End Get
        Set(ByVal value As Decimal)
            mInm_comdue = value
        End Set
    End Property

    Public Property Inm_paynet() As Nullable(Of Boolean)
        Get
            Return mInm_paynet
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_paynet = value
        End Set
    End Property

    Public Property Inm_vatinv() As Nullable(Of Boolean)
        Get
            Return mInm_vatinv
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_vatinv = value
        End Set
    End Property

    Public Property Inm_cominv() As Nullable(Of Boolean)
        Get
            Return mInm_cominv
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_cominv = value
        End Set
    End Property

    Public Property Inm_morpax() As String
        Get
            Return mInm_morpax
        End Get
        Set(ByVal value As String)
            mInm_morpax = value
        End Set
    End Property

    Public Property Inm_dposit() As Decimal
        Get
            Return mInm_dposit
        End Get
        Set(ByVal value As Decimal)
            mInm_dposit = value
        End Set
    End Property

    Public Property Inm_depok() As Nullable(Of Boolean)
        Get
            Return mInm_depok
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_depok = value
        End Set
    End Property

    Public Property Inm_depbr() As String
        Get
            Return mInm_depbr
        End Get
        Set(ByVal value As String)
            mInm_depbr = value
        End Set
    End Property

    Public Property Inm_depbnk() As String
        Get
            Return mInm_depbnk
        End Get
        Set(ByVal value As String)
            mInm_depbnk = value
        End Set
    End Property

    Public Property Inm_baldue() As Nullable(Of DateTime)
        Get
            Return mInm_baldue
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mInm_baldue = value
        End Set
    End Property

    Public Property Inm_paytyp() As String
        Get
            Return mInm_paytyp
        End Get
        Set(ByVal value As String)
            mInm_paytyp = value
        End Set
    End Property

    Public Property Inm_ccid() As String
        Get
            Return mInm_ccid
        End Get
        Set(ByVal value As String)
            mInm_ccid = value
        End Set
    End Property

    Public Property Inm_ccno() As String
        Get
            Return mInm_ccno
        End Get
        Set(ByVal value As String)
            mInm_ccno = value
        End Set
    End Property

    Public Property Inm_ccstdt() As String
        Get
            Return mInm_ccstdt
        End Get
        Set(ByVal value As String)
            mInm_ccstdt = value
        End Set
    End Property

    Public Property Inm_ccexp() As String
        Get
            Return mInm_ccexp
        End Get
        Set(ByVal value As String)
            mInm_ccexp = value
        End Set
    End Property

    Public Property Inm_ccauth() As String
        Get
            Return mInm_ccauth
        End Get
        Set(ByVal value As String)
            mInm_ccauth = value
        End Set
    End Property

    Public Property Inm_issue() As Nullable(Of Double)
        Get
            Return mInm_issue
        End Get
        Set(ByVal value As Nullable(Of Double))
            mInm_issue = value
        End Set
    End Property

    Public Property Inm_merch() As Nullable(Of Boolean)
        Get
            Return mInm_merch
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_merch = value
        End Set
    End Property

    Public Property Inm_mfee() As Decimal
        Get
            Return mInm_mfee
        End Get
        Set(ByVal value As Decimal)
            mInm_mfee = value
        End Set
    End Property

    Public Property Inm_ccmeth() As String
        Get
            Return mInm_ccmeth
        End Get
        Set(ByVal value As String)
            mInm_ccmeth = value
        End Set
    End Property

    Public Property Inm_ccamt() As Decimal
        Get
            Return mInm_ccamt
        End Get
        Set(ByVal value As Decimal)
            mInm_ccamt = value
        End Set
    End Property

    Public Property Inm_ccxmit() As Nullable(Of DateTime)
        Get
            Return mInm_ccxmit
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mInm_ccxmit = value
        End Set
    End Property

    Public Property Inm_print() As Nullable(Of Double)
        Get
            Return mInm_print
        End Get
        Set(ByVal value As Nullable(Of Double))
            mInm_print = value
        End Set
    End Property

    Public Property Inm_con1pd() As Nullable(Of DateTime)
        Get
            Return mInm_con1pd
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mInm_con1pd = value
        End Set
    End Property

    Public Property Inm_con2pd() As Nullable(Of DateTime)
        Get
            Return mInm_con2pd
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mInm_con2pd = value
        End Set
    End Property

    Public Property Inm_orig() As String
        Get
            Return mInm_orig
        End Get
        Set(ByVal value As String)
            mInm_orig = value
        End Set
    End Property

    Public Property Inm_locked() As Nullable(Of Boolean)
        Get
            Return mInm_locked
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_locked = value
        End Set
    End Property

    Public Property Inm_erflag() As Nullable(Of Boolean)
        Get
            Return mInm_erflag
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_erflag = value
        End Set
    End Property

    Public Property Inm_erdesc() As String
        Get
            Return mInm_erdesc
        End Get
        Set(ByVal value As String)
            mInm_erdesc = value
        End Set
    End Property

    Public Property Inm_ukey() As String
        Get
            Return mInm_ukey
        End Get
        Set(ByVal value As String)
            mInm_ukey = value
        End Set
    End Property

    Public Property Inm_change() As Nullable(Of Boolean)
        Get
            Return mInm_change
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_change = value
        End Set
    End Property

    Public Property Inm_who() As String
        Get
            Return mInm_who
        End Get
        Set(ByVal value As String)
            mInm_who = value
        End Set
    End Property

    Public Property Inm_note() As String
        Get
            Return mInm_note
        End Get
        Set(ByVal value As String)
            mInm_note = value
        End Set
    End Property

    Public Property Inm_crsref() As String
        Get
            Return mInm_crsref
        End Get
        Set(ByVal value As String)
            mInm_crsref = value
        End Set
    End Property

    Public Property Inm_bdm() As String
        Get
            Return mInm_bdm
        End Get
        Set(ByVal value As String)
            mInm_bdm = value
        End Set
    End Property

    Public Property Inm_pcity() As String
        Get
            Return mInm_pcity
        End Get
        Set(ByVal value As String)
            mInm_pcity = value
        End Set
    End Property

    Public Property Inm_ino() As String
        Get
            Return mInm_ino
        End Get
        Set(ByVal value As String)
            mInm_ino = value
        End Set
    End Property

    Public Property Inm_cos() As String
        Get
            Return mInm_cos
        End Get
        Set(ByVal value As String)
            mInm_cos = value
        End Set
    End Property

    Public Property Inm_domint() As String
        Get
            Return mInm_domint
        End Get
        Set(ByVal value As String)
            mInm_domint = value
        End Set
    End Property

    Public Property Inm_apok() As Nullable(Of Boolean)
        Get
            Return mInm_apok
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_apok = value
        End Set
    End Property

    Public Property Inm_arok() As Nullable(Of Boolean)
        Get
            Return mInm_arok
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_arok = value
        End Set
    End Property

    Public Property Inm_voided() As String
        Get
            Return mInm_voided
        End Get
        Set(ByVal value As String)
            mInm_voided = value
        End Set
    End Property

    Public Property Inm_agcomm() As Decimal
        Get
            Return mInm_agcomm
        End Get
        Set(ByVal value As Decimal)
            mInm_agcomm = value
        End Set
    End Property

    Public Property Inm_agvat() As Decimal
        Get
            Return mInm_agvat
        End Get
        Set(ByVal value As Decimal)
            mInm_agvat = value
        End Set
    End Property

    Public Property Inm_agdcpc() As Decimal
        Get
            Return mInm_agdcpc
        End Get
        Set(ByVal value As Decimal)
            mInm_agdcpc = value
        End Set
    End Property

    Public Property Inm_atol() As Nullable(Of Boolean)
        Get
            Return mInm_atol
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_atol = value
        End Set
    End Property

    Public Property Inm_abond() As String
        Get
            Return mInm_abond
        End Get
        Set(ByVal value As String)
            mInm_abond = value
        End Set
    End Property

    Public Property Inm_afare() As Decimal
        Get
            Return mInm_afare
        End Get
        Set(ByVal value As Decimal)
            mInm_afare = value
        End Set
    End Property

    Public Property Inm_aheads() As Decimal
        Get
            Return mInm_aheads
        End Get
        Set(ByVal value As Decimal)
            mInm_aheads = value
        End Set
    End Property

    Public Property Inm_nfare() As Nullable(Of Boolean)
        Get
            Return mInm_nfare
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_nfare = value
        End Set
    End Property

    Public Property Inm_rebate() As Decimal
        Get
            Return mInm_rebate
        End Get
        Set(ByVal value As Decimal)
            mInm_rebate = value
        End Set
    End Property

    Public Property Inm_fee() As Decimal
        Get
            Return mInm_fee
        End Get
        Set(ByVal value As Decimal)
            mInm_fee = value
        End Set
    End Property

    Public Property Inm_feevt() As Decimal
        Get
            Return mInm_feevt
        End Get
        Set(ByVal value As Decimal)
            mInm_feevt = value
        End Set
    End Property

    Public Property Inm_feebas() As String
        Get
            Return mInm_feebas
        End Get
        Set(ByVal value As String)
            mInm_feebas = value
        End Set
    End Property

    Public Property Inm_dla() As Nullable(Of DateTime)
        Get
            Return mInm_dla
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mInm_dla = value
        End Set
    End Property

    Public Property Inm_bywho() As String
        Get
            Return mInm_bywho
        End Get
        Set(ByVal value As String)
            mInm_bywho = value
        End Set
    End Property

    Public Property Inm_cinvrf() As String
        Get
            Return mInm_cinvrf
        End Get
        Set(ByVal value As String)
            mInm_cinvrf = value
        End Set
    End Property

    Public Property Tmpfld() As String
        Get
            Return mTmpfld
        End Get
        Set(ByVal value As String)
            mTmpfld = value
        End Set
    End Property

    Public Property Inm_3rdpty() As Nullable(Of Boolean)
        Get
            Return mInm_3rdpty
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInm_3rdpty = value
        End Set
    End Property

    Public Property Inm_ourcc() As String
        Get
            Return mInm_ourcc
        End Get
        Set(ByVal value As String)
            mInm_ourcc = value
        End Set
    End Property

    Public Property Inm_itcode() As String
        Get
            Return mInm_itcode
        End Get
        Set(ByVal value As String)
            mInm_itcode = value
        End Set
    End Property

    Public Property Inm_bkcur() As String
        Get
            Return mInm_bkcur
        End Get
        Set(ByVal value As String)
            mInm_bkcur = value
        End Set
    End Property

    Public Property Inm_bkroe() As Decimal
        Get
            Return mInm_bkroe
        End Get
        Set(ByVal value As Decimal)
            mInm_bkroe = value
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

    Public Property Datecreated() As Nullable(Of DateTime)
        Get
            Return mDatecreated
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mDatecreated = value
        End Set
    End Property

    Public Property Inm_miles1() As String
        Get
            Return mInm_miles1
        End Get
        Set(ByVal value As String)
            mInm_miles1 = value
        End Set
    End Property

    Public Property Inm_miles2() As String
        Get
            Return mInm_miles2
        End Get
        Set(ByVal value As String)
            mInm_miles2 = value
        End Set
    End Property

    Public Property Inm_km1() As String
        Get
            Return mInm_km1
        End Get
        Set(ByVal value As String)
            mInm_km1 = value
        End Set
    End Property

    Public Property Inm_km2() As String
        Get
            Return mInm_km2
        End Get
        Set(ByVal value As String)
            mInm_km2 = value
        End Set
    End Property

    Public Property Inm_disvat() As Decimal
        Get
            Return mInm_disvat
        End Get
        Set(ByVal value As Decimal)
            mInm_disvat = value
        End Set
    End Property

    Private Shared Function makeBOSSinvmainFromRow( _
            ByVal r As IDataReader _
        ) As BOSSinvmain
        Return New BOSSinvmain( _
                clsUseful.notInteger(r.Item("invmainID")), _
                clsUseful.notString(r.Item("inm_no")), _
                clsUseful.notNumber(r.Item("inm_line")), _
                clsUseful.notBoolean(r.Item("inm_retail")), _
                clsUseful.notString(r.Item("inm_br")), _
                clsUseful.notString(r.Item("inm_type")), _
                CDate(r.Item("inm_invdt")), _
                clsUseful.notString(r.Item("inm_entry")), _
                clsUseful.notString(r.Item("inm_custid")), _
                clsUseful.notString(r.Item("inm_costc")), _
                clsUseful.notString(r.Item("inm_pono")), _
                CDate(r.Item("inm_cdate")), _
                clsUseful.notString(r.Item("inm_prod")), _
                clsUseful.notString(r.Item("inm_suppid")), _
                clsUseful.notString(r.Item("inm_supnam")), _
                clsUseful.notString(r.Item("inm_htltel")), _
                clsUseful.notBoolean(r.Item("inm_bsp")), _
                clsUseful.notString(r.Item("inm_docmnt")), _
                clsUseful.notString(r.Item("inm_etkt")), _
                clsUseful.notNumber(r.Item("inm_books")), _
                clsUseful.notString(r.Item("inm_exch")), _
                clsUseful.notString(r.Item("inm_origtk")), _
                clsUseful.notBoolean(r.Item("inm_online")), _
                CDate(r.Item("inm_bspdte")), _
                CDate(r.Item("inm_start")), _
                CDate(r.Item("inm_end")), _
                clsUseful.notString(r.Item("inm_ldname")), _
                clsUseful.notString(r.Item("inm_savecd")), _
                clsUseful.notDecimal(r.Item("inm_mxfare")), _
                clsUseful.notDecimal(r.Item("inm_lwfare")), _
                clsUseful.notBoolean(r.Item("inm_bookng")), _
                clsUseful.notString(r.Item("inm_fcrncy")), _
                clsUseful.notDecimal(r.Item("inm_ffare")), _
                clsUseful.notDecimal(r.Item("inm_drate")), _
                clsUseful.notDecimal(r.Item("inm_days")), _
                clsUseful.notString(r.Item("inm_curncy")), _
                clsUseful.notDecimal(r.Item("inm_fare")), _
                clsUseful.notDecimal(r.Item("inm_srvchg")), _
                clsUseful.notDecimal(r.Item("inm_ubtax")), _
                clsUseful.notDecimal(r.Item("inm_yqtax")), _
                clsUseful.notDecimal(r.Item("inm_othtax")), _
                clsUseful.notDecimal(r.Item("inm_tax")), _
                clsUseful.notDecimal(r.Item("inm_ourchg")), _
                clsUseful.notDecimal(r.Item("inm_amtvat")), _
                clsUseful.notDecimal(r.Item("inm_supvat")), _
                clsUseful.notDecimal(r.Item("inm_ourvat")), _
                clsUseful.notDecimal(r.Item("inm_amount")), _
                clsUseful.notDecimal(r.Item("inm_dscpct")), _
                clsUseful.notDecimal(r.Item("inm_discnt")), _
                clsUseful.notDecimal(r.Item("inm_billed")), _
                clsUseful.notDecimal(r.Item("inm_bilvat")), _
                clsUseful.notDecimal(r.Item("inm_compct")), _
                clsUseful.notDecimal(r.Item("inm_comvat")), _
                clsUseful.notDecimal(r.Item("inm_vtoncm")), _
                clsUseful.notDecimal(r.Item("inm_trucom")), _
                clsUseful.notDecimal(r.Item("inm_othcom")), _
                clsUseful.notDecimal(r.Item("inm_comamt")), _
                clsUseful.notDecimal(r.Item("inm_comrcv")), _
                clsUseful.notDecimal(r.Item("inm_comdue")), _
                clsUseful.notBoolean(r.Item("inm_paynet")), _
                clsUseful.notBoolean(r.Item("inm_vatinv")), _
                clsUseful.notBoolean(r.Item("inm_cominv")), _
                clsUseful.notString(r.Item("inm_morpax")), _
                clsUseful.notDecimal(r.Item("inm_dposit")), _
                clsUseful.notBoolean(r.Item("inm_depok")), _
                clsUseful.notString(r.Item("inm_depbr")), _
                clsUseful.notString(r.Item("inm_depbnk")), _
                CDate(r.Item("inm_baldue")), _
                clsUseful.notString(r.Item("inm_paytyp")), _
                clsUseful.notString(r.Item("inm_ccid")), _
                clsUseful.notString(r.Item("inm_ccno")), _
                clsUseful.notString(r.Item("inm_ccstdt")), _
                clsUseful.notString(r.Item("inm_ccexp")), _
                clsUseful.notString(r.Item("inm_ccauth")), _
                clsUseful.notNumber(r.Item("inm_issue")), _
                clsUseful.notBoolean(r.Item("inm_merch")), _
                clsUseful.notDecimal(r.Item("inm_mfee")), _
                clsUseful.notString(r.Item("inm_ccmeth")), _
                clsUseful.notDecimal(r.Item("inm_ccamt")), _
                CDate(r.Item("inm_ccxmit")), _
                clsUseful.notNumber(r.Item("inm_print")), _
                CDate(r.Item("inm_con1pd")), _
                CDate(r.Item("inm_con2pd")), _
                clsUseful.notString(r.Item("inm_orig")), _
                clsUseful.notBoolean(r.Item("inm_locked")), _
                clsUseful.notBoolean(r.Item("inm_erflag")), _
                clsUseful.notString(r.Item("inm_erdesc")), _
                clsUseful.notString(r.Item("inm_ukey")), _
                clsUseful.notBoolean(r.Item("inm_change")), _
                clsUseful.notString(r.Item("inm_who")), _
                clsUseful.notString(r.Item("inm_note")), _
                clsUseful.notString(r.Item("inm_crsref")), _
                clsUseful.notString(r.Item("inm_bdm")), _
                clsUseful.notString(r.Item("inm_pcity")), _
                clsUseful.notString(r.Item("inm_ino")), _
                clsUseful.notString(r.Item("inm_cos")), _
                clsUseful.notString(r.Item("inm_domint")), _
                clsUseful.notBoolean(r.Item("inm_apok")), _
                clsUseful.notBoolean(r.Item("inm_arok")), _
                clsUseful.notString(r.Item("inm_voided")), _
                clsUseful.notDecimal(r.Item("inm_agcomm")), _
                clsUseful.notDecimal(r.Item("inm_agvat")), _
                clsUseful.notDecimal(r.Item("inm_agdcpc")), _
                clsUseful.notBoolean(r.Item("inm_atol")), _
                clsUseful.notString(r.Item("inm_abond")), _
                clsUseful.notDecimal(r.Item("inm_afare")), _
                clsUseful.notDecimal(r.Item("inm_aheads")), _
                clsUseful.notBoolean(r.Item("inm_nfare")), _
                clsUseful.notDecimal(r.Item("inm_rebate")), _
                clsUseful.notDecimal(r.Item("inm_fee")), _
                clsUseful.notDecimal(r.Item("inm_feevt")), _
                clsUseful.notString(r.Item("inm_feebas")), _
                CDate(r.Item("inm_dla")), _
                clsUseful.notString(r.Item("inm_bywho")), _
                clsUseful.notString(r.Item("inm_cinvrf")), _
                clsUseful.notString(r.Item("tmpfld")), _
                clsUseful.notBoolean(r.Item("inm_3rdpty")), _
                clsUseful.notString(r.Item("inm_ourcc")), _
                clsUseful.notString(r.Item("inm_itcode")), _
                clsUseful.notString(r.Item("inm_bkcur")), _
                clsUseful.notDecimal(r.Item("inm_bkroe")), _
                clsUseful.notString(r.Item("chksum")), _
                CDate(r.Item("datecreated")), _
                clsUseful.notString(r.Item("inm_miles1")), _
                clsUseful.notString(r.Item("inm_miles2")), _
                clsUseful.notString(r.Item("inm_km1")), _
                clsUseful.notString(r.Item("inm_km2")), _
                clsUseful.notDecimal(r.Item("inm_disvat")), _
                clsUseful.notString(r.Item("inm_cccvv")), _
                clsUseful.notString(r.Item("inm_gcid")), _
                clsUseful.notString(r.Item("inm_gcno")))
    End Function

    Public Shared Function [get]( _
            ByVal pInvmainID As Integer _
        ) As BOSSinvmain
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Using r As IDataReader = dbh.callSP("BOSSinvmain_get", "@invmainID", pInvmainID)
                If Not r.Read() Then
                    Throw New Exception("No BOSSinvmain with id " & pInvmainID)
                End If
                Dim ret As BOSSinvmain = makeBOSSinvmainFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of BOSSinvmain)
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim ret As New List(Of BOSSinvmain)()
            Using r As IDataReader = dbh.callSP("BOSSinvmain_list")
                While r.Read()
                    ret.Add(makeBOSSinvmainFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            mInvmainID = CInt(dbh.callSPSingleValue("BOSSinvmain_save", "@InvmainID", mInvmainID, "@Inm_no", mInm_no, "@Inm_line", mInm_line, _
                                                    "@Inm_retail", mInm_retail, "@Inm_br", mInm_br, "@Inm_type", mInm_type, "@Inm_invdt", mInm_invdt, _
                                                    "@Inm_entry", mInm_entry, "@Inm_custid", mInm_custid, "@Inm_costc", mInm_costc, "@Inm_pono", mInm_pono, _
                                                    "@Inm_cdate", mInm_cdate, "@Inm_prod", mInm_prod, "@Inm_suppid", mInm_suppid, "@Inm_supnam", mInm_supnam, _
                                                    "@Inm_htltel", mInm_htltel, "@Inm_bsp", mInm_bsp, "@Inm_docmnt", mInm_docmnt, "@Inm_etkt", mInm_etkt, _
                                                    "@Inm_books", mInm_books, "@Inm_exch", mInm_exch, "@Inm_origtk", mInm_origtk, "@Inm_online", mInm_online, _
                                                    "@Inm_bspdte", mInm_bspdte, "@Inm_start", mInm_start, "@Inm_end", mInm_end, "@Inm_ldname", mInm_ldname, _
                                                    "@Inm_savecd", mInm_savecd, "@Inm_mxfare", mInm_mxfare, "@Inm_lwfare", mInm_lwfare, "@Inm_bookng", mInm_bookng, _
                                                    "@Inm_fcrncy", mInm_fcrncy, "@Inm_ffare", mInm_ffare, "@Inm_drate", mInm_drate, "@Inm_days", mInm_days, _
                                                    "@Inm_curncy", mInm_curncy, "@Inm_fare", mInm_fare, "@Inm_srvchg", mInm_srvchg, "@Inm_ubtax", mInm_ubtax, _
                                                    "@Inm_yqtax", mInm_yqtax, "@Inm_othtax", mInm_othtax, "@Inm_tax", mInm_tax, "@Inm_ourchg", mInm_ourchg, _
                                                    "@Inm_amtvat", mInm_amtvat, "@Inm_supvat", mInm_supvat, "@Inm_ourvat", mInm_ourvat, "@Inm_amount", mInm_amount, _
                                                    "@Inm_dscpct", mInm_dscpct, "@Inm_discnt", mInm_discnt, "@Inm_billed", mInm_billed, "@Inm_bilvat", mInm_bilvat, _
                                                    "@Inm_compct", mInm_compct, "@Inm_comvat", mInm_comvat, "@Inm_vtoncm", mInm_vtoncm, "@Inm_trucom", mInm_trucom, _
                                                    "@Inm_othcom", mInm_othcom, "@Inm_comamt", mInm_comamt, "@Inm_comrcv", mInm_comrcv, "@Inm_comdue", mInm_comdue, _
                                                    "@Inm_paynet", mInm_paynet, "@Inm_vatinv", mInm_vatinv, "@Inm_cominv", mInm_cominv, "@Inm_morpax", mInm_morpax, _
                                                    "@Inm_dposit", mInm_dposit, "@Inm_depok", mInm_depok, "@Inm_depbr", mInm_depbr, "@Inm_depbnk", mInm_depbnk, _
                                                    "@Inm_baldue", mInm_baldue, "@Inm_paytyp", mInm_paytyp, "@Inm_ccid", mInm_ccid, "@Inm_ccno", mInm_ccno, "@Inm_ccstdt", _
                                                    mInm_ccstdt, "@Inm_ccexp", mInm_ccexp, "@Inm_ccauth", mInm_ccauth, "@Inm_issue", mInm_issue, "@Inm_merch", mInm_merch, _
                                                    "@Inm_mfee", mInm_mfee, "@Inm_ccmeth", mInm_ccmeth, "@Inm_ccamt", mInm_ccamt, "@Inm_ccxmit", mInm_ccxmit, "@Inm_print", _
                                                    mInm_print, "@Inm_con1pd", mInm_con1pd, "@Inm_con2pd", mInm_con2pd, "@Inm_orig", mInm_orig, "@Inm_locked", mInm_locked, _
                                                    "@Inm_erflag", mInm_erflag, "@Inm_erdesc", mInm_erdesc, "@Inm_ukey", mInm_ukey, "@Inm_change", mInm_change, "@Inm_who", _
                                                    mInm_who, "@Inm_note", mInm_note, "@Inm_crsref", mInm_crsref, "@Inm_bdm", mInm_bdm, "@Inm_pcity", mInm_pcity, "@Inm_ino", _
                                                    mInm_ino, "@Inm_cos", mInm_cos, "@Inm_domint", mInm_domint, "@Inm_apok", mInm_apok, "@Inm_arok", mInm_arok, "@Inm_voided", _
                                                    mInm_voided, "@Inm_agcomm", mInm_agcomm, "@Inm_agvat", mInm_agvat, "@Inm_agdcpc", mInm_agdcpc, "@Inm_atol", mInm_atol, _
                                                    "@Inm_abond", mInm_abond, "@Inm_afare", mInm_afare, "@Inm_aheads", mInm_aheads, "@Inm_nfare", mInm_nfare, "@Inm_rebate", _
                                                    mInm_rebate, "@Inm_fee", mInm_fee, "@Inm_feevt", mInm_feevt, "@Inm_feebas", mInm_feebas, "@Inm_dla", mInm_dla, "@Inm_bywho", _
                                                    mInm_bywho, "@Inm_cinvrf", mInm_cinvrf, "@Tmpfld", mTmpfld, "@Inm_3rdpty", mInm_3rdpty, "@Inm_ourcc", mInm_ourcc, _
                                                    "@Inm_itcode", mInm_itcode, "@Inm_bkcur", mInm_bkcur, "@Inm_bkroe", mInm_bkroe, "@Chksum", mChksum, "@Datecreated", _
                                                    mDatecreated, "@Inm_miles1", mInm_miles1, "@Inm_miles2", mInm_miles2, "@Inm_km1", mInm_km1, "@Inm_km2", mInm_km2, _
                                                    "@Inm_disvat", mInm_disvat, "@inm_cccvv", minm_cccvv, "@inm_gcid", minm_gcid, "@inm_gcno", minm_gcno))
            Return mInvmainID
        End Using
    End Function

    Public Shared Function BOSSinvmainCdate(ByVal pInm_no As String) As DateTime
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim dtRet As Date = CDate(dbh.callSPSingleValue("BOSSinvmain_cdate", "@inm_no", pInm_no))
            Return dtRet
        End Using
    End Function

    Public Shared Sub delete(ByVal pInvmainID As Integer, ByVal pInm_no As String)
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            dbh.callNonQuerySP("BOSSinvmain_delete", "@InvmainID", pInvmainID, "@inm_no", pInm_no)
        End Using
    End Sub

    Public Shared Sub saveComm(ByVal pInm_no As String, ByVal pInm_ukey As String, ByVal pinm_comrcv As Decimal, ByVal pinm_comdue As Decimal)
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            dbh.callNonQuerySP("BOSSinvmain_saveComm", "@inm_no", pInm_no, "@inm_comrcv", pinm_comrcv, "@inm_comdue", pinm_comdue, "@Inm_ukey", pInm_ukey)
        End Using
    End Sub

    Public Shared Function invoiceCount(ByVal startdate As String, ByVal enddate As String, ByVal custid1 As String, ByVal custid2 As String) As Integer
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim intRet As Integer = clsUseful.notInteger(dbh.callSPSingleValueCanReturnNothing("invoice_count",
                                                                                            "@startdate", startdate,
                                                                                            "@enddate", enddate,
                                                                                            "@custid1", custid1,
                                                                                            "@custid2", custid2))
            Return intRet
        End Using
    End Function
End Class
