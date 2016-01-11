Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class Highway

    Public Sub New( _
        ByVal pInvoicenumber As String, _
        ByVal pInvoiceamount As String, _
        ByVal pInvoicedate As String, _
        ByVal pLinedescription As String, _
        ByVal pCostcentre As String, _
        ByVal pFirstName As String, _
        ByVal pLastName As String, _
        ByVal pTraveller As String, _
        ByVal pProduct As String, _
        ByVal pProductdesc As String, _
        ByVal pProductType As String, _
        ByVal pDeparturedate As String, _
        ByVal pArrivaldate As String, _
        ByVal pOriginal As String, _
        ByVal pRailticketref As String, _
        ByVal pAirticketref As String, _
        ByVal pHotelticketref As String, _
        ByVal pRaildistance As String, _
        ByVal pRailtransactionref As String, _
        ByVal pAirtransactionref As String, _
        ByVal pHoteltransactionref As String, _
        ByVal pBossOrigin As String, _
        ByVal pBossDestination As String, _
        ByVal pEvolviOrigin As String, _
        ByVal pEvolviDestination As String, _
        ByVal pVenueName As String, _
        ByVal pPo As String)
        mInvoicenumber = pInvoicenumber
        mInvoiceamount = pInvoiceamount
        mInvoicedate = pInvoicedate
        mLinedescription = pLinedescription
        mCostcentre = pCostcentre
        mFirstName = pFirstName
        mLastName = pLastName
        mTraveller = pTraveller
        mProduct = pProduct
        mProductdesc = pProductdesc
        mProductType = pProductType
        mDeparturedate = pDeparturedate
        mArrivaldate = pArrivaldate
        mOriginal = pOriginal
        mRailticketref = pRailticketref
        mAirticketref = pAirticketref
        mHotelticketref = pHotelticketref
        mRaildistance = pRaildistance
        mRailtransactionref = pRailtransactionref
        mAirtransactionref = pAirtransactionref
        mHoteltransactionref = pHoteltransactionref
        mBossOrigin = pBossOrigin
        mBossDestination = pBossDestination
        mEvolviOrigin = pEvolviOrigin
        mEvolviDestination = pEvolviDestination
        mVenueName = pVenueName
        mPo = pPo
    End Sub

    Public Sub New( _
)
    End Sub

    Private mInvoicenumber As String
    Private mInvoiceamount As String
    Private mInvoicedate As String
    Private mLinedescription As String
    Private mCostcentre As String
    Private mFirstName As String
    Private mLastName As String
    Private mTraveller As String
    Private mProduct As String
    Private mProductdesc As String
    Private mProductType As String
    Private mDeparturedate As String
    Private mArrivaldate As String
    Private mOriginal As String
    Private mRailticketref As String
    Private mAirticketref As String
    Private mHotelticketref As String
    Private mRaildistance As String
    Private mRailtransactionref As String
    Private mAirtransactionref As String
    Private mHoteltransactionref As String
    Private mBossOrigin As String
    Private mBossDestination As String
    Private mEvolviOrigin As String
    Private mEvolviDestination As String
    Private mVenueName As String
    Private mPo As String

    Public Property Po() As String
        Get
            Return mPo
        End Get
        Set(ByVal value As String)
            mPo = value
        End Set
    End Property

    Public Property Invoicenumber() As String
        Get
            Return mInvoicenumber
        End Get
        Set(ByVal value As String)
            mInvoicenumber = value
        End Set
    End Property

    Public Property Invoiceamount() As String
        Get
            Return mInvoiceamount
        End Get
        Set(ByVal value As String)
            mInvoiceamount = value
        End Set
    End Property

    Public Property Invoicedate() As String
        Get
            Return mInvoicedate
        End Get
        Set(ByVal value As String)
            mInvoicedate = value
        End Set
    End Property

    Public Property Linedescription() As String
        Get
            Return mLinedescription
        End Get
        Set(ByVal value As String)
            mLinedescription = value
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

    Public Property FirstName() As String
        Get
            Return mFirstName
        End Get
        Set(ByVal value As String)
            mFirstName = value
        End Set
    End Property

    Public Property LastName() As String
        Get
            Return mLastName
        End Get
        Set(ByVal value As String)
            mLastName = value
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

    Public Property Product() As String
        Get
            Return mProduct
        End Get
        Set(ByVal value As String)
            mProduct = value
        End Set
    End Property

    Public Property Productdesc() As String
        Get
            Return mProductdesc
        End Get
        Set(ByVal value As String)
            mProductdesc = value
        End Set
    End Property

    Public Property ProductType() As String
        Get
            Return mProductType
        End Get
        Set(ByVal value As String)
            mProductType = value
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

    Public Property Arrivaldate() As String
        Get
            Return mArrivaldate
        End Get
        Set(ByVal value As String)
            mArrivaldate = value
        End Set
    End Property

    Public Property Original() As String
        Get
            Return mOriginal
        End Get
        Set(ByVal value As String)
            mOriginal = value
        End Set
    End Property

    Public Property Railticketref() As String
        Get
            Return mRailticketref
        End Get
        Set(ByVal value As String)
            mRailticketref = value
        End Set
    End Property

    Public Property Airticketref() As String
        Get
            Return mAirticketref
        End Get
        Set(ByVal value As String)
            mAirticketref = value
        End Set
    End Property

    Public Property Hotelticketref() As String
        Get
            Return mHotelticketref
        End Get
        Set(ByVal value As String)
            mHotelticketref = value
        End Set
    End Property

    Public Property Raildistance() As String
        Get
            Return mRaildistance
        End Get
        Set(ByVal value As String)
            mRaildistance = value
        End Set
    End Property

    Public Property Railtransactionref() As String
        Get
            Return mRailtransactionref
        End Get
        Set(ByVal value As String)
            mRailtransactionref = value
        End Set
    End Property

    Public Property Airtransactionref() As String
        Get
            Return mAirtransactionref
        End Get
        Set(ByVal value As String)
            mAirtransactionref = value
        End Set
    End Property

    Public Property Hoteltransactionref() As String
        Get
            Return mHoteltransactionref
        End Get
        Set(ByVal value As String)
            mHoteltransactionref = value
        End Set
    End Property

    Public Property BossOrigin() As String
        Get
            Return mBossOrigin
        End Get
        Set(ByVal value As String)
            mBossOrigin = value
        End Set
    End Property

    Public Property BossDestination() As String
        Get
            Return mBossDestination
        End Get
        Set(ByVal value As String)
            mBossDestination = value
        End Set
    End Property

    Public Property EvolviOrigin() As String
        Get
            Return mEvolviOrigin
        End Get
        Set(ByVal value As String)
            mEvolviOrigin = value
        End Set
    End Property

    Public Property EvolviDestination() As String
        Get
            Return mEvolviDestination
        End Get
        Set(ByVal value As String)
            mEvolviDestination = value
        End Set
    End Property

    Public Property VenueName() As String
        Get
            Return mVenueName
        End Get
        Set(ByVal value As String)
            mVenueName = value
        End Set
    End Property

    Private Shared Function makeHighwayFromRow( _
            ByVal r As IDataReader _
        ) As Highway
        Return New Highway( _
                clsNYS.notString(r.Item("invoicenumber")), _
                clsNYS.notString(r.Item("invoiceamount")), _
                clsNYS.notString(r.Item("invoicedate")), _
                clsNYS.notString(r.Item("linedescription")), _
                clsNYS.notString(r.Item("costcentre")), _
                clsNYS.notString(r.Item("firstname")), _
                clsNYS.notString(r.Item("lastname")), _
                clsNYS.notString(r.Item("traveller")), _
                clsNYS.notString(r.Item("product")), _
                clsNYS.notString(r.Item("productdesc")), _
                clsNYS.notString(r.Item("producttype")), _
                clsNYS.notString(r.Item("departuredate")), _
                clsNYS.notString(r.Item("arrivaldate")), _
                clsNYS.notString(r.Item("original")), _
                clsNYS.notString(r.Item("railticketref")), _
                clsNYS.notString(r.Item("airticketref")), _
                clsNYS.notString(r.Item("hotelticketref")), _
                clsNYS.notString(r.Item("raildistance")), _
                clsNYS.notString(r.Item("railtransactionref")), _
                clsNYS.notString(r.Item("airtransactionref")), _
                clsNYS.notString(r.Item("hoteltransactionref")), _
                clsNYS.notString(r.Item("bossorigin")), _
                clsNYS.notString(r.Item("bossdestination")), _
                clsNYS.notString(r.Item("evolviorigin")), _
                clsNYS.notString(r.Item("evolvidestination")), _
                clsNYS.notString(r.Item("venuename")), _
                clsNYS.notString(r.Item("po")))
    End Function

    Public Shared Function list(ByVal pstartdate As String, ByVal penddate As String) As List(Of Highway)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of Highway)()
            Using r As IDataReader = dbh.callSP("FeederFile_Highways", _
                                                "@startdate", pstartdate, _
                                                "@enddate", penddate)
                While r.Read()
                    ret.Add(makeHighwayFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

End Class
