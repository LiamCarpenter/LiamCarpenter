Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class ClientMapping

    Public Sub New( _
        ByVal pClientMappingsID As Integer, _
        ByVal pClientID As Nullable(Of Integer), _
        ByVal pCostCentreValue As String, _
        ByVal pCostCentreDefault As String, _
        ByVal pPOValue As String, _
        ByVal pPODefault As String, _
        ByVal pCREF1Value As String, _
        ByVal pCREF1Default As String, _
        ByVal pCREF2Value As String, _
        ByVal pCREF2Default As String, _
        ByVal pCREF3Value As String, _
        ByVal pCREF3Default As String, _
        ByVal pCREF4Value As String, _
        ByVal pCREF4Default As String, _
        ByVal pCREF5Value As String, _
        ByVal pCREF5Default As String, _
        ByVal pCREF6Value As String, _
        ByVal pCREF6Default As String, _
        ByVal pCREF7Value As String, _
        ByVal pCREF7Default As String, _
        ByVal pCREF8Value As String, _
        ByVal pCREF8Default As String, _
        ByVal pCREF9Value As String, _
        ByVal pCREF9Default As String)
        mClientMappingsID = pClientMappingsID
        mClientID = pClientID
        mCostCentreValue = pCostCentreValue
        mCostCentreDefault = pCostCentreDefault
        mPOValue = pPOValue
        mPODefault = pPODefault
        mCREF1Value = pCREF1Value
        mCREF1Default = pCREF1Default
        mCREF2Value = pCREF2Value
        mCREF2Default = pCREF2Default
        mCREF3Value = pCREF3Value
        mCREF3Default = pCREF3Default
        mCREF4Value = pCREF4Value
        mCREF4Default = pCREF4Default
        mCREF5Value = pCREF5Value
        mCREF5Default = pCREF5Default
        mCREF6Value = pCREF6Value
        mCREF6Default = pCREF6Default
        mCREF7Value = pCREF7Value
        mCREF7Default = pCREF7Default
        mCREF8Value = pCREF8Value
        mCREF8Default = pCREF8Default
        mCREF9Value = pCREF9Value
        mCREF9Default = pCREF9Default
    End Sub

    Public Sub New( _
)
    End Sub

    Private mClientMappingsID As Integer
    Private mClientID As Nullable(Of Integer)
    Private mCostCentreValue As String
    Private mPOValue As String
    Private mCREF1Value As String
    Private mCREF2Value As String
    Private mCREF3Value As String
    Private mCREF4Value As String
    Private mCREF5Value As String
    Private mCREF6Value As String
    Private mCREF7Value As String
    Private mCREF8Value As String
    Private mCREF9Value As String
    Private mCostCentreDefault As String
    Private mPODefault As String
    Private mCREF1Default As String
    Private mCREF2Default As String
    Private mCREF3Default As String
    Private mCREF4Default As String
    Private mCREF5Default As String
    Private mCREF6Default As String
    Private mCREF7Default As String
    Private mCREF8Default As String
    Private mCREF9Default As String

    Public Property ClientMappingsID() As Integer
        Get
            Return mClientMappingsID
        End Get
        Set(ByVal value As Integer)
            mClientMappingsID = value
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

    Public Property CostCentreValue() As String
        Get
            Return mCostCentreValue
        End Get
        Set(ByVal value As String)
            mCostCentreValue = value
        End Set
    End Property

    Public Property POValue() As String
        Get
            Return mPOValue
        End Get
        Set(ByVal value As String)
            mPOValue = value
        End Set
    End Property

    Public Property CREF1Value() As String
        Get
            Return mCREF1Value
        End Get
        Set(ByVal value As String)
            mCREF1Value = value
        End Set
    End Property

    Public Property CREF2Value() As String
        Get
            Return mCREF2Value
        End Get
        Set(ByVal value As String)
            mCREF2Value = value
        End Set
    End Property

    Public Property CREF3Value() As String
        Get
            Return mCREF3Value
        End Get
        Set(ByVal value As String)
            mCREF3Value = value
        End Set
    End Property

    Public Property CREF4Value() As String
        Get
            Return mCREF4Value
        End Get
        Set(ByVal value As String)
            mCREF4Value = value
        End Set
    End Property

    Public Property CREF5Value() As String
        Get
            Return mCREF5Value
        End Get
        Set(ByVal value As String)
            mCREF5Value = value
        End Set
    End Property

    Public Property CREF6Value() As String
        Get
            Return mCREF6Value
        End Get
        Set(ByVal value As String)
            mCREF6Value = value
        End Set
    End Property

    Public Property CREF7Value() As String
        Get
            Return mCREF7Value
        End Get
        Set(ByVal value As String)
            mCREF7Value = value
        End Set
    End Property

    Public Property CREF8Value() As String
        Get
            Return mCREF8Value
        End Get
        Set(ByVal value As String)
            mCREF8Value = value
        End Set
    End Property

    Public Property CREF9Value() As String
        Get
            Return mCREF9Value
        End Get
        Set(ByVal value As String)
            mCREF9Value = value
        End Set
    End Property

    Public Property CostCentreDefault() As String
        Get
            Return mCostCentreDefault
        End Get
        Set(ByVal value As String)
            mCostCentreDefault = value
        End Set
    End Property

    Public Property PODefault() As String
        Get
            Return mPODefault
        End Get
        Set(ByVal value As String)
            mPODefault = value
        End Set
    End Property

    Public Property CREF1Default() As String
        Get
            Return mCREF1Default
        End Get
        Set(ByVal value As String)
            mCREF1Default = value
        End Set
    End Property

    Public Property CREF2Default() As String
        Get
            Return mCREF2Default
        End Get
        Set(ByVal value As String)
            mCREF2Default = value
        End Set
    End Property

    Public Property CREF3Default() As String
        Get
            Return mCREF3Default
        End Get
        Set(ByVal value As String)
            mCREF3Default = value
        End Set
    End Property

    Public Property CREF4Default() As String
        Get
            Return mCREF4Default
        End Get
        Set(ByVal value As String)
            mCREF4Default = value
        End Set
    End Property

    Public Property CREF5Default() As String
        Get
            Return mCREF5Default
        End Get
        Set(ByVal value As String)
            mCREF5Default = value
        End Set
    End Property

    Public Property CREF6Default() As String
        Get
            Return mCREF6Default
        End Get
        Set(ByVal value As String)
            mCREF6Default = value
        End Set
    End Property

    Public Property CREF7Default() As String
        Get
            Return mCREF7Default
        End Get
        Set(ByVal value As String)
            mCREF7Default = value
        End Set
    End Property

    Public Property CREF8Default() As String
        Get
            Return mCREF8Default
        End Get
        Set(ByVal value As String)
            mCREF8Default = value
        End Set
    End Property

    Public Property CREF9Default() As String
        Get
            Return mCREF9Default
        End Get
        Set(ByVal value As String)
            mCREF9Default = value
        End Set
    End Property

    Private Shared Function makeClientMappingFromRow( _
            ByVal r As IDataReader _
        ) As ClientMapping
        Return New ClientMapping( _
                clsStuff.notWholeNumber(r.Item("ClientMappingsID")), _
                toNullableInteger(r.Item("ClientID")), _
                clsStuff.notString(r.Item("CostCentreValue")), _
                clsStuff.notString(r.Item("CostCentreDefault")), _
                clsStuff.notString(r.Item("POValue")), _
                clsStuff.notString(r.Item("PODefault")), _
                clsStuff.notString(r.Item("CREF1Value")), _
                clsStuff.notString(r.Item("CREF1Default")), _
                clsStuff.notString(r.Item("CREF2Value")), _
                clsStuff.notString(r.Item("CREF2Default")), _
                clsStuff.notString(r.Item("CREF3Value")), _
                clsStuff.notString(r.Item("CREF3Default")), _
                clsStuff.notString(r.Item("CREF4Value")), _
                clsStuff.notString(r.Item("CREF4Default")), _
                clsStuff.notString(r.Item("CREF5Value")), _
                clsStuff.notString(r.Item("CREF5Default")), _
                clsStuff.notString(r.Item("CREF6Value")), _
                clsStuff.notString(r.Item("CREF6Default")), _
                clsStuff.notString(r.Item("CREF7Value")), _
                clsStuff.notString(r.Item("CREF7Default")), _
                clsStuff.notString(r.Item("CREF8Value")), _
                clsStuff.notString(r.Item("CREF8Default")), _
                clsStuff.notString(r.Item("CREF9Value")), _
                clsStuff.notString(r.Item("CREF9Default")))
    End Function

    Public Shared Function getClientMapping(ByVal pClientID As Integer) As ClientMapping
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("ClientMappings_get", "@ClientID", pClientID)
                Dim ret As New ClientMapping
                If r.Read() Then
                    ret = makeClientMappingFromRow(r)
                End If
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of ClientMapping)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of ClientMapping)()
            Using r As IDataReader = dbh.callSP("ClientMappings_list")
                While r.Read()
                    ret.Add(makeClientMappingFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save(ByVal pClientMappingsID As Integer, _
                         ByVal pClientID As Integer, _
                         ByVal pCostCentreValue As String, _
                         ByVal pCostCentreDefault As String, _
                         ByVal pmPOValue As String, _
                         ByVal pmPODefault As String, _
                         ByVal pCREF1Value As String, _
                         ByVal pCREF1Default As String, _
                         ByVal pCREF2Value As String, _
                         ByVal pCREF2Default As String, _
                         ByVal pCREF3Value As String, _
                         ByVal pCREF3Default As String, _
                         ByVal pCREF4Value As String, _
                         ByVal pCREF4Default As String, _
                         ByVal pCREF5Value As String, _
                         ByVal pCREF5Default As String, _
                         ByVal pCREF6Value As String, _
                         ByVal pCREF6Default As String, _
                         ByVal pCREF7Value As String, _
                         ByVal pCREF7Default As String, _
                         ByVal pCREF8Value As String, _
                         ByVal pCREF8Default As String, _
                         ByVal pCREF9Value As String, _
                         ByVal pCREF9Default As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mClientMappingsID = CInt(dbh.callSPSingleValue("ClientMappings_save", _
                                                           "@ClientMappingsID", pClientMappingsID, _
                                                           "@ClientID", pClientID, _
                                                           "@CostCentreValue", pCostCentreValue, _
                                                           "@CostCentreDefault", pCostCentreDefault, _
                                                           "@POValue", pmPOValue, _
                                                           "@PODefault", pmPODefault, _
                                                           "@CREF1Value", pCREF1Value, _
                                                           "@CREF1Default", pCREF1Default, _
                                                           "@CREF2Value", pCREF2Value, _
                                                           "@CREF2Default", pCREF2Default, _
                                                           "@CREF3Value", pCREF3Value, _
                                                           "@CREF3Default", pCREF3Default, _
                                                           "@CREF4Value", pCREF4Value, _
                                                           "@CREF4Default", pCREF4Default, _
                                                           "@CREF5Value", pCREF5Value, _
                                                           "@CREF5Default", pCREF5Default, _
                                                           "@CREF6Value", pCREF6Value, _
                                                           "@CREF6Default", pCREF6Default, _
                                                           "@CREF7Value", pCREF7Value, _
                                                           "@CREF7Default", pCREF7Default, _
                                                           "@CREF8Value", pCREF8Value, _
                                                           "@CREF8Default", pCREF8Default, _
                                                           "@CREF9Value", pCREF9Value, _
                                                           "@CREF9Default", pCREF9Default))
            Return mClientMappingsID
        End Using
    End Function

    Public Shared Sub delete( _
            ByVal pClientMappingsID As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("ClientMappings_delete", "@ClientMappingsID", pClientMappingsID)
        End Using
    End Sub

    Public Sub delete()
        delete(mClientMappingsID)
    End Sub

End Class
