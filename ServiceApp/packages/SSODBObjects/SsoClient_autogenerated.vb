Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils
Imports Microsoft.VisualBasic
Imports System.Data

Partial Public Class SsoClient

    Public Sub New( _
        ByVal pSsoClientID As Integer, _
        ByVal pSsoClientName As String, _
        ByVal pSsoClientConfermaID As String, _
        ByVal pSsoClientSbookerUrl As String, _
        ByVal pSsoClientWebformsUrl As String, _
        ByVal pSsoClientEtravelUrl As String, _
        ByVal pSsoClientEtravelKey As String, _
        ByVal pSsoClientEtravelKeyEnc As String, _
        ByVal pSsoClientEtravelSiteCode As String, _
        ByVal pSsoClientEvolviUrl As String, _
        ByVal pSsoClientMiUrl As String, _
        ByVal pSsoClientConfermaVersion As Integer, _
        ByVal pSsoClientPrefix As String, _
        ByVal pSsoClientPrefix2 As String,
        ByVal pssoClientIsLive As Boolean)
        mSsoClientID = pSsoClientID
        mSsoClientName = pSsoClientName
        mSsoClientConfermaID = pSsoClientConfermaID
        mSsoClientSbookerUrl = pSsoClientSbookerUrl
        mSsoClientWebformsUrl = pSsoClientWebformsUrl
        mSsoClientEtravelUrl = pSsoClientEtravelUrl
        mSsoClientEtravelKey = pSsoClientEtravelKey
        mSsoClientEtravelKeyEnc = pSsoClientEtravelKeyEnc
        mSsoClientEtravelSiteCode = pSsoClientEtravelSiteCode
        mSsoClientEvolviUrl = pSsoClientEvolviUrl
        mSsoClientMiUrl = pSsoClientMiUrl
        mSsoClientConfermaVersion = pSsoClientConfermaVersion
        mSsoClientPrefix = pSsoClientPrefix
        mSsoClientPrefix2 = pSsoClientPrefix2
        mssoClientIsLive = pssoClientIsLive
    End Sub

    Public Sub New()
    End Sub

    Private mSsoClientID As Integer
    Private mSsoClientName As String
    Private mSsoClientConfermaID As String
    Private mSsoClientSbookerUrl As String
    Private mSsoClientWebformsUrl As String
    Private mSsoClientEtravelUrl As String
    Private mSsoClientEtravelKey As String
    Private mSsoClientEtravelKeyEnc As String
    Private mSsoClientEvolviUrl As String
    Private mSsoClientMiUrl As String
    Private mSsoClientEtravelSiteCode As String
    Private mSsoClientConfermaVersion As Integer
    Private mSsoClientPrefix As String
    Private mSsoClientPrefix2 As String
    Private mssoClientIsLive As Boolean

    Public Property ssoClientIsLive() As String
        Get
            Return mssoClientIsLive
        End Get
        Set(ByVal value As String)
            mssoClientIsLive = value
        End Set
    End Property

    Public Property SsoClientPrefix2() As String
        Get
            Return mSsoClientPrefix2
        End Get
        Set(ByVal value As String)
            mSsoClientPrefix2 = value
        End Set
    End Property

    Public Property SsoClientPrefix() As String
        Get
            Return mSsoClientPrefix
        End Get
        Set(ByVal value As String)
            mSsoClientPrefix = value
        End Set
    End Property

    Public Property SsoClientConfermaVersion() As Integer
        Get
            Return mSsoClientConfermaVersion
        End Get
        Set(ByVal value As Integer)
            mSsoClientConfermaVersion = value
        End Set
    End Property

    Public Property SsoClientEtravelSiteCode() As String
        Get
            Return mSsoClientEtravelSiteCode
        End Get
        Set(ByVal value As String)
            mSsoClientEtravelSiteCode = value
        End Set
    End Property

    Public Property SsoClientMiUrl() As String
        Get
            Return mSsoClientMiUrl
        End Get
        Set(ByVal value As String)
            mSsoClientMiUrl = value
        End Set
    End Property

    Public Property SsoClientID() As Integer
        Get
            Return mSsoClientID
        End Get
        Set(ByVal value As Integer)
            mSsoClientID = value
        End Set
    End Property

    Public Property SsoClientName() As String
        Get
            Return mSsoClientName
        End Get
        Set(ByVal value As String)
            mSsoClientName = value
        End Set
    End Property

    Public Property SsoClientConfermaID() As String
        Get
            Return mSsoClientConfermaID
        End Get
        Set(ByVal value As String)
            mSsoClientConfermaID = value
        End Set
    End Property

    Public Property SsoClientSbookerUrl() As String
        Get
            Return mSsoClientSbookerUrl
        End Get
        Set(ByVal value As String)
            mSsoClientSbookerUrl = value
        End Set
    End Property

    Public Property SsoClientWebformsUrl() As String
        Get
            Return mSsoClientWebformsUrl
        End Get
        Set(ByVal value As String)
            mSsoClientWebformsUrl = value
        End Set
    End Property

    Public Property SsoClientEtravelUrl() As String
        Get
            Return mSsoClientEtravelUrl
        End Get
        Set(ByVal value As String)
            mSsoClientEtravelUrl = value
        End Set
    End Property

    Public Property SsoClientEtravelKey() As String
        Get
            Return mSsoClientEtravelKey
        End Get
        Set(ByVal value As String)
            mSsoClientEtravelKey = value
        End Set
    End Property

    Public Property SsoClientEtravelKeyEnc() As String
        Get
            Return mSsoClientEtravelKeyEnc
        End Get
        Set(ByVal value As String)
            mSsoClientEtravelKeyEnc = value
        End Set
    End Property

    Public Property SsoClientEvolviUrl() As String
        Get
            Return mSsoClientEvolviUrl
        End Get
        Set(ByVal value As String)
            mSsoClientEvolviUrl = value
        End Set
    End Property

    Private Shared Function makeSsoClientFromRow( _
            ByVal r As IDataReader _
        ) As SsoClient
        Return New SsoClient( _
                ClsNys.notInteger(r.Item("ssoClientID")), _
                ClsNys.notString(r.Item("ssoClientName")), _
                ClsNys.notString(r.Item("ssoClientConfermaID")), _
                ClsNys.notString(r.Item("ssoClientSbookerUrl")), _
                ClsNys.notString(r.Item("ssoClientWebformsUrl")), _
                ClsNys.notString(r.Item("ssoClientEtravelUrl")), _
                ClsNys.notString(r.Item("ssoClientEtravelKey")), _
                ClsNys.notString(r.Item("ssoClientEtravelKeyEnc")), _
                ClsNys.notString(r.Item("ssoClientEtravelSiteCode")), _
                ClsNys.notString(r.Item("ssoClientEvolviUrl")), _
                ClsNys.notString(r.Item("ssoClientMiUrl")), _
                ClsNys.notInteger(r.Item("ssoClientConfermaVersion")), _
                ClsNys.notString(r.Item("ssoClientPrefix")), _
                ClsNys.notString(r.Item("ssoClientPrefix2")), _
                ClsNys.notBoolean(r.Item("ssoClientIsLive")))
    End Function

    Public Shared Function [get]( _
            ByVal pSsoClientID As Integer, ByVal pssoClientName As String _
        ) As SsoClient
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            Using r As IDataReader = dbh.callSP("ssoClient_get", _
                                                "@ssoClientID", pSsoClientID, _
                                                "@ssoClientName", pssoClientName)
                If Not r.Read() Then
                    Throw New Exception("No SsoClient with id " & IIf(pSsoClientID = 0, pssoClientName, pSsoClientID))
                End If
                Dim ret As SsoClient = makeSsoClientFromRow(r)
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function list() As List(Of SsoClient)
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            Dim ret As New List(Of SsoClient)()
            Using r As IDataReader = dbh.callSP("ssoClient_list")
                While r.Read()
                    ret.Add(makeSsoClientFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            mSsoClientID = CInt(dbh.callSPSingleValue("ssoClient_save", _
                                                      "@SsoClientID", mSsoClientID, _
                                                      "@SsoClientName", mSsoClientName, _
                                                      "@SsoClientConfermaID", mSsoClientConfermaID, _
                                                      "@SsoClientSbookerUrl", mSsoClientSbookerUrl, _
                                                      "@SsoClientWebformsUrl", mSsoClientWebformsUrl, _
                                                      "@SsoClientEtravelUrl", mSsoClientEtravelUrl, _
                                                      "@SsoClientEtravelKey", mSsoClientEtravelKey, _
                                                      "@SsoClientEtravelKeyEnc", mSsoClientEtravelKeyEnc, _
                                                      "@SsoClientEtravelSiteCode", mSsoClientEtravelSiteCode, _
                                                      "@SsoClientEvolviUrl", mSsoClientEvolviUrl, _
                                                      "@ssoClientMiUrl", mSsoClientMiUrl, _
                                                      "@ssoClientConfermaVersion", mSsoClientConfermaVersion, _
                                                      "@ssoClientPrefix", mSsoClientPrefix, _
                                                      "@ssoClientPrefix2", mSsoClientPrefix2, _
                                                      "@ssoClientIsLive", mssoClientIsLive))
            Return mSsoClientID
        End Using
    End Function

    Public Shared Sub delete( _
            ByVal pSsoClientID As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            dbh.callNonQuerySP("ssoClient_delete", "@SsoClientID", pSsoClientID)
        End Using
    End Sub

    Public Sub delete()
        delete(mSsoClientID)
    End Sub

End Class
