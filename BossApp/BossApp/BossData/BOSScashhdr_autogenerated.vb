Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSScashhdr

    Public Sub New( _
        ByVal pCashhdrid As Integer, _
        ByVal pCsh_key As String, _
        ByVal pCsh_br As String, _
        ByVal pCsh_bnkdte As Nullable(Of DateTime), _
        ByVal pCsh_batch As String, _
        ByVal pCsh_type As String, _
        ByVal pCsh_from As String, _
        ByVal pCsh_id As String, _
        ByVal pCsh_ref As String, _
        ByVal pCsh_fop As Decimal, _
        ByVal pCsh_vrfnd As Nullable(Of Boolean), _
        ByVal pCsh_rfnd As Nullable(Of Boolean), _
        ByVal pCsh_prod As String, _
        ByVal pCsh_bank As String, _
        ByVal pCsh_curncy As String, _
        ByVal pCsh_amount As Decimal, _
        ByVal pCsh_used As Decimal, _
        ByVal pCsh_remain As Decimal, _
        ByVal pCsh_atol As Decimal, _
        ByVal pCsh_rdate As Nullable(Of DateTime), _
        ByVal pCsh_note As String, _
        ByVal pCsh_locked As Nullable(Of Boolean), _
        ByVal pCsh_recon As Nullable(Of Boolean), _
        ByVal pCsh_recdte As Nullable(Of DateTime), _
        ByVal pCsh_status As String, _
        ByVal pCsh_who As String, _
        ByVal pCsh_cctxn As String, _
        ByVal pCsh_ccid As String, _
        ByVal pCsh_ccno As String, _
        ByVal pCsh_ccauth As String, _
        ByVal pCsh_ccexp As String, _
        ByVal pCsh_issue As Decimal, _
        ByVal pCsh_merch As Nullable(Of Boolean), _
        ByVal pCsh_ccxmit As Nullable(Of DateTime), _
        ByVal pCsh_cshamt As Decimal, _
        ByVal pCsh_ccamt As Decimal, _
        ByVal pCsh_chqamt As Decimal, _
        ByVal pCsh_vchamt As Decimal, _
        ByVal pCsh_othamt As Decimal, _
        ByVal pCsh_ccchg As Decimal, _
        ByVal pCsh_mfee As Decimal, _
        ByVal pCsh_bal As Decimal, _
        ByVal pCsh_chqgte As String, _
        ByVal pCsh_chqno As String, _
        ByVal pCsh_vchid As String, _
        ByVal pCsh_vchref As String, _
        ByVal pCsh_vchnte As String, _
        ByVal pCsh_othid As String, _
        ByVal pCsh_othref As String, _
        ByVal pCsh_othnte As String, _
        ByVal pChksum As String)
        mCashhdrid = pCashhdrid
        mCsh_key = pCsh_key
        mCsh_br = pCsh_br
        mCsh_bnkdte = pCsh_bnkdte
        mCsh_batch = pCsh_batch
        mCsh_type = pCsh_type
        mCsh_from = pCsh_from
        mCsh_id = pCsh_id
        mCsh_ref = pCsh_ref
        mCsh_fop = pCsh_fop
        mCsh_vrfnd = pCsh_vrfnd
        mCsh_rfnd = pCsh_rfnd
        mCsh_prod = pCsh_prod
        mCsh_bank = pCsh_bank
        mCsh_curncy = pCsh_curncy
        mCsh_amount = pCsh_amount
        mCsh_used = pCsh_used
        mCsh_remain = pCsh_remain
        mCsh_atol = pCsh_atol
        mCsh_rdate = pCsh_rdate
        mCsh_note = pCsh_note
        mCsh_locked = pCsh_locked
        mCsh_recon = pCsh_recon
        mCsh_recdte = pCsh_recdte
        mCsh_status = pCsh_status
        mCsh_who = pCsh_who
        mCsh_cctxn = pCsh_cctxn
        mCsh_ccid = pCsh_ccid
        mCsh_ccno = pCsh_ccno
        mCsh_ccauth = pCsh_ccauth
        mCsh_ccexp = pCsh_ccexp
        mCsh_issue = pCsh_issue
        mCsh_merch = pCsh_merch
        mCsh_ccxmit = pCsh_ccxmit
        mCsh_cshamt = pCsh_cshamt
        mCsh_ccamt = pCsh_ccamt
        mCsh_chqamt = pCsh_chqamt
        mCsh_vchamt = pCsh_vchamt
        mCsh_othamt = pCsh_othamt
        mCsh_ccchg = pCsh_ccchg
        mCsh_mfee = pCsh_mfee
        mCsh_bal = pCsh_bal
        mCsh_chqgte = pCsh_chqgte
        mCsh_chqno = pCsh_chqno
        mCsh_vchid = pCsh_vchid
        mCsh_vchref = pCsh_vchref
        mCsh_vchnte = pCsh_vchnte
        mCsh_othid = pCsh_othid
        mCsh_othref = pCsh_othref
        mCsh_othnte = pCsh_othnte
        mChksum = pChksum
    End Sub

    Public Sub New( _
)
    End Sub

    Private mCashhdrid As Integer
    Private mCsh_key As String
    Private mCsh_br As String
    Private mCsh_bnkdte As Nullable(Of DateTime)
    Private mCsh_batch As String
    Private mCsh_type As String
    Private mCsh_from As String
    Private mCsh_id As String
    Private mCsh_ref As String
    Private mCsh_fop As Decimal
    Private mCsh_vrfnd As Nullable(Of Boolean)
    Private mCsh_rfnd As Nullable(Of Boolean)
    Private mCsh_prod As String
    Private mCsh_bank As String
    Private mCsh_curncy As String
    Private mCsh_amount As Decimal
    Private mCsh_used As Decimal
    Private mCsh_remain As Decimal
    Private mCsh_atol As Decimal
    Private mCsh_rdate As Nullable(Of DateTime)
    Private mCsh_note As String
    Private mCsh_locked As Nullable(Of Boolean)
    Private mCsh_recon As Nullable(Of Boolean)
    Private mCsh_recdte As Nullable(Of DateTime)
    Private mCsh_status As String
    Private mCsh_who As String
    Private mCsh_cctxn As String
    Private mCsh_ccid As String
    Private mCsh_ccno As String
    Private mCsh_ccauth As String
    Private mCsh_ccexp As String
    Private mCsh_issue As Decimal
    Private mCsh_merch As Nullable(Of Boolean)
    Private mCsh_ccxmit As Nullable(Of DateTime)
    Private mCsh_cshamt As Decimal
    Private mCsh_ccamt As Decimal
    Private mCsh_chqamt As Decimal
    Private mCsh_vchamt As Decimal
    Private mCsh_othamt As Decimal
    Private mCsh_ccchg As Decimal
    Private mCsh_mfee As Decimal
    Private mCsh_bal As Decimal
    Private mCsh_chqgte As String
    Private mCsh_chqno As String
    Private mCsh_vchid As String
    Private mCsh_vchref As String
    Private mCsh_vchnte As String
    Private mCsh_othid As String
    Private mCsh_othref As String
    Private mCsh_othnte As String
    Private mChksum As String

    Public Property Cashhdrid() As Integer
        Get
            Return mCashhdrid
        End Get
        Set(ByVal value As Integer)
            mCashhdrid = value
        End Set
    End Property

    Public Property Csh_key() As String
        Get
            Return mCsh_key
        End Get
        Set(ByVal value As String)
            mCsh_key = value
        End Set
    End Property

    Public Property Csh_br() As String
        Get
            Return mCsh_br
        End Get
        Set(ByVal value As String)
            mCsh_br = value
        End Set
    End Property

    Public Property Csh_bnkdte() As Nullable(Of DateTime)
        Get
            Return mCsh_bnkdte
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mCsh_bnkdte = value
        End Set
    End Property

    Public Property Csh_batch() As String
        Get
            Return mCsh_batch
        End Get
        Set(ByVal value As String)
            mCsh_batch = value
        End Set
    End Property

    Public Property Csh_type() As String
        Get
            Return mCsh_type
        End Get
        Set(ByVal value As String)
            mCsh_type = value
        End Set
    End Property

    Public Property Csh_from() As String
        Get
            Return mCsh_from
        End Get
        Set(ByVal value As String)
            mCsh_from = value
        End Set
    End Property

    Public Property Csh_id() As String
        Get
            Return mCsh_id
        End Get
        Set(ByVal value As String)
            mCsh_id = value
        End Set
    End Property

    Public Property Csh_ref() As String
        Get
            Return mCsh_ref
        End Get
        Set(ByVal value As String)
            mCsh_ref = value
        End Set
    End Property

    Public Property Csh_fop() As Decimal
        Get
            Return mCsh_fop
        End Get
        Set(ByVal value As Decimal)
            mCsh_fop = value
        End Set
    End Property

    Public Property Csh_vrfnd() As Nullable(Of Boolean)
        Get
            Return mCsh_vrfnd
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mCsh_vrfnd = value
        End Set
    End Property

    Public Property Csh_rfnd() As Nullable(Of Boolean)
        Get
            Return mCsh_rfnd
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mCsh_rfnd = value
        End Set
    End Property

    Public Property Csh_prod() As String
        Get
            Return mCsh_prod
        End Get
        Set(ByVal value As String)
            mCsh_prod = value
        End Set
    End Property

    Public Property Csh_bank() As String
        Get
            Return mCsh_bank
        End Get
        Set(ByVal value As String)
            mCsh_bank = value
        End Set
    End Property

    Public Property Csh_curncy() As String
        Get
            Return mCsh_curncy
        End Get
        Set(ByVal value As String)
            mCsh_curncy = value
        End Set
    End Property

    Public Property Csh_amount() As Decimal
        Get
            Return mCsh_amount
        End Get
        Set(ByVal value As Decimal)
            mCsh_amount = value
        End Set
    End Property

    Public Property Csh_used() As Decimal
        Get
            Return mCsh_used
        End Get
        Set(ByVal value As Decimal)
            mCsh_used = value
        End Set
    End Property

    Public Property Csh_remain() As Decimal
        Get
            Return mCsh_remain
        End Get
        Set(ByVal value As Decimal)
            mCsh_remain = value
        End Set
    End Property

    Public Property Csh_atol() As Decimal
        Get
            Return mCsh_atol
        End Get
        Set(ByVal value As Decimal)
            mCsh_atol = value
        End Set
    End Property

    Public Property Csh_rdate() As Nullable(Of DateTime)
        Get
            Return mCsh_rdate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mCsh_rdate = value
        End Set
    End Property

    Public Property Csh_note() As String
        Get
            Return mCsh_note
        End Get
        Set(ByVal value As String)
            mCsh_note = value
        End Set
    End Property

    Public Property Csh_locked() As Nullable(Of Boolean)
        Get
            Return mCsh_locked
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mCsh_locked = value
        End Set
    End Property

    Public Property Csh_recon() As Nullable(Of Boolean)
        Get
            Return mCsh_recon
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mCsh_recon = value
        End Set
    End Property

    Public Property Csh_recdte() As Nullable(Of DateTime)
        Get
            Return mCsh_recdte
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mCsh_recdte = value
        End Set
    End Property

    Public Property Csh_status() As String
        Get
            Return mCsh_status
        End Get
        Set(ByVal value As String)
            mCsh_status = value
        End Set
    End Property

    Public Property Csh_who() As String
        Get
            Return mCsh_who
        End Get
        Set(ByVal value As String)
            mCsh_who = value
        End Set
    End Property

    Public Property Csh_cctxn() As String
        Get
            Return mCsh_cctxn
        End Get
        Set(ByVal value As String)
            mCsh_cctxn = value
        End Set
    End Property

    Public Property Csh_ccid() As String
        Get
            Return mCsh_ccid
        End Get
        Set(ByVal value As String)
            mCsh_ccid = value
        End Set
    End Property

    Public Property Csh_ccno() As String
        Get
            Return mCsh_ccno
        End Get
        Set(ByVal value As String)
            mCsh_ccno = value
        End Set
    End Property

    Public Property Csh_ccauth() As String
        Get
            Return mCsh_ccauth
        End Get
        Set(ByVal value As String)
            mCsh_ccauth = value
        End Set
    End Property

    Public Property Csh_ccexp() As String
        Get
            Return mCsh_ccexp
        End Get
        Set(ByVal value As String)
            mCsh_ccexp = value
        End Set
    End Property

    Public Property Csh_issue() As Decimal
        Get
            Return mCsh_issue
        End Get
        Set(ByVal value As Decimal)
            mCsh_issue = value
        End Set
    End Property

    Public Property Csh_merch() As Nullable(Of Boolean)
        Get
            Return mCsh_merch
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mCsh_merch = value
        End Set
    End Property

    Public Property Csh_ccxmit() As Nullable(Of DateTime)
        Get
            Return mCsh_ccxmit
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mCsh_ccxmit = value
        End Set
    End Property

    Public Property Csh_cshamt() As Decimal
        Get
            Return mCsh_cshamt
        End Get
        Set(ByVal value As Decimal)
            mCsh_cshamt = value
        End Set
    End Property

    Public Property Csh_ccamt() As Decimal
        Get
            Return mCsh_ccamt
        End Get
        Set(ByVal value As Decimal)
            mCsh_ccamt = value
        End Set
    End Property

    Public Property Csh_chqamt() As Decimal
        Get
            Return mCsh_chqamt
        End Get
        Set(ByVal value As Decimal)
            mCsh_chqamt = value
        End Set
    End Property

    Public Property Csh_vchamt() As Decimal
        Get
            Return mCsh_vchamt
        End Get
        Set(ByVal value As Decimal)
            mCsh_vchamt = value
        End Set
    End Property

    Public Property Csh_othamt() As Decimal
        Get
            Return mCsh_othamt
        End Get
        Set(ByVal value As Decimal)
            mCsh_othamt = value
        End Set
    End Property

    Public Property Csh_ccchg() As Decimal
        Get
            Return mCsh_ccchg
        End Get
        Set(ByVal value As Decimal)
            mCsh_ccchg = value
        End Set
    End Property

    Public Property Csh_mfee() As Decimal
        Get
            Return mCsh_mfee
        End Get
        Set(ByVal value As Decimal)
            mCsh_mfee = value
        End Set
    End Property

    Public Property Csh_bal() As Decimal
        Get
            Return mCsh_bal
        End Get
        Set(ByVal value As Decimal)
            mCsh_bal = value
        End Set
    End Property

    Public Property Csh_chqgte() As String
        Get
            Return mCsh_chqgte
        End Get
        Set(ByVal value As String)
            mCsh_chqgte = value
        End Set
    End Property

    Public Property Csh_chqno() As String
        Get
            Return mCsh_chqno
        End Get
        Set(ByVal value As String)
            mCsh_chqno = value
        End Set
    End Property

    Public Property Csh_vchid() As String
        Get
            Return mCsh_vchid
        End Get
        Set(ByVal value As String)
            mCsh_vchid = value
        End Set
    End Property

    Public Property Csh_vchref() As String
        Get
            Return mCsh_vchref
        End Get
        Set(ByVal value As String)
            mCsh_vchref = value
        End Set
    End Property

    Public Property Csh_vchnte() As String
        Get
            Return mCsh_vchnte
        End Get
        Set(ByVal value As String)
            mCsh_vchnte = value
        End Set
    End Property

    Public Property Csh_othid() As String
        Get
            Return mCsh_othid
        End Get
        Set(ByVal value As String)
            mCsh_othid = value
        End Set
    End Property

    Public Property Csh_othref() As String
        Get
            Return mCsh_othref
        End Get
        Set(ByVal value As String)
            mCsh_othref = value
        End Set
    End Property

    Public Property Csh_othnte() As String
        Get
            Return mCsh_othnte
        End Get
        Set(ByVal value As String)
            mCsh_othnte = value
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

    Private Shared Function makeBOSScashhdrFromRow( _
            ByVal r As IDataReader _
        ) As BOSScashhdr
        Return New BOSScashhdr( _
                clsUseful.notInteger(r.Item("cashhdrid")), _
                clsUseful.notString(r.Item("csh_key")), _
                clsUseful.notString(r.Item("csh_br")), _
                toNullableDate(r.Item("csh_bnkdte")), _
                clsUseful.notString(r.Item("csh_batch")), _
                clsUseful.notString(r.Item("csh_type")), _
                clsUseful.notString(r.Item("csh_from")), _
                clsUseful.notString(r.Item("csh_id")), _
                clsUseful.notString(r.Item("csh_ref")), _
                toNullableFloat(r.Item("csh_fop")), _
                toNullableBoolean(r.Item("csh_vrfnd")), _
                toNullableBoolean(r.Item("csh_rfnd")), _
                clsUseful.notString(r.Item("csh_prod")), _
                clsUseful.notString(r.Item("csh_bank")), _
                clsUseful.notString(r.Item("csh_curncy")), _
                toNullableFloat(r.Item("csh_amount")), _
                toNullableFloat(r.Item("csh_used")), _
                toNullableFloat(r.Item("csh_remain")), _
                toNullableFloat(r.Item("csh_atol")), _
                toNullableDate(r.Item("csh_rdate")), _
                clsUseful.notString(r.Item("csh_note")), _
                toNullableBoolean(r.Item("csh_locked")), _
                toNullableBoolean(r.Item("csh_recon")), _
                toNullableDate(r.Item("csh_recdte")), _
                clsUseful.notString(r.Item("csh_status")), _
                clsUseful.notString(r.Item("csh_who")), _
                clsUseful.notString(r.Item("csh_cctxn")), _
                clsUseful.notString(r.Item("csh_ccid")), _
                clsUseful.notString(r.Item("csh_ccno")), _
                clsUseful.notString(r.Item("csh_ccauth")), _
                clsUseful.notString(r.Item("csh_ccexp")), _
                toNullableFloat(r.Item("csh_issue")), _
                toNullableBoolean(r.Item("csh_merch")), _
                toNullableDate(r.Item("csh_ccxmit")), _
                toNullableFloat(r.Item("csh_cshamt")), _
                toNullableFloat(r.Item("csh_ccamt")), _
                toNullableFloat(r.Item("csh_chqamt")), _
                toNullableFloat(r.Item("csh_vchamt")), _
                toNullableFloat(r.Item("csh_othamt")), _
                toNullableFloat(r.Item("csh_ccchg")), _
                toNullableFloat(r.Item("csh_mfee")), _
                toNullableFloat(r.Item("csh_bal")), _
               clsUseful.notString(r.Item("csh_chqgte")), _
                clsUseful.notString(r.Item("csh_chqno")), _
                clsUseful.notString(r.Item("csh_vchid")), _
                clsUseful.notString(r.Item("csh_vchref")), _
                clsUseful.notString(r.Item("csh_vchnte")), _
                clsUseful.notString(r.Item("csh_othid")), _
                clsUseful.notString(r.Item("csh_othref")), _
                clsUseful.notString(r.Item("csh_othnte")), _
                clsUseful.notString(r.Item("chksum")))
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            mCashhdrid = CInt(dbh.callSPSingleValue("BOSScashhdr_save", _
                                                    "@Cashhdrid", mCashhdrid, _
                                                    "@Csh_key", mCsh_key, _
                                                    "@Csh_br", mCsh_br, _
                                                    "@Csh_bnkdte", mCsh_bnkdte, _
                                                    "@Csh_batch", mCsh_batch, _
                                                    "@Csh_type", mCsh_type, _
                                                    "@Csh_from", mCsh_from, _
                                                    "@Csh_id", mCsh_id, _
                                                    "@Csh_ref", mCsh_ref, _
                                                    "@Csh_fop", mCsh_fop, _
                                                    "@Csh_vrfnd", mCsh_vrfnd, _
                                                    "@Csh_rfnd", mCsh_rfnd, _
                                                    "@Csh_prod", mCsh_prod, _
                                                    "@Csh_bank", mCsh_bank, _
                                                    "@Csh_curncy", mCsh_curncy, _
                                                    "@Csh_amount", mCsh_amount, _
                                                    "@Csh_used", mCsh_used, _
                                                    "@Csh_remain", mCsh_remain, _
                                                    "@Csh_atol", mCsh_atol, _
                                                    "@Csh_rdate", mCsh_rdate, _
                                                    "@Csh_note", mCsh_note, _
                                                    "@Csh_locked", mCsh_locked, _
                                                    "@Csh_recon", mCsh_recon, _
                                                    "@Csh_recdte", mCsh_recdte, _
                                                    "@Csh_status", mCsh_status, _
                                                    "@Csh_who", mCsh_who, _
                                                    "@Csh_cctxn", mCsh_cctxn, _
                                                    "@Csh_ccid", mCsh_ccid, _
                                                    "@Csh_ccno", mCsh_ccno, _
                                                    "@Csh_ccauth", mCsh_ccauth, _
                                                    "@Csh_ccexp", mCsh_ccexp, _
                                                    "@Csh_issue", mCsh_issue, _
                                                    "@Csh_merch", mCsh_merch, _
                                                    "@Csh_ccxmit", mCsh_ccxmit, _
                                                    "@Csh_cshamt", mCsh_cshamt, _
                                                    "@Csh_ccamt", mCsh_ccamt, _
                                                    "@Csh_chqamt", mCsh_chqamt, _
                                                    "@Csh_vchamt", mCsh_vchamt, _
                                                    "@Csh_othamt", mCsh_othamt, _
                                                    "@Csh_ccchg", mCsh_ccchg, _
                                                    "@Csh_mfee", mCsh_mfee, _
                                                    "@Csh_bal", mCsh_bal, _
                                                    "@Csh_chqgte", mCsh_chqgte, _
                                                    "@Csh_chqno", mCsh_chqno, _
                                                    "@Csh_vchid", mCsh_vchid, _
                                                    "@Csh_vchref", mCsh_vchref, _
                                                    "@Csh_vchnte", mCsh_vchnte, _
                                                    "@Csh_othid", mCsh_othid, _
                                                    "@Csh_othref", mCsh_othref, _
                                                    "@Csh_othnte", mCsh_othnte, _
                                                    "@Chksum", mChksum))
            Return mCashhdrid
        End Using
    End Function

End Class
