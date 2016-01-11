Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class clsGroup
    'R15 CR
    Public Sub New( _
        ByVal pgroupid As Integer, _
        ByVal pgroupname As String, _
        ByVal pcubit As Nullable(Of Boolean), _
        ByVal pgroupstatus As String, _
        ByVal pgroupinitials As String)
        mgroupid = pgroupid
        mgroupname = pgroupname
        mcubit = pcubit
        mgroupstatus = pgroupstatus
        mgroupinitials = pgroupinitials
    End Sub

    Public Sub New( _
        ByVal pcodeid As Integer, _
        ByVal pcustomername As String, _
        ByVal pcompanyname As String, _
        ByVal pcustomercode As String)
        mcodeid = pcodeid
        mcustomername = pcustomername
        mcompanyname = pcompanyname
        mcustomercode = pcustomercode
    End Sub

    Public Sub New( _
        ByVal pccid As Integer, _
        ByVal pgroupid As Integer, _
        ByVal pexpression As String)
        mccid = pccid
        mgroupid = pgroupid
        mexpression = pexpression
    End Sub

    Public Sub New( _
        ByVal pgroupid As Integer, _
        ByVal pgroupname As String)
        mgroupid = pgroupid
        mgroupname = pgroupname
    End Sub

    Public Sub New( _
        ByVal pBossCode As String)
        mBossCode = pBossCode
    End Sub
    Public Sub New( _
    )
    End Sub

    Private mBossCode As String
    Private mcodeid As Integer
    Private mcompanyid As Integer
    Private mccid As Integer
    Private mgroupid As Integer
    Private mcustomername As String
    Private mcompanyname As String
    Private mcustomercode As String
    Private mgroupname As String
    Private mexpression As String
    Private mcubit As Nullable(Of Boolean)
    Private mgroupstatus As String
    Private mgroupinitials As String

    Public Property BossCode() As String
        Get
            Return mBossCode
        End Get
        Set(ByVal value As String)
            mBossCode = value
        End Set
    End Property

    Public Property customername() As String
        Get
            Return mcustomername
        End Get
        Set(ByVal value As String)
            mcustomername = value
        End Set
    End Property

    Public Property companyname() As String
        Get
            Return mcompanyname
        End Get
        Set(ByVal value As String)
            mcompanyname = value
        End Set
    End Property

    Public Property customercode() As String
        Get
            Return mcustomercode
        End Get
        Set(ByVal value As String)
            mcustomercode = value
        End Set
    End Property

    Public Property companyid() As Integer
        Get
            Return mcompanyid
        End Get
        Set(ByVal value As Integer)
            mcompanyid = value
        End Set
    End Property

    Public Property codeid() As Integer
        Get
            Return mcodeid
        End Get
        Set(ByVal value As Integer)
            mcodeid = value
        End Set
    End Property

    Public Property ccid() As Integer
        Get
            Return mccid
        End Get
        Set(ByVal value As Integer)
            mccid = value
        End Set
    End Property

    Public Property expression() As String
        Get
            Return mexpression
        End Get
        Set(ByVal value As String)
            mexpression = value
        End Set
    End Property

    Public Property groupid() As Integer
        Get
            Return mgroupid
        End Get
        Set(ByVal value As Integer)
            mgroupid = value
        End Set
    End Property

    Public Property groupname() As String
        Get
            Return mgroupname
        End Get
        Set(ByVal value As String)
            mgroupname = value
        End Set
    End Property

    Public Property cubit() As Nullable(Of Boolean)
        Get
            Return mcubit
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mcubit = value
        End Set
    End Property

    'R15 CR
    Public Property groupstatus() As String
        Get
            Return mgroupstatus
        End Get
        Set(ByVal value As String)
            mgroupstatus = value
        End Set
    End Property

    'R15 CR
    Public Property groupinitials() As String
        Get
            Return mgroupinitials
        End Get
        Set(ByVal value As String)
            mgroupinitials = value
        End Set
    End Property

    Private Shared Function makeGroupFromRow( _
            ByVal r As IDataReader _
        ) As clsGroup
        Return New clsGroup( _
                clsStuff.notWholeNumber(r.Item("groupid")), _
                clsStuff.notString(r.Item("groupname")))
    End Function

    Private Shared Function makeGroupFromRowCC( _
            ByVal r As IDataReader _
        ) As clsGroup
        Return New clsGroup( _
                clsStuff.notWholeNumber(r.Item("ccid")), _
                clsStuff.notWholeNumber(r.Item("groupid")), _
                clsStuff.notString(r.Item("expression")))
    End Function
    'R15 CR
    Private Shared Function makeGroupFromRowCubit( _
            ByVal r As IDataReader _
        ) As clsGroup
        Return New clsGroup( _
                clsStuff.notWholeNumber(r.Item("groupid")), _
                clsStuff.notString(r.Item("groupname")), _
                clsStuff.notBoolean(r.Item("cubit")), _
                clsStuff.notString(r.Item("groupstatus")), _
                clsStuff.notString(r.Item("groupinitials")))
    End Function

    Public Shared Function list() As List(Of clsGroup)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsGroup)()
            Using r As IDataReader = dbh.callSP("group_list", "@databasename", getConfig("databasename"))
                While r.Read()
                    ret.Add(makeGroupFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function
    'R15 CR
    Public Shared Function listwithcubit() As List(Of clsGroup)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsGroup)()
            Using r As IDataReader = dbh.callSP("group_listwithcubit", "@databasename", getConfig("databasename"))
                While r.Read()
                    ret.Add(makeGroupFromRowCubit(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function CostCodeExpList(ByVal pintgroupid As Integer) As List(Of clsGroup)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsGroup)()
            Using r As IDataReader = dbh.callSP("CostCodeExp_get", "@groupid", pintgroupid)
                While r.Read()
                    ret.Add(makeGroupFromRowCC(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function getBossCode(ByVal pstreoecode As String) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim expression As String = clsStuff.notString(dbh.callSPSingleValueCanReturnNothing _
                                                          ("getBossCode", _
                                                           "@databasename", getConfig("databasename"), _
                                                           "@eoecode", pstreoecode))
            Return expression
        End Using
    End Function

    Public Shared Function checkPurposeCode(ByVal pstrcode As String) As Boolean
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim expression As Boolean = clsStuff.notBoolean(dbh.callSPSingleValueCanReturnNothing _
                                                          ("checkPurposeCode", _
                                                           "@code", pstrcode))
            Return expression
        End Using
    End Function

    Public Shared Function getEOEFormat(ByVal pstreoecode As String) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim expression As String = clsStuff.notString(dbh.callSPSingleValueCanReturnNothing _
                                                          ("getEOEFormat", _
                                                           "@databasename", getConfig("databasename"), _
                                                           "@eoecode", pstreoecode))
            Return expression
        End Using
    End Function

    Public Shared Function getEOEFormatType(ByVal pstreoecode As String) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim expression As String = clsStuff.notString(dbh.callSPSingleValueCanReturnNothing _
                                                          ("getEOEFormatType", _
                                                           "@databasename", getConfig("databasename"), _
                                                           "@eoecode", pstreoecode))
            Return expression
        End Using
    End Function

    Public Shared Function CostCodeExpGet(ByVal pintgroupid As Integer) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim expression As String = clsStuff.notString(dbh.callSPSingleValueCanReturnNothing _
                                                          ("CostCodeExp_get", "@groupid", pintgroupid))
            Return expression
        End Using
    End Function

    Public Shared Function AnchorCostCodeCheck(ByVal pstrCostCode As String, ByVal pDb As Integer) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing _
                                                          ("AnchorCostCode_Check", _
                                                           "@CostCode", pstrCostCode, _
                                                           "@db", pDb))
            Return ret
        End Using
    End Function

    Public Shared Function EOECostCentreCheck(ByVal pstrCostCentre As String) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As String = clsStuff.notString(dbh.callSPSingleValueCanReturnNothing _
                                                          ("EOECostCentre_Check", _
                                                           "@CostCentre", pstrCostCentre))
            Return ret
        End Using
    End Function

    Public Shared Function UHSMCostCodeCheck(ByVal pstrCostCode As String, ByVal pDb As Integer) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As String = clsStuff.notString(dbh.callSPSingleValueCanReturnNothing _
                                                          ("UHSMCostCode_Check", _
                                                           "@CostCode", pstrCostCode, _
                                                           "@db", pDb))
            Return ret
        End Using
    End Function

    Public Shared Function UHSMCostCodeCheckExists(ByVal pstrCostCode As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing _
                                                          ("UHSMCostCodeExists_Check", _
                                                           "@CostCode", pstrCostCode))
            Return ret
        End Using
    End Function

    Public Shared Function NPSAProjectCodeExists(ByVal pstrProjectCode As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing _
                                                          ("NPSAProjectCode_Check", _
                                                           "@ProjectCode", pstrProjectCode))
            Return ret
        End Using
    End Function

    Public Shared Function DCCostCodeExists(ByVal pstrCostCode As String) As Boolean
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As Boolean = clsStuff.notBoolean(dbh.callSPSingleValueCanReturnNothing _
                                                          ("DCCostCode_Check", _
                                                           "@CostCode", pstrCostCode))
            Return ret
        End Using
    End Function

    'R2.16 CR
    Public Shared Function EACTCostCodeExists(ByVal pstrEACTCode As String) As Boolean
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As Boolean = clsStuff.notBoolean(dbh.callSPSingleValueCanReturnNothing _
                                                     ("EACTCodeCode_Check", _
                                                      "@EACTCode", pstrEACTCode))
            Return ret
        End Using

    End Function

    Public Shared Function CostCodeExpFormatGet(ByVal pintgroupid As Integer) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim expressionFormat As String = clsStuff.notString(dbh.callSPSingleValueCanReturnNothing _
                                                          ("CostCodeExpFormat_get", "@groupid", pintgroupid))
            Return expressionFormat
        End Using
    End Function

    Public Shared Function getGroupIDFromName( _
               ByVal pgroupname As String _
           ) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intID As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing("group_getIDfromName", _
                                                            "@groupname", pgroupname, _
                                                            "@databasename", getConfig("databasename")))
            Return intID
        End Using
    End Function

    'Public Shared Function getGroupIDsFromName(ByVal pcompanyname As String) As List(Of clsGroup)
    '    Using dbh As New SqlDatabaseHandle(getConnection)
    '        Dim ret As New List(Of clsGroup)()
    '        Using r As IDataReader = dbh.callSP("group_getIDsfromName", _
    '                                            "@companyname", pcompanyname, _
    '                                            "@databasename", getConfig("databasename"))
    '            While r.Read()
    '                ret.Add(makeGroupFromRowBothIDs(r))
    '            End While
    '        End Using
    '        Return ret
    '    End Using
    'End Function

    'R2.20G SA
    Public Shared Function NHSLACostCodeCheckExists(ByVal pstrCostCode As String) As Integer
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim ret As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing _
                                                          ("NHSLACostCodeExists_Check", _
                                                           "@CostCode", pstrCostCode))
            Return ret
        End Using
    End Function

    'R2.20G SA
    Public Shared Function NHSLAExpenseTypeExists(ByVal pstrExpenseType As String) As Integer
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim ret As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing _
                                                          ("NHSLAExpenseTypeExists_Check", _
                                                           "@ExpenseType", pstrExpenseType))
            Return ret
        End Using
    End Function

    ''R2.21 SA 
    'Public Shared Function getCompanyID(ByVal pstrCompanyName As String) As Integer
    '    Using dbh As New SqlDatabaseHandle(getMevisConnection)
    '        Dim intRet As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing _
    '                                                       ("group_getID", _
    '                                                        "@companyname", pstrCompanyName))

    '        Return intRet
    '    End Using
    'End Function

End Class

