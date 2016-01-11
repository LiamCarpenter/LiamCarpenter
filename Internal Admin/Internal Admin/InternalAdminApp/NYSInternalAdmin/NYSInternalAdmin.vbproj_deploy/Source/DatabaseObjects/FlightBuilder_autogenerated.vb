Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class FlightBuilder

    Public Sub New( _
        ByVal pCustomer As String, _
        ByVal pBookeddate As String, _
        ByVal pTraveller As String, _
        ByVal pBooker As String, _
        ByVal pCostcentre As String, _
        ByVal pTot_cref1 As String, _
        ByVal pTot_cref2 As String, _
        ByVal pRef_3 As String, _
        ByVal pRef_4 As String, _
        ByVal pRef_5 As String, _
        ByVal pRef_6 As String, _
        ByVal pRef_7 As String, _
        ByVal pRef_8 As String, _
        ByVal pRef_9 As String, _
        ByVal pTot_fileno As String, _
        ByVal pTot_pono As String, _
        ByVal pDivision1 As String, _
        ByVal pDivision2 As String, _
        ByVal pDivision3 As String, _
        ByVal pDepartdate As String, _
        ByVal pSupplier As String, _
        ByVal pTravelclass As String, _
        ByVal pFare As Decimal, _
        ByVal pTaxes As Decimal, _
        ByVal pFees As Decimal, _
        ByVal pBilled As Decimal, _
        ByVal pCompfare As Decimal, _
        ByVal pSaving As Decimal, _
        ByVal pInm_savecd As String, _
        ByVal pRouting As String, _
        ByVal pFlyfrom As String, _
        ByVal pFlyto As String, _
        ByVal pIatafrom As String, _
        ByVal pIatato As String, _
        ByVal pTotalMiles As Nullable(Of Integer), _
        ByVal pCo2 As Nullable(Of Integer), _
        ByVal pTot_invno As String, _
        ByVal pTot_crsref As String, _
        ByVal pEtravel As String, _
        ByVal pSingleReturn As String, _
        ByVal pIntDom As String, _
        ByVal pTicketRef As String, _
        ByVal pFirstLegMiles As String, _
        ByVal pReasoncode As String, _
        ByVal pGroupCompany As String)
        mCustomer = pCustomer
        mBookeddate = pBookeddate
        mTraveller = pTraveller
        mBooker = pBooker
        mCostcentre = pCostcentre
        mTot_cref1 = pTot_cref1
        mTot_cref2 = pTot_cref2
        mRef_3 = pRef_3
        mRef_4 = pRef_4
        mRef_5 = pRef_5
        mRef_6 = pRef_6
        mRef_7 = pRef_7
        mRef_8 = pRef_8
        mRef_9 = pRef_9
        mTot_fileno = pTot_fileno
        mTot_pono = pTot_pono
        mDivision1 = pDivision1
        mDivision2 = pDivision2
        mDivision3 = pDivision3
        mDepartdate = pDepartdate
        mSupplier = pSupplier
        mTravelclass = pTravelclass
        mFare = pFare
        mTaxes = pTaxes
        mFees = pFees
        mBilled = pBilled
        mCompfare = pCompfare
        mSaving = pSaving
        mInm_savecd = pInm_savecd
        mRouting = pRouting
        mFlyfrom = pFlyfrom
        mFlyto = pFlyto
        mIatafrom = pIatafrom
        mIatato = pIatato
        mTotalMiles = pTotalMiles
        mCo2 = pCo2
        mTot_invno = pTot_invno
        mTot_crsref = pTot_crsref
        mEtravel = pEtravel
        mSingleReturn = pSingleReturn
        mIntDom = pIntDom
        mTicketRef = pTicketRef
        mFirstLegMiles = pFirstLegMiles
        mReasoncode = pReasoncode
        mGroupCompany = pGroupCompany
    End Sub

    Public Sub New( _
)
    End Sub

    Private mCustomer As String
    Private mBookeddate As String
    Private mTraveller As String
    Private mBooker As String
    Private mCostcentre As String
    Private mTot_cref1 As String
    Private mTot_cref2 As String
    Private mRef_3 As String
    Private mRef_4 As String
    Private mRef_5 As String
    Private mRef_6 As String
    Private mRef_7 As String
    Private mRef_8 As String
    Private mRef_9 As String
    Private mTot_fileno As String
    Private mTot_pono As String
    Private mDivision1 As String
    Private mDivision2 As String
    Private mDivision3 As String
    Private mDepartdate As String
    Private mSupplier As String
    Private mTravelclass As String
    Private mFare As Decimal
    Private mTaxes As Decimal
    Private mFees As Decimal
    Private mBilled As Decimal
    Private mCompfare As Decimal
    Private mSaving As Decimal
    Private mInm_savecd As String
    Private mRouting As String
    Private mFlyfrom As String
    Private mFlyto As String
    Private mIatafrom As String
    Private mIatato As String
    Private mTotalMiles As Nullable(Of Integer)
    Private mCo2 As Nullable(Of Integer)
    Private mTot_invno As String
    Private mTot_crsref As String
    Private mEtravel As String
    Private mSingleReturn As String
    Private mIntDom As String
    Private mTicketRef As String
    Private mFirstLegMiles As String
    Private mReasoncode As String
    Private mGroupCompany As String

    Public Property Customer() As String
        Get
            Return mCustomer
        End Get
        Set(ByVal value As String)
            mCustomer = value
        End Set
    End Property

    Public Property Bookeddate() As String
        Get
            Return mBookeddate
        End Get
        Set(ByVal value As String)
            mBookeddate = value
        End Set
    End Property

    Public Property Traveller() As String
        Get
            Return mTraveller
        End Get
        Set(ByVal value As String)
            mTraveller = value
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

    Public Property Ref_6() As String
        Get
            Return mRef_6
        End Get
        Set(ByVal value As String)
            mRef_6 = value
        End Set
    End Property

    Public Property Ref_7() As String
        Get
            Return mRef_7
        End Get
        Set(ByVal value As String)
            mRef_7 = value
        End Set
    End Property

    Public Property Ref_8() As String
        Get
            Return mRef_8
        End Get
        Set(ByVal value As String)
            mRef_8 = value
        End Set
    End Property

    Public Property Ref_9() As String
        Get
            Return mRef_9
        End Get
        Set(ByVal value As String)
            mRef_9 = value
        End Set
    End Property

    Public Property Tot_fileno() As String
        Get
            Return mTot_fileno
        End Get
        Set(ByVal value As String)
            mTot_fileno = value
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

    Public Property Departdate() As String
        Get
            Return mDepartdate
        End Get
        Set(ByVal value As String)
            mDepartdate = value
        End Set
    End Property

    Public Property Supplier() As String
        Get
            Return mSupplier
        End Get
        Set(ByVal value As String)
            mSupplier = value
        End Set
    End Property

    Public Property Travelclass() As String
        Get
            Return mTravelclass
        End Get
        Set(ByVal value As String)
            mTravelclass = value
        End Set
    End Property

    Public Property Fare() As Decimal
        Get
            Return mFare
        End Get
        Set(ByVal value As Decimal)
            mFare = value
        End Set
    End Property

    Public Property Taxes() As Decimal
        Get
            Return mTaxes
        End Get
        Set(ByVal value As Decimal)
            mTaxes = value
        End Set
    End Property

    Public Property Fees() As Decimal
        Get
            Return mFees
        End Get
        Set(ByVal value As Decimal)
            mFees = value
        End Set
    End Property

    Public Property Billed() As Decimal
        Get
            Return mBilled
        End Get
        Set(ByVal value As Decimal)
            mBilled = value
        End Set
    End Property

    Public Property Compfare() As Decimal
        Get
            Return mCompfare
        End Get
        Set(ByVal value As Decimal)
            mCompfare = value
        End Set
    End Property

    Public Property Saving() As Decimal
        Get
            Return mSaving
        End Get
        Set(ByVal value As Decimal)
            mSaving = value
        End Set
    End Property

    Public Property Inm_savecd() As String
        Get
            Return mInm_savecd
        End Get
        Set(ByVal value As String)
            mInm_savecd = value
        End Set
    End Property

    Public Property Routing() As String
        Get
            Return mRouting
        End Get
        Set(ByVal value As String)
            mRouting = value
        End Set
    End Property

    Public Property Flyfrom() As String
        Get
            Return mFlyfrom
        End Get
        Set(ByVal value As String)
            mFlyfrom = value
        End Set
    End Property

    Public Property Flyto() As String
        Get
            Return mFlyto
        End Get
        Set(ByVal value As String)
            mFlyto = value
        End Set
    End Property

    Public Property Iatafrom() As String
        Get
            Return mIatafrom
        End Get
        Set(ByVal value As String)
            mIatafrom = value
        End Set
    End Property

    Public Property Iatato() As String
        Get
            Return mIatato
        End Get
        Set(ByVal value As String)
            mIatato = value
        End Set
    End Property

    Public Property TotalMiles() As Nullable(Of Integer)
        Get
            Return mTotalMiles
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mTotalMiles = value
        End Set
    End Property

    Public Property Co2() As Nullable(Of Integer)
        Get
            Return mCo2
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mCo2 = value
        End Set
    End Property

    Public Property Tot_invno() As String
        Get
            Return mTot_invno
        End Get
        Set(ByVal value As String)
            mTot_invno = value
        End Set
    End Property

    Public Property Tot_crsref() As String
        Get
            Return mTot_crsref
        End Get
        Set(ByVal value As String)
            mTot_crsref = value
        End Set
    End Property

    Public Property Etravel() As String
        Get
            Return mEtravel
        End Get
        Set(ByVal value As String)
            mEtravel = value
        End Set
    End Property

    Public Property SingleReturn() As String
        Get
            Return mSingleReturn
        End Get
        Set(ByVal value As String)
            mSingleReturn = value
        End Set
    End Property

    Public Property IntDom() As String
        Get
            Return mIntDom
        End Get
        Set(ByVal value As String)
            mIntDom = value
        End Set
    End Property

    Public Property TicketRef() As String
        Get
            Return mTicketRef
        End Get
        Set(ByVal value As String)
            mTicketRef = value
        End Set
    End Property

    Public Property FirstLegMiles() As String
        Get
            Return mFirstLegMiles
        End Get
        Set(ByVal value As String)
            mFirstLegMiles = value
        End Set
    End Property

    Public Property Reasoncode() As String
        Get
            Return mReasoncode
        End Get
        Set(ByVal value As String)
            mReasoncode = value
        End Set
    End Property

    Public Property GroupCompany() As String
        Get
            Return mGroupCompany
        End Get
        Set(ByVal value As String)
            mGroupCompany = value
        End Set
    End Property

    Private Shared Function makeFlightBuilderFromRow( _
            ByVal r As IDataReader _
        ) As FlightBuilder
        Return New FlightBuilder( _
               clsNYS.notString(r.Item("customer")), _
                clsNYS.notString(r.Item("bookeddate")), _
                clsNYS.notString(r.Item("traveller")), _
                clsNYS.notString(r.Item("booker")), _
                clsNYS.notString(r.Item("costcentre")), _
                clsNYS.notString(r.Item("tot_cref1")), _
                clsNYS.notString(r.Item("tot_cref2")), _
                clsNYS.notString(r.Item("ref_3")), _
                clsNYS.notString(r.Item("ref_4")), _
                clsNYS.notString(r.Item("ref_5")), _
                clsNYS.notString(r.Item("ref_6")), _
                clsNYS.notString(r.Item("ref_7")), _
                clsNYS.notString(r.Item("ref_8")), _
                clsNYS.notString(r.Item("ref_9")), _
                clsNYS.notString(r.Item("tot_fileno")), _
                clsNYS.notString(r.Item("tot_pono")), _
                clsNYS.notString(r.Item("division1")), _
                clsNYS.notString(r.Item("division2")), _
                clsNYS.notString(r.Item("division3")), _
                clsNYS.notString(r.Item("departdate")), _
                clsNYS.notString(r.Item("supplier")), _
                clsNYS.notString(r.Item("travelclass")), _
                clsNYS.notDecimal(r.Item("fare")), _
                clsNYS.notDecimal(r.Item("taxes")), _
                clsNYS.notDecimal(r.Item("fees")), _
                clsNYS.notDecimal(r.Item("billed")), _
                clsNYS.notDecimal(r.Item("compfare")), _
                clsNYS.notDecimal(r.Item("saving")), _
                clsNYS.notString(r.Item("inm_savecd")), _
                clsNYS.notString(r.Item("routing")), _
                clsNYS.notString(r.Item("flyfrom")), _
                clsNYS.notString(r.Item("flyto")), _
                clsNYS.notString(r.Item("iatafrom")), _
                clsNYS.notString(r.Item("iatato")), _
                clsNYS.notInteger(r.Item("totalMiles")), _
                clsNYS.notInteger(r.Item("co2")), _
                clsNYS.notString(r.Item("tot_invno")), _
                clsNYS.notString(r.Item("tot_crsref")), _
                clsNYS.notString(r.Item("etravel")), _
                clsNYS.notString(r.Item("singleReturn")), _
                clsNYS.notString(r.Item("IntDom")), _
                clsNYS.notString(r.Item("TicketRef")), _
                clsNYS.notString(r.Item("firstLegMiles")), _
                clsNYS.notString(r.Item("reasoncode")), _
                clsNYS.notString(r.Item("GroupCompany")))
    End Function

    Public Shared Function flightDataByTicket(ByVal pstartdate As String, ByVal penddate As String) As List(Of FlightBuilder)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of FlightBuilder)()
            Using r As IDataReader = dbh.callSP("flightDataByTicket", "@startdate", pstartdate, _
                                                                        "@enddate", penddate, _
                                                                        "@client", "", _
                                                                        "@client2", "")
                While r.Read()
                    ret.Add(makeFlightBuilderFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

End Class
