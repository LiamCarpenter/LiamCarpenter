Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils
Imports Microsoft.VisualBasic
Imports System.Data

Partial Public Class SsoUser

    'R2.21 CR - added trained bools & unsubscribed bool
    Public Sub New( _
        ByVal pSsoUserID As Integer, _
        ByVal pSsoUserFirstname As String, _
        ByVal pSsoUserLastName As String, _
        ByVal pSsoUserEmail As String, _
        ByVal pSsoUserPassword As String, _
        ByVal pSsoUserConferma As Nullable(Of Boolean), _
        ByVal pSsoUserEtravel As Nullable(Of Boolean), _
        ByVal pSsoUserEvolvi As Nullable(Of Boolean), _
        ByVal pSsoUserSbooker As Nullable(Of Boolean), _
        ByVal pSsoUserWebforms As Nullable(Of Boolean), _
        ByVal pSsoClientID As Nullable(Of Integer), _
        ByVal pSsoUserActive As Nullable(Of Boolean), _
        ByVal pSsoUserMi As Nullable(Of Boolean), _
        ByVal pssoUserPasswordDate As String, _
        ByVal pssoUserOldPasswords As String,
        ByVal pssoUserResetDate As String,
        ByVal pssoUserSentDate As String,
        ByVal pssoUserEvolviPassword As String,
        ByVal pssoUserSbookerGroup As String,
        ByVal pssoEbisID As String,
        ByVal pssoUserTerms As Boolean,
        ByVal pssoUserDeleted As Boolean,
        ByVal pssoUserTrainedEvolvi As Boolean,
        ByVal pssoUserTrainedEtravel As Boolean,
        ByVal pssoUserTrainedConferma As Boolean,
        ByVal pssoUserTrainedSbooker As Boolean,
        ByVal pssoUserUnsubscribed As Boolean)
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

        'R2.2 CR
        mSsoUserResetDate = pssoUserResetDate
        'R2.2 NM
        mssoUserSentDate = pssoUserSentDate
        'R2.4 NM
        mssoUserEvolviPassword = pssoUserEvolviPassword

        'R2.4 CR
        mssoUserSbookerGroup = pssoUserSbookerGroup

        'R2.15 CR - RCN Feeder
        mssoUserEbisID = pssoEbisID
        'R2.15 NM
        mssoUserTerms = pssoUserTerms
        mssoUserDeleted = pssoUserDeleted

        'R2.21 CR
        mssoUserTrainedEvolvi = pssoUserTrainedEvolvi
        mssoUserTrainedEtravel = pssoUserTrainedEtravel
        mssoUserTrainedConferma = pssoUserTrainedConferma
        mssoUserTrainedSbooker = pssoUserTrainedSbooker
        mssoUserUnsubscribed = pssoUserUnsubscribed
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

    'R2.21 CR - added trained bools & unsubscribed bool
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
        ByVal pssoUserDeleted As Boolean,
        ByVal pssoUserTrainedEvolvi As Boolean,
        ByVal pssoUserTrainedEtravel As Boolean,
        ByVal pssoUserTrainedConferma As Boolean,
        ByVal pssoUserTrainedSbooker As Boolean,
        ByVal pssoUserUnsubscribed As Boolean)
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

        'R2.4 CR
        mssoUserSbookerGroup = pssoUserSbookerGroup

        'R2.15 CR - RCN Feeder
        mssoUserEbisID = pssoEbisID
        'R2.15 NM
        mssoUserTerms = pssoUserTerms
        mssoUserDeleted = pssoUserDeleted

        'R2.21 CR
        mssoUserTrainedEvolvi = pssoUserTrainedEvolvi
        mssoUserTrainedEtravel = pssoUserTrainedEtravel
        mssoUserTrainedConferma = pssoUserTrainedConferma
        mssoUserTrainedSbooker = pssoUserTrainedSbooker
        mssoUserUnsubscribed = pssoUserUnsubscribed
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
    Private mSsoUserConferma As Nullable(Of Boolean)
    Private mSsoUserEtravel As Nullable(Of Boolean)
    Private mSsoUserEvolvi As Nullable(Of Boolean)
    Private mSsoUserSbooker As Nullable(Of Boolean)
    Private mSsoUserWebforms As Nullable(Of Boolean)
    Private mSsoClientID As Nullable(Of Integer)
    Private mSsoUserActive As Nullable(Of Boolean)
    Private mSsoUsermi As Nullable(Of Boolean)
    Private mSsoClientName As String

    'R2.2 CR
    Private mSsoUserResetDate As String
    'R2.2 NM
    Private mssoUserSentDate As String
    'R2.4 NM
    Private mssoUserEvolviPassword As String
    'R2.4 CR
    Private mssoUserSbookerGroup As String

    'R2.15 CR - RCN Feeder
    Private mssoUserEbisID As String
    Private mssoUserTerms As Boolean
    Private mssoUserDeleted As Boolean

    'R2.21 CR
    Private mssoUserTrainedEvolvi As Boolean
    Private mssoUserTrainedEtravel As Boolean
    Private mssoUserTrainedConferma As Boolean
    Private mssoUserTrainedSbooker As Boolean
    Private mssoUserUnsubscribed As Boolean

    'R2.15 NM
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

    'R2.2 CR
    Public Property UserResetDate As String
        Get
            Return mSsoUserResetDate
        End Get
        Set(ByVal value As String)
            mSsoUserResetDate = value
        End Set
    End Property

    'R2.2 NM
    Public Property UserSentDate As String
        Get
            Return mssoUserSentDate
        End Get
        Set(ByVal value As String)
            mssoUserSentDate = value
        End Set
    End Property

    'R2.4 NM
    Public Property UserEvolviPassword As String
        Get
            Return mssoUserEvolviPassword
        End Get
        Set(ByVal value As String)
            mssoUserEvolviPassword = value
        End Set
    End Property

    'R2.4 CR
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

    Public Property SsoUsermi() As Nullable(Of Boolean)
        Get
            Return mSsoUsermi
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSsoUsermi = value
        End Set
    End Property

    Public Property SsoUserActive() As Nullable(Of Boolean)
        Get
            Return mSsoUserActive
        End Get
        Set(ByVal value As Nullable(Of Boolean))
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

    Public Property SsoUserConferma() As Nullable(Of Boolean)
        Get
            Return mSsoUserConferma
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSsoUserConferma = value
        End Set
    End Property

    Public Property SsoUserEtravel() As Nullable(Of Boolean)
        Get
            Return mSsoUserEtravel
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSsoUserEtravel = value
        End Set
    End Property

    Public Property SsoUserEvolvi() As Nullable(Of Boolean)
        Get
            Return mSsoUserEvolvi
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSsoUserEvolvi = value
        End Set
    End Property

    Public Property SsoUserSbooker() As Nullable(Of Boolean)
        Get
            Return mSsoUserSbooker
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSsoUserSbooker = value
        End Set
    End Property

    Public Property SsoUserWebforms() As Nullable(Of Boolean)
        Get
            Return mSsoUserWebforms
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSsoUserWebforms = value
        End Set
    End Property

    Public Property SsoClientID() As Nullable(Of Integer)
        Get
            Return mSsoClientID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mSsoClientID = value
        End Set
    End Property

    'R2.15 CR - RCN Feeder
    Public Property SsoUserEbisID() As String
        Get
            Return mssoUserEbisID
        End Get
        Set(value As String)
            mssoUserEbisID = value
        End Set
    End Property

    'R2.21 CR
    Public Property SsoUserTrainedEvolvi As Boolean
        Get
            Return mssoUserTrainedEvolvi
        End Get
        Set(value As Boolean)
            mssoUserTrainedEvolvi = value
        End Set
    End Property

    'R2.21 CR
    Public Property SsoUserTraninedEtravel As Boolean
        Get
            Return mssoUserTrainedEtravel
        End Get
        Set(value As Boolean)
            mssoUserTrainedEtravel = value
        End Set
    End Property

    'R2.21 CR
    Public Property SsoUserTrainedConferma As Boolean
        Get
            Return mssoUserTrainedConferma
        End Get
        Set(value As Boolean)
            mssoUserTrainedConferma = value
        End Set
    End Property

    'R2.21 CR
    Public Property SsoUserTrainedSbooker As Boolean
        Get
            Return mssoUserTrainedSbooker
        End Get
        Set(value As Boolean)
            mssoUserTrainedSbooker = value
        End Set
    End Property

    'R2.21 CR
    Public Property SsoUserUnsubscribed As Boolean
        Get
            Return mssoUserUnsubscribed
        End Get
        Set(value As Boolean)
            mssoUserUnsubscribed = value
        End Set
    End Property

    Private Shared Function makeSsoUserFromRow( _
            ByVal r As IDataReader _
        ) As SsoUser
        Return New SsoUser( _
                ClsNys.notInteger(r.Item("ssoUserID")), _
                ClsNys.notString(r.Item("ssoUserFirstname")), _
                ClsNys.notString(r.Item("ssoUserLastName")), _
                ClsNys.notString(r.Item("ssoUserEmail")), _
                ClsNys.notString(r.Item("ssoUserPassword")), _
                toNullableBoolean(r.Item("ssoUserConferma")), _
                toNullableBoolean(r.Item("ssoUserEtravel")), _
                toNullableBoolean(r.Item("ssoUserEvolvi")), _
                toNullableBoolean(r.Item("ssoUserSbooker")), _
                toNullableBoolean(r.Item("ssoUserWebforms")), _
                toNullableInteger(r.Item("ssoClientID")), _
                toNullableBoolean(r.Item("ssoUserActive")), _
                toNullableBoolean(r.Item("ssoUserMi")), _
                ClsNys.notString(r.Item("ssoUserPasswordDate")), _
                ClsNys.notString(r.Item("ssoUserOldPasswords")), _
                ClsNys.notString(r.Item("ssoUserResetDate")), _
                ClsNys.notString(r.Item("ssoUserSentDate")), _
                ClsNys.notString(r.Item("ssoUserEvolviPassword")),
                ClsNys.notString(r.Item("ssoUserSbookerGroup")),
                ClsNys.notString(r.Item("ssoUserEbisID")),
                ClsNys.notBoolean(r.Item("ssoUserTerms")),
                ClsNys.notBoolean(r.Item("ssoUserDeleted")),
                ClsNys.notBoolean(r.Item("ssoUserTrainedEvolvi")),
                ClsNys.notBoolean(r.Item("ssoUserTrainedEtravel")),
                ClsNys.notBoolean(r.Item("ssoUserTrainedConferma")),
                ClsNys.notBoolean(r.Item("ssoUserTrainedSbooker")),
                ClsNys.notBoolean(r.Item("ssoUserUnsubscribed")))
    End Function

    Private Shared Function makeSsoUserPasswordFromRow( _
             ByVal r As IDataReader _
         ) As SsoUser
        Return New SsoUser( _
                ClsNys.notInteger(r.Item("ssoUserID")), _
                ClsNys.notString(r.Item("ssoUserPassword")), _
                ClsNys.notString(r.Item("ssoUserPasswordDate")), _
                toNullableBoolean(r.Item("ssoUserOldPasswords")))
    End Function

    Private Shared Function makeSsoUserDetailsFromRow( _
           ByVal r As IDataReader _
       ) As SsoUser
        Return New SsoUser( _
                ClsNys.notInteger(r.Item("ssoUserID")), _
                ClsNys.notString(r.Item("username")), _
                ClsNys.notString(r.Item("ssoUserEmail")), _
                ClsNys.notString(r.Item("Conferma")), _
                ClsNys.notString(r.Item("EtravelUrl")), _
                ClsNys.notString(r.Item("EtravelKey")), _
                ClsNys.notString(r.Item("EtravelSiteCode")), _
                ClsNys.notString(r.Item("Evolvi")), _
                ClsNys.notString(r.Item("Sbooker")), _
                ClsNys.notString(r.Item("Webforms")), _
                ClsNys.notString(r.Item("Mi")), _
                ClsNys.notString(r.Item("SsoClientName")), _
                ClsNys.notInteger(r.Item("ConfermaVersion")), _
                ClsNys.notInteger(r.Item("SsoClientID")), _
                ClsNys.notString(r.Item("ssoUserEvolviPassword")),
                ClsNys.notString(r.Item("ssoUserSbookerGroup")),
                ClsNys.notString(r.Item("ssoUserEbisID")),
                ClsNys.notBoolean(r.Item("ssoUserTerms")),
                ClsNys.notBoolean(r.Item("ssoUserDeleted")),
                ClsNys.notBoolean(r.Item("ssoUserTrainedEvolvi")),
                ClsNys.notBoolean(r.Item("ssoUserTrainedEtravel")),
                ClsNys.notBoolean(r.Item("ssoUserTrainedConferma")),
                ClsNys.notBoolean(r.Item("ssoUserTrainedSbooker")),
                ClsNys.notBoolean(r.Item("ssoUserUnsubscribed")))
    End Function

    Public Shared Function list(ByVal pSsoClientID As Integer) As List(Of SsoUser)
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            Dim ret As New List(Of SsoUser)()
            Using r As IDataReader = dbh.callSP("SsoUser_list", _
                                                "@SsoClientID", pSsoClientID)
                While r.Read()
                    ret.Add(makeSsoUserFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function [get](ByVal pssoUserID As Integer, _
            ByVal pssoUserEmail As String) As SsoUser
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            Using r As IDataReader = dbh.callSP("ssoUser_get", _
                                                "@ssoUserID", pssoUserID, _
                                                "@ssoUserEmail", pssoUserEmail)
                Dim ret As New SsoUser
                If r.Read Then
                    ret = makeSsoUserFromRow(r)
                End If
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function [getDetails](ByVal pssoUserID As Integer) As SsoUser
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            Using r As IDataReader = dbh.callSP("SsoUser_getDetails", _
                                                "@ssoUserID", pssoUserID)
                Dim ret As New SsoUser
                If r.Read Then
                    ret = makeSsoUserDetailsFromRow(r)
                End If
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function [checkEmailExists](ByVal pssoUserEmail As String) As Boolean
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            Dim blnRet As Boolean = ClsNys.notBoolean(dbh.callSPSingleValueCanReturnNothing("ssoUser_emailexists", _
                                                "@ssoUserEmail", pssoUserEmail))
            Return blnRet
        End Using
    End Function

    Public Shared Function [SsoUserDuplicatesCheck]() As Integer
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            Dim intRet As Integer = ClsNys.notInteger(dbh.callSPSingleValueCanReturnNothing("SsoUser_duplicatesCheck"))
            Return intRet
        End Using
    End Function

    Public Shared Function [checkEmailUnique](ByVal pssoUserEmail As String) As Integer
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            Dim intRet As Integer = ClsNys.notInteger(dbh.callSPSingleValueCanReturnNothing("ssoUser_emailunique", _
                                                "@ssoUserEmail", pssoUserEmail))
            Return intRet
        End Using
    End Function

    'R2.21 CR - add all trained bools & unsubscribed bool
    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            mSsoUserID = CInt(dbh.callSPSingleValue("ssoUser_save", _
                                                    "@SsoUserID", mSsoUserID, _
                                                    "@SsoUserFirstname", mSsoUserFirstname, _
                                                    "@SsoUserLastName", mSsoUserLastName, _
                                                    "@SsoUserEmail", mSsoUserEmail, _
                                                    "@SsoUserPassword", mSsoUserPassword, _
                                                    "@SsoUserConferma", mSsoUserConferma, _
                                                    "@SsoUserEtravel", mSsoUserEtravel, _
                                                    "@SsoUserEvolvi", mSsoUserEvolvi, _
                                                    "@SsoUserSbooker", mSsoUserSbooker, _
                                                    "@SsoUserWebforms", mSsoUserWebforms, _
                                                    "@SsoClientID", mSsoClientID, _
                                                    "@SsoUserActive", mSsoUserActive, _
                                                    "@ssoUserMi", mSsoUsermi, _
                                                    "@ssoUserResetDate", mSsoUserResetDate, _
                                                    "@ssoUserSentDate", mssoUserSentDate, _
                                                    "@ssoUserEvolviPassword", mssoUserEvolviPassword, _
                                                    "@ssoUserSbookerGroup", mssoUserSbookerGroup, _
                                                    "@ssoUserEbisID", mssoUserEbisID, _
                                                    "@ssoUserTerms", mssoUserTerms, _
                                                    "@ssoUserDeleted", mssoUserDeleted, _
                                                    "@ssoUserTrainedEvolvi", mssoUserTrainedEvolvi, _
                                                    "@ssoUserTrainedEtravel", mssoUserTrainedEtravel, _
                                                    "@ssoUserTrainedConferma", mssoUserTrainedConferma, _
                                                    "@ssoUserTrainedSbooker", mssoUserTrainedSbooker, _
                                                    "@ssoUserUnsubscribed", mssoUserUnsubscribed))
            Return mSsoUserID
        End Using
    End Function

    Public Function savePassword() As Integer
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            mSsoUserID = CInt(dbh.callSPSingleValue("ssoUser_savepassword", _
                                                    "@SsoUserID", mSsoUserID, _
                                                    "@SsoUserPassword", mSsoUserPassword, _
                                                    "@SsoUserPasswordDate", mssoUserPasswordDate, _
                                                    "@SsoUseroldPasswords", mssoUserOldPasswords))
            Return mSsoUserID
        End Using
    End Function

    Public Shared Sub delete(ByVal pSsoUserID As Integer)
        Using dbh As New SqlDatabaseHandle(getSSOConnection)
            dbh.callNonQuerySP("ssoUser_delete", "@SsoUserID", pSsoUserID)
        End Using
    End Sub

End Class
