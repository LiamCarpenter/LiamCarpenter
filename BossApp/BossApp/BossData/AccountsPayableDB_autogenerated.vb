Imports EvoUtilities.ConfigUtils
Imports EvoDatabaseUtils

Partial Public Class AccountsPayableDB

    Public Sub New( _
        ByVal pInvoiceRef As String)
        mInvoiceRef = pInvoiceRef
    End Sub

    Public Sub New( _
)
    End Sub

    Private mInvoiceRef As String
    
    Public Property InvoiceRef() As String
        Get
            Return mInvoiceRef
        End Get
        Set(ByVal value As String)
            mInvoiceRef = value
        End Set
    End Property

    Private Shared Function makeAccountsPayableList( _
            ByVal r As IDataReader _
        ) As AccountsPayableDB
        Return New AccountsPayableDB( _
                clsUseful.notString(r.Item("invoiceRef")))
    End Function

    Public Shared Function accountsPayableGetInvoices(ByVal pStartDate As String, ByVal pEndDate As String) As List(Of AccountsPayableDB)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of AccountsPayableDB)()
            Using r As IDataReader = dbh.callSP("accountsPayable_getInvoices", _
                                                "@StartDate", pStartDate, _
                                                "@EndDate", pEndDate)
                While r.Read()
                    ret.Add(makeAccountsPayableList(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function commissionGetInvoices(ByVal pDateLimit As String) As List(Of AccountsPayableDB)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of AccountsPayableDB)()
            Using r As IDataReader = dbh.callSP("commission_getInvoices", _
                                                "@DateLimit", pDateLimit)
                While r.Read()
                    ret.Add(makeAccountsPayableList(r))
                End While
            End Using
            Return ret
        End Using
    End Function

End Class
