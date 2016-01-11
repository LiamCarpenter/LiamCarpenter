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
    ByVal pTotalPrice As Decimal)
        mOrderNumber = pOrderNumber
        mTotalPrice = pTotalPrice
    End Sub

    Public Sub New( _
    ByVal pOrderNumber As String)
        mOrderNumber = pOrderNumber
    End Sub

    Public Sub New( _
)
    End Sub

    Private mTotalPrice As Decimal
    Private mAmount As Decimal
    Private mOrderNumber As String
    Private mPo As String
    Private mPOValue As Decimal

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
            clsStuff.notString(r.Item("Po")), _
            CDec(r.Item("PoValue")))
    End Function

    Private Shared Function makeSummaryPOFromRow( _
           ByVal r As IDataReader _
           ) As clsPo
        Return New clsPo( _
            clsStuff.notString(r.Item("OrderNUmber")), _
            CDec(r.Item("TotalPrice")))
    End Function

    Private Shared Function makeSummaryBlanketFromRow( _
           ByVal r As IDataReader _
           ) As clsPo
        Return New clsPo( _
            clsStuff.notString(r.Item("OrderNUmber")))
    End Function
   
    Public Structure costs
        Public bossTotal, confirmedMevis, potentialMevis, importedCubit, potentialCubit As Double
    End Structure

    Public Shared Function allValuesGet(ByVal pPO As String) As costs
        Dim ret As New costs
        ret.bossTotal = runBossLivePoCheck(pPO)

        'If boss = -1 Then
        '    boss = notDouble(dbh.callSPSingleValueCanReturnNothing("O2Po_getBossCost", "@PO", pPO))
        'End If

        ret.confirmedMevis = confirmedMevisValueGet(pPO)
        ret.potentialMevis = potentialMevisValueGet(pPO)
        ret.importedCubit = importedCubitValueGet(pPO)
        ret.potentialCubit = potentialCubitValueGet(pPO)

        Return ret
    End Function

    Public Shared Function runBossLivePoCheck(ByVal pstrPO As String) As Decimal

        Dim dblRet As Decimal = 0

        Try
            Dim ConnectionString As String
            ConnectionString = "Provider = VFPOLEDB;Data Source=" & getConfig("BOSSTablesPath") & ";Mode=Read;CollatingSequence=MACHINE;"

            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(ConnectionString)
            dBaseConnection.Open()

            Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT inm_billed,inm_bilvat,inm_disvat FROM Invmain where inm_pono = '" & pstrPO & "'", dBaseConnection)
            Dim dBaseDataReader As System.Data.OleDb.OleDbDataReader = dBaseCommand.ExecuteReader()

            While dBaseDataReader.Read
                Dim dblGross As Decimal = CDec(dBaseDataReader("inm_billed").ToString)
                Dim dblNormalVat As Decimal = CDec(dBaseDataReader("inm_bilvat").ToString)
                Dim dblDispVat As Decimal = CDec(dBaseDataReader("inm_disvat").ToString)
                Dim dblNett As Decimal = dblGross - (dblNormalVat + dblDispVat)
                dblRet = dblRet + dblNett
            End While

        Catch ex As Exception
            dblRet = -1
        End Try

        Return dblRet
    End Function

    Public Shared Function checkPOExists(ByVal pPO As String) As Boolean
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim retValue As Boolean = clsStuff.notBoolean(dbh.callSPSingleValueCanReturnNothing("O2Po_exists", _
                                                                                        "@PO", pPO))
            Return retValue
        End Using
    End Function

    Public Shared Function poBlanketList() As List(Of clsPo)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsPo)()
            Using r As IDataReader = dbh.callSP("O2PoBlanket_list")
                While r.Read()
                    ret.Add(makeSummaryPOFromRow(r))
                End While
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function POValueGet(ByVal pPO As String) As Double
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim retValue As Double = clsStuff.notDouble(dbh.callSPSingleValueCanReturnNothing("O2Po_getValue", _
                                                                                "@OrderNumber", pPO))
            Return retValue
        End Using
    End Function

    Public Shared Function confirmedMevisValueGet(ByVal pPO As String) As Double
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim retValue As Double = clsStuff.notDouble(dbh.callSPSingleValueCanReturnNothing("O2Po_getConfirmedCost", _
                                                                                "@PO", pPO))
            Return retValue
        End Using
    End Function

    Public Shared Function potentialMevisValueGet(ByVal pPO As String) As Double
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim retValue As Double = clsStuff.notDouble(dbh.callSPSingleValueCanReturnNothing("O2Po_getPotentialCost", _
                                                                                "@PO", pPO))
            Return retValue
        End Using
    End Function

    Public Shared Function importedCubitValueGet(ByVal pPO As String) As Double
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim retValue As Double = clsStuff.notDouble(dbh.callSPSingleValueCanReturnNothing("POValueImported_check", _
                                                                                "@PO", pPO))
            Return retValue
        End Using
    End Function

    Public Shared Function potentialCubitValueGet(ByVal pPO As String) As Double
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim retValue As Double = clsStuff.notDouble(dbh.callSPSingleValueCanReturnNothing("POValueBooked_check", _
                                                                                "@PO", pPO))
            Return retValue
        End Using
    End Function
End Class


