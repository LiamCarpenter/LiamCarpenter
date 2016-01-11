Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

''' <summary>
''' Class clsSystemNYSUser
''' </summary>
''' <remarks>
''' Created 12/03/2009 Nick Massarella
''' Class retrieves currently logged in user details from their Windows login
'''  to allow access to application and record against amendments of records
''' </remarks>
<Serializable()> _
Partial Public Class clsVenue

    Public Sub New( _
        ByVal pvereference As Integer, _
        ByVal pvename As String, _
        ByVal pveaddress1 As String, _
        ByVal pveaddress2 As String, _
        ByVal ptoname As String, _
        ByVal parname As String, _
        ByVal pvepostcode As String, _
        ByVal pbosscode As String, _
        ByVal pveeh As Double, _
        ByVal pvefb As Double, _
        ByVal pverh As Double, _
        ByVal pvedd As Double, _
        ByVal pveex As Double, _
        ByVal pvecx As Double, _
        ByVal ptransient As String, _
        ByVal ptransientgroup As String, _
        ByVal ptransientdefault As String)
        mvereference = pvereference
        mvename = pvename
        mveaddress1 = pveaddress1
        mveaddress2 = pveaddress2
        mtoname = ptoname
        marname = parname
        mvepostcode = pvepostcode
        mbosscode = pbosscode
        mveeh = pveeh
        mvefb = pvefb
        mverh = pverh
        mvedd = pvedd
        mveex = pveex
        mvecx = pvecx
        mtransient = ptransient
        mtransientgroup = ptransientgroup
        mtransientdefault = ptransientdefault
    End Sub

    Public Sub New(ByVal pitemvalue As String)
        mitemvalue = pitemvalue
    End Sub

    Public Sub New()
    End Sub

    Private mvereference As Integer
    Private mvename As String
    Private mveaddress1 As String
    Private mveaddress2 As String
    Private mtoname As String
    Private marname As String
    Private mvepostcode As String
    Private mbosscode As String
    Private mveeh As Double
    Private mvefb As Double
    Private mverh As Double
    Private mvedd As Double
    Private mveex As Double
    Private mvecx As Double
    Private mitemvalue As String
    Private mtransient As String
    Private mtransientgroup As String
    Private mtransientdefault As String

    Public Property transientdefault() As String
        Get
            Return mtransientdefault
        End Get
        Set(ByVal value As String)
            mtransientdefault = value
        End Set
    End Property

    Public Property transient() As String
        Get
            Return mtransient
        End Get
        Set(ByVal value As String)
            mtransient = value
        End Set
    End Property

    Public Property transientgroup() As String
        Get
            Return mtransientgroup
        End Get
        Set(ByVal value As String)
            mtransientgroup = value
        End Set
    End Property

    Public Property itemvalue() As String
        Get
            Return mitemvalue
        End Get
        Set(ByVal value As String)
            mitemvalue = value
        End Set
    End Property

    Public Property vereference() As Integer
        Get
            Return mvereference
        End Get
        Set(ByVal value As Integer)
            mvereference = value
        End Set
    End Property

    Public Property vename() As String
        Get
            Return mvename
        End Get
        Set(ByVal value As String)
            mvename = value
        End Set
    End Property

    Public Property veaddress1() As String
        Get
            Return mveaddress1
        End Get
        Set(ByVal value As String)
            mveaddress1 = value
        End Set
    End Property

    Public Property veaddress2() As String
        Get
            Return mveaddress2
        End Get
        Set(ByVal value As String)
            mveaddress2 = value
        End Set
    End Property

    Public Property toname() As String
        Get
            Return mtoname
        End Get
        Set(ByVal value As String)
            mtoname = value
        End Set
    End Property

    Public Property arname() As String
        Get
            Return marname
        End Get
        Set(ByVal value As String)
            marname = value
        End Set
    End Property

    Public Property vepostcode() As String
        Get
            Return mvepostcode
        End Get
        Set(ByVal value As String)
            mvepostcode = value
        End Set
    End Property

    Public Property bosscode() As String
        Get
            Return mbosscode
        End Get
        Set(ByVal value As String)
            mbosscode = value
        End Set
    End Property

    Public Property veeh() As Double
        Get
            Return mveeh
        End Get
        Set(ByVal value As Double)
            mveeh = value
        End Set
    End Property

    Public Property vefb() As Double
        Get
            Return mvefb
        End Get
        Set(ByVal value As Double)
            mvefb = value
        End Set
    End Property

    Public Property verh() As Double
        Get
            Return mverh
        End Get
        Set(ByVal value As Double)
            mverh = value
        End Set
    End Property

    Public Property vedd() As Double
        Get
            Return mvedd
        End Get
        Set(ByVal value As Double)
            mvedd = value
        End Set
    End Property

    Public Property vecx() As Double
        Get
            Return mvecx
        End Get
        Set(ByVal value As Double)
            mvecx = value
        End Set
    End Property

    Public Property veex() As Double
        Get
            Return mveex
        End Get
        Set(ByVal value As Double)
            mveex = value
        End Set
    End Property

    ''' <summary>
    ''' Function makeSystemnysuserFromRow
    ''' </summary>
    ''' <param name="r"></param>
    ''' <returns>
    ''' Venue record details
    ''' </returns>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' </remarks>
    Private Shared Function makeVenueFromRow( _
            ByVal r As IDataReader _
        ) As clsVenue
        Return New clsVenue( _
                clsStuff.notWholeNumber(r.Item("vereference")), _
                clsStuff.notString(r.Item("vename")), _
                clsStuff.notString(r.Item("veaddress1")), _
                clsStuff.notString(r.Item("veaddress2")), _
                clsStuff.notString(r.Item("toname")), _
                clsStuff.notString(r.Item("arname")), _
                clsStuff.notString(r.Item("vepostcode")), _
                clsStuff.notString(r.Item("bosscode")), _
                clsStuff.notDouble(r.Item("veeh")), _
                clsStuff.notDouble(r.Item("vefb")), _
                clsStuff.notDouble(r.Item("verh")), _
                clsStuff.notDouble(r.Item("vedd")), _
                clsStuff.notDouble(r.Item("veex")), _
                clsStuff.notDouble(r.Item("vecx")), _
                clsStuff.notString(r.Item("transient")), _
                clsStuff.notString(r.Item("transientgroup")), _
                clsStuff.notString(r.Item("transientdefault")))
    End Function

    ''' <summary>
    ''' Function makeSystemnysuserFromRow2
    ''' </summary>
    ''' <param name="r"></param>
    ''' <returns>
    ''' Venue ane record
    ''' </returns>
    ''' <remarks>
    ''' Created 18/03/2009 Nick Massarella
    ''' </remarks>
    Private Shared Function makeVenueFromRow2( _
            ByVal r As IDataReader _
        ) As clsVenue
        Return New clsVenue( _
                clsStuff.notString(r.Item("itemvalue")))
    End Function

    ''' <summary>
    ''' Function Populate
    ''' </summary>
    ''' <param name="pvenuename1"></param>
    ''' <param name="pvenuename2"></param>
    ''' <param name="pstrandor"></param>
    ''' <returns>
    ''' A list of venues matching on name
    ''' </returns>
    ''' <remarks>
    ''' Created 12/03/2009 Nick Massarella
    ''' Updated 20/05/2009 - added groupid so can join and retrieve transient rates if any
    ''' </remarks>
    Public Shared Function Populate(ByVal pvenuename1 As String, ByVal pvenuename2 As String, _
                     ByVal pstrandor As String, ByVal pgroupid As String) As List(Of clsVenue)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsVenue)()
            Using r As IDataReader = dbh.callSP("venue_feedfind", _
                                                "@venuename1", pvenuename1, _
                                                "@venuename2", pvenuename2, _
                                                "@databasename", getConfig("databasename"), _
                                                "@andor", pstrandor, _
                                                "@groupid", pgroupid)
                While r.Read()
                    ret.Add(makeVenueFromRow(r))
                End While
                Return ret
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Function venueNameFind
    ''' </summary>
    ''' <param name="pvenuename"></param>
    ''' <returns>
    ''' A list of venue names that start with the passed in value
    ''' </returns>
    ''' <remarks>
    ''' Created 18/03/2009 Nick Massarella
    ''' </remarks>
    Public Shared Function venueNameFind(ByVal pvenuename As String) As List(Of clsVenue)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsVenue)()
            Using r As IDataReader = dbh.callSP("venue_namefind", _
                                                "@venuename", pvenuename, _
                                                "@databasename", getConfig("databasename"))
                While r.Read()
                    ret.Add(makeVenueFromRow2(r))
                End While
                Return ret
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Function venueAlternativenameCheck
    ''' </summary>
    ''' <param name="pstrvenuename"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function venueAlternativenameCheck(ByVal pstrvenuename As String) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim strvenuename As String = clsStuff.notString(dbh.callSPSingleValueCanReturnNothing _
                                                ("venueAlternativename_check", _
                                                 "@venuename", pstrvenuename))
            Return strvenuename
        End Using
    End Function

    Public Shared Function venueAlternativeRefCheck(ByVal pstrvenuename As String) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim strvenueRef As String = clsStuff.notString(dbh.callSPSingleValueCanReturnNothing _
                                                ("venueAlternativeRef_check", _
                                                 "@venuename", pstrvenuename))
            Return strvenueRef
        End Using
    End Function

    Public Shared Function venueCheckNonComm(ByVal pintVenueRef As Integer) As Boolean
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim blnRet As Boolean = clsStuff.notBoolean(dbh.callSPSingleValueCanReturnNothing _
                                                ("venue_CheckNonComm", _
                                                 "@ve_reference", pintVenueRef))
            Return blnRet
        End Using
    End Function

    ''' <summary>
    ''' Function venueExactNameFind
    ''' </summary>
    ''' <param name="pvenuename"></param>
    ''' <returns>
    ''' A list of venues with exactly the same name as the passed in value
    ''' </returns>
    ''' <remarks>
    ''' Created 25/03/2009 Nick Massarella
    ''' Updated 20/05/2009 - added groupid so can join and retrieve transient rates if any
    ''' </remarks>
    Public Shared Function venueExactNameFind(ByVal pvenuename As String, ByVal pgroupid As String) As List(Of clsVenue)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsVenue)()
            Using r As IDataReader = dbh.callSP("venue_exactnamefind", _
                                                "@venuename", pvenuename, _
                                                "@databasename", getConfig("databasename"), _
                                                "@groupid", pgroupid)
                While r.Read()
                    ret.Add(makeVenueFromRow(r))
                End While
                Return ret
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Function venueInvoiceFind
    ''' </summary>
    ''' <param name="pvenueinvoice"></param>
    ''' <returns>
    ''' A list of venue invoices that start with the passed in value
    ''' </returns>
    ''' <remarks>
    ''' Created 18/03/2009 Nick Massarella
    ''' </remarks>
    Public Shared Function venueInvoiceFind(ByVal pvenueinvoice As String) As List(Of clsVenue)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsVenue)()
            Using r As IDataReader = dbh.callSP("venue_invoicelist", _
                                                "@invoiceno", pvenueinvoice)
                While r.Read()
                    ret.Add(makeVenueFromRow2(r))
                End While
                Return ret
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Function guestNameFind
    ''' </summary>
    ''' <param name="pguestname"></param>
    ''' <returns>
    ''' A list of guest names that start with the passed in value
    ''' </returns>
    ''' <remarks>
    ''' Created 18/03/2009 Nick Massarella
    ''' </remarks>
    Public Shared Function guestNameFind(ByVal pguestname As String) As List(Of clsVenue)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsVenue)()
            Using r As IDataReader = dbh.callSP("guest_namelist", _
                                                "@guestname", pguestname)
                While r.Read()
                    ret.Add(makeVenueFromRow2(r))
                End While
                Return ret
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Function guestPNRFind
    ''' </summary>
    ''' <param name="pguestpnr"></param>
    ''' <returns>
    ''' A list of guest PNRs that start with the passed in value
    ''' </returns>
    ''' <remarks>
    ''' Created 18/03/2009 Nick Massarella
    ''' </remarks>
    Public Shared Function guestPNRFind(ByVal pguestpnr As String) As List(Of clsVenue)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of clsVenue)()
            Using r As IDataReader = dbh.callSP("guest_PNRlist", _
                                                "@guestpnr", pguestpnr)
                While r.Read()
                    ret.Add(makeVenueFromRow2(r))
                End While
                Return ret
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Function save
    ''' </summary>
    ''' <param name="pintref"></param>
    ''' <param name="pstrname"></param>
    ''' <param name="pstrconfermaname"></param>
    ''' <param name="pstrcode"></param>
    ''' <param name="pintuserid"></param>
    ''' <returns>
    ''' VenueAlternateID which will be greater than 0 if saved correctly
    ''' </returns>
    ''' <remarks>
    ''' Created 18/03/2009 Nick Massarella
    ''' </remarks>
    Public Shared Function save(ByVal pintref As Integer, ByVal pstrname As String, _
                                ByVal pstrconfermaname As String, ByVal pstrcode As String, _
                                ByVal pintuserid As Integer, ByVal pdblDD As Double, _
                                ByVal pdblEX As Double) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim mId As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValue("VenueAlternate_save", _
                                                   "@vereference", pintref, _
                                                   "@vename", pstrname, _
                                                   "@confermaname", pstrconfermaname, _
                                                   "@bosscode", pstrcode, _
                                                   "@userid", pintuserid, _
                                                   "@venueDD", pdblDD, _
                                                   "@venueEX", pdblEX))
            Return mId
        End Using
    End Function

    Public Shared Function saveInvoiceAlternate(ByVal pintref As Integer, ByVal pstrname As String, _
                                    ByVal pstrconfermaname As String, ByVal pstrcode As String, _
                                    ByVal pintuserid As Integer) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim mId As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValue("VenueAlternate_saveInvoice", _
                                                   "@vereference", pintref, _
                                                   "@vename", pstrname, _
                                                   "@confermaname", pstrconfermaname, _
                                                   "@bosscode", pstrcode, _
                                                   "@userid", pintuserid))
            Return mId
        End Using
    End Function

    ''' <summary>
    ''' Function saveBossCode
    ''' </summary>
    ''' <param name="pintref"></param>
    ''' <param name="pstrbosscode"></param>
    ''' <returns>
    ''' Venue reference if saved correctly
    ''' </returns>
    ''' <remarks>
    ''' Created 31/03/2009 Nick Massarella
    ''' Saves venue Boss code to VenuesDB if the bosscode field was empty
    ''' </remarks>
    Public Shared Function saveBossCode(ByVal pintref As Integer, ByVal pstrbosscode As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim mId As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValue("VenueBossCode_save", _
                                                   "@vereference", pintref, _
                                                   "@bosscode", pstrbosscode, _
                                                   "@databasename", getConfig("databasename")))
            Return mId
        End Using
    End Function

End Class


