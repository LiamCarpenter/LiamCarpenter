Imports EvoUtilities.ConfigUtils
Imports System.Net.Mail
Imports NysDat
Imports System.Collections.Generic
Imports System.Globalization

Partial Public Class IAAdminFlightClass
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
                        If Session.Item("LoginType") IsNot Nothing Then
                            Response.Redirect("IALogonAdmin.aspx?User=falseX")
                        End If
                    End If
                    populateClasses()
                    populateClassInfo(0)

                    btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                    btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                    If ddClasses.Items.Count <= 1 Then
                        btnAdd.Visible = True
                    End If
                End If
            Catch ex As Exception
                handleexception(ex, "IAAdminFlightClass", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub populateClasses()
        Using New clslogger(log, className, "populateClasses")
            Dim oFClassList As New List(Of clsFlightClass)
            Dim oFClass As New clsFlightClass

            oFClassList = clsFlightClass.list

            ddClasses.Items.Clear()
            ddClasses.Items.Add(New ListItem("Please select", ""))

            For Each oFClass In oFClassList
                ddclasses.Items.Add(New ListItem(oFClass.Code, oFClass.ClassID.ToString))
            Next

            ddclasses.AutoPostBack = True

        End Using
    End Sub

    Private Sub populateClassInfo(ByVal pstrSelectedClass As Integer)
        Using New clslogger(log, className, "populateClassInfo")
            Dim oFClass As New clsFlightClass
            Dim intClassID As Integer

            If hdnClassID.Value = "" Then
                intClassID = 0
            Else
                intClassID = notInteger(hdnClassID.Value)
            End If

            oFClass = clsFlightClass.get(pstrSelectedClass)

            If notInteger(oFClass.ClassID) > 0 Then
                txtDefaultClass.Text = oFClass.DefaultClass
                txtBritishAirwaysClass.Text = oFClass.BritishAirwaysClass
                txtAirFranceClass.Text = oFClass.AirFranceClass
                txtFlightClassCode.Text = oFClass.Code
                hdnClassID.Value = oFClass.ClassID.ToString
                ddClasses.SelectedValue = oFClass.ClassID.ToString

                btnSave.Visible = True
                btnAdd.Visible = False
            Else
                btnAdd.Visible = True
                btnSave.Visible = False
            End If

        End Using
    End Sub

    Protected Sub ddClasses_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddClasses.SelectedIndexChanged
        Using New clslogger(log, className, "ddClasses_SelectedIndexChanged")
            Try
                If ddClasses.SelectedIndex < 1 Then
                    Throw New EvoFriendlyException("Please select a Flight Class to edit.", "Info")
                Else
                    populateClassInfo(notInteger(ddClasses.SelectedValue))

                    pnlButtons.Visible = True
                    pnlDetails.Visible = True
                    pnlSearch.Visible = True
                    btnSave.Visible = True
                    btnCancel.Visible = True
                    btnAdd.Visible = False
                End If

            Catch ex As Exception
                handleexception(ex, "IAAdminFlightClass", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Using New clslogger(log, className, "btnSave_Click")
            Try
                Dim strValidate As String = validateFields()
                If strValidate = "" Then
                    saveFlightClass(notInteger(hdnClassID.Value))

                    pnlButtons.Visible = False
                    pnlDetails.Visible = False
                    pnlSearch.Visible = True

                    txtFlightClassCode.Enabled = False

                    btnAdd.Visible = False
                    btnSave.Visible = False
                    btnCancel.Visible = False
                Else
                    Throw New EvoFriendlyException(strValidate, "IAFlightClassAdmin_UpdateClass")
                End If
            Catch ex As Exception
                handleexception(ex, "IAAdminFlightClass", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Using New clslogger(log, className, "btnAdd_Click")
            Try
                Dim strValidate As String = validateFields()
                If strValidate = "" Then
                    Dim intCheck As Integer = clsFlightClass.checkClass(txtFlightClassCode.Text)
                    If intCheck = 1 Then
                        Throw New EvoFriendlyException("'" & txtFlightClassCode.Text & "' Flight class code already exists, please select from drop down to edit!", "Info")
                    End If
                    Dim intClassID As Integer = saveFlightClass(0)

                    pnlButtons.Visible = False
                    pnlDetails.Visible = False
                    pnlSearch.Visible = True

                    txtFlightClassCode.Enabled = False

                    btnAdd.Visible = False
                    btnSave.Visible = False
                    btnCancel.Visible = False

                    populateClasses()
                    ddClasses.SelectedValue = intClassID.ToString
                Else
                    Throw New EvoFriendlyException(strValidate, "IAFlightClassAdmin_AddClass")
                End If
            Catch ex As Exception
                handleexception(ex, "IAAdminFlightClass", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Using New clslogger(log, className, "btnCancel_Click")
            pnlButtons.Visible = False
            pnlDetails.Visible = False
            pnlSearch.Visible = True

            txtFlightClassCode.Enabled = False

            btnAdd.Visible = False
            btnSave.Visible = False
            btnCancel.Visible = False
        End Using
    End Sub

    Private Function validateFields() As String
        Using New clslogger(log, className, "validateFields")

            If txtFlightClassCode.Text = "" Then
                Return "Flight Class Code is empty"
            End If

            Return ""
        End Using
    End Function

    Private Function saveFlightClass(ByVal pintClassID As Integer) As Integer
        Using New clslogger(log, className, "saveFlightClass")
            Dim oFlightClass As New clsFlightClass

            With oFlightClass
                .Code = txtFlightClassCode.Text.ToUpper
                .ClassID = pintClassID
                .DefaultClass = txtDefaultClass.Text
                .BritishAirwaysClass = txtBritishAirwaysClass.Text
                .AirFranceClass = txtAirFranceClass.Text
            End With

            Dim intReturnID As Integer
            intReturnID = oFlightClass.save()

            Return intReturnID
        End Using
    End Function

    Protected Sub lnkAddNewClass_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkAddNewClass.Click
        Using New clslogger(log, className, "lnkAddNewClass_Click")
            btnSave.Visible = False
            btnAdd.Visible = True
            btnCancel.visible = True

            pnlButtons.Visible = True
            pnlDetails.Visible = True
            pnlSearch.Visible = False

            txtFlightClassCode.Enabled = True

            'clear boxes
            txtDefaultClass.Text = ""
            txtBritishAirwaysClass.Text = ""
            txtAirFranceClass.Text = ""
            txtFlightClassCode.Text = ""
            ddclasses.SelectedValue = ""
        End Using
    End Sub

    Private Sub IAFlightClassAdmin_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAEvolviAdmin_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                                      ucReportMenu)
                fp.pageName = "IAAdminFlightClass"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAAdminFlightClass", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
         Response.Redirect("IALogonAdmin.aspx")
    End Sub
End Class