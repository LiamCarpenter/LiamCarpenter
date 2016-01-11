Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

''' <summary>
''' 
''' </summary>
''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
Partial Public Class FeedInvoice

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pInvoiceid"></param>
    ''' <param name="ptransactionnumber"></param>
    ''' <param name="pSystemnysuserid"></param>
    ''' <param name="pInvoiceexportednett"></param>
    ''' <param name="pInvoiceexportedvat"></param>
    ''' <param name="pInvoiceexportedgross"></param>
    ''' <param name="pInvoiceexport"></param>
    ''' <param name="pInvoiceexportdate"></param>
    ''' <param name="ptransactionfee"></param>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Sub New( _
        ByVal pInvoiceid As Integer, _
        ByVal ptransactionnumber As Integer, _
        ByVal pSystemnysuserid As Nullable(Of Integer), _
        ByVal pInvoiceexportednett As String, _
        ByVal pInvoiceexportedvat As String, _
        ByVal pInvoiceexportedgross As String, _
        ByVal pInvoiceexport As Nullable(Of Boolean), _
        ByVal pInvoiceexportdate As DateTime, _
        ByVal ptransactionfee As String)
        mInvoiceid = pInvoiceid
        mtransactionnumber = ptransactionnumber
        mSystemnysuserid = pSystemnysuserid
        mInvoiceexportednett = pInvoiceexportednett
        mInvoiceexportedvat = pInvoiceexportedvat
        mInvoiceexportedgross = pInvoiceexportedgross
        mInvoiceexport = pInvoiceexport
        mInvoiceexportdate = pInvoiceexportdate
        mtransactionfee = ptransactionfee
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ptransactionnumber"></param>
    ''' <param name="pdataitemnett"></param>
    ''' <param name="pdataitemvat"></param>
    ''' <param name="pdataitemgross"></param>
    ''' <param name="ptransactionfee"></param>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Sub New( _
        ByVal ptransactionnumber As Integer, _
        ByVal pdataitemnett As String, _
        ByVal pdataitemvat As String, _
        ByVal pdataitemgross As String, _
        ByVal ptransactionfee As String)
        mtransactionnumber = ptransactionnumber
        mdataitemnett = pdataitemnett
        mdataitemvat = pdataitemvat
        mdataitemgross = pdataitemgross
        mtransactionfee = ptransactionfee
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pdataid"></param>
    ''' <param name="pcategoryname"></param>
    ''' <param name="pdataitemnett"></param>
    ''' <param name="pdataitemvat"></param>
    ''' <param name="pdataitemgross"></param>
    ''' <param name="ptransactionfee"></param>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Sub New( _
        ByVal pdataid As Integer, _
        ByVal pcategoryname As String, _
        ByVal pdataitemnett As String, _
        ByVal pdataitemvat As String, _
        ByVal pdataitemgross As String, _
        ByVal ptransactionfee As String)
        mdataid = pdataid
        mcategoryname = pcategoryname
        mdataitemnett = pdataitemnett
        mdataitemvat = pdataitemvat
        mdataitemgross = pdataitemgross
        mtransactionfee = ptransactionfee
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pbossid"></param>
    ''' <param name="pbosstotal"></param>
    ''' <param name="pbosscomNett"></param>
    ''' <param name="pbosscomVat"></param>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Sub New( _
        ByVal pbossid As String, _
        ByVal pbosstotal As Double, _
        ByVal pbosscomNett As Double, _
        ByVal pbosscomVat As Double)
        mbossid = pbossid
        mbosstotal = pbosstotal
        mbosscomNett = pbosscomNett
        mbosscomVat = pbosscomVat
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pdirectorate"></param>
    ''' <param name="pdepartment"></param>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Sub New( _
        ByVal pdirectorate As String, _
        ByVal pdepartment As String)
        mdirectorate = pdirectorate
        mdepartment = pdepartment
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pCustomerCode"></param>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Sub New( _
        ByVal pCustomerCode As String)
        mCustomerCode = pCustomerCode
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Sub New( _
)
    End Sub

    Private mdataid As Integer
    Private mbossid As String
    Private mbosstotal As Double
    Private mbosscomNett As Double
    Private mbosscomVat As Double
    Private mInvoiceid As Integer
    Private mtransactionnumber As Integer
    Private mSystemnysuserid As Nullable(Of Integer)
    Private mInvoiceexportednett As String
    Private mInvoiceexportedvat As String
    Private mInvoiceexportedgross As String
    Private mdataitemnett As String
    Private mdataitemvat As String
    Private mdataitemgross As String
    Private mtransactionfee As String
    Private mInvoiceexport As Nullable(Of Boolean)
    Private mInvoiceexportdate As DateTime
    Private mdirectorate As String
    Private mdepartment As String
    Private mCustomerCode As String
    Private mcategoryname As String

    Public Property categoryname() As String
        Get
            Return mcategoryname
        End Get
        Set(ByVal value As String)
            mcategoryname = value
        End Set
    End Property

    Public Property bosscomVat() As Double
        Get
            Return mbosscomVat
        End Get
        Set(ByVal value As Double)
            mbosscomVat = value
        End Set
    End Property

    Public Property bosscomNett() As Double
        Get
            Return mbosscomNett
        End Get
        Set(ByVal value As Double)
            mbosscomNett = value
        End Set
    End Property

    Public Property bosstotal() As Double
        Get
            Return mbosstotal
        End Get
        Set(ByVal value As Double)
            mbosstotal = value
        End Set
    End Property

    Public Property bossid() As String
        Get
            Return mbossid
        End Get
        Set(ByVal value As String)
            mbossid = value
        End Set
    End Property

    Public Property CustomerCode() As String
        Get
            Return mCustomerCode
        End Get
        Set(ByVal value As String)
            mCustomerCode = value
        End Set
    End Property

    Public Property department() As String
        Get
            Return mdepartment
        End Get
        Set(ByVal value As String)
            mdepartment = value
        End Set
    End Property

    Public Property directorate() As String
        Get
            Return mdirectorate
        End Get
        Set(ByVal value As String)
            mdirectorate = value
        End Set
    End Property

    Public Property dataid() As Integer
        Get
            Return mdataid
        End Get
        Set(ByVal value As Integer)
            mdataid = value
        End Set
    End Property

    Public Property transactionnumber() As Integer
        Get
            Return mtransactionnumber
        End Get
        Set(ByVal value As Integer)
            mtransactionnumber = value
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

    Public Property Systemnysuserid() As Nullable(Of Integer)
        Get
            Return mSystemnysuserid
        End Get
        Set(ByVal value As Nullable(Of Integer))
            mSystemnysuserid = value
        End Set
    End Property

    Public Property transactionfee() As String
        Get
            Return mtransactionfee
        End Get
        Set(ByVal value As String)
            mtransactionfee = value
        End Set
    End Property

    Public Property dataitemnett() As String
        Get
            Return mdataitemnett
        End Get
        Set(ByVal value As String)
            mdataitemnett = value
        End Set
    End Property

    Public Property dataitemvat() As String
        Get
            Return mdataitemvat
        End Get
        Set(ByVal value As String)
            mdataitemvat = value
        End Set
    End Property

    Public Property dataitemgross() As String
        Get
            Return mdataitemgross
        End Get
        Set(ByVal value As String)
            mdataitemgross = value
        End Set
    End Property

    Public Property Invoiceexportednett() As String
        Get
            Return mInvoiceexportednett
        End Get
        Set(ByVal value As String)
            mInvoiceexportednett = value
        End Set
    End Property

    Public Property Invoiceexportedvat() As String
        Get
            Return mInvoiceexportedvat
        End Get
        Set(ByVal value As String)
            mInvoiceexportedvat = value
        End Set
    End Property

    Public Property Invoiceexportedgross() As String
        Get
            Return mInvoiceexportedgross
        End Get
        Set(ByVal value As String)
            mInvoiceexportedgross = value
        End Set
    End Property

    Public Property Invoiceexport() As Nullable(Of Boolean)
        Get
            Return mInvoiceexport
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            mInvoiceexport = value
        End Set
    End Property

    Public Property Invoiceexportdate() As DateTime
        Get
            Return mInvoiceexportdate
        End Get
        Set(ByVal value As DateTime)
            mInvoiceexportdate = value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r"></param>
    ''' <returns></returns>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Private Shared Function makeFeedInvoiceFromRow( _
            ByVal r As IDataReader _
        ) As FeedInvoice
        Return New FeedInvoice( _
                clsStuff.notWholeNumber(r.Item("invoiceid")), _
                clsStuff.notWholeNumber(r.Item("transactionnumber")), _
                toNullableInteger(r.Item("systemnysuserid")), _
                clsStuff.notString(r.Item("invoiceexportednett")), _
                clsStuff.notString(r.Item("invoiceexportedvat")), _
                clsStuff.notString(r.Item("invoiceexportedgross")), _
                toNullableBoolean(r.Item("invoiceexport")), _
                CDate(r.Item("invoiceexportdate")), _
                clsStuff.notString(r.Item("transactionfee")))
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r"></param>
    ''' <returns></returns>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Private Shared Function makeFeedInvoiceFromRowBoss( _
            ByVal r As IDataReader _
        ) As FeedInvoice
        Return New FeedInvoice( _
                clsStuff.notString(r.Item("bossid")), _
                clsStuff.notDouble(r.Item("bosstotal")), _
                clsStuff.notDouble(r.Item("bosscomNett")), _
                clsStuff.notDouble(r.Item("bosscomVat")))
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r"></param>
    ''' <returns></returns>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Private Shared Function makeFeedInvoiceFromRowBossAll( _
            ByVal r As IDataReader _
        ) As FeedInvoice
        Return New FeedInvoice( _
                clsStuff.notWholeNumber(r.Item("dataid")), _
                clsStuff.notString(r.Item("categoryname")), _
                clsStuff.notString(r.Item("nettamount")), _
                clsStuff.notString(r.Item("vatamount")), _
                clsStuff.notString(r.Item("totalamount")), _
                clsStuff.notString(r.Item("transactionfee")))
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r"></param>
    ''' <returns></returns>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Private Shared Function makeFeedInvoiceFromRowTotals( _
            ByVal r As IDataReader _
        ) As FeedInvoice
        Return New FeedInvoice( _
                clsStuff.notWholeNumber(r.Item("transactionnumber")), _
                clsStuff.notString(r.Item("nettamount")), _
                clsStuff.notString(r.Item("vatamount")), _
                clsStuff.notString(r.Item("totalamount")), _
                clsStuff.notString(r.Item("transactionfee")))
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r"></param>
    ''' <returns></returns>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Private Shared Function makeFeedInvoiceFromRowCima( _
            ByVal r As IDataReader _
        ) As FeedInvoice
        Return New FeedInvoice( _
                clsStuff.notString(r.Item("directorate")), _
                clsStuff.notString(r.Item("department")))
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r"></param>
    ''' <returns></returns>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Private Shared Function makeFeedInvoiceFromRowCode( _
            ByVal r As IDataReader _
        ) As FeedInvoice
        Return New FeedInvoice( _
                clsStuff.notString(r.Item("customercode")))
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pcimacode"></param>
    ''' <returns></returns>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Shared Function getCima( _
            ByVal pcimacode As Integer _
        ) As FeedInvoice
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("cima_get", "@cimacode", pcimacode)
                Dim ret As New FeedInvoice
                If r.Read() Then
                    ret = makeFeedInvoiceFromRowCima(r)
                End If
                Return ret
            End Using
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pstrcustomername"></param>
    ''' <returns></returns>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Shared Function getCode( _
            ByVal pstrcustomername As String _
        ) As String
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim strCode As String = clsStuff.notString(dbh.callSPSingleValue _
                                    ("code_get", "@customername", pstrcustomername))
            Return strCode
        End Using
    End Function

    Public Shared Function getAnchorCode(ByVal pstrcode As String) As String
        Using dbh As New SqlDatabaseHandle(getMevisConnection)
            Dim strCode As String = clsStuff.notString(dbh.callSPSingleValueCanReturnNothing _
                                    ("anchorCode_get", "@code", pstrcode))
            Return strCode
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pInvoiceid"></param>
    ''' <returns></returns>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Shared Function [get]( _
            ByVal pInvoiceid As Integer _
        ) As FeedInvoice
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("FeedInvoice_get", "@invoiceid", pInvoiceid)
                Dim ret As New FeedInvoice
                If r.Read() Then
                    ret = makeFeedInvoiceFromRow(r)
                End If
                Return ret
            End Using
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' 
    ''' </summary>
    ''' <param name="pinttransactionnumber"></param>
    ''' <param name="pintinvoiceid"></param>
    ''' <returns></returns>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Shared Function bookingTotalValue(ByVal pinttransactionnumber As Integer, ByVal pintinvoiceid As Integer) As FeedInvoice
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("FeedData_bookingTotalValueget", "@transactionnumber", pinttransactionnumber, _
                                                "@invoiceid", pintinvoiceid)
                Dim ret As New FeedInvoice
                If r.Read() Then
                    ret = makeFeedInvoiceFromRowTotals(r)
                End If
                Return ret
            End Using
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Shared Function list() As List(Of FeedInvoice)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of FeedInvoice)()
            Using r As IDataReader = dbh.callSP("FeedInvoice_list")
                While r.Read()
                    ret.Add(makeFeedInvoiceFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pintinvoiceid"></param>
    ''' <param name="pstrbossid"></param>
    ''' <param name="pdbltotal"></param>
    ''' <param name="pdblcomnett"></param>
    ''' <param name="pdblcomvat"></param>
    ''' <returns></returns>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Shared Function FeedInvoiceBossSave(ByVal pintinvoiceid As Integer, ByVal pstrbossid As String, _
                                        ByVal pdbltotal As Double, ByVal pdblcomnett As Double, _
                                        ByVal pdblcomvat As Double) As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim intid As Integer = clsStuff.notWholeNumber(dbh.callSPSingleValue("FeedInvoiceBoss_save", _
                                                    "@Invoiceid", pintinvoiceid, _
                                                    "@bossid", pstrbossid, _
                                                    "@bosstotal", pdbltotal, _
                                                    "@bosscomNett", pdblcomnett, _
                                                    "@bosscomVat", pdblcomvat))
            Return intid
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pintTransactionnumber"></param>
    ''' <returns></returns>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Shared Function FeedInvoiceBossGet(ByVal pintTransactionnumber As Integer) As FeedInvoice
        Using dbh As New SqlDatabaseHandle(getConnection)
            Using r As IDataReader = dbh.callSP("FeedInvoiceBoss_get", "@Transactionnumber", _
                                                pintTransactionnumber)
                Dim ret As New FeedInvoice
                If r.Read() Then
                    ret = makeFeedInvoiceFromRowBoss(r)
                End If
                Return ret
            End Using
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pintTransactionnumber"></param>
    ''' <returns></returns>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Shared Function FeedInvoiceBossGetAll(ByVal pintTransactionnumber As Integer) As List(Of FeedInvoice)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of FeedInvoice)()
            Using r As IDataReader = dbh.callSP("FeedInvoiceBoss_getAll", "@Transactionnumber", _
                                                pintTransactionnumber)
                While r.Read()
                    ret.Add(makeFeedInvoiceFromRowBossAll(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Function save() As Integer
        Using dbh As New SqlDatabaseHandle(getConnection)
            mInvoiceid = clsStuff.notWholeNumber(dbh.callSPSingleValue("FeedInvoice_saveAM", _
                                                    "@Invoiceid", mInvoiceid, _
                                                    "@transactionnumber", mtransactionnumber, _
                                                    "@Systemnysuserid", mSystemnysuserid, _
                                                    "@Invoiceexportednett", mInvoiceexportednett, _
                                                    "@Invoiceexportedvat", mInvoiceexportedvat, _
                                                    "@Invoiceexportedgross", mInvoiceexportedgross, _
                                                    "@Invoiceexport", mInvoiceexport, _
                                                    "@Invoiceexportdate", mInvoiceexportdate, _
                                                    "@transactionfee", mtransactionfee))
            Return mInvoiceid
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pInvoiceid"></param>
    ''' <remarks>Created 27/03/2009 Nick Massarella</remarks>
    Public Shared Sub delete( _
            ByVal pInvoiceid As Integer _
        )
        Using dbh As New SqlDatabaseHandle(getConnection)
            dbh.callNonQuerySP("FeedInvoice_delete", "@Invoiceid", pInvoiceid)
        End Using
    End Sub

End Class
