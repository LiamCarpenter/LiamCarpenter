Option Strict On
Option Explicit On
Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Imports System.Data


Namespace NSConfigUtils
    Public Module ConfigUtils

        Private configurationAppSettings As New System.Configuration.AppSettingsReader

        Public Function getConfig(ByVal key As String) As String
            Return CType(configurationAppSettings.GetValue(key, GetType(System.String)), String)
        End Function

        Public Function GetHotelGroup(ByVal intVenueid As Integer) As String
            Dim ret As String = ""
            Using dbh As New SqlDatabaseHandle(getConfig("connectionString"))
                Using r As IDataReader = dbh.runSQLQuery("select v.hg_code from enquiryvenue e join Venue v on e.venueRef = v.ve_reference where  e.enquiryvenueid= " + intVenueid.ToString())
                    Dim retdb As New List(Of String)
                    Dim i As Integer = 0
                    While r.Read()
                        If i = 0 Then
                            ret = r.GetString(0)
                        End If
                        i += 1
                    End While
                End Using
            End Using
            Return ret

        End Function

        Public Function IsConfigRateOverrideNonComm(ByVal intVenueid As Integer) As Boolean
            Dim GroupCode As String = GetHotelGroup(intVenueid)

            Dim OverrideNonComm As String = getConfig("NYSVenueGroupsOverrideNonComm")
            If (OverrideNonComm.IndexOf(GroupCode) <> -1) Then
                IsConfigRateOverrideNonComm = True
            Else
                IsConfigRateOverrideNonComm = False
            End If
        End Function


        Public Function IsConfigRate(ByVal strgroupid As String, Optional ByVal strcompanyid As String = "none") As Boolean
            Dim rate As Decimal
            Dim Rates As String
            If strcompanyid = "none" Then 'apply only group client cost
                Rates = getConfig("NYSNonCommClientsRates")
                '  <add key="NYSNonCommClientsRates" value="100040=004.7500;100076=006.0000;100101=000.0000"/>  e.g. 100101 non at group but some at company
                If (Rates.IndexOf(strgroupid) <> -1) Then
                    Dim st As Integer = Rates.IndexOf(strgroupid) + 7
                    rate = CDec(Rates.Substring(st, 8))
                Else
                    rate = 0
                End If


            Else 'apply company costs
                'first find a company rates
                Rates = getConfig("NYSNonCommClientsCompanyRates")
                If (Rates.IndexOf(strcompanyid) <> -1) Then
                    Dim st As Integer = Rates.IndexOf(strcompanyid) + 7
                    rate = CDec(Rates.Substring(st, 8))
                Else 'default back to group rate
                    Rates = getConfig("NYSNonCommClientsRates")
                    If (Rates.IndexOf(strcompanyid) <> -1) Then
                        Dim st As Integer = Rates.IndexOf(strcompanyid) + 7
                        rate = CDec(Rates.Substring(st, 8))
                    Else 'default back to group rate

                        rate = 0
                    End If

                End If
            End If

            If rate > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function IsConfigRate(ByRef decfee As Decimal, ByVal strgroupid As String, Optional ByVal strcompanyid As String = "none") As Boolean
            Dim rate As Decimal
            Dim Rates As String
            If strcompanyid = "none" Then 'apply only group client cost
                Rates = getConfig("NYSNonCommClientsRates")
                '  <add key="NYSNonCommClientsRates" value="100040=004.7500;100076=006.0000;100101=000.0000"/>  e.g. 100101 non at group but some at company

                If (Rates.IndexOf(strgroupid) <> -1) Then
                    Dim st As Integer = Rates.IndexOf(strgroupid) + 7
                    rate = CDec(Rates.Substring(st, 8))
                Else
                    rate = 0
                End If

            Else 'apply company costs
                'first find a company rates
                Rates = getConfig("NYSNonCommClientsCompanyRates")
                If (Rates.IndexOf(strcompanyid) <> -1) Then
                    Dim st As Integer = Rates.IndexOf(strcompanyid) + 7
                    rate = CDec(Rates.Substring(st, 8))
                Else 'default back to group rate
                    Rates = getConfig("NYSNonCommClientsRates")
                    Dim st As Integer = Rates.IndexOf(strgroupid) + 7
                    rate = CDec(Rates.Substring(st, 8))
                End If
            End If
            decfee = rate
            If rate > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function ReadConfigRate(ByVal strgroupid As String, Optional ByVal strcompanyid As String = "none") As Decimal
            Dim ret As Decimal = 0

            Dim Rates As String
            If strcompanyid = "none" Then 'apply only group client cost
                Rates = getConfig("NYSNonCommClientsRates")
                '  <add key="NYSNonCommClientsRates" value="100040=004.7500;100076=006.0000;100101=000.0000"/>  e.g. 100101 non at group but some at company
                If (Rates.IndexOf(strgroupid) <> -1) Then
                    Dim st As Integer = Rates.IndexOf(strgroupid) + 7
                    ret = CDec(Rates.Substring(st, 8))
                Else
                    ret = 0
                End If

            Else 'apply company costs
                'first find a company rates
                Rates = getConfig("NYSNonCommClientsCompanyRates")
                If (Rates.IndexOf(strcompanyid) <> -1) Then
                    Dim st As Integer = Rates.IndexOf(strcompanyid) + 7
                    ret = CDec(Rates.Substring(st, 8))
                Else 'default back to group rate
                    Rates = getConfig("NYSNonCommClientsRates")
                    If (Rates.IndexOf(strgroupid) <> -1) Then
                        Dim st As Integer = Rates.IndexOf(strgroupid) + 7
                        ret = CDec(Rates.Substring(st, 8))
                    Else
                        ret = 0
                    End If
                End If

            End If
            Return ret
        End Function

        Public Function IsConfigRateWithVenue(ByVal intVenueid As Integer, ByVal strgroupid As String, Optional ByVal strcompanyid As String = "none") As Boolean
            Dim rate As Decimal

            If IsConfigRateOverrideNonComm(intVenueid) Then
                rate = 0
            Else
                Dim test As Integer = NysDat.clsEnquiryVenueDat.nonCommissionCheck(0, intVenueid)
                If test = -1 Or test = 1 Then
                    Dim Rates As String
                    If strcompanyid = "none" Then 'apply only group client cost
                        Rates = getConfig("NYSNonCommClientsRates")
                        '  <add key="NYSNonCommClientsRates" value="100040=004.7500;100076=006.0000;100101=000.0000"/>  e.g. 100101 non at group but some at company
                        If (Rates.IndexOf(strgroupid) <> -1) Then
                            Dim st As Integer = Rates.IndexOf(strgroupid) + 7
                            rate = CDec(Rates.Substring(st, 8))
                        Else
                            rate = 0
                        End If


                    Else 'apply company costs
                        'first find a company rates
                        Rates = getConfig("NYSNonCommClientsCompanyRates")
                        If (Rates.IndexOf(strcompanyid) <> -1) Then
                            Dim st As Integer = Rates.IndexOf(strcompanyid) + 7
                            rate = CDec(Rates.Substring(st, 8))
                        Else 'default back to group rate
                            Rates = getConfig("NYSNonCommClientsRates")
                            If (Rates.IndexOf(strcompanyid) <> -1) Then
                                Dim st As Integer = Rates.IndexOf(strcompanyid) + 7
                                rate = CDec(Rates.Substring(st, 8))
                            Else 'default back to group rate
                                rate = 0
                            End If

                        End If
                    End If
                Else
                    rate = 0
                End If

                End If

                If rate > 0 Then
                    Return True
                Else
                    Return False
                End If
        End Function

        Public Function ReadConfigRateWithVenue(ByVal intVenueid As Integer, ByVal strgroupid As String, Optional ByVal strcompanyid As String = "none") As Decimal
            Dim ret As Decimal = 0

            If IsConfigRateOverrideNonComm(intVenueid) Then
                ret = 0
            Else
                Dim test As Integer = NysDat.clsEnquiryVenueDat.nonCommissionCheck(0, intVenueid)
                If test = -1 Or test = 1 Then
                    Dim Rates As String
                    If strcompanyid = "none" Then 'apply only group client cost
                        Rates = getConfig("NYSNonCommClientsRates")
                        '  <add key="NYSNonCommClientsRates" value="100040=004.7500;100076=006.0000;100101=000.0000"/>  e.g. 100101 non at group but some at company
                        If (Rates.IndexOf(strgroupid) <> -1) Then
                            Dim st As Integer = Rates.IndexOf(strgroupid) + 7
                            ret = CDec(Rates.Substring(st, 8))
                        Else
                            ret = 0
                        End If

                    Else 'apply company costs
                        'first find a company rates
                        Rates = getConfig("NYSNonCommClientsCompanyRates")
                        If (Rates.IndexOf(strcompanyid) <> -1) Then
                            Dim st As Integer = Rates.IndexOf(strcompanyid) + 7
                            ret = CDec(Rates.Substring(st, 8))
                        Else 'default back to group rate
                            Rates = getConfig("NYSNonCommClientsRates")
                            If (Rates.IndexOf(strgroupid) <> -1) Then
                                Dim st As Integer = Rates.IndexOf(strgroupid) + 7
                                ret = CDec(Rates.Substring(st, 8))
                            Else
                                ret = 0
                            End If
                        End If

                    End If
                Else
                    ret = 0 'temp should be 0
                End If
            End If
           

            Return ret
        End Function


        Function ReadConfigRateWithVenueEnquiry(ByVal intVenueid As Integer, ByVal intEnquiryid As Integer) As Decimal
            Dim ret As Decimal = 0


            Try
                If IsConfigRateOverrideNonComm(intVenueid) Then
                    ret = 0
                Else


                    Dim test As Integer = NysDat.clsEnquiryVenueDat.nonCommissionCheck(0, intVenueid)
                    If test = -1 Or test = 1 Then
                        Dim Rates As String
                        '' Need to get enquiry
                        Dim strcompanyid As String = ""
                        Dim strgroupid As String = ""
                        strcompanyid = GetCompany(intEnquiryid, strgroupid)
                        If strcompanyid = "none" Then 'apply only group client cost
                            Rates = getConfig("NYSNonCommClientsRates")
                            '  <add key="NYSNonCommClientsRates" value="100040=004.7500;100076=006.0000;100101=000.0000"/>  e.g. 100101 non at group but some at company
                            If (Rates.IndexOf(strgroupid) <> -1) Then
                                Dim st As Integer = Rates.IndexOf(strgroupid) + 7
                                ret = CDec(Rates.Substring(st, 8))
                            Else
                                ret = 0
                            End If

                        Else 'apply company costs
                            'first find a company rates
                            Rates = getConfig("NYSNonCommClientsCompanyRates")
                            If (Rates.IndexOf(strcompanyid) <> -1) Then
                                Dim st As Integer = Rates.IndexOf(strcompanyid) + 7
                                ret = CDec(Rates.Substring(st, 8))
                            Else 'default back to group rate
                                Rates = getConfig("NYSNonCommClientsRates")
                                If (Rates.IndexOf(strgroupid) <> -1) Then
                                    Dim st As Integer = Rates.IndexOf(strgroupid) + 7
                                    ret = CDec(Rates.Substring(st, 8))
                                Else
                                    ret = 0
                                End If
                            End If

                        End If
                    Else
                        ret = 0 'temp should be 0
                    End If
                End If

                Return ret

            Catch ex As Exception
                Throw New Exception("ReadConfigRateWithVenueEnquiry: Venue " & intVenueid.ToString() & " Enquiry " & intEnquiryid.ToString() & " **-" & ex.Message)

            Finally 'last resort don't apply charge ---- can't charge a fee if our systems are wrong!
                ret = 0
            End Try


        End Function



        Public Function GetCompany(ByVal intEventid As Integer, ByRef strgroupid As String) As String
            Dim ret As String = "xx" 'No find
            Using dbh As New SqlDatabaseHandle(getConfig("connectionString"))
                Using r As IDataReader = dbh.runSQLQuery("select companyid,groupid from Enquiry  where enquiryid = " + intEventid.ToString())
                    Dim retdb As New List(Of String)
                    Dim i As Integer = 0
                    While r.Read()
                        If i = 0 Then
                            ret = r.GetInt32(0).ToString
                            strgroupid = r.GetInt32(1).ToString
                        End If
                        i += 1
                    End While
                End Using
            End Using

            If ret = "xx" Then
                strgroupid = "xx"
            End If
            Return ret

        End Function

        Public Function ConfermaEmail(ByVal pref As String) As String
            Using dbh As New SqlDatabaseHandle(getConfig("connectionString"))
                Dim ret As String = ""
                Using r As IDataReader = dbh.callSP("AutoInvoiceConferma_email", "@ref", pref)
                    While r.Read()
                        ret = r.GetString(0).ToString
                    End While
                End Using
                Return ret
            End Using
        End Function
    End Module
End Namespace