Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSSinvroute

    Public Sub New( _
        ByVal pInvrouteID As Integer, _
        ByVal pInr_key As String, _
        ByVal pInr_invno As String, _
        ByVal pInr_prod As String, _
        ByVal pInr_bookno As Nullable(Of Integer), _
        ByVal pInr_segno As Nullable(Of Integer), _
        ByVal pInr_from As String, _
        ByVal pInr_to As String, _
        ByVal pInr_sttime As String, _
        ByVal pInr_start As Nullable(Of DateTime), _
        ByVal pInr_stterm As String, _
        ByVal pInr_etime As String, _
        ByVal pInr_end As Nullable(Of DateTime), _
        ByVal pInr_eterm As String, _
        ByVal pInr_flwncr As String, _
        ByVal pInr_flight As String, _
        ByVal pInr_class As String, _
        ByVal pInr_bpmt As Decimal, _
        ByVal pInr_fbasis As String, _
        ByVal pInr_fare As Decimal, _
        ByVal pInr_dest As Nullable(Of Boolean), _
        ByVal pInr_miles As Nullable(Of Integer), _
        ByVal pInr_fee As Decimal, _
        ByVal pInr_feevt As Decimal, _
        ByVal pInr_feebas As String, _
        ByVal pInr_status As String, _
        ByVal pDatecreated As Nullable(Of DateTime))
        mInvrouteID = pInvrouteID
        mInr_key = pInr_key
        mInr_invno = pInr_invno
        mInr_prod = pInr_prod
        mInr_bookno = pInr_bookno
        mInr_segno = pInr_segno
        mInr_from = pInr_from
        mInr_to = pInr_to
        mInr_sttime = pInr_sttime
        mInr_start = pInr_start
        mInr_stterm = pInr_stterm
        mInr_etime = pInr_etime
        mInr_end = pInr_end
        mInr_eterm = pInr_eterm
        mInr_flwncr = pInr_flwncr
        mInr_flight = pInr_flight
        mInr_class = pInr_class
        mInr_bpmt = pInr_bpmt
        mInr_fbasis = pInr_fbasis
        mInr_fare = pInr_fare
        mInr_dest = pInr_dest
        mInr_miles = pInr_miles
        mInr_fee = pInr_fee
        mInr_feevt = pInr_feevt
        mInr_feebas = pInr_feebas
        mInr_status = pInr_status
        mDatecreated = pDatecreated
    End Sub

    Public Sub New( _
)
    End Sub

    Private mInvrouteID As Integer
    Private mInr_key As String
    Private mInr_invno As String
    Private mInr_prod As String
    Private mInr_bookno As Nullable(Of Integer)
    Private mInr_segno As Nullable(Of Integer)
    Private mInr_from As String
    Private mInr_to As String
    Private mInr_sttime As String
    Private mInr_start As Nullable(Of DateTime)
    Private mInr_stterm As String
    Private mInr_etime As String
    Private mInr_end As Nullable(Of DateTime)
    Private mInr_eterm As String
    Private mInr_flwncr As String
    Private mInr_flight As String
    Private mInr_class As String
    Private mInr_bpmt As Decimal
    Private mInr_fbasis As String
    Private mInr_fare As Decimal
    Private mInr_dest As Nullable(Of Boolean)
    Private mInr_miles As Nullable(Of Integer)
    Private mInr_fee As Decimal
    Private mInr_feevt As Decimal
    Private mInr_feebas As String
    Private mInr_status As String
    Private mDatecreated As Nullable(Of DateTime)

    Public Property InvrouteID() As Integer
        Get
            Return mInvrouteID
        End Get
        Set(ByVal value As Integer)
            mInvrouteID = value
        End Set
    End Property

    Public Property Inr_key() As String
        Get
            Return mInr_key
        End Get
        Set(ByVal value As String)
            mInr_key = value
        End Set
    End Property

    Public Property Inr_invno() As String
        Get
            Return mInr_invno
        End Get
        Set(ByVal value As String)
            mInr_invno = value
        End Set
    End Property

    Public Property Inr_prod() As String
        Get
            Return mInr_prod
        End Get
        Set(ByVal value As String)
            mInr_prod = value
        End Set
    End Property

    Public Property Inr_bookno() As Nullable(Of Integer)
        Get
            Return mInr_bookno
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mInr_bookno = value
        End Set
    End Property

    Public Property Inr_segno() As Nullable(Of Integer)
        Get
            Return mInr_segno
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mInr_segno = value
        End Set
    End Property

    Public Property Inr_from() As String
        Get
            Return mInr_from
        End Get
        Set(ByVal value As String)
            mInr_from = value
        End Set
    End Property

    Public Property Inr_to() As String
        Get
            Return mInr_to
        End Get
        Set(ByVal value As String)
            mInr_to = value
        End Set
    End Property

    Public Property Inr_sttime() As String
        Get
            Return mInr_sttime
        End Get
        Set(ByVal value As String)
            mInr_sttime = value
        End Set
    End Property

    Public Property Inr_start() As Nullable(Of DateTime)
        Get
            Return mInr_start
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mInr_start = value
        End Set
    End Property

    Public Property Inr_stterm() As String
        Get
            Return mInr_stterm
        End Get
        Set(ByVal value As String)
            mInr_stterm = value
        End Set
    End Property

    Public Property Inr_etime() As String
        Get
            Return mInr_etime
        End Get
        Set(ByVal value As String)
            mInr_etime = value
        End Set
    End Property

    Public Property Inr_end() As Nullable(Of DateTime)
        Get
            Return mInr_end
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mInr_end = value
        End Set
    End Property

    Public Property Inr_eterm() As String
        Get
            Return mInr_eterm
        End Get
        Set(ByVal value As String)
            mInr_eterm = value
        End Set
    End Property

    Public Property Inr_flwncr() As String
        Get
            Return mInr_flwncr
        End Get
        Set(ByVal value As String)
            mInr_flwncr = value
        End Set
    End Property

    Public Property Inr_flight() As String
        Get
            Return mInr_flight
        End Get
        Set(ByVal value As String)
            mInr_flight = value
        End Set
    End Property

    Public Property Inr_class() As String
        Get
            Return mInr_class
        End Get
        Set(ByVal value As String)
            mInr_class = value
        End Set
    End Property

    Public Property Inr_bpmt() As Decimal
        Get
            Return mInr_bpmt
        End Get
        Set(ByVal value As Decimal)
            mInr_bpmt = value
        End Set
    End Property

    Public Property Inr_fbasis() As String
        Get
            Return mInr_fbasis
        End Get
        Set(ByVal value As String)
            mInr_fbasis = value
        End Set
    End Property

    Public Property Inr_fare() As Decimal
        Get
            Return mInr_fare
        End Get
        Set(ByVal value As Decimal)
            mInr_fare = value
        End Set
    End Property

    Public Property Inr_dest() As Nullable(Of Boolean)
        Get
            Return mInr_dest
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInr_dest = value
        End Set
    End Property

    Public Property Inr_miles() As Nullable(Of Integer)
        Get
            Return mInr_miles
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mInr_miles = value
        End Set
    End Property

    Public Property Inr_fee() As Decimal
        Get
            Return mInr_fee
        End Get
        Set(ByVal value As Decimal)
            mInr_fee = value
        End Set
    End Property

    Public Property Inr_feevt() As Decimal
        Get
            Return mInr_feevt
        End Get
        Set(ByVal value As Decimal)
            mInr_feevt = value
        End Set
    End Property

    Public Property Inr_feebas() As String
        Get
            Return mInr_feebas
        End Get
        Set(ByVal value As String)
            mInr_feebas = value
        End Set
    End Property

    Public Property Inr_status() As String
        Get
            Return mInr_status
        End Get
        Set(ByVal value As String)
            mInr_status = value
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

    Private Shared Function makeBOSSinvrouteFromRow( _
            ByVal r As IDataReader _
        ) As BOSSinvroute
        Return New BOSSinvroute( _
                clsUseful.notInteger(r.Item("invrouteID")), _
                clsUseful.notString(r.Item("inr_key")), _
                clsUseful.notString(r.Item("inr_invno")), _
                clsUseful.notString(r.Item("inr_prod")), _
                clsUseful.notInteger(r.Item("inr_bookno")), _
                clsUseful.notInteger(r.Item("inr_segno")), _
                clsUseful.notString(r.Item("inr_from")), _
                clsUseful.notString(r.Item("inr_to")), _
                clsUseful.notString(r.Item("inr_sttime")), _
                CDate(r.Item("inr_start")), _
                clsUseful.notString(r.Item("inr_stterm")), _
                clsUseful.notString(r.Item("inr_etime")), _
                CDate(r.Item("inr_end")), _
                clsUseful.notString(r.Item("inr_eterm")), _
                clsUseful.notString(r.Item("inr_flwncr")), _
                clsUseful.notString(r.Item("inr_flight")), _
                clsUseful.notString(r.Item("inr_class")), _
                clsUseful.notDecimal(r.Item("inr_bpmt")), _
                clsUseful.notString(r.Item("inr_fbasis")), _
                clsUseful.notDecimal(r.Item("inr_fare")), _
                clsUseful.notBoolean(r.Item("inr_dest")), _
                clsUseful.notInteger(r.Item("inr_miles")), _
                clsUseful.notDecimal(r.Item("inr_fee")), _
                clsUseful.notDecimal(r.Item("inr_feevt")), _
                clsUseful.notString(r.Item("inr_feebas")), _
                clsUseful.notString(r.Item("inr_status")), _
                CDate(r.Item("datecreated")))
    End Function

    Public Shared Function [get]( _
            ByVal pInvrouteID As Integer _
        ) As BOSSinvroute
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Using r As IDataReader = dbh.callSP("BOSSinvroute_get", "@invrouteID", pInvrouteID)
                If Not r.Read() Then
                    Throw New Exception("No BOSSinvroute with id " & pInvrouteID)
                End If
                Dim ret As BOSSinvroute = makeBOSSinvrouteFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of BOSSinvroute)
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim ret As New List(Of BOSSinvroute)()
            Using r As IDataReader = dbh.callSP("BOSSinvroute_list")
                While r.Read()
                    ret.Add(makeBOSSinvrouteFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            mInvrouteID = CInt(dbh.callSPSingleValue("BOSSinvroute_save", "@InvrouteID", mInvrouteID, "@Inr_key", mInr_key, _
                                                     "@Inr_invno", mInr_invno, "@Inr_prod", mInr_prod, "@Inr_bookno", mInr_bookno, _
                                                     "@Inr_segno", mInr_segno, "@Inr_from", mInr_from, "@Inr_to", mInr_to, "@Inr_sttime", _
                                                     mInr_sttime, "@Inr_start", mInr_start, "@Inr_stterm", mInr_stterm, "@Inr_etime", _
                                                     mInr_etime, "@Inr_end", mInr_end, "@Inr_eterm", mInr_eterm, "@Inr_flwncr", mInr_flwncr, _
                                                     "@Inr_flight", mInr_flight, "@Inr_class", mInr_class, "@Inr_bpmt", mInr_bpmt, "@Inr_fbasis", _
                                                     mInr_fbasis, "@Inr_fare", mInr_fare, "@Inr_dest", mInr_dest, "@Inr_miles", mInr_miles, "@Inr_fee", _
                                                     mInr_fee, "@Inr_feevt", mInr_feevt, "@Inr_feebas", mInr_feebas, "@Inr_status", mInr_status, "@Datecreated", mDatecreated))
            Return mInvrouteID
        End Using
    End Function

    Public Shared Sub delete(ByVal pInvrouteID As Integer, ByVal pInr_invno As String)
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            dbh.callNonQuerySP("BOSSinvroute_delete", "@InvrouteID", pInvrouteID, "@Inr_invno", pInr_invno)
        End Using
    End Sub
End Class
