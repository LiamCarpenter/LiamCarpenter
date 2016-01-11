Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Public Class clsSSOUser

    Public Sub New( _
        ByVal pSsoUserID As Integer, _
        ByVal pSsoUserFirstname As String, _
        ByVal pSsoUserLastName As String, _
        ByVal pSsoUserEmail As String, _
        ByVal pSsoUserPassword As String, _
        ByVal pSsoUserConferma As Boolean, _
        ByVal pSsoUserEtravel As Boolean, _
        ByVal pSsoUserEvolvi As Boolean, _
        ByVal pSsoUserSbooker As Boolean, _
        ByVal pSsoUserWebforms As Boolean, _
        ByVal pSsoClientID As Integer, _
        ByVal pSsoUserActive As Boolean, _
        ByVal pSsoUserMi As Boolean, _
        ByVal pssoUserPasswordDate As String, _
        ByVal pssoUserOldPasswords As String,
        ByVal pssoUserResetDate As String,
        ByVal pssoUserSentDate As String,
        ByVal pssoUserEvolviPassword As String,
        ByVal pssoUserSbookerGroup As String,
        ByVal pssoEbisID As String,
        ByVal pssoUserTerms As Boolean,
        ByVal pssoUserDeleted As Boolean)
        mSsoUserID = pSsoUserID
        mSsoUserFirstname = pSsoUserFirstname
        mSsoUserLastName = pSsoUserLastName
        mSsoUserEmail = pSsoUserEmail
        mSsoUserPassword = pSsoUserPassword
        mSsoUserConferma = pSsoUserConferma
        mSsoUserEtravel = pSsoUserEtravel
        mSsoUserEvolvi = pSsoUserEvolvi
        mSsoUserSbooker = pSsoUserSbooker
        mSsoUserWebforms = pSsoUserWebforms
        mSsoClientID = pSsoClientID
        mSsoUserActive = pSsoUserActive
        mSsoUsermi = pSsoUserMi
        mssoUserPasswordDate = pssoUserPasswordDate
        mssoUserOldPasswords = pssoUserOldPasswords
        mSsoUserResetDate = pssoUserResetDate
        mssoUserSentDate = pssoUserSentDate
        mssoUserEvolviPassword = pssoUserEvolviPassword
        mssoUserSbookerGroup = pssoUserSbookerGroup
        mssoUserEbisID = pssoEbisID
        mssoUserTerms = pssoUserTerms
        mssoUserDeleted = pssoUserDeleted
    End Sub

    Public Sub New( _
         ByVal pSsoUserID As Integer, _
         ByVal pSsoUserPassword As String, _
         ByVal pssoUserPasswordDate As String, _
         ByVal pssoUserOldPasswords As String)
        mSsoUserID = pSsoUserID
        mSsoUserPassword = pSsoUserPassword
        mssoUserPasswordDate = pssoUserPasswordDate
        mssoUserOldPasswords = pssoUserOldPasswords
    End Sub

    Public Sub New( _
        ByVal pSsoUserID As Integer, _
        ByVal pUsername As String, _
        ByVal pSsoUserEmail As String, _
        ByVal pConferma As String, _
        ByVal pEtravelUrl As String, _
        ByVal pEtravelKey As String, _
        ByVal pEtravelSiteCode As String, _
        ByVal pEvolvi As String, _
        ByVal pSbooker As String, _
        ByVal pWebforms As String, _
        ByVal pMi As String, _
        ByVal pSsoClientName As String, _
        ByVal pConfermaVersion As Integer, _
        ByVal pSsoClientID As Integer,
        ByVal pssoUserEvolviPassword As String, _
        ByVal pssoUserSbookerGroup As String, _
        ByVal pssoEbisID As String,
        ByVal pssoUserTerms As Boolean,
        ByVal pssoUserDeleted As Boolean)
        mSsoUserID = pSsoUserID
        mUsername = pUsername
        mSsoUserEmail = pSsoUserEmail
        mConferma = pConferma
        mEtravelUrl = pEtravelUrl
        mEtravelKey = pEtravelKey
        mEtravelSiteCode = pEtravelSiteCode
        mEvolvi = pEvolvi
        mSbooker = pSbooker
        mWebforms = pWebforms
        mMi = pMi
        mSsoClientName = pSsoClientName
        mConfermaVersion = pConfermaVersion
        mSsoClientID = pSsoClientID
        mssoUserEvolviPassword = pssoUserEvolviPassword
        mssoUserSbookerGroup = pssoUserSbookerGroup
        mssoUserEbisID = pssoEbisID
        mssoUserTerms = pssoUserTerms
        mssoUserDeleted = pssoUserDeleted
    End Sub

    Public Sub New( _
)
    End Sub


    Private mConfermaVersion As Integer
    Private mssoUserPasswordDate As String
    Private mssoUserOldPasswords As String
    Private mUsername As String
    Private mConferma As String
    Private mEtravelUrl As String
    Private mEtravelKey As String
    Private mEtravelSiteCode As String
    Private mEvolvi As String
    Private mSbooker As String
    Private mWebforms As String
    Private mMi As String
    Private mSsoUserID As Integer
    Private mSsoUserFirstname As String
    Private mSsoUserLastName As String
    Private mSsoUserEmail As String
    Private mSsoUserPassword As String
    Private mSsoUserConferma As Boolean
    Private mSsoUserEtravel As Boolean
    Private mSsoUserEvolvi As Boolean
    Private mSsoUserSbooker As Boolean
    Private mSsoUserWebforms As Boolean
    Private mSsoClientID As Integer
    Private mSsoUserActive As Boolean
    Private mSsoUsermi As Boolean
    Private mSsoClientName As String
    Private mSsoUserResetDate As String
    Private mssoUserSentDate As String
    Private mssoUserEvolviPassword As String
    Private mssoUserSbookerGroup As String
    Private mssoUserEbisID As String
    Private mssoUserTerms As Boolean
    Private mssoUserDeleted As Boolean

    Public Property ssoUserDeleted As Boolean
        Get
            Return mssoUserDeleted
        End Get
        Set(ByVal value As Boolean)
            mssoUserDeleted = value
        End Set
    End Property
    Public Property ssoUserTerms As Boolean
        Get
            Return mssoUserTerms
        End Get
        Set(ByVal value As Boolean)
            mssoUserTerms = value
        End Set
    End Property

    Public Property UserResetDate As String
        Get
            Return mSsoUserResetDate
        End Get
        Set(ByVal value As String)
            mSsoUserResetDate = value
        End Set
    End Property

    Public Property UserSentDate As String
        Get
            Return mssoUserSentDate
        End Get
        Set(ByVal value As String)
            mssoUserSentDate = value
        End Set
    End Property

    Public Property UserEvolviPassword As String
        Get
            Return mssoUserEvolviPassword
        End Get
        Set(ByVal value As String)
            mssoUserEvolviPassword = value
        End Set
    End Property

    Public Property UserSbookerGroup As String
        Get
            Return mssoUserSbookerGroup
        End Get
        Set(ByVal value As String)
            mssoUserSbookerGroup = value
        End Set
    End Property

    Public Property ConfermaVersion() As Integer
        Get
            Return mConfermaVersion
        End Get
        Set(ByVal value As Integer)
            mConfermaVersion = value
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

    Public Property EtravelSiteCode() As String
        Get
            Return mEtravelSiteCode
        End Get
        Set(ByVal value As String)
            mEtravelSiteCode = value
        End Set
    End Property

    Public Property ssoUserPasswordDate() As String
        Get
            Return mssoUserPasswordDate
        End Get
        Set(ByVal value As String)
            mssoUserPasswordDate = value
        End Set
    End Property

    Public Property ssoUserOldPasswords() As String
        Get
            Return mssoUserOldPasswords
        End Get
        Set(ByVal value As String)
            mssoUserOldPasswords = value
        End Set
    End Property

    Public Property username() As String
        Get
            Return mUsername
        End Get
        Set(ByVal value As String)
            mUsername = value
        End Set
    End Property

    Public Property Conferma() As String
        Get
            Return mConferma
        End Get
        Set(ByVal value As String)
            mConferma = value
        End Set
    End Property

    Public Property EtravelUrl() As String
        Get
            Return mEtravelUrl
        End Get
        Set(ByVal value As String)
            mEtravelUrl = value
        End Set
    End Property

    Public Property EtravelKey() As String
        Get
            Return mEtravelKey
        End Get
        Set(ByVal value As String)
            mEtravelKey = value
        End Set
    End Property

    Public Property Evolvi() As String
        Get
            Return mEvolvi
        End Get
        Set(ByVal value As String)
            mEvolvi = value
        End Set
    End Property

    Public Property sbooker() As String
        Get
            Return mSbooker
        End Get
        Set(ByVal value As String)
            mSbooker = value
        End Set
    End Property

    Public Property webforms() As String
        Get
            Return mWebforms
        End Get
        Set(ByVal value As String)
            mWebforms = value
        End Set
    End Property

    Public Property mi() As String
        Get
            Return mMi
        End Get
        Set(ByVal value As String)
            mMi = value
        End Set
    End Property

    Public Property SsoUsermi() As Boolean
        Get
            Return mSsoUsermi
        End Get
        Set(ByVal value As Boolean)
            mSsoUsermi = value
        End Set
    End Property

    Public Property SsoUserActive() As Boolean
        Get
            Return mSsoUserActive
        End Get
        Set(ByVal value As Boolean)
            mSsoUserActive = value
        End Set
    End Property

    Public Property SsoUserID() As Integer
        Get
            Return mSsoUserID
        End Get
        Set(ByVal value As Integer)
            mSsoUserID = value
        End Set
    End Property

    Public Property SsoUserFirstname() As String
        Get
            Return mSsoUserFirstname
        End Get
        Set(ByVal value As String)
            mSsoUserFirstname = value
        End Set
    End Property

    Public Property SsoUserLastName() As String
        Get
            Return mSsoUserLastName
        End Get
        Set(ByVal value As String)
            mSsoUserLastName = value
        End Set
    End Property

    Public Property SsoUserEmail() As String
        Get
            Return mSsoUserEmail
        End Get
        Set(ByVal value As String)
            mSsoUserEmail = value
        End Set
    End Property

    Public Property SsoUserPassword() As String
        Get
            Return mSsoUserPassword
        End Get
        Set(ByVal value As String)
            mSsoUserPassword = value
        End Set
    End Property

    Public Property SsoUserConferma() As Boolean
        Get
            Return mSsoUserConferma
        End Get
        Set(ByVal value As Boolean)
            mSsoUserConferma = value
        End Set
    End Property

    Public Property SsoUserEtravel() As Boolean
        Get
            Return mSsoUserEtravel
        End Get
        Set(ByVal value As Boolean)
            mSsoUserEtravel = value
        End Set
    End Property

    Public Property SsoUserEvolvi() As Boolean
        Get
            Return mSsoUserEvolvi
        End Get
        Set(ByVal value As Boolean)
            mSsoUserEvolvi = value
        End Set
    End Property

    Public Property SsoUserSbooker() As Boolean
        Get
            Return mSsoUserSbooker
        End Get
        Set(ByVal value As Boolean)
            mSsoUserSbooker = value
        End Set
    End Property

    Public Property SsoUserWebforms() As Boolean
        Get
            Return mSsoUserWebforms
        End Get
        Set(ByVal value As Boolean)
            mSsoUserWebforms = value
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

    Public Property SsoUserEbisID() As String
        Get
            Return mssoUserEbisID
        End Get
        Set(value As String)
            mssoUserEbisID = value
        End Set
    End Property

    Private Shared Function makeSsoUserFromRow( _
            ByVal r As IDataReader _
        ) As clsSSOUser
        Return New clsSSOUser( _
                clsNYS.notInteger(r.Item("ssoUserID")), _
                clsNYS.notString(r.Item("ssoUserFirstname")), _
                clsNYS.notString(r.Item("ssoUserLastName")), _
                clsNYS.notString(r.Item("ssoUserEmail")), _
                clsNYS.notString(r.Item("ssoUserPassword")), _
                clsNYS.notBoolean(r.Item("ssoUserConferma")), _
                clsNYS.notBoolean(r.Item("ssoUserEtravel")), _
                clsNYS.notBoolean(r.Item("ssoUserEvolvi")), _
                clsNYS.notBoolean(r.Item("ssoUserSbooker")), _
                clsNYS.notBoolean(r.Item("ssoUserWebforms")), _
                clsNYS.notInteger(r.Item("ssoClientID")), _
                clsNYS.notBoolean(r.Item("ssoUserActive")), _
                clsNYS.notBoolean(r.Item("ssoUserMi")), _
                clsNYS.notString(r.Item("ssoUserPasswordDate")), _
                clsNYS.notString(r.Item("ssoUserOldPasswords")), _
                clsNYS.notString(r.Item("ssoUserResetDate")), _
                clsNYS.notString(r.Item("ssoUserSentDate")), _
                clsNYS.notString(r.Item("ssoUserEvolviPassword")),
                clsNYS.notString(r.Item("ssoUserSbookerGroup")),
                clsNYS.notString(r.Item("ssoUserEbisID")),
                clsNYS.notBoolean(r.Item("ssoUserTerms")),
                clsNYS.notBoolean(r.Item("ssoUserDeleted")))
    End Function

    Private Shared Function makeSsoUserPasswordFromRow( _
             ByVal r As IDataReader _
         ) As clsSSOUser
        Return New clsSSOUser( _
                clsNYS.notInteger(r.Item("ssoUserID")), _
                clsNYS.notString(r.Item("ssoUserPassword")), _
                clsNYS.notString(r.Item("ssoUserPasswordDate")), _
                clsNYS.notBoolean(r.Item("ssoUserOldPasswords")))
    End Function

    Private Shared Function makeSsoUserDetailsFromRow( _
           ByVal r As IDataReader _
       ) As clsSSOUser
        Return New clsSSOUser( _
                clsNYS.notInteger(r.Item("ssoUserID")), _
                clsNYS.notString(r.Item("username")), _
                clsNYS.notString(r.Item("ssoUserEmail")), _
                clsNYS.notString(r.Item("Conferma")), _
                clsNYS.notString(r.Item("EtravelUrl")), _
                clsNYS.notString(r.Item("EtravelKey")), _
                clsNYS.notString(r.Item("EtravelSiteCode")), _
                clsNYS.notString(r.Item("Evolvi")), _
                clsNYS.notString(r.Item("Sbooker")), _
                clsNYS.notString(r.Item("Webforms")), _
                clsNYS.notString(r.Item("Mi")), _
                clsNYS.notString(r.Item("SsoClientName")), _
                clsNYS.notInteger(r.Item("ConfermaVersion")), _
                clsNYS.notInteger(r.Item("SsoClientID")), _
                clsNYS.notString(r.Item("ssoUserEvolviPassword")),
                clsNYS.notString(r.Item("ssoUserSbookerGroup")),
                clsNYS.notString(r.Item("ssoUserEbisID")),
                clsNYS.notBoolean(r.Item("ssoUserTerms")),
                clsNYS.notBoolean(r.Item("ssoUserDeleted")))
    End Function

    Public Shared Function findEbisID(ByVal pstrEmail As String, _
            ByVal pstrFirstname As String, _
            ByVal pstrLastname As String) As clsSSOUser
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            Using r As IDataReader = dbh.callSP("ssoUser_findEbisID", _
                                                "@ssoUserEmail", pstrEmail, _
                                                "@ssoUserFirstname", pstrFirstname, _
                                                "@ssoUserLastname", pstrLastname)
                Dim ret As New clsSSOUser
                Dim intCount As Integer = 0
                If r.Read Then
                    ret = makeSsoUserFromRow(r)
                    intCount += 1
                End If

                'only return 1 row, if more than 1 user found then act as if none found
                If intCount = 1 Then
                    Return ret
                Else
                    Return New clsSSOUser
                End If
            End Using
        End Using
    End Function

    'R2.23.1 AI
    Public Shared Function GetRCNSsoUserEbisID(ByVal pFirstName As String, ByVal pLastName As String) As String
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            Dim strRet As String = clsNYS.notString(dbh.callSPSingleValueCanReturnNothing("RCNssoUserEbisID_get", _
                                                                                   "@firstname", pFirstName, _
                                                                                   "@Lastname", pLastName))
            Return strRet
        End Using
    End Function

End Class
