Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils


''' <summary>
'''  Class FeedImportData - Accesses database to add/edit/list records in FeedAmendment table
''' </summary>
''' <remarks>Created 17/03/2009 Nick Massarella</remarks>
Partial Public Class FeedImportData



    'R15 CR
    Public Sub New(ByVal pAICol6 As String)
        mAICol6 = pAICol6
    End Sub
    Public Sub New( _
        ByVal pDataid As Integer, _
        ByVal pTransactionnumber As Nullable(Of Integer), _
        ByVal pTransactionlinenumber As Nullable(Of Long), _
        ByVal pTransactiondate As Nullable(Of DateTime), _
        ByVal pArrivaldate As Nullable(Of DateTime), _
        ByVal pDeparturedate As Nullable(Of DateTime), _
        ByVal pPassengername As String, _
        ByVal pRef1 As String, _
        ByVal pRef2 As String, _
        ByVal pRef3 As String, _
        ByVal pDept As String, _
        ByVal pBooker As String, _
        ByVal pBookerinitials As String, _
        ByVal pNettamount As Nullable(Of Double), _
        ByVal pVatamount As Nullable(Of Double), _
        ByVal pVatrate As Nullable(Of Double), _
        ByVal pTotalamount As Nullable(Of Double), _
        ByVal pVenuename As String, _
        ByVal pVenuedetails As String, _
        ByVal pRoomdetails As String, _
        ByVal pGroupid As Nullable(Of Integer), _
        ByVal pGroupName As String, _
        ByVal pConfermainvoicenumber As Nullable(Of Integer), _
        ByVal pCategoryid As Nullable(Of Integer), _
        ByVal pCategoryname As String, _
        ByVal pCategorybosscode As String, _
        ByVal pSupplierinvoice As String, _
        ByVal pGuestPNR As String, _
        ByVal pCostcode As String, _
        ByVal pParameterid As Nullable(Of Integer), _
        ByVal pVenuereference As Nullable(Of Integer), _
        ByVal pVenuebosscode As String, _
        ByVal pVenueEX As Nullable(Of Double), _
        ByVal pVenueDD As Nullable(Of Double), _
        ByVal pStatusid As Nullable(Of Integer), _
        ByVal pTransactionvaluenew As String, _
        ByVal pParameterstart As Nullable(Of DateTime), _
        ByVal pParameterend As Nullable(Of DateTime), _
        ByVal pTransactionid As Nullable(Of Integer), _
        ByVal pTransactioncode As String, _
        ByVal pTransactionvalue As Nullable(Of Double), _
        ByVal pStatusname As String, _
        ByVal pInvoiceid As Integer, _
        ByVal pInvoiceDate As String, _
        ByVal pFailreason As String, _
        ByVal pActualcomm As Integer, _
        ByVal pBookedDate As Nullable(Of DateTime), _
        ByVal pSendGross As Nullable(Of Boolean), _
        ByVal pTransactionExVat As Nullable(Of Boolean), _
        ByVal pAICol6 As String, _
        ByVal pAICol7 As String, _
        ByVal pAICol8 As String, _
        ByVal pAICol9 As String, _
        ByVal pAICol10 As String, _
        ByVal pCurrency As String, _
        ByVal pCancellation As Boolean, _
        ByVal pExportExclude As Boolean, _
        ByVal pOutofPolicyReason As String, _
        ByVal pLast4Digits As String, _
        ByVal pTravellerEmail As String, _
        ByVal pBookerEmail As String)
        mDataid = pDataid
        mTransactionnumber = pTransactionnumber
        mTransactionlinenumber = pTransactionlinenumber
        mTransactiondate = pTransactiondate
        mArrivaldate = pArrivaldate
        mDeparturedate = pDeparturedate
        mPassengername = pPassengername
        mRef1 = pRef1
        mRef2 = pRef2
        mRef3 = pRef3
        mDept = pDept
        mBooker = pBooker
        mBookerinitials = pBookerinitials
        mNettamount = pNettamount
        mVatamount = pVatamount
        mVatrate = pVatrate
        mTotalamount = pTotalamount
        mVenuename = pVenuename
        mVenuedetails = pVenuedetails
        mRoomdetails = pRoomdetails
        mGroupid = pGroupid
        mGroupName = pGroupName
        mConfermainvoicenumber = pConfermainvoicenumber
        mCategoryid = pCategoryid
        mCategoryname = pCategoryname
        mCategorybosscode = pCategorybosscode
        mSupplierinvoice = pSupplierinvoice
        mGuestPNR = pGuestPNR
        mCostcode = pCostcode
        mParameterid = pParameterid
        mVenuereference = pVenuereference
        mVenuebosscode = pVenuebosscode
        mVenueEX = pVenueEX
        mVenueDD = pVenueDD
        mStatusid = pStatusid
        mTransactionvaluenew = pTransactionvaluenew
        mParameterstart = pParameterstart
        mParameterend = pParameterend
        mTransactionid = pTransactionid
        mTransactioncode = pTransactioncode
        mTransactionvalue = pTransactionvalue
        mStatusname = pStatusname
        mInvoiceid = pInvoiceid
        mInvoiceDate = pInvoiceDate
        mFailreason = pFailreason
        mActualcomm = pActualcomm
        mBookedDate = pBookedDate
        mSendGross = pSendGross
        mTransactionExVat = pTransactionExVat
        mAICol6 = pAICol6
        mAICol7 = pAICol7
        mAICol8 = pAICol8
        mAICol9 = pAICol9
        mAICol10 = pAICol10
        mCurrency = pCurrency
        mCancellation = pCancellation
        mExportExclude = pExportExclude
        mOutofPolicyReason = pOutofPolicyReason
        mLast4Digits = pLast4Digits
        mTravellerEmail = pTravellerEmail
        'R2.20C CR
        mBookerEmail = pBookerEmail


    End Sub

    'R2.12 CR - added travelleremail
    'R2.3 NM
    Public Sub New( _
        ByVal pDataid As Integer, _
        ByVal pTransactionnumber As Nullable(Of Integer), _
        ByVal pTransactionlinenumber As Nullable(Of Long), _
        ByVal pTransactiondate As Nullable(Of DateTime), _
        ByVal pArrivaldate As Nullable(Of DateTime), _
        ByVal pDeparturedate As Nullable(Of DateTime), _
        ByVal pPassengername As String, _
        ByVal pRef1 As String, _
        ByVal pRef2 As String, _
        ByVal pRef3 As String, _
        ByVal pDept As String, _
        ByVal pBooker As String, _
        ByVal pBookerinitials As String, _
        ByVal pNettamount As Nullable(Of Double), _
        ByVal pVatamount As Nullable(Of Double), _
        ByVal pVatrate As Nullable(Of Double), _
        ByVal pTotalamount As Nullable(Of Double), _
        ByVal pVenuename As String, _
        ByVal pVenuedetails As String, _
        ByVal pRoomdetails As String, _
        ByVal pGroupid As Nullable(Of Integer), _
        ByVal pGroupName As String, _
        ByVal pConfermainvoicenumber As Nullable(Of Integer), _
        ByVal pCategoryid As Nullable(Of Integer), _
        ByVal pCategoryname As String, _
        ByVal pCategorybosscode As String, _
        ByVal pSupplierinvoice As String, _
        ByVal pGuestPNR As String, _
        ByVal pCostcode As String, _
        ByVal pParameterid As Nullable(Of Integer), _
        ByVal pVenuereference As Nullable(Of Integer), _
        ByVal pVenuebosscode As String, _
        ByVal pVenueEX As Nullable(Of Double), _
        ByVal pVenueDD As Nullable(Of Double), _
        ByVal pStatusid As Nullable(Of Integer), _
        ByVal pTransactionvaluenew As String, _
        ByVal pParameterstart As Nullable(Of DateTime), _
        ByVal pParameterend As Nullable(Of DateTime), _
        ByVal pTransactionid As Nullable(Of Integer), _
        ByVal pTransactioncode As String, _
        ByVal pTransactionvalue As Nullable(Of Double), _
        ByVal pStatusname As String, _
        ByVal pInvoiceid As Integer, _
        ByVal pInvoiceDate As String, _
        ByVal pFailreason As String, _
        ByVal pActualcomm As Integer, _
        ByVal pBookedDate As Nullable(Of DateTime), _
        ByVal pSendGross As Nullable(Of Boolean), _
        ByVal pTransactionExVat As Nullable(Of Boolean), _
        ByVal pAICol6 As String, _
        ByVal pAICol7 As String, _
        ByVal pAICol8 As String, _
        ByVal pAICol9 As String, _
        ByVal pAICol10 As String, _
        ByVal pCurrency As String, _
        ByVal pCancellation As Boolean, _
        ByVal pExportExclude As Boolean, _
        ByVal pccVal As String, _
        ByVal ppoVal As String, _
        ByVal pCREF1Val As String, _
        ByVal pCREF2Val As String, _
        ByVal pCREF3Val As String, _
        ByVal pcomplaintsText As String, _
        ByVal pCREF4Val As String, _
        ByVal pCREF5Val As String, _
        ByVal pCREF6Val As String, _
        ByVal pCREF7Val As String, _
        ByVal pCREF8Val As String, _
        ByVal pCREF9Val As String, _
        ByVal pinvoicefee As Decimal, _
        ByVal pTransactionType As String, _
        ByVal pTravellerEmail As String, _
        ByVal pBookerEmail As String, _
        ByVal pTTransactionType As String)

        mDataid = pDataid
        mTransactionnumber = pTransactionnumber
        mTransactionlinenumber = pTransactionlinenumber
        mTransactiondate = pTransactiondate
        mArrivaldate = pArrivaldate
        mDeparturedate = pDeparturedate
        mPassengername = pPassengername
        mRef1 = pRef1
        mRef2 = pRef2
        mRef3 = pRef3
        mDept = pDept
        mBooker = pBooker
        mBookerinitials = pBookerinitials
        mNettamount = pNettamount
        mVatamount = pVatamount
        mVatrate = pVatrate
        mTotalamount = pTotalamount
        mVenuename = pVenuename
        mVenuedetails = pVenuedetails
        mRoomdetails = pRoomdetails
        mGroupid = pGroupid
        mGroupName = pGroupName
        mConfermainvoicenumber = pConfermainvoicenumber
        mCategoryid = pCategoryid
        mCategoryname = pCategoryname
        mCategorybosscode = pCategorybosscode
        mSupplierinvoice = pSupplierinvoice
        mGuestPNR = pGuestPNR
        mCostcode = pCostcode
        mParameterid = pParameterid
        mVenuereference = pVenuereference
        mVenuebosscode = pVenuebosscode
        mVenueEX = pVenueEX
        mVenueDD = pVenueDD
        mStatusid = pStatusid
        mTransactionvaluenew = pTransactionvaluenew
        mParameterstart = pParameterstart
        mParameterend = pParameterend
        mTransactionid = pTransactionid
        mTransactioncode = pTransactioncode
        mTransactionvalue = pTransactionvalue
        mStatusname = pStatusname
        mInvoiceid = pInvoiceid
        mInvoiceDate = pInvoiceDate
        mFailreason = pFailreason
        mActualcomm = pActualcomm
        mBookedDate = pBookedDate
        mSendGross = pSendGross
        mTransactionExVat = pTransactionExVat
        mAICol6 = pAICol6
        mAICol7 = pAICol7
        mAICol8 = pAICol8
        mAICol9 = pAICol9
        mAICol10 = pAICol10
        mCurrency = pCurrency
        mCancellation = pCancellation
        'R17 CR
        mExportExclude = pExportExclude
        mccVal = pccVal
        mpoVal = ppoVal
        mCREF1Val = pCREF1Val
        mCREF2Val = pCREF2Val
        mCREF3Val = pCREF3Val
        mcomplaintsText = pcomplaintsText
        mCREF4Val = pCREF4Val
        mCREF5Val = pCREF5Val
        mCREF6Val = pCREF6Val
        mCREF7Val = pCREF7Val
        mCREF8Val = pCREF8Val
        mCREF9Val = pCREF9Val
        minvoicefee = pinvoicefee
        mTransactionType = pTransactionType
        'R2.12 CR
        mTravellerEmail = pTravellerEmail
        'R2.20C CR
        mBookerEmail = pBookerEmail

        'R2.21.1 SA 
        mTTransactionType = pTTransactionType
    End Sub

    ''' <summary>
    ''' Instantiate class
    ''' </summary>
    ''' <remarks>Created 17/03/2009 Nick Massarella</remarks>
    Public Sub New( _
)
    End Sub

    ''' <summary>
    ''' Instantiate class with one set parameter
    ''' </summary>
    ''' <param name="pTransactionnumber"></param>
    ''' <remarks>Created 17/03/2009 Nick Massarella</remarks>
    Public Sub New(ByVal pTransactionnumber As Nullable(Of Integer))
        mTransactionnumber = pTransactionnumber
    End Sub

    Public Sub New(ByVal pTransactionnumber As Nullable(Of Integer), ByVal oDataid As Nullable(Of Integer))
        mTransactionnumber = pTransactionnumber
        mDataid_null = oDataid
    End Sub

    Public Sub New(ByVal pTransactionnumber As Nullable(Of Integer), _
                   ByVal pCostcode As String, _
                   ByVal pExpression As String, _
                   ByVal pPo As String)
        mTransactionnumber = pTransactionnumber
        mCostcode = pCostcode
        mExpression = pExpression
        mPo = pPo
    End Sub

    'R2.20C CR
    Private mBookerEmail As String

    Private mTransactionType As String 'from client option table
    Private mTravellerEmail As String
    Private mcomplaintsText As String
    Private mOutofPolicyReason As String
    Private mLast4Digits As String
    Private mPo As String
    Private mCancellation As Boolean
    Private mCurrency As String
    Private mAICol6 As String
    Private mAICol7 As String
    Private mAICol8 As String
    Private mAICol9 As String
    Private mAICol10 As String
    Private mExpression As String
    Private mDataid As Integer
    Private mDataid_null As Nullable(Of Integer)
    Private mTransactionnumber As Nullable(Of Integer)
    Private mTransactionlinenumber As Nullable(Of Long)
    'Private mTransactionlinenumber As Nullable(Of Integer)
    Private mTransactiondate As Nullable(Of DateTime)
    Private mArrivaldate As Nullable(Of DateTime)
    Private mDeparturedate As Nullable(Of DateTime)
    Private mPassengername As String
    Private mRef1 As String
    Private mRef2 As String
    Private mRef3 As String
    Private mDept As String
    Private mBooker As String
    Private mBookerinitials As String
    Private mNettamount As Nullable(Of Double)
    Private mVatamount As Nullable(Of Double)
    Private mVatrate As Nullable(Of Double)
    Private mTotalamount As Nullable(Of Double)
    Private mVenuename As String
    Private mVenuedetails As String
    Private mRoomdetails As String
    Private mGroupid As Nullable(Of Integer)
    Private mGroupName As String
    Private mConfermainvoicenumber As Nullable(Of Integer)
    Private mCategoryid As Nullable(Of Integer)
    Private mCategoryname As String
    Private mCategorybosscode As String
    Private mSupplierinvoice As String
    Private mGuestPNR As String
    Private mCostcode As String
    Private mParameterid As Nullable(Of Integer)
    Private mVenuereference As Nullable(Of Integer)
    Private mVenuebosscode As String
    Private mVenueEX As Nullable(Of Double)
    Private mVenueDD As Nullable(Of Double)
    Private mStatusid As Nullable(Of Integer)
    Private mTransactionvaluenew As String
    Private mParameterstart As Nullable(Of DateTime)
    Private mParameterend As Nullable(Of DateTime)
    Private mTransactionid As Nullable(Of Integer)
    Private mTransactioncode As String
    Private mTransactionvalue As Nullable(Of Double)
    Private mStatusname As String
    Private mInvoiceid As Integer
    Private mInvoiceDate As String
    Private mFailreason As String
    Private mActualcomm As Integer
    Private mBookedDate As Nullable(Of DateTime)
    Private mSendGross As Nullable(Of Boolean)
    Private minvoicefee As Decimal
    Private mTransactionExVat As Nullable(Of Boolean)

    'R17 CR
    Private mExportExclude As Nullable(Of Boolean)
    Private mccVal As String
    Private mpoVal As String
    Private mCREF1Val As String
    Private mCREF2Val As String
    Private mCREF3Val As String
    Private mCREF4Val As String
    Private mCREF5Val As String
    Private mCREF6Val As String
    Private mCREF7Val As String
    Private mCREF8Val As String
    Private mCREF9Val As String


    'R2.21.1 SA - transaction type from transaction table
    Private mTTransactionType As String

    'R2.20C CR
    Public Property BookerEmail() As String
        Get
            Return mBookerEmail
        End Get
        Set(value As String)
            mBookerEmail = value
        End Set
    End Property

    Public Property TransactionType() As String
        Get
            Return mTransactionType
        End Get
        Set(ByVal value As String)
            mTransactionType = value
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

    Public Property complaintsText() As String
        Get
            Return mcomplaintsText
        End Get
        Set(ByVal value As String)
            mcomplaintsText = value
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

    Public Property ccVal() As String
        Get
            Return mccVal
        End Get
        Set(ByVal value As String)
            mccVal = value
        End Set
    End Property

    Public Property poVal() As String
        Get
            Return mpoVal
        End Get
        Set(ByVal value As String)
            mpoVal = value
        End Set
    End Property

    Public Property CREF1Val() As String
        Get
            Return mCREF1Val
        End Get
        Set(ByVal value As String)
            mCREF1Val = value
        End Set
    End Property

    Public Property CREF2Val() As String
        Get
            Return mCREF2Val
        End Get
        Set(ByVal value As String)
            mCREF2Val = value
        End Set
    End Property

    Public Property CREF3Val() As String
        Get
            Return mCREF3Val
        End Get
        Set(ByVal value As String)
            mCREF3Val = value
        End Set
    End Property

    Public Property CREF4Val() As String
        Get
            Return mCREF4Val
        End Get
        Set(ByVal value As String)
            mCREF4Val = value
        End Set
    End Property

    Public Property CREF5Val() As String
        Get
            Return mCREF5Val
        End Get
        Set(ByVal value As String)
            mCREF5Val = value
        End Set
    End Property

    Public Property CREF6Val() As String
        Get
            Return mCREF6Val
        End Get
        Set(ByVal value As String)
            mCREF6Val = value
        End Set
    End Property

    Public Property CREF7Val() As String
        Get
            Return mCREF7Val
        End Get
        Set(ByVal value As String)
            mCREF7Val = value
        End Set
    End Property

    Public Property CREF8Val() As String
        Get
            Return mCREF8Val
        End Get
        Set(ByVal value As String)
            mCREF8Val = value
        End Set
    End Property

    Public Property CREF9Val() As String
        Get
            Return mCREF9Val
        End Get
        Set(ByVal value As String)
            mCREF9Val = value
        End Set
    End Property

    Public Property Cancellation() As Boolean
        Get
            Return mCancellation
        End Get
        Set(ByVal value As Boolean)
            mCancellation = value
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

    Public Property Po() As String
        Get
            Return mPo
        End Get
        Set(ByVal value As String)
            mPo = value
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

    Public Property Expression() As String
        Get
            Return mExpression
        End Get
        Set(ByVal value As String)
            mExpression = value
        End Set
    End Property

    Public Property Actualcomm() As Integer
        Get
            Return mActualcomm
        End Get
        Set(ByVal value As Integer)
            mActualcomm = value
        End Set
    End Property

    Public Property Failreason() As String
        Get
            Return mFailreason
        End Get
        Set(ByVal value As String)
            mFailreason = value
        End Set
    End Property

    Public Property Invoiceid() As Integer
        Get
            Return mInvoiceid
        End Get
        Set(ByVal value As Integer)
            mInvoiceid = value
        End Set
    End Property

    Public Property InvoiceDate() As String
        Get
            Return mInvoiceDate
        End Get
        Set(ByVal value As String)
            mInvoiceDate = value
        End Set
    End Property

    Public Property Dataid() As Integer
        Get
            Return mDataid
        End Get
        Set(ByVal value As Integer)
            mDataid = value
        End Set
    End Property

    Public Property Dataid_null() As Nullable(Of Integer)
        Get
            Return mDataid_null
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mDataid_null = value
        End Set
    End Property

    Public Property Transactionnumber() As Nullable(Of Integer)
        Get
            Return mTransactionnumber
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mTransactionnumber = value
        End Set
    End Property

    'R2.9 SA 
    Public Property Transactionlinenumber() As Nullable(Of Long)
        Get
            Return mTransactionlinenumber
        End Get
        Set(ByVal value As Nullable(Of Long))
            mTransactionlinenumber = value
        End Set
    End Property

    Public Property BookedDate() As Nullable(Of DateTime)
        Get
            Return mBookedDate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mBookedDate = value
        End Set
    End Property

    Public Property Transactiondate() As Nullable(Of DateTime)
        Get
            Return mTransactiondate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mTransactiondate = value
        End Set
    End Property

    Public Property Arrivaldate() As Nullable(Of DateTime)
        Get
            Return mArrivaldate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mArrivaldate = value
        End Set
    End Property

    Public Property Departuredate() As Nullable(Of DateTime)
        Get
            Return mDeparturedate
        End Get
        Set(ByVal value As Nullable(Of DateTime))
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

    Public Property Dept() As String
        Get
            Return mDept
        End Get
        Set(ByVal value As String)
            mDept = value
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

    Public Property Bookerinitials() As String
        Get
            Return mBookerinitials
        End Get
        Set(ByVal value As String)
            mBookerinitials = value
        End Set
    End Property

    Public Property Nettamount() As Nullable(Of Double)
        Get
            Return mNettamount
        End Get
        Set(ByVal value As Nullable(Of Double))
            mNettamount = value
        End Set
    End Property

    Public Property Vatamount() As Nullable(Of Double)
        Get
            Return mVatamount
        End Get
        Set(ByVal value As Nullable(Of Double))
            mVatamount = value
        End Set
    End Property

    Public Property Vatrate() As Nullable(Of Double)
        Get
            Return mVatrate
        End Get
        Set(ByVal value As Nullable(Of Double))
            mVatrate = value
        End Set
    End Property

    Public Property Totalamount() As Nullable(Of Double)
        Get
            Return mTotalamount
        End Get
        Set(ByVal value As Nullable(Of Double))
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

    Public Property Groupid() As Nullable(Of Integer)
        Get
            Return mGroupid
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mGroupid = value
        End Set
    End Property

    Public Property GroupName() As String
        Get
            Return mGroupName
        End Get
        Set(ByVal value As String)
            mGroupName = value
        End Set
    End Property

    Public Property Confermainvoicenumber() As Nullable(Of Integer)
        Get
            Return mConfermainvoicenumber
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mConfermainvoicenumber = value
        End Set
    End Property

    Public Property Categoryid() As Nullable(Of Integer)
        Get
            Return mCategoryid
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mCategoryid = value
        End Set
    End Property

    Public Property Categoryname() As String
        Get
            Return mCategoryname
        End Get
        Set(ByVal value As String)
            mCategoryname = value
        End Set
    End Property

    Public Property Categorybosscode() As String
        Get
            Return mCategorybosscode
        End Get
        Set(ByVal value As String)
            mCategorybosscode = value
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

    Public Property Parameterid() As Nullable(Of Integer)
        Get
            Return mParameterid
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mParameterid = value
        End Set
    End Property

    Public Property Venuereference() As Nullable(Of Integer)
        Get
            Return mVenuereference
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mVenuereference = value
        End Set
    End Property

    Public Property Venuebosscode() As String
        Get
            Return mVenuebosscode
        End Get
        Set(ByVal value As String)
            mVenuebosscode = value
        End Set
    End Property

    Public Property VenueEX() As Nullable(Of Double)
        Get
            Return mVenueEX
        End Get
        Set(ByVal value As Nullable(Of Double))
            mVenueEX = value
        End Set
    End Property

    Public Property VenueDD() As Nullable(Of Double)
        Get
            Return mVenueDD
        End Get
        Set(ByVal value As Nullable(Of Double))
            mVenueDD = value
        End Set
    End Property

    Public Property Statusid() As Nullable(Of Integer)
        Get
            Return mStatusid
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mStatusid = value
        End Set
    End Property

    Public Property Transactionvaluenew() As String
        Get
            Return mTransactionvaluenew
        End Get
        Set(ByVal value As String)
            mTransactionvaluenew = value
        End Set
    End Property

    Public Property Parameterstart() As Nullable(Of DateTime)
        Get
            Return mParameterstart
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mParameterstart = value
        End Set
    End Property

    Public Property Parameterend() As Nullable(Of DateTime)
        Get
            Return mParameterend
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            mParameterend = value
        End Set
    End Property

    Public Property Transactionid() As Nullable(Of Integer)
        Get
            Return mTransactionid
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mTransactionid = value
        End Set
    End Property

    Public Property Transactioncode() As String
        Get
            Return mTransactioncode
        End Get
        Set(ByVal value As String)
            mTransactioncode = value
        End Set
    End Property

    Public Property Statusname() As String
        Get
            Return mStatusname
        End Get
        Set(ByVal value As String)
            mStatusname = value
        End Set
    End Property

    Public Property Transactionvalue() As Nullable(Of Double)
        Get
            Return mTransactionvalue
        End Get
        Set(ByVal value As Nullable(Of Double))
            mTransactionvalue = value
        End Set
    End Property

    Public Property SendGross() As Nullable(Of Boolean)
        Get
            Return mSendGross
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mSendGross = value
        End Set
    End Property

    Public Property invoicefee() As Decimal
        Get
            Return minvoicefee
        End Get
        Set(ByVal value As Decimal)
            minvoicefee = value
        End Set
    End Property

    Public Property TransactionExVat() As Nullable(Of Boolean)
        Get
            Return mTransactionExVat
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mTransactionExVat = value
        End Set
    End Property

    'CR
    Public Property ExcludeFromExport() As Nullable(Of Boolean)
        Get
            Return mExportExclude
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mExportExclude = value
        End Set
    End Property

    'R2.21.1 SA 
    Public Property TTransactionType() As String
        Get
            Return mTTransactionType
        End Get
        Set(ByVal value As String)
            mTTransactionType = value
        End Set
    End Property

    ''' <summary>
    ''' Function makeFeedDataFromRowTrans - creates a collection of FeedImportData records
    ''' </summary>
    ''' <param name="r"></param>
    ''' <returns>A FeedImportData record</returns>
    ''' <remarks>Created 17/03/2009 Nick Massarella</remarks>
    Private Shared Function makeFeedDataFromRowTrans( _
            ByVal r As IDataReader _
        ) As FeedImportData
        Return New FeedImportData(toNullableInteger(r.Item("transactionnumber")))

    End Function
    Private Shared Function makeFeedDataFrom2RowTrans( _
        ByVal r As IDataReader _
    ) As FeedImportData
        Return New FeedImportData(toNullableInteger(r.Item("transactionnumber")), toNullableInteger(r.Item("dataid")))

    End Function
    Private Shared Function makeFeedDataFromRowPocode( _
        ByVal r As IDataReader) As FeedImportData
        Return New FeedImportData(clsStuff.notString(r.Item(0)))
    End Function

    Private Shared Function makeFeedDataFromRowCC( _
            ByVal r As IDataReader _
        ) As FeedImportData
        Return New FeedImportData( _
                toNullableInteger(r.Item("transactionnumber")), _
                clsStuff.notString(r.Item("costcode")), _
                clsStuff.notString(r.Item("expression")), _
                clsStuff.notString(r.Item("po")))
    End Function

    Private Shared Function makeFeedImportDataFromRowNew( _
            ByVal r As IDataReader _
        ) As FeedImportData
        Return New FeedImportData( _
                clsStuff.notWholeNumber(r.Item("dataid")), _
                toNullableInteger(r.Item("transactionnumber")), _
                toNullableLong(r.Item("transactionlinenumber")), _
                toNullableDate(r.Item("transactiondate")), _
                toNullableDate(r.Item("arrivaldate")), _
                toNullableDate(r.Item("departuredate")), _
                clsStuff.notString(r.Item("passengername")), _
                clsStuff.notString(r.Item("ref1")), _
                clsStuff.notString(r.Item("ref2")), _
                clsStuff.notString(r.Item("ref3")), _
                clsStuff.notString(r.Item("dept")), _
                clsStuff.notString(r.Item("booker")), _
                clsStuff.notString(r.Item("bookerinitials")), _
                toNullableFloat(r.Item("nettamount")), _
                toNullableFloat(r.Item("vatamount")), _
                toNullableFloat(r.Item("vatrate")), _
                toNullableFloat(r.Item("totalamount")), _
                clsStuff.notString(r.Item("venuename")), _
                clsStuff.notString(r.Item("venuedetails")), _
                clsStuff.notString(r.Item("roomdetails")), _
                toNullableInteger(r.Item("groupid")), _
                clsStuff.notString(r.Item("groupname")), _
                toNullableInteger(r.Item("confermainvoicenumber")), _
                toNullableInteger(r.Item("categoryid")), _
                clsStuff.notString(r.Item("categoryname")), _
                clsStuff.notString(r.Item("categorybosscode")), _
                clsStuff.notString(r.Item("supplierinvoice")), _
                clsStuff.notString(r.Item("guestPNR")), _
                clsStuff.notString(r.Item("costcode")), _
                toNullableInteger(r.Item("parameterid")), _
                toNullableInteger(r.Item("venuereference")), _
                clsStuff.notString(r.Item("venuebosscode")), _
                toNullableFloat(r.Item("venueEX")), _
                toNullableFloat(r.Item("venueDD")), _
                toNullableInteger(r.Item("statusid")), _
                clsStuff.notString(r.Item("transactionvaluenew")), _
                toNullableDate(r.Item("parameterstart")), _
                toNullableDate(r.Item("parameterend")), _
                clsStuff.notWholeNumber(r.Item("parameterend")), _
                clsStuff.notString(r.Item("transactioncode")), _
                toNullableFloat(r.Item("transactionvalue")), _
                clsStuff.notString(r.Item("statusname")), _
                clsStuff.notWholeNumber(r.Item("invoiceid")), _
                clsStuff.notString(r.Item("invoiceexportdate")), _
                clsStuff.notString(r.Item("failreason")), _
                clsStuff.notWholeNumber(r.Item("actualcomm")), _
                toNullableDate(r.Item("bookeddate")), _
                toNullableBoolean(r.Item("SendGross")), _
                toNullableBoolean(r.Item("TransactionExVat")), _
                clsStuff.notString(r.Item("AICol6")), _
                clsStuff.notString(r.Item("AICol7")), _
                clsStuff.notString(r.Item("AICol8")), _
                clsStuff.notString(r.Item("AICol9")), _
                clsStuff.notString(r.Item("AICol10")), _
                clsStuff.notString(r.Item("Currency")), _
                clsStuff.notBoolean(r.Item("Cancellation")), _
                clsStuff.notBoolean(r.Item("ExcludeFromExport")), _
                clsStuff.notString(r.Item("OutofPolicyReason")), _
                clsStuff.notString(r.Item("Last4Digits")), _
                clsStuff.notString(r.Item("TravellerEmail")), _
                clsStuff.notString(r.Item("BookerEmail")))
    End Function

    'R2.12 CR - added travelleremail
    Private Shared Function makeExportFromRowNew( _
            ByVal r As IDataReader _
        ) As FeedImportData
        Return New FeedImportData( _
                clsStuff.notWholeNumber(r.Item("dataid")), _
                toNullableInteger(r.Item("transactionnumber")), _
                toNullableLong(r.Item("transactionlinenumber")), _
                toNullableDate(r.Item("transactiondate")), _
                toNullableDate(r.Item("arrivaldate")), _
                toNullableDate(r.Item("departuredate")), _
                clsStuff.notString(r.Item("passengername")), _
                clsStuff.notString(r.Item("ref1")), _
                clsStuff.notString(r.Item("ref2")), _
                clsStuff.notString(r.Item("ref3")), _
                clsStuff.notString(r.Item("dept")), _
                clsStuff.notString(r.Item("booker")), _
                clsStuff.notString(r.Item("bookerinitials")), _
                toNullableFloat(r.Item("nettamount")), _
                toNullableFloat(r.Item("vatamount")), _
                toNullableFloat(r.Item("vatrate")), _
                toNullableFloat(r.Item("totalamount")), _
                clsStuff.notString(r.Item("venuename")), _
                clsStuff.notString(r.Item("venuedetails")), _
                clsStuff.notString(r.Item("roomdetails")), _
                toNullableInteger(r.Item("groupid")), _
                clsStuff.notString(r.Item("groupname")), _
                toNullableInteger(r.Item("confermainvoicenumber")), _
                toNullableInteger(r.Item("categoryid")), _
                clsStuff.notString(r.Item("categoryname")), _
                clsStuff.notString(r.Item("categorybosscode")), _
                clsStuff.notString(r.Item("supplierinvoice")), _
                clsStuff.notString(r.Item("guestPNR")), _
                clsStuff.notString(r.Item("costcode")), _
                toNullableInteger(r.Item("parameterid")), _
                toNullableInteger(r.Item("venuereference")), _
                clsStuff.notString(r.Item("venuebosscode")), _
                toNullableFloat(r.Item("venueEX")), _
                toNullableFloat(r.Item("venueDD")), _
                toNullableInteger(r.Item("statusid")), _
                clsStuff.notString(r.Item("transactionvaluenew")), _
                toNullableDate(r.Item("parameterstart")), _
                toNullableDate(r.Item("parameterend")), _
                clsStuff.notWholeNumber(r.Item("parameterend")), _
                clsStuff.notString(r.Item("transactioncode")), _
                toNullableFloat(r.Item("transactionvalue")), _
                clsStuff.notString(r.Item("statusname")), _
                clsStuff.notWholeNumber(r.Item("invoiceid")), _
                clsStuff.notString(r.Item("invoiceexportdate")), _
                clsStuff.notString(r.Item("failreason")), _
                clsStuff.notWholeNumber(r.Item("actualcomm")), _
                toNullableDate(r.Item("bookeddate")), _
                toNullableBoolean(r.Item("SendGross")), _
                toNullableBoolean(r.Item("TransactionExVat")), _
                clsStuff.notString(r.Item("AICol6")), _
                clsStuff.notString(r.Item("AICol7")), _
                clsStuff.notString(r.Item("AICol8")), _
                clsStuff.notString(r.Item("AICol9")), _
                clsStuff.notString(r.Item("AICol10")), _
                clsStuff.notString(r.Item("Currency")), _
                clsStuff.notBoolean(r.Item("Cancellation")), _
                clsStuff.notBoolean(r.Item("ExcludeFromExport")), _
                clsStuff.notString(r.Item("ccVal")), _
                clsStuff.notString(r.Item("poVal")), _
                clsStuff.notString(r.Item("CREF1Val")), _
                clsStuff.notString(r.Item("CREF2Val")), _
                clsStuff.notString(r.Item("CREF3Val")), _
                clsStuff.notString(r.Item("complaintsText")), _
                clsStuff.notString(r.Item("CREF4Val")), _
                clsStuff.notString(r.Item("CREF5Val")), _
                clsStuff.notString(r.Item("CREF6Val")), _
                clsStuff.notString(r.Item("CREF7Val")), _
                clsStuff.notString(r.Item("CREF8Val")), _
                clsStuff.notString(r.Item("CREF9Val")), _
                clsStuff.notDecimal(r.Item("invoicefee")), _
                clsStuff.notString(r.Item("TransactionType")), _
                clsStuff.notString(r.Item("TravellerEmail")), _
                clsStuff.notString(r.Item("BookerEmail")), _
                clsStuff.notString(r.Item("TTransactionType")))
    End Function

    ''' <summary>
    ''' Function [get] - retrieves a FeedImportData record by its unique ID
    ''' </summary>
    ''' <param name="pDataid"></param>
    ''' <returns>A FeedImportData record</returns>
    ''' <remarks>Created 17/03/2009 Nick Massarella</remarks>
    Public Shared Function getForEdit( _
            ByVal pDataid As Integer _
        ) As FeedImportData
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New FeedImportData
            Using r As IDataReader = dbh.callSP("FeedImportData_get", "@dataid", pDataid)
                If r.Read() Then
                    ret = makeExportFromRowNew(r)
                End If
                Return ret
            End Using
        End Using
    End Function

    Public Shared Function CostCodeCheck( _
                ByVal pintgroupid As Integer, _
                    ByVal pintstatus As Integer, _
                    ByVal pinteoe As Integer) As List(Of FeedImportData)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of FeedImportData)()
            Using r As IDataReader = dbh.callSP("FeedData_CostCodeCheck", _
                                                "@groupid", pintgroupid, _
                                                "@statusid", pintstatus, _
                                                "@eoe", pinteoe)
                While r.Read()
                    ret.Add(makeFeedDataFromRowCC(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    ''' <summary>
    ''' Function list - retrieves a list FeedImportData record by passed in values
    ''' </summary>
    ''' <param name="pintgroupid"></param>
    ''' <param name="pstrvenuename"></param>
    ''' <param name="pstrinvoiceno"></param>
    ''' <param name="pstrguestpnr"></param>
    ''' <param name="pstrguestname"></param>
    ''' <param name="pintstatus"></param>
    ''' <returns>A list of FeedImportData records</returns>
    ''' <remarks>Created 17/03/2009 Nick Massarella</remarks>
    Public Shared Function list(ByVal pintgroupid As Integer, ByVal pstrvenuename As String, _
                                ByVal pstrinvoiceno As String, ByVal pstrguestpnr As String, _
                                ByVal pstrguestname As String, ByVal pintstatus As Integer) As List(Of FeedImportData)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of FeedImportData)()
            Using r As IDataReader = dbh.callSP("FeedImportData_list", "@groupid", pintgroupid, _
                                                "@venuename", pstrvenuename, _
                                                "@invoiceno", pstrinvoiceno, _
                                                "@guestpnr", pstrguestpnr, _
                                                "@guestname", pstrguestname, _
                                                "@statusid", pintstatus)
                While r.Read()
                    ret.Add(makeFeedImportDataFromRowNew(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    ''' <summary>
    ''' Function save - saves FeedImportData record
    ''' </summary>
    ''' <returns>Saved FeedImportData record ID</returns>
    ''' <remarks>Created 17/03/2009 Nick Massarella</remarks>
    Public Function saveImport() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection) 'connectionString="server=nys-sql2012\nys;Initial Catalog=CUBIT;User ID=Cubit;Password=H@ndym@n"

            Dim tst As String

            Try
                mDataid = clsStuff.notWholeNumber(dbh.callSPSingleValue("FeedImportData_save", _
                                                     "@Dataid", mDataid, _
                                                     "@Transactionnumber", mTransactionnumber, _
                                                     "@Transactionlinenumber", mTransactionlinenumber, _
                                                     "@Transactiondate", mTransactiondate, _
                                                     "@Arrivaldate", mArrivaldate, _
                                                     "@Departuredate", mDeparturedate, _
                                                     "@Passengername", mPassengername, _
                                                     "@Ref1", mRef1, _
                                                     "@Ref2", mRef2, _
                                                     "@Ref3", mRef3, _
                                                     "@Dept", mDept, _
                                                     "@Booker", mBooker, _
                                                     "@Bookerinitials", mBookerinitials, _
                                                     "@Nettamount", mNettamount, _
                                                     "@Vatamount", mVatamount, _
                                                     "@Vatrate", mVatrate, _
                                                     "@Totalamount", mTotalamount, _
                                                     "@Venuename", mVenuename, _
                                                     "@Venuedetails", mVenuedetails, _
                                                     "@Roomdetails", mRoomdetails, _
                                                     "@Groupid", mGroupid, _
                                                     "@GroupName", mGroupName, _
                                                     "@Confermainvoicenumber", mConfermainvoicenumber, _
                                                     "@Categoryid", mCategoryid, _
                                                     "@Supplierinvoice", mSupplierinvoice, _
                                                     "@GuestPNR", mGuestPNR, _
                                                     "@Costcode", mCostcode, _
                                                     "@Parameterid", mParameterid, _
                                                     "@Venuereference", mVenuereference, _
                                                     "@Venuebosscode", mVenuebosscode, _
                                                     "@VenueEX", mVenueEX, _
                                                     "@VenueDD", mVenueDD, _
                                                     "@Statusid", mStatusid, _
                                                     "@Transactionvaluenew", mTransactionvaluenew, _
                                                     "@BookedDate", mBookedDate, _
                                                     "@AICol6", mAICol6, _
                                                     "@AICol7", mAICol7, _
                                                     "@AICol8", mAICol8, _
                                                     "@AICol9", mAICol9, _
                                                     "@AICol10", mAICol10, _
                                                     "@Currency", mCurrency, _
                                                     "@Cancellation", mCancellation, _
                                                     "@OutofPolicyReason", mOutofPolicyReason, _
                                                     "@Last4Digits", mLast4Digits, _
                                                     "@TravellerEmail", mTravellerEmail, _
                                                     "@BookerEmail", mBookerEmail))
            Catch ex As Exception

            End Try


            Return mDataid
        End Using
    End Function

    ''' <summary>
    ''' Sub delete - deletes FeedImportData record by its unique ID
    ''' </summary>
    ''' <param name="pBookingIDs"></param>
    ''' <remarks>Created 17/03/2009 Nick Massarella</remarks>
    Public Shared Sub delete(ByVal pBookingIDs As String)
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("FeedImportData_delete", "@Bookingids", pBookingIDs)
        End Using
    End Sub

    Public Shared Sub deleteAll()
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("FeedImportData_deleteAll")
        End Using
    End Sub

    ''' <summary>
    ''' Function savevalue - saves updated by user transactionvalue 
    ''' </summary>
    ''' <param name="pintuserid"></param>
    ''' <param name="pintdataid"></param>
    ''' <param name="pstrvalue"></param>
    ''' <param name="pstrname"></param>
    ''' <param name="pintreference"></param>
    ''' <param name="pstrbosscode"></param>
    ''' <param name="pdblcommissionEX"></param>
    ''' <param name="pdblcommissionDD"></param>
    ''' <param name="pstrcostcode"></param>
    ''' <returns>The updated FeedImportData record unique ID</returns>
    ''' <remarks>
    ''' Created 17/03/2009 Nick Massarella
    ''' R15 CR - added optional pstrpocode
    ''' </remarks> 
    Public Shared Function savevalue(ByVal pintuserid As Integer, ByVal pintdataid As Integer, ByVal pstrvalue As String, ByVal pstrname As String, _
                          ByVal pintreference As Integer, ByVal pstrbosscode As String, ByVal pdblcommissionEX As Double, ByVal pdblcommissionDD As Double, _
                          ByVal pstrcostcode As String, ByVal pstraicol6 As String, ByVal pstraicol7 As String, ByVal pstraicol8 As String, _
                          ByVal pstraicol9 As String, ByVal pstraicol10 As String, ByVal pstrTravellerEmail As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)

            Dim intDataid As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValue("FeedData_savevalue", _
                                                 "@userid", pintuserid, _
                                                 "@Dataid", pintdataid, _
                                                 "@transactionvaluenew", pstrvalue, _
                                                 "@venuename", pstrname, _
                                                 "@venuereference", pintreference, _
                                                 "@venuebosscode", pstrbosscode, _
                                                 "@venueEX", pdblcommissionEX, _
                                                 "@venueDD", pdblcommissionDD, _
                                                 "@costcode", pstrcostcode, _
                                                 "@aicol6", pstraicol6, _
                                                 "@aicol7", pstraicol7, _
                                                 "@aicol8", pstraicol8, _
                                                 "@aicol9", pstraicol9, _
                                                 "@aicol10", pstraicol10, _
                                                 "@travellerEmail", pstrTravellerEmail))
            Return intDataid
        End Using
    End Function

    'R16 NM
    Public Shared Function saveAllRelated(ByVal pintdataid As Integer, ByVal pintPO As Integer) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intDataid As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing("FeedData_saveAllRelated", _
                                                 "@Dataid", pintdataid, _
                                                 "@savepo", pintPO))
            Return intDataid
        End Using
    End Function

    ''' <summary>
    ''' Function saveFail - adds the failure text to the failed record
    ''' </summary>
    ''' <param name="pinttransactionnumber"></param>
    ''' <param name="pstrfailreason"></param>
    ''' <returns>The updated FeedImportData record unique ID</returns>
    ''' <remarks>Created 17/03/2009 Nick Massarella</remarks>
    Public Shared Function saveFail(ByVal pinttransactionnumber As Integer, ByVal pstrfailreason As String) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intDataid As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValue("FeedImportData_failsave", _
                                                 "@transactionnumber", pinttransactionnumber, _
                                                 "@failreason", pstrfailreason))
            Return intDataid
        End Using
    End Function

    ''' <summary>
    ''' Function saveStatus - updates a FeedImportData record's status ID
    ''' </summary>
    ''' <param name="pintdataid"></param>
    ''' <param name="pintstatusid"></param>
    ''' <returns>The updated FeedImportData record unique ID</returns>
    ''' <remarks>Created 17/03/2009 Nick Massarella</remarks>
    Public Shared Function saveStatus(ByVal pintdataid As Integer, ByVal pintstatusid As Integer) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intDataid As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValue("FeedImportData_statussave", _
                                                 "@Dataid", pintdataid, _
                                                 "@statusid", pintstatusid))
            Return intDataid
        End Using
    End Function

    ''' <summary>
    ''' Function SetBookingIDtoExportStatus - updates FeedImportData records status ID grouped by transaction number
    ''' </summary>
    ''' <param name="pinttransactionnumber"></param>
    ''' <param name="pintstatusid"></param>
    ''' <returns>The updated FeedImportData transaction number</returns>
    ''' <remarks>Created 17/03/2009 Nick Massarella
    ''' </remarks>
    Public Shared Function SetBookingIDtoExportStatus(ByVal pinttransactionnumber As Integer, _
                                               ByVal pintstatusid As Integer) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intDataid As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValue("SetBookingIDtoExportStatusAM", _
                                                 "@transactionnumber", pinttransactionnumber, _
                                                 "@statusid", pintstatusid))
            Return intDataid
        End Using
    End Function

    ''' <summary>
    ''' Function exportList - retrieves a list of FeedImportData records at a set status
    ''' </summary>
    ''' <param name="pintgroupid"></param>
    ''' <param name="pintstatus"></param>
    ''' <returns>A list of distinct transaction numbers</returns>
    ''' <remarks>Created 17/03/2009 Nick Massarella</remarks>
    Public Shared Function exportList(ByVal pintgroupid As Integer, _
                                    ByVal pintstatus As Integer) As List(Of FeedImportData)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of FeedImportData)()
            Using r As IDataReader = dbh.callSP("FeedData_exportlistAM", "@groupid", pintgroupid, _
                                                "@statusid", pintstatus)
                While r.Read()
                    ret.Add(makeFeedDataFrom2RowTrans(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    ''' <summary>
    ''' Function exportCheck - check that FeedImportData records grouped together 
    ''' by transaction number are at same status
    ''' </summary>
    ''' <param name="pinttransactionnumber"></param>
    ''' <param name="pintstatus"></param>
    ''' <returns>FeedImportData record ID if exists</returns>
    ''' <remarks>Created 17/03/2009 Nick Massarella</remarks>
    Public Shared Function exportCheck(ByVal pinttransactionnumber As Integer, _
                                    ByVal pintstatus As Integer) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intid As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing _
                                                ("FeedData_exportcheck", _
                                                "@transactionnumber", pinttransactionnumber, _
                                                "@statusid", pintstatus))
            Return intid
        End Using
    End Function

    Public Shared Function exportCheckExtra(ByVal pinttransactionnumber As Integer) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intid As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing _
                                                        ("FeedData_exportCheckExtra", _
                                                        "@transactionnumber", pinttransactionnumber))
            Return intid
        End Using
    End Function

    ''' <summary>
    ''' Function exportListBossCodeCheck - checks FeedImportData records ready for export have a 
    ''' valid Bosscode
    ''' </summary>
    ''' <param name="pintgroupid"></param>
    ''' <param name="pintstatus"></param>
    ''' <returns>All records where the Bosscode is invalid</returns>
    ''' <remarks>Created 17/03/2009 Nick Massarella</remarks>
    Public Shared Function exportListBossCodeCheck(ByVal pintgroupid As Integer, _
                                    ByVal pintstatus As Integer) As List(Of FeedImportData)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of FeedImportData)()
            Using r As IDataReader = dbh.callSP("FeedData_exportlistBossCodeCheck", _
                                                "@groupid", pintgroupid, _
                                                "@statusid", pintstatus)
                While r.Read()
                    ret.Add(makeFeedDataFromRowTrans(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    'R?? SA - BUG FIX!!
    'Public Shared Function ExportIsAllNegative(ByVal pinttransactionnumber As Integer) As Boolean
    '    Using dbh As New SqlDatabaseHandle(getConnection)
    '        Dim ret As Boolean = False

    '        ret = clsStuff.notBoolean(dbh.callSPSingleValueCanReturnNothing _
    '                                            ("FeedData_ExportIsAllNegative", _
    '                                            "@transactionnumber", pinttransactionnumber))

    '        Return ret
    '    End Using
    'End Function

    Public Shared Function exportByBookingIDList(ByVal pinttransactionnumber As Integer, _
                                                 ByVal pintstatusid As Integer) _
                                        As List(Of FeedImportData)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of FeedImportData)()
            Using r As IDataReader = dbh.callSP("FeedData_exportByBookingID", _
                                                "@transactionnumber", pinttransactionnumber, _
                                                "@statusid", pintstatusid)
                While r.Read()
                    ret.Add(makeExportFromRowNew(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    'R15 CR
    Public Shared Function poCodeFind(ByVal pstrpocode As String, ByVal pstrguestname As String, ByVal pstrgroupname As String) As List(Of FeedImportData)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of FeedImportData)()
            Using r As IDataReader = dbh.callSP("FeedData_getPoList", _
                                                "@pocode", pstrpocode, _
                                                "@guestname", pstrguestname, _
                                                "@groupname", pstrgroupname)
                While r.Read()
                    ret.Add(makeFeedDataFromRowPocode(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function saveExcludeFromExport(ByVal pstrTransactionNumber As String, _
                                                 ByVal pblnExcludeFromExport As Boolean) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As Integer = 0

            ret = clsStuff.notWholeNumber(dbh.callSPSingleValueCanReturnNothing _
                                                ("FeedImportData_saveExcludeFromExportAM", _
                                                "@transactionnumber", CInt(pstrTransactionNumber), _
                                                "@excludefromexport", pblnExcludeFromExport))

            Return ret
        End Using
    End Function

    'R2.7 NM
    Public Shared Function exportValueGet(ByVal pinttransactionnumber As Integer) As Decimal
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim dRet As Decimal = clsStuff.notDecimal(dbh.callSPSingleValueCanReturnNothing("exportValue_get", _
                                                                    "@transactionnumber", pinttransactionnumber))
            Return dRet
        End Using
    End Function

    'R2.15 NM
    Public Shared Function isLateRooms(ByVal pinttransactionnumber As Integer) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim strRet As String = clsStuff.notString(dbh.callSPSingleValueCanReturnNothing("isLateRooms", _
                                                                    "@transactionnumber", pinttransactionnumber))
            Return strRet
        End Using
    End Function

    'R2.18 CR
    Public Shared Function getPreviousBOSSNumber(ByVal pstrBookingRef As String) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            'R2.21c SA- modified SP name! 'getPreviousInvoiceNumber
            Dim strRet As String = clsStuff.notString(dbh.callSPSingleValueCanReturnNothing("getPreviousBOSSNumber", _
                                                                                            "@bookingRef", pstrBookingRef))
            Return strRet
        End Using
    End Function

    'R2.21 CR
    ''' <summary>
    ''' Gets the column value for the specified column name, for the record selected
    ''' </summary>
    ''' <param name="pintRecordDataID"></param>
    ''' <param name="pstrColumnName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getBookingColumnValue(pintRecordDataID As Integer, pstrColumnName As String) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As String = clsStuff.notString(dbh.callSPSingleValueCanReturnNothing("FeedImportData_getBookingColumnValue", _
                                                                                            "@DataID", pintRecordDataID, _
                                                                                            "@ColumnName", pstrColumnName))

            Return ret
        End Using
    End Function

    'R2.21.4 SA 
    Public Shared Function deleteSingle(ByVal pintDataID As Integer) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As Integer = CInt(dbh.callSPSingleValueCanReturnNothing("FeedImportData_deleteSingle", _
                                                                                            "@DataID", pintDataID))
            Return ret
        End Using
    End Function

    'R2.9 SA 
    Public Shared Function getOverallTransactionValue(ByVal pintTransactionnumber As Integer) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As Integer = CInt(dbh.callSPSingleValueCanReturnNothing("FeedImportData_getOverallTransactionValue", _
                                                                            "@Transactionnumber", pintTransactionnumber))
            Return ret
        End Using
    End Function

End Class
