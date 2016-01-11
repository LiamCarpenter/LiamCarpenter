Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class clsClientStatement_ConsolidatedInvoices

    Public Sub New(ByVal pConsolidatedRef As String, _
                   ByVal pEmail As String, _
                   ByVal pFileName As String, _
                   ByVal pDateSent As Date)
        mConsolidatedRef = pConsolidatedRef
        mEmail = pEmail
        mFileName = pFileName
        mDateSent = pDateSent
    End Sub
    Public Sub New()
    End Sub

    Public Sub New(ByVal pConsolidatedRef As String)
        mConsolidatedRef = pConsolidatedRef
    End Sub

    Private mConsolidatedRef As String
    Private mEmail As String
    Private mFileName As String
    Private mDateSent As Date

    Public Property ConsolidatedRef() As String
        Get
            Return mConsolidatedRef
        End Get
        Set(ByVal value As String)
            mConsolidatedRef = value
        End Set
    End Property

    Public Property email() As String
        Get
            Return mEmail
        End Get
        Set(ByVal value As String)
            mEmail = value
        End Set
    End Property

    Public Property fileName() As String
        Get
            Return mFileName
        End Get
        Set(ByVal value As String)
            mFileName = value
        End Set
    End Property

    Public Property dateSent() As Date
        Get
            Return mDateSent
        End Get
        Set(ByVal value As Date)
            mDateSent = value
        End Set
    End Property

    Public Shared Function getCIno(ByVal pInvoiceNo As String) As String
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim strRet As String = clsNYS.notString(dbh.callSPSingleValueCanReturnNothing("ClientStatement_getCIno", _
                                                 "@InvoiceNo", pInvoiceNo))
            Return strRet
        End Using
    End Function

    Public Shared Function getCIdetails(ByVal pInvoiceNo As String) As clsClientStatement_ConsolidatedInvoices
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim details As New clsClientStatement_ConsolidatedInvoices
            Using r As IDataReader = dbh.callSP("ClientStatement_getCIdetails", _
                                                "@InvoiceNo", pInvoiceNo)
                While r.Read()
                    With details
                        .mConsolidatedRef = clsNYS.notString(r.Item("CoverInvoice"))
                        .mEmail = clsNYS.notString(r.Item("email"))
                        .mFileName = clsNYS.notString(r.Item("Filename"))
                        .mDateSent = (r.Item("date_sent"))
                    End With
                End While
            End Using
            Return details
        End Using
    End Function

    Public Shared Function getFilePath(ByVal pConsolidatedRef As String) As String
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim strRet As String = dbh.callSPSingleValueCanReturnNothing("ClientStatement_getCIfilePath", _
                                                                         "@CoverInvoice", pConsolidatedRef)
            Return strRet
        End Using
    End Function

    Public Shared Function getID(ByVal pClientBossCode As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim intRet As Integer = clsNYS.notInteger(dbh.callSPSingleValueCanReturnNothing("ClientonConsPlus_getID", _
                                                                                            "@ClientBossCode", pClientBossCode))
            Return intRet
        End Using
    End Function

End Class

