Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class clsPo

    Public Sub New( _
    ByVal pAmount As Decimal, _
    ByVal pPo As String, _
    ByVal pPOValue As Decimal)
        mAmount = pAmount
        mPo = pPo
        mPOValue = pPOValue
    End Sub

    Public Sub New( _
    ByVal pOrderNumber As String, _
    ByVal pTotalPrice As Decimal, _
    ByVal pRequesterName As String, _
    ByVal pRequesterEmail As String)
        mOrderNumber = pOrderNumber
        mTotalPrice = pTotalPrice
        mRequesterName = pRequesterName
        mRequesterEmail = pRequesterEmail
    End Sub

    Public Sub New( _
    ByVal pOrderNumber As String)
        mOrderNumber = pOrderNumber
    End Sub

    Public Sub New( _
)
    End Sub

    Private mRequesterName As String
    Private mRequesterEmail As String
    Private mTotalPrice As Decimal
    Private mAmount As Decimal
    Private mOrderNumber As String
    Private mPo As String
    Private mPOValue As Decimal

    Public Property RequesterName() As String
        Get
            Return mRequesterName
        End Get
        Set(ByVal value As String)
            mRequesterName = value
        End Set
    End Property

    Public Property RequesterEmail() As String
        Get
            Return mRequesterEmail
        End Get
        Set(ByVal value As String)
            mRequesterEmail = value
        End Set
    End Property

    Public Property OrderNumber() As String
        Get
            Return mOrderNumber
        End Get
        Set(ByVal value As String)
            mOrderNumber = value
        End Set
    End Property

    Public Property Po() As String
        Get
            Return mPo
        End Get
        Set(ByVal value As String)
            mPo = value
        End Set
    End Property

    Public Property POValue() As Decimal
        Get
            Return mPOValue
        End Get
        Set(ByVal value As Decimal)
            mPOValue = value
        End Set
    End Property

    Public Property TotalPrice() As Decimal
        Get
            Return mTotalPrice
        End Get
        Set(ByVal value As Decimal)
            mTotalPrice = value
        End Set
    End Property

    Public Property Amount() As Decimal
        Get
            Return mAmount
        End Get
        Set(ByVal value As Decimal)
            mAmount = value
        End Set
    End Property

    Private Shared Function makeSummaryFromRow( _
           ByVal r As IDataReader _
           ) As clsPo
        Return New clsPo( _
            CDec(r.Item("Amount")), _
            clsNYS.notString(r.Item("Po")), _
            CDec(r.Item("PoValue")))
    End Function

    Private Shared Function makeSummaryPOFromRow( _
           ByVal r As IDataReader _
           ) As clsPo
        Return New clsPo( _
            clsNYS.notString(r.Item("OrderNumber")), _
            CDec(r.Item("TotalPrice")), _
            clsNYS.notString(r.Item("RequesterName")), _
            clsNYS.notString(r.Item("RequesterEmail")))
    End Function

    Private Shared Function makeSummaryBlanketFromRow( _
           ByVal r As IDataReader _
           ) As clsPo
        Return New clsPo( _
            clsNYS.notString(r.Item("OrderNumber")))
    End Function

    Public Shared Function poBlanketList(ByVal pstrRequesterName As String) As List(Of clsPo)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsPo)()
            Using r As IDataReader = dbh.callSP("O2PoBlanket_list", "@RequesterName", pstrRequesterName)
                While r.Read()
                    ret.Add(makeSummaryPOFromRow(r))
                End While
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function POBookedBlanketCheck() As List(Of clsPo)
        Using dbh As New SqlDatabaseHandle(getCUBITConnection)
            Dim ret As New List(Of clsPo)()
            Using r As IDataReader = dbh.callSP("POBookedBlanket_check")
                While r.Read()
                    ret.Add(makeSummaryBlanketFromRow(r))
                End While
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function POImportedBlanketCheck() As List(Of clsPo)
        Using dbh As New SqlDatabaseHandle(getCUBITConnection)
            Dim ret As New List(Of clsPo)()
            Using r As IDataReader = dbh.callSP("POImportedBlanket_check")
                While r.Read()
                    ret.Add(makeSummaryBlanketFromRow(r))
                End While
                Return ret
            End Using
        End Using
    End Function

    'R2.18 CR
    ''' <summary>
    ''' Returns a list summarising ALL PO's, not just blanket ones
    ''' </summary>
    ''' <param name="pstrRequesterName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function poReportList(ByVal pstrRequesterName As String) As List(Of clsPo)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsPo)()
            Using r As IDataReader = dbh.callSP("O2PoReport_list", "@RequesterName", pstrRequesterName)
                While r.Read()
                    ret.Add(makeSummaryPOFromRow(r))
                End While
                Return ret
            End Using
        End Using
    End Function


    Public Shared Function POValueGet(ByVal pPO As String) As Double
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim retValue As Double = clsNYS.notNumber(dbh.callSPSingleValueCanReturnNothing("O2Po_getValue", _
                                                                                "@OrderNumber", pPO))
            Return retValue
        End Using
    End Function

    Public Shared Function confirmedMevisValueGet(ByVal pPO As String) As Double
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim retValue As Double = clsNYS.notNumber(dbh.callSPSingleValueCanReturnNothing("O2Po_getConfirmedCost", _
                                                                                "@PO", pPO))
            Return retValue
        End Using
    End Function

    Public Shared Function potentialMevisValueGet(ByVal pPO As String) As Double
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim retValue As Double = clsNYS.notNumber(dbh.callSPSingleValueCanReturnNothing("O2Po_getPotentialCost", _
                                                                                "@PO", pPO))
            Return retValue
        End Using
    End Function

    Public Shared Function importedCubitValueGet(ByVal pPO As String) As Double
        Using dbh As New SqlDatabaseHandle(getCUBITConnection)
            Dim retValue As Double = clsNYS.notNumber(dbh.callSPSingleValueCanReturnNothing("POValueImported_check", _
                                                                                "@PO", pPO))
            Return retValue
        End Using
    End Function

    Public Shared Function potentialCubitValueGet(ByVal pPO As String) As Double
        Using dbh As New SqlDatabaseHandle(getCUBITConnection)
            Dim retValue As Double = clsNYS.notNumber(dbh.callSPSingleValueCanReturnNothing("POValueBooked_check", _
                                                                                "@PO", pPO))
            Return retValue
        End Using
    End Function

    'R2.22.2 CR
    Public Shared Function getPOListForCustomer(ByVal pintCompanyID As Integer, ByVal pintGroupID As Integer) As List(Of String)
        Using dbh As New SqlDatabaseHandle(getConfig("connectionString"))
            Dim oList As New List(Of String)
            Using r As IDataReader = dbh.callSP("enquiry_getPoListForCustomer", _
                                                "@Companyid", pintCompanyID, _
                                                "@GroupID", pintGroupID)
                While r.Read
                    oList.Add(r.Item("PO"))
                End While
            End Using
            Return oList
        End Using
    End Function
End Class


