Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BossCommissionOverride

    Public Sub New( _
        ByVal pInvoiceNo As String, _
        ByVal pCommissionNett As Decimal, _
        ByVal pCommissionVat As Decimal, _
        ByVal pCommissionReceived As Decimal, _
        ByVal pCommissionDue As Decimal, _
        ByVal pPayNet As String, _
        ByVal pBookingTotal As Decimal, _
        ByVal pCurrency As String)
        mInvoiceNo = pInvoiceNo
        mCommissionNett = pCommissionNett
        mCommissionVat = pCommissionVat
        mCommissionReceived = pCommissionReceived
        mCommissionDue = pCommissionDue
        mPayNet = pPayNet
        mBookingTotal = pBookingTotal

        'R2.16 CR
        If pCurrency = "" Then
            pCurrency = "GBP"
        End If
        mCurrency = pCurrency
    End Sub

    Public Sub New( _
)
    End Sub

    Private mInvoiceNo As String
    Private mCommissionNett As Decimal
    Private mCommissionVat As Decimal
    Private mCommissionReceived As Decimal
    Private mCommissionDue As Decimal
    Private mPayNet As String
    Private mBookingTotal As Decimal

    'R2.16 CR
    Private mCurrency As String

    Public Property InvoiceNo() As String
        Get
            Return mInvoiceNo
        End Get
        Set(ByVal value As String)
            mInvoiceNo = value
        End Set
    End Property

    Public Property CommissionNett() As Decimal
        Get
            Return mCommissionNett
        End Get
        Set(ByVal value As Decimal)
            mCommissionNett = value
        End Set
    End Property

    Public Property CommissionVat() As Decimal
        Get
            Return mCommissionVat
        End Get
        Set(ByVal value As Decimal)
            mCommissionVat = value
        End Set
    End Property

    Public Property CommissionReceived() As Decimal
        Get
            Return mCommissionReceived
        End Get
        Set(ByVal value As Decimal)
            mCommissionReceived = value
        End Set
    End Property

    Public Property CommissionDue() As Decimal
        Get
            Return mCommissionDue
        End Get
        Set(ByVal value As Decimal)
            mCommissionDue = value
        End Set
    End Property

    Public Property PayNet() As String
        Get
            Return mPayNet
        End Get
        Set(ByVal value As String)
            mPayNet = value
        End Set
    End Property

    Public Property BookingTotal() As Decimal
        Get
            Return mBookingTotal
        End Get
        Set(ByVal value As Decimal)
            mBookingTotal = value
        End Set
    End Property

    'R2.16 CR
    Public Property Currency As String
        Get
            Return mCurrency
        End Get
        Set(ByVal value As String)
            mCurrency = value
        End Set
    End Property

    Private Shared Function makeBossCommissionCheckFromRow( _
            ByVal r As IDataReader _
        ) As BossCommissionOverride
        Return New BossCommissionOverride( _
                clsUseful.notString(r.Item("invoiceNo")), _
                clsUseful.notDecimal(r.Item("commissionNett")), _
                clsUseful.notDecimal(r.Item("commissionVat")), _
                clsUseful.notDecimal(r.Item("commissionReceived")), _
                clsUseful.notDecimal(r.Item("commissionDue")), _
                clsUseful.notString(r.Item("payNet")), _
                clsUseful.notDecimal(r.Item("bookingTotal")), _
        clsUseful.notString(r.Item("currency")))
    End Function

    Public Shared Function getOverride(ByVal pInvoiceNo As String) As List(Of BossCommissionOverride)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BossCommissionOverride)()
            Using r As IDataReader = dbh.callSP("BossCommissionOverride_get", "@InvoiceNo", pInvoiceNo)
                While r.Read()
                    ret.Add(makeBossCommissionCheckFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function getSql(ByVal pInvoiceNo As String) As List(Of BossCommissionOverride)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BossCommissionOverride)()
            Using r As IDataReader = dbh.callSP("BossCommissionOverride_SqlGet", "@InvoiceNo", pInvoiceNo)
                While r.Read()
                    ret.Add(makeBossCommissionCheckFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim iRet As Integer = clsUseful.notInteger(dbh.callSPSingleValue("BossCommissionOverride_save", _
                                                                "@InvoiceNo", mInvoiceNo, _
                                                                "@CommissionNett", mCommissionNett, _
                                                                "@CommissionVat", mCommissionVat, _
                                                                "@CommissionReceived", mCommissionReceived, _
                                                                "@CommissionDue", mCommissionDue, _
                                                                "@PayNet", mPayNet, _
                                                                "@BookingTotal", mBookingTotal, _
                                                                "@Currency", mCurrency.ToUpper))
            Return iRet
        End Using
    End Function

    Public Shared Function delete(ByVal pInvoiceNo As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intRet As Integer = clsUseful.notInteger(dbh.callSPSingleValueCanReturnNothing("BossCommissionOverride_delete", "@InvoiceNo", pInvoiceNo))
            Return intRet
        End Using
    End Function

End Class
