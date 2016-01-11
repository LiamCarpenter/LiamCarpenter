Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils
Imports Microsoft.VisualBasic
Imports System.Data
Public Class SsoCodeList
    Public Sub New()
    End Sub

    Public Sub New(pCodeID As Integer,
                   pCode As String,
                   pCodeName As String,
                   pCodeType As String,
                   pClientID As Integer,
                   pIsDuplicate As Boolean)
        mCodeID = pCodeID
        mCode = pCode
        mCodeName = pCodeName
        mCodeType = pCodeType
        mSsoClientID = pClientID
        mIsDuplicate = pIsDuplicate
    End Sub

    Private mCodeID As Integer
    Private mCode As String
    Private mCodeName As String
    Private mCodeType As String
    Private mSsoClientID As Integer
    Private mIsDuplicate As Boolean

    Public Property CodeID As Integer
        Get
            Return mCodeID
        End Get
        Set(value As Integer)
            mCodeID = value
        End Set
    End Property

    Public Property Code As String
        Get
            Return mCode
        End Get
        Set(value As String)
            mCode = value
        End Set
    End Property

    Public Property CodeName As String
        Get
            Return mCodeName
        End Get
        Set(value As String)
            mCodeName = value
        End Set
    End Property

    Public Property CodeType As String
        Get
            Return mCodeType
        End Get
        Set(value As String)
            mCodeType = value
        End Set
    End Property

    Public Property SsoClientID As Integer
        Get
            Return mSsoClientID
        End Get
        Set(value As Integer)
            mSsoClientID = value
        End Set
    End Property

    Public Property CodeIsDuplicate As Boolean
        Get
            Return mIsDuplicate
        End Get
        Set(value As Boolean)
            mIsDuplicate = value
        End Set
    End Property

    Private Shared Function makeSsoCodeListFromRow( _
        ByVal r As IDataReader _
    ) As SsoCodeList
        Return New SsoCodeList( _
            r.Item("CodeID"), _
            r.Item("Code"), _
            r.Item("CodeName"), _
            r.Item("CodeType"), _
            r.Item("ssoClientID"), _
            r.Item("CodeIsDuplicate")
            )
    End Function

    ''' <summary>
    ''' Search for codes for the selected client
    ''' </summary>
    ''' <param name="pintClientID"></param>
    ''' <param name="pstrCode"></param>
    ''' <param name="pstrCodeName"></param>
    ''' <param name="pstrCodeType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function search(pintClientID As Integer, pstrCode As String, pstrCodeName As String, pstrCodeType As String) As List(Of SsoCodeList)
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            Dim ret As New List(Of SsoCodeList)
            Using r As IDataReader = dbh.callSP("ssoCodeList_Search", _
                                                "@ClientID", pintClientID, _
                                                "@Code", pstrCode, _
                                                "@CodeName", pstrCodeName, _
                                                "@CodeType", pstrCodeType)
                While r.Read()
                    ret.Add(makeSsoCodeListFromRow(r))
                End While
            End Using

            Return ret
        End Using
    End Function

    ''' <summary>
    ''' List unique code types for the specified client.
    ''' Returns a list of strings, not of class instances
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function listClientCodeTypes(pintClientID As Integer) As List(Of String)
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            Dim ret As New List(Of String)
            Using r As IDataReader = dbh.callSP("ssoCodeList_clientCodeTypes", _
                                                "@ClientID", pintClientID)
                While r.Read()
                    ret.Add(r.Item("CodeType"))
                End While
            End Using

            Return ret
        End Using
    End Function

    Public Shared Function listAll() As List(Of SsoCodeList)
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            Dim ret As New List(Of SsoCodeList)

            Using r As IDataReader = dbh.callSP("ssoCodeList_listAll")
                While r.Read()
                    Dim oSingleInstance As New SsoCodeList
                    With oSingleInstance
                        .CodeID = r.Item("CodeID")
                        .Code = r.Item("Code")
                        .CodeName = r.Item("CodeName")
                        .CodeType = r.Item("CodeType")
                        .SsoClientID = r.Item("ssoClientID")
                    End With
                    ret.Add(r.Item("CodeType"))
                End While
            End Using

            Return ret
        End Using
    End Function

    ''' <summary>
    ''' Save codes from the syncing service, doesn't do it by ID because ID's could be different.
    ''' Keeps all old codes too - which is good for feeder files
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub saveSync()
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            dbh.callNonQuerySP("ssoCodeList_saveSync", _
                               "@Code", mCode, _
                               "@CodeName", mCodeName, _
                               "@CodeType", mCodeType, _
                               "@ssoClientID", mSsoClientID, _
                               "@IsDuplicate", mIsDuplicate)
        End Using
    End Sub

    'R2.22.2 CR
    Public Shared Sub delete(pCode As String)
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            dbh.callNonQuerySP("ssoCodeList_delete", _
                               "@Code", pCode)
        End Using
    End Sub

End Class
