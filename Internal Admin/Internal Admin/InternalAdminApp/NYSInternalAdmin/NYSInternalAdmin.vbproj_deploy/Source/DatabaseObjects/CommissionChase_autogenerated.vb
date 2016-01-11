Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class CommissionChase

    Public Sub New( _
        ByVal pCommissionChaseID As Integer, _
        ByVal pInvoiceid As String, _
        ByVal pDate1 As String, _
        ByVal pDate2 As String, _
        ByVal pDate3 As String, _
        ByVal pDate4 As String, _
        ByVal pDate5 As String, _
        ByVal pDateSpecial As String, _
        ByVal pVenuename As String, _
        ByVal pInvoiceamount As String, _
        ByVal pCommnet As String, _
        ByVal pCommvat As String, _
        ByVal pCommtotal As String, _
        ByVal pInvoicedate As String, _
        ByVal pEventdate As String, _
        ByVal pEmailto As String, _
        ByVal pSentBy As String)
        mCommissionChaseID = pCommissionChaseID
        mInvoiceid = pInvoiceid
        mDate1 = pDate1
        mDate2 = pDate2
        mDate3 = pDate3
        mDate4 = pDate4
        mDate5 = pDate5
        mDateSpecial = pDateSpecial
        mVenuename = pVenuename
        mInvoiceamount = pInvoiceamount
        mCommnet = pCommnet
        mCommvat = pCommvat
        mCommtotal = pCommtotal
        mInvoicedate = pInvoicedate
        mEventdate = pEventdate
        mEmailto = pEmailto
        mSentBy = pSentBy
    End Sub

    Public Sub New( _
         byVal pInvoiceid As String)
        mInvoiceid = pInvoiceid
    End Sub

    Public Sub New( _
)
    End Sub

    Private mCommissionChaseID As Integer
    Private mInvoiceid As String
    Private mDate1 As String
    Private mDate2 As String
    Private mDate3 As String
    Private mDate4 As String
    Private mDate5 As String
    Private mDateSpecial As String
    Private mVenuename As String
    Private mInvoiceamount As String
    Private mCommnet As String
    Private mCommvat As String
    Private mCommtotal As String
    Private mInvoicedate As String
    Private mEventdate As String
    Private mEmailto As String
    Private mSentBy As String

    Public Property SentBy() As String
        Get
            Return mSentBy
        End Get
        Set(ByVal value As String)
            mSentBy = value
        End Set
    End Property

    Public Property CommissionChaseID() As Integer
        Get
            Return mCommissionChaseID
        End Get
        Set(ByVal value As Integer)
            mCommissionChaseID = value
        End Set
    End Property

    Public Property Invoiceid() As String
        Get
            Return mInvoiceid
        End Get
        Set(ByVal value As String)
            mInvoiceid = value
        End Set
    End Property

    Public Property Date1() As String
        Get
            Return mDate1
        End Get
        Set(ByVal value As String)
            mDate1 = value
        End Set
    End Property

    Public Property Date2() As String
        Get
            Return mDate2
        End Get
        Set(ByVal value As String)
            mDate2 = value
        End Set
    End Property

    Public Property Date3() As String
        Get
            Return mDate3
        End Get
        Set(ByVal value As String)
            mDate3 = value
        End Set
    End Property

    Public Property Date4() As String
        Get
            Return mDate4
        End Get
        Set(ByVal value As String)
            mDate4 = value
        End Set
    End Property

    Public Property Date5() As String
        Get
            Return mDate5
        End Get
        Set(ByVal value As String)
            mDate5 = value
        End Set
    End Property

    Public Property DateSpecial() As String
        Get
            Return mDateSpecial
        End Get
        Set(ByVal value As String)
            mDateSpecial = value
        End Set
    End Property

    Public Property Venuename() As String
        Get
            Return mVenuename
        End Get
        Set(ByVal value As String)
            mVenuename = value
        End Set
    End Property

    Public Property Invoiceamount() As String
        Get
            Return mInvoiceamount
        End Get
        Set(ByVal value As String)
            mInvoiceamount = value
        End Set
    End Property

    Public Property Commnet() As String
        Get
            Return mCommnet
        End Get
        Set(ByVal value As String)
            mCommnet = value
        End Set
    End Property

    Public Property Commvat() As String
        Get
            Return mCommvat
        End Get
        Set(ByVal value As String)
            mCommvat = value
        End Set
    End Property

    Public Property Commtotal() As String
        Get
            Return mCommtotal
        End Get
        Set(ByVal value As String)
            mCommtotal = value
        End Set
    End Property

    Public Property Invoicedate() As String
        Get
            Return mInvoicedate
        End Get
        Set(ByVal value As String)
            mInvoicedate = value
        End Set
    End Property

    Public Property Eventdate() As String
        Get
            Return mEventdate
        End Get
        Set(ByVal value As String)
            mEventdate = value
        End Set
    End Property

    Public Property Emailto() As String
        Get
            Return mEmailto
        End Get
        Set(ByVal value As String)
            mEmailto = value
        End Set
    End Property

    Private Shared Function makeCommissionChaseFromRow( _
            ByVal r As IDataReader _
        ) As CommissionChase
        Return New CommissionChase( _
                clsNYS.notInteger(r.Item("commissionChaseID")), _
                clsNYS.notString(r.Item("invoiceid")), _
                clsNYS.notString(r.Item("date1")), _
                clsNYS.notString(r.Item("date2")), _
                clsNYS.notString(r.Item("date3")), _
                clsNYS.notString(r.Item("date4")), _
                clsNYS.notString(r.Item("date5")), _
                clsNYS.notString(r.Item("dateSpecial")), _
                clsNYS.notString(r.Item("venuename")), _
                clsNYS.notString(r.Item("invoiceamount")), _
                clsNYS.notString(r.Item("commnet")), _
                clsNYS.notString(r.Item("commvat")), _
                clsNYS.notString(r.Item("commtotal")), _
                clsNYS.notString(r.Item("invoicedate")), _
                clsNYS.notString(r.Item("eventdate")), _
                clsNYS.notString(r.Item("emailto")), _
                clsNYS.notString(r.Item("sentBy")))
    End Function

    Private Shared Function makeCommissionChaseFromRowInvoice( _
            ByVal r As IDataReader _
        ) As CommissionChase
        Return New CommissionChase( _
                clsNYS.notString(r.Item("invoiceid")))
    End Function

    Public Shared Function [get]( _
            ByVal pCommissionChaseID As Integer _
        ) As CommissionChase
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("commissionChase_get", "@commissionChaseID", pCommissionChaseID)
                If Not r.Read() Then
                    Throw New Exception("No CommissionChase with id " & pCommissionChaseID)
                End If
                Dim ret As CommissionChase = makeCommissionChaseFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of CommissionChase)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of CommissionChase)()
            Using r As IDataReader = dbh.callSP("commissionChase_list")
                While r.Read()
                    ret.Add(makeCommissionChaseFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function bossCommissionClaimUnpaidInvoice() As List(Of CommissionChase)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of CommissionChase)()
            Using r As IDataReader = dbh.callSP("bossCommission_ClaimUnpaidInvoice")
                While r.Read()
                    ret.Add(makeCommissionChaseFromRowInvoice(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function outstandingCommissionInvoice(ByVal pDateLimit As String, ByVal pVenueName As String) As List(Of CommissionChase)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of CommissionChase)()
            Using r As IDataReader = dbh.callSP("boss_outstandingcommissionInvoice", "@datelimit", pDateLimit, "@venuename", pVenueName)
                While r.Read()
                    ret.Add(makeCommissionChaseFromRowInvoice(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mCommissionChaseID = CInt(dbh.callSPSingleValue("commissionChase_save", "@CommissionChaseID", mCommissionChaseID, "@Invoiceid", mInvoiceid, "@Date1", mDate1, _
                                                            "@Date2", mDate2, "@Date3", mDate3, "@Date4", mDate4, "@Date5", mDate5, "@DateSpecial", mDateSpecial, _
                                                            "@Venuename", mVenuename, "@Invoiceamount", mInvoiceamount, "@Commnet", mCommnet, "@Commvat", mCommvat, _
                                                            "@Commtotal", mCommtotal, "@Invoicedate", mInvoicedate, "@Eventdate", mEventdate, _
                                                            "@Emailto", mEmailto, "@sentBy", mSentBy))
            Return mCommissionChaseID
        End Using
    End Function

    Public Shared Sub saveDate(ByVal pType As Integer, ByVal pDate As String, ByVal pstrInvNo As String)
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("CommissionChase_saveDate", "@Type", pType, "@Date", pDate, "@invoiceid", pstrInvNo)
        End Using
    End Sub

    Public Shared Sub updateSentBy(ByVal pIds As String, ByVal pType As String)
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("commissionChase_updateSentBy", "@Ids", pIds, "@type", pType)
        End Using
    End Sub

    Public Shared Sub delete( _
            ByVal pCommissionChaseID As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("commissionChase_delete", "@CommissionChaseID", pCommissionChaseID)
        End Using
    End Sub

    Public Sub delete()
        delete(mCommissionChaseID)
    End Sub

End Class
