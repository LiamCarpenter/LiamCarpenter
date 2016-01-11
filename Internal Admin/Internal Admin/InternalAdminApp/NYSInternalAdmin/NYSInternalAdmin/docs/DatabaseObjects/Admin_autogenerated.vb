﻿Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class Admin_autogenerated123

    Public Sub New(ByVal pEmailAddress As String, _
                    ByVal pReference As String, _
                    ByVal pSendtoEmail As Boolean, _
                    ByVal pFilelocation As String)
        mEmailAddress = pEmailAddress
        mReference = pReference
        mSendtoEmail = pSendtoEmail
        mFileLocation = pFilelocation
    End Sub

    Public Sub New(ByVal pBossRef As String)
        mBossRef = pBossRef
    End Sub
    Public Sub New()

    End Sub

    Private mBossRef As String
    Private mEmailAddress As String
    Private mReference As String
    Private mSendtoEmail As Boolean
    Private mFileLocation As String

    Public ReadOnly Property BossRef() As String
        Get
            Return mBossRef
        End Get
    End Property

    Public Property EmailAddess() As String
        Get
            Return mEmailAddress
        End Get
        Set(ByVal value As String)
            mEmailAddress = value
        End Set
    End Property

    Public Property Reference() As String
        Get
            Return mReference
        End Get
        Set(ByVal value As String)
            mReference = value
        End Set
    End Property

    Public Property SendtoEmail() As Boolean
        Get
            Return mSendtoEmail
        End Get
        Set(ByVal value As Boolean)
            mSendtoEmail = value
        End Set
    End Property

    Public Property FileLocation() As String
        Get
            Return mFileLocation
        End Get
        Set(ByVal value As String)
            mFileLocation = value
        End Set
    End Property

    Private Shared Function makeBossCodeFromRow(ByVal r As IDataReader) As Admin_autogenerated
        Return New Admin_autogenerated(toStr(r.Item("csd_BossRef")))
    End Function

    Public Shared Function listBossRef() As List(Of Admin_autogenerated)
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim ret As New List(Of Admin_autogenerated)()
            Using r As IDataReader = dbh.callSP("ClientStatementDetails_listBossRef")
                While r.Read()
                    ret.Add(makeBossCodeFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function getDetails(ByVal pBossRef As String) As Admin_autogenerated
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim details As New Admin_autogenerated
            Using r As IDataReader = dbh.callSP("ClientStatementDetails_getDetails", _
                                                                "csd_BossRef", pBossRef)
                While r.Read()
                    With details
                        .mEmailAddress = clsNYS.notString(r.Item("csd_EmailAddress"))
                        .mReference = clsNYS.notString(r.Item("csd_Reference"))
                        .mSendtoEmail = clsNYS.notBoolean(r.Item("csd_SendtoEmail"))
                        .mFileLocation = clsNYS.notString(r.Item("csd_CI_FileLocation"))
                    End With
                End While
            End Using
            Return details
        End Using
    End Function

    Public Shared Function getID(ByVal pBossRef As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim intRet As Integer = CInt(dbh.callSPSingleValue("ClientStatementDetails_getID", _
                                                                "csd_BossRef", pBossRef))

            Return intRet
        End Using
    End Function

    Public Shared Function getEmail(ByVal pBossRef As String) As String
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim strRet As String = CStr(dbh.callSPSingleValue("ClientStatementDetails_getEmailAddress", _
                                                    "csd_BossRef", pBossRef))
            Return strRet
        End Using
    End Function

    Public Shared Function getFileLocation(ByVal pBossRef As String) As String
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim strRet As String = dbh.callSPSingleValueCanReturnNothing("ClientStatementDetails_getFileLocation", _
                                                                               "@csd_BossRef", pBossRef)
            Return strRet
        End Using
    End Function

    Public Shared Function update(ByVal pID As Integer, _
                                 ByVal pBossRef As String, _
                                 ByVal pEmailAddress As String, _
                                 ByVal pReference As String, _
                                 ByVal pSendtoEmail As Boolean,
                                 ByVal pFileLocation As String) As Boolean
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim blnRet As Boolean = CBool(dbh.callSPSingleValue("ClientStatementDetails_update", _
                                                               "csd_ID", pID, _
                                                               "csd_BossRef", pBossRef, _
                                                               "csd_EmailAddress", pEmailAddress, _
                                                               "csd_Reference", pReference, _
                                                               "csd_SendtoEmail", pSendtoEmail, _
                                                               "csd_CI_FileLocation", pFileLocation))
            Return blnRet
        End Using
    End Function

End Class
