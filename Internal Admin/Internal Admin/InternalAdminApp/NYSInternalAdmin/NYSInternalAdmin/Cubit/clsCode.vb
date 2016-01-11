Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class clsCode

    Public Sub New( _
        ByVal pCodeid As Integer, _
        ByVal pCustomername As String, _
        ByVal pCustomercode As String)
        mCodeid = pCodeid
        mCustomername = pCustomername
        mCustomercode = pCustomercode
    End Sub

    'Public Sub New( _
    '    ByVal ptransactionnumber As Integer)
    '    mtransactionnumber = ptransactionnumber
    'End Sub

    'R2.21 SA
    Public Sub New( _
                   ByVal pCustomername As String, _
                    ByVal pCustomercode As String, _
                    ByVal pTransactionno As Integer)
        mCustomername = pCustomername
        mCustomercode = pCustomercode
        mTransactionnumber = pTransactionno
    End Sub


    Public Sub New( _
)
    End Sub

    'Private mtransactionnumber As Integer
    Private mCodeid As Integer
    Private mCustomername As String
    Private mCustomercode As String

    'R2.21 
    Private mTransactionnumber As Integer
    Public Property TransactionNumber() As Integer
        Get
            Return mTransactionnumber
        End Get
        Set(ByVal value As Integer)
            mTransactionnumber = value
        End Set
    End Property

    'Public Property transactionnumber() As Integer
    '    Get
    '        Return mtransactionnumber
    '    End Get
    '    Set(ByVal value As Integer)
    '        mtransactionnumber = value
    '    End Set
    'End Property

    Public Property Codeid() As Integer
        Get
            Return mCodeid
        End Get
        Set(ByVal value As Integer)
            mCodeid = value
        End Set
    End Property

    Public Property Customername() As String
        Get
            Return mCustomername
        End Get
        Set(ByVal value As String)
            mCustomername = value
        End Set
    End Property

    Public Property Customercode() As String
        Get
            Return mCustomercode
        End Get
        Set(ByVal value As String)
            mCustomercode = value
        End Set
    End Property

    'Private Shared Function testRow( _
    '       ByVal r As IDataReader _
    '   ) As clsCode
    '    Return New clsCode( _
    '            CInt(r.Item("transactionnumber")))
    'End Function

    Private Shared Function makeCodeFromRow( _
            ByVal r As IDataReader _
        ) As clsCode
        Return New clsCode( _
                CInt(r.Item("codeid")), _
                toStr(r.Item("customername")), _
                toStr(r.Item("customercode")))
    End Function

    Public Shared Function [get]( _
            ByVal pCodeid As Integer _
        ) As clsCode
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("code_get", "@codeid", pCodeid)
                If Not r.Read() Then
                    Throw New Exception("No Code with id " & pCodeid)
                End If
                Dim ret As clsCode = makeCodeFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    'Public Shared Function testlist() As List(Of clsCode)
    '    Using dbh As New SqlDatabaseHandle(getConnection)
    '        Dim ret As New List(Of clsCode)()
    '        Using r As IDataReader = dbh.callSP("test_list")
    '            While r.Read()
    '                ret.Add(testRow(r))
    '            End While
    '        End Using
    '        Return ret
    '    End Using
    'End Function

    Public Shared Function testCategory(ByVal ptransactionnumber As Integer) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intRet As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing("test_Category", "@transactionnumber", ptransactionnumber))
            Return intRet
        End Using
    End Function

    Public Shared Function list() As List(Of clsCode)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsCode)()
            Using r As IDataReader = dbh.callSP("code_list")
                While r.Read()
                    ret.Add(makeCodeFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mCodeid = CInt(dbh.callSPSingleValue("code_save", "@Codeid", mCodeid, "@Customername", mCustomername, "@Customercode", mCustomercode))
            Return mCodeid
        End Using
    End Function

    Public Shared Function checkLVCode(ByVal pACKCode As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intRet As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing("checkLVCode", "@ACKCode", pACKCode))
            Return intRet
        End Using
    End Function

    Public Shared Sub delete( _
            ByVal pCodeid As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("code_delete", "@Codeid", pCodeid)
        End Using
    End Sub

    Public Sub delete()
        delete(mCodeid)
    End Sub

    Public Shared Function getByName(ByVal pstrGroupName As String) As clsCode
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("code_getByName", "@customername", pstrGroupName)
                Dim ret As New clsCode
                If r.Read() Then
                    'Throw New Exception("No Code with name " & pstrGroupName)
                    ret = makeCodeFromRow(r)
                End If
                Return ret
            End Using
        End Using

    End Function

    'R2.21 SA
    Public Shared Function BossCodeSave(ByVal pintTransactionno As Integer, _
                          ByVal pstrCustomerName As String, _
                          ByVal pstrCustomerCode As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intRet As Integer
            intRet = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing("Customercode_save", _
                                                           "@customername", pstrCustomerName, _
                                                           "@Customercode", pstrCustomerCode, _
                                                           "@transactionno", pintTransactionno))
            Return intRet
        End Using
    End Function

    'Public Structure costs
    '    Public boss, confirmed, potential As Double
    'End Structure

    'Public Shared Function allValues_get(ByVal pPO As String) As costs
    '    Using dbh As New SqlDatabaseHandle(getConfig("connectionString"))
    '        Dim ret As New costs
    '        Dim boss As Double = runBossLivePoCheck(pPO)

    '        'If boss = -1 Then
    '        '    boss = notDouble(dbh.callSPSingleValueCanReturnNothing("O2Po_getBossCost", "@PO", pPO))
    '        'End If

    '        ret.boss = boss
    '        Dim confirmed As Double = clsStuff.notDouble(dbh.callSPSingleValueCanReturnNothing("O2Po_getConfirmedCost", _
    '                                                                            "@PO", pPO))
    '        ret.confirmed = confirmed
    '        Dim potential As Double = clsStuff.notDouble(dbh.callSPSingleValueCanReturnNothing("O2Po_getPotentialCost", _
    '                                                                            "@PO", pPO))
    '        ret.potential = potential
    '        Return ret
    '    End Using
    'End Function

    'Public Shared Function runBossLivePoCheck(ByVal pstrPO As String) As Decimal

    '    Dim dblRet As Decimal = 0

    '    Try
    '        Dim ConnectionString As String
    '        ConnectionString = "Provider = VFPOLEDB;Data Source=" & getConfig("BOSSTablesPath") & ";Mode=Read;CollatingSequence=MACHINE;"

    '        Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(ConnectionString)
    '        dBaseConnection.Open()

    '        Dim dBaseCommand As New System.Data.OleDb.OleDbCommand("SELECT inm_billed,inm_bilvat,inm_disvat FROM Invmain where inm_pono = '" & pstrPO & "'", dBaseConnection)
    '        Dim dBaseDataReader As System.Data.OleDb.OleDbDataReader = dBaseCommand.ExecuteReader()

    '        While dBaseDataReader.Read
    '            Dim dblGross As Decimal = CDec(dBaseDataReader("inm_billed").ToString)
    '            Dim dblNormalVat As Decimal = CDec(dBaseDataReader("inm_bilvat").ToString)
    '            Dim dblDispVat As Decimal = CDec(dBaseDataReader("inm_disvat").ToString)
    '            Dim dblNett As Decimal = dblGross - (dblNormalVat + dblDispVat)
    '            dblRet = dblRet + dblNett
    '        End While

    '    Catch ex As Exception
    '        dblRet = -1
    '    End Try

    '    Return dblRet
    'End Function

    'Public Shared Function [confirmedValue_get](ByVal pPO As String) As Double
    '    Using dbh As New SqlDatabaseHandle(getConfig("connectionString"))
    '        Dim retValue As Double = clsStuff.notDouble(dbh.callSPSingleValueCanReturnNothing("O2Po_getConfirmedCost", _
    '                                                                                        "@PO", pPO))
    '        Return retValue
    '    End Using
    'End Function

    'Public Shared Function [potentialValue_get](ByVal pPO As String) As Double
    '    Using dbh As New SqlDatabaseHandle(getConfig("connectionString"))
    '        Dim retValue As Double = clsStuff.notDouble(dbh.callSPSingleValueCanReturnNothing("O2Po_getPotentialCost", _
    '                                                                                        "@PO", pPO))
    '        Return retValue
    '    End Using
    'End Function

    'Public Shared Function O2PoGetValue(ByVal pPO As String) As Double
    '    Using dbh As New SqlDatabaseHandle(getConfig("connectionString"))
    '        Dim retValue As Double = clsStuff.notDouble(dbh.callSPSingleValueCanReturnNothing("O2Po_getValue", _
    '                                                                                        "@PO", pPO))
    '        Return retValue
    '    End Using
    'End Function
End Class
