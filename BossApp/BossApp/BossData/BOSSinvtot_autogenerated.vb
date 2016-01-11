Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSSinvtot

    Public Sub New( _
        ByVal pInvtotID As Integer, _
        ByVal pTot_ukey As String, _
        ByVal pTot_custid As String, _
        ByVal pTot_invno As String, _
        ByVal pTot_change As Nullable(Of Integer), _
        ByVal pTot_crsref As String, _
        ByVal pTot_br As String, _
        ByVal pTot_pono As String, _
        ByVal pTot_costc As String, _
        ByVal pTot_type As String, _
        ByVal pTot_invdt As Nullable(Of DateTime), _
        ByVal pTot_duedt As Nullable(Of DateTime), _
        ByVal pTot_fare As Decimal, _
        ByVal pTot_tax As Decimal, _
        ByVal pTot_srvchg As Decimal, _
        ByVal pTot_ourchg As Decimal, _
        ByVal pTot_ourvat As Decimal, _
        ByVal pTot_supvat As Decimal, _
        ByVal pTot_amtvat As Decimal, _
        ByVal pTot_bilvat As Decimal, _
        ByVal pTot_amount As Decimal, _
        ByVal pTot_billed As Decimal, _
        ByVal pTot_discnt As Decimal, _
        ByVal pTot_ccamt As Decimal, _
        ByVal pTot_recvd As Decimal, _
        ByVal pTot_dposit As Decimal, _
        ByVal pTot_comm As Decimal, _
        ByVal pTot_comdue As Decimal, _
        ByVal pTot_vtoncm As Decimal, _
        ByVal pTot_disput As Nullable(Of Boolean), _
        ByVal pTot_reason As String, _
        ByVal pTot_noerrs As Nullable(Of Boolean), _
        ByVal pTot_note As String, _
        ByVal pTot_raddr As String, _
        ByVal pTot_rtelno As String, _
        ByVal pTot_paxs As Nullable(Of Integer), _
        ByVal pTot_morcrs As String, _
        ByVal pTot_retail As Nullable(Of Boolean), _
        ByVal pTot_bdm As String, _
        ByVal pTot_print As Nullable(Of Integer), _
        ByVal pTot_errs As Nullable(Of Boolean), _
        ByVal pTot_atol As Nullable(Of Boolean), _
        ByVal pTot_atolx As String, _
        ByVal pTot_cref1 As String, _
        ByVal pTot_cref2 As String, _
        ByVal pTot_agcomm As Decimal, _
        ByVal pTot_agvat As Decimal, _
        ByVal pTot_fileno As String, _
        ByVal pTot_nofee As Nullable(Of Boolean), _
        ByVal pTot_crstyp As String, _
        ByVal pTot_noprnt As Nullable(Of Boolean), _
        ByVal pTot_email As String, _
        ByVal pTot_curncy As String, _
        ByVal pTot_roe As Decimal, _
        ByVal pTot_cursym As String, _
        ByVal pDatecreated As Nullable(Of DateTime))
        mInvtotID = pInvtotID
        mTot_ukey = pTot_ukey
        mTot_custid = pTot_custid
        mTot_invno = pTot_invno
        mTot_change = pTot_change
        mTot_crsref = pTot_crsref
        mTot_br = pTot_br
        mTot_pono = pTot_pono
        mTot_costc = pTot_costc
        mTot_type = pTot_type
        mTot_invdt = pTot_invdt
        mTot_duedt = pTot_duedt
        mTot_fare = pTot_fare
        mTot_tax = pTot_tax
        mTot_srvchg = pTot_srvchg
        mTot_ourchg = pTot_ourchg
        mTot_ourvat = pTot_ourvat
        mTot_supvat = pTot_supvat
        mTot_amtvat = pTot_amtvat
        mTot_bilvat = pTot_bilvat
        mTot_amount = pTot_amount
        mTot_billed = pTot_billed
        mTot_discnt = pTot_discnt
        mTot_ccamt = pTot_ccamt
        mTot_recvd = pTot_recvd
        mTot_dposit = pTot_dposit
        mTot_comm = pTot_comm
        mTot_comdue = pTot_comdue
        mTot_vtoncm = pTot_vtoncm
        mTot_disput = pTot_disput
        mTot_reason = pTot_reason
        mTot_noerrs = pTot_noerrs
        mTot_note = pTot_note
        mTot_raddr = pTot_raddr
        mTot_rtelno = pTot_rtelno
        mTot_paxs = pTot_paxs
        mTot_morcrs = pTot_morcrs
        mTot_retail = pTot_retail
        mTot_bdm = pTot_bdm
        mTot_print = pTot_print
        mTot_errs = pTot_errs
        mTot_atol = pTot_atol
        mTot_atolx = pTot_atolx
        mTot_cref1 = pTot_cref1
        mTot_cref2 = pTot_cref2
        mTot_agcomm = pTot_agcomm
        mTot_agvat = pTot_agvat
        mTot_fileno = pTot_fileno
        mTot_nofee = pTot_nofee
        mTot_crstyp = pTot_crstyp
        mTot_noprnt = pTot_noprnt
        mTot_email = pTot_email
        mTot_curncy = pTot_curncy
        mTot_roe = pTot_roe
        mTot_cursym = pTot_cursym
        mDatecreated = pDatecreated
    End Sub

    Public Sub New( _
)
    End Sub

    Public Sub New(ByVal pCustomerID As String)
        mCustomerID = pCustomerID
    End Sub

    Private mCustomerID As String
    Private mInvtotID As Integer
    Private mTot_ukey As String
    Private mTot_custid As String
    Private mTot_invno As String
    Private mTot_change As Nullable(Of Integer)
    Private mTot_crsref As String
    Private mTot_br As String
    Private mTot_pono As String
    Private mTot_costc As String
    Private mTot_type As String
    Private mTot_invdt As Nullable(Of DateTime)
    Private mTot_duedt As Nullable(Of DateTime)
    Private mTot_fare As Decimal
    Private mTot_tax As Decimal
    Private mTot_srvchg As Decimal
    Private mTot_ourchg As Decimal
    Private mTot_ourvat As Decimal
    Private mTot_supvat As Decimal
    Private mTot_amtvat As Decimal
    Private mTot_bilvat As Decimal
    Private mTot_amount As Decimal
    Private mTot_billed As Decimal
    Private mTot_discnt As Decimal
    Private mTot_ccamt As Decimal
    Private mTot_recvd As Decimal
    Private mTot_dposit As Decimal
    Private mTot_comm As Decimal
    Private mTot_comdue As Decimal
    Private mTot_vtoncm As Decimal
    Private mTot_disput As Nullable(Of Boolean)
    Private mTot_reason As String
    Private mTot_noerrs As Nullable(Of Boolean)
    Private mTot_note As String
    Private mTot_raddr As String
    Private mTot_rtelno As String
    Private mTot_paxs As Nullable(Of Integer)
    Private mTot_morcrs As String
    Private mTot_retail As Nullable(Of Boolean)
    Private mTot_bdm As String
    Private mTot_print As Nullable(Of Integer)
    Private mTot_errs As Nullable(Of Boolean)
    Private mTot_atol As Nullable(Of Boolean)
    Private mTot_atolx As String
    Private mTot_cref1 As String
    Private mTot_cref2 As String
    Private mTot_agcomm As Decimal
    Private mTot_agvat As Decimal
    Private mTot_fileno As String
    Private mTot_nofee As Nullable(Of Boolean)
    Private mTot_crstyp As String
    Private mTot_noprnt As Nullable(Of Boolean)
    Private mTot_email As String
    Private mTot_curncy As String
    Private mTot_roe As Decimal
    Private mTot_cursym As String
    Private mDatecreated As Nullable(Of DateTime)

    Public Property customerid() As String
        Get
            Return mCustomerID
        End Get
        Set(ByVal value As String)
            mCustomerID = value
        End Set
    End Property

    Public Property InvtotID() As Integer
        Get
            Return mInvtotID
        End Get
        Set(ByVal value As Integer)
            mInvtotID = value
        End Set
    End Property

    Public Property Tot_ukey() As String
        Get
            Return mTot_ukey
        End Get
        Set(ByVal value As String)
            mTot_ukey = value
        End Set
    End Property

    Public Property Tot_custid() As String
        Get
            Return mTot_custid
        End Get
        Set(ByVal value As String)
            mTot_custid = value
        End Set
    End Property

    Public Property Tot_invno() As String
        Get
            Return mTot_invno
        End Get
        Set(ByVal value As String)
            mTot_invno = value
        End Set
    End Property

    Public Property Tot_change() As Nullable(Of Integer)
        Get
            Return mTot_change
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mTot_change = value
        End Set
    End Property

    Public Property Tot_crsref() As String
        Get
            Return mTot_crsref
        End Get
        Set(ByVal value As String)
            mTot_crsref = value
        End Set
    End Property

    Public Property Tot_br() As String
        Get
            Return mTot_br
        End Get
        Set(ByVal value As String)
            mTot_br = value
        End Set
    End Property

    Public Property Tot_pono() As String
        Get
            Return mTot_pono
        End Get
        Set(ByVal value As String)
            mTot_pono = value
        End Set
    End Property

    Public Property Tot_costc() As String
        Get
            Return mTot_costc
        End Get
        Set(ByVal value As String)
            mTot_costc = value
        End Set
    End Property

    Public Property Tot_type() As String
        Get
            Return mTot_type
        End Get
        Set(ByVal value As String)
            mTot_type = value
        End Set
    End Property

    Public Property Tot_invdt() As Nullable(Of DateTime)
        Get
            Return mTot_invdt
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mTot_invdt = value
        End Set
    End Property

    Public Property Tot_duedt() As Nullable(Of DateTime)
        Get
            Return mTot_duedt
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mTot_duedt = value
        End Set
    End Property

    Public Property Tot_fare() As Decimal
        Get
            Return mTot_fare
        End Get
        Set(ByVal value As Decimal)
            mTot_fare = value
        End Set
    End Property

    Public Property Tot_tax() As Decimal
        Get
            Return mTot_tax
        End Get
        Set(ByVal value As Decimal)
            mTot_tax = value
        End Set
    End Property

    Public Property Tot_srvchg() As Decimal
        Get
            Return mTot_srvchg
        End Get
        Set(ByVal value As Decimal)
            mTot_srvchg = value
        End Set
    End Property

    Public Property Tot_ourchg() As Decimal
        Get
            Return mTot_ourchg
        End Get
        Set(ByVal value As Decimal)
            mTot_ourchg = value
        End Set
    End Property

    Public Property Tot_ourvat() As Decimal
        Get
            Return mTot_ourvat
        End Get
        Set(ByVal value As Decimal)
            mTot_ourvat = value
        End Set
    End Property

    Public Property Tot_supvat() As Decimal
        Get
            Return mTot_supvat
        End Get
        Set(ByVal value As Decimal)
            mTot_supvat = value
        End Set
    End Property

    Public Property Tot_amtvat() As Decimal
        Get
            Return mTot_amtvat
        End Get
        Set(ByVal value As Decimal)
            mTot_amtvat = value
        End Set
    End Property

    Public Property Tot_bilvat() As Decimal
        Get
            Return mTot_bilvat
        End Get
        Set(ByVal value As Decimal)
            mTot_bilvat = value
        End Set
    End Property

    Public Property Tot_amount() As Decimal
        Get
            Return mTot_amount
        End Get
        Set(ByVal value As Decimal)
            mTot_amount = value
        End Set
    End Property

    Public Property Tot_billed() As Decimal
        Get
            Return mTot_billed
        End Get
        Set(ByVal value As Decimal)
            mTot_billed = value
        End Set
    End Property

    Public Property Tot_discnt() As Decimal
        Get
            Return mTot_discnt
        End Get
        Set(ByVal value As Decimal)
            mTot_discnt = value
        End Set
    End Property

    Public Property Tot_ccamt() As Decimal
        Get
            Return mTot_ccamt
        End Get
        Set(ByVal value As Decimal)
            mTot_ccamt = value
        End Set
    End Property

    Public Property Tot_recvd() As Decimal
        Get
            Return mTot_recvd
        End Get
        Set(ByVal value As Decimal)
            mTot_recvd = value
        End Set
    End Property

    Public Property Tot_dposit() As Decimal
        Get
            Return mTot_dposit
        End Get
        Set(ByVal value As Decimal)
            mTot_dposit = value
        End Set
    End Property

    Public Property Tot_comm() As Decimal
        Get
            Return mTot_comm
        End Get
        Set(ByVal value As Decimal)
            mTot_comm = value
        End Set
    End Property

    Public Property Tot_comdue() As Decimal
        Get
            Return mTot_comdue
        End Get
        Set(ByVal value As Decimal)
            mTot_comdue = value
        End Set
    End Property

    Public Property Tot_vtoncm() As Decimal
        Get
            Return mTot_vtoncm
        End Get
        Set(ByVal value As Decimal)
            mTot_vtoncm = value
        End Set
    End Property

    Public Property Tot_disput() As Nullable(Of Boolean)
        Get
            Return mTot_disput
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mTot_disput = value
        End Set
    End Property

    Public Property Tot_reason() As String
        Get
            Return mTot_reason
        End Get
        Set(ByVal value As String)
            mTot_reason = value
        End Set
    End Property

    Public Property Tot_noerrs() As Nullable(Of Boolean)
        Get
            Return mTot_noerrs
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mTot_noerrs = value
        End Set
    End Property

    Public Property Tot_note() As String
        Get
            Return mTot_note
        End Get
        Set(ByVal value As String)
            mTot_note = value
        End Set
    End Property

    Public Property Tot_raddr() As String
        Get
            Return mTot_raddr
        End Get
        Set(ByVal value As String)
            mTot_raddr = value
        End Set
    End Property

    Public Property Tot_rtelno() As String
        Get
            Return mTot_rtelno
        End Get
        Set(ByVal value As String)
            mTot_rtelno = value
        End Set
    End Property

    Public Property Tot_paxs() As Nullable(Of Integer)
        Get
            Return mTot_paxs
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mTot_paxs = value
        End Set
    End Property

    Public Property Tot_morcrs() As String
        Get
            Return mTot_morcrs
        End Get
        Set(ByVal value As String)
            mTot_morcrs = value
        End Set
    End Property

    Public Property Tot_retail() As Nullable(Of Boolean)
        Get
            Return mTot_retail
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mTot_retail = value
        End Set
    End Property

    Public Property Tot_bdm() As String
        Get
            Return mTot_bdm
        End Get
        Set(ByVal value As String)
            mTot_bdm = value
        End Set
    End Property

    Public Property Tot_print() As Nullable(Of Integer)
        Get
            Return mTot_print
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mTot_print = value
        End Set
    End Property

    Public Property Tot_errs() As Nullable(Of Boolean)
        Get
            Return mTot_errs
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mTot_errs = value
        End Set
    End Property

    Public Property Tot_atol() As Nullable(Of Boolean)
        Get
            Return mTot_atol
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mTot_atol = value
        End Set
    End Property

    Public Property Tot_atolx() As String
        Get
            Return mTot_atolx
        End Get
        Set(ByVal value As String)
            mTot_atolx = value
        End Set
    End Property

    Public Property Tot_cref1() As String
        Get
            Return mTot_cref1
        End Get
        Set(ByVal value As String)
            mTot_cref1 = value
        End Set
    End Property

    Public Property Tot_cref2() As String
        Get
            Return mTot_cref2
        End Get
        Set(ByVal value As String)
            mTot_cref2 = value
        End Set
    End Property

    Public Property Tot_agcomm() As Decimal
        Get
            Return mTot_agcomm
        End Get
        Set(ByVal value As Decimal)
            mTot_agcomm = value
        End Set
    End Property

    Public Property Tot_agvat() As Decimal
        Get
            Return mTot_agvat
        End Get
        Set(ByVal value As Decimal)
            mTot_agvat = value
        End Set
    End Property

    Public Property Tot_fileno() As String
        Get
            Return mTot_fileno
        End Get
        Set(ByVal value As String)
            mTot_fileno = value
        End Set
    End Property

    Public Property Tot_nofee() As Nullable(Of Boolean)
        Get
            Return mTot_nofee
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mTot_nofee = value
        End Set
    End Property

    Public Property Tot_crstyp() As String
        Get
            Return mTot_crstyp
        End Get
        Set(ByVal value As String)
            mTot_crstyp = value
        End Set
    End Property

    Public Property Tot_noprnt() As Nullable(Of Boolean)
        Get
            Return mTot_noprnt
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mTot_noprnt = value
        End Set
    End Property

    Public Property Tot_email() As String
        Get
            Return mTot_email
        End Get
        Set(ByVal value As String)
            mTot_email = value
        End Set
    End Property

    Public Property Tot_curncy() As String
        Get
            Return mTot_curncy
        End Get
        Set(ByVal value As String)
            mTot_curncy = value
        End Set
    End Property

    Public Property Tot_roe() As Decimal
        Get
            Return mTot_roe
        End Get
        Set(ByVal value As Decimal)
            mTot_roe = value
        End Set
    End Property

    Public Property Tot_cursym() As String
        Get
            Return mTot_cursym
        End Get
        Set(ByVal value As String)
            mTot_cursym = value
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

    Private Shared Function makeBOSSinvtotCustomer( _
             ByVal r As IDataReader _
         ) As BOSSinvtot
        Return New BOSSinvtot( _
                clsUseful.notString(r.Item("customerid")))
    End Function

    Private Shared Function makeBOSSinvtotFromRow( _
            ByVal r As IDataReader _
        ) As BOSSinvtot
        Return New BOSSinvtot( _
                clsUseful.notInteger(r.Item("invtotID")), _
                clsUseful.notString(r.Item("tot_ukey")), _
                clsUseful.notString(r.Item("tot_custid")), _
                clsUseful.notString(r.Item("tot_invno")), _
                clsUseful.notInteger(r.Item("tot_change")), _
                clsUseful.notString(r.Item("tot_crsref")), _
                clsUseful.notString(r.Item("tot_br")), _
                clsUseful.notString(r.Item("tot_pono")), _
                clsUseful.notString(r.Item("tot_costc")), _
                clsUseful.notString(r.Item("tot_type")), _
                CDate(r.Item("tot_invdt")), _
                CDate(r.Item("tot_duedt")), _
                clsUseful.notDecimal(r.Item("tot_fare")), _
                clsUseful.notDecimal(r.Item("tot_tax")), _
                clsUseful.notDecimal(r.Item("tot_srvchg")), _
                clsUseful.notDecimal(r.Item("tot_ourchg")), _
                clsUseful.notDecimal(r.Item("tot_ourvat")), _
                clsUseful.notDecimal(r.Item("tot_supvat")), _
                clsUseful.notDecimal(r.Item("tot_amtvat")), _
                clsUseful.notDecimal(r.Item("tot_bilvat")), _
                clsUseful.notDecimal(r.Item("tot_amount")), _
                clsUseful.notDecimal(r.Item("tot_billed")), _
                clsUseful.notDecimal(r.Item("tot_discnt")), _
                clsUseful.notDecimal(r.Item("tot_ccamt")), _
                clsUseful.notDecimal(r.Item("tot_recvd")), _
                clsUseful.notDecimal(r.Item("tot_dposit")), _
                clsUseful.notDecimal(r.Item("tot_comm")), _
                clsUseful.notDecimal(r.Item("tot_comdue")), _
                clsUseful.notDecimal(r.Item("tot_vtoncm")), _
                clsUseful.notBoolean(r.Item("tot_disput")), _
                clsUseful.notString(r.Item("tot_reason")), _
                clsUseful.notBoolean(r.Item("tot_noerrs")), _
                clsUseful.notString(r.Item("tot_note")), _
                clsUseful.notString(r.Item("tot_raddr")), _
                clsUseful.notString(r.Item("tot_rtelno")), _
                clsUseful.notInteger(r.Item("tot_paxs")), _
                clsUseful.notString(r.Item("tot_morcrs")), _
                clsUseful.notBoolean(r.Item("tot_retail")), _
                clsUseful.notString(r.Item("tot_bdm")), _
                clsUseful.notInteger(r.Item("tot_print")), _
                clsUseful.notBoolean(r.Item("tot_errs")), _
                clsUseful.notBoolean(r.Item("tot_atol")), _
                clsUseful.notString(r.Item("tot_atolx")), _
                clsUseful.notString(r.Item("tot_cref1")), _
                clsUseful.notString(r.Item("tot_cref2")), _
                clsUseful.notDecimal(r.Item("tot_agcomm")), _
                clsUseful.notDecimal(r.Item("tot_agvat")), _
                clsUseful.notString(r.Item("tot_fileno")), _
                clsUseful.notBoolean(r.Item("tot_nofee")), _
                clsUseful.notString(r.Item("tot_crstyp")), _
                clsUseful.notBoolean(r.Item("tot_noprnt")), _
                clsUseful.notString(r.Item("tot_email")), _
                clsUseful.notString(r.Item("tot_curncy")), _
                clsUseful.notDecimal(r.Item("tot_roe")), _
                clsUseful.notString(r.Item("tot_cursym")), _
                CDate(r.Item("datecreated")))
    End Function

    Public Shared Function [get]( _
            ByVal pInvtotID As Integer _
        ) As BOSSinvtot
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Using r As IDataReader = dbh.callSP("BOSSinvtot_get", "@invtotID", pInvtotID)
                If Not r.Read() Then
                    Throw New Exception("No BOSSinvtot with id " & pInvtotID)
                End If
                Dim ret As BOSSinvtot = makeBOSSinvtotFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of BOSSinvtot)
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim ret As New List(Of BOSSinvtot)()
            Using r As IDataReader = dbh.callSP("BOSSinvtot_list")
                While r.Read()
                    ret.Add(makeBOSSinvtotFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            mInvtotID = CInt(dbh.callSPSingleValue("BOSSinvtot_save", "@InvtotID", mInvtotID, "@Tot_ukey", mTot_ukey, "@Tot_custid", mTot_custid, _
                                                   "@Tot_invno", mTot_invno, "@Tot_change", mTot_change, "@Tot_crsref", mTot_crsref, "@Tot_br", _
                                                   mTot_br, "@Tot_pono", mTot_pono, "@Tot_costc", mTot_costc, "@Tot_type", mTot_type, "@Tot_invdt", _
                                                   mTot_invdt, "@Tot_duedt", mTot_duedt, "@Tot_fare", mTot_fare, "@Tot_tax", mTot_tax, "@Tot_srvchg", _
                                                   mTot_srvchg, "@Tot_ourchg", mTot_ourchg, "@Tot_ourvat", mTot_ourvat, "@Tot_supvat", mTot_supvat, _
                                                   "@Tot_amtvat", mTot_amtvat, "@Tot_bilvat", mTot_bilvat, "@Tot_amount", mTot_amount, "@Tot_billed", _
                                                   mTot_billed, "@Tot_discnt", mTot_discnt, "@Tot_ccamt", mTot_ccamt, "@Tot_recvd", mTot_recvd, "@Tot_dposit", _
                                                   mTot_dposit, "@Tot_comm", mTot_comm, "@Tot_comdue", mTot_comdue, "@Tot_vtoncm", mTot_vtoncm, "@Tot_disput", _
                                                   mTot_disput, "@Tot_reason", mTot_reason, "@Tot_noerrs", mTot_noerrs, "@Tot_note", mTot_note, "@Tot_raddr", _
                                                   mTot_raddr, "@Tot_rtelno", mTot_rtelno, "@Tot_paxs", mTot_paxs, "@Tot_morcrs", mTot_morcrs, "@Tot_retail", _
                                                   mTot_retail, "@Tot_bdm", mTot_bdm, "@Tot_print", mTot_print, "@Tot_errs", mTot_errs, "@Tot_atol", mTot_atol, _
                                                   "@Tot_atolx", mTot_atolx, "@Tot_cref1", mTot_cref1, "@Tot_cref2", mTot_cref2, "@Tot_agcomm", mTot_agcomm, _
                                                   "@Tot_agvat", mTot_agvat, "@Tot_fileno", mTot_fileno, "@Tot_nofee", mTot_nofee, "@Tot_crstyp", mTot_crstyp, _
                                                   "@Tot_noprnt", mTot_noprnt, "@Tot_email", mTot_email, "@Tot_curncy", mTot_curncy, "@Tot_roe", mTot_roe, _
                                                   "@Tot_cursym", mTot_cursym, "@Datecreated", mDatecreated))
            Return mInvtotID
        End Using
    End Function

    Public Shared Sub delete(ByVal pInvtotID As Integer, ByVal pInvRef As String)
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            dbh.callNonQuerySP("BOSSinvtot_delete", "@InvtotID", pInvtotID, "@Tot_invno", pInvRef)
        End Using
    End Sub

    Public Shared Sub saveRecvd(ByVal pUkey As String, ByVal pInvRef As String, ByVal pTot_recvd As Decimal)
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            dbh.callNonQuerySP("BOSSInvtot_saveRecvd", "@Tot_ukey", pUkey, "@Tot_invno", pInvRef, "@Tot_recvd", pTot_recvd)
        End Using
    End Sub

    Public Shared Function listCustomer() As List(Of BOSSinvtot)
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim ret As New List(Of BOSSinvtot)()
            Using r As IDataReader = dbh.callSP("BOSSinvtotCustomer_get")
                While r.Read()
                    ret.Add(makeBOSSinvtotCustomer(r))
                End While
            End Using
            Return ret
        End Using
    End Function
End Class
