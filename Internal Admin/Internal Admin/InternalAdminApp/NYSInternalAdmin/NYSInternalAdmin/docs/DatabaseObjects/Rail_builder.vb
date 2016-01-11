Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class RailBuilder

    Public Sub New( _
        ByVal pEvolvidataid As String, _
        ByVal pInm_custid As String, _
        ByVal pTransactiontype As String, _
        ByVal pBookingref As String, _
        ByVal pTicketref As String, _
        ByVal pTransactionnumber As String, _
        ByVal pMachine As String, _
        ByVal pCostcentre As String, _
        ByVal pTot_cref1 As String, _
        ByVal pTot_cref2 As String, _
        ByVal pTot_pono As String, _
        ByVal pBookerLocation As String, _
        ByVal pBookingAgentFirstName As String, _
        ByVal pBookingAgentLastName As String, _
        ByVal pTravellerTitle As String, _
        ByVal pTravellerFirstName As String, _
        ByVal pTravellerLastName As String, _
        ByVal pDivision1 As String, _
        ByVal pDivision2 As String, _
        ByVal pDivision3 As String, _
        ByVal pOriginName As String, _
        ByVal pDestinationName As String, _
        ByVal pTicketRoute As String, _
        ByVal pTicketClass As String, _
        ByVal pTicketCode As String, _
        ByVal pTicketName As String, _
        ByVal pTickettype As String, _
        ByVal pSinglereturn As String, _
        ByVal pJourneyleg As String, _
        ByVal pTOCName As String, _
        ByVal pDeparture As String, _
        ByVal pTransactionDate As String, _
        ByVal pLeadtime As String, _
        ByVal pLowestreason As String, _
        ByVal pGrossacceptedfare As String, _
        ByVal pDiscount As String, _
        ByVal pFees As String, _
        ByVal pNet As String, _
        ByVal pLowestpossiblefare As String, _
        ByVal pFullyFlexibleFare As String, _
        ByVal pSaving As String, _
        ByVal pSavingdeclined As String, _
        ByVal pEOEPositiveSaving As String, _
        ByVal pEOENegativeSaving As String, _
        ByVal pEOEFurtherSavingsDeclined As String, _
        ByVal pSavingsperc As String, _
        ByVal pOverspend As String, _
        ByVal pDistance As String, _
        ByVal pEmissions As String, _
        ByVal pJourneytime As String, _
        ByVal pInm_no As String, _
        ByVal pFulfilmentType As String, _
        ByVal pBookingmethod As String, _
        ByVal pReasonfortravel As String, _
        ByVal pMail As String, _
        ByVal pTransfee As String, _
        ByVal pPostfee As String, _
        ByVal pCommission As String)
        mEvolvidataid = pEvolvidataid
        mInm_custid = pInm_custid
        mTransactiontype = pTransactiontype
        mBookingref = pBookingref
        mTicketref = pTicketref
        mTransactionnumber = pTransactionnumber
        mMachine = pMachine
        mCostcentre = pCostcentre
        mTot_cref1 = pTot_cref1
        mTot_cref2 = pTot_cref2
        mTot_pono = pTot_pono
        mBookerLocation = pBookerLocation
        mBookingAgentFirstName = pBookingAgentFirstName
        mBookingAgentLastName = pBookingAgentLastName
        mTravellerTitle = pTravellerTitle
        mTravellerFirstName = pTravellerFirstName
        mTravellerLastName = pTravellerLastName
        mDivision1 = pDivision1
        mDivision2 = pDivision2
        mDivision3 = pDivision3
        mOriginName = pOriginName
        mDestinationName = pDestinationName
        mTicketRoute = pTicketRoute
        mTicketClass = pTicketClass
        mTicketCode = pTicketCode
        mTicketName = pTicketName
        mTickettype = pTickettype
        mSinglereturn = pSinglereturn
        mJourneyleg = pJourneyleg
        mTOCName = pTOCName
        mDeparture = pDeparture
        mTransactionDate = pTransactionDate
        mLeadtime = pLeadtime
        mLowestreason = pLowestreason
        mGrossacceptedfare = pGrossacceptedfare
        mDiscount = pDiscount
        mFees = pFees
        mNet = pNet
        mLowestpossiblefare = pLowestpossiblefare
        mFullyFlexibleFare = pFullyFlexibleFare
        mSaving = pSaving
        mSavingdeclined = pSavingdeclined
        mEOEPositiveSaving = pEOEPositiveSaving
        mEOENegativeSaving = pEOENegativeSaving
        mEOEFurtherSavingsDeclined = pEOEFurtherSavingsDeclined
        mSavingsperc = pSavingsperc
        mOverspend = pOverspend
        mDistance = pDistance
        mEmissions = pEmissions
        mJourneytime = pJourneytime
        mInm_no = pInm_no
        mFulfilmentType = pFulfilmentType
        mBookingmethod = pBookingmethod
        mReasonfortravel = pReasonfortravel
        mMail = pMail
        mTransfee = pTransfee
        mPostfee = pPostfee
        mCommission = pCommission
    End Sub

    Public Sub New( _
        ByVal pBookingref As String, _
        ByVal pNumOccurrences As Integer, _
        ByVal pTravellerFirstName As String, _
        ByVal pTravellerLastName As String)
        mBookingref = pBookingref
        mNumOccurrences = pNumOccurrences
        mTravellerFirstName = pTravellerFirstName
        mTravellerLastName = pTravellerLastName
    End Sub

    Public Sub New( _
)
    End Sub

    Private mNumOccurrences As Integer
    Private mEvolvidataid As String
    Private mInm_custid As String
    Private mTransactiontype As String
    Private mBookingref As String
    Private mTicketref As String
    Private mTransactionnumber As String
    Private mMachine As String
    Private mCostcentre As String
    Private mTot_cref1 As String
    Private mTot_cref2 As String
    Private mTot_pono As String
    Private mBookerLocation As String
    Private mBookingAgentFirstName As String
    Private mBookingAgentLastName As String
    Private mTravellerTitle As String
    Private mTravellerFirstName As String
    Private mTravellerLastName As String
    Private mDivision1 As String
    Private mDivision2 As String
    Private mDivision3 As String
    Private mOriginName As String
    Private mDestinationName As String
    Private mTicketRoute As String
    Private mTicketClass As String
    Private mTicketCode As String
    Private mTicketName As String
    Private mTickettype As String
    Private mSinglereturn As String
    Private mJourneyleg As String
    Private mTOCName As String
    Private mDeparture As String
    Private mTransactionDate As String
    Private mLeadtime As String
    Private mLowestreason As String
    Private mGrossacceptedfare As String
    Private mDiscount As String
    Private mFees As String
    Private mNet As String
    Private mLowestpossiblefare As String
    Private mFullyFlexibleFare As String
    Private mSaving As String
    Private mSavingdeclined As String
    Private mEOEPositiveSaving As String
    Private mEOENegativeSaving As String
    Private mEOEFurtherSavingsDeclined As String
    Private mSavingsperc As String
    Private mOverspend As String
    Private mDistance As String
    Private mEmissions As String
    Private mJourneytime As String
    Private mInm_no As String
    Private mFulfilmentType As String
    Private mBookingmethod As String
    Private mReasonfortravel As String
    Private mMail As String
    Private mTransfee As String
    Private mPostfee As String
    Private mCommission As String

    Public Property NumOccurrences() As Integer
        Get
            Return mNumOccurrences
        End Get
        Set(ByVal value As Integer)
            mNumOccurrences = value
        End Set
    End Property

    Public Property Evolvidataid() As String
        Get
            Return mEvolvidataid
        End Get
        Set(ByVal value As String)
            mEvolvidataid = value
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

    Public Property Transactiontype() As String
        Get
            Return mTransactiontype
        End Get
        Set(ByVal value As String)
            mTransactiontype = value
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

    Public Property Ticketref() As String
        Get
            Return mTicketref
        End Get
        Set(ByVal value As String)
            mTicketref = value
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

    Public Property Machine() As String
        Get
            Return mMachine
        End Get
        Set(ByVal value As String)
            mMachine = value
        End Set
    End Property

    Public Property Costcentre() As String
        Get
            Return mCostcentre
        End Get
        Set(ByVal value As String)
            mCostcentre = value
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

    Public Property Tot_pono() As String
        Get
            Return mTot_pono
        End Get
        Set(ByVal value As String)
            mTot_pono = value
        End Set
    End Property

    Public Property BookerLocation() As String
        Get
            Return mBookerLocation
        End Get
        Set(ByVal value As String)
            mBookerLocation = value
        End Set
    End Property

    Public Property BookingAgentFirstName() As String
        Get
            Return mBookingAgentFirstName
        End Get
        Set(ByVal value As String)
            mBookingAgentFirstName = value
        End Set
    End Property

    Public Property BookingAgentLastName() As String
        Get
            Return mBookingAgentLastName
        End Get
        Set(ByVal value As String)
            mBookingAgentLastName = value
        End Set
    End Property

    Public Property TravellerTitle() As String
        Get
            Return mTravellerTitle
        End Get
        Set(ByVal value As String)
            mTravellerTitle = value
        End Set
    End Property

    Public Property TravellerFirstName() As String
        Get
            Return mTravellerFirstName
        End Get
        Set(ByVal value As String)
            mTravellerFirstName = value
        End Set
    End Property

    Public Property TravellerLastName() As String
        Get
            Return mTravellerLastName
        End Get
        Set(ByVal value As String)
            mTravellerLastName = value
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

    Public Property OriginName() As String
        Get
            Return mOriginName
        End Get
        Set(ByVal value As String)
            mOriginName = value
        End Set
    End Property

    Public Property DestinationName() As String
        Get
            Return mDestinationName
        End Get
        Set(ByVal value As String)
            mDestinationName = value
        End Set
    End Property

    Public Property TicketRoute() As String
        Get
            Return mTicketRoute
        End Get
        Set(ByVal value As String)
            mTicketRoute = value
        End Set
    End Property

    Public Property TicketClass() As String
        Get
            Return mTicketClass
        End Get
        Set(ByVal value As String)
            mTicketClass = value
        End Set
    End Property

    Public Property TicketCode() As String
        Get
            Return mTicketCode
        End Get
        Set(ByVal value As String)
            mTicketCode = value
        End Set
    End Property

    Public Property TicketName() As String
        Get
            Return mTicketName
        End Get
        Set(ByVal value As String)
            mTicketName = value
        End Set
    End Property

    Public Property Tickettype() As String
        Get
            Return mTickettype
        End Get
        Set(ByVal value As String)
            mTickettype = value
        End Set
    End Property

    Public Property Singlereturn() As String
        Get
            Return mSinglereturn
        End Get
        Set(ByVal value As String)
            mSinglereturn = value
        End Set
    End Property

    Public Property Journeyleg() As String
        Get
            Return mJourneyleg
        End Get
        Set(ByVal value As String)
            mJourneyleg = value
        End Set
    End Property

    Public Property TOCName() As String
        Get
            Return mTOCName
        End Get
        Set(ByVal value As String)
            mTOCName = value
        End Set
    End Property

    Public Property Departure() As String
        Get
            Return mDeparture
        End Get
        Set(ByVal value As String)
            mDeparture = value
        End Set
    End Property

    Public Property TransactionDate() As String
        Get
            Return mTransactionDate
        End Get
        Set(ByVal value As String)
            mTransactionDate = value
        End Set
    End Property

    Public Property Leadtime() As String
        Get
            Return mLeadtime
        End Get
        Set(ByVal value As String)
            mLeadtime = value
        End Set
    End Property

    Public Property Lowestreason() As String
        Get
            Return mLowestreason
        End Get
        Set(ByVal value As String)
            mLowestreason = value
        End Set
    End Property

    Public Property Grossacceptedfare() As String
        Get
            Return mGrossacceptedfare
        End Get
        Set(ByVal value As String)
            mGrossacceptedfare = value
        End Set
    End Property

    Public Property Discount() As String
        Get
            Return mDiscount
        End Get
        Set(ByVal value As String)
            mDiscount = value
        End Set
    End Property

    Public Property Fees() As String
        Get
            Return mFees
        End Get
        Set(ByVal value As String)
            mFees = value
        End Set
    End Property

    Public Property Net() As String
        Get
            Return mNet
        End Get
        Set(ByVal value As String)
            mNet = value
        End Set
    End Property

    Public Property Lowestpossiblefare() As String
        Get
            Return mLowestpossiblefare
        End Get
        Set(ByVal value As String)
            mLowestpossiblefare = value
        End Set
    End Property

    Public Property FullyFlexibleFare() As String
        Get
            Return mFullyFlexibleFare
        End Get
        Set(ByVal value As String)
            mFullyFlexibleFare = value
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

    Public Property Savingdeclined() As String
        Get
            Return mSavingdeclined
        End Get
        Set(ByVal value As String)
            mSavingdeclined = value
        End Set
    End Property

    Public Property EOEPositiveSaving() As String
        Get
            Return mEOEPositiveSaving
        End Get
        Set(ByVal value As String)
            mEOEPositiveSaving = value
        End Set
    End Property

    Public Property EOENegativeSaving() As String
        Get
            Return mEOENegativeSaving
        End Get
        Set(ByVal value As String)
            mEOENegativeSaving = value
        End Set
    End Property

    Public Property EOEFurtherSavingsDeclined() As String
        Get
            Return mEOEFurtherSavingsDeclined
        End Get
        Set(ByVal value As String)
            mEOEFurtherSavingsDeclined = value
        End Set
    End Property

    Public Property Savingsperc() As String
        Get
            Return mSavingsperc
        End Get
        Set(ByVal value As String)
            mSavingsperc = value
        End Set
    End Property

    Public Property Overspend() As String
        Get
            Return mOverspend
        End Get
        Set(ByVal value As String)
            mOverspend = value
        End Set
    End Property

    Public Property Distance() As String
        Get
            Return mDistance
        End Get
        Set(ByVal value As String)
            mDistance = value
        End Set
    End Property

    Public Property Emissions() As String
        Get
            Return mEmissions
        End Get
        Set(ByVal value As String)
            mEmissions = value
        End Set
    End Property

    Public Property Journeytime() As String
        Get
            Return mJourneytime
        End Get
        Set(ByVal value As String)
            mJourneytime = value
        End Set
    End Property

    Public Property Inm_no() As String
        Get
            Return mInm_no
        End Get
        Set(ByVal value As String)
            mInm_no = value
        End Set
    End Property

    Public Property FulfilmentType() As String
        Get
            Return mFulfilmentType
        End Get
        Set(ByVal value As String)
            mFulfilmentType = value
        End Set
    End Property

    Public Property Bookingmethod() As String
        Get
            Return mBookingmethod
        End Get
        Set(ByVal value As String)
            mBookingmethod = value
        End Set
    End Property

    Public Property Reasonfortravel() As String
        Get
            Return mReasonfortravel
        End Get
        Set(ByVal value As String)
            mReasonfortravel = value
        End Set
    End Property

    Public Property Mail() As String
        Get
            Return mMail
        End Get
        Set(ByVal value As String)
            mMail = value
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

    Public Property Postfee() As String
        Get
            Return mPostfee
        End Get
        Set(ByVal value As String)
            mPostfee = value
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

    Private Shared Function makeRailBuilderFromRow( _
            ByVal r As IDataReader _
        ) As RailBuilder
        Return New RailBuilder( _
                clsNYS.notString(r.Item("evolvidataid")), _
                clsNYS.notString(r.Item("inm_custid")), _
                clsNYS.notString(r.Item("transactiontype")), _
                clsNYS.notString(r.Item("bookingref")), _
                clsNYS.notString(r.Item("ticketref")), _
                clsNYS.notString(r.Item("transactionnumber")), _
                clsNYS.notString(r.Item("machine")), _
                clsNYS.notString(r.Item("costcentre")), _
                clsNYS.notString(r.Item("tot_cref1")), _
                clsNYS.notString(r.Item("tot_cref2")), _
                clsNYS.notString(r.Item("tot_pono")), _
                clsNYS.notString(r.Item("bookerLocation")), _
                clsNYS.notString(r.Item("BookingAgentFirstName")), _
                clsNYS.notString(r.Item("BookingAgentLastName")), _
                clsNYS.notString(r.Item("TravellerTitle")), _
                clsNYS.notString(r.Item("TravellerFirstName")), _
                clsNYS.notString(r.Item("TravellerLastName")), _
                clsNYS.notString(r.Item("division1")), _
                clsNYS.notString(r.Item("division2")), _
                clsNYS.notString(r.Item("division3")), _
                clsNYS.notString(r.Item("OriginName")), _
                clsNYS.notString(r.Item("DestinationName")), _
                clsNYS.notString(r.Item("TicketRoute")), _
                clsNYS.notString(r.Item("TicketClass")), _
                clsNYS.notString(r.Item("TicketCode")), _
                clsNYS.notString(r.Item("TicketName")), _
                clsNYS.notString(r.Item("tickettype")), _
                clsNYS.notString(r.Item("singlereturn")), _
                clsNYS.notString(r.Item("journeyleg")), _
                clsNYS.notString(r.Item("TOCName")), _
                clsNYS.notString(r.Item("departure")), _
                clsNYS.notString(r.Item("TransactionDate")), _
                clsNYS.notString(r.Item("leadtime")), _
                clsNYS.notString(r.Item("lowestreason")), _
                clsNYS.notString(r.Item("grossacceptedfare")), _
                clsNYS.notString(r.Item("discount")), _
                clsNYS.notString(r.Item("fees")), _
                clsNYS.notString(r.Item("net")), _
                clsNYS.notString(r.Item("lowestpossiblefare")), _
                clsNYS.notString(r.Item("FullyFlexibleFare")), _
                clsNYS.notString(r.Item("saving")), _
                clsNYS.notString(r.Item("savingdeclined")), _
                clsNYS.notString(r.Item("EOEPositiveSaving")), _
                clsNYS.notString(r.Item("EOENegativeSaving")), _
                clsNYS.notString(r.Item("EOEFurtherSavingsDeclined")), _
                clsNYS.notString(r.Item("savingsperc")), _
                clsNYS.notString(r.Item("Overspend")), _
                clsNYS.notString(r.Item("Distance")), _
                clsNYS.notString(r.Item("Emissions")), _
                clsNYS.notString(r.Item("journeytime")), _
                clsNYS.notString(r.Item("inm_no")), _
                clsNYS.notString(r.Item("FulfilmentType")), _
                clsNYS.notString(r.Item("bookingmethod")), _
                clsNYS.notString(r.Item("reasonfortravel")), _
                clsNYS.notString(r.Item("mail")), _
                clsNYS.notString(r.Item("transfee")), _
                clsNYS.notString(r.Item("postfee")), _
                clsNYS.notString(r.Item("commission")))
    End Function

    Private Shared Function makeJourneyFromRow( _
            ByVal r As IDataReader _
        ) As RailBuilder
        Return New RailBuilder( _
                clsNYS.notString(r.Item("bookingref")), _
                clsNYS.notInteger(r.Item("NumOccurrences")), _
                clsNYS.notString(r.Item("TravellerFirstName")), _
                clsNYS.notString(r.Item("TravellerLastName")))
    End Function

    Public Shared Function railSalesJourneyLegs(ByVal pstrStartDate As String, ByVal pstrEndDate As String) As List(Of RailBuilder)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of RailBuilder)()
            Using r As IDataReader = dbh.callSP("railSalesJourneyLegs", "@StartDate", pstrStartDate, "@EndDate", pstrEndDate)
                While r.Read()
                    ret.Add(makeJourneyFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function railSalesWithoutEvolviRecords(ByVal pstrStartDate As String, ByVal pstrEndDate As String, ByVal pstrClient As String, ByVal pstrClient2 As String) As List(Of RailBuilder)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of RailBuilder)()
            Using r As IDataReader = dbh.callSP("railSalesWithoutEvolviRecords", "@StartDate", pstrStartDate, "@EndDate", pstrEndDate, "@client", pstrClient, "@client2", pstrClient2)
                While r.Read()
                    ret.Add(makeRailBuilderFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function railSalesWithEvolviRecords(ByVal pstrStartDate As String, ByVal pstrEndDate As String, ByVal pstrClient As String, ByVal pstrClient2 As String) As List(Of RailBuilder)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of RailBuilder)()
            Using r As IDataReader = dbh.callSP("railSalesWithEvolviRecords", "@StartDate", pstrStartDate, "@EndDate", pstrEndDate, "@client", pstrClient, "@client2", pstrClient2)
                While r.Read()
                    ret.Add(makeRailBuilderFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function railCreditsWithEvolviRecords(ByVal pstrStartDate As String, ByVal pstrEndDate As String, ByVal pstrClient As String, ByVal pstrClient2 As String) As List(Of RailBuilder)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of RailBuilder)()
            Using r As IDataReader = dbh.callSP("railCreditsWithEvolviRecords", "@StartDate", pstrStartDate, "@EndDate", pstrEndDate, "@client", pstrClient, "@client2", pstrClient2)
                While r.Read()
                    ret.Add(makeRailBuilderFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function railCreditsWithNoRefundButSaleEvolviRecords(ByVal pstrStartDate As String, ByVal pstrEndDate As String, ByVal pstrClient As String, ByVal pstrClient2 As String) As List(Of RailBuilder)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of RailBuilder)()
            Using r As IDataReader = dbh.callSP("railCreditsWithNoRefundButSaleEvolviRecords", "@StartDate", pstrStartDate, "@EndDate", pstrEndDate, "@client", pstrClient, "@client2", pstrClient2)
                While r.Read()
                    ret.Add(makeRailBuilderFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function railCreditsWithNonIssueEvolviRecords(ByVal pstrStartDate As String, ByVal pstrEndDate As String, ByVal pstrClient As String, ByVal pstrClient2 As String) As List(Of RailBuilder)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of RailBuilder)()
            Using r As IDataReader = dbh.callSP("railCreditsWithNonIssueEvolviRecords", "@StartDate", pstrStartDate, "@EndDate", pstrEndDate, "@client", pstrClient, "@client2", pstrClient2)
                While r.Read()
                    ret.Add(makeRailBuilderFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function railCreditsWithoutEvolviRecords(ByVal pstrStartDate As String, ByVal pstrEndDate As String, ByVal pstrClient As String, ByVal pstrClient2 As String) As List(Of RailBuilder)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of RailBuilder)()
            Using r As IDataReader = dbh.callSP("railCreditsWithoutEvolviRecords", "@StartDate", pstrStartDate, "@EndDate", pstrEndDate, "@client", pstrClient, "@client2", pstrClient2)
                While r.Read()
                    ret.Add(makeRailBuilderFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function intRailSalesWithoutEvolviRecords(ByVal pstrStartDate As String, ByVal pstrEndDate As String, ByVal pstrClient As String, ByVal pstrClient2 As String) As List(Of RailBuilder)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of RailBuilder)()
            Using r As IDataReader = dbh.callSP("intRailSalesWithoutEvolviRecords", "@StartDate", pstrStartDate, "@EndDate", pstrEndDate, "@client", pstrClient, "@client2", pstrClient2)
                While r.Read()
                    ret.Add(makeRailBuilderFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function intRailCreditsWithoutEvolviRecords(ByVal pstrStartDate As String, ByVal pstrEndDate As String, ByVal pstrClient As String, ByVal pstrClient2 As String) As List(Of RailBuilder)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of RailBuilder)()
            Using r As IDataReader = dbh.callSP("intRailCreditsWithoutEvolviRecords", "@StartDate", pstrStartDate, "@EndDate", pstrEndDate, "@client", pstrClient, "@client2", pstrClient2)
                While r.Read()
                    ret.Add(makeRailBuilderFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function
End Class
