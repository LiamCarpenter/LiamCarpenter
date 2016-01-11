Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class Bednight

    Public Sub New( _
        ByVal pInm_custid As String, _
        ByVal pInm_ldname As String, _
        ByVal pPassenger As String, _
        ByVal pBooker As String, _
        ByVal pBookedDate As String, _
        ByVal pInm_costc As String, _
        ByVal pTot_cref1 As String, _
        ByVal pTot_cref2 As String, _
        ByVal pRef_3 As String, _
        ByVal pRef_4 As String, _
        ByVal pRef_5 As String, _
        ByVal pTot_pono As String, _
        ByVal pDivision1 As String, _
        ByVal pDivision2 As String, _
        ByVal pDivision3 As String, _
        ByVal pArrivaldate As String, _
        ByVal pDeparturedate As String, _
        ByVal pNights As String, _
        ByVal pVenuename As String, _
        ByVal pSup_name As String, _
        ByVal pHotelchain As String, _
        ByVal pTo_name As String, _
        ByVal pSup_add2 As String, _
        ByVal pPostcode As String, _
        ByVal pRoomdetails As String, _
        ByVal pNett As String, _
        ByVal pGross As String, _
        ByVal pRoomnett As String, _
        ByVal pRoomgross As String, _
        ByVal pRoomcostpernight As String, _
        ByVal pExtrasnett As String, _
        ByVal pExtrasgross As String, _
        ByVal pExtrasdetails As String, _
        ByVal pBookingfeenett As String, _
        ByVal pBookingfeegross As String, _
        ByVal pTransfee As String, _
        ByVal pExtrastransfee As String, _
        ByVal pComparison As String, _
        ByVal pSaving As String, _
        ByVal pCommission As String, _
        ByVal pInvoiceno As String, _
        ByVal pBookingref As String, _
        ByVal pBookerinitials As String, _
        ByVal pBookingtype As String, _
        ByVal pAICol6 As String, _
        ByVal pOutofPolicyReason As String, _
        ByVal pInm_invdt As String, _
        ByVal pTotalincfees As String, _
        ByVal pTravellers As String, _
        ByVal pClientMI As String, _
        ByVal pCancellationDate As String)
        mInm_custid = pInm_custid
        mInm_ldname = pInm_ldname
        mPassenger = pPassenger
        mBooker = pBooker
        mBookedDate = pBookedDate
        mInm_costc = pInm_costc
        mTot_cref1 = pTot_cref1
        mTot_cref2 = pTot_cref2
        mRef_3 = pRef_3
        mRef_4 = pRef_4
        mRef_5 = pRef_5
        mTot_pono = pTot_pono
        mDivision1 = pDivision1
        mDivision2 = pDivision2
        mDivision3 = pDivision3
        mArrivaldate = pArrivaldate
        mDeparturedate = pDeparturedate
        mNights = pNights
        mVenuename = pVenuename
        mSup_name = pSup_name
        mHotelchain = pHotelchain
        mTo_name = pTo_name
        mSup_add2 = pSup_add2
        mPostcode = pPostcode
        mRoomdetails = pRoomdetails
        mNett = pNett
        mGross = pGross
        mRoomnett = pRoomnett
        mRoomgross = pRoomgross
        mRoomcostpernight = pRoomcostpernight
        mExtrasnett = pExtrasnett
        mExtrasgross = pExtrasgross
        mExtrasdetails = pExtrasdetails
        mBookingfeenett = pBookingfeenett
        mBookingfeegross = pBookingfeegross
        mTransfee = pTransfee
        mExtrastransfee = pExtrastransfee
        mComparison = pComparison
        mSaving = pSaving
        mCommission = pCommission
        mInvoiceno = pInvoiceno
        mBookingref = pBookingref
        mBookerinitials = pBookerinitials
        mBookingtype = pBookingtype
        mAICol6 = pAICol6
        mOutofPolicyReason = pOutofPolicyReason
        mInm_invdt = pInm_invdt
        mTotalincfees = pTotalincfees
        mTravellers = pTravellers
        mClientMI = pClientMI
        mCancellationDate = pCancellationDate
    End Sub

    Public Sub New( _
        ByVal pInvoiceno As String, _
        ByVal pInm_ldname As String, _
        ByVal pCategoryID As Integer, _
        ByVal ptransactionnumber As Integer, _
        ByVal pExtrasdetails As String)
        mInvoiceno = pInvoiceno
        mInm_ldname = pInm_ldname
        mCategoryID = pCategoryID
        mtransactionnumber = ptransactionnumber
        mExtrasdetails = pExtrasdetails
    End Sub

    Public Sub New( _
)
    End Sub

    Private mCancellationDate As String
    Private mCategoryID As Integer
    Private mtransactionnumber As Integer
    Private mInm_custid As String
    Private mInm_ldname As String
    Private mPassenger As String
    Private mBooker As String
    Private mBookedDate As String
    Private mInm_costc As String
    Private mTot_cref1 As String
    Private mTot_cref2 As String
    Private mRef_3 As String
    Private mRef_4 As String
    Private mRef_5 As String
    Private mTot_pono As String
    Private mDivision1 As String
    Private mDivision2 As String
    Private mDivision3 As String
    Private mArrivaldate As String
    Private mDeparturedate As String
    Private mNights As String
    Private mVenuename As String
    Private mSup_name As String
    Private mHotelchain As String
    Private mTo_name As String
    Private mSup_add2 As String
    Private mPostcode As String
    Private mRoomdetails As String
    Private mNett As String
    Private mGross As String
    Private mRoomnett As String
    Private mRoomgross As String
    Private mRoomcostpernight As String
    Private mExtrasnett As String
    Private mExtrasgross As String
    Private mExtrasdetails As String
    Private mBookingfeenett As String
    Private mBookingfeegross As String
    Private mTransfee As String
    Private mExtrastransfee As String
    Private mComparison As String
    Private mSaving As String
    Private mCommission As String
    Private mInvoiceno As String
    Private mBookingref As String
    Private mBookerinitials As String
    Private mBookingtype As String
    Private mAICol6 As String
    Private mOutofPolicyReason As String
    Private mInm_invdt As String
    Private mTotalincfees As String
    Private mTravellers As String
    Private mClientMI As String

    Public Property transactionnumber() As Integer
        Get
            Return mtransactionnumber
        End Get
        Set(ByVal value As Integer)
            mtransactionnumber = value
        End Set
    End Property

    Public Property categoryid() As Integer
        Get
            Return mCategoryID
        End Get
        Set(ByVal value As Integer)
            mCategoryID = value
        End Set
    End Property

    Public Property CancellationDate() As String
        Get
            Return mCancellationDate
        End Get
        Set(ByVal value As String)
            mCancellationDate = value
        End Set
    End Property

    Public Property Inm_custid() As String
        Get
            Return mInm_custid
        End Get
        Set(ByVal value As String)
            mInm_custid = value
        End Set
    End Property

    Public Property Inm_ldname() As String
        Get
            Return mInm_ldname
        End Get
        Set(ByVal value As String)
            mInm_ldname = value
        End Set
    End Property

    Public Property Passenger() As String
        Get
            Return mPassenger
        End Get
        Set(ByVal value As String)
            mPassenger = value
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

    Public Property Inm_costc() As String
        Get
            Return mInm_costc
        End Get
        Set(ByVal value As String)
            mInm_costc = value
        End Set
    End Property

    Public Property Tot_cref1() As String
        Get
            Return mTot_cref1
        End Get
        Set(ByVal value As String)
            mTot_cref1 = value
        End Set
    End Property

    Public Property Tot_cref2() As String
        Get
            Return mTot_cref2
        End Get
        Set(ByVal value As String)
            mTot_cref2 = value
        End Set
    End Property

    Public Property Ref_3() As String
        Get
            Return mRef_3
        End Get
        Set(ByVal value As String)
            mRef_3 = value
        End Set
    End Property

    Public Property Ref_4() As String
        Get
            Return mRef_4
        End Get
        Set(ByVal value As String)
            mRef_4 = value
        End Set
    End Property

    Public Property Ref_5() As String
        Get
            Return mRef_5
        End Get
        Set(ByVal value As String)
            mRef_5 = value
        End Set
    End Property

    Public Property Tot_pono() As String
        Get
            Return mTot_pono
        End Get
        Set(ByVal value As String)
            mTot_pono = value
        End Set
    End Property

    Public Property Division1() As String
        Get
            Return mDivision1
        End Get
        Set(ByVal value As String)
            mDivision1 = value
        End Set
    End Property

    Public Property Division2() As String
        Get
            Return mDivision2
        End Get
        Set(ByVal value As String)
            mDivision2 = value
        End Set
    End Property

    Public Property Division3() As String
        Get
            Return mDivision3
        End Get
        Set(ByVal value As String)
            mDivision3 = value
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

    Public Property Nights() As String
        Get
            Return mNights
        End Get
        Set(ByVal value As String)
            mNights = value
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

    Public Property Sup_name() As String
        Get
            Return mSup_name
        End Get
        Set(ByVal value As String)
            mSup_name = value
        End Set
    End Property

    Public Property Hotelchain() As String
        Get
            Return mHotelchain
        End Get
        Set(ByVal value As String)
            mHotelchain = value
        End Set
    End Property

    Public Property To_name() As String
        Get
            Return mTo_name
        End Get
        Set(ByVal value As String)
            mTo_name = value
        End Set
    End Property

    Public Property Sup_add2() As String
        Get
            Return mSup_add2
        End Get
        Set(ByVal value As String)
            mSup_add2 = value
        End Set
    End Property

    Public Property Postcode() As String
        Get
            Return mPostcode
        End Get
        Set(ByVal value As String)
            mPostcode = value
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

    Public Property Nett() As String
        Get
            Return mNett
        End Get
        Set(ByVal value As String)
            mNett = value
        End Set
    End Property

    Public Property Gross() As String
        Get
            Return mGross
        End Get
        Set(ByVal value As String)
            mGross = value
        End Set
    End Property

    Public Property Roomnett() As String
        Get
            Return mRoomnett
        End Get
        Set(ByVal value As String)
            mRoomnett = value
        End Set
    End Property

    Public Property Roomgross() As String
        Get
            Return mRoomgross
        End Get
        Set(ByVal value As String)
            mRoomgross = value
        End Set
    End Property

    Public Property Roomcostpernight() As String
        Get
            Return mRoomcostpernight
        End Get
        Set(ByVal value As String)
            mRoomcostpernight = value
        End Set
    End Property

    Public Property Extrasnett() As String
        Get
            Return mExtrasnett
        End Get
        Set(ByVal value As String)
            mExtrasnett = value
        End Set
    End Property

    Public Property Extrasgross() As String
        Get
            Return mExtrasgross
        End Get
        Set(ByVal value As String)
            mExtrasgross = value
        End Set
    End Property

    Public Property Extrasdetails() As String
        Get
            Return mExtrasdetails
        End Get
        Set(ByVal value As String)
            mExtrasdetails = value
        End Set
    End Property

    Public Property Bookingfeenett() As String
        Get
            Return mBookingfeenett
        End Get
        Set(ByVal value As String)
            mBookingfeenett = value
        End Set
    End Property

    Public Property Bookingfeegross() As String
        Get
            Return mBookingfeegross
        End Get
        Set(ByVal value As String)
            mBookingfeegross = value
        End Set
    End Property

    Public Property Transfee() As String
        Get
            Return mTransfee
        End Get
        Set(ByVal value As String)
            mTransfee = value
        End Set
    End Property

    Public Property Extrastransfee() As String
        Get
            Return mExtrastransfee
        End Get
        Set(ByVal value As String)
            mExtrastransfee = value
        End Set
    End Property

    Public Property Comparison() As String
        Get
            Return mComparison
        End Get
        Set(ByVal value As String)
            mComparison = value
        End Set
    End Property

    Public Property Saving() As String
        Get
            Return mSaving
        End Get
        Set(ByVal value As String)
            mSaving = value
        End Set
    End Property

    Public Property Commission() As String
        Get
            Return mCommission
        End Get
        Set(ByVal value As String)
            mCommission = value
        End Set
    End Property

    Public Property Invoiceno() As String
        Get
            Return mInvoiceno
        End Get
        Set(ByVal value As String)
            mInvoiceno = value
        End Set
    End Property

    Public Property Bookingref() As String
        Get
            Return mBookingref
        End Get
        Set(ByVal value As String)
            mBookingref = value
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

    Public Property Bookingtype() As String
        Get
            Return mBookingtype
        End Get
        Set(ByVal value As String)
            mBookingtype = value
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

    Public Property OutofPolicyReason() As String
        Get
            Return mOutofPolicyReason
        End Get
        Set(ByVal value As String)
            mOutofPolicyReason = value
        End Set
    End Property

    Public Property Inm_invdt() As String
        Get
            Return mInm_invdt
        End Get
        Set(ByVal value As String)
            mInm_invdt = value
        End Set
    End Property

    Public Property Totalincfees() As String
        Get
            Return mTotalincfees
        End Get
        Set(ByVal value As String)
            mTotalincfees = value
        End Set
    End Property

    Public Property Travellers() As String
        Get
            Return mTravellers
        End Get
        Set(ByVal value As String)
            mTravellers = value
        End Set
    End Property

    Public Property ClientMI() As String
        Get
            Return mClientMI
        End Get
        Set(ByVal value As String)
            mClientMI = value
        End Set
    End Property

    Private Shared Function makeBednightFromRow( _
            ByVal r As IDataReader _
        ) As Bednight
        Return New Bednight( _
                clsNYS.notString(r.Item("inm_custid")), _
                clsNYS.notString(r.Item("inm_ldname")), _
                clsNYS.notString(r.Item("passenger")), _
                clsNYS.notString(r.Item("booker")), _
                clsNYS.notString(r.Item("BookedDate")), _
                clsNYS.notString(r.Item("inm_costc")), _
                clsNYS.notString(r.Item("tot_cref1")), _
                clsNYS.notString(r.Item("tot_cref2")), _
                clsNYS.notString(r.Item("ref_3")), _
                clsNYS.notString(r.Item("ref_4")), _
                clsNYS.notString(r.Item("ref_5")), _
                clsNYS.notString(r.Item("tot_pono")), _
                clsNYS.notString(r.Item("division1")), _
                clsNYS.notString(r.Item("division2")), _
                clsNYS.notString(r.Item("division3")), _
                clsNYS.notString(r.Item("arrivaldate")), _
                clsNYS.notString(r.Item("departuredate")), _
                clsNYS.notString(r.Item("nights")), _
                clsNYS.notString(r.Item("venuename")), _
                clsNYS.notString(r.Item("sup_name")), _
                clsNYS.notString(r.Item("hotelchain")), _
                clsNYS.notString(r.Item("to_name")), _
                clsNYS.notString(r.Item("sup_add2")), _
                clsNYS.notString(r.Item("postcode")), _
                clsNYS.notString(r.Item("roomdetails")), _
                clsNYS.notString(r.Item("nett")), _
                clsNYS.notString(r.Item("gross")), _
                clsNYS.notString(r.Item("roomnett")), _
                clsNYS.notString(r.Item("roomgross")), _
                clsNYS.notString(r.Item("roomcostpernight")), _
                clsNYS.notString(r.Item("extrasnett")), _
                clsNYS.notString(r.Item("extrasgross")), _
                clsNYS.notString(r.Item("extrasdetails")), _
                clsNYS.notString(r.Item("bookingfeenett")), _
                clsNYS.notString(r.Item("bookingfeegross")), _
                clsNYS.notString(r.Item("transfee")), _
                clsNYS.notString(r.Item("extrastransfee")), _
                clsNYS.notString(r.Item("Comparison")), _
                clsNYS.notString(r.Item("saving")), _
                clsNYS.notString(r.Item("commission")), _
                clsNYS.notString(r.Item("invoiceno")), _
                clsNYS.notString(r.Item("bookingref")), _
                clsNYS.notString(r.Item("bookerinitials")), _
                clsNYS.notString(r.Item("bookingtype")), _
                clsNYS.notString(r.Item("AICol6")), _
                clsNYS.notString(r.Item("OutofPolicyReason")), _
                clsNYS.notString(r.Item("inm_invdt")), _
                clsNYS.notString(r.Item("totalincfees")), _
                clsNYS.notString(r.Item("travellers")), _
                clsNYS.notString(r.Item("clientMI")), _
                clsNYS.notString(r.Item("CancellationDate")))
    End Function

    Private Shared Function makeBednightExtrasFromRow( _
            ByVal r As IDataReader _
        ) As Bednight
        Return New Bednight( _
                clsNYS.notString(r.Item("invoiceno")), _
                clsNYS.notString(r.Item("inm_ldname")), _
                clsNYS.notInteger(r.Item("CategoryID")), _
                clsNYS.notInteger(r.Item("transactionnumber")), _
                clsNYS.notString(r.Item("extrasdetails")))
    End Function

    Public Shared Function bedNightSalesWithCubitRecords(ByVal pstrStartDate As String, ByVal pstrEndDate As String) As List(Of Bednight)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of Bednight)()
            Using r As IDataReader = dbh.callSP("bedNightSalesWithCubitRecords", "@StartDate", pstrStartDate, "@EndDate", pstrEndDate)
                While r.Read()
                    ret.Add(makeBednightFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function bedNightSalesWithOutCubitRecords(ByVal pstrStartDate As String, ByVal pstrEndDate As String) As List(Of Bednight)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of Bednight)()
            Using r As IDataReader = dbh.callSP("bedNightSalesWithOutCubitRecords", "@StartDate", pstrStartDate, "@EndDate", pstrEndDate)
                While r.Read()
                    ret.Add(makeBednightFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function bedNightCredits(ByVal pstrStartDate As String, ByVal pstrEndDate As String) As List(Of Bednight)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of Bednight)()
            Using r As IDataReader = dbh.callSP("bedNightCredits", "@StartDate", pstrStartDate, "@EndDate", pstrEndDate)
                While r.Read()
                    ret.Add(makeBednightFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function bedNightGroups(ByVal pstrStartDate As String, ByVal pstrEndDate As String) As List(Of Bednight)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of Bednight)()
            Using r As IDataReader = dbh.callSP("bedNightGroups", "@StartDate", pstrStartDate, "@EndDate", pstrEndDate)
                While r.Read()
                    ret.Add(makeBednightFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function bedNightExtras(ByVal pstrStartDate As String, ByVal pstrEndDate As String) As List(Of Bednight)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of Bednight)()
            Using r As IDataReader = dbh.callSP("bedNightExtras", "@StartDate", pstrStartDate, "@EndDate", pstrEndDate)
                While r.Read()
                    ret.Add(makeBednightExtrasFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

End Class
