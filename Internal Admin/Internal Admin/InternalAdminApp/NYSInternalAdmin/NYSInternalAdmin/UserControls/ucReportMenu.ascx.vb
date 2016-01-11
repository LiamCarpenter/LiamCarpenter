Imports AjaxControlToolkit
Imports log4net
Imports EvoUtilities.ConfigUtils

Partial Public Class ucReportMenu
    Inherits System.Web.UI.UserControl

    Private mpageName As String

    Private Shared ReadOnly className As String
    Protected Shared log As log4net.ILog = _
   log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub

    Public Property pageName() As String
        Get
            Return mpageName
        End Get
        Set(ByVal value As String)
            mpageName = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using New clslogger(log, className, "Page_Load")

        'If Not IsPostBack Then
            Panel2.CssClass = "nyspanels"
            Accordion2.SelectedIndex = -1

            Dim blnTTEnabled As Boolean = False
            If Not Session.Item("TTEnabled") Is Nothing Then
                blnTTEnabled = CBool(Session.Item("TTEnabled"))
            End If
            Dim blnAllEnabled As Boolean = False
            If Not Session.Item("AllEnabled") Is Nothing Then
                blnAllEnabled = CBool(Session.Item("AllEnabled"))
            End If

            Dim blnInternalEnabled As Boolean = False
            If CStr(Session.Item("clientname")) = "All Clients" Then
                blnAllEnabled = False
                blnTTEnabled = False
                blnInternalEnabled = True
            End If

            Dim oUser As NysDat.clsSystemNYSUser
            oUser = CType(Session.Item("loggedinuser"), NysDat.clsSystemNYSUser)

            Dim blnShowAdmin As Boolean = False
            If Session.Item("UserGroup") IsNot Nothing Then
                If mpageName <> "IAComplaintsFeedback" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Feedback Analysis", "../IAComplaintsFeedback.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAComplaintsAllFields" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Complaints Analysis", "../IAComplaintsAllFields.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAComplaintsBookings" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Bookings Analysis", "../IAComplaintsBookings.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAEvolviProductivity" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Evolvi Productivity", "../IAEvolviProductivity.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IACompanyCreditCard" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Company Credit Card Purchasing", "../IACompanyCreditCard.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAConfermaReport" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Hotel Rooms by Booked Date", "../IAConfermaReport.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAConfermaReportArrival" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Hotel Rooms by Arrival Date", "../IAConfermaReportArrival.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAInternalSelfCheck" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Self/Quality Check", "../IAInternalSelfCheck.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAOutstandingCommission" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Outstanding Commission - All Clients", "../IAOutstandingCommission.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAUnpaidCommission" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Final letter - Unpaid Commission - All Clients", "../IAUnpaidCommission.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAContractsSummary" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Contracts Summary", "../IAContractsSummary.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAContractsSummaryWeekPies" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Contracts Summary Weekly Charts", "../IAContractsSummaryWeekPies.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAContractsSummaryByUser" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Contracts Summary By User", "../IAContractsSummaryByUser.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAContractsAmendSummary" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Contract Amendments Summary", "../IAContractsAmendSummary.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                'R2.20 SA - changed page name to Contracts Not Received
                If mpageName <> "IAContractsNotReceived" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Contracts Not Received", "../IAContractsNotReceived.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAContractsNotSigned" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Contracts Not Signed", "../IAContractsNotSigned.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAContractsAA" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Contracts Awaiting Authorisation", "../IAContractsAA.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If

                If mpageName <> "IABlanketPoCheck" Then
                    Dim blnO2 As Boolean = False
                    If CStr(Session.Item("clientname")).ToUpper = "O2" Then
                        blnO2 = True
                    End If
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "O2 Blanket PO Check", "../IABlanketPoCheck.aspx", blnO2)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If

                'R2.22.2 CR
                If mpageName <> "IAPoCheckJLR" Then
                    Dim blnJLR As Boolean = False
                    If CStr(Session.Item("clientname")).ToUpper = "JAGUAR LAND ROVER" Then
                        blnJLR = True
                    End If
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "JLR PO Check", "../IAPoCheckJLR.aspx", blnJLR)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If

                'R2.18 CR
                If mpageName <> "IAPoCheck" Then
                    Dim blnO2 As Boolean = False
                    If CStr(Session.Item("clientname")).ToUpper = "O2" Then
                        blnO2 = True
                    End If
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "O2 PO Check (All)", "../IAPoCheck.aspx", blnO2)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If

                If mpageName <> "IAOOP" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Out of Policy", "../IAOOP.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If

                'R2.14 CR
                If mpageName <> "IARSAReport" AndAlso CStr(Session.Item("clientname")).ToUpper = "RSA" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "RSA Deposits/Invoices report", "../IARSAReport.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If

                'R2.14 SA 
                If mpageName <> "IAEnquiryBespoke" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Bespoke report", "../IAEnquiryBespoke.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If

                'R2.14 SA 
                If mpageName <> " IAEnquiryCancellationFormCheck" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Cancellation Form Check", "../IAEnquiryCancellationFormCheck.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If

                If mpageName <> "IAEnquiryTakeOver" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Enquiry TakeOver", "../IAEnquiryTakeOver.aspx", blnAllEnabled)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If

                'R2.17 SA 
                If mpageName <> "IAEnquiryGovConfirmedVenueReport" AndAlso CStr(Session.Item("clientname")).ToUpper = "ENVIRONMENT AGENCY" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Goverment Confirmed Venue", "../IAEnquiryGovConfirmedVenueReport.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If

                'R2.20A SA 
                If mpageName <> "IAInactiveMevisContacts" AndAlso CStr(Session.Item("clientname")).ToUpper <> "ALL CLIENTS" Then
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane1.ContentContainer, "Inactive Mevis Users", "../IAInactiveMevisContacts.aspx", True)
                    AccordionPane1.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If


                If Session.Item("FeederFileClient") IsNot Nothing Then
                    If CStr(Session.Item("FeederFileClient")) <> "none" Then
                        blnShowAdmin = True
                        If mpageName <> "IAFeederFile" Then
                            AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                            addLink(AccordionPane2.ContentContainer, "Feeder File Generation", "../IAFeederFile.aspx", blnAllEnabled, False)
                            AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                        End If
                    End If
                End If

                'only comm users allowed here
                If getConfig("CommUsers").ToString.ToLower.Contains(oUser.Systemnysuserlastname.ToLower) Then
                    If mpageName <> "IACommissionClaim" Then
                        AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        addLink(AccordionPane2.ContentContainer, "Commission Claim", "../IACommissionClaim.aspx", True)
                        AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                    End If
                    If mpageName <> "MICommissionOverride" Then
                        AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        addLink(AccordionPane2.ContentContainer, "Commission Override", "../MICommissionOverride.aspx", True)
                        AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                    End If
                    If mpageName <> "IACommissionStatement" Then
                        AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        addLink(AccordionPane2.ContentContainer, "Commission Statement", "../IACommissionStatement.aspx", True)
                        AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                    End If
                End If

                'only Boss users allowed here
                If getConfig("BossUsers").ToString.ToLower.Contains(oUser.Systemnysuserlastname.ToLower) Then
                    If mpageName <> "IABoss" Then
                        AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        addLink(AccordionPane2.ContentContainer, "BOSS Tables Updater", "../IABoss.aspx", True, False)
                        AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                    End If

                End If

                'R2.17 SA - Only Client Statement Users allowed here
                If getConfig("ClientUsers").ToString.ToLower.Contains(oUser.Systemnysuserlastname.ToLower) Then
                    If mpageName <> "IAClientStatements" Then
                        AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        addLink(AccordionPane2.ContentContainer, "Client Statement", "../IAClientStatements.aspx", True)
                        AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                    End If
                    If mpageName <> "IAADmin" Then
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        addLink(AccordionPane4.ContentContainer, "Client Statements (Admin)", "../IAAdmin.aspx", True, False)
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                    End If
                    'AM----> if use client statements can also use venue feedback
                    If mpageName <> "IAADmin" Then
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        addLink(AccordionPane4.ContentContainer, "Feed Back Emails", "../AMFeedback.aspx", True, False)
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("<br />"))

                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        addLink(AccordionPane4.ContentContainer, "Venue Chaser", "../AMVenueChaser.aspx", True, False)
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("<br />"))

                    End If
                End If

                If mpageName <> "IAFeeCalc" Then
                    AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane2.ContentContainer, "Print Fee Calc", "../IAFeeCalc.aspx", True, False)
                    AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IABoss2" Then
                    AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane2.ContentContainer, "BOSS Tables Updater - All Users", "../IABoss2.aspx", True, False)
                    AccordionPane2.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                '   Internal(TT)
                If mpageName <> "IAAirTTAllClients" Then
                    AccordionPane3.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane3.ContentContainer, "Air - All Clients (Data from BOSS)", "../IAAirTTAllClients.aspx", blnInternalEnabled, False)
                    AccordionPane3.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAUKRailTTAllClients" Then
                    AccordionPane3.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane3.ContentContainer, "UK Rail - All Clients", "../IAUKRailTTAllClients.aspx", blnInternalEnabled, False)
                    AccordionPane3.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If
                If mpageName <> "IAInternationalRailTTAllClients" Then
                    AccordionPane3.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    addLink(AccordionPane3.ContentContainer, "International Rail - All Clients", "../IAInternationalRailTTAllClients.aspx", blnInternalEnabled, False)
                    AccordionPane3.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                End If

                ''R2.21.4 SA 
                'If mpageName <> "IAOtherTTAllClients" Then
                '    AccordionPane3.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                '    addLink(AccordionPane3.ContentContainer, "Ancillaries Report - All Clients", "../IAOtherTTAllClients.aspx", blnInternalEnabled, False)
                '    AccordionPane3.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                'End If

                'only admin Admin links if user is in Admin Group
                'R2.16 CR - add accounts to see these too!
                If CStr(Session.Item("UserGroup")).ToLower = "admin" Or CStr(Session.Item("UserGroup")).ToLower = "accounts" Then
                    blnShowAdmin = True
                    If mpageName <> "IAAdminEvolvi" Then
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        addLink(AccordionPane4.ContentContainer, "Evolvi Admin", "../IAAdminEvolvi.aspx", True, False)
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                    End If

                    If mpageName <> "IATestEvolvi" Then
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        addLink(AccordionPane4.ContentContainer, "Evolvi Test", "../IATestEvolvi.aspx", True, False)
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                    End If
                    If mpageName <> "IAAdminFlightClass" Then
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        addLink(AccordionPane4.ContentContainer, "Flight Class Admin", "../IAAdminFlightClass.aspx", True, False)
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                    End If

                    If mpageName <> "IAFeraInvoices" And CStr(Session.Item("clientname")).ToUpper = "FERA" Then
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        addLink(AccordionPane4.ContentContainer, "Fera Invoices Check", "../IAFeraInvoices.aspx", True, False)
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                    End If
                    If mpageName <> "IACostCodeAdmin" And CStr(Session.Item("clientname")).ToUpper = "BS-DWP" Then
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        addLink(AccordionPane4.ContentContainer, "Cost Code Admin", "../IACostCodeAdmin.aspx", True, False)
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                    End If
                    If mpageName <> "IAO2Invoice" And CStr(Session.Item("clientname")).ToUpper = "O2" Then
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        addLink(AccordionPane4.ContentContainer, "O2 Invoice Admin", "../IAO2Invoice.aspx", True, False)
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                    End If
                    If mpageName <> "IAMIBuilder" Then
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                        addLink(AccordionPane4.ContentContainer, "MI Builder", "../IAMIBuilder.aspx", True, False)
                        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                    End If

                    ''R2.20E SA - add report page for CCP
                    'If getConfig("CCPReportUsers").ToString.ToLower.Contains(oUser.Systemnysuserlastname.ToLower) Then
                    '    If mpageName <> "IAEAEnquiriesSummary" Then
                    '        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    '        addLink(AccordionPane4.ContentContainer, "EA Enquiries Summary", "../IAEAEnquiriesSummary.aspx", True, False)
                    '        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                    '    End If
                    'End If
                    ''R2.20E SA - add admin page for CCP
                    'If getConfig("CCPUsers").ToString.ToLower.Contains(oUser.Systemnysuserlastname.ToLower) Then
                    '    If mpageName <> "IAAdminCCP" Then
                    '        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                    '        addLink(AccordionPane4.ContentContainer, "Credit Card Payment - Admin", "../IAAdminCCP.aspx", True, False)
                    '        AccordionPane4.ContentContainer.Controls.Add(New LiteralControl("<br />"))
                    '    End If
                    'End If

                End If
                AccordionPane4.Visible = True
            Else
                AccordionPane4.Visible = False
            End If
            'End If
            If blnShowAdmin Then
                AccordionPane3.Visible = True
            Else
                AccordionPane3.Visible = False
            End If

        End Using
    End Sub

    Private Sub addLink(ByVal acp As AjaxControlToolkit.AccordionContentPanel, _
                       ByVal pLabel As String, ByVal pUrl As String, ByVal pbEnabled As Boolean, _
                       Optional ByVal pbAddOnClick As Boolean = True)
        Using New clslogger(log, className, "addLink")
            Dim lnk As New HyperLink
            lnk.NavigateUrl = pUrl
            lnk.Text = pLabel
            lnk.CssClass = "nyslink"
            lnk.Enabled = pbEnabled
            If pbAddOnClick And pbEnabled Then
                lnk.Attributes.Add("onClick", "toggleDiv('pnTrans');")
            End If
            acp.Controls.Add(lnk)
        End Using
    End Sub

End Class