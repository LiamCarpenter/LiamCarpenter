Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSSpayhdr

    Public Sub New( _
        ByVal pPayhdrID As Integer, _
        ByVal pPyh_key As String, _
        ByVal pPyh_type As String, _
        ByVal pPyh_cdate As Nullable(Of DateTime), _
        ByVal pPyh_date As Nullable(Of DateTime), _
        ByVal pPyh_bnkac As String, _
        ByVal pPyh_bnklgr As String, _
        ByVal pPyh_bnkref As String, _
        ByVal pPyh_branch As String, _
        ByVal pPyh_payee As String, _
        ByVal pPyh_suptyp As String, _
        ByVal pPyh_paynam As String, _
        ByVal pPyh_net As Decimal, _
        ByVal pPyh_vat As Decimal, _
        ByVal pPyh_bnkchg As Decimal, _
        ByVal pPyh_amt As Decimal, _
        ByVal pPyh_used As Decimal, _
        ByVal pPyh_remain As Decimal, _
        ByVal pPyh_who As String, _
        ByVal pPyh_recon As Nullable(Of Boolean), _
        ByVal pPyh_recdte As Nullable(Of DateTime), _
        ByVal pPyh_note As String, _
        ByVal pPyh_locked As Nullable(Of Boolean), _
        ByVal pPyh_recur As Nullable(Of Boolean), _
        ByVal pPyh_reckey As String, _
        ByVal pPyh_freq As String, _
        ByVal pPyh_rstart As Nullable(Of DateTime), _
        ByVal pPyh_howmny As Decimal)
        mPayhdrID = pPayhdrID
        mPyh_key = pPyh_key
        mPyh_type = pPyh_type
        mPyh_cdate = pPyh_cdate
        mPyh_date = pPyh_date
        mPyh_bnkac = pPyh_bnkac
        mPyh_bnklgr = pPyh_bnklgr
        mPyh_bnkref = pPyh_bnkref
        mPyh_branch = pPyh_branch
        mPyh_payee = pPyh_payee
        mPyh_suptyp = pPyh_suptyp
        mPyh_paynam = pPyh_paynam
        mPyh_net = pPyh_net
        mPyh_vat = pPyh_vat
        mPyh_bnkchg = pPyh_bnkchg
        mPyh_amt = pPyh_amt
        mPyh_used = pPyh_used
        mPyh_remain = pPyh_remain
        mPyh_who = pPyh_who
        mPyh_recon = pPyh_recon
        mPyh_recdte = pPyh_recdte
        mPyh_note = pPyh_note
        mPyh_locked = pPyh_locked
        mPyh_recur = pPyh_recur
        mPyh_reckey = pPyh_reckey
        mPyh_freq = pPyh_freq
        mPyh_rstart = pPyh_rstart
        mPyh_howmny = pPyh_howmny
    End Sub

    Public Sub New( _
)
    End Sub

    Private mPayhdrID As Integer
    Private mPyh_key As String
    Private mPyh_type As String
    Private mPyh_cdate As Nullable(Of DateTime)
    Private mPyh_date As Nullable(Of DateTime)
    Private mPyh_bnkac As String
    Private mPyh_bnklgr As String
    Private mPyh_bnkref As String
    Private mPyh_branch As String
    Private mPyh_payee As String
    Private mPyh_suptyp As String
    Private mPyh_paynam As String
    Private mPyh_net As Decimal
    Private mPyh_vat As Decimal
    Private mPyh_bnkchg As Decimal
    Private mPyh_amt As Decimal
    Private mPyh_used As Decimal
    Private mPyh_remain As Decimal
    Private mPyh_who As String
    Private mPyh_recon As Nullable(Of Boolean)
    Private mPyh_recdte As Nullable(Of DateTime)
    Private mPyh_note As String
    Private mPyh_locked As Nullable(Of Boolean)
    Private mPyh_recur As Nullable(Of Boolean)
    Private mPyh_reckey As String
    Private mPyh_freq As String
    Private mPyh_rstart As Nullable(Of DateTime)
    Private mPyh_howmny As Decimal

    Public Property PayhdrID() As Integer
        Get
            Return mPayhdrID
        End Get
        Set(ByVal value As Integer)
            mPayhdrID = value
        End Set
    End Property

    Public Property Pyh_key() As String
        Get
            Return mPyh_key
        End Get
        Set(ByVal value As String)
            mPyh_key = value
        End Set
    End Property

    Public Property Pyh_type() As String
        Get
            Return mPyh_type
        End Get
        Set(ByVal value As String)
            mPyh_type = value
        End Set
    End Property

    Public Property Pyh_cdate() As Nullable(Of DateTime)
        Get
            Return mPyh_cdate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mPyh_cdate = value
        End Set
    End Property

    Public Property Pyh_date() As Nullable(Of DateTime)
        Get
            Return mPyh_date
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mPyh_date = value
        End Set
    End Property

    Public Property Pyh_bnkac() As String
        Get
            Return mPyh_bnkac
        End Get
        Set(ByVal value As String)
            mPyh_bnkac = value
        End Set
    End Property

    Public Property Pyh_bnklgr() As String
        Get
            Return mPyh_bnklgr
        End Get
        Set(ByVal value As String)
            mPyh_bnklgr = value
        End Set
    End Property

    Public Property Pyh_bnkref() As String
        Get
            Return mPyh_bnkref
        End Get
        Set(ByVal value As String)
            mPyh_bnkref = value
        End Set
    End Property

    Public Property Pyh_branch() As String
        Get
            Return mPyh_branch
        End Get
        Set(ByVal value As String)
            mPyh_branch = value
        End Set
    End Property

    Public Property Pyh_payee() As String
        Get
            Return mPyh_payee
        End Get
        Set(ByVal value As String)
            mPyh_payee = value
        End Set
    End Property

    Public Property Pyh_suptyp() As String
        Get
            Return mPyh_suptyp
        End Get
        Set(ByVal value As String)
            mPyh_suptyp = value
        End Set
    End Property

    Public Property Pyh_paynam() As String
        Get
            Return mPyh_paynam
        End Get
        Set(ByVal value As String)
            mPyh_paynam = value
        End Set
    End Property

    Public Property Pyh_net() As Decimal
        Get
            Return mPyh_net
        End Get
        Set(ByVal value As Decimal)
            mPyh_net = value
        End Set
    End Property

    Public Property Pyh_vat() As Decimal
        Get
            Return mPyh_vat
        End Get
        Set(ByVal value As Decimal)
            mPyh_vat = value
        End Set
    End Property

    Public Property Pyh_bnkchg() As Decimal
        Get
            Return mPyh_bnkchg
        End Get
        Set(ByVal value As Decimal)
            mPyh_bnkchg = value
        End Set
    End Property

    Public Property Pyh_amt() As Decimal
        Get
            Return mPyh_amt
        End Get
        Set(ByVal value As Decimal)
            mPyh_amt = value
        End Set
    End Property

    Public Property Pyh_used() As Decimal
        Get
            Return mPyh_used
        End Get
        Set(ByVal value As Decimal)
            mPyh_used = value
        End Set
    End Property

    Public Property Pyh_remain() As Decimal
        Get
            Return mPyh_remain
        End Get
        Set(ByVal value As Decimal)
            mPyh_remain = value
        End Set
    End Property

    Public Property Pyh_who() As String
        Get
            Return mPyh_who
        End Get
        Set(ByVal value As String)
            mPyh_who = value
        End Set
    End Property

    Public Property Pyh_recon() As Nullable(Of Boolean)
        Get
            Return mPyh_recon
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mPyh_recon = value
        End Set
    End Property

    Public Property Pyh_recdte() As Nullable(Of DateTime)
        Get
            Return mPyh_recdte
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mPyh_recdte = value
        End Set
    End Property

    Public Property Pyh_note() As String
        Get
            Return mPyh_note
        End Get
        Set(ByVal value As String)
            mPyh_note = value
        End Set
    End Property

    Public Property Pyh_locked() As Nullable(Of Boolean)
        Get
            Return mPyh_locked
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mPyh_locked = value
        End Set
    End Property

    Public Property Pyh_recur() As Nullable(Of Boolean)
        Get
            Return mPyh_recur
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mPyh_recur = value
        End Set
    End Property

    Public Property Pyh_reckey() As String
        Get
            Return mPyh_reckey
        End Get
        Set(ByVal value As String)
            mPyh_reckey = value
        End Set
    End Property

    Public Property Pyh_freq() As String
        Get
            Return mPyh_freq
        End Get
        Set(ByVal value As String)
            mPyh_freq = value
        End Set
    End Property

    Public Property Pyh_rstart() As Nullable(Of DateTime)
        Get
            Return mPyh_rstart
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mPyh_rstart = value
        End Set
    End Property

    Public Property Pyh_howmny() As Decimal
        Get
            Return mPyh_howmny
        End Get
        Set(ByVal value As Decimal)
            mPyh_howmny = value
        End Set
    End Property

    Private Shared Function makeBOSSpayhdrFromRow( _
            ByVal r As IDataReader _
        ) As BOSSpayhdr
        Return New BOSSpayhdr( _
                clsUseful.notInteger(r.Item("payhdrID")), _
                clsUseful.notString(r.Item("pyh_key")), _
                clsUseful.notString(r.Item("pyh_type")), _
                toNullableDate(r.Item("pyh_cdate")), _
                toNullableDate(r.Item("pyh_date")), _
                clsUseful.notString(r.Item("pyh_bnkac")), _
                clsUseful.notString(r.Item("pyh_bnklgr")), _
                clsUseful.notString(r.Item("pyh_bnkref")), _
                clsUseful.notString(r.Item("pyh_branch")), _
                clsUseful.notString(r.Item("pyh_payee")), _
                clsUseful.notString(r.Item("pyh_suptyp")), _
                clsUseful.notString(r.Item("pyh_paynam")), _
                toNullableFloat(r.Item("pyh_net")), _
                toNullableFloat(r.Item("pyh_vat")), _
                toNullableFloat(r.Item("pyh_bnkchg")), _
                toNullableFloat(r.Item("pyh_amt")), _
                toNullableFloat(r.Item("pyh_used")), _
                toNullableFloat(r.Item("pyh_remain")), _
                clsUseful.notString(r.Item("pyh_who")), _
                toNullableBoolean(r.Item("pyh_recon")), _
                toNullableDate(r.Item("pyh_recdte")), _
                clsUseful.notString(r.Item("pyh_note")), _
                toNullableBoolean(r.Item("pyh_locked")), _
                toNullableBoolean(r.Item("pyh_recur")), _
                clsUseful.notString(r.Item("pyh_reckey")), _
                clsUseful.notString(r.Item("pyh_freq")), _
                toNullableDate(r.Item("pyh_rstart")), _
                toNullableFloat(r.Item("pyh_howmny")))
    End Function

    Public Function save() As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim strRet As String = clsUseful.notString(dbh.callSPSingleValueCanReturnNothing("BOSSpayhdr_save", "@PayhdrID", mPayhdrID, "@Pyh_key", mPyh_key, "@Pyh_type", mPyh_type, _
                                                                                             "@Pyh_cdate", mPyh_cdate, "@Pyh_date", mPyh_date, "@Pyh_bnkac", mPyh_bnkac, "@Pyh_bnklgr", mPyh_bnklgr, _
                                                                                             "@Pyh_bnkref", mPyh_bnkref, "@Pyh_branch", mPyh_branch, "@Pyh_payee", mPyh_payee, "@Pyh_suptyp", mPyh_suptyp, _
                                                                                             "@Pyh_paynam", mPyh_paynam, "@Pyh_net", mPyh_net, "@Pyh_vat", mPyh_vat, "@Pyh_bnkchg", mPyh_bnkchg, _
                                                                                             "@Pyh_amt", mPyh_amt, "@Pyh_used", mPyh_used, "@Pyh_remain", mPyh_remain, "@Pyh_who", mPyh_who, _
                                                                                             "@Pyh_recon", mPyh_recon, "@Pyh_recdte", mPyh_recdte, "@Pyh_note", mPyh_note, "@Pyh_locked", _
                                                                                             mPyh_locked, "@Pyh_recur", mPyh_recur, "@Pyh_reckey", mPyh_reckey, "@Pyh_freq", mPyh_freq, _
                                                                                             "@Pyh_rstart", mPyh_rstart, "@Pyh_howmny", mPyh_howmny))
            Return strRet
        End Using
    End Function

End Class
