Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Public Class clsTravellerProfiles
    Private mTravellerID As Integer
    Private mTravellerFirstname As String
    Private mTravellerLastname As String
    Private mTravellerEmail As String
    Private mTravellerCode1 As String
    Private mTravellerCode2 As String
    Private mTravellerSex As String
    Private mTelephone As String
    Private mTravellerActive As Boolean
    Private mLastUpdated As DateTime
    'R1.2 SA 
    Private mTravellerGroupID As Integer
    Private mMobile As String
    Private mTravellerProjectCode As String
    'R1.2.1 SA 
    Private mPhoneNumbersDateRequested As String
    Private mPhoneExt As String
    'R1.2.3 SA 
    Private mBusinessMobile As String
    Private mTravellerLocation As String
    'R1.3 SA 
    Private mTravellerCode3 As String
    Private mTravellerCode4 As String
    Private mTravellerCode5 As String
    Private mTravellerCode6 As String

    Public Sub New()
    End Sub

    'R1.3 SA - added travellercodes 3,4,5,6
    'R1.2.1 SA - added TravellerMobile, EXT 
    Public Sub New(pintTravellerID As Integer, _
                    pstrTravellerFirstname As String, _
                    pstrTravellerLastname As String, _
                    pstrTravellerEmail As String, _
                    pstrTravellerCode1 As String, _
                    pstrTravellerCode2 As String, _
                    pstrTravellerSex As String, _
                    pblnTravellerActive As Boolean, _
                    pstrTravellerLocation As String, _
                    pstrMobile As String, _
                    pstrBusinessMobile As String, _
                    pstrTelephone As String, _
                    pstrPhoneExt As String,
                    pstrTravellerCode3 As String, _
                    pstrTravellerCode4 As String, _
                    pstrTravellerCode5 As String, _
                    pstrTravellerCode6 As String)
        mTravellerID = pintTravellerID
        mTravellerFirstname = pstrTravellerFirstname
        mTravellerLastname = pstrTravellerLastname
        mTravellerEmail = pstrTravellerEmail
        mTravellerCode1 = pstrTravellerCode1
        mTravellerCode2 = pstrTravellerCode2
        mTravellerSex = pstrTravellerSex
        mTravellerActive = pblnTravellerActive
        mTelephone = pstrTelephone
        'R1.2.1 SA 
        mMobile = pstrMobile
        mPhoneExt = pstrPhoneExt
        'R1.2.3 SA 
        mBusinessMobile = pstrBusinessMobile
        mTravellerLocation = pstrTravellerLocation
        'R1.3 SA 
        mTravellerCode3 = pstrTravellerCode3
        mTravellerCode4 = pstrTravellerCode4
        mTravellerCode5 = pstrTravellerCode5
        mTravellerCode6 = pstrTravellerCode6

    End Sub

    Public Sub clear()
        mTravellerID = Nothing
        mTravellerFirstname = Nothing
        mTravellerLastname = Nothing
        mTravellerEmail = Nothing
        mTravellerCode1 = Nothing
        mTravellerCode2 = Nothing
        mTravellerSex = Nothing
        'mTelephone = Nothing
        mTravellerActive = Nothing
        'R1.2.1 SA 
        mMobile = Nothing
        'R1.3 SA 
        mTravellerCode3 = Nothing
        mTravellerCode4 = Nothing
        mTravellerCode5 = Nothing
        mTravellerCode6 = Nothing
    End Sub

    Public Property ID As Integer
        Get
            Return mTravellerID
        End Get
        Set(value As Integer)
            mTravellerID = value
        End Set
    End Property

    Public Property FirstName As String
        Get
            Return mTravellerFirstname
        End Get
        Set(value As String)
            mTravellerFirstname = value
        End Set
    End Property

    Public Property LastName As String
        Get
            Return mTravellerLastname
        End Get
        Set(value As String)
            mTravellerLastname = value
        End Set
    End Property

    Public Property Email As String
        Get
            Return mTravellerEmail
        End Get
        Set(value As String)
            mTravellerEmail = value
        End Set
    End Property

    Public Property Code1 As String
        Get
            Return mTravellerCode1
        End Get
        Set(value As String)
            mTravellerCode1 = value
        End Set
    End Property

    Public Property Code2 As String
        Get
            Return mTravellerCode2
        End Get
        Set(value As String)
            mTravellerCode2 = value
        End Set
    End Property

    Public Property Sex As String
        Get
            Return mTravellerSex
        End Get
        Set(value As String)
            mTravellerSex = value
        End Set
    End Property

    Public Property Telephone As String
        Get
            Return mTelephone
        End Get
        Set(value As String)
            mTelephone = value
        End Set
    End Property

    Public Property Active As Boolean
        Get
            Return mTravellerActive
        End Get
        Set(value As Boolean)
            mTravellerActive = value
        End Set
    End Property

    Public Property LastUpdated As DateTime
        Get
            Return mLastUpdated
        End Get
        Set(value As DateTime)
            mLastUpdated = value
        End Set
    End Property

    'R1.2 SA 
    Public Property GroupID() As Integer
        Get
            Return mTravellerGroupID
        End Get
        Set(ByVal value As Integer)
            mTravellerGroupID = value
        End Set
    End Property

    'R1.2 SA 
    Public Property ProjectCode As String
        Get
            Return mTravellerProjectCode
        End Get
        Set(ByVal value As String)
            mTravellerProjectCode = value
        End Set
    End Property

    'R1.2 SA 
    Public Property Mobile As String
        Get
            Return mMobile
        End Get
        Set(value As String)
            mMobile = value
        End Set
    End Property

    'R1.2.1 SA 
    Public Property PhoneNumbresDateRequested As String
        Get
            Return mPhoneNumbersDateRequested
        End Get
        Set(value As String)
            mPhoneNumbersDateRequested = value
        End Set
    End Property
    'R1.2.1 SA 
    Public Property PhoneExt As String
        Get
            Return mPhoneExt
        End Get
        Set(value As String)
            mPhoneExt = value
        End Set
    End Property

    'R1.2.3 SA 
    Public Property BusinessMobile As String
        Get
            Return mBusinessMobile
        End Get
        Set(ByVal value As String)
            mBusinessMobile = value
        End Set
    End Property
    'R1.2.3 SA 
    Public Property Location As String
        Get
            Return mTravellerLocation
        End Get
        Set(ByVal value As String)
            mTravellerLocation = value
        End Set
    End Property

    'R1.3 SA 
    Public Property Code3 As String
        Get
            Return mTravellerCode3
        End Get
        Set(value As String)
            mTravellerCode3 = value
        End Set
    End Property
    'R1.3 SA 
    Public Property Code4 As String
        Get
            Return mTravellerCode4
        End Get
        Set(value As String)
            mTravellerCode4 = value
        End Set
    End Property
    'R1.3 SA 
    Public Property Code5 As String
        Get
            Return mTravellerCode5
        End Get
        Set(value As String)
            mTravellerCode5 = value
        End Set
    End Property
    'R1.3 SA 
    Public Property Code6 As String
        Get
            Return mTravellerCode6
        End Get
        Set(value As String)
            mTravellerCode6 = value
        End Set
    End Property

    'R1.3 SA - added code3,4,5,6
    'R1.2.1 SA - added TravellerMobile, PhoneExt
    Private Function makeTravellerFromRow(ByVal r As IDataReader) As clsTravellerProfiles
        Return New clsTravellerProfiles(CInt(r.Item("TravellerID")), _
                                  CStr(r.Item("TravellerFirstname")), _
                                  CStr(r.Item("TravellerLastname")), _
                                  CStr(r.Item("TravellerEmail")), _
                                  CStr(r.Item("TravellerCode1")), _
                                  CStr(r.Item("TravellerCode2")), _
                                  CStr(r.Item("TravellerSex")), _
                                  CBool(r.Item("TravellerActive")), _
                                  CStr(r.Item("TravellerLocation")), _
                                  CStr(r.Item("Mobile")), _
                                  CStr(r.Item("BusinessMobile")), _
                                  CStr(r.Item("Telephone")), _
                                  CStr(r.Item("PhoneExt")), _
                                  CStr(r.Item("TravellerCode3")), _
                                  CStr(r.Item("TravellerCode4")), _
                                  CStr(r.Item("TravellerCode5")), _
                                  CStr(r.Item("TravellerCode6")))
    End Function

    'R1.2.3 SA - removed telephone
    'R1.2.1 SA - added Mobile
    'R1.2 SA - added pintCompanyID
    Public Function Search(ByVal pintCompanyID As Integer) As List(Of clsTravellerProfiles)
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim ret As New List(Of clsTravellerProfiles)()
            Using r As IDataReader = dbh.callSP("TravellerProfiles_search", _
                                                "@TravellerGroupID", pintCompanyID, _
                                                "@FirstName", mTravellerFirstname, _
                                                "@LastName", mTravellerLastname, _
                                                "@Email", mTravellerEmail, _
                                                "@Code1", mTravellerCode1, _
                                                "@Code2", mTravellerCode2, _
                                                "@Sex", mTravellerSex, _
                                                "@Mobile", mMobile)
                '"@Active", mTravellerActive)
                '"@Telephone", mTravellerTelephone, _
                While r.Read
                    ret.Add(makeTravellerFromRow(r))
                End While
            End Using

            Return ret
        End Using
    End Function

    'R1.2.1 SA - added  TravellerMobile
    Public Function listAll(ByVal pintCompanyID As Integer) As List(Of clsTravellerProfiles)
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim ret As New List(Of clsTravellerProfiles)()
            Using r As IDataReader = dbh.callSP("TravellerProfiles_listAll", _
                                                "@TravellerGroupID", pintCompanyID)
                While r.Read
                    ret.Add(makeTravellerFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    'R1.2.1 SA - Added Traveller Mobile 
    Public Shared Function listActive(ByVal pintCompanyID As Integer) As List(Of clsTravellerProfiles)
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim ret As New List(Of clsTravellerProfiles)()
            Using r As IDataReader = dbh.callSP("TravellerProfiles_listActive", _
                                                "@TravellerGroupID", pintCompanyID)
                While r.Read
                    ret.Add(New clsTravellerProfiles(CInt(r.Item("TravellerID")), _
                                 CStr(r.Item("TravellerFirstname")), _
                                  CStr(r.Item("TravellerLastname")), _
                                  CStr(r.Item("TravellerEmail")), _
                                  CStr(r.Item("TravellerCode1")), _
                                  CStr(r.Item("TravellerCode2")), _
                                  CStr(r.Item("TravellerSex")), _
                                  CBool(r.Item("TravellerActive")), _
                                  CStr(r.Item("TravellerLocation")), _
                                  CStr(r.Item("Mobile")), _
                                  CStr(r.Item("BusinessMobile")), _
                                  CStr(r.Item("Telephone")), _
                                  CStr(r.Item("PhoneExt")), _
                                  CStr(r.Item("TravellerCode3")), _
                                  CStr(r.Item("TravellerCode4")), _
                                  CStr(r.Item("TravellerCode5")), _
                                  CStr(r.Item("TravellerCode6"))))
                End While
            End Using
            Return ret
        End Using
    End Function

    ''' <summary>
    ''' Returns a list of profiles that were in the last LV Traveller file - but that are not in the current file
    ''' </summary>
    ''' <returns>list(of clsLvTraveller) containing deactivated users</returns>
    ''' <remarks></remarks>
    Public Shared Function listDeactivatedProfiles(ByVal pintCompanyID As Integer) As List(Of clsTravellerProfiles)
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim ret As New List(Of clsTravellerProfiles)()
            Using r As IDataReader = dbh.callSP("TravellerProfiles_listDeactivated", _
                                                "@TravellerGroupID", pintCompanyID)
                While r.Read
                    ret.Add(New clsTravellerProfiles(CInt(r.Item("TravellerID")), _
                                  CStr(r.Item("TravellerFirstname")), _
                                  CStr(r.Item("TravellerLastname")), _
                                  CStr(r.Item("TravellerEmail")), _
                                  CStr(r.Item("TravellerCode1")), _
                                  CStr(r.Item("TravellerCode2")), _
                                  CStr(r.Item("TravellerSex")), _
                                  CBool(r.Item("TravellerActive")), _
                                  CStr(r.Item("TravellerLocation")), _
                                  CStr(r.Item("Mobile")), _
                                  CStr(r.Item("BusinessMobile")), _
                                  CStr(r.Item("Telephone")), _
                                  CStr(r.Item("PhoneExt")), _
                                  CStr(r.Item("TravellerCode3")), _
                                  CStr(r.Item("TravellerCode4")), _
                                  CStr(r.Item("TravellerCode5")), _
                                  CStr(r.Item("TravellerCode6"))))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function checkEmail(pstrEmail As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim ret As Integer = 0
            Using r As IDataReader = dbh.callSP("TravellerProfiles_CheckEmail", _
                                                "@TravellerEmail", pstrEmail)
                While r.Read
                    ret = CInt(r.Item("TravellerID"))
                End While
            End Using
            Return ret
        End Using
    End Function

    'R1.2 CR - ERROR FIX
    'checkEmail function has been changed for Interceptor but broke the traveller file import procedure!! (it used checkEmail to get the traveller profile ID for updating)
    'this has been added to fix the traveller profile import
    Public Shared Function getIDByEmail(pstrEmail As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim ret As Integer = 0
            Using r As IDataReader = dbh.callSP("TravellerProfiles_getIDByEmail", _
                                                "@TravellerEmail", pstrEmail)
                While r.Read
                    ret = CInt(r.Item("TravellerID"))
                End While
            End Using
            Return ret
        End Using
    End Function

    'R1.2 SA - added pintCompanyID
    Public Sub saveImport()
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim ret As Integer = CInt(dbh.callSPSingleValueCanReturnNothing("TravellerProfiles_SaveImport", _
                                                                       "@ID", mTravellerID, _
                                                                       "@FirstName", mTravellerFirstname, _
                                                                       "@LastName", mTravellerLastname, _
                                                                       "@Email", mTravellerEmail, _
                                                                       "@Code1", mTravellerCode1, _
                                                                       "@Code2", mTravellerCode2, _
                                                                       "@Sex", mTravellerSex, _
                                                                       "@Active", mTravellerActive, _
                                                                       "@LastUpdated", mLastUpdated, _
                                                                       "@GroupID", mTravellerGroupID))
            mTravellerID = ret
        End Using
    End Sub

    'R1.3 SA - added code3,4,5,6
    'R1.2.1 SA - added mPhoneNumberDateRequested, mPhoneExt
       Public Sub saveManual()
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim ret As Integer = CInt(dbh.callSPSingleValueCanReturnNothing("TravellerProfiles_SaveManual", _
                                                                       "@ID", mTravellerID, _
                                                                       "@FirstName", mTravellerFirstname, _
                                                                       "@LastName", mTravellerLastname, _
                                                                       "@Email", mTravellerEmail, _
                                                                       "@Code1", mTravellerCode1, _
                                                                       "@Code2", mTravellerCode2, _
                                                                       "@Sex", mTravellerSex, _
                                                                       "@Telephone", mTelephone, _
                                                                       "@Active", mTravellerActive, _
                                                                       "@LastUpdated", mLastUpdated,
                                                                       "@GroupID", mTravellerGroupID, _
                                                                       "@Mobile", mMobile, _
                                                                       "@PhoneDateRequested", mPhoneNumbersDateRequested, _
                                                                       "@PhoneExt", mPhoneExt, _
                                                                       "@Location", mTravellerLocation, _
                                                                       "@BusinessMobile", mBusinessMobile, _
                                                                       "@Code3", mTravellerCode3, _
                                                                       "@Code4", mTravellerCode4, _
                                                                       "@Code5", mTravellerCode5, _
                                                                       "@Code6", mTravellerCode6))

            mTravellerID = ret
        End Using
    End Sub

    'R1.2 SA - added pintCompanyID
    ''' <summary>
    ''' Deactivates the profiles that are older than the datetime passed - because all new records imported should be saved with the same datetime
    ''' </summary>
    ''' <param name="pdteNewRecordsDate"></param>
    ''' <remarks></remarks>
    Public Shared Sub deactivateOldProfiles(pdteNewRecordsDate As DateTime, _
                                            ByVal pintCompanyID As Integer)
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            dbh.callNonQuerySP("TravellerProfiles_deactivateOldProfiles", _
                               "@NewRecordsDate", pdteNewRecordsDate, _
                               "@TravellerGroupID", pintCompanyID)
        End Using
    End Sub

    Public Sub populate(ByVal pintID As Integer, ByVal pstrEmail As String)
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            'Dim ret As New clsLVTraveller
            Using r As IDataReader = dbh.callSP("TravellerProfiles_populate", _
                                                "@TravellerID", pintID, _
                                                "@TravellerEmail", pstrEmail)
                r.Read()
                mTravellerID = CInt(r.Item("TravellerID"))
                mTravellerFirstname = CStr(r.Item("TravellerFirstname"))
                mTravellerLastname = CStr(r.Item("TravellerLastname"))
                mTravellerEmail = CStr(r.Item("TravellerEmail"))
                mTravellerCode2 = CStr(r.Item("TravellerCode2"))
                mTravellerCode1 = CStr(r.Item("TravellerCode1"))
                mTravellerSex = CStr(r.Item("TravellerSex"))
                mTravellerActive = CBool(r.Item("TravellerActive"))
                mTravellerGroupID = CInt(r.Item("TravellerGroupID"))
                'R1.2.3 SA 
                mTravellerLocation = CStr(r.Item("TravellerLocation"))
                mMobile = CStr(r.Item("Mobile"))
                mBusinessMobile = CStr(r.Item("BusinessMobile"))
                mTelephone = CStr(r.Item("Telephone"))

                'R1.2.1 SA 
                mPhoneExt = CStr(r.Item("PhoneExt"))
                mPhoneNumbersDateRequested = CStr(r.Item("PhoneDateRequested"))

                'R1.3 SA 
                mTravellerCode3 = CStr(r.Item("TravellerCode3"))
                mTravellerCode4 = CStr(r.Item("TravellerCode4"))
                mTravellerCode5 = CStr(r.Item("TravellerCode5"))
                mTravellerCode6 = CStr(r.Item("TravellerCode6"))
            End Using
            'Return ret
        End Using
    End Sub

    'R1.2 SA - added pintCompanyID
    Public Shared Sub backupTravellers(ByVal pintCompanyID As Integer)
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            dbh.callNonQuerySP("TravellerProfiles_backup", _
                               "@TravellerGroupID", pintCompanyID)
        End Using
    End Sub

    'R1.2 SA
    Public Sub savePhones()
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim ret As Integer = CInt(dbh.callSPSingleValueCanReturnNothing("TravellerTelephones_Save", _
                                                                       "@Email", mTravellerEmail, _
                                                                       "@Telephone", mTelephone, _
                                                                       "@Mobile", mMobile, _
                                                                       "@GroupID", mTravellerGroupID,
                                                                       "@BusinessMobile", mBusinessMobile))
            mTravellerID = ret
        End Using
    End Sub

    'R1.2 SA
    Public Shared Function getByEmail(pstrEmail As String) As clsTravellerProfiles
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Using r As IDataReader = dbh.callSP("TravellerProfiles_getByEmail", _
                                                "@TravellerEmail", pstrEmail)
                Dim oRet As New clsTravellerProfiles
                While r.Read()
                    With oRet
                        .mTravellerID = CInt(r.Item("TravellerID"))
                        .mTravellerEmail = CStr(r.Item("TravellerEmail"))
                        .mTravellerCode2 = CStr(r.Item("TravellerCode2"))
                        .mTravellerCode1 = CStr(r.Item("TravellerCode1"))
                        'R1.3 SA 
                        .mTravellerCode3 = CStr(r.Item("TravellerCode3"))
                        .mTravellerCode4 = CStr(r.Item("TravellerCode4"))
                        .mTravellerCode5 = CStr(r.Item("TravellerCode5"))
                        .mTravellerCode6 = CStr(r.Item("TravellerCode6"))
                        '.mTravellerProjectCode = ""
                        '.mTelephone = ""
                    End With
                End While
                Return oRet
            End Using
        End Using
    End Function

    'R1.2 SA
    Public Shared Function checkCode1(ByVal pstrCode1 As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim intRet As Integer = CInt(dbh.callSPSingleValueCanReturnNothing("TravellerProfiles_checkCode1", _
                                                                                     "@TravellerCode1", pstrCode1))
            Return intRet
        End Using
    End Function

    'R1.2 SA 
    Public Shared Function checkCode2(ByVal pstrCode2 As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim intRet As Integer = CInt(dbh.callSPSingleValueCanReturnNothing("TravellerProfiles_checkCode2", _
                                                                                     "@TravellerCode2", pstrCode2))
            Return intRet
        End Using
    End Function

    'R1.2 SA 
    Public Shared Function getCode1(ByVal pstrEmail As String) As String
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim strRet As String = CStr(dbh.callSPSingleValueCanReturnNothing("TravellerProfiles_getCode1", _
                                                                                    "@TravellerEmail", pstrEmail))
            Return strRet
        End Using
    End Function

    'R1.2 SA
    Public Shared Function getCode2(ByVal pstrEmail As String) As String
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim strRet As String = CStr(dbh.callSPSingleValueCanReturnNothing("TravellerProfiles_getCode2", _
                                                                                    "@TravellerEmail", pstrEmail))
            Return strRet
        End Using
    End Function

    'R1.2.3 SA 
    Public Sub saveLocation()
        Using dbh As New SqlDatabaseHandle(getConfig("ConnectionString"))
            Dim ret As Integer = CInt(dbh.callSPSingleValueCanReturnNothing("TravellerProfiles_SaveLocation", _
                                                                       "@Email", mTravellerEmail, _
                                                                       "@Location", mTravellerLocation))
            mTravellerID = ret
        End Using
    End Sub

End Class
