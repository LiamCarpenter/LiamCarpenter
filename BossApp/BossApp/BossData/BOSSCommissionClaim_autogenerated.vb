Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class BOSSCommissionClaim

    Public Sub New( _
)
    End Sub

    Public Sub New(ByVal pinvoiceid As String, _
                   ByVal pvenuename As String, _
                   ByVal pemailto As String)
        minvoiceid = pinvoiceid
        mvenuename = pvenuename
        memailto = pemailto
    End Sub

    Public Sub New(ByVal pinvoiceid As String, _
                       ByVal pvenuename As String, _
                       ByVal pinvoiceamount As Decimal, _
                       ByVal pcommnet As Decimal, _
                       ByVal pcommvat As Decimal, _
                       ByVal pcommtotal As Decimal, _
                       ByVal pcommrecvd As Decimal, _
                       ByVal pinvoicedate As String, _
                       ByVal pleadname As String, _
                       ByVal pref As String, _
                       ByVal peventdate As String, _
                       ByVal pproduct As String, _
                       ByVal paccountsemail As String, _
                       ByVal pvenueemail As String, _
                       ByVal platestcontactemail As String, _
                       ByVal paddress1 As String, _
                       ByVal paddress2 As String, _
                       ByVal paddress3 As String, _
                       ByVal paddress4 As String, _
                       ByVal ppostcode As String, _
                       ByVal ptelephone As String, _
                       ByVal pfax As String, _
                       ByVal pCurrency As String)
        minvoiceid = pinvoiceid
        mvenuename = pvenuename
        minvoiceamount = pinvoiceamount
        mcommnet = pcommnet
        mcommvat = pcommvat
        mcommtotal = pcommtotal
        mcommrecvd = pcommrecvd
        minvoicedate = pinvoicedate
        mleadname = pleadname
        mref = pref
        meventdate = peventdate
        mproduct = pproduct
        maccountsemail = paccountsemail
        mvenueemail = pvenueemail
        mlatestcontactemail = platestcontactemail
        maddress1 = paddress1
        maddress2 = paddress2
        maddress3 = paddress3
        maddress4 = paddress4
        mpostcode = ppostcode
        mtelephone = ptelephone
        mfax = pfax

        'R2.16 CR
        mCurrency = pCurrency
    End Sub

    Private memailto As String
    Private maddress1 As String
    Private maddress2 As String
    Private maddress3 As String
    Private maddress4 As String
    Private mpostcode As String
    Private mtelephone As String
    Private mfax As String
    Private minvoiceid As String
    Private mvenuename As String
    Private minvoiceamount As Decimal
    Private mcommnet As Decimal
    Private mcommvat As Decimal
    Private mcommtotal As Decimal
    Private mcommrecvd As Decimal
    Private minvoicedate As String
    Private mleadname As String
    Private mref As String
    Private meventdate As String
    Private mproduct As String
    Private maccountsemail As String
    Private mvenueemail As String
    Private mlatestcontactemail As String

    'R2.16 CR
    Private mCurrency As String

    Public Property emailto() As String
        Get
            Return memailto
        End Get
        Set(ByVal value As String)
            memailto = value
        End Set
    End Property

    Public Property invoiceid() As String
        Get
            Return minvoiceid
        End Get
        Set(ByVal value As String)
            minvoiceid = value
        End Set
    End Property

    Public Property venuename() As String
        Get
            Return mvenuename
        End Get
        Set(ByVal value As String)
            mvenuename = value
        End Set
    End Property

    Public Property invoiceamount() As Decimal
        Get
            Return minvoiceamount
        End Get
        Set(ByVal value As Decimal)
            minvoiceamount = value
        End Set
    End Property
    Public Property commnet() As Decimal
        Get
            Return mcommnet
        End Get
        Set(ByVal value As Decimal)
            mcommnet = value
        End Set
    End Property
    Public Property commvat() As Decimal
        Get
            Return mcommvat
        End Get
        Set(ByVal value As Decimal)
            mcommvat = value
        End Set
    End Property
    Public Property commtotal() As Decimal
        Get
            Return mcommtotal
        End Get
        Set(ByVal value As Decimal)
            mcommtotal = value
        End Set
    End Property
    Public Property invoicedate() As String
        Get
            Return minvoicedate
        End Get
        Set(ByVal value As String)
            minvoicedate = value
        End Set
    End Property

    Public Property leadname() As String
        Get
            Return mleadname
        End Get
        Set(ByVal value As String)
            mleadname = value
        End Set
    End Property

    Public Property ref() As String
        Get
            Return mref
        End Get
        Set(ByVal value As String)
            mref = value
        End Set
    End Property

    Public Property eventdate() As String
        Get
            Return meventdate
        End Get
        Set(ByVal value As String)
            meventdate = value
        End Set
    End Property

    Public Property product() As String
        Get
            Return mproduct
        End Get
        Set(ByVal value As String)
            mproduct = value
        End Set
    End Property

    Public Property accountsemail() As String
        Get
            Return maccountsemail
        End Get
        Set(ByVal value As String)
            maccountsemail = value
        End Set
    End Property

    Public Property venueemail() As String
        Get
            Return mvenueemail
        End Get
        Set(ByVal value As String)
            mvenueemail = value
        End Set
    End Property

    Public Property latestcontactemail() As String
        Get
            Return mlatestcontactemail
        End Get
        Set(ByVal value As String)
            mlatestcontactemail = value
        End Set
    End Property

    Public Property address1() As String
        Get
            Return maddress1
        End Get
        Set(ByVal value As String)
            maddress1 = value
        End Set
    End Property

    Public Property address2() As String
        Get
            Return maddress2
        End Get
        Set(ByVal value As String)
            maddress2 = value
        End Set
    End Property

    Public Property address3() As String
        Get
            Return maddress3
        End Get
        Set(ByVal value As String)
            maddress3 = value
        End Set
    End Property

    Public Property address4() As String
        Get
            Return maddress4
        End Get
        Set(ByVal value As String)
            maddress4 = value
        End Set
    End Property

    Public Property postcode() As String
        Get
            Return mpostcode
        End Get
        Set(ByVal value As String)
            mpostcode = value
        End Set
    End Property

    Public Property telephone() As String
        Get
            Return mtelephone
        End Get
        Set(ByVal value As String)
            mtelephone = value
        End Set
    End Property

    Public Property fax() As String
        Get
            Return mfax
        End Get
        Set(ByVal value As String)
            mfax = value
        End Set
    End Property

    'R2.16 CR
    Public Property Currency() As String
        Get
            Return mCurrency
        End Get
        Set(value As String)
            mCurrency = value
        End Set
    End Property

    'R2.16 CR - added currency
    Private Shared Function makeBOSSCommFromRow( _
            ByVal r As IDataReader _
        ) As BOSSCommissionClaim
        Return New BOSSCommissionClaim( _
                clsUseful.notString(r.Item("invoiceid")), _
                clsUseful.notString(r.Item("venuename")), _
                CDec(r.Item("invoiceamount")), _
                CDec(r.Item("commnet")), _
                CDec(r.Item("commvat")), _
                CDec(r.Item("commtotal")), _
                CDec(r.Item("commrecvd")), _
                clsUseful.notString(r.Item("invoicedate")), _
                clsUseful.notString(r.Item("leadname")), _
                clsUseful.notString(r.Item("ref")), _
                clsUseful.notString(r.Item("eventdate")), _
                clsUseful.notString(r.Item("product")), _
                clsUseful.notString(r.Item("accountsemail")), _
                clsUseful.notString(r.Item("venueemail")), _
                clsUseful.notString(r.Item("latestcontactemail")), _
                clsUseful.notString(r.Item("address1")), _
                clsUseful.notString(r.Item("address2")), _
                clsUseful.notString(r.Item("address3")), _
                clsUseful.notString(r.Item("address4")), _
                clsUseful.notString(r.Item("postcode")), _
                clsUseful.notString(r.Item("telephone")), _
                clsUseful.notString(r.Item("fax")), _
        clsUseful.notString(r.Item("currency")))
    End Function

    Private Shared Function makeBOSSCommFromRowSearch( _
            ByVal r As IDataReader _
        ) As BOSSCommissionClaim
        Return New BOSSCommissionClaim( _
                clsUseful.notString(r.Item("invoiceid")), _
                clsUseful.notString(r.Item("venuename")), _
                clsUseful.notString(r.Item("emailto")))
    End Function

    Public Shared Function commissionClaimSpecial() As List(Of BOSSCommissionClaim)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BOSSCommissionClaim)()
            Using r As IDataReader = dbh.callSP("bossCommission_ClaimSpecial") ', "@startdate", pstrStartDate, "@enddate", pstrEndDate)
                While r.Read()
                    ret.Add(makeBOSSCommFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function bossCommissionSearch(ByVal pstrSearch As String) As List(Of BOSSCommissionClaim)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BOSSCommissionClaim)()
            Using r As IDataReader = dbh.callSP("bossCommission_search", "@search", pstrSearch)
                While r.Read()
                    ret.Add(makeBOSSCommFromRowSearch(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function commissionClaimIndiv(ByVal pInvoiceID As String) As List(Of BOSSCommissionClaim)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BOSSCommissionClaim)()
            Using r As IDataReader = dbh.callSP("bossCommission_ClaimIndividual", "@invoiceid", pInvoiceID)
                While r.Read()
                    ret.Add(makeBOSSCommFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function commissionClaimIndividual(ByVal pbSpecial2 As Boolean, _
                                                     ByVal pbSpecialFinal As Boolean, _
                                                     ByVal pbSpeciallaw As Boolean, _
                                                     ByVal pbClaimInitial As Boolean, _
                                                     ByVal pbClaim2 As Boolean, _
                                                     ByVal pbClaim3 As Boolean, _
                                                     ByVal pbClaim4 As Boolean, _
                                                     ByVal pbClaim5 As Boolean) As List(Of BOSSCommissionClaim)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BOSSCommissionClaim)()
            Dim strProc As String = ""
            If pbSpecial2 Then
                strProc = "bossCommission_ClaimSpecial2"
            ElseIf pbSpecialFinal Then
                strProc = "bossCommission_ClaimSpecialFinal"
            ElseIf pbSpeciallaw Then
                strProc = "bossCommission_ClaimSpecialLaw"
            ElseIf pbClaimInitial Then
                strProc = "bossCommission_ClaimInitial"
            ElseIf pbClaim2 Then
                strProc = "bossCommission_Claim2"
            ElseIf pbClaim3 Then
                strProc = "bossCommission_Claim3"
            ElseIf pbClaim4 Then
                strProc = "bossCommission_Claim4"
            ElseIf pbClaim5 Then
                strProc = "bossCommission_Claim5"
            End If
            Using r As IDataReader = dbh.callSP(strProc)
                While r.Read()
                    ret.Add(makeBOSSCommFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

    Public Shared Function commissionClaim(ByVal pbSpecial2 As Boolean, _
                                                     ByVal pbSpecialFinal As Boolean, _
                                                     ByVal pbSpeciallaw As Boolean, _
                                                     ByVal pbClaimInitial As Boolean, _
                                                     ByVal pbClaim2 As Boolean, _
                                                     ByVal pbClaim3 As Boolean, _
                                                     ByVal pbClaim4 As Boolean, _
                                                     ByVal pbClaim5 As Boolean) As List(Of BOSSCommissionClaim)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of BOSSCommissionClaim)()
            Dim strProc As String = ""
            If pbSpecial2 Then
                strProc = "bossCommission_ClaimSpecial2"
            ElseIf pbSpecialFinal Then
                strProc = "bossCommission_ClaimSpecialFinal"
            ElseIf pbSpeciallaw Then
                strProc = "bossCommission_ClaimSpecialLaw"
            ElseIf pbClaimInitial Then
                strProc = "bossCommission_ClaimInitial"
            ElseIf pbClaim2 Then
                strProc = "bossCommission_Claim2"
            ElseIf pbClaim3 Then
                strProc = "bossCommission_Claim3"
            ElseIf pbClaim4 Then
                strProc = "bossCommission_Claim4"
            ElseIf pbClaim5 Then
                strProc = "bossCommission_Claim5"
            End If
            Using r As IDataReader = dbh.callSP(strProc)
                While r.Read()
                    ret.Add(makeBOSSCommFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function


End Class
