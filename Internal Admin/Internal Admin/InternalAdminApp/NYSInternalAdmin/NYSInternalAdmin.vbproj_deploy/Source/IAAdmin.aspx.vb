Imports EvoUtilities.ConfigUtils
Imports System.Net.Mail
Imports NysDat
Imports System.Collections.Generic
Imports System.Globalization
Partial Public Class IAAdmin
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
                    populateBossRef()
                    'R2.23 SA 
                    clearBoxes()
                    setControls(True)
                    btnlogout.Attributes.Add("OnMouseOver", "this.src='images/logout_over.gif';")
                    btnlogout.Attributes.Add("OnMouseOut", "this.src='images/logout_out.gif';")
                End If
            Catch ex As Exception
                handleexception(ex, "IAAdmin", Me.Page)
            End Try
        End Using
    End Sub

    Private Sub populateBossRef()
        Using New clslogger(log, className, "populateBossCode")
            Try
                ddBossRef.Items.Clear()
                ddBossRef.Items.Add(New ListItem("Please select", CStr(0)))
                Dim oBossRefList As List(Of clsClientStatementDetails)
                oBossRefList = clsClientStatementDetails.listBossRef()
                For Each gr As clsClientStatementDetails In oBossRefList
                    ddBossRef.Items.Add(gr.BossRef)
                Next
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    'R2.23 SA - added 
    Private Sub clearBoxes()

        lblMessage.Text = ""
        ddBossRef.SelectedIndex = 0
        txtBossRef.Text = ""
        txtEmailAddress.Text = ""

        ddReference.SelectedIndex = 0
        ddReference2.SelectedIndex = 0
        ddReference3.SelectedIndex = 0
        ddReference4.SelectedIndex = 0
        txtReference1.Text = ""
        txtReference2.Text = ""
        txtReference3.Text = ""
        txtReference4.Text = ""

        chkSendtoEmailAddress.Checked = False
        chkonConinvoices.Checked = False

        'R?? SA - commented out for now! Might need again later! 
        'chkBranch00.Checked = False
        'chkBranch01.Checked = False
        'chkBranch08.Checked = False

    End Sub

    'R2.23 SA - recoded 
    Private Sub setControls(ByVal pValue As Boolean)

        ddBossRef.Enabled = pValue
        btnAdd.Visible = pValue
        btnChange.Visible = pValue

        txtEmailAddress.Enabled = Not (pValue)

        'txtBossRef.Visible = Not (pValue)
        'lblBossRefNew.Visible = Not (pValue)

        ddReference.Enabled = Not (pValue)
        ddReference2.Enabled = Not (pValue)
        ddReference3.Enabled = Not (pValue)
        ddReference4.Enabled = Not (pValue)

        txtReference1.Enabled = Not (pValue)
        txtReference2.Enabled = Not (pValue)
        txtReference3.Enabled = Not (pValue)
        txtReference4.Enabled = Not (pValue)

        chkSendtoEmailAddress.Enabled = Not (pValue)
        chkonConinvoices.Enabled = Not (pValue)

        'R?? SA - commented out for now! Might need again later! 
        'chkBranch00.Enabled = Not (pValue)
        'chkBranch01.Enabled = Not (pValue)
        'chkBranch08.Enabled = Not (pValue)

        btnSave.Visible = Not (pValue)
        btnCancel.Visible = Not (pValue)
        btnOk.Visible = Not (pValue)
        btnClose.Visible = Not (pValue)

        'lblMessage.Visible = Not (pValue)
        'pnlMessage.Visible = Not (pValue)

        'R2.23 SA - don't need this now. keep it for later
        'R2.21.1 SA 
        'chksenddaily.Checked = False
        'chksendtobooker.Checked = False

    End Sub

    Protected Sub ddBossRef_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddBossRef.SelectedIndexChanged
        Using New clslogger(log, className, "ddBossRef_SelectedIndexChanged")
            Try

                ''R2.18 CR - disable some of the boxes
                'txtBossRef.Enabled = False
                'txtEmailAddress.Enabled = False
                'ddReference.Enabled = False
                'chkSendtoEmailAddress.Enabled = False
                'ddReference2.Enabled = False
                ''R2.23 SA 
                'ddReference3.Enabled = False
                'ddReference4.Enabled = False
                'txtReference1.Enabled = False
                'txtReference2.Enabled = False
                'txtReference3.Enabled = False
                'txtReference4.Enabled = False

                'R2.18 CR - show/hide buttons
                btnChange.Visible = True
                btnSave.Visible = False
                btnCancel.Visible = False

                'R2.23 SA - commented out for use later 
                ''R2.21.1 SA 
                'chksenddaily.Enabled = False
                'chksendtobooker.Enabled = False

                If ddBossRef.SelectedIndex <> 0 Then
                    Dim oAdmin As New clsClientStatementDetails()
                    oAdmin = clsClientStatementDetails.getDetails(notString(ddBossRef.SelectedItem.Text))

                    If notString(oAdmin.EmailAddress) <> "" Then
                        txtEmailAddress.Text = oAdmin.EmailAddress
                    Else
                        txtEmailAddress.Text = ""
                    End If
                    ddReference.SelectedValue = oAdmin.Reference
                    chkSendtoEmailAddress.Checked = notBoolean(oAdmin.SendEmail)
                    ddReference2.SelectedValue = oAdmin.Reference2
                    'R2.23 SA 
                    ddReference3.SelectedValue = oAdmin.Reference3
                    ddReference4.SelectedValue = oAdmin.Reference4
                    txtReference1.Text = oAdmin.ReferenceTitle
                    txtReference2.Text = oAdmin.Reference2Title
                    txtReference3.Text = oAdmin.Reference3Title
                    txtReference4.Text = oAdmin.Reference4Title
                    chkonConinvoices.Checked = oAdmin.OnConInvocie
                    'R?? SA - commented out for now! Might need again later! 
                    'chkBranch00.Checked = oAdmin.Branch00
                    'chkBranch01.Checked = oAdmin.Branch01
                    'chkBranch08.Checked = oAdmin.Branch08
                    'R2.23 SA - commented out, could use later 
                    ''R2.21.1 SA 
                    'chksenddaily.Checked = oAdmin.SendDailyReminders
                    'chksendtobooker.Checked = oAdmin.SendReminderToBooker
                Else
                    'R2.23 SA 
                    'txtEmailAddress.Text = ""
                    'ddReference.SelectedIndex = 0
                    'cbSendtoEmailAddress.Checked = False
                    'txtBossRef.Text = ""
                    'ddReference2.SelectedIndex = 0
                    'ddReference3.SelectedIndex = 0
                    'ddReference4.SelectedIndex = 0

                    clearBoxes()
                    setControls(True)

                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click
        Using New clslogger(log, className, "btnChange_Click")
            Try
                If ddBossRef.SelectedIndex <> 0 Then

                    'R2.23 SA 
                    txtBossRef.Text = ""
                    txtBossRef.Visible = False
                    lblBossRefNew.Visible = False
                    setControls(False)
                    'R2.18 CR - hide the change button so people don't click thinking it saves
                    'btnChange.Visible = False

                    'txtEmailAddress.ReadOnly = False
                    'btnSave.Visible = True
                    'btnCancel.Visible = True

                    ''R2.18 CR - hide the change button so people don't click thinking it saves
                    'btnChange.Visible = False

                    ''R2.18 CR - enable some of the boxes
                    'txtBossRef.Enabled = True
                    'txtEmailAddress.Enabled = True
                    'ddReference.Enabled = True
                    'chkSendtoEmailAddress.Enabled = True

                    'ddReference2.Enabled = True
                    ''R2.23 SA 
                    'ddReference3.Enabled = True
                    'ddReference4.Enabled = True
                    'txtReference1.Enabled = True
                    'txtReference2.Enabled = True
                    'txtReference3.Enabled = True
                    'txtReference4.Enabled = True

                    ''R2.21.2 SA 
                    'chksenddaily.Enabled = True
                    'chksendtobooker.Enabled = True
                Else
                    lblMessage.Text = "Please select a Boss Reference to continue!"
                    btnClose.Visible = True
                    btnClose.Enabled = True
                    lblMessage.Visible = True
                    pnlMessage.Style("TOP") = "200px"
                    pnlMessage.Style("LEFT") = "250px"
                    pnlMessage.Visible = True
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Using New clslogger(log, className, "btnSave_Click")
            Try
                Dim intID As Integer
                Dim strBossRef As String = ""
                Dim blnResult As Boolean

                'R2.20C CR - check validity of multiple email addresses
                If txtEmailAddress.Text.Contains(";") Then
                    'multiple addresses - check them all
                    For Each strSingleEmailAddress As String In txtEmailAddress.Text.Split(";")
                        'check each email address for validity
                        If strSingleEmailAddress <> "" Then
                            If Not Regex.IsMatch(strSingleEmailAddress.Trim, emailRegex) Then
                                Throw New EvoFriendlyException("Email address is incorrectly formatted, please check.", "Info")
                            End If
                        End If
                    Next
                Else
                    If Not Regex.IsMatch(txtEmailAddress.Text, emailRegex) Then
                        Throw New EvoFriendlyException("Email address is incorrectly formatted, please check.", "Info")
                    End If
                End If

                'Dim mRegExp As New Regex(clsNYS.emailRegex)
                'If Not mRegExp.IsMatch(txtEmailAddress.Text) Then
                '    Throw New EvoFriendlyException("Email address is incorrectly formatted, please check.", "Info")
                'End If

                'R2.?? SA - commented out! was not working 
                'If ddBossRef.Enabled = False Then
                '    If txtBossRef.Visible = True AndAlso notString(txtBossRef.Text) <> "" Then
                '        intID = 0
                '        strBossRef = txtBossRef.Text
                '    End If
                'Else
                '    intID = clsClientStatementDetails.getID(ddBossRef.SelectedItem.Text)
                '    strBossRef = ddBossRef.SelectedItem.Text
                'End If

                'R?? - SA Bug Fix!
                If txtBossRef.Visible = True AndAlso notString(txtBossRef.Text) <> "" Then
                    intID = 0
                    strBossRef = txtBossRef.Text
                Else
                    intID = clsClientStatementDetails.getID(ddBossRef.SelectedItem.Text)
                    strBossRef = ddBossRef.SelectedItem.Text
                End If


                If strBossRef <> "" Then
                    'R2.23 SA added: ddReference3.SelectedValue, ddReference4.SelectedValue, txtReference1.Text, _
                    '  txtReference2.Text, txtReference3.Text, txtReference4.Text
                    blnResult = clsClientStatementDetails.update(intID, strBossRef, notString(txtEmailAddress.Text), ddReference.SelectedValue, chkSendtoEmailAddress.Checked, _
                                                           ddReference2.SelectedValue, ddReference3.SelectedValue, ddReference4.SelectedValue, txtReference1.Text, _
                                                           txtReference2.Text, txtReference3.Text, txtReference4.Text, False, False, False, chkonConinvoices.Checked)
                    'R2.23 SA 
                    txtBossRef.Text = ""
                    txtBossRef.Visible = False
                    lblBossRefNew.Visible = False

                    If blnResult = True Then
                        lblMessage.Text = "Saved"
                        lblMessage.Visible = True
                        pnlMessage.Style("TOP") = "200px"
                        pnlMessage.Style("LEFT") = "250px"
                        pnlMessage.Visible = True
                        btnOk.Enabled = True
                        btnOk.Visible = True
                    Else
                        lblMessage.Text = "Save failed!"
                        lblMessage.Visible = True
                        pnlMessage.Style("TOP") = "200px"
                        pnlMessage.Style("LEFT") = "250px"
                        pnlMessage.Visible = True
                        btnOk.Enabled = True
                        btnOk.Visible = True
                    End If
                Else
                    lblMessage.Text = "Boss Reference must have a valid value!"
                    lblMessage.Visible = True
                    btnClose.Visible = True
                    btnClose.Enabled = True
                    pnlMessage.Style("TOP") = "200px"
                    pnlMessage.Style("LEFT") = "250px"
                    pnlMessage.Visible = True
                End If
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Using New clslogger(log, className, "btnAdd_Click")
            Try
                populateBossRef()
                'ddBossRef.Enabled = False
                'txtEmailAddress.Text = ""
                'txtEmailAddress.ReadOnly = False
                'ddReference.SelectedIndex = 0
                'chkSendtoEmailAddress.Checked = False
                'txtBossRef.Text = ""
                'txtBossRef.Visible = True
                'lblBossRefNew.Visible = True
                'btnSave.Visible = True
                'btnCancel.Visible = True
                ''R2.18 CR - enable some of the boxes
                'txtBossRef.Enabled = True
                'txtEmailAddress.Enabled = True
                'ddReference.Enabled = True
                'chkSendtoEmailAddress.Enabled = True

                'ddReference2.SelectedIndex = 0
                'ddReference2.Enabled = True
                ''R2.23 SA 
                'ddReference3.SelectedIndex = 0
                'ddReference3.Enabled = True
                'ddReference4.SelectedIndex = 0
                'ddReference4.Enabled = True
                'txtReference1.Enabled = True
                'txtReference2.Enabled = True
                'txtReference3.Enabled = True
                'txtReference4.Enabled = True

                clearBoxes()
                setControls(False)

                txtBossRef.Text = ""
                txtBossRef.Visible = True
                lblBossRefNew.Visible = True
                txtBossRef.Enabled = True

                ''R2.21.1 SA 
                'chksenddaily.Enabled = True
                'chksendtobooker.Enabled = True
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Using New clslogger(log, className, "btnOk_Click")
            Try
                lblMessage.Text = ""
                btnOk.Visible = False
                btnOk.Enabled = False
                pnlMessage.Visible = False
                clearBoxes()
                setControls(True)
                populateBossRef()
                'R2.23 SA 
                txtBossRef.Text = ""
                txtBossRef.Visible = False
                lblBossRefNew.Visible = False
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Using New clslogger(log, className, "btnclose_Click")
            Try
                lblMessage.Text = ""
                btnClose.Visible = False
                btnClose.Enabled = False
                pnlMessage.Visible = False
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Using New clslogger(log, className, "btnCancel_Click")
            Try
                'R2.23 SA 
                txtBossRef.Text = ""
                txtBossRef.Visible = False
                lblBossRefNew.Visible = False

                setControls(True)
                populateBossRef()
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using
    End Sub

    Private Sub IAAdmin_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Using New clslogger(log, className, "IAAdmin_PreRender")
            Try
                Dim fp As ucReportMenu = DirectCast(LoadControl("UserControls/ucReportMenu.ascx"),  _
                    ucReportMenu)
                fp.pageName = "IAAdmin"
                phMenu.Controls.Add(fp)
            Catch ex As Exception
                handleexception(ex, "IAControl", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub btnlogout_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnlogout.Click
        Using New clslogger(log, className, "btnlogout_Click")
            Try
                Response.Redirect("IALogonAdmin.aspx")
            Catch ex As Exception
                handleexception(ex, "IAClientStatements", Me.Page)
            End Try
        End Using
    End Sub

    Protected Sub txtBossRef_TextChanged(sender As Object, e As EventArgs) Handles txtBossRef.TextChanged
        Using New clsLoggerDat(log, className, "txtBossRef_TextChanged")
            Try
                Dim blnCheck As Boolean = False
                For Each item As ListItem In ddBossRef.Items
                    If item.Text.ToLower.Trim = txtBossRef.Text.ToLower.Trim Then
                        blnCheck = True
                        lblMessage.Text = "Boss Reference already exsists! Please enter new reference instead:"
                        txtBossRef.Text = ""
                        btnClose.Enabled = True
                        btnClose.Visible = True
                        lblMessage.Visible = True
                        pnlMessage.Style("TOP") = "200px"
                        pnlMessage.Style("LEFT") = "250px"
                        pnlMessage.Visible = True
                    End If
                Next
            Catch ex As Exception
                If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                    handleexception(ex, "IAAdmin", Page)
                End If
            End Try
        End Using
    End Sub

End Class