Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class DWPCostcentre

    Public Sub New( _
        ByVal pDWPCostcentreID As Integer, _
        ByVal pBusinessunit As String, _
        ByVal pCostcentre As Nullable(Of Integer), _
        ByVal pDescription As String)
        mDWPCostcentreID = pDWPCostcentreID
        mBusinessunit = pBusinessunit
        mCostcentre = pCostcentre
        mDescription = pDescription
    End Sub

    Public Sub New( _
)
    End Sub

    Private mDWPCostcentreID As Integer
    Private mBusinessunit As String
    Private mCostcentre As Nullable(Of Integer)
    Private mDescription As String

    Public Property DWPCostcentreID() As Integer
        Get
            Return mDWPCostcentreID
        End Get
        Set(ByVal value As Integer)
            mDWPCostcentreID = value
        End Set
    End Property

    Public Property Businessunit() As String
        Get
            Return mBusinessunit
        End Get
        Set(ByVal value As String)
            mBusinessunit = value
        End Set
    End Property

    Public Property Costcentre() As Nullable(Of Integer)
        Get
            Return mCostcentre
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mCostcentre = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return mDescription
        End Get
        Set(ByVal value As String)
            mDescription = value
        End Set
    End Property

    Private Shared Function makeDWPCostcentreFromRow( _
            ByVal r As IDataReader _
        ) As DWPCostcentre
        Return New DWPCostcentre( _
                clsNYS.notInteger(r.Item("DWPCostcentreID")), _
                clsNYS.notString(r.Item("businessunit")), _
                toNullableInteger(r.Item("costcentre")), _
                clsNYS.notString(r.Item("description")))
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mDWPCostcentreID = CInt(dbh.callSPSingleValue("DWPCostcentre_save", "@DWPCostcentreID", mDWPCostcentreID, "@Businessunit", mBusinessunit, _
                                                          "@Costcentre", mCostcentre, "@Description", mDescription))
            Return mDWPCostcentreID
        End Using
    End Function

    Public Shared Sub delete()
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("DWPCostcentre_delete")
        End Using
    End Sub

End Class
