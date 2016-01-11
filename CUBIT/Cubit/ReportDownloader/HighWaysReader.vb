Option Explicit On
Option Strict On

Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Globalization
Imports DatabaseObjects
Imports EvoUtilities.ConfigUtils
Imports ReportDownloader.Utility
Imports CSVParser

Partial Public Class dataImportHighways
    Public Sub New( _
        ByVal pTransactionnumber As String, _
        ByVal pTransactionlinenumber As String, _
        ByVal pTransactiondate As String, _
        ByVal pDeparturedate As String, _
        ByVal pArrivaldate As String, _
        ByVal pPassengername As String, _
        ByVal pTotalamount As String, _
        ByVal pVatamount As String, _
        ByVal pVatrate As String, _
        ByVal pNettamount As String, _
        ByVal pVenuename As String, _
        ByVal pVenuedetails As String, _
        ByVal pRoomdetails As String, _
        ByVal pCompany As String, _
        ByVal pConfermainvoicenumber As String, _
        ByVal pCategory As String, _
        ByVal pSupplierinvoice As String, _
        ByVal pGuestPNR As String, _
        ByVal pRef1 As String, _
        ByVal pBooker As String, _
        ByVal pBookedDate As String, _
        ByVal pOutofPolicyReason As String, _
        ByVal pLast4Digits As String, _
        ByVal pTravellerEmail As String, _
        ByVal pBookerEmail As String, _
        ByVal pCostcode As String, _
        ByVal pRef2 As String, _
        ByVal pRef3 As String, _
        ByVal pBookerinitials As String, _
        ByVal pCategoryID As String, _
        ByVal pAICol6 As String, _
        ByVal pAICol7 As String, _
        ByVal pAICol8 As String, _
        ByVal pAICol9 As String, _
        ByVal pAICol10 As String, _
        ByVal pCurrency As String)
        mTransactionnumber = pTransactionnumber
        mTransactionlinenumber = pTransactionlinenumber
        mTransactiondate = pTransactiondate
        mArrivaldate = pArrivaldate
        mDeparturedate = pDeparturedate
        mPassengername = pPassengername
        mTotalamount = pTotalamount
        mVatamount = pVatamount
        mVatrate = pVatrate
        mNettamount = pNettamount
        mVenuename = pVenuename
        mVenuedetails = pVenuedetails
        mRoomdetails = pRoomdetails
        mCompany = pCompany
        mConfermainvoicenumber = pConfermainvoicenumber
        mCategory = pCategory
        mSupplierinvoice = pSupplierinvoice
        mGuestPNR = pGuestPNR
        mRef1 = pRef1
        mBooker = pBooker
        mBookedDate = pBookedDate
        mOutofPolicyReason = pOutofPolicyReason
        mLast4Digits = pLast4Digits
        mTravellerEmail = pTravellerEmail
        mCostcode = pCostcode
        mRef2 = pRef2
        mRef3 = pRef3
        mBookerinitials = pBookerinitials
        mCategoryID = pCategoryID
        mAICol6 = pAICol6
        mAICol7 = pAICol7
        mAICol8 = pAICol8
        mAICol9 = pAICol9
        mAICol10 = pAICol10
        mCurrency = pCurrency
        'R2.20C CR
        mBookerEmail = pBookerEmail
    End Sub

    'R2.20C CR
    Private mBookerEmail As String

    Private mTravellerEmail As String
    Private mOutofPolicyReason As String
    Private mLast4Digits As String
    Private mAICol6 As String
    Private mAICol7 As String
    Private mAICol8 As String
    Private mAICol9 As String
    Private mAICol10 As String
    Private mTransactionnumber As String
    Private mTransactionlinenumber As String
    Private mTransactiondate As String
    Private mDeparturedate As String
    Private mArrivaldate As String
    Private mPassengername As String
    Private mTotalamount As String
    Private mVatamount As String
    Private mVatrate As String
    Private mNettamount As String
    Private mVenuename As String
    Private mVenuedetails As String
    Private mRoomdetails As String
    Private mCompany As String
    Private mConfermainvoicenumber As String
    Private mCategory As String
    Private mSupplierinvoice As String
    Private mGuestPNR As String
    Private mRef1 As String
    Private mBooker As String
    Private mBookedDate As String
    Private mCostcode As String
    Private mRef2 As String
    Private mRef3 As String
    Private mBookerinitials As String
    Private mCategoryID As String
    Private mCurrency As String

    'R2.20C CR
    Public Property BookerEmail() As String
        Get
            Return mBookerEmail
        End Get
        Set(value As String)
            mBooker = value
        End Set
    End Property

    Public Property TravellerEmail() As String
        Get
            Return mTravellerEmail
        End Get
        Set(ByVal value As String)
            mTravellerEmail = value
        End Set
    End Property

    Public Property OutofPolicyReason() As String
        Get
            Return mOutofPolicyReason
        End Get
        Set(ByVal value As String)
            mOutofPolicyReason = value
        End Set
    End Property

    Public Property Last4Digits() As String
        Get
            Return mLast4Digits
        End Get
        Set(ByVal value As String)
            mLast4Digits = value
        End Set
    End Property

    Public Property Currency() As String
        Get
            Return mCurrency
        End Get
        Set(ByVal value As String)
            mCurrency = value
        End Set
    End Property

    Public Property AICol6() As String
        Get
            Return mAICol6
        End Get
        Set(ByVal value As String)
            mAICol6 = value
        End Set
    End Property

    Public Property AICol7() As String
        Get
            Return mAICol7
        End Get
        Set(ByVal value As String)
            mAICol7 = value
        End Set
    End Property

    Public Property AICol8() As String
        Get
            Return mAICol8
        End Get
        Set(ByVal value As String)
            mAICol8 = value
        End Set
    End Property

    Public Property AICol9() As String
        Get
            Return mAICol9
        End Get
        Set(ByVal value As String)
            mAICol9 = value
        End Set
    End Property

    Public Property AICol10() As String
        Get
            Return mAICol10
        End Get
        Set(ByVal value As String)
            mAICol10 = value
        End Set
    End Property

    Public Property Transactionnumber() As String
        Get
            Return mTransactionnumber
        End Get
        Set(ByVal value As String)
            mTransactionnumber = value
        End Set
    End Property

    Public Property Transactionlinenumber() As String
        Get
            Return mTransactionlinenumber
        End Get
        Set(ByVal value As String)
            mTransactionlinenumber = value
        End Set
    End Property

    Public Property Transactiondate() As String
        Get
            Return mTransactiondate
        End Get
        Set(ByVal value As String)
            mTransactiondate = value
        End Set
    End Property

    Public Property Arrivaldate() As String
        Get
            Return mArrivaldate
        End Get
        Set(ByVal value As String)
            mArrivaldate = value
        End Set
    End Property

    Public Property Departuredate() As String
        Get
            Return mDeparturedate
        End Get
        Set(ByVal value As String)
            mDeparturedate = value
        End Set
    End Property

    Public Property Passengername() As String
        Get
            Return mPassengername
        End Get
        Set(ByVal value As String)
            mPassengername = value
        End Set
    End Property

    Public Property Ref1() As String
        Get
            Return mRef1
        End Get
        Set(ByVal value As String)
            mRef1 = value
        End Set
    End Property

    Public Property Ref2() As String
        Get
            Return mRef2
        End Get
        Set(ByVal value As String)
            mRef2 = value
        End Set
    End Property

    Public Property Ref3() As String
        Get
            Return mRef3
        End Get
        Set(ByVal value As String)
            mRef3 = value
        End Set
    End Property

    Public Property Booker() As String
        Get
            Return mBooker
        End Get
        Set(ByVal value As String)
            mBooker = value
        End Set
    End Property

    Public Property BookedDate() As String
        Get
            Return mBookedDate
        End Get
        Set(ByVal value As String)
            mBookedDate = value
        End Set
    End Property

    Public Property Bookerinitials() As String
        Get
            Return mBookerinitials
        End Get
        Set(ByVal value As String)
            mBookerinitials = value
        End Set
    End Property

    Public Property Nettamount() As String
        Get
            Return mNettamount
        End Get
        Set(ByVal value As String)
            mNettamount = value
        End Set
    End Property

    Public Property Vatamount() As String
        Get
            Return mVatamount
        End Get
        Set(ByVal value As String)
            mVatamount = value
        End Set
    End Property

    Public Property Vatrate() As String
        Get
            Return mVatrate
        End Get
        Set(ByVal value As String)
            mVatrate = value
        End Set
    End Property

    Public Property Totalamount() As String
        Get
            Return mTotalamount
        End Get
        Set(ByVal value As String)
            mTotalamount = value
        End Set
    End Property

    Public Property Venuename() As String
        Get
            Return mVenuename
        End Get
        Set(ByVal value As String)
            mVenuename = value
        End Set
    End Property

    Public Property Venuedetails() As String
        Get
            Return mVenuedetails
        End Get
        Set(ByVal value As String)
            mVenuedetails = value
        End Set
    End Property

    Public Property Roomdetails() As String
        Get
            Return mRoomdetails
        End Get
        Set(ByVal value As String)
            mRoomdetails = value
        End Set
    End Property

    Public Property Company() As String
        Get
            Return mCompany
        End Get
        Set(ByVal value As String)
            mCompany = value
        End Set
    End Property

    Public Property Confermainvoicenumber() As String
        Get
            Return mConfermainvoicenumber
        End Get
        Set(ByVal value As String)
            mConfermainvoicenumber = value
        End Set
    End Property

    Public Property Category() As String
        Get
            Return mCategory
        End Get
        Set(ByVal value As String)
            mCategory = value
        End Set
    End Property

    Public Property Supplierinvoice() As String
        Get
            Return mSupplierinvoice
        End Get
        Set(ByVal value As String)
            mSupplierinvoice = value
        End Set
    End Property

    Public Property GuestPNR() As String
        Get
            Return mGuestPNR
        End Get
        Set(ByVal value As String)
            mGuestPNR = value
        End Set
    End Property

    Public Property Costcode() As String
        Get
            Return mCostcode
        End Get
        Set(ByVal value As String)
            mCostcode = value
        End Set
    End Property

    Public Property CategoryID() As String
        Get
            Return mCategoryID
        End Get
        Set(ByVal value As String)
            mCategoryID = value
        End Set
    End Property

End Class

Public Class HighWaysReader

    Private Shared ReadOnly className As String

    Protected Shared log As log4net.ILog = _
      log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub


    ''' <summary>
    ''' Function checkLength - tests the length of the passed in value
    ''' </summary>
    ''' <param name="fieldName"></param>
    ''' <param name="a"></param>
    ''' <param name="l"></param>
    ''' <returns>An exception if the test fails</returns>
    ''' <remarks>Created 13/03/2009 Nick Massarella</remarks>
    Private Function checkLength(ByVal fieldName As String, ByVal a As String, ByVal l As Integer) As Boolean
        'skip check if array is too short: return true by default
        If a.Length > l Then
            Throw New Exception("field too long: " & fieldName)
        End If
    End Function

    Public Function getHighwaysBookings() As Integer
        Try
            Dim oDetails As List(Of clsFeedBookedData)
            oDetails = clsFeedBookedData.highwaysList()

            Dim dinosaurs As New List(Of dataImportHighways)

            'work out VAT from booked date
            Dim VatCalc As Double = 0
            Dim VatRate As Double = 17.5
            For Each oDetail As clsFeedBookedData In oDetails
                If oDetail.BookedDate < CDate("01/01/2011") Then
                    VatCalc = 1.175
                ElseIf oDetail.BookedDate < CDate("01/01/2011") Then
                    VatCalc = 1.2
                    VatRate = 20
                Else
                    VatCalc = 1.175
                End If
                Dim v As New dataImportHighways(CStr(oDetail.BookingId), _
                                                CStr(oDetail.LineNumber), _
                                                CStr(oDetail.BookedDate), _
                                                CStr(oDetail.DepartureDate), _
                                                CStr(oDetail.ArrivalDate), _
                                                oDetail.LeadPassengerName, _
                                                CStr(oDetail.TotalBilledInGbp), _
                                                CStr(oDetail.TotalBilledInGbp - (Math.Round(CDbl(oDetail.TotalBilledInGbp / VatCalc), 2))), _
                                                CStr(VatRate), _
                                                CStr(Math.Round(CDbl(oDetail.TotalBilledInGbp / VatCalc), 2)), _
                                                oDetail.SupplierName, _
                                                oDetail.HotelDetails, _
                                                oDetail.RoomDetails, _
                                                oDetail.Groupname, _
                                                "0", _
                                                "Room", _
                                                "", _
                                                oDetail.GuestPNR, _
                                                oDetail.BookingReference, _
                                                oDetail.RefBooker, _
                                                CStr(oDetail.BookedDate), _
                                                oDetail.OutofPolicyReason, _
                                                "", _
                                                oDetail.TravellerEmail, _
                                                oDetail.BookerEmail, _
                                                oDetail.AICostCode, _
                                                oDetail.AIBillInstruct, _
                                                oDetail.AIComments, _
                                                oDetail.AIAgentBkr, _
                                                CStr(clsStuff.notWholeNumber(FeedCategory.getCatergoryIDFromName("Room"))), _
                                                oDetail.AICol6, _
                                                oDetail.AICol7, _
                                                oDetail.AICol8, _
                                                oDetail.AICol9, _
                                                oDetail.AICol10, _
                                                oDetail.TransactionCurrency)
                dinosaurs.Add(v)
            Next

            Dim Sorter As New Sorter(Of dataImportHighways)

            Sorter.SortString = "Transactionnumber,GuestPNR,CategoryID"
            dinosaurs.Sort(Sorter)

            Dim intRet As Integer = saveData(dinosaurs)
            If intRet = -1 Then
                Return -1
            Else
                Return intRet
            End If

        Catch ex As Exception
            log.Error(ex.Message)
            Return -1
        End Try
    End Function

    Private Structure getGroupDetailsRet
        Public pintgroupid, pintcompanyid As Integer
    End Structure

    Private Function getGroupDetails(ByVal pstrName As String) As getGroupDetailsRet
        Dim ret As New getGroupDetailsRet
        Dim intgroupid As Integer  = clsStuff.notWholeNumber(clsGroup.getGroupIDFromName(pstrName))
        ret.pintcompanyid = 0
        ret.pintgroupid = intgroupid
        Return ret
    End Function

    Private Function saveData(ByVal dinosaurs As List(Of dataImportHighways)) As Integer
        Dim strErrorBookingIDs As String = ""
        Dim strCurrentBookingID As String = ""
        Dim strLastBookingID As String = ""
        Dim strCurrentGuestPNR As String = ""
        Dim strLastGuestPNR As String = ""
        Dim strerror As String = ""
        Dim intcounter As Integer = 0
        Dim blncancel As Boolean = False

        For Each dino As dataImportHighways In dinosaurs
            Try
                Dim strpassengername As String = dino.Passengername.Replace("NULL", "")
                strpassengername = strpassengername.ToLower.Replace("mr ", "")
                strpassengername = strpassengername.ToLower.Replace("mrs ", "")
                strpassengername = strpassengername.ToLower.Replace("ms ", "")
                strpassengername = strpassengername.ToLower.Replace("dr ", "")
                strpassengername = strpassengername.ToLower.Replace("miss ", "")
                strpassengername = strpassengername.ToLower.Replace("prof ", "")
                strpassengername = strpassengername.Trim

                Dim intIndexOFSpace As Integer = strpassengername.IndexOf(" ")

                If intIndexOFSpace > 0 Then
                    Dim strfirstname As String = Mid(strpassengername, 1, intIndexOFSpace)
                    Dim strlastname As String = Mid(strpassengername, intIndexOFSpace + 1)

                    strpassengername = strlastname.Trim & "/" & strfirstname.Trim
                End If

                'just check to see if headers have been included
                'first lets check the group exists
                strCurrentBookingID = dino.Transactionnumber
                strCurrentGuestPNR = dino.GuestPNR
                If dino.Company = "LV" Then
                    dino.Company = "LV="
                ElseIf dino.Company.ToUpper = "ANCHOR TRUST" Then
                    dino.Company = "BS - ANCHOR TRUST"
                ElseIf dino.Company.ToUpper = "CIMA" Then
                    'Dim str As String = ""
                End If

                'first try get group details from company name
                Dim retValues As getGroupDetailsRet = getGroupDetails(dino.Company)

                If retValues.pintgroupid = 0 Then
                    Throw New Exception("Group '" & dino.Company & "' does not exist.")
                End If
                'get transaction charge type, PP or BKG
                Dim oP As clsClientOption
                oP = clsClientOption.get(retValues.pintgroupid)
                Dim strTType As String = "BKG"
                If Not oP Is Nothing Then
                    If Not oP.TransactionType Is Nothing Then
                        strTType = oP.TransactionType
                    End If
                End If

                'R2.7 NM
                ' Need to get full venuename as Conferma chops off the name if it's too long
                Dim NewVenueName As String = Mid(dino.Venuedetails, 1, dino.Venuedetails.IndexOf(";"))
                dino.Venuename = NewVenueName
                checkLength("Last4Digits", dino.Last4Digits, 4)
                checkLength("OutofPolicyReason", dino.OutofPolicyReason, 200)
                checkLength("Passengername", dino.Passengername, 200)
                checkLength("Ref1", dino.Ref1, 500)
                checkLength("Ref2", dino.Ref2, 500)
                checkLength("Ref3", dino.Ref3, 500)
                checkLength("Booker", dino.Booker, 100)
                checkLength("Bookerinitials", dino.Bookerinitials, 20)
                checkLength("VenueName", dino.Venuename, 200)
                checkLength("Venuedetails", dino.Venuedetails, 3000)
                checkLength("Roomdetails", dino.Roomdetails, 200)
                checkLength("Category", dino.Category, 50)
                checkLength("Supplierinvoice", dino.Supplierinvoice, 1000)
                checkLength("GuestPNR", dino.GuestPNR, 200)
                checkLength("Costcode", dino.Costcode, 1000)
                checkLength("AICol6", dino.AICol6, 200)
                checkLength("AICol7", dino.AICol7, 200)
                checkLength("AICol8", dino.AICol8, 200)
                checkLength("AICol9", dino.AICol9, 200)
                checkLength("AICol10", dino.AICol10, 200)
                checkLength("Currency", dino.Currency, 100)
                checkLength("TravellerEmail", dino.TravellerEmail, 200)
                checkLength("BookerEmail", dino.BookerEmail, 200)

                Try
                    Dim inttest As Integer = CInt(dino.Transactionnumber)
                Catch ex As Exception
                    Throw New Exception("Transactionnumber is not a number.")
                End Try

                If dino.Venuename = "" Then
                    Throw New Exception("There is no Venue name.")
                End If

                Try
                    If clsStuff.notString(dino.Transactiondate.Replace("NULL", "")) <> "" Then
                        Dim dttest As Date = CDate(dino.Transactiondate)
                    Else
                        dino.Transactiondate = dino.Arrivaldate
                    End If
                Catch ex As Exception
                    Throw New Exception("Transactiondate is not a date.")
                End Try
                Try
                    Dim dttest As Date = CDate(dino.Departuredate)
                Catch ex As Exception
                    Throw New Exception("Departuredate is not a date.")
                End Try
                Try
                    Dim dttest As Date = CDate(dino.Arrivaldate)
                Catch ex As Exception
                    Throw New Exception("Arrivaldate is not a date.")
                End Try
                Try
                    Dim dttest As Date = CDate(dino.BookedDate)
                Catch ex As Exception
                    Throw New Exception("BookedDate is not a date.")
                End Try
                Try
                    Dim dbltest As Double = CDbl(dino.Nettamount)
                Catch ex As Exception
                    Throw New Exception("Nettamount is not a numeric value.")
                End Try
                Try
                    Dim dbltest As Double = CDbl(dino.Vatamount)
                Catch ex As Exception
                    Throw New Exception("Vatamount is not a numeric value.")
                End Try
                Try
                    Dim dbltest As Double = CDbl(dino.Vatrate)
                Catch ex As Exception
                    Throw New Exception("Vatrate is not a numeric value.")
                End Try
                Try
                    Dim dbltest As Double = CDbl(dino.Totalamount)
                Catch ex As Exception
                    Throw New Exception("Totalamount is not a numeric value.")
                End Try

                Dim venues As List(Of clsVenue)
                Dim venuereference As Integer = 0
                Dim venueDD As Double = 0
                Dim venueEX As Double = 0
                Dim venueBOSScode As String = ""
                Dim venuename As String = ""
                Dim venuetransient As String = ""
                Dim venuetransientgroup As String = ""
                Dim venuetransientdefault As String = ""

                Dim Importvenuename As String = dino.Venuename

                'first check to see if hotel name used by Conferma has a different name in VenuesDB
                Dim strAlternativeName As String = clsStuff.notString(clsVenue.venueAlternativenameCheck _
                                                             (Importvenuename))
                If strAlternativeName <> "" Then
                    venuename = strAlternativeName
                Else
                    venuename = Importvenuename
                End If

                'now go and see if the name exists in VenuesDB if it does bring back values
                venues = clsVenue.venueExactNameFind(venuename.Replace("'", "''''"), CStr(retValues.pintgroupid))
                If venues.Count > 0 Then
                    For Each venue As clsVenue In venues
                        venuereference = venue.vereference
                        venueDD = venue.vedd
                        venueEX = venue.veex
                        venueBOSScode = venue.bosscode
                        venuename = venue.vename
                        venuetransient = venue.transient
                        venuetransientgroup = venue.transientgroup
                        venuetransientdefault = venue.transientdefault
                    Next
                End If

                'uses new transient values from Database, 
                'individual overrides all, 
                'then group, then default, then conference
                If venuetransient <> "" Then
                    venueDD = CDbl(venuetransient)
                ElseIf venuetransientgroup <> "" Then
                    venueDD = CDbl(venuetransientgroup)
                ElseIf venuetransientgroup <> "" Then
                    venueDD = CDbl(venuetransientdefault)
                End If

                'get parameterid and transaction value form bookerinitials
                Dim strcode As String = ""
                If dino.Bookerinitials.ToLower = "con" Or dino.Bookerinitials = "" Then
                    strcode = "online"
                    If dino.Bookerinitials = "" Or dino.Bookerinitials.ToLower = "nys corporate" Then
                        dino.Bookerinitials = "CON"
                    End If
                ElseIf dino.Bookerinitials.ToLower = "man" Then
                    strcode = "offline"
                Else
                    strcode = "offline"
                End If
                'get categoryid from database
                If dino.Category.ToLower = "cancellation fee" Then
                    dino.Category = "Room"
                    blncancel = True
                End If
                Dim intcatid As Integer = clsStuff.notWholeNumber _
                                        (FeedCategory.getCatergoryIDFromName(dino.Category))
                'get parameterid from database
                Dim dtBookedDate As Date = CDate(dino.BookedDate)
                dtBookedDate = CDate(Format(dtBookedDate, "dd/MM/yyyy"))

                Dim strType As String = dino.Category

                If dino.Category.ToLower = "room" Then
                    strType = "room"
                ElseIf dino.Category.ToLower = "meals" Or _
                    dino.Category.ToLower = "beverages" Then
                    strType = "meals/beverages"
                Else 'If dino.Category.ToLower <> "room" Then
                    strType = "all extras"
                End If

                Dim intExtrasCharges As Integer = FeedParameter.parameterCheck(retValues.pintgroupid)
                Dim intparamid As Integer = FeedParameter.parameterValueGet(strType, _
                                                    strcode, dtBookedDate, retValues.pintgroupid)
                'this will add a transaction value to each person
                'if group is set us as PP booking types
                If intparamid > 0 Then
                    If strTType = "PP" Then
                        If strLastGuestPNR = strCurrentGuestPNR And strCurrentGuestPNR <> "" Then
                            If dino.Category.ToLower = "room" Then
                                intparamid = 0
                            End If
                        End If
                    End If
                End If

                Dim fd As New FeedImportData(0, clsStuff.notWholeNumber(dino.Transactionnumber), _
                                             clsStuff.notWholeNumber(dino.Transactionlinenumber), _
                                             CType(dino.Transactiondate, Date?), _
                                             CType(dino.Arrivaldate, Date?), _
                                             CType(dino.Departuredate, Date?), _
                                             strpassengername, _
                                             dino.Ref1.Replace("NULL", ""), _
                                             dino.Ref2.Replace("NULL", ""), _
                                             dino.Ref3.Replace("NULL", ""), _
                                             "", _
                                             dino.Booker.Replace("NULL", ""), _
                                             dino.Bookerinitials.Replace("NULL", ""), _
                                             CType(dino.Nettamount, Double?), _
                                             CType(dino.Vatamount, Double?), _
                                             CType(dino.Vatrate, Double?), _
                                             CType(dino.Totalamount, Double?), _
                                             venuename, _
                                             dino.Venuedetails.Replace("NULL", ""), _
                                             dino.Roomdetails.Replace("NULL", ""), _
                                             retValues.pintgroupid, _
                                             dino.Company, _
                                             CType(dino.Confermainvoicenumber, Integer?), _
                                             intcatid, _
                                             "", "", _
                                             dino.Supplierinvoice.Replace("NULL", ""), _
                                             dino.GuestPNR.Replace("NULL", ""), _
                                             dino.Costcode.Replace("NULL", "").Replace(" ", ""), _
                                             intparamid, venuereference, _
                                             venueBOSScode, venueEX, venueDD, _
                                             0, "", Now, Now, 0, "", 0, "", 0, "", "", 0, _
                                             dtBookedDate, _
                                             False, False, _
                                             dino.AICol6, _
                                             dino.AICol7, _
                                             dino.AICol8, _
                                             dino.AICol9, _
                                             dino.AICol10, _
                                             dino.Currency, _
                                             blncancel, _
                                             False, _
                                             dino.OutofPolicyReason, _
                                             dino.Last4Digits, _
                                             dino.TravellerEmail, _
                                             dino.BookerEmail)

                If fd.saveImport() = 0 Then

                    strErrorBookingIDs = strErrorBookingIDs & strCurrentBookingID & ","
                    strerror = "Failure in feedimportdata_save stored procedure"
                Else
                    strerror = "OK"
                End If
            Catch ex As Exception
                Return -1

            End Try
            intcounter = intcounter + 1
            strLastGuestPNR = dino.GuestPNR
        Next
        Return intcounter
    End Function

End Class

