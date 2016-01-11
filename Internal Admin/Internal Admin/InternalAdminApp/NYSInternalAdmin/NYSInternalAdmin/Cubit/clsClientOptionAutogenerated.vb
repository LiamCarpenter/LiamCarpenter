Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class clsClientOption

    'R2.21.1 SA - added APT, APTFee, CX
    'R2.21 SA - added BossOption, OOHFee, OOH
    Public Sub New( _
        ByVal pClientOptionsID As Integer, _
        ByVal pClientID As Nullable(Of Integer), _
        ByVal pTransactionType As String, _
        ByVal pSendGross As Nullable(Of Boolean), _
        ByVal pTransactionExVat As Nullable(Of Boolean), _
        ByVal pExtrasCharges As Nullable(Of Integer), _
        ByVal paicostcodeValue As String, _
        ByVal paicol6Value As String, _
        ByVal paicol7Value As String, _
        ByVal paicol8Value As String, _
        ByVal paicol9Value As String, _
        ByVal paicol10Value As String, _
        ByVal pAdditional As Nullable(Of Boolean), _
        ByVal pAdditionalFee As Decimal, _
        ByVal pinvoicefee As Decimal, _
        ByVal pBossOption As String, _
        ByVal pOOH As Nullable(Of Boolean), _
        ByVal pOOHFee As Decimal, _
        ByVal pAPT As Nullable(Of Boolean), _
        ByVal pAPTFee As Decimal, _
        ByVal pCX As Nullable(Of Boolean))
        mClientOptionsID = pClientOptionsID
        mClientID = pClientID
        mTransactionType = pTransactionType
        mSendGross = pSendGross
        mTransactionExVat = pTransactionExVat
        mExtrasCharges = pExtrasCharges
        maicostcodeValue = paicostcodeValue
        maicol6Value = paicol6Value
        maicol7Value = paicol7Value
        maicol8Value = paicol8Value
        maicol9Value = paicol9Value
        maicol10Value = paicol10Value
        mAdditional = pAdditional
        mAdditionalfee = pAdditionalFee
        minvoicefee = pinvoicefee
        'R2.21 SA 
        mBossOption = pBossOption
        mOOH = pOOH
        mOOHFee = pOOHFee

        'R2.21.1 SA 
        mAPT = pAPT
        mAPTFee = pAPTFee
        mCX = pCX
    End Sub

    Public Sub New( _
)
    End Sub

    Private mClientOptionsID As Integer
    Private mClientID As Nullable(Of Integer)
    Private mExtrasCharges As Nullable(Of Integer)
    Private mTransactionType As String
    Private maicostcodeValue As String
    Private maicol6Value As String
    Private maicol7Value As String
    Private maicol8Value As String
    Private maicol9Value As String
    Private maicol10Value As String
    Private mSendGross As Nullable(Of Boolean)
    Private mTransactionExVat As Nullable(Of Boolean)
    Private mAdditional As Nullable(Of Boolean)
    Private minvoicefee As Decimal
    Private mAdditionalfee As Decimal
    'R2.21 SA 
    Private mBossOption As String
    Private mOOH As Nullable(Of Boolean)
    Private mOOHFee As Decimal

    'R2.21.1 SA 
    Private mAPT As Nullable(Of Boolean) 'serviced apartments
    Private mAPTFee As Decimal 'serviced apartment charge 
    Private mCX As Nullable(Of Boolean) 'for cancelled booking 

    Public Property ClientOptionsID() As Integer
        Get
            Return mClientOptionsID
        End Get
        Set(ByVal value As Integer)
            mClientOptionsID = value
        End Set
    End Property

    Public Property ExtrasCharges() As Nullable(Of Integer)
        Get
            Return mExtrasCharges
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mExtrasCharges = value
        End Set
    End Property

    Public Property ClientID() As Nullable(Of Integer)
        Get
            Return mClientID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mClientID = value
        End Set
    End Property

    Public Property TransactionType() As String
        Get
            Return mTransactionType
        End Get
        Set(ByVal value As String)
            mTransactionType = value
        End Set
    End Property

    Public Property aicostcodeValue() As String
        Get
            Return maicostcodeValue
        End Get
        Set(ByVal value As String)
            maicostcodeValue = value
        End Set
    End Property

    Public Property aicol6Value() As String
        Get
            Return maicol6Value
        End Get
        Set(ByVal value As String)
            maicol6Value = value
        End Set
    End Property

    Public Property aicol7Value() As String
        Get
            Return maicol7Value
        End Get
        Set(ByVal value As String)
            maicol7Value = value
        End Set
    End Property

    Public Property aicol8Value() As String
        Get
            Return maicol8Value
        End Get
        Set(ByVal value As String)
            maicol8Value = value
        End Set
    End Property

    Public Property aicol9Value() As String
        Get
            Return maicol9Value
        End Get
        Set(ByVal value As String)
            maicol9Value = value
        End Set
    End Property

    Public Property aicol10Value() As String
        Get
            Return maicol10Value
        End Get
        Set(ByVal value As String)
            maicol10Value = value
        End Set
    End Property

    Public Property SendGross() As Nullable(Of Boolean)
        Get
            Return mSendGross
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSendGross = value
        End Set
    End Property

    Public Property additional() As Nullable(Of Boolean)
        Get
            Return mAdditional
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mAdditional = value
        End Set
    End Property

    Public Property invoicefee() As Decimal
        Get
            Return minvoicefee
        End Get
        Set(ByVal value As Decimal)
            minvoicefee = value
        End Set
    End Property

    Public Property additionalfee() As Decimal
        Get
            Return mAdditionalfee
        End Get
        Set(ByVal value As Decimal)
            mAdditionalfee = value
        End Set
    End Property

    Public Property TransactionExVat() As Nullable(Of Boolean)
        Get
            Return mTransactionExVat
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mTransactionExVat = value
        End Set
    End Property

    'R2.21 SA 
    Public Property BossOption() As String
        Get
            Return mBossOption
        End Get
        Set(ByVal value As String)
            mBossOption = value
        End Set
    End Property

    'R2.21 SA 
    Public Property OOH() As Nullable(Of Boolean)
        Get
            Return mOOH
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mOOH = value
        End Set
    End Property

    'R2.21 SA 
    Public Property OOHFee() As Decimal
        Get
            Return mOOHFee
        End Get
        Set(ByVal value As Decimal)
            mOOHFee = value
        End Set
    End Property

    'R2.21.1 SA 
    Public Property APT() As Nullable(Of Boolean)
        Get
            Return mAPT
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mAPT = value
        End Set
    End Property

    'R2.21.1 SA 
    Public Property APTFee() As Decimal
        Get
            Return mAPTFee
        End Get
        Set(ByVal value As Decimal)
            mAPTFee = value
        End Set
    End Property

    Public Property CX() As Nullable(Of Boolean)
        Get
            Return mCX
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mCX = value
        End Set
    End Property

    'R2.21.1 SA - added APT. APTFee, CX
    'R2.21 SA - added OOH, OOHFee, BossOption 
    Private Shared Function makeClientOptionFromRow( _
            ByVal r As IDataReader _
        ) As clsClientOption
        Return New clsClientOption( _
                CInt(r.Item("ClientOptionsID")), _
                toNullableInteger(r.Item("ClientID")), _
                clsStuff.notString(r.Item("TransactionType")), _
                toNullableBoolean(r.Item("SendGross")), _
                toNullableBoolean(r.Item("TransactionExVat")), _
                toNullableInteger(r.Item("ExtrasCharges")), _
                clsStuff.notString(r.Item("aicostcodevalue")), _
                clsStuff.notString(r.Item("aicol6Value")), _
                clsStuff.notString(r.Item("aicol7Value")), _
                clsStuff.notString(r.Item("aicol8Value")), _
                clsStuff.notString(r.Item("aicol9Value")), _
                clsStuff.notString(r.Item("aicol10Value")), _
                toNullableBoolean(r.Item("additional")), _
                clsStuff.notDecimal(r.Item("additionalfee")), _
                clsStuff.notDecimal(r.Item("invoicefee")), _
                clsStuff.notString(r.Item("BossOption")), _
                toNullableBoolean(r.Item("OOH")), _
                clsStuff.notDecimal(r.Item("OOHFee")), _
                toNullableBoolean(r.Item("APT")), _
                clsStuff.notDecimal(r.Item("APTFee")), _
                toNullableBoolean(r.Item("CX")))
    End Function

    Public Shared Function [get]( _
            ByVal pClientID As Integer _
        ) As clsClientOption
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("ClientOptions_get", "@ClientID", pClientID)
                Dim ret As New clsClientOption
                If r.Read() Then
                    ret = makeClientOptionFromRow(r)
                End If
                Return ret
            End Using
        End Using
    End Function

    'R2.21.1 SA - added APT, APTFee, CX
    'R2.21 SA - added BossOption, OOH, OOHFee
    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mClientOptionsID = clsStuff.notWholeNumber(dbh.callSPSingleValue("ClientOptions_save", _
                                                          "@ClientOptionsID", mClientOptionsID, _
                                                          "@ClientID", mClientID, _
                                                          "@TransactionType", mTransactionType, _
                                                          "@SendGross", mSendGross, _
                                                          "@TransactionExVat", mTransactionExVat, _
                                                          "@ExtrasCharges", mExtrasCharges, _
                                                          "@aicostcodeValue", maicostcodeValue, _
                                                          "@aicol6Value", maicol6Value, _
                                                          "@aicol7Value", maicol7Value, _
                                                          "@aicol8Value", maicol8Value, _
                                                          "@aicol9Value", maicol9Value, _
                                                          "@aicol10Value", maicol10Value, _
                                                          "@additional", mAdditional, _
                                                          "@additionalfee", mAdditionalfee, _
                                                          "@invoicefee", minvoicefee, _
                                                          "@BossOption", mBossOption, _
                                                          "@OOH", mOOH, _
                                                          "@OOHFee", mOOHFee, _
                                                          "@APT", mAPT, _
                                                          "@APTFee", mAPTFee,
                                                          "@CX", mCX))
            Return mClientOptionsID
        End Using
    End Function

    Public Shared Sub delete( _
            ByVal pClientOptionsID As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("ClientOptions_delete", "@ClientOptionsID", pClientOptionsID)
        End Using
    End Sub

    Public Sub delete()
        delete(mClientOptionsID)
    End Sub

    Public Shared Function getFee(ByVal pClientID As Integer) As Decimal
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim retValue As Decimal = clsStuff.notDecimal(dbh.callSPSingleValueCanReturnNothing("getFee", _
                                                                                                "@ClientID", pClientID))
            Return retValue
        End Using
    End Function

    Public Shared Function checkCategory(ByVal pTransactionNumber As Integer, ByVal pGuestPNR As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim retValue As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing("checkCategory", _
                                                                                                    "@TransactionNumber", pTransactionNumber, _
                                                                                                    "@GuestPNR", pGuestPNR))
            Return retValue
        End Using
    End Function

    'R2.21 SA 
    Public Shared Function getBossOption(ByVal pClientID As Integer) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim strRet As String = clsStuff.notString(dbh.callSPSingleValueCanReturnNothing("ClientOption_getBossOption", _
                                                                                            "@ClientID", pClientID))
            Return strRet
        End Using
    End Function

    'R2.21 SA 
    Public Shared Function checkOOHFee(ByVal pClientID As Integer) As Boolean
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim blnRet As Boolean
            blnRet = clsStuff.notBoolean(dbh.callSPSingleValueCanReturnNothing("ClientOption_checkOOHFee", _
                                                                                "@ClientID", pClientID))
            Return blnRet
        End Using
    End Function

    'R2.21 SA
    Public Shared Function getOOHFee(ByVal pClientID As Integer) As Decimal
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim retValue As Decimal = clsStuff.notDecimal(dbh.callSPSingleValueCanReturnNothing("ClientOption_getOOHFee", _
                                                                                                 "@ClientID", pClientID))
            Return retValue
        End Using
    End Function

    'R2.21.1 SA 
    Public Shared Function checkAPTFee(ByVal pClientID As Integer) As Boolean
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim blnRet As Boolean
            blnRet = clsStuff.notBoolean(dbh.callSPSingleValueCanReturnNothing("ClientOption_checkAPTFee", _
                                                                                "@ClientID", pClientID))
            Return blnRet
        End Using
    End Function

    'R2.21.1 SA
    Public Shared Function getAPTFee(ByVal pClientID As Integer) As Decimal
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim retValue As Decimal = clsStuff.notDecimal(dbh.callSPSingleValueCanReturnNothing("ClientOption_getAPTFee", _
                                                                                                 "@ClientID", pClientID))
            Return retValue
        End Using
    End Function

    'R2.21.1 SA 
    Public Shared Function checkCXFee(ByVal pClientID As Integer) As Boolean
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim blnRet As Boolean
            blnRet = clsStuff.notBoolean(dbh.callSPSingleValueCanReturnNothing("ClientOption_checkCXFee", _
                                                                                "@ClientID", pClientID))
            Return blnRet
        End Using
    End Function

    'R2.21.1 SA 
    Public Shared Function getCXFee(ByVal pClientID As Integer, ByVal pParameterID As Integer,
                                    ByVal pTransactionID As Integer) As Decimal
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim retValue As Decimal = clsStuff.notDecimal(dbh.callSPSingleValueCanReturnNothing("ClientOption_getCXFee", _
                                                                                                 "@ClientID", pClientID, _
                                                                                                 "@ParameterID", pParameterID, _
                                                                                                 "@TransactionID", pTransactionID))
            Return retValue
        End Using
    End Function


End Class
