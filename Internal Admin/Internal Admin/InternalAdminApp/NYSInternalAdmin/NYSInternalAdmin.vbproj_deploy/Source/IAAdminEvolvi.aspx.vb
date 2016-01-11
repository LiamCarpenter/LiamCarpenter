Imports EvoUtilities.ConfigUtils
Imports System.Net.Mail
Imports NysDat
Imports System.Collections.Generic
Imports System.Globalization

'R2.23 SA - changed the whole class - adding new controls and removing old functionality
Partial Public Class IAAdminEvolvi
    Inherits clsNYS

    Private Shared ReadOnly className As String

    Shared Sub New()
        className = System.Reflection.MethodBase. _
        GetCurrentMethod().DeclaringType.FullName
        log = log4net.LogManager.GetLogger(className)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using New clslogger(log, className, "Page_Load")
            Try
                If Not Me.IsPostBack Then
                    Dim strRet As String = setUser()
                    If strRet.StartsWith("ERROR") Then
                        Response.Redirect("IALogonAdmin.aspx?User=falseX")
                    End If
                    populateGroups()
                    populateGroupInfo("")

                    btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                    btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                    If ddGroup.Items.Count <= 1 Then

                        'btnAdd.Visible = True
                    End If
                End If
            Catch ex As Exception
                handleexception(ex, "IAAdminEvolvi", Me.Page)
            End Try
        End Using

    End Sub

    Private Sub populateGroups()
        Using New clslogger(log, className, "populateGroups")
            Dim oGroupsList As New List(Of clsInterceptorClient)
            Dim oGroup As New clsInterceptorClient

            oGroupsList = clsInterceptorClient.list

            ddGroup.Items.Clear()
            ddGroup.Items.Add(New ListItem("Please select", ""))

            For Each oGroup In oGroupsList
                ddGroup.Items.Add(New ListItem(oGroup.ClientName, oGroup.BossCode))
            Next

            ddGroup.AutoPostBack = True

        End Using
    End Sub

    Private Sub changeDropDowns(ByVal dd As System.Web.UI.WebControls.DropDownList, _
                                ByVal pstrvalue As String, ByVal pblnclear As Boolean, _
                                ByVal pblntext As Boolean)
        Using New clslogger(log, className, "changeDropDowns")
            If pblnclear = True Then
                For intcount As Integer = 0 To dd.Items.Count - 1
                    If dd.Items(intcount).Selected = True Then
                        dd.Items(intcount).Selected = False
                    End If
                Next
                dd.SelectedIndex = 0
            Else
                If pblntext = True Then
                    For intcount As Integer = 0 To dd.Items.Count - 1
                        If dd.Items(intcount).Text.ToLower = pstrvalue.ToLower Then
                            dd.SelectedIndex = intcount
                            Exit For
                        End If
                    Next
                Else
                    For intcount As Integer = 0 To dd.Items.Count - 1
                        If dd.Items(intcount).Value.ToLower = pstrvalue.ToLower Then
                            dd.SelectedIndex = intcount
                            Exit For
                        End If
                    Next
                End If
            End If
        End Using
    End Sub

    Private Sub clearBoxes()
        Using New clslogger(log, className, "clearBoxes")
            txtClientName.Text = ""
            txtBossCode.Text = ""
            chkInterceptorFeesActive.Checked = False

            txtKioskOnlineFeePerTicket.Text = ""
            txtKioskOnlineFeePerPax.Text = ""
            txtKioskOnlineFeePerOrderItem.Text = ""
            txtKioskOnlineFeePerBasket.Text = ""

            txtKioskOfflineFeePerTicket.Text = ""
            txtKioskOfflineFeePerPax.Text = ""
            txtKioskOfflineFeePerOrderItem.Text = ""
            txtKioskOfflineFeePerBasket.Text = ""

            txtToDOnlineFeePerTicket.Text = ""
            txtToDOnlineFeePerPax.Text = ""
            txtToDOnlineFeePerOrderItem.Text = ""
            txtToDOnlineFeePerBasket.Text = ""

            txtToDOfflineFeePerTicket.Text = ""
            txtToDOfflineFeePerPax.Text = ""
            txtToDOfflineFeePerOrderItem.Text = ""
            txtToDOfflineFeePerBasket.Text = ""

            txtPostageOnlineFeePerTicket.Text = ""
            txtPostageOnlineFeePerPax.Text = ""
            txtPostageOnlineFeePerOrderItem.Text = ""
            txtPostageOnlineFeePerBasket.Text = ""

            txtPostageOfflineFeePerTicket.Text = ""
            txtPostageOfflineFeePerPax.Text = ""
            txtPostageOfflineFeePerOrderItem.Text = ""
            txtPostageOfflineFeePerBasket.Text = ""

            txtCourierOnlineFeePerTicket.Text = ""
            txtCourierOnlineFeePerPax.Text = ""
            txtCourierOnlineFeePerOrderItem.Text = ""
            txtCourierOnlineFeePerBasket.Text = ""

            txtCourierOfflineFeePerTicket.Text = ""
            txtCourierOfflineFeePerPax.Text = ""
            txtCourierOfflineFeePerOrderItem.Text = ""
            txtCourierOfflineFeePerBasket.Text = ""

            txtSDOnlineFeePerTicket.Text = ""
            txtSDOnlineFeePerPax.Text = ""
            txtSDOnlineFeePerOrderItem.Text = ""
            txtSDOnlineFeePerBasket.Text = ""

            txtSDOfflineFeePerTicket.Text = ""
            txtSDOfflineFeePerPax.Text = ""
            txtSDOfflineFeePerOrderItem.Text = ""
            txtSDOfflineFeePerBasket.Text = ""

            txtSSDOnlineFeePerTicket.Text = ""
            txtSSDOnlineFeePerPax.Text = ""
            txtSSDOnlineFeePerOrderItem.Text = ""
            txtSSDOnlineFeePerBasket.Text = ""

            txtSSDOfflineFeePerTicket.Text = ""
            txtSSDOfflineFeePerPax.Text = ""
            txtSSDOfflineFeePerOrderItem.Text = ""
            txtSSDOfflineFeePerBasket.Text = ""

            hdnClientID.Value = ""
            ddGroup.SelectedValue = ""

            chkBooker.Checked = False
            chkCustomPO.Checked = False
            txtCustomPO.Text = ""

            ddSplitNode.SelectedIndex = 0
            txtSplitField.Text = ""
            txtSplitFieldValue.Text = ""
            txtUniqueFieldName.Text = ""

            'R2.23.1 SA
            chkInterceptorDiscountActive.Checked = False
            txtDiscountPercentage.Text = ""
        End Using
    End Sub

    Protected Sub ddGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddGroup.SelectedIndexChanged
        Using New clslogger(log, className, "ddGroup_SelectedIndexChanged")
            Try
                If ddGroup.SelectedValue = "" Then
                    'clear all boxes
                    clearBoxes()

                    pnlButtons.Visible = False
                    pnlDetails.Visible = False
                    pnlSearch.Visible = True
                Else
                    populateGroupInfo(ddGroup.SelectedValue)

                    pnlButtons.Visible = True
                    pnlDetails.Visible = True
                    pnlSearch.Visible = True
                End If

            Catch ex As Exception
                handleexception(ex, "IAAdminEvolvi", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub populateGroupInfo(ByVal pstrSelectedGroup As String)
        Using New clslogger(log, className, "populateGroupInfo")
            Dim oInterceptorClient As New clsInterceptorClient
            Dim intClientID As Integer

            If hdnClientID.Value = "" Then
                intClientID = 0
            Else
                intClientID = notInteger(hdnClientID.Value)
            End If

            oInterceptorClient = clsInterceptorClient.checkBossCode(pstrSelectedGroup, intClientID)

            If notInteger(oInterceptorClient.ClientID) > 0 Then
                hdnClientID.Value = oInterceptorClient.ClientID.ToString
                txtClientName.Text = oInterceptorClient.ClientName
                ddGroup.SelectedValue = oInterceptorClient.BossCode
                txtBossCode.Text = oInterceptorClient.BossCode
                'R2.23 SA 
                chkInterceptorFeesActive.Checked = notBoolean(oInterceptorClient.InterceptorFeesActive)
                txtKioskOnlineFeePerTicket.Text = notString(oInterceptorClient.KioskOnlineFeePerTicket)
                txtKioskOnlineFeePerPax.Text = notString(oInterceptorClient.KioskOnlineFeePerTraveller)
                txtKioskOnlineFeePerOrderItem.Text = notString(oInterceptorClient.KioskOnlineFeePerOrderItem)
                txtKioskOnlineFeePerBasket.Text = notString(oInterceptorClient.KioskOnlineFeePerBasket)
                txtKioskOfflineFeePerTicket.Text = notString(oInterceptorClient.KioskOfflineFeePerTicket)
                txtKioskOfflineFeePerPax.Text = notString(oInterceptorClient.KioskOfflineFeePerTraveller)
                txtKioskOfflineFeePerOrderItem.Text = notString(oInterceptorClient.KioskOfflineFeePerOrderItem)
                txtKioskOfflineFeePerBasket.Text = notString(oInterceptorClient.KioskOfflineFeePerBasket)
                txtToDOnlineFeePerTicket.Text = notString(oInterceptorClient.ToDOnlineFeePerTicket)
                txtToDOnlineFeePerPax.Text = notString(oInterceptorClient.ToDOnlineFeePerTraveller)
                txtToDOnlineFeePerOrderItem.Text = notString(oInterceptorClient.ToDOnlineFeePerOrderItem)
                txtToDOnlineFeePerBasket.Text = notString(oInterceptorClient.ToDOnlineFeePerBasket)
                txtToDOfflineFeePerTicket.Text = notString(oInterceptorClient.ToDOfflineFeePerTicket)
                txtToDOfflineFeePerPax.Text = notString(oInterceptorClient.ToDOfflineFeePerTraveller)
                txtToDOfflineFeePerOrderItem.Text = notString(oInterceptorClient.ToDOfflineFeePerOrderItem)
                txtToDOfflineFeePerBasket.Text = notString(oInterceptorClient.ToDOfflineFeePerBasket)
                txtPostageOnlineFeePerTicket.Text = notString(oInterceptorClient.PostageOnlineFeePerTicket)
                txtPostageOnlineFeePerPax.Text = notString(oInterceptorClient.PostageOnlineFeePerTraveller)
                txtPostageOnlineFeePerOrderItem.Text = notString(oInterceptorClient.PostageOnlineFeePerOrderItem)
                txtPostageOnlineFeePerBasket.Text = notString(oInterceptorClient.PostageOnlineFeePerBasket)
                txtPostageOfflineFeePerTicket.Text = notString(oInterceptorClient.PostageOfflineFeePerTicket)
                txtPostageOfflineFeePerPax.Text = notString(oInterceptorClient.PostageOfflineFeePerTraveller)
                txtPostageOfflineFeePerOrderItem.Text = notString(oInterceptorClient.PostageOfflineFeePerOrderItem)
                txtPostageOfflineFeePerBasket.Text = notString(oInterceptorClient.PostageOfflineFeePerBasket)
                txtCourierOnlineFeePerTicket.Text = notString(oInterceptorClient.CourierOnlineFeePerTicket)
                txtCourierOnlineFeePerPax.Text = notString(oInterceptorClient.CourierOnlineFeePerTraveller)
                txtCourierOnlineFeePerOrderItem.Text = notString(oInterceptorClient.CourierOnlineFeePerOrderItem)
                txtCourierOnlineFeePerBasket.Text = notString(oInterceptorClient.CourierOnlineFeePerBasket)
                txtCourierOfflineFeePerTicket.Text = notString(oInterceptorClient.CourierOfflineFeePerTicket)
                txtCourierOfflineFeePerPax.Text = notString(oInterceptorClient.CourierOfflineFeePerTraveller)
                txtCourierOfflineFeePerOrderItem.Text = notString(oInterceptorClient.CourierOfflineFeePerOrderItem)
                txtCourierOfflineFeePerBasket.Text = notString(oInterceptorClient.CourierOfflineFeePerBasket)
                txtSDOnlineFeePerTicket.Text = notString(oInterceptorClient.SDOnlineFeePerTicket)
                txtSDOnlineFeePerPax.Text = notString(oInterceptorClient.SDOnlineFeePerTraveller)
                txtSDOnlineFeePerOrderItem.Text = notString(oInterceptorClient.SDOnlineFeePerOrderItem)
                txtSDOnlineFeePerBasket.Text = notString(oInterceptorClient.SDOnlineFeePerBasket)
                txtSDOfflineFeePerTicket.Text = notString(oInterceptorClient.SDOfflineFeePerTicket)
                txtSDOfflineFeePerPax.Text = notString(oInterceptorClient.SDOfflineFeePerTraveller)
                txtSDOfflineFeePerOrderItem.Text = notString(oInterceptorClient.SDOfflineFeePerOrderItem)
                txtSDOfflineFeePerBasket.Text = notString(oInterceptorClient.SDOfflineFeePerBasket)
                txtSSDOnlineFeePerTicket.Text = notString(oInterceptorClient.SSDOnlineFeePerTicket)
                txtSSDOnlineFeePerPax.Text = notString(oInterceptorClient.SSDOnlineFeePerTraveller)
                txtSSDOnlineFeePerOrderItem.Text = notString(oInterceptorClient.SSDOnlineFeePerOrderItem)
                txtSSDOnlineFeePerBasket.Text = notString(oInterceptorClient.SSDOnlineFeePerBasket)
                txtSSDOfflineFeePerTicket.Text = notString(oInterceptorClient.SSDOfflineFeePerTicket)
                txtSSDOfflineFeePerPax.Text = notString(oInterceptorClient.SSDOfflineFeePerTraveller)
                txtSSDOfflineFeePerOrderItem.Text = notString(oInterceptorClient.SSDOfflineFeePerOrderItem)
                txtSSDOfflineFeePerBasket.Text = notString(oInterceptorClient.SSDOfflineFeePerBasket)
                chkBooker.Checked = notBoolean(oInterceptorClient.AddBookerToInvoice)
                'R2.10 CR
                txtCustomPO.Text = oInterceptorClient.CustomPO
                chkCustomPO.Checked = notBoolean(oInterceptorClient.CustomPOActive)
                'chkOfflineFeePerTicket.Checked = notBoolean(oInterceptorClient.OfflineFeePerTicket)
                'R2.13 CR
                ddSplitNode.SelectedValue = oInterceptorClient.SplitNodeName.Trim
                txtSplitField.Text = oInterceptorClient.SplitFieldName.Trim
                txtSplitFieldValue.Text = oInterceptorClient.SplitFieldValue.Trim
                txtUniqueFieldName.Text = oInterceptorClient.SplitUniqueFieldName.Trim
                If oInterceptorClient.SplitNodeName = "CustomField" Then
                    txtSplitField.Enabled = True
                    txtSplitFieldValue.Enabled = True
                    txtUniqueFieldName.Enabled = True
                End If
                'R2.23.1 SA 
                chkInterceptorDiscountActive.Checked = notBoolean(oInterceptorClient.InterceptorDiscountActive)
                txtDiscountPercentage.Text = notString(oInterceptorClient.InterceptorDiscountPercent)

                btnSave.Visible = True
                btnAdd.Visible = False
            Else
                btnAdd.Visible = True
                btnSave.Visible = False
            End If

        End Using
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Using New clslogger(log, className, "btnSave_Click")
            Try
                Dim strValidate As String = validateFields()
                If strValidate = "" Then
                    saveInterceptorClient(notInteger(hdnClientID.Value))
                    'R2.23 SA 
                    clearBoxes()

                    populateGroups()
                Else
                    Throw New EvoFriendlyException(strValidate, "EvolviAdmin_UpdateClient")
                End If
            Catch ex As Exception
                handleexception(ex, "IAAdminEvolvi", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Using New clslogger(log, className, "btnAdd_Click")
            Try
                Dim strValidate As String = validateFields()
                If strValidate = "" Then
                    saveInterceptorClient(0)

                    pnlButtons.Visible = False
                    pnlDetails.Visible = False
                    pnlSearch.Visible = True

                    btnAdd.Visible = False
                    btnSave.Visible = False
                    btnCancel.Visible = False

                    populateGroups()
                    changeDropDowns(ddGroup, "", True, False)
                    changeDropDowns(ddGroup, txtBossCode.Text, False, False)

                Else
                    Throw New EvoFriendlyException(strValidate, "EvolviAdmin_AddClient")
                End If
            Catch ex As Exception
                handleexception(ex, "IAAdminEvolvi", Me.Page)
            End Try
        End Using
    End Sub

    Private Function validateFields() As String
        Using New clslogger(log, className, "validateFields")

            'check boss code is unique
            Dim oInterceptorClient As New clsInterceptorClient
            If hdnClientID.Value = "" Then
                hdnClientID.Value = CStr(0)
            End If
            oInterceptorClient = clsInterceptorClient.checkBossCode(txtBossCode.Text, notInteger(hdnClientID.Value))

            If notInteger(oInterceptorClient.ClientID) > 0 Then
                Return "BOSS Code entered is in use, please use another."
            End If

            'check client name is not null
            If txtClientName.Text = "" Then
                Return "Please enter a Client Name."
            End If

            'Check Kiosk Online per ticket 
            If txtKioskOnlineFeePerTicket.Text = "" Then
                txtKioskOnlineFeePerTicket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtKioskOnlineFeePerTicket.Text)
            Catch ex As Exception
                Return "Please enter a valid Kiosk Per Ticket Online Fee!"
            End Try
            'Check Kiosk Online per passenger
            If txtKioskOnlineFeePerPax.Text = "" Then
                txtKioskOnlineFeePerPax.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtKioskOnlineFeePerPax.Text)
            Catch ex As Exception
                Return "Please enter a valid Kiosk Per Passenger Online Fee!"
            End Try
            'Check Kiosk Online per order item
            If txtKioskOnlineFeePerOrderItem.Text = "" Then
                txtKioskOnlineFeePerOrderItem.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtKioskOnlineFeePerOrderItem.Text)
            Catch ex As Exception
                Return "Please enter a valid Kiosk Per Order Item Online Fee!"
            End Try
            'Check Kiosk Online per basket
            If txtKioskOnlineFeePerBasket.Text = "" Then
                txtKioskOnlineFeePerBasket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtKioskOnlineFeePerBasket.Text)
            Catch ex As Exception
                Return "Please enter a valid Kiosk Per Basket Online Fee!"
            End Try

            'Check Kiosk Offline per ticket 
            If txtKioskOfflineFeePerTicket.Text = "" Then
                txtKioskOfflineFeePerTicket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtKioskOfflineFeePerTicket.Text)
            Catch ex As Exception
                Return "Please enter a valid Kiosk Per Ticket Online Fee!"
            End Try
            'Check Kiosk Offline per passenger
            If txtKioskOfflineFeePerPax.Text = "" Then
                txtKioskOfflineFeePerPax.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtKioskOfflineFeePerPax.Text)
            Catch ex As Exception
                Return "Please enter a valid Kiosk Per Passenger Online Fee!"
            End Try
            'Check Kiosk Offline per order item
            If txtKioskOfflineFeePerOrderItem.Text = "" Then
                txtKioskOfflineFeePerOrderItem.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtKioskOfflineFeePerOrderItem.Text)
            Catch ex As Exception
                Return "Please enter a valid Kiosk Per Order Item Online Fee!"
            End Try
            'Check Kiosk Offline per basket
            If txtKioskOfflineFeePerBasket.Text = "" Then
                txtKioskOfflineFeePerBasket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtKioskOfflineFeePerBasket.Text)
            Catch ex As Exception
                Return "Please enter a valid Kiosk Per Basket Online Fee!"
            End Try

            'Check ToD Online per ticket 
            If txtToDOnlineFeePerTicket.Text = "" Then
                txtToDOnlineFeePerTicket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtToDOnlineFeePerTicket.Text)
            Catch ex As Exception
                Return "Please enter a valid ToD Per Ticket Online Fee!"
            End Try
            'Check ToD Online per passenger
            If txtToDOnlineFeePerPax.Text = "" Then
                txtToDOnlineFeePerPax.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtToDOnlineFeePerPax.Text)
            Catch ex As Exception
                Return "Please enter a valid ToD Per Passenger Online Fee!"
            End Try
            'Check ToD Online per order item
            If txtToDOnlineFeePerOrderItem.Text = "" Then
                txtToDOnlineFeePerOrderItem.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtToDOnlineFeePerOrderItem.Text)
            Catch ex As Exception
                Return "Please enter a valid ToD Per Order Item Online Fee!"
            End Try
            'Check ToD Online per basket
            If txtToDOnlineFeePerBasket.Text = "" Then
                txtToDOnlineFeePerBasket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtToDOnlineFeePerBasket.Text)
            Catch ex As Exception
                Return "Please enter a valid ToD Per Basket Online Fee!"
            End Try

            'Check ToD Offline per ticket 
            If txtToDOfflineFeePerTicket.Text = "" Then
                txtToDOfflineFeePerTicket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtToDOfflineFeePerTicket.Text)
            Catch ex As Exception
                Return "Please enter a valid ToD Per Ticket Online Fee!"
            End Try
            'Check ToD Offline per passenger
            If txtToDOfflineFeePerPax.Text = "" Then
                txtToDOfflineFeePerPax.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtToDOfflineFeePerPax.Text)
            Catch ex As Exception
                Return "Please enter a valid ToD Per Passenger Online Fee!"
            End Try
            'Check ToD Offline per order item
            If txtToDOfflineFeePerOrderItem.Text = "" Then
                txtToDOfflineFeePerOrderItem.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtToDOfflineFeePerOrderItem.Text)
            Catch ex As Exception
                Return "Please enter a valid ToD Per Order Item Online Fee!"
            End Try
            'Check ToD Offline per basket
            If txtToDOfflineFeePerBasket.Text = "" Then
                txtToDOfflineFeePerBasket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtToDOfflineFeePerBasket.Text)
            Catch ex As Exception
                Return "Please enter a valid ToD Per Basket Online Fee!"
            End Try

            'Check Postage Online per ticket 
            If txtPostageOnlineFeePerTicket.Text = "" Then
                txtPostageOnlineFeePerTicket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtPostageOnlineFeePerTicket.Text)
            Catch ex As Exception
                Return "Please enter a valid Postage Per Ticket Online Fee!"
            End Try
            'Check Postage Online per passenger
            If txtPostageOnlineFeePerPax.Text = "" Then
                txtPostageOnlineFeePerPax.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtPostageOnlineFeePerPax.Text)
            Catch ex As Exception
                Return "Please enter a valid Postage Per Passenger Online Fee!"
            End Try
            'Check Postage Online per order item
            If txtPostageOnlineFeePerOrderItem.Text = "" Then
                txtPostageOnlineFeePerOrderItem.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtPostageOnlineFeePerOrderItem.Text)
            Catch ex As Exception
                Return "Please enter a valid Postage Per Order Item Online Fee!"
            End Try
            'Check Postage Online per basket
            If txtPostageOnlineFeePerBasket.Text = "" Then
                txtPostageOnlineFeePerBasket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtPostageOnlineFeePerBasket.Text)
            Catch ex As Exception
                Return "Please enter a valid Postage Per Basket Online Fee!"
            End Try

            'Check Postage Offline per ticket 
            If txtPostageOfflineFeePerTicket.Text = "" Then
                txtPostageOfflineFeePerTicket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtPostageOfflineFeePerTicket.Text)
            Catch ex As Exception
                Return "Please enter a valid Postage Per Ticket Online Fee!"
            End Try
            'Check Postage Offline per passenger
            If txtPostageOfflineFeePerPax.Text = "" Then
                txtPostageOfflineFeePerPax.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtPostageOfflineFeePerPax.Text)
            Catch ex As Exception
                Return "Please enter a valid Postage Per Passenger Online Fee!"
            End Try
            'Check Postage Offline per order item
            If txtPostageOfflineFeePerOrderItem.Text = "" Then
                txtPostageOfflineFeePerOrderItem.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtPostageOfflineFeePerOrderItem.Text)
            Catch ex As Exception
                Return "Please enter a valid Postage Per Order Item Online Fee!"
            End Try
            'Check Postage Offline per basket
            If txtPostageOfflineFeePerBasket.Text = "" Then
                txtPostageOfflineFeePerBasket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtPostageOfflineFeePerBasket.Text)
            Catch ex As Exception
                Return "Please enter a valid Postage Per Basket Online Fee!"
            End Try

            'Check Courier Online per ticket 
            If txtCourierOnlineFeePerTicket.Text = "" Then
                txtCourierOnlineFeePerTicket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtCourierOnlineFeePerTicket.Text)
            Catch ex As Exception
                Return "Please enter a valid Courier Per Ticket Online Fee!"
            End Try
            'Check Courier Online per passenger
            If txtCourierOnlineFeePerPax.Text = "" Then
                txtCourierOnlineFeePerPax.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtCourierOnlineFeePerPax.Text)
            Catch ex As Exception
                Return "Please enter a valid Courier Per Passenger Online Fee!"
            End Try
            'Check Courier Online per order item
            If txtCourierOnlineFeePerOrderItem.Text = "" Then
                txtCourierOnlineFeePerOrderItem.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtCourierOnlineFeePerOrderItem.Text)
            Catch ex As Exception
                Return "Please enter a valid Courier Per Order Item Online Fee!"
            End Try
            'Check Courier Online per basket
            If txtCourierOnlineFeePerBasket.Text = "" Then
                txtCourierOnlineFeePerBasket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtCourierOnlineFeePerBasket.Text)
            Catch ex As Exception
                Return "Please enter a valid Courier Per Basket Online Fee!"
            End Try

            'Check Courier Offline per ticket 
            If txtCourierOfflineFeePerTicket.Text = "" Then
                txtCourierOfflineFeePerTicket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtCourierOfflineFeePerTicket.Text)
            Catch ex As Exception
                Return "Please enter a valid Courier Per Ticket Online Fee!"
            End Try
            'Check Courier Offline per passenger
            If txtCourierOfflineFeePerPax.Text = "" Then
                txtCourierOfflineFeePerPax.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtCourierOfflineFeePerPax.Text)
            Catch ex As Exception
                Return "Please enter a valid Courier Per Passenger Online Fee!"
            End Try
            'Check Courier Offline per order item
            If txtCourierOfflineFeePerOrderItem.Text = "" Then
                txtCourierOfflineFeePerOrderItem.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtCourierOfflineFeePerOrderItem.Text)
            Catch ex As Exception
                Return "Please enter a valid Courier Per Order Item Online Fee!"
            End Try
            'Check Courier Offline per basket
            If txtCourierOfflineFeePerBasket.Text = "" Then
                txtCourierOfflineFeePerBasket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtCourierOfflineFeePerBasket.Text)
            Catch ex As Exception
                Return "Please enter a valid Courier Per Basket Online Fee!"
            End Try

            'Check SD Online per ticket 
            If txtSDOnlineFeePerTicket.Text = "" Then
                txtSDOnlineFeePerTicket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSDOnlineFeePerTicket.Text)
            Catch ex As Exception
                Return "Please enter a valid Special Delivery Per Ticket Online Fee!"
            End Try
            'Check SD Online per passenger
            If txtSDOnlineFeePerPax.Text = "" Then
                txtSDOnlineFeePerPax.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSDOnlineFeePerPax.Text)
            Catch ex As Exception
                Return "Please enter a valid Special Delivery Per Passenger Online Fee!"
            End Try
            'Check SD Online per order item
            If txtSDOnlineFeePerOrderItem.Text = "" Then
                txtSDOnlineFeePerOrderItem.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSDOnlineFeePerOrderItem.Text)
            Catch ex As Exception
                Return "Please enter a valid Special Delivery Per Order Item Online Fee!"
            End Try
            'Check SD Online per basket
            If txtSDOnlineFeePerBasket.Text = "" Then
                txtSDOnlineFeePerBasket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSDOnlineFeePerBasket.Text)
            Catch ex As Exception
                Return "Please enter a valid Special Delivery Per Basket Online Fee!"
            End Try

            'Check SD Offline per ticket 
            If txtSDOfflineFeePerTicket.Text = "" Then
                txtSDOfflineFeePerTicket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSDOfflineFeePerTicket.Text)
            Catch ex As Exception
                Return "Please enter a valid Special Delivery Per Ticket Online Fee!"
            End Try
            'Check SD Offline per passenger
            If txtSDOfflineFeePerPax.Text = "" Then
                txtSDOfflineFeePerPax.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSDOfflineFeePerPax.Text)
            Catch ex As Exception
                Return "Please enter a valid Special Delivery Per Passenger Online Fee!"
            End Try
            'Check SD Offline per order item
            If txtSDOfflineFeePerOrderItem.Text = "" Then
                txtSDOfflineFeePerOrderItem.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSDOfflineFeePerOrderItem.Text)
            Catch ex As Exception
                Return "Please enter a valid Special Delivery Per Order Item Online Fee!"
            End Try
            'Check SD Offline per basket
            If txtSDOfflineFeePerBasket.Text = "" Then
                txtSDOfflineFeePerBasket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSDOfflineFeePerBasket.Text)
            Catch ex As Exception
                Return "Please enter a valid Special Delivery Per Basket Online Fee!"
            End Try

            'Check SSD Online per ticket 
            If txtSSDOnlineFeePerTicket.Text = "" Then
                txtSSDOnlineFeePerTicket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSSDOnlineFeePerTicket.Text)
            Catch ex As Exception
                Return "Please enter a valid Saturday Special Delivery Per Ticket Online Fee!"
            End Try
            'Check SSD Online per passenger
            If txtSSDOnlineFeePerPax.Text = "" Then
                txtSSDOnlineFeePerPax.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSSDOnlineFeePerPax.Text)
            Catch ex As Exception
                Return "Please enter a valid Saturday Special Delivery Per Passenger Online Fee!"
            End Try
            'Check SSD Online per order item
            If txtSSDOnlineFeePerOrderItem.Text = "" Then
                txtSSDOnlineFeePerOrderItem.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSSDOnlineFeePerOrderItem.Text)
            Catch ex As Exception
                Return "Please enter a valid Saturday Special Delivery Per Order Item Online Fee!"
            End Try
            'Check SSD Online per basket
            If txtSSDOnlineFeePerBasket.Text = "" Then
                txtSSDOnlineFeePerBasket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSSDOnlineFeePerBasket.Text)
            Catch ex As Exception
                Return "Please enter a valid Saturday Special Delivery Per Basket Online Fee!"
            End Try

            'Check SSD Offline per ticket 
            If txtSSDOfflineFeePerTicket.Text = "" Then
                txtSSDOfflineFeePerTicket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSSDOfflineFeePerTicket.Text)
            Catch ex As Exception
                Return "Please enter a valid Saturday Special Delivery Per Ticket Online Fee!"
            End Try
            'Check SSD Offline per passenger
            If txtSSDOfflineFeePerPax.Text = "" Then
                txtSSDOfflineFeePerPax.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSSDOfflineFeePerPax.Text)
            Catch ex As Exception
                Return "Please enter a valid Saturday Special Delivery Per Passenger Online Fee!"
            End Try
            'Check SSD Offline per order item
            If txtSSDOfflineFeePerOrderItem.Text = "" Then
                txtSSDOfflineFeePerOrderItem.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSSDOfflineFeePerOrderItem.Text)
            Catch ex As Exception
                Return "Please enter a valid Saturday Special Delivery Per Order Item Online Fee!"
            End Try
            'Check SSD Offline per basket
            If txtSSDOfflineFeePerBasket.Text = "" Then
                txtSSDOfflineFeePerBasket.Text = "0.00"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtSSDOfflineFeePerBasket.Text)
            Catch ex As Exception
                Return "Please enter a valid Saturday Special Delivery Per Basket Online Fee!"
            End Try

            'R2.23.1 SA 
            If txtDiscountPercentage.Text = "" Then
                txtDiscountPercentage.Text = "0"
            End If
            If chkInterceptorDiscountActive.Checked = True AndAlso txtDiscountPercentage.Text = "0" Then
                Return "Please enter a valid Discount percentage, or uncheck 'Apply discount through Interceptor' box!"
            End If
            If chkInterceptorDiscountActive.Checked = False AndAlso txtDiscountPercentage.Text <> "0" Then
                Return "Please check the 'Apply discount through Interceptor' box or remove the 'Discount Percentage' value!"
            End If
            Try
                Dim dblTest As Double
                dblTest = CDbl(txtDiscountPercentage.Text)
            Catch ex As Exception
                Return "Please enter a valid Discount Percentage Fee!"
            End Try

            Return ""
        End Using
    End Function

    Private Sub saveInterceptorClient(ByVal pintClientID As Integer)
        Using New clslogger(log, className, "saveInterceptorClient")
            Dim oInterceptorClient As New clsInterceptorClient

            With oInterceptorClient
                .ClientID = pintClientID
                .ClientName = txtclientname.text
                .BossCode = txtBossCode.Text.ToUpper
                .InterceptorFeesActive = chkInterceptorFeesActive.Checked

                .KioskOnlineFeePerTicket = CDbl(txtKioskOnlineFeePerTicket.Text)
                .KioskOnlineFeePerTraveller = CDbl(txtKioskOnlineFeePerPax.Text)
                .KioskOnlineFeePerOrderItem = CDbl(txtKioskOnlineFeePerOrderItem.Text)
                .KioskOnlineFeePerBasket = CDbl(txtKioskOnlineFeePerBasket.Text)
                .KioskOfflineFeePerTicket = CDbl(txtKioskOfflineFeePerTicket.Text)
                .KioskOfflineFeePerTraveller = CDbl(txtKioskOfflineFeePerPax.Text)
                .KioskOfflineFeePerOrderItem = CDbl(txtKioskOfflineFeePerOrderItem.Text)
                .KioskOfflineFeePerBasket = CDbl(txtKioskOfflineFeePerBasket.Text)

                .ToDOnlineFeePerTicket = CDbl(txtToDOnlineFeePerTicket.Text)
                .ToDOnlineFeePerTraveller = CDbl(txtToDOnlineFeePerPax.Text)
                .ToDOnlineFeePerOrderItem = CDbl(txtToDOnlineFeePerOrderItem.Text)
                .ToDOnlineFeePerBasket = CDbl(txtToDOnlineFeePerBasket.Text)
                .ToDOfflineFeePerTicket = CDbl(txtToDOfflineFeePerTicket.Text)
                .ToDOfflineFeePerTraveller = CDbl(txtToDOfflineFeePerPax.Text)
                .ToDOfflineFeePerOrderItem = CDbl(txtToDOfflineFeePerOrderItem.Text)
                .ToDOfflineFeePerBasket = CDbl(txtToDOfflineFeePerBasket.Text)

                .PostageOnlineFeePerTicket = CDbl(txtPostageOnlineFeePerTicket.Text)
                .PostageOnlineFeePerTraveller = CDbl(txtPostageOnlineFeePerPax.Text)
                .PostageOnlineFeePerOrderItem = CDbl(txtPostageOnlineFeePerOrderItem.Text)
                .PostageOnlineFeePerBasket = CDbl(txtPostageOnlineFeePerBasket.Text)
                .PostageOfflineFeePerTicket = CDbl(txtPostageOfflineFeePerTicket.Text)
                .PostageOfflineFeePerTraveller = CDbl(txtPostageOfflineFeePerPax.Text)
                .PostageOfflineFeePerOrderItem = CDbl(txtPostageOfflineFeePerOrderItem.Text)
                .PostageOfflineFeePerBasket = CDbl(txtPostageOfflineFeePerBasket.Text)

                .CourierOnlineFeePerTicket = CDbl(txtCourierOnlineFeePerTicket.Text)
                .CourierOnlineFeePerTraveller = CDbl(txtCourierOnlineFeePerPax.Text)
                .CourierOnlineFeePerOrderItem = CDbl(txtCourierOnlineFeePerOrderItem.Text)
                .CourierOnlineFeePerBasket = CDbl(txtCourierOnlineFeePerBasket.Text)
                .CourierOfflineFeePerTicket = CDbl(txtCourierOfflineFeePerTicket.Text)
                .CourierOfflineFeePerTraveller = CDbl(txtCourierOfflineFeePerPax.Text)
                .CourierOfflineFeePerOrderItem = CDbl(txtCourierOfflineFeePerOrderItem.Text)
                .CourierOfflineFeePerBasket = CDbl(txtCourierOfflineFeePerBasket.Text)

                .SDOnlineFeePerTicket = CDbl(txtSDOnlineFeePerTicket.Text)
                .SDOnlineFeePerTraveller = CDbl(txtSDOnlineFeePerPax.Text)
                .SDOnlineFeePerOrderItem = CDbl(txtSDOnlineFeePerOrderItem.Text)
                .SDOnlineFeePerBasket = CDbl(txtSDOnlineFeePerBasket.Text)
                .SDOfflineFeePerTicket = CDbl(txtSDOfflineFeePerTicket.Text)
                .SDOfflineFeePerTraveller = CDbl(txtSDOfflineFeePerPax.Text)
                .SDOfflineFeePerOrderItem = CDbl(txtSDOfflineFeePerOrderItem.Text)
                .SDOfflineFeePerBasket = CDbl(txtSDOfflineFeePerBasket.Text)


                .SSDOnlineFeePerTicket = CDbl(txtSSDOnlineFeePerTicket.Text)
                .SSDOnlineFeePerTraveller = CDbl(txtSSDOnlineFeePerPax.Text)
                .SSDOnlineFeePerOrderItem = CDbl(txtSSDOnlineFeePerOrderItem.Text)
                .SSDOnlineFeePerBasket = CDbl(txtSSDOnlineFeePerBasket.Text)
                .SSDOfflineFeePerTicket = CDbl(txtSSDOfflineFeePerTicket.Text)
                .SSDOfflineFeePerTraveller = CDbl(txtSSDOfflineFeePerPax.Text)
                .SSDOfflineFeePerOrderItem = CDbl(txtSSDOfflineFeePerOrderItem.Text)
                .SSDOfflineFeePerBasket = CDbl(txtSSDOfflineFeePerBasket.Text)

                .AddBookerToInvoice = chkBooker.Checked
                'R2.10 CR
                .CustomPO = txtCustomPO.Text
                .CustomPOActive = chkCustomPO.Checked
                'R2.13 CR
                .SplitNodeName = ddSplitNode.SelectedValue
                .SplitFieldName = txtSplitField.Text
                .SplitFieldValue = txtSplitFieldValue.Text
                .SplitUniqueFieldName = txtUniqueFieldName.Text

                'R2.23.1 SA 
                .InterceptorDiscountActive = chkInterceptorDiscountActive.Checked
                .InterceptorDiscountPercent = CDbl(txtDiscountPercentage.Text)
            End With
            oInterceptorClient.save()
        End Using
    End Sub

    Protected Sub lnkAddNewClient_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkAddNewClient.Click
        Using New clslogger(log, className, "lnkAddNewClient_Click")
            btnSave.Visible = False
            btnAdd.Visible = True
            btnCancel.visible = True

            pnlButtons.Visible = True
            pnlDetails.Visible = True
            pnlSearch.Visible = False

            clearBoxes()
        End Using
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Using New clslogger(log, className, "btnCancel_Click")
            pnlButtons.Visible = False
            pnlDetails.Visible = False
            pnlSearch.Visible = True

            btnAdd.Visible = False
            btnSave.Visible = False
            btnCancel.visible = False
        End Using
    End Sub

    Private Sub IAEvolviAdmin_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAEvolviAdmin_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                                      ucReportMenu)
                fp.pageName = "IAAdminEvolvi"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAAdminEvolvi", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Response.Redirect("IALogonAdmin.aspx")
    End Sub

    'R2.13 CR
    Protected Sub ddSplitNode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddSplitNode.SelectedIndexChanged
        If ddSplitNode.SelectedValue = "" Then
            txtSplitField.Text = ""
            txtSplitFieldValue.Text = ""
            txtUniqueFieldName.Text = ""
            txtSplitField.Enabled = False
            txtSplitFieldValue.Enabled = False
            txtUniqueFieldName.Enabled = False
        ElseIf ddSplitNode.SelectedValue = "Passenger" Then
            txtSplitField.Text = ""
            txtSplitFieldValue.Text = ""
            txtUniqueFieldName.Text = "Ref"
            txtSplitField.Enabled = False
            txtSplitFieldValue.Enabled = False
            txtUniqueFieldName.Enabled = False
        ElseIf ddSplitNode.SelectedValue = "CustomField" Then
            txtSplitField.Text = ""
            txtSplitFieldValue.Text = ""
            txtUniqueFieldName.Text = ""
            txtSplitField.Enabled = True
            txtSplitFieldValue.Enabled = True
            txtUniqueFieldName.Enabled = True
        End If
    End Sub
End Class