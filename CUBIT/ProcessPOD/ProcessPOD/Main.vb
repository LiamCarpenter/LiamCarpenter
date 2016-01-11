Module Main

    'We can load all these details as the list is small it will save requests to the database while processing
    Public Property FeedTransactionList As List(Of Cubit.EF.FeedTransaction)
    Public Property FeedParameterList As List(Of Cubit.EF.FeedParameter)
    Public Property NewFeedImportList As New List(Of Cubit.EF.FeedImportData)

    Sub Main()

        Using Cubit As Cubit.EF.CUBITEntities = New Cubit.EF.CUBITEntities

            Dim ErrorCount As Integer = 0

            'Load all the Records where the departure date is yesterday and the payment method wasn't HT and it hasn't been cancelled
            Dim CheckDate As String = Date.Now.AddDays(-36).ToString("dd MMM yyyy")
            Dim TodaysDate As String = Date.Now.ToString("dd MMM yyyy")
            Dim FeedBookedDataList = (From x In Cubit.FeedBookedData Where x.PaymentMethod <> "HT" And x.DepartureDate >= CheckDate And x.DepartureDate < TodaysDate And x.currentstatus.ToLower = "c" And x.TransferredtoImportData <> True).ToList

            'Use the list so we only load the Parameters and Transactions that are needed
            Dim DistinctGroupIDList = (From x In FeedBookedDataList Select x.groupid Distinct).ToList
            FeedParameterList = (From x In Cubit.FeedParameter Where DistinctGroupIDList.Contains(x.groupid)).ToList
            Console.WriteLine("Loaded " & FeedParameterList.Count & " Feed Parameters")

            Dim DistinctTransactionIDList = (From x In FeedParameterList Select x.transactionid Distinct).ToList
            FeedTransactionList = (From x In Cubit.FeedTransaction Where DistinctTransactionIDList.Contains(x.transactionid)).ToList
            Console.WriteLine("Loaded " & FeedTransactionList.Count & " Feed Transactions")

            For Each FeedBooked In FeedBookedDataList
                'Set it to transfered
                FeedBooked.TransferredtoImportData = True
                Console.WriteLine("Processing: " & FeedBooked.LeadPassengerName)

                'Create a TempImportData to save back to the database
                Dim TempImportData As New Cubit.EF.FeedImportData
                Try
                    CreateFeedImportFromFeedBooked(TempImportData, FeedBooked)
                    NewFeedImportList.Add(TempImportData)
                Catch ex As Exception
                    ErrorCount += 1
                End Try

                Cubit.FeedImportData.Add(TempImportData)
                Cubit.SaveChanges()
            Next

            Console.WriteLine(ErrorCount & " errors")
            Console.WriteLine(NewFeedImportList.Count & " added to the list")
            For Each x In NewFeedImportList
                Console.WriteLine(x.passengername & " " & x.parameterid)
            Next

        End Using

    End Sub

    Public Sub CreateFeedImportFromFeedBooked(ByRef Import As Cubit.EF.FeedImportData, ByVal Booked As Cubit.EF.FeedBookedData)
        Import.transactionnumber = Booked.BookingId
        Import.transactionlinenumber = Booked.LineNumber
        Import.transactiondate = Booked.BookedDate
        Import.arrivaldate = Booked.ArrivalDate
        Import.departuredate = Booked.DepartureDate
        Import.ref1 = Booked.BookingReference
        Import.booker = Booked.Creator
        Import.ref3 = Booked.AIComments
        Import.venuename = Booked.SupplierName
        Import.venuedetails = Booked.HotelDetails
        Import.roomdetails = Booked.RoomDetails
        Import.groupid = Booked.groupid
        Import.groupname = Booked.groupname

        'SPIE change to POD
        If Booked.groupid = 100134 Then
            Import.groupid = 100138
            Import.groupname = "SPIE POD"
        End If

        Import.totalamount = Booked.TotalBilledInGbp
        Import.bookerinitials = Left(Booked.AIAgentBkr, 10)
        Import.AICol6 = Booked.AICol6
        Import.AICol7 = Booked.AICol7
        Import.AICol8 = Booked.AICol8
        Import.AICol9 = Booked.AICol9
        Import.AICol10 = Booked.AICol10
        Import.TravellerEmail = Booked.TravellerEmail
        Import.BookerEmail = Booked.BookerEmail
        Import.currency = "GBP" 'We take the TotalBilledInGbp so we can alway put through as GBP
        Import.guestPNR = Booked.GuestPNR
        Import.cancellation = 0
        Import.statusid = 1000
        Import.datecreated = Date.Now
        Import.BookedDate = Booked.BookedDate
        Import.lastupdated = Date.Now
        Import.categoryid = 1000
        Import.BookingOnly = True
        Import.confermainvoicenumber = 0
        Import.supplierinvoice = ""
        Import.costcode = ""
        Import.transactionvaluenew = ""
        Import.failreason = ""
        Import.ref2 = 0
        Import.excludeFromExport = False
        Import.Last4Digits = ""
        Import.OutofPolicyReason = ""
        Import.complaintsText = ""
        Import.dept = ""

        'Create The Passenger name in the correct format
        Dim strpassengername As String = Booked.LeadPassengerName.Replace("NULL", "")
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

        Import.passengername = strpassengername

        'Calculate the nett ammounts
        'Assume VAT is 20% because if it changes so does the calculation to retrospectively calculate VAT 
        Import.vatrate = 20
        Import.vatamount = Math.Round(CDbl(Import.totalamount / 6), 2)
        Import.nettamount = Math.Round(CDbl(Import.totalamount - Import.vatamount), 2)

        'Load up the venue details based on venuename

        'First Lets search in VenueAlternate to see if we can find a different name
        Using Cubit As Cubit.EF.CUBITEntities = New Cubit.EF.CUBITEntities
            Dim TempVenueName = Import.venuename
            Dim VenueAlternate = (From x In Cubit.VenueAlternate Where x.confermaname = TempVenueName).FirstOrDefault

            If VenueAlternate IsNot Nothing Then
                'Load the venue based on the ve_reference
                Using VenueDB As NYSDBEntities = New NYSDBEntities
                    Dim Venue = (From x In VenueDB.venue Where x.ve_reference = VenueAlternate.ve_reference).FirstOrDefault

                    If Venue IsNot Nothing Then
                        Import.venuereference = Venue.ve_reference
                        Import.venuebosscode = Venue.boss_code
                        Import.venueEX = Venue.ve_EX.GetValueOrDefault(0)
                        Import.venueDD = Venue.ve_DD.GetValueOrDefault(0)
                    Else

                    End If

                End Using
            Else
                'Attempt to load by the name
                Using VenueDB As NYSDBEntities = New NYSDBEntities
                    Dim Venue = (From x In VenueDB.venue Where x.ve_name = TempVenueName).FirstOrDefault

                    If Venue IsNot Nothing Then
                        Import.venuereference = Venue.ve_reference
                        Import.venuebosscode = Venue.boss_code
                        Import.venueEX = Venue.ve_EX
                        Import.venueDD = Venue.ve_DD
                    Else

                    End If

                End Using
            End If

        End Using

        'Get the ParameterID
        Dim GroupID As Integer = Import.groupid
        Dim Transactiontype As String = "room"
        Dim TransactionCode As String = "offline"
        Dim BookingDate As DateTime = Import.BookedDate

        'Work out TransactionCode from bookerinitials
        If Import.bookerinitials.ToLower = "con" Or Import.bookerinitials = "" Or Import.bookerinitials.ToLower = "nys corporate" Then
            TransactionCode = "online"
            Import.bookerinitials = "CON"
        ElseIf Import.bookerinitials.ToLower = "man" Then
            TransactionCode = "offline"
        Else
            TransactionCode = "offline"
        End If

        Dim SelectedParameterID = (From x In FeedParameterList Where x.groupid = GroupID
                                   Join transaction In FeedTransactionList On transaction.transactionid Equals x.transactionid
                                   Where transaction.transactioncode = TransactionCode And
                                   transaction.transactiontype = Transactiontype And
                                   BookingDate >= x.parameterstart And
                                   BookingDate <= x.parameterend
                                   Select x.parameterid).FirstOrDefault

        Import.parameterid = SelectedParameterID


    End Sub

End Module