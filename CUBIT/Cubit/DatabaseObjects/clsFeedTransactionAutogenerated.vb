Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class FeedTransaction

    Public Sub New( _
        ByVal pTransactionid As Integer, _
        ByVal pTransactioncode As String, _
        ByVal pTransactionvalue As Nullable(Of Double), _
        ByVal pTransactiontype As String)
        mTransactionid = pTransactionid
        mTransactioncode = pTransactioncode
        mTransactionvalue = pTransactionvalue
        mTransactiontype = pTransactiontype
    End Sub

    Public Sub New( _
)
    End Sub

    Private mTransactionid As Integer
    Private mTransactioncode As String
    Private mTransactionvalue As Nullable(Of Double)
    Private mTransactiontype As String

    Public Property Transactionid() As Integer
        Get
            Return mTransactionid
        End Get
        Set(ByVal value As Integer)
            mTransactionid = value
        End Set
    End Property

    Public Property Transactioncode() As String
        Get
            Return mTransactioncode
        End Get
        Set(ByVal value As String)
            mTransactioncode = value
        End Set
    End Property

    Public Property Transactiontype() As String
        Get
            Return mTransactiontype
        End Get
        Set(ByVal value As String)
            mTransactiontype = value
        End Set
    End Property

    Public Property Transactionvalue() As Nullable(Of Double)
        Get
            Return mTransactionvalue
        End Get
        Set(ByVal value As Nullable(Of Double))
            mTransactionvalue = value
        End Set
    End Property

    Private Shared Function makeFeedTransactionFromRow(ByVal r As IDataReader) As FeedTransaction
        Return New FeedTransaction( _
                clsStuff.notWholeNumber(r.Item("transactionid")), _
                clsStuff.notString(r.Item("transactioncode")), _
                toNullableFloat(r.Item("transactionvalue")), _
                clsStuff.notString(r.Item("transactiontype")))
    End Function

    Public Shared Function [get](ByVal pTransactionid As Integer) As FeedTransaction
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("FeedTransaction_get", _
                                                "@transactionid", pTransactionid)
                Dim ret As New FeedTransaction
                If r.Read() Then
                    ret = makeFeedTransactionFromRow(r)
                End If
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of FeedTransaction)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of FeedTransaction)()
            Using r As IDataReader = dbh.callSP("FeedTransaction_list")
                While r.Read()
                    ret.Add(makeFeedTransactionFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mTransactionid = clsStuff.notWholeNumber(dbh.callSPSingleValue("FeedTransaction_save", _
                                                        "@Transactionid", mTransactionid, _
                                                        "@Transactioncode", mTransactioncode, _
                                                        "@Transactionvalue", mTransactionvalue, _
                                                        "@Transactiontype", mTransactiontype))
            Return mTransactionid
        End Using
    End Function

    Public Shared Function TransactionCheck(ByVal pintTransactionNumber As Integer, _
                                        ByVal pstrPassengerName As String, _
                                        ByVal pdblTotalAmount As Double) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim id As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing("transaction_check", _
                                                        "@Transactionnumber", pintTransactionNumber, _
                                                        "@passenger", pstrPassengerName, _
                                                        "@amount", pdblTotalAmount))
            Return id
        End Using
    End Function

    Public Shared Function TransactionBookingCheck(ByVal pintBookingNumber As Integer, _
                                       ByVal pstrPassengerName As String, _
                                       ByVal pdblTotalAmount As Double, _
                                       ByVal pstrStatus As String) As Integer

        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim id As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing("TransactionBooking_check", _
                                                        "@BookingNumber", pintBookingNumber, _
                                                        "@passenger", pstrPassengerName, _
                                                        "@amount", pdblTotalAmount, _
                                                        "@status", pstrStatus))
            Return id
        End Using
    End Function

    Public Shared Sub delete(ByVal pTransactionid As Integer)
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("FeedTransaction_delete", "@Transactionid", pTransactionid)
        End Using
    End Sub

End Class
