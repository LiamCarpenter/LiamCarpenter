Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class O2PoAdmin

    Public Sub New( _
        ByVal pO2PoAdminID As Integer, _
        ByVal pInvoiceref As String, _
        ByVal pPO As String, _
        ByVal pInvoicestatus As String, _
        ByVal pDatesent As String)
        mO2PoAdminID = pO2PoAdminID
        mInvoiceref = pInvoiceref
        mPO = pPO
        mInvoicestatus = pInvoicestatus
        mDatesent = pDatesent
    End Sub

    Public Sub New( _
        ByVal pInvoiceref As String, _
        ByVal pPO As String, _
        ByVal pcrsref As String)
        mInvoiceref = pInvoiceref
        mPO = pPO
        mcrsref = pcrsref
    End Sub

    Public Sub New( _
)
    End Sub

    Private mcrsref As String
    Private mO2PoAdminID As Integer
    Private mInvoiceref As String
    Private mPO As String
    Private mInvoicestatus As String
    Private mDatesent As String

    Public Property crsref() As String
        Get
            Return mcrsref
        End Get
        Set(ByVal value As String)
            mcrsref = value
        End Set
    End Property

    Public Property O2PoAdminID() As Integer
        Get
            Return mO2PoAdminID
        End Get
        Set(ByVal value As Integer)
            mO2PoAdminID = value
        End Set
    End Property

    Public Property Invoiceref() As String
        Get
            Return mInvoiceref
        End Get
        Set(ByVal value As String)
            mInvoiceref = value
        End Set
    End Property

    Public Property PO() As String
        Get
            Return mPO
        End Get
        Set(ByVal value As String)
            mPO = value
        End Set
    End Property

    Public Property Invoicestatus() As String
        Get
            Return mInvoicestatus
        End Get
        Set(ByVal value As String)
            mInvoicestatus = value
        End Set
    End Property

    Public Property Datesent() As String
        Get
            Return mDatesent
        End Get
        Set(ByVal value As String)
            mDatesent = value
        End Set
    End Property

    Private Shared Function makeO2PoAdminFromRow( _
            ByVal r As IDataReader _
        ) As O2PoAdmin
        Return New O2PoAdmin( _
                clsNYS.notInteger(r.Item("O2PoAdminID")), _
                clsNYS.notString(r.Item("Invoiceref")), _
                clsNYS.notString(r.Item("PO")), _
                clsNYS.notString(r.Item("invoicestatus")), _
                clsNYS.notString(r.Item("datesent")))
    End Function

    Private Shared Function makeO2PoAdminFromRowAwaiting( _
            ByVal r As IDataReader _
        ) As O2PoAdmin
        Return New O2PoAdmin( _
                clsNYS.notString(r.Item("Invoiceref")), _
                clsNYS.notString(r.Item("PO")), _
                clsNYS.notString(r.Item("crsref")))
    End Function

    Public Shared Function [get]( _
            ByVal pO2PoAdminID As Integer _
        ) As O2PoAdmin
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("O2PoAdmin_get", "@O2PoAdminID", pO2PoAdminID)
                If Not r.Read() Then
                    Throw New Exception("No O2PoAdmin with id " & pO2PoAdminID)
                End If
                Dim ret As O2PoAdmin = makeO2PoAdminFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function O2PoAdminGetAwaiting() As List(Of O2PoAdmin)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of O2PoAdmin)()
            Using r As IDataReader = dbh.callSP("O2PoAdmin_getAwaiting")
                While r.Read()
                    ret.Add(makeO2PoAdminFromRowAwaiting(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function list(ByVal pstrStatus As String) As List(Of O2PoAdmin)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of O2PoAdmin)()
            Using r As IDataReader = dbh.callSP("O2PoAdmin_list", "@status", pstrStatus)
                While r.Read()
                    ret.Add(makeO2PoAdminFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mO2PoAdminID = clsNYS.notInteger(dbh.callSPSingleValue("O2PoAdmin_save", "@O2PoAdminID", mO2PoAdminID, "@Invoiceref", mInvoiceref, "@PO", mPO, _
                                                                   "@Invoicestatus", mInvoicestatus, "@datesent", mDatesent))
            Return mO2PoAdminID
        End Using
    End Function

    Public Shared Function saveStatus(ByVal pO2PoAdminID As Integer, ByVal pInvoicestatus As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intRet As Integer = clsNYS.notInteger(dbh.callSPSingleValue("O2PoAdmin_saveStatus", "@O2PoAdminID", pO2PoAdminID, "@Invoicestatus", pInvoicestatus))
            Return intRet
        End Using
    End Function

    Public Shared Sub delete( _
            ByVal pO2PoAdminID As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("O2PoAdmin_delete", "@O2PoAdminID", pO2PoAdminID)
        End Using
    End Sub

    Public Sub delete()
        delete(mO2PoAdminID)
    End Sub

End Class
