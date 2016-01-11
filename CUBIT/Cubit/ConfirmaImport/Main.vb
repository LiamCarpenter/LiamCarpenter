Imports System.IO

Module Main

    Sub Main()

        Using Cubit As Cubit.EF.CUBITEntities = New Cubit.EF.CUBITEntities

            Dim FileName2 As String = System.Configuration.ConfigurationSettings.AppSettings("ConfirmaFileLocation") & "\" & Date.Now.AddDays(-133).ToString("dd-MM-yyyy") & ".csv"

            For counter = 1 To 133

                Dim RemoveDays = 0 - counter

                Dim FileName As String = System.Configuration.ConfigurationSettings.AppSettings("ConfirmaFileLocation") & "\" & Date.Now.AddDays(RemoveDays).ToString("dd-MM-yyyy") & ".csv"

                Try

                    Dim File As New StreamReader(FileName)
                    Console.WriteLine("Processing :" & FileName)
                    Dim FirstRecord = True

                    For Each Line In Split(File.ReadToEnd, vbCrLf)
                        If FirstRecord Then
                            FirstRecord = False
                        Else
                            Dim LineSplit = Split(Line.ToString, """,""")
                            Console.WriteLine("Processing: " & LineSplit(Enums.ConfirmaFile.TransactionNumber).Replace("""", ""))

                            If LineSplit.Length < 45 Then
                            Else
                                Dim TempConfirmaImport As New Cubit.EF.ConfirmaImport()
                                TempConfirmaImport.AIAgentBkr = LineSplit(Enums.ConfirmaFile.AIAgentBkr)
                                TempConfirmaImport.AIBillInstruct = LineSplit(Enums.ConfirmaFile.AIBillInstruct)
                                TempConfirmaImport.AIBkrName = LineSplit(Enums.ConfirmaFile.AIBkrName)
                                TempConfirmaImport.AICol10 = LineSplit(Enums.ConfirmaFile.AICol10)
                                TempConfirmaImport.AICol6 = LineSplit(Enums.ConfirmaFile.AICol6)
                                TempConfirmaImport.AICol7 = LineSplit(Enums.ConfirmaFile.AICol7)
                                TempConfirmaImport.AICol8 = LineSplit(Enums.ConfirmaFile.AICol8)
                                TempConfirmaImport.AICol9 = LineSplit(Enums.ConfirmaFile.AICol9)
                                TempConfirmaImport.AIComments = LineSplit(Enums.ConfirmaFile.AIComments)
                                TempConfirmaImport.AICostCode = LineSplit(Enums.ConfirmaFile.AICostCode)
                                TempConfirmaImport.ArrivalDate = LineSplit(Enums.ConfirmaFile.ArrivalDate)
                                TempConfirmaImport.BookedDate = LineSplit(Enums.ConfirmaFile.BookedDate)
                                TempConfirmaImport.BookerEmail = LineSplit(Enums.ConfirmaFile.BookerEmail)
                                TempConfirmaImport.BookingReference = LineSplit(Enums.ConfirmaFile.BookingReference)
                                TempConfirmaImport.Company = LineSplit(Enums.ConfirmaFile.Company)
                                TempConfirmaImport.ConfermaInvoiceNo = LineSplit(Enums.ConfirmaFile.ConfermaInvoiceNo)
                                TempConfirmaImport.CountryCode = LineSplit(Enums.ConfirmaFile.CountryCode)
                                TempConfirmaImport.Department = LineSplit(Enums.ConfirmaFile.Department)
                                TempConfirmaImport.DepartureDate = LineSplit(Enums.ConfirmaFile.DepartureDate)
                                TempConfirmaImport.DESCRIPTION = LineSplit(Enums.ConfirmaFile.DESCRIPTION)
                                TempConfirmaImport.GuestPNR = LineSplit(Enums.ConfirmaFile.GuestPNR)
                                TempConfirmaImport.HotelDetails = LineSplit(Enums.ConfirmaFile.HotelDetails)
                                TempConfirmaImport.ImportDate = Date.Now
                                TempConfirmaImport.InvoiceLastModifiedDate = LineSplit(Enums.ConfirmaFile.InvoiceLastModifiedDate)
                                TempConfirmaImport.InvoiceLineDetailID = LineSplit(Enums.ConfirmaFile.InvoiceLineDetailID).Replace("""", "")
                                TempConfirmaImport.Last4Digits = LineSplit(Enums.ConfirmaFile.Last4Digits)
                                TempConfirmaImport.LeadPassengerName = LineSplit(Enums.ConfirmaFile.LeadPassengerName)
                                TempConfirmaImport.MajorDestination = LineSplit(Enums.ConfirmaFile.MajorDestination)
                                TempConfirmaImport.OutofPolicyReason = LineSplit(Enums.ConfirmaFile.OutofPolicyReason)
                                TempConfirmaImport.PaymentMethod = LineSplit(Enums.ConfirmaFile.PaymentMethod)
                                TempConfirmaImport.RefBooker = LineSplit(Enums.ConfirmaFile.RefBooker)
                                TempConfirmaImport.RoomDetails = LineSplit(Enums.ConfirmaFile.RoomDetails)
                                TempConfirmaImport.SaleVatCode = LineSplit(Enums.ConfirmaFile.SaleVatCode)
                                TempConfirmaImport.SupplierID = LineSplit(Enums.ConfirmaFile.SupplierID)
                                TempConfirmaImport.SupplierInvoiceRef = LineSplit(Enums.ConfirmaFile.SupplierInvoiceRef)
                                TempConfirmaImport.SupplierName = LineSplit(Enums.ConfirmaFile.SupplierName)
                                TempConfirmaImport.TotalBilledInGbp = LineSplit(Enums.ConfirmaFile.TotalBilledInGbp)
                                TempConfirmaImport.TotalSaleAmount = LineSplit(Enums.ConfirmaFile.TotalSaleAmount)
                                TempConfirmaImport.TotalSaleVatAmount = LineSplit(Enums.ConfirmaFile.TotalSaleVatAmount)
                                TempConfirmaImport.TransactionCurrency = LineSplit(Enums.ConfirmaFile.TransactionCurrency)
                                TempConfirmaImport.TransactionDate = LineSplit(Enums.ConfirmaFile.TransactionDate)
                                TempConfirmaImport.TransactionLineNumber = LineSplit(Enums.ConfirmaFile.TransactionLineNumber)
                                TempConfirmaImport.TransactionNumber = LineSplit(Enums.ConfirmaFile.TransactionNumber).Replace("""", "")
                                TempConfirmaImport.TransactionType = LineSplit(Enums.ConfirmaFile.TransactionType)
                                TempConfirmaImport.TravellerEmail = LineSplit(Enums.ConfirmaFile.TravellerEmail)

                                Cubit.ConfirmaImport.Add(TempConfirmaImport)
                            End If

                        End If
                    Next

                    Console.WriteLine("Saving Changes")
                    Cubit.SaveChanges()

                    File.Close()

                Catch ex As Exception
                    Console.WriteLine("Processing :" & FileName)
                    Console.WriteLine(ex.Message)
                    Console.ReadLine()
                End Try
            Next
            
        End Using

    End Sub

End Module

Public Class Enums

    Enum ConfirmaFile As Integer
        TransactionNumber = 0
        TransactionLineNumber = 1
        TransactionDate = 2
        DepartureDate = 3
        ArrivalDate = 4
        LeadPassengerName = 5
        BookingReference = 6
        MajorDestination = 7
        RefBooker = 8
        Department = 9
        TransactionType = 10
        TransactionCurrency = 11
        TotalSaleAmount = 12
        TotalSaleVatAmount = 13
        SaleVatCode = 14
        NetAmount = 15
        TotalBilledInGbp = 16
        PaymentMethod = 17
        SupplierID = 18
        SupplierName = 19
        HotelDetails = 20
        RoomDetails = 21
        Company = 22
        ConfermaInvoiceNo = 23
        DESCRIPTION = 24
        SupplierInvoiceRef = 25
        CountryCode = 26
        GuestPNR = 27
        InvoiceLastModifiedDate = 28
        BookedDate = 29
        OutofPolicyReason = 30
        Last4Digits = 31
        TravellerEmail = 32
        BookerEmail = 33
        AIBkrName = 34
        AICostCode = 35
        AIBillInstruct = 36
        AIComments = 37
        AIAgentBkr = 38
        AICol6 = 39
        AICol7 = 40
        AICol8 = 41
        AICol9 = 42
        AICol10 = 43
        InvoiceLineDetailID = 44

    End Enum
End Class